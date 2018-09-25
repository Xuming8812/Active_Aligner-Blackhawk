<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fHexapod
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fHexapod))
        Me.img = New System.Windows.Forms.ImageList(Me.components)
        Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.TabPageSetting = New System.Windows.Forms.TabPage()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.dgvAlignment = New System.Windows.Forms.DataGridView()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.dgvForceGauge = New System.Windows.Forms.DataGridView()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.dgvSetting = New System.Windows.Forms.DataGridView()
        Me.lblSaveSetting = New System.Windows.Forms.Label()
        Me.btnSaveSettings = New System.Windows.Forms.Button()
        Me.TabPageStage = New System.Windows.Forms.TabPage()
        Me.txtHexapodV = New System.Windows.Forms.TextBox()
        Me.txtHexapodW = New System.Windows.Forms.TextBox()
        Me.txtHexapodU = New System.Windows.Forms.TextBox()
        Me.txtHexapodZ = New System.Windows.Forms.TextBox()
        Me.txtHexapodY = New System.Windows.Forms.TextBox()
        Me.txtHexapodX = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnJogPanel = New System.Windows.Forms.Button()
        Me.btnStopMove = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.btnMoveXYZ = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodV = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodW = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodU = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodZ = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodY = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodV = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodW = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodU = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodZ = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodY = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodX = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodX = New System.Windows.Forms.Button()
        Me.nudJogHexapodV = New System.Windows.Forms.NumericUpDown()
        Me.nudJogHexapodW = New System.Windows.Forms.NumericUpDown()
        Me.nudJogHexapodU = New System.Windows.Forms.NumericUpDown()
        Me.nudJogHexapodZ = New System.Windows.Forms.NumericUpDown()
        Me.nudJogHexapodY = New System.Windows.Forms.NumericUpDown()
        Me.nudJogHexapodX = New System.Windows.Forms.NumericUpDown()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.btnMoveStageV = New System.Windows.Forms.Button()
        Me.btnMoveStageW = New System.Windows.Forms.Button()
        Me.btnMoveStageU = New System.Windows.Forms.Button()
        Me.btnMoveStageZ = New System.Windows.Forms.Button()
        Me.btnMoveStageY = New System.Windows.Forms.Button()
        Me.btnMoveStageX = New System.Windows.Forms.Button()
        Me.btnSaveConfiguredPositionNew = New System.Windows.Forms.Button()
        Me.nudHexapodV = New System.Windows.Forms.NumericUpDown()
        Me.nudHexapodW = New System.Windows.Forms.NumericUpDown()
        Me.btnSaveConfiguredPosition = New System.Windows.Forms.Button()
        Me.lstConfiguredPositions = New System.Windows.Forms.ListBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.nudHexapodZ = New System.Windows.Forms.NumericUpDown()
        Me.nudHexapodU = New System.Windows.Forms.NumericUpDown()
        Me.nudHexapodY = New System.Windows.Forms.NumericUpDown()
        Me.nudHexapodX = New System.Windows.Forms.NumericUpDown()
        Me.tc = New System.Windows.Forms.TabControl()
        Me.TabPageSetting.SuspendLayout()
        CType(Me.dgvAlignment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvForceGauge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageStage.SuspendLayout()
        CType(Me.nudJogHexapodV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tc.SuspendLayout()
        Me.SuspendLayout()
        '
        'img
        '
        Me.img.ImageStream = CType(resources.GetObject("img.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.img.TransparentColor = System.Drawing.Color.Transparent
        Me.img.Images.SetKeyName(0, "Connect")
        Me.img.Images.SetKeyName(1, "Disconnect")
        Me.img.Images.SetKeyName(2, "Refresh")
        Me.img.Images.SetKeyName(3, "Refresh Force Gauge")
        Me.img.Images.SetKeyName(4, "Read Calibration Data")
        Me.img.Images.SetKeyName(5, "Save To EEPROM")
        Me.img.Images.SetKeyName(6, "Turn Drivers Off")
        Me.img.Images.SetKeyName(7, "Stop")
        Me.img.Images.SetKeyName(8, "Home")
        Me.img.Images.SetKeyName(9, "arrow_20.ICO")
        Me.img.Images.SetKeyName(10, "Move")
        '
        'TabPageSetting
        '
        Me.TabPageSetting.Controls.Add(Me.Label45)
        Me.TabPageSetting.Controls.Add(Me.dgvAlignment)
        Me.TabPageSetting.Controls.Add(Me.Label44)
        Me.TabPageSetting.Controls.Add(Me.dgvForceGauge)
        Me.TabPageSetting.Controls.Add(Me.Label39)
        Me.TabPageSetting.Controls.Add(Me.dgvSetting)
        Me.TabPageSetting.Controls.Add(Me.lblSaveSetting)
        Me.TabPageSetting.Controls.Add(Me.btnSaveSettings)
        Me.TabPageSetting.Location = New System.Drawing.Point(4, 25)
        Me.TabPageSetting.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPageSetting.Name = "TabPageSetting"
        Me.TabPageSetting.Size = New System.Drawing.Size(1059, 536)
        Me.TabPageSetting.TabIndex = 2
        Me.TabPageSetting.Text = "Settings"
        Me.TabPageSetting.UseVisualStyleBackColor = True
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(616, 50)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(173, 18)
        Me.Label45.TabIndex = 29
        Me.Label45.Text = "Alignment Parameters"
        '
        'dgvAlignment
        '
        Me.dgvAlignment.AllowUserToAddRows = False
        Me.dgvAlignment.AllowUserToDeleteRows = False
        Me.dgvAlignment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAlignment.Location = New System.Drawing.Point(619, 71)
        Me.dgvAlignment.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvAlignment.Name = "dgvAlignment"
        Me.dgvAlignment.RowTemplate.Height = 23
        Me.dgvAlignment.Size = New System.Drawing.Size(424, 451)
        Me.dgvAlignment.TabIndex = 28
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(316, 50)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(239, 18)
        Me.Label44.TabIndex = 27
        Me.Label44.Text = "Force && Vision For Positioning"
        '
        'dgvForceGauge
        '
        Me.dgvForceGauge.AllowUserToAddRows = False
        Me.dgvForceGauge.AllowUserToDeleteRows = False
        Me.dgvForceGauge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvForceGauge.Location = New System.Drawing.Point(319, 71)
        Me.dgvForceGauge.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvForceGauge.Name = "dgvForceGauge"
        Me.dgvForceGauge.RowTemplate.Height = 23
        Me.dgvForceGauge.Size = New System.Drawing.Size(281, 451)
        Me.dgvForceGauge.TabIndex = 26
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(12, 50)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(113, 18)
        Me.Label39.TabIndex = 25
        Me.Label39.Text = "Initial Settings"
        '
        'dgvSetting
        '
        Me.dgvSetting.AllowUserToAddRows = False
        Me.dgvSetting.AllowUserToDeleteRows = False
        Me.dgvSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSetting.Location = New System.Drawing.Point(15, 71)
        Me.dgvSetting.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvSetting.Name = "dgvSetting"
        Me.dgvSetting.RowTemplate.Height = 23
        Me.dgvSetting.Size = New System.Drawing.Size(281, 451)
        Me.dgvSetting.TabIndex = 24
        '
        'lblSaveSetting
        '
        Me.lblSaveSetting.AutoSize = True
        Me.lblSaveSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSaveSetting.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSaveSetting.Location = New System.Drawing.Point(333, 17)
        Me.lblSaveSetting.Name = "lblSaveSetting"
        Me.lblSaveSetting.Size = New System.Drawing.Size(125, 20)
        Me.lblSaveSetting.TabIndex = 23
        Me.lblSaveSetting.Text = "Initial Settings"
        '
        'btnSaveSettings
        '
        Me.btnSaveSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSettings.ForeColor = System.Drawing.SystemColors.Highlight
        Me.btnSaveSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveSettings.Location = New System.Drawing.Point(15, 8)
        Me.btnSaveSettings.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSaveSettings.Name = "btnSaveSettings"
        Me.btnSaveSettings.Size = New System.Drawing.Size(312, 35)
        Me.btnSaveSettings.TabIndex = 22
        Me.btnSaveSettings.Text = "Save Changes to Configuration File  "
        Me.btnSaveSettings.UseVisualStyleBackColor = True
        '
        'TabPageStage
        '
        Me.TabPageStage.Controls.Add(Me.txtHexapodV)
        Me.TabPageStage.Controls.Add(Me.txtHexapodW)
        Me.TabPageStage.Controls.Add(Me.txtHexapodU)
        Me.TabPageStage.Controls.Add(Me.txtHexapodZ)
        Me.TabPageStage.Controls.Add(Me.txtHexapodY)
        Me.TabPageStage.Controls.Add(Me.txtHexapodX)
        Me.TabPageStage.Controls.Add(Me.Label10)
        Me.TabPageStage.Controls.Add(Me.Label8)
        Me.TabPageStage.Controls.Add(Me.btnJogPanel)
        Me.TabPageStage.Controls.Add(Me.btnStopMove)
        Me.TabPageStage.Controls.Add(Me.Label12)
        Me.TabPageStage.Controls.Add(Me.Label28)
        Me.TabPageStage.Controls.Add(Me.Label29)
        Me.TabPageStage.Controls.Add(Me.Label30)
        Me.TabPageStage.Controls.Add(Me.btnSync)
        Me.TabPageStage.Controls.Add(Me.btnMoveXYZ)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodV)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodW)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodU)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodZ)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodY)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodV)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodW)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodU)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodZ)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodY)
        Me.TabPageStage.Controls.Add(Me.btnJogSubHexapodX)
        Me.TabPageStage.Controls.Add(Me.btnJogAddHexapodX)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodV)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodW)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodU)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodZ)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodY)
        Me.TabPageStage.Controls.Add(Me.nudJogHexapodX)
        Me.TabPageStage.Controls.Add(Me.Label43)
        Me.TabPageStage.Controls.Add(Me.Label42)
        Me.TabPageStage.Controls.Add(Me.Label41)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageV)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageW)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageU)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageZ)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageY)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageX)
        Me.TabPageStage.Controls.Add(Me.btnSaveConfiguredPositionNew)
        Me.TabPageStage.Controls.Add(Me.nudHexapodV)
        Me.TabPageStage.Controls.Add(Me.nudHexapodW)
        Me.TabPageStage.Controls.Add(Me.btnSaveConfiguredPosition)
        Me.TabPageStage.Controls.Add(Me.lstConfiguredPositions)
        Me.TabPageStage.Controls.Add(Me.Label13)
        Me.TabPageStage.Controls.Add(Me.nudHexapodZ)
        Me.TabPageStage.Controls.Add(Me.nudHexapodU)
        Me.TabPageStage.Controls.Add(Me.nudHexapodY)
        Me.TabPageStage.Controls.Add(Me.nudHexapodX)
        Me.TabPageStage.Location = New System.Drawing.Point(4, 25)
        Me.TabPageStage.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPageStage.Name = "TabPageStage"
        Me.TabPageStage.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPageStage.Size = New System.Drawing.Size(1059, 536)
        Me.TabPageStage.TabIndex = 0
        Me.TabPageStage.Text = "Manual Align"
        Me.TabPageStage.UseVisualStyleBackColor = True
        '
        'txtHexapodV
        '
        Me.txtHexapodV.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodV.Location = New System.Drawing.Point(317, 164)
        Me.txtHexapodV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodV.Name = "txtHexapodV"
        Me.txtHexapodV.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodV.TabIndex = 102
        '
        'txtHexapodW
        '
        Me.txtHexapodW.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodW.Location = New System.Drawing.Point(317, 190)
        Me.txtHexapodW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodW.Name = "txtHexapodW"
        Me.txtHexapodW.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodW.TabIndex = 101
        '
        'txtHexapodU
        '
        Me.txtHexapodU.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodU.Location = New System.Drawing.Point(317, 138)
        Me.txtHexapodU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodU.Name = "txtHexapodU"
        Me.txtHexapodU.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodU.TabIndex = 100
        '
        'txtHexapodZ
        '
        Me.txtHexapodZ.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodZ.Location = New System.Drawing.Point(317, 114)
        Me.txtHexapodZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodZ.Name = "txtHexapodZ"
        Me.txtHexapodZ.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodZ.TabIndex = 99
        '
        'txtHexapodY
        '
        Me.txtHexapodY.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodY.Location = New System.Drawing.Point(317, 86)
        Me.txtHexapodY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodY.Name = "txtHexapodY"
        Me.txtHexapodY.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodY.TabIndex = 98
        '
        'txtHexapodX
        '
        Me.txtHexapodX.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodX.Location = New System.Drawing.Point(317, 62)
        Me.txtHexapodX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHexapodX.Name = "txtHexapodX"
        Me.txtHexapodX.Size = New System.Drawing.Size(82, 22)
        Me.txtHexapodX.TabIndex = 97
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(212, 89)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(63, 16)
        Me.Label10.TabIndex = 159
        Me.Label10.Text = "Stage Y"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(212, 64)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 16)
        Me.Label8.TabIndex = 158
        Me.Label8.Text = "Stage X"
        '
        'btnJogPanel
        '
        Me.btnJogPanel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogPanel.Location = New System.Drawing.Point(534, 6)
        Me.btnJogPanel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogPanel.Name = "btnJogPanel"
        Me.btnJogPanel.Size = New System.Drawing.Size(108, 26)
        Me.btnJogPanel.TabIndex = 148
        Me.btnJogPanel.Text = "Jog Panel"
        Me.btnJogPanel.UseVisualStyleBackColor = True
        '
        'btnStopMove
        '
        Me.btnStopMove.BackColor = System.Drawing.Color.Red
        Me.btnStopMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopMove.ForeColor = System.Drawing.Color.Yellow
        Me.btnStopMove.Location = New System.Drawing.Point(646, 6)
        Me.btnStopMove.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnStopMove.Name = "btnStopMove"
        Me.btnStopMove.Size = New System.Drawing.Size(108, 26)
        Me.btnStopMove.TabIndex = 141
        Me.btnStopMove.Text = "Stop Move"
        Me.btnStopMove.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(212, 116)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 16)
        Me.Label12.TabIndex = 134
        Me.Label12.Text = "Stage Z"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(214, 142)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(64, 16)
        Me.Label28.TabIndex = 136
        Me.Label28.Text = "Stage U"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(212, 190)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(67, 16)
        Me.Label29.TabIndex = 137
        Me.Label29.Text = "Stage W"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(212, 166)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(63, 16)
        Me.Label30.TabIndex = 139
        Me.Label30.Text = "Stage V"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSync
        '
        Me.btnSync.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSync.Location = New System.Drawing.Point(758, 6)
        Me.btnSync.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(108, 26)
        Me.btnSync.TabIndex = 132
        Me.btnSync.Text = "Refresh"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'btnMoveXYZ
        '
        Me.btnMoveXYZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveXYZ.Location = New System.Drawing.Point(422, 6)
        Me.btnMoveXYZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveXYZ.Name = "btnMoveXYZ"
        Me.btnMoveXYZ.Size = New System.Drawing.Size(108, 26)
        Me.btnMoveXYZ.TabIndex = 131
        Me.btnMoveXYZ.Text = "Move XYZ"
        Me.btnMoveXYZ.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodV
        '
        Me.btnJogSubHexapodV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodV.Location = New System.Drawing.Point(709, 162)
        Me.btnJogSubHexapodV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodV.Name = "btnJogSubHexapodV"
        Me.btnJogSubHexapodV.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodV.TabIndex = 127
        Me.btnJogSubHexapodV.Text = "Sub"
        Me.btnJogSubHexapodV.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodW
        '
        Me.btnJogSubHexapodW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodW.Location = New System.Drawing.Point(709, 187)
        Me.btnJogSubHexapodW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodW.Name = "btnJogSubHexapodW"
        Me.btnJogSubHexapodW.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodW.TabIndex = 126
        Me.btnJogSubHexapodW.Text = "Sub"
        Me.btnJogSubHexapodW.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodU
        '
        Me.btnJogSubHexapodU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodU.Location = New System.Drawing.Point(709, 138)
        Me.btnJogSubHexapodU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodU.Name = "btnJogSubHexapodU"
        Me.btnJogSubHexapodU.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodU.TabIndex = 125
        Me.btnJogSubHexapodU.Text = "Sub"
        Me.btnJogSubHexapodU.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodZ
        '
        Me.btnJogSubHexapodZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodZ.Location = New System.Drawing.Point(709, 113)
        Me.btnJogSubHexapodZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodZ.Name = "btnJogSubHexapodZ"
        Me.btnJogSubHexapodZ.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodZ.TabIndex = 124
        Me.btnJogSubHexapodZ.Text = "Sub"
        Me.btnJogSubHexapodZ.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodY
        '
        Me.btnJogSubHexapodY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodY.Location = New System.Drawing.Point(709, 86)
        Me.btnJogSubHexapodY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodY.Name = "btnJogSubHexapodY"
        Me.btnJogSubHexapodY.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodY.TabIndex = 123
        Me.btnJogSubHexapodY.Text = "Sub"
        Me.btnJogSubHexapodY.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodV
        '
        Me.btnJogAddHexapodV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodV.Location = New System.Drawing.Point(657, 162)
        Me.btnJogAddHexapodV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodV.Name = "btnJogAddHexapodV"
        Me.btnJogAddHexapodV.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodV.TabIndex = 120
        Me.btnJogAddHexapodV.Text = "Add"
        Me.btnJogAddHexapodV.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodW
        '
        Me.btnJogAddHexapodW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodW.Location = New System.Drawing.Point(657, 187)
        Me.btnJogAddHexapodW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodW.Name = "btnJogAddHexapodW"
        Me.btnJogAddHexapodW.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodW.TabIndex = 119
        Me.btnJogAddHexapodW.Text = "Add"
        Me.btnJogAddHexapodW.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodU
        '
        Me.btnJogAddHexapodU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodU.Location = New System.Drawing.Point(657, 138)
        Me.btnJogAddHexapodU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodU.Name = "btnJogAddHexapodU"
        Me.btnJogAddHexapodU.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodU.TabIndex = 118
        Me.btnJogAddHexapodU.Text = "Add"
        Me.btnJogAddHexapodU.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodZ
        '
        Me.btnJogAddHexapodZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodZ.Location = New System.Drawing.Point(657, 113)
        Me.btnJogAddHexapodZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodZ.Name = "btnJogAddHexapodZ"
        Me.btnJogAddHexapodZ.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodZ.TabIndex = 117
        Me.btnJogAddHexapodZ.Text = "Add"
        Me.btnJogAddHexapodZ.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodY
        '
        Me.btnJogAddHexapodY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodY.Location = New System.Drawing.Point(657, 86)
        Me.btnJogAddHexapodY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodY.Name = "btnJogAddHexapodY"
        Me.btnJogAddHexapodY.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodY.TabIndex = 116
        Me.btnJogAddHexapodY.Text = "Add"
        Me.btnJogAddHexapodY.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodX
        '
        Me.btnJogSubHexapodX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodX.Location = New System.Drawing.Point(709, 61)
        Me.btnJogSubHexapodX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogSubHexapodX.Name = "btnJogSubHexapodX"
        Me.btnJogSubHexapodX.Size = New System.Drawing.Size(45, 22)
        Me.btnJogSubHexapodX.TabIndex = 115
        Me.btnJogSubHexapodX.Text = "Sub"
        Me.btnJogSubHexapodX.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodX
        '
        Me.btnJogAddHexapodX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodX.Location = New System.Drawing.Point(657, 61)
        Me.btnJogAddHexapodX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnJogAddHexapodX.Name = "btnJogAddHexapodX"
        Me.btnJogAddHexapodX.Size = New System.Drawing.Size(45, 22)
        Me.btnJogAddHexapodX.TabIndex = 114
        Me.btnJogAddHexapodX.Text = "Add"
        Me.btnJogAddHexapodX.UseVisualStyleBackColor = True
        '
        'nudJogHexapodV
        '
        Me.nudJogHexapodV.DecimalPlaces = 4
        Me.nudJogHexapodV.Location = New System.Drawing.Point(581, 163)
        Me.nudJogHexapodV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodV.Name = "nudJogHexapodV"
        Me.nudJogHexapodV.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodV.TabIndex = 111
        Me.nudJogHexapodV.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogHexapodW
        '
        Me.nudJogHexapodW.DecimalPlaces = 4
        Me.nudJogHexapodW.Location = New System.Drawing.Point(581, 188)
        Me.nudJogHexapodW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodW.Name = "nudJogHexapodW"
        Me.nudJogHexapodW.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodW.TabIndex = 110
        Me.nudJogHexapodW.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogHexapodU
        '
        Me.nudJogHexapodU.DecimalPlaces = 4
        Me.nudJogHexapodU.Location = New System.Drawing.Point(581, 138)
        Me.nudJogHexapodU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodU.Name = "nudJogHexapodU"
        Me.nudJogHexapodU.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodU.TabIndex = 109
        Me.nudJogHexapodU.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogHexapodZ
        '
        Me.nudJogHexapodZ.DecimalPlaces = 4
        Me.nudJogHexapodZ.Location = New System.Drawing.Point(581, 114)
        Me.nudJogHexapodZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodZ.Name = "nudJogHexapodZ"
        Me.nudJogHexapodZ.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodZ.TabIndex = 108
        Me.nudJogHexapodZ.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogHexapodY
        '
        Me.nudJogHexapodY.DecimalPlaces = 4
        Me.nudJogHexapodY.Location = New System.Drawing.Point(581, 86)
        Me.nudJogHexapodY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodY.Name = "nudJogHexapodY"
        Me.nudJogHexapodY.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodY.TabIndex = 107
        Me.nudJogHexapodY.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogHexapodX
        '
        Me.nudJogHexapodX.DecimalPlaces = 4
        Me.nudJogHexapodX.Location = New System.Drawing.Point(581, 62)
        Me.nudJogHexapodX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudJogHexapodX.Name = "nudJogHexapodX"
        Me.nudJogHexapodX.Size = New System.Drawing.Size(69, 22)
        Me.nudJogHexapodX.TabIndex = 106
        Me.nudJogHexapodX.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(579, 41)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(72, 16)
        Me.Label43.TabIndex = 105
        Me.Label43.Text = "Jog (mm)"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(315, 41)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(89, 16)
        Me.Label42.TabIndex = 96
        Me.Label42.Text = "Actual (mm)"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(410, 41)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(92, 16)
        Me.Label41.TabIndex = 95
        Me.Label41.Text = "Target (mm)"
        '
        'btnMoveStageV
        '
        Me.btnMoveStageV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageV.Location = New System.Drawing.Point(504, 162)
        Me.btnMoveStageV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageV.Name = "btnMoveStageV"
        Me.btnMoveStageV.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageV.TabIndex = 92
        Me.btnMoveStageV.Text = "Move"
        Me.btnMoveStageV.UseVisualStyleBackColor = True
        '
        'btnMoveStageW
        '
        Me.btnMoveStageW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageW.Location = New System.Drawing.Point(504, 187)
        Me.btnMoveStageW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageW.Name = "btnMoveStageW"
        Me.btnMoveStageW.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageW.TabIndex = 91
        Me.btnMoveStageW.Text = "Move"
        Me.btnMoveStageW.UseVisualStyleBackColor = True
        '
        'btnMoveStageU
        '
        Me.btnMoveStageU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageU.Location = New System.Drawing.Point(504, 138)
        Me.btnMoveStageU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageU.Name = "btnMoveStageU"
        Me.btnMoveStageU.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageU.TabIndex = 90
        Me.btnMoveStageU.Text = "Move"
        Me.btnMoveStageU.UseVisualStyleBackColor = True
        '
        'btnMoveStageZ
        '
        Me.btnMoveStageZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageZ.Location = New System.Drawing.Point(504, 112)
        Me.btnMoveStageZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageZ.Name = "btnMoveStageZ"
        Me.btnMoveStageZ.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageZ.TabIndex = 89
        Me.btnMoveStageZ.Text = "Move"
        Me.btnMoveStageZ.UseVisualStyleBackColor = True
        '
        'btnMoveStageY
        '
        Me.btnMoveStageY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageY.Location = New System.Drawing.Point(504, 86)
        Me.btnMoveStageY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageY.Name = "btnMoveStageY"
        Me.btnMoveStageY.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageY.TabIndex = 88
        Me.btnMoveStageY.Text = "Move"
        Me.btnMoveStageY.UseVisualStyleBackColor = True
        '
        'btnMoveStageX
        '
        Me.btnMoveStageX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageX.Location = New System.Drawing.Point(504, 61)
        Me.btnMoveStageX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMoveStageX.Name = "btnMoveStageX"
        Me.btnMoveStageX.Size = New System.Drawing.Size(70, 22)
        Me.btnMoveStageX.TabIndex = 87
        Me.btnMoveStageX.Text = "Move"
        Me.btnMoveStageX.UseVisualStyleBackColor = True
        '
        'btnSaveConfiguredPositionNew
        '
        Me.btnSaveConfiguredPositionNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveConfiguredPositionNew.Location = New System.Drawing.Point(318, 6)
        Me.btnSaveConfiguredPositionNew.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSaveConfiguredPositionNew.Name = "btnSaveConfiguredPositionNew"
        Me.btnSaveConfiguredPositionNew.Size = New System.Drawing.Size(100, 26)
        Me.btnSaveConfiguredPositionNew.TabIndex = 86
        Me.btnSaveConfiguredPositionNew.Text = "Save New"
        Me.btnSaveConfiguredPositionNew.UseVisualStyleBackColor = True
        '
        'nudHexapodV
        '
        Me.nudHexapodV.Location = New System.Drawing.Point(407, 163)
        Me.nudHexapodV.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodV.Name = "nudHexapodV"
        Me.nudHexapodV.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodV.TabIndex = 84
        '
        'nudHexapodW
        '
        Me.nudHexapodW.Location = New System.Drawing.Point(407, 188)
        Me.nudHexapodW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodW.Name = "nudHexapodW"
        Me.nudHexapodW.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodW.TabIndex = 82
        '
        'btnSaveConfiguredPosition
        '
        Me.btnSaveConfiguredPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveConfiguredPosition.Location = New System.Drawing.Point(206, 6)
        Me.btnSaveConfiguredPosition.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSaveConfiguredPosition.Name = "btnSaveConfiguredPosition"
        Me.btnSaveConfiguredPosition.Size = New System.Drawing.Size(108, 26)
        Me.btnSaveConfiguredPosition.TabIndex = 81
        Me.btnSaveConfiguredPosition.Text = "Save Current"
        Me.btnSaveConfiguredPosition.UseVisualStyleBackColor = True
        '
        'lstConfiguredPositions
        '
        Me.lstConfiguredPositions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstConfiguredPositions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstConfiguredPositions.FormattingEnabled = True
        Me.lstConfiguredPositions.IntegralHeight = False
        Me.lstConfiguredPositions.ItemHeight = 16
        Me.lstConfiguredPositions.Location = New System.Drawing.Point(6, 29)
        Me.lstConfiguredPositions.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lstConfiguredPositions.Name = "lstConfiguredPositions"
        Me.lstConfiguredPositions.Size = New System.Drawing.Size(195, 965)
        Me.lstConfiguredPositions.TabIndex = 80
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 11)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(155, 16)
        Me.Label13.TabIndex = 79
        Me.Label13.Text = "Configured Positions:"
        '
        'nudHexapodZ
        '
        Me.nudHexapodZ.Location = New System.Drawing.Point(407, 114)
        Me.nudHexapodZ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodZ.Name = "nudHexapodZ"
        Me.nudHexapodZ.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodZ.TabIndex = 78
        '
        'nudHexapodU
        '
        Me.nudHexapodU.Location = New System.Drawing.Point(407, 138)
        Me.nudHexapodU.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodU.Name = "nudHexapodU"
        Me.nudHexapodU.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodU.TabIndex = 75
        '
        'nudHexapodY
        '
        Me.nudHexapodY.Location = New System.Drawing.Point(407, 86)
        Me.nudHexapodY.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodY.Name = "nudHexapodY"
        Me.nudHexapodY.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodY.TabIndex = 77
        '
        'nudHexapodX
        '
        Me.nudHexapodX.DecimalPlaces = 4
        Me.nudHexapodX.Location = New System.Drawing.Point(407, 62)
        Me.nudHexapodX.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.nudHexapodX.Name = "nudHexapodX"
        Me.nudHexapodX.Size = New System.Drawing.Size(91, 22)
        Me.nudHexapodX.TabIndex = 76
        Me.nudHexapodX.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'tc
        '
        Me.tc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tc.Controls.Add(Me.TabPageStage)
        Me.tc.Controls.Add(Me.TabPageSetting)
        Me.tc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tc.Location = New System.Drawing.Point(6, 6)
        Me.tc.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tc.Name = "tc"
        Me.tc.SelectedIndex = 0
        Me.tc.Size = New System.Drawing.Size(1067, 565)
        Me.tc.TabIndex = 0
        '
        'fHexapod
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1076, 574)
        Me.Controls.Add(Me.tc)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "fHexapod"
        Me.Text = "Engineering Controller"
        Me.TabPageSetting.ResumeLayout(False)
        Me.TabPageSetting.PerformLayout()
        CType(Me.dgvAlignment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvForceGauge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvSetting, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageStage.ResumeLayout(False)
        Me.TabPageStage.PerformLayout()
        CType(Me.nudJogHexapodV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tc.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents img As System.Windows.Forms.ImageList
    Friend WithEvents tmrRefresh As System.Windows.Forms.Timer
    Friend WithEvents TabPageSetting As System.Windows.Forms.TabPage
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents dgvAlignment As System.Windows.Forms.DataGridView
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents dgvForceGauge As System.Windows.Forms.DataGridView
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents dgvSetting As System.Windows.Forms.DataGridView
    Friend WithEvents lblSaveSetting As System.Windows.Forms.Label
    Friend WithEvents btnSaveSettings As System.Windows.Forms.Button
    Friend WithEvents TabPageStage As System.Windows.Forms.TabPage
    Friend WithEvents txtHexapodV As System.Windows.Forms.TextBox
    Friend WithEvents txtHexapodW As System.Windows.Forms.TextBox
    Friend WithEvents txtHexapodU As System.Windows.Forms.TextBox
    Friend WithEvents txtHexapodZ As System.Windows.Forms.TextBox
    Friend WithEvents txtHexapodY As System.Windows.Forms.TextBox
    Friend WithEvents txtHexapodX As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnJogPanel As System.Windows.Forms.Button
    Friend WithEvents btnStopMove As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents btnMoveXYZ As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodV As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodW As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodU As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodZ As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodY As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodV As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodW As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodU As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodZ As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodY As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodX As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodX As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodV As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogHexapodW As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogHexapodU As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogHexapodZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogHexapodY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogHexapodX As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents btnMoveStageV As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageW As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageU As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageZ As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageY As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageX As System.Windows.Forms.Button
    Friend WithEvents btnSaveConfiguredPositionNew As System.Windows.Forms.Button
    Friend WithEvents nudHexapodV As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHexapodW As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSaveConfiguredPosition As System.Windows.Forms.Button
    Friend WithEvents lstConfiguredPositions As System.Windows.Forms.ListBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents nudHexapodZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHexapodU As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHexapodY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHexapodX As System.Windows.Forms.NumericUpDown
    Friend WithEvents tc As System.Windows.Forms.TabControl
End Class
