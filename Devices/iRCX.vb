
Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Threading

Namespace Instrument

    Public Class iRCX

#Region "enum and structure"

        Public Enum EAxis
            X
            Y
            Z
            A
            B
            C
        End Enum

        Public Enum EMoveToTarget
            Absolute
            Relative
        End Enum

        Public Enum EServoStatus
            ServoON
            ServoOFF
        End Enum

        Public Enum EDriveMode
            Position
            Joint
        End Enum
#End Region

        Dim cliSocket As Socket
        Private mTimeOut As Double
        Private mComplete As Boolean

        Public Sub New()

        End Sub

        Public Function Initialize(ByVal RaiseError As Boolean) As Boolean
            Dim reply As String
            Dim remoteEP As New IPEndPoint(Net.IPAddress.Parse("192.168.0.20"), 10008)
            cliSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                cliSocket.Connect(remoteEP)
                SendCmd("OPEN=")
                System.Threading.Thread.Sleep(500)
                reply = ReadString()
                reply = QueryString("CNTLON")
            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "Mistsubishi robot", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
            mComplete = True
            Return True
        End Function

        Public Sub Close()
            SendCmd("CLOSE")
            cliSocket.Close()
        End Sub


#Region "system error and info"       
        Public Sub EmergencyReset()
            Me.SendCmd("ERRLOGCLR")
        End Sub
#End Region

#Region "Config"
        Public Property ServoMode() As EServoStatus
            Get

            End Get
            Set(ByVal value As EServoStatus)
                Select Case value
                    Case EServoStatus.ServoON
                        Me.SendCmd("SRVON")
                    Case EServoStatus.ServoOFF
                        Me.SendCmd("SRVOFF")
                End Select
            End Set
        End Property

        Public ReadOnly Property CurrentPosition() As bhRobotArm.PositionStructure
            Get
                Dim p As bhRobotArm.PositionStructure
                Dim s As String
                Dim data() As String

                s = Me.QueryString("PPOSF")
                s = Me.QueryString("PPOSF")
                data = s.Split(";")
                p.X = data(1)
                p.Y = data(3)
                p.Z = data(5)
                p.A = data(7)
                p.B = data(9)
                p.C = data(11)
                Return p
            End Get
        End Property
        Public ReadOnly Property CurrentJointPosition() As bhRobotArm.PositionStructure
            Get
                Dim p As bhRobotArm.PositionStructure
                Dim s As String
                Dim data() As String

                s = Me.QueryString("JPOSF")
                s = Me.QueryString("JPOSF")
                data = s.Split(";")
                p.X = data(1)
                p.Y = data(3)
                p.Z = data(5)
                p.A = data(7)
                p.B = data(9)
                p.C = data(11)
                Return p

            End Get
        End Property

        Public Property Speed() As Double
            Get

            End Get
            Set(ByVal value As Double)
                Me.SendCmd("OVRD=" + value.ToString("0.00"))
            End Set
        End Property


        Public Function SavePositionToP1(ByVal point As String) As Boolean
            Dim position As bhRobotArm.PositionStructure
            position = Me.CurrentPosition
            Dim sData As String
            sData = point + "=(" + position.X.ToString + "," + position.Y.ToString + "," + position.Z.ToString + "," _
                            + position.A.ToString + "," + position.B.ToString + "," + position.C.ToString + ")" + "(7,0)"
            Return Me.SendCmd(sData)
        End Function
#End Region

#Region "Home, Move and More"
        Public Function MoveToSafe() As Boolean
            Return Me.SendCmd("MOVSP")
        End Function

        Public Function Align() As Boolean
            Return Me.SendCmd("ALIGN")
        End Function

        Public Function Move(ByVal Target As bhRobotArm.PositionStructure) As Boolean
            Dim s, fmt As String
            fmt = "{0,9}{1,7},{2,7},{3,7},{4,7},{5,7},{6,7}"
            s = String.Format(fmt, "EXECMOV (", Target.X.ToString("0.00"), Target.Y.ToString("0.00"), Target.Z.ToString("0.00"), Target.A.ToString("0.00"), _
                              Target.B.ToString("0.00"), Target.C.ToString("0.00")) + ")(7,0)"
            s = QueryString(s)
            If s.Contains("oK") Then
                Return True
            Else : Return False
            End If

        End Function

        Public Function MoveToP1() As Boolean
            Return Me.SendCmd("EXECMOV P1")
        End Function

        Public Function DriveRelative(ByVal axis As EAxis) As Boolean
            Select Case axis
                Case EAxis.X
                    Me.SendCmd("JOG00;00;00;01;00;00")
                Case EAxis.Y
                    Me.SendCmd("JOG00;00;00;02;00;00")
                Case EAxis.Z
                    Me.SendCmd("JOG00;00;00;04;00;00")
                Case EAxis.A
                    Me.SendCmd("JOG00;00;00;08;00;00")
                Case EAxis.B
                    Me.SendCmd("JOG00;00;00;10;00;00")
                Case EAxis.C
                    Me.SendCmd("JOG00;00;00;20;00;00")
            End Select
            Return True
        End Function

        Public Function DriveRelativeback(ByVal axis As EAxis) As Boolean
            Select Case axis
                Case EAxis.X
                    Me.SendCmd("JOG00;00;01;00;00;00")
                Case EAxis.Y
                    Me.SendCmd("JOG00;00;02;00;00;00")
                Case EAxis.Z
                    Me.SendCmd("JOG00;00;04;00;00;00")
                Case EAxis.A
                    Me.SendCmd("JOG00;00;08;00;00;00")
                Case EAxis.B
                    Me.SendCmd("JOG00;00;10;00;00;00")
                Case EAxis.C
                    Me.SendCmd("JOG00;00;20;00;00;00")
            End Select
            Return True
        End Function

        Public ReadOnly Property OperationComplete() As Boolean
            Get
                Return mComplete
            End Get
        End Property

        Public Sub StopMotion()            
            SendCmd("STOP")
        End Sub
#End Region

#Region "socket communication"

        Public Function SendCmd(ByVal sCmd As String) As Boolean
            Dim sRobot_ID As String = "1;1;"
            Dim msg As Byte() = Encoding.UTF8.GetBytes(sRobot_ID + sCmd)
            Dim bytes() As Byte = New Byte(1024) {}
            Dim data As String = String.Empty

            cliSocket.Send(msg)
            'Dim s As String
            's = ReadString()

            'Return s.Contains("oK")
            'mComplete = True
            Return True
        End Function

        Public Function QueryString(ByVal sCmd As String) As String
            Dim sRobot_ID As String = "1;1;"
            Dim msg As Byte() = Encoding.UTF8.GetBytes(sRobot_ID + sCmd)
            cliSocket.Send(msg)

            Dim s As String
            s = ReadString()

            Return s
        End Function

        Public Function ReadString() As String
            Dim timeOut = 200000
            Dim Tstart As Date = Now
            Dim bytesRec As Integer
            Dim bytes() As Byte = New Byte(1024) {}
            Dim data As String = String.Empty

            While bytesRec <= 0 And Now.Subtract(Tstart).TotalMilliseconds < timeOut
                bytesRec = cliSocket.Receive(bytes)
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec)
            End While

            Return data
        End Function
#End Region

    End Class
End Namespace

