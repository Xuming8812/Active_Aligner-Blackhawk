Option Explicit On
Option Strict On
Option Infer Off

Public Class XpsStage

#Region "enum and structure"
    Public Structure Position2D
        Public Sub New(ByVal X0 As Double, ByVal Y0 As Double)
            X = X0
            Y = Y0
        End Sub
        Public X As Double
        Public Y As Double
    End Structure

    Public Structure Position3D
        Public Sub New(ByVal X0 As Double, ByVal Y0 As Double, ByVal Z0 As Double)
            X = X0
            Y = Y0
            Z = Z0
        End Sub
        Public X As Double
        Public Y As Double
        Public Z As Double
    End Structure

    Public Enum AxisNameEnum
        '<StageInfo>
        'Name	Axis	Home	LimitLo	LimitHi	Velocity
        'StageX	1	0	-50.0	50.0	30.0
        'StageY	3	0	-68	80.0	10.0
        'StageZ	2	0	-25.0	25.0	10.0
        'OutX	4	0	-50.0	50.0	0.4
        'OutY	5	0	-90	90	0.4
        'OutZ	6	0	-12	0	0.4
        'BeamX	7	0	0	41	5
        'Angle	8	0	0	50.0	5.0
        '</StageInfo>

        'starte from zero, 
        'do not change name, it matches that in the configuration file
        'do not the order, they are the order by Configured Position Index
        StageX = 0
        StageY
        StageZ
        BeamScanX
        BeamScanY
        BeamScanZ
        'the following two are not on XPS system, but that is fine
        Angle
        Probe
    End Enum
    Public Shared AxisCount As Integer = [Enum].GetNames(GetType(AxisNameEnum)).Length

    Public Enum StagePositionEnum
        'do not move the order or the value of the following items, 
        'their values are used externally in scirpt
        LoadUnload = 0

        'misc
        FirstPickupLocation
        EpoxyApplication

        'alignment
        PbcAlign

        BsAlign1
        BsAlign2

        LensAlignCh1
        LensAlignCh2
        LensAlignCh3
        LensAlignCh4

        'beam scan
        BeamScanNear
        BeamScanMid
        BeamScanFar

        'stage safety
        YforSafeMove = 100
        ZforSafeMove
        ZforCheck
    End Enum

    Public Enum PartEnum
        PBC
        BS1
        BS2
        Lens1
        Lens2
        Lens3
        Lens4
    End Enum
    Public Shared PartCount As Integer = [Enum].GetNames(GetType(PartEnum)).Length
#End Region

#Region "utility class"
    Public Class StageInfo
        Public Name As String
        Public Axis As Integer
        Public Home As Double
        Public LimitLo As Double
        Public LimitHi As Double
        Public Velocity As Double
        Public Installed As Boolean

        Public Shared Function Parse(ByVal sConfig As String) As StageInfo
            Dim x As StageInfo
            Dim data() As String

            sConfig = sConfig.Trim
            data = sConfig.Split(ControlChars.Tab)

            Try
                x = New StageInfo
                x.Name = data(0)
                x.Axis = Convert.ToInt32(data(1))
                x.Home = Convert.ToDouble(data(2))
                x.LimitLo = Convert.ToDouble(data(3))
                x.LimitHi = Convert.ToDouble(data(4))
                x.Velocity = Convert.ToDouble(data(5))
                x.Installed = (data(6) = "1")
                Return x
            Catch ex As Exception
                Return Nothing
            End Try

        End Function
    End Class

    Public Class ConfiguredStagePosition
        Private mLabel As String
        Private mPositions() As Double

        Public Sub New(ByVal Label As String, ByVal Positions() As Double)
            mLabel = Label
            mPositions = Positions
        End Sub

        Public ReadOnly Property Label() As String
            Get
                Return mLabel
            End Get
        End Property

        Public ReadOnly Property Positions() As Double()
            Get
                Return mPositions
            End Get
        End Property

        Public ReadOnly Property TableString() As String
            Get
                Dim s As String
                Dim i As Integer

                s = mLabel
                For i = 0 To AxisCount - 1
                    s += ControlChars.Tab
                    If Double.IsNaN(mPositions(i)) Then
                        s += String.Format("{0,7:0.000}", "NA")
                    Else
                        s += String.Format("{0,7:0.000}", mPositions(i))
                    End If
                Next

                Return s
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return mLabel
        End Function

#Region "shared functions"
        Public Shared ReadOnly Property TableHeader() As String
            Get
                Dim s, header, names() As String
                names = [Enum].GetNames(GetType(AxisNameEnum))
                header = "Label"
                For Each s In names
                    header += ControlChars.Tab + s
                Next
                Return header
            End Get
        End Property

        Public Shared Function Parse(ByVal s As String) As ConfiguredStagePosition
            Dim sData() As String
            Dim i As Integer
            Dim v As Double
            Dim Positions(AxisCount - 1) As Double

            sData = s.Split(ControlChars.Tab)
            If sData.Length < (AxisCount + 1) Then Return Nothing

            For i = 0 To AxisCount - 1
                If Double.TryParse(sData(i + 1), v) Then
                    Positions(i) = v
                Else
                    Positions(i) = Double.NaN
                End If
            Next

            Return New ConfiguredStagePosition(sData(0).Trim(), Positions)
        End Function

#End Region

    End Class
#End Region

    Private mStageData(AxisCount - 1) As StageInfo
    Private mConfiguredPositions As Dictionary(Of String, ConfiguredStagePosition)

    Private mPartPositionInTray As Dictionary(Of Integer, Position2D)
    Private mXYSafetyWindow(PartCount - 1) As SafetyWindow

    Private mXPS As Instrument.iXPS
    Private mPara As w2.w2IniFileXML

#Region "public access"
    Public ReadOnly Property XPSController() As Instrument.iXPS
        Get
            Return mXPS
        End Get
    End Property

    Public Function Initialize(ByRef hXPS As Instrument.iXPS, ByRef hConfig As w2.w2IniFileXML) As Boolean
        Dim s, data() As String
        Dim index As Integer
        Dim v As Double
        Dim x As StageInfo

        mXPS = hXPS
        mPara = hConfig

        'get stage data
        s = mPara.ReadParameter("MotionTable", "StageInfo", "")
        s = s.Trim()
        data = s.Split(ControlChars.Cr)
        For Each s In data
            x = StageInfo.Parse(s)
            If x Is Nothing Then Continue For

            index = CType([Enum].Parse(GetType(AxisNameEnum), x.Name), Integer)
            mStageData(index) = x
        Next

        'change motor speed
        If mXPS IsNot Nothing Then
            For Each x In mStageData
                If Not x.Installed Then Continue For
                mXPS.Axis = x.Axis
                v = mXPS.VelocityMaximum
                If x.Velocity > v Then x.Velocity = v
                mXPS.Velocity = x.Velocity
            Next
        End If

        'known stage positions and their safety window
        If Not Me.ParseConfiguredPositions() Then Return False
        If Not Me.ParsePositionSafetyWindow() Then Return False

        'parts position in tray
        If Not Me.ParsePartPositionsInTray() Then Return False

        'validate
        For Each x In mStageData
            If x Is Nothing Then
                MessageBox.Show("Missing one or more stage configuration info!", "Motor Stage", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        Next

        Return True
    End Function

    Public Function GetStageName(ByVal axis As AxisNameEnum) As String
        Dim s As String

        Select Case axis
            Case AxisNameEnum.Angle
                s = "PI Angle Stage"
            Case AxisNameEnum.Probe
                s = "Probe Pin Stage"

            Case AxisNameEnum.BeamScanX
                s = "Beam Scan Stage X"
            Case AxisNameEnum.BeamScanY
                s = "Beam Scan Stage X"
            Case AxisNameEnum.BeamScanZ
                s = "Beam Scan Stage X"
           
            Case AxisNameEnum.StageX
                s = "Main Stage X"
            Case AxisNameEnum.StageY
                s = "Main Stage Y"
            Case AxisNameEnum.StageZ
                s = "Main Stage Z"

            Case Else
                s = "Unknown stage " & axis
        End Select

        Return s
    End Function

    Public Function GetStagePositionLabel(ByVal Position As StagePositionEnum) As String
        Dim label As String

        label = [Enum].GetName(GetType(StagePositionEnum), Position)
        label = w2String.AddSpaceBetweenWords(label)

        Return label
    End Function

    Public ReadOnly Property StageData() As StageInfo()
        Get
            Return mStageData
        End Get
    End Property
#End Region

#Region "move, position, velocity"
    Public Property ActiveAxis() As AxisNameEnum
        Get
            Dim i, ii As Integer
            ii = mStageData.Length - 1
            For i = 0 To ii
                If mStageData(i).Axis = mXPS.Axis Then
                    Return CType(i, AxisNameEnum)
                End If
            Next
            'this should not happen
            Return AxisNameEnum.StageZ
        End Get
        Set(ByVal value As AxisNameEnum)
            mXPS.Axis = mStageData(value).Axis
        End Set
    End Property

    Public ReadOnly Property HaveMotor(ByVal axis As AxisNameEnum) As Boolean
        Get
            Return mStageData(axis).Installed
        End Get
    End Property

    Public Function IsControllerReady() As Boolean
        Dim s As String
        Dim i, ii As Integer
        Dim iStatus As Integer

        ii = AxisCount - 1
        For i = 0 To ii
            'not installed, it is OK
            If Not mStageData(i).Installed Then Continue For
            'check missing stage by reading its status
            mXPS.Axis = mStageData(i).Axis
            iStatus = mXPS.StatusCode
            If mXPS.LastError <> "" Then
                s = "Missing controller/motor for " + Me.GetStageName(CType(i, AxisNameEnum))
                MessageBox.Show(s)
                Return False
            End If
            'try to enable driver
            If Not mXPS.DriveEnabled Then mXPS.DriveEnabled = True
            'check if it can be enabled
            If Not mXPS.StageReady Then
                s = "Stage " + Me.GetStageName(CType(i, AxisNameEnum)) + " is not initialized or homed"
                MessageBox.Show(s)
                Return False
            End If
        Next

        Return True
    End Function

    Public Function IsStageReady(ByVal axis As AxisNameEnum) As Boolean
        Dim ready As Boolean

        'not ready is no stage
        If Not mStageData(axis).Installed Then Return False

        mXPS.Axis = mStageData(axis).Axis
        If Not mXPS.DriveEnabled Then
            mXPS.DriveEnabled = True
        End If
        ready = mXPS.StageReady
        Return ready
    End Function

    Public Sub SetStageVelocity(ByVal Axis As AxisNameEnum, ByVal NewVelocity As Double)
        mXPS.Axis = mStageData(Axis).Axis
        mXPS.Velocity = NewVelocity
    End Sub

    Public Sub RecoverStageDefaultVelocity(ByVal Axis As AxisNameEnum)
        mXPS.Axis = mStageData(Axis).Axis
        mXPS.Velocity = mStageData(Axis).Velocity
    End Sub

    Public Function GetStagePosition(ByVal Axis As AxisNameEnum) As Double
        Dim v As Double

        mXPS.Axis = mStageData(Axis).Axis
        v = mXPS.CurrentPosition()
        Return v - mStageData(Axis).Home
    End Function

    Public Sub GetStageTravelLimit(ByVal Axis As AxisNameEnum, ByRef Min As Double, ByRef Max As Double)
        ''get hardware
        'Do
        '    mXPS.Axis = mStageData(Axis).Axis
        '    mXPS.GetTravelLimit(Min, Max)
        '    Min -= mStageData(Axis).Home
        '    Max -= mStageData(Axis).Home
        'Loop While (Min > Max)
        ''add the software limit
        'Min = Math.Max(Min, mStageData(Axis).LimitLo)
        'Max = Math.Min(Max, mStageData(Axis).LimitHi)
        'just use the software limit for now. We will make sure the software limit is inside the XPS travel limit
        'this reduce the time to read XPS, which seems to be inconsistent from time to time when reading is too fast
        Min = mStageData(Axis).LimitLo
        Max = mStageData(Axis).LimitHi
    End Sub

    Public Function MoveStageNoWait(ByVal Axis As AxisNameEnum, ByVal Selection As Instrument.iMotionController.MoveToTargetMethodEnum, ByVal Target As Double) As Boolean

        mXPS.Axis = mStageData(Axis).Axis
        If Selection = Instrument.iMotionController.MoveToTargetMethodEnum.Absolute Then
            'for absolute move, add software home offset to the hardware offset
            Target += mStageData(Axis).Home
        End If
        Return mXPS.MoveNoWait(Selection, Target)
    End Function

#End Region

#Region "Part position in tray"
    Private Function ParsePartPositionsInTray() As Boolean
        Dim s, lines(), data() As String
        Dim index As Integer
        Dim X, Y As Double

        'get storage
        mPartPositionInTray = New Dictionary(Of Integer, Position2D)

        'get table 
        s = mPara.ReadParameter("MotionTable", "PartTray", "")
        s = s.Trim()
        lines = s.Split(ControlChars.Cr)

        'parse table
        For Each s In lines
            s = s.Trim()
            'first line is table header
            If s.StartsWith("Index") Then Continue For
            If s = "" Then Continue For

            'parse row and column - we will ignore them this time 
            Select Case True
                Case s.StartsWith("Col")
                    Continue For
                Case s.StartsWith("Row")
                    Continue For
            End Select

            'parse info
            data = s.Split(ControlChars.Tab)
            Try
                index = Convert.ToInt32(data(0).Trim())
                X = Convert.ToDouble(data(1).Trim())
                Y = Convert.ToDouble(data(2).Trim())
            Catch ex As Exception
                s = "Error parsing part positions in the tray: " + ControlChars.CrLf + s
                s += ControlChars.Tab + "This entry will be ignored."
                MessageBox.Show(s, "Lens Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End Try

            mPartPositionInTray.Add(index, New Position2D(X, Y))
        Next

        Return True
    End Function

    Public Function GetPartPositionInTray(ByVal Index As Integer) As Position2D
        Dim position As Position2D

        If mPartPositionInTray.ContainsKey(Index) Then
            position = mPartPositionInTray(Index)
        Else
            position = New Position2D(Double.NaN, Double.NaN)
        End If

        Return position
    End Function
#End Region

#Region "configured positions"
    Public Function IsAlignPosition(ByVal Position As StagePositionEnum) As Boolean
        Dim s As String
        s = Position.ToString()
        Return s.EndsWith("Align")
    End Function

    Private Function ParseConfiguredPositions() As Boolean
        Dim s, data() As String
        Dim x As ConfiguredStagePosition

        'New
        mConfiguredPositions = New Dictionary(Of String, ConfiguredStagePosition)

        'get table
        s = mPara.ReadParameter("MotionTable", "ConfiguredPositions", "")
        s = s.Trim()
        data = s.Split(ControlChars.Cr)

        'parse table
        For Each s In data
            s = s.Trim()
            'first line is table header
            If s = ConfiguredStagePosition.TableHeader Then Continue For
            'parse info
            x = ConfiguredStagePosition.Parse(s)
            If x Is Nothing Then
                s = "Error parsing configuraed stage information: " + ControlChars.CrLf + s
                MessageBox.Show(s, "Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            mConfiguredPositions.Add(x.Label, x)
        Next

        Return True
    End Function

    Public Sub SaveConfiguredPositions()
        Dim table As String
        Dim x As ConfiguredStagePosition

        table = ControlChars.CrLf + ControlChars.Tab + ConfiguredStagePosition.TableHeader

        For Each x In mConfiguredPositions.Values
            table += ControlChars.CrLf + ControlChars.Tab + x.TableString
        Next
        table += ControlChars.CrLf + "    "

        mPara.WriteParameter("MotionTable", "ConfiguredPositions", table)
    End Sub

    Public ReadOnly Property HaveConfiguredPosition(ByVal Label As String) As Boolean
        Get
            Return mConfiguredPositions.ContainsKey(Label)
        End Get
    End Property

    Public ReadOnly Property ConfiguredPositions() As Dictionary(Of String, ConfiguredStagePosition)
        Get
            Return mConfiguredPositions
        End Get
    End Property

    Public ReadOnly Property ConfiguredPosition(ByVal Position As StagePositionEnum) As ConfiguredStagePosition
        Get
            Dim label As String
            label = Me.GetStagePositionLabel(Position)

            If mConfiguredPositions.ContainsKey(label) Then
                Return mConfiguredPositions(label)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property ConfiguredPosition(ByVal Label As String) As ConfiguredStagePosition
        Get
            If mConfiguredPositions.ContainsKey(Label) Then
                Return mConfiguredPositions(Label)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Sub AddConfiguredPosition(ByVal Position As ConfiguredStagePosition)
        mConfiguredPositions.Add(Position.Label, Position)
        Me.SaveConfiguredPositions()
    End Sub

    Public Sub UpdateConfiguredPosition(ByVal Position As ConfiguredStagePosition)
        Dim i As Integer
        Dim OldPositions() As Double

        'get old position
        OldPositions = mConfiguredPositions(Position.Label).Positions

        'for the new position, we will NULL the value that was NULL in the original label
        For i = 0 To AxisCount - 1
            If Double.IsNaN(OldPositions(i)) Then Position.Positions(i) = Double.NaN
        Next

        'update the old data with new one
        mConfiguredPositions(Position.Label) = Position

        'save it
        Me.SaveConfiguredPositions()
    End Sub

    Public Function GetConfiguredStagePosition(ByVal Position As StagePositionEnum) As Position3D
        Dim label As String
        Dim x, y, z As Double

        'get data entry label
        label = Me.GetStagePositionLabel(Position)

        'reject if label is not valid
        If Not Me.HaveConfiguredPosition(label) Then
            Return New Position3D(Double.NaN, Double.NaN, Double.NaN)
        End If

        x = mConfiguredPositions(label).Positions(AxisNameEnum.StageX)
        y = mConfiguredPositions(label).Positions(AxisNameEnum.StageY)
        z = mConfiguredPositions(label).Positions(AxisNameEnum.StageZ)

        Return New Position3D(x, y, z)

    End Function

    Public Function GetConfiguredBeamScanPosition(ByVal Position As StagePositionEnum) As Position3D
        Dim label As String
        Dim x, y, z As Double

        'get data entry label
        label = Me.GetStagePositionLabel(Position)

        'reject if label is not valid
        If Not Me.HaveConfiguredPosition(label) Then
            Return New Position3D(Double.NaN, Double.NaN, Double.NaN)
        End If

        x = mConfiguredPositions(label).Positions(AxisNameEnum.BeamScanX)
        y = mConfiguredPositions(label).Positions(AxisNameEnum.BeamScanY)
        z = mConfiguredPositions(label).Positions(AxisNameEnum.BeamScanZ)

        Return New Position3D(x, y, z)

    End Function

#End Region

#Region "safty window"
    Public Class SafetyWindow
        Private mXmin As Double
        Private mXmax As Double
        Private mYmin As Double
        Private mYmax As Double

        Public Sub New(ByVal NominalX As Double, ByVal NominalY As Double, ByVal XMinus As Double, ByVal XPlus As Double, ByVal YMinus As Double, ByVal YPlus As Double)
            mXmin = NominalX - XMinus
            mXmax = NominalX + XPlus
            mYmin = NominalY - YMinus
            mYmax = NominalY + YPlus
        End Sub

        Public ReadOnly Property Valid() As Boolean
            Get
                If Double.IsNaN(mXmin) Then Return False
                If Double.IsNaN(mXmax) Then Return False
                If Double.IsNaN(mYmin) Then Return False
                If Double.IsNaN(mYmax) Then Return False
                Return True
            End Get
        End Property

        Public Function IsInsideWindow(ByVal Position As Position2D) As Boolean
            If Position.X < mXmin Then Return False
            If Position.X > mXmax Then Return False
            If Position.Y < mYmin Then Return False
            If Position.Y > mYmax Then Return False
            Return True
        End Function

        Public Function IsBothInsideWindow(ByVal Position1 As Position2D, ByVal Position2 As Position2D) As Boolean
            If Not Me.IsInsideWindow(Position1) Then Return False
            If Not Me.IsInsideWindow(Position2) Then Return False
            Return True
        End Function
    End Class

    Private Function ParsePositionSafetyWindow() As Boolean
        Dim i As Integer
        Dim sKey As String
        Dim Position As StagePositionEnum
        Dim NominalPosition As Position3D
        Dim XMinus, XPlus, YMinus, YPlus As Double

        For i = 0 To PartCount - 1
            Select Case CType(i, PartEnum)
                Case PartEnum.PBC
                    Position = StagePositionEnum.PbcAlign
                Case PartEnum.BS1
                    Position = StagePositionEnum.BsAlign1
                Case PartEnum.BS2
                    Position = StagePositionEnum.BsAlign2
                Case PartEnum.Lens1
                    Position = StagePositionEnum.LensAlignCh1
                Case PartEnum.Lens2
                    Position = StagePositionEnum.LensAlignCh2
                Case PartEnum.Lens3
                    Position = StagePositionEnum.LensAlignCh3
                Case PartEnum.Lens4
                    Position = StagePositionEnum.LensAlignCh4
                Case Else
                    Return False
            End Select

            NominalPosition = Me.GetConfiguredStagePosition(Position)

            'get window 
            sKey = [Enum].GetName(GetType(StagePositionEnum), Position)
            sKey = sKey.Replace("Align", "") + "SafetyWindow"
            XMinus = mPara.ReadParameter(sKey, "Xminus", 0.1)
            XPlus = mPara.ReadParameter(sKey, "Xplus", 0.1)
            YMinus = mPara.ReadParameter(sKey, "Yminus", 0.1)
            YPlus = mPara.ReadParameter(sKey, "Xplus", 0.1)

            'build class
            mXYSafetyWindow(i) = New SafetyWindow(NominalPosition.X, NominalPosition.Y, XMinus, XPlus, YMinus, YPlus)
        Next
        Return True
    End Function

    Public ReadOnly Property YforSafeMove() As Double
        Get
            Dim label As String
            label = Me.GetStagePositionLabel(StagePositionEnum.YforSafeMove)
            Return mConfiguredPositions(label).Positions(AxisNameEnum.StageY)
        End Get
    End Property

    Public ReadOnly Property ZforSafeMove() As Double
        Get
            Dim label As String
            label = Me.GetStagePositionLabel(StagePositionEnum.ZforSafeMove)
            Return mConfiguredPositions(label).Positions(AxisNameEnum.StageZ)
        End Get
    End Property

    Public ReadOnly Property ZforVisualCheck() As Double
        Get
            Dim label As String
            label = Me.GetStagePositionLabel(StagePositionEnum.ZforCheck)
            Return mConfiguredPositions(label).Positions(AxisNameEnum.StageZ)
        End Get
    End Property

    Public Function IsMoveSafe(ByVal Axis As AxisNameEnum, ByVal Position1 As Double, ByVal Position2 As Double) As Boolean
        Dim P1, P2 As Position2D
        Dim v As Double

        'check Z first, if Z is low, it is fine
        v = Me.GetStagePosition(AxisNameEnum.StageZ)
        If v <= Me.ZforSafeMove Then Return True

        'Z is high, check window
        Select Case Axis
            Case AxisNameEnum.StageX
                v = Me.GetStagePosition(AxisNameEnum.StageY)
                P1 = New Position2D(Position1, v)
                P2 = New Position2D(Position2, v)
                Return Me.IsMoveSafe(P1, P2)

            Case AxisNameEnum.StageY
                v = Me.GetStagePosition(AxisNameEnum.StageX)
                P1 = New Position2D(v, Position1)
                P2 = New Position2D(v, Position2)
                Return Me.IsMoveSafe(P1, P2)

            Case Else
                'all the other axis are presumbly safe
                Return True
        End Select

    End Function

    Public Function IsMoveSafe(ByVal Position1 As Position2D, ByVal Position2 As Position2D) As Boolean
        Dim i As Integer
        'loop through all the possible windows
        For i = 0 To mXYSafetyWindow.Length - 1
            If Not mXYSafetyWindow(i).Valid Then Continue For
            If mXYSafetyWindow(i).IsBothInsideWindow(Position1, Position2) Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

End Class


