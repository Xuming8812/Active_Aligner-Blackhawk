Option Explicit On
Option Strict On

Imports NationalInstruments

Namespace Instrument

    Public Class iGPIB
        Inherits iInstrumentGeneric

        Protected mGPIB As NI4882.Device

        Protected mModel As String = ""
        Protected mManufacturer As String = ""
        Protected mSerialNumber As String = ""
        Protected mFirmwareVersion As String = ""

        Protected mAdrsBoard As Integer
        Protected mAdrsDevice As Integer

        Public Overrides Function Initialize(ByVal AdrsBoard As Integer, ByVal AdrsInstrument As Integer, ByVal RaiseError As Boolean) As Boolean
            Dim s As String
            Dim sData() As String

            'save data
            mAdrsBoard = AdrsBoard
            mAdrsDevice = AdrsInstrument

            'start device
            mGPIB = New NI4882.Device(AdrsBoard, Convert.ToByte(AdrsInstrument))
            With mGPIB
                'set a default timeout value
                .IOTimeout = NI4882.TimeoutValue.T1s

                'get instrument model number
                Try
                    Dim Wait As Integer = 500
                    s = QueryString("*IDN?", Wait)
                    sData = s.Split(","c)

                    mManufacturer = sData(0).Trim

                    If sData.Length > 1 Then mModel = sData(1).Trim
                    If mModel = "0" Then mModel = mManufacturer 'some old instrument mix this up

                    If sData.Length > 2 Then mSerialNumber = sData(2).Trim
                    If sData.Length > 3 Then mFirmwareVersion = sData(3).Trim

                    'return for success
                    Return True

                Catch ex As NI4882.GpibException
                    If ex.ErrorCode = NI4882.GpibError.IOOperationAborted Then
                        'time out error is ok as some old instrument does not implement *IDN? command
                        Return True
                    Else
                        If RaiseError Then _
                            MessageBox.Show(ex.ErrorMessage, Me.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If

                Catch ex As Exception
                    If RaiseError Then _
                        MessageBox.Show(ex.Message, Me.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End Try

            End With

        End Function

        Public Overrides ReadOnly Property Name() As String
            Get
                Return "GPIB basic interface"
            End Get
        End Property

        Public ReadOnly Property AddressBoard() As Integer
            Get
                Return mAdrsBoard
            End Get
        End Property

        Public ReadOnly Property AddressDevice() As Integer
            Get
                Return mAdrsDevice
            End Get
        End Property

        Public ReadOnly Property GPIBDevice() As NI4882.Device
            Get
                Return mGPIB
            End Get
        End Property

        Public Overridable Property ModelName() As String
            Get
                Return mModel
            End Get
            Set(ByVal Value As String)
                mModel = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SerialNumber() As String
            Get
                Return mSerialNumber
            End Get
        End Property

        Public Overridable ReadOnly Property FirmwareVersion() As String
            Get
                Return mFirmwareVersion
            End Get
        End Property

        Public Overridable ReadOnly Property Manufacturer() As String
            Get
                Return mManufacturer
            End Get
        End Property

        Public Overrides Property Connected() As Boolean
            Get
                Return Not (mGPIB Is Nothing)
            End Get
            Set(ByVal Value As Boolean)
                'do not need to 
            End Set
        End Property

        Public Overridable Sub InitializeSetting(ByVal sSetting As String)
            Dim cmdBuff() As String = sSetting.Split(";"c)
            For Each s As String In cmdBuff
                SendCmd(s)
            Next s
        End Sub

#Region "protected read/write"
        Public Overrides Sub SendCmd(ByVal sCmd As String)
            mGPIB.Write(sCmd)
        End Sub

        Public Overloads Overrides Function QueryString(ByVal sCmd As String) As String
            Return QueryString(sCmd, 0)
        End Function

        Protected Overloads Function QueryString(ByVal sCmd As String, ByVal BufferSize As Integer) As String
            mGPIB.Write(sCmd)
            Return ReadData(CInt(BufferSize))
        End Function

        Protected Overloads Function QueryStringWait(ByVal sCmd As String, ByVal WaitTime_ms As Integer) As String
            mGPIB.Write(sCmd)
            If WaitTime_ms >= 0 Then System.Threading.Thread.Sleep(WaitTime_ms)
            Return ReadData(0)
        End Function

        Protected Overloads Function QueryStringWait(ByVal sCmd As String, ByVal WaitBit As Byte, ByVal TimeOut_ms As Integer) As String
            'write command
            mGPIB.Write(sCmd)
            'wait until data is available
            Me.WaitStatusBit(WaitBit, TimeOut_ms)
            'read data
            Return ReadData(0)
        End Function

        Public Overrides Function QueryValue(ByVal sCmd As String) As Double
            Dim s As String = QueryString(sCmd)
            If IsNumeric(s) Then
                Return Convert.ToDouble(s)
            Else
                Return Double.NaN
            End If
        End Function

        Private Function ReadData(ByVal BufferSize As Integer) As String
            Dim s As String

            'read
            If BufferSize = 0 Then
                s = mGPIB.ReadString()
            Else
                s = mGPIB.ReadString(BufferSize)
            End If

            'remove EOL character
            s = s.Replace(vbLf, "")
            s = s.Replace(vbCr, "")

            'strip quto
            s = s.Replace("""", "")

            'return
            Return s

        End Function

        Protected Function WaitStatusBit(ByVal Flag As Byte, ByVal Timeout_ms As Integer) As Boolean
            Dim bStatus As NI4882.SerialPollFlags
            Dim tStart As DateTime
            Dim tSpan As Double

            'wait until data is available
            tStart = DateTime.Now
            While ((bStatus And Flag) <> Flag) And (tSpan < Timeout_ms)
                'take a nap
                System.Threading.Thread.Sleep(100)
                'check status
                bStatus = mGPIB.SerialPoll()
                'check time
                tSpan = DateTime.Now.Subtract(tStart).TotalMilliseconds
            End While

            Return ((bStatus And Flag) = Flag)
        End Function

        Protected Function ReadBinaryData(ByVal sCmd As String, ByRef Data() As Byte) As Boolean
            'setup binary read
            If Not Me.SetupBinaryRead(sCmd) Then Return False
            'read data to file
            Data = mGPIB.ReadByteArray()
        End Function

        Protected Function ReadBinaryToFile(ByVal sCmd As String, ByVal sFile As String) As Boolean
            'setup binary read
            If Not SetupBinaryRead(sCmd) Then Return False
            'read data to file
            mGPIB.ReadToFile(sFile)
        End Function

        Private Function SetupBinaryRead(ByVal sCmd As String) As Boolean
            Dim i As Integer
            Dim s As String

            'send cmd
            mGPIB.Write(sCmd)

            'parse data using IEEE 488.2 binary format
            'read leading "#"
            s = mGPIB.ReadString(1)
            If s <> "#" Then Return False
            'read the first digit, which is the length of the following size string
            s = mGPIB.ReadString(1)
            If Not Integer.TryParse(s, i) Then Return False
            'read the size string, this is the size (number of bytes) of the binary data followed
            s = mGPIB.ReadString(i)
            If Not Integer.TryParse(s, i) Then Return False
            'set the buffer size
            mGPIB.DefaultBufferSize = i
            'return
            Return True
        End Function
#End Region

    End Class
End Namespace

