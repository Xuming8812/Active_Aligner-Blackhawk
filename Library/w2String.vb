Option Explicit On
Option Strict On

Imports System.Text.RegularExpressions
Public Class w2String

#Region "Concatenate"
    Public Shared Function Concatenate(ByVal Delimiter As String, ByVal ParamArray Parts As Object()) As String
        Dim x, y As Object
        Dim aArray As Array
        Dim s As String = ""
        For Each x In Parts
            If TypeOf x Is Array Then
                aArray = CType(x, Array)
                For Each y In aArray
                    If TypeOf y Is Double Then y = CType(y, Single)
                    s += (Delimiter + y.ToString)
                Next y
            Else
                If TypeOf x Is Double Then x = CType(x, Single)
                s += (Delimiter + x.ToString)
            End If
        Next x
        If s.StartsWith(Delimiter) Then s = s.Substring(Delimiter.Length)
        Return s
    End Function

    Public Shared Function Concatenate(ByVal Delimiter As String, ByVal fmt As String, ByVal Data As Double()) As String
        Dim v As Double
        Dim s As String = ""

        For Each v In Data
            If fmt.StartsWith("{") Then
                s += Delimiter + String.Format(fmt, v)
            Else
                s += Delimiter + v.ToString(fmt)
            End If
        Next
        If s.StartsWith(Delimiter) Then s = s.Substring(Delimiter.Length)
        Return s
    End Function
#End Region

#Region "regular expression"
    Public Class RegexPatterns
        Public Const DecimalNumber As String = "[+-]?(?:\d+\.?\d*|\d*\.?\d+)"
        Public Const SpaceDelimitedValues As String = "(?:(?("")"".+""|\S+))"
        Public Const UnsignedIntegerNumber As String = "(?:\d+)"
        Public Const EndingDigits As String = "(?:\d+)$"
    End Class

    Public Shared Function ExtractMatches(ByVal s As String, ByVal Pattern As String) As String()
        Dim L As New List(Of String)
        Dim r As New Regex(Pattern)
        Dim m As Match

        m = r.Match(s)
        While m.Success
            L.Add(m.Value)
            m = m.NextMatch()
        End While

        Return L.ToArray()
    End Function

    Public Shared Function ExtractDecimalNumbers(ByVal s As String) As String()
        Return ExtractMatches(s, RegexPatterns.DecimalNumber)
    End Function

    Public Shared Function ExtractDecimalNumberFirst(ByVal s As String) As String
        Dim x() As String = ExtractDecimalNumbers(s)
        If x.Length = 0 Then
            Return ""
        Else
            Return x(0)
        End If
    End Function

    Public Shared Function ExtractDecimalNumberLast(ByVal s As String) As String
        Dim x() As String = ExtractDecimalNumbers(s)
        If x.Length = 0 Then
            Return ""
        Else
            Return x(x.Length - 1)
        End If
    End Function

    Public Shared Function ExtractDecimalNumbersAsDouble(ByVal s As String) As Double()
        Dim L As New List(Of Double)
        Dim v As Double
        Dim r As New Regex(RegexPatterns.DecimalNumber)
        Dim m As Match

        m = r.Match(s)
        While m.Success
            If Double.TryParse(m.Value, v) Then L.Add(v)
            m = m.NextMatch()
        End While

        Return L.ToArray()
    End Function

    Public Shared Function ExtractIntegerNumbers(ByVal s As String) As String()
        Return ExtractMatches(s, RegexPatterns.UnsignedIntegerNumber)
    End Function

    Public Shared Function ExtractIntegerNumberFirst(ByVal s As String) As String
        Dim x() As String = ExtractIntegerNumbers(s)
        If x.Length = 0 Then
            Return ""
        Else
            Return x(0)
        End If
    End Function

    Public Shared Function ExtractIntegerNumberLast(ByVal s As String) As String
        Dim x() As String = ExtractIntegerNumbers(s)
        If x.Length = 0 Then
            Return ""
        Else
            Return x(x.Length - 1)
        End If
    End Function

    Public Shared Function ExtractIntegerNumbersAsDouble(ByVal s As String) As Integer()
        Dim L As New List(Of Integer)
        Dim v As Integer
        Dim r As New Regex(RegexPatterns.UnsignedIntegerNumber)
        Dim m As Match

        m = r.Match(s)
        While m.Success
            If Integer.TryParse(m.Value, v) Then L.Add(v)
            m = m.NextMatch()
        End While

        Return L.ToArray()
    End Function

    Public Shared Function GetCommandValuePair(ByVal s As String, ByRef Command As String, ByRef Value As Double) As Boolean
        Dim L As New List(Of String)
        Dim r As Regex
        Dim m As Match

        'try both cmd and value
        r = New Regex("^(?<cmd>[a-zA-Z]+)\s+(?<value>[+-]?(?:\d+\.?\d*|\d*\.?\d+))")
        m = r.Match(s)
        If m.Success Then
            Command = m.Groups("cmd").Value
            Value = Double.Parse(m.Groups("value").Value)
            Return True
        End If

        'cmd only
        r = New Regex("^(?<cmd>[a-zA-Z]+)\s*")
        m = r.Match(s)
        If m.Success Then
            Command = m.Groups("cmd").Value
            Value = Double.NaN
            Return True
        End If

        'false
        Return False

    End Function

    Public Shared Function SplitSpaceDelimitedValues(ByVal s As String) As String()
        Return ExtractMatches(s, RegexPatterns.SpaceDelimitedValues)
    End Function

    Public Shared Function ExtractLeadingAlphaCharacters(ByVal s As String) As String
        Dim r As New System.Text.RegularExpressions.Regex("^(?:[a-zA-Z]+)")
        Dim m As System.Text.RegularExpressions.Match
        m = r.Match(s)
        If m.Success Then
            Return m.Value
        Else
            Return s
        End If
    End Function

    Public Shared Function ExtractEndingDigits(ByVal s As String) As Integer
        Dim v() As String = ExtractMatches(s, RegexPatterns.EndingDigits)
        If v.Length > 0 Then
            Return Integer.Parse(v(0))
        Else
            Return -1
        End If
    End Function

    Private Function GetQueryString(ByVal sTemplate As String, ByVal ParamArray sValue() As String) As String
        Dim i As Integer
        Dim r As Regex
        Dim m As Match

        'add quotations to avoid reserved characters used in product name, such as "-" and etc
        For i = 0 To sValue.Length - 1
            If IsNumeric(sValue(i)) Then Continue For
            If (Not sValue(i).StartsWith("'")) Then sValue(i) = "'" & sValue(i) & "'"
        Next

        r = New Regex("(?:\@[^@]+\@)")
        'r = New Regex("\@[^@]+\@")
        m = r.Match(sTemplate)
        i = 0
        While m.Success And (i < sValue.Length)
            sTemplate = sTemplate.Replace(m.Value, sValue(i))
            m = m.NextMatch()
            i += 1
        End While

        Return sTemplate
    End Function
#End Region

    Public Shared Function AddSpaceBetweenWords(ByVal s As String) As String
        Dim i, ii As Integer
        Dim C, NextC As String

        ii = s.Length - 2
        C = s.Substring(i + 1, 1)

        'do this backword because we are keep inserting spaces to the string
        For i = ii To 1 Step -1
            NextC = C
            C = s.Substring(i, 1)
            'from Up to Lo
            If C.ToUpper = C And NextC.ToLower = NextC Then s = s.Insert(i, " ")
            'from Lo to Up
            If C.ToLower = C And NextC.ToUpper = NextC Then s = s.Insert(i + 1, " ")
        Next

        'remove the double sapce
        s = s.Replace("  ", " ")

        'back
        Return s
    End Function

#Region "split data by comma, space, or tab"
    Public Shared Function SplitDataAsString(ByVal sData As String) As String()
        Return SplitDataAsString(sData, True)
    End Function

    Private Shared Function SplitDataAsString(ByVal sData As String, ByVal bRemoveDuplicatedDelimiter As Boolean) As String()
        Dim Delimiter As Char

        If sData.Contains(ControlChars.Tab) Then
            Delimiter = ControlChars.Tab
        ElseIf sData.Contains(","c) Then
            Delimiter = ","c
        Else
            Delimiter = " "c
        End If

        Dim ss() As String = sData.Split(Delimiter)
        If bRemoveDuplicatedDelimiter Then
            Dim x As New List(Of String)
            For Each s As String In ss
                If s <> "" Then x.Add(s)
            Next
            ss = x.ToArray()
        End If

        Return ss
    End Function

    Public Shared Function SplitDataAsDouble(ByVal sData As String) As Double()
        Return SplitDataAsDouble(sData, True)
    End Function

    Private Shared Function SplitDataAsDouble(ByVal sData As String, ByVal bRemoveDuplicatedDelimiter As Boolean) As Double()
        Dim v() As Double
        Dim s() As String = SplitDataAsString(sData, bRemoveDuplicatedDelimiter)
        Try
            v = Array.ConvertAll(s, New System.Converter(Of String, Double)(AddressOf Convert.ToDouble))
        Catch ex As Exception
            ReDim v(s.Length - 1)
            For i As Integer = 0 To s.Length - 1
                v(i) = Val(s(i))
            Next
        End Try

        Return v
    End Function
#End Region
End Class
