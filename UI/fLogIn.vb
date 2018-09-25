Imports System.IO

Public Class fLogIn

    Public Sub New(ByVal MdiParent As Form, ByVal IsPass As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If IsPass Then
            Me.pbImage.ImageLocation = Application.StartupPath + "\Pass.png"
        Else
            Me.pbImage.ImageLocation = Application.StartupPath + "\Fail.png"
        End If

    End Sub


End Class