Option Explicit On
Option Strict On
Option Infer Off

Namespace Instrument

    Public Class iLightSource

#Region "enum and structure"

        Public Enum ESourse
            Top
            Bottom
            Side
            Angle
        End Enum

        Public Enum EStatus
            LightON
            LightOFF
        End Enum

        Public Enum ECmd
            OpenChannel = 1
            CloseChannel
            SetParameter
            ReadParameter
            OpenAll
            CloseAll
        End Enum

#End Region

        Private mPort As IO.Ports.SerialPort

        Public Sub New()

        End Sub

        Public Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean
            Dim v As Double
            Dim b As Boolean
            Dim i As Integer

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 1000
                mPort.ReadTimeout = 5000
                mPort.Encoding = System.Text.Encoding.ASCII

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "OC9501 Light Controller", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try

            Return True
        End Function

        Public Sub Close()
            mPort.Close()
        End Sub

        Public Function OpenChannel(ByVal channel As Integer) As Boolean
            Dim data() As Byte
            'data = Me.QueryData(ECmd.OpenChannel, channel, 0)
            'Return data(0) = &H24
            Me.SendCmd(ECmd.OpenChannel, channel, 0)
            Return True
        End Function

        Public Function CloseChannel(ByVal channel As Integer) As Boolean
            Dim data() As Byte
            'data = Me.QueryData(ECmd.CloseChannel, channel, 0)
            'Return data(0) = &H24
            Me.SendCmd(ECmd.CloseChannel, channel, 0)
            Return True
        End Function

        Public Function SetChannelParameter(ByVal channel As Integer, ByVal value As Integer) As Boolean
            Dim data() As Byte
            'data = Me.QueryData(ECmd.SetParameter, channel, value)
            'Return data(0) = &H24
            Me.SendCmd(ECmd.SetParameter, channel, value)
            Return True
        End Function

        Public Function ReadChannelParameter(ByVal channel As Integer) As Integer
            Dim data() As Byte
            data = Me.QueryData(ECmd.ReadParameter, channel, 0)
            Return data(3)
        End Function

        Public Function OpenAll() As Boolean
            Dim data() As Byte
            'data = Me.QueryData(ECmd.OpenAll, 1, 0)
            'Return data(0) = &H24
            Me.SendCmd(ECmd.OpenAll, 1, 0)
            Return True
        End Function

        Public Function CloseAll() As Boolean
            Dim data() As Byte
            'data = Me.QueryData(ECmd.CloseAll, 1, 0)
            'Return data(0) = &H24
            Me.SendCmd(ECmd.CloseAll, 1, 0)
            Return True
        End Function

#Region "port communication"

        Private Sub SendCmd(ByVal CmdID As Integer, ByVal channel As Integer, ByVal value As Integer)
            Dim e As New System.Text.ASCIIEncoding
            Dim i, length As Integer
            Dim Data(5) As Integer
            Dim sCmd As String
            Dim check1, check2 As Integer
            Dim a, b As Integer

            'pre code $ - RS232
            Data(0) = &H24
            'cmd id
            Data(1) = CmdID
            'channel
            Data(2) = channel
            'value
            Data(3) = 0
            Data(4) = value \ 16
            Data(5) = value Mod 16

            'data
            check1 = Data(0) \ 16
            check2 = Data(0) Mod 16
            For i = 1 To 5
                a = Asc(Hex(Data(i)).ToString()) \ 16
                b = Asc(Hex(Data(i)).ToString()) Mod 16
                check1 = check1 Xor a
                check2 = check2 Xor b
            Next

            sCmd = "$"
            For i = 1 To 5
                sCmd += Hex(Data(i)).ToString()
            Next
            sCmd += Hex(check1).ToString()
            sCmd += Hex(check2).ToString()


            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            'mPort.Write(Data, 0, Data.Length)
            mPort.WriteLine(sCmd)

            'delay
            System.Threading.Thread.Sleep(50)
        End Sub

        Private Function QueryData(ByVal CmdID As Integer, ByVal channel As Integer, ByVal value As Integer) As Byte()
            Dim data(4) As Byte
            Dim reply As String
            mPort.NewLine = vbCrLf
            Me.SendCmd(CmdID, channel, value)
            System.Threading.Thread.Sleep(100)
            'mPort.Read(data, 0, 4)
            reply = mPort.ReadLine()

            Return data
        End Function
#End Region

    End Class
End Namespace