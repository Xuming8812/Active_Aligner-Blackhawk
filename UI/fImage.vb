Imports System.IO

Public Class fImage

    Public Sub New(ByVal MdiParent As Form)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TopLevel = False
        Me.Parent = MdiParent

    End Sub

    Public Sub SetImage(ByVal folder As String)
        Dim newFile As FileInfo
        Dim DirInfo As DirectoryInfo = New DirectoryInfo(folder)
        Dim files() As FileInfo = DirInfo.GetFiles()
        Dim time As DateTime = New DateTime(1900, 1, 1)
        For Each f As FileInfo In files
            If f.LastWriteTime > time Then
                time = f.LastWriteTime
                newFile = f
            End If
        Next

        If newFile IsNot Nothing Then
            Me.pbImage.ImageLocation = newFile.FullName
            Me.pbImage.SizeMode = PictureBoxSizeMode.Zoom
            Me.Text = newFile.Name
        End If

        Me.EnsureVisible()
    End Sub

    Public Sub EnsureVisible()
        Me.Location = New Point(Me.Parent.Right - Me.Width - 20, Me.Parent.Top + 50)

        Me.Visible = True
        If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
        'Me.BringToFront()
        Me.Refresh()
    End Sub

    Private Sub tBtnViewFile_Click(sender As Object, e As EventArgs) Handles tBtnViewFile.Click
        Dim fileOpenDlg As OpenFileDialog = New OpenFileDialog()
        Dim dlgResult As DialogResult
        Dim filePath As String

        dlgResult = fileOpenDlg.ShowDialog()
        If dlgResult = Windows.Forms.DialogResult.OK Then
            filePath = fileOpenDlg.FileName
            Me.pbImage.ImageLocation = filePath
            Me.Text = fileOpenDlg.SafeFileName
            Me.pbImage.SizeMode = PictureBoxSizeMode.Zoom
            Me.EnsureVisible()
        End If

    End Sub


    Private Sub fImage_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
        e.Cancel = True
    End Sub
End Class