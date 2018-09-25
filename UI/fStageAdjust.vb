Option Explicit On
Option Strict On
'Option Infer Off

Imports System.Windows.Forms

Public Class fStageAdjust
    Private Const PositionFormat As String = "{0,8:0.0000}"

    Private mStage As iXpsStage

    Public Sub New(ByRef hStage As iXpsStage, ByVal Text As String, ByVal Target As iXpsStage.Position3D)

        InitializeComponent()

        mStage = hStage
        lblText.Text = Text
        If (Target.X = 0 And Target.Y = 0 And Target.Z = 0) Then
            lblStageX0.Text = "No target"
            lblStageY0.Text = "No target"
            lblStageZ0.Text = "No target"
        Else
            lblStageX0.Text = String.Format(PositionFormat, Target.X)
            lblStageY0.Text = String.Format(PositionFormat, Target.Y)
            lblStageZ0.Text = String.Format(PositionFormat, Target.Z)
        End If

        nudStep.DecimalPlaces = 4
        nudStep.Increment = 0.001D
        nudStep.Minimum = 0.0001D
        nudStep.Maximum = 0.2D
        nudStep.Value = 0.001D
        nudStep.Focus()

        Me.KeyPreview = True
        Me.SyncStage()

        Dim ctrl As New List(Of Control)
        w2.w2Misc.GetAllControls(Of Button)(Me, ctrl)
        For Each btn As Button In ctrl
            AddHandler btn.Click, AddressOf btn_Click
        Next

        ctrl.Clear()
        w2.w2Misc.GetAllControls(Of CheckBox)(Me, ctrl)
        For Each chk As CheckBox In ctrl
            AddHandler chk.CheckedChanged, AddressOf chk_CheckedChanged
        Next
    End Sub

    Private Sub MoveStage(ByVal Axis As iXpsStage.AxisNameEnum, ByVal Amount As Double)
        mStage.MoveStageNoWait(Axis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, Amount)
        While mStage.XPSController.StageMoving
            System.Threading.Thread.Sleep(10)
        End While
        System.Threading.Thread.Sleep(100)
        Me.SyncStage()
    End Sub

    Private Sub SyncStage()

        If mStage.XPSController Is Nothing Then Return
        lblStageX.Text = String.Format(PositionFormat, mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX))
        lblStageY.Text = String.Format(PositionFormat, mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY))
        lblStageZ.Text = String.Format(PositionFormat, mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ))

    End Sub

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button

        btn = CType(sender, Button)

        'prevent double hits
        RemoveHandler btn.Click, AddressOf btn_Click

        'process event
        Select Case btn.Name
            Case btnOK.Name
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()

            Case btnCancel.Name
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.Close()

            Case btnXPlus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageX, nudStep.Value)
            Case btnXMinus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageX, -nudStep.Value)

            Case btnYPlus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageY, nudStep.Value)
            Case btnYMinus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageY, -nudStep.Value)

            Case btnZPlus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageZ, nudStep.Value)
            Case btnZMinus.Name
                Me.MoveStage(iXpsStage.AxisNameEnum.StageZ, -nudStep.Value)
        End Select

        're-enable event
        AddHandler btn.Click, AddressOf btn_Click
    End Sub


    Private Sub chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox

        chk = CType(sender, CheckBox)
        If Not chk.Checked Then Return

        Select Case chk.Name
            Case chkStep1.Name
                nudStep.Value = 0.001D
            Case chkStep10.Name
                nudStep.Value = 0.01D
            Case chkStep100.Name
                nudStep.Value = 0.1D
        End Select

        chk.Checked = False
    End Sub

    Private Sub fAdjustStage_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'e.Cancel = e.CloseReason = CloseReason.UserClosing
    End Sub

    Private Sub fAdjustStage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        Select Case e.KeyCode
            Case Keys.L
                Me.MoveStage(iXpsStage.AxisNameEnum.StageX, -nudStep.Value)
            Case Keys.Right, Keys.R
                Me.MoveStage(iXpsStage.AxisNameEnum.StageX, nudStep.Value)

            Case Keys.PageUp, Keys.U
                Me.MoveStage(iXpsStage.AxisNameEnum.StageZ, nudStep.Value)
            Case Keys.PageDown, Keys.D
                Me.MoveStage(iXpsStage.AxisNameEnum.StageZ, -nudStep.Value)

            Case Keys.I
                Me.MoveStage(iXpsStage.AxisNameEnum.StageY, nudStep.Value)
            Case Keys.O
                Me.MoveStage(iXpsStage.AxisNameEnum.StageY, -nudStep.Value)

            Case Keys.Enter
                Return
        End Select
    End Sub

End Class
