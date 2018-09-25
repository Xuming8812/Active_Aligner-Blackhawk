<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fControl))
        Me.tc = New System.Windows.Forms.TabControl()
        Me.TabPageStage = New System.Windows.Forms.TabPage()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.nudLightSource22 = New System.Windows.Forms.NumericUpDown()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.nudLightSource21 = New System.Windows.Forms.NumericUpDown()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.nudLightSource12 = New System.Windows.Forms.NumericUpDown()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.nudLightSource11 = New System.Windows.Forms.NumericUpDown()
        Me.chkLightSource11 = New System.Windows.Forms.CheckBox()
        Me.chkLightSource22 = New System.Windows.Forms.CheckBox()
        Me.chkLightSource12 = New System.Windows.Forms.CheckBox()
        Me.chkLightSource21 = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnSaveGoldImage = New System.Windows.Forms.Button()
        Me.lstGoldImage = New System.Windows.Forms.ListBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.nudUvPower = New System.Windows.Forms.NumericUpDown()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ooUvLamp = New BlackHawk.ucOnOff()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.nudUvTime = New System.Windows.Forms.NumericUpDown()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.btnUV = New System.Windows.Forms.Button()
        Me.lstConfiguredPositions = New System.Windows.Forms.ListBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.btnJogSubProbe = New System.Windows.Forms.Button()
        Me.btnJogAddProbe = New System.Windows.Forms.Button()
        Me.nudJogProbe = New System.Windows.Forms.NumericUpDown()
        Me.txtProbe = New System.Windows.Forms.TextBox()
        Me.btnMoveProbe = New System.Windows.Forms.Button()
        Me.nudProbe = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkGPIO3_6 = New System.Windows.Forms.CheckBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.chkGPIO3_5 = New System.Windows.Forms.CheckBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.chkGPIO3_4 = New System.Windows.Forms.CheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.chkGPIO3_3 = New System.Windows.Forms.CheckBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.chkGPIO3_2 = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.chkGPIO3_1 = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.chkVacuumPbs = New System.Windows.Forms.CheckBox()
        Me.chkVacuumLens = New System.Windows.Forms.CheckBox()
        Me.txtCdaLine = New System.Windows.Forms.TextBox()
        Me.btnVacuumCda = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkVacuumCda = New System.Windows.Forms.CheckBox()
        Me.txtVacuumLine = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnEpoxyTrigger = New System.Windows.Forms.Button()
        Me.chkEpoxy = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.txtVacuumPbs = New System.Windows.Forms.TextBox()
        Me.txtVacuumLens = New System.Windows.Forms.TextBox()
        Me.txtForcePbs = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtForceLens = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnJogSubAnglePbs = New System.Windows.Forms.Button()
        Me.btnJogAddAnglePbs = New System.Windows.Forms.Button()
        Me.nudJogAnglePbs = New System.Windows.Forms.NumericUpDown()
        Me.txtAnglePbs = New System.Windows.Forms.TextBox()
        Me.btnMoveAnglePbs = New System.Windows.Forms.Button()
        Me.nudAnglePbs = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkLddProtection = New System.Windows.Forms.CheckBox()
        Me.txtTemperature = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.nudTemperature = New System.Windows.Forms.NumericUpDown()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.chkProbeCdaOn = New System.Windows.Forms.CheckBox()
        Me.btnTrunLddOff = New System.Windows.Forms.Button()
        Me.btnClampORG = New System.Windows.Forms.Button()
        Me.chkVacuumPackage = New System.Windows.Forms.CheckBox()
        Me.chkProbeClampEnable = New System.Windows.Forms.CheckBox()
        Me.chkProbeClampClose = New System.Windows.Forms.CheckBox()
        Me.txtVoltage = New System.Windows.Forms.TextBox()
        Me.optCH4 = New System.Windows.Forms.RadioButton()
        Me.optCH3 = New System.Windows.Forms.RadioButton()
        Me.optCH2 = New System.Windows.Forms.RadioButton()
        Me.optCH1 = New System.Windows.Forms.RadioButton()
        Me.nudCurrent = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnJogPanel = New System.Windows.Forms.Button()
        Me.btnStopMove = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.btnMoveXYZ = New System.Windows.Forms.Button()
        Me.btnJogSubAngleLens = New System.Windows.Forms.Button()
        Me.btnJogSubBeamScanY = New System.Windows.Forms.Button()
        Me.btnJogSubBeamScanZ = New System.Windows.Forms.Button()
        Me.btnJogSubBeamScanX = New System.Windows.Forms.Button()
        Me.btnJogSubStageZ = New System.Windows.Forms.Button()
        Me.btnJogSubStageY = New System.Windows.Forms.Button()
        Me.btnJogAddAngleLens = New System.Windows.Forms.Button()
        Me.btnJogAddBeamScanY = New System.Windows.Forms.Button()
        Me.btnJogAddBeamScanZ = New System.Windows.Forms.Button()
        Me.btnJogAddBeamScanX = New System.Windows.Forms.Button()
        Me.btnJogAddStageZ = New System.Windows.Forms.Button()
        Me.btnJogAddStageY = New System.Windows.Forms.Button()
        Me.btnJogSubStageX = New System.Windows.Forms.Button()
        Me.btnJogAddStageX = New System.Windows.Forms.Button()
        Me.nudJogAngleLens = New System.Windows.Forms.NumericUpDown()
        Me.nudJogBeamScanY = New System.Windows.Forms.NumericUpDown()
        Me.nudJogBeamScanZ = New System.Windows.Forms.NumericUpDown()
        Me.nudJogBeamScanX = New System.Windows.Forms.NumericUpDown()
        Me.nudJogStageZ = New System.Windows.Forms.NumericUpDown()
        Me.nudJogStageY = New System.Windows.Forms.NumericUpDown()
        Me.nudJogStageX = New System.Windows.Forms.NumericUpDown()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.txtAngleLens = New System.Windows.Forms.TextBox()
        Me.txtBeamScanY = New System.Windows.Forms.TextBox()
        Me.txtBeamScanZ = New System.Windows.Forms.TextBox()
        Me.txtBeamScanX = New System.Windows.Forms.TextBox()
        Me.txtStageZ = New System.Windows.Forms.TextBox()
        Me.txtStageY = New System.Windows.Forms.TextBox()
        Me.txtStageX = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.btnMoveAngleLens = New System.Windows.Forms.Button()
        Me.btnMoveBeamScanY = New System.Windows.Forms.Button()
        Me.btnMoveBeamScanZ = New System.Windows.Forms.Button()
        Me.btnMoveBeamScanX = New System.Windows.Forms.Button()
        Me.btnMoveStageZ = New System.Windows.Forms.Button()
        Me.btnMoveStageY = New System.Windows.Forms.Button()
        Me.btnMoveStageX = New System.Windows.Forms.Button()
        Me.btnSaveConfiguredPositionNew = New System.Windows.Forms.Button()
        Me.nudAngleLens = New System.Windows.Forms.NumericUpDown()
        Me.nudBeamScanY = New System.Windows.Forms.NumericUpDown()
        Me.nudBeamScanZ = New System.Windows.Forms.NumericUpDown()
        Me.btnDeleteConfiguredPosition = New System.Windows.Forms.Button()
        Me.btnSaveConfiguredPosition = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.nudStageZ = New System.Windows.Forms.NumericUpDown()
        Me.nudBeamScanX = New System.Windows.Forms.NumericUpDown()
        Me.nudStageY = New System.Windows.Forms.NumericUpDown()
        Me.nudStageX = New System.Windows.Forms.NumericUpDown()
        Me.TabPageXPS = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.txtHexapodV = New System.Windows.Forms.TextBox()
        Me.nudHexapodX = New System.Windows.Forms.NumericUpDown()
        Me.txtHexapodW = New System.Windows.Forms.TextBox()
        Me.nudHexapodY = New System.Windows.Forms.NumericUpDown()
        Me.txtHexapodU = New System.Windows.Forms.TextBox()
        Me.nudHexapodU = New System.Windows.Forms.NumericUpDown()
        Me.txtHexapodZ = New System.Windows.Forms.TextBox()
        Me.nudHexapodZ = New System.Windows.Forms.NumericUpDown()
        Me.txtHexapodY = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.txtHexapodX = New System.Windows.Forms.TextBox()
        Me.lstHexapodConfiguredPositions = New System.Windows.Forms.ListBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.btnSaveHexapodConfiguredPosition = New System.Windows.Forms.Button()
        Me.nudHexapodW = New System.Windows.Forms.NumericUpDown()
        Me.btnJogHexapodPanel = New System.Windows.Forms.Button()
        Me.nudHexapodV = New System.Windows.Forms.NumericUpDown()
        Me.btnStopHexapodMove = New System.Windows.Forms.Button()
        Me.btnSaveHexapodConfiguredPositionNew = New System.Windows.Forms.Button()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.btnMoveHexapodX = New System.Windows.Forms.Button()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.btnMoveHexapodY = New System.Windows.Forms.Button()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.btnMoveHexapodZ = New System.Windows.Forms.Button()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.btnMoveHexapodU = New System.Windows.Forms.Button()
        Me.btnSyncHexapod = New System.Windows.Forms.Button()
        Me.btnMoveHexapodW = New System.Windows.Forms.Button()
        Me.btnMoveHexapodXYZ = New System.Windows.Forms.Button()
        Me.btnMoveHexapodV = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodV = New System.Windows.Forms.Button()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.btnJogSubHexapodW = New System.Windows.Forms.Button()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.btnJogSubHexapodU = New System.Windows.Forms.Button()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.btnJogSubHexapodZ = New System.Windows.Forms.Button()
        Me.nudJogHexapodX = New System.Windows.Forms.NumericUpDown()
        Me.btnJogSubHexapodY = New System.Windows.Forms.Button()
        Me.nudJogHexapodY = New System.Windows.Forms.NumericUpDown()
        Me.btnJogAddHexapodV = New System.Windows.Forms.Button()
        Me.nudJogHexapodZ = New System.Windows.Forms.NumericUpDown()
        Me.btnJogAddHexapodW = New System.Windows.Forms.Button()
        Me.nudJogHexapodU = New System.Windows.Forms.NumericUpDown()
        Me.btnJogAddHexapodU = New System.Windows.Forms.Button()
        Me.nudJogHexapodW = New System.Windows.Forms.NumericUpDown()
        Me.btnJogAddHexapodZ = New System.Windows.Forms.Button()
        Me.nudJogHexapodV = New System.Windows.Forms.NumericUpDown()
        Me.btnJogAddHexapodY = New System.Windows.Forms.Button()
        Me.btnJogAddHexapodX = New System.Windows.Forms.Button()
        Me.btnJogSubHexapodX = New System.Windows.Forms.Button()
        Me.btnPiC843 = New System.Windows.Forms.Button()
        Me.btnSyncXPS = New System.Windows.Forms.Button()
        Me.dgvXPS = New System.Windows.Forms.DataGridView()
        Me.btnXPS = New System.Windows.Forms.Button()
        Me.TabPageSetting = New System.Windows.Forms.TabPage()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.dgvAlignment = New System.Windows.Forms.DataGridView()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.dgvForceGauge = New System.Windows.Forms.DataGridView()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.dgvSetting = New System.Windows.Forms.DataGridView()
        Me.lblSaveSetting = New System.Windows.Forms.Label()
        Me.btnSaveSettings = New System.Windows.Forms.Button()
        Me.img = New System.Windows.Forms.ImageList(Me.components)
        Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.Label51 = New System.Windows.Forms.Label()
        Me.btnJogSubPiLS = New System.Windows.Forms.Button()
        Me.btnJogAddPiLS = New System.Windows.Forms.Button()
        Me.nudJogPiLS = New System.Windows.Forms.NumericUpDown()
        Me.txtPiLS = New System.Windows.Forms.TextBox()
        Me.btnMovePiLS = New System.Windows.Forms.Button()
        Me.nudPiLS = New System.Windows.Forms.NumericUpDown()
        Me.tc.SuspendLayout()
        Me.TabPageStage.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.nudLightSource22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLightSource21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLightSource12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLightSource11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.nudUvPower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudUvTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogProbe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudProbe, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.nudJogAnglePbs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAnglePbs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudTemperature, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogAngleLens, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogBeamScanY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogBeamScanZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogBeamScanX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogStageZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogStageY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogStageX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAngleLens, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBeamScanY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBeamScanZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStageZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBeamScanX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStageY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStageX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageXPS.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.nudHexapodX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHexapodV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodU, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogHexapodV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvXPS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSetting.SuspendLayout()
        CType(Me.dgvAlignment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvForceGauge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSetting, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJogPiLS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPiLS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tc
        '
        Me.tc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tc.Controls.Add(Me.TabPageStage)
        Me.tc.Controls.Add(Me.TabPageXPS)
        Me.tc.Controls.Add(Me.TabPageSetting)
        Me.tc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tc.Location = New System.Drawing.Point(8, 8)
        Me.tc.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tc.Name = "tc"
        Me.tc.SelectedIndex = 0
        Me.tc.Size = New System.Drawing.Size(1533, 783)
        Me.tc.TabIndex = 0
        '
        'TabPageStage
        '
        Me.TabPageStage.Controls.Add(Me.Label51)
        Me.TabPageStage.Controls.Add(Me.btnJogSubPiLS)
        Me.TabPageStage.Controls.Add(Me.btnJogAddPiLS)
        Me.TabPageStage.Controls.Add(Me.nudJogPiLS)
        Me.TabPageStage.Controls.Add(Me.txtPiLS)
        Me.TabPageStage.Controls.Add(Me.btnMovePiLS)
        Me.TabPageStage.Controls.Add(Me.nudPiLS)
        Me.TabPageStage.Controls.Add(Me.GroupBox7)
        Me.TabPageStage.Controls.Add(Me.GroupBox3)
        Me.TabPageStage.Controls.Add(Me.GroupBox4)
        Me.TabPageStage.Controls.Add(Me.lstConfiguredPositions)
        Me.TabPageStage.Controls.Add(Me.Label20)
        Me.TabPageStage.Controls.Add(Me.btnJogSubProbe)
        Me.TabPageStage.Controls.Add(Me.btnJogAddProbe)
        Me.TabPageStage.Controls.Add(Me.nudJogProbe)
        Me.TabPageStage.Controls.Add(Me.txtProbe)
        Me.TabPageStage.Controls.Add(Me.btnMoveProbe)
        Me.TabPageStage.Controls.Add(Me.nudProbe)
        Me.TabPageStage.Controls.Add(Me.GroupBox2)
        Me.TabPageStage.Controls.Add(Me.Label10)
        Me.TabPageStage.Controls.Add(Me.Label8)
        Me.TabPageStage.Controls.Add(Me.GroupBox5)
        Me.TabPageStage.Controls.Add(Me.Label1)
        Me.TabPageStage.Controls.Add(Me.btnJogSubAnglePbs)
        Me.TabPageStage.Controls.Add(Me.btnJogAddAnglePbs)
        Me.TabPageStage.Controls.Add(Me.nudJogAnglePbs)
        Me.TabPageStage.Controls.Add(Me.txtAnglePbs)
        Me.TabPageStage.Controls.Add(Me.btnMoveAnglePbs)
        Me.TabPageStage.Controls.Add(Me.nudAnglePbs)
        Me.TabPageStage.Controls.Add(Me.GroupBox1)
        Me.TabPageStage.Controls.Add(Me.btnJogPanel)
        Me.TabPageStage.Controls.Add(Me.btnStopMove)
        Me.TabPageStage.Controls.Add(Me.Label12)
        Me.TabPageStage.Controls.Add(Me.Label28)
        Me.TabPageStage.Controls.Add(Me.Label29)
        Me.TabPageStage.Controls.Add(Me.Label30)
        Me.TabPageStage.Controls.Add(Me.Label24)
        Me.TabPageStage.Controls.Add(Me.btnSync)
        Me.TabPageStage.Controls.Add(Me.btnMoveXYZ)
        Me.TabPageStage.Controls.Add(Me.btnJogSubAngleLens)
        Me.TabPageStage.Controls.Add(Me.btnJogSubBeamScanY)
        Me.TabPageStage.Controls.Add(Me.btnJogSubBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.btnJogSubBeamScanX)
        Me.TabPageStage.Controls.Add(Me.btnJogSubStageZ)
        Me.TabPageStage.Controls.Add(Me.btnJogSubStageY)
        Me.TabPageStage.Controls.Add(Me.btnJogAddAngleLens)
        Me.TabPageStage.Controls.Add(Me.btnJogAddBeamScanY)
        Me.TabPageStage.Controls.Add(Me.btnJogAddBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.btnJogAddBeamScanX)
        Me.TabPageStage.Controls.Add(Me.btnJogAddStageZ)
        Me.TabPageStage.Controls.Add(Me.btnJogAddStageY)
        Me.TabPageStage.Controls.Add(Me.btnJogSubStageX)
        Me.TabPageStage.Controls.Add(Me.btnJogAddStageX)
        Me.TabPageStage.Controls.Add(Me.nudJogAngleLens)
        Me.TabPageStage.Controls.Add(Me.nudJogBeamScanY)
        Me.TabPageStage.Controls.Add(Me.nudJogBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.nudJogBeamScanX)
        Me.TabPageStage.Controls.Add(Me.nudJogStageZ)
        Me.TabPageStage.Controls.Add(Me.nudJogStageY)
        Me.TabPageStage.Controls.Add(Me.nudJogStageX)
        Me.TabPageStage.Controls.Add(Me.Label43)
        Me.TabPageStage.Controls.Add(Me.txtAngleLens)
        Me.TabPageStage.Controls.Add(Me.txtBeamScanY)
        Me.TabPageStage.Controls.Add(Me.txtBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.txtBeamScanX)
        Me.TabPageStage.Controls.Add(Me.txtStageZ)
        Me.TabPageStage.Controls.Add(Me.txtStageY)
        Me.TabPageStage.Controls.Add(Me.txtStageX)
        Me.TabPageStage.Controls.Add(Me.Label42)
        Me.TabPageStage.Controls.Add(Me.Label41)
        Me.TabPageStage.Controls.Add(Me.btnMoveAngleLens)
        Me.TabPageStage.Controls.Add(Me.btnMoveBeamScanY)
        Me.TabPageStage.Controls.Add(Me.btnMoveBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.btnMoveBeamScanX)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageZ)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageY)
        Me.TabPageStage.Controls.Add(Me.btnMoveStageX)
        Me.TabPageStage.Controls.Add(Me.btnSaveConfiguredPositionNew)
        Me.TabPageStage.Controls.Add(Me.nudAngleLens)
        Me.TabPageStage.Controls.Add(Me.nudBeamScanY)
        Me.TabPageStage.Controls.Add(Me.nudBeamScanZ)
        Me.TabPageStage.Controls.Add(Me.btnDeleteConfiguredPosition)
        Me.TabPageStage.Controls.Add(Me.btnSaveConfiguredPosition)
        Me.TabPageStage.Controls.Add(Me.Label13)
        Me.TabPageStage.Controls.Add(Me.nudStageZ)
        Me.TabPageStage.Controls.Add(Me.nudBeamScanX)
        Me.TabPageStage.Controls.Add(Me.nudStageY)
        Me.TabPageStage.Controls.Add(Me.nudStageX)
        Me.TabPageStage.Location = New System.Drawing.Point(4, 29)
        Me.TabPageStage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPageStage.Name = "TabPageStage"
        Me.TabPageStage.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPageStage.Size = New System.Drawing.Size(1525, 750)
        Me.TabPageStage.TabIndex = 0
        Me.TabPageStage.Text = "Manual Align"
        Me.TabPageStage.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label32)
        Me.GroupBox7.Controls.Add(Me.nudLightSource22)
        Me.GroupBox7.Controls.Add(Me.Label34)
        Me.GroupBox7.Controls.Add(Me.nudLightSource21)
        Me.GroupBox7.Controls.Add(Me.Label33)
        Me.GroupBox7.Controls.Add(Me.nudLightSource12)
        Me.GroupBox7.Controls.Add(Me.Label27)
        Me.GroupBox7.Controls.Add(Me.nudLightSource11)
        Me.GroupBox7.Controls.Add(Me.chkLightSource11)
        Me.GroupBox7.Controls.Add(Me.chkLightSource22)
        Me.GroupBox7.Controls.Add(Me.chkLightSource12)
        Me.GroupBox7.Controls.Add(Me.chkLightSource21)
        Me.GroupBox7.Location = New System.Drawing.Point(1325, 55)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox7.Size = New System.Drawing.Size(189, 176)
        Me.GroupBox7.TabIndex = 186
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Light"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(7, 36)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(46, 20)
        Me.Label32.TabIndex = 180
        Me.Label32.Text = "CH1"
        '
        'nudLightSource22
        '
        Me.nudLightSource22.Location = New System.Drawing.Point(99, 132)
        Me.nudLightSource22.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudLightSource22.Name = "nudLightSource22"
        Me.nudLightSource22.Size = New System.Drawing.Size(81, 26)
        Me.nudLightSource22.TabIndex = 177
        Me.nudLightSource22.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(9, 140)
        Me.Label34.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(46, 20)
        Me.Label34.TabIndex = 179
        Me.Label34.Text = "CH4"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudLightSource21
        '
        Me.nudLightSource21.Location = New System.Drawing.Point(99, 99)
        Me.nudLightSource21.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudLightSource21.Name = "nudLightSource21"
        Me.nudLightSource21.Size = New System.Drawing.Size(81, 26)
        Me.nudLightSource21.TabIndex = 176
        Me.nudLightSource21.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(7, 105)
        Me.Label33.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(46, 20)
        Me.Label33.TabIndex = 178
        Me.Label33.Text = "CH3"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudLightSource12
        '
        Me.nudLightSource12.Location = New System.Drawing.Point(99, 65)
        Me.nudLightSource12.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudLightSource12.Name = "nudLightSource12"
        Me.nudLightSource12.Size = New System.Drawing.Size(81, 26)
        Me.nudLightSource12.TabIndex = 175
        Me.nudLightSource12.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(7, 69)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(46, 20)
        Me.Label27.TabIndex = 181
        Me.Label27.Text = "CH2"
        '
        'nudLightSource11
        '
        Me.nudLightSource11.Location = New System.Drawing.Point(99, 32)
        Me.nudLightSource11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudLightSource11.Name = "nudLightSource11"
        Me.nudLightSource11.Size = New System.Drawing.Size(81, 26)
        Me.nudLightSource11.TabIndex = 174
        Me.nudLightSource11.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'chkLightSource11
        '
        Me.chkLightSource11.AutoSize = True
        Me.chkLightSource11.Location = New System.Drawing.Point(71, 33)
        Me.chkLightSource11.Name = "chkLightSource11"
        Me.chkLightSource11.Size = New System.Drawing.Size(43, 24)
        Me.chkLightSource11.TabIndex = 182
        Me.chkLightSource11.Text = "  "
        Me.chkLightSource11.UseVisualStyleBackColor = True
        '
        'chkLightSource22
        '
        Me.chkLightSource22.AutoSize = True
        Me.chkLightSource22.Location = New System.Drawing.Point(71, 136)
        Me.chkLightSource22.Name = "chkLightSource22"
        Me.chkLightSource22.Size = New System.Drawing.Size(43, 24)
        Me.chkLightSource22.TabIndex = 185
        Me.chkLightSource22.Text = "  "
        Me.chkLightSource22.UseVisualStyleBackColor = True
        '
        'chkLightSource12
        '
        Me.chkLightSource12.AutoSize = True
        Me.chkLightSource12.Location = New System.Drawing.Point(71, 68)
        Me.chkLightSource12.Name = "chkLightSource12"
        Me.chkLightSource12.Size = New System.Drawing.Size(43, 24)
        Me.chkLightSource12.TabIndex = 183
        Me.chkLightSource12.Text = "  "
        Me.chkLightSource12.UseVisualStyleBackColor = True
        '
        'chkLightSource21
        '
        Me.chkLightSource21.AutoSize = True
        Me.chkLightSource21.Location = New System.Drawing.Point(71, 101)
        Me.chkLightSource21.Name = "chkLightSource21"
        Me.chkLightSource21.Size = New System.Drawing.Size(43, 24)
        Me.chkLightSource21.TabIndex = 184
        Me.chkLightSource21.Text = "  "
        Me.chkLightSource21.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnSaveGoldImage)
        Me.GroupBox3.Controls.Add(Me.lstGoldImage)
        Me.GroupBox3.Location = New System.Drawing.Point(1027, 55)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox3.Size = New System.Drawing.Size(291, 366)
        Me.GroupBox3.TabIndex = 169
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Gold Image"
        '
        'btnSaveGoldImage
        '
        Me.btnSaveGoldImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveGoldImage.Location = New System.Drawing.Point(159, 21)
        Me.btnSaveGoldImage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveGoldImage.Name = "btnSaveGoldImage"
        Me.btnSaveGoldImage.Size = New System.Drawing.Size(117, 35)
        Me.btnSaveGoldImage.TabIndex = 133
        Me.btnSaveGoldImage.Text = "Save"
        Me.btnSaveGoldImage.UseVisualStyleBackColor = True
        '
        'lstGoldImage
        '
        Me.lstGoldImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstGoldImage.FormattingEnabled = True
        Me.lstGoldImage.IntegralHeight = False
        Me.lstGoldImage.ItemHeight = 20
        Me.lstGoldImage.Location = New System.Drawing.Point(11, 64)
        Me.lstGoldImage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.lstGoldImage.Name = "lstGoldImage"
        Me.lstGoldImage.Size = New System.Drawing.Size(265, 284)
        Me.lstGoldImage.TabIndex = 81
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.nudUvPower)
        Me.GroupBox4.Controls.Add(Me.Label23)
        Me.GroupBox4.Controls.Add(Me.ooUvLamp)
        Me.GroupBox4.Controls.Add(Me.btnStop)
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Controls.Add(Me.nudUvTime)
        Me.GroupBox4.Controls.Add(Me.Label22)
        Me.GroupBox4.Controls.Add(Me.btnUV)
        Me.GroupBox4.Location = New System.Drawing.Point(712, 550)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox4.Size = New System.Drawing.Size(307, 181)
        Me.GroupBox4.TabIndex = 168
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "UV Process"
        '
        'nudUvPower
        '
        Me.nudUvPower.Location = New System.Drawing.Point(229, 77)
        Me.nudUvPower.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudUvPower.Name = "nudUvPower"
        Me.nudUvPower.Size = New System.Drawing.Size(68, 26)
        Me.nudUvPower.TabIndex = 28
        Me.nudUvPower.Value = New Decimal(New Integer() {102, 0, 0, 65536})
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(161, 81)
        Me.Label23.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(61, 20)
        Me.Label23.TabIndex = 27
        Me.Label23.Text = "Power"
        '
        'ooUvLamp
        '
        Me.ooUvLamp.IconBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ooUvLamp.IconScale = 0.5R
        Me.ooUvLamp.Location = New System.Drawing.Point(108, 27)
        Me.ooUvLamp.MaximumSize = New System.Drawing.Size(133, 31)
        Me.ooUvLamp.MinimumSize = New System.Drawing.Size(133, 31)
        Me.ooUvLamp.Name = "ooUvLamp"
        Me.ooUvLamp.Size = New System.Drawing.Size(133, 31)
        Me.ooUvLamp.State = BlackHawk.ucOnOff.StateEnum.Unknown
        Me.ooUvLamp.TabIndex = 26
        '
        'btnStop
        '
        Me.btnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStop.Location = New System.Drawing.Point(177, 123)
        Me.btnStop.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(93, 36)
        Me.btnStop.TabIndex = 25
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(15, 35)
        Me.Label21.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(61, 20)
        Me.Label21.TabIndex = 19
        Me.Label21.Text = "Lamp "
        '
        'nudUvTime
        '
        Me.nudUvTime.DecimalPlaces = 1
        Me.nudUvTime.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudUvTime.Location = New System.Drawing.Point(84, 77)
        Me.nudUvTime.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudUvTime.Name = "nudUvTime"
        Me.nudUvTime.Size = New System.Drawing.Size(68, 26)
        Me.nudUvTime.TabIndex = 11
        Me.nudUvTime.Value = New Decimal(New Integer() {102, 0, 0, 65536})
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(8, 81)
        Me.Label22.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(70, 20)
        Me.Label22.TabIndex = 10
        Me.Label22.Text = "Time_s"
        '
        'btnUV
        '
        Me.btnUV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUV.Location = New System.Drawing.Point(19, 123)
        Me.btnUV.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnUV.Name = "btnUV"
        Me.btnUV.Size = New System.Drawing.Size(93, 36)
        Me.btnUV.TabIndex = 9
        Me.btnUV.Text = "Start"
        Me.btnUV.UseVisualStyleBackColor = True
        '
        'lstConfiguredPositions
        '
        Me.lstConfiguredPositions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstConfiguredPositions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstConfiguredPositions.FormattingEnabled = True
        Me.lstConfiguredPositions.IntegralHeight = False
        Me.lstConfiguredPositions.ItemHeight = 20
        Me.lstConfiguredPositions.Location = New System.Drawing.Point(0, 39)
        Me.lstConfiguredPositions.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.lstConfiguredPositions.Name = "lstConfiguredPositions"
        Me.lstConfiguredPositions.ScrollAlwaysVisible = True
        Me.lstConfiguredPositions.Size = New System.Drawing.Size(259, 1316)
        Me.lstConfiguredPositions.TabIndex = 80
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(283, 291)
        Me.Label20.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(58, 20)
        Me.Label20.TabIndex = 167
        Me.Label20.Text = "Probe"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnJogSubProbe
        '
        Me.btnJogSubProbe.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubProbe.Location = New System.Drawing.Point(945, 284)
        Me.btnJogSubProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubProbe.Name = "btnJogSubProbe"
        Me.btnJogSubProbe.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubProbe.TabIndex = 166
        Me.btnJogSubProbe.Text = "Sub"
        Me.btnJogSubProbe.UseVisualStyleBackColor = True
        '
        'btnJogAddProbe
        '
        Me.btnJogAddProbe.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddProbe.Location = New System.Drawing.Point(876, 284)
        Me.btnJogAddProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddProbe.Name = "btnJogAddProbe"
        Me.btnJogAddProbe.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddProbe.TabIndex = 165
        Me.btnJogAddProbe.Text = "Add"
        Me.btnJogAddProbe.UseVisualStyleBackColor = True
        '
        'nudJogProbe
        '
        Me.nudJogProbe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nudJogProbe.DecimalPlaces = 4
        Me.nudJogProbe.Location = New System.Drawing.Point(775, 285)
        Me.nudJogProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogProbe.Name = "nudJogProbe"
        Me.nudJogProbe.Size = New System.Drawing.Size(92, 26)
        Me.nudJogProbe.TabIndex = 164
        Me.nudJogProbe.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'txtProbe
        '
        Me.txtProbe.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtProbe.Location = New System.Drawing.Point(423, 285)
        Me.txtProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtProbe.Name = "txtProbe"
        Me.txtProbe.Size = New System.Drawing.Size(108, 26)
        Me.txtProbe.TabIndex = 163
        '
        'btnMoveProbe
        '
        Me.btnMoveProbe.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveProbe.Location = New System.Drawing.Point(672, 284)
        Me.btnMoveProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveProbe.Name = "btnMoveProbe"
        Me.btnMoveProbe.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveProbe.TabIndex = 162
        Me.btnMoveProbe.Text = "Move"
        Me.btnMoveProbe.UseVisualStyleBackColor = True
        '
        'nudProbe
        '
        Me.nudProbe.Location = New System.Drawing.Point(543, 285)
        Me.nudProbe.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudProbe.Name = "nudProbe"
        Me.nudProbe.Size = New System.Drawing.Size(121, 26)
        Me.nudProbe.TabIndex = 161
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_6)
        Me.GroupBox2.Controls.Add(Me.Label18)
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_5)
        Me.GroupBox2.Controls.Add(Me.Label19)
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_4)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_3)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_2)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.chkGPIO3_1)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Location = New System.Drawing.Point(712, 440)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox2.Size = New System.Drawing.Size(307, 92)
        Me.GroupBox2.TabIndex = 160
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "XPS IO"
        '
        'chkGPIO3_6
        '
        Me.chkGPIO3_6.AutoSize = True
        Me.chkGPIO3_6.Location = New System.Drawing.Point(261, 47)
        Me.chkGPIO3_6.Name = "chkGPIO3_6"
        Me.chkGPIO3_6.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_6.TabIndex = 182
        Me.chkGPIO3_6.Text = "  "
        Me.chkGPIO3_6.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(253, 23)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(34, 20)
        Me.Label18.TabIndex = 181
        Me.Label18.Text = "3.6"
        '
        'chkGPIO3_5
        '
        Me.chkGPIO3_5.AutoSize = True
        Me.chkGPIO3_5.Location = New System.Drawing.Point(213, 47)
        Me.chkGPIO3_5.Name = "chkGPIO3_5"
        Me.chkGPIO3_5.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_5.TabIndex = 180
        Me.chkGPIO3_5.Text = "  "
        Me.chkGPIO3_5.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(204, 24)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(34, 20)
        Me.Label19.TabIndex = 179
        Me.Label19.Text = "3.5"
        '
        'chkGPIO3_4
        '
        Me.chkGPIO3_4.AutoSize = True
        Me.chkGPIO3_4.Location = New System.Drawing.Point(164, 47)
        Me.chkGPIO3_4.Name = "chkGPIO3_4"
        Me.chkGPIO3_4.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_4.TabIndex = 178
        Me.chkGPIO3_4.Text = "  "
        Me.chkGPIO3_4.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(155, 24)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(34, 20)
        Me.Label16.TabIndex = 177
        Me.Label16.Text = "3.4"
        '
        'chkGPIO3_3
        '
        Me.chkGPIO3_3.AutoSize = True
        Me.chkGPIO3_3.Location = New System.Drawing.Point(115, 47)
        Me.chkGPIO3_3.Name = "chkGPIO3_3"
        Me.chkGPIO3_3.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_3.TabIndex = 176
        Me.chkGPIO3_3.Text = "  "
        Me.chkGPIO3_3.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(107, 24)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(34, 20)
        Me.Label17.TabIndex = 175
        Me.Label17.Text = "3.3"
        '
        'chkGPIO3_2
        '
        Me.chkGPIO3_2.AutoSize = True
        Me.chkGPIO3_2.Location = New System.Drawing.Point(67, 47)
        Me.chkGPIO3_2.Name = "chkGPIO3_2"
        Me.chkGPIO3_2.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_2.TabIndex = 174
        Me.chkGPIO3_2.Text = "  "
        Me.chkGPIO3_2.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(57, 24)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(34, 20)
        Me.Label14.TabIndex = 173
        Me.Label14.Text = "3.2"
        '
        'chkGPIO3_1
        '
        Me.chkGPIO3_1.AutoSize = True
        Me.chkGPIO3_1.Location = New System.Drawing.Point(17, 47)
        Me.chkGPIO3_1.Name = "chkGPIO3_1"
        Me.chkGPIO3_1.Size = New System.Drawing.Size(43, 24)
        Me.chkGPIO3_1.TabIndex = 172
        Me.chkGPIO3_1.Text = "  "
        Me.chkGPIO3_1.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 24)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 20)
        Me.Label11.TabIndex = 171
        Me.Label11.Text = "3.1"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(283, 119)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 20)
        Me.Label10.TabIndex = 159
        Me.Label10.Text = "Main Stage Y"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(283, 85)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(121, 20)
        Me.Label8.TabIndex = 158
        Me.Label8.Text = "Main Stage X"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkVacuumPbs)
        Me.GroupBox5.Controls.Add(Me.chkVacuumLens)
        Me.GroupBox5.Controls.Add(Me.txtCdaLine)
        Me.GroupBox5.Controls.Add(Me.btnVacuumCda)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.chkVacuumCda)
        Me.GroupBox5.Controls.Add(Me.txtVacuumLine)
        Me.GroupBox5.Controls.Add(Me.Label7)
        Me.GroupBox5.Controls.Add(Me.btnEpoxyTrigger)
        Me.GroupBox5.Controls.Add(Me.chkEpoxy)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Controls.Add(Me.Label4)
        Me.GroupBox5.Controls.Add(Me.Label2)
        Me.GroupBox5.Controls.Add(Me.Label31)
        Me.GroupBox5.Controls.Add(Me.txtVacuumPbs)
        Me.GroupBox5.Controls.Add(Me.txtVacuumLens)
        Me.GroupBox5.Controls.Add(Me.txtForcePbs)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.txtForceLens)
        Me.GroupBox5.Location = New System.Drawing.Point(289, 440)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox5.Size = New System.Drawing.Size(413, 291)
        Me.GroupBox5.TabIndex = 157
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Pickup Tool"
        '
        'chkVacuumPbs
        '
        Me.chkVacuumPbs.AutoSize = True
        Me.chkVacuumPbs.Location = New System.Drawing.Point(337, 39)
        Me.chkVacuumPbs.Name = "chkVacuumPbs"
        Me.chkVacuumPbs.Size = New System.Drawing.Size(43, 24)
        Me.chkVacuumPbs.TabIndex = 171
        Me.chkVacuumPbs.Text = "  "
        Me.chkVacuumPbs.UseVisualStyleBackColor = True
        '
        'chkVacuumLens
        '
        Me.chkVacuumLens.AutoSize = True
        Me.chkVacuumLens.Location = New System.Drawing.Point(217, 39)
        Me.chkVacuumLens.Name = "chkVacuumLens"
        Me.chkVacuumLens.Size = New System.Drawing.Size(43, 24)
        Me.chkVacuumLens.TabIndex = 170
        Me.chkVacuumLens.Text = "  "
        Me.chkVacuumLens.UseVisualStyleBackColor = True
        '
        'txtCdaLine
        '
        Me.txtCdaLine.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtCdaLine.Location = New System.Drawing.Point(171, 253)
        Me.txtCdaLine.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtCdaLine.Name = "txtCdaLine"
        Me.txtCdaLine.Size = New System.Drawing.Size(108, 26)
        Me.txtCdaLine.TabIndex = 161
        '
        'btnVacuumCda
        '
        Me.btnVacuumCda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVacuumCda.Location = New System.Drawing.Point(293, 147)
        Me.btnVacuumCda.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnVacuumCda.Name = "btnVacuumCda"
        Me.btnVacuumCda.Size = New System.Drawing.Size(108, 31)
        Me.btnVacuumCda.TabIndex = 169
        Me.btnVacuumCda.Text = "Clean Line"
        Me.btnVacuumCda.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 256)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(127, 20)
        Me.Label9.TabIndex = 160
        Me.Label9.Text = "Line Pressure"
        '
        'chkVacuumCda
        '
        Me.chkVacuumCda.AutoSize = True
        Me.chkVacuumCda.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkVacuumCda.Location = New System.Drawing.Point(11, 151)
        Me.chkVacuumCda.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkVacuumCda.Name = "chkVacuumCda"
        Me.chkVacuumCda.Size = New System.Drawing.Size(241, 24)
        Me.chkVacuumCda.TabIndex = 168
        Me.chkVacuumCda.Text = "Vacuum Line With CDA  "
        Me.chkVacuumCda.UseVisualStyleBackColor = True
        '
        'txtVacuumLine
        '
        Me.txtVacuumLine.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtVacuumLine.Location = New System.Drawing.Point(171, 219)
        Me.txtVacuumLine.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtVacuumLine.Name = "txtVacuumLine"
        Me.txtVacuumLine.Size = New System.Drawing.Size(108, 26)
        Me.txtVacuumLine.TabIndex = 158
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 221)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 20)
        Me.Label7.TabIndex = 159
        Me.Label7.Text = "Line Vacuum"
        '
        'btnEpoxyTrigger
        '
        Me.btnEpoxyTrigger.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEpoxyTrigger.Location = New System.Drawing.Point(293, 183)
        Me.btnEpoxyTrigger.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnEpoxyTrigger.Name = "btnEpoxyTrigger"
        Me.btnEpoxyTrigger.Size = New System.Drawing.Size(108, 31)
        Me.btnEpoxyTrigger.TabIndex = 167
        Me.btnEpoxyTrigger.Text = "Dispense"
        Me.btnEpoxyTrigger.UseVisualStyleBackColor = True
        '
        'chkEpoxy
        '
        Me.chkEpoxy.AutoSize = True
        Me.chkEpoxy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEpoxy.Location = New System.Drawing.Point(11, 185)
        Me.chkEpoxy.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkEpoxy.Name = "chkEpoxy"
        Me.chkEpoxy.Size = New System.Drawing.Size(245, 24)
        Me.chkEpoxy.TabIndex = 166
        Me.chkEpoxy.Text = "Epoxy: Move Out            "
        Me.chkEpoxy.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 35)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(138, 20)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Vacuum On/Off"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(303, 15)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 20)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Hexapod"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(199, 15)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 20)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Lens"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(11, 71)
        Me.Label31.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(136, 20)
        Me.Label31.TabIndex = 20
        Me.Label31.Text = "Pressure (kPa)"
        '
        'txtVacuumPbs
        '
        Me.txtVacuumPbs.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtVacuumPbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtVacuumPbs.Location = New System.Drawing.Point(293, 67)
        Me.txtVacuumPbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtVacuumPbs.Name = "txtVacuumPbs"
        Me.txtVacuumPbs.Size = New System.Drawing.Size(109, 26)
        Me.txtVacuumPbs.TabIndex = 21
        '
        'txtVacuumLens
        '
        Me.txtVacuumLens.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtVacuumLens.Location = New System.Drawing.Point(171, 67)
        Me.txtVacuumLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtVacuumLens.Name = "txtVacuumLens"
        Me.txtVacuumLens.Size = New System.Drawing.Size(108, 26)
        Me.txtVacuumLens.TabIndex = 19
        '
        'txtForcePbs
        '
        Me.txtForcePbs.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtForcePbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtForcePbs.Location = New System.Drawing.Point(293, 107)
        Me.txtForcePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtForcePbs.Name = "txtForcePbs"
        Me.txtForcePbs.Size = New System.Drawing.Size(109, 26)
        Me.txtForcePbs.TabIndex = 17
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(11, 109)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(106, 20)
        Me.Label15.TabIndex = 11
        Me.Label15.Text = "Force Z (N)"
        '
        'txtForceLens
        '
        Me.txtForceLens.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtForceLens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtForceLens.Location = New System.Drawing.Point(171, 107)
        Me.txtForceLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtForceLens.Name = "txtForceLens"
        Me.txtForceLens.Size = New System.Drawing.Size(109, 26)
        Me.txtForceLens.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(284, 360)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 20)
        Me.Label1.TabIndex = 156
        Me.Label1.Text = "Angle, Hexa"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnJogSubAnglePbs
        '
        Me.btnJogSubAnglePbs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubAnglePbs.Location = New System.Drawing.Point(947, 356)
        Me.btnJogSubAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubAnglePbs.Name = "btnJogSubAnglePbs"
        Me.btnJogSubAnglePbs.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubAnglePbs.TabIndex = 155
        Me.btnJogSubAnglePbs.Text = "Sub"
        Me.btnJogSubAnglePbs.UseVisualStyleBackColor = True
        '
        'btnJogAddAnglePbs
        '
        Me.btnJogAddAnglePbs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddAnglePbs.Location = New System.Drawing.Point(877, 356)
        Me.btnJogAddAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddAnglePbs.Name = "btnJogAddAnglePbs"
        Me.btnJogAddAnglePbs.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddAnglePbs.TabIndex = 154
        Me.btnJogAddAnglePbs.Text = "Add"
        Me.btnJogAddAnglePbs.UseVisualStyleBackColor = True
        '
        'nudJogAnglePbs
        '
        Me.nudJogAnglePbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nudJogAnglePbs.DecimalPlaces = 4
        Me.nudJogAnglePbs.Location = New System.Drawing.Point(776, 356)
        Me.nudJogAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogAnglePbs.Name = "nudJogAnglePbs"
        Me.nudJogAnglePbs.Size = New System.Drawing.Size(92, 26)
        Me.nudJogAnglePbs.TabIndex = 153
        Me.nudJogAnglePbs.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'txtAnglePbs
        '
        Me.txtAnglePbs.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtAnglePbs.Location = New System.Drawing.Point(424, 355)
        Me.txtAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtAnglePbs.Name = "txtAnglePbs"
        Me.txtAnglePbs.Size = New System.Drawing.Size(108, 26)
        Me.txtAnglePbs.TabIndex = 152
        '
        'btnMoveAnglePbs
        '
        Me.btnMoveAnglePbs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveAnglePbs.Location = New System.Drawing.Point(673, 356)
        Me.btnMoveAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveAnglePbs.Name = "btnMoveAnglePbs"
        Me.btnMoveAnglePbs.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveAnglePbs.TabIndex = 151
        Me.btnMoveAnglePbs.Text = "Move"
        Me.btnMoveAnglePbs.UseVisualStyleBackColor = True
        '
        'nudAnglePbs
        '
        Me.nudAnglePbs.Location = New System.Drawing.Point(544, 356)
        Me.nudAnglePbs.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudAnglePbs.Name = "nudAnglePbs"
        Me.nudAnglePbs.Size = New System.Drawing.Size(121, 26)
        Me.nudAnglePbs.TabIndex = 150
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkLddProtection)
        Me.GroupBox1.Controls.Add(Me.txtTemperature)
        Me.GroupBox1.Controls.Add(Me.Label26)
        Me.GroupBox1.Controls.Add(Me.nudTemperature)
        Me.GroupBox1.Controls.Add(Me.Label25)
        Me.GroupBox1.Controls.Add(Me.chkProbeCdaOn)
        Me.GroupBox1.Controls.Add(Me.btnTrunLddOff)
        Me.GroupBox1.Controls.Add(Me.btnClampORG)
        Me.GroupBox1.Controls.Add(Me.chkVacuumPackage)
        Me.GroupBox1.Controls.Add(Me.chkProbeClampEnable)
        Me.GroupBox1.Controls.Add(Me.chkProbeClampClose)
        Me.GroupBox1.Controls.Add(Me.txtVoltage)
        Me.GroupBox1.Controls.Add(Me.optCH4)
        Me.GroupBox1.Controls.Add(Me.optCH3)
        Me.GroupBox1.Controls.Add(Me.optCH2)
        Me.GroupBox1.Controls.Add(Me.optCH1)
        Me.GroupBox1.Controls.Add(Me.nudCurrent)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(1027, 440)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox1.Size = New System.Drawing.Size(480, 291)
        Me.GroupBox1.TabIndex = 149
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Package"
        '
        'chkLddProtection
        '
        Me.chkLddProtection.AutoSize = True
        Me.chkLddProtection.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.chkLddProtection.Location = New System.Drawing.Point(340, 32)
        Me.chkLddProtection.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkLddProtection.Name = "chkLddProtection"
        Me.chkLddProtection.Size = New System.Drawing.Size(114, 24)
        Me.chkLddProtection.TabIndex = 175
        Me.chkLddProtection.Text = "Protection"
        Me.chkLddProtection.UseVisualStyleBackColor = True
        '
        'txtTemperature
        '
        Me.txtTemperature.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtTemperature.Location = New System.Drawing.Point(380, 111)
        Me.txtTemperature.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtTemperature.Name = "txtTemperature"
        Me.txtTemperature.Size = New System.Drawing.Size(85, 26)
        Me.txtTemperature.TabIndex = 174
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(248, 115)
        Me.Label26.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(112, 20)
        Me.Label26.TabIndex = 173
        Me.Label26.Text = "T Read (oC)"
        '
        'nudTemperature
        '
        Me.nudTemperature.DecimalPlaces = 2
        Me.nudTemperature.Location = New System.Drawing.Point(145, 112)
        Me.nudTemperature.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudTemperature.Name = "nudTemperature"
        Me.nudTemperature.Size = New System.Drawing.Size(91, 26)
        Me.nudTemperature.TabIndex = 172
        Me.nudTemperature.Value = New Decimal(New Integer() {9999, 0, 0, 131072})
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(11, 115)
        Me.Label25.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(97, 20)
        Me.Label25.TabIndex = 171
        Me.Label25.Text = "T Set (oC)"
        '
        'chkProbeCdaOn
        '
        Me.chkProbeCdaOn.AutoSize = True
        Me.chkProbeCdaOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProbeCdaOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkProbeCdaOn.Location = New System.Drawing.Point(11, 241)
        Me.chkProbeCdaOn.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkProbeCdaOn.Name = "chkProbeCdaOn"
        Me.chkProbeCdaOn.Size = New System.Drawing.Size(208, 24)
        Me.chkProbeCdaOn.TabIndex = 170
        Me.chkProbeCdaOn.Text = "Push Probe Stage     "
        Me.chkProbeCdaOn.UseVisualStyleBackColor = True
        '
        'btnTrunLddOff
        '
        Me.btnTrunLddOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTrunLddOff.Location = New System.Drawing.Point(67, 151)
        Me.btnTrunLddOff.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnTrunLddOff.Name = "btnTrunLddOff"
        Me.btnTrunLddOff.Size = New System.Drawing.Size(116, 31)
        Me.btnTrunLddOff.TabIndex = 169
        Me.btnTrunLddOff.Text = "LDD OFF"
        Me.btnTrunLddOff.UseVisualStyleBackColor = True
        '
        'btnClampORG
        '
        Me.btnClampORG.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClampORG.Location = New System.Drawing.Point(295, 151)
        Me.btnClampORG.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnClampORG.Name = "btnClampORG"
        Me.btnClampORG.Size = New System.Drawing.Size(116, 31)
        Me.btnClampORG.TabIndex = 169
        Me.btnClampORG.Text = "ORG"
        Me.btnClampORG.UseVisualStyleBackColor = True
        '
        'chkVacuumPackage
        '
        Me.chkVacuumPackage.AutoSize = True
        Me.chkVacuumPackage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkVacuumPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkVacuumPackage.Location = New System.Drawing.Point(251, 241)
        Me.chkVacuumPackage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkVacuumPackage.Name = "chkVacuumPackage"
        Me.chkVacuumPackage.Size = New System.Drawing.Size(205, 24)
        Me.chkVacuumPackage.TabIndex = 168
        Me.chkVacuumPackage.Text = "Vacuum For Package"
        Me.chkVacuumPackage.UseVisualStyleBackColor = True
        '
        'chkProbeClampEnable
        '
        Me.chkProbeClampEnable.AutoSize = True
        Me.chkProbeClampEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProbeClampEnable.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.chkProbeClampEnable.Location = New System.Drawing.Point(11, 209)
        Me.chkProbeClampEnable.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkProbeClampEnable.Name = "chkProbeClampEnable"
        Me.chkProbeClampEnable.Size = New System.Drawing.Size(206, 24)
        Me.chkProbeClampEnable.TabIndex = 167
        Me.chkProbeClampEnable.Text = "Enable Auto Clamp   "
        Me.chkProbeClampEnable.UseVisualStyleBackColor = True
        '
        'chkProbeClampClose
        '
        Me.chkProbeClampClose.AutoSize = True
        Me.chkProbeClampClose.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkProbeClampClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkProbeClampClose.Location = New System.Drawing.Point(252, 209)
        Me.chkProbeClampClose.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.chkProbeClampClose.Name = "chkProbeClampClose"
        Me.chkProbeClampClose.Size = New System.Drawing.Size(207, 24)
        Me.chkProbeClampClose.TabIndex = 163
        Me.chkProbeClampClose.Text = "Close Probe Clamp   "
        Me.chkProbeClampClose.UseVisualStyleBackColor = True
        '
        'txtVoltage
        '
        Me.txtVoltage.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtVoltage.Location = New System.Drawing.Point(380, 69)
        Me.txtVoltage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtVoltage.Name = "txtVoltage"
        Me.txtVoltage.Size = New System.Drawing.Size(85, 26)
        Me.txtVoltage.TabIndex = 161
        '
        'optCH4
        '
        Me.optCH4.AutoSize = True
        Me.optCH4.Location = New System.Drawing.Point(231, 35)
        Me.optCH4.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.optCH4.Name = "optCH4"
        Me.optCH4.Size = New System.Drawing.Size(67, 24)
        Me.optCH4.TabIndex = 160
        Me.optCH4.TabStop = True
        Me.optCH4.Text = "CH4"
        Me.optCH4.UseVisualStyleBackColor = True
        '
        'optCH3
        '
        Me.optCH3.AutoSize = True
        Me.optCH3.Location = New System.Drawing.Point(159, 35)
        Me.optCH3.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.optCH3.Name = "optCH3"
        Me.optCH3.Size = New System.Drawing.Size(67, 24)
        Me.optCH3.TabIndex = 159
        Me.optCH3.TabStop = True
        Me.optCH3.Text = "CH3"
        Me.optCH3.UseVisualStyleBackColor = True
        '
        'optCH2
        '
        Me.optCH2.AutoSize = True
        Me.optCH2.Location = New System.Drawing.Point(87, 35)
        Me.optCH2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.optCH2.Name = "optCH2"
        Me.optCH2.Size = New System.Drawing.Size(67, 24)
        Me.optCH2.TabIndex = 158
        Me.optCH2.TabStop = True
        Me.optCH2.Text = "CH2"
        Me.optCH2.UseVisualStyleBackColor = True
        '
        'optCH1
        '
        Me.optCH1.AutoSize = True
        Me.optCH1.Location = New System.Drawing.Point(15, 35)
        Me.optCH1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.optCH1.Name = "optCH1"
        Me.optCH1.Size = New System.Drawing.Size(67, 24)
        Me.optCH1.TabIndex = 157
        Me.optCH1.TabStop = True
        Me.optCH1.Text = "CH1"
        Me.optCH1.UseVisualStyleBackColor = True
        '
        'nudCurrent
        '
        Me.nudCurrent.DecimalPlaces = 2
        Me.nudCurrent.Location = New System.Drawing.Point(145, 68)
        Me.nudCurrent.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudCurrent.Name = "nudCurrent"
        Me.nudCurrent.Size = New System.Drawing.Size(89, 26)
        Me.nudCurrent.TabIndex = 21
        Me.nudCurrent.Value = New Decimal(New Integer() {9999, 0, 0, 131072})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 73)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(125, 20)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Current (mA) "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(252, 73)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 20)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Voltage (V) "
        '
        'btnJogPanel
        '
        Me.btnJogPanel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogPanel.Location = New System.Drawing.Point(865, 8)
        Me.btnJogPanel.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogPanel.Name = "btnJogPanel"
        Me.btnJogPanel.Size = New System.Drawing.Size(144, 35)
        Me.btnJogPanel.TabIndex = 148
        Me.btnJogPanel.Text = "Jog Panel"
        Me.btnJogPanel.UseVisualStyleBackColor = True
        '
        'btnStopMove
        '
        Me.btnStopMove.BackColor = System.Drawing.Color.Red
        Me.btnStopMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopMove.ForeColor = System.Drawing.Color.Yellow
        Me.btnStopMove.Location = New System.Drawing.Point(1015, 8)
        Me.btnStopMove.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnStopMove.Name = "btnStopMove"
        Me.btnStopMove.Size = New System.Drawing.Size(144, 35)
        Me.btnStopMove.TabIndex = 141
        Me.btnStopMove.Text = "Stop Move"
        Me.btnStopMove.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(283, 155)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(119, 20)
        Me.Label12.TabIndex = 134
        Me.Label12.Text = "Main Stage Z"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(285, 189)
        Me.Label28.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(123, 20)
        Me.Label28.TabIndex = 136
        Me.Label28.Text = "Beam Scan X"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(283, 253)
        Me.Label29.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(121, 20)
        Me.Label29.TabIndex = 137
        Me.Label29.Text = "Beam Scan Z"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(283, 221)
        Me.Label30.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(122, 20)
        Me.Label30.TabIndex = 139
        Me.Label30.Text = "Beam Scan Y"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(284, 323)
        Me.Label24.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(107, 20)
        Me.Label24.TabIndex = 140
        Me.Label24.Text = "Angle, Main"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSync
        '
        Me.btnSync.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSync.Location = New System.Drawing.Point(1164, 8)
        Me.btnSync.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(144, 35)
        Me.btnSync.TabIndex = 132
        Me.btnSync.Text = "Refresh"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'btnMoveXYZ
        '
        Me.btnMoveXYZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveXYZ.Location = New System.Drawing.Point(716, 8)
        Me.btnMoveXYZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveXYZ.Name = "btnMoveXYZ"
        Me.btnMoveXYZ.Size = New System.Drawing.Size(144, 35)
        Me.btnMoveXYZ.TabIndex = 131
        Me.btnMoveXYZ.Text = "Move XYZ"
        Me.btnMoveXYZ.UseVisualStyleBackColor = True
        '
        'btnJogSubAngleLens
        '
        Me.btnJogSubAngleLens.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubAngleLens.Location = New System.Drawing.Point(947, 319)
        Me.btnJogSubAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubAngleLens.Name = "btnJogSubAngleLens"
        Me.btnJogSubAngleLens.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubAngleLens.TabIndex = 129
        Me.btnJogSubAngleLens.Text = "Sub"
        Me.btnJogSubAngleLens.UseVisualStyleBackColor = True
        '
        'btnJogSubBeamScanY
        '
        Me.btnJogSubBeamScanY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubBeamScanY.Location = New System.Drawing.Point(945, 216)
        Me.btnJogSubBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubBeamScanY.Name = "btnJogSubBeamScanY"
        Me.btnJogSubBeamScanY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubBeamScanY.TabIndex = 127
        Me.btnJogSubBeamScanY.Text = "Sub"
        Me.btnJogSubBeamScanY.UseVisualStyleBackColor = True
        '
        'btnJogSubBeamScanZ
        '
        Me.btnJogSubBeamScanZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubBeamScanZ.Location = New System.Drawing.Point(945, 249)
        Me.btnJogSubBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubBeamScanZ.Name = "btnJogSubBeamScanZ"
        Me.btnJogSubBeamScanZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubBeamScanZ.TabIndex = 126
        Me.btnJogSubBeamScanZ.Text = "Sub"
        Me.btnJogSubBeamScanZ.UseVisualStyleBackColor = True
        '
        'btnJogSubBeamScanX
        '
        Me.btnJogSubBeamScanX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubBeamScanX.Location = New System.Drawing.Point(945, 184)
        Me.btnJogSubBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubBeamScanX.Name = "btnJogSubBeamScanX"
        Me.btnJogSubBeamScanX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubBeamScanX.TabIndex = 125
        Me.btnJogSubBeamScanX.Text = "Sub"
        Me.btnJogSubBeamScanX.UseVisualStyleBackColor = True
        '
        'btnJogSubStageZ
        '
        Me.btnJogSubStageZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubStageZ.Location = New System.Drawing.Point(945, 151)
        Me.btnJogSubStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubStageZ.Name = "btnJogSubStageZ"
        Me.btnJogSubStageZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubStageZ.TabIndex = 124
        Me.btnJogSubStageZ.Text = "Sub"
        Me.btnJogSubStageZ.UseVisualStyleBackColor = True
        '
        'btnJogSubStageY
        '
        Me.btnJogSubStageY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubStageY.Location = New System.Drawing.Point(945, 115)
        Me.btnJogSubStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubStageY.Name = "btnJogSubStageY"
        Me.btnJogSubStageY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubStageY.TabIndex = 123
        Me.btnJogSubStageY.Text = "Sub"
        Me.btnJogSubStageY.UseVisualStyleBackColor = True
        '
        'btnJogAddAngleLens
        '
        Me.btnJogAddAngleLens.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddAngleLens.Location = New System.Drawing.Point(877, 319)
        Me.btnJogAddAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddAngleLens.Name = "btnJogAddAngleLens"
        Me.btnJogAddAngleLens.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddAngleLens.TabIndex = 122
        Me.btnJogAddAngleLens.Text = "Add"
        Me.btnJogAddAngleLens.UseVisualStyleBackColor = True
        '
        'btnJogAddBeamScanY
        '
        Me.btnJogAddBeamScanY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddBeamScanY.Location = New System.Drawing.Point(876, 216)
        Me.btnJogAddBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddBeamScanY.Name = "btnJogAddBeamScanY"
        Me.btnJogAddBeamScanY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddBeamScanY.TabIndex = 120
        Me.btnJogAddBeamScanY.Text = "Add"
        Me.btnJogAddBeamScanY.UseVisualStyleBackColor = True
        '
        'btnJogAddBeamScanZ
        '
        Me.btnJogAddBeamScanZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddBeamScanZ.Location = New System.Drawing.Point(876, 249)
        Me.btnJogAddBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddBeamScanZ.Name = "btnJogAddBeamScanZ"
        Me.btnJogAddBeamScanZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddBeamScanZ.TabIndex = 119
        Me.btnJogAddBeamScanZ.Text = "Add"
        Me.btnJogAddBeamScanZ.UseVisualStyleBackColor = True
        '
        'btnJogAddBeamScanX
        '
        Me.btnJogAddBeamScanX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddBeamScanX.Location = New System.Drawing.Point(876, 184)
        Me.btnJogAddBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddBeamScanX.Name = "btnJogAddBeamScanX"
        Me.btnJogAddBeamScanX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddBeamScanX.TabIndex = 118
        Me.btnJogAddBeamScanX.Text = "Add"
        Me.btnJogAddBeamScanX.UseVisualStyleBackColor = True
        '
        'btnJogAddStageZ
        '
        Me.btnJogAddStageZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddStageZ.Location = New System.Drawing.Point(876, 151)
        Me.btnJogAddStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddStageZ.Name = "btnJogAddStageZ"
        Me.btnJogAddStageZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddStageZ.TabIndex = 117
        Me.btnJogAddStageZ.Text = "Add"
        Me.btnJogAddStageZ.UseVisualStyleBackColor = True
        '
        'btnJogAddStageY
        '
        Me.btnJogAddStageY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddStageY.Location = New System.Drawing.Point(876, 115)
        Me.btnJogAddStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddStageY.Name = "btnJogAddStageY"
        Me.btnJogAddStageY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddStageY.TabIndex = 116
        Me.btnJogAddStageY.Text = "Add"
        Me.btnJogAddStageY.UseVisualStyleBackColor = True
        '
        'btnJogSubStageX
        '
        Me.btnJogSubStageX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubStageX.Location = New System.Drawing.Point(945, 81)
        Me.btnJogSubStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubStageX.Name = "btnJogSubStageX"
        Me.btnJogSubStageX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubStageX.TabIndex = 115
        Me.btnJogSubStageX.Text = "Sub"
        Me.btnJogSubStageX.UseVisualStyleBackColor = True
        '
        'btnJogAddStageX
        '
        Me.btnJogAddStageX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddStageX.Location = New System.Drawing.Point(876, 81)
        Me.btnJogAddStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddStageX.Name = "btnJogAddStageX"
        Me.btnJogAddStageX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddStageX.TabIndex = 114
        Me.btnJogAddStageX.Text = "Add"
        Me.btnJogAddStageX.UseVisualStyleBackColor = True
        '
        'nudJogAngleLens
        '
        Me.nudJogAngleLens.DecimalPlaces = 4
        Me.nudJogAngleLens.Location = New System.Drawing.Point(776, 320)
        Me.nudJogAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogAngleLens.Name = "nudJogAngleLens"
        Me.nudJogAngleLens.Size = New System.Drawing.Size(92, 26)
        Me.nudJogAngleLens.TabIndex = 113
        Me.nudJogAngleLens.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogBeamScanY
        '
        Me.nudJogBeamScanY.DecimalPlaces = 4
        Me.nudJogBeamScanY.Location = New System.Drawing.Point(775, 217)
        Me.nudJogBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogBeamScanY.Name = "nudJogBeamScanY"
        Me.nudJogBeamScanY.Size = New System.Drawing.Size(92, 26)
        Me.nudJogBeamScanY.TabIndex = 111
        Me.nudJogBeamScanY.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogBeamScanZ
        '
        Me.nudJogBeamScanZ.DecimalPlaces = 4
        Me.nudJogBeamScanZ.Location = New System.Drawing.Point(775, 251)
        Me.nudJogBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogBeamScanZ.Name = "nudJogBeamScanZ"
        Me.nudJogBeamScanZ.Size = New System.Drawing.Size(92, 26)
        Me.nudJogBeamScanZ.TabIndex = 110
        Me.nudJogBeamScanZ.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogBeamScanX
        '
        Me.nudJogBeamScanX.DecimalPlaces = 4
        Me.nudJogBeamScanX.Location = New System.Drawing.Point(775, 184)
        Me.nudJogBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogBeamScanX.Name = "nudJogBeamScanX"
        Me.nudJogBeamScanX.Size = New System.Drawing.Size(92, 26)
        Me.nudJogBeamScanX.TabIndex = 109
        Me.nudJogBeamScanX.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogStageZ
        '
        Me.nudJogStageZ.DecimalPlaces = 4
        Me.nudJogStageZ.Location = New System.Drawing.Point(775, 152)
        Me.nudJogStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogStageZ.Name = "nudJogStageZ"
        Me.nudJogStageZ.Size = New System.Drawing.Size(92, 26)
        Me.nudJogStageZ.TabIndex = 108
        Me.nudJogStageZ.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogStageY
        '
        Me.nudJogStageY.DecimalPlaces = 4
        Me.nudJogStageY.Location = New System.Drawing.Point(775, 115)
        Me.nudJogStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogStageY.Name = "nudJogStageY"
        Me.nudJogStageY.Size = New System.Drawing.Size(92, 26)
        Me.nudJogStageY.TabIndex = 107
        Me.nudJogStageY.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'nudJogStageX
        '
        Me.nudJogStageX.DecimalPlaces = 4
        Me.nudJogStageX.Location = New System.Drawing.Point(775, 83)
        Me.nudJogStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogStageX.Name = "nudJogStageX"
        Me.nudJogStageX.Size = New System.Drawing.Size(92, 26)
        Me.nudJogStageX.TabIndex = 106
        Me.nudJogStageX.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(772, 55)
        Me.Label43.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(89, 20)
        Me.Label43.TabIndex = 105
        Me.Label43.Text = "Jog (mm)"
        '
        'txtAngleLens
        '
        Me.txtAngleLens.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtAngleLens.Location = New System.Drawing.Point(424, 320)
        Me.txtAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtAngleLens.Name = "txtAngleLens"
        Me.txtAngleLens.Size = New System.Drawing.Size(108, 26)
        Me.txtAngleLens.TabIndex = 104
        '
        'txtBeamScanY
        '
        Me.txtBeamScanY.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtBeamScanY.Location = New System.Drawing.Point(423, 219)
        Me.txtBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtBeamScanY.Name = "txtBeamScanY"
        Me.txtBeamScanY.Size = New System.Drawing.Size(108, 26)
        Me.txtBeamScanY.TabIndex = 102
        '
        'txtBeamScanZ
        '
        Me.txtBeamScanZ.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtBeamScanZ.Location = New System.Drawing.Point(423, 253)
        Me.txtBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtBeamScanZ.Name = "txtBeamScanZ"
        Me.txtBeamScanZ.Size = New System.Drawing.Size(108, 26)
        Me.txtBeamScanZ.TabIndex = 101
        '
        'txtBeamScanX
        '
        Me.txtBeamScanX.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtBeamScanX.Location = New System.Drawing.Point(423, 184)
        Me.txtBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtBeamScanX.Name = "txtBeamScanX"
        Me.txtBeamScanX.Size = New System.Drawing.Size(108, 26)
        Me.txtBeamScanX.TabIndex = 100
        '
        'txtStageZ
        '
        Me.txtStageZ.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtStageZ.Location = New System.Drawing.Point(423, 152)
        Me.txtStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtStageZ.Name = "txtStageZ"
        Me.txtStageZ.Size = New System.Drawing.Size(108, 26)
        Me.txtStageZ.TabIndex = 99
        '
        'txtStageY
        '
        Me.txtStageY.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtStageY.Location = New System.Drawing.Point(423, 115)
        Me.txtStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtStageY.Name = "txtStageY"
        Me.txtStageY.Size = New System.Drawing.Size(108, 26)
        Me.txtStageY.TabIndex = 98
        '
        'txtStageX
        '
        Me.txtStageX.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtStageX.Location = New System.Drawing.Point(423, 83)
        Me.txtStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtStageX.Name = "txtStageX"
        Me.txtStageX.Size = New System.Drawing.Size(108, 26)
        Me.txtStageX.TabIndex = 97
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(420, 55)
        Me.Label42.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(112, 20)
        Me.Label42.TabIndex = 96
        Me.Label42.Text = "Actual (mm)"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(547, 55)
        Me.Label41.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(113, 20)
        Me.Label41.TabIndex = 95
        Me.Label41.Text = "Target (mm)"
        '
        'btnMoveAngleLens
        '
        Me.btnMoveAngleLens.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveAngleLens.Location = New System.Drawing.Point(673, 319)
        Me.btnMoveAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveAngleLens.Name = "btnMoveAngleLens"
        Me.btnMoveAngleLens.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveAngleLens.TabIndex = 94
        Me.btnMoveAngleLens.Text = "Move"
        Me.btnMoveAngleLens.UseVisualStyleBackColor = True
        '
        'btnMoveBeamScanY
        '
        Me.btnMoveBeamScanY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveBeamScanY.Location = New System.Drawing.Point(672, 216)
        Me.btnMoveBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveBeamScanY.Name = "btnMoveBeamScanY"
        Me.btnMoveBeamScanY.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveBeamScanY.TabIndex = 92
        Me.btnMoveBeamScanY.Text = "Move"
        Me.btnMoveBeamScanY.UseVisualStyleBackColor = True
        '
        'btnMoveBeamScanZ
        '
        Me.btnMoveBeamScanZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveBeamScanZ.Location = New System.Drawing.Point(672, 249)
        Me.btnMoveBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveBeamScanZ.Name = "btnMoveBeamScanZ"
        Me.btnMoveBeamScanZ.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveBeamScanZ.TabIndex = 91
        Me.btnMoveBeamScanZ.Text = "Move"
        Me.btnMoveBeamScanZ.UseVisualStyleBackColor = True
        '
        'btnMoveBeamScanX
        '
        Me.btnMoveBeamScanX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveBeamScanX.Location = New System.Drawing.Point(672, 184)
        Me.btnMoveBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveBeamScanX.Name = "btnMoveBeamScanX"
        Me.btnMoveBeamScanX.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveBeamScanX.TabIndex = 90
        Me.btnMoveBeamScanX.Text = "Move"
        Me.btnMoveBeamScanX.UseVisualStyleBackColor = True
        '
        'btnMoveStageZ
        '
        Me.btnMoveStageZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageZ.Location = New System.Drawing.Point(672, 149)
        Me.btnMoveStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveStageZ.Name = "btnMoveStageZ"
        Me.btnMoveStageZ.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveStageZ.TabIndex = 89
        Me.btnMoveStageZ.Text = "Move"
        Me.btnMoveStageZ.UseVisualStyleBackColor = True
        '
        'btnMoveStageY
        '
        Me.btnMoveStageY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageY.Location = New System.Drawing.Point(672, 115)
        Me.btnMoveStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveStageY.Name = "btnMoveStageY"
        Me.btnMoveStageY.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveStageY.TabIndex = 88
        Me.btnMoveStageY.Text = "Move"
        Me.btnMoveStageY.UseVisualStyleBackColor = True
        '
        'btnMoveStageX
        '
        Me.btnMoveStageX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveStageX.Location = New System.Drawing.Point(672, 81)
        Me.btnMoveStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveStageX.Name = "btnMoveStageX"
        Me.btnMoveStageX.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveStageX.TabIndex = 87
        Me.btnMoveStageX.Text = "Move"
        Me.btnMoveStageX.UseVisualStyleBackColor = True
        '
        'btnSaveConfiguredPositionNew
        '
        Me.btnSaveConfiguredPositionNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveConfiguredPositionNew.Location = New System.Drawing.Point(577, 8)
        Me.btnSaveConfiguredPositionNew.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveConfiguredPositionNew.Name = "btnSaveConfiguredPositionNew"
        Me.btnSaveConfiguredPositionNew.Size = New System.Drawing.Size(133, 35)
        Me.btnSaveConfiguredPositionNew.TabIndex = 86
        Me.btnSaveConfiguredPositionNew.Text = "Save New"
        Me.btnSaveConfiguredPositionNew.UseVisualStyleBackColor = True
        '
        'nudAngleLens
        '
        Me.nudAngleLens.Location = New System.Drawing.Point(544, 320)
        Me.nudAngleLens.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudAngleLens.Name = "nudAngleLens"
        Me.nudAngleLens.Size = New System.Drawing.Size(121, 26)
        Me.nudAngleLens.TabIndex = 85
        '
        'nudBeamScanY
        '
        Me.nudBeamScanY.Location = New System.Drawing.Point(543, 217)
        Me.nudBeamScanY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudBeamScanY.Name = "nudBeamScanY"
        Me.nudBeamScanY.Size = New System.Drawing.Size(121, 26)
        Me.nudBeamScanY.TabIndex = 84
        '
        'nudBeamScanZ
        '
        Me.nudBeamScanZ.Location = New System.Drawing.Point(543, 251)
        Me.nudBeamScanZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudBeamScanZ.Name = "nudBeamScanZ"
        Me.nudBeamScanZ.Size = New System.Drawing.Size(121, 26)
        Me.nudBeamScanZ.TabIndex = 82
        '
        'btnDeleteConfiguredPosition
        '
        Me.btnDeleteConfiguredPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteConfiguredPosition.Location = New System.Drawing.Point(427, 8)
        Me.btnDeleteConfiguredPosition.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnDeleteConfiguredPosition.Name = "btnDeleteConfiguredPosition"
        Me.btnDeleteConfiguredPosition.Size = New System.Drawing.Size(144, 35)
        Me.btnDeleteConfiguredPosition.TabIndex = 81
        Me.btnDeleteConfiguredPosition.Text = "Del Current"
        Me.btnDeleteConfiguredPosition.UseVisualStyleBackColor = True
        '
        'btnSaveConfiguredPosition
        '
        Me.btnSaveConfiguredPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveConfiguredPosition.Location = New System.Drawing.Point(275, 8)
        Me.btnSaveConfiguredPosition.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveConfiguredPosition.Name = "btnSaveConfiguredPosition"
        Me.btnSaveConfiguredPosition.Size = New System.Drawing.Size(144, 35)
        Me.btnSaveConfiguredPosition.TabIndex = 81
        Me.btnSaveConfiguredPosition.Text = "Save Current"
        Me.btnSaveConfiguredPosition.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(8, 15)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(190, 20)
        Me.Label13.TabIndex = 79
        Me.Label13.Text = "Configured Positions:"
        '
        'nudStageZ
        '
        Me.nudStageZ.Location = New System.Drawing.Point(543, 152)
        Me.nudStageZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudStageZ.Name = "nudStageZ"
        Me.nudStageZ.Size = New System.Drawing.Size(121, 26)
        Me.nudStageZ.TabIndex = 78
        '
        'nudBeamScanX
        '
        Me.nudBeamScanX.Location = New System.Drawing.Point(543, 184)
        Me.nudBeamScanX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudBeamScanX.Name = "nudBeamScanX"
        Me.nudBeamScanX.Size = New System.Drawing.Size(121, 26)
        Me.nudBeamScanX.TabIndex = 75
        '
        'nudStageY
        '
        Me.nudStageY.Location = New System.Drawing.Point(543, 115)
        Me.nudStageY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudStageY.Name = "nudStageY"
        Me.nudStageY.Size = New System.Drawing.Size(121, 26)
        Me.nudStageY.TabIndex = 77
        '
        'nudStageX
        '
        Me.nudStageX.DecimalPlaces = 4
        Me.nudStageX.Location = New System.Drawing.Point(543, 83)
        Me.nudStageX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudStageX.Name = "nudStageX"
        Me.nudStageX.Size = New System.Drawing.Size(121, 26)
        Me.nudStageX.TabIndex = 76
        Me.nudStageX.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'TabPageXPS
        '
        Me.TabPageXPS.Controls.Add(Me.GroupBox6)
        Me.TabPageXPS.Controls.Add(Me.btnPiC843)
        Me.TabPageXPS.Controls.Add(Me.btnSyncXPS)
        Me.TabPageXPS.Controls.Add(Me.dgvXPS)
        Me.TabPageXPS.Controls.Add(Me.btnXPS)
        Me.TabPageXPS.Location = New System.Drawing.Point(4, 29)
        Me.TabPageXPS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPageXPS.Name = "TabPageXPS"
        Me.TabPageXPS.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPageXPS.Size = New System.Drawing.Size(1525, 724)
        Me.TabPageXPS.TabIndex = 1
        Me.TabPageXPS.Text = "Motion Controller"
        Me.TabPageXPS.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.Label35)
        Me.GroupBox6.Controls.Add(Me.txtHexapodV)
        Me.GroupBox6.Controls.Add(Me.nudHexapodX)
        Me.GroupBox6.Controls.Add(Me.txtHexapodW)
        Me.GroupBox6.Controls.Add(Me.nudHexapodY)
        Me.GroupBox6.Controls.Add(Me.txtHexapodU)
        Me.GroupBox6.Controls.Add(Me.nudHexapodU)
        Me.GroupBox6.Controls.Add(Me.txtHexapodZ)
        Me.GroupBox6.Controls.Add(Me.nudHexapodZ)
        Me.GroupBox6.Controls.Add(Me.txtHexapodY)
        Me.GroupBox6.Controls.Add(Me.Label36)
        Me.GroupBox6.Controls.Add(Me.txtHexapodX)
        Me.GroupBox6.Controls.Add(Me.lstHexapodConfiguredPositions)
        Me.GroupBox6.Controls.Add(Me.Label37)
        Me.GroupBox6.Controls.Add(Me.btnSaveHexapodConfiguredPosition)
        Me.GroupBox6.Controls.Add(Me.nudHexapodW)
        Me.GroupBox6.Controls.Add(Me.btnJogHexapodPanel)
        Me.GroupBox6.Controls.Add(Me.nudHexapodV)
        Me.GroupBox6.Controls.Add(Me.btnStopHexapodMove)
        Me.GroupBox6.Controls.Add(Me.btnSaveHexapodConfiguredPositionNew)
        Me.GroupBox6.Controls.Add(Me.Label38)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodX)
        Me.GroupBox6.Controls.Add(Me.Label40)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodY)
        Me.GroupBox6.Controls.Add(Me.Label46)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodZ)
        Me.GroupBox6.Controls.Add(Me.Label47)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodU)
        Me.GroupBox6.Controls.Add(Me.btnSyncHexapod)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodW)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodXYZ)
        Me.GroupBox6.Controls.Add(Me.btnMoveHexapodV)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodV)
        Me.GroupBox6.Controls.Add(Me.Label48)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodW)
        Me.GroupBox6.Controls.Add(Me.Label49)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodU)
        Me.GroupBox6.Controls.Add(Me.Label50)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodZ)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodX)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodY)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodY)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodV)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodZ)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodW)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodU)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodU)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodW)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodZ)
        Me.GroupBox6.Controls.Add(Me.nudJogHexapodV)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodY)
        Me.GroupBox6.Controls.Add(Me.btnJogAddHexapodX)
        Me.GroupBox6.Controls.Add(Me.btnJogSubHexapodX)
        Me.GroupBox6.Location = New System.Drawing.Point(8, -247)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GroupBox6.Size = New System.Drawing.Size(1235, 331)
        Me.GroupBox6.TabIndex = 171
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Hexapod"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(291, 109)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(75, 20)
        Me.Label35.TabIndex = 158
        Me.Label35.Text = "Stage X"
        '
        'txtHexapodV
        '
        Me.txtHexapodV.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodV.Location = New System.Drawing.Point(429, 244)
        Me.txtHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodV.Name = "txtHexapodV"
        Me.txtHexapodV.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodV.TabIndex = 102
        '
        'nudHexapodX
        '
        Me.nudHexapodX.DecimalPlaces = 4
        Me.nudHexapodX.Location = New System.Drawing.Point(549, 108)
        Me.nudHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodX.Name = "nudHexapodX"
        Me.nudHexapodX.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodX.TabIndex = 76
        Me.nudHexapodX.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'txtHexapodW
        '
        Me.txtHexapodW.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodW.Location = New System.Drawing.Point(429, 277)
        Me.txtHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodW.Name = "txtHexapodW"
        Me.txtHexapodW.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodW.TabIndex = 101
        '
        'nudHexapodY
        '
        Me.nudHexapodY.Location = New System.Drawing.Point(549, 140)
        Me.nudHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodY.Name = "nudHexapodY"
        Me.nudHexapodY.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodY.TabIndex = 77
        '
        'txtHexapodU
        '
        Me.txtHexapodU.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodU.Location = New System.Drawing.Point(429, 209)
        Me.txtHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodU.Name = "txtHexapodU"
        Me.txtHexapodU.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodU.TabIndex = 100
        '
        'nudHexapodU
        '
        Me.nudHexapodU.Location = New System.Drawing.Point(549, 209)
        Me.nudHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodU.Name = "nudHexapodU"
        Me.nudHexapodU.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodU.TabIndex = 75
        '
        'txtHexapodZ
        '
        Me.txtHexapodZ.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodZ.Location = New System.Drawing.Point(429, 177)
        Me.txtHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodZ.Name = "txtHexapodZ"
        Me.txtHexapodZ.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodZ.TabIndex = 99
        '
        'nudHexapodZ
        '
        Me.nudHexapodZ.Location = New System.Drawing.Point(549, 177)
        Me.nudHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodZ.Name = "nudHexapodZ"
        Me.nudHexapodZ.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodZ.TabIndex = 78
        '
        'txtHexapodY
        '
        Me.txtHexapodY.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodY.Location = New System.Drawing.Point(429, 140)
        Me.txtHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodY.Name = "txtHexapodY"
        Me.txtHexapodY.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodY.TabIndex = 98
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(15, 36)
        Me.Label36.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(190, 20)
        Me.Label36.TabIndex = 79
        Me.Label36.Text = "Configured Positions:"
        '
        'txtHexapodX
        '
        Me.txtHexapodX.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtHexapodX.Location = New System.Drawing.Point(429, 108)
        Me.txtHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtHexapodX.Name = "txtHexapodX"
        Me.txtHexapodX.Size = New System.Drawing.Size(108, 26)
        Me.txtHexapodX.TabIndex = 97
        '
        'lstHexapodConfiguredPositions
        '
        Me.lstHexapodConfiguredPositions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstHexapodConfiguredPositions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstHexapodConfiguredPositions.FormattingEnabled = True
        Me.lstHexapodConfiguredPositions.IntegralHeight = False
        Me.lstHexapodConfiguredPositions.ItemHeight = 20
        Me.lstHexapodConfiguredPositions.Location = New System.Drawing.Point(15, 64)
        Me.lstHexapodConfiguredPositions.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.lstHexapodConfiguredPositions.Name = "lstHexapodConfiguredPositions"
        Me.lstHexapodConfiguredPositions.Size = New System.Drawing.Size(259, 239)
        Me.lstHexapodConfiguredPositions.TabIndex = 80
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(291, 144)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(74, 20)
        Me.Label37.TabIndex = 159
        Me.Label37.Text = "Stage Y"
        '
        'btnSaveHexapodConfiguredPosition
        '
        Me.btnSaveHexapodConfiguredPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveHexapodConfiguredPosition.Location = New System.Drawing.Point(283, 33)
        Me.btnSaveHexapodConfiguredPosition.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveHexapodConfiguredPosition.Name = "btnSaveHexapodConfiguredPosition"
        Me.btnSaveHexapodConfiguredPosition.Size = New System.Drawing.Size(144, 35)
        Me.btnSaveHexapodConfiguredPosition.TabIndex = 81
        Me.btnSaveHexapodConfiguredPosition.Text = "Save Current"
        Me.btnSaveHexapodConfiguredPosition.UseVisualStyleBackColor = True
        '
        'nudHexapodW
        '
        Me.nudHexapodW.Location = New System.Drawing.Point(549, 276)
        Me.nudHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodW.Name = "nudHexapodW"
        Me.nudHexapodW.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodW.TabIndex = 82
        '
        'btnJogHexapodPanel
        '
        Me.btnJogHexapodPanel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogHexapodPanel.Location = New System.Drawing.Point(719, 33)
        Me.btnJogHexapodPanel.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogHexapodPanel.Name = "btnJogHexapodPanel"
        Me.btnJogHexapodPanel.Size = New System.Drawing.Size(144, 35)
        Me.btnJogHexapodPanel.TabIndex = 148
        Me.btnJogHexapodPanel.Text = "Jog Panel"
        Me.btnJogHexapodPanel.UseVisualStyleBackColor = True
        '
        'nudHexapodV
        '
        Me.nudHexapodV.Location = New System.Drawing.Point(549, 243)
        Me.nudHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudHexapodV.Name = "nudHexapodV"
        Me.nudHexapodV.Size = New System.Drawing.Size(121, 26)
        Me.nudHexapodV.TabIndex = 84
        '
        'btnStopHexapodMove
        '
        Me.btnStopHexapodMove.BackColor = System.Drawing.Color.Red
        Me.btnStopHexapodMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopHexapodMove.ForeColor = System.Drawing.Color.Yellow
        Me.btnStopHexapodMove.Location = New System.Drawing.Point(868, 33)
        Me.btnStopHexapodMove.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnStopHexapodMove.Name = "btnStopHexapodMove"
        Me.btnStopHexapodMove.Size = New System.Drawing.Size(144, 35)
        Me.btnStopHexapodMove.TabIndex = 141
        Me.btnStopHexapodMove.Text = "Stop Move"
        Me.btnStopHexapodMove.UseVisualStyleBackColor = False
        '
        'btnSaveHexapodConfiguredPositionNew
        '
        Me.btnSaveHexapodConfiguredPositionNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveHexapodConfiguredPositionNew.Location = New System.Drawing.Point(431, 33)
        Me.btnSaveHexapodConfiguredPositionNew.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveHexapodConfiguredPositionNew.Name = "btnSaveHexapodConfiguredPositionNew"
        Me.btnSaveHexapodConfiguredPositionNew.Size = New System.Drawing.Size(133, 35)
        Me.btnSaveHexapodConfiguredPositionNew.TabIndex = 86
        Me.btnSaveHexapodConfiguredPositionNew.Text = "Save New"
        Me.btnSaveHexapodConfiguredPositionNew.UseVisualStyleBackColor = True
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(291, 180)
        Me.Label38.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(73, 20)
        Me.Label38.TabIndex = 134
        Me.Label38.Text = "Stage Z"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMoveHexapodX
        '
        Me.btnMoveHexapodX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodX.Location = New System.Drawing.Point(679, 107)
        Me.btnMoveHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodX.Name = "btnMoveHexapodX"
        Me.btnMoveHexapodX.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodX.TabIndex = 87
        Me.btnMoveHexapodX.Text = "Move"
        Me.btnMoveHexapodX.UseVisualStyleBackColor = True
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(292, 213)
        Me.Label40.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(76, 20)
        Me.Label40.TabIndex = 136
        Me.Label40.Text = "Stage U"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnMoveHexapodY
        '
        Me.btnMoveHexapodY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodY.Location = New System.Drawing.Point(679, 140)
        Me.btnMoveHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodY.Name = "btnMoveHexapodY"
        Me.btnMoveHexapodY.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodY.TabIndex = 88
        Me.btnMoveHexapodY.Text = "Move"
        Me.btnMoveHexapodY.UseVisualStyleBackColor = True
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(291, 277)
        Me.Label46.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(80, 20)
        Me.Label46.TabIndex = 137
        Me.Label46.Text = "Stage W"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMoveHexapodZ
        '
        Me.btnMoveHexapodZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodZ.Location = New System.Drawing.Point(679, 173)
        Me.btnMoveHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodZ.Name = "btnMoveHexapodZ"
        Me.btnMoveHexapodZ.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodZ.TabIndex = 89
        Me.btnMoveHexapodZ.Text = "Move"
        Me.btnMoveHexapodZ.UseVisualStyleBackColor = True
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(291, 245)
        Me.Label47.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(75, 20)
        Me.Label47.TabIndex = 139
        Me.Label47.Text = "Stage V"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMoveHexapodU
        '
        Me.btnMoveHexapodU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodU.Location = New System.Drawing.Point(679, 209)
        Me.btnMoveHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodU.Name = "btnMoveHexapodU"
        Me.btnMoveHexapodU.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodU.TabIndex = 90
        Me.btnMoveHexapodU.Text = "Move"
        Me.btnMoveHexapodU.UseVisualStyleBackColor = True
        '
        'btnSyncHexapod
        '
        Me.btnSyncHexapod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSyncHexapod.Location = New System.Drawing.Point(1019, 33)
        Me.btnSyncHexapod.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSyncHexapod.Name = "btnSyncHexapod"
        Me.btnSyncHexapod.Size = New System.Drawing.Size(144, 35)
        Me.btnSyncHexapod.TabIndex = 132
        Me.btnSyncHexapod.Text = "Refresh"
        Me.btnSyncHexapod.UseVisualStyleBackColor = True
        '
        'btnMoveHexapodW
        '
        Me.btnMoveHexapodW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodW.Location = New System.Drawing.Point(679, 275)
        Me.btnMoveHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodW.Name = "btnMoveHexapodW"
        Me.btnMoveHexapodW.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodW.TabIndex = 91
        Me.btnMoveHexapodW.Text = "Move"
        Me.btnMoveHexapodW.UseVisualStyleBackColor = True
        '
        'btnMoveHexapodXYZ
        '
        Me.btnMoveHexapodXYZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodXYZ.Location = New System.Drawing.Point(571, 33)
        Me.btnMoveHexapodXYZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodXYZ.Name = "btnMoveHexapodXYZ"
        Me.btnMoveHexapodXYZ.Size = New System.Drawing.Size(144, 35)
        Me.btnMoveHexapodXYZ.TabIndex = 131
        Me.btnMoveHexapodXYZ.Text = "Move XYZ"
        Me.btnMoveHexapodXYZ.UseVisualStyleBackColor = True
        '
        'btnMoveHexapodV
        '
        Me.btnMoveHexapodV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMoveHexapodV.Location = New System.Drawing.Point(679, 241)
        Me.btnMoveHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMoveHexapodV.Name = "btnMoveHexapodV"
        Me.btnMoveHexapodV.Size = New System.Drawing.Size(93, 29)
        Me.btnMoveHexapodV.TabIndex = 92
        Me.btnMoveHexapodV.Text = "Move"
        Me.btnMoveHexapodV.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodV
        '
        Me.btnJogSubHexapodV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodV.Location = New System.Drawing.Point(952, 241)
        Me.btnJogSubHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodV.Name = "btnJogSubHexapodV"
        Me.btnJogSubHexapodV.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodV.TabIndex = 127
        Me.btnJogSubHexapodV.Text = "Sub"
        Me.btnJogSubHexapodV.UseVisualStyleBackColor = True
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(555, 80)
        Me.Label48.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(113, 20)
        Me.Label48.TabIndex = 95
        Me.Label48.Text = "Target (mm)"
        '
        'btnJogSubHexapodW
        '
        Me.btnJogSubHexapodW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodW.Location = New System.Drawing.Point(952, 275)
        Me.btnJogSubHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodW.Name = "btnJogSubHexapodW"
        Me.btnJogSubHexapodW.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodW.TabIndex = 126
        Me.btnJogSubHexapodW.Text = "Sub"
        Me.btnJogSubHexapodW.UseVisualStyleBackColor = True
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(427, 80)
        Me.Label49.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(112, 20)
        Me.Label49.TabIndex = 96
        Me.Label49.Text = "Actual (mm)"
        '
        'btnJogSubHexapodU
        '
        Me.btnJogSubHexapodU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodU.Location = New System.Drawing.Point(952, 209)
        Me.btnJogSubHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodU.Name = "btnJogSubHexapodU"
        Me.btnJogSubHexapodU.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodU.TabIndex = 125
        Me.btnJogSubHexapodU.Text = "Sub"
        Me.btnJogSubHexapodU.UseVisualStyleBackColor = True
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(779, 80)
        Me.Label50.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(89, 20)
        Me.Label50.TabIndex = 105
        Me.Label50.Text = "Jog (mm)"
        '
        'btnJogSubHexapodZ
        '
        Me.btnJogSubHexapodZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodZ.Location = New System.Drawing.Point(952, 176)
        Me.btnJogSubHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodZ.Name = "btnJogSubHexapodZ"
        Me.btnJogSubHexapodZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodZ.TabIndex = 124
        Me.btnJogSubHexapodZ.Text = "Sub"
        Me.btnJogSubHexapodZ.UseVisualStyleBackColor = True
        '
        'nudJogHexapodX
        '
        Me.nudJogHexapodX.DecimalPlaces = 4
        Me.nudJogHexapodX.Location = New System.Drawing.Point(781, 108)
        Me.nudJogHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodX.Name = "nudJogHexapodX"
        Me.nudJogHexapodX.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodX.TabIndex = 106
        Me.nudJogHexapodX.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogSubHexapodY
        '
        Me.btnJogSubHexapodY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodY.Location = New System.Drawing.Point(952, 140)
        Me.btnJogSubHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodY.Name = "btnJogSubHexapodY"
        Me.btnJogSubHexapodY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodY.TabIndex = 123
        Me.btnJogSubHexapodY.Text = "Sub"
        Me.btnJogSubHexapodY.UseVisualStyleBackColor = True
        '
        'nudJogHexapodY
        '
        Me.nudJogHexapodY.DecimalPlaces = 4
        Me.nudJogHexapodY.Location = New System.Drawing.Point(781, 140)
        Me.nudJogHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodY.Name = "nudJogHexapodY"
        Me.nudJogHexapodY.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodY.TabIndex = 107
        Me.nudJogHexapodY.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogAddHexapodV
        '
        Me.btnJogAddHexapodV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodV.Location = New System.Drawing.Point(883, 241)
        Me.btnJogAddHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodV.Name = "btnJogAddHexapodV"
        Me.btnJogAddHexapodV.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodV.TabIndex = 120
        Me.btnJogAddHexapodV.Text = "Add"
        Me.btnJogAddHexapodV.UseVisualStyleBackColor = True
        '
        'nudJogHexapodZ
        '
        Me.nudJogHexapodZ.DecimalPlaces = 4
        Me.nudJogHexapodZ.Location = New System.Drawing.Point(781, 177)
        Me.nudJogHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodZ.Name = "nudJogHexapodZ"
        Me.nudJogHexapodZ.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodZ.TabIndex = 108
        Me.nudJogHexapodZ.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogAddHexapodW
        '
        Me.btnJogAddHexapodW.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodW.Location = New System.Drawing.Point(883, 275)
        Me.btnJogAddHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodW.Name = "btnJogAddHexapodW"
        Me.btnJogAddHexapodW.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodW.TabIndex = 119
        Me.btnJogAddHexapodW.Text = "Add"
        Me.btnJogAddHexapodW.UseVisualStyleBackColor = True
        '
        'nudJogHexapodU
        '
        Me.nudJogHexapodU.DecimalPlaces = 4
        Me.nudJogHexapodU.Location = New System.Drawing.Point(781, 209)
        Me.nudJogHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodU.Name = "nudJogHexapodU"
        Me.nudJogHexapodU.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodU.TabIndex = 109
        Me.nudJogHexapodU.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogAddHexapodU
        '
        Me.btnJogAddHexapodU.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodU.Location = New System.Drawing.Point(883, 209)
        Me.btnJogAddHexapodU.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodU.Name = "btnJogAddHexapodU"
        Me.btnJogAddHexapodU.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodU.TabIndex = 118
        Me.btnJogAddHexapodU.Text = "Add"
        Me.btnJogAddHexapodU.UseVisualStyleBackColor = True
        '
        'nudJogHexapodW
        '
        Me.nudJogHexapodW.DecimalPlaces = 4
        Me.nudJogHexapodW.Location = New System.Drawing.Point(781, 276)
        Me.nudJogHexapodW.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodW.Name = "nudJogHexapodW"
        Me.nudJogHexapodW.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodW.TabIndex = 110
        Me.nudJogHexapodW.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogAddHexapodZ
        '
        Me.btnJogAddHexapodZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodZ.Location = New System.Drawing.Point(883, 176)
        Me.btnJogAddHexapodZ.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodZ.Name = "btnJogAddHexapodZ"
        Me.btnJogAddHexapodZ.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodZ.TabIndex = 117
        Me.btnJogAddHexapodZ.Text = "Add"
        Me.btnJogAddHexapodZ.UseVisualStyleBackColor = True
        '
        'nudJogHexapodV
        '
        Me.nudJogHexapodV.DecimalPlaces = 4
        Me.nudJogHexapodV.Location = New System.Drawing.Point(781, 243)
        Me.nudJogHexapodV.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogHexapodV.Name = "nudJogHexapodV"
        Me.nudJogHexapodV.Size = New System.Drawing.Size(92, 26)
        Me.nudJogHexapodV.TabIndex = 111
        Me.nudJogHexapodV.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'btnJogAddHexapodY
        '
        Me.btnJogAddHexapodY.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodY.Location = New System.Drawing.Point(883, 140)
        Me.btnJogAddHexapodY.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodY.Name = "btnJogAddHexapodY"
        Me.btnJogAddHexapodY.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodY.TabIndex = 116
        Me.btnJogAddHexapodY.Text = "Add"
        Me.btnJogAddHexapodY.UseVisualStyleBackColor = True
        '
        'btnJogAddHexapodX
        '
        Me.btnJogAddHexapodX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddHexapodX.Location = New System.Drawing.Point(883, 107)
        Me.btnJogAddHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddHexapodX.Name = "btnJogAddHexapodX"
        Me.btnJogAddHexapodX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddHexapodX.TabIndex = 114
        Me.btnJogAddHexapodX.Text = "Add"
        Me.btnJogAddHexapodX.UseVisualStyleBackColor = True
        '
        'btnJogSubHexapodX
        '
        Me.btnJogSubHexapodX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubHexapodX.Location = New System.Drawing.Point(952, 107)
        Me.btnJogSubHexapodX.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubHexapodX.Name = "btnJogSubHexapodX"
        Me.btnJogSubHexapodX.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubHexapodX.TabIndex = 115
        Me.btnJogSubHexapodX.Text = "Sub"
        Me.btnJogSubHexapodX.UseVisualStyleBackColor = True
        '
        'btnPiC843
        '
        Me.btnPiC843.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPiC843.Location = New System.Drawing.Point(535, 8)
        Me.btnPiC843.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnPiC843.Name = "btnPiC843"
        Me.btnPiC843.Size = New System.Drawing.Size(144, 41)
        Me.btnPiC843.TabIndex = 133
        Me.btnPiC843.Text = "PI C843"
        Me.btnPiC843.UseVisualStyleBackColor = True
        '
        'btnSyncXPS
        '
        Me.btnSyncXPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSyncXPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSyncXPS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSyncXPS.Location = New System.Drawing.Point(256, 8)
        Me.btnSyncXPS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSyncXPS.Name = "btnSyncXPS"
        Me.btnSyncXPS.Size = New System.Drawing.Size(240, 41)
        Me.btnSyncXPS.TabIndex = 15
        Me.btnSyncXPS.Text = "Update XPS Status"
        Me.btnSyncXPS.UseVisualStyleBackColor = True
        '
        'dgvXPS
        '
        Me.dgvXPS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvXPS.Location = New System.Drawing.Point(8, 56)
        Me.dgvXPS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvXPS.Name = "dgvXPS"
        Me.dgvXPS.Size = New System.Drawing.Size(1165, 304)
        Me.dgvXPS.TabIndex = 14
        '
        'btnXPS
        '
        Me.btnXPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnXPS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnXPS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnXPS.Location = New System.Drawing.Point(8, 8)
        Me.btnXPS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnXPS.Name = "btnXPS"
        Me.btnXPS.Size = New System.Drawing.Size(240, 41)
        Me.btnXPS.TabIndex = 13
        Me.btnXPS.Text = "Newport Web Interface  "
        Me.btnXPS.UseVisualStyleBackColor = True
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
        Me.TabPageSetting.Location = New System.Drawing.Point(4, 29)
        Me.TabPageSetting.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPageSetting.Name = "TabPageSetting"
        Me.TabPageSetting.Size = New System.Drawing.Size(1525, 724)
        Me.TabPageSetting.TabIndex = 2
        Me.TabPageSetting.Text = "Settings"
        Me.TabPageSetting.UseVisualStyleBackColor = True
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(821, 67)
        Me.Label45.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(215, 24)
        Me.Label45.TabIndex = 29
        Me.Label45.Text = "Alignment Parameters"
        '
        'dgvAlignment
        '
        Me.dgvAlignment.AllowUserToAddRows = False
        Me.dgvAlignment.AllowUserToDeleteRows = False
        Me.dgvAlignment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAlignment.Location = New System.Drawing.Point(825, 95)
        Me.dgvAlignment.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvAlignment.Name = "dgvAlignment"
        Me.dgvAlignment.RowTemplate.Height = 23
        Me.dgvAlignment.Size = New System.Drawing.Size(565, 601)
        Me.dgvAlignment.TabIndex = 28
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(421, 67)
        Me.Label44.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(296, 24)
        Me.Label44.TabIndex = 27
        Me.Label44.Text = "Force && Vision For Positioning"
        '
        'dgvForceGauge
        '
        Me.dgvForceGauge.AllowUserToAddRows = False
        Me.dgvForceGauge.AllowUserToDeleteRows = False
        Me.dgvForceGauge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvForceGauge.Location = New System.Drawing.Point(425, 95)
        Me.dgvForceGauge.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvForceGauge.Name = "dgvForceGauge"
        Me.dgvForceGauge.RowTemplate.Height = 23
        Me.dgvForceGauge.Size = New System.Drawing.Size(375, 601)
        Me.dgvForceGauge.TabIndex = 26
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(16, 67)
        Me.Label39.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(138, 24)
        Me.Label39.TabIndex = 25
        Me.Label39.Text = "Initial Settings"
        '
        'dgvSetting
        '
        Me.dgvSetting.AllowUserToAddRows = False
        Me.dgvSetting.AllowUserToDeleteRows = False
        Me.dgvSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSetting.Location = New System.Drawing.Point(20, 95)
        Me.dgvSetting.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvSetting.Name = "dgvSetting"
        Me.dgvSetting.RowTemplate.Height = 23
        Me.dgvSetting.Size = New System.Drawing.Size(375, 601)
        Me.dgvSetting.TabIndex = 24
        '
        'lblSaveSetting
        '
        Me.lblSaveSetting.AutoSize = True
        Me.lblSaveSetting.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSaveSetting.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSaveSetting.Location = New System.Drawing.Point(444, 23)
        Me.lblSaveSetting.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSaveSetting.Name = "lblSaveSetting"
        Me.lblSaveSetting.Size = New System.Drawing.Size(148, 25)
        Me.lblSaveSetting.TabIndex = 23
        Me.lblSaveSetting.Text = "Initial Settings"
        '
        'btnSaveSettings
        '
        Me.btnSaveSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSettings.ForeColor = System.Drawing.SystemColors.Highlight
        Me.btnSaveSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveSettings.Location = New System.Drawing.Point(20, 11)
        Me.btnSaveSettings.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSaveSettings.Name = "btnSaveSettings"
        Me.btnSaveSettings.Size = New System.Drawing.Size(416, 47)
        Me.btnSaveSettings.TabIndex = 22
        Me.btnSaveSettings.Text = "Save Changes to Configuration File  "
        Me.btnSaveSettings.UseVisualStyleBackColor = True
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
        'tmrRefresh
        '
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(283, 396)
        Me.Label51.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(60, 20)
        Me.Label51.TabIndex = 193
        Me.Label51.Text = "PI, LS"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnJogSubPiLS
        '
        Me.btnJogSubPiLS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogSubPiLS.Location = New System.Drawing.Point(946, 392)
        Me.btnJogSubPiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogSubPiLS.Name = "btnJogSubPiLS"
        Me.btnJogSubPiLS.Size = New System.Drawing.Size(60, 29)
        Me.btnJogSubPiLS.TabIndex = 192
        Me.btnJogSubPiLS.Text = "Sub"
        Me.btnJogSubPiLS.UseVisualStyleBackColor = True
        '
        'btnJogAddPiLS
        '
        Me.btnJogAddPiLS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJogAddPiLS.Location = New System.Drawing.Point(876, 392)
        Me.btnJogAddPiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnJogAddPiLS.Name = "btnJogAddPiLS"
        Me.btnJogAddPiLS.Size = New System.Drawing.Size(60, 29)
        Me.btnJogAddPiLS.TabIndex = 191
        Me.btnJogAddPiLS.Text = "Add"
        Me.btnJogAddPiLS.UseVisualStyleBackColor = True
        '
        'nudJogPiLS
        '
        Me.nudJogPiLS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.nudJogPiLS.DecimalPlaces = 4
        Me.nudJogPiLS.Location = New System.Drawing.Point(775, 392)
        Me.nudJogPiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudJogPiLS.Name = "nudJogPiLS"
        Me.nudJogPiLS.Size = New System.Drawing.Size(92, 26)
        Me.nudJogPiLS.TabIndex = 190
        Me.nudJogPiLS.Value = New Decimal(New Integer() {10000, 0, 0, 262144})
        '
        'txtPiLS
        '
        Me.txtPiLS.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtPiLS.Location = New System.Drawing.Point(423, 391)
        Me.txtPiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txtPiLS.Name = "txtPiLS"
        Me.txtPiLS.Size = New System.Drawing.Size(108, 26)
        Me.txtPiLS.TabIndex = 189
        '
        'btnMovePiLS
        '
        Me.btnMovePiLS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMovePiLS.Location = New System.Drawing.Point(672, 392)
        Me.btnMovePiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnMovePiLS.Name = "btnMovePiLS"
        Me.btnMovePiLS.Size = New System.Drawing.Size(93, 29)
        Me.btnMovePiLS.TabIndex = 188
        Me.btnMovePiLS.Text = "Move"
        Me.btnMovePiLS.UseVisualStyleBackColor = True
        '
        'nudPiLS
        '
        Me.nudPiLS.Location = New System.Drawing.Point(543, 392)
        Me.nudPiLS.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.nudPiLS.Name = "nudPiLS"
        Me.nudPiLS.Size = New System.Drawing.Size(121, 26)
        Me.nudPiLS.TabIndex = 187
        '
        'fControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1545, 795)
        Me.Controls.Add(Me.tc)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "fControl"
        Me.Text = "Engineering Controller"
        Me.tc.ResumeLayout(False)
        Me.TabPageStage.ResumeLayout(False)
        Me.TabPageStage.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        CType(Me.nudLightSource22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLightSource21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLightSource12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLightSource11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.nudUvPower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudUvTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogProbe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudProbe, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.nudJogAnglePbs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAnglePbs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudTemperature, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogAngleLens, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogBeamScanY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogBeamScanZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogBeamScanX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogStageZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogStageY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogStageX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAngleLens, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBeamScanY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBeamScanZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStageZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBeamScanX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStageY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStageX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageXPS.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.nudHexapodX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHexapodV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodU, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogHexapodV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvXPS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSetting.ResumeLayout(False)
        Me.TabPageSetting.PerformLayout()
        CType(Me.dgvAlignment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvForceGauge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvSetting, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJogPiLS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPiLS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tc As System.Windows.Forms.TabControl
    Friend WithEvents TabPageStage As System.Windows.Forms.TabPage
    Friend WithEvents TabPageXPS As System.Windows.Forms.TabPage
    Friend WithEvents btnMoveXYZ As System.Windows.Forms.Button
    Friend WithEvents btnJogSubAngleLens As System.Windows.Forms.Button
    Friend WithEvents btnJogSubBeamScanY As System.Windows.Forms.Button
    Friend WithEvents btnJogSubBeamScanZ As System.Windows.Forms.Button
    Friend WithEvents btnJogSubBeamScanX As System.Windows.Forms.Button
    Friend WithEvents btnJogSubStageZ As System.Windows.Forms.Button
    Friend WithEvents btnJogSubStageY As System.Windows.Forms.Button
    Friend WithEvents btnJogAddAngleLens As System.Windows.Forms.Button
    Friend WithEvents btnJogAddBeamScanY As System.Windows.Forms.Button
    Friend WithEvents btnJogAddBeamScanZ As System.Windows.Forms.Button
    Friend WithEvents btnJogAddBeamScanX As System.Windows.Forms.Button
    Friend WithEvents btnJogAddStageZ As System.Windows.Forms.Button
    Friend WithEvents btnJogAddStageY As System.Windows.Forms.Button
    Friend WithEvents btnJogSubStageX As System.Windows.Forms.Button
    Friend WithEvents btnJogAddStageX As System.Windows.Forms.Button
    Friend WithEvents nudJogAngleLens As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogBeamScanY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogBeamScanZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogBeamScanX As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogStageZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogStageY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudJogStageX As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents txtAngleLens As System.Windows.Forms.TextBox
    Friend WithEvents txtBeamScanY As System.Windows.Forms.TextBox
    Friend WithEvents txtBeamScanZ As System.Windows.Forms.TextBox
    Friend WithEvents txtBeamScanX As System.Windows.Forms.TextBox
    Friend WithEvents txtStageZ As System.Windows.Forms.TextBox
    Friend WithEvents txtStageY As System.Windows.Forms.TextBox
    Friend WithEvents txtStageX As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents btnMoveAngleLens As System.Windows.Forms.Button
    Friend WithEvents btnMoveBeamScanY As System.Windows.Forms.Button
    Friend WithEvents btnMoveBeamScanZ As System.Windows.Forms.Button
    Friend WithEvents btnMoveBeamScanX As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageZ As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageY As System.Windows.Forms.Button
    Friend WithEvents btnMoveStageX As System.Windows.Forms.Button
    Friend WithEvents btnSaveConfiguredPositionNew As System.Windows.Forms.Button
    Friend WithEvents nudAngleLens As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBeamScanY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBeamScanZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSaveConfiguredPosition As System.Windows.Forms.Button
    Friend WithEvents lstConfiguredPositions As System.Windows.Forms.ListBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents nudStageZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudBeamScanX As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudStageY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudStageX As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents dgvXPS As System.Windows.Forms.DataGridView
    Friend WithEvents btnXPS As System.Windows.Forms.Button
    Friend WithEvents btnStopMove As System.Windows.Forms.Button
    Friend WithEvents img As System.Windows.Forms.ImageList
    Friend WithEvents btnSyncXPS As System.Windows.Forms.Button
    Friend WithEvents tmrRefresh As System.Windows.Forms.Timer
    Friend WithEvents btnJogPanel As System.Windows.Forms.Button
    Friend WithEvents TabPageSetting As System.Windows.Forms.TabPage
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents dgvAlignment As System.Windows.Forms.DataGridView
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents dgvForceGauge As System.Windows.Forms.DataGridView
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents dgvSetting As System.Windows.Forms.DataGridView
    Friend WithEvents lblSaveSetting As System.Windows.Forms.Label
    Friend WithEvents btnSaveSettings As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubAnglePbs As System.Windows.Forms.Button
    Friend WithEvents btnJogAddAnglePbs As System.Windows.Forms.Button
    Friend WithEvents nudJogAnglePbs As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtAnglePbs As System.Windows.Forms.TextBox
    Friend WithEvents btnMoveAnglePbs As System.Windows.Forms.Button
    Friend WithEvents nudAnglePbs As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudCurrent As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtVoltage As System.Windows.Forms.TextBox
    Friend WithEvents optCH4 As System.Windows.Forms.RadioButton
    Friend WithEvents optCH3 As System.Windows.Forms.RadioButton
    Friend WithEvents optCH2 As System.Windows.Forms.RadioButton
    Friend WithEvents optCH1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents txtVacuumLens As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtForceLens As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtVacuumPbs As System.Windows.Forms.TextBox
    Friend WithEvents txtForcePbs As System.Windows.Forms.TextBox
    Friend WithEvents txtVacuumLine As System.Windows.Forms.TextBox
    Friend WithEvents txtCdaLine As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkProbeClampClose As System.Windows.Forms.CheckBox
    Friend WithEvents btnPiC843 As System.Windows.Forms.Button
    Friend WithEvents btnEpoxyTrigger As System.Windows.Forms.Button
    Friend WithEvents chkEpoxy As System.Windows.Forms.CheckBox
    Friend WithEvents chkProbeClampEnable As System.Windows.Forms.CheckBox
    Friend WithEvents chkVacuumCda As System.Windows.Forms.CheckBox
    Friend WithEvents btnVacuumCda As System.Windows.Forms.Button
    Friend WithEvents chkVacuumPackage As System.Windows.Forms.CheckBox
    Friend WithEvents chkVacuumPbs As System.Windows.Forms.CheckBox
    Friend WithEvents chkVacuumLens As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkGPIO3_6 As System.Windows.Forms.CheckBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents chkGPIO3_5 As System.Windows.Forms.CheckBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents chkGPIO3_4 As System.Windows.Forms.CheckBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents chkGPIO3_3 As System.Windows.Forms.CheckBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents chkGPIO3_2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents chkGPIO3_1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnClampORG As System.Windows.Forms.Button
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubProbe As System.Windows.Forms.Button
    Friend WithEvents btnJogAddProbe As System.Windows.Forms.Button
    Friend WithEvents nudJogProbe As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtProbe As System.Windows.Forms.TextBox
    Friend WithEvents btnMoveProbe As System.Windows.Forms.Button
    Friend WithEvents nudProbe As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ooUvLamp As BlackHawk.ucOnOff
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents nudUvTime As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents btnUV As System.Windows.Forms.Button
    Friend WithEvents nudUvPower As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents chkProbeCdaOn As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSaveGoldImage As System.Windows.Forms.Button
    Friend WithEvents lstGoldImage As System.Windows.Forms.ListBox
    Friend WithEvents txtTemperature As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents nudTemperature As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents chkLddProtection As System.Windows.Forms.CheckBox
    Friend WithEvents btnTrunLddOff As System.Windows.Forms.Button
    Friend WithEvents btnDeleteConfiguredPosition As System.Windows.Forms.Button
    Friend WithEvents nudLightSource22 As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLightSource21 As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLightSource12 As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLightSource11 As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkLightSource22 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLightSource21 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLightSource12 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLightSource11 As System.Windows.Forms.CheckBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents txtHexapodV As System.Windows.Forms.TextBox
    Friend WithEvents nudHexapodX As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtHexapodW As System.Windows.Forms.TextBox
    Friend WithEvents nudHexapodY As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtHexapodU As System.Windows.Forms.TextBox
    Friend WithEvents nudHexapodU As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtHexapodZ As System.Windows.Forms.TextBox
    Friend WithEvents nudHexapodZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtHexapodY As System.Windows.Forms.TextBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents txtHexapodX As System.Windows.Forms.TextBox
    Friend WithEvents lstHexapodConfiguredPositions As System.Windows.Forms.ListBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents btnSaveHexapodConfiguredPosition As System.Windows.Forms.Button
    Friend WithEvents nudHexapodW As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogHexapodPanel As System.Windows.Forms.Button
    Friend WithEvents nudHexapodV As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnStopHexapodMove As System.Windows.Forms.Button
    Friend WithEvents btnSaveHexapodConfiguredPositionNew As System.Windows.Forms.Button
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents btnMoveHexapodX As System.Windows.Forms.Button
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents btnMoveHexapodY As System.Windows.Forms.Button
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents btnMoveHexapodZ As System.Windows.Forms.Button
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents btnMoveHexapodU As System.Windows.Forms.Button
    Friend WithEvents btnSyncHexapod As System.Windows.Forms.Button
    Friend WithEvents btnMoveHexapodW As System.Windows.Forms.Button
    Friend WithEvents btnMoveHexapodXYZ As System.Windows.Forms.Button
    Friend WithEvents btnMoveHexapodV As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodV As System.Windows.Forms.Button
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubHexapodW As System.Windows.Forms.Button
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubHexapodU As System.Windows.Forms.Button
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubHexapodZ As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodX As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogSubHexapodY As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodY As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogAddHexapodV As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogAddHexapodW As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodU As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogAddHexapodU As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodW As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogAddHexapodZ As System.Windows.Forms.Button
    Friend WithEvents nudJogHexapodV As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnJogAddHexapodY As System.Windows.Forms.Button
    Friend WithEvents btnJogAddHexapodX As System.Windows.Forms.Button
    Friend WithEvents btnJogSubHexapodX As System.Windows.Forms.Button
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents btnJogSubPiLS As System.Windows.Forms.Button
    Friend WithEvents btnJogAddPiLS As System.Windows.Forms.Button
    Friend WithEvents nudJogPiLS As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtPiLS As System.Windows.Forms.TextBox
    Friend WithEvents btnMovePiLS As System.Windows.Forms.Button
    Friend WithEvents nudPiLS As System.Windows.Forms.NumericUpDown
End Class
