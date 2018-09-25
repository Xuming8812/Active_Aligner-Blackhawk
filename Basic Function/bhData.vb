Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class BlackHawkData
    Public Enum DataFileEnum
        Log
        Result
    End Enum

#Region "internal constant and utility"
    Private Text As String = "BlackHawk Data"
    Private Const SectionFile As String = "Files"
    Private Const SectionDatabase As String = "Database"

    Private mIniFile As w2.w2IniFile
#End Region

    Public Function Initialize(ByVal sIniFile As String) As Boolean
        Dim success As Boolean

        If IO.Path.GetDirectoryName(sIniFile) = "" Then
            sIniFile = IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, sIniFile)
        End If
        If Not IO.File.Exists(sIniFile) Then
            sIniFile = "Cannot find the configuration file " & ControlChars.CrLf & sIniFile
            MessageBox.Show(sIniFile, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        mIniFile = New w2.w2IniFile(sIniFile)
    
        success = Me.ConnectToTestDatabase()
        
        Return success
    End Function

    Public ReadOnly Property IniFile() As w2.w2IniFile
        Get
            Return mIniFile
        End Get
    End Property

#Region "Error log"
    Public Sub ExtractErrorToSingleFile(ByVal SN As String, ByVal Time As Date)
        Dim s, sErrorLog, sDataLog As String
        Dim r As IO.StreamReader
        Dim w As IO.StreamWriter

        'get the last log file
        s = Me.GetDataFile(SN, DataFileEnum.Log, False, Time)
        'only file with suffix is static, we will try to get the last one
        sDataLog = Me.GetBackupFiles(s)(1)
        r = New IO.StreamReader(sDataLog)

        'we will append data only
        sErrorLog = mIniFile.ReadParameter("Files", "ErrorSummary", "C:\Data\ErrorLog.txt")
        w = New IO.StreamWriter(sErrorLog, True)

        s = ""
        While r.Peek() > 0
            s = r.ReadLine().Trim()
            If s.StartsWith("X   ") Or s.StartsWith("!   ") Then
                w.WriteLine(w2String.Concatenate(vbTab, Date.Now.ToString("MM/dd/yyyy HH:mm:ss"), SN, sDataLog, s))
            End If
        End While

        'close files
        r.Close()
        w.Close()
    End Sub
#End Region

#Region "data path and file"
    Public Function BackupDataFile(ByVal sFile As String) As String
        Dim s As String
        Dim i As Integer = 0
        Dim sExt As String = IO.Path.GetExtension(sFile)
        Dim sRoot As String = IO.Path.GetFileNameWithoutExtension(sFile)

        sRoot = IO.Path.Combine(IO.Path.GetDirectoryName(sFile), sRoot)

        s = sFile
        While IO.File.Exists(s)
            s = sRoot + "-" + i.ToString() + sExt
            i += 1
        End While

        IO.File.Copy(sFile, s, False)

        Return s
    End Function

    Public Function GetBackupFiles(ByVal sFile As String) As String()
        Dim sFiles() As String
        Dim index() As Integer
        Dim i, ii As Integer
        Dim k, j As Integer
        Dim s, sPath As String

        s = sFile
        sFile = IO.Path.GetFileName(s)
        sPath = IO.Path.GetFullPath(s)
        sPath = sPath.Replace(sFile, "")
        sFile = sFile.Replace(".txt", "*.txt")

        sFiles = IO.Directory.GetFiles(sPath, sFile)

        'sort files by index descending
        ii = sFiles.Length - 1
        ReDim index(ii)
        For i = 0 To ii
            k = sFiles(i).LastIndexOf("-")
            j = sFiles(i).LastIndexOf(".txt")
            s = sFiles(i).Substring(k + 1, (j - k - 1))
            'index(i) = Convert.ToInt32(s)
            If Not Integer.TryParse(s, index(i)) Then
                index(i) = 999
            End If
        Next

        Array.Sort(index, sFiles)
        Array.Reverse(sFiles)

        Return sFiles
    End Function

    Public ReadOnly Property RootDataPath() As String
        Get
            Return mIniFile.ReadParameter(SectionFile, "RootPath", "C:\Data\")
        End Get
    End Property

    Public Function GetDataPath(ByVal SerialNumber As String, ByVal CreatePath As Boolean, ByVal Time As Date) As String
        Dim sPath As String
        Dim sLetter As String
        Dim s As String
        Dim i As Integer
        Dim Number As Integer

        'root path
        sPath = Me.RootDataPath
        If Not IO.Directory.Exists(sPath) Then
            If CreatePath Then
                IO.Directory.CreateDirectory(sPath)
            Else
                Return ""
            End If
        End If

        'parse serial number
        SerialNumber = SerialNumber.ToUpper()

        'If IsNumeric(SerialNumber) Then
        '    sPath = IO.Path.Combine(sPath, SerialNumber)
        '    Return sPath
        'End If

        If IsNumeric(SerialNumber) Then
            sLetter = ""
            s = SerialNumber
        Else
            sLetter = w2String.ExtractLeadingAlphaCharacters(SerialNumber)
            s = SerialNumber.Replace(sLetter, "")
        End If

        'get path for the numeric part of the serial number
        Try
            Number = Integer.Parse(s)

            i = 1000 * (Number \ 1000)
            s = sLetter & i & "-" & sLetter & (i + 999)
            sPath = IO.Path.Combine(sPath, s)
            If Not IO.Directory.Exists(sPath) Then
                If CreatePath Then IO.Directory.CreateDirectory(sPath)
            End If

            i = 100 * (Number \ 100)
            s = sLetter & i & "-" & sLetter & (i + 99)
            sPath = IO.Path.Combine(sPath, s)

            If Not IO.Directory.Exists(sPath) Then
                If CreatePath Then IO.Directory.CreateDirectory(sPath)
            End If

            sPath = IO.Path.Combine(sPath, SerialNumber)
            sPath += "_" + Time.ToString("yyyyMMddHH")
            If Not IO.Directory.Exists(sPath) Then
                If CreatePath Then IO.Directory.CreateDirectory(sPath)
            End If

        Catch ex As Exception
            If Not CreatePath Then
                s = "Failed to parse the serial number into file path structures " & SerialNumber
                s = s & ControlChars.CrLf & "Numeric number expected for the serial number after the letters."
                MessageBox.Show(s, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return ""
            End If
        End Try

        'return
        Return sPath
    End Function

    Public Function GetDataFile(ByVal SerialNumber As String, ByVal FileType As DataFileEnum, ByVal Time As Date) As String
        Return GetDataFile(SerialNumber, FileType, False, Time)
    End Function

    Public Function GetDataFile(ByVal SerialNumber As String, ByVal FileType As DataFileEnum, ByVal CreateFile As Boolean, ByVal Time As Date) As String
        Dim s As String
        Dim sPath As String
        Dim sFileName, sFile As String

        'get data path
        sPath = Me.GetDataPath(SerialNumber, CreateFile, Time)
        If sPath = "" Then Return ""

        'add space to the file name
        sFileName = [Enum].GetName(GetType(DataFileEnum), FileType)
        sFileName = w2String.AddSpaceBetweenWords(sFileName)
        sFileName += ".txt"

        'add serial number to the file name
        sFileName = SerialNumber & " " & sFileName

        'build full file name
        sFile = IO.Path.Combine(sPath, sFileName)

        'validate file
        If Not IO.File.Exists(sFile) Then
            If Not CreateFile Then
                s = "Cannot find the default data file " & sFile
                MessageBox.Show(s, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                sFile = ""
            End If
        End If

        'return 
        Return sFile
    End Function

    Public Function InsertTextToFileName(ByVal FileName As String, ByVal Text As String) As String
        Dim s As String

        'add space to the inserted text
        If Not Text.StartsWith(" ") Then Text = " " + Text

        'get file extension
        s = IO.Path.GetExtension(FileName)

        'insert text
        If s = "" Then
            FileName += Text
        Else
            Text += s
            FileName = FileName.Replace(s, Text)
        End If

        Return FileName
    End Function
#End Region

#Region "database"
    Public Enum dbCommandEnum
        ProcessData
    End Enum

    Private mDataConnection As SqlConnection

    Public Function GetDataAdapter(ByVal SerialNumber As String, ByVal DataType As dbCommandEnum, ByVal RaiseError As Boolean) As SqlDataAdapter
        Dim s As String
        Dim sQuery As String
        Dim dbConnection As SqlConnection

        sQuery = GetQueryCommand(DataType, SerialNumber)
        If sQuery = "" Then Return Nothing

        'which database to connect?
        If Not ConnectToTestDatabase() Then Return Nothing
        dbConnection = mDataConnection

        'open
        Try
            'open connection, get adpater
            Dim DataCommand As SqlCommand = New SqlCommand(sQuery, dbConnection)
            Dim DataAdapter As SqlDataAdapter = New SqlDataAdapter(DataCommand)
            'build insert command
            Dim CmdBuilder As New SqlCommandBuilder(DataAdapter)
            DataAdapter.InsertCommand = CmdBuilder.GetInsertCommand()
            'get data table
            Return DataAdapter
        Catch ex As Exception
            If RaiseError Then
                s = "Database error " & vbCrLf & ex.Message
                MessageBox.Show(s)
            End If
            Return Nothing
        End Try
    End Function


    Private Function ConnectToTestDatabase() As Boolean
        'fine if it is already open
        If mDataConnection IsNot Nothing Then Return True

        'get connection string
        Dim s As String = ""
        s = mIniFile.ReadParameter(SectionDatabase, "Connection", "")
        If s = "" Then
            's = "Cannot found the connection string in the configuration file for database."
            'MessageBox.Show(s, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return True
        End If

        'OK, try a new connection
        mDataConnection = New SqlConnection(s)
        Try
            mDataConnection.Open()
            Return True
        Catch ex As Exception

            s = "Database error " & ControlChars.CrLf & ex.Message
            MessageBox.Show(s)
            mDataConnection = Nothing
            Return False
        End Try
    End Function

    Private Function GetQueryCommand(ByVal CommandType As dbCommandEnum, ByVal ParamArray sParameter() As String) As String
        Dim s As String
        Dim sKey As String

        'get key
        sKey = "Cmd" + [Enum].GetName(GetType(dbCommandEnum), CommandType)

        'get SQL
        s = mIniFile.ReadParameter(SectionDatabase, sKey, "")
        If s = "" Then Return ""

        If sParameter.Length = 0 Then
            Return s
        Else
            Return GetQueryString(s, sParameter)
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

    'Private Function GetQueryString(ByVal sTemplate As String, ByVal ParamArray sValue() As String) As String
    '    Dim i As Integer
    '    Dim r As Regex
    '    Dim m As Match

    '    'add quotations to avoid reserved characters used in product name, such as "-" and etc
    '    For i = 0 To sValue.Length - 1
    '        If (Not sValue(i).StartsWith("'")) Then sValue(i) = "'" & sValue(i) & "'"
    '    Next

    '    r = New Regex("(?:\[[^[]+\])")
    '    m = r.Match(sTemplate)
    '    i = 0
    '    While m.Success And (i < sValue.Length)
    '        sTemplate = sTemplate.Replace(m.Value, sValue(i))
    '        i += 1
    '    End While

    '    Return sTemplate
    'End Function

#End Region
End Class

