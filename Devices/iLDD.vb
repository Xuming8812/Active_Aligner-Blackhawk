Option Explicit On
Option Strict On
Option Infer Off

Namespace Instrument
    Public Class iOC9501LDD
        Inherits iMultiChannelLDD

        Private mPort As IO.Ports.SerialPort

        Public Sub New(ByVal ChannelCount As Integer, ByVal DefaultCurrent As Double)
            MyBase.New(ChannelCount, DefaultCurrent)
        End Sub

        Public Overrides Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean
            Dim v As Double
            Dim b As Boolean
            dim i As Integer

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 1000
                mPort.ReadTimeout = 5000

                'get protection state
                b = Me.EnabledProtectionState()
                If b Then Me.EnabledProtectionState = False

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "OC9501 LDD", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try

            'test
            'Me.EnabledProtectionState = True
            'b = Me.EnabledProtectionState()

            'enable TEC
            Me.TECEnabled = True

            System.Threading.Thread.Sleep(2000)

            'try to read temperature
            'v = Me.TemperatureReading

            For i = 0 To mChannelCount - 1
                mCurrent(i) = Me.Current(i + 1)
            Next


            Return True
        End Function

#Region "serial communication"
        Private Function QueryData(ByVal CmdGroup As Byte, ByVal CmdID As Byte, ByVal bData() As Byte) As Byte()
            Dim data() As Byte = New Byte() {}
            Dim NewData() As Byte
            Dim len As Integer

            Try
                Me.SendCmd(CmdGroup, CmdID, bData)

                ReDim data(3)
                mPort.Read(data, 0, 4)

                len = data(2)
                len = len << 8
                len += data(3)
                len += 2                    '2 is for CRC

                ReDim data(len - 1)
                mPort.Read(data, 0, len)

                ReDim NewData(len - 14 - 1)
                Array.Copy(data, 12, NewData, 0, NewData.Length)

                Return NewData
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Private Sub SendCmd(ByVal CmdGroup As Byte, ByVal CmdID As Byte, ByVal bData() As Byte)
            Dim e As New System.Text.ASCIIEncoding
            Dim i, ii, length As Integer
            Dim Data() As Byte
            Dim x As New w2Math.CheckSum

            'build command, "data" length does not include Pre-Code, lengh, and CRC
            ii = bData.Length
            length = 18 + ii
            ReDim Data(length - 1)

            'pre code - RS232
            Data(0) = &HF6
            Data(1) = &H28
            'length
            Data(2) = Convert.ToByte((length And &HFF00) >> 8)
            Data(3) = Convert.ToByte(length And &HFF)
            'cmd type
            Data(4) = CmdGroup
            'cmd ID
            Data(5) = CmdID
            'client ID
            Data(6) = &H0
            'status
            Data(7) = &H0
            'serial ID
            Data(8) = &H12
            Data(9) = &H34
            Data(10) = &H56
            Data(11) = &H78
            'dummy
            Data(12) = &H0
            Data(13) = &H0
            Data(14) = &H0
            Data(15) = &H0
            'data
            For i = 0 To ii - 1
                Data(16 + i) = bData(i)
            Next

            'CRC, null them first, so that it will not affect the CRC
            Data(16 + ii) = &H0
            Data(17 + ii) = x.CRC8Ex(Data, Data.Length - 2)

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            mPort.Write(Data, 0, Data.Length)

            'delay
            System.Threading.Thread.Sleep(50)
        End Sub

        Private Function QueryData(ByVal CmdID As Byte, ByVal Channel As Integer) As Integer
            Dim data As Byte()
            dim i As Integer

            data = Me.QueryData(&H4, CmdID, New Byte() {Convert.ToByte(Channel)})
            If data.Length = 1 Then
                Return data(0)
            Else
                'for this command, the first byte is channel, the 2 following byte is data
                If Channel <> data(0) Then MessageBox.Show("Data Error")
                i = data(1)
                i = (i << 8)
                i = i + data(2)
                Return i
            End If

        End Function

        Private Function QueryData(ByVal CmdID As Byte) As Integer
            Dim data As Byte()
            Dim i As Integer

            data = Me.QueryData(&H4, CmdID, New Byte() {})
            If data.Length = 1 Then
                Return data(0)
            Else
                i = data(0)
                i = (i << 8)
                i = i + data(1)
                Return i
            End If
        End Function

        Private Sub SendCmd(ByVal CmdID As Byte, ByVal Channel As Integer, ByVal Data As Integer)
            Dim bData(2) As Byte

            bData(0) = Convert.ToByte(Channel)
            bData(1) = Convert.ToByte((Data And &HFF00) >> 8)
            bData(2) = Convert.ToByte(Data And &HFF)

            Me.SendCmd(&H4, CmdID, bData)
        End Sub

        Private Sub SendCmd(ByVal CmdID As Byte, ByVal Data As Integer)
            Dim bData(1) As Byte

            bData(0) = Convert.ToByte((Data And &HFF00) >> 8)
            bData(1) = Convert.ToByte(Data And &HFF)

            Me.SendCmd(&H4, CmdID, bData)
        End Sub
#End Region

        Public Overrides Property EnabledProtectionState() As Boolean
            Get
                Dim data() As Byte
                data = Me.QueryData(&H2, &H8, New Byte() {})
                Return data(0) = 0
            End Get

            Set(ByVal value As Boolean)
                Dim data(0) As Byte
                If value Then
                    data(0) = 0
                Else
                    data(0) = 1
                End If
                Me.SendCmd(&H2, &H7, data)

                System.Threading.Thread.Sleep(2000)
            End Set
        End Property

        Public Overrides Property Current(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&HD, Channel)
                Return 0.01 * data
            End Get
            Set(ByVal value As Double)
                Dim data As Integer
                data = Convert.ToInt32(100.0 * value)
                Me.SendCmd(&HE, Channel, data)
            End Set
        End Property

        Public Overrides ReadOnly Property MPDCurrent(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&HB, Channel)
                Return 0.0001 * data
            End Get
        End Property

        Public Overrides ReadOnly Property MPDVoltage(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&HC, Channel)
                Return 0.01 * data
            End Get
        End Property

        Public Overrides ReadOnly Property TECCurrent As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H14)
                Return 0.1 * data
            End Get
        End Property

        Private mEnabled As Boolean
        Public Overrides Property TECEnabled As Boolean
            Get
                Return mEnabled
            End Get
            Set(ByVal value As Boolean)
                Dim b As Byte
                mEnabled = value
                b = 0
                If value Then b = 1
                Me.SendCmd(&H4, &H1, New Byte() {b})
            End Set
        End Property

        Public Overrides ReadOnly Property TECVoltage As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H15)
                Return 0.001 * data
            End Get
        End Property

        Public Overrides ReadOnly Property TemperatureReading As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H4)

                Return 0.01 * data
            End Get
        End Property

        Public Overrides Property TemperatureSetpoint As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H2)
                Return 0.01 * data
            End Get
            Set(ByVal value As Double)
                Dim data As Integer
                data = Convert.ToInt32(100.0 * value)
                Me.SendCmd(&H3, data)
            End Set
        End Property

        Public Overrides Property Vcrossing(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H9, Channel)
                Return 0.001 * data
            End Get
            Set(ByVal value As Double)
                Dim data As Integer
                data = Convert.ToInt32(1000.0 * value)
                Me.SendCmd(&HA, Channel, data)
            End Set
        End Property

        Public Overrides ReadOnly Property Voltage(ByVal channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&HF, channel)
                Return 0.001 * data
            End Get
        End Property

        Public Overrides Property Von(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H7, Channel)
                Return 0.001 * data
            End Get
            Set(ByVal value As Double)
                Dim data As Integer
                data = Convert.ToInt32(1000.0 * value)
                Me.SendCmd(&H8, Channel, data)
            End Set
        End Property

        Public Overrides Property Vpp(ByVal Channel As Integer) As Double
            Get
                Dim data As Integer
                data = Me.QueryData(&H5, Channel)
                Return 0.001 * data
            End Get
            Set(ByVal value As Double)
                Dim data As Integer
                data = Convert.ToInt32(1000.0 * value)
                Me.SendCmd(&H6, Channel, data)
            End Set
        End Property
    End Class

    Public MustInherit Class iMultiChannelLDD
        Protected mChannelCount As Integer
        Protected mCurrent() As Double
        Friend Address As Integer

        Public Sub New(ByVal ChannelCount As Integer, ByVal DefaultCurrent As Double)
            Dim i As Integer

            mChannelCount = ChannelCount

            ReDim mCurrent(mChannelCount - 1)
            For i = 0 To mChannelCount - 1
                mCurrent(i) = DefaultCurrent
            Next
        End Sub

        Public ReadOnly Property ChannelCount As Integer
            Get
                Return mChannelCount
            End Get
        End Property

        Public MustOverride Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean

        Public MustOverride Property EnabledProtectionState() As Boolean

        Public MustOverride Property Current(ByVal Channel As Integer) As Double
        Public MustOverride ReadOnly Property Voltage(ByVal channel As Integer) As Double

        Public MustOverride ReadOnly Property MPDCurrent(ByVal Channel As Integer) As Double
        Public MustOverride ReadOnly Property MPDVoltage(ByVal Channel As Integer) As Double

        Public MustOverride Property Vpp(ByVal Channel As Integer) As Double
        Public MustOverride Property Von(ByVal Channel As Integer) As Double
        Public MustOverride Property Vcrossing(ByVal Channel As Integer) As Double

        Public MustOverride Property TECEnabled() As Boolean
        Public MustOverride ReadOnly Property TECCurrent() As Double
        Public MustOverride ReadOnly Property TECVoltage() As Double

        Public MustOverride Property TemperatureSetpoint As Double
        Public MustOverride ReadOnly Property TemperatureReading As Double

        Public Sub SetSingleChannelCurrent(ByVal Channel As Integer, ByVal Current As Double)

            Dim b As Boolean
            b = Me.EnabledProtectionState
            If b Then
                Me.EnabledProtectionState = False
                System.Threading.Thread.Sleep(1000)
            End If


            If Channel = 0 Then Channel = 2
            mCurrent(Channel - 1) = Current
            Me.TurnSingleChannelOn(Channel)
        End Sub

        Public Sub TurnSingleChannelOn(ByVal Channel As Integer)
            'Dim v(mChannelCount - 1) As Double
            'to match the wrong wire connection , Added by Ming
            Dim b As Boolean
            b = Me.EnabledProtectionState
            If b Then
                Me.EnabledProtectionState = False
                System.Threading.Thread.Sleep(1000)
            End If

            mCurrent(Channel - 1) = 85

            Dim i As Integer
            'set current, do this first, the zeroing of other channel acts as a delay fot this set channel
            Me.Current(Channel) = mCurrent(Channel - 1)
            'zero other channels
            For i = 1 To mChannelCount
                If i <> Channel Then
                    'zero setting
                    Me.Current(i) = 0.0
                End If
                'read back
                'v(i - 1) = Me.Current(i)
            Next

        End Sub

    End Class
End Namespace


