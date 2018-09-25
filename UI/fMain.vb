Option Explicit On
Option Strict On
Option Infer Off

Public Class fMain
    Private mIniFile As w2.w2IniFile
    Private mLogin As w2.w2Login
    Private mFunction As fFunction

    Public Function Initialize(ByVal sIniFile As String, ByVal hLogin As w2.w2Login) As Boolean
        'pass down
        mIniFile = New w2.w2IniFile(sIniFile)
        mLogin = hLogin

        'GUI
        Me.Text = "BlackHawk for OC9501 Project | Rev " + Application.ProductVersion
        Me.Cursor = Cursors.WaitCursor
        Me.IsMdiContainer = True
        Me.WindowState = FormWindowState.Maximized
        Me.KeyPreview = True
        Me.SetupGUI()
        Me.Show()
        Application.DoEvents()

        'spwan function panel
        Me.SpawnFunctionPanel()

        'done
        Me.Cursor = Cursors.Default
        Me.Refresh()

        Return True
    End Function

    Private Sub SpawnFunctionPanel()
        'start function form
        mFunction = New fFunction

        'put it to the panel
        pFunction.Width = mIniFile.ReadParameter("GUI", "PanelWidth", pFunction.Width)
        mFunction.TopLevel = False
        mFunction.FormBorderStyle = Windows.Forms.FormBorderStyle.None

        Dim h As Integer = mFunction.scLeftRight.SplitterDistance
        pFunction.Controls.Add(mFunction)
        mFunction.Dock = DockStyle.Fill
        mFunction.scLeftRight.SplitterDistance = h
        mFunction.Focus()

        'do this last, this may be slow, but we at least have GUI setup
        mFunction.Initialize(Me, mIniFile, mLogin)
    End Sub

#Region "building GUI"
    Private Sub SetupGUI()
        'tool bar
        'tbr.Font = New Font("Arial", 10, FontStyle.Bold)
        'tbr.ImageList = img

        'tbr.Items.Add("Login")
        'tbr.Items.Add("User Account")
        'tbr.Items.Add(New ToolStripSeparator)
        'tbr.Items.Add("Data Folder")
        'tbr.Items.Add(New ToolStripDropDownButton("View Plot"))
        'tbr.Items.Add(New ToolStripSeparator)
        'tbr.Items.Add(New ToolStripDropDownButton("Save"))
        'tbr.Items.Add(New ToolStripDropDownButton("Print"))
        'tbr.Items.Add(New ToolStripSeparator)
        'tbr.Items.Add("Close All Plots")
        'For Each btn As ToolStripItem In tbr.Items
        '    If btn.Text <> "" AndAlso btn.Name = "" Then
        '        btn.Name = btn.Text
        '        btn.ImageKey = btn.Text
        '    End If
        'Next

        'drop down menu for print and save
        'Dim menu As New ToolStripDropDownMenu
        'menu.ImageList = img
        'menu.Items.Add("Message")
        'menu.Items.Add("Table")
        ''menu.Items.Add("Plot")
        'For Each btn As ToolStripItem In menu.Items
        '    If btn.Text <> "" AndAlso btn.Name = "" Then
        '        btn.Name = btn.Text
        '        btn.ImageKey = btn.Text
        '        AddHandler btn.Click, AddressOf DropDownMenu_Click
        '    End If
        'Next

        'CType(tbr.Items("Save"), ToolStripDropDownButton).DropDown = menu
        'CType(tbr.Items("Print"), ToolStripDropDownButton).DropDown = menu

        pFunction.Width = mIniFile.ReadParameter("GUI", "PanelWidth", pFunction.Width)

        'update screen
        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private mDropDownMenuParent As String = ""
    Private Sub DropDownMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If mDropDownMenuParent = "" Then Return

        Dim cdl As New SaveFileDialog
        Dim x As ToolStripItem = CType(sender, ToolStripItem)

        'Select Case x.Text
        '    Case "Message"
        '        Select Case mDropDownMenuParent
        '            Case "Save"
        '                cdl.OverwritePrompt = True
        '                cdl.Title = "Save process message"
        '                cdl.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
        '                cdl.FilterIndex = 1
        '                If Windows.Forms.DialogResult.OK <> cdl.ShowDialog() Then Return
        '                mFunction.ProcessMessage.SaveToFile(cdl.FileName)
        '            Case "Print"
        '                mFunction.ProcessMessage.Print()
        '        End Select

        '    Case "Table"
        '        Select Case mDropDownMenuParent
        '            Case "Save"
        '                cdl.OverwritePrompt = True
        '                cdl.Title = "Save process script"
        '                cdl.Filter = "CSV (comma delimited value) file (*.csv)|*.csv"
        '                If Windows.Forms.DialogResult.OK <> cdl.ShowDialog() Then Return
        '                w2.DataGridViewHelper.Helper.SaveTextToFile(mFunction.dgv, cdl.FileName, ",")
        '            Case "Print"
        '                w2.DataGridViewHelper.Helper.Print(mFunction.dgv, w2.DataGridViewHelper.PrintPageOrientation.Portrait, True)
        '        End Select

        'End Select

    End Sub

#End Region

#Region "menu handling"
    Private Sub tbr_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        Dim sKey As String = e.ClickedItem.Text

        Select Case sKey
            Case "Login"
                mLogin.Login(mLogin.Username)
                Me.UpdateLoginInfo()

            Case "User Account"
                mLogin.ManageAccounts()
                Me.UpdateLoginInfo()

            Case "Data Folder"
                Me.OpenDataFolder()

        End Select
    End Sub

    Private Sub OpenDataFolder()
        Dim SerialNumber, Text As String

        'default
        SerialNumber = ""
        Text = ""

        'get serial number from active panel
        If mFunction IsNot Nothing Then
            SerialNumber = mFunction.txtSN.Text
        End If

        'request SN
        If SerialNumber = "" Then
            Text = "Please enter a laser serial number to start"
            SerialNumber = Microsoft.VisualBasic.InputBox(Text, Me.Name)
            SerialNumber = SerialNumber.Trim()
            If SerialNumber = "" Then Return
        End If

        'get data path
        Text = mFunction.hData.GetDataPath(SerialNumber, False, Now)

        'spawn data folder
        If Text <> "" Then Process.Start(Text)
    End Sub

    Public Sub UpdateLoginInfo()
        'tbr.Items("User Account").Enabled = (mLogin.UserLevel < w2.w2Login.AccessLevel.Worker)
        If mFunction IsNot Nothing Then mFunction.UpdateLoginInfo()
    End Sub

#End Region

#Region "callback"
    Private Sub fMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        'check if we can close
        If mFunction IsNot Nothing Then
            If mFunction.ScriptBusy Then
                MessageBox.Show("Please stop the script before closing the windown", Me.Text, MessageBoxButtons.OK)
                e.Cancel = True
            End If
            mFunction .Close ()
        End If
    End Sub

    Private Sub fMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F1

            Case Keys.F2

        End Select
    End Sub

    Private Sub fMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim width As Integer

        If Not Me.Visible Then Return

        width = mIniFile.ReadParameter("GUI", "PanelWidth", 0)

        Select Case Me.Width
            Case Is < pFunction.Width
                pFunction.Width = 4 * Me.Width \ 5
            Case Is > width
                If width > 0 Then pFunction.Width = width
        End Select

    End Sub

    Private Sub Splitter1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs)
        pFunction.Width = e.X
        mIniFile.WriteParameter("GUI", "PanelWidth", pFunction.Width.ToString())
    End Sub

#End Region

End Class
