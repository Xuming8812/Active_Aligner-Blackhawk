<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fStageAdjust
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fStageAdjust))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnXPlus = New System.Windows.Forms.Button()
        Me.btnXMinus = New System.Windows.Forms.Button()
        Me.btnYPlus = New System.Windows.Forms.Button()
        Me.btnYMinus = New System.Windows.Forms.Button()
        Me.nudStep = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnZMinus = New System.Windows.Forms.Button()
        Me.btnZPlus = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblText = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblStageX = New System.Windows.Forms.Label()
        Me.lblStageY = New System.Windows.Forms.Label()
        Me.lblStageZ = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblStageZ0 = New System.Windows.Forms.Label()
        Me.lblStageY0 = New System.Windows.Forms.Label()
        Me.lblStageX0 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.chkStep1 = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkStep10 = New System.Windows.Forms.CheckBox()
        Me.chkStep100 = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.nudStep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnOK, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(286, 295)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(243, 40)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.Location = New System.Drawing.Point(5, 4)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(111, 32)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(126, 4)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(112, 32)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnXPlus
        '
        Me.btnXPlus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnXPlus.Image = CType(resources.GetObject("btnXPlus.Image"), System.Drawing.Image)
        Me.btnXPlus.Location = New System.Drawing.Point(214, 114)
        Me.btnXPlus.Name = "btnXPlus"
        Me.btnXPlus.Size = New System.Drawing.Size(40, 40)
        Me.btnXPlus.TabIndex = 1
        Me.btnXPlus.UseVisualStyleBackColor = False
        '
        'btnXMinus
        '
        Me.btnXMinus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnXMinus.Image = CType(resources.GetObject("btnXMinus.Image"), System.Drawing.Image)
        Me.btnXMinus.Location = New System.Drawing.Point(3, 117)
        Me.btnXMinus.Name = "btnXMinus"
        Me.btnXMinus.Size = New System.Drawing.Size(40, 40)
        Me.btnXMinus.TabIndex = 2
        Me.btnXMinus.UseVisualStyleBackColor = False
        '
        'btnYPlus
        '
        Me.btnYPlus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnYPlus.Image = CType(resources.GetObject("btnYPlus.Image"), System.Drawing.Image)
        Me.btnYPlus.Location = New System.Drawing.Point(201, 15)
        Me.btnYPlus.Name = "btnYPlus"
        Me.btnYPlus.Size = New System.Drawing.Size(40, 40)
        Me.btnYPlus.TabIndex = 3
        Me.btnYPlus.UseVisualStyleBackColor = False
        '
        'btnYMinus
        '
        Me.btnYMinus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnYMinus.Image = CType(resources.GetObject("btnYMinus.Image"), System.Drawing.Image)
        Me.btnYMinus.Location = New System.Drawing.Point(16, 205)
        Me.btnYMinus.Name = "btnYMinus"
        Me.btnYMinus.Size = New System.Drawing.Size(40, 40)
        Me.btnYMinus.TabIndex = 4
        Me.btnYMinus.UseVisualStyleBackColor = False
        '
        'nudStep
        '
        Me.nudStep.Location = New System.Drawing.Point(87, 133)
        Me.nudStep.Name = "nudStep"
        Me.nudStep.Size = New System.Drawing.Size(82, 24)
        Me.nudStep.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.LightSkyBlue
        Me.Label1.Location = New System.Drawing.Point(85, 114)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 18)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Step Size"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnZMinus)
        Me.Panel1.Controls.Add(Me.btnYPlus)
        Me.Panel1.Controls.Add(Me.btnZPlus)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnXPlus)
        Me.Panel1.Controls.Add(Me.nudStep)
        Me.Panel1.Controls.Add(Me.btnXMinus)
        Me.Panel1.Controls.Add(Me.btnYMinus)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(12, 9)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(259, 259)
        Me.Panel1.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.SkyBlue
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(5, 181)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 18)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Out"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.SkyBlue
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(149, 220)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 18)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Down"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.SkyBlue
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(209, 157)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 18)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Right"
        '
        'btnZMinus
        '
        Me.btnZMinus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnZMinus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnZMinus.Image = CType(resources.GetObject("btnZMinus.Image"), System.Drawing.Image)
        Me.btnZMinus.Location = New System.Drawing.Point(103, 205)
        Me.btnZMinus.Name = "btnZMinus"
        Me.btnZMinus.Size = New System.Drawing.Size(40, 40)
        Me.btnZMinus.TabIndex = 8
        Me.btnZMinus.UseVisualStyleBackColor = False
        '
        'btnZPlus
        '
        Me.btnZPlus.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnZPlus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnZPlus.Image = CType(resources.GetObject("btnZPlus.Image"), System.Drawing.Image)
        Me.btnZPlus.Location = New System.Drawing.Point(103, 15)
        Me.btnZPlus.Name = "btnZPlus"
        Me.btnZPlus.Size = New System.Drawing.Size(40, 40)
        Me.btnZPlus.TabIndex = 7
        Me.btnZPlus.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(253, 253)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'lblText
        '
        Me.lblText.Location = New System.Drawing.Point(277, 14)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(258, 96)
        Me.lblText.TabIndex = 8
        Me.lblText.Text = "Label2"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(289, 123)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(163, 18)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Stage Position (mm)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(289, 191)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(18, 18)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Y"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(290, 213)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(18, 18)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Z"
        '
        'lblStageX
        '
        Me.lblStageX.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageX.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageX.Location = New System.Drawing.Point(325, 169)
        Me.lblStageX.Name = "lblStageX"
        Me.lblStageX.Size = New System.Drawing.Size(89, 18)
        Me.lblStageX.TabIndex = 12
        Me.lblStageX.Text = "Stage X"
        '
        'lblStageY
        '
        Me.lblStageY.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageY.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageY.Location = New System.Drawing.Point(325, 191)
        Me.lblStageY.Name = "lblStageY"
        Me.lblStageY.Size = New System.Drawing.Size(89, 18)
        Me.lblStageY.TabIndex = 13
        Me.lblStageY.Text = "Stage X"
        '
        'lblStageZ
        '
        Me.lblStageZ.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageZ.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageZ.Location = New System.Drawing.Point(325, 213)
        Me.lblStageZ.Name = "lblStageZ"
        Me.lblStageZ.Size = New System.Drawing.Size(89, 18)
        Me.lblStageZ.TabIndex = 14
        Me.lblStageZ.Text = "Stage X"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(289, 166)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(19, 18)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "X"
        '
        'lblStageZ0
        '
        Me.lblStageZ0.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageZ0.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageZ0.Location = New System.Drawing.Point(427, 213)
        Me.lblStageZ0.Name = "lblStageZ0"
        Me.lblStageZ0.Size = New System.Drawing.Size(87, 18)
        Me.lblStageZ0.TabIndex = 18
        Me.lblStageZ0.Text = "Stage X"
        '
        'lblStageY0
        '
        Me.lblStageY0.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageY0.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageY0.Location = New System.Drawing.Point(427, 191)
        Me.lblStageY0.Name = "lblStageY0"
        Me.lblStageY0.Size = New System.Drawing.Size(87, 18)
        Me.lblStageY0.TabIndex = 17
        Me.lblStageY0.Text = "Stage X"
        '
        'lblStageX0
        '
        Me.lblStageX0.BackColor = System.Drawing.SystemColors.Window
        Me.lblStageX0.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStageX0.Location = New System.Drawing.Point(427, 169)
        Me.lblStageX0.Name = "lblStageX0"
        Me.lblStageX0.Size = New System.Drawing.Size(87, 18)
        Me.lblStageX0.TabIndex = 16
        Me.lblStageX0.Text = "Stage X"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(325, 144)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 18)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Actual"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(427, 144)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 18)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Final Target"
        '
        'chkStep1
        '
        Me.chkStep1.AutoSize = True
        Me.chkStep1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStep1.Location = New System.Drawing.Point(103, 277)
        Me.chkStep1.Name = "chkStep1"
        Me.chkStep1.Size = New System.Drawing.Size(83, 20)
        Me.chkStep1.TabIndex = 12
        Me.chkStep1.Text = "0.001 mm"
        Me.chkStep1.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Location = New System.Drawing.Point(12, 276)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 54)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "Quick Step Size Selection"
        '
        'chkStep10
        '
        Me.chkStep10.AutoSize = True
        Me.chkStep10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStep10.Location = New System.Drawing.Point(103, 297)
        Me.chkStep10.Name = "chkStep10"
        Me.chkStep10.Size = New System.Drawing.Size(83, 20)
        Me.chkStep10.TabIndex = 22
        Me.chkStep10.Text = "0.010 mm"
        Me.chkStep10.UseVisualStyleBackColor = True
        '
        'chkStep100
        '
        Me.chkStep100.AutoSize = True
        Me.chkStep100.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStep100.Location = New System.Drawing.Point(103, 317)
        Me.chkStep100.Name = "chkStep100"
        Me.chkStep100.Size = New System.Drawing.Size(83, 20)
        Me.chkStep100.TabIndex = 23
        Me.chkStep100.Text = "0.100 mm"
        Me.chkStep100.UseVisualStyleBackColor = True
        '
        'fStageAdjust
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(543, 340)
        Me.Controls.Add(Me.chkStep100)
        Me.Controls.Add(Me.chkStep10)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.chkStep1)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblStageZ0)
        Me.Controls.Add(Me.lblStageY0)
        Me.Controls.Add(Me.lblStageX0)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblStageZ)
        Me.Controls.Add(Me.lblStageY)
        Me.Controls.Add(Me.lblStageX)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblText)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fStageAdjust"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "fAdjustStage"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.nudStep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnXPlus As System.Windows.Forms.Button
    Friend WithEvents btnXMinus As System.Windows.Forms.Button
    Friend WithEvents btnYPlus As System.Windows.Forms.Button
    Friend WithEvents btnYMinus As System.Windows.Forms.Button
    Friend WithEvents nudStep As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblText As System.Windows.Forms.Label
    Friend WithEvents btnZPlus As System.Windows.Forms.Button
    Friend WithEvents btnZMinus As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblStageX As System.Windows.Forms.Label
    Friend WithEvents lblStageY As System.Windows.Forms.Label
    Friend WithEvents lblStageZ As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblStageZ0 As System.Windows.Forms.Label
    Friend WithEvents lblStageY0 As System.Windows.Forms.Label
    Friend WithEvents lblStageX0 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents chkStep1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkStep10 As System.Windows.Forms.CheckBox
    Friend WithEvents chkStep100 As System.Windows.Forms.CheckBox

End Class
