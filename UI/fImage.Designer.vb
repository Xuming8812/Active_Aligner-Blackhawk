﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fImage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fImage))
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tBtnViewFile = New System.Windows.Forms.ToolStripButton()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbImage
        '
        Me.pbImage.BackColor = System.Drawing.Color.Black
        Me.pbImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbImage.Location = New System.Drawing.Point(0, 0)
        Me.pbImage.Margin = New System.Windows.Forms.Padding(2)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(487, 368)
        Me.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbImage.TabIndex = 0
        Me.pbImage.TabStop = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tBtnViewFile})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(487, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tBtnViewFile
        '
        Me.tBtnViewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tBtnViewFile.Image = CType(resources.GetObject("tBtnViewFile.Image"), System.Drawing.Image)
        Me.tBtnViewFile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tBtnViewFile.Name = "tBtnViewFile"
        Me.tBtnViewFile.Size = New System.Drawing.Size(23, 22)
        Me.tBtnViewFile.Text = "打开图片"
        '
        'fImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(487, 368)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.pbImage)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "fImage"
        Me.Text = "fImage"
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tBtnViewFile As System.Windows.Forms.ToolStripButton
End Class
