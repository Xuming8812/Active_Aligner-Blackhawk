Imports System.IO
Imports System.Text
Imports System.Windows.Forms

Public Class w2CsvHelper
    Private sw As StreamWriter

    Public Sub New(ByVal sFileName As String, ByVal columns As DataGridViewColumnCollection)
        Try
            sw = New StreamWriter(New FileStream(sFileName, FileMode.Create), Encoding.GetEncoding("GB2312"))
            Dim strColu As StringBuilder = New StringBuilder()

            For i As Integer = 0 To columns.Count - 1
                strColu.Append(columns(i).HeaderText)
                strColu.Append(",")
            Next
            strColu.Remove(strColu.Length - 1, 1)
            sw.WriteLine(strColu)
            sw.Flush()
        Catch ex As Exception

        End Try

    End Sub

    Public Sub New(ByVal sFileName As String, ByVal columns() As String)

        If File.Exists(sFileName) Then
            sw = New StreamWriter(New FileStream(sFileName, FileMode.Append), Encoding.GetEncoding("GB2312"))
        Else
            Try
                sw = New StreamWriter(New FileStream(sFileName, FileMode.Create), Encoding.GetEncoding("GB2312"))
                Dim strColu As StringBuilder = New StringBuilder()

                For i As Integer = 0 To columns.Length - 1
                    strColu.Append(columns(i))
                    strColu.Append(",")
                Next
                strColu.Remove(strColu.Length - 1, 1)
                sw.WriteLine(strColu)
                sw.Flush()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub AppendLine(ByVal time As DateTime, ByVal values() As Double)
        Dim strValue As StringBuilder = New StringBuilder()

        strValue.Append(time)
        strValue.Append(",")
        For i As Integer = 0 To values.Length - 1
            strValue.Append(values(i))
            strValue.Append(",")
        Next
        strValue.Remove(strValue.Length - 1, 1)
        sw.WriteLine(strValue)
        sw.Flush()
    End Sub

    Public Sub AppendLine(ByVal ParamArray values() As Object)
        Dim strValue As StringBuilder = New StringBuilder()

        For i As Integer = 0 To values.Length - 1
            strValue.Append(values(i))
            strValue.Append(",")
        Next
        strValue.Remove(strValue.Length - 1, 1)
        sw.WriteLine(strValue)
        sw.Flush()
    End Sub

    Public Sub Close()
        sw.Close()
    End Sub
End Class
