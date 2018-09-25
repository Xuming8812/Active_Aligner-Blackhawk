Option Strict On
Option Explicit On
Option Infer Off

Public Class BlackHawkFunction
    Private mParent As fMain
    Private mFormImage As fImage

    Private mIniFile As w2.w2IniFile
    Private mParaFile As w2.w2IniFileXML

    Private mMsgInfo As w2.w2MessageService
    Private mMsgData As w2.w2MessageService

    Private mData As BlackHawkData
    Private mPara As BlackHawkParameters

    Public Function Initialize(ByVal hParent As fMain, ByRef hIniFile As w2.w2IniFile, ByRef hParaFile As w2.w2IniFileXML, ByRef hDataTool As BlackHawkData, hMsgInfo As w2.w2MessageService, ByRef hMsgData As w2.w2MessageService) As Boolean
        Dim success As Boolean

        mParent = hParent

        mData = hDataTool
        mIniFile = hIniFile
        mParaFile = hParaFile

        mMsgInfo = hMsgInfo
        mMsgData = hMsgData

        'get parameter - do this before instrument
        mMsgInfo.PostMessage("Read parameters from file ... ")
        mPara = New BlackHawkParameters(mParaFile)

        'instrument
        mMsgInfo.PostMessage("")
        mMsgInfo.PostMessage("Initialize instrument ... ")
        mInst = New InstrumentList(Me)
        success = mInst.InitializeAll()
        If Not success Then Return False

        'start tool class
        mStage = New StageFunctions(Me)
        mUtility = New InstrumentUtility(Me)

        Return True
    End Function

    Public Sub InitializeNewProcess()
        mPara.ClearAlignmentParameters()
        mStage.HaveSavedPosition = False
        mStage.ClearRecordedData()
    End Sub


    Public ReadOnly Property Parameter() As BlackHawkParameters
        Get
            Return mPara
        End Get
    End Property

    Public Property IsAutoLoadUnload() As Boolean
    Public Property AutoUnloadRequested As Boolean


#Region "Stop"
    Private mStop As Boolean
    Public Sub [Stop]()
        mStop = True
    End Sub

    Public Sub ClearStop()
        mStop = False
    End Sub

    Protected Function CheckStop() As Boolean
        Application.DoEvents()
        If mStop Then
            mMsgInfo.PostMessage("   Stopping process ... ")
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "data"
    Public Function SaveResultsToCsv() As Boolean
        Dim success As Boolean
        'Dim v As Double
        'Dim rowHead, rowContent, rowTail As String
        Dim stepName As String = ""

        'ack
        mMsgInfo.PostMessage("    Save results to the csv ... ")

        Dim fileName As String
        Dim columns(10) As String
        Dim csvTestData As w2CsvHelper
        fileName = mData.RootDataPath + "\Test_Data.csv"
        columns(0) = "Serial Number"
        columns(1) = "ItemName"
        columns(2) = "TestValue"
        columns(3) = "ResultDesc"
        columns(4) = "ResultStatus"
        columns(5) = "Step"
        columns(6) = "Lens Lot"
        columns(7) = "Epoxy Batch"
        columns(8) = "Operator"
        columns(9) = "Date"
        csvTestData = New w2CsvHelper(fileName, columns)

        'Try
        '    Select Case mTracking.Step
        '        Case KnownProcess.CollimatingLens
        '            stepName = "Collimating Lens"
        '        Case KnownProcess.AdjustmentLens
        '            stepName = "Adjustment Lens"
        '        Case KnownProcess.Offline
        '            stepName = "Offline"
        '        Case KnownProcess.Unknown
        '            stepName = "Unknown"
        '    End Select

        '    rowHead = mTracking.SerialNumber
        '    rowTail = stepName + "," + mTracking.LensLot + "," + mTracking.EpoxyBatch + "," + mTracking.Operator + "," + mTracking.Date.ToString("yyyy-MM-dd HH:mm:ss")

        '    'fill result
        '    v = mResult.AlignmentLocation.X
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "STAGE_POSITION_X" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.AlignmentLocation.Y
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "STAGE_POSITION_Y" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.AlignmentLocation.Z
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "STAGE_POSITION_Z" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.TecTemperature
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "CHIP_TEMPERATURE" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.ChipCurrent
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "CHIP_CURRENT" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.ChipCurrentPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.ChipVoltage
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "CHIP_VOLTAGE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.ChipVoltagePass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.PdBias
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "PD_BIAS" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.PdBiasPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.PdDarkCurrent
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "PD_DARK_CURRENT" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.PdDarkCurrentPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.PdCurrent(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "PD_CURRENT_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.PdCurrentPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.PdCurrent(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "PD_CURRENT_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.PdCurrentPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.PdCurrent(ProcessSteps.PostUvCure)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "PD_CURRENT_POST_UV_CURE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.PdCurrentPass(ProcessSteps.PostUvCure), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.IdealWX
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WAIST_X" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.IdealWXPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.IdealWY
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WAIST_Y" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.IdealWYPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamCenterX
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_CENTER_X" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamCenterXPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamCenterY
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_CENTER_Y" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamCenterYPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthX
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_X" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthXPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthY
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_Y" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthYPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamIntensity(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_INTENSITY_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamIntensityPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamIntensity(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_INTENSITY_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamIntensityPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamIntensity(ProcessSteps.PostUvCure)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_INTENSITY_POST_UV_CURE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamIntensityPass(ProcessSteps.PostUvCure), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.IdealDiff(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_Z_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.IdealDiffPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.IdealDiff(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_Z_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.IdealDiffPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.IdealDiff(ProcessSteps.PostUvCure)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_Z_POST_UV_CURE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.IdealDiffPass(ProcessSteps.PostUvCure), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    'Added by Alex per Huawei's data request 20131209
        '    v = mResult.InitialPositionAngleX
        '    If Not Double.IsNaN(v) And v <> 0 Then
        '        rowContent = "VGROOVE_ANGLE_X" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.InitialPositionOffset.X
        '    If Not Double.IsNaN(v) And v <> 0 Then
        '        rowContent = "VGROOVE_OFFSET_X" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.InitialPositionOffset.Y
        '    If Not Double.IsNaN(v) And v <> 0 Then
        '        rowContent = "VGROOVE_OFFSET_Y" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.ColLensPositionZ
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "STAGE_POSITION_Z_COL_LENS_FORCE" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.ColLensPositionZ_Retreat
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "STAGE_POSITION_Z_COL_LENS_RETREAT" + "," + v.ToString() + "," + " " + "," + "PASS"
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWaistAdjustmentSlope
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_ADJUSTMENT_SLOPE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWaistAdjustmentSlopePass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthNear(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_NEAR_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthNearPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthFar(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_FAR_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthFarPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthIdealY(ProcessSteps.Initial)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_IDEAL_Y_INITIAL" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthIdealYPass(ProcessSteps.Initial), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthDiffLo
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_DIFF_LO" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthDiffLoPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthDiffHi
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_DIFF_HI" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthDiffHiPass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthIdealZ(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_IDEAL_Z_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthIdealZPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthIdealY(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_IDEAL_Y_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthIdealYPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthNear(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_NEAR_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthNearPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.BeamWidthFar(ProcessSteps.PostEpoxyApplication)
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "BEAM_WIDTH_FAR_POST_UV_APPLICATION" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.BeamWidthFarPass(ProcessSteps.PostEpoxyApplication), "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.LensAngle
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "LENS_ANGLE_SIDE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.LensAnglePass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.SiobAngle1
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "SIOB_ANGLE1_SIDE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.SiobAngle1Pass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    v = mResult.SiobAngle2
        '    If Not Double.IsNaN(v) Then
        '        rowContent = "SIOB_ANGLE2_SIDE" + "," + v.ToString() + "," + " " + "," + CStr(IIf(mResult.SiobAngle2Pass, "PASS", "Fail"))
        '        csvTestData.AppendLine(rowHead + "," + rowContent + "," + rowTail)
        '    End If

        '    success = True

        'Catch ex As Exception
        '    mMsgInfo.PostMessage("X   Database error " + ex.Message)

        '    success = False
        'End Try


        'update data table
        ' csvTestData.Close()

        'return
        Return success
    End Function
#End Region

    Public Function WaitForTime(ByVal WaitTimeInSecond As Double, ByVal Message As String) As Boolean
        Dim T As Date
        Dim seconds As Double
        Dim s As String

        seconds = 0.0
        T = Date.Now
        While seconds < WaitTimeInSecond
            System.Threading.Thread.Sleep(100)
            If Me.CheckStop() Then Return False

            seconds = Date.Now.Subtract(T).TotalSeconds

            s = Message + " " + seconds.ToString("0.0") + " of " + WaitTimeInSecond.ToString("0.0") + " sec"
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
        End While

        Return True
    End Function

    Public Function ShowMessageBox(ByVal Text As String, ByVal caption As String, ByVal buttons As System.Windows.Forms.MessageBoxButtons, _
                                   ByVal icon As System.Windows.Forms.MessageBoxIcon, _
                                   Optional ByVal defaultButton As System.Windows.Forms.MessageBoxDefaultButton = MessageBoxDefaultButton.Button1 _
                                  ) As Windows.Forms.DialogResult
        Dim r As Windows.Forms.DialogResult
        mInst.XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Fail)
        r = MessageBox.Show(Text, caption, buttons, icon, defaultButton)
        mInst.XpsIO.SetPostLight(iXpsIO.PostLightStatusEnum.Running)

        Return r
    End Function
End Class
