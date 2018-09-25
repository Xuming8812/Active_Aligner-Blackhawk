Option Explicit On
Option Strict On

Namespace Instrument
    Public Class iOmegaDP40
        Private mPort As IO.Ports.SerialPort
        Private mSubAdrs As Integer

        Public ReadOnly Property Name() As String
            Get
                Return "Omega DP40 COntroller"
            End Get
        End Property

        Public Overloads Function Initialize(ByVal sPort As String, ByVal BaudRate As Integer, ByVal RaiseError As Boolean) As Boolean
            Dim v As Double

            'set port
            mPort = New IO.Ports.SerialPort(sPort, BaudRate, IO.Ports.Parity.Odd, 7, IO.Ports.StopBits.One)
            mPort.ReadTimeout = 2000

            'open port
            Try
                mPort.Open()
                System.Threading.Thread.Sleep(200)
                v = Me.ReadProcessValue()
                Return True
            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, Me.Name)
                If mPort.IsOpen Then mPort.Close()
                Return False
            End Try

        End Function

        Public Property SubAddress() As Integer
            Get
                Return mSubAdrs
            End Get
            Set(ByVal value As Integer)
                mSubAdrs = value
            End Set
        End Property

        Public Function ReadProcessValue() As Double
            Dim s As String
            s = Me.QueryString("V01")

            'Dim i As Integer = 0
            'While i < 10 And s.Contains("?") Or s.Contains("<") Or s.Contains(">")
            '    System.Threading.Thread.Sleep(200)
            '    s = Me.QueryString("V01")
            '    i += 1
            'End While

            Return Val(s)
        End Function

        Public Function ReadProcessValue(ByVal SubAddress As Integer) As Double
            Me.SubAddress = SubAddress
            Return Me.ReadProcessValue()
        End Function

        Private Function QueryString(ByVal sCmd As String) As String
            Dim s As String
            'clear buffer
            mPort.DiscardInBuffer()
            mPort.ReadExisting()
            System.Threading.Thread.Sleep(100)
            'send command
            Me.SendCmd(sCmd)
            System.Threading.Thread.Sleep(100)
            'read data
            s = mPort.ReadExisting()
            'remove known characters
            s = s.Replace(sCmd, "")
            s = s.Replace(ControlChars.Cr, "")
            Return s
        End Function

        Private Sub SendCmd(ByVal sCmd As String)
            Dim s As String

            mPort.DiscardOutBuffer()
            s = "*" + sCmd + ControlChars.Cr
            mPort.Write(s)

            System.Threading.Thread.Sleep(100)
        End Sub

        'Private Sub SendCmdWithAdrs(ByVal sCmd As String)
        '    Dim s As String

        '    mPort.DiscardOutBuffer()

        '    s = "*" + Convert.ToString(mSubAdrs, 16)
        '    s += sCmd + ControlChars.Cr
        '    mPort.Write(s)

        '    System.Threading.Thread.Sleep(100)
        'End Sub
    End Class
End Namespace