Option Explicit On
Option Strict On
Option Infer Off

Imports BlackHawk.Instrument

Partial Public Class BlackHawkFunction
    Private mUtility As InstrumentUtility

    Public ReadOnly Property Utility() As InstrumentUtility
        Get
            Return mUtility
        End Get
    End Property

    Public Class InstrumentUtility
        Private mTool As BlackHawkFunction
        Private mPara As BlackHawkParameters
        Private mInst As InstrumentList
        Private mMsgData As w2.w2MessageService
        Private mMsgInfo As w2.w2MessageService
        'Add By Ming
        Private mData As BlackHawkData

        Private mXPS As Instrument.iXPS
        Private mStage As StageFunctions
        Private mIO As iXpsIO

        Public Sub New(ByVal hTool As BlackHawkFunction)
            mTool = hTool
            mInst = hTool.mInst
            mPara = hTool.mPara
            mMsgData = hTool.mMsgData
            mMsgInfo = hTool.mMsgInfo

            '   Add By Ming
            mData = hTool.mData

            mIO = mInst.XpsIO
            mStage = hTool.mStage
        End Sub

#Region "vacuum"
        'Public Function TurnLensVacuumOn(ByVal sLine As String, ByVal TurnOn As Boolean) As Boolean
        '    Dim eLine As iXpsIO.VacuumLine

        '    Try
        '        eLine = CType(sLine, iXpsIO.VacuumLine)
        '    Catch ex As Exception
        '        mMsgInfo.PostMessage("X   Wrong vacuum line identification: " + sLine + "Error: " + ex.Message)
        '        Return False
        '    End Try

        '    Return Me.TurnLensVacuumOn(eLine, TurnOn)
        'End Function

        Public Function TurnLensVacuumOn(ByVal eLine As iXpsIO.VacuumLine, ByVal TurnOn As Boolean) As Boolean
            Dim s, sx As String
            Dim v0, v1 As Double
            Dim k As Integer
            Dim status, success As Boolean

            Dim response As DialogResult

            Const VacuumRetryCount As Integer = 8

            'info
            Select Case eLine
                Case iXpsIO.VacuumLine.Hexapod
                    s = "    Turn Hexapod arm "
                Case iXpsIO.VacuumLine.Main
                    s = "    Turn lens arm"
                Case iXpsIO.VacuumLine.Package
                    s = "    Tuen package"
                Case Else
                    Return False
            End Select

            s += " vacuum " + IIf(TurnOn, "on", "off").ToString() + " ..."
            mMsgInfo.PostMessage(s)

            'stop vent
            mIO.VacuumLinePressurized = False

            'get base vacuum level
            v0 = mInst.XpsIO.VacuumLevel(eLine)
            If Not Double.IsNaN(v0) Then
                s = "      Before switch = " + v0.ToString("0.0")
                mMsgInfo.PostMessage(s)
            End If

            'set
            k = 0
Retry:
            mIO.VacuumEnabled(eLine) = TurnOn
            mTool.WaitForTime(0.5, "Wait for vacuum")
            v1 = mInst.XpsIO.VacuumLevel(eLine)
            status = mInst.XpsIO.VacuumEnabledReadback(eLine)

            'show vacuum level
            If Not Double.IsNaN(v1) Then
                s = "       After switch = " + v1.ToString("0.0")
                mMsgInfo.PostMessage(s)
            End If

            'digit feedback
            If status <> TurnOn Then
                If k < VacuumRetryCount Then
                    System.Threading.Thread.Sleep(100)
                    k += 1
                    GoTo Retry
                Else
                    If TurnOn Then
                        s = "There seems to be insufficient vacuum. Please make sure main vacuum is switched on and that part is at the right location."
                    Else
                        s = "The vacuum is still htolding. Please check vacuum switch vent."
                    End If
                End If

                response = mTool.ShowMessageBox(s, "Vacuum", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information)

                If response = DialogResult.Retry Then
                    k = 0
                    GoTo Retry
                End If
            End If

            'analogy check
            If Not (Double.IsNaN(v0) Or Double.IsNaN(v1)) Then
                s = ""
                s += "             change = " + (v1 - v0).ToString("0.0") + ControlChars.CrLf
                s += "         min change = " + mPara.VacuumCda.MinVacuumChange.ToString("0.0")
                mMsgInfo.PostMessage(s)
                If Math.Abs(v1 - v0) < mPara.VacuumCda.MinVacuumChange Then
                    s = "There seems to be insufficient vacuum pressure change. Can you verify that vacuum is indeed switched?"
                    response = mTool.ShowMessageBox(s, "Vacuum", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                    Select Case response
                        Case DialogResult.Yes
                            mMsgInfo.PostMessage("!   Change smaller than expected min. Operator varified!")
                            status = TurnOn
                        Case DialogResult.No
                            GoTo Retry
                        Case DialogResult.Cancel
                            mMsgInfo.PostMessage("X   Change smaller than expected min. Operation cancelled!")
                            success = False
                    End Select
                End If
            End If

            'final check
            If status = TurnOn Then
                mMsgInfo.PostMessage("    Part vacuum ready.")
                success = True
            Else
                If TurnOn Then
                    sx = "Please confirm that vacuum is indeed on"
                Else
                    sx = "Please confirm that vacuum is indeed off"
                End If

                response = mTool.ShowMessageBox(sx, "Vacuum", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                If response = DialogResult.Yes Then
                    mMsgInfo.PostMessage("!   Vacuum state is manually confirmed!")
                    success = True
                Else
                    mMsgInfo.PostMessage("X   " + s)
                    success = False
                End If
            End If

            If (Not success) And mTool.IsAutoLoadUnload Then
                response = mTool.ShowMessageBox("Auto Unload DUT?", "Unload", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                mTool.AutoUnloadRequested = (response = DialogResult.Yes)
            End If

            Return success
        End Function

        Public Function CleanVacuumLineWithCda(ByVal ShowMessage As Boolean, ByVal Duration As Double) As Boolean
            Dim s As String
            Dim r As Windows.Forms.DialogResult

            mMsgInfo.PostMessage("    Clean vacuum line with compressed air")

            'warning
            If ShowMessage Then
                s = "Please make sure there is no part on and under the vacuum tips!"
                r = MessageBox.Show(s, "Clean Vacuum Line", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                If r = DialogResult.Cancel Then
                    mMsgInfo.PostMessage("!   Operation cancelled")
                    Return False
                End If
            End If

            'switch to CDA
            mMsgInfo.PostMessage("      Switch to compressed dry air")
            mIO.VacuumLinePressurized = True

            'wait
            mMsgInfo.PostMessage("      Wait for " + Duration.ToString("0.0") + " seconds")
            mTool.WaitForTime(Duration, "Wait for the vacuum line to be claned")

            'switch to vacuum
            mMsgInfo.PostMessage("      Switch back to vacuum")
            mIO.VacuumLinePressurized = False

            'done
            Return (mIO.VacuumLinePressurized = False)

        End Function

        Public Function TurnCdaOn(ByVal iLine As Integer, ByVal TurnOn As Boolean) As Boolean
            Dim s As String
            Dim status, success As Boolean

            s = "CDA 4." + iLine.ToString() + IIf(TurnOn, " on", " off").ToString() + " ..."
            mMsgInfo.PostMessage(s)

            mInst.XPS.DigitOutput(4, iLine) = TurnOn

            'get base vacuum level
            status = mInst.XPS.DigitOutput(4, iLine)

            success = status = TurnOn
            Return success
        End Function

        Public Function TurnGpioOn(ByVal iLine As Integer, ByVal TurnOn As Boolean) As Boolean
            Dim s As String
            Dim status, success As Boolean

            s = "GPIO 3." + iLine.ToString() + IIf(TurnOn, " on", " off").ToString() + " ..."
            mMsgInfo.PostMessage(s)

            mInst.XPS.DigitOutput(3, iLine) = TurnOn

            'get base vacuum level
            status = mInst.XPS.DigitOutput(3, iLine)

            success = status = TurnOn
            Return success
        End Function

        Public Function VaccumSwitch(ByVal iLine As Integer, ByVal TurnOn As Boolean) As Boolean
            Dim status, success As Boolean
            mInst.XPS.DigitOutput(4, 8) = False
            mInst.XPS.DigitOutput(4, 4 + iLine) = TurnOn

            'get base vacuum level
            status = mInst.XPS.DigitOutput(4, 4 + iLine)

            success = status = TurnOn
            Return success
        End Function






        Public Function DispenseGlue() As Boolean
            mInst.XPS.DigitOutput(3, 0) = False
            System.Threading.Thread.Sleep(100)
            mInst.XPS.DigitOutput(3, 0) = True

            Return True

        End Function
#End Region

#Region "probe clamp"
        Public Function IsProbeClampOpen() As Boolean
            Return (mInst.ProbeClamp.Position < mPara.ProbeClamp.OpenPosition)
        End Function

        Public Function CloseProbeClamp(ByVal Close As Boolean) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim elapse As Double
            Dim tStart As Date

            s = IIf(Close, "Close", "Open").ToString()
            s = "    " + s + " clamp for the probe pin ..."
            mMsgInfo.PostMessage(s)

            mInst.ProbeClamp.Enabled = True
            If Close Then
                success = mInst.ProbeClamp.CloseGrip(mPara.ProbeClamp.Speed, mPara.ProbeClamp.Force)
            Else
                success = mInst.ProbeClamp.OpenGrip(mPara.ProbeClamp.Speed, mPara.ProbeClamp.Force)
            End If

            If Not success Then
                s = "X   Failed to open/close probe clamp. Error Code =  0x" + Convert.ToString(mInst.ProbeClamp.ErrorCode, 16)
                mMsgInfo.PostMessage(s)
                Return False
            End If

            'wait
            tStart = Date.Now
            s = "Wait for clamp to open/close, elapse {1:0.00} second"

            While mInst.ProbeClamp.ClampBusy
                If mTool.CheckStop() Then
                    mInst.ProbeClamp.StopMotion()
                    Return False
                End If

                'relax and show data
                System.Threading.Thread.Sleep(100)
                elapse = Date.Now.Subtract(tStart).TotalSeconds
                mMsgData.PostMessage(String.Format(s, elapse), w2.w2MessageService.MessageDisplayStyle.NewStatus)
            End While

            Return True
        End Function

        Public Function MoveProbeClamp(ByVal IsRelativeMove As Boolean, ByVal Target As Double) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim elapse As Double
            Dim tStart As Date

            s = IIf(IsRelativeMove, "Move Clamp by ", "Move Clamp to ").ToString()
            s = "    " + s + Target.ToString("0.00") + "  ..."
            mMsgInfo.PostMessage(s)

            mInst.ProbeClamp.Enabled = True
            If IsRelativeMove Then
                If Target > 0 Then
                    success = mInst.ProbeClamp.MoveRelative(Target, mPara.ProbeClamp.Speed)
                Else
                    Dim pos As Double = mInst.ProbeClamp.Position
                    success = mInst.ProbeClamp.Move(pos + Target, mPara.ProbeClamp.Speed)
                End If

            Else
                success = mInst.ProbeClamp.Move(Target, mPara.ProbeClamp.Speed)
            End If

            If Not success Then
                s = "X   Failed to move probe clamp. Error Code =  0x" + Convert.ToString(mInst.ProbeClamp.ErrorCode, 16)
                mMsgInfo.PostMessage(s)
                Return False
            End If

            'wait
            tStart = Date.Now
            s = "Wait for clamp to move, elapse {1:0.00} second"

            While mInst.ProbeClamp.ClampBusy
                If mTool.CheckStop() Then
                    mInst.ProbeClamp.StopMotion()
                    Return False
                End If

                'relax and show data
                System.Threading.Thread.Sleep(100)
                elapse = Date.Now.Subtract(tStart).TotalSeconds
                mMsgData.PostMessage(String.Format(s, elapse), w2.w2MessageService.MessageDisplayStyle.NewStatus)
            End While

            Return True
        End Function

#End Region

#Region "Diode"
        Public Function CheckLddContact() As Boolean
            Dim i As Integer
            Dim s, fmt As String
            Dim success As Boolean
            Dim vRead, iRead, iSet As Double

            mMsgInfo.PostMessage("    Check probe contact and laser diodes ... ")

            fmt = w2String.Concatenate(ControlChars.Tab, "{0,2:0}", "{1,6:0.00}", "{2,6:0.00}", "{3,6:0.00}")
            s = String.Format(fmt, "CH", "iSet", "iRead", "vRead")
            mMsgInfo.PostMessage(s)

            iSet = mPara.LaserDiode.DefaultCurrent

            success = True
            For i = 1 To mInst.LDD.ChannelCount
                'apply current
                mInst.LDD.Current(i) = iSet
                'read voltage back
                vRead = mInst.LDD.Voltage(i)
                iRead = mInst.LDD.Current(i)
                'show
                s = String.Format(fmt, i, iSet, iRead, vRead)
                mMsgInfo.PostMessage(s)
                'check
                If vRead < mPara.LaserDiode.MinVoltage Then
                    success = False
                    s = "Diode voltage {0:0.00}V below the spec limit {1:0.00}V. Please check contact for possible short."
                    s = String.Format(s, vRead, mPara.LaserDiode.MinVoltage)
                    mMsgInfo.PostMessage("X   " + s)
                End If
                If Math.Abs(iRead - iSet) > mPara.LaserDiode.MaxCurrentError Then
                    success = False
                    s = "Current set/read error {0:0.00}mA above the spec limit {1:0.00}mA. Please check contact for possible short."
                    s = String.Format(s, iRead - iSet, mPara.LaserDiode.MaxCurrentError)
                    mMsgInfo.PostMessage("X   " + s)
                End If
            Next i

            'NOTE: depend driver board capability we can check the read back current if they are actual
            '      depend package status, we can also check the MPD current reading
            s = String.Format(fmt, mPara.LaserDiode.MinVoltage, "", "", "")
            mMsgInfo.PostMessage("X   " + s)

            Return success
        End Function

        Public Function TurnLddOff() As Boolean

            Dim i As Integer

            mMsgInfo.PostMessage("Turn LDD off for loading or unloading...")

            For i = 1 To mInst.LDD.ChannelCount
                mInst.LDD.Current(i) = 0.0

            Next
            Return True
        End Function

        Public Function TurnLddOn(ByVal channel As Integer, ByVal current As Double) As Boolean
            Dim s As String
            Dim i As Integer
            'set current, do this first, the zeroing of other channel acts as a delay fot this set channel
            mInst.LDD.Current(channel) = current

            s = "Apply current {0:0.0}mA to Channel {1}"
            s = String.Format(s, current, channel)
            mMsgInfo.PostMessage(s)

            'zero other channels
            For i = 1 To 4
                If i <> channel Then
                    'zero setting
                    mInst.LDD.Current(i) = 0.0
                End If
                'read back
            Next

            Return True
        End Function




        Public Function SetTemperature(ByVal temp As Double) As Boolean
            Dim tempRead As Double
            Dim success As Boolean
            Dim s As String
            Dim k As Integer



            k = 0
            mInst.LDD.TemperatureSetpoint = temp
            tempRead = mInst.LDD.TemperatureReading

            'While True
            '    System.Threading.Thread.Sleep(100)
            '    tempRead = mInst.LDD.TemperatureReading
            '    If Math.Abs(tempRead - temp) < 0.1 Then
            '        s = "Temperature {0:0.00} has be set to package."
            '        s = String.Format(s, temp)
            '        mMsgInfo.PostMessage(s)
            '        success = True
            '        Exit While
            '    Else
            '        k += 1
            '        s = "Wait for temperature to stablize..."
            '        mMsgInfo.PostMessage(s)
            '        System.Threading.Thread.Sleep(200)
            '        If k > 5 Then
            '            Return False
            '        End If
            '    End If
            'End While

            Return True
        End Function
#End Region




#Region "Force Guage"
        Public Function GetForceGuageReading(ByVal eStage As iXpsStage.StageEnum, ByVal Samples As Integer) As Double
            Dim v As Double
            Dim i As Integer
            Dim flag As Boolean
            Dim value As Double

            Dim x As Instrument.iOmegaDP40

            Select Case eStage
                Case iXpsStage.StageEnum.Hexapod
                    x = mInst.ForceGaugeHexapod
                Case iXpsStage.StageEnum.Main
                    x = mInst.ForceGaugeMain
                Case Else
                    Return Double.NaN
            End Select

            If x IsNot Nothing Then
                v = 0

                flag = True

                For i = 1 To Samples
                    'While (flag)
                    '    value = x.ReadProcessValue()
                    '    'If value > 100 Then
                    '    '    flag = False
                    '    'End If
                    '    flag = False
                    'End While
                    value = x.ReadProcessValue()
                    v += value
                    flag = True
                Next
                v /= Samples
            Else
                Return Double.NaN
            End If

            Return v
        End Function

        'Public Function TestContactWithForceGauge() As Boolean
        '    Dim s As String
        '    Dim success As Boolean
        '    Dim v, F0, F1 As Double

        '    mMsgInfo.PostMessage("    Check contact with force gauage ... ")

        '    F0 = Me.GetForceGuageReading(fPartTray.PartEnum.Lens, mPara.zTouchSense.Samples)
        '    s = "      Initial force " + F0.ToString("0.0")
        '    mMsgInfo.PostMessage(s)

        '    v = mPara.zTouchSense.SimpleCheckZStep
        '    s = "         Move stage " + v.ToString("0.0") + " mm"
        '    mMsgInfo.PostMessage(s)
        '    mTool.mInst.StageBase.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.zTouchSense.Velocity)
        '    mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, v, False)

        '    F1 = Me.GetForceGuageReading(fPartTray.PartEnum.Lens, mPara.zTouchSense.Samples)
        '    s = "      Current force " + F1.ToString("0.0")
        '    mMsgInfo.PostMessage(s)

        '    v = (F1 - F0)
        '    s = "       Force change " + v.ToString("0.0")
        '    mMsgInfo.PostMessage(s)

        '    success = (v > mPara.zTouchSense.ForceChangeThreshold)
        '    If success Then
        '        s = "       Touch confirmed!)"
        '    Else
        '        s = "X    Force change < expected threshold of " + mPara.zTouchSense.ForceChangeThreshold.ToString("0.0")
        '    End If
        '    mMsgInfo.PostMessage(s)

        '    'return stage back 
        '    v = -mPara.zTouchSense.SimpleCheckZStep
        '    s = "         Move stage back"
        '    mMsgInfo.PostMessage(s)
        '    mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, v, False)

        '    'recover speed
        '    mTool.mInst.StageBase.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

        '    Return success
        'End Function
#End Region

#Region "epoxy"
        Public Function ApplyEpoxy(ByVal sPart As String, ByVal PauseForVisualCheck As Boolean, ByVal seconds As Double, ByVal Steps As Double, ByVal Amount As Double) As Boolean
            Dim s As String
            Dim zSafe As Double
            Dim success As Boolean
            Dim partIndex As Integer
            Dim ePart As iXpsStage.PartEnum
            Dim bsLength As Double

            Try
                ePart = CType(sPart, iXpsStage.PartEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong part identification: " + sPart + "Error: " + ex.Message)
                Return False
            End Try


            mMsgInfo.PostMessage("      Lift arm to top")
            success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, -50.0, False)
            If Not success Then Return False

            s = "    Apply epoxy for part: " + ePart.ToString() + " ... "
            mMsgInfo.PostMessage(s)

            mMsgInfo.PostMessage("      Move epoxy tool out")
            mIO.EpoxyMoveOut = True

            partIndex = CInt(sPart) + 10

            mMsgInfo.PostMessage("      Move stage to the tool")
            success = mStage.MoveStageToNamedPosition(partIndex.ToString(), PauseForVisualCheck)
            If Not success Then Return False

            Select Case partIndex
                Case 10
                    mMsgInfo.PostMessage("      Apply epoxy for first spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")

                    mMsgInfo.PostMessage("      Move to second spot...")
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -1, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.6, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 1, False)

                    mMsgInfo.PostMessage("      Apply epoxy for second spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")
                Case 11
                    mMsgInfo.PostMessage("      Apply epoxy for first spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")

                    mMsgInfo.PostMessage("      Move to second spot...")
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -1, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.6, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 1, False)

                    mMsgInfo.PostMessage("      Apply epoxy for second spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")
                Case 12
                    mMsgInfo.PostMessage("      Apply epoxy for first spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")

                    mMsgInfo.PostMessage("      Move to second spot...")
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -1, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.6, False)
                    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 1, False)

                    mMsgInfo.PostMessage("      Apply epoxy for second spot...")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")
                Case Else
                    mMsgInfo.PostMessage("      Apply epoxy")
                    mIO.EpoxyTriggerOnce()

                    mMsgInfo.PostMessage("      Wait for a few seconds")
                    mTool.WaitForTime(seconds, "Wait for glue to flow")
            End Select

            mMsgInfo.PostMessage("      Lift arm slowly at first...")
            mStage.MoveStageBySteps("2", Steps, Amount)

            mMsgInfo.PostMessage("      Lift arm to safe place")
            zSafe = mInst.StageBase.ZforSafeMove
            success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, -50.0, False)
            If Not success Then Return False

            mMsgInfo.PostMessage("      Move epoxy tool back")
            mIO.EpoxyMoveOut = False
            Return True
        End Function
#End Region

#Region "UV Tool"
        Public Function RunUvCure() As Boolean
            Dim HaveCtrl As Boolean
            Dim response As DialogResult
            Dim s, fmt As String

            fmt = "Wait UV exposure to finish in {0:0.00} sec, Elapse {1:0.00} sec"

            mMsgInfo.PostMessage("    Start UV Cure process ...")

            'check if we have UV control
            HaveCtrl = (mInst.UvLamp IsNot Nothing)

            If HaveCtrl Then
                'check error
                s = ""
                If mInst.UvLamp.AlarmOn Then s += "Alarm on, "
                If Not mInst.UvLamp.LampOn Then s += "Lamp off, "
                If Not mInst.UvLamp.LampReady Then s += "Lamp not ready, "
                If mInst.UvLamp.ShutterOpen Then s += "Shutter open and exposure running, "
                'If mInst.UvLamp.NeedCalibration Then s += "Need calibration"

                If s <> "" Then
                    s = "X   UV cure tool is not ready: " + s
                    mMsgInfo.PostMessage(s)
                    Return False
                End If

                'start async run
                mInst.UvLamp.RunExposure(False)
                mMsgInfo.PostMessage("      Start exposure for " + mInst.UvLamp.ExposureTimeExpected.ToString("0.00") + " sec")

                'wait

                'loop
                While mInst.UvLamp.ExposureInProgress
                    'check stop
                    Application.DoEvents()
                    If mTool.mStop Then
                        mMsgInfo.PostMessage("      Stopping UV exposure ...")
                        mInst.UvLamp.StopUvExposure()
                    End If
                    'wait
                    System.Threading.Thread.Sleep(100)
                    'display
                    s = String.Format(fmt, mInst.UvLamp.ExposureTimeExpected, mInst.UvLamp.ExposureTimePassed)
                    mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
                End While

                'done
                If mTool.mStop Then
                    s = " Stopped!"
                Else
                    s = " Done!"
                End If

                mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.ContinuingStatus)
                Return (Not mTool.mStop)

            Else
                mMsgInfo.PostMessage("      No controller found. Proceed the process manually.")

                s = "Please have the UV cure tool ready."
                response = MessageBox.Show(s, "UV Cure", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                If response = DialogResult.Cancel Then Return False
                mMsgInfo.PostMessage("      Tool ready confirmed by operator")


                s = "Please proceed with the UV cure. Clock OK when it is done."
                response = MessageBox.Show(s, "UV Cure", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                If response = DialogResult.Cancel Then Return False
                mMsgInfo.PostMessage("      Cure finished and confirmed by operator")

                Return True
            End If


        End Function

        Public Function RunUvCureFutansi(ByVal channel As iFUTANSI.ChannelEnum, ByVal OnOff As Boolean) As Boolean
            mInst.UvLamp.ActiveSingleChannel = channel
            mInst.UvLamp.ShutterOpen = OnOff

            Return True
        End Function
#End Region

#Region "Vision System"
        Public Enum CcdViewIndex
            PackageView = 1
            EpoxyView
            LensTopView = 21
            LensBotView = 7
            Bs1TopView = 31
            Bs1BotView = 81
            Bs2TopView = 3
            Bs2BotView = 8
            PbsTopView = 4
            PbsBotView = 9
            LensSecondTopView = 2
            PickupBotView = 5
            EpoxyPinBotView = 6
            PickupSideView = 10
            EpoxyPinSideView = 11
            Recheck = 12
            TrayPlateTopView = 15
            AutoFocusforLens = 16
            AutoFocusforBS = 18
            AutoFocusforEpoxyPin = 17
            OmuxTopView = 13
            TopCcdCalibration = 19
            BotCcdCalibration = 20
            TopCcdAngleCalibration = 22
            BotCcdAngleCalibration = 23
            PbsRecheckView = 24

        End Enum

        Public Function ProcessCcdView(ByVal sPosition As String, ByVal eType As fPartTray.PartEnum, ByVal PartIndexInTray As Integer) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim angle, X0, Y0, Angle0 As Double
            Dim offset As iXpsStage.Position2D
            Dim target As iXpsStage.Position3D
            Dim ePosition As iXpsStage.StagePositionEnum
            Dim ViewIndex As CcdViewIndex

            'parse position
            Try
                ePosition = CType(sPosition, iXpsStage.StagePositionEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong position identification: " + sPosition + "Error: " + ex.Message)
                Return False
            End Try

            'ack
            mMsgInfo.PostMessage("    Image process for " + ePosition.ToString() + " ...")

            'we will only accept CCD positions
            If Not mInst.StageBase.IsCcdPosition(ePosition) Then
                mMsgInfo.PostMessage("X   The requested position is not a CCD view position")
                Return False
            End If

            'get index for the image processor
            Select Case ePosition
                Case iXpsStage.StagePositionEnum.CcdPackage1View, iXpsStage.StagePositionEnum.CcdPackage2View, iXpsStage.StagePositionEnum.CcdPackage3View, iXpsStage.StagePositionEnum.CcdPackage4View
                    ViewIndex = CcdViewIndex.PackageView


                Case iXpsStage.StagePositionEnum.CcdPartTopView
                    Select Case eType
                        Case fPartTray.PartEnum.Lens
                            ViewIndex = CcdViewIndex.LensTopView

                        Case fPartTray.PartEnum.BS1
                            ViewIndex = CcdViewIndex.Bs1TopView

                        Case fPartTray.PartEnum.BS2
                            ViewIndex = CcdViewIndex.Bs2TopView

                        Case fPartTray.PartEnum.PBS
                            ViewIndex = CcdViewIndex.PbsTopView
                    End Select

                Case iXpsStage.StagePositionEnum.CcdPartBottomView
                    Select Case eType
                        Case fPartTray.PartEnum.Lens
                            ViewIndex = CcdViewIndex.LensBotView

                        Case fPartTray.PartEnum.BS1
                            ViewIndex = CcdViewIndex.Bs1BotView

                        Case fPartTray.PartEnum.BS2
                            ViewIndex = CcdViewIndex.Bs2BotView

                        Case fPartTray.PartEnum.PBS
                            ViewIndex = CcdViewIndex.PbsBotView
                    End Select

                Case iXpsStage.StagePositionEnum.CcdEpoxyView
                    ViewIndex = CcdViewIndex.EpoxyView

                Case iXpsStage.StagePositionEnum.CcdOmuxView
                    ViewIndex = CcdViewIndex.OmuxTopView

                Case iXpsStage.StagePositionEnum.CcdPbsRecheckView
                    ViewIndex = CcdViewIndex.PbsRecheckView

                Case Else
                    mMsgInfo.PostMessage("X   The requested position is not a CCD view position")
                    Return False

            End Select

            'get stage position
            target = mInst.StageBase.GetConfiguredStagePosition(ePosition)

            mMsgInfo.PostMessage("       Target position = " + target.ToString(CInt("0.000")))

            If ePosition = iXpsStage.StagePositionEnum.CcdPartTopView Then
                'we need to compensate the position for the part pickup
                'get part position inside the tray
                offset = mInst.StageBase.GetPartPositionInTray(PartIndexInTray)

                'add offset
                target.X += offset.X
                target.Y += offset.Y
                If eType <> fPartTray.PartEnum.Lens Then
                    target.Z -= 0.5
                End If



                s = "      Add offset to the part in the tray"
                s += ControlChars.CrLf + "      Part Idx in tray = " + PartIndexInTray.ToString("0.000")
                s += ControlChars.CrLf + "       Offset position = " + offset.ToString(CInt("0.000"))
                s += ControlChars.CrLf + "        Final position = " + target.ToString(CInt("0.000"))
                mMsgInfo.PostMessage(s)
            End If

            If ePosition = iXpsStage.StagePositionEnum.CcdPartBottomView Then
                If eType <> fPartTray.PartEnum.Lens Then
                    target.Z -= 0.4
                End If
            End If

            'do move now
            success = mStage.MoveStage3Axis(target.X, target.Y, target.Z, False)
            If Not success Then Return False

            'do image process
            success = Me.ProcessImage(ViewIndex, 0, offset.X, offset.Y, angle)
            If Not success Then
                mMsgInfo.PostMessage("X   Failed to get vision data. Error Code = " + mInst.VisionDll.GetErrorString())
                Return False
            End If

            s = "      Offset X = " + offset.X.ToString("0.000")
            s += ControlChars.CrLf + "       Offset Y = " + offset.Y.ToString("0.000")
            s += ControlChars.CrLf + "       Angle = " + angle.ToString("0.000")
            mMsgInfo.PostMessage(s)

            'If ViewIndex = 21 Then
            '    success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.31, False)
            '    success = Me.ProcessImage(CcdViewIndex.LensSecondTopView, 0, offset.X, offset.Y, angle)
            '    s = "      Another Method To Calculate the offset of Lens By focusing on the arc "
            '    s += "      Offset X = " + offset.X.ToString("0.000")
            '    s += ControlChars.CrLf + "       Offset Y = " + offset.Y.ToString("0.000")
            '    s += ControlChars.CrLf + "       Angle = " + angle.ToString("0.000")
            '    mMsgInfo.PostMessage(s)
            'End If

            'get gold image offset

            Select Case ViewIndex
                Case CcdViewIndex.Bs1TopView, CcdViewIndex.Bs2TopView, CcdViewIndex.LensTopView, CcdViewIndex.PbsTopView
                    offset.X -= 0
                    offset.Y -= 0
                    angle -= 0
                Case CcdViewIndex.Bs1BotView, CcdViewIndex.Bs2BotView, CcdViewIndex.PbsBotView, CcdViewIndex.LensBotView
                    offset.X -= 0
                    offset.Y -= 0
                    angle -= 0
                Case CcdViewIndex.PackageView
                    'success = mPara.ReadGoldImgaeOffset(ViewIndex, X0, Y0, Angle0)
                    'If Not success Then
                    '    mMsgInfo.PostMessage("X   Failed to read the gold image offset data")
                    '    Return False
                    'End If

                    ''the difference 
                    'offset.X -= X0
                    'offset.Y -= Y0
                    'angle -= Angle0

                    offset.X -= 0
                    offset.Y -= 0
                    angle -= 0
            End Select

            Select Case ViewIndex
                Case CcdViewIndex.Bs1BotView, CcdViewIndex.Bs2BotView, CcdViewIndex.PbsBotView, CcdViewIndex.LensBotView
                    mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, angle, False)
                    success = Me.ProcessImage(ViewIndex, 0, offset.X, offset.Y, angle)
                    s = "After Rotate the part:"
                    s += ControlChars.CrLf + "      Offset X = " + offset.X.ToString("0.000")
                    s += ControlChars.CrLf + "       Offset Y = " + offset.Y.ToString("0.000")
                    s += ControlChars.CrLf + "       Angle = " + angle.ToString("0.000")
                    mMsgInfo.PostMessage(s)

            End Select


            'process image result - some data will be saved and applied later
            Select Case ePosition
                Case iXpsStage.StagePositionEnum.CcdPackage1View
                    mPara.UpdatePackageOffset(0, offset, angle)

                Case iXpsStage.StagePositionEnum.CcdPackage2View
                    mPara.UpdatePackageOffset(1, offset, angle)

                Case iXpsStage.StagePositionEnum.CcdPackage3View
                    mPara.UpdatePackageOffset(2, offset, angle)

                Case iXpsStage.StagePositionEnum.CcdPackage4View
                    mPara.UpdatePackageOffset(3, offset, angle)

                Case iXpsStage.StagePositionEnum.CcdPartBottomView
                    mPara.UpdatePartOffset(offset, angle)

                Case iXpsStage.StagePositionEnum.CcdPartTopView
                    mPara.UpdatePartOffsetInTray(offset, angle)

                Case iXpsStage.StagePositionEnum.CcdOmuxView
                    mPara.UpdateOmuxOffset(offset, angle)
            End Select

            Return True
        End Function

        Public Function SaveGoldImageData(ByVal eSelection As CcdViewIndex) As Boolean
            Dim success As Boolean
            Dim s As String
            Dim X, Y, Angle As Double
            Dim X1, Y1, Angle1 As Double

            'ack
            mMsgInfo.PostMessage("    Take gold image and save data to config file ...")

            'no image process
            mMsgInfo.PostMessage("      Take image and do image process")
            success = Me.ProcessImage(eSelection, 0, X, Y, Angle)
            If Not success Then
                mMsgInfo.PostMessage("X   Failed to get vision data. Error Code = " + mInst.VisionDll.GetErrorString())
                Return False
            End If

            'save X, Y, and Angle to file
            mMsgInfo.PostMessage("      Save data to file")
            success = mPara.SaveGoldImageOffset(eSelection, X, Y, Angle)
            If Not success Then
                mMsgInfo.PostMessage("X   Filed to save gold image offset to config file")
                Return False
            End If

            'try to read back
            mMsgInfo.PostMessage("      Read back from file")
            success = mPara.ReadGoldImgaeOffset(eSelection, X1, Y1, Angle1)
            If Not success Then
                mMsgInfo.PostMessage("X   Failed to read the gold image offset data")
                Return False
            End If

            'show data
            s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,10}", "{1,10:0.000000}", "{2,10:0.000000}")
            mMsgInfo.PostMessage(String.Format(s, "Item", "Saved", "Read"))
            mMsgInfo.PostMessage(String.Format(s, "Offset X", X, X1))
            mMsgInfo.PostMessage(String.Format(s, "Offset Y", Y, Y1))
            mMsgInfo.PostMessage(String.Format(s, "Angle   ", Angle, Angle1))

            'done
            Return True
        End Function

        Private Function ProcessImage(ByVal eSelection As CcdViewIndex, ByVal ImageIndex As Integer, ByRef X As Double, ByRef Y As Double, ByRef Angle As Double) As Boolean
            Dim success As Boolean
            Dim ImageFolder As String

            'get image data
            mInst.VisionDll.GetVisionData(eSelection, 0, 1, 0, X, Y, Angle)

            System.Threading.Thread.Sleep(200)

            success = mInst.VisionDll.GetVisionData(eSelection, 0, 1, 1, X, Y, Angle)
            If Not success Then Return False

            'do scaling
            Select Case eSelection
                Case CcdViewIndex.Bs2TopView, CcdViewIndex.LensTopView, CcdViewIndex.PbsTopView
                    'top view XY scaling, in mm
                    X *= -0.001
                    Y *= -0.001
                Case CcdViewIndex.Bs1TopView
                    X *= -0.001
                    Y *= -0.001
                Case CcdViewIndex.PackageView
                    X *= -0.001
                    Y *= -0.001
                Case CcdViewIndex.Bs2BotView, CcdViewIndex.LensBotView, CcdViewIndex.PbsBotView, CcdViewIndex.Bs1BotView
                    'bottom view XY scaling
                    X *= 0.001
                    Y *= -0.001
                    Angle *= -1

                Case CcdViewIndex.EpoxyView
                    'side view XY scaling
                    X *= 0.001
                    Y *= 0.001

            End Select

            If mTool.mFormImage Is Nothing Then
                mTool.mFormImage = New fImage(mTool.mParent)
            End If
            ImageFolder = Application.StartupPath & "\Scene" & eSelection & "\ImageSave"
            mTool.mFormImage.SetImage(ImageFolder)

            Return True
        End Function

        Public Function ProcessImage(ByVal ViewIndex As Integer) As Boolean
            Dim X, Y, angle As Double
            Dim success As Boolean
            Dim ImageFolder As String
            Dim s As String
            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)
            Select Case ViewIndex
                Case 1, 21, 31, 3, 4
                    X *= -0.001
                    Y *= -0.001
                Case 11
                    X *= 1
                    Y *= 1
                Case Else
                    X *= 0.001
                    Y *= -0.001
            End Select
            s = "      Offset X = " + X.ToString("0.000")
            s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
            s += ControlChars.CrLf + "       Angle = " + angle.ToString("0.000")
            mMsgInfo.PostMessage(s)

            If mTool.mFormImage Is Nothing Then
                mTool.mFormImage = New fImage(mTool.mParent)
            End If
            ImageFolder = Application.StartupPath & "\Scene" & ViewIndex.ToString() & "\ImageSave"
            mTool.mFormImage.SetImage(ImageFolder)


            Return success
        End Function

        Public Function AutoFocus(ByVal sPosition As String, ByVal range As Double, ByVal eStep As Double) As Boolean

            Dim ePosition As iXpsStage.StagePositionEnum
            Dim success, reversed, peakFound As Boolean
            Dim s, sStep As String
            Dim angle, X, Y, Z, Z0, fineStep As Double
            Dim para, para0, LastPeak As Double
            Dim ImageFolder As String
            Dim n As Integer

            Dim target As iXpsStage.Position3D
            Dim ViewIndex As Integer

            fineStep = eStep / 5

            Try
                ePosition = CType(sPosition, iXpsStage.StagePositionEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong position identification: " + sPosition + "Error: " + ex.Message)
                Return False
            End Try

            target = mInst.StageBase.GetConfiguredStagePosition(ePosition)
            mMsgInfo.PostMessage("       Target position = " + target.ToString(CInt("0.000")))

            success = mStage.MoveStage3Axis(target.X, target.Y, target.Z, False)
            If Not success Then Return False

            Select Case sPosition
                Case "71"
                    ViewIndex = 16
                Case "72"
                    ViewIndex = 17
                Case "73"
                    mMsgInfo.PostMessage("      Move epoxy tool out")
                    mIO.EpoxyMoveOut = True
                    ViewIndex = 18
                Case "74"
                    ViewIndex = 19
            End Select

            success = mInst.VisionDll.GetVisionData(ViewIndex, 1, 1, 1, X, Y, angle)
            para0 = X

            s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,7:0.0000}", "{1,7:0.00}", "{2}")
            mMsgInfo.PostMessage(String.Format(s, "Z", "Focus Para", "Adjustment"))

            Z0 = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)

            sStep = "Initial"
            reversed = False
            peakFound = False

            n = 0
            While True
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit While
                End If

                'get data
                Z = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)
                success = mInst.VisionDll.GetVisionData(ViewIndex, 1, 1, 0, X, Y, angle)
                If Not success Then Exit While

                para = X

                'show data
                mMsgInfo.PostMessage(String.Format(s, Z, para, sStep))
                If peakFound Then
                    success = True
                    success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)
                    If mTool.mFormImage Is Nothing Then
                        mTool.mFormImage = New fImage(mTool.mParent)
                    End If
                    ImageFolder = Application.StartupPath & "\Scene" & ViewIndex.ToString() & "\ImageSave"
                    mTool.mFormImage.SetImage(ImageFolder)

                    Exit While
                End If

                'check intensity - adjust laser current first
                Select Case True

                    Case n = 0
                        'first X adjustment, change flag
                        sStep = "Stage Z first"

                    Case para > LastPeak
                        'peak still increasing, keep going
                        sStep = "Stage Z " + Math.Sign(eStep).ToString("0")

                    Case para < LastPeak
                        'peak decrease

                        If n = 1 Then
                            n = 0
                            eStep = -eStep
                            sStep = "Stage Z large step reverse, first step decrease"
                        Else
                            'move back, at reduced step size
                            eStep = -Math.Sign(eStep) * fineStep
                            'do not do reverse twice
                            If reversed Then
                                peakFound = True
                                mMsgInfo.PostMessage("<- Signal drop again", w2.w2MessageService.MessageDisplayStyle.ContinuingMessage)
                                sStep = "<- Back to peak"
                            Else
                                'peak decreasing, reverse X
                                reversed = True
                                sStep = "Stage Z " + Math.Sign(eStep).ToString("0")
                            End If
                        End If
                End Select

                'check range
                If Math.Abs(Z + eStep - Z0) > range Then
                    s = "X   Stage Z has moved away from initial position by {0:0.0000}, more than it is allowed {1:0.0000} "
                    s = String.Format(s, (Z + eStep - Z0), range)
                    mMsgInfo.PostMessage(s)
                    success = False
                    Exit While
                End If

                'move X now
                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, iMotionController.MoveToTargetMethodEnum.Relative, eStep, False)
                If Not success Then
                    Exit While
                End If
                success = mStage.WaitStageToStop("Wait for Stage Z to stop", True)
                If Not success Then Exit While

                'next 
                LastPeak = para
                n += 1
            End While



            Return success
        End Function
        Public Function AutoCalibration(ByVal index As String) As Boolean
            Dim success, isPartTray, isEpoxyPin, isPickup As Boolean

            Select Case index
                Case "0"
                    isPartTray = False
                    isEpoxyPin = False
                    isPickup = True
                Case "1"
                    isPartTray = True
                    isEpoxyPin = False
                    isPickup = False
                Case "2"
                    isPartTray = False
                    isEpoxyPin = True
                    isPickup = False
                Case Else
                    isPartTray = True
                    isEpoxyPin = True
                    isPickup = True
            End Select

            If isPartTray Then
                success = AutoCalibrateTrayPlate()
            End If

            If isPickup Then
                success = AutoCalibratePickup()
            End If

            If isEpoxyPin Then
                success = AutoCalibrateEpoxyPin()
            End If



            Return success

        End Function

        Public Function AutoCalibrateTrayPlate() As Boolean
            Dim success As Boolean
            Dim ViewIndex As Integer
            Dim ImageFolder As String
            Dim target, target0, offset As iXpsStage.Position3D
            Dim X, Y, angle As Double

            Dim s As String
            ViewIndex = 15

            mMsgInfo.PostMessage("    Proceed Part Tray Plate Calibration Automatically...")
            mMsgInfo.PostMessage("    Move To Part Tray Plate Top View Position...")

            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.CcdPartTopView)

            success = mStage.MoveStage3Axis(target0.X, target0.Y, target0.Z, False)
            If Not success Then Return False

            mMsgInfo.PostMessage("    Image Process For Part Tray Plate...")

            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 0, X, Y, angle)
            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)
            'Show image
            If mTool.mFormImage Is Nothing Then
                mTool.mFormImage = New fImage(mTool.mParent)
            End If
            ImageFolder = Application.StartupPath & "\Scene" & ViewIndex.ToString() & "\ImageSave"
            mTool.mFormImage.SetImage(ImageFolder)



            X *= -0.001
            Y *= -0.001

            While (Math.Abs(X) > 0.005) Or (Math.Abs(Y) > 0.005)
                s = "      Offset X = " + X.ToString("0.000")
                s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
                mMsgInfo.PostMessage(s)

                mMsgInfo.PostMessage("    Correct The Tray Plate Position And Do Image Process Again...")

                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, iMotionController.MoveToTargetMethodEnum.Relative, -X, False)
                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, iMotionController.MoveToTargetMethodEnum.Relative, -Y, False)

                success = mInst.VisionDll.GetVisionData(15, 0, 1, 0, X, Y, angle)
                success = mInst.VisionDll.GetVisionData(15, 0, 1, 1, X, Y, angle)

                X *= -0.001
                Y *= -0.001

            End While
            mMsgInfo.PostMessage("   The Final Offset:")
            s = "      Offset X = " + X.ToString("0.000")
            s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
            mMsgInfo.PostMessage(s)

            mMsgInfo.PostMessage("   Update The Configured Position For Part Tray Plate...")
            target.X = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageX)
            target.Y = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageY)
            target.Z = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)

            mMsgInfo.PostMessage("   Update The Ccd Part Top View Position...")
            success = Me.SaveConfiguredPositionAndShow("Ccd Part Top View", target, target0)

            offset.X = target.X - target0.X
            offset.Y = target.Y - target0.Y
            offset.Z = target.Z - target0.Z


            mMsgInfo.PostMessage("   Update The Lens Pickup Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.LensPickup)

            target.X = target0.X + offset.X
            target.Y = target0.Y + offset.Y
            target.Z = target0.Z

            success = Me.SaveConfiguredPositionAndShow("Lens Pickup", target, target0)

            mMsgInfo.PostMessage("   Part Tray Plate Calibration Finished!")

            Return success

        End Function

        Public Function AutoCalibratePickup() As Boolean
            Dim success As Boolean
            Dim target0, target As iXpsStage.Position3D
            Dim X, Y, angle As Double
            Dim offset As Double
            Dim ImageFolder As String
            Dim ViewIndex As Integer
            Dim s As String

            ViewIndex = 5

            mMsgInfo.PostMessage("    Proceed Pickup Tool Calibration Automatically...")
            mMsgInfo.PostMessage("    Move To Partup Tool Bottom View Position And Auto Focus On Pickup Tool...")


            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.AutoFocusForPickup)
            success = mStage.MoveStage3Axis(target0.X, target0.Y, target0.Z, False)

            'offset = target.Z - target0.Z

            mMsgInfo.PostMessage("    Image Process For Pickup Tool...")

            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 0, X, Y, angle)
            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)


            'Show image
            If mTool.mFormImage Is Nothing Then
                mTool.mFormImage = New fImage(mTool.mParent)
            End If
            ImageFolder = Application.StartupPath & "\Scene" & ViewIndex.ToString() & "\ImageSave"
            mTool.mFormImage.SetImage(ImageFolder)



            X *= 0.001
            Y *= -0.001

            While (Math.Abs(X) > 0.005) Or (Math.Abs(Y) > 0.005)
                s = "      Offset X = " + X.ToString("0.000")
                s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
                mMsgInfo.PostMessage(s)

                mMsgInfo.PostMessage("    Correct Pickup Position And Do Image Process Again...")

                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, iMotionController.MoveToTargetMethodEnum.Relative, -X, False)
                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, iMotionController.MoveToTargetMethodEnum.Relative, -Y, False)

                success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 0, X, Y, angle)
                success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)

                X *= 0.001
                Y *= -0.001

            End While

            mMsgInfo.PostMessage("   The Final Offset:")
            s = "      Offset X = " + X.ToString("0.000")
            s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
            mMsgInfo.PostMessage(s)

            mMsgInfo.PostMessage("   Update The Related Configured Positions For Pickup Tool")
            'Update auto focus positions for pickup
            mMsgInfo.PostMessage("   Update The Auto Focus For Pickup Position...")

            target.X = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageX)
            target.Y = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageY)
            target.Z = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)

            success = Me.SaveConfiguredPositionAndShow("Auto Focus For Pickup", target, target0)


            'Update ccd bottom view position for part
            mMsgInfo.PostMessage("   Update The Ccd Part Bottom View Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.CcdPartBottomView)

            target.X = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageX)
            target.Y = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageY)
            target.Z = target0.Z

            success = Me.SaveConfiguredPositionAndShow("Ccd Part Bottom View", target, target0)

            mMsgInfo.PostMessage("   Pickup Tool Calibration Finished!")




            Return success

        End Function

        Public Function AutoCalibrateEpoxyPin() As Boolean
            Dim success As Boolean
            Dim target0, target, offset As iXpsStage.Position3D
            Dim X, Y, angle As Double
            Dim ViewIndex As Integer
            Dim ImageFolder As String

            ViewIndex = 6


            Dim s As String

            mMsgInfo.PostMessage("    Proceed Epoxy Pin Calibration Automatically...")
            mMsgInfo.PostMessage("    Move To Epoxy Pin Bottom View Position And Auto Focus On Epoxy Pin...")
            success = Me.AutoFocus("73", 0.5, 0.05)

            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.AutoFocusForPin)



            mMsgInfo.PostMessage("    Image Process For Epoxy Pin...")

            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 0, X, Y, angle)
            success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)

            'Show image
            If mTool.mFormImage Is Nothing Then
                mTool.mFormImage = New fImage(mTool.mParent)
            End If
            ImageFolder = Application.StartupPath & "\Scene" & ViewIndex.ToString() & "\ImageSave"
            mTool.mFormImage.SetImage(ImageFolder)

            X *= 0.001
            Y *= -0.001

            While (Math.Abs(X) > 0.005) And (Math.Abs(Y) > 0.005)
                s = "      Offset X = " + X.ToString("0.000")
                s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
                mMsgInfo.PostMessage(s)

                mMsgInfo.PostMessage("    Correct Epoxy Pin Position And Do Image Process Again...")

                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, iMotionController.MoveToTargetMethodEnum.Relative, -X, False)
                success = mStage.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, iMotionController.MoveToTargetMethodEnum.Relative, -Y, False)

                success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 0, X, Y, angle)
                success = mInst.VisionDll.GetVisionData(ViewIndex, 0, 1, 1, X, Y, angle)

                X *= 0.001
                Y *= -0.001

            End While


            mMsgInfo.PostMessage("   The Final Offset:")
            s = "      Offset X = " + X.ToString("0.000")
            s += ControlChars.CrLf + "       Offset Y = " + Y.ToString("0.000")
            mMsgInfo.PostMessage(s)

            mMsgInfo.PostMessage("   Update The Related Configured Positions For Epoxy Pin")

            mMsgInfo.PostMessage("   Update The Auto Focus For Pin Position...")

            target.X = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageX)
            target.Y = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageY)
            target.Z = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)

            success = Me.SaveConfiguredPositionAndShow("Auto Focus For Pin", target, target0)

            offset.X = target.X - target0.X
            offset.Y = target.Y - target0.Y
            offset.Z = target.Z - target0.Z

            mMsgInfo.PostMessage("   Update The Lens 1 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Lens1Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Lens 1 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Lens 2 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Lens2Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Lens 2 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Lens 3 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Lens3Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Lens 3 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Lens 4 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Lens4Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Lens 4 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Bs 1 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Bs1Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Bs 1 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Bs 2 Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.Bs2Epoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Bs 2 Epoxy", target, target0)

            mMsgInfo.PostMessage("   Update The Pbs Epoxy Position...")
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.PbsEpoxy)
            target.X = target.X + offset.X
            target.Y = target.Y + offset.Y
            target.Z = target.Z + offset.Z
            success = Me.SaveConfiguredPositionAndShow("Pbs Epoxy", target, target0)

            mMsgInfo.PostMessage("      Move epoxy tool in")
            mIO.EpoxyMoveOut = False

            mMsgInfo.PostMessage("   Epoxy Pin Calibration Finished!")


            'Save pin offset to file
            target0 = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.AutoFocusForPin)
            target = mInst.StageBase.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.AutoFocusForPickup)

            offset.X = target.X - target0.X
            offset.Y = target.Y - target0.Y
            offset.Z = target.Z - target0.Z

            mPara.SavePinOffsetToFile(offset.X, offset.Y, offset.Z)
            Return success

        End Function

        Public Function SaveConfiguredPositionAndShow(ByVal label As String, ByVal targetposition As iXpsStage.Position3D, ByVal oldposition As iXpsStage.Position3D) As Boolean
            Dim s As String
            Dim success As Boolean

            mMsgInfo.PostMessage("   The old position is :")
            s = "      X = " + oldposition.X.ToString("0.000")
            s += ControlChars.CrLf + "       Y = " + oldposition.Y.ToString("0.000")
            s += ControlChars.CrLf + "       Z = " + oldposition.Z.ToString("0.000")
            mMsgInfo.PostMessage(s)

            success = mInst.StageBase.SaveConfiguredPosition(label, targetposition)

            mMsgInfo.PostMessage("   The new position is :")
            s = "      X = " + targetposition.X.ToString("0.000")
            s += ControlChars.CrLf + "       Y = " + targetposition.Y.ToString("0.000")
            s += ControlChars.CrLf + "       Z = " + targetposition.Z.ToString("0.000")
            mMsgInfo.PostMessage(s)

            Return success

        End Function

#End Region

#Region "Beam Profile"
        Public Function RecordProfileInfo(ByVal samples As Integer, ByVal mode As iBeamProfiler.DataAcquisitionMode, ByVal count As Integer) As Boolean
            Dim beamData As iBeamProfiler.SimpleData
            beamData = mInst.NanoScan.AcquireData(samples, mode)
            Dim s As String
            Dim StageX, StageY, StageZ As Double



            Dim dateResults As Date



            Dim BeamResults As String
            Dim i As Integer
            Dim path As String = "E:\Data\Beamresults.txt"
            Dim Header As String = "Time          StageX       StageY      StageZ      BeamX     BeamY"
            Dim w As New IO.StreamWriter(path)
            Const Format As String = "{0,10}    {1,10:0.000000}  {2,10:0.000000}  {3,10:0.000000}   {4,6:0.00} {5,6:0.00}"
            'w.WriteLine("Date" + ControlChars.Tab + dateResults.Month.ToString + "." + dateResults.Day.ToString)
            'w.WriteLine("Time" + ControlChars.Tab + dateResults.Hour.ToString + ":" + dateResults.Minute.ToString)
            w.WriteLine()
            w.WriteLine(Header)

            If count <= 0 Then count = 100

            For i = 0 To count
                dateResults = Now
                StageX = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageX)
                StageY = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageY)
                StageZ = mStage.GetCurrentPosition(iXpsStage.AxisNameEnum.StageZ)

                beamData = mInst.NanoScan.AcquireData(samples, mode)
                BeamResults = String.Format(Format, dateResults.Minute.ToString + ":" + dateResults.Second.ToString, StageX, StageY, StageZ, beamData.CentroidX, beamData.CentroidY)
                s = "      Beam Info"
                s += ControlChars.CrLf + "      Centroid Position X: " + beamData.CentroidX.ToString("0.0") + "um, Y: " + beamData.CentroidY.ToString("0.0") + "um"
                s += ControlChars.CrLf + "      TotalPower = " + beamData.TotalEnergy.ToString("0.00" + "mW")
                mMsgInfo.PostMessage(s)
                mMsgInfo.PostMessage(BeamResults)
                System.Threading.Thread.Sleep(100)
                w.WriteLine(BeamResults)

            Next

            w.Close()

            Return True
        End Function

        Public Function GetBeamProfileInfo(ByVal samples As Integer, ByVal mode As iBeamProfiler.DataAcquisitionMode) As Boolean
            Dim beamData As iBeamProfiler.SimpleData
            beamData = mInst.NanoScan.AcquireData(samples, mode)
            Dim s As String

            s = "      Beam Info"
            s += ControlChars.CrLf + "      Beam Width    X: " + beamData.FWHMX.ToString("0.0") + "um, Y: " + beamData.FWHMY.ToString("0.0") + "um"
            s += ControlChars.CrLf + "      Centroid Position X: " + beamData.CentroidX.ToString("0.0") + "um, Y: " + beamData.CentroidY.ToString("0.0") + "um"
            s += ControlChars.CrLf + "      TotalPower = " + beamData.TotalEnergy.ToString("0.00" + "mW")
            mMsgInfo.PostMessage(s)


            Return True
        End Function


        Public Function Move1DAndRecordBeamProfileInfo(ByVal samples As Integer, ByVal mode As iBeamProfiler.DataAcquisitionMode, ByVal sAxis As String, ByVal Steps As Double, ByVal Amounts As Double) As Boolean
            Dim beamData As iBeamProfiler.SimpleData
            beamData = mInst.NanoScan.AcquireData(samples, mode)
            Dim eAxis As iXpsStage.AxisNameEnum
            eAxis = CType(sAxis, iXpsStage.AxisNameEnum)
            Dim s, errormessage, BeamResults As String
            Dim i As Integer
            Dim header1 As String = " "
            Dim header2 As String = "Pos    WidthX   WidthY    PosX    PosY"
            Dim dateResults As Date
            dateResults = Now
            Dim currentposition, originposition As Double
            Dim r As DialogResult

            Select Case sAxis
                Case "0"
                    header1 = "Record Beam Profile Date While X Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
                Case "1"
                    header1 = "Record Beam Profile Date While Y Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
                Case "2"
                    If Math.Abs(Steps * Amounts) > 1 Then
                        errormessage = "Move Z Axis is extremely dangerous. Do you want to continue any way?"
                        r = MessageBox.Show(errormessage, " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        If r = DialogResult.No Then
                            Return False
                        End If
                    End If

                    header1 = "Record Beam Profile Date While Z Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
            End Select

            Dim path As String = "E:\Data\Beamresults_OneAxisScan.txt"
            Dim w As New IO.StreamWriter(path)
            Const Format As String = "{0,7:0.0000}    {1,6:0.00}  {2,6:0.00}  {3,6:0.00}  {4,6:0.00}"
            w.WriteLine("Date" + ControlChars.Tab + dateResults.Month.ToString + "." + dateResults.Day.ToString)
            w.WriteLine("Time" + ControlChars.Tab + dateResults.Hour.ToString + ":" + dateResults.Minute.ToString)
            w.WriteLine(header1)
            w.WriteLine()
            w.WriteLine(header2)

            originposition = mStage.GetCurrentPosition(eAxis)

            For i = 0 To CInt(Amounts)
                beamData = mInst.NanoScan.AcquireData(samples, mode)
                s = "      Beam Info"
                s += ControlChars.CrLf + "      Beam Width    X: " + beamData.D4SigmaX.ToString("0.0") + "um, Y: " + beamData.D4SigmaY.ToString("0.0") + "um"
                s += ControlChars.CrLf + "      Centroid Position X: " + beamData.CentroidX.ToString("0.0") + "um, Y: " + beamData.CentroidY.ToString("0.0") + "um"
                s += ControlChars.CrLf + "      TotalPower = " + beamData.TotalEnergy.ToString("0.00" + "mW")
                mMsgInfo.PostMessage(s)
                System.Threading.Thread.Sleep(500)
                currentposition = mStage.GetCurrentPosition(eAxis)
                BeamResults = String.Format(Format, currentposition, beamData.D4SigmaX, beamData.D4SigmaY, beamData.CentroidX, beamData.CentroidY)
                w.WriteLine(BeamResults)
                mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, Steps, True)
            Next
            w.Close()
            mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, originposition, True)
            Return True
        End Function

        Public Function Move1DAndCalculateSlope(ByVal samples As Integer, ByVal mode As iBeamProfiler.DataAcquisitionMode, ByVal sAxis As String, ByVal Steps As Double, ByVal Amounts As Double, ByVal SN As String) As Boolean
            Dim beamData As iBeamProfiler.SimpleData
            beamData = mInst.NanoScan.AcquireData(samples, mode)
            Dim eAxis As iXpsStage.AxisNameEnum
            eAxis = CType(sAxis, iXpsStage.AxisNameEnum)
            Dim s, errormessage, BeamResults As String
            Dim i, ii As Integer
            Dim header1 As String = " "
            Dim header2 As String = "Pos    WidthX   WidthY    PosX    PosY"
            Dim dateResults As Date
            dateResults = Now
            Dim currentposition, originposition, BeamPositionXstep(9), BeamPositionYstep(9), BeamPositionX(CInt(Amounts)), BeamPositionY(CInt(Amounts)), StagePosition(CInt(Amounts)), PositionXmean, PositionYmean As Double
            Dim Xmean(1), Ymean(1), Zmean(1), XXmean(1), XYmean(1), XZmean(1) As Double
            Dim kx(1), ky(1), bx(1), by(1) As Double
            Dim r As DialogResult

            Select Case sAxis
                Case "0"
                    header1 = "Record Beam Profile Date While X Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
                Case "1"
                    header1 = "Record Beam Profile Date While Y Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
                Case "2"
                    If Math.Abs(Steps * Amounts) > 1 Then
                        errormessage = "Move Z Axis is extremely dangerous. Do you want to continue any way?"
                        r = MessageBox.Show(errormessage, " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        If r = DialogResult.No Then
                            Return False
                        End If
                    End If

                    header1 = "Record Beam Profile Date While Z Axis Moving by " + Steps.ToString() + "mm per step for " + Amounts.ToString() + " times."
            End Select

            Dim path As String = "E:\Data\Beamresults_OneAxisScan" + SN + ".txt"
            Dim w As New IO.StreamWriter(path)
            Const Format As String = "{0,7:0.0000}    {1,6:0.00}  {2,6:0.00}  {3,6:0.00}  {4,6:0.00}"
            w.WriteLine("Date" + ControlChars.Tab + dateResults.Month.ToString + "." + dateResults.Day.ToString)
            w.WriteLine("Time" + ControlChars.Tab + dateResults.Hour.ToString + ":" + dateResults.Minute.ToString)
            w.WriteLine(header1)
            w.WriteLine()
            w.WriteLine(header2)

            originposition = mStage.GetCurrentPosition(eAxis)
            PositionXmean = 0
            PositionYmean = 0


            For i = 0 To CInt(Amounts)
                currentposition = mStage.GetCurrentPosition(eAxis)
                PositionXmean = 0
                PositionYmean = 0
                For ii = 0 To 9

                    beamData = mInst.NanoScan.AcquireData(samples, mode)
                    s = "      Beam Info"
                    s += ControlChars.CrLf + "      Centroid Position X: " + beamData.CentroidX.ToString("0.0") + "um, Y: " + beamData.CentroidY.ToString("0.0") + "um"
                    mMsgInfo.PostMessage(s)

                    BeamPositionXstep(ii) = beamData.CentroidX
                    BeamPositionYstep(ii) = beamData.CentroidY
                    PositionXmean = PositionXmean + BeamPositionXstep(ii)
                    PositionYmean = PositionYmean + BeamPositionYstep(ii)

                    System.Threading.Thread.Sleep(200)

                    BeamResults = String.Format(Format, currentposition, beamData.D4SigmaX, beamData.D4SigmaY, beamData.CentroidX, beamData.CentroidY)
                    w.WriteLine(BeamResults)
                Next
                PositionXmean = PositionXmean / 10
                PositionYmean = PositionYmean / 10
                s = "The mean X coordinate of the Beam is: " + PositionXmean.ToString("0.0")
                s += ControlChars.CrLf + "The mean Y coordinate of the Beam is: " + PositionYmean.ToString("0.0")
                mMsgInfo.PostMessage(s)
                w.WriteLine("At position of " + currentposition.ToString("0.0000") + ", The mean X coordinate of the Beam is: " + PositionXmean.ToString("0.0"))
                w.WriteLine("At position of " + currentposition.ToString("0.0000") + ", The mean Y coordinate of the Beam is: " + PositionYmean.ToString("0.0"))

                BeamPositionX(i) = PositionXmean
                BeamPositionY(i) = PositionYmean
                StagePosition(i) = Steps * i * 1000


                If (i < CInt(Amounts)) Then
                    mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, Steps, True)
                End If


                'mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, Steps, True)
            Next


            Xmean(0) = 0
            Ymean(0) = 0
            Zmean(0) = 0
            XXmean(0) = 0
            XYmean(0) = 0
            XZmean(0) = 0


            For i = 0 To CInt(Amounts)
                Xmean(0) = Xmean(0) + StagePosition(i)
                Ymean(0) = Ymean(0) + BeamPositionX(i)
                Zmean(0) = Zmean(0) + BeamPositionY(i)
                XXmean(0) = XXmean(0) + StagePosition(i) * StagePosition(i)
                XYmean(0) = XYmean(0) + StagePosition(i) * BeamPositionX(i)
                XZmean(0) = XZmean(0) + StagePosition(i) * BeamPositionY(i)

            Next
            Xmean(0) /= (CInt(Amounts) + 1)
            Ymean(0) /= (CInt(Amounts) + 1)
            Zmean(0) /= (CInt(Amounts) + 1)
            XXmean(0) /= (CInt(Amounts) + 1)
            XYmean(0) /= (CInt(Amounts) + 1)
            XZmean(0) /= (CInt(Amounts) + 1)

            kx(0) = (XYmean(0) - Xmean(0) * Ymean(0)) / (XXmean(0) - Xmean(0) * Xmean(0))
            bx(0) = Ymean(0) - kx(0) * Xmean(0)
            ky(0) = (XZmean(0) - Zmean(0) * Xmean(0)) / (XXmean(0) - Xmean(0) * Xmean(0))
            by(0) = Zmean(0) - ky(0) * Xmean(0)

            s = "The fomula of Beam X position is y=" + kx(0).ToString("0.00") + "x+" + bx(0).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)

            s = "The fomula of Beam Y position is y=" + ky(0).ToString("0.00") + "x+" + by(0).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)


            w.WriteLine()
            w.WriteLine()

            w.WriteLine("Repeat Scanning By Moving The Stage Back To Its Origin Position")

            w.WriteLine()
            w.WriteLine()
            w.WriteLine(header2)

            PositionXmean = 0
            PositionYmean = 0

            For i = 0 To CInt(Amounts)
                currentposition = mStage.GetCurrentPosition(eAxis)
                PositionXmean = 0
                PositionYmean = 0
                For ii = 0 To 9


                    beamData = mInst.NanoScan.AcquireData(samples, mode)
                    s = "      Beam Info"
                    s += ControlChars.CrLf + "      Centroid Position X: " + beamData.CentroidX.ToString("0.0") + "um, Y: " + beamData.CentroidY.ToString("0.0") + "um"
                    mMsgInfo.PostMessage(s)

                    BeamPositionXstep(ii) = beamData.CentroidX
                    BeamPositionYstep(ii) = beamData.CentroidY
                    PositionXmean = PositionXmean + BeamPositionXstep(ii)
                    PositionYmean = PositionYmean + BeamPositionYstep(ii)

                    System.Threading.Thread.Sleep(200)

                    BeamResults = String.Format(Format, currentposition, beamData.D4SigmaX, beamData.D4SigmaY, beamData.CentroidX, beamData.CentroidY)
                    w.WriteLine(BeamResults)
                Next
                PositionXmean = PositionXmean / 10
                PositionYmean = PositionYmean / 10
                s = "The mean X coordinate of the Beam is: " + PositionXmean.ToString("0.0")
                s += ControlChars.CrLf + "The mean Y coordinate of the Beam is: " + PositionYmean.ToString("0.0")
                mMsgInfo.PostMessage(s)
                w.WriteLine("At position of " + currentposition.ToString("0.0000") + ", The mean X coordinate of the Beam is: " + PositionXmean.ToString("0.0"))
                w.WriteLine("At position of " + currentposition.ToString("0.0000") + ", The mean Y coordinate of the Beam is: " + PositionYmean.ToString("0.0"))

                BeamPositionX(i) = PositionXmean
                BeamPositionY(i) = PositionYmean
                StagePosition(i) = Steps * i * 1000

                If (i < CInt(Amounts)) Then
                    mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -Steps, True)
                End If

            Next


            Xmean(1) = 0
            Ymean(1) = 0
            Zmean(1) = 0
            XXmean(1) = 0
            XYmean(1) = 0
            XZmean(1) = 0


            For i = 0 To CInt(Amounts)
                Xmean(1) = Xmean(1) + StagePosition(i)
                Ymean(1) = Ymean(1) + BeamPositionX(i)
                Zmean(1) = Zmean(1) + BeamPositionY(i)
                XXmean(1) = XXmean(1) + StagePosition(i) * StagePosition(i)
                XYmean(1) = XYmean(1) + StagePosition(i) * BeamPositionX(i)
                XZmean(1) = XZmean(1) + StagePosition(i) * BeamPositionY(i)

            Next
            Xmean(1) /= (CInt(Amounts) + 1)
            Ymean(1) /= (CInt(Amounts) + 1)
            Zmean(1) /= (CInt(Amounts) + 1)
            XXmean(1) /= (CInt(Amounts) + 1)
            XYmean(1) /= (CInt(Amounts) + 1)
            XZmean(1) /= (CInt(Amounts) + 1)

            kx(1) = (XYmean(1) - Xmean(1) * Ymean(1)) / (XXmean(1) - Xmean(1) * Xmean(1))
            bx(1) = Ymean(1) - kx(1) * Xmean(1)
            ky(1) = (XZmean(1) - Zmean(1) * Xmean(1)) / (XXmean(1) - Xmean(1) * Xmean(1))
            by(1) = Zmean(1) - ky(1) * Xmean(1)

            s = "The fomula of Beam X position is y=" + kx(1).ToString("0.00") + "x+" + bx(1).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)

            s = "The fomula of Beam Y position is y=" + ky(1).ToString("0.00") + "x+" + by(1).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)
            s = "The X slope of forward movement is " + kx(0).ToString("0.00") + ". The X slope of backward movement is " + kx(1).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)
            s = "The average X slope is" + (kx(0) / 2 - kx(1) / 2).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)

            s = "The Y slope of forward movement is " + ky(0).ToString("0.00") + ". The Y slope of backward movement is " + ky(1).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)
            s = "The average Y slope is" + (ky(0) / 2 - ky(1) / 2).ToString("0.00")
            mMsgInfo.PostMessage(s)
            w.WriteLine(s)

            w.WriteLine("Complete!")

            w.Close()
            mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, originposition, True)
            Return True
        End Function


        Public Function CalculatePitch(ByVal samples As Integer, ByVal mode As iBeamProfiler.DataAcquisitionMode, ByVal nearPos As Double, ByVal farPos As Double) As Boolean
            Dim beamData As iBeamProfiler.SimpleData

            Dim i As Integer
            Dim Xpos(2), Ypos(2), Xmean(2), Ymean(2) As Double
            Dim s As String




            Dim eAxis As iXpsStage.AxisNameEnum
            eAxis = CType(3, iXpsStage.AxisNameEnum)

            s = "Move Beam to Near Position..."
            mMsgInfo.PostMessage(s)
            mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nearPos, True)

            For i = 0 To 9
                System.Threading.Thread.Sleep(200)
                beamData = mInst.NanoScan.AcquireData(samples, mode)
                Xmean(0) = Xmean(0) + beamData.CentroidX
                Ymean(0) = Ymean(0) + beamData.CentroidY

            Next
            Xpos(0) = Xmean(0) / 10
            Ypos(0) = Ymean(0) / 10
            s = "The mean X coordinate of Beam at near position is " + Xpos(0).ToString("0.00") + " um."
            s += ControlChars.CrLf + "The mean Y coordinate of Beam at near position is " + Ypos(0).ToString("0.00") + " um."
            s += ControlChars.CrLf + "Move Beam to Far Position..."
            mMsgInfo.PostMessage(s)
            mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nearPos, True)

            For i = 0 To 9
                System.Threading.Thread.Sleep(200)
                beamData = mInst.NanoScan.AcquireData(samples, mode)
                Xmean(1) = Xmean(1) + beamData.CentroidX
                Ymean(1) = Ymean(1) + beamData.CentroidY
            Next
            Xpos(1) = Xmean(1) / 10
            Ypos(1) = Ymean(1) / 10
            s = "The mean X coordinate of Beam at far position is " + Xpos(1).ToString("0.00") + " um."
            s += ControlChars.CrLf + "The mean Y coordinate of Beam at far position is " + Ypos(1).ToString("0.00") + " um."

            mMsgInfo.PostMessage(s)
            mTool.StageTool.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, farPos, True)

            Xpos(2) = Xpos(0) - 100 * (Xpos(1) - Xpos(0)) / (farPos - nearPos)
            Ypos(2) = Ypos(0) - 100 * (Ypos(1) - Ypos(0)) / (farPos - nearPos)



            Return True

        End Function




#End Region
    End Class

End Class
