<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fFunction
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fFunction))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.btnAbort = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.btnUnload = New System.Windows.Forms.Button()
        Me.txtSN = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.menu = New System.Windows.Forms.MenuStrip()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudFailIndex = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.nudPassIndex = New System.Windows.Forms.NumericUpDown()
        Me.chkAutoLoad = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.panelPartTray = New System.Windows.Forms.Panel()
        Me.dgvDUT = New System.Windows.Forms.DataGridView()
        Me.scLeftRight = New System.Windows.Forms.SplitContainer()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabTPS = New System.Windows.Forms.TabPage()
        Me.dgvScript = New System.Windows.Forms.DataGridView()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tabGraph = New System.Windows.Forms.TabPage()
        Me.tabDut = New System.Windows.Forms.TabPage()
        Me.tabLens = New System.Windows.Forms.TabPage()
        Me.tabSummary = New System.Windows.Forms.TabPage()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSummary = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtData = New System.Windows.Forms.TextBox()
        Me.sbr = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.img = New System.Windows.Forms.ImageList(Me.components)
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.pbRunning = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.nudFailIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPassIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDUT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scLeftRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scLeftRight.Panel1.SuspendLayout()
        Me.scLeftRight.Panel2.SuspendLayout()
        Me.scLeftRight.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabTPS.SuspendLayout()
        CType(Me.dgvScript, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDut.SuspendLayout()
        Me.tabLens.SuspendLayout()
        Me.tabSummary.SuspendLayout()
        Me.sbr.SuspendLayout()
        CType(Me.pbRunning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pbRunning)
        Me.Panel1.Controls.Add(Me.lblTime)
        Me.Panel1.Controls.Add(Me.btnAbort)
        Me.Panel1.Controls.Add(Me.btnPause)
        Me.Panel1.Controls.Add(Me.btnRun)
        Me.Panel1.Controls.Add(Me.btnUnload)
        Me.Panel1.Controls.Add(Me.txtSN)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.menu)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1733, 213)
        Me.Panel1.TabIndex = 3
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(864, 173)
        Me.lblTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(73, 20)
        Me.lblTime.TabIndex = 6
        Me.lblTime.Text = "00:00:00"
        '
        'btnAbort
        '
        Me.btnAbort.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbort.Location = New System.Drawing.Point(579, 111)
        Me.btnAbort.Name = "btnAbort"
        Me.btnAbort.Size = New System.Drawing.Size(130, 41)
        Me.btnAbort.TabIndex = 5
        Me.btnAbort.Text = "Abort"
        Me.btnAbort.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPause.Location = New System.Drawing.Point(390, 111)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(130, 41)
        Me.btnPause.TabIndex = 4
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnRun
        '
        Me.btnRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRun.Location = New System.Drawing.Point(579, 48)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(130, 41)
        Me.btnRun.TabIndex = 3
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'btnUnload
        '
        Me.btnUnload.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnload.Location = New System.Drawing.Point(390, 48)
        Me.btnUnload.Name = "btnUnload"
        Me.btnUnload.Size = New System.Drawing.Size(130, 41)
        Me.btnUnload.TabIndex = 2
        Me.btnUnload.Text = "Unload"
        Me.btnUnload.UseVisualStyleBackColor = True
        '
        'txtSN
        '
        Me.txtSN.Location = New System.Drawing.Point(17, 75)
        Me.txtSN.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtSN.Name = "txtSN"
        Me.txtSN.Size = New System.Drawing.Size(238, 26)
        Me.txtSN.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 43)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(129, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Serial Number"
        '
        'menu
        '
        Me.menu.Location = New System.Drawing.Point(0, 0)
        Me.menu.Name = "menu"
        Me.menu.Size = New System.Drawing.Size(1733, 24)
        Me.menu.TabIndex = 7
        Me.menu.Text = "MenuStrip1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(515, 9)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 20)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Failed"
        '
        'nudFailIndex
        '
        Me.nudFailIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nudFailIndex.Location = New System.Drawing.Point(593, 7)
        Me.nudFailIndex.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudFailIndex.Name = "nudFailIndex"
        Me.nudFailIndex.Size = New System.Drawing.Size(69, 22)
        Me.nudFailIndex.TabIndex = 31
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(297, 9)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(121, 20)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Units Passed"
        '
        'nudPassIndex
        '
        Me.nudPassIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nudPassIndex.Location = New System.Drawing.Point(438, 7)
        Me.nudPassIndex.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudPassIndex.Name = "nudPassIndex"
        Me.nudPassIndex.Size = New System.Drawing.Size(69, 22)
        Me.nudPassIndex.TabIndex = 29
        '
        'chkAutoLoad
        '
        Me.chkAutoLoad.AutoSize = True
        Me.chkAutoLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoLoad.Location = New System.Drawing.Point(157, 9)
        Me.chkAutoLoad.Margin = New System.Windows.Forms.Padding(4)
        Me.chkAutoLoad.Name = "chkAutoLoad"
        Me.chkAutoLoad.Size = New System.Drawing.Size(116, 24)
        Me.chkAutoLoad.TabIndex = 28
        Me.chkAutoLoad.Text = "Auto Load"
        Me.chkAutoLoad.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(7, 9)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(123, 20)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Package Tray"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(4, 6)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 20)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Part Tray"
        '
        'panelPartTray
        '
        Me.panelPartTray.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.panelPartTray.BackColor = System.Drawing.SystemColors.Control
        Me.panelPartTray.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panelPartTray.Location = New System.Drawing.Point(4, 34)
        Me.panelPartTray.Margin = New System.Windows.Forms.Padding(4)
        Me.panelPartTray.Name = "panelPartTray"
        Me.panelPartTray.Size = New System.Drawing.Size(820, 546)
        Me.panelPartTray.TabIndex = 22
        '
        'dgvDUT
        '
        Me.dgvDUT.AllowUserToAddRows = False
        Me.dgvDUT.AllowUserToDeleteRows = False
        Me.dgvDUT.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dgvDUT.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvDUT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDUT.Location = New System.Drawing.Point(4, 40)
        Me.dgvDUT.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvDUT.Name = "dgvDUT"
        Me.dgvDUT.ReadOnly = True
        Me.dgvDUT.Size = New System.Drawing.Size(820, 541)
        Me.dgvDUT.TabIndex = 24
        '
        'scLeftRight
        '
        Me.scLeftRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.scLeftRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scLeftRight.Location = New System.Drawing.Point(0, 221)
        Me.scLeftRight.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.scLeftRight.Name = "scLeftRight"
        '
        'scLeftRight.Panel1
        '
        Me.scLeftRight.Panel1.Controls.Add(Me.TabControl1)
        '
        'scLeftRight.Panel2
        '
        Me.scLeftRight.Panel2.Controls.Add(Me.Label7)
        Me.scLeftRight.Panel2.Controls.Add(Me.txtData)
        Me.scLeftRight.Size = New System.Drawing.Size(1733, 587)
        Me.scLeftRight.SplitterDistance = 838
        Me.scLeftRight.SplitterWidth = 5
        Me.scLeftRight.TabIndex = 4
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabTPS)
        Me.TabControl1.Controls.Add(Me.tabGraph)
        Me.TabControl1.Controls.Add(Me.tabDut)
        Me.TabControl1.Controls.Add(Me.tabLens)
        Me.TabControl1.Controls.Add(Me.tabSummary)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(836, 585)
        Me.TabControl1.TabIndex = 0
        '
        'tabTPS
        '
        Me.tabTPS.Controls.Add(Me.dgvScript)
        Me.tabTPS.Controls.Add(Me.Label9)
        Me.tabTPS.Location = New System.Drawing.Point(4, 25)
        Me.tabTPS.Name = "tabTPS"
        Me.tabTPS.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTPS.Size = New System.Drawing.Size(828, 556)
        Me.tabTPS.TabIndex = 0
        Me.tabTPS.Text = "TPS"
        Me.tabTPS.UseVisualStyleBackColor = True
        '
        'dgvScript
        '
        Me.dgvScript.AllowUserToAddRows = False
        Me.dgvScript.AllowUserToDeleteRows = False
        Me.dgvScript.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvScript.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvScript.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScript.Location = New System.Drawing.Point(11, 30)
        Me.dgvScript.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvScript.Name = "dgvScript"
        Me.dgvScript.ReadOnly = True
        Me.dgvScript.Size = New System.Drawing.Size(810, 517)
        Me.dgvScript.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(7, 7)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(132, 20)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Process Steps"
        '
        'tabGraph
        '
        Me.tabGraph.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.tabGraph.Location = New System.Drawing.Point(4, 25)
        Me.tabGraph.Name = "tabGraph"
        Me.tabGraph.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGraph.Size = New System.Drawing.Size(828, 584)
        Me.tabGraph.TabIndex = 1
        Me.tabGraph.Text = "Graph"
        '
        'tabDut
        '
        Me.tabDut.Controls.Add(Me.dgvDUT)
        Me.tabDut.Controls.Add(Me.Label5)
        Me.tabDut.Controls.Add(Me.Label12)
        Me.tabDut.Controls.Add(Me.Label10)
        Me.tabDut.Controls.Add(Me.nudFailIndex)
        Me.tabDut.Controls.Add(Me.chkAutoLoad)
        Me.tabDut.Controls.Add(Me.nudPassIndex)
        Me.tabDut.Location = New System.Drawing.Point(4, 25)
        Me.tabDut.Name = "tabDut"
        Me.tabDut.Size = New System.Drawing.Size(828, 584)
        Me.tabDut.TabIndex = 2
        Me.tabDut.Text = "DUT"
        Me.tabDut.UseVisualStyleBackColor = True
        '
        'tabLens
        '
        Me.tabLens.Controls.Add(Me.panelPartTray)
        Me.tabLens.Controls.Add(Me.Label8)
        Me.tabLens.Location = New System.Drawing.Point(4, 25)
        Me.tabLens.Name = "tabLens"
        Me.tabLens.Size = New System.Drawing.Size(828, 584)
        Me.tabLens.TabIndex = 3
        Me.tabLens.Text = "Lens"
        Me.tabLens.UseVisualStyleBackColor = True
        '
        'tabSummary
        '
        Me.tabSummary.Controls.Add(Me.Label6)
        Me.tabSummary.Controls.Add(Me.txtSummary)
        Me.tabSummary.Location = New System.Drawing.Point(4, 25)
        Me.tabSummary.Name = "tabSummary"
        Me.tabSummary.Size = New System.Drawing.Size(828, 584)
        Me.tabSummary.TabIndex = 4
        Me.tabSummary.Text = "Summary"
        Me.tabSummary.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(4, 8)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(133, 20)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Summary Data"
        '
        'txtSummary
        '
        Me.txtSummary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSummary.Location = New System.Drawing.Point(4, 37)
        Me.txtSummary.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtSummary.Multiline = True
        Me.txtSummary.Name = "txtSummary"
        Me.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtSummary.Size = New System.Drawing.Size(820, 538)
        Me.txtSummary.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(7, 2)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(178, 20)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Process Information"
        '
        'txtData
        '
        Me.txtData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtData.Location = New System.Drawing.Point(7, 34)
        Me.txtData.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtData.Size = New System.Drawing.Size(862, 538)
        Me.txtData.TabIndex = 0
        '
        'sbr
        '
        Me.sbr.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
        Me.sbr.Location = New System.Drawing.Point(0, 808)
        Me.sbr.Name = "sbr"
        Me.sbr.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.sbr.Size = New System.Drawing.Size(1733, 25)
        Me.sbr.TabIndex = 4
        Me.sbr.Text = "StatusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(1713, 20)
        Me.lblStatus.Spring = True
        Me.lblStatus.Text = "Some Notes"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'img
        '
        Me.img.ImageStream = CType(resources.GetObject("img.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.img.TransparentColor = System.Drawing.Color.Magenta
        Me.img.Images.SetKeyName(0, "Connect")
        Me.img.Images.SetKeyName(1, "Disconn")
        Me.img.Images.SetKeyName(2, "Stop")
        Me.img.Images.SetKeyName(3, "Run")
        Me.img.Images.SetKeyName(4, "Save")
        Me.img.Images.SetKeyName(5, "Print")
        Me.img.Images.SetKeyName(6, "Table")
        Me.img.Images.SetKeyName(7, "Plot")
        Me.img.Images.SetKeyName(8, "Message")
        Me.img.Images.SetKeyName(9, "Script")
        Me.img.Images.SetKeyName(10, "Cal Data")
        Me.img.Images.SetKeyName(11, "Save Module Data")
        Me.img.Images.SetKeyName(12, "Unload")
        Me.img.Images.SetKeyName(13, "View Plot")
        Me.img.Images.SetKeyName(14, "Data Folder")
        Me.img.Images.SetKeyName(15, "Pause")
        Me.img.Images.SetKeyName(16, "New Lot")
        Me.img.Images.SetKeyName(17, "Controller")
        Me.img.Images.SetKeyName(18, "Home All")
        Me.img.Images.SetKeyName(19, "Camera View")
        Me.img.Images.SetKeyName(20, "BG Cal")
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 213)
        Me.Splitter1.Margin = New System.Windows.Forms.Padding(4)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(1733, 8)
        Me.Splitter1.TabIndex = 6
        Me.Splitter1.TabStop = False
        '
        'pbRunning
        '
        Me.pbRunning.Image = CType(resources.GetObject("pbRunning.Image"), System.Drawing.Image)
        Me.pbRunning.Location = New System.Drawing.Point(802, 39)
        Me.pbRunning.Name = "pbRunning"
        Me.pbRunning.Size = New System.Drawing.Size(198, 131)
        Me.pbRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbRunning.TabIndex = 8
        Me.pbRunning.TabStop = False
        '
        'fFunction
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScrollMargin = New System.Drawing.Size(10, 10)
        Me.ClientSize = New System.Drawing.Size(1733, 833)
        Me.Controls.Add(Me.scLeftRight)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.sbr)
        Me.MainMenuStrip = Me.menu
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "fFunction"
        Me.Text = "fFunction"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudFailIndex, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPassIndex, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDUT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLeftRight.Panel1.ResumeLayout(False)
        Me.scLeftRight.Panel2.ResumeLayout(False)
        Me.scLeftRight.Panel2.PerformLayout()
        CType(Me.scLeftRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scLeftRight.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabTPS.ResumeLayout(False)
        Me.tabTPS.PerformLayout()
        CType(Me.dgvScript, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDut.ResumeLayout(False)
        Me.tabDut.PerformLayout()
        Me.tabLens.ResumeLayout(False)
        Me.tabLens.PerformLayout()
        Me.tabSummary.ResumeLayout(False)
        Me.tabSummary.PerformLayout()
        Me.sbr.ResumeLayout(False)
        Me.sbr.PerformLayout()
        CType(Me.pbRunning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents scLeftRight As System.Windows.Forms.SplitContainer
    Friend WithEvents dgvScript As System.Windows.Forms.DataGridView
    Friend WithEvents sbr As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents img As System.Windows.Forms.ImageList
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents txtSummary As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents panelPartTray As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dgvDUT As System.Windows.Forms.DataGridView
    Friend WithEvents txtSN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudFailIndex As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents nudPassIndex As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkAutoLoad As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabTPS As System.Windows.Forms.TabPage
    Friend WithEvents tabGraph As System.Windows.Forms.TabPage
    Friend WithEvents tabDut As System.Windows.Forms.TabPage
    Friend WithEvents tabLens As System.Windows.Forms.TabPage
    Friend WithEvents tabSummary As System.Windows.Forms.TabPage
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents btnAbort As System.Windows.Forms.Button
    Friend WithEvents btnPause As System.Windows.Forms.Button
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnUnload As System.Windows.Forms.Button
    Friend WithEvents menu As System.Windows.Forms.MenuStrip
    Friend WithEvents pbRunning As System.Windows.Forms.PictureBox
End Class
