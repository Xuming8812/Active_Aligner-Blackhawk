Option Explicit On
Option Strict On
Option Infer Off

Partial Public Class BlackHawkFunction
    Private mInst As InstrumentList

    Public ReadOnly Property Instruments() As InstrumentList
        Get
            Return mInst
        End Get
    End Property

    Public Class InstrumentList
        Private Const Text As String = "Instrument"

        'tool
        Private mIniFile As w2.w2IniFile
        Private mParaFile As w2.w2IniFileXML
        Private mMsg As w2.w2MessageService

        Private mPara As BlackHawkParameters

        '------------------------------------------ -Stage
        Public XPS As Instrument.iXPS
        Public PiAngle As Instrument.iPiGCS843
        Public Hexopod As Instrument.iPiGCS2
        Public PiLS As Instrument.iPiLS65

        Public XpsIO As iXpsIO
        Public StageBase As iXpsStage

        '-------------------------------------------Other
        Public ProbeClamp As Instrument.iTaiyoClamp

        'changed by Ming to run different channels
        Public UvLamp As Instrument.iUvCure

        Public UvlampSecond As Instrument.iUvCureSecond

        Public Uvlamplic As Instrument.iLamplic

        Public NanoScan As Instrument.iBeamProfiler

        Public LDD As Instrument.iMultiChannelLDD

        Public LightSource1 As Instrument.iLightSource
        Public LightSource2 As Instrument.iLightSource

        Public ForceGaugeMain As Instrument.iOmegaDP40
        Public ForceGaugeHexapod As Instrument.iOmegaDP40

        Public RCX As Instrument.iRCX

        Public RobotArm As bhRobotArm

        Public VisionDll As Instrument.iVisionSystem

        Public Sub New(ByVal hTool As BlackHawkFunction)

            mMsg = hTool.mMsgInfo
            mPara = hTool.mPara
            mIniFile = hTool.mIniFile
            mParaFile = hTool.mParaFile

        End Sub

        Public Function InitializeAll() As Boolean
            Dim success As Boolean

            success = Me.InitializeLightSource1()
            success = Me.InitializeLightSource2()

            'get XPS first, many IO are from that box
            success = Me.InitializeXPS()
            If Not success Then Return False

            success = Me.InitializePiAngle()
            If Not success Then Return False

            success = Me.InitializePiHexopod()
            If Not success Then Return False

            success = Me.InitializePiLS()
            If Not success Then Return False

            'initialize sub classes
            mMsg.PostMessage("")
            mMsg.PostMessage("Configuring the XPS IO interface... ")
            XpsIO = New iXpsIO()
            success = XpsIO.Initialize(XPS, mParaFile)
            If Not success Then Return False

            mMsg.PostMessage("")
            mMsg.PostMessage("Configuring the stage info ... ")
            StageBase = New iXpsStage
            success = StageBase.Initialize(mParaFile, XPS, PiAngle, Hexopod, PiLS)
            If Not success Then Return False

            'other standard instrument
            success = Me.InitializeClampForProbe()
            If Not success Then Return False

            success = Me.InitializeForceGauge()
            If Not success Then Return False

            success = Me.InitializeLDD()
            If Not success Then Return False

            success = Me.InitializeNanoScan()
            If Not success Then Return False

            success = Me.InitializeUvLamp()
            If Not success Then Return False

            success = Me.InitializeUvLampSecond()
            If Not success Then Return False


            success = Me.InitializeRCX()
            If Not success Then Return False

            success = Me.InitializeVision()
            If Not success Then Return False

            mMsg.PostMessage("")
            mMsg.PostMessage("Configuring the Robot info ... ")
            RobotArm = New bhRobotArm
            success = RobotArm.Initialize(RCX, mParaFile)

            If Not success Then Return False


            Return success
        End Function

        Public Function CloseAll() As Boolean
            Dim i As Integer

            If NanoScan IsNot Nothing Then NanoScan.Close()
            If UvLamp IsNot Nothing Then UvLamp.Close()
            If XPS IsNot Nothing Then XPS.Close()
            If LDD IsNot Nothing Then
                For i = 1 To LDD.ChannelCount
                    LDD.Current(i) = 0.0
                Next
            End If
            If LDD IsNot Nothing Then LDD.EnabledProtectionState = True
            If VisionDll IsNot Nothing Then VisionDll.Close()



            Return True
        End Function

#Region "indivudial instrument"
        Private Function InitializeClampForProbe() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsTaiyo", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using Taiyo Clamp for the probe pin")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing Taiyo Clamp for the probe pin " + Adrs + " ... ")

            'start
            s = ""
            ProbeClamp = New Instrument.iTaiyoClamp(0)
            success = ProbeClamp.Initialize(Adrs, True)
            If success Then
                ProbeClamp.Enabled = True
            Else
                ProbeClamp = Nothing
            End If

            Return (ProbeClamp IsNot Nothing)
        End Function

        Private Function InitializeForceGauge() As Boolean
            Dim s, adrs As String
            Dim rate As Integer

            'get baud rate
            rate = mIniFile.ReadParameter("Instrument", "ForceGaugeBaudRate", 9600)

            'force gauge for lens
            adrs = mIniFile.ReadParameter("Instrument", "AdrsForceGaugeMain", "-1")
            If adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Initializing force gauge for main tool at port " + adrs + " ... ")

                ForceGaugeMain = New Instrument.iOmegaDP40
                If Not ForceGaugeMain.Initialize(adrs, rate, True) Then
                    s = "Failed to initialize force gauge controller at COM port " + adrs + " for lens vacuum tip. Application will be terminated."
                    MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ForceGaugeMain = Nothing
                    Return False
                End If
            Else
                mMsg.PostMessage("    Configured for not useing use force gauge for lens vacuum tip")
            End If

            'force gauage for BS
            adrs = mIniFile.ReadParameter("Instrument", "AdrsForceGaugeHexapod", "-1")
            If adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Initializing force gauge for beam splitter vacuum tip at port " + adrs + " ... ")

                ForceGaugeHexapod = New Instrument.iOmegaDP40
                If Not ForceGaugeHexapod.Initialize(adrs, rate, True) Then
                    s = "Failed to initialize force gauge controller at COM port " + adrs + " for beam splitter vacuum tip. Application will be terminated."
                    MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ForceGaugeHexapod = Nothing
                    Return False
                End If
            Else
                mMsg.PostMessage("    Configured for not useing force gauge for beam splitter vacuum tip")
            End If

            Return True
        End Function

        Private Function InitializeLDD() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsLDD", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using OC9501 Driver Board")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing OC9501 driver board at " + Adrs + " ... ")

            'start
            s = ""

            LDD = New Instrument.iOC9501LDD(4, mPara.LaserDiode.DefaultCurrent)
            success = LDD.Initialize(Adrs, True)
            If success Then

            Else
                LDD = Nothing
            End If

            Return (LDD IsNot Nothing)
        End Function

        Private Function InitializeNanoScan() As Boolean
            Dim s As String
            Dim adrs As Integer
            Dim success As Boolean

            adrs = mIniFile.ReadParameter("Instrument", "AdrsBeamScan", -1)
            If adrs < 0 Then
                mMsg.PostMessage("    Configured for not using beam scan")
                Return True
            End If

            s = mIniFile.ReadParameter("Instrument", "TypeBeamScan", "")
            Select Case s
                Case "Newport"
                    mMsg.PostMessage("    Initializing NanoScan service. This may take a few minues ... ")

                    NanoScan = New Instrument.iNewportNanoScan
                    success = NanoScan.Initialize("")
                    If Not success Then
                        s = "Failed to initialize Nano Scan. Application will be closed."
                        MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If

                Case "Thorlabs"
                    mMsg.PostMessage("    Initializing Thorlabs beam scan")

                    NanoScan = New Instrument.iThorlabsBP200BeamScan
                    success = NanoScan.Initialize(adrs.ToString())
                    If Not success Then
                        s = "Failed to initialize Nano Scan. Application will be closed."
                        MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If

                Case Else
                    mMsg.PostMessage("    Beam scan type: " + s + " is not supported!")
                    Return False
            End Select

            ' If Not NanoScan.AutoGain Then NanoScan.Gain = mPara.BeamScan.Gain
            NanoScan.SamplingFrequency = mPara.BeamScan.SampleFrequency
            NanoScan.SamplingResolution = mPara.BeamScan.SampleResolution

            'Dim x As Instrument.iBeamProfiler.SimpleData
            'x = NanoScan.AcquireData(5, Instrument.iBeamProfiler.DataAcquisitionMode.PeakWidthEnergy)

            Return True
        End Function

        Private Function InitializeUvLamp() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean
            Dim r As DialogResult

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsUvLamp", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using UV Lamp")
                Return True
            End If

            s = mIniFile.ReadParameter("Instrument", "TypeUvLamp", "")
            Select Case s
                Case "FUWO"
                    'ack
                    mMsg.PostMessage("    Initializing FUWO UV LED tool at port " + Adrs + " ... ")
                    UvLamp = New Instrument.iFUWO(Instrument.iFUWO.ChannelEnum.ChannelAll)

                Case "EXFO"
                    'ack
                    mMsg.PostMessage("    Initializing OmniCure S2000 UV Lamp at port " + Adrs + " ... ")
                    UvLamp = New Instrument.iOmniCure

                Case "LAMPLIC"
                    'ack
                    mMsg.PostMessage("    Initializing Lamplic P1 UV Lamp at port " + Adrs + " ... ")
                    UvLamp = New Instrument.iLamplic(Instrument.iLamplic.ChannelEnum.Channel1)

                Case "FUTANSI"
                    'ack
                    mMsg.PostMessage("    Initializing Futansi UV Lamp at port " + Adrs + " ... ")
                    UvLamp = New Instrument.iFUTANSI(Instrument.iFUTANSI.ChannelEnum.Channel1)

                Case Else
                    mMsg.PostMessage("    UV Lamp type: " + s + " is not supported!")
                    Return True
            End Select

            'start initialzation
            s = ""
            success = UvLamp.Initialize(Adrs, True)

            If success Then
                'check UV lamp status
                If UvLamp.AlarmOn Then
                    s = "UV lamp alarm is on."
                    'ElseIf UvLamp.NeedCalibration Then
                    '    s = "UV lamp need calibration."
                End If
            Else
                UvLamp = Nothing
            End If

            If s <> "" Then
                s += " Do you want to continue?"
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Fail)
                r = MessageBox.Show(s, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Running)
                Return r = DialogResult.Yes
                If r = DialogResult.No Then
                    UvLamp.Close()
                    UvLamp = Nothing
                End If
            End If

            If (UvLamp Is Nothing) Then
                s = "UV lamp is not connected or available. Do you want to continue any way?"
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Fail)
                r = MessageBox.Show(s, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Running)
                Return r = DialogResult.Yes
            Else
                Return True
            End If


        End Function

        Private Function InitializeUvLampSecond() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean
            Dim r As DialogResult

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsUvLampSecond", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using the Second UV Lamp")
                Return True
            End If

            s = mIniFile.ReadParameter("Instrument", "TypeUvLampSecond", "")
            Select Case s
                Case "EXFO"
                    'ack
                    mMsg.PostMessage("    Initializing OmniCure S2000 UV Lamp at port " + Adrs + " ... ")
                    UvlampSecond = New Instrument.iOmniCureSecond

                Case "FUTANSI"
                    'ack
                    mMsg.PostMessage("    Initializing Futansi UV Lamp at port " + Adrs + " ... ")
                    UvlampSecond = New Instrument.iFUTANSISecond(Instrument.iFUTANSISecond.ChannelEnum.Channel1)

                Case Else
                    mMsg.PostMessage("    UV Lamp type: " + s + " is not supported!")
                    Return True
            End Select

            'start initialzation
            s = ""
            success = UvlampSecond.Initialize(Adrs, True)

            If success Then
                'check UV lamp status
                If UvlampSecond.AlarmOn Then
                    s = "UV lamp alarm is on."
                    'ElseIf UvlampSecond.NeedCalibration Then
                    '    s = "UV lamp need calibration."
                End If
            Else
                UvlampSecond = Nothing
            End If

            If s <> "" Then
                s += " Do you want to continue?"
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Fail)
                r = MessageBox.Show(s, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Running)
                Return r = DialogResult.Yes
                If r = DialogResult.No Then
                    UvlampSecond.Close()
                    UvlampSecond = Nothing
                End If
            End If

            If (UvlampSecond Is Nothing) Then
                s = "UV lamp is not connected or available. Do you want to continue any way?"
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Fail)
                r = MessageBox.Show(s, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Running)
                Return r = DialogResult.Yes
            Else
                Return True
            End If


        End Function

        Private Function InitializeLightSource1() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsLightSource1", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using OC9501 Driver Board")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing light source 1 controller at " + Adrs + " ... ")

            'start
            s = ""

            LightSource1 = New Instrument.iLightSource()
            success = LightSource1.Initialize(Adrs, True)
            If success Then
                LightSource1.SetChannelParameter(1, mPara.LightSource1.Channel1Value)
                LightSource1.OpenChannel(1)
                LightSource1.SetChannelParameter(2, mPara.LightSource1.Channel2Value)
                LightSource1.OpenChannel(2)
            Else
                LightSource1 = Nothing
            End If

            Return (LightSource1 IsNot Nothing)
        End Function

        Private Function InitializeLightSource2() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            Adrs = mIniFile.ReadParameter("Instrument", "AdrsLightSource2", "")
            If Not Adrs.StartsWith("COM") Then
                mMsg.PostMessage("    Configured for not using OC9501 Driver Board")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing light source 2 controller at " + Adrs + " ... ")

            'start
            s = ""

            LightSource2 = New Instrument.iLightSource()
            success = LightSource2.Initialize(Adrs, True)
            If success Then
                LightSource2.SetChannelParameter(1, mPara.LightSource2.Channel1Value)
                LightSource2.OpenChannel(1)
                LightSource2.SetChannelParameter(2, mPara.LightSource2.Channel2Value)
                LightSource2.OpenChannel(2)
            Else
                LightSource2 = Nothing
            End If

            Return (LightSource2 IsNot Nothing)
        End Function

        Private Function InitializeVision() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean
          
            Adrs = mIniFile.ReadParameter("Instrument", "VisionSystem", "")
            If Adrs = "" Or Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not using Vision System")
                Return True
            End If

            mMsg.PostMessage("    Initializing Vision System...")

            VisionDll = New Instrument.iVisionSystem

            success = VisionDll.Initialize()

            If success Then
                Return True
            Else
                VisionDll = Nothing
                s = "Failed to initialize vision DLL."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

        End Function
#End Region

#Region "stage"
        Private Function InitializeXPS() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            'address
            Adrs = mIniFile.ReadParameter("Instrument", "AdrsXPS", "-")
            If Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not useing Newport XPS motion controller")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing Newport XPS motion controller at IP address " + Adrs + " ... ")

            'initialize
            s = ""
            XPS = New Instrument.iXPS(8)
            success = XPS.Initialize(Adrs, True)

            'fail ack
            If Not success Then
                XPS = Nothing
                s = "Failed to initialize Newport XPS motion controller at IP address " + Adrs + ". Application will be closed."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return success

        End Function

        Private Function InitializePiAngle() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            'address
            Adrs = mIniFile.ReadParameter("Instrument", "AdrsPiAngle", "-")
            If Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not useing PI angle stage")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing PI angle stage at " + Adrs + " ... ")

            'initialize
            s = ""
            PiAngle = New Instrument.iPiGCS843(1)
            success = PiAngle.Initialize(Adrs, True)

            'fail ack
            If Not success Then
                PiAngle = Nothing
                s = "Failed to initialize PI angle stage " + Adrs + ". Application will be closed."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            'Home is necessary before moved
            success = PiAngle.HomeAndWait()

            Return success
        End Function

        Private Function InitializePiHexopod() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            'address
            Adrs = mIniFile.ReadParameter("Instrument", "AdrsPiHexopod", "-")
            If Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not useing PI Hexopod stage")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing PI Hexopod stage at " + Adrs + " ... ")

            'initialize
            s = ""
            Hexopod = New Instrument.iPiGCS2(6)
            success = Hexopod.Initialize(Adrs, True)

            'fail ack
            If Not success Then
                Hexopod = Nothing
                s = "Failed to initialize PI Hexopod stage " + Adrs + ". Application will be closed."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            'Home is necessary before moved
            Hexopod.Axis = 0    ' home all axis
            success = Hexopod.HomeAndWait()


            Return success
        End Function

        Private Function InitializePiLS() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            'address
            Adrs = mIniFile.ReadParameter("Instrument", "AdrsPiLS65", "-")
            If Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not useing PI Hexopod stage")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing PI Hexopod stage at " + Adrs + " ... ")

            'initialize
            s = ""
            PiLS = New Instrument.iPiLS65(1)
            success = PiLS.Initialize(Adrs, True)

            'fail ack
            If Not success Then
                PiLS = Nothing
                s = "Failed to initialize PI Linear stage " + Adrs + ". Application will be closed."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            'Home is necessary before moved
            PiLS.Axis = 1   ' home all axis
            success = PiLS.HomeAndWait()


            Return success
        End Function

        Private Function InitializeRCX() As Boolean
            Dim s, Adrs As String
            Dim success As Boolean

            'address
            Adrs = mIniFile.ReadParameter("Instrument", "AdrsRCX", "-")
            If Adrs.StartsWith("-") Then
                mMsg.PostMessage("    Configured for not useing Mitsubishi Robot")
                Return True
            End If

            'ack
            mMsg.PostMessage("    Initializing Mitsubishi Robot ... ")

            'initialize
            s = ""
            RCX = New Instrument.iRCX
            success = RCX.Initialize(True)

            'fail ack
            If Not success Then
                Hexopod = Nothing
                s = "Failed to initialize Mitsubishi Robot. Application will be closed."
                MessageBox.Show(s, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Return success
        End Function
#End Region



    End Class
End Class



