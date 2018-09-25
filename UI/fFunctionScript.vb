Option Strict On
Option Explicit On

Partial Public Class fFunction

    Public Enum ScriptRecipeNumber
        GroupHeader = 0

        MessageboxOKOnly = -1
        MessageboxYesNo = -2
        UpdateATEReportLog = -3

        DelayFixedTime = 1

        InitializeNewProcess = 10
        UnloadSample
        HomeAllStages

        RecordData = 20
        Vibration = 21

        '100 block for the utility
        Vacuum_CleanLine = 100
        Vacuum_TurnOn
        CDA_TurnOn
        GPIO_TurnOn
        DispenseGlue
        Vacuum_Switch




        CloseProbeClamp = 106
        CheckLddContact
        MoveProbeClamp

        TurnLddOn
        TurnLddOff
        SetTemperature

        ApplyEpoxy = 115

        RunUvProcess = 120
        RunSingleChannelUV = 121
        RunSingleChannelFutansi
        RunSecondUvProcess

        CcdVisionProcess = 130
        DoSimpleViewProcess
        AutoFocus
        AutoCalibration

        '200 block for the stage
        MoveStageToNamedPosition = 200
        MoveStageToPartPickupPosition           'pickup is special becase there is tray offset
        MoveStageAfterPickup
        RotateAfterViewProcess

        MoveStageTouchSense = 210

        SaveStagePositions = 220
        MoveStageToSavedPosition

        'simple stage utility
        MoveStage1Axis = 290
        MoveStage3Axis
        MoveHexapod1Axis
        MoveHexapodAngle
        MoveStageBySteps
        MoveBeam


        'Added by Ming to Do some Test
        GetCollimatedDate = 298

        '300 block for the alignment
        LensAlignCenterBeamScan = 300
        LensAlignAdjustX
        LensAlignAdjustYZ_Coarse
        LensAlignAdjustYZ_Fine
        LensAlignAdjustYZ_Overlap

        LensAlignAdjustYZ_ScanForNearFar = 310
        LensAlignAdjustYZ_ScanForOverlap
        LensAlignAngleScan

        LensAlignAdjustYZ_FailSafeFine = 320
        LensAlignAdjustYZ_FailSafeOverlap

        MeasureBeamPitchError = 330
        MeasureAngleAndPitch

        PbsAlignAdjustAngle = 350

        GetBeamProfileInfo = 390
        RecordBeamProfileInfo = 391
        Move1DAndRecordBeamProfileInfo = 392

        Move1DAndCalculateSlope = 393
        Lens3DAlignForEnergy


        'robotic arm function, same as OC8500
        CalibrateArm = 500
        LoadDut
        UnloadDut
        MoveRobot
        MoveRobotRelative
        OpenArmClamp
        CloseArmClampe
    End Enum

    Private Sub LoadScript()
        mScript.LoadScript("")
        Application.DoEvents()

        'change form caption
        Me.Text = IO.Path.GetFileNameWithoutExtension(mScript.ScriptFile)
        Me.SetScriptNameToTitle(mScript.ScriptName)

        'remove resume
        btnPause.Enabled = False
        btnPause.Text = "Paulse"
    End Sub

    Private Sub RunTestScriptAll(ByVal StartNew As Boolean)
        Dim s, sFile As String
        Dim T As Date
        Dim success As Boolean

        'auto or manual load?
        mProcess.AutoLoad = chkAutoLoad.Checked
        If mProcess.AutoLoad Then
            mDut.ClearTable()
            success = mDut.ParseDuts()
            If Not success Then Return
        Else
            'make a dummy 
            mDut.Items.Clear()
            mDut.Items.Add(New w2DutItem(1, True, txtSN.Text))
            mDut.LastAccessedItem = -1
        End If

        'loop through DUT
        For Each dut As w2DutItem In mDut.Items
            'skip 
            If Not dut.IsSelected Then Continue For
            If (Not mProcess.AutoLoad) And dut.Number <= mDut.LastAccessedItem Then Continue For

            'serial number
            If dut.SN = "" Then
                s = "Please enter unit serial number before starting."
                MessageBox.Show(s, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtSN.Focus()
                Return
            End If

            'log time
            T = Date.Now

            'update the active SN
            txtSN.Text = dut.SN

            'high light - DUT number is 1 based, Row index is zero based
            If mProcess.AutoLoad Then mDut.HighLightRow(dut.Number - 1, w2DUTEx.ItemAccessStatus.Running, "", "")

            'set running lamp
            'mTool.TriColouredLantern = ApacheIO.XpsIO.RunningLantern      'added by Alex on Jan 16th

            'do actual script for this unit
            mTool.IsAutoLoadUnload = mProcess.AutoLoad
            success = Me.RunTestScriptOne(StartNew, T)

            'update unit status
            s = Date.Now.ToString("HH:mm:ss ")
            s += Date.Now.Subtract(T).TotalSeconds.ToString("0000")

            sFile = mData.GetDataFile(dut.SN, BlackHawkData.DataFileEnum.Log, False, T)
            If success Then           
                mDut.HighLightRow(dut.Number - 1, w2DUTEx.ItemAccessStatus.Passed, s, sFile)
            ElseIf mProcess.Stop Then
                mDut.HighLightRow(dut.Number - 1, w2DUTEx.ItemAccessStatus.Stopped, s, sFile)
            Else
                mDut.HighLightRow(dut.Number - 1, w2DUTEx.ItemAccessStatus.Failed, mProcess.FailedScript.Trim(), sFile)
            End If

            'done for next 
            If Not success Then Exit For
        Next

    End Sub

    Private Function RunTestScriptOne(ByVal StartNew As Boolean, ByVal tStart As Date) As Boolean
        Dim T As Date
        Dim i, iStart As Integer
        Dim success, successAll, unknown As Boolean
        Dim SN, sFile As String
        Dim s, s1, s2 As String
        Dim v0, v1, v2, v3, v4, v5 As Double

        'clear stop flag
        mTool.ClearStop()
        mProcess.Stop = False

        'run condition
        If StartNew Or mScript.ScriptFinished Then
            'clear status
            mScript.ClearTable()
            mMsgDataHost.ClearAll()
            mMsgInfoHost.ClearAll()
            Me.Refresh()
            iStart = 0

            Me.InitializeNewProcess(-1)

            'new entry info
            mMsgData.PostMessage("    Serial Number  " + txtSN.Text)
            'mMsgData.PostMessage("    Lens Lot Num   " + txtLensLot.Text)
            'mMsgData.PostMessage("    BS Lot Num     " + txtBsLot.Text)
            'mMsgData.PostMessage("    Epoxy Batch    " + txtEpoxyLot.Text)
            mMsgData.PostMessage("    Station ID     " + mStation)
            'mMsgData.PostMessage("    Operator       " + txtOperator.Text)
            mMsgData.PostMessage("")
        Else
            'get to know where to start
            iStart = mScript.QueryResumeTestEntryIndex()
            If iStart = -1 Then Return False
        End If

        'assume success
        success = True
        successAll = True
        mProcess.FailedScript = ""

        'run script now
        For i = iStart To mScript.Items.Count - 1
            'skip unselected
            If Not mScript.IsSelectedToRun(i) Then Continue For

            'skip if it is a group header
            If mScript.Items(i).IsGroupHeader Then Continue For

            'skip test module if previous setp failed and the step is soemthing necessary
            If Not success Then
                Select Case mScript.Items(i).Number
                    Case ScriptRecipeNumber.UnloadDut
                        'continue to the loop section
                        If Not mTool.AutoUnloadRequested Then
                            ' stop here
                            mProcess.AutoLoad = False
                            Exit For
                        End If
                        'proceed to the script
                    Case Else
                        'skip the loop section, continue to the next until we found the step that is needed 
                        Continue For
                End Select
            End If

            'message
            mScript.HighLightRow(i, w2.w2Script.ItemAccessStatus.Running, "")
            s = ControlChars.CrLf + "*** Starting script: " + mScript.Items(i).Text + " @ " & Date.Now
            mMsgInfo.PostMessage(s)

            'flag/clock
            unknown = False
            T = Date.Now

            Select Case CType(mScript.Items(i).Number, ScriptRecipeNumber)
                '************************************************************************************************************message
                Case ScriptRecipeNumber.MessageboxOKOnly
                    s = mScript.Items(i).GetParameterString(0)
                    'mTool.TriColouredLantern = ApacheIO.XpsIO.FailLantern
                    MessageBox.Show(s, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'mTool.TriColouredLantern = ApacheIO.XpsIO.RunningLantern
                    success = True

                Case ScriptRecipeNumber.MessageboxYesNo
                    Dim x As DialogResult
                    s = mScript.Items(i).GetParameterString(0)
                    'mTool.TriColouredLantern = ApacheIO.XpsIO.FailLantern
                    x = MessageBox.Show(s, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    'mTool.TriColouredLantern = ApacheIO.XpsIO.RunningLantern
                    success = (x = Windows.Forms.DialogResult.Yes)

                Case ScriptRecipeNumber.UpdateATEReportLog
                    s = mScript.Items(i).GetParameterString(0)
                    If s = "" Then s = "1"
                    Dim AteReportLog As String = mIniFile.ReadParameter("Files", "AteReportLog", "C:\Data\AteReportLog.txt")
                    Dim AteLogWriter As New IO.StreamWriter(AteReportLog, False)
                    If s = "1" Then
                        AteLogWriter.Write("Test_Data.xlsx")
                    Else
                        AteLogWriter.Write("")
                    End If
                    AteLogWriter.Close()

                Case ScriptRecipeNumber.Vibration
                    v0 = mScript.Items(i).GetParameterValue(0)
                    success = mTool.StageTool.Vibration(v0)
                    '********************************************************************************************************basic functions
                Case ScriptRecipeNumber.DelayFixedTime
                    v0 = mScript.Items(i).GetParameterValue(0)
                    If Double.IsNaN(v0) Then v0 = 0.001
                    If v0 < 0.0 Then v0 = 0.001
                    success = mTool.WaitForTime(v0, "Wait for time")

                    '********************************************************************************************************vacuum
                Case ScriptRecipeNumber.Vacuum_CleanLine
                    v0 = mScript.Items(i).GetParameterValue(0)
                    success = mTool.Utility.CleanVacuumLineWithCda(True, v0)

                Case ScriptRecipeNumber.Vacuum_TurnOn
                    s = mScript.Items(i).GetParameterString(0)          'vacuum line: 0 = main arm, 1 = hexapod, 2 = package, 3 = input line, may not have switch
                    s1 = mScript.Items(i).GetParameterString(1)         'on/off
                    success = mTool.WaitForTime(0.5, "Wait for time")
                    success = Me.TurnVacuumOn(s, s1)                    'handle this locally because it is a bit involved related to part consumption
                    success = mTool.WaitForTime(0.5, "Wait for time")
                Case ScriptRecipeNumber.CDA_TurnOn
                    Dim TurnOn As Boolean
                    s = mScript.Items(i).GetParameterString(0)          'cda line
                    s1 = mScript.Items(i).GetParameterString(1)         'on/off
                    TurnOn = (s1 = "1")
                    success = mTool.Utility.TurnCdaOn(CInt(s), TurnOn)

                Case ScriptRecipeNumber.GPIO_TurnOn
                    Dim TurnOn As Boolean
                    s = mScript.Items(i).GetParameterString(0)          'cda line
                    s1 = mScript.Items(i).GetParameterString(1)         'on/off
                    TurnOn = (s1 = "1")
                    success = mTool.Utility.TurnGpioOn(CInt(s), TurnOn)
                    'Add by Ming
                Case ScriptRecipeNumber.DispenseGlue
                    success = mTool.Utility.DispenseGlue()

                Case ScriptRecipeNumber.Vacuum_Switch
                    Dim TurnOn As Boolean
                    s = mScript.Items(i).GetParameterString(0)          'cda line
                    s1 = mScript.Items(i).GetParameterString(1)         'on/off
                    TurnOn = (s1 = "1")
                    success = mTool.Utility.VaccumSwitch(CInt(s), TurnOn)




                    '********************************************************************************************************Probe Clamp / LDD check
                Case ScriptRecipeNumber.CloseProbeClamp
                    s = mScript.Items(i).GetParameterString(0)          '1 = close
                    success = mTool.Utility.CloseProbeClamp(s = "1")

                Case ScriptRecipeNumber.MoveProbeClamp
                    s = mScript.Items(i).GetParameterString(0)          '1 = Relative
                    v1 = mScript.Items(i).GetParameterValue(1)          'move target/amount
                    success = mTool.Utility.MoveProbeClamp(s = "1", v1)

                Case ScriptRecipeNumber.CheckLddContact
                    success = mTool.Utility.CheckLddContact()
                    '********************************************************************************************************Current / Temperature Apply

                Case ScriptRecipeNumber.TurnLddOn
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'current

                    success = mTool.Utility.TurnLddOn(CInt(v0), v1)


                Case ScriptRecipeNumber.TurnLddOff
                    success = mTool.Utility.TurnLddOff

                Case ScriptRecipeNumber.SetTemperature
                    v0 = mScript.Items(i).GetParameterValue(0)          'temperature
                    v1 = mScript.Items(i).GetParameterValue(1)          'time

                    success = mTool.Utility.SetTemperature(v0)
                    success = mTool.WaitForTime(v1, "Wait for glue to dry...")
                    success = mTool.Utility.SetTemperature(45)
                    '********************************************************************************************************Epoxy
                Case ScriptRecipeNumber.ApplyEpoxy
                    s = mScript.Items(i).GetParameterString(0)          'part enum, 0-6 for PBS1, PBS2, PBS3, lens1, lens2, lens3, lens4
                    s1 = mScript.Items(i).GetParameterString(1)         '1 for visual check
                    v1 = mScript.Items(i).GetParameterValue(2)          'wait time
                    v2 = mScript.Items(i).GetParameterValue(3)          'steps
                    v3 = mScript.Items(i).GetParameterValue(4)          'amounts
                    success = mTool.Utility.ApplyEpoxy(s, s1 = "1", v1, v2, v3)

                Case ScriptRecipeNumber.RunUvProcess
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    mTool.Instruments.UvLamp.ExposureTime = v0
                    System.Threading.Thread.Sleep(100)
                    mTool.Instruments.UvLamp.PowerLevel = v1
                    success = mTool.Utility.RunUvCure()

                    'added by Ming
                Case ScriptRecipeNumber.RunSingleChannelUV
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    v2 = mScript.Items(i).GetParameterValue(2)
                    mTool.Instruments.UvLamp.ActiveSingleChannel = CInt(v0)
                    System.Threading.Thread.Sleep(100)
                    mTool.Instruments.UvLamp.ExposureTime = v1
                    System.Threading.Thread.Sleep(100)
                    mTool.Instruments.UvLamp.PowerLevel = v2
                    success = mTool.Utility.RunUvCure()

                Case ScriptRecipeNumber.RunSingleChannelFutansi
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    v2 = mScript.Items(i).GetParameterValue(2)
                    v3 = mScript.Items(i).GetParameterValue(3)
                    mTool.Instruments.UvLamp.ExposureTime = v2
                    mTool.Instruments.UvLamp.PowerLevel = v3
                    mTool.Instruments.UvLamp.ActiveSingleChannel = CType(CInt(v0), Instrument.iFUTANSI.ChannelEnum)
                    If v1 <> 0 Then
                        mTool.Instruments.UvLamp.ShutterOpen = True
                    Else
                        mTool.Instruments.UvLamp.ShutterOpen = False
                    End If
                    success = True


                Case ScriptRecipeNumber.RunSecondUvProcess
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    v2 = mScript.Items(i).GetParameterValue(2)
                    v3 = mScript.Items(i).GetParameterValue(3)
                    mTool.Instruments.UvlampSecond.ExposureTime = v2
                    mTool.Instruments.UvlampSecond.PowerLevel = v3
                    mTool.Instruments.UvlampSecond.ActiveSingleChannel = CType(CInt(v0), Instrument.iFUTANSISecond.ChannelEnum)
                    If v1 <> 0 Then
                        mTool.Instruments.UvlampSecond.ShutterOpen = True
                    Else
                        mTool.Instruments.UvlampSecond.ShutterOpen = False
                    End If
                    success = mTool.WaitForTime(v2, "Wait for Uv Cure Complete...")

                    '********************************************************************************************************Vision
                Case ScriptRecipeNumber.CcdVisionProcess
                    s = mScript.Items(i).GetParameterString(0)          'PartPositionEnum
                    s1 = mScript.Items(i).GetParameterString(1)         'part enum, this is needed for the part in the tray, 0 = lens, 1 = PBS,2=BS2,3=BS1
                    If s1 <> "" Then
                        success = Me.GtePartIndexForPickup(s1)
                        If s = "35" Or s = "30" Or s = "31" Or s = "32" Or s = "33" Then
                            success = True
                        End If
                        'get index for the next part in the tray, it is not consumed after vacuum pickup
                    Else
                        success = True                                  'only part pick up needs CCD
                    End If

                    If success Then
                        success = mTool.Utility.ProcessCcdView(s, CType(s1, fPartTray.PartEnum), mProcess.PartIndex)
                    End If

                Case ScriptRecipeNumber.DoSimpleViewProcess
                    v0 = mScript.Items(i).GetParameterValue(0)
                    success = mTool.Utility.ProcessImage(CInt(v0))
                    success = mTool.WaitForTime(1, "Wait for Process Complete...")

                Case ScriptRecipeNumber.AutoFocus
                    s = mScript.Items(i).GetParameterString(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    v2 = mScript.Items(i).GetParameterValue(2)
                    success = mTool.Utility.AutoFocus(s, v1, v2)

                Case ScriptRecipeNumber.AutoCalibration
                    s = mScript.Items(i).GetParameterString(0)
                    success = mTool.Utility.AutoCalibration(s)

                    '********************************************************************************************************Move stage
                Case ScriptRecipeNumber.MoveStage1Axis
                    s = mScript.Items(i).GetParameterString(0)          'axis, 0-2 = main satge XYZ, 3-5 = beam scan XYZ, 6 = main arm angle, 7 = hexapod angle
                    s1 = mScript.Items(i).GetParameterString(1)         '0 = absolute move, 1 = relative move
                    v2 = mScript.Items(i).GetParameterValue(2)          'move target/amount
                    success = mTool.StageTool.MoveStage1Axis(s, s1, v2)

                Case ScriptRecipeNumber.MoveStage3Axis
                    v0 = mScript.Items(i).GetParameterValue(0)          'stage X
                    v1 = mScript.Items(i).GetParameterValue(1)          'stage Y
                    v2 = mScript.Items(i).GetParameterValue(2)          'stage Z
                    success = mTool.StageTool.MoveStage3Axis(v0, v1, v2, True)

                Case ScriptRecipeNumber.MoveStageToNamedPosition
                    s = mScript.Items(i).GetParameterString(0)          'PartPositionEnum,  0-6 for BS1, PBS, PBS2, lens1, lens2, lens3, lens4
                    s1 = mScript.Items(i).GetParameterString(1)         '1 for visual check
                    success = mTool.StageTool.MoveStageToNamedPosition(s, s1 = "1")

                Case ScriptRecipeNumber.MoveStageToPartPickupPosition
                    'note that script parameter order and function parameter order are different
                    s = mScript.Items(i).GetParameterString(0)          'part type, Lens = 0, PSB = 1, BS2 = 2, BS1 = 3
                    s1 = mScript.Items(i).GetParameterString(1)         'visual check (1) or not (0)
                    s2 = mScript.Items(i).GetParameterString(2)         'arm to use, main (0) or hexapod (1)
                    success = Me.GtePartIndexForPickup(s)               'get index for the next part in the tray, it is not consumed after vacuum pickup
                    If success Then
                        success = mTool.StageTool.MoveForPartPickup(CType(s2, iXpsStage.StageEnum), CType(s, fPartTray.PartEnum), mProcess.PartIndex, s1 = "1")
                    End If

                Case ScriptRecipeNumber.RotateAfterViewProcess
                    success = mTool.StageTool.RotateAfterViewProcess()

                Case ScriptRecipeNumber.MoveStageTouchSense
                    s = mScript.Items(i).GetParameterString(0)          'arm to use, main (0) or hexapod (1)
                    s1 = mScript.Items(i).GetParameterString(1)         'part: 0 = lens, 1 = PBS, 2 = part pickup, 3 = epoxy, 4 for everything else
                    success = mTool.StageTool.zTouchWithForceGauge(s, s1)

                Case ScriptRecipeNumber.SaveStagePositions
                    success = mTool.StageTool.SaveCurrentStagePosition()

                Case ScriptRecipeNumber.MoveStageToSavedPosition
                    s = mScript.Items(i).GetParameterString(0)          '1 for visual check
                    success = mTool.StageTool.MoveStageBackToSavedPosition(s = "1")

                Case ScriptRecipeNumber.MoveHexapod1Axis
                    s = mScript.Items(i).GetParameterString(0)          'axis, 1-6 = XYZUVW
                    s1 = mScript.Items(i).GetParameterString(1)         '0 = absolute move, 1 = relative move
                    v2 = mScript.Items(i).GetParameterValue(2)          'move target/amount
                    success = mTool.StageTool.MoveHexapod1Axis(CInt(s), CType(s1, Instrument.iMotionController.MoveToTargetMethodEnum), v2)


                Case ScriptRecipeNumber.MoveStageBySteps
                    s = mScript.Items(i).GetParameterString(0)          'axis, 0-2 = main satge XYZ, 3-5 = beam scan XYZ, 6 = main arm angle, 7 = hexapod angle
                    v1 = mScript.Items(i).GetParameterValue(1)          'steps
                    v2 = mScript.Items(i).GetParameterValue(2)          'amounts
                    success = mTool.StageTool.MoveStageBySteps(s, v1, v2)

                Case ScriptRecipeNumber.MoveBeam
                    v1 = mScript.Items(i).GetParameterValue(0)

                    success = mTool.StageTool.MoveBeamGage(v1, False)


                    'Add by Ming to do some test and begin with 298

                Case ScriptRecipeNumber.GetCollimatedDate
                    s = mScript.Items(i).GetParameterString(0)
                    success = mTool.StageTool.GetBeamCollimationData(s = "1")


                    '----------------------------------------------------------------------------------------------------Lens alignment
                Case ScriptRecipeNumber.LensAlignCenterBeamScan
                    success = mTool.StageTool.CenterBeamScan(Double.NaN)
                    'Simplified By Ming
                Case ScriptRecipeNumber.LensAlignAdjustX
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    'If s = "" Then s = "1"
                    's1 = mScript.Items(i).GetParameterString(1)         'beam scan location, 0 for near, 1 for far
                    'If s1 = "" Then s1 = "0"
                    'success = mTool.StageTool.AlignLensForIntensity(CType(s, Integer), s1 = "0")
                    success = mTool.StageTool.AlignLensForSmallestBeam(CType(s, Integer))

                Case ScriptRecipeNumber.Lens3DAlignForEnergy
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    'If s = "" Then s = "1"
                    's1 = mScript.Items(i).GetParameterString(1)         'beam scan location, 0 for near, 1 for far
                    'If s1 = "" Then s1 = "0"
                    'success = mTool.StageTool.AlignLensForIntensity(CType(s, Integer), s1 = "0")
                    success = mTool.StageTool.AlignLensForTotalEnergy(CType(s, Integer))




                Case ScriptRecipeNumber.LensAlignAdjustYZ_Coarse
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    If s = "" Then s = "1"
                    success = mTool.StageTool.AlignLensForBeamSteering_Coarse(CType(s, Integer))

                Case ScriptRecipeNumber.LensAlignAdjustYZ_Fine
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    If s = "" Then s = "1"
                    success = mTool.StageTool.AlignLensForSteering_Fine(CType(s, Integer))

                Case ScriptRecipeNumber.LensAlignAdjustYZ_Overlap
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    If s = "" Then s = "1"
                    success = mTool.StageTool.AlignLensForSteering_Overlap(CType(s, Integer))

                Case ScriptRecipeNumber.MeasureBeamPitchError
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned
                    If s = "" Then s = "1"
                    success = mTool.StageTool.MeasureOverlapPitch(CType(s, Integer))
                Case ScriptRecipeNumber.MeasureAngleAndPitch
                    success = mTool.StageTool.MeasureAngleAndPitch()
                    '********************************************************************************************************Read Beam Info
                Case ScriptRecipeNumber.GetBeamProfileInfo
                    success = mTool.Utility.GetBeamProfileInfo(10, Instrument.iBeamProfiler.DataAcquisitionMode.AllSummaryData)
                    'Add by Ming
                Case ScriptRecipeNumber.RecordBeamProfileInfo
                    v0 = mScript.Items(i).GetParameterValue(0)          'sample
                    s = mScript.Items(i).GetParameterString(1)          'mode
                    v1 = mScript.Items(i).GetParameterValue(2)
                    success = mTool.Utility.RecordProfileInfo(CInt(v0), CType(s, Instrument.iBeamProfiler.DataAcquisitionMode), CInt(v1))
                Case ScriptRecipeNumber.Move1DAndRecordBeamProfileInfo
                    v0 = mScript.Items(i).GetParameterValue(0)          'sample
                    s = mScript.Items(i).GetParameterString(1)          'mode
                    s1 = mScript.Items(i).GetParameterString(2)
                    v1 = mScript.Items(i).GetParameterValue(3)
                    v2 = mScript.Items(i).GetParameterValue(4)
                    success = mTool.Utility.Move1DAndCalculateSlope(CInt(v0), CType(s, Instrument.iBeamProfiler.DataAcquisitionMode), s1, v1, v2, txtSN.Text)
                    'success = mTool.Utility.Move1DAndRecordBeamProfileInfo(CInt(v0), CType(s, Instrument.iBeamProfiler.DataAcquisitionMode), s1, v1, v2)
                Case ScriptRecipeNumber.Move1DAndCalculateSlope
                    v0 = mScript.Items(i).GetParameterValue(0)          'sample
                    s = mScript.Items(i).GetParameterString(1)          'mode
                    s1 = mScript.Items(i).GetParameterString(2)
                    v1 = mScript.Items(i).GetParameterValue(3)
                    v2 = mScript.Items(i).GetParameterValue(4)
                    success = mTool.Utility.Move1DAndCalculateSlope(CInt(v0), CType(s, Instrument.iBeamProfiler.DataAcquisitionMode), s1, v1, v2, txtSN.Text)



                    '----------------------------------------------------------------------------------------------------Lens alignment extra
                Case ScriptRecipeNumber.LensAlignAdjustYZ_ScanForNearFar
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'Y range
                    v2 = mScript.Items(i).GetParameterValue(2)          'Y step size
                    v3 = mScript.Items(i).GetParameterValue(3)          'Z range
                    v4 = mScript.Items(i).GetParameterValue(4)          'Z step size
                    success = mTool.StageTool.AlignLensByScan_NearFar(Convert.ToInt32(v0), v1, v2, v3, v4)

                Case ScriptRecipeNumber.LensAlignAdjustYZ_FailSafeFine                  '-- this is LensAlignAdjustYZ_Fine followed by LensAlignAdjustYZ_ScanForNearFar if LensAlignAdjustYZ_Fine fails 
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'Y range
                    v2 = mScript.Items(i).GetParameterValue(2)          'Y step size
                    v3 = mScript.Items(i).GetParameterValue(3)          'Z range
                    v4 = mScript.Items(i).GetParameterValue(4)          'Z step size
                    success = mTool.StageTool.AlignLensForSteering_FinePlusScan(Convert.ToInt32(v0), v1, v2, v3, v4)

                Case ScriptRecipeNumber.LensAlignAdjustYZ_ScanForOverlap
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'Y range
                    v2 = mScript.Items(i).GetParameterValue(2)          'Y step size
                    v3 = mScript.Items(i).GetParameterValue(3)          'Z range
                    v4 = mScript.Items(i).GetParameterValue(4)          'Z step size
                    success = mTool.StageTool.AlignLensByScan_Overlap(Convert.ToInt32(v0), v1, v2, v3, v4)

                Case ScriptRecipeNumber.LensAlignAdjustYZ_FailSafeOverlap           '-- this is LensAlignAdjustYZ_Overlap followed by LensAlignAdjustYZ_ScanForOverlap if LensAlignAdjustYZ_Overlap fails 
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'Y range
                    v2 = mScript.Items(i).GetParameterValue(2)          'Y step size
                    v3 = mScript.Items(i).GetParameterValue(3)          'Z range
                    v4 = mScript.Items(i).GetParameterValue(4)          'Z step size
                    success = mTool.StageTool.AlignLensForSteering_OverlapPlusScan(Convert.ToInt32(v0), v1, v2, v3, v4)

                Case ScriptRecipeNumber.LensAlignAngleScan
                    v0 = mScript.Items(i).GetParameterValue(0)          'channel
                    v1 = mScript.Items(i).GetParameterValue(1)          'Angle range
                    v2 = mScript.Items(i).GetParameterValue(2)          'Angle step size
                    success = mTool.StageTool.AlignLensOverlapByAngle(Convert.ToInt32(v0), v1, v2)

                    '----------------------------------------------------------------------------------------------------Align PBS
                Case ScriptRecipeNumber.PbsAlignAdjustAngle
                    s = mScript.Items(i).GetParameterString(0)          'channel to be aligned, value 1-3 for PBS1-3
                    If s = "" Then s = "1"
                    success = mTool.StageTool.AlignPbsByAngle(CType(s, Integer))

                    '----------------------------------------------------------------------------------------------------record data, post epoxy align
                Case ScriptRecipeNumber.RecordData
                    s = mScript.Items(i).GetParameterString(0)          'step: 0 initial, 1 post epoxy, 2 post UV
                    s1 = mScript.Items(i).GetParameterString(1)         'Part Enum,  1-7 for PBS1, PBS2, PBS3, lens1, lens2, lens3, lens4
                    success = mTool.StageTool.RecordData(s, s1)

                Case ScriptRecipeNumber.CalibrateArm
                    success = mTool.Instruments.RobotArm.PrepareArm()

                Case ScriptRecipeNumber.LoadDut


                Case ScriptRecipeNumber.UnloadDut


                Case ScriptRecipeNumber.MoveRobot
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    v2 = mScript.Items(i).GetParameterValue(2)
                    v3 = mScript.Items(i).GetParameterValue(3)
                    v4 = mScript.Items(i).GetParameterValue(4)
                    v5 = mScript.Items(i).GetParameterValue(5)
                    mTool.Instruments.RobotArm.MoveArm(v0, v1, v2, v3, v4, v5)

                Case ScriptRecipeNumber.MoveRobotRelative
                    v0 = mScript.Items(i).GetParameterValue(0)
                    v1 = mScript.Items(i).GetParameterValue(1)
                    mTool.Instruments.RobotArm.MoveArmRelative(CType(v0, Instrument.iRCX.EAxis), v1)

                Case ScriptRecipeNumber.OpenArmClamp
                    mTool.Utility.CloseProbeClamp(False)

                Case ScriptRecipeNumber.CloseArmClampe
                    mTool.Utility.CloseProbeClamp(True)

                Case Else
                    unknown = True
                    success = True
            End Select

            '------------------------------------------------------------------------------------------------------------------------------------------
            If unknown Then
                s = "      Unknow test script " & mScript.Items(i).Number & ". Test skipped."
                mMsgInfo.PostMessage(s)
                mScript.HighLightRow(i, w2.w2Script.ItemAccessStatus.UnknownSkipped, "Unknown Test")
            Else
                s = Date.Now.ToString("dd/MM HH:mm:ss ")
                s += Date.Now.Subtract(T).TotalSeconds.ToString("0000")

                If success Then
                    mScript.HighLightRow(i, w2.w2Script.ItemAccessStatus.Passed, s)
                ElseIf mProcess.Stop Then
                    mScript.HighLightRow(i, w2.w2Script.ItemAccessStatus.Stopped, s)
                Else
                    mProcess.FailedScript += (i + 1).ToString() + " " + mScript.Items(i).Text + ControlChars.CrLf
                    mScript.HighLightRow(i, w2.w2Script.ItemAccessStatus.Failed, s)
                End If
            End If

            s = "*** End script: " + mScript.Items(i).Text + " @ " & Date.Now
            mMsgInfo.PostMessage(s)

            'status
            successAll = successAll And success

            'exit if stop is requested
            If mProcess.Stop Then Exit For

        Next i

        'save result to a CSV file
        ' mTool.SaveResultsToCsv()

        'save the log
        SN = txtSN.Text
        sFile = mData.GetDataFile(SN, BlackHawkData.DataFileEnum.Log, True, tStart)
        w2.DataGridViewHelper.Helper.SaveTextToFile(dgvScript, sFile, ControlChars.Tab)
        mMsgInfoHost.SaveToFile(sFile, True)
        mData.BackupDataFile(sFile)

        'save error log to a single log file
        mData.ExtractErrorToSingleFile(SN, tStart)

        'save result - if there is anything
        s = txtSummary.Text.Trim()
        If s <> "" Then
            sFile = mData.GetDataFile(SN, BlackHawkData.DataFileEnum.Result, True, tStart)
            mMsgDataHost.SaveToFile(sFile, True)
            mData.BackupDataFile(sFile)
        End If

        Return successAll
    End Function

    Private Function InitializeNewProcess(ByVal iStep As Integer) As Boolean


        mTool.Parameter.ClearAlignmentParameters()

    End Function

    Private Sub ShowScriptError(ByVal sError As String, ByVal recipe As w2.w2RecipeItem)
        Dim s As String

        s = "X   Error parsing script parameters: " + recipe.Parameters
        s += ControlChars.CrLf + "X                           message: " + sError
        mMsgInfo.PostMessage(s)

    End Sub

#Region "Part Tray Special"
    Private Function GtePartIndexForPickup(ByVal sPart As String) As Boolean
        Dim success As Boolean
        Dim tray As fPartTray
        Dim part As fPartTray.PartEnum

        'get tray tool
        If mUiPanels(UIPanelEnum.PartTray) Is Nothing Then
            mMsgInfo.PostMessage("X   Missing part tray for part pick up!")
            Return False
        End If

        'parse part
        Try
            part = CType(sPart, fPartTray.PartEnum)
        Catch ex As Exception
            mMsgInfo.PostMessage("X   Wrong part identification: " + sPart + "Error: " + ex.Message)
            Return False
        End Try

        'get index from the tray
        tray = CType(mUiPanels(UIPanelEnum.PartTray), fPartTray)
        mProcess.PartIndex = tray.GetFirstFilledPartIndex(part)
        If mProcess.PartIndex = -1 Then
            mMsgInfo.PostMessage("X   Part tray is empty, refill!")
            Return False
        Else
            success = True
        End If

        'done
        Return success
    End Function

    Private Function TurnVacuumOn(ByVal sLine As String, ByVal sOnOff As String) As Boolean
        Dim eLine As iXpsIO.VacuumLine
        Dim TurnOn, success As Boolean
        Dim tray As fPartTray


        'parse parameters
        TurnOn = (sOnOff = "1")
        Try
            eLine = CType(sLine, iXpsIO.VacuumLine)
        Catch ex As Exception
            mMsgInfo.PostMessage("X   Wrong vacuum line identification: " + sLine + "Error: " + ex.Message)
            Return False
        End Try

        'do work
        success = mTool.Utility.TurnLensVacuumOn(eLine, TurnOn)
        If Not success Then Return False

        'consume part if vacuum if on for part picking
        If TurnOn Then
            Select Case eLine
                Case iXpsIO.VacuumLine.Hexapod, iXpsIO.VacuumLine.Main
                    tray = CType(mUiPanels(UIPanelEnum.PartTray), fPartTray)
                    tray.EmptyOnePart(mProcess.PartIndex)

                Case Else
                    'do nothing if vacuum is on for somethin else
            End Select
        End If

        Return True
    End Function



#End Region
End Class
