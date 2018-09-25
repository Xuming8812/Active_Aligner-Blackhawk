<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fPartTray
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.panelTray = New System.Windows.Forms.Panel()
        Me.btnFillAll = New System.Windows.Forms.Button()
        Me.btnEmptyAll = New System.Windows.Forms.Button()
        Me.btnFillRest = New System.Windows.Forms.Button()
        Me.btnFillOne = New System.Windows.Forms.Button()
        Me.btnEmptyOne = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'panelTray
        '
        Me.panelTray.BackColor = System.Drawing.Color.Silver
        Me.panelTray.Location = New System.Drawing.Point(3, 3)
        Me.panelTray.Name = "panelTray"
        Me.panelTray.Size = New System.Drawing.Size(412, 330)
        Me.panelTray.TabIndex = 0
        '
        'btnFillAll
        '
        Me.btnFillAll.BackColor = System.Drawing.Color.PaleGreen
        Me.btnFillAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFillAll.Location = New System.Drawing.Point(512, 12)
        Me.btnFillAll.Name = "btnFillAll"
        Me.btnFillAll.Size = New System.Drawing.Size(70, 21)
        Me.btnFillAll.TabIndex = 1
        Me.btnFillAll.Text = "Fill All"
        Me.btnFillAll.UseVisualStyleBackColor = False
        '
        'btnEmptyAll
        '
        Me.btnEmptyAll.BackColor = System.Drawing.Color.Azure
        Me.btnEmptyAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmptyAll.Location = New System.Drawing.Point(512, 81)
        Me.btnEmptyAll.Name = "btnEmptyAll"
        Me.btnEmptyAll.Size = New System.Drawing.Size(70, 21)
        Me.btnEmptyAll.TabIndex = 2
        Me.btnEmptyAll.Text = "Empty All"
        Me.btnEmptyAll.UseVisualStyleBackColor = False
        '
        'btnFillRest
        '
        Me.btnFillRest.BackColor = System.Drawing.Color.PaleGreen
        Me.btnFillRest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFillRest.Location = New System.Drawing.Point(512, 35)
        Me.btnFillRest.Name = "btnFillRest"
        Me.btnFillRest.Size = New System.Drawing.Size(70, 21)
        Me.btnFillRest.TabIndex = 3
        Me.btnFillRest.Text = "Fill Rest"
        Me.btnFillRest.UseVisualStyleBackColor = False
        '
        'btnFillOne
        '
        Me.btnFillOne.BackColor = System.Drawing.Color.PaleGreen
        Me.btnFillOne.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFillOne.Location = New System.Drawing.Point(512, 58)
        Me.btnFillOne.Name = "btnFillOne"
        Me.btnFillOne.Size = New System.Drawing.Size(70, 21)
        Me.btnFillOne.TabIndex = 4
        Me.btnFillOne.Text = "Fill One"
        Me.btnFillOne.UseVisualStyleBackColor = False
        '
        'btnEmptyOne
        '
        Me.btnEmptyOne.BackColor = System.Drawing.Color.Azure
        Me.btnEmptyOne.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmptyOne.Location = New System.Drawing.Point(512, 104)
        Me.btnEmptyOne.Name = "btnEmptyOne"
        Me.btnEmptyOne.Size = New System.Drawing.Size(70, 21)
        Me.btnEmptyOne.TabIndex = 5
        Me.btnEmptyOne.Text = "Empty One"
        Me.btnEmptyOne.UseVisualStyleBackColor = False
        '
        'fPartTray
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(605, 373)
        Me.Controls.Add(Me.btnEmptyOne)
        Me.Controls.Add(Me.btnFillOne)
        Me.Controls.Add(Me.btnFillRest)
        Me.Controls.Add(Me.btnEmptyAll)
        Me.Controls.Add(Me.btnFillAll)
        Me.Controls.Add(Me.panelTray)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "fPartTray"
        Me.Text = "Parts in Tray"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panelTray As System.Windows.Forms.Panel
    Friend WithEvents btnFillAll As System.Windows.Forms.Button
    Friend WithEvents btnEmptyAll As System.Windows.Forms.Button
    Friend WithEvents btnFillRest As System.Windows.Forms.Button
    Friend WithEvents btnFillOne As System.Windows.Forms.Button
    Friend WithEvents btnEmptyOne As System.Windows.Forms.Button
End Class
