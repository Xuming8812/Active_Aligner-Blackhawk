Option Explicit On
Option Strict On

Namespace Instrument
    Public Class iXPS
        Inherits iMotionController

        '0 for debug and 1 for release
        'Const Flag As Integer = 0
        Const Flag As Integer = 1
#Region "XPS_C8 API"
        Private Declare Function TCP_ConnectToServer Lib "XPS_C8_drivers.dll" (ByVal Ip_Address As String, ByVal Ip_Port As Integer, ByVal TimeOut As Double) As Integer

        Private Declare Sub TCP_SetTimeout Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TimeOut As Double)

        Private Declare Sub TCP_CloseSocket Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer)
        Private Declare Function TCP_GetError Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer) As String

        Private Declare Function GetLibraryVersion Lib "XPS_C8_drivers.dll" () As String

        Private Declare Function ErrorStringGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ErrorCode As Integer, ByVal ErrorString As String) As Integer

        Private Declare Function FirmwareVersionGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal Version As String) As Integer

        Private Declare Function ElapsedTimeGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ElapsedTime As Double) As Integer

        Private Declare Function TCLScriptExecute Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TCLFileName As String, ByVal TaskName As String, ByVal ParametersList As String) As Integer
        Private Declare Function TCLScriptExecuteAndWait Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TCLFileName As String, ByVal TaskName As String, ByVal InputParametersList As String, ByVal OutputParametersList As String) As Integer
        Private Declare Function TCLScriptKill Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TaskName As String) As Integer

        Private Declare Function TimerGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TimerName As String, ByVal FrequencyTicks As Integer) As Integer
        Private Declare Function TimerSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TimerName As String, ByVal FrequencyTicks As Integer) As Integer

        Private Declare Function Reboot Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer) As Integer

        Private Declare Function EventAdd Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal EventName As String, ByVal EventParameter As String, ByVal ActionName As String, ByVal ActionParameter1 As String, ByVal ActionParameter2 As String, ByVal ActionParameter3 As String) As Integer
        Private Declare Function EventGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal EventsAndActionsList As String) As Integer
        Private Declare Function EventRemove Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal EventName As String, ByVal EventParameter As String) As Integer
        Private Declare Function EventWait Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal EventName As String, ByVal EventParameter As String) As Integer

        Private Declare Function GatheringConfigurationGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TypeName As String) As Integer
        Private Declare Function GatheringConfigurationSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal TypeNameList As String) As Integer
        Private Declare Function GatheringCurrentNumberGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal CurrentNumber As Integer, ByVal MaximumSamplesNumber As Integer) As Integer
        Private Declare Function GatheringStopAndSave Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer) As Integer
        Private Declare Function GatheringExternalConfigurationSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal TypeNameList As String) As Integer
        Private Declare Function GatheringExternalConfigurationGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal TypeName As String) As Integer
        Private Declare Function GatheringExternalCurrentNumberGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal CurrentNumber As Integer, ByVal MaximumSamplesNumber As Integer) As Integer
        Private Declare Function GatheringExternalStopAndSave Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer) As Integer
        Private Declare Function GlobalArrayGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal Number As Integer, ByVal ValueString As String) As Integer
        Private Declare Function GlobalArraySet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal Number As Integer, ByVal ValueString As String) As Integer

        Private Declare Function GPIOAnalogGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal GPIONameList As String, ByRef AnalogValue As Double) As Integer
        Private Declare Function GPIOAnalogSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal GPIONameList As String, ByRef AnalogOutputValue As Double) As Integer
        Private Declare Function GPIOAnalogGainGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal GPIONameList As String, ByRef AnalogInputGainValue As Integer) As Integer
        Private Declare Function GPIOAnalogGainSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal NbElements As Integer, ByVal GPIONameList As String, ByRef AnalogInputGainValue As Integer) As Integer
        Private Declare Function GPIODigitalGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GPIOName As String, ByRef DigitalValue As UShort) As Integer
        Private Declare Function GPIODigitalSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GPIOName As String, ByVal Mask As UShort, ByVal DigitalOutputValue As UShort) As Integer

        Private Declare Function GroupAnalogTrackingModeEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal TypeName As String) As Integer
        Private Declare Function GroupAnalogTrackingModeDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupHomeSearch Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupHomeSearchAndRelativeMove Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal TargetDisplacement As Double) As Integer
        Private Declare Function GroupInitialize Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupInitializeWithEncoderCalibration Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupJogParametersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function GroupJogParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function GroupJogCurrentGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function GroupJogModeEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupJogModeDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupKill Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupMoveAbort Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupMoveAbsolute Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByRef TargetPosition As Double) As Integer
        Private Declare Function GroupMoveRelative Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByRef TargetDisplacement As Double) As Integer
        Private Declare Function GroupMotionDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupMotionEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function GroupPositionCurrentGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByRef CurrentEncoderPosition As Double) As Integer
        Private Declare Function GroupPositionSetpointGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal SetPointPosition As Double) As Integer
        Private Declare Function GroupPositionTargetGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal NbElements As Integer, ByVal TargetPosition As Double) As Integer
        Private Declare Function GroupStatusGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByRef Status As Integer) As Integer
        Private Declare Function GroupStatusStringGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupStatusCode As Integer, ByVal GroupStatusString As String) As Integer

        Private Declare Function KillAll Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer) As Integer

        Private Declare Function PositionerAnalogTrackingPositionParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal GPIOName As String, ByVal Offset As Double, ByVal ScaleValue As Double, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function PositionerAnalogTrackingPositionParametersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal GPIOName As String, ByVal Offset As Double, ByVal ScaleValue As Double, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function PositionerAnalogTrackingVelocityParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal GPIOName As String, ByVal Offset As Double, ByVal ScaleValue As Double, ByVal DeadBandThreshold As Double, ByVal Order As Integer, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function PositionerAnalogTrackingVelocityParametersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal GPIOName As String, ByVal Offset As Double, ByVal ScaleValue As Double, ByVal DeadBandThreshold As Double, ByVal Order As Integer, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function PositionerBacklashGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal BacklashValue As Double, ByVal BacklaskStatus As String) As Integer
        Private Declare Function PositionerBacklashSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal BacklashValue As Double) As Integer
        Private Declare Function PositionerBacklashEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String) As Integer
        Private Declare Function PositionerBacklashDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String) As Integer
        Private Declare Function PositionerCorrectorNotchFiltersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal NotchFrequency1 As Double, ByVal NotchBandwith1 As Double, ByVal NotchGain1 As Double, ByVal NotchFrequency2 As Double, ByVal NotchBandwith2 As Double, ByVal NotchGain2 As Double) As Integer
        Private Declare Function PositionerCorrectorNotchFiltersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal NotchFrequency1 As Double, ByVal NotchBandwith1 As Double, ByVal NotchGain1 As Double, ByVal NotchFrequency2 As Double, ByVal NotchBandwith2 As Double, ByVal NotchGain2 As Double) As Integer
        Private Declare Function PositionerCorrectorPIDFFAccelerationSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainAcceleration As Double) As Integer
        Private Declare Function PositionerCorrectorPIDFFAccelerationGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainAcceleration As Double) As Integer
        Private Declare Function PositionerCorrectorPIDFFVelocitySet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainVelocity As Double) As Integer
        Private Declare Function PositionerCorrectorPIDFFVelocityGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainVelocity As Double) As Integer
        Private Declare Function PositionerCorrectorPIDDualFFVoltageSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainVelocity As Double, ByVal FeedForwardGainAcceleration As Double, ByVal Friction As Double) As Integer
        Private Declare Function PositionerCorrectorPIDDualFFVoltageGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal KD As Double, ByVal KS As Double, ByVal IntegrationTime As Double, ByVal DerivativeFilterCutOffFrequency As Double, ByVal GKP As Double, ByVal GKI As Double, ByVal GKD As Double, ByVal KForm As Double, ByVal FeedForwardGainVelocity As Double, ByVal FeedForwardGainAcceleration As Double, ByVal Friction As Double) As Integer
        Private Declare Function PositionerCorrectorPIPositionSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal IntegrationTime As Double) As Integer
        Private Declare Function PositionerCorrectorPIPositionGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ClosedLoopStatus As Boolean, ByVal KP As Double, ByVal KI As Double, ByVal IntegrationTime As Double) As Integer
        Private Declare Function PositionerCorrectorTypeGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal CorrectorType As String) As Integer
        Private Declare Function PositionerCurrentVelocityAccelerationFiltersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal CurrentVelocityCutOffFrequency As Double, ByVal CurrentAccelerationCutOffFrequency As Double) As Integer
        Private Declare Function PositionerCurrentVelocityAccelerationFiltersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal CurrentVelocityCutOffFrequency As Double, ByVal CurrentAccelerationCutOffFrequency As Double) As Integer
        Private Declare Function PositionerEncoderAmplitudeValuesGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal MaxSinusAmplitude As Double, ByVal CurrentSinusAmplitude As Double, ByVal MaxCosinusAmplitude As Double, ByVal CurrentCosinusAmplitude As Double) As Integer
        Private Declare Function PositionerEncoderCalibrationParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal SinusOffset As Double, ByVal CosinusOffset As Double, ByVal DifferentialGain As Double, ByVal PhaseCompensation As Double) As Integer
        Private Declare Function PositionerErrorGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal ErrorCode As Integer) As Integer
        Private Declare Function PositionerErrorStringGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerErrorCode As Integer, ByVal PositionerErrorString As String) As Integer
        Private Declare Function PositionerHardwareStatusGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal HardwareStatus As Integer) As Integer
        Private Declare Function PositionerHardwareStatusStringGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerHardwareStatus As Integer, ByVal PositonerHardwareStatusString As String) As Integer
        Private Declare Function PositionerHardInterpolatorFactorGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal InterpolationFactor As Integer) As Integer
        Private Declare Function PositionerHardInterpolatorFactorSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal InterpolationFactor As Integer) As Integer
        Private Declare Function PositionerMaximumVelocityAndAccelerationGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByRef MaximumVelocity As Double, ByRef MaximumAcceleration As Double) As Integer
        Private Declare Function PositionerMotionDoneGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal PositionWindow As Double, ByVal VelocityWindow As Double, ByVal CheckingTime As Double, ByVal MeanPeriod As Double, ByVal TimeOut As Double) As Integer
        Private Declare Function PositionerMotionDoneSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal PositionWindow As Double, ByVal VelocityWindow As Double, ByVal CheckingTime As Double, ByVal MeanPeriod As Double, ByVal TimeOut As Double) As Integer
        Private Declare Function PositionerPositionCompareGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal MinimumPosition As Double, ByVal MaximumPosition As Double, ByVal PositionStep As Double, ByVal EnableState As Boolean) As Integer
        Private Declare Function PositionerPositionCompareSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal MinimumPosition As Double, ByVal MaximumPosition As Double, ByVal PositionStep As Double) As Integer
        Private Declare Function PositionerPositionCompareEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String) As Integer
        Private Declare Function PositionerPositionCompareDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String) As Integer
        Private Declare Function PositionerSGammaExactVelocityAjustedDisplacementGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal DesiredDisplacement As Double, ByVal AdjustedDisplacement As Double) As Integer
        Private Declare Function PositionerSGammaParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByRef Velocity As Double, ByRef Acceleration As Double, ByRef MinimumTjerkTime As Double, ByRef MaximumTjerkTime As Double) As Integer
        Private Declare Function PositionerSGammaParametersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal Velocity As Double, ByVal Acceleration As Double, ByVal MinimumTjerkTime As Double, ByVal MaximumTjerkTime As Double) As Integer
        Private Declare Function PositionerSGammaPreviousMotionTimesGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal SettingTime As Double, ByVal SettlingTime As Double) As Integer
        Private Declare Function PositionerUserTravelLimitsGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByRef UserMinimumTarget As Double, ByRef UserMaximumTarget As Double) As Integer
        Private Declare Function PositionerUserTravelLimitsSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal UserMinimumTarget As Double, ByVal UserMaximumTarget As Double) As Integer

        Private Declare Function MultipleAxesPVTVerification Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String) As Integer
        Private Declare Function MultipleAxesPVTVerificationResultGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal FileName As String, ByVal MinimumPosition As Double, ByVal MaximumPosition As Double, ByVal MaximumVelocity As Double, ByVal MaximumAcceleration As Double) As Integer
        Private Declare Function MultipleAxesPVTExecution Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal ExecutionNumber As Integer) As Integer
        Private Declare Function MultipleAxesPVTParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal CurrentElementNumber As Integer) As Integer
        Private Declare Function SingleAxisSlaveModeEnable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function SingleAxisSlaveModeDisable Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String) As Integer
        Private Declare Function SingleAxisSlaveParametersSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal PositionerName As String, ByVal Ratio As Double) As Integer
        Private Declare Function SingleAxisSlaveParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal PositionerName As String, ByVal Ratio As Double) As Integer
        Private Declare Function XYLineArcVerification Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String) As Integer
        Private Declare Function XYLineArcVerificationResultGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal FileName As String, ByVal MinimumPosition As Double, ByVal MaximumPosition As Double, ByVal MaximumVelocity As Double, ByVal MaximumAcceleration As Double) As Integer
        Private Declare Function XYLineArcExecution Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal Velocity As Double, ByVal Acceleration As Double, ByVal ExecutionNumber As Integer) As Integer
        Private Declare Function XYLineArcParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal Velocity As Double, ByVal Acceleration As Double, ByVal CurrentElementNumber As Integer) As Integer
        Private Declare Function XYZSplineVerification Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String) As Integer
        Private Declare Function XYZSplineVerificationResultGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerName As String, ByVal FileName As String, ByVal MinimumPosition As Double, ByVal MaximumPosition As Double, ByVal MaximumVelocity As Double, ByVal MaximumAcceleration As Double) As Integer
        Private Declare Function XYZSplineExecution Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal Velocity As Double, ByVal Acceleration As Double) As Integer
        Private Declare Function XYZSplineParametersGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupName As String, ByVal FileName As String, ByVal Velocity As Double, ByVal Acceleration As Double, ByVal CurrentElementNumber As Integer) As Integer
        Private Declare Function EEPROMCIESet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal CardNumber As Integer, ByVal ReferenceString As String) As Integer
        Private Declare Function EEPROMDACOffsetCIESet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PlugNumber As Integer, ByVal DAC1Offset As Double, ByVal DAC2Offset As Double) As Integer
        Private Declare Function EEPROMDriverSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PlugNumber As Integer, ByVal ReferenceString As String) As Integer
        Private Declare Function EEPROMINTSet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal CardNumber As Integer, ByVal ReferenceString As String) As Integer

        Private Declare Function CPUCoreAndBoardSupplyVoltagesGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal VoltageCPUCore As Double, ByVal SupplyVoltage1P5V As Double, ByVal SupplyVoltage3P3V As Double, ByVal SupplyVoltage5V As Double, ByVal SupplyVoltage12V As Double, ByVal SupplyVoltageM12V As Double, ByVal SupplyVoltageM5V As Double, ByVal SupplyVoltage5VSB As Double) As Integer
        Private Declare Function CPUTemperatureAndFanSpeedGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal CPUTemperature As Double, ByVal CPUFanSpeed As Double) As Integer

        Private Declare Function ActionListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ActionList As String) As Integer
        Private Declare Function ActionExtendedListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ActionList As String) As Integer
        Private Declare Function APIExtendedListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal Method As String) As Integer
        Private Declare Function APIListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal Method As String) As Integer
        Private Declare Function ErrorListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ErrorsList As String) As Integer
        Private Declare Function EventListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal EventList As String) As Integer
        Private Declare Function EventExtendedListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal EventList As String) As Integer
        Private Declare Function GatheringListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal list As String) As Integer
        Private Declare Function GatheringExtendedListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal list As String) As Integer
        Private Declare Function GatheringExternalListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal list As String) As Integer
        Private Declare Function GroupStatusListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal GroupStatusList As String) As Integer
        Private Declare Function HardwareInternalListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal InternalHardwareList As String) As Integer
        Private Declare Function HardwareDriverAndStageGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PlugNumber As Integer, ByVal DriverName As String, ByVal StageName As String) As Integer
        Private Declare Function ObjectsListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal ObjectsList As String) As Integer
        Private Declare Function PositionerErrorListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerErrorList As String) As Integer
        Private Declare Function PositionerHardwareStatusListGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal PositionerHardwareStatusList As String) As Integer
        Private Declare Function GatheringUserDatasGet Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal UserData1 As Double, ByVal UserData2 As Double, ByVal UserData3 As Double, ByVal UserData4 As Double, ByVal UserData5 As Double, ByVal UserData6 As Double, ByVal UserData7 As Double, ByVal UserData8 As Double) As Integer
        Private Declare Function TestTCP Lib "XPS_C8_drivers.dll" (ByVal SocketIndex As Integer, ByVal InputString As String, ByVal ReturnString As String) As Integer
#End Region

        Private mAdrs As String
        Private mSocketID As Integer = -1
        Private mGroup As String
        Private mPositioner As String
        Private mError As Integer = 0
        Private mTimeOut As Double

        Public Sub New(ByVal AxisCount As Integer)
            MyBase.New(AxisCount)
        End Sub

        Public Overrides Function Initialize(ByVal IPAdrs As String, ByVal RaiseError As Boolean) As Boolean
            Return Me.Initialize(IPAdrs, 5001, RaiseError)
        End Function

        Public Overloads Function Initialize(ByVal IPAdrs As String, ByVal iPort As Integer, ByVal RaiseError As Boolean) As Boolean
            'initial value for timeout - 2 seconds
            Me.TimeOut = 2

            'try connection with long timeout
            mSocketID = TCP_ConnectToServer(IPAdrs, iPort, mTimeOut)

            'error
            If (mSocketID = -1) And RaiseError Then
                MessageBox.Show("Error connecting to Newport XPS at " + IPAdrs + " Port " + iPort.ToString(), "Newport XPS")
            End If

            'passdown
            mAdrs = IPAdrs

            'setup axis group name
            Me.TimeOut = 0.5    '500ms timeout
            Me.Axis = 1

            'return
            Return (mSocketID >= 0 And Me.ControllerVersion <> "")
        End Function

        Public Overrides Sub Close()
            If mSocketID >= 0 Then TCP_CloseSocket(mSocketID)
        End Sub

        Public ReadOnly Property IPAddress() As String
            Get
                Return mAdrs
            End Get
        End Property

#Region "system error and info"
        Public Overrides ReadOnly Property ControllerVersion() As String
            Get
                Dim s As String = Space(513)
                Dim p As Integer

                mError = FirmwareVersionGet(mSocketID, s)
                If mError = 0 Then
                    p = s.IndexOf(Chr(0))
                    If p <> -1 Then
                        Return s.Substring(0, p)
                    Else
                        Return s
                    End If
                Else
                    Return Me.LastError
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StageData() As iMotionController.StageInformation
            Get
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property LastErrorCode() As Integer
            Get
                Return mError
            End Get
        End Property

        Public Overrides ReadOnly Property LastError() As String
            Get
                Dim p As Integer
                Dim s As String = Space(251)

                Select Case mError
                    Case 0
                        Return ""
                    Case -2
                        Return "Timeout"
                    Case Else
                        p = ErrorStringGet(mSocketID, mError, s)
                        If p = 0 Then
                            p = s.IndexOf(Chr(0))
                            If p <> -1 Then
                                Return s.Substring(0, p)
                            Else
                                Return s
                            End If
                        Else
                            Return "Have error and also failed to find the error string"
                        End If
                End Select
            End Get
        End Property

        Public ReadOnly Property DLLVersion() As String
            Get
                Return GetLibraryVersion()
            End Get
        End Property

        Public Property TimeOut() As Double
            Get
                TimeOut = mTimeOut
            End Get
            Set(ByVal value As Double)
                mTimeOut = value
                TCP_SetTimeout(mSocketID, mTimeOut)
            End Set
        End Property

        'using the following hardware info API cause the stage in brake state 
        'and requires system reboot.

        'Public ReadOnly Property ObjectsList() As String
        '    Get
        '        Dim s As String = Space(512)
        '        mError = ObjectsListGet(mSocketID, s)
        '        If mError = 0 Then
        '            s = s.Substring(0, s.IndexOf(Chr(0)))
        '            Return s
        '        Else
        '            Return Me.LastError
        '        End If
        '    End Get
        'End Property

        'Public ReadOnly Property HardwareInfo() As String
        '    Get
        '        Dim s As String = Space(512)
        '        mError = HardwareInternalListGet(mSocketID, s)
        '        If mError = 0 Then
        '            s = s.Substring(0, s.IndexOf(Chr(0)))
        '            Return s
        '        Else
        '            Return Me.LastError
        '        End If
        '    End Get
        'End Property

        'Public ReadOnly Property HardwareInfo(ByVal iSlot As Integer) As String
        '    Get
        '        Dim sDriver As String = Space(255)
        '        Dim sStage As String = Space(255)
        '        mError = HardwareDriverAndStageGet(mSocketID, iSlot, sDriver, sStage)
        '        If mError = 0 Then
        '            sDriver = sDriver.Substring(0, sDriver.IndexOf(Chr(0)))
        '            sStage = sStage.Substring(0, sStage.IndexOf(Chr(0)))
        '            If sDriver = "" Then
        '                Return ""
        '            Else
        '                Return sDriver & "," & sStage
        '            End If
        '        Else
        '            Return ""
        '        End If
        '    End Get
        'End Property

#End Region

#Region "Config"
        Public Overrides Property Axis() As Integer
            Get
                Return mAxis
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then Return
                If value > mAxisCount Then Return
                mAxis = value
                If Flag = 0 Then
                    mGroup = "GROUP" + mAxis.ToString()
                    mPositioner = mGroup & ".POSITIONER"
                Else
                    mGroup = "Group" + mAxis.ToString()
                    mPositioner = mGroup & ".Pos"
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property AxisName As String
            Get
                Return mGroup
            End Get
        End Property

        Public ReadOnly Property DriverDisabled() As Boolean
            Get
                Dim lStatus As Integer = Me.StatusCode()
                Return (lStatus >= 20) And (lStatus <= 36)
            End Get
        End Property

        Public Overrides Property DriveEnabled() As Boolean
            Get
                Return Me.StageReady
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    mError = GroupMotionEnable(mSocketID, mGroup)
                Else
                    mError = GroupMotionDisable(mSocketID, mGroup)
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property CurrentPosition() As Double
            Get
                Dim i As Integer
                Const MaxTrial As Integer = 10

                'somehow the position will reture the end-of-motion status code
                're-query if it is read
                Dim v As Double
                i = 0
                While True
                    mError = GroupPositionCurrentGet(mSocketID, mGroup, 1, v)
                    If mError = 0 Then
                        'If Not (v = 11.0# Or v = 12.0#) Then Return v
                        'loop if not
                        Return v
                    Else
                        i += 1
                        If i = MaxTrial Then Return Double.NaN
                    End If
                End While
            End Get
        End Property

        Public Overrides Property Velocity() As Double
            Get
                Dim v, a, tmin, tmax As Double
                mError = PositionerSGammaParametersGet(mSocketID, mPositioner, v, a, tmin, tmax)
                Return v
            End Get
            Set(ByVal value As Double)
                Dim v, a, tmin, tmax As Double
                'cap value
                v = Me.VelocityMaximum
                If value > v Then value = v
                'get parameters
                mError = PositionerSGammaParametersGet(mSocketID, mPositioner, v, a, tmin, tmax)
                If mError <> 0 Then Return
                'set new parameter
                v = value
                mError = PositionerSGammaParametersSet(mSocketID, mPositioner, v, a, tmin, tmax)
            End Set
        End Property

        Public Overrides Property VelocityMaximum() As Double
            Get
                Dim v, a As Double
                mError = PositionerMaximumVelocityAndAccelerationGet(mSocketID, mPositioner, v, a)
                If mError = 0 Then
                    Return v
                Else
                    Return Double.NaN
                End If
            End Get
            Set(ByVal value As Double)
                'do nothing
            End Set
        End Property

        Public Overrides Property Acceleration() As Double
            Get

            End Get
            Set(ByVal value As Double)
                'do nothing
            End Set
        End Property

        Public Overrides Property Deceleration() As Double
            Get
                Return Me.Acceleration
            End Get
            Set(ByVal value As Double)
                Me.Acceleration = value
            End Set
        End Property

        Public Overrides Property AccelerationMaximum() As Double
            Get
                Dim v, a As Double
                mError = PositionerMaximumVelocityAndAccelerationGet(mSocketID, mPositioner, v, a)
                If mError = 0 Then
                    Return a
                Else
                    Return Double.NaN
                End If
            End Get
            Set(ByVal value As Double)
                'do nothing
            End Set
        End Property
#End Region

#Region "Travel limits"
        Public Function GetTravelLimit(ByRef Min As Double, ByRef Max As Double) As Boolean
            mError = PositionerUserTravelLimitsGet(mSocketID, mPositioner, Min, Max)
            Return (mError = 0)
        End Function

        Public Function SetTravelLimit(ByVal Min As Double, ByVal Max As Double) As Boolean
            mError = PositionerUserTravelLimitsSet(mSocketID, mPositioner, Min, Max)
            Return (mError = 0)
        End Function
#End Region

#Region "Status"
        '------------------------------------------------------------------------------------Status Code-------------------------
        '0 Not initialized state
        '1 Not initialized state due to an emergency brake : see positioner status
        '2 Not initialized state due to an emergency stop : see positioner status
        '3 Not initialized state due to a following error during homing
        '4 Not initialized state due to a following error
        '5 Not initialized state due to an homing timeout
        '6 Not initialized state due to a motion done timeout during homing
        '7 Not initialized state due to a KillAll command
        '8 Not initialized state due to an end of run after homing
        '9 Not initialized state due to an encoder calibration error
        '10 Ready state due to an AbortMove command
        '11 Ready state from homing
        '12 Ready state from motion
        '13 Ready State due to a MotionEnable command
        '14 Ready state from slave
        '15 Ready state from jogging
        '16 Ready state from analog tracking
        '17 Ready state from trajectory
        '18 Ready state from spinning
        '20 Disable state
        '21 Disabled state due to a following error on ready state
        '22 Disabled state due to a following error during motion
        '23 Disabled state due to a motion done timeout during moving
        '24 Disabled state due to a following error on slave state
        '25 Disabled state due to a following error on jogging state
        '26 Disabled state due to a following error during trajectory
        '27 Disabled state due to a motion done timeout during trajectory
        '28 Disabled state due to a following error during analog tracking
        '29 Disabled state due to a slave error during motion
        '30 Disabled state due to a slave error on slave state
        '31 Disabled state due to a slave error on jogging state
        '32 Disabled state due to a slave error during trajectory
        '33 Disabled state due to a slave error during analog tracking
        '34 Disabled state due to a slave error on ready state
        '35 Disabled state due to a following error on spinning state
        '36 Disabled state due to a slave error on spinning state
        '37 Disabled state due to a following error on auto-tuning
        '38 Disabled state due to a slave error on auto-tuning
        '40 Emergency braking
        '41 Motor initialization state
        '42 Not referenced state
        '43 Homing state
        '44 Moving state
        '45 Trajectory state
        '46 Slave state due to a SlaveEnable command
        '47 Jogging state due to a JogEnable command
        '48 Analog tracking state due to a TrackingEnable command
        '49 Analog interpolated encoder calibrating state
        '50 Not initialized state due to a mechanical zero inconsistency during homing
        '51 Spinning state due to a SpinParametersSet command

        '63 Not initialized state due to a motor initialization error
        '64 Referencing state

        '66 Not initialized state due to a perpendicularity error homing
        '67 Not initialized state due to a master/slave error during homing
        '68 Auto-tuning state
        '69 Scaling calibration state
        '70 Ready state from auto-tuning
        '71 Not initialized state from scaling calibration
        '72 Not initialized state due to a scaling calibration error
        '73 Excitation signal generation state
        '74 Disable state due to a following error on excitation signal generation state
        '75 Disable state due to a master/slave error on excitation signal generation state
        '76 Disable state due to an emergency stop on excitation signal generation state
        '77 Ready state from excitation signal generation
        '------------------------------------------------------------------------------------Status Code-------------------------

        Public Overrides ReadOnly Property StageReady() As Boolean
            Get
                Dim lStatus As Integer = Me.StatusCode()
                Return (lStatus >= 10) And (lStatus <= 18)
            End Get
        End Property

        Public ReadOnly Property StatusCode() As Integer
            Get
                Dim lStatus As Integer
                While True
                    mError = GroupStatusGet(mSocketID, mGroup, lStatus)
                    Select Case mError
                        Case 0
                            Return lStatus
                        Case -1
                            'loop back
                        Case Else
                            Return mError
                    End Select
                End While

                Return -1
            End Get
        End Property

        Public ReadOnly Property StatusString() As String
            Get
                Dim lStatus As Integer
                Dim s As String = Space(256)

                lStatus = Me.StatusCode()
                If mError = 0 Then
                    mError = GroupStatusStringGet(mSocketID, lStatus, s)
                    If mError = 0 Then
                        Return s.Substring(0, s.IndexOf(Chr(0)))
                    Else
                        Return Me.LastError
                    End If
                Else
                    Return Me.LastError
                End If
            End Get
        End Property
#End Region

#Region "Home, Move, and more"
        Public Overrides Function InitializeMotion() As Boolean
            mError = GroupInitialize(mSocketID, mGroup)
            Return (mError = 0)
        End Function

        Public Overrides ReadOnly Property StageMoving() As Boolean
            Get
                Dim lStatus As Integer = Me.StatusCode()
                Return (lStatus = 43) Or (lStatus = 44)
            End Get
        End Property

        Public Overrides Function KillMotionAllAxis() As Boolean
            mError = KillAll(mSocketID)
            Return (mError = 0)
        End Function

        Public Overrides Function KillMotion() As Boolean
            mError = GroupKill(mSocketID, mGroup)
            Return (mError = 0)
        End Function

        Public Overrides Function HaltMotion() As Boolean
            Dim lStatus As Integer

            lStatus = Me.StatusCode()
            If mError = 0 Then
                Select Case lStatus
                    Case 44
                        'moving, then abort the motion
                        mError = GroupMoveAbort(mSocketID, mGroup)
                    Case 43
                        'homing, then kill the motion
                        Me.KillMotion()
                    Case Else
                        mError = 0
                End Select
                'timeout OK
                If mError = -2 Then mError = 0
                'emergency signal ok
                If mError = -26 Then mError = 0
            End If

            Return (mError = 0)
        End Function

        Protected Overrides Function StartMove(ByVal Method As iMotionController.MoveToTargetMethodEnum, ByVal Target As Double) As Boolean

            'check ready
            If Not Me.StageReady() Then Return False

            'move
            Select Case Method
                Case MoveToTargetMethodEnum.Absolute
                    mError = GroupMoveAbsolute(mSocketID, mGroup, 1, Target)
                Case MoveToTargetMethodEnum.Relative
                    mError = GroupMoveRelative(mSocketID, mGroup, 1, Target)
            End Select

            'return
            Return (mError = 0) Or (mError = -2)
        End Function

        Protected Overrides Function StartHome() As Boolean
            Dim lStatus As Integer

            'check for status
            lStatus = Me.StatusCode()

            'stage is ready [10-18] or NOTREF [42], proceed
            If ((lStatus >= 10) And (lStatus <= 18)) Or lStatus = 42 Then
                mError = GroupHomeSearch(mSocketID, mGroup)
                Return (mError = 0) Or (mError = -2)
            Else
                Return False
            End If

        End Function
#End Region

#Region "GPIO"
        'for digital IO, valid port is 1,2,3,4 for GPIO1, GPIO2, GPIO3, and GPIO4 connectors
        'note that the bit is zero based while the Input/Output on GPIO connector is one-based
        'so there is an offset (1) bteween bit number and the Input/Output label number
        Public Overrides ReadOnly Property DigitInput(ByVal Port As Integer) As Integer
            Get
                If Port < 1 Then Return -1
                If Port > 4 Then Return -1

                Dim Value As UShort
                mError = GPIODigitalGet(mSocketID, "GPIO" & Port & ".DI", Value)
                If mError = 0 Then
                    Return Convert.ToInt32(Value)
                Else
                    Return -1
                End If
            End Get
        End Property

        Public Overrides Property DigitOutput(ByVal Port As Integer) As Integer
            Get
                Dim v As UShort
                GPIODigitalGet(mSocketID, "GPIO" & Port & ".DO", v)
                Return v
            End Get
            Set(ByVal value As Integer)
                'we will mask all the bits for setting
                Me.SetDigitalOutput(Port, &HFF, value)
            End Set
        End Property

        'we will overload this because the XPS can set indivudal line with a mask
        Public Overloads Property DigitOutput(ByVal Port As Integer, ByVal Bit As Integer) As Boolean
            Get
                Dim v As Integer
                Dim mask As Integer
                v = Me.DigitOutput(Port)
                mask = 1 << Bit
                Return (v And mask) = mask
            End Get
            Set(ByVal value As Boolean)
                Dim Mask As Integer
                Dim Data As Integer

                Mask = (1 << Bit)
                If value Then
                    Data = (1 << Bit)
                Else
                    Data = 0
                End If

                Me.SetDigitalOutput(Port, Mask, Data)
            End Set
        End Property

        Public Function SetDigitalOutput(ByVal Port As Integer, ByVal Mask As Integer, ByVal Value As Integer) As Boolean
            'there are only 3 connectors/ports for output
            Select Case Port
                Case 1, 3, 4
                    'OK
                Case Else
                    Return False
            End Select

            mError = GPIODigitalSet(mSocketID, "GPIO" & Port & ".DO", Convert.ToUInt16(Mask), Convert.ToUInt16(Value))
            Return (mError = 0)
        End Function

#Region "range control for ADC"
        Public Enum ADCRangeEnum
            PNError = 0
            PN10V = 1
            PN05V = 2
            PN2_5V = 4
            PN1_25V = 8
        End Enum

        Public Property AIRange(ByVal Channel As Integer) As ADCRangeEnum
            Get
                If Channel < 1 Then Return ADCRangeEnum.PNError
                If Channel > 4 Then Return ADCRangeEnum.PNError

                Dim Gain As Integer
                mError = GPIOAnalogGainGet(mSocketID, 1, "GPIO2.ADC" & Channel, Gain)
                If mError = 0 Then
                    Return CType(Gain, ADCRangeEnum)
                Else
                    Return ADCRangeEnum.PNError
                End If
            End Get
            Set(ByVal value As ADCRangeEnum)
                If Channel < 1 Then Return
                If Channel > 4 Then Return

                Dim Gain As Integer = CType(value, Integer)
                mError = GPIOAnalogGainSet(mSocketID, 1, "GPIO2.ADC" & Channel, Gain)
            End Set
        End Property


#End Region

        Public Overrides Function ReadAnalogyInput(ByVal Channel As Integer) As Double
            Dim Value As Double

            If Channel < 1 Then Return Double.NaN
            If Channel > 4 Then Return Double.NaN

            mError = GPIOAnalogGet(mSocketID, 1, "GPIO2.ADC" & Channel, Value)
            If mError = 0 Then
                Return Value
            Else
                Return Double.NaN
            End If
        End Function

        Public Overrides Function SetAnalogyOutput(ByVal Channel As Integer, ByVal Value As Double) As Boolean
            If Channel < 1 Then Return False
            If Channel > 4 Then Return False

            mError = GPIOAnalogSet(mSocketID, 1, "GPIO2.DAC" & Channel, Value)
            Return (mError = 0)
        End Function
#End Region

    End Class
End Namespace
