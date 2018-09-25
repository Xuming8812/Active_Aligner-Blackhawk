Option Explicit On
Option Strict On

Imports PI
Namespace Instrument

    Public Class iPiLS65
        Inherits iMotionController

        Private mPort As IO.Ports.SerialPort

        Public Sub New(ByVal AxisCount As Integer)
            MyBase.New(AxisCount)
        End Sub

        Public Overrides Function Initialize(sPort As String, RaiseError As Boolean) As Boolean
            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 1000
                mPort.ReadTimeout = 5000

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "PI LS65", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False
            End Try

            Return True
        End Function

        Public Overrides Sub Close()
            mPort.Close()
        End Sub

        Public Overrides ReadOnly Property StageReady() As Boolean
            Get
                Dim reply As String
                Dim statusCode As Byte
                reply = Me.QueryData("nstatus")
                statusCode = CByte(reply)
                Return (statusCode And 133) <> 1
            End Get
        End Property

        Public Overrides Property Axis() As Integer
            Get
                Return mAxis
            End Get
            Set(ByVal value As Integer)
                mAxis = value
            End Set
        End Property

        Public Overrides ReadOnly Property AxisName As String
            Get
                Return mAxis.ToString()
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentPosition As Double
            Get
                Dim reply As String
                reply = Me.QueryData("npos")
                Return CDbl(reply)
            End Get
        End Property

        Public Overrides ReadOnly Property StageMoving As Boolean
            Get
                 Dim reply As String
                Dim statusCode As Byte
                reply = Me.QueryData("nstatus")
                statusCode = CByte(reply)
                Return (statusCode And 1) = 1
            End Get
        End Property

        Public Overrides Function InitializeMotion() As Boolean
            DriveEnabled = True
            Return True
        End Function

        Protected Overrides Function StartHome() As Boolean
            Me.SendCmd("ncal")
            Return True
        End Function

        Protected Overrides Function StartMove(Method As iMotionController.MoveToTargetMethodEnum, Target As Double) As Boolean
            'check ready
            If Me.StageMoving Then Return False

            Dim value(0) As Double
            value(0) = Target

            'move
            Select Case Method
                Case MoveToTargetMethodEnum.Absolute
                    Me.SendCmd(Target, "nmove")
                Case MoveToTargetMethodEnum.Relative
                    Me.SendCmd(Target, "nrmove")
            End Select

            'return
            Return True
        End Function

        Public Overrides Property DriveEnabled As Boolean
            Get
                Dim reply As String
                reply = Me.QueryData("getaxis")
                Return CInt(reply) <> 0
            End Get
            Set(value As Boolean)
                Me.SendCmd(CInt(IIf(value, 1, 0)), "setaxis")
            End Set
        End Property

        Public Overrides Function HaltMotion() As Boolean
            Me.SendCmd("nabort")
            Return True
        End Function

        Public Overrides Function KillMotion() As Boolean
            Me.SendCmd(Double.NaN, "nabort")
            Return True
        End Function

        Public Overrides Function KillMotionAllAxis() As Boolean
            Me.SendCmd(Double.NaN, "nabort")
            Return True
        End Function

#Region "system error and info"
        Public Overrides ReadOnly Property ControllerVersion() As String
            Get
                Dim s As String
                s = Me.QueryData("nversion")
                Return s
            End Get
        End Property

        Public Overrides ReadOnly Property StageData() As iMotionController.StageInformation
            Get
                Dim x As StageInformation
                x.ModelNumber = Me.QueryData("nidentify")
                x.Partnumber = Me.QueryData("nidentify")
                x.SerialNumber = Me.QueryData("getserialno")
                Return x
            End Get
        End Property

        Public ReadOnly Property LastErrorCode() As Integer
            Get
                Dim s As String = Me.QueryData("getnerror")
                Return CInt(s)
            End Get
        End Property

        Public Overrides ReadOnly Property LastError() As String
            Get
                Dim s As String = Me.QueryData("getnerror")
                Return s
            End Get
        End Property
#End Region

#Region "Aux - GPIO and ADC/DAC"
        Public Overloads Overrides ReadOnly Property DigitInput(Port As Integer) As Integer
            Get

            End Get
        End Property

        Public Overloads Overrides Property DigitOutput(Port As Integer) As Integer
            Get

            End Get
            Set(value As Integer)

            End Set
        End Property

        Public Overrides Function ReadAnalogyInput(Channel As Integer) As Double
            Return Double.NaN
        End Function

        Public Overrides Function SetAnalogyOutput(Channel As Integer, Value As Double) As Boolean

        End Function
#End Region

#Region "velocity acceleration"
        Public Overrides Property Acceleration As Double
            Get
                Dim reply As String
                reply = Me.QueryData("getnaccel")
                Return CDbl(reply)
            End Get
            Set(value As Double)
                Me.SendCmd(value, "setnaccel")
            End Set
        End Property

        Public Overrides Property AccelerationMaximum As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property Deceleration As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property Velocity As Double
            Get
                Dim reply As String
                reply = Me.QueryData("getnvel")
                Return CDbl(reply)
            End Get
            Set(value As Double)
                Me.SendCmd(value, "setnvel")
            End Set
        End Property

        Public Overrides Property VelocityMaximum As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property
#End Region

#Region "serial communication"
        Private Sub SendCmd(ByVal cmd As String)
            Me.SendCmd(Double.NaN, cmd)
        End Sub

        Private Sub SendCmd(ByVal parameter As Integer, ByVal cmd As String)
            Me.SendCmd(CDbl(parameter), cmd)
        End Sub

        Private Sub SendCmd(ByVal parameter As Double, ByVal cmd As String)
            Dim s As String
            If Double.IsNaN(parameter) Then
                s = mAxis & " " & cmd
            Else
                s = parameter & " " & mAxis & " " & cmd
            End If

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            mPort.WriteLine(s)

            'delay
            System.Threading.Thread.Sleep(50)
        End Sub

        Private Function QueryData(ByVal parameter As Double, ByVal cmd As String) As String
            Me.SendCmd(parameter, cmd)

            Dim reply As String
            reply = mPort.ReadLine()
            Return reply
        End Function

        Private Function QueryData(ByVal cmd As String) As String
            Me.SendCmd(Double.NaN, cmd)

            Dim reply As String
            reply = mPort.ReadLine()
            Return reply
        End Function
#End Region
    End Class
End Namespace
