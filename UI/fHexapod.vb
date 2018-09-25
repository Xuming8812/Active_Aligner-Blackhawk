Option Explicit On
Option Strict On

Public Class fHexapod
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

    Private mHexapod As Instrument.iPiGCS2

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

        mHexapod = hTool.Instruments.Hexopod
        mStageBase = mTool.Instruments.StageBase
        mStageTool = mTool.StageTool

        mSettingHelper = New w2ConfigTableHelper(mTool.Parameter.IniFile.FileName, "SettingTableGUI.xml")

        'interface
        Me.SetupGUI()
        Me.SetupSettingGUI()

        'load list
        Me.LoadConfiguredPositionList()

        'default page - read stage position
        tc.SelectTab(0)
        Me.SyncStagePositions()

        Me.SetStageTargetToCurrentPosition()
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

        'done
        mSync = False
    End Sub

#End Region

#Region "Sync"
    Public Sub Sync()
        Me.SyncStagePositions()
    End Sub

    Private Sub SyncStagePositions()
        mSync = True

        'stage
        If mHexapod Is Nothing Then
            'disable all the buttons? 
        Else

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.X
            txtHexapodX.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageX.Enabled = True
            btnJogAddHexapodX.Enabled = True
            btnJogSubHexapodX.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.Y
            txtHexapodY.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageY.Enabled = True
            btnJogAddHexapodY.Enabled = True
            btnJogSubHexapodY.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.Z
            txtHexapodZ.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageZ.Enabled = True
            btnJogAddHexapodZ.Enabled = True
            btnJogSubHexapodZ.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.U
            txtHexapodU.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageU.Enabled = True
            btnJogAddHexapodU.Enabled = True
            btnJogSubHexapodU.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.V
            txtHexapodV.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageV.Enabled = True
            btnJogAddHexapodV.Enabled = True
            btnJogSubHexapodV.Enabled = True

            mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.W
            txtHexapodW.Text = mHexapod.CurrentPosition.ToString("0.0000")
            btnMoveStageW.Enabled = True
            btnJogAddHexapodW.Enabled = True
            btnJogSubHexapodW.Enabled = True

            'check actual/target 
            Me.SyncStageTargetColor()

        End If

        mSync = False
    End Sub

    Private Sub SyncStageTargetColor()
        Me.SetStageTargetColor(txtHexapodX, nudHexapodX)
        Me.SetStageTargetColor(txtHexapodY, nudHexapodY)
        Me.SetStageTargetColor(txtHexapodZ, nudHexapodZ)

        Me.SetStageTargetColor(txtHexapodU, nudHexapodU)
        Me.SetStageTargetColor(txtHexapodV, nudHexapodV)
        Me.SetStageTargetColor(txtHexapodW, nudHexapodW)

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

        mHexapod.Axis = Instrument.iPiGCS2.AxisEnum.All

        success = mHexapod.HomeAndWait

        If success Then

        Else
            mStageTool.MessageService.PostMessage("X   Hexapod Error: " + mHexapod.LastError)
        End If
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
        If mStageBase.HaveHexapodConfiguredPosition(Label) Then
            s = "We are going to overwrite the configured position for " + Label + ". Do you want to continue?"
            r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = Windows.Forms.DialogResult.No Then Return
            'update the new value
            mStageBase.UpdateHexapodConfiguredPosition(x)
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
            mStageBase.AddHexapodConfiguredPosition(x)
            'add new item to the list
            lstConfiguredPositions.Items.Add(x)
        End If

        'commit this to config file
        mStageBase.SaveHexapodConfiguredPositions()

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
            '--------------------------------------
            Case btnSaveSettings.Name
                Me.SaveSettings()

            Case btnSaveConfiguredPosition.Name
                Me.SaveConfiguredPositions(False)

            Case btnSaveConfiguredPositionNew.Name
                Me.SaveConfiguredPositions(True)

            Case btnStopMove.Name
                mTool.Stop()

            Case btnSync.Name
                Me.SyncStagePositions()

                '--------------------------------------Jog stage - 6 axis plus 1 for Angle move
            Case btnMoveXYZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveStage3Axis(nudHexapodX.Value, nudHexapodY.Value, nudHexapodZ.Value, True)

            Case btnMoveStageX.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.X, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodX.Value, True)
            Case btnMoveStageY.Name
                WaitForFewSeconds(500)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Y, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodY.Value, True)
            Case btnMoveStageZ.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.Z, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodZ.Value, True)
            Case btnMoveStageU.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.U, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodU.Value, True)
            Case btnMoveStageV.Name
                WaitForFewSeconds(500)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.V, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodV.Value, True)
            Case btnMoveStageW.Name
                WaitForFewSeconds(300)
                mStageTool.MoveHexapod1Axis(Instrument.iPiGCS2.AxisEnum.W, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, nudHexapodW.Value, True)


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

            Case btnStopMove.Name
                mStop = True
        End Select

        'sync stage once if stage is moved
        If btn.Name.Contains("Move") Or btn.Name.Contains("Jog") Then Me.SyncStagePositions()

        're-enable event
        AddHandler btn.Click, AddressOf btn_Click
    End Sub

    Private Sub lstConfiguredPositions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstConfiguredPositions.SelectedIndexChanged
        Dim v, Positions() As Double

        If lstConfiguredPositions.SelectedIndex < 0 Then Return

        'first, let's copy the actual position to the target so that we do not mess up color later
        Me.SetStageTargetToCurrentPosition()

        'now load the configured position, those nulled will be left as the actual position we loaded in the first step
        Positions = CType(lstConfiguredPositions.Items(lstConfiguredPositions.SelectedIndex), iXpsStage.ConfiguredStagePosition).Positions

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

        'these are stage changes, we will use button to do the work, just sync the color
        Me.SyncStageTargetColor()
        'early return 
        Return

    End Sub

    Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim txt As TextBox

        'we are using text box for display only here, just supress any key input
        txt = CType(sender, TextBox)
        e.SuppressKeyPress = True
    End Sub

#End Region
    Private Sub WaitForFewSeconds(ByVal WaitTime As Integer)
        System.Threading.Thread.Sleep(WaitTime)
    End Sub

#Region "Config file settings"
    Private Sub SetupSettingGUI()
        Dim ctrl As New List(Of Control)

        lblSaveSetting.Visible = False

        w2.w2Misc.GetAllControls(Of DataGridView)(TabPageSetting, ctrl)

        mDgvConfig = ctrl.ToArray()

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

#End Region



End Class