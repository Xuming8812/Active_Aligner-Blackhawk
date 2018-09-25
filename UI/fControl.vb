Option Explicit On
Option Strict On

Public Class fControl
    Private Enum XpsColIndex
        Axis = 0
        Enabled = 1
        Position = 2
        Min = 3
        Max = 4

        NewPosition = 5
        RelativeMove = 6

        Move = 7
        Home = 8

        iStatus = 9
        sStatus = 10
    End Enum

    Private Enum HexapodColIndex
        Axis = 0
        Enabled = 1
        Position = 2
        Min = 3
        Max = 4

        NewPosition = 5
        RelativeMove = 6

        Move = 7
        Home = 8

        iStatus = 9
        sStatus = 10
    End Enum

    Private mXPS As Instrument.iXPS
    Private mHexapod As Instrument.iPiGCS2

    Private mXpsIO As iXpsIO
    Private mStageBase As iXpsStage
    Private mStageTool As BlackHawkFunction.StageFunctions
    Private mTool As BlackHawkFunction

    Private mDgvConfig As Control()

    Private mSync As Boolean
    Private mSettingHelper As w2ConfigTableHelper

    Private mStop As Boolean

    Public Sub New(ByRef hTool As BlackHawkFunction)
        InitializeComponent()

        mTool = hTool

        mXPS = mTool.Instruments.XPS
        mXpsIO = mTool.Instruments.XpsIO
        mHexapod = hTool.Instruments.Hexopod

        mStageBase = mTool.Instruments.StageBase
        mStageTool = mTool.StageTool

        mSettingHelper = New w2ConfigTableHelper(mTool.Parameter.IniFile.FileName, "SettingTableGUI.xml")

        'interface
        Me.SetupGUI()
        Me.SetupSettingGUI()
        Me.SetupXpsInterface()

        'load list
        Me.LoadConfiguredPositionList()
        Me.LoadHexapodConfiguredPositionList()

        'default page - read stage position
        tc.SelectTab(0)
        Me.SyncSignals()
        Me.SyncStagePositions()
        Me.SetStageTargetToCurrentPosition()

        Me.SyncHexapodPositions()
        Me.SetHexapodTargetToCurrentPosition()
    End Sub

#Region "GUI setup"
    Private Sub SetupGUI()
        Dim ctrl As New List(Of Control)
        Dim x As w2NumericUpDownHelper

        'flag
        mSync = True

        'buttons
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of Button)(Me, ctrl)
        For Each btn As Button In ctrl
            AddHandler btn.Click, AddressOf btn_Click
        Next

        'text box for readout
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of TextBox)(Me, ctrl)
        For Each txt As TextBox In ctrl
            'we need to eliminate the nud being collected as text box
            If TypeOf txt.Parent Is NumericUpDown Then Continue For
            txt.BackColor = System.Drawing.Color.FromArgb(235, 235, 235)
            txt.BorderStyle = BorderStyle.FixedSingle
            AddHandler txt.KeyDown, AddressOf txt_KeyDown
        Next

        'nud
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of NumericUpDown)(Me, ctrl)
        For Each nud As NumericUpDown In ctrl
            AddHandler nud.ValueChanged, AddressOf nud_ValueChanged
            x = New w2NumericUpDownHelper(nud, "ControllerGUI.ini")
            nud.BorderStyle = BorderStyle.FixedSingle
        Next

        'check
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of CheckBox)(Me, ctrl)
        For Each chk As CheckBox In ctrl
            AddHandler chk.CheckedChanged, AddressOf chk_CheckedChanged
            chk.FlatStyle = FlatStyle.Standard
        Next

        'radio button
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of RadioButton)(Me, ctrl)
        For Each opt As RadioButton In ctrl
            AddHandler opt.CheckedChanged, AddressOf opt_CheckedChanged
            opt.FlatStyle = FlatStyle.Standard
        Next

        'on/off control
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of ucOnOff)(Me, ctrl)
        For Each oo As ucOnOff In ctrl
            oo.State = ucOnOff.StateEnum.Unknown
            AddHandler oo.StateChanged, AddressOf oo_StateChanged
        Next

        'dgv - error handling is done in the help class
        AddHandler dgvXPS.DataError, AddressOf dgv_DataError
        'ctrl.Clear()
        'w2.w2Misc.GetAllControls(Of DataGridView)(Me, ctrl)
        'For Each dgv As DataGridView In ctrl
        '    AddHandler dgv.DataError, AddressOf dgv_DataError
        'Next

        'list for the gold image
        lstGoldImage.SelectionMode = SelectionMode.One
        lstGoldImage.Items.Clear()
        lstGoldImage.Items.AddRange(w2ComboListItem.BuildList(GetType(BlackHawkFunction.InstrumentUtility.CcdViewIndex)))

        Me.lstConfiguredPositions.Size = New Size(195, Me.Height - 115)
        Me.GroupBox6.Location = New Point(6, Me.dgvXPS.Location.Y + Me.dgvXPS.Height + 15)
        'done
        mSync = False
    End Sub

    Public Sub SetupXpsInterface()
        Dim i As Integer

        With dgvXPS
            .Left = 10
            .Width = .Parent.ClientRectangle.Width - .Left - 10
            '.Height = .Parent.ClientRectangle.Height - .Top - 10
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom

            .EditMode = DataGridViewEditMode.EditOnEnter

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .ReadOnly = False

            .RowHeadersVisible = False
            .RowCount = mStageBase.StageData.Length
            .ColumnCount = [Enum].GetValues(GetType(XpsColIndex)).Length

            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("Airal", 10, FontStyle.Bold)

            .Columns(XpsColIndex.Axis).HeaderText = "Axis"
            .Columns(XpsColIndex.Axis).ReadOnly = True

            .Columns.RemoveAt(XpsColIndex.Enabled)
            .Columns.Insert(XpsColIndex.Enabled, New DataGridViewCheckBoxColumn)
            .Columns(XpsColIndex.Enabled).HeaderText = "Enable"

            .Columns(XpsColIndex.Position).HeaderText = "Position" + ControlChars.CrLf + "(mm)"
            .Columns(XpsColIndex.Position).ReadOnly = True
            .Columns(XpsColIndex.Position).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(XpsColIndex.Min).HeaderText = "Limit" + ControlChars.CrLf + "Min (mm)"
            .Columns(XpsColIndex.Min).ReadOnly = True
            .Columns(XpsColIndex.Min).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(XpsColIndex.Max).HeaderText = "Limit" + ControlChars.CrLf + "Max (mm)"
            .Columns(XpsColIndex.Max).ReadOnly = True
            .Columns(XpsColIndex.Max).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns.RemoveAt(XpsColIndex.RelativeMove)
            .Columns.Insert(XpsColIndex.RelativeMove, New DataGridViewCheckBoxColumn)
            .Columns(XpsColIndex.RelativeMove).HeaderText = "Move" + ControlChars.CrLf + "Relative"

            .Columns(XpsColIndex.NewPosition).HeaderText = "Target" + ControlChars.CrLf + "(mm)"
            .Columns(XpsColIndex.NewPosition).ValueType = GetType(Double)
            .Columns(XpsColIndex.NewPosition).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns.RemoveAt(XpsColIndex.Home)
            Dim x As New DataGridViewImageColumn
            x.Image = img.Images("Home")
            .Columns.Insert(XpsColIndex.Home, x)
            .Columns(XpsColIndex.Home).HeaderText = "Init" + ControlChars.CrLf + "Home"

            .Columns.RemoveAt(XpsColIndex.Move)
            Dim y As New DataGridViewImageColumn
            y.Image = img.Images("Move")
            .Columns.Insert(XpsColIndex.Move, y)
            .Columns(XpsColIndex.Move).HeaderText = "Move"

            .Columns(XpsColIndex.iStatus).HeaderText = "Status" + ControlChars.CrLf + "Code"
            .Columns(XpsColIndex.iStatus).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(XpsColIndex.sStatus).HeaderText = "Status Text"

            For i = 0 To .ColumnCount - 1
                .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            For i = 0 To mStageBase.StageData.Length - 1
                If TypeOf mStageBase.StageData(i).Controller Is Instrument.iXPS Then
                    .Item(XpsColIndex.Axis, i).Value = mStageBase.GetStageName(CType(i, iXpsStage.AxisNameEnum))
                    .Item(XpsColIndex.Min, i).Value = mStageBase.StageData(i).LimitLo.ToString("0.000")
                    .Item(XpsColIndex.Max, i).Value = mStageBase.StageData(i).LimitHi.ToString("0.000")
                End If
            Next

            '.Columns(.ColumnCount - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .AutoResizeColumns()
            .AutoResizeRows()

            .ScrollBars = ScrollBars.Both
            'do not show selection
            .MultiSelect = False

            .Enabled = (mXPS IsNot Nothing)
        End With


    End Sub
#End Region

#Region "Sync"
    Public Sub Sync()
        Me.SyncSignals()
        Me.SyncStagePositions()
        SyncInstruments()
    End Sub

    Private Sub SyncSignals()
        Dim v As Double
        Dim i, channel As Integer

        mSync = True

        'LDD
        If mTool.Instruments.LDD IsNot Nothing Then
            With mTool.Instruments.LDD
                channel = 0
                For i = 1 To 4
                    If .Current(i) <> 0 Then channel = i
                Next

                If channel <> 0 Then
                    optCH1.Checked = (channel = 1)
                    optCH2.Checked = (channel = 2)
                    optCH3.Checked = (channel = 3)
                    optCH4.Checked = (channel = 4)

                    v = .Current(channel)
                    nudCurrent.Value = Convert.ToDecimal(v)
                    txtVoltage.Text = .Voltage(channel).ToString("0.00")
                Else
                    'ALL channels are off, we will just do nothing
                    nudCurrent.Value = 0
                    txtVoltage.Text = ""
                End If

                nudTemperature.Value = Convert.ToDecimal(.TemperatureSetpoint)
                txtTemperature.Text = .TemperatureReading.ToString("0.00")

                chkLddProtection.Checked = .EnabledProtectionState
            End With
        End If

        'XPS IO
        If mXpsIO.HaveController Then
            'vacuum

            chkVacuumPackage.Checked = mXpsIO.VacuumEnabledReadback(iXpsIO.VacuumLine.Package)

            chkVacuumLens.Checked = mXpsIO.VacuumEnabled(iXpsIO.VacuumLine.Main)
            chkVacuumPbs.Checked = mXpsIO.VacuumEnabled(iXpsIO.VacuumLine.Hexapod)

            chkVacuumCda.Checked = mXpsIO.VacuumLinePressurized

            txtVacuumLens.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Main).ToString("0.0")
            txtVacuumPbs.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Hexapod).ToString("0.0")

            txtVacuumLine.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.LineInput).ToString("0.0")

            'CDA
            txtCdaLine.Text = mXpsIO.CompressAirPressure.ToString("0.000")
            chkEpoxy.Checked = mXpsIO.EpoxyMoveOut

            chkProbeCdaOn.Checked = mXpsIO.ProbePositionOn

            chkGPIO3_1.Checked = mXPS.DigitOutput(3, 0)
            chkGPIO3_2.Checked = mXPS.DigitOutput(3, 1)
            chkGPIO3_3.Checked = mXPS.DigitOutput(3, 2)
            chkGPIO3_4.Checked = mXPS.DigitOutput(3, 3)
            chkGPIO3_5.Checked = mXPS.DigitOutput(3, 4)
            chkGPIO3_6.Checked = mXPS.DigitOutput(3, 5)
        End If

        'probe
        If mTool.Instruments.ProbeClamp IsNot Nothing Then
            chkProbeClampEnable.Checked = mTool.Instruments.ProbeClamp.Enabled
            chkProbeClampClose.Checked = Not mTool.Utility.IsProbeClampOpen()
        End If

        'force gauge
        'txtForceLens.Text = mTool.Utility.GetForceGuageReading(iXpsStage.StageEnum.Main, 5).ToString("0")
        'txtForcePbs.Text = mTool.Utility.GetForceGuageReading(iXpsStage.StageEnum.Hexapod, 5).ToString("0")

        mSync = False
    End Sub

    Private Sub SyncStagePositions()
        mSync = True

        'stage
        If mXPS Is Nothing Then
            'disable all the buttons? 
        Else
            'raw stage info
            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.PiLS) Then
                txtPiLS.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.PiLS).ToString("0.0000")
                btnMovePiLS.Enabled = True
                btnJogAddPiLS.Enabled = True
                btnJogSubPiLS.Enabled = True
            Else
                'txtAngle.Text = "N/A"
                'txtAngle.Enabled = False
                btnMovePiLS.Enabled = False
                btnJogAddPiLS.Enabled = False
                btnJogSubPiLS.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.AngleMain) Then
                txtAngleLens.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain).ToString("0.0000")
                btnMoveAngleLens.Enabled = True
                btnJogAddAngleLens.Enabled = True
                btnJogSubAngleLens.Enabled = True
            Else
                'txtAngle.Text = "N/A"
                'txtAngle.Enabled = False
                btnMoveAngleLens.Enabled = False
                btnJogAddAngleLens.Enabled = False
                btnJogSubAngleLens.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.AngleHexapod) Then
                txtAnglePbs.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.AngleHexapod).ToString("0.0000")
                btnMoveAnglePbs.Enabled = True
                btnJogAddAnglePbs.Enabled = True
                btnJogSubAnglePbs.Enabled = True
            Else
                'txtAngle.Text = "N/A"
                'txtAngle.Enabled = False
                btnMoveAnglePbs.Enabled = False
                btnJogAddAnglePbs.Enabled = False
                btnJogSubAnglePbs.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.Probe) Then
                txtProbe.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.Probe).ToString("0.0000")
                'txtProbe2.Text = txtProbe.Text
                btnMoveProbe.Enabled = True
                btnJogAddProbe.Enabled = True
                btnJogSubProbe.Enabled = True
            Else
                'txtBeamGage.Text = "N/A"
                'txtBeamGage.Enabled = False
                btnMoveProbe.Enabled = False
                btnJogAddProbe.Enabled = False
                btnJogSubProbe.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.BeamScanX) Then
                txtBeamScanX.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX).ToString("0.0000")
                btnMoveBeamScanX.Enabled = True
                btnJogAddBeamScanX.Enabled = True
                btnJogSubBeamScanX.Enabled = True
            Else
                'txtPigtailX.Text = "N/A"
                'txtPigtailX.Enabled = False
                btnMoveBeamScanX.Enabled = False
                btnJogAddBeamScanX.Enabled = False
                btnJogSubBeamScanX.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.BeamScanY) Then
                txtBeamScanY.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanY).ToString("0.0000")
                btnMoveBeamScanY.Enabled = True
                btnJogAddBeamScanY.Enabled = True
                btnJogSubBeamScanY.Enabled = True
            Else
                'txtPigtailY.Text = "N/A"
                'txtPigtailY.Enabled = False
                btnMoveBeamScanY.Enabled = False
                btnJogAddBeamScanY.Enabled = False
                btnJogSubBeamScanY.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.BeamScanZ) Then
                txtBeamScanZ.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanZ).ToString("0.0000")
                btnMoveBeamScanZ.Enabled = True
                btnJogAddBeamScanZ.Enabled = True
                btnJogSubBeamScanZ.Enabled = True
            Else
                'txtPigtailZ.Text = "N/A"
                'txtPigtailZ.Enabled = False
                btnMoveBeamScanZ.Enabled = False
                btnJogAddBeamScanZ.Enabled = False
                btnJogSubBeamScanZ.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.StageX) Then
                txtStageX.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.StageX).ToString("0.0000")
                btnMoveStageX.Enabled = True
                btnJogAddStageX.Enabled = True
                btnJogSubStageX.Enabled = True
            Else
                'txtStageX.Text = "N/A"
                'txtStageX.Enabled = False
                btnMoveStageX.Enabled = False
                btnJogAddStageX.Enabled = False
                btnJogSubStageX.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.StageY) Then
                txtStageY.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.StageY).ToString("0.0000")
                btnMoveStageY.Enabled = True
                btnJogAddStageY.Enabled = True
                btnJogSubStageY.Enabled = True
            Else
                'txtStageY.Text = "N/A"
                txtStageY.Enabled = False
                btnMoveStageY.Enabled = False
                btnJogAddStageY.Enabled = False
                btnJogSubStageY.Enabled = False
            End If

            If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.StageZ) Then
                txtStageZ.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.StageZ).ToString("0.0000")
                btnMoveStageZ.Enabled = True
                btnJogAddStageZ.Enabled = True
                btnJogSubStageZ.Enabled = True
            Else
                'txtStageZ.Text = "N/A"
                txtStageZ.Enabled = False
                btnMoveStageZ.Enabled = False
                btnJogAddStageZ.Enabled = False
                btnJogSubStageZ.Enabled = False
            End If

            'If mStageBase.IsStageReady(iXpsStage.AxisNameEnum.Probe) Then
            '    txtClampPos.Text = mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.Probe).ToString("0.0000")
            '    btnMoveProbe.Enabled = True
            '    btnJogAddProbe.Enabled = True
            '    btnJogSubProbe.Enabled = True
            'Else
            '    txtClampPos.Enabled = False
            '    btnMoveProbe.Enabled = False
            '    btnJogAddProbe.Enabled = False
            '    btnJogSubProbe.Enabled = False
            'End If

            'check actual/target 
            Me.SyncStageTargetColor()

        End If

        mSync = False
    End Sub

    Private Sub SyncStageTargetColor()
        Me.SetStageTargetColor(txtStageX, nudStageX)
        Me.SetStageTargetColor(txtStageY, nudStageY)
        Me.SetStageTargetColor(txtStageZ, nudStageZ)

        Me.SetStageTargetColor(txtBeamScanX, nudBeamScanX)
        Me.SetStageTargetColor(txtBeamScanY, nudBeamScanY)
        Me.SetStageTargetColor(txtBeamScanZ, nudBeamScanZ)

        Me.SetStageTargetColor(txtAngleLens, nudAngleLens)

        Me.SetStageTargetColor(txtPiLS, nudPiLS)

        Me.SetStageTargetColor(txtProbe, nudProbe)
    End Sub

    Private Sub SyncInstruments()
        Dim s As String
UVSpecial:
        Try
            If mTool.Instruments.UvLamp Is Nothing Then
                btnUV.Enabled = False
                'lblError.Text = "No UV lamp"
            Else
                If mTool.Instruments.UvLamp.LampOn Then
                    ooUvLamp.State = ucOnOff.StateEnum.On
                Else
                    ooUvLamp.State = ucOnOff.StateEnum.Off
                End If

                nudUvTime.Value = Convert.ToDecimal(mTool.Instruments.UvLamp.ExposureTime)
                System.Threading.Thread.Sleep(200)
                nudUvPower.Value = Convert.ToDecimal(mTool.Instruments.UvLamp.PowerLevel)

                s = ""
                If mTool.Instruments.UvLamp.AlarmOn Then s += "Alarm on, "
                If Not mTool.Instruments.UvLamp.LampOn Then s += "Lamp off, "
                If Not mTool.Instruments.UvLamp.LampReady Then s += "Lamp not ready, "
                If mTool.Instruments.UvLamp.NeedCalibration Then s += "Need calibration, "
                If s.EndsWith(",") Then s = s.Substring(0, s.Length - 1)

                If s <> "" Then
                    'lblUvAlarm.BackColor = Color.Yellow
                    'lblUvAlarm.ForeColor = Color.Red
                    'lblUvAlarm.Text = "Alarm On"
                    mStageTool.MessageService.PostMessage("UV Lamp Error : " + s)
                Else
                    'lblUvAlarm.Visible = False
                End If
            End If
        Catch ex As Exception
            mTool.Instruments.UvLamp = Nothing
            GoTo UVSpecial
        End Try
    End Sub

    Private Sub SetStageTargetColor(ByRef txt As TextBox, ByVal nud As NumericUpDown)
        Dim s As String
        Dim v As Decimal

        Const Tol As Double = 0.001

        s = txt.Text.Trim()
        If s = "" Then
            nud.BackColor = SystemColors.Window
        Else
            v = CType(s, Decimal)
            If Math.Abs(v - nud.Value) < Tol Then
                nud.BackColor = SystemColors.Window
            Else
                nud.BackColor = Color.LightPink
            End If
        End If

    End Sub

    Private Sub SetStageTargetToCurrentPosition()
        Dim s As String

        s = txtStageX.Text : If s <> "" Then nudStageX.Value = CType(s, Decimal)
        s = txtStageY.Text : If s <> "" Then nudStageY.Value = CType(s, Decimal)
        s = txtStageZ.Text : If s <> "" Then nudStageZ.Value = CType(s, Decimal)

        s = txtBeamScanX.Text : If s <> "" Then nudBeamScanX.Value = CType(s, Decimal)
        s = txtBeamScanY.Text : If s <> "" Then nudBeamScanY.Value = CType(s, Decimal)
        s = txtBeamScanZ.Text : If s <> "" Then nudBeamScanZ.Value = CType(s, Decimal)

        s = txtAngleLens.Text : If s <> "" Then nudAngleLens.Value = CType(s, Decimal)
        s = txtAnglePbs.Text : If s <> "" Then nudAnglePbs.Value = CType(s, Decimal)

        s = txtPiLS.Text : If s <> "" Then nudPiLS.Value = CType(s, Decimal)

        s = txtProbe.Text : If s <> "" Then nudProbe.Value = CType(s, Decimal)
    End Sub

    Private Sub SyncXPStable()
        Dim r As Integer

        If mXPS Is Nothing Then Return

        For r = 0 To dgvXPS.RowCount - 1
            Me.SyncXpsTableOneRow(r)
        Next r

        dgvXPS.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

    End Sub

    Private Sub SyncXpsTableOneRow(ByVal RowIndex As Integer)
        Dim c, iStatus As Integer
        Dim cell As DataGridViewCell

        If mXPS Is Nothing Then Return

        mSync = True

        Me.Cursor = Cursors.WaitCursor

        mXPS.Axis = mStageBase.StageData(RowIndex).Axis

        With dgvXPS
            iStatus = mXPS.StatusCode
            If mXPS.LastError = "" And mStageBase.StageData(RowIndex).Installed Then
                .Rows(RowIndex).ReadOnly = False

                .Item(XpsColIndex.iStatus, RowIndex).Value = iStatus
                .Item(XpsColIndex.sStatus, RowIndex).Value = mXPS.StatusString
                .Item(XpsColIndex.Enabled, RowIndex).Value = mXPS.DriveEnabled

                .Item(XpsColIndex.Position, RowIndex).Value = mStageBase.GetStagePosition(CType(RowIndex, iXpsStage.AxisNameEnum)).ToString("0.000000")

            Else
                .Rows(RowIndex).ReadOnly = True

                .Item(XpsColIndex.Axis, RowIndex).Value = "N/A"
                For c = 1 To .ColumnCount - 1
                    cell = .Item(c, RowIndex)
                    If TypeOf cell Is DataGridViewTextBoxCell Then
                        cell.Value = ""
                    ElseIf TypeOf cell Is DataGridViewCheckBoxCell Then
                        cell.Value = False
                    Else
                        'do nothing for image and button cells
                    End If
                Next
            End If

            .AutoResizeRow(RowIndex, DataGridViewAutoSizeRowMode.AllCells)
        End With

        Me.Cursor = Cursors.Default
        mSync = False
    End Sub
#End Region

#Region "Sync Hexapod"
    Public Sub SyncHexapod()
        Me.SyncHexapodPositions()
    End Sub

    Private Sub SyncHexapodPositions()
        mSync = True

        'stage
        If mHexapod Is Nothing Then
            'disable all the buttons? 
        Else

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.X
            txtHexapodX.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodX.Enabled = True
            btnJogAddHexapodX.Enabled = True
            btnJogSubHexapodX.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.Y
            txtHexapodY.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodY.Enabled = True
            btnJogAddHexapodY.Enabled = True
            btnJogSubHexapodY.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.Z
            txtHexapodZ.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodZ.Enabled = True
            btnJogAddHexapodZ.Enabled = True
            btnJogSubHexapodZ.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.U
            txtHexapodU.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodU.Enabled = True
            btnJogAddHexapodU.Enabled = True
            btnJogSubHexapodU.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.V
            txtHexapodV.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodV.Enabled = True
            btnJogAddHexapodV.Enabled = True
            btnJogSubHexapodV.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.W
            txtHexapodW.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveHexapodW.Enabled = True
            btnJogAddHexapodW.Enabled = True
            btnJogSubHexapodW.Enabled = True

            'check actual/target 
            Me.SyncStageTargetColor()

        End If

        mSync = False
    End Sub

    Private Sub SyncHexapodTargetColor()
        Me.SetStageTargetColor(txtHexapodX, nudHexapodX)
        Me.SetStageTargetColor(txtHexapodY, nudHexapodY)
        Me.SetStageTargetColor(txtHexapodZ, nudHexapodZ)

        Me.SetStageTargetColor(txtHexapodU, nudHexapodU)
        Me.SetStageTargetColor(txtHexapodV, nudHexapodV)
        Me.SetStageTargetColor(txtHexapodW, nudHexapodW)

    End Sub

    Private Sub SetHexapodTargetColor(ByRef txt As TextBox, ByVal nud As NumericUpDown)
        Dim s As String
        Dim v As Decimal

        Const Tol As Double = 0.001

        s = txt.Text.Trim()
        If s = "" Then
            nud.BackColor = SystemColors.Window
        Else
            v = CType(s, Decimal)
            If Math.Abs(v - nud.Value) < Tol Then
                nud.BackColor = SystemColors.Window
            Else
                nud.BackColor = Color.LightPink
            End If
        End If

    End Sub

    Private Sub SetHexapodTargetToCurrentPosition()
        Dim s As String

        s = txtHexapodX.Text : If s <> "" Then nudHexapodX.Value = CType(s, Decimal)
        s = txtHexapodY.Text : If s <> "" Then nudHexapodY.Value = CType(s, Decimal)
        s = txtHexapodZ.Text : If s <> "" Then nudHexapodZ.Value = CType(s, Decimal)

        s = txtHexapodU.Text : If s <> "" Then nudHexapodU.Value = CType(s, Decimal)
        s = txtHexapodV.Text : If s <> "" Then nudHexapodV.Value = CType(s, Decimal)
        s = txtHexapodW.Text : If s <> "" Then nudHexapodW.Value = CType(s, Decimal)

    End Sub

#End Region

#Region "XPS direct control"
    Private Sub HomeStage(ByVal RowIndex As Integer)
        Dim success As Boolean

        mXPS.Axis = mStageBase.StageData(RowIndex).Axis

        If mXPS.StageReady() Then Return

        success = mXPS.InitializeMotion()
        If success Then success = mXPS.HomeNoWait()

        If success Then
            mStageTool.WaitStageToStop("Wait stage homing to finish", True)
        Else
            mStageTool.MessageService.PostMessage("X   Motor Error: " + mXPS.LastError)
        End If
    End Sub

    Private Sub MoveStageDirect(ByVal RowIndex As Integer)
        Dim x As Object
        Dim success As Boolean
        Dim Action As Instrument.iMotionController.MoveToTargetMethodEnum
        Dim Amount As Double

        x = dgvXPS.Item(XpsColIndex.NewPosition, RowIndex).Value()
        If x Is Nothing Then
            mStageTool.MessageService.PostMessage("X   Please enter the target position!")
            Return
        End If

        Amount = Convert.ToDouble(x)
        mXPS.Axis = mStageBase.StageData(RowIndex).Axis
        If CType(dgvXPS.Item(XpsColIndex.RelativeMove, RowIndex).Value, Boolean) Then
            Action = Instrument.iMotionController.MoveToTargetMethodEnum.Relative
        Else
            Action = Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
        End If
        success = mStageBase.MoveStageNoWait(CType(RowIndex, iXpsStage.AxisNameEnum), Action, Amount)

        'monitor the motion process
        If success Then
            mStageTool.WaitStageToStop("Wait stage homing to finish", True)
        Else
            mStageTool.MessageService.PostMessage("X   Motor Error: " + mXPS.LastError)
        End If

        'sync
        Me.SyncXpsTableOneRow(RowIndex)
    End Sub
#End Region

#Region "Configured position management"
    Public Sub LoadConfiguredPositionList()
        Dim s As String
        Dim x As iXpsStage.ConfiguredStagePosition

        'build the configured position list
        lstConfiguredPositions.SelectionMode = SelectionMode.One
        lstConfiguredPositions.Items.Clear()
        For Each x In mStageBase.ConfiguredPositions.Values
            'we will not load the safe position to GUI to avoid accidental changes
            s = x.Label.ToLower()
            If s.Contains("safe") Then Continue For
            lstConfiguredPositions.Items.Add(x)
        Next
        lstConfiguredPositions.SelectedIndex = 0
    End Sub

    Private Sub SaveConfiguredPositions(ByVal NewEntry As Boolean)
        Dim s, Label As String
        Dim i, ii As Integer
        Dim r As Windows.Forms.DialogResult
        Dim Positions(iXpsStage.AxisCount - 1) As Double
        Dim x, x2 As iXpsStage.ConfiguredStagePosition

        'get currently displayed positions
        Positions(iXpsStage.AxisNameEnum.StageX) = Val(txtStageX.Text)
        Positions(iXpsStage.AxisNameEnum.StageY) = Val(txtStageY.Text)
        Positions(iXpsStage.AxisNameEnum.StageZ) = Val(txtStageZ.Text)

        Positions(iXpsStage.AxisNameEnum.BeamScanX) = Val(txtBeamScanX.Text)
        Positions(iXpsStage.AxisNameEnum.BeamScanY) = Val(txtBeamScanY.Text)
        Positions(iXpsStage.AxisNameEnum.BeamScanZ) = Val(txtBeamScanZ.Text)

        Positions(iXpsStage.AxisNameEnum.AngleMain) = Val(txtAngleLens.Text)
        Positions(iXpsStage.AxisNameEnum.AngleHexapod) = Val(txtAnglePbs.Text)
        Positions(iXpsStage.AxisNameEnum.PiLS) = Val(txtPiLS.Text)

        'get label
        If NewEntry Then
            'get label
            Label = InputBox("Please enter the label for this position configuration", "Configured Position")
            Label = Label.Trim()
            If Label = "" Then Return
            'make sure label is unique
            If mStageBase.HaveConfiguredPosition(Label) Then
                s = "The label [" + Label + "] is already used. Do you want to overwrite the original position?"
                r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If r = Windows.Forms.DialogResult.No Then Return
            End If

        Else
            'get label
            i = lstConfiguredPositions.SelectedIndex
            If i = -1 Then
                s = "No current position selected. Please use Save As New to save a new configured position!"
                MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Label = CType(lstConfiguredPositions.Items(i), iXpsStage.ConfiguredStagePosition).Label
        End If

        'build a new class
        x = New iXpsStage.ConfiguredStagePosition(Label, Positions)

        're-ask the question to confirm 
        If mStageBase.HaveConfiguredPosition(Label) Then
            s = "We are going to overwrite the configured position for " + Label + ". Do you want to continue?"
            r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = Windows.Forms.DialogResult.No Then Return
            'update the new value
            mStageBase.UpdateConfiguredPosition(x)
            'update the new value to the list box
            ii = lstConfiguredPositions.Items.Count - 1
            For i = 0 To ii
                x2 = CType(lstConfiguredPositions.Items(i), iXpsStage.ConfiguredStagePosition)
                If x2.Label = x.Label Then
                    lstConfiguredPositions.Items(i) = x
                    Exit For
                End If
            Next
        Else
            'add new 
            mStageBase.AddConfiguredPosition(x)
            'add new item to the list
            lstConfiguredPositions.Items.Add(x)
        End If

        'commit this to config file
        mStageBase.SaveConfiguredPositions()

    End Sub

    Private Sub DeleteConfiguredPositions()
        Dim s, Label As String
        Dim i, ii As Integer
        Dim r As Windows.Forms.DialogResult
        Dim Positions(iXpsStage.AxisCount - 1) As Double
        Dim x, x2 As iXpsStage.ConfiguredStagePosition

        i = lstConfiguredPositions.SelectedIndex
        Label = CType(lstConfiguredPositions.Items(i), iXpsStage.ConfiguredStagePosition).Label

        s = "We are going to delete the configured position for " + Label + ". Do you want to continue?"
        r = MessageBox.Show(s, "Delete Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If r = Windows.Forms.DialogResult.No Then Return

        lstConfiguredPositions.Items.Remove(i)

        mStageBase.SaveConfiguredPositions()


    End Sub

    Public Sub LoadHexapodConfiguredPositionList()
        Dim s As String
        Dim x As iXpsStage.ConfiguredStagePosition

        'build the configured position list
        lstHexapodConfiguredPositions.SelectionMode = SelectionMode.One
        lstHexapodConfiguredPositions.Items.Clear()
        For Each x In mStageBase.HexapodConfiguredPositions.Values
            'we will not load the safe position to GUI to avoid accidental changes
            s = x.Label.ToLower()
            If s.Contains("safe") Then Continue For
            lstHexapodConfiguredPositions.Items.Add(x)
        Next
        lstHexapodConfiguredPositions.SelectedIndex = 0
    End Sub

    Private Sub SaveHexapodConfiguredPositions(ByVal NewEntry As Boolean)
        Dim s, Label As String
        Dim i, ii As Integer
        Dim r As Windows.Forms.DialogResult
        Dim Positions(iXpsStage.AxisCount - 1) As Double
        Dim x, x2 As iXpsStage.ConfiguredStagePosition

        'get currently displayed positions
        Positions(Instrument.iPiGCS2.AxisEnum.X) = Val(txtHexapodX.Text)
        Positions(Instrument.iPiGCS2.AxisEnum.Y) = Val(txtHexapodY.Text)
        Positions(Instrument.iPiGCS2.AxisEnum.Z) = Val(txtHexapodZ.Text)

        Positions(Instrument.iPiGCS2.AxisEnum.U) = Val(txtHexapodU.Text)
        Positions(Instrument.iPiGCS2.AxisEnum.V) = Val(txtHexapodV.Text)
        Positions(Instrument.iPiGCS2.AxisEnum.W) = Val(txtHexapodW.Text)

        'get label
        If NewEntry Then
            'get label
            Label = InputBox("Please enter the label for this position configuration", "Configured Position")
            Label = Label.Trim()
            If Label = "" Then Return
            'make sure label is unique
            If mStageBase.HaveConfiguredPosition(Label) Then
                s = "The label [" + Label + "] is already used. Do you want to overwrite the original position?"
                r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                If r = Windows.Forms.DialogResult.No Then Return
            End If

        Else
            'get label
            i = lstHexapodConfiguredPositions.SelectedIndex
            If i = -1 Then
                s = "No current position selected. Please use Save As New to save a new configured position!"
                MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Label = CType(lstHexapodConfiguredPositions.Items(i), iXpsStage.ConfiguredStagePosition).Label
        End If

        'build a new class
        x = New iXpsStage.ConfiguredStagePosition(Label, Positions)

        're-ask the question to confirm 
        If mStageBase.HaveHexapodConfiguredPosition(Label) Then
            s = "We are going to overwrite the configured position for " + Label + ". Do you want to continue?"
            r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = Windows.Forms.DialogResult.No Then Return
            'update the new value
            mStageBase.UpdateHexapodConfiguredPosition(x)
            'update the new value to the list box
            ii = lstHexapodConfiguredPositions.Items.Count - 1
            For i = 0 To ii
                x2 = CType(lstHexapodConfiguredPositions.Items(i), iXpsStage.ConfiguredStagePosition)
                If x2.Label = x.Label Then
                    lstHexapodConfiguredPositions.Items(i) = x
                    Exit For
                End If
            Next
        Else
            'add new 
            mStageBase.AddHexapodConfiguredPosition(x)
            'add new item to the list
            lstHexapodConfiguredPositions.Items.Add(x)
        End If

        'commit this to config file
        mStageBase.SaveHexapodConfiguredPositions()

    End Sub

#End Region

#Region "Gold Image"
    Private Sub SaveGoldImgae()
        Dim x As w2ComboListItem

        If lstGoldImage.SelectedIndex = -1 Then
            MessageBox.Show("Please select the image type first before save")
            Return
        End If

        x = CType(lstGoldImage.SelectedItem, w2ComboListItem)
        mTool.Utility.SaveGoldImageData(CType(x.Value, BlackHawkFunction.InstrumentUtility.CcdViewIndex))

    End Sub
#End Region

#Region "GUI callback"
    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button

        btn = CType(sender, Button)

        'prevent double hits
        RemoveHandler btn.Click, AddressOf btn_Click

        'clear stop
        If btn.Name <> btnStopMove.Name Then mTool.ClearStop()

        Select Case btn.Name
            '---------------------------------------misc 
            Case btnEpoxyTrigger.Name
                mXpsIO.EpoxyTriggerOnce()


            Case btnVacuumCda.Name
                mTool.Utility.CleanVacuumLineWithCda(True, 2.0)

            Case btnSaveGoldImage.Name
                Me.SaveGoldImgae()

                '--------------------------------------
            Case btnSaveSettings.Name
                Me.SaveSettings()

            Case btnSaveConfiguredPosition.Name
                Me.SaveConfiguredPositions(False)

            Case btnDeleteConfiguredPosition.Name
                'Me.DeleteConfiguredPositions()

            Case btnSaveConfiguredPositionNew.Name
                Me.SaveConfiguredPositions(True)

            Case btnXPS.Name
                If mXPS Is Nothing Then Return
                System.Diagnostics.Process.Start("http://" + mXPS.IPAddress)

            Case btnPiC843.Name
                mTool.Instruments.PiAngle.OpenEditDialog()

            Case btnStopMove.Name
                mTool.Stop()

            Case btnSync.Name
                Me.SyncStagePositions()
                Me.SyncSignals()
                Me.SyncInstruments()

            Case btnSyncXPS.Name
                Me.SyncXPStable()

            Case btnJogPanel.Name
                Dim f As fStageAdjust
                f = New fStageAdjust(mStageBase, "", Nothing)
                f.ShowDialog(Me)

                '--------------------------------------Jog stage - 8 axis plus 1 for XYZ move
            Case btnMoveXYZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage3Axis(nudStageX.Value, nudStageY.Value, nudStageZ.Value, True)

            Case btnMoveStageX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudStageX.Value, True)
            Case btnMoveStageY.Name
                WaitForFewSeconds(500)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudStageY.Value, True)
            Case btnMoveStageZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudStageZ.Value, True)

            Case btnMoveBeamScanX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudBeamScanX.Value, True)
            Case btnMoveBeamScanY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudBeamScanY.Value, True)
            Case btnMoveBeamScanZ.Name
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudBeamScanZ.Value, True)

            Case btnMoveProbe.Name
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.Probe, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudProbe.Value, True)
            Case btnMoveAngleLens.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudAngleLens.Value - mStageBase.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain), True)
            Case btnMoveAnglePbs.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleHexapod, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudAnglePbs.Value, True)

                '--------------------------------------Jog stage - 8 axis plus
            Case btnJogAddStageX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogStageX.Value, True)
            Case btnJogSubStageX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogStageX.Value, True)

            Case btnJogAddStageY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogStageY.Value, True)
            Case btnJogSubStageY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogStageY.Value, True)

            Case btnJogAddStageZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogStageZ.Value, True)
            Case btnJogSubStageZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogStageZ.Value, True)

            Case btnJogAddBeamScanX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogBeamScanX.Value, True)
            Case btnJogSubBeamScanX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogBeamScanX.Value, True)

            Case btnJogAddBeamScanY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogBeamScanY.Value, True)
            Case btnJogSubBeamScanY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogBeamScanY.Value, True)

            Case btnJogAddBeamScanZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogBeamScanZ.Value, True)
            Case btnJogSubBeamScanZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogBeamScanZ.Value, True)

            Case btnJogAddProbe.Name
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.Probe, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudProbe.Value, True)
            Case btnJogSubProbe.Name
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.Probe, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudProbe.Value, True)

            Case btnJogAddAngleLens.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogAngleLens.Value, True)
            Case btnJogSubAngleLens.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogAngleLens.Value, True)
            Case btnJogAddAnglePbs.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleHexapod, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogAnglePbs.Value, True)
            Case btnJogSubAnglePbs.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleHexapod, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogAnglePbs.Value, True)

                'Case btnMoveProbe.Name
                '    WaitForFewSeconds(300)
                '    mTool.Instruments.ProbeClamp.Move(nudProbe.Value, mTool.Parameter.ProbeClamp.Speed)
                'Case btnJogAddProbe.Name
                '    WaitForFewSeconds(300)
                '    mTool.Instruments.ProbeClamp.MoveRelative(nudJogProbe.Value, mTool.Parameter.ProbeClamp.Speed)
                'Case btnJogSubProbe.Name
                '    WaitForFewSeconds(300)
                '    Dim pos As Double = mTool.Instruments.ProbeClamp.Position
                '    mTool.Instruments.ProbeClamp.Move(pos - nudJogProbe.Value, mTool.Parameter.ProbeClamp.Speed)

            Case btnClampORG.Name
                WaitForFewSeconds(300)
                mTool.Instruments.ProbeClamp.ORG()
            Case btnTrunLddOff.Name
                WaitForFewSeconds(300)
                mTool.Utility.TurnLddOff()
            Case btnUV.Name
                WaitForFewSeconds(300)
                mTool.Instruments.UvLamp.ExposureTime = nudUvTime.Value
                System.Threading.Thread.Sleep(200)
                mTool.Instruments.UvLamp.PowerLevel = nudUvPower.Value
                mTool.Utility.RunUvCure()

            Case btnStop.Name
                mStop = True

            Case btnSaveHexapodConfiguredPosition.Name
                Me.SaveHexapodConfiguredPositions(False)

            Case btnSaveHexapodConfiguredPositionNew.Name
                Me.SaveHexapodConfiguredPositions(True)

            Case btnStopHexapodMove.Name
                mTool.Stop()

            Case btnSyncHexapod.Name
                Me.SyncHexapodPositions()

                '--------------------------------------Jog stage - 6 axis plus 1 for Angle move
            Case btnMoveHexapodXYZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage3Axis(nudHexapodX.Value, nudHexapodY.Value, nudHexapodZ.Value, True)

            Case btnMoveHexapodX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.X, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodX.Value, True)
            Case btnMoveHexapodY.Name
                WaitForFewSeconds(500)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Y, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodY.Value, True)
            Case btnMoveHexapodZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Z, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodZ.Value, True)
            Case btnMoveHexapodU.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.U, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodU.Value, True)
            Case btnMoveHexapodV.Name
                WaitForFewSeconds(500)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.V, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodV.Value, True)
            Case btnMoveHexapodW.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.W, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodW.Value, True)

            Case btnMovePiLS.Name
                WaitForFewSeconds(300)
                mStageTool.MovePiLS(Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudPiLS.Value, True)


                '--------------------------------------Jog stage - 8 axis plus
            Case btnJogAddHexapodX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.X, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodX.Value, True)
            Case btnJogSubHexapodX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.X, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodX.Value, True)

            Case btnJogAddHexapodY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Y, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodY.Value, True)
            Case btnJogSubHexapodY.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Y, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodY.Value, True)

            Case btnJogAddHexapodZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Z, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodZ.Value, True)
            Case btnJogSubHexapodZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Z, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodZ.Value, True)

            Case btnJogAddHexapodU.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.U, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodU.Value, True)
            Case btnJogSubHexapodU.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.U, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodU.Value, True)

            Case btnJogAddHexapodV.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.V, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodV.Value, True)
            Case btnJogSubHexapodV.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.V, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodV.Value, True)

            Case btnJogAddHexapodW.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.W, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogHexapodW.Value, True)
            Case btnJogSubHexapodW.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.W, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogHexapodW.Value, True)

            Case btnJogAddPiLS.Name
                WaitForFewSeconds(300)
                mStageTool.MovePiLS(Instrument.iMotionController.MoveToTargetMethodEnum.Relative, nudJogPiLS.Value, True)

            Case btnJogSubPiLS.Name
                WaitForFewSeconds(300)
                mStageTool.MovePiLS(Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -nudJogPiLS.Value, True)

        End Select

        'sync stage once if stage is moved
        If btn.Name.Contains("MoveHexapod") Or btn.Name.Contains("JogAddHexapod") Or btn.Name.Contains("JogSubHexapod") Then
            Me.SyncHexapodPositions()
        ElseIf btn.Name.Contains("Move") Or btn.Name.Contains("Jog") Then
            Me.SyncStagePositions()
        End If

        're-enable event
        AddHandler btn.Click, AddressOf btn_Click
    End Sub

    Private Sub chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim Checked As Boolean

        'return if the value is changed internally
        If mSync Then Return

        'get handler
        chk = CType(sender, CheckBox)
        Checked = chk.Checked

        'process event
        Select Case chk.Name
            '-----------------------------------LDD
            Case chkLddProtection.Name
                If mTool.Instruments.LDD IsNot Nothing Then
                    mTool.Instruments.LDD.EnabledProtectionState = Checked
                End If

                '-----------------------------------vacuum
            Case chkVacuumLens.Name
                mXpsIO.VacuumEnabled(iXpsIO.VacuumLine.Main) = Checked
                txtVacuumLens.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Main).ToString("0.0")
                txtVacuumPbs.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Hexapod).ToString("0.0")

            Case chkVacuumPbs.Name
                mXpsIO.VacuumEnabled(iXpsIO.VacuumLine.Hexapod) = Checked
                txtVacuumLens.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Main).ToString("0.0")
                txtVacuumPbs.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Hexapod).ToString("0.0")

            Case chkVacuumPackage.Name
                mXpsIO.VacuumEnabled(iXpsIO.VacuumLine.Package) = Checked
                txtVacuumLens.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Main).ToString("0.0")
                txtVacuumPbs.Text = mXpsIO.VacuumLevel(iXpsIO.VacuumLine.Hexapod).ToString("0.0")

            Case chkVacuumCda.Name
                mXpsIO.VacuumLinePressurized = Checked

                '-------------------------------epoxy
            Case chkEpoxy.Name
                mXpsIO.EpoxyMoveOut = Checked

                '-------------------------------probe
            Case chkProbeClampEnable.Name
                mTool.Instruments.ProbeClamp.Enabled = Checked

            Case chkProbeCdaOn.Name
                mXpsIO.ProbePositionOn = Checked

            Case chkProbeClampClose.Name
                mTool.Utility.CloseProbeClamp(Checked)

            Case chkGPIO3_1.Name
                mXPS.DigitOutput(3, 0) = Checked

            Case chkGPIO3_2.Name
                mXPS.DigitOutput(3, 1) = Checked

            Case chkGPIO3_3.Name
                mXPS.DigitOutput(3, 2) = Checked

            Case chkGPIO3_4.Name
                mXPS.DigitOutput(3, 3) = Checked

            Case chkGPIO3_5.Name
                mXPS.DigitOutput(3, 4) = Checked

            Case chkGPIO3_6.Name
                mXPS.DigitOutput(3, 5) = Checked

            Case chkLightSource11.Name
                If Checked Then
                    mTool.Instruments.LightSource1.OpenChannel(1)
                Else
                    mTool.Instruments.LightSource1.CloseChannel(1)
                End If

            Case chkLightSource12.Name
                If Checked Then
                    mTool.Instruments.LightSource1.OpenChannel(2)
                Else
                    mTool.Instruments.LightSource1.CloseChannel(2)
                End If

            Case chkLightSource21.Name
                If Checked Then
                    mTool.Instruments.LightSource2.OpenChannel(1)
                Else
                    mTool.Instruments.LightSource2.CloseChannel(1)
                End If

            Case chkLightSource22.Name
                If Checked Then
                    mTool.Instruments.LightSource2.OpenChannel(2)
                Else
                    mTool.Instruments.LightSource2.CloseChannel(2)
                End If

        End Select

        'done, do a sync
        Me.SyncSignals()
        Me.Refresh()
    End Sub

    Private Sub opt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim opt As RadioButton
        Dim Checked As Boolean

        'return if the value is changed internally
        If mSync Then Return

        'get handler
        opt = CType(sender, RadioButton)
        Checked = opt.Checked

        'we will only process "on"
        If Not Checked Then Return

        'process event
        Select Case opt.Name
            Case optCH1.Name
                mTool.Instruments.LDD.TurnSingleChannelOn(1)
                Me.SyncSignals()

            Case optCH2.Name
                mTool.Instruments.LDD.TurnSingleChannelOn(2)
                Me.SyncSignals()

            Case optCH3.Name
                mTool.Instruments.LDD.TurnSingleChannelOn(3)
                Me.SyncSignals()

            Case optCH4.Name
                mTool.Instruments.LDD.TurnSingleChannelOn(4)
                Me.SyncSignals()

        End Select
    End Sub

    Private Sub oo_StateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oo As ucOnOff
        Dim enabled As Boolean

        If mSync Then Return

        oo = CType(sender, ucOnOff)
        enabled = oo.State = ucOnOff.StateEnum.On

        Select Case oo.Name


        End Select

        System.Threading.Thread.Sleep(300)
        Me.SyncSignals()
    End Sub

    Private Sub lstConfiguredPositions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstConfiguredPositions.SelectedIndexChanged
        Dim v, Positions() As Double

        If lstConfiguredPositions.SelectedIndex < 0 Then Return

        'first, let's copy the actual position to the target so that we do not mess up color later
        Me.SetStageTargetToCurrentPosition()

        'now load the configured position, those nulled will be left as the actual position we loaded in the first step
        Positions = CType(lstConfiguredPositions.Items(lstConfiguredPositions.SelectedIndex), iXpsStage.ConfiguredStagePosition).Positions

        v = Positions(iXpsStage.AxisNameEnum.StageX) : If Not Double.IsNaN(v) Then nudStageX.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.StageY) : If Not Double.IsNaN(v) Then nudStageY.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.StageZ) : If Not Double.IsNaN(v) Then nudStageZ.Value = Convert.ToDecimal(v)

        v = Positions(iXpsStage.AxisNameEnum.BeamScanX) : If Not Double.IsNaN(v) Then nudBeamScanX.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.BeamScanY) : If Not Double.IsNaN(v) Then nudBeamScanY.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.BeamScanZ) : If Not Double.IsNaN(v) Then nudBeamScanZ.Value = Convert.ToDecimal(v)

        'v = Positions(iXpsStage.AxisNameEnum.Probe) : If Not Double.IsNaN(v) Then nudProbe.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.AngleMain) : If Not Double.IsNaN(v) Then nudAngleLens.Value = Convert.ToDecimal(v)
        v = Positions(iXpsStage.AxisNameEnum.AngleHexapod) : If Not Double.IsNaN(v) Then nudAnglePbs.Value = Convert.ToDecimal(v)

        v = Positions(iXpsStage.AxisNameEnum.PiLS) : If Not Double.IsNaN(v) Then nudPiLS.Value = Convert.ToDecimal(v)
    End Sub

    Private Sub lstHexapodConfiguredPositions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstHexapodConfiguredPositions.SelectedIndexChanged
        Dim v, Positions() As Double

        If lstHexapodConfiguredPositions.SelectedIndex < 0 Then Return

        'first, let's copy the actual position to the target so that we do not mess up color later
        Me.SetStageTargetToCurrentPosition()

        'now load the configured position, those nulled will be left as the actual position we loaded in the first step
        Positions = CType(lstHexapodConfiguredPositions.Items(lstHexapodConfiguredPositions.SelectedIndex), iXpsStage.ConfiguredStagePosition).Positions

        v = Positions(Instrument.iPiGCS2.AxisEnum.X) : If Not Double.IsNaN(v) Then nudHexapodX.Value = Convert.ToDecimal(v)
        v = Positions(Instrument.iPiGCS2.AxisEnum.Y) : If Not Double.IsNaN(v) Then nudHexapodY.Value = Convert.ToDecimal(v)
        v = Positions(Instrument.iPiGCS2.AxisEnum.Z) : If Not Double.IsNaN(v) Then nudHexapodZ.Value = Convert.ToDecimal(v)

        v = Positions(Instrument.iPiGCS2.AxisEnum.U) : If Not Double.IsNaN(v) Then nudHexapodU.Value = Convert.ToDecimal(v)
        v = Positions(Instrument.iPiGCS2.AxisEnum.V) : If Not Double.IsNaN(v) Then nudHexapodV.Value = Convert.ToDecimal(v)
        v = Positions(Instrument.iPiGCS2.AxisEnum.W) : If Not Double.IsNaN(v) Then nudHexapodW.Value = Convert.ToDecimal(v)

    End Sub

    Private Sub nud_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim nud As NumericUpDown
        Dim value As Decimal
        Dim channel As Integer

        If mSync Then Return

        'get handler
        nud = CType(sender, NumericUpDown)
        value = nud.Value

        'update few - stage will not be automatically moved. 
        Select Case nud.Name
            Case nudCurrent.Name
                If mTool.Instruments.LDD Is Nothing Then Return
                Select Case True
                    Case optCH1.Checked : channel = 1
                    Case optCH2.Checked : channel = 2
                    Case optCH3.Checked : channel = 3
                    Case optCH4.Checked : channel = 4
                End Select
                mTool.Instruments.LDD.SetSingleChannelCurrent(channel, value)

            Case nudTemperature.Name
                If mTool.Instruments.LDD Is Nothing Then Return
                mTool.Instruments.LDD.TemperatureSetpoint = value

            Case nudLightSource11.Name
                mTool.Instruments.LightSource1.SetChannelParameter(1, CInt(value))

            Case nudLightSource12.Name
                mTool.Instruments.LightSource1.SetChannelParameter(2, CInt(value))

            Case nudLightSource21.Name
                mTool.Instruments.LightSource2.SetChannelParameter(1, CInt(value))

            Case nudLightSource22.Name
                mTool.Instruments.LightSource2.SetChannelParameter(2, CInt(value))

            Case Else
                'these are stage changes, we will use button to do the work, just sync the color
                Me.SyncStageTargetColor()

                Me.SyncHexapodTargetColor()
                'early return 
                Return
        End Select

        Me.SyncSignals()
    End Sub

    Private Sub tc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tc.SelectedIndexChanged
        Select Case tc.SelectedTab.Name
            Case TabPageStage.Name
                Me.SyncStagePositions()
                Me.SyncSignals()

            Case TabPageXPS.Name
                'Me.SyncXPStable()
                dgvXPS.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

            Case TabPageSetting.Name
                dgvAlignment.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
                dgvForceGauge.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
                dgvSetting.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        End Select
    End Sub

    Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim txt As TextBox

        'we are using text box for display only here, just supress any key input
        txt = CType(sender, TextBox)
        e.SuppressKeyPress = True
    End Sub

    Private Sub tmrRefresh_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrRefresh.Tick
        Me.SyncSignals()
    End Sub
#End Region

    Private Sub WaitForFewSeconds(ByVal WaitTime As Integer)
        System.Threading.Thread.Sleep(WaitTime)
    End Sub

#Region "XPS DGV call backs"
    Private Sub dgv_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvXPS.CellClick

        Select Case e.ColumnIndex
            Case XpsColIndex.Home
                Me.HomeStage(e.RowIndex)
            Case XpsColIndex.Move
                Me.MoveStageDirect(e.RowIndex)
            Case Else
                Me.Cursor = Cursors.Default
                Return
        End Select

        Me.SyncXpsTableOneRow(e.RowIndex)
    End Sub

    Private Sub dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvXPS.CellValueChanged
        Dim Enabled As Boolean

        If e.RowIndex < 0 Then Return
        If e.ColumnIndex < 0 Then Return
        If mSync Then Return
        If mXPS Is Nothing Then Return

        Enabled = Convert.ToBoolean(dgvXPS.Item(XpsColIndex.Enabled, e.RowIndex).Value)

        mXPS.Axis = mStageBase.StageData(e.RowIndex).Axis
        Select Case e.ColumnIndex
            Case XpsColIndex.Enabled
                mXPS.DriveEnabled = Enabled

            Case XpsColIndex.Min, XpsColIndex.Max
                'Dim Min, Max As Double
                'Min = Convert.ToDouble(dgv.Item(XpsColIndex.Min, e.RowIndex).Value)
                'Max = Convert.ToDouble(dgv.Item(XpsColIndex.Max, e.RowIndex).Value)
                'mInst.XPS.SetTravelLimit(Min, Max)

            Case Else
                Me.Cursor = Cursors.Default
                Return
        End Select

        Me.SyncXpsTableOneRow(e.RowIndex)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub dgv_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvXPS.CurrentCellDirtyStateChanged
        If mSync = True Then Return

        If TypeOf dgvXPS.CurrentCell Is DataGridViewCheckBoxCell Or TypeOf dgvXPS.CurrentCell Is DataGridViewComboBoxCell Then
            dgvXPS.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgv_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)

        If e.RowIndex < 0 Then Return
        If e.ColumnIndex < 0 Then Return
        If mSync Then Return

        Dim Cell As DataGridViewCell = CType(sender, DataGridView).Item(e.ColumnIndex, e.RowIndex)

        If Cell.ValueType.Equals(GetType(Date)) Then
            MessageBox.Show("Please enter a valid date!", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf Not Cell.ValueType.Equals(GetType(String)) Then
            MessageBox.Show("Please enter an enumeric number!", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(e.Exception.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

#Region "Config file settings"
    Private Sub SetupSettingGUI()
        Dim ctrl As New List(Of Control)

        lblSaveSetting.Visible = False

        w2.w2Misc.GetAllControls(Of DataGridView)(TabPageSetting, ctrl)

        mDgvConfig = ctrl.ToArray()

        For Each dgv As DataGridView In mDgvConfig
            AddHandler dgv.CellValidating, AddressOf dgv_CellValidating
            mSettingHelper.SetupTable(dgv)
            mSettingHelper.PopulateTable(dgv)
        Next

        dgvAlignment.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right

    End Sub

    Private Sub SaveSettings()
        'wait
        Me.Cursor = Cursors.WaitCursor

        'frist save data to the table and commit
        For Each dgv As DataGridView In mDgvConfig
            mSettingHelper.SaveTable(dgv)
        Next

        'do parameter read again to read the new table into the memory
        mTool.Parameter.ReadParameters()

        'done
        Me.Cursor = Cursors.Default
        lblSaveSetting.Text = "Parameters saved on " + Date.Now.ToString()
    End Sub

    Private Sub dgv_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)
        If e.ColumnIndex < 0 Then Return
        If e.RowIndex < 0 Then Return

        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.EditingControl Is Nothing Then Return
        If dgv.EditingControl.Text = dgv.Item(e.ColumnIndex, e.RowIndex).Value.ToString() Then Return

        lblSaveSetting.Text = "Parameter changed, but not saved!"
        lblSaveSetting.ForeColor = Color.Red
        lblSaveSetting.Visible = True
    End Sub
#End Region




End Class