Option Strict On
Option Explicit On
Option Infer Off

Imports System.Data.SqlClient

Public Class BlackHawkParameters
    Private mIniFile As w2.w2IniFileXML
    
#Region "structures"
    '--------------------------------------------------------------------------------------------------Laser Diode
    Public Structure LaserDiodeInfo
        Public DefaultCurrent As Double
        Public MinCurrent As Double
        Public MaxCurrent As Double
        Public CurrentStep As Double
        Public MinVoltage As Double
        Public MaxCurrentError As Double
    End Structure

    Private mLaserDiode As LaserDiodeInfo
    Public ReadOnly Property LaserDiode As LaserDiodeInfo
        Get
            Return mLaserDiode
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Beam Scan parameters
    Public Structure BeamScanInfo
        Public Samples As Integer
        Public Gain As Double
        Public SampleFrequency As Double
        Public SampleResolution As Double
    End Structure

    Private mBeamScan As BeamScanInfo
    Public ReadOnly Property BeamScan As BeamScanInfo
        Get
            Return mBeamScan
        End Get
    End Property

    Public Structure LightSourceInfo
        Public Channel1Value As Integer
        Public Channel2Value As Integer
    End Structure

    Private mLightSource1 As LightSourceInfo
    Private mLightSource2 As LightSourceInfo
    Public ReadOnly Property LightSource1 As LightSourceInfo
        Get
            Return mLightSource1
        End Get
    End Property
    Public ReadOnly Property LightSource2 As LightSourceInfo
        Get
            Return mLightSource2
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Probe clamp
    Public Structure ProbeClampInfo
        Public Speed As Double
        Public Force As Double
        Public OpenPosition As Double
    End Structure

    Private mProbeClamp As ProbeClampInfo
    Public ReadOnly Property ProbeClamp As ProbeClampInfo
        Get
            Return mProbeClamp
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Vacuum
    Public Structure VacuumCdaInfo
        Public MinVacuumChange As Double
    End Structure

    Private mVacuumCda As VacuumCdaInfo
    Public ReadOnly Property VacuumCda As VacuumCdaInfo
        Get
            Return mVacuumCda
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Z Touch Sense
    Public Structure zTouchSenseInfo
        Public Samples As Integer
        Public Velocity As Double
        Public StepSize As Double
        Public MaxMove As Double

        Public BondLineLensMin As Double
        Public BondLineLensMax As Double

        Public BondLineBS As Double

        Public GapForPartPickup As Double
        Public GapForApplyEpoxy As Double
        Public GapForOther As Double

        Public ForceChangeThreshold As Double
    End Structure

    Private mzTouchSense As zTouchSenseInfo
    Public ReadOnly Property zTouchSense As zTouchSenseInfo
        Get
            Return mzTouchSense
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Z Slow Move
    Public Structure zSlowMoveInfo
        Public Velocity As Double
        Public StepSize As Double
        Public StepCount As Integer
        Public StepDelay As Integer

        Public ReadOnly Property OffsetDistance As Double
            Get
                Return StepCount * StepSize
            End Get
        End Property
    End Structure

    Private mzSlowMove As zSlowMoveInfo
    Public ReadOnly Property zSlowMove As zSlowMoveInfo
        Get
            Return mzSlowMove
        End Get
    End Property

    '--------------------------------------------------------------------------------------------------Alignment parameters
    Public Structure AlignmentInfo
        'Public FocalLength As Double
        Public MaxPitchError As Double
        Public MaxSteeringY As Double
        Public MaxSteeringZ As Double
        Public MinSteeringY As Double
        Public MinSteeringZ As Double
        Public MinPower As Double
        Public MaxPower As Double
        Public MinYY As Double
        Public MaxYY As Double
        Public MinZZ As Double
        Public MaxZZ As Double

        Public StageSpeed As Double
        Public StageStepX As Double
        Public StageStepFineX As Double
        'added by Ming to do initial alignment for total energy
        Public StageStepXforEnergy As Double
        Public StageStepFineXforEnergy As Double

        Public bsLength As Double

        Public MaxStageMoveX As Double
        Public MaxStageMoveY As Double
        Public MaxStageMoveZ As Double
        Public MaxStageMoveAngle As Double

        Public BeamScanXOffset As Double
        Public BeamScanXList As Double()

        'post epoxy adjustment
        Public PostEpoxyStepSize As Double
        Public postEpoxyMaxMove As Double

        'the following are alignment result
        Public StageMinZ As Double
        Public StageMaxZ As Double
        Public StageTouchZ As Double

        Public PackageOffsetXY() As iXpsStage.Position2D
        Public PackageOffsetAngle() As Double

        Public PartOffsetXY As iXpsStage.Position2D
        Public PartOffsetAngle As Double

        Public PartInTrayOffsetXY As iXpsStage.Position2D
        Public PartInTrayOffsetAngle As Double

        Public OmuxOffsetXY As iXpsStage.Position2D

        Public CcdAngle As Double

        Public PinOffsetX As Double
        Public PinOffsetY As Double
        Public PinOffsetZ As Double

    End Structure

    Private mAlignment As AlignmentInfo
    Public ReadOnly Property Alignment As AlignmentInfo
        Get
            Return mAlignment
        End Get
    End Property

#End Region

    Public Sub New(ByVal hIniFile As w2.w2IniFileXML)
        mIniFile = hIniFile
        Me.ReadParameters()
        Me.ClearAlignmentParameters()
    End Sub

    Public ReadOnly Property IniFile() As w2.w2IniFile
        Get
            Return mIniFile
        End Get
    End Property

#Region "wrote back from outside - alignment offset from CCD ro zTouch"
    Public Sub ClearAlignmentParameters()
        'dim i As Integer

        ReDim mAlignment.PackageOffsetXY(3), mAlignment.PackageOffsetAngle(3)

        'For i = 0 To 3
        '    mAlignment.PackageOffsetXY(i).X = 0
        '    mAlignment.PackageOffsetXY(i).Y = 0
        '    mAlignment.PackageOffsetAngle(i) = 0
        'Next
        
        'mAlignment.PartOffsetXY.X = 0
        'mAlignment.PartOffsetXY.Y = 0
        'mAlignment.PartOffsetAngle = 0

        'mAlignment.PartInTrayOffsetXY.X = 0
        'mAlignment.PartInTrayOffsetXY.Y = 0
        'mAlignment.PartInTrayOffsetAngle = 0
    End Sub

    Public Sub UpdateStageLimitForZ(ByVal zMin As Double, ByVal zMax As Double, ByVal zTouch As Double)
        mAlignment.StageMinZ = zMin
        mAlignment.StageMaxZ = zMax
        mAlignment.StageTouchZ = zTouch
    End Sub

    Public Sub UpdatePackageOffset(ByVal Index As Integer, ByVal XY As iXpsStage.Position2D, ByVal Angle As Double)
        mAlignment.PackageOffsetXY(Index) = XY
        mAlignment.PackageOffsetAngle(Index) = Angle
    End Sub

    Public Sub UpdatePartOffset(ByVal XY As iXpsStage.Position2D, ByVal Angle As Double)
        mAlignment.PartOffsetXY = XY
        mAlignment.PartOffsetAngle = Angle
    End Sub

    Public Sub UpdatePartOffsetInTray(ByVal XY As iXpsStage.Position2D, ByVal Angle As Double)
        mAlignment.PartInTrayOffsetXY = XY
        mAlignment.PartInTrayOffsetAngle = Angle
    End Sub

    Public Sub UpdateOmuxOffset(ByVal XY As iXpsStage.Position2D, ByVal Angle As Double)
        mAlignment.OmuxOffsetXY = XY
    End Sub


#End Region

#Region "alignment parameters"
    Public Enum AlignCoefEnum
        PbsAngle
        LensY
        LensZ
        LensfineY
        LensfineZ
        lensYoverlap
        lensZoverlap
    End Enum

    Private mCoef([Enum].GetValues(GetType(AlignCoefEnum)).Length - 1) As Queue(Of Double)
    Private mBufferSize As Integer

    Public ReadOnly Property AlignCoef(ByVal selection As AlignCoefEnum) As Double
        Get
            Return mCoef(selection).Average()
        End Get
    End Property

    ''' <summary>
    ''' Update the alignment ring buffer with a new coefficient if it passes the validation check and return the new average from the buffer
    ''' </summary>
    ''' <param name="selection">the coefficient selection</param>
    ''' <param name="NewCoef">the new coefficient</param>
    ''' <returns>the average coefficient of the buffer</returns>
    ''' <remarks></remarks>
    Public Function UpdateAlignCoef(ByVal selection As AlignCoefEnum, ByVal NewCoef As Double) As Double
        Dim Diff, AveCoef As Double
        Const MaxPctDiff As Double = 0.3

        AveCoef = Me.AlignCoef(selection)

        'validate the new data, if the % difference is too large, use the previous data
        If Double.IsNaN(NewCoef) Then Return NewCoef

        Diff = NewCoef - AveCoef
        If Math.Abs(Diff / AveCoef) > MaxPctDiff Then Return AveCoef

        'remove an old one if the buffer is full
        If mCoef(selection).Count >= mBufferSize Then mCoef(selection).Dequeue()

        'add the new one
        mCoef(selection).Enqueue(NewCoef)

        'return the new average
        Return Me.AlignCoef(selection)
    End Function

    Public Function SaveAlignCoefToFile() As Boolean
        Const section As String = "Alignment"

        mIniFile.WriteParameter(section, "FocalLengthY", mCoef(AlignCoefEnum.LensY).Average.ToString())
        mIniFile.WriteParameter(section, "FocalLengthZ", mCoef(AlignCoefEnum.LensZ).Average.ToString())
        mIniFile.WriteParameter(section, "FocalLengthfineY", mCoef(AlignCoefEnum.LensfineY).Average.ToString())
        mIniFile.WriteParameter(section, "FocalLengthfineZ", mCoef(AlignCoefEnum.LensfineZ).Average.ToString())
        mIniFile.WriteParameter(section, "AngleCoef", mCoef(AlignCoefEnum.PbsAngle).Average.ToString())

        Return True

    End Function

    Public Function SaveOverlapCoefToFile() As Boolean
        Const section As String = "Alignment"

        mIniFile.WriteParameter(section, "FocalLengthY_overlap", mCoef(AlignCoefEnum.lensYoverlap).Average.ToString())
        mIniFile.WriteParameter(section, "FocalLengthZ_overlap", mCoef(AlignCoefEnum.lensZoverlap).Average.ToString())
        Return True

    End Function

    Public Function SavePinOffsetToFile(ByVal PinOffsetX As Double, ByVal PinOffsetY As Double, ByVal PinOffsetZ As Double) As Boolean
        Const section As String = "Alignment"
        mIniFile.WriteParameter(section, "PinOffsetX", PinOffsetX.ToString())
        mIniFile.WriteParameter(section, "PinOffsetY", PinOffsetY.ToString())
        mIniFile.WriteParameter(section, "PinOffsetZ", PinOffsetZ.ToString())

    End Function
#End Region

    Public Function SaveGoldImageOffset(ByVal eSelection As BlackHawkFunction.InstrumentUtility.CcdViewIndex, ByVal X As Double, ByVal Y As Double, ByVal Angle As Double) As Boolean
        Dim sKey, sData As String

        sKey = eSelection.ToString()
        sData = w2String.Concatenate(",", X.ToString("0.000000"), Y.ToString("0.000000"), Angle.ToString("0.000000"))

        Return mIniFile.WriteParameter("GoldImage", sKey, sData)
    End Function

    Public Function ReadGoldImgaeOffset(ByVal eSelection As BlackHawkFunction.InstrumentUtility.CcdViewIndex, ByRef X As Double, ByRef Y As Double, ByRef Angle As Double) As Boolean
        Dim sKey, s, sData() As String

        sKey = eSelection.ToString()
        s = mIniFile.ReadParameter("GoldImage", sKey, "")

        sData = s.Split(","c)
        Try
            X = Double.Parse(sData(0))
            Y = Double.Parse(sData(1))
            Angle = Double.Parse(sData(2))

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    'there is only read here. User write back is handled in the GUI write back
    Public Sub ReadParameters()
        Dim s, section As String
        Dim v As Double

        'because we are modifying the data in another GUI, we will force a new read from file, otherwide, the XML is still in the buffer and does not get updated
        mIniFile = New w2.w2IniFileXML(mIniFile.FileName)

        'laser diode
        section = "LaserDiode"
        mLaserDiode.DefaultCurrent = mIniFile.ReadParameter(section, "DefaultCurrent", 85.0)
        mLaserDiode.MinCurrent = mIniFile.ReadParameter(section, "MinCurrent", 85.0)
        mLaserDiode.MaxCurrent = mIniFile.ReadParameter(section, "MaxCurrent", 85.0)
        mLaserDiode.CurrentStep = mIniFile.ReadParameter(section, "CurrentStep", 5.0)
        mLaserDiode.MinVoltage = mIniFile.ReadParameter(section, "MinVoltage", 1.0)
        mLaserDiode.MaxCurrentError = mIniFile.ReadParameter(section, "MaxCurrentError", 10.0)

        'beam scan
        section = "BeamScan"
        mBeamScan.Gain = mIniFile.ReadParameter(section, "Gain", 0.0)
        mBeamScan.SampleFrequency = mIniFile.ReadParameter(section, "SampleFrequency", 10.0)
        mBeamScan.SampleResolution = mIniFile.ReadParameter(section, "SampleResolution", 1.0)
        mBeamScan.Samples = mIniFile.ReadParameter(section, "Samples", 5)

        'light source
        section = "LightSource1"
        mLightSource1.Channel1Value = CInt(mIniFile.ReadParameter(section, "Channel1", 0.0))
        mLightSource1.Channel2Value = CInt(mIniFile.ReadParameter(section, "Channel2", 0.0))
        section = "LightSource2"
        mLightSource2.Channel1Value = CInt(mIniFile.ReadParameter(section, "Channel1", 0.0))
        mLightSource2.Channel2Value = CInt(mIniFile.ReadParameter(section, "Channel2", 0.0))

        'probe clamp
        section = "ProbeClamp"
        mProbeClamp.Force = mIniFile.ReadParameter(section, "Force", 50.0)
        mProbeClamp.Speed = mIniFile.ReadParameter(section, "Speed", 60.0)
        mProbeClamp.OpenPosition = mIniFile.ReadParameter(section, "OpenPosition", 0.0)

        'vacuum CDA
        section = "VacuumCda"
        mVacuumCda.MinVacuumChange = mIniFile.ReadParameter(section, "MinVacuumChange", 50.0)

        'zTouch
        section = "zTouchSense"
        mzTouchSense.Samples = mIniFile.ReadParameter(section, "Samples", 1)
        mzTouchSense.ForceChangeThreshold = mIniFile.ReadParameter(section, "ForceChangeThreshold", 25.0)

        mzTouchSense.Velocity = mIniFile.ReadParameter(section, "Velocity", 1.0)
        mzTouchSense.StepSize = mIniFile.ReadParameter(section, "StepSize", 0.005)
        mzTouchSense.MaxMove = mIniFile.ReadParameter(section, "MaxMove", 0.1)

        mzTouchSense.BondLineBS = mIniFile.ReadParameter(section, "BondLineBS", 0.01)
        mzTouchSense.BondLineLensMin = mIniFile.ReadParameter(section, "BondLineLensMin", 0.01)
        mzTouchSense.BondLineLensMax = mIniFile.ReadParameter(section, "BondLineLensMax", 0.07)

        'zSlowMove
        section = "zSlowMove"
        mzSlowMove.StepCount = mIniFile.ReadParameter(section, "StepCount", 10)
        mzSlowMove.StepDelay = mIniFile.ReadParameter(section, "StepDelay", 100)
        mzSlowMove.StepSize = mIniFile.ReadParameter(section, "StepSize", 0.05)
        mzSlowMove.Velocity = mIniFile.ReadParameter(section, "Velocity", 1.0)

        'alignment
        section = "Alignment"

        '---------------------------------------------------------------------
        'the following are alignment "gain" coef, we will use a ring buffer to contain the data
        mBufferSize = mIniFile.ReadParameter(section, "RingBufferSize", 5)

        v = mIniFile.ReadParameter(section, "FocalLengthY", 110.0)
        mCoef(AlignCoefEnum.LensY) = New Queue(Of Double)
        mCoef(AlignCoefEnum.LensY).Enqueue(v)
        v = mIniFile.ReadParameter(section, "FocalLengthZ", 150.0)
        mCoef(AlignCoefEnum.LensZ) = New Queue(Of Double)
        mCoef(AlignCoefEnum.LensZ).Enqueue(v)
        'added by Ming
        v = mIniFile.ReadParameter(section, "FocalLengthfineY", 500.0)
        mCoef(AlignCoefEnum.LensfineY) = New Queue(Of Double)
        mCoef(AlignCoefEnum.LensfineY).Enqueue(v)

        v = mIniFile.ReadParameter(section, "FocalLengthfineZ", 750.0)
        mCoef(AlignCoefEnum.LensfineZ) = New Queue(Of Double)
        mCoef(AlignCoefEnum.LensfineZ).Enqueue(v)

        v = mIniFile.ReadParameter(section, "FocalLengthY_overlap", 600.0)
        mCoef(AlignCoefEnum.lensYoverlap) = New Queue(Of Double)
        mCoef(AlignCoefEnum.lensYoverlap).Enqueue(v)

        v = mIniFile.ReadParameter(section, "FocalLengthZ_overlap", 600.0)
        mCoef(AlignCoefEnum.lensZoverlap) = New Queue(Of Double)
        mCoef(AlignCoefEnum.lensZoverlap).Enqueue(v)


        v = mIniFile.ReadParameter(section, "AngleCoef", 10.0)
        mCoef(AlignCoefEnum.PbsAngle) = New Queue(Of Double)
        mCoef(AlignCoefEnum.PbsAngle).Enqueue(v)
        '-----------------------------------------------------------------------

        mAlignment.BeamScanXOffset = mIniFile.ReadParameter(section, "BeamScanXOffset", 55.0)
        s = mIniFile.ReadParameter(section, "BeamScanXList", "")
        mAlignment.BeamScanXList = w2String.ExtractDecimalNumbersAsDouble(s)

        mAlignment.MaxPitchError = mIniFile.ReadParameter(section, "MaxPitchError", 0.01)
        mAlignment.MaxSteeringY = mIniFile.ReadParameter(section, "MaxSteeringY", 0.01)
        mAlignment.MaxSteeringZ = mIniFile.ReadParameter(section, "MaxSteeringZ", 0.01)
        'Added by Ming
        mAlignment.MinSteeringY = mIniFile.ReadParameter(section, "MinSteeringY", 0.005)
        mAlignment.MinSteeringZ = mIniFile.ReadParameter(section, "MinSteeringZ", 0.005)

        mAlignment.MinPower = mIniFile.ReadParameter(section, "MinPower", 1.0)
        mAlignment.MaxPower = mIniFile.ReadParameter(section, "MaxPower", 5.0)
        mAlignment.MinYY = mIniFile.ReadParameter(section, "MinYY", 4400.0)
        mAlignment.MaxYY = mIniFile.ReadParameter(section, "MaxYY", 4600.0)
        mAlignment.MinZZ = mIniFile.ReadParameter(section, "MinZZ", 4400.0)
        mAlignment.MaxZZ = mIniFile.ReadParameter(section, "MaxZZ", 4600.0)

        mAlignment.StageSpeed = mIniFile.ReadParameter(section, "StageSpeed", 1.0)
        mAlignment.StageStepX = mIniFile.ReadParameter(section, "StageStepX", 0.001)
        mAlignment.StageStepFineX = mIniFile.ReadParameter(section, "StageStepFineX", 0.0001)
        mAlignment.StageStepXforEnergy = mIniFile.ReadParameter(section, "StageStepXforEnergy", 0.005)
        mAlignment.StageStepFineXforEnergy = mIniFile.ReadParameter(section, "StageStepFineXforEnergy", 0.001)

        mAlignment.MaxStageMoveX = mIniFile.ReadParameter(section, "MaxStageMoveX", 0.1)
        mAlignment.MaxStageMoveY = mIniFile.ReadParameter(section, "MaxStageMoveY", 0.1)
        mAlignment.MaxStageMoveZ = mIniFile.ReadParameter(section, "MaxStageMoveZ", 0.1)
        mAlignment.MaxStageMoveAngle = mIniFile.ReadParameter(section, "MaxStageMoveAngle", 1.5)

        mAlignment.PostEpoxyStepSize = mIniFile.ReadParameter(section, "PostEpoxyStepSize", 0.0001)
        mAlignment.postEpoxyMaxMove = mIniFile.ReadParameter(section, "PostEpoxyMaxMove", 0.002)

        mAlignment.bsLength = mIniFile.ReadParameter(section, "BsLength", 0.65)

        mAlignment.CcdAngle = mIniFile.ReadParameter(section, "CcdAngle", 1.5)

        mAlignment.PinOffsetX = mIniFile.ReadParameter(section, "PinOffsetX", 1.5)
        mAlignment.PinOffsetY = mIniFile.ReadParameter(section, "PinOffsetY", 1.5)
        mAlignment.PinOffsetZ = mIniFile.ReadParameter(section, "PinOffsetZ", 1.5)

    End Sub

End Class
