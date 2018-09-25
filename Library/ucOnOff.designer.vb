<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucOnOff
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblStatus = New System.Windows.Forms.Label
        Me.optOn = New System.Windows.Forms.RadioButton
        Me.optOff = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(3, 6)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(15, 15)
        Me.lblStatus.TabIndex = 0
        '
        'optOn
        '
        Me.optOn.AutoSize = True
        Me.optOn.Location = New System.Drawing.Point(50, 5)
        Me.optOn.Name = "optOn"
        Me.optOn.Size = New System.Drawing.Size(39, 17)
        Me.optOn.TabIndex = 1
        Me.optOn.TabStop = True
        Me.optOn.Text = "On"
        Me.optOn.UseVisualStyleBackColor = True
        '
        'optOff
        '
        Me.optOff.AutoSize = True
        Me.optOff.Location = New System.Drawing.Point(102, 5)
        Me.optOff.Name = "optOff"
        Me.optOff.Size = New System.Drawing.Size(39, 17)
        Me.optOff.TabIndex = 2
        Me.optOff.TabStop = True
        Me.optOff.Text = "Off"
        Me.optOff.UseVisualStyleBackColor = True
        '
        'ucOnOff
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.optOff)
        Me.Controls.Add(Me.optOn)
        Me.Controls.Add(Me.lblStatus)
        Me.Name = "ucOnOff"
        Me.Size = New System.Drawing.Size(167, 33)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents optOn As System.Windows.Forms.RadioButton
    Friend WithEvents optOff As System.Windows.Forms.RadioButton

End Class
