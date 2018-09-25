Option Explicit On
Option Strict On
Option Infer Off

Public Class iXpsStage

#Region "enum and structure"
    Public Structure Position2D
        Public Sub New(ByVal X0 As Double, ByVal Y0 As Double)
            X = X0
            Y = Y0
        End Sub
        Public X As Double
        Public Y As Double

        Public Overrides Function ToString() As String
            Const s As String = "({0,9:0000}, {1,9:0000})"
            Return String.Format(s, X, Y)
        End Function
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

        Public Overrides Function ToString() As String
            Const s As String = "({0,9:0000}, {1,9:0000}, {2,9:0000})"
            Return String.Format(s, X, Y, Z)
        End Function
    End Structure

    Public Enum AxisNameEnum
        'starte from zero, and no jump, we are using this to do the loop
        'do not change the name, it matches that in the configuration file
        'do not change the order, they are the order by Configured Position Index
        StageX = 0
        StageY
        StageZ
        BeamScanX
        BeamScanY
        BeamScanZ
        Probe
        'the following two are not on XPS system, but that is fine
        AngleMain
        AngleHexapod
        PiLS
    End Enum
    Public Shared AxisCount As Integer = [Enum].GetNames(GetType(AxisNameEnum)).Length

    Private Const AlignEpoxyEnumOffset As Integer = 10
    Public Enum StagePositionEnum
        'do not move the order or the value of the following items, 
        'their values are used externally in scirpt

        'alignment position
        Bs1Align = PartEnum.BS1
        PbsAlign = PartEnum.PBS
        Bs2Align = PartEnum.BS2

        Lens1Align = PartEnum.Lens1
        Lens2Align = PartEnum.Lens2
        Lens3Align = PartEnum.Lens3
        Lens4Align = PartEnum.Lens4

        'Epoxy - note that epoxy position is offset from alignment by 10
        Bs1Epoxy = PartEnum.BS1 + AlignEpoxyEnumOffset
        PbsEpoxy = PartEnum.PBS + AlignEpoxyEnumOffset
        Bs2Epoxy = PartEnum.BS2 + AlignEpoxyEnumOffset

        Lens1Epoxy = PartEnum.Lens1 + AlignEpoxyEnumOffset
        Lens2Epoxy = PartEnum.Lens2 + AlignEpoxyEnumOffset
        Lens3Epoxy = PartEnum.Lens3 + AlignEpoxyEnumOffset
        Lens4Epoxy = PartEnum.Lens4 + AlignEpoxyEnumOffset

        'the other are relative random
        LoadUnload = 20

        'work location
        LensPickup
        HexpodPickup
        EpoxyDump
        EpoxyCalibration

        'CCD view
        CcdPackage1View = 30
        CcdPackage2View
        CcdPackage3View
        CcdPackage4View
        CcdPartTopView
        CcdPartBottomView
        CcdEpoxyView
        CcdOmuxView
        CcdRechekcView
        CcdPbsRecheckView

        'beam scan
        BeamScanNear = 60
        BeamScanMid
        BeamScanFar

        AutoFocusForLens = 71
        AutoFocusForBS
        AutoFocusForPin
        AutoFocusForPickup

        'stage safety
        YforSafeMove = 100
        ZforSafeMove
        ZforCheck

        Test = 111

    End Enum

    Public Enum PartEnum
        BS1         '= BS1
        PBS         '= PBS
        BS2         '= BS2
        Lens1
        Lens2
        Lens3
        Lens4
    End Enum
    Public Shared PartCount As Integer = [Enum].GetNames(GetType(PartEnum)).Length

    Public Enum StageEnum
        Main
        Hexapod
    End Enum
#End Region

#Region "utility class"
    Public Class StageInfo
        Public Controller As Instrument.iMotionController
        Public Name As String
        Public Axis As Integer
        Public Home As Double
        Public LimitLo As Double
        Public LimitHi As Double
        Public Velocity As Double
        Public Installed As Boolean

        Public Sub New()
            Controller = Nothing
            Name = ""
        End Sub

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
    Private mHexapodConfiguredPositions As Dictionary(Of String, ConfiguredStagePosition)

    Private mPartPositionInTray As Dictionary(Of Integer, Position2D)
    Private mXYSafetyWindow(PartCount - 1) As SafetyWindow

    Private mXPS As Instrument.iXPS

    Private mHexapod As Instrument.iPiGCS2
    Private mPI As Instrument.iPiGCS843

    Private mPiLS As Instrument.iPiLS65

    Private mPara As w2.w2IniFileXML

#Region "public access"
    Public ReadOnly Property XPSController() As Instrument.iXPS
        Get
            Return mXPS
        End Get
    End Property

    Public ReadOnly Property PiAngleController() As Instrument.iPiGCS843
        Get
            Return mPI
        End Get
    End Property

    Public ReadOnly Property PiHexapod() As Instrument.iPiGCS2
        Get
            Return mHexapod
        End Get
    End Property

    Public ReadOnly Property PiLS() As Instrument.iPiLS65
        Get
            Return mPiLS
        End Get
    End Property

    'we need to add two additional motor here
    Public Function Initialize(ByRef hConfig As w2.w2IniFileXML, ByRef hXPS As Instrument.iXPS, ByRef hPI As Instrument.iPiGCS843, _
                               ByRef hHexapod As Instrument.iPiGCS2, ByRef hPiLS As Instrument.iPiLS65) As Boolean
        Dim s, data() As String
        Dim index As Integer
        Dim v As Double
        Dim x As StageInfo

        mXPS = hXPS
        mPI = hPI
        mHexapod = hHexapod
        mPiLS = hPiLS
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
        For Each x In mStageData
            If x Is Nothing Then Continue For
            If Not x.Installed Then Continue For
            Select Case x.Axis
                Case Is > 20
                    'PI motor for angle
                    x.Controller = hPI
                    x.Axis -= 20

                Case Is > 30
                    'PI motor for angle
                    x.Controller = hHexapod
                    x.Axis -= 30

                Case Else
                    'standard XPS motor
                    x.Controller = hXPS
            End Select

            If x.Controller IsNot Nothing Then
                x.Controller.Axis = x.Axis
                v = x.Controller.VelocityMaximum
                If x.Velocity > v Then x.Velocity = v
                x.Controller.Velocity = x.Velocity
            End If
        Next

        'known stage positions and their safety window
        If Not Me.ParseConfiguredPositions() Then Return False
        If Not Me.ParsePositionSafetyWindow() Then Return False
        If Not Me.ParseHexapodConfiguredPositions() Then Return False

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
            Case AxisNameEnum.AngleMain
                s = "PI Angle Stage on Main Arm"

            Case AxisNameEnum.AngleHexapod
                s = "PI Angle Stage from Hexapod"

            Case AxisNameEnum.PiLS
                s = "PI Linear Stage"

            Case AxisNameEnum.Probe
                s = "Probe Pin Stage"

            Case AxisNameEnum.BeamScanX
                s = "Beam Scan Stage X"
            Case AxisNameEnum.BeamScanY
                s = "Beam Scan Stage Y"
            Case AxisNameEnum.BeamScanZ
                s = "Beam Scan Stage Z"

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
    Public Function IsControllerReady() As Boolean
        Dim s As String
        Dim i, ii As Integer
        Dim iStatus As Integer
        Dim ctrl As Instrument.iMotionController

        ii = AxisCount - 1
        For i = 0 To ii
            'no controller, skip
            If mStageData(i).Controller Is Nothing Then Continue For

            'get controller
            ctrl = mStageData(i).Controller

            'select active axis
            ctrl.Axis = mStageData(i).Axis

            Select Case True
                Case (ctrl Is mXPS)
                    'check missing stage by reading its status
                    iStatus = mXPS.StatusCode
                    If mXPS.LastError <> "" Then
                        s = "Missing controller/motor for " + Me.GetStageName(CType(i, AxisNameEnum))
                        MessageBox.Show(s)
                        Return False
                    End If

            End Select

            'try to enable driver
            If Not ctrl.DriveEnabled Then ctrl.DriveEnabled = True
            'check if it can be enabled
            If Not ctrl.StageReady Then
                s = "Stage " + Me.GetStageName(CType(i, AxisNameEnum)) + " is not initialized or homed"
                MessageBox.Show(s)
                Return False
            End If
        Next

        Return True
    End Function

    Public Function IsStageReady(ByVal axis As AxisNameEnum) As Boolean
        'not ready is no stage
        If mStageData(axis).Controller Is Nothing Then Return False

        With mStageData(axis).Controller
            .Axis = mStageData(axis).Axis
            If Not .DriveEnabled Then .DriveEnabled = True

            Return .StageReady
        End With

    End Function

    Public Function IsStageReadyAll() As Boolean
        Dim i As Integer

        For i = 0 To AxisCount - 1
            If Not Me.IsStageReady(CType(i, AxisNameEnum)) Then Return False
        Next
        Return True
    End Function

    Public ReadOnly Property StageMoving() As Boolean
        Get
            Dim i As Integer
            For i = 0 To AxisCount - 1
                If mStageData(i).Controller Is Nothing Then Continue For

                mStageData(i).Controller.Axis = mStageData(i).Axis

                If mStageData(i).Controller.StageMoving Then Return True
            Next

            Return False
        End Get
    End Property


    Public Sub SetStageVelocity(ByVal Axis As AxisNameEnum, ByVal NewVelocity As Double)
        mStageData(Axis).Controller.Axis = mStageData(Axis).Axis
        mStageData(Axis).Controller.Velocity = NewVelocity
    End Sub

    Public Sub SetStageAccerleration(ByVal Axis As AxisNameEnum, ByVal NewAccerleration As Double)
        mStageData(Axis).Controller.Axis = mStageData(Axis).Axis
        mStageData(Axis).Controller.Acceleration = NewAccerleration
    End Sub

    Public Sub RecoverStageDefaultVelocity(ByVal Axis As AxisNameEnum)
        mStageData(Axis).Axis = mStageData(Axis).Axis
        mStageData(Axis).Controller.Velocity = mStageData(Axis).Velocity
    End Sub

    Public Function GetStagePosition(ByVal Axis As AxisNameEnum) As Double
        Dim v As Double
        mStageData(Axis).Controller.Axis = mStageData(Axis).Axis
        v = mStageData(Axis).Controller.CurrentPosition()
        Return v - mStageData(Axis).Home
    End Function

    Public Sub GetStageTravelLimit(ByVal Axis As AxisNameEnum, ByRef Min As Double, ByRef Max As Double)
        Min = mStageData(Axis).LimitLo
        Max = mStageData(Axis).LimitHi
    End Sub

    Public Sub HaltMotion()
        Dim i As Integer

        'do this for all axis
        For i = 0 To AxisCount - 1
            If mStageData(i).Controller Is Nothing Then Continue For

            mStageData(i).Controller.Axis = mStageData(i).Axis
            mStageData(i).Controller.HaltMotion()
        Next

    End Sub

    Public Function MoveStageNoWait(ByVal Axis As AxisNameEnum, ByVal Selection As Instrument.iMotionController.MoveToTargetMethodEnum, ByVal Target As Double) As Boolean
        mStageData(Axis).Controller.Axis = mStageData(Axis).Axis
        'If Selection = Instrument.iMotionController.MoveToTargetMethodEnum.Absolute Then
        '    'for absolute move, add software home offset to the hardware offset
        '    Target += mStageData(Axis).Home
        'End If
        Return mStageData(Axis).Controller.MoveNoWait(Selection, Target)
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
            If s.StartsWith(";") Then Continue For
            If s = "" Then Continue For

            'parse row and column - we will ignore them this time - they are hard coded in the tary class
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

    Public Function IsEpoxyPosition(ByVal Position As StagePositionEnum) As Boolean
        Dim s As String
        s = Position.ToString()
        Return s.EndsWith("Epoxy")
    End Function

    Public Function IsCcdPosition(ByVal Position As StagePositionEnum) As Boolean
        Dim s As String
        s = Position.ToString()
        Return s.StartsWith("Ccd") And s.EndsWith("View")
    End Function

    Public Function GetAlignmentPsotionEnum(ByVal EpoxyPositionEnum As StagePositionEnum) As StagePositionEnum
        Return CType(EpoxyPositionEnum - AlignEpoxyEnumOffset, StagePositionEnum)
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

    Public Function SaveConfiguredPosition(ByVal Label As String, ByVal target As iXpsStage.Position3D) As Boolean
        Dim s As String
        Dim i, ii As Integer
        Dim Positions(iXpsStage.AxisCount - 1) As Double
        Dim x, x2 As iXpsStage.ConfiguredStagePosition

        'get currently displayed positions
        Positions(iXpsStage.AxisNameEnum.StageX) = target.X
        Positions(iXpsStage.AxisNameEnum.StageY) = target.Y
        Positions(iXpsStage.AxisNameEnum.StageZ) = target.Z

        'build a new class
        x = New iXpsStage.ConfiguredStagePosition(Label, Positions)

        're-ask the question to confirm 
        If HaveConfiguredPosition(x.Label) Then
            'update the new value
            UpdateConfiguredPosition(x)

        End If

        'commit this to config file
        SaveConfiguredPositions()

        Return True
    End Function

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

    Public Sub RemoveConfiguredPosition(ByVal Position As ConfiguredStagePosition)
        mConfiguredPositions.Remove(Position.Label)
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


    Private Function ParseHexapodConfiguredPositions() As Boolean
        Dim s, data() As String
        Dim x As ConfiguredStagePosition

        'New
        mHexapodConfiguredPositions = New Dictionary(Of String, ConfiguredStagePosition)

        'get table
        s = mPara.ReadParameter("MotionTable", "HexapodConfiguredPositions", "")
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
            mHexapodConfiguredPositions.Add(x.Label, x)
        Next

        Return True
    End Function

    Public Sub SaveHexapodConfiguredPositions()
        Dim table As String
        Dim x As ConfiguredStagePosition

        table = ControlChars.CrLf + ControlChars.Tab + ConfiguredStagePosition.TableHeader

        For Each x In mHexapodConfiguredPositions.Values
            table += ControlChars.CrLf + ControlChars.Tab + x.TableString
        Next
        table += ControlChars.CrLf + "    "

        mPara.WriteParameter("MotionTable", "HexapodConfiguredPositions", table)
    End Sub

    Public ReadOnly Property HaveHexapodConfiguredPosition(ByVal Label As String) As Boolean
        Get
            Return mHexapodConfiguredPositions.ContainsKey(Label)
        End Get
    End Property

    Public ReadOnly Property HexapodConfiguredPositions() As Dictionary(Of String, ConfiguredStagePosition)
        Get
            Return mHexapodConfiguredPositions
        End Get
    End Property

    Public ReadOnly Property HexapodConfiguredPosition(ByVal Position As StagePositionEnum) As ConfiguredStagePosition
        Get
            Dim label As String
            label = Me.GetStagePositionLabel(Position)

            If mHexapodConfiguredPositions.ContainsKey(label) Then
                Return mHexapodConfiguredPositions(label)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property HexapodConfiguredPosition(ByVal Label As String) As ConfiguredStagePosition
        Get
            If mHexapodConfiguredPositions.ContainsKey(Label) Then
                Return mHexapodConfiguredPositions(Label)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Sub AddHexapodConfiguredPosition(ByVal Position As ConfiguredStagePosition)
        mHexapodConfiguredPositions.Add(Position.Label, Position)
        Me.SaveHexapodConfiguredPositions()
    End Sub

    Public Sub UpdateHexapodConfiguredPosition(ByVal Position As ConfiguredStagePosition)
        Dim i As Integer
        Dim OldPositions() As Double

        'get old position
        OldPositions = mHexapodConfiguredPositions(Position.Label).Positions

        'for the new position, we will NULL the value that was NULL in the original label
        For i = 0 To AxisCount - 1
            If Double.IsNaN(OldPositions(i)) Then Position.Positions(i) = Double.NaN
        Next

        'update the old data with new one
        mHexapodConfiguredPositions(Position.Label) = Position

        'save it
        Me.SaveHexapodConfiguredPositions()
    End Sub

    Public Function GetConfiguredHexapodPosition(ByVal Position As StagePositionEnum) As Position3D
        Dim label As String
        Dim x, y, z As Double

        'get data entry label
        label = Me.GetStagePositionLabel(Position)

        'reject if label is not valid
        If Not Me.HaveHexapodConfiguredPosition(label) Then
            Return New Position3D(Double.NaN, Double.NaN, Double.NaN)
        End If

        x = mHexapodConfiguredPositions(label).Positions(AxisNameEnum.StageX)
        y = mHexapodConfiguredPositions(label).Positions(AxisNameEnum.StageY)
        z = mHexapodConfiguredPositions(label).Positions(AxisNameEnum.StageZ)

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
        Dim ePosition As StagePositionEnum
        Dim NominalPosition As Position3D
        Dim XMinus, XPlus, YMinus, YPlus As Double

        For i = 0 To PartCount - 1
            'since we made the alignment postion enum the same value as part enum, we can do simple type change here
            ePosition = CType(i, StagePositionEnum)
            NominalPosition = Me.GetConfiguredStagePosition(ePosition)

            'get window 
            sKey = [Enum].GetName(GetType(PartEnum), ePosition)
            sKey += "SafetyWindow"
            XMinus = mPara.ReadParameter(sKey, "Xminus", 0.1)
            XPlus = mPara.ReadParameter(sKey, "Xplus", 0.2)
            YMinus = mPara.ReadParameter(sKey, "Yminus", 0.1)
            YPlus = mPara.ReadParameter(sKey, "Yplus", 0.1)

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


