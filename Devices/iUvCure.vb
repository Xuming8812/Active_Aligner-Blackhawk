Option Strict On
Option Explicit On

Namespace Instrument
    Public MustInherit Class iUvCure
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

    Public Class iFUWO
        Inherits iUvCure

        Public Enum ChannelEnum
            Channel1 = 1
            Channel2 = 2
            Channel3 = 3
            Channel4 = 4
            ChannelAll = &HFF
        End Enum

        Private mPort As IO.Ports.SerialPort

        Public Property ActiveChannel As ChannelEnum

        Public Sub New(ByVal iActiveChannel As ChannelEnum)
            Me.ActiveChannel = iActiveChannel
        End Sub

        Public Overrides Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean
            Dim success As Boolean
            Dim dataSent(), DataReceived() As Byte

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 300
                mPort.ReadTimeout = 1000

                'establish communication and set exposure to auto program 
                dataSent = New Byte() {&H20, &H4}
                DataReceived = New Byte() {}
                success = Me.SendCmd(dataSent, DataReceived)
                If success Then success = (DataReceived(0) = &H4)

                'pick program A
                dataSent = New Byte() {&H8, &H1}
                DataReceived = New Byte() {}
                success = Me.SendCmd(dataSent, DataReceived)
                If success Then success = (DataReceived(0) = &H1)

                If success Then
                    Return True
                Else

                    If RaiseError Then MessageBox.Show("Cannot estabilish communication with FUWO.", "FUWO UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If mPort.IsOpen Then mPort.Close()
                    Return False
                End If

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "FUWO UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try

        End Function

        Public Overrides Sub Close()
            mPort.Close()
        End Sub

        Protected Overrides Sub StartExposure()
            Me.ShutterOpen = True
        End Sub

        Protected Overrides ReadOnly Property ExposureRunning As Boolean
            Get
                Return Me.ShutterOpen
            End Get
        End Property

        Public Overrides Property ShutterOpen As Boolean
            Get
                Dim data As Byte() = New Byte() {}
                Me.SendCmd(New Byte() {&H0, &HFF}, data)
                Select Case Me.ActiveChannel
                    Case ChannelEnum.Channel1, ChannelEnum.ChannelAll
                        Return data(0) = 1
                    Case ChannelEnum.Channel2
                        Return data(1) = 1
                    Case ChannelEnum.Channel3
                        Return data(2) = 1
                    Case ChannelEnum.Channel4
                        Return data(3) = 1
                    Case Else
                        Return False
                End Select
            End Get
            Set(value As Boolean)
                Dim data As Byte() = New Byte() {}
                If value Then
                    Me.SendCmdWithChannelInfo(&H2, data)
                Else
                    Me.SendCmd(New Byte() {&H6, &HFF}, data)
                End If
            End Set
        End Property

#Region "serial communication"
        Private Function SendCmdWithChannelInfo(ByVal cmd As Byte, ByRef DataIn() As Byte) As Boolean
            Dim data(1) As Byte
            data(0) = cmd
            data(1) = Convert.ToByte(Me.ActiveChannel)
            Return Me.SendCmd(data, DataIn)
        End Function

        Private Function SendCmd(ByVal DataOut() As Byte, ByRef DataIn() As Byte) As Boolean
            Dim i, length, checksum As Integer
            Dim Cmd() As Byte

            length = DataOut.Length + 4
            ReDim Cmd(length - 1)

            'build command
            Cmd(0) = &H3C
            Cmd(1) = Convert.ToByte(DataOut.Length + 1)             'includes the check sum

            checksum = 0
            For i = 0 To DataOut.Length - 1
                Cmd(i + 2) = DataOut(i)
                checksum += DataOut(i)
            Next
            Cmd(length - 2) = Convert.ToByte(checksum And &HFF)
            Cmd(length - 1) = &HD

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()

            mPort.Write(Cmd, 0, length)

            'delay
            System.Threading.Thread.Sleep(20)

            'read data back
            mPort.Read(Cmd, 0, 2)
            'cmd(1) contains the length of the returnd data
            length = Cmd(1)
            mPort.Read(Cmd, 0, length)

            'cmd now contains the checksum and ending code, we will ignore them, no checksum check
            'but we will check the return command data, it should be want is sent + 1
            If Cmd(0) = DataOut(0) + 1 Then
                length = length - 3   'no cmd, no checksum, no ending type
                ReDim DataIn(length - 1)
                For i = 0 To length - 1
                    DataIn(i) = Cmd(i + 1)
                Next
                Return True
            Else
                Return False
            End If
        End Function

#End Region

        Public Overrides ReadOnly Property AlarmOn As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Function CheckAlarm(CheckCalibration As Boolean) As Boolean
            Return True
        End Function

        Public Overrides ReadOnly Property ExposureFailed As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Property ExposureTime As Double
            Get
                Dim data() As Byte = New Byte() {}
                Me.SendCmdWithChannelInfo(&HE, data)
                Return 0.1 * BitConverter.ToInt32(data, 0)
            End Get
            Set(value As Double)
                Dim data() As Byte = New Byte() {}
                Dim NewData() As Byte
                Dim x As Integer

                Me.SendCmdWithChannelInfo(&HE, data)
                x = Convert.ToInt32(10 * value)

                ReDim NewData(6)
                NewData(0) = &HC
                NewData(1) = Convert.ToByte(x And &HFF)
                NewData(2) = Convert.ToByte((x >> 8) And &HFF)
                NewData(3) = Convert.ToByte((x >> 16) And &HFF)
                NewData(4) = Convert.ToByte((x >> 24) And &HFF)
                NewData(5) = data(4)
                NewData(6) = data(5)

                Me.SendCmd(NewData, data)
            End Set
        End Property

        Public Overrides Property ActiveSingleChannel As Integer
            Get
                Return ActiveChannel
            End Get
            Set(value As Integer)
                ActiveChannel = CType(value, ChannelEnum)
            End Set
        End Property

        Public Overrides Property PowerLevel As Double
            Get
                Dim data() As Byte = New Byte() {}
                Me.SendCmdWithChannelInfo(&HE, data)
                Return data(4)
            End Get
            Set(value As Double)
                Dim data() As Byte = New Byte() {}
                Dim NewData() As Byte
                Dim x As Byte

                Me.SendCmdWithChannelInfo(&HE, data)
                x = Convert.ToByte(value)

                ReDim NewData(6)
                NewData(0) = &HC
                NewData(1) = data(0)
                NewData(2) = data(1)
                NewData(3) = data(2)
                NewData(4) = data(3)
                NewData(5) = x
                NewData(6) = data(5)

                Me.SendCmd(NewData, data)
            End Set
        End Property

        Public Overrides Property LampOn As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
                'do nothing
            End Set
        End Property

        Public Overrides ReadOnly Property LampReady As Boolean
            Get
                'FUWO does not have RS232 command for this, it has a digit output
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property NeedCalibration As Boolean
            Get
                Return False
            End Get
        End Property
    End Class

    Public Class iFUTANSI
        Inherits iUvCure

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

    Public Class iLamplic
        Inherits iUvCure

        Public Enum ChannelEnum
            Channel1 = 0
            Channel2 = 1
            Channel3 = 2
            Channel4 = 3
            ChannelAll = &HFF
        End Enum

        Public Enum ECmd
            SetParameters = 1
            GetParameters
            EnableBeep
            EnablePedalMode
            EnableKeypadMode
            GetWorkMode
            GetCureTime
            EnableAllChannelsCure
            OnOffSingleChannelCure
            EnableChannel
            SetAddress
        End Enum

        Public Enum ECureType
            ConstantPower
            StepPower
        End Enum

        Public Structure UvParameterStructure
            Dim CureType As ECureType
            Dim StepCount As Integer
            Dim CureTime() As Double
            Dim CurePower() As Integer
        End Structure

        Private mPort As IO.Ports.SerialPort
        Private mAddress As Integer

        Public Property ActiveChannel As ChannelEnum

        Public Sub New(ByVal iActiveChannel As ChannelEnum)
            Me.ActiveChannel = iActiveChannel
        End Sub

        Public Overrides Function Initialize(ByVal sPort As String, ByVal RaiseError As Boolean) As Boolean
            'Dim success As Boolean

            'set serial port
            mPort = New IO.Ports.SerialPort(sPort, 9600, IO.Ports.Parity.None, 8, IO.Ports.StopBits.One)

            'open port
            Try
                mPort.Open()
                mPort.Handshake = IO.Ports.Handshake.None
                mPort.WriteTimeout = 300
                mPort.ReadTimeout = 1000

                Me.Address = 1
                System.Threading.Thread.Sleep(200)
                Dim para As UvParameterStructure
                para = UVParameter(Me.ActiveChannel)
                System.Threading.Thread.Sleep(200)
                para.StepCount = 1
                para.CureTime(0) = 10
                para.CurePower(0) = 40
                Me.UVParameter(Me.ActiveChannel) = para

                para = UVParameter(1)
                System.Threading.Thread.Sleep(200)
                para.StepCount = 1
                para.CureTime(0) = 10
                para.CurePower(0) = 40
                Me.UVParameter(1) = para




                System.Threading.Thread.Sleep(200)
                para = Me.UVParameter(Me.ActiveChannel)
                System.Threading.Thread.Sleep(200)
                For i = 1 To 4
                    Me.EnableChannel(Me.ActiveChannel) = False
                    System.Threading.Thread.Sleep(200)
                Next

                If para.StepCount = 1 Then
                    Return True
                Else

                    If RaiseError Then MessageBox.Show("Cannot estabilish communication with Lamplic.", "Lamplic UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If mPort.IsOpen Then mPort.Close()
                    Return False
                End If

            Catch e As Exception
                If RaiseError Then MessageBox.Show(e.Message, "Lamplic UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If mPort.IsOpen Then mPort.Close()
                Return False

            End Try
        End Function

        Public Overrides Sub Close()
            For i = 1 To 4
                Me.SetChannel(ActiveChannel) = False
                Me.EnableChannel(i) = False
            Next
            mPort.Close()
        End Sub

        Protected Overrides Sub StartExposure()
            Me.ShutterOpen = True
        End Sub

        Protected Overrides ReadOnly Property ExposureRunning As Boolean
            Get
                Return Me.ShutterOpen
            End Get
        End Property

        Public Overrides Property ShutterOpen As Boolean
            Get
                Return Now.Subtract(mStartTime).TotalSeconds < mExpectedTime
            End Get
            Set(value As Boolean)
                Dim data As Byte() = New Byte() {}
                If value Then
                    Select Case ActiveChannel
                        Case ChannelEnum.Channel1
                            Me.EnableChannel(0) = True
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(0) = True
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel2
                            Me.EnableChannel(1) = True
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(1) = True
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel3
                            Me.EnableChannel(2) = True
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(2) = True
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel4
                            Me.EnableChannel(3) = True
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(3) = True
                            System.Threading.Thread.Sleep(200)
                    End Select



                    'Me.EnableChannel(ActiveChannel) = True
                    'System.Threading.Thread.Sleep(200)
                    'Me.SetChannel(ActiveChannel) = True
                    'System.Threading.Thread.Sleep(200)
                    'Me.EnableChannel(ChannelEnum.Channel2) = True
                    'System.Threading.Thread.Sleep(200)
                    'Me.SetChannel(ChannelEnum.Channel2) = True
                Else
                    Select Case ActiveChannel
                        Case ChannelEnum.Channel1
                            Me.EnableChannel(0) = False
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(0) = False
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel2
                            Me.EnableChannel(1) = False
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(1) = False
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel3
                            Me.EnableChannel(2) = False
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(2) = False
                            System.Threading.Thread.Sleep(200)
                        Case ChannelEnum.Channel4
                            Me.EnableChannel(3) = False
                            System.Threading.Thread.Sleep(200)
                            Me.SetChannel(3) = False
                            System.Threading.Thread.Sleep(200)
                    End Select


                    'Me.EnableChannel(ActiveChannel) = False
                    'System.Threading.Thread.Sleep(200)
                    'Me.SetChannel(ActiveChannel) = False
                End If
            End Set
        End Property

#Region "serial communication"
        Private Function SendCmdWithChannelInfo(ByVal cmd As Byte, ByRef DataIn() As Byte) As Boolean
            Dim data(1) As Byte
            data(0) = cmd
            data(1) = Convert.ToByte(Me.ActiveChannel)
            Return Me.SendCmd(data, DataIn)
        End Function

        Private Function SendCmd(ByVal DataOut() As Byte, ByRef DataIn() As Byte) As Boolean
            Dim i, length, checksum As Integer
            Dim Cmd() As Byte

            length = DataOut.Length + 4
            ReDim Cmd(length - 1)

            'build command
            Cmd(0) = &H3C
            Cmd(1) = Convert.ToByte(DataOut.Length + 1)             'includes the check sum

            checksum = 0
            For i = 0 To DataOut.Length - 1
                Cmd(i + 2) = DataOut(i)
                checksum += DataOut(i)
            Next
            Cmd(length - 2) = Convert.ToByte(checksum And &HFF)
            Cmd(length - 1) = &HD

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()

            mPort.Write(Cmd, 0, length)

            'delay
            System.Threading.Thread.Sleep(20)

            'read data back
            mPort.Read(Cmd, 0, 2)
            'cmd(1) contains the length of the returnd data
            length = Cmd(1)
            mPort.Read(Cmd, 0, length)

            'cmd now contains the checksum and ending code, we will ignore them, no checksum check
            'but we will check the return command data, it should be want is sent + 1
            If Cmd(0) = DataOut(0) + 1 Then
                length = length - 3   'no cmd, no checksum, no ending type
                ReDim DataIn(length - 1)
                For i = 0 To length - 1
                    DataIn(i) = Cmd(i + 1)
                Next
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub SendCmd(ByVal CmdID As Integer, ByVal bData() As Byte)
            Dim e As New System.Text.ASCIIEncoding
            Dim i, ii, length As Integer
            Dim CRC As Integer
            Dim Data() As Byte

            'build command, "data" length does not include Pre-Code, lengh, and CRC
            If bData IsNot Nothing Then
                ii = bData.Length
                length = 6 + ii
                ReDim Data(length - 1)
            Else
                ReDim Data(6)
                length = Data.Length
                ii = 1
            End If

            'pre code - RS232
            Data(0) = &H55
            Data(1) = &HAA
            'length
            Data(2) = Convert.ToByte(4 + ii - 1)
            Data(3) = Convert.ToByte(mAddress)
            Data(4) = Convert.ToByte(CmdID)
            'Data(5) = Convert.ToByte(CmdChannel)

            'data
            If bData IsNot Nothing Then
                For i = 0 To ii - 1
                    Data(5 + i) = bData(i)
                Next
            End If

            'calculate CRC
            CRC = 0
            For i = 0 To length - 2
                CRC += Data(i)
            Next
            'CRC
            Data(length - 1) = Convert.ToByte(CRC And &HFF)

            'send data
            mPort.DiscardInBuffer()
            mPort.DiscardOutBuffer()
            mPort.Write(Data, 0, Data.Length)

            'delay
            System.Threading.Thread.Sleep(20)
        End Sub

        Private Function QueryData(ByVal CmdID As Integer, ByVal bData() As Byte) As Byte()
            Dim data() As Byte = New Byte(60) {}
            Dim NewData() As Byte
            Dim len As Integer

            Me.SendCmd(CByte(CmdID), bData)
            System.Threading.Thread.Sleep(1000)
            mPort.Read(data, 0, mPort.BytesToRead)

            len = data(2)
            len += 3

            ReDim NewData(len - 1)

            Array.Copy(data, NewData, NewData.Length)

            Return NewData
        End Function

        Private Function QueryData(ByVal CmdID As Integer, ByVal bData() As Byte, ByVal BytesToRead As Integer) As Byte()
            Dim data() As Byte = New Byte(60) {}
            'Dim NewData() As Byte
            Dim n As Integer

            Me.SendCmd(CByte(CmdID), bData)
            While mPort.BytesToRead < BytesToRead And n < 15
                System.Threading.Thread.Sleep(200)
                n += 1
            End While

            If mPort.BytesToRead < 20 Or mPort.BytesToRead > 60 Then
                System.Threading.Thread.Sleep(200)
                n = 0
                Me.SendCmd(CByte(CmdID), bData)
                While mPort.BytesToRead < BytesToRead And n < 15
                    System.Threading.Thread.Sleep(200)
                    n += 1
                End While
            End If

            mPort.Read(data, 0, mPort.BytesToRead)

            'len = data(2)
            'len += 3

            'ReDim NewData(len - 1)

            'Array.Copy(data, NewData, NewData.Length)

            Return data
        End Function

#End Region

        Public Overrides ReadOnly Property AlarmOn As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Function CheckAlarm(CheckCalibration As Boolean) As Boolean
            Return True
        End Function

        Public Overrides ReadOnly Property ExposureFailed As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Property ExposureTime As Double
            Get
                Dim para As UvParameterStructure
                Select Case ActiveChannel
                    Case ChannelEnum.Channel1
                        para = Me.UVParameter(0)
                    Case ChannelEnum.Channel2
                        para = Me.UVParameter(1)
                    Case ChannelEnum.Channel3
                        para = Me.UVParameter(2)
                    Case ChannelEnum.Channel4
                        para = Me.UVParameter(3)
                End Select


                'para = Me.UVParameter(ActiveChannel)
                Return para.CureTime(0)
            End Get
            Set(value As Double)
                Dim para As UvParameterStructure
                System.Threading.Thread.Sleep(200)
                Select Case ActiveChannel
                    Case ChannelEnum.Channel1
                        para = Me.UVParameter(0)
                        para.CureTime(0) = value
                        System.Threading.Thread.Sleep(200)
                        Me.UVParameter(0) = para
                    Case ChannelEnum.Channel2
                        para = Me.UVParameter(1)
                        para.CureTime(0) = value
                        System.Threading.Thread.Sleep(200)
                        Me.UVParameter(1) = para
                    Case ChannelEnum.Channel3
                        para = Me.UVParameter(2)
                        para.CureTime(0) = value
                        System.Threading.Thread.Sleep(200)
                        Me.UVParameter(2) = para
                    Case ChannelEnum.Channel4
                        para = Me.UVParameter(3)
                        para.CureTime(0) = value
                        System.Threading.Thread.Sleep(200)
                        Me.UVParameter(3) = para
                End Select



                'para = Me.UVParameter(0)
                'para.CureTime(0) = value
                'System.Threading.Thread.Sleep(200)
                'Me.UVParameter(0) = para
                'For i = 0 To 1
                '    System.Threading.Thread.Sleep(200)
                '    para = Me.UVParameter(i)
                '    para.CureTime(0) = value
                '    System.Threading.Thread.Sleep(200)
                '    Me.UVParameter(i) = para
                'Next
            End Set
        End Property

        Public Overrides Property ActiveSingleChannel As Integer
            Get
                Return ActiveChannel
            End Get
            Set(value As Integer)
                ActiveChannel = CType(value, ChannelEnum)
            End Set
        End Property

        Public Overrides Property PowerLevel As Double
            Get
                Dim para As UvParameterStructure
                Select Case ActiveChannel
                    Case ChannelEnum.Channel1
                        para = Me.UVParameter(0)
                    Case ChannelEnum.Channel2
                        para = Me.UVParameter(1)
                    Case ChannelEnum.Channel3
                        para = Me.UVParameter(2)
                    Case ChannelEnum.Channel4
                        para = Me.UVParameter(3)
                End Select
                'para = Me.UVParameter(ActiveChannel)
                Return para.CurePower(0)
            End Get
            Set(value As Double)
                If value < 0 Or value > 100 Then
                    MessageBox.Show("Make Sure Power is from 0 to 100%!", "Lamplic UV Lamp", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Dim para As UvParameterStructure
                    System.Threading.Thread.Sleep(100)

                    Select Case ActiveChannel
                        Case ChannelEnum.Channel1
                            para = Me.UVParameter(0)
                            para.CurePower(0) = CInt(value)
                            System.Threading.Thread.Sleep(100)
                            Me.UVParameter(0) = para
                        Case ChannelEnum.Channel2
                            para = Me.UVParameter(1)
                            para.CurePower(0) = CInt(value)
                            System.Threading.Thread.Sleep(100)
                            Me.UVParameter(1) = para
                        Case ChannelEnum.Channel3
                            para = Me.UVParameter(2)
                            para.CurePower(0) = CInt(value)
                            System.Threading.Thread.Sleep(100)
                            Me.UVParameter(2) = para
                        Case ChannelEnum.Channel4
                            para = Me.UVParameter(3)
                            para.CurePower(0) = CInt(value)
                            System.Threading.Thread.Sleep(100)
                            Me.UVParameter(3) = para
                    End Select



                    'para = Me.UVParameter(0)
                    'para.CurePower(0) = CInt(value)
                    'System.Threading.Thread.Sleep(100)
                    'Me.UVParameter(0) = para
                    'For i = 0 To 1
                    '    System.Threading.Thread.Sleep(100)
                    '    para = Me.UVParameter(i)
                    '    para.CurePower(0) = CInt(value)
                    '    System.Threading.Thread.Sleep(100)
                    '    Me.UVParameter(i) = para
                    'Next
                End If
            End Set
        End Property

        Public Overrides Property LampOn As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
                'do nothing
            End Set
        End Property

        Public Overrides ReadOnly Property LampReady As Boolean
            Get
                'FUWO does not have RS232 command for this, it has a digit output
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property NeedCalibration As Boolean
            Get
                Return False
            End Get
        End Property

        Public Property Address As Integer
            Get
                Return mAddress
            End Get
            Set(value As Integer)
                Dim data(0) As Byte
                data(0) = CByte(value)
                SendCmd(ECmd.SetAddress, data)
                mAddress = value
            End Set
        End Property

        Public Property UVParameter(ByVal channel As Integer) As UvParameterStructure
            Get
                Dim data(0) As Byte
                data(0) = CByte(channel)
                data = QueryData(ECmd.GetParameters, data, 54)
                Dim para As UvParameterStructure = New UvParameterStructure()
                para.CureType = CType([Enum].Parse(GetType(ECureType), data(6).ToString()), ECureType)
                para.StepCount = data(7)
                ReDim para.CureTime(para.StepCount)
                ReDim para.CurePower(para.StepCount)
                For i As Integer = 0 To para.StepCount
                    para.CureTime(i) = (255 * data(9 + 2 * i) + data(8 + 2 * i)) / 10
                    para.CurePower(i) = data(40 + i)
                Next

                Return para
            End Get
            Set(value As UvParameterStructure)
                Dim data(50) As Byte
                data(0) = CByte(channel)
                data(1) = CByte(value.CureType)
                data(2) = CByte(1)
                For i As Integer = 0 To value.StepCount - 1
                    data(3 + 2 * i) = CByte(10 * value.CureTime(i) Mod 255)
                    data(4 + 2 * i) = CByte(CInt(10 * value.CureTime(i)) \ 255)
                    data(35 + i) = CByte(value.CurePower(i))
                Next
                SendCmd(ECmd.SetParameters, data)
            End Set
        End Property

        Public Property EnableChannel(ByVal channel As Integer) As Boolean
            Get

            End Get
            Set(value As Boolean)
                Dim data(3) As Byte
                For i As Integer = 0 To 3
                    data(i) = 1
                Next
                If value Then
                    data(channel) = 1
                Else
                    data(channel) = 0
                End If
                SendCmd(ECmd.EnableChannel, data)
            End Set
        End Property

        Public Property SetChannel(ByVal channel As Integer) As Boolean
            Get

            End Get
            Set(value As Boolean)
                Dim data(0) As Byte
                data(0) = CByte(49 + channel)
                SendCmd(ECmd.OnOffSingleChannelCure, data)
            End Set
        End Property

    End Class

    Public Class iOmniCure
        Inherits iUvCure
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

