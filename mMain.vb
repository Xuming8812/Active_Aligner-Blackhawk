Option Explicit On
Option Strict On

Module mMain
    Public Sub Main(ByVal sArgs() As String)
        Const Title As String = "BlackHawk"
        Dim s, sFile As String

        'config
        sFile = Title + ".ini"
        sFile = IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, sFile)
        If Not IO.File.Exists(sFile) Then
            s = "Cannot found the application configuration file " + ControlChars.Cr + sFile
            s += ControlChars.Cr + "Application will be terminated."
            MessageBox.Show(s, Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        'login 
        Dim login As New w2.w2Login
        ' If Not login.Initialize(sFile) Then Return

        'special for debug
        ' If Not login.Login() Then Return

        'main form
        Dim f As New fMain
        If f.Initialize(sFile, login) Then
            'Application.EnableVisualStyles()
            'Application.DoEvents()
            Application.Run(f)
        Else
            s = "Failed to initialize the application. Application will be terminated."
            MessageBox.Show(s, Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            f.Close()
            Return
        End If

    End Sub

    'Private Sub Tester()
    '    Dim i As Integer
    '    Dim Data(4) As Byte
    '    Data(0) = &H20
    '    Data(1) = &H3
    '    Data(2) = 0
    '    Data(3) = 0

    '    ' Array.Reverse(Data)
    '    i = BitConverter.ToInt32(Data, 0)
    '    i = Data(3)
    '    i = i << 8
    '    i += Data(2)
    '    i = i << 8
    '    i += Data(1)
    '    i = i << 8
    '    i += Data(0)


    'End Sub
End Module
