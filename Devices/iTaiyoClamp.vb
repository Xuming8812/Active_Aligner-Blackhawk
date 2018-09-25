Option Explicit On
Option Strict On

Namespace Instrument
    Public Class iTaiyoClamp
        Private mPort As IO.Ports.SerialPort
        Private mSubAdrs As Byte
        Dim mStatus As Integer

        Public Enum ErrorMask
            InOperation = &H1
            PositionOutOfRange = &H2
            ServoOff = &H4
            CommandError = &H20
            Alarm = &H40
            CommunicationError = &H80
        End Enum

        Public Enum StatusMask
            Ready = &H1
            Busy = &H2
            Alarm = &H4
            InPosition = &H8
            Hold = &H10
            RunLED = &H20
            ReadyLED = &H40
            AlarmLED = &H80
        End Enum


        Public Sub New(ByVal AdrsDevice As Integer)
            mSubAdrs = Convert.ToByte(AdrsDevice)
        End Sub

        Public Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.Even, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 300
                mPort.ReadTimeout = 1000

                'test NOP
                Me.SendCmd(&H30, New Byte() {}, New Byte() {})

                If mStatus = 0 Then
                    Return True
                Else
                    If RaiseError Then MessageBox.Show("Cannot estabilish communication with Taiyo controller.", "Taiyo Controller", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If mPort.IsOpen Then mPort.Close()
                    Return False
                End If

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "Taiyo Controller", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False
            End Try

        End Function

        Public Property Enabled As Boolean
            Get
                Dim data(0) As Byte
                Me.SendCmd(&H46, New Byte() {}, data)
                Return (data(0) = 1)
            End Get
            Set(value As Boolean)
                Dim data(0) As Byte
                data(0) = 0
                If value Then data(0) = 1
                Me.SendCmd(&H31, data, New Byte() {})
            End Set
        End Property

        Public ReadOnly Property Position() As Double
            Get
                Dim data(0) As Byte
                Dim i As Integer
                Me.SendCmd(&H41, New Byte() {}, data)
                i = BitConverter.ToInt32(data, 0)
                Return 0.01 * i
            End Get
        End Property

        Public Function StopMotion() As Boolean
            Return Me.SendCmd(&H10, New Byte() {}, New Byte() {})
        End Function

        Public Function ORG() As Boolean
            Return Me.SendCmd(&H11, New Byte() {}, New Byte() {})
        End Function

        Public Function Move(ByVal Target As Double, ByVal Speed As Double) As Boolean
            Dim Data(4) As Byte
            If Target < 0 Then Target = 0
            Dim intTarget As Int32 = CInt(100 * Target)
            Data(3) = CByte(intTarget >> 24)
            Data(2) = CByte((intTarget And &HFF0000) >> 16)
            Data(1) = CByte((intTarget And &HFF00) >> 8)
            Data(0) = CByte(intTarget And &HFF)
            Data(4) = Convert.ToByte(Speed)

            Return Me.SendCmd(&H17, Data, New Byte() {})
        End Function

        Public Function MoveRelative(ByVal Target As Double, ByVal Speed As Double) As Boolean
            Dim Data(4) As Byte
            Dim intTarget As Int32 = CInt(100 * Math.Abs(Target))
            
            Data(0) = CByte(intTarget And &HFF)
            Data(1) = CByte((intTarget And &HFF00) >> 8)
            Data(2) = CByte((intTarget And &HFF0000) >> 16)
            Data(3) = CByte(intTarget >> 24)
            Data(4) = Convert.ToByte(Speed)
            Return Me.SendCmd(&H16, Data, New Byte() {})

        End Function

        Public Function OpenGrip(ByVal Speed As Double, ByVal Force As Double) As Boolean
            Dim Data(1) As Byte
            Data(0) = Convert.ToByte(Speed)
            Data(1) = Convert.ToByte(Force)
            Return Me.SendCmd(&H20, Data, New Byte() {})
        End Function

        Public Function CloseGrip(ByVal Speed As Double, ByVal Force As Double) As Boolean
            Dim Data(1) As Byte
            Data(0) = Convert.ToByte(Speed)
            Data(1) = Convert.ToByte(Force)
            Return Me.SendCmd(&H21, Data, New Byte() {})
        End Function

        Public ReadOnly Property ClampBusy As Boolean
            Get
                Dim data(2) As Byte
                Me.SendCmd(&H52, New Byte() {}, data)
                Return (data(2) And StatusMask.Busy) = StatusMask.Busy
            End Get
        End Property

#Region "serial communication"
        Public ReadOnly Property ErrorCode As Integer
            Get
                Return mStatus
            End Get
        End Property

        Private Function SendCmd(ByVal register As Byte, ByVal DataOut() As Byte, ByRef DataIn() As Byte) As Boolean
            Dim i, length, checksum As Integer
            Dim Cmd() As Byte

            length = DataOut.Length + 4
            ReDim Cmd(length - 1)

            'build command
            Cmd(0) = Convert.ToByte(length)
            Cmd(1) = mSubAdrs
            Cmd(2) = register

            checksum = Cmd(0) + Cmd(1) + Cmd(2)
            For i = 0 To DataOut.Length - 1
                Cmd(i + 3) = DataOut(i)
                checksum += DataOut(i)
            Next
            Cmd(length - 1) = Convert.ToByte(checksum And &HFF)

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()

            mPort.Write(Cmd, 0, length)

            'delay
            For i = 0 To 99
                System.Threading.Thread.Sleep(50)
                If mPort.BytesToRead >= 4 Then Exit For
            Next

            'read the rest of the data
            length = mPort.BytesToRead()
            ReDim Cmd(length - 1)
            mPort.Read(Cmd, 0, length)

            'get data out
            mStatus = Cmd(2)
            If length > 4 Then
                ReDim DataIn(length - 5)
                For i = 0 To length - 5
                    DataIn(i) = Cmd(i + 3)
                Next
            End If

            Return (mStatus And (ErrorMask.CommandError + ErrorMask.CommunicationError)) = 0
        End Function

#End Region
    End Class
End Namespace


