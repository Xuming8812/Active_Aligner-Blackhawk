Option Explicit On
Option Strict On
Option Infer Off

Imports BlackHawk.Instrument.iRCX

Public Class bhRobotArm

    Public Structure Position2D
        Public Sub New(ByVal X0 As Double, ByVal Y0 As Double)
            X = X0
            Y = Y0
        End Sub
        Public X As Double
        Public Y As Double
    End Structure

    Public Structure PositionStructure
        Public Sub New(ByVal X0 As Double, ByVal Y0 As Double, ByVal Z0 As Double, _
                       ByVal A0 As Double, ByVal B0 As Double, ByVal C0 As Double)
            X = X0
            Y = Y0
            Z = Z0
            A = A0
            B = B0
            C = C0
        End Sub
        Public Sub New(ByVal X0 As String, ByVal Y0 As String, ByVal Z0 As String, _
                      ByVal A0 As String, ByVal B0 As String, ByVal C0 As String)
            X = Convert.ToDouble(IIf(X0 = "NA", Double.NaN, X0))
            Y = Convert.ToDouble(IIf(Y0 = "NA", Double.NaN, Y0))
            Z = Convert.ToDouble(IIf(Z0 = "NA", Double.NaN, Z0))
            A = Convert.ToDouble(IIf(A0 = "NA", Double.NaN, A0))
            B = Convert.ToDouble(IIf(B0 = "NA", Double.NaN, B0))
            C = Convert.ToDouble(IIf(C0 = "NA", Double.NaN, C0))
        End Sub
        Public X As Double
        Public Y As Double
        Public Z As Double
        Public A As Double
        Public B As Double
        Public C As Double
    End Structure

    Public Structure ArmInfoStructure
        Public Speed As Double
        Public SafeZ As Double
        Public PickUpZDown As Double
        Public PickUpAClose As Double
        Public ZAfterPickUp As Double
        Public LoadZDown As Double
        Public LoadAOpen As Double
        Public ZAfterLoad As Double
        Public UnloadZDown As Double
        Public UnlaodAClose As Double
        Public PlaceZDown As Double
        Public ZAfterPlace As Double
        Public PlaceAOpen As Double

        Public MinStageZForRobotHome As Double
    End Structure

    Public Enum EPositionType
        Configured
        CoS
        FinishedGood
        Discard
    End Enum

    Public Class ConfiguredArmPosition
        Public Const TableHeader As String = "Label           	 X  Y  Z	A	B C"
        Public Const AxisCount As Integer = 6

        Private mLabel As String
        Private mPositions As PositionStructure

        Public Sub New(ByVal Label As String, ByVal Positions As PositionStructure)
            mLabel = Label
            mPositions = Positions
        End Sub

        Public ReadOnly Property Label As String
            Get
                Return mLabel
            End Get
        End Property

        Public ReadOnly Property Positions() As PositionStructure
            Get
                Return mPositions
            End Get
        End Property

        Public ReadOnly Property TableString() As String
            Get
                Dim s As String

                s = String.Format("{0,-33}", mLabel)

                s += ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.X), "NA", mPositions.X)) + ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.Y), "NA", mPositions.Y)) + ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.Z), "NA", mPositions.Z)) + ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.A), "NA", mPositions.A)) + ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.B), "NA", mPositions.B)) + ControlChars.Tab
                s += String.Format("{0,7:0.000}", IIf(Double.IsNaN(mPositions.C), "NA", mPositions.C))

                Return s
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return mLabel
        End Function

        Public Shared Function Parse(ByVal s As String) As ConfiguredArmPosition
            Dim sData() As String

            Dim Positions As PositionStructure

            sData = s.Split(ControlChars.Tab)

            If sData.Length < (AxisCount + 1) Then Return Nothing

            Positions = New PositionStructure(sData(1).Trim(), sData(2).Trim(), sData(3).Trim(), _
                                          sData(4).Trim(), sData(5).Trim(), sData(6).Trim())

            Return New ConfiguredArmPosition(sData(0).Trim(), Positions)

        End Function

    End Class

    Private mConfiguredPositions As Dictionary(Of String, ConfiguredArmPosition)

    Private mCoSPositionInTray As Dictionary(Of String, ConfiguredArmPosition)

    Private mCoSFGIndex As Integer
    Private mCoSFGPositionInTray As Dictionary(Of String, ConfiguredArmPosition)

    Private mCoSDiscardIndex As Integer
    Private mCoSDiscardPositionInTray As Dictionary(Of String, ConfiguredArmPosition)

    Private mRCX As Instrument.iRCX
    Private mPara As w2.w2IniFileXML

    Public ArmInfo As ArmInfoStructure

    Public Function Initialize(ByRef hRCX As Instrument.iRCX, ByRef hConfig As w2.w2IniFileXML) As Boolean
        Dim s As String

        mRCX = hRCX
        mPara = hConfig

        ArmInfo.Speed = Convert.ToDouble(mPara.ReadParameter("ArmTable", "AutoSpeed", "5"))
        ArmInfo.SafeZ = Convert.ToDouble(mPara.ReadParameter("ArmTable", "SafeZ", "10"))
        ArmInfo.PickUpZDown = Convert.ToDouble(mPara.ReadParameter("ArmTable", "PickUpZDown", "5"))
        ArmInfo.PickUpAClose = Convert.ToDouble(mPara.ReadParameter("ArmTable", "PickUpAClose", "5"))
        ArmInfo.ZAfterPickUp = Convert.ToDouble(mPara.ReadParameter("ArmTable", "ZAfterPickUp", "5"))
        ArmInfo.LoadZDown = Convert.ToDouble(mPara.ReadParameter("ArmTable", "LoadZDown", "5"))
        ArmInfo.LoadAOpen = Convert.ToDouble(mPara.ReadParameter("ArmTable", "LoadAOpen", "5"))
        ArmInfo.ZAfterLoad = Convert.ToDouble(mPara.ReadParameter("ArmTable", "ZAfterLoad", "5"))
        ArmInfo.UnloadZDown = Convert.ToDouble(mPara.ReadParameter("ArmTable", "UnloadZDown", "5"))
        ArmInfo.UnlaodAClose = Convert.ToDouble(mPara.ReadParameter("ArmTable", "UnloadAClose", "5"))
        ArmInfo.PlaceZDown = Convert.ToDouble(mPara.ReadParameter("ArmTable", "PlaceZDown", "5"))
        ArmInfo.PlaceAOpen = Convert.ToDouble(mPara.ReadParameter("ArmTable", "PlaceAOpen", "5"))
        ArmInfo.ZAfterPlace = Convert.ToDouble(mPara.ReadParameter("ArmTable", "ZAfterPlace", "5"))
        ArmInfo.MinStageZForRobotHome = Convert.ToDouble(mPara.ReadParameter("ArmTable", "MinStageZForRobotHome", "20"))

        If mRCX IsNot Nothing Then
            mRCX.Speed = Convert.ToDouble(ArmInfo.Speed)
        End If

        Try
            'known arm positions
            Me.ParseConfiguredPositions()

            'cos data
            Me.ParseCoSPositionsInTray()

            Me.ParseCoSFGPositionsInTray()

            Me.ParseCoSDsicardPositionsInTray()

            CoSFGIndex = 0
            CoSDiscardIndex = 0
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Return True
    End Function


#Region "move, position, velocity"

    Public Function GetArmPosition() As PositionStructure

        Return mRCX.CurrentPosition
    End Function

    Public Function MoveArm(ByVal Target As PositionStructure) As Boolean
        Return mRCX.Move(Target)
    End Function

    Public Function MoveArm(ByVal x As Double, ByVal y As Double, ByVal z As Double, ByVal a As Double, ByVal b As Double, ByVal c As Double) As Boolean
        Dim pt As PositionStructure
        pt = GetArmPosition()

        pt.X = x
        pt.Y = y
        pt.Z = z
        pt.A = a
        pt.B = b
        pt.C = c
        Return MoveArm(pt)
    End Function

    Public Function MoveArmToSafePosition() As Boolean
        Return MoveArm(ConfiguredArmMovePosition(ArmPosition.CoSSafty))
    End Function

    'Public Function MoveArmForDut(ByVal Target As PositionStructure) As Boolean
    '    Dim pt As PositionStructure
    '    pt = GetArmPosition()

    '    pt.X = Target.X
    '    pt.Y = Target.Y
    '    pt.R = Target.R
    '    MoveArm(pt)

    '    mRCX.Drive(EAxis.Z, Target.Z)
    'End Function

    Public Function MoveArmAbsolute(ByVal axis As EAxis, ByVal target As Double) As Boolean
        Dim pt As PositionStructure
        pt = GetArmPosition()

        Select Case axis
            Case EAxis.X
                pt.X = target
            Case EAxis.Y
                pt.Y = target
            Case EAxis.Z
                pt.Z = target
            Case EAxis.A
                pt.A = target
            Case EAxis.B
                pt.B = target
            Case EAxis.C
                pt.C = target
        End Select
        Return MoveArm(pt)
    End Function

    Public Function MoveArmRelative(ByVal axis As EAxis, ByVal target As Double) As Boolean
        Dim pt As PositionStructure
        pt = GetArmPosition()

        Select Case axis
            Case EAxis.X
                pt.X += target
            Case EAxis.Y
                pt.Y += target
            Case EAxis.Z
                pt.Z += target
            Case EAxis.A
                pt.A += target
            Case EAxis.B
                pt.B += target
            Case EAxis.C
                pt.C += target
        End Select
        Return MoveArm(pt)
    End Function

    Public Function PrepareArm() As Boolean

        mRCX.ServoMode = Instrument.iRCX.EServoStatus.ServoON

        If mRCX IsNot Nothing Then
            mRCX.Speed = ArmInfo.Speed
        End If
        Return True
    End Function



#End Region

#Region "Configured positions"
    Private Sub ParseCoSPositionsInTray()
        Dim s, lines() As String
        Dim x As ConfiguredArmPosition

        'New
        mCoSPositionInTray = New Dictionary(Of String, ConfiguredArmPosition)

        'get table 
        s = ""
        s = mPara.ReadParameter("ArmTable", "CoSTray", "")

        s = s.Trim()
        lines = s.Split(ControlChars.Cr)

        'parse table
        For Each s In lines
            s = s.Trim()
            'first line is table header
            If s = ConfiguredArmPosition.TableHeader Then Continue For
            'parse info
            x = ConfiguredArmPosition.Parse(s)
            If x Is Nothing Then
                s = "Error parsing configuraed stage information: " + ControlChars.CrLf + s
                s += ControlChars.Tab + "This entry will be ignored."
                MessageBox.Show(s, "Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Continue For
            End If

            mCoSPositionInTray.Add(x.Label.Trim(), x)

        Next

    End Sub

    Private Sub ParseCoSFGPositionsInTray()
        Dim s, lines() As String
        Dim x As ConfiguredArmPosition

        'New
        mCoSFGPositionInTray = New Dictionary(Of String, ConfiguredArmPosition)

        'get table 
        s = ""
        s = mPara.ReadParameter("ArmTable", "CoSFGTray", "")

        s = s.Trim()
        lines = s.Split(ControlChars.Cr)

        'parse table
        For Each s In lines
            s = s.Trim()
            'first line is table header
            If s = ConfiguredArmPosition.TableHeader Then Continue For
            'parse info
            x = ConfiguredArmPosition.Parse(s)
            If x Is Nothing Then
                s = "Error parsing configuraed stage information: " + ControlChars.CrLf + s
                s += ControlChars.Tab + "This entry will be ignored."
                MessageBox.Show(s, "Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Continue For
            End If

            mCoSFGPositionInTray.Add(x.Label.Trim(), x)

        Next
    End Sub

    Private Sub ParseCoSDsicardPositionsInTray()
        Dim s, lines() As String
        Dim x As ConfiguredArmPosition

        'New
        mCoSDiscardPositionInTray = New Dictionary(Of String, ConfiguredArmPosition)

        'get table 
        s = ""
        s = mPara.ReadParameter("ArmTable", "CoSDiscardTray", "")

        s = s.Trim()
        lines = s.Split(ControlChars.Cr)

        'parse table
        For Each s In lines
            s = s.Trim()
            'first line is table header
            If s = ConfiguredArmPosition.TableHeader Then Continue For
            'parse info
            x = ConfiguredArmPosition.Parse(s)
            If x Is Nothing Then
                s = "Error parsing configuraed stage information: " + ControlChars.CrLf + s
                s += ControlChars.Tab + "This entry will be ignored."
                MessageBox.Show(s, "Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Continue For
            End If

            mCoSDiscardPositionInTray.Add(x.Label.Trim(), x)

        Next
    End Sub

    Public Function GetCoSPositionInTray(ByVal Index As Integer) As ConfiguredArmPosition
        Try

            Return mCoSPositionInTray(Index.ToString())

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public ReadOnly Property TotalCoSPositionsInTray() As Integer
        Get
            Return mCoSPositionInTray.Values.Count
        End Get
    End Property

    Public Property CoSFGIndex() As Integer
        Get
            Return mCoSFGIndex
        End Get
        Set(value As Integer)
            mCoSFGIndex = value
        End Set
    End Property

    Public Function GetCoSFGPositionInTray(ByVal Index As Integer) As ConfiguredArmPosition
        Try

            Return mCoSFGPositionInTray(Index.ToString())

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public ReadOnly Property TotalCoSFGPositionsInTray() As Integer
        Get
            Return mCoSFGPositionInTray.Values.Count
        End Get
    End Property

    Public Property CoSDiscardIndex() As Integer
        Get
            Return mCoSDiscardIndex
        End Get
        Set(value As Integer)
            mCoSDiscardIndex = value
        End Set
    End Property

    Public Function GetCoSDiscardPositionInTray(ByVal Index As Integer) As ConfiguredArmPosition
        Try

            Return mCoSDiscardPositionInTray(Index.ToString())

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public ReadOnly Property TotalCoSDiscardPositionsInTray() As Integer
        Get
            Return mCoSDiscardPositionInTray.Values.Count
        End Get
    End Property

    Private Sub ParseConfiguredPositions()
        Dim s, data() As String
        Dim x As ConfiguredArmPosition

        'New
        mConfiguredPositions = New Dictionary(Of String, ConfiguredArmPosition)

        'get table
        s = mPara.ReadParameter("ArmTable", "ConfiguredPositions", "")
        s = s.Trim()
        data = s.Split(ControlChars.Cr)

        'parse table
        For Each s In data
            s = s.Trim()
            'first line is table header
            If s = ConfiguredArmPosition.TableHeader Then Continue For
            'parse info
            x = ConfiguredArmPosition.Parse(s)
            If x Is Nothing Then
                s = "Error parsing configuraed stage information: " + ControlChars.CrLf + s
                s += ControlChars.Tab + "This entry will be ignored."
                MessageBox.Show(s, "Configured Position", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Continue For
            End If
            mConfiguredPositions.Add(x.Label, x)
        Next

    End Sub

    Public ReadOnly Property ConfiguredPositions() As Dictionary(Of String, ConfiguredArmPosition)
        Get
            Return mConfiguredPositions
        End Get
    End Property

    Public ReadOnly Property HavePosition(ByVal Label As String, ByVal PositionType As EPositionType, ByVal TrayIndex As String) As Boolean
        Get
            Select Case PositionType
                Case EPositionType.Configured
                    Return mConfiguredPositions.ContainsKey(Label)
                Case EPositionType.CoS
                    Return mCoSPositionInTray.ContainsKey(TrayIndex)
                Case EPositionType.FinishedGood
                    Return mCoSFGPositionInTray.ContainsKey(TrayIndex)
                Case EPositionType.Discard
                    Return mCoSDiscardPositionInTray.ContainsKey(TrayIndex)
                Case Else
                    Return False
            End Select
        End Get
    End Property

    Public ReadOnly Property ConfiguredPosition(ByVal Label As String) As ConfiguredArmPosition
        Get
            If mConfiguredPositions.ContainsKey(Label) Then
                Return mConfiguredPositions(Label)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Sub AddPosition(ByVal Position As ConfiguredArmPosition, ByVal PositionType As EPositionType, ByVal TrayIndex As String)
        Select Case PositionType
            Case EPositionType.Configured
                mConfiguredPositions.Add(Position.Label, Position)
            Case EPositionType.CoS
                mCoSPositionInTray.Add(TrayIndex, Position)
            Case EPositionType.FinishedGood
                mCoSFGPositionInTray.Add(TrayIndex, Position)
            Case EPositionType.Discard
                mCoSDiscardPositionInTray.Add(TrayIndex, Position)
        End Select

    End Sub

    Public Sub UpdatePosition(ByVal Position As ConfiguredArmPosition, ByVal positionType As EPositionType)

        Dim OldPositions As PositionStructure

        Select Case positionType
            Case EPositionType.Configured
                'get old position
                OldPositions = mConfiguredPositions(Position.Label).Positions
                'update the old data with new one
                mConfiguredPositions(Position.Label) = Position
            Case EPositionType.CoS
                'get old position
                OldPositions = mCoSPositionInTray(Position.Label).Positions
                'update the old data with new one
                mCoSPositionInTray(Position.Label) = Position
            Case EPositionType.FinishedGood
                'get old position
                OldPositions = mCoSFGPositionInTray(Position.Label).Positions
                'update the old data with new one
                mCoSFGPositionInTray(Position.Label) = Position
            Case EPositionType.Discard
                'get old position
                OldPositions = mCoSDiscardPositionInTray(Position.Label).Positions
                'update the old data with new one
                mCoSDiscardPositionInTray(Position.Label) = Position
        End Select


    End Sub

    Public Sub SaveConfiguredPositions(ByVal positionType As EPositionType)
        Dim table As String
        Dim x As ConfiguredArmPosition

        table = ControlChars.CrLf + ControlChars.Tab + ConfiguredArmPosition.TableHeader

        Select Case positionType
            Case EPositionType.Configured
                For Each x In mConfiguredPositions.Values
                    table += ControlChars.CrLf + ControlChars.Tab + x.TableString
                Next
                table += ControlChars.CrLf + "    "

                mPara.WriteParameter("ArmTable", "ConfiguredPositions", table)
            Case EPositionType.CoS
                For Each x In mCoSPositionInTray.Values
                    table += ControlChars.CrLf + ControlChars.Tab + x.TableString
                Next
                table += ControlChars.CrLf + "    "

                mPara.WriteParameter("ArmTable", "CoSTray", table)
            Case EPositionType.FinishedGood
                For Each x In mCoSFGPositionInTray.Values
                    table += ControlChars.CrLf + ControlChars.Tab + x.TableString
                Next
                table += ControlChars.CrLf + "    "

                mPara.WriteParameter("ArmTable", "CoSFGTray", table)
            Case EPositionType.Discard
                For Each x In mCoSDiscardPositionInTray.Values
                    table += ControlChars.CrLf + ControlChars.Tab + x.TableString
                Next
                table += ControlChars.CrLf + "    "

                mPara.WriteParameter("ArmTable", "CoSDiscardTray", table)
        End Select

    End Sub

#End Region

#Region "Named configured positions"
    Private mStagePositionTol As Double = 0.01   '10um

    Public Enum ArmPosition
        'do not move the order or the value of the following items, they are used externally in scirpt
        CoSPickUp

        CoSLoadReady
        CoSLoad

        CoSUnloadready
        CoSUnload

        CoSPlace
        CoSThrow

        CoSSafty
    End Enum

    Public ReadOnly Property ConfiguredArmMovePosition(ByVal Position As ArmPosition) As PositionStructure
        Get
            Dim label As String

            Select Case Position
                Case ArmPosition.CoSPickUp
                    label = "Robot Pickup Position"
                Case ArmPosition.CoSLoadReady
                    label = "Robot Loadready Position"
                Case ArmPosition.CoSLoad
                    label = "Robot Load Position"
                Case ArmPosition.CoSUnloadready
                    label = "Robot Unloadready Position"
                Case ArmPosition.CoSUnload
                    label = "Robot Unload Position"
                Case ArmPosition.CoSPlace
                    label = "Robot Place Position"
                Case ArmPosition.CoSThrow
                    label = "Robot Throw Position"
                Case ArmPosition.CoSSafty
                    label = "Robot Safty Position"

                Case Else
                    Return New PositionStructure(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN)
            End Select


            Return mConfiguredPositions(label).Positions
        End Get
    End Property

#End Region

End Class