Option Explicit On
Option Strict On

Public Class w2NumericUpDownHelper
    Private mMenu As ContextMenuStrip
    Private mDisplay As ToolStripItem
    Private mToolTip As ToolTip

    Private mNud As NumericUpDown
    Private mMin As Decimal
    Private mMax As Decimal

    Public Sub New(ByVal hNud As NumericUpDown, ByVal sConfigFile As String)
        Dim iniFile As w2.w2IniFile
        Dim s, section() As String

        'ctrl
        mNud = hNud
        s = mNud.Name

        'config
        sConfigFile = IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, sConfigFile)
        If IO.File.Exists(sConfigFile) Then
            iniFile = New w2.w2IniFile(sConfigFile)
            'check if we have condig
            section = iniFile.GetAllSections()
            If Array.IndexOf(section, s) >= 0 Then
                'config
                With mNud
                    .DecimalPlaces = iniFile.ReadParameter(s, "DecimalPlaces", 1)
                    .Minimum = Convert.ToDecimal(iniFile.ReadParameter(s, "Minimum", 0.0))
                    .Maximum = Convert.ToDecimal(iniFile.ReadParameter(s, "Maximum", 100.0))
                    .Increment = Convert.ToDecimal(iniFile.ReadParameter(s, "Increment", 1.0))
                    .Value = Convert.ToDecimal(iniFile.ReadParameter(s, "Value", 1.0))
                End With
            Else
                MessageBox.Show("No configuration entry for " + s, "UI Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        Else
            MessageBox.Show("Cannot found user interface configuration file " + sConfigFile, "UI Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        'the maximum increment will be one tenth of maximum value
        mMax = 0.1D * mNud.Maximum
        'minimum will be harder as it may be zero or negative
        mMin = Convert.ToDecimal(1.0 / 10 ^ mNud.DecimalPlaces)
        If mNud.Minimum > 0 And mNud.Minimum < mMin Then
            mMin = mNud.Minimum
        End If

        'set up menu
        mMenu = New ContextMenuStrip
        With mMenu.Items
            mDisplay = .Add("Increment Step = " & mNud.Increment)
            .Add("Increase Increment 10X")
            .Add("Decrease Increment 10X")
            .Add("Set Increment to ...")
        End With

        'tool tip
        mToolTip = New ToolTip()
        mToolTip.SetToolTip(mNud.Controls(0), mDisplay.Text)

        'pass down
        mNud.ContextMenuStrip = mMenu

        'add event handler
        AddHandler mMenu.ItemClicked, AddressOf ContextMenu_ItemClicked

    End Sub

    Private Sub ContextMenu_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        Dim v As Decimal
        Dim s As String

        Select Case e.ClickedItem.Text.Substring(0, 2)
            Case "In"
                v = 10 * mNud.Increment

            Case "De"
                v = 0.1D * mNud.Increment

            Case "Se"
                s = InputBox("Set the new increment value", "", mNud.Increment.ToString())
                s = s.Trim()
                If s = "" Then Return
                If Not Decimal.TryParse(s, v) Then Return
        End Select

        If v > mMax Then v = mMax
        If v < mMin Then v = mMin

        mNud.Increment = v
        mDisplay.Text = ("Increment Step = " & mNud.Increment)
        mToolTip.SetToolTip(mNud.Controls(0), mDisplay.Text)
    End Sub
End Class

Public Class w2ConfigTableHelper
    Private Enum ColIndex
        Text
        Value
        Section
        Key
    End Enum

    Private mIniFile As w2.w2IniFileXML
    Private mGuiFile As w2.w2IniFileXML

    Public Sub New(ByVal sConfigFile As String, ByVal sGuiFile As String)
        mIniFile = New w2.w2IniFileXML(sConfigFile)
        mGuiFile = New w2.w2IniFileXML(sGuiFile)
    End Sub

    Public Sub SetupTable(ByVal dgv As DataGridView)
        'add event handler for error handling
        AddHandler dgv.DataError, AddressOf dgv_DataError

        'build GUI
        With dgv
            .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Bottom

            .EditMode = DataGridViewEditMode.EditOnEnter

            .RowCount = 0
            .RowHeadersVisible = False
            .ColumnCount = [Enum].GetValues(GetType(ColIndex)).Length
            .Columns(ColIndex.Section).Visible = False
            .Columns(ColIndex.Key).Visible = False

            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)

            .DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 10, FontStyle.Regular)

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .ReadOnly = False

            .Columns(ColIndex.Text).HeaderText = "Function"
            .Columns(ColIndex.Text).ReadOnly = True

            .Columns(ColIndex.Value).HeaderText = "Value"
            .Columns(ColIndex.Value).ReadOnly = False
            .Columns(ColIndex.Value).ValueType = GetType(Decimal)

            For i = 0 To .ColumnCount - 1
                .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .Columns(ColIndex.Value).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .AutoResizeColumns()
            .AutoResizeRows()

            .ScrollBars = ScrollBars.Vertical
            'do not show selection
            .MultiSelect = False
        End With
    End Sub

    Public Sub PopulateTable(ByVal dgv As DataGridView)
        Dim r As Integer
        Dim s, msg, sText, sSection, sKey, sValue As String
        Dim Lines(), Data() As String
        Dim xFont As Font

        'build header font
        xFont = New Font(dgv.DefaultCellStyle.Font.Name, dgv.DefaultCellStyle.Font.Size, FontStyle.Bold)

        'get GUI instructions
        sSection = dgv.Name
        s = mGuiFile.ReadParameter(sSection, "Data", "")
        Lines = s.Split(ControlChars.Cr)

        'populate the table
        dgv.Rows.Clear()
        For Each s In Lines
            s = s.Trim()

            If s = "" Then Continue For
            If s.StartsWith(";") Then Continue For

            Data = s.Split(ControlChars.Tab)
            sText = Data(0).Trim()

            If sText.StartsWith("[") And sText.EndsWith("]") Then
                r = dgv.Rows.Add(Data(0))
                dgv.Rows(r).DefaultCellStyle.Font = xFont
                dgv.Rows(r).DefaultCellStyle.ForeColor = Color.DarkCyan
                dgv.Rows(r).ReadOnly = True

            ElseIf Data.Length = 3 Then
                sSection = Data(1).Trim()
                sKey = Data(2).Trim()
                sValue = mIniFile.ReadParameter(sSection, sKey, "").Trim()

                r = dgv.Rows.Add(sText, sValue, sSection, sKey)
                If IsNumeric(sValue) Then
                    dgv.Item(ColIndex.Value, r).ValueType = GetType(Decimal)
                Else
                    dgv.Item(ColIndex.Value, r).ValueType = GetType(String)
                End If
            Else
                msg = "Error parsing GUI instruction file "
                msg += ControlChars.CrLf + "File" + ControlChars.CrLf + mGuiFile.FileName
                msg += ControlChars.CrLf + "Table" + ControlChars.CrLf + dgv.Name
                msg += ControlChars.CrLf + "Line" + ControlChars.CrLf + s

                MessageBox.Show(msg, "Config File GUI", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        Next
    End Sub

    Public Sub SaveTable(ByVal dgv As DataGridView)
        Dim sValue, sSection, sKey As String
        Dim row As DataGridViewRow

        For Each row In dgv.Rows
            'note that the order is fixed when the table is populated
            sValue = CType(row.Cells(ColIndex.Value).Value, String)
            sSection = CType(row.Cells(ColIndex.Section).Value, String)
            sKey = CType(row.Cells(ColIndex.Key).Value, String)
            'save data
            If sSection = "" Or sKey = "" Then Continue For
            mIniFile.WriteParameter(sSection, sKey, sValue)
        Next
    End Sub

    Private Sub dgv_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)

        If e.RowIndex < 0 Then Return
        If e.ColumnIndex < 0 Then Return

        Dim Cell As DataGridViewCell = CType(sender, DataGridView).Item(e.ColumnIndex, e.RowIndex)

        If Cell.ValueType.Equals(GetType(Date)) Then
            MessageBox.Show("Please enter a valid date!", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf Cell.ValueType.Equals(GetType(Decimal)) Then
            MessageBox.Show("Please enter an enumeric number!", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(e.Exception.Message, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class

Public Class w2ComboListItem

    Private mValue As Integer
    Private mText As String

    Public Sub New(ByVal Value As Integer, ByVal Text As String)
        mValue = Value
        mText = Text
    End Sub

    Public ReadOnly Property Value() As Integer
        Get
            Return mValue
        End Get
    End Property

    Public ReadOnly Property Text() As String
        Get
            Return mText
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return mText
    End Function

    Public Shared Function BuildList(ByVal EnumType As System.Type) As w2ComboListItem()
        Dim Value, Text As Array
        Dim i, ii As Integer
        Dim data As w2ComboListItem()

        Value = [Enum].GetValues(EnumType)
        Text = [Enum].GetNames(EnumType)

        ii = Value.Length - 1
        ReDim data(ii)
        For i = 0 To ii
            data(i) = New w2ComboListItem(CType(Value.GetValue(i), Int32), CType(Text.GetValue(i), String))
        Next

        Return data
    End Function
End Class