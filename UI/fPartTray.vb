Option Explicit On
Option Strict On

Public Class fPartTray
    Private Const Gap As Integer = 5
    Private Const PartWidth As Integer = 32
    'Private PartHeight() As Integer = New Integer() {30, 42}
    Private PartHeight() As Integer = New Integer() {25, 35, 35, 35}

    Private Enum ClickMode
        FillRest
        FillOne
        EmptyOne
        None
    End Enum

    Public Enum PartEnum
        'do not change order, this determinds their position
        Lens
        PBS
        BS2
        BS1
    End Enum
    Public Shared ReadOnly PartCount As Integer = [Enum].GetValues(GetType(PartEnum)).Length

    Public Structure PartItem

    End Structure

    Private mCols(PartCount - 1) As Integer
    Private mRows(PartCount - 1) As Integer
    Private mParts(PartCount - 1)() As Label
    Private mFillColor As Color
    Private mEmptyColor As Color
    Private mClickMode As ClickMode

    Public Event PartUpdated(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Public Sub New()
        Dim i As Integer

        'first thing first
        Me.InitializeComponent()

        'we are going to fix this to match the hardware
        mCols(PartEnum.Lens) = 6
        mRows(PartEnum.Lens) = 8
        mCols(PartEnum.BS1) = 6
        mRows(PartEnum.BS1) = 2
        mCols(PartEnum.BS2) = 6
        mRows(PartEnum.BS2) = 2
        mCols(PartEnum.PBS) = 6
        mRows(PartEnum.PBS) = 2

        For i = 0 To PartCount - 1
            ReDim mParts(i)(mRows(i) * mCols(i) - 1)
        Next

        mFillColor = btnFillAll.BackColor
        mEmptyColor = btnEmptyAll.BackColor
        mClickMode = ClickMode.None

        Me.BuildTray()
    End Sub

#Region "public function"
    Public Function GetFirstFilledPartIndex(ByVal part As PartEnum) As Integer
        Dim i As Integer
        For i = 0 To mParts(part).Length - 1
            If mParts(part)(i).BackColor = mFillColor Then
                Return CType(mParts(part)(i).Tag, Integer)
            End If
        Next
        Return -1
    End Function

    Public Sub EmptyOnePart(ByVal Index As Integer)
        Dim i, k, idx As Integer

        'we will loop through all the parts
        For i = 0 To PartCount - 1
            For k = 0 To mParts(i).Length - 1
                idx = CType(mParts(i)(k).Tag, Integer)
                If Index = idx Then
                    mParts(i)(k).BackColor = mEmptyColor
                    Return
                End If
            Next
        Next
    End Sub

    Public Sub FillParts(ByVal part As PartEnum, ByVal StartIndex As Integer)
        Me.SetPart(part, True, StartIndex - 1, mParts(part).Length - 1)
    End Sub
#End Region

#Region "local function"
    Private Function PartEnumToPartLabel(ByVal Index As Integer) As String
        'Dim s As String
        's = CType(Index, PartEnum).ToString()
        's = s.Substring(0, 1)
        'Return s
        Select Case Index
            Case PartEnum.Lens : Return "L"
            Case PartEnum.PBS : Return "P"
            Case PartEnum.BS1 : Return "A"
            Case PartEnum.BS2 : Return "B"
            Case Else
                Return ""
        End Select
    End Function

    Private Function PartLabelToPartEnum(ByVal Label As String) As Integer
        Label = Label.Substring(0, 1)
        Select Case Label
            Case "L" : Return PartEnum.Lens
            Case "A" : Return PartEnum.BS1
            Case "B" : Return PartEnum.BS2
            Case "P" : Return PartEnum.PBS
        End Select
        Return -1
    End Function

    Private Function PartLabelToPartIndex(ByVal Label As String) As Integer
        Label = Label.Substring(1)
        Return Convert.ToInt32(Label)
    End Function

    Private Sub BuildTray()
        Dim sPart As String
        Dim i, r, c, idxT, idx(PartCount - 1) As Integer
        Dim left, top, leftAlign As Integer
        Dim lbl As Label
        Dim ctrl As List(Of Control)

        For i = 0 To PartCount - 1
            idx(i) = 0
        Next
        idxT = 0

        top = Gap

        'lens on the left, move from top to bottom
        sPart = Me.PartEnumToPartLabel(0)

        For r = 1 To mRows(0)
            'the part is arranged by col, so left keep moving
            left = Gap

            For c = 1 To mCols(0)
                idxT += 1
                idx(0) += 1

                lbl = New Label()
                lbl.Parent = panelTray
                lbl.Size = New Size(PartWidth, PartHeight(0))
                lbl.Location = New Point(left, top)
                lbl.BackColor = mEmptyColor
                lbl.ForeColor = Color.DarkGray

                lbl.TextAlign = ContentAlignment.MiddleCenter
                lbl.Font = New Font("Courier New", 10, FontStyle.Bold)

                lbl.Text = sPart + idx(0).ToString()
                lbl.Visible = True

                lbl.Tag = idxT
                mParts(0)(idx(0) - 1) = lbl
                AddHandler lbl.Click, AddressOf ctrl_Click

                'update position
                left += (PartWidth + Gap)
            Next c
            top += (PartHeight(0) + Gap)
        Next r


        'now we do BS1, BS2, and PBS
        top = Gap
        leftAlign = left
        For i = 1 To PartCount - 1
            'part
            sPart = Me.PartEnumToPartLabel(i)
            'part are now arranged by rows
            For r = 1 To mRows(i)
                'reset left
                left = leftAlign
                'do all columns
                For c = 1 To mCols(i)
                    idxT += 1
                    idx(i) += 1

                    lbl = New Label()
                    lbl.Parent = panelTray
                    lbl.Size = New Size(PartWidth, PartHeight(i))
                    lbl.Location = New Point(left, top)
                    lbl.BackColor = mEmptyColor
                    lbl.ForeColor = Color.DarkGray

                    lbl.TextAlign = ContentAlignment.MiddleCenter
                    lbl.Font = New Font("Courier New", 10, FontStyle.Bold)

                    lbl.Text = sPart + idx(i).ToString()
                    lbl.Visible = True

                    lbl.Tag = idxT
                    mParts(i)(idx(i) - 1) = lbl
                    AddHandler lbl.Click, AddressOf ctrl_Click

                    left += (PartWidth + Gap)
                Next c

                top += (PartHeight(i) + Gap)
            Next r
        Next

        'tray size
        panelTray.Width = left
        panelTray.Height = top
        panelTray.Location = New Point(0, Gap)

        'buttons
        ctrl = New List(Of Control)
        left = panelTray.Left + panelTray.Width + Gap
        w2.w2Misc.GetAllControls(Of Button)(Me, ctrl)
        For Each btn As Button In ctrl
            btn.Left = left
            AddHandler btn.Click, AddressOf ctrl_Click
        Next

        'form size
        Me.Height = (Me.Height - Me.ClientRectangle.Height) + 2 * Gap + panelTray.Height
        Me.Width = (Me.Width - Me.ClientRectangle.Width) + btnFillAll.Left + btnFillAll.Width + Gap
        Me.KeyPreview = True

    End Sub

    Private Sub SetPart(ByVal Part As PartEnum, ByVal DoFill As Boolean, ByVal FirstIndex As Integer, ByVal LastIndex As Integer)
        Dim i As Integer
        Dim color As Color

        If DoFill Then
            color = mFillColor
        Else
            color = mEmptyColor
        End If

        For i = FirstIndex To LastIndex
            mParts(Part)(i).BackColor = color
        Next

        RaiseEvent PartUpdated(Me, New System.EventArgs())
    End Sub

    Private Sub ctrl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ctrl As Control
        Dim idx, i As Integer

        ctrl = CType(sender, Control)
        Select Case ctrl.Name
            Case btnFillAll.Name
                Me.Cursor = Cursors.Default
                For i = 0 To PartCount - 1
                    Me.SetPart(CType(i, PartEnum), True, 0, mParts(i).Length - 1)
                Next

            Case btnEmptyAll.Name
                Me.Cursor = Cursors.Default
                For i = 0 To PartCount - 1
                    Me.SetPart(CType(i, PartEnum), False, 0, mParts(i).Length - 1)
                Next

            Case btnFillOne.Name
                mClickMode = ClickMode.FillOne
                Me.Cursor = Cursors.Cross

            Case btnFillRest.Name
                mClickMode = ClickMode.FillRest
                Me.Cursor = Cursors.Cross

            Case btnEmptyOne.Name
                mClickMode = ClickMode.EmptyOne
                Me.Cursor = Cursors.Cross
        End Select

        If TypeOf ctrl Is Label Then
            'get index, note that label is 1 based and internal is 0 based
            idx = Me.PartLabelToPartIndex(ctrl.Text)
            idx -= 1
            'get part
            i = Me.PartLabelToPartEnum(ctrl.Text)
            'do work
            Select Case mClickMode
                Case ClickMode.EmptyOne
                    Me.SetPart(CType(i, PartEnum), False, idx, idx)
                Case ClickMode.FillOne
                    Me.SetPart(CType(i, PartEnum), True, idx, idx)
                Case ClickMode.FillRest
                    Me.SetPart(CType(i, PartEnum), True, idx, mParts(i).Length - 1)
            End Select
        End If

    End Sub

    Private Sub fPartTray_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            mClickMode = ClickMode.None
            Me.Cursor = Cursors.Default
        End If
    End Sub
#End Region
End Class