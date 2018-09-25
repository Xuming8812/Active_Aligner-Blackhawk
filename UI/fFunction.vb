Option Explicit On
'Option Strict On

Imports System.Data.SqlClient

Public Class fFunction
#Region "parameters and varibales"
    Private Structure ProcessStatus
        Public Running As Boolean
        Public [Stop] As Boolean
        Public Sync As Boolean
        Public AutoLoad As Boolean
        Public PartIndex As Integer
        Public FailedScript As String
    End Structure
    Private mProcess As ProcessStatus

    Private Enum UIPanelEnum
        PartTray
        Control
        RCX
        Hexapod
    End Enum
    Private mUiPanels() As Form

    Private mParent As fMain
    Private mIniFile As w2.w2IniFile
    Private mParaFile As w2.w2IniFileXML

    Private mScript As w2.w2ScriptEx

    Private mMsgInfoHost As w2.w2TextMessage
    Private mMsgInfo As w2.w2MessageService

    Private mMsgDataHost As w2.w2TextMessage
    Private mMsgData As w2.w2MessageService

    Private mLogin As w2.w2Login

    Private mDut As w2DUTEx

    Private mData As BlackHawkData
    Private mTool As BlackHawkFunction
    Private mStation As String


#End Region

    Public ReadOnly Property hData() As BlackHawkData
        Get
            Return mData
        End Get
    End Property

    Public Function Initialize(ByRef hParent As fMain, ByRef hIniFile As w2.w2IniFile, ByRef hLogin As w2.w2Login) As Boolean
        Dim sArgs() As String
        Dim s As String

        'passdown
        mParent = hParent
        mIniFile = hIniFile
        mLogin = hLogin

        'process data display
        mMsgInfoHost = New w2.w2TextMessage(txtData, lblStatus)
        mMsgInfoHost.Editable = True
        mMsgInfo = mMsgInfoHost.MessageService

        mMsgDataHost = New w2.w2TextMessage(txtSummary, lblStatus)
        mMsgDataHost.Editable = True
        mMsgData = mMsgDataHost.MessageService

        'GUI setting
        Me.SetupGUI()
        Me.Show()

        'test data engine
        mMsgInfo.PostMessage("Connecting to database and file server ... ")
        mData = New BlackHawkData()
        If Not mData.Initialize(hIniFile.FileName) Then
            mMsgInfo.PostMessage("X   Failed to connect to database")
            Return False
        End If

        'parameter file
        mMsgInfo.PostMessage("Read configuration parameters ... ")
        s = mIniFile.ReadParameter("File", "ParameterFile", "BlackHawkParameter.xml")
        s = IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, s)
        Try
            mParaFile = New w2.w2IniFileXML(s)
        Catch ex As Exception
            mMsgInfo.PostMessage("X   Failed to parse the parameter file " + s)
            mMsgInfo.PostMessage("    " + ex.Message)
            Return False
        End Try

        'start function 
        mTool = New BlackHawkFunction
        If Not mTool.Initialize(hParent, mIniFile, mParaFile, mData, mMsgInfo, mMsgData) Then Return False

        'additional UI
        ReDim mUiPanels([Enum].GetValues(GetType(UIPanelEnum)).Length - 1)
        Me.SpawnPanel(UIPanelEnum.PartTray)
        
        'script file
        s = ""
        sArgs = System.Environment.GetCommandLineArgs()
        If sArgs.Length >= 3 Then s = sArgs(2)
        If (s <> "") And (Not IO.File.Exists(s)) Then
            s = IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, s)
        End If

        'script helper
        mScript = New w2.w2ScriptEx(dgvScript)
        mScript.LoadScript(s)
        Me.SetScriptNameToTitle(mScript.ScriptName)

        'DUT GUI
        mDut = New w2DUTEx(dgvDUT)
        mDut.LoadDuts(10)

        'final GUI presentation - these need to be done at last
        Me.GetLotInfo()
        Me.UpdateLoginInfo()
        Me.EnableControls()

        Return True
    End Function

    Public Sub SetScriptNameToTitle(ByVal sScript As String)
        Dim s As String
        Dim p As Integer

        Const Key As String = " | Script - "

        s = mParent.Text
        p = s.IndexOf(key)
        If p > 0 Then s = s.Substring(0, p)

        mParent.Text = s + Key + sScript
    End Sub

    Private Sub GetLotInfo()
        Dim tray As fPartTray
        Dim i As Integer

        txtSN.Text = ""
        'txtOperator.Text = "Operator"    'this is now updated by the login process
        mStation = mIniFile.ReadParameter("Process", "StationName", "")

        'txtEpoxyLot.Text = mIniFile.ReadParameter("Process", "EpoxyLot", "")
        'txtLensLot.Text = mIniFile.ReadParameter("Process", "LensLot", "")
        'txtBsLot.Text = mIniFile.ReadParameter("Process", "PbsLot", "BS")

        nudPassIndex.DecimalPlaces = 0
        nudPassIndex.Minimum = 0
        nudPassIndex.Maximum = dgvDUT.RowCount
        nudPassIndex.Value = mIniFile.ReadParameter("Process", "DutPassIndex", 0)

        nudFailIndex.DecimalPlaces = 0
        nudFailIndex.Minimum = 0
        nudFailIndex.Maximum = dgvDUT.RowCount
        nudFailIndex.Value = mIniFile.ReadParameter("Process", "DutFailIndex", 0)

        tray = CType(mUiPanels(UIPanelEnum.PartTray), fPartTray)
        i = mIniFile.ReadParameter("Process", "LensIndex", 0)
        If i > 0 Then tray.FillParts(fPartTray.PartEnum.Lens, i)

        'todo
        i = mIniFile.ReadParameter("Process", "BS1Index", 0)
        If i > 0 Then tray.FillParts(fPartTray.PartEnum.BS1, i)
        i = mIniFile.ReadParameter("Process", "BS2Index", 0)
        If i > 0 Then tray.FillParts(fPartTray.PartEnum.BS2, i)
        i = mIniFile.ReadParameter("Process", "PBSIndex", 0)
        If i > 0 Then tray.FillParts(fPartTray.PartEnum.PBS, i)
    End Sub

    Private Sub SaveLotInfo()
        Dim tray As fPartTray

        'mIniFile.WriteParameter("Process", "EpoxyLot", txtEpoxyLot.Text)
        'mIniFile.WriteParameter("Process", "LensLot", txtLensLot.Text)
        'mIniFile.WriteParameter("Process", "PbsLot", txtBsLot.Text)

        tray = CType(mUiPanels(UIPanelEnum.PartTray), fPartTray)
        mIniFile.WriteParameter("Process", "LensIndex", tray.GetFirstFilledPartIndex(fPartTray.PartEnum.Lens).ToString())
        mIniFile.WriteParameter("Process", "BS1Index", tray.GetFirstFilledPartIndex(fPartTray.PartEnum.BS1).ToString())
        mIniFile.WriteParameter("Process", "BS2Index", tray.GetFirstFilledPartIndex(fPartTray.PartEnum.BS2).ToString())
        mIniFile.WriteParameter("Process", "PBSIndex", tray.GetFirstFilledPartIndex(fPartTray.PartEnum.PBS).ToString())

        mIniFile.WriteParameter("Process", "DutPassIndex", nudPassIndex.Value.ToString())
        mIniFile.WriteParameter("Process", "DutFailIndex", nudFailIndex.Value.ToString())
    End Sub

#Region "Public access points"
    Public Function ScriptBusy() As Boolean
        Return mProcess.Running
    End Function

    Public Sub UpdateLoginInfo()
        Dim enabled As Boolean
        Dim openScriptAccess As Boolean

        'fill info
        'txtOperator.Text = mLogin.Username
        'enable disable
        enabled = (mLogin.UserLevel < w2.w2Login.AccessLevel.Worker)
        openScriptAccess = mIniFile.ReadParameter("User Login", "OpenScriptAccess", False)
        If openScriptAccess Then
            menu.Items("File").Enabled = True
        Else
            menu.Items("File").Enabled = enabled
        End If
        menu.Items("File").Enabled = True
    End Sub

#End Region

#Region "GUI setup and conditional display"
    Private Sub EnableControls()
        Dim Enabled As Boolean
        Dim openScriptAccess As Boolean

        'condition
        'Enabled = (txtSN.Text.Trim() <> "") AndAlso (txtLensLot.Text <> "") AndAlso (txtEpoxyLot.Text <> "")
        'Enabled = Enabled And (txtOperator.Text <> "")

        Enabled = True

        'tool buttons
        btnRun.Enabled = Enabled
        btnAbort.Enabled = False
        btnPause.Enabled = False

        'script GUI display
        If Enabled Then
            dgvScript.DefaultCellStyle.BackColor = Color.White
        Else
            dgvScript.DefaultCellStyle.BackColor = Color.LightGray
        End If

        'conditional enable

        Select Case mLogin.UserLevel
            Case w2.w2Login.AccessLevel.Engineer, w2.w2Login.AccessLevel.SuperUser
                'only engineer can access the interface
                mScript.Enabled = Enabled
            Case Else
                openScriptAccess = mIniFile.ReadParameter("User Login", "OpenScriptAccess", False)
                If openScriptAccess Then
                    mScript.Enabled = Enabled
                Else
                    'always disable this
                    mScript.Enabled = False
                End If

        End Select

    End Sub

    Private Sub EnableTestMode(ByVal IsTestMode As Boolean)
        'tool buttons
        btnAbort.Enabled = IsTestMode
        btnRun.Enabled = Not IsTestMode

        'tbr.Items("Controller").Enabled = Not IsTestMode
        btnUnload.Enabled = Not IsTestMode
        'tbr.Items("Home All").Enabled = Not IsTestMode
        'tbr.Items("Camera View").Enabled = Not IsTestMode
        'tbr.Items("BG Cal").Enabled = Not IsTestMode

        'toggle the text for pause/resume
        If IsTestMode Then
            btnPause.Enabled = True
            btnPause.Text = "Pause"
            pbRunning.Show()
        ElseIf mScript.ScriptFinished Then
            btnPause.Enabled = False
            btnPause.Text = "Pause"
            pbRunning.Hide()
        Else
            btnPause.Enabled = True
            btnPause.Text = "Resume"
            pbRunning.Show()
        End If

        'no lens position change
        'nudLensIndex.Enabled = Not IsTestMode
        'nudEpoxyTray.Enabled = Not IsTestMode

        'GUI panels
        'If IsTestMode Then
        '    'Me.ClosePanel(UIPanelEnum.Controller)
        '    If mUiPanels(UIPanelEnum.Controller) IsNot Nothing Then mUiPanels(UIPanelEnum.Controller).Visible = False
        '    Application.DoEvents()
        'End If

        ''camera view special
        'Me.SetCameraButtonState(Not mTool.Instrument.VisionViewRunning)

        'refresh GUI
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub SetupGUI()
        Dim ctrl As List(Of Control)
        Dim cbo As ToolStripComboBox = Nothing

        'check box
        ctrl = New List(Of Control)
        w2.w2Misc.GetAllControls(Of TextBox)(Me, ctrl)
        For Each txt As TextBox In ctrl
            If TypeOf txt.Parent Is NumericUpDown Then Continue For
            AddHandler txt.TextChanged, AddressOf txt_TextChanged
            AddHandler txt.KeyDown, AddressOf txt_KeyDown
        Next

        ''control size
        scLeftRight.Width = Panel1.ClientSize.Width - scLeftRight.Left * 2
        scLeftRight.SplitterDistance = scLeftRight.Width \ 2

        'menu bar
        'menu.Font = New Font("Arial", 15, FontStyle.Bold)
        'menu.ImageList = img
        menu.Items.Add("File", Nothing).Name = "File"
        menu.Items.Add("Edit", Nothing).Name = "Edit"
        menu.Items.Add("System", Nothing).Name = "System"
        menu.Items.Add("Configuration", Nothing).Name = "Configuration"
        menu.Items.Add("Tools", Nothing).Name = "Tools"
        menu.Items.Add("Help", Nothing).Name = "Help"

        Dim toolItem As ToolStripMenuItem = CType(menu.Items("Tools"), ToolStripMenuItem)
        toolItem.DropDownItems.Add("Controller", Nothing, AddressOf menu_ItemClicked).Name = "Controller"
        toolItem.DropDownItems.Add("Robot", Nothing, AddressOf menu_ItemClicked).Name = "Robot"

        pbRunning.Hide()

        ''tbr.Items.Add("New Lot")
        ''tbr.Items.Add(New ToolStripSeparator)
        'menu.Items.Add("File")
        'menu.Items.Add("Script")
        'menu.Items.Add("Edit")
        'menu.Items.Add("System")
        'menu.Items.Add(New ToolStripSeparator)
        'menu.Items.Add("Configuration")
        ''tbr.Items.Add("Hexapod")
        'menu.Items.Add("Tools")
        'menu.Items.Add("Help")

        'For Each btn As ToolStripItem In menu.Items
        '    If btn.Text <> "" AndAlso btn.Name = "" Then
        '        btn.Name = btn.Text
        '        btn.ImageKey = btn.Text
        '        If btn.Name = "Hexapod" Then btn.ImageKey = "Controller"
        '    End If
        'Next

        'do this after name is given
        'smallFont = New Font("Arial", 16, FontStyle.Bold)
        'tbr.Items("Script").Font = smallFont
        'tbr.Items("Home All").Font = smallFont
        'tbr.Items("Camera View").Font = smallFont

        'tbr.Items("Camera View").Text = "Start Camera"
        'tbr.Items("BG Cal").ToolTipText = "Calibration BeamGage Background"
        'tbr.Items("Robot").ImageKey = "Controller"
        'tbr.Items("Auto").ImageKey = "Run"

    End Sub
#End Region

#Region "call backs"
    Private Sub menu_ItemClicked(sender As System.Object, e As System.EventArgs) Handles menu.ItemClicked
        Dim sKey As String = sender.Name 'CType(e, ToolStripItemClickedEventArgs).ClickedItem.Text

        Select Case sKey
            Case "Controller"
                Me.SpawnPanel(UIPanelEnum.Control)

            Case "Hexapod"
                Me.SpawnPanel(UIPanelEnum.Hexapod)

            Case "Robot"
                Me.SpawnPanel(UIPanelEnum.RCX)

            Case "New Lot"
                Dim s As String
                s = InputBox("Plese input the serial number for the new device", "New Device", "")
                s = s.ToUpper()
                txtSN.Text = s

            Case "Script"
                Me.LoadScript()

            Case "Stop", "Pause"
                mProcess.Stop = True
                mTool.Stop()

            Case "Unload"
                'Added by Ming

                mTool.StageTool.UnloadPart()

            Case "Home All"
                Dim success As Boolean
                success = mTool.StageTool.HomeAllStages()
                If success Then
                    MessageBox.Show("All stages initialized and homed successfully!", "Home All", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Failed to initialize and home some stages. Please check the log for detail.", "Home All", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            Case "Run", "Resume"
                If mProcess.Running Then Return
                mProcess.Running = True
                Me.EnableTestMode(True)
                Me.RunTestScriptAll(sKey = "Run")
                Me.SaveLotInfo()
                Me.EnableTestMode(False)
                mProcess.Running = False

            Case "Auto"
                'If mLogin.UserLevel = w2.w2Login.AccessLevel.SuperUser Then
                '    mParent.chkAuto.Checked = True
                '    mParent.Panel1.Show()
                '    e.ClickedItem.Text = "Manual"
                '    mParent.TabControlPanel.SelectedTab = mParent.TabPageTray
                'Else
                '    MessageBox.Show("必须管理员才能执行此操作！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If

            Case "Manual"
                'If mLogin.UserLevel = w2.w2Login.AccessLevel.SuperUser Then
                '    mParent.chkAuto.Checked = False
                '    mParent.Panel1.Hide()
                '    e.ClickedItem.Text = "Auto"
                '    mParent.TabControlPanel.SelectedTab = mParent.TabPagePlot
                'Else
                '    MessageBox.Show("必须管理员才能执行此操作！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If

            Case "Camera View", "Start Camera", "Stop Camera"
                'Select Case True
                '    Case sKey.Contains("Start")
                '        mTool.Instrument.StartCameraAndBeamGageView()
                '        Me.SetCameraButtonState(False)

                '    Case sKey.Contains("Stop")
                '        mTool.Instrument.StopCameraAndBeamGageView()
                '        Me.SetCameraButtonState(True)

                'End Select

            Case "BG Cal"
                'If mTool.Instrument.BeamScan Is Nothing Then
                '    MessageBox.Show("Beam Gage is not available for calibration")
                'Else
                '    mTool.CalibrateBeamScan()
                'End If
        End Select
    End Sub

    Private Sub SetCameraButtonState(ByVal SetForStart As Boolean)
        Dim x As ToolStripItem

        'x = tbr.Items("Camera View")
        'If SetForStart Then
        '    x.Text = "Start Camera"
        '    x.BackColor = tbr.BackColor
        'Else
        '    x.Text = "Stop Camera"
        '    x.BackColor = Color.LightGreen
        'End If

    End Sub

    Private Sub fFunction_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If mTool IsNot Nothing Then
            mTool.Instruments.CloseAll()
        End If
    End Sub

    Private Sub fFunction_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If mProcess.Running Then
            MessageBox.Show("Please stop the script before closing the windown", Me.Text, MessageBoxButtons.OK)
            e.Cancel = True
        End If
    End Sub

    Private Sub txt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As TextBox

        txt = CType(sender, TextBox)
        Select Case txt.Name
            Case txtData.Name, txtSummary.Name

            Case Else
                Me.EnableControls()
        End Select

    End Sub

    Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim txt As TextBox

        txt = CType(sender, TextBox)
        Select Case txt.Name
            Case txtData.Name, txtSummary.Name

                'Case txtStation.Name
                '    e.SuppressKeyPress = True

            Case Else
                e.SuppressKeyPress = mProcess.Running
                'we will allow the change for the other lot info
        End Select


    End Sub

    Private Sub txtSN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSN.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)
        If KeyAscii >= 97 And KeyAscii <= 122 Then
            KeyAscii = KeyAscii - 32    'a and A are offset by 32
            e.KeyChar = Chr(KeyAscii)
        End If
    End Sub


#End Region

#Region "UI panels"
    Private Sub SpawnPanel(ByVal Panel As UIPanelEnum)
        Dim f As Form

        mProcess.Sync = True

        f = mUiPanels(Panel)
        If f IsNot Nothing Then
            f.Visible = True
            f.BringToFront()
            If f.WindowState = FormWindowState.Minimized Then f.WindowState = FormWindowState.Normal
            f.Select()
            Return
        End If

        Select Case Panel
            Case UIPanelEnum.Control
                f = New fControl(mTool)
                f.Show()

            Case UIPanelEnum.Hexapod
                f = New fHexapod(mTool)
                f.Show()

            Case UIPanelEnum.RCX

                f = New fRCX
                If Not CType(f, fRCX).Initialize(mTool) Then
                    f = Nothing
                    Return
                End If
                f.Show()

            Case UIPanelEnum.PartTray
                f = New fPartTray()
                'AddHandler CType(f, fPartTray).PartUpdated, AddressOf fPartTray_PartUpdated
                f.TopLevel = False
                f.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                f.Parent = panelPartTray
                f.Location = New Point(0, 0)
                f.Show()
            Case Else
                Return
        End Select

        If f.TopLevel Then
            f.Left = (Screen.PrimaryScreen.WorkingArea.Width - f.Width) \ 2
            f.Top = (Screen.PrimaryScreen.WorkingArea.Height - f.Height) \ 2
        Else
            If f.Parent Is Nothing Then
                f.MdiParent = mParent
                f.Location = New Drawing.Point(0, 0)
            End If
        End If
        f.BringToFront()
        f.Focus()
        Application.DoEvents()

        'set back
        mUiPanels(Panel) = f

        'call backs
        AddHandler f.FormClosed, AddressOf f_FormClosed

        'release 
        mProcess.Sync = False
    End Sub

    Private Function ClosePanels() As Boolean
        For Each f As Form In mUiPanels
            If f IsNot Nothing Then
                f.Close()
                If f.Visible Then Return False
            End If
        Next
        Return True
    End Function

    Private Sub ClosePanel(ByVal Panel As UIPanelEnum)
        If mUiPanels(Panel) Is Nothing Then Return
        mUiPanels(Panel).Close()
    End Sub

    Private Sub f_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        Select Case True
            Case TypeOf sender Is fControl
                mUiPanels(UIPanelEnum.Control) = Nothing

            Case TypeOf sender Is fHexapod
                mUiPanels(UIPanelEnum.Hexapod) = Nothing

        End Select
    End Sub

#End Region

    Private Sub fFunction_VisibleChanged(sender As Object, e As System.EventArgs) Handles Me.VisibleChanged
        dgvDUT.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        dgvScript.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
    End Sub

    Private Function tbr() As Object
        Throw New NotImplementedException
    End Function


End Class