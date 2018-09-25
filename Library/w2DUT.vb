Option Explicit On
Option Strict On

Public Class w2DUTEx
    Public Enum ColIndex
        Selected
        SN
        'Alps
        'LightPath
        Status
        Results
        Log
    End Enum

    Public Enum ItemAccessStatus
        Running
        Passed
        Stopped
        Failed
        UnkownSkipped
    End Enum

    Public mItems As List(Of w2DutItem)
    Private WithEvents mdgv As DataGridView
    Private mInternalChange As Boolean

    Public Sub New(ByVal dgv As DataGridView)
        MyBase.New()
        mdgv = dgv
        mItems = New List(Of w2DutItem)

        Me.SetupGUI()
    End Sub

    Public Property Enabled() As Boolean
        Get
            Return mdgv.Enabled
        End Get
        Set(ByVal value As Boolean)
            mdgv.Enabled = value
            If value Then
                mdgv.DefaultCellStyle.BackColor = Color.White
            Else
                mdgv.DefaultCellStyle.BackColor = Color.LightGray
            End If
        End Set
    End Property

    Public Sub ClearTable()
        Dim Row As DataGridViewRow
        mInternalChange = True
        For Each Row In mdgv.Rows
            Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor

            Row.Cells(ColIndex.Status).Value = ""
            Row.Cells(ColIndex.Status).Style.BackColor = mdgv.DefaultCellStyle.BackColor
            Row.Cells(ColIndex.Results).Value = ""
            Row.Cells(ColIndex.Log).Value = ""
        Next
    End Sub

    Public Function IsSelectedToRun(ByVal Index As Integer) As Boolean
        Return Convert.ToBoolean(mdgv.Rows(Index).Cells(ColIndex.Selected).Value)
    End Function

    Public Sub HighLightRow(ByVal Index As Integer, ByVal highlight As ItemAccessStatus, ByVal ResultText As String, ByVal LogFilePath As String)
        Dim Row As DataGridViewRow = mdgv.Rows(Index)

        'do base class
        Me.LastAccessedItem = Index
        Me.LastAccessedItemStatus = highlight

        'do GUI
        Select Case highlight
            Case ItemAccessStatus.Running
                'If StatusText = "" Then StatusText = "RUNNING"
                Row.DefaultCellStyle.BackColor = Color.LightSeaGreen
                Row.Cells(ColIndex.Status).Value = "RUNNING"
                Row.Cells(ColIndex.Status).Style.BackColor = Color.Red

            Case ItemAccessStatus.Failed
                'If StatusText = "" Then StatusText = Date.Now.ToString("HH:mm:ss")
                Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor
                Row.Cells(ColIndex.Status).Value = "FAIL"
                Row.Cells(ColIndex.Status).Style.BackColor = Color.Red

            Case ItemAccessStatus.Stopped
                'If StatusText = "" Then StatusText = Date.Now.ToString("HH:mm:ss")
                Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor
                Row.Cells(ColIndex.Status).Value = "STOPPED"
                Row.Cells(ColIndex.Status).Style.BackColor = Color.Yellow

            Case ItemAccessStatus.Passed
                'If StatusText = "" Then StatusText = Date.Now.ToString("HH:mm:ss")
                Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor
                Row.Cells(ColIndex.Status).Value = "PASS"
                Row.Cells(ColIndex.Status).Style.BackColor = Color.LawnGreen

            Case ItemAccessStatus.UnkownSkipped
                'If StatusText = "" Then StatusText = "Unknown Script!"
                Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor
                Row.Cells(ColIndex.Status).Value = "SKIPPED"
                Row.Cells(ColIndex.Status).Style.BackColor = Color.BlueViolet

        End Select

        Row.Cells(ColIndex.Results).Value = ResultText
        Row.Cells(ColIndex.Log).Value = LogFilePath

        If mdgv.Visible Then
            mdgv.FirstDisplayedScrollingRowIndex = Index
            mdgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        End If

        mdgv.Refresh()
    End Sub

    Public Sub LoadDuts(ByVal number As Integer)

        mdgv.Rows.Add(number)
        For i As Integer = 1 To number
            mdgv.Rows(i - 1).HeaderCell.Value = i.ToString()
        Next

    End Sub

    Private Sub SetupGUI()
        Dim i As Integer

        mInternalChange = True

        With mdgv
            .RowHeadersVisible = True
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False

            .ColumnCount = [Enum].GetValues(GetType(ColIndex)).Length

            .DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 8)
            .RowHeadersDefaultCellStyle.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            '.ColumnHeadersHeight = 30
            .RowHeadersWidth = 50

            .Columns.RemoveAt(ColIndex.Selected)
            .Columns.Insert(ColIndex.Selected, New DataGridViewCheckBoxColumn)
            '.Columns.RemoveAt(ColIndex.Alps)
            '.Columns.Insert(ColIndex.Alps, New DataGridViewCheckBoxColumn)
            '.Columns.RemoveAt(ColIndex.LightPath)
            '.Columns.Insert(ColIndex.LightPath, New DataGridViewCheckBoxColumn)

            .Columns(ColIndex.Selected).HeaderText = "Selected?"
            .Columns(ColIndex.SN).HeaderText = "   SN   "
            '.Columns(ColIndex.Alps).HeaderText = "Alps"
            '.Columns(ColIndex.LightPath).HeaderText = "LightPath"
            .Columns(ColIndex.Status).HeaderText = "Status"
            .Columns(ColIndex.Results).HeaderText = "Results"
            .Columns(ColIndex.Log).HeaderText = "Log"

            .ReadOnly = False
            For i = 0 To .ColumnCount - 1
                .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(i).ReadOnly = True
            Next

            .Columns(ColIndex.SN).ReadOnly = False
            .Columns(ColIndex.Selected).ReadOnly = False
            '.Columns(ColIndex.Alps).ReadOnly = False
            '.Columns(ColIndex.LightPath).ReadOnly = False
            .Columns(ColIndex.Log).ReadOnly = False

            .Columns(ColIndex.SN).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing

        End With

        Dim cm As New w2.w2ContextMenu(mdgv)
        cm.Remove("Delete")
        cm.MenuHandlingSuppressed = True
        AddHandler mdgv.ContextMenuStrip.ItemClicked, AddressOf dgvContextMenu_ItemClicked

        mInternalChange = False

    End Sub

    Private Sub dgvContextMenu_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        'flag
        mInternalChange = True

        'do change
        CType(sender, ContextMenuStrip).Visible = False
        Select Case e.ClickedItem.Text
            Case "Copy"
                mdgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
                mdgv.Focus()
                SendKeys.Send("^C")

            Case "Select None"
                w2.DataGridViewHelper.Helper.ClearColumnValue(mdgv.Columns(mdgv.SelectedCells(0).ColumnIndex))
                'Select Case mdgv.SelectedCells(0).ColumnIndex
                '    Case ColIndex.Alps
                '        w2.DataGridViewHelper.Helper.ClearColumnValue(mdgv.Columns(ColIndex.Alps))
                '    Case 3
                '        w2.DataGridViewHelper.Helper.ClearColumnValue(mdgv.Columns(ColIndex.LightPath))
                '    Case Else
                '        w2.DataGridViewHelper.Helper.ClearColumnValue(mdgv.Columns(ColIndex.Selected))
                'End Select

            Case "Select All"
                w2.DataGridViewHelper.Helper.SetColumnValue(mdgv.Columns(mdgv.SelectedCells(0).ColumnIndex), 0, mdgv.RowCount - 1, True)
                'Select Case mdgv.SelectedCells(0).ColumnIndex
                '    Case 2
                '        w2.DataGridViewHelper.Helper.SetColumnValue(mdgv.Columns(ColIndex.Alps), 0, mdgv.RowCount - 1, True)
                '    Case 3
                '        w2.DataGridViewHelper.Helper.SetColumnValue(mdgv.Columns(ColIndex.LightPath), 0, mdgv.RowCount - 1, True)
                '    Case Else
                '        w2.DataGridViewHelper.Helper.SetColumnValue(mdgv.Columns(ColIndex.Selected), 0, mdgv.RowCount - 1, True)
                'End Select

            Case "Clear All"
                For Each Row As DataGridViewRow In mdgv.Rows
                    Row.DefaultCellStyle.BackColor = mdgv.DefaultCellStyle.BackColor
                    Row.Cells(ColIndex.Selected).Value = False
                    'Row.Cells(ColIndex.Alps).Value = False
                    'Row.Cells(ColIndex.LightPath).Value = False
                    Row.Cells(ColIndex.SN).Value = ""
                    Row.Cells(ColIndex.Status).Value = ""
                    Row.Cells(ColIndex.Status).Style.BackColor = mdgv.DefaultCellStyle.BackColor
                    Row.Cells(ColIndex.Results).Value = ""
                    Row.Cells(ColIndex.Log).Value = ""
                Next
        End Select

        'flag
        mInternalChange = False
    End Sub

    Private Sub mdgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles mdgv.CellValueChanged
        'ignore process if not called from GUI
        If mInternalChange Then Return

        'set up flag now
        mInternalChange = True

        Select Case mdgv.CurrentCell.ColumnIndex
            Case ColIndex.Selected
        End Select

        'clear flag
        mInternalChange = False
    End Sub

    Private Sub mdgv_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mdgv.CurrentCellDirtyStateChanged
        If mdgv.CurrentCell.ColumnIndex = ColIndex.Selected AndAlso mdgv.IsCurrentCellDirty Then
            mdgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub mdgv_DoubleClick(sender As Object, e As System.EventArgs) Handles mdgv.DoubleClick
        Dim s As String

        Try
            If mdgv.CurrentCell Is Nothing Then Return
            If mdgv.CurrentCell.ColumnIndex <> ColIndex.Log Then Return
            s = w2.DataGridViewHelper.Helper.GetCellText(mdgv.CurrentCell).ToString()
            If s <> "" Then Process.Start(s)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub mdgv_VisibleChanged(sender As Object, e As System.EventArgs) Handles mdgv.VisibleChanged
        If mdgv.Visible Then
            mdgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        End If
    End Sub


    Public Function ParseDuts() As Boolean
        mItems.Clear()
        LastAccessedItem = 0

        For i As Integer = 1 To mdgv.Rows.Count
            mItems.Add(New w2DutItem(i, Convert.ToBoolean(mdgv.Rows(i - 1).Cells(ColIndex.Selected).Value), Convert.ToString(mdgv.Rows(i - 1).Cells(ColIndex.SN).Value)))

            'make sure SN is not empty
            If Convert.ToBoolean(mdgv.Rows(i - 1).Cells(ColIndex.Selected).Value) Then
                If Convert.ToString(mdgv.Rows(i - 1).Cells(ColIndex.SN).Value) = "" Then
                    MessageBox.Show("SN Cannot Be Empty for Any Selected Unit!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            End If
        Next

        Return True

    End Function

    Public ReadOnly Property Items As List(Of w2DutItem)
        Get
            Return mItems
        End Get
    End Property

    Protected mLastAccessedItem As Integer
    Protected mLastAccessedItemStatus As ItemAccessStatus

    Public ReadOnly Property DutFinished() As Boolean
        Get
            Dim Finished As Boolean
            Finished = (mLastAccessedItemStatus = ItemAccessStatus.UnkownSkipped) Or (mLastAccessedItemStatus = ItemAccessStatus.Passed)
            Return (mLastAccessedItem = mItems.Count - 1) And Finished
        End Get
    End Property

    Public Property LastAccessedItem() As Integer
        Get
            Return mLastAccessedItem
        End Get
        Set(ByVal value As Integer)
            mLastAccessedItem = value
        End Set
    End Property

    Public Property LastAccessedItemStatus() As ItemAccessStatus
        Get
            Return mLastAccessedItemStatus
        End Get
        Set(ByVal value As ItemAccessStatus)
            mLastAccessedItemStatus = value
        End Set
    End Property

    Public Function QueryResumeTestEntryIndex() As Integer
        Dim s As String
        Dim iStart, iDisplay As Integer
        Dim response As DialogResult

        If Me.DutFinished Then
            s = "Script already finished. Restart from the beginning?"
            response = MessageBox.Show(s, "Resume Test", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If response = DialogResult.Yes Then
                iStart = 0
            Else
                iStart = -1
            End If
        Else
            Select Case mLastAccessedItemStatus
                Case ItemAccessStatus.Passed, ItemAccessStatus.UnkownSkipped
                    iStart = mLastAccessedItem + 1
                Case Else
                    iDisplay = mLastAccessedItem + 1     'THE DISPLAY IS 1 BASED, INTERNAL IS 0 BASED
                    s = "Step " & iDisplay.ToString() + ": " + mItems(mLastAccessedItem).SN
                    s += ControlChars.CrLf + "is not finished successfully."
                    s += ControlChars.CrLf + "Do you want to skip that and resume from Step " + (iDisplay + 1).ToString()
                    response = MessageBox.Show(s, "Resume Test", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    If response = DialogResult.Yes Then
                        iStart = mLastAccessedItem + 1
                    Else
                        iStart = mLastAccessedItem
                    End If
            End Select
        End If

        Return iStart
    End Function


End Class

Public Class w2DutItem
    Private mNumber As Integer
    Private mSelected As Boolean
    Private mSN As String
    Private mStatus As String
    Private mResults As String
    Private mLog As String

    Public Sub New(ByVal Number As Integer, ByVal IsSelected As Boolean, ByVal SN As String)
        mNumber = Number
        mSelected = IsSelected
        mSN = SN
    End Sub

    Public ReadOnly Property Number() As Integer
        Get
            Return mNumber
        End Get
    End Property

    Public ReadOnly Property IsSelected() As Boolean
        Get
            Return mSelected
        End Get
    End Property

    Public ReadOnly Property SN() As String
        Get
            Return mSN
        End Get
    End Property

    Public Property Status() As String
        Get
            Return mStatus
        End Get
        Set(value As String)
            mStatus = value
        End Set
    End Property

    Public Property Results() As String
        Get
            Return mResults
        End Get
        Set(value As String)
            mResults = value
        End Set
    End Property

    Public Property Log() As String
        Get
            Return mLog
        End Get
        Set(value As String)
            mLog = value
        End Set
    End Property
End Class
