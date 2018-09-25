Option Strict On
Option Explicit On

Namespace Instrument
    Public MustInherit Class iUvCureSecond
        Private mWorker As System.ComponentModel.BackgroundWorker

        Public Sub New()
            mWorker = New System.ComponentModel.BackgroundWorker
            mWorker.WorkerSupportsCancellation = True
            AddHandler mWorker.DoWork, AddressOf mWorker_DoWork
            AddHandler mWorker.RunWorkerCompleted, AddressOf mWorker_RunWorkerCompleted
        End Sub

        Public MustOverride Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean
        Public MustOverride Sub Close()

        Public MustOverride Property ShutterOpen As Boolean
        Public MustOverride Property LampOn As Boolean

        Public MustOverride Property ExposureTime As Double
        Public MustOverride Property PowerLevel() As Double

        Public MustOverride ReadOnly Property AlarmOn As Boolean
        Public MustOverride ReadOnly Property LampReady() As Boolean
        Public MustOverride ReadOnly Property NeedCalibration() As Boolean
        Public MustOverride ReadOnly Property ExposureFailed() As Boolean

        Public MustOverride Property ActiveSingleChannel As Integer


        'following are used for async run
        Protected MustOverride Function CheckAlarm(ByVal CheckCalibration As Boolean) As Boolean
        Protected MustOverride ReadOnly Property ExposureRunning As Boolean
        Protected MustOverride Sub StartExposure()

#Region "asycn run"
        Protected mStartTime As Date
        Protected mExpectedTime As Double

        Public ReadOnly Property ExposureInProgress() As Boolean
            Get
                Return mWorker.IsBusy
            End Get
        End Property

        Public ReadOnly Property ExposureTimePassed() As Double
            Get
                Return Date.Now.Subtract(mStartTime).TotalSeconds
            End Get
        End Property

        Public ReadOnly Property ExposureTimeExpected() As Double
            Get
                Return mExpectedTime
            End Get
        End Property

        Public Sub StopUvExposure()
            Me.ShutterOpen = False
            mWorker.CancelAsync()
        End Sub

        Public Function RunExposure(ByVal CheckCalibration As Boolean) As Boolean
            'check alarm
            'If Not Me.CheckAlarm(CheckCalibration) Then Return False
            System.Threading.Thread.Sleep(200)
            mExpectedTime = Me.ExposureTime()

            'do actual run
            Me.StartExposure()
            mStartTime = Now
            'start wait  
            mWorker.RunWorkerAsync()

            'return success
            Return True
        End Function

        Protected Sub mWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
            mStartTime = Date.Now
            While Me.ExposureRunning
                System.Threading.Thread.Sleep(100)
                If mWorker.CancellationPending Then Exit While
            End While
        End Sub

        Protected Sub mWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
            'do nothing
        End Sub
#End Region

    End Class

    Public Class iFUTANSISecond
        Inherits iUvCureSecond

        Private mPort As IO.Ports.SerialPort

        Public Enum ChannelEnum
            Channel1 = 1
            Channel2 = 2
            Channel3 = 3
            Channel4 = 4
            ChannelAll = &HFF
        End Enum

        Public Property ActiveChannel As ChannelEnum

        Public Overrides Property ActiveSingleChannel As Integer
            Get
                Return ActiveChannel
            End Get
            Set(value As Integer)
                ActiveChannel = CType(value, ChannelEnum)
            End Set
        End Property

        Public Sub New(ByVal iActiveChannel As ChannelEnum)
            Me.ActiveChannel = iActiveChannel
        End Sub

        Public forestring As String
        Public backstring As String

        Public controlString As String

        Public Overrides Function Initialize(sPort As String, RaiseError As Boolean) As Boolean
            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 300
                mPort.ReadTimeout = 1000

                'establish communication and set exposure to auto program 
                Dim s As String
                Dim i As Integer = 0
                For i = 0 To 10
                    s = Me.QueryString("FUTANSI")
                    If s.Length > 0 Then
                        Me.SendCmd(15)
                        Return True
                    Else
                        Dim cmd As Byte
                        cmd = 1
                        s = Me.QueryString(cmd)
                        If s.Length > 0 Then
                            Me.SendCmd(15)
                            Return True
                        End If
                    End If
                Next

                If RaiseError Then MessageBox.Show("Cannot estabilish communication with Futansi.", "Futansi UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "Futansi UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try
        End Function

        Public Overrides ReadOnly Property AlarmOn As Boolean
            Get

            End Get
        End Property

        Protected Overrides Function CheckAlarm(CheckCalibration As Boolean) As Boolean

        End Function

        Public Overrides Sub Close()
            mPort.Close()
        End Sub

        Public Overrides ReadOnly Property ExposureFailed As Boolean
            Get

            End Get
        End Property

        Protected Overrides ReadOnly Property ExposureRunning As Boolean
            Get

            End Get
        End Property

        Public Overrides Property ExposureTime As Double
            Get

            End Get
            Set(value As Double)
                If value >= 100 Then
                    backstring = "A " + CType(value, String) + ".0S!"
                ElseIf value > 9 Then
                    backstring = "A 0" + CType(value, String) + ".0S!"
                Else
                    backstring = "A 00" + CType(value, String) + ".0S!"
                End If

            End Set
        End Property
        Public Overrides Property LampOn As Boolean

        Public Overrides ReadOnly Property LampReady As Boolean
            Get

            End Get
        End Property

        Public Overrides ReadOnly Property NeedCalibration As Boolean
            Get

            End Get
        End Property

        Public Overrides Property PowerLevel As Double
            Get

            End Get
            Set(value As Double)
                If value >= 100 Then
                    forestring = CType(value, String) + "%"
                ElseIf value > 9 Then
                    forestring = "0" + CType(value, String) + "%"
                Else
                    forestring = "00" + CType(value, String) + "%"
                End If

            End Set
        End Property
        Public Overrides Property ShutterOpen As Boolean
            Get

            End Get
            Set(value As Boolean)
                If value Then
                    Select Case ActiveChannel
                        Case ChannelEnum.Channel1
                            controlString = "CH1 " + forestring + backstring

                        Case ChannelEnum.Channel2
                            controlString = "CH2 " + forestring + backstring
                        Case ChannelEnum.Channel3
                            controlString = "CH3 " + forestring + backstring
                        Case ChannelEnum.Channel4
                            controlString = "CH4 " + forestring + backstring
                    End Select
                    Dim s As String
                    s = Me.QueryString(controlString)
                    StopExposure()
                    StartExposure()
                Else
                    StopExposure()
                End If
            End Set
        End Property

        Protected Overrides Sub StartExposure()
            Dim cmd As Byte
            Select Case ActiveChannel
                Case ChannelEnum.Channel1
                    cmd = CByte(2 ^ 4 + 2 ^ 0)
                Case ChannelEnum.Channel2
                    cmd = CByte(2 ^ 4 + 2 ^ 1)
                Case ChannelEnum.Channel3
                    cmd = CByte(2 ^ 4 + 2 ^ 2)
                Case ChannelEnum.Channel4
                    cmd = CByte(2 ^ 4 + 2 ^ 3)
                Case ChannelEnum.ChannelAll
                    cmd = 31
            End Select
            System.Threading.Thread.Sleep(500)
            Me.SendCmd(cmd)
        End Sub

        Protected Sub StopExposure()
            Dim cmd As Byte
            Select Case ActiveChannel
                Case ChannelEnum.Channel1
                    cmd = CByte(2 ^ 0)
                Case ChannelEnum.Channel2
                    cmd = CByte(2 ^ 1)
                Case ChannelEnum.Channel3
                    cmd = CByte(2 ^ 2)
                Case ChannelEnum.Channel4
                    cmd = CByte(2 ^ 3)
                Case ChannelEnum.ChannelAll
                    cmd = 15
            End Select

            Me.SendCmd(cmd)
        End Sub

#Region "serial communication"
        Private Function SendCmd(ByVal DataOut As Byte) As Boolean
            Dim Cmd(0) As Byte
            Cmd(0) = DataOut
            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            mPort.Write(Cmd, 0, 1)

            Cmd(0) = 33
            mPort.Write(Cmd, 0, 1)

            System.Threading.Thread.Sleep(20)

            Return True
        End Function

        Private Function QueryString(ByVal sCmd As String) As String
            Dim s As String
            'clear buffer
            mPort.DiscardInBuffer()
            mPort.ReadExisting()
            System.Threading.Thread.Sleep(100)
            'send command
            Me.SendCmd(sCmd)
            System.Threading.Thread.Sleep(300)
            'read data
            s = mPort.ReadExisting()
            'remove known characters
            s = s.Replace(sCmd, "")
            s = s.Replace(ControlChars.Cr, "")
            Return s
        End Function

        Private Function QueryString(ByVal sCmd As Byte) As String
            Dim s As String
            'clear buffer
            mPort.DiscardInBuffer()
            mPort.ReadExisting()
            System.Threading.Thread.Sleep(100)
            'send command
            Me.SendCmd(sCmd)
            System.Threading.Thread.Sleep(300)
            'read data
            s = mPort.ReadExisting()
            'remove known characters
            s = s.Replace(CType(sCmd, String), "")
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
#End Region
    End Class


    Public Class iOmniCureSecond
        Inherits iUvCureSecond
        Private mPort As IO.Ports.SerialPort
        Private ActiveChannel As Integer

        Public Overrides Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 19200, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.NewLine = ControlChars.Cr
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 300
                mPort.ReadTimeout = 1000

                'establish communication
                If "READY" <> Me.QueryString("CONN") Then
                    If RaiseError Then MessageBox.Show("Cannot estabilish communication with OmniCur.", "OmniCure UV Curer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If mPort.IsOpen Then mPort.Close()
                    Return False
                Else
                    Return True
                End If

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "OmniCure UV Curer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try

            Return True
        End Function

        Public Overrides Sub Close()
            Me.SendCmd("DCON")
            mPort.Close()
        End Sub

        Protected Overrides Sub StartExposure()
            Me.SendCmd("RUN")
        End Sub

        Protected Overrides ReadOnly Property ExposureRunning As Boolean
            Get
                Dim tPassed As Double
                tPassed = Date.Now.Subtract(mStartTime).TotalSeconds
                Return (tPassed < mExpectedTime)
            End Get
        End Property

        Public Overrides Property ShutterOpen() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.ShutterOpen) = StatusMask.ShutterOpen
            End Get
            Set(ByVal value As Boolean)
                Dim s As String
                s = IIf(value, "OPN", "CLS").ToString()
                Me.SendCmd(s)
            End Set
        End Property

        Public Overrides Property LampOn() As Boolean
            Get
                Dim status As Integer = Me.GetStatus()
                Return (status And StatusMask.LampOn) = StatusMask.LampOn
            End Get
            Set(ByVal value As Boolean)
                Dim s As String
                s = IIf(value, "TON", "TOF").ToString()
                Me.SendCmd(s)
            End Set
        End Property

        Public Overrides Property ActiveSingleChannel As Integer
            Get
                Return ActiveChannel
            End Get
            Set(value As Integer)
                ActiveChannel = value
            End Set
        End Property

#Region "lamp proeprty"
        Public ReadOnly Property SerialNumber() As String
            Get
                Return Me.QueryString("GSN")
            End Get
        End Property

        Public Structure LampConfiguration
            Public Enum LampType
                SurfaceCuring = 0
                Standard = 1
            End Enum
            Public Abused As Boolean
            Public Type As LampType
            Public Hours As Integer
        End Structure

        Public ReadOnly Property Lamp() As LampConfiguration
            Get
                Dim k As Integer
                Dim x As LampConfiguration

                k = Me.QueryInteger("GLH")

                x.Abused = (k And &H8000) = &H8000

                If (k And &H4000) = &H4000 Then
                    x.Type = LampConfiguration.LampType.Standard
                Else
                    x.Type = LampConfiguration.LampType.SurfaceCuring
                End If

                x.Hours = (k And &H3FFF)

                Return x
            End Get
        End Property

        Public ReadOnly Property CalibratedLampHours() As Integer
            Get
                Return Me.QueryInteger("CLH")
            End Get
        End Property
#End Region

#Region "alarm, status"
        Private Enum StatusMask
            AlarmOn = &H1
            LampOn = &H2
            ShutterOpen = &H4
            HomeFaulty = &H8
            LampReady = &H10
            FrontLockout = &H20
            InCalibratio = &H40
            ExposureFault = &H80
        End Enum

        Private Function GetStatus() As Integer
            Dim status As Integer = Me.QueryInteger("GUS")
            Return status
        End Function

        Protected Overrides Function CheckAlarm(ByVal CheckCalibration As Boolean) As Boolean
            Dim k As Integer
            Dim s As String

            'check if we can run
            k = Me.GetStatus()
            s = ""
            If (k And StatusMask.AlarmOn) = StatusMask.AlarmOn Then s += "Alarm On" + ControlChars.CrLf
            If (k And StatusMask.LampOn) <> StatusMask.LampOn Then s += "Lamp is not on" + ControlChars.CrLf
            If (k And StatusMask.LampReady) <> StatusMask.LampReady Then s += "Lamp is not ready" + ControlChars.CrLf
            If (k And StatusMask.ShutterOpen) = StatusMask.ShutterOpen Then s += "Shutter open, exposure running" + ControlChars.CrLf

            If s <> "" Then
                MessageBox.Show(s, "UV Cure", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'warning
            'If CheckCalibration And (k And StatusMask.InCalibratio) <> StatusMask.InCalibratio Then
            '    s = "Lamp is not calibrated. Do you want to continue?"
            '    If Windows.Forms.DialogResult.Yes <> MessageBox.Show(s, "UV Cure", MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then
            '        Return False
            '    End If
            'End If

            'done
            Return True
        End Function

        Public Property FrontPanelLockout() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.FrontLockout) = StatusMask.FrontLockout
            End Get
            Set(ByVal value As Boolean)
                Dim s As String
                s = IIf(value, "LOC", "ULOC").ToString()
                Me.SendCmd(s)
            End Set
        End Property

        Public Overrides ReadOnly Property AlarmOn() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.AlarmOn) = StatusMask.AlarmOn
            End Get
        End Property

        Public Sub ClearAlarm()
            Me.SendCmd("CLR")
        End Sub

        Public Overrides ReadOnly Property NeedCalibration() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.InCalibratio) <> StatusMask.InCalibratio
            End Get
        End Property

        Public Sub ClearUnitCalibration()
            Me.SendCmd("CLC")
        End Sub

        Public Overrides ReadOnly Property LampReady() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.LampReady) = StatusMask.LampReady
            End Get
        End Property

        Public Overrides ReadOnly Property ExposureFailed() As Boolean
            Get
                Return (Me.GetStatus() And StatusMask.HomeFaulty) = StatusMask.HomeFaulty
            End Get
        End Property
#End Region

#Region "setting"
        Public Overrides Property ExposureTime() As Double
            Get
                Dim time As Double = Me.QueryInteger("GTM")
                Return 0.1 * time
            End Get
            Set(ByVal value As Double)
                value *= 10
                If value < 2 Then
                    value = 2
                ElseIf value > 9999 Then
                    value = 9999
                End If
                Me.SendCmd("STM" + value.ToString("0"))
            End Set
        End Property

        Public Overrides Property PowerLevel() As Double
            Get
                Return Me.QueryDouble("GPW")
            End Get
            Set(ByVal value As Double)
                Me.SendCmd("SPW" + value.ToString("0.00"))
            End Set
        End Property

        Public Property Irradiance() As Double
            Get
                Return Me.QueryDouble("GIR")
            End Get
            Set(ByVal value As Double)
                If value < 0 Then value = 0.1
                Me.SendCmd("SIR" + value.ToString("0.00"))
            End Set
        End Property

        Public ReadOnly Property IrradianceActual() As Double
            Get
                Return Me.QueryDouble("GIM")
            End Get
        End Property

        Public ReadOnly Property IrradianceMaximum() As Double
            Get
                Return Me.QueryDouble("GMP")
            End Get
        End Property

        Public Property IrisLevel() As Double
            Get
                Return 0.01 * Me.QueryInteger("GIL")
            End Get
            Set(ByVal value As Double)
                value *= 100.0
                If value < 1 Then
                    value = 1
                ElseIf value > 100 Then
                    value = 100
                End If
                Me.SendCmd("SIL" & value.ToString("0"))
            End Set
        End Property

#End Region

#Region "serial communication"
        Private Function QueryString(ByVal sCmd As String) As String
            Dim s As String
            Me.SendCmd(sCmd)
            s = mPort.ReadLine()
            'remove the CRC from the end
            s = s.Substring(0, s.Length - 2)
            Return s
        End Function

        Private Function QueryInteger(ByVal sCmd As String) As Integer
            Dim s As String
            Dim k As Integer
            s = Me.QueryString(sCmd)
            If Integer.TryParse(s, k) Then
                Return k
            Else
                Return 0
            End If
        End Function

        Private Function QueryDouble(ByVal sCmd As String) As Double
            Dim s As String
            Dim v As Double
            s = Me.QueryString(sCmd)
            If Double.TryParse(s, v) Then
                Return v
            Else
                Return Double.NaN
            End If
        End Function

        Private Sub SendCmd(ByVal sCmd As String)
            Dim e As New System.Text.ASCIIEncoding
            Dim CRC As Byte
            Dim Data() As Byte

            'build command
            Data = e.GetBytes(sCmd)
            CRC = w2Math.CheckSum.CRC8(Data)

            sCmd += Convert.ToString(CRC, 16).ToUpper()
            sCmd += ControlChars.Cr

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            mPort.Write(sCmd)

            'delay
            System.Threading.Thread.Sleep(20)
        End Sub


#End Region

    End Class
End Namespace

