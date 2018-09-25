<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fMain))
        Me.img = New System.Windows.Forms.ImageList(Me.components)
        Me.pFunction = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'img
        '
        Me.img.ImageStream = CType(resources.GetObject("img.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.img.TransparentColor = System.Drawing.Color.Magenta
        Me.img.Images.SetKeyName(0, "Connect")
        Me.img.Images.SetKeyName(1, "Disconnect")
        Me.img.Images.SetKeyName(2, "Stop")
        Me.img.Images.SetKeyName(3, "Run Script")
        Me.img.Images.SetKeyName(4, "Save")
        Me.img.Images.SetKeyName(5, "Print")
        Me.img.Images.SetKeyName(6, "Table")
        Me.img.Images.SetKeyName(7, "Plot")
        Me.img.Images.SetKeyName(8, "Message")
        Me.img.Images.SetKeyName(9, "Load Script")
        Me.img.Images.SetKeyName(10, "Module Data")
        Me.img.Images.SetKeyName(11, "Save Module Data")
        Me.img.Images.SetKeyName(12, "Download Module Data")
        Me.img.Images.SetKeyName(13, "View Plot")
        Me.img.Images.SetKeyName(14, "Data Folder")
        Me.img.Images.SetKeyName(15, "Data Review")
        Me.img.Images.SetKeyName(16, "Close All Plots")
        Me.img.Images.SetKeyName(17, "Login")
        Me.img.Images.SetKeyName(18, "User Account")
        '
        'pFunction
        '
        Me.pFunction.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pFunction.Location = New System.Drawing.Point(0, 0)
        Me.pFunction.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pFunction.Name = "pFunction"
        Me.pFunction.Size = New System.Drawing.Size(1808, 741)
        Me.pFunction.TabIndex = 4
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(1808, 741)
        Me.Controls.Add(Me.pFunction)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "fMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents img As System.Windows.Forms.ImageList
    Friend WithEvents pFunction As System.Windows.Forms.Panel

End Class
