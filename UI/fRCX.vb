Option Explicit On
Option Strict On

Imports BlackHawk.Instrument.iRCX

Public Class fRCX

    Private mFunction As BlackHawkFunction
    Private mInst As BlackHawkFunction.InstrumentList
    Private mSync As Boolean
    Private mStop As Boolean

    Private InactiveTextColor As Color = System.Drawing.Color.FromArgb(235, 235, 235)

    Public Function Initialize(ByRef hFunction As BlackHawkFunction) As Boolean
        mFunction = hFunction
        mInst = mFunction.Instruments
        mFunction.ClearStop()

        'GUI
        Me.SetupGUI()

        Me.AutoScroll = True

        Me.StartPosition = FormStartPosition.CenterParent

        'If mFunction.hLogin.UserLevel = w2.w2Login.AccessLevel.Worker Then
        '    gbArm.Enabled = False
        '    gbInitialize.Enabled = False
        'End If

        Me.Show()

        'more GUI
        Me.LoadConfiguredPositionList()

        'sync
        lblError.Text = ""
        Me.SyncStatus()


        Me.SyncStageTargetToPosition()

        'done
        Return True
    End Function

#Region "GUI setup"
    Private Sub SetupGUI()
        Dim ctrl As New List(Of Control)
        Dim x As w2NumericUpDownHelper
        Dim L1, L2 As Integer

        'flag
        mSync = True

        'tool bar
        tbr.Font = New Font("Arial", 10, FontStyle.Bold)
        tbr.ImageList = img

        tbr.Items.Add("Refresh")
        tbr.Items.Add("Stop")
        tbr.Items.Add(New ToolStripSeparator)

        For Each btn As ToolStripItem In tbr.Items
            If btn.Text <> "" AndAlso btn.Name = "" Then
                btn.Name = btn.Text
                btn.ImageKey = btn.Text
            End If
        Next

        'swape the position
        L1 = nudArmX.Left
        L2 = txtArmX.Left

        nudArmX.Left = L2
        txtArmX.Left = L1

        nudArmY.Left = L2
        txtArmY.Left = L1

        nudArmZ.Left = L2
        txtArmZ.Left = L1

        nudArmA.Left = L2
        txtArmA.Left = L1

        nudArmB.Left = L2
        txtArmB.Left = L1

        nudArmC.Left = L2
        txtArmC.Left = L1
        'buttons
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of Button)(Me, ctrl)
        For Each btn As Button In ctrl
            AddHandler btn.Click, AddressOf btn_Click
        Next

        'check box
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of CheckBox)(Me, ctrl)
        For Each ckh As CheckBox In ctrl
            AddHandler ckh.CheckedChanged, AddressOf chk_CheckedChanged
        Next

        'text box for readout
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of TextBox)(Me, ctrl)
        For Each txt As TextBox In ctrl
            If TypeOf txt.Parent Is NumericUpDown Then
                'do nothing for contained text box
            Else
                txt.BackColor = Me.InactiveTextColor
            End If
            'we need to eliminate the nud being collected as text box
            If TypeOf txt.Parent Is NumericUpDown Then Continue For
            'true text box
            AddHandler txt.KeyDown, AddressOf txt_KeyDown
        Next

        'nud
        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of NumericUpDown)(Me, ctrl)
        For Each nud As NumericUpDown In ctrl
            AddHandler nud.ValueChanged, AddressOf nud_ValueChanged
            x = New w2NumericUpDownHelper(nud, "ApacheUI.ini")
        Next

        'done
        mSync = False

    End Sub

#End Region

#Region "Configured position management"
    Public Sub LoadConfiguredPositionList()
        Dim s As String

        'build the configured position list
        lstConfiguredPositions.SelectionMode = SelectionMode.One
        lstConfiguredPositions.Items.Clear()
        For Each x As bhRobotArm.ConfiguredArmPosition In mInst.RobotArm.ConfiguredPositions.Values
            'we will not load the safe position to GUI to avoid accident changes
            s = x.Label
            If s.Contains("Safe") Then Continue For

            Select Case s
                Case "Robot Pickup Position"
                    If nudTrayIndex.Value > mInst.RobotArm.TotalCoSPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSPositionsInTray
                    x = New bhRobotArm.ConfiguredArmPosition(s, mInst.RobotArm.GetCoSPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)

                Case "Robot Place Position"
                    If nudTrayIndex.Value > mInst.RobotArm.TotalCoSFGPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSFGPositionsInTray
                    x = New bhRobotArm.ConfiguredArmPosition(s, mInst.RobotArm.GetCoSFGPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)

                Case "Robot Throw Position"
                    If nudTrayIndex.Value > mInst.RobotArm.TotalCoSDiscardPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSDiscardPositionsInTray
                    x = New bhRobotArm.ConfiguredArmPosition(s, mInst.RobotArm.GetCoSDiscardPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)
                Case Else

            End Select

            lstConfiguredPositions.Items.Add(x)
        Next
        lstConfiguredPositions.SelectedIndex = 0
    End Sub

    Private Sub SaveConfiguredPositions(ByVal NewEntry As Boolean, ByVal TrayIndex As String)
        Dim s, Label As String
        Dim i, ii As Integer
        Dim r As Windows.Forms.DialogResult
        Dim Positions As bhRobotArm.PositionStructure
        Dim x, x2 As bhRobotArm.ConfiguredArmPosition
        Dim positionType As bhRobotArm.EPositionType

        'get positions
        Positions = New bhRobotArm.PositionStructure(Val(txtArmX.Text), Val(txtArmY.Text), Val(txtArmZ.Text), _
                                                         Val(txtArmA.Text), Val(txtArmB.Text), Double.NaN)

        'get label
        If NewEntry Then
            'get label
            Label = InputBox("Please enter the label for this position configuration", "Configured Position")
            Label = Label.Trim()
            If Label = "" Then Return
            'make sure label is unique
            If mInst.RobotArm.HavePosition(Label, bhRobotArm.EPositionType.Configured, TrayIndex) Then
                s = "The label [" + Label + "] is already configured. Do you want to overwrite the original position?"
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
            Label = CType(lstConfiguredPositions.Items(i), bhRobotArm.ConfiguredArmPosition).Label
        End If

        'build a new class
        Select Case Label
            Case "Robot Pickup Position"
                positionType = bhRobotArm.EPositionType.CoS
                x = New bhRobotArm.ConfiguredArmPosition(TrayIndex, Positions)
            Case "Robot Place Position"
                positionType = bhRobotArm.EPositionType.FinishedGood
                x = New bhRobotArm.ConfiguredArmPosition(TrayIndex, Positions)
            Case "Robot Throw Position"
                positionType = bhRobotArm.EPositionType.Discard
                x = New bhRobotArm.ConfiguredArmPosition(TrayIndex, Positions)
            Case Else
                positionType = bhRobotArm.EPositionType.Configured
                x = New bhRobotArm.ConfiguredArmPosition(Label, Positions)
        End Select

        're-ask the question to confirm 
        If mInst.RobotArm.HavePosition(Label, positionType, TrayIndex) Then
            s = "We are going to overwrite the configured position for " + Label + ". Do you want to continue?"
            r = MessageBox.Show(s, "Save Configured Position", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If r = Windows.Forms.DialogResult.No Then Return
            'update the new value
            mInst.RobotArm.UpdatePosition(x, positionType)
            'update the new value to the list box
            ii = lstConfiguredPositions.Items.Count - 1
            For i = 0 To ii
                x2 = CType(lstConfiguredPositions.Items(i), bhRobotArm.ConfiguredArmPosition)
                If x2.Label = x.Label Then
                    lstConfiguredPositions.Items(i) = x
                    Exit For
                End If
            Next
        Else
            mInst.RobotArm.AddPosition(x, positionType, TrayIndex)
            'add new item to the list
            lstConfiguredPositions.Items.Add(x)
        End If

        'commit this to config file
        mInst.RobotArm.SaveConfiguredPositions(positionType)

    End Sub
#End Region


#Region "Sync and set value"

    Private Sub SyncStatus()
        mStop = False

        Me.SyncArmPositions()
        If Me.CheckStop() Then Return

    End Sub

    Private Sub SyncArmPositions()

        mSync = True
        'If Not mInst.iRCX.OperationComplete Then
        '    MessageBox.Show("机械手上一操作还未完成！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If

        'stage
        If mInst.RCX IsNot Nothing Then

            Me.ShowStatus("Get stage status and position ... ")

            Dim position As bhRobotArm.PositionStructure
            'position = mInst.RCX.CurrentPosition
            position = mInst.RobotArm.GetArmPosition

            txtArmX.Text = position.X.ToString("0.0000")
            txtArmY.Text = position.Y.ToString("0.0000")
            txtArmZ.Text = position.Z.ToString("0.0000")
            txtArmA.Text = position.A.ToString("0.0000")
            txtArmB.Text = position.B.ToString("0.0000")
            txtArmC.Text = position.C.ToString("0.0000")
            Me.SyncStageTargetColor()

            position = mInst.RCX.CurrentJointPosition
            txtMotor1.Text = position.X.ToString("0.0000")
            txtMotor2.Text = position.Y.ToString("0.0000")
            txtMotor3.Text = position.Z.ToString("0.0000")
            txtMotor4.Text = position.A.ToString("0.0000")
            txtMotor5.Text = position.B.ToString("0.0000")
            txtMotor6.Text = position.C.ToString("0.0000")


        End If

        Me.ShowStatus("")
        mSync = False
    End Sub

    Private Sub SyncStageTargetToPosition()
        Dim s As String

        s = txtArmX.Text
        If s <> "" Then nudArmX.Value = CType(s, Decimal)

        s = txtArmY.Text
        If s <> "" Then nudArmY.Value = CType(s, Decimal)

        s = txtArmZ.Text
        If s <> "" Then nudArmZ.Value = CType(s, Decimal)

        s = txtArmA.Text
        If s <> "" Then nudArmA.Value = CType(s, Decimal)

        s = txtArmB.Text
        If s <> "" Then nudArmB.Value = CType(s, Decimal)

        s = txtArmC.Text
        If s <> "" Then nudArmC.Value = CType(s, Decimal)
    End Sub

    Private Sub SyncStageTargetColor()
        Me.SetArmTargetColor(txtArmX, nudArmX)
        Me.SetArmTargetColor(txtArmY, nudArmY)
        Me.SetArmTargetColor(txtArmZ, nudArmZ)

        Me.SetArmTargetColor(txtArmB, nudArmB)
        Me.SetArmTargetColor(txtArmA, nudArmA)
        Me.SetArmTargetColor(txtArmC, nudArmC)
    End Sub

    Private Sub SetArmTargetColor(ByRef txt As TextBox, ByVal nud As NumericUpDown)
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

#End Region


#Region "status, stop"
    Private Sub ShowStatus(ByVal status As String)
        lblStatus.Text = status
        lblStatus.Owner.Refresh()
    End Sub

    Private Function CheckStop() As Boolean
        Application.DoEvents()
        If mStop Then
            Me.ShowStatus("Stop requested!")
        End If
        Return mStop
    End Function
#End Region

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button
        Dim s As String = ""

        'If Not mInst.Stage.StageIsReadyAll Then
        '    MessageBox.Show("Stage is not ready!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If

        'If Not mInst.Stage.IsStageInUnloadWindow(s) Then
        '    MessageBox.Show(s + vbNewLine + "Stage is not in Load Position Window!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If


        If mSync Then Return

        mSync = True
        Me.Cursor = Cursors.WaitCursor

        'If Not mInst.RCX.OperationComplete Then
        '    MessageBox.Show("机械手上一操作还未完成！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    mSync = False
        '    Return
        'End If

        btn = CType(sender, Button)

        Select Case btn.Name

            Case btnMoveAll.Name
                Dim target As bhRobotArm.PositionStructure
                target.X = nudArmX.Value
                target.Y = nudArmY.Value
                target.Z = nudArmZ.Value
                target.A = nudArmA.Value
                target.B = nudArmB.Value
                target.C = nudArmC.Value
                mInst.RCX.Move(target)
            Case btnMoveToSafe.Name
                mInst.RCX.MoveToSafe()
            Case btnServoOn.Name
                mInst.RCX.ServoMode = EServoStatus.ServoON
            Case btnServoOff.Name
                mInst.RCX.ServoMode = EServoStatus.ServoOFF
                '--------------------------------------Configured position management
            Case btnSaveConfiguredPosition.Name
                SyncArmPositions()
                Me.SaveConfiguredPositions(False, nudTrayIndex.Value.ToString("0"))

                '--------------------------------------Move stage - 8 axis plus 2 extra for beam near and far field
            Case btnMoveArmC.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.C, nudArmC.Value)

            Case btnMoveArmB.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.B, nudArmB.Value)

            Case btnMoveArmA.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.A, nudArmA.Value)

            Case btnMoveArmX.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.X, nudArmX.Value)

            Case btnMoveArmY.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.Y, nudArmY.Value)

            Case btnMoveArmZ.Name
                mInst.RobotArm.MoveArmAbsolute(EAxis.Z, nudArmZ.Value)

                '--------------------------------------Jog stage - 8 axis plus 2 extra for beam near and far field
            Case btnArmXJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.X, nudArmXJog.Value)
            Case btnArmXJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.X, -nudArmXJog.Value)

            Case btnArmYJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.Y, nudArmYJog.Value)
            Case btnArmYJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.Y, -nudArmYJog.Value)

            Case btnArmZJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.Z, nudArmZJog.Value)
            Case btnArmZJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.Z, -nudArmZJog.Value)

            Case btnArmAJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.A, nudArmAJog.Value)
            Case btnArmAJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.A, -nudArmAJog.Value)

            Case btnArmBJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.B, nudArmBJog.Value)
            Case btnArmBJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.B, -nudArmBJog.Value)

            Case btnArmCJogAdd.Name
                mInst.RobotArm.MoveArmRelative(EAxis.C, nudArmCJog.Value)
            Case btnArmCJogSub.Name
                mInst.RobotArm.MoveArmRelative(EAxis.C, -nudArmCJog.Value)

            Case btnMotor1DriveAdd.Name
                mInst.RCX.Speed = nudMotor1Drive.Value / 1.5 * 100
                mInst.RCX.DriveRelative(EAxis.X)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor1DriveSub.Name
                mInst.RCX.Speed = nudMotor1Drive.Value / 1.5 * 100
                mInst.RCX.DriveRelativeback(EAxis.X)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnMotor2DriveAdd.Name
                mInst.RCX.Speed = nudMotor2Drive.Value / 0.75 * 100
                mInst.RCX.DriveRelative(EAxis.Y)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor2DriveSub.Name
                mInst.RCX.Speed = nudMotor2Drive.Value / 0.75 * 100
                mInst.RCX.DriveRelativeback(EAxis.Y)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnMotor3DriveAdd.Name
                mInst.RCX.Speed = nudMotor3Drive.Value / 1.5 * 100
                mInst.RCX.DriveRelative(EAxis.Z)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor3DriveSub.Name
                mInst.RCX.Speed = nudMotor3Drive.Value / 1.5 * 100
                mInst.RCX.DriveRelativeback(EAxis.Z)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnMotor4DriveAdd.Name
                mInst.RCX.Speed = nudMotor4Drive.Value / 3.2 * 100
                mInst.RCX.DriveRelative(EAxis.A)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor4DriveSub.Name
                mInst.RCX.Speed = nudMotor4Drive.Value / 3.2 * 100
                mInst.RCX.DriveRelativeback(EAxis.A)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnMotor5DriveAdd.Name
                mInst.RCX.Speed = nudMotor5Drive.Value / 3.2 * 100
                mInst.RCX.DriveRelative(EAxis.B)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor5DriveSub.Name
                mInst.RCX.Speed = nudMotor5Drive.Value / 3.2 * 100
                mInst.RCX.DriveRelativeback(EAxis.B)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnMotor6DriveAdd.Name
                mInst.RCX.Speed = nudMotor6Drive.Value / 5.1 * 100
                mInst.RCX.DriveRelative(EAxis.C)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value
            Case btnMotor6DriveSub.Name
                mInst.RCX.Speed = nudMotor6Drive.Value / 5.1 * 100
                mInst.RCX.DriveRelativeback(EAxis.C)
                System.Threading.Thread.Sleep(200)
                mInst.RCX.Speed = nudSpeed.Value

            Case btnOpenClamp.Name
                mFunction.Utility.CloseProbeClamp(False)
            Case btnCloseClamp.Name
                mFunction.Utility.CloseProbeClamp(True)

            Case btnAlign.Name
                mInst.RCX.Align()

            Case btnSpeed.Name
                mInst.RCX.Speed = nudSpeed.Value

            Case btnClearEmg.Name
                mInst.RCX.EmergencyReset()
        End Select

        mSync = False
        Me.Cursor = Cursors.Arrow


    End Sub

    Private Sub chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim Checked As Boolean

        'return if the value is changed internally
        If mSync Then Return

        'If Not mInst.RCX.OperationComplete Then
        '    MessageBox.Show("机械手上一操作还未完成！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If

        Me.Cursor = Cursors.WaitCursor

        'get handler
        chk = CType(sender, CheckBox)
        Checked = chk.Checked

        'process event
        Select Case chk.Name

            'Case chkServoOn.Name
            '    If mInst.RCX IsNot Nothing Then
            '        If Checked Then
            '            mInst.RCX.ServoMode = EServoStatus.ServoON
            '        Else
            '            mInst.RCX.ServoMode = EServoStatus.ServoOFF
            '        End If
            '    End If



        End Select
        Me.Cursor = Cursors.Arrow

    End Sub

    Private Sub nud_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim nud As NumericUpDown
        Dim value As Decimal

        'return if the value is changed internally
        If mSync Then Return

        'get handler
        nud = CType(sender, NumericUpDown)
        value = nud.Value

        'update few - stage will not be automatically moved. 
        Select Case nud.Name
            Case nudArmX.Name, nudArmY.Name, nudArmZ.Name, nudArmA.Name, nudArmB.Name, nudArmC.Name
                Me.SyncStageTargetColor()
                Return  ' to fix no reponse on first Move Button Click
        End Select

    End Sub

    Private Sub tbr_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tbr.ItemClicked

        Me.Cursor = Cursors.WaitCursor

        Select Case e.ClickedItem.Text
            Case "Refresh"
                Me.SyncArmPositions()

            Case "Stop"
                mStop = True
                mInst.RCX.StopMotion()
        End Select

        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        'we are using text box for display only here, just supress any key input
        e.SuppressKeyPress = True

    End Sub

    Private Sub fController_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If mSync Then
            Me.ShowStatus("Window cannot be closed while fController is being initialized!")
            Return
        End If

        'If mInst.RobotArm Then
        '    MessageBox.Show("Window cannot be closed while stage is moving!", Me.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    e.Cancel = True
        'End If
    End Sub


    Private Sub lstConfiguredPositions_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstConfiguredPositions.SelectedIndexChanged
        Dim v As Double
        Dim Label As String
        Dim configuredPositions As bhRobotArm.ConfiguredArmPosition

        If lstConfiguredPositions.SelectedIndex < 0 Then Return

        'first, let's copy the actual position to the target
        Me.SyncStageTargetToPosition()

        'now load the configured position, those nulled will be left as the actual position we loaded in the first step
        Label = CType(lstConfiguredPositions.SelectedItem, bhRobotArm.ConfiguredArmPosition).Label

        Select Case Label
            Case "Robot Pickup Position"
                If nudTrayIndex.Value > mInst.RobotArm.TotalCoSPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSPositionsInTray
                configuredPositions = New bhRobotArm.ConfiguredArmPosition(Label, mInst.RobotArm.GetCoSPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)
            Case "Robot Place Position"
                If nudTrayIndex.Value > mInst.RobotArm.TotalCoSFGPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSFGPositionsInTray
                configuredPositions = New bhRobotArm.ConfiguredArmPosition(Label, mInst.RobotArm.GetCoSFGPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)
            Case "Robot Throw Position"
                If nudTrayIndex.Value > mInst.RobotArm.TotalCoSDiscardPositionsInTray Then nudTrayIndex.Value = mInst.RobotArm.TotalCoSDiscardPositionsInTray
                configuredPositions = New bhRobotArm.ConfiguredArmPosition(Label, mInst.RobotArm.GetCoSDiscardPositionInTray(Convert.ToInt16(nudTrayIndex.Value)).Positions)
            Case Else
                configuredPositions = CType(lstConfiguredPositions.Items(lstConfiguredPositions.SelectedIndex), bhRobotArm.ConfiguredArmPosition)
        End Select

        v = configuredPositions.Positions.X : If Not Double.IsNaN(v) Then nudArmX.Value = Convert.ToDecimal(v)
        v = configuredPositions.Positions.Y : If Not Double.IsNaN(v) Then nudArmY.Value = Convert.ToDecimal(v)
        v = configuredPositions.Positions.Z : If Not Double.IsNaN(v) Then nudArmZ.Value = Convert.ToDecimal(v)

        v = configuredPositions.Positions.A : If Not Double.IsNaN(v) Then nudArmA.Value = Convert.ToDecimal(v)
        v = configuredPositions.Positions.B : If Not Double.IsNaN(v) Then nudArmB.Value = Convert.ToDecimal(v)
        v = configuredPositions.Positions.C : If Not Double.IsNaN(v) Then nudArmC.Value = Convert.ToDecimal(v)
    End Sub

    
    Private Sub Label28_Click(sender As Object, e As EventArgs) Handles Label28.Click, Label7.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub
End Class