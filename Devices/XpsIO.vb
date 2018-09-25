Option Explicit On
Option Strict On
Option Infer Off

Public Class XpsIO
    Private mXPS As Instrument.iXPS

    Private Structure VacuumChannelList
        Public GainChip As Integer
        Public CollimatingLens As Integer
        Public AdjustmentLens As Integer
        Public Spare As Integer
    End Structure

    Private Structure LanternChannelList
        Public Running As Integer
        Public Fail As Integer
        Public Standby As Integer
    End Structure

    Private Structure XpsIoChannels
        Public DigitalIoPort As Integer

        Public VacuumDigitalChannel As VacuumChannelList
        Public VacuumAnalogyChannel As VacuumChannelList
        Public PowerMeterChannel As Integer
        Public DoorInterlock As Integer
        Public Relay1 As Integer    'added by Alex on Jan 16th
        Public Relay2 As Integer    'added by Alex on Jan 16th
        Public SparePneumatic As Integer    'added by Alex on Jan 16th

        Public AlarmIOPort As Integer    'added by Alex on Jan 16th
        Public LanternChannel As LanternChannelList    'added by Alex on Jan 16th
    End Structure

    Private mPara As XpsIoChannels

    Public Function Initialize(ByRef hXPS As Instrument.iXPS, ByRef hConfig As w2.w2IniFileXML) As Boolean
        Const section As String = "XpsIoTable"

        mXPS = hXPS

        'read the parameter
        mPara.DigitalIoPort = hConfig.ReadParameter(section, "DigitalIoPort", 2)
        mPara.PowerMeterChannel = hConfig.ReadParameter(section, "PowerMeter", 1)
        mPara.DoorInterlock = hConfig.ReadParameter(section, "DoorInterlock", 1)
        mPara.Relay1 = hConfig.ReadParameter(section, "Relay1", 1)
        mPara.Relay2 = hConfig.ReadParameter(section, "Relay2", 2)
        mPara.SparePneumatic = hConfig.ReadParameter(section, "SparePneumatic", 8)

        'note that digit IO channel is zero based internally
        mPara.VacuumDigitalChannel.GainChip = hConfig.ReadParameter(section, "GainChip", 2)
        mPara.VacuumDigitalChannel.CollimatingLens = hConfig.ReadParameter(section, "CollimatingLens", 4)
        mPara.VacuumDigitalChannel.AdjustmentLens = hConfig.ReadParameter(section, "AdjustmentLens", 5)
        mPara.VacuumDigitalChannel.Spare = hConfig.ReadParameter(section, "VacuumSpare", 3)

        mPara.VacuumAnalogyChannel.GainChip = hConfig.ReadParameter(section, "GainChipVacuumLevel", 2)
        mPara.VacuumAnalogyChannel.CollimatingLens = hConfig.ReadParameter(section, "CollimatingLensVacuumLebel", 4)
        mPara.VacuumAnalogyChannel.AdjustmentLens = hConfig.ReadParameter(section, "AdjustmentLensVacuumLebel", 5)
        mPara.VacuumAnalogyChannel.Spare = hConfig.ReadParameter(section, "SpareVacuumLevel", 3)

        mPara.AlarmIOPort = hConfig.ReadParameter(section, "AlarmPort", 3)
        mPara.LanternChannel.Running = hConfig.ReadParameter(section, "Running", 1)
        mPara.LanternChannel.Fail = hConfig.ReadParameter(section, "Fail", 2)
        mPara.LanternChannel.Standby = hConfig.ReadParameter(section, "Standby", 3)

        Return True
    End Function

    Public ReadOnly Property DoorClosed() As Boolean
        Get
            Return mXPS.DigitInput(mPara.DigitalIoPort, mPara.DoorInterlock)
        End Get
    End Property

    ' pneumatic power meter is inversed on Mar 26th
    Public Property PneumaticPowerMeter() As Boolean
        Get
            Return mXPS.DigitOutput(mPara.DigitalIoPort, mPara.PowerMeterChannel - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.PowerMeterChannel - 1) = value
        End Set
    End Property

    Public ReadOnly Property VacuumGainChipReadBack() As Boolean
        Get
            Return mXPS.DigitInput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.GainChip - 2)
        End Get
    End Property

    Public Property VacuumGainChip() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.GainChip - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.GainChip - 1) = Not value
        End Set
    End Property

    Public ReadOnly Property VacuumSpareReadBack() As Boolean
        Get
            Return mXPS.DigitInput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.Spare - 2)
        End Get
    End Property

    Public Property VacuumSpare() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.Spare - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.Spare - 1) = Not value
        End Set
    End Property

    Public ReadOnly Property VacuumCollimatingLensReadback() As Boolean
        Get
            Return mXPS.DigitInput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.CollimatingLens - 2)
        End Get
    End Property

    Public Property VacuumPart() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.CollimatingLens - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.CollimatingLens - 1) = Not value
        End Set
    End Property

    Public ReadOnly Property VacuumFeedbackPart() As Boolean
        Get
            Return mXPS.DigitInput(mPara.DigitalIoPort, mPara.VacuumDigitalChannel.AdjustmentLens - 2)
        End Get
    End Property


    Public Property Relay1() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.Relay1 - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.Relay1 - 1) = Not value
        End Set
    End Property

    Public Property Relay2() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.Relay2 - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.Relay2 - 1) = Not value
        End Set
    End Property

    Public Property SparePneumatic() As Boolean
        Get
            Return Not mXPS.DigitOutput(mPara.DigitalIoPort, mPara.SparePneumatic - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.DigitalIoPort, mPara.SparePneumatic - 1) = Not value
        End Set
    End Property

    Public ReadOnly Property VacuumLevelGainChip() As Double
        Get
            Return Me.GetVacuumPressure(mPara.VacuumAnalogyChannel.GainChip)
        End Get
    End Property

    Public ReadOnly Property VacuumLevelSpare() As Double
        Get
            Return Me.GetVacuumPressure(mPara.VacuumAnalogyChannel.Spare)
        End Get
    End Property

    Public ReadOnly Property VacuumLevelCollimatingLens() As Double
        Get
            Return Me.GetVacuumPressure(mPara.VacuumAnalogyChannel.CollimatingLens)
        End Get
    End Property

    Public ReadOnly Property VacuumLevelAdjustmentLens() As Double
        Get
            Return Me.GetVacuumPressure(mPara.VacuumAnalogyChannel.AdjustmentLens)
        End Get
    End Property

    Private Function GetVacuumPressure(ByVal Channel As Integer) As Double
        Dim V As Double

        Const V1 As Double = 1.0
        Const V2 As Double = 5.0
        Const P1 As Double = -0.23
        Const P2 As Double = -103.38

        'convert analogy voltage to pressure in kPa 
        V = mXPS.ReadAnalogyInput(Channel)

        V -= V1
        V /= (V2 - V1)
        V *= (P2 - P1)
        V += P1

        Return V
    End Function

#Region "post light"
    Public Sub SetFailLantern()
        RunningLantern = False
        FailLantern = True
        StandbyLantern = False
    End Sub

    Public Sub SetRunningLantern()
        RunningLantern = True
        FailLantern = False
        StandbyLantern = False
    End Sub

    Public Sub SetStandbyLantern()
        RunningLantern = False
        FailLantern = False
        StandbyLantern = True
    End Sub

    Private Property RunningLantern() As Boolean  'added by Alex on Jan 16th
        Get
            Return mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Running - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Running - 1) = value
        End Set
    End Property

    Private Property FailLantern() As Boolean  'added by Alex on Jan 16th
        Get
            Return mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Fail - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Fail - 1) = value
        End Set
    End Property

    Private Property StandbyLantern() As Boolean  'added by Alex on Jan 16th
        Get
            Return mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Standby - 1)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara.AlarmIOPort, mPara.LanternChannel.Standby - 1) = value
        End Set
    End Property
#End Region

End Class
