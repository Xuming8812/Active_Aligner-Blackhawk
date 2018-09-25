Option Explicit On
Option Strict On
Option Infer Off

Partial Public Class BlackHawkFunction
    Private mStage As StageFunctions

    Public ReadOnly Property StageTool() As StageFunctions
        Get
            Return mStage
        End Get
    End Property

    Public Class StageFunctions

#Region "initialization, local"
        Private mTool As BlackHawkFunction
        Private mPara As BlackHawkParameters
        Private mMsgData As w2.w2MessageService
        Private mMsgInfo As w2.w2MessageService

        Private mStage As iXpsStage

        Public Sub New(ByVal hTool As BlackHawkFunction)
            mTool = hTool
            mPara = hTool.mPara
            mMsgData = hTool.mMsgData
            mMsgInfo = hTool.mMsgInfo

            If mTool.mInst.StageBase IsNot Nothing Then
                mStage = mTool.mInst.StageBase
            End If

            Me.HaveSavedPosition = False
        End Sub

        Public ReadOnly Property MessageService() As w2.w2MessageService
            Get
                Return mMsgInfo
            End Get
        End Property
#End Region

#Region "Simple Move"
        Public Function Vibration(ByVal Amount As Double) As Boolean
            Dim i As Integer

            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)
            For i = 0 To 100
                mStage.SetStageAccerleration(iXpsStage.AxisNameEnum.StageZ, 1000)
                mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, 300)
                Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Amount, False)
                Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, 0.0, False)
            Next


            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            Return True


        End Function



        Public Function MoveStage1Axis(ByVal sAxis As String, ByVal sMethod As String, ByVal Amount As Double) As Boolean
            Dim eAxis As iXpsStage.AxisNameEnum
            Dim eMethod As Instrument.iMotionController.MoveToTargetMethodEnum

            Try
                eAxis = CType(sAxis, iXpsStage.AxisNameEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Invalid stage axis selection : " + sAxis + "Error: " + ex.Message)
                Return False
            End Try

            Try
                eMethod = CType(sMethod, Instrument.iMotionController.MoveToTargetMethodEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Invalid stage move methid: " + sMethod + "Error: " + ex.Message)
                Return False
            End Try

            Return Me.MoveStage1Axis(eAxis, eMethod, Amount, True)
        End Function

        Public Function GetCurrentPosition(ByVal Axis As iXpsStage.AxisNameEnum) As Double
            Return mStage.GetStagePosition(Axis)
        End Function

        Public Function MoveStage1Axis(ByVal Axis As iXpsStage.AxisNameEnum, ByVal Method As Instrument.iMotionController.MoveToTargetMethodEnum, _
                                       ByVal Amount As Double, ByVal ShowDetail As Boolean) As Boolean
            Dim s, StageName As String
            Dim CurrentPosition, TargetPosition As Double
            Dim success As Boolean

            'ack
            StageName = mStage.GetStageName(Axis)
            s = "    Moving " + StageName
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    s += " to target position " + Amount.ToString("0.0000")
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    s += " relative by " + Amount.ToString("0.0000")
            End Select
            mMsgData.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgData.PostMessage(s)

            'get positions
            CurrentPosition = mStage.GetStagePosition(Axis)

            'check travel limit
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    TargetPosition = Amount
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    TargetPosition = Amount + CurrentPosition
            End Select
            success = Me.ValidateStagePosition(Axis, TargetPosition)
            If Not success Then Return False

            'check safety
            'success = mStage.IsMoveSafe(Axis, CurrentPosition, TargetPosition)
            'If Math.Abs(CurrentPosition - TargetPosition) > 0.2 And Not success Then
            '    s = String.Format("X   Direct move from {0:0.0000} to {1:0.0000} is not safe", CurrentPosition, TargetPosition)
            '    s += ControlChars.CrLf + "      Please move the Z/Package down first to make room for the move!"
            '    mMsgData.PostMessage(s)
            '    Return False
            'End If

            'move stage now
            success = mStage.MoveStageNoWait(Axis, Method, Amount)
            If Not success Then
                Me.ShowStageError(StageName)
                Return False
            End If

            'wait stage move to finish
            success = Me.WaitStageToStop("Wait " + StageName + " to finish the move", ShowDetail)
            If Not success Then Return False

            'done
            Return True
        End Function

        Public Function MoveStageBySteps(ByVal sAxis As String, ByVal Steps As Double, ByVal Amount As Double) As Boolean
            Dim eAxis As iXpsStage.AxisNameEnum
            Dim i As Integer
            Try
                eAxis = CType(sAxis, iXpsStage.AxisNameEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Invalid stage axis selection : " + sAxis + "Error: " + ex.Message)
                Return False
            End Try

            For i = 0 To CInt(Amount) - 1
                Me.MoveStage1Axis(eAxis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, Steps, True)
            Next

            Return True

        End Function

        Public Function MoveStage3Axis(ByVal PositionX As Double, ByVal PositionY As Double, ByVal PositionZ As Double, ByVal PauseForVisualCheck As Boolean) As Boolean
            Dim s, fmt As String
            Dim X0, Y0, Z0, BeamX, safe As Double
            Dim success As Boolean

            'ack
            fmt = "      Target position X, Y, Z = ({0:0.0000}, {1:0.0000}, {2:0.0000})"
            s = String.Format(fmt, PositionX, PositionY, PositionZ)
            mMsgData.PostMessage(s)

            'recover speed
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageX, 20)
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, 20)
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, 20)


            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageX)
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageY)
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            'get current position
            X0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            Y0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            Z0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            BeamX = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)

            safe = 250

            If BeamX > safe Then
                fmt = "      Move NanoScan to {0:0.0000} to be safe"
                s = String.Format(fmt, safe)
                mMsgData.PostMessage(s)
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, safe, False)
                If Not success Then Return False
                System.Threading.Thread.Sleep(300)
            End If


            'raise Z first
            safe = mStage.ZforSafeMove
            If Z0 > safe Then
                fmt = "      RAISE Z to {0:0.0000} to be safe"
                s = String.Format(fmt, safe)
                mMsgData.PostMessage(s)
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, safe, False)
                If Not success Then Return False
                System.Threading.Thread.Sleep(300)
            End If



            'Move Y back next
            'safe = mStage.YforSafeMove
            'If Y0 > safe Then
            '    fmt = "      Move Y back to {0:0.0000} to be safe"
            '    s = String.Format(fmt, safe)
            '    mMsgData.PostMessage(s)
            '    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, safe)
            '    If Not success Then
            '        Me.ShowStageError("Stage Y")
            '        Return False
            '    End If
            '    success = Me.WaitStageToStop("Wait Stage Y to finish the move", True)
            '    If Not success Then Return False
            '    System.Threading.Thread.Sleep(300)
            'End If

            'move X to target - use base function to skip the safety check
            mMsgData.PostMessage("      Move X to the target")
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, PositionX)
            If Not success Then
                Me.ShowStageError("Stage X")
                Return False
            End If
            success = Me.WaitStageToStop("Wait Stage X to finish the move", True)
            If Not success Then Return False
            System.Threading.Thread.Sleep(300)

            'move Y to target - use base function to skip the safety check
            mMsgData.PostMessage("      Move Y to the target")
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, PositionY)
            If Not success Then
                Me.ShowStageError("Stage Y")
                Return False
            End If
            success = Me.WaitStageToStop("Wait Stage Y to finish the move", True)
            If Not success Then Return False
            System.Threading.Thread.Sleep(300)

            'move z half way for check
            'If PauseForVisualCheck AndAlso PositionZ > mStage.ZforVisualCheck Then
            '    mMsgData.PostMessage("      Move Z to the visual check position")

            '    safe = mStage.ZforVisualCheck
            '    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, safe, False)
            '    If Not success Then Return False

            '    'add some wait here for the stage to stable
            '    'park here for a while so that XY and really stable
            '    mTool.WaitForTime(1.0, "Wait for XY stage stable!")

            '    success = Me.ConfirmVisualCheck(New iXpsStage.Position3D(PositionX, PositionY, PositionZ))
            '    If Not success Then Return False
            'End If

            'park here for a while so that XY and really stable
            mTool.WaitForTime(1.0, "Wait for XY stage stable!")

            'move Z back to target
            mMsgData.PostMessage("      Move Z to the target")
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, PositionZ, False)
            If Not success Then Return False

            'get final position
            System.Threading.Thread.Sleep(500)
            X0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            Y0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            Z0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            fmt = "       Final position X, Y, Z = ({0:0.0000}, {1:0.0000}, {2:0.0000})"
            s = String.Format(fmt, X0, Y0, Z0)
            mMsgData.PostMessage(s)

            'done
            Return True
        End Function

        Public Function MoveHexapod1Axis(ByVal Axis As Integer, ByVal Method As Instrument.iMotionController.MoveToTargetMethodEnum, _
                                      ByVal Amount As Double, Optional ByVal ShowDetail As Boolean = True) As Boolean
            Dim s, StageName As String
            Dim CurrentPosition, TargetPosition As Double
            Dim success As Boolean

            'ack
            mStage.PiHexapod.Axis = Axis
            StageName = "Hexapod " + mStage.PiHexapod.AxisName
            s = "    Moving " + StageName
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    s += " to target position " + Amount.ToString("0.0000")
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    s += " relative by " + Amount.ToString("0.0000")
            End Select
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgInfo.PostMessage(s)

            'get positions
            CurrentPosition = mStage.PiHexapod.CurrentPosition

            'check travel limit
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    TargetPosition = Amount
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    TargetPosition = Amount + CurrentPosition
            End Select
            'success = Me.ValidateStagePosition(Axis, TargetPosition)
            'If Not success Then Return False

            'check safety
            'success = mStage.IsMoveSafe(Axis, CurrentPosition, TargetPosition)
            'If Math.Abs(CurrentPosition - TargetPosition) > 0.2 And Not success Then
            '    s = String.Format("X   Direct move from {0:0.0000} to {1:0.0000} is not safe", CurrentPosition, TargetPosition)
            '    s += ControlChars.CrLf + "      Please move the Z/Package down first to make room for the move!"
            '    mMsgData.PostMessage(s)
            '    Return False
            'End If

            'move stage now
            success = mStage.PiHexapod.MoveAndWait(Method, Amount)
            If Not success Then
                Me.ShowStageError(StageName)
                Return False
            End If

            'done
            Return True
        End Function

        Public Function MovePiLS(ByVal Method As Instrument.iMotionController.MoveToTargetMethodEnum, _
                                     ByVal Amount As Double, Optional ByVal ShowDetail As Boolean = True) As Boolean
            Dim s, StageName As String
            Dim CurrentPosition, TargetPosition As Double
            Dim success As Boolean

            'ack
            StageName = "PiLS "
            s = "    Moving " + StageName
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    s += " to target position " + Amount.ToString("0.0000")
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    s += " relative by " + Amount.ToString("0.0000")
            End Select
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgInfo.PostMessage(s)

            'get positions
            CurrentPosition = mStage.PiLS.CurrentPosition

            'check travel limit
            Select Case Method
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Absolute
                    TargetPosition = Amount
                Case Instrument.iMotionController.MoveToTargetMethodEnum.Relative
                    TargetPosition = Amount + CurrentPosition
            End Select
            'success = Me.ValidateStagePosition(Axis, TargetPosition)
            'If Not success Then Return False

            'check safety
            'success = mStage.IsMoveSafe(Axis, CurrentPosition, TargetPosition)
            'If Math.Abs(CurrentPosition - TargetPosition) > 0.2 And Not success Then
            '    s = String.Format("X   Direct move from {0:0.0000} to {1:0.0000} is not safe", CurrentPosition, TargetPosition)
            '    s += ControlChars.CrLf + "      Please move the Z/Package down first to make room for the move!"
            '    mMsgData.PostMessage(s)
            '    Return False
            'End If

            'move stage now
            success = mStage.PiLS.MoveAndWait(Method, Amount)
            If Not success Then
                s = ""
                s += ControlChars.CrLf + "X   Failed to move stage: PiLS"
                s += ControlChars.CrLf + "              Error Code: " + mStage.PiLS.LastError
                mMsgData.PostMessage(s)
                Return False
            End If

            'done
            Return True
        End Function

#End Region

#Region "alignment related move"
        'Part is only passed for display. It is not used for anything related to position
        'position is retrived based on the PartIndexInTray
        Public Function MoveForPartPickup(ByVal eStage As iXpsStage.StageEnum, ByVal Part As fPartTray.PartEnum, _
                                          ByVal PartIndexInTray As Integer, ByVal PauseForVisualCheck As Boolean) As Boolean
            Dim success As Boolean
            Dim s, fmt As String
            Dim angleOffset As Double
            Dim offset As iXpsStage.Position2D
            Dim nominal, target As iXpsStage.Position3D

            Select Case eStage
                Case iXpsStage.StageEnum.Hexapod
                    mMsgInfo.PostMessage("    Move stage for part pick up using hexapod vacuum arm ... ")
                    nominal = mStage.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.HexpodPickup)
                Case iXpsStage.StageEnum.Main
                    mMsgInfo.PostMessage("    Move stage for part pick up using fixed vacuum arm ... ")
                    nominal = mStage.GetConfiguredStagePosition(iXpsStage.StagePositionEnum.LensPickup)
            End Select

            'get part position inside the tray
            offset = mStage.GetPartPositionInTray(PartIndexInTray)
            angleOffset = Me.GetCurrentPosition(iXpsStage.AxisNameEnum.AngleMain)
            If Math.Abs(angleOffset) > 0.5 Then
                Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -angleOffset, True)
            End If

            'add offset
            target.X = nominal.X + offset.X - mPara.Alignment.PartInTrayOffsetXY.X
            target.Y = nominal.Y + offset.Y - mPara.Alignment.PartInTrayOffsetXY.Y

            Select Case Part
                Case fPartTray.PartEnum.Lens
                    'Added by Ming to add the thickness of bs&pbs
                    target.Z = nominal.Z
                Case fPartTray.PartEnum.PBS
                    target.Z = nominal.Z - 0.5
                Case fPartTray.PartEnum.BS1
                    target.Z = nominal.Z - 0.5
                Case fPartTray.PartEnum.BS2
                    target.Z = nominal.Z - 0.5
            End Select
            angleOffset = Me.GetCurrentPosition(iXpsStage.AxisNameEnum.AngleMain)
            If Math.Abs(angleOffset) > 0.5 Then
                Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -angleOffset, True)
            End If

            angleOffset = mPara.Alignment.PartInTrayOffsetAngle

            'show some info
            fmt = w2String.Concatenate(ControlChars.Tab, "{0,15}", "{1,7:0.000}", "{2,7:0.000}", "{3,7:0.000}", "{4,7:0.000}")
            s = "    Pick up part from the part tray"
            s += ControlChars.CrLf + "      Idx in tray" + ControlChars.Tab + PartIndexInTray.ToString()
            s += ControlChars.CrLf + "        Part Type" + ControlChars.Tab + Part.ToString()
            s += ControlChars.CrLf + String.Format(fmt, "Position", "Nominal", " Offset1", "offset2", " Actual")
            s += ControlChars.CrLf + String.Format(fmt, "X (mm)", nominal.X, offset.X, mPara.Alignment.PartInTrayOffsetXY.X, target.X)
            s += ControlChars.CrLf + String.Format(fmt, "Y (mm)", nominal.Y, offset.Y, mPara.Alignment.PartInTrayOffsetXY.Y, target.Y)
            s += ControlChars.CrLf + String.Format(fmt, "Z (mm)", nominal.Z, "", "", target.Z)
            s += ControlChars.CrLf + String.Format(fmt, "Angle", "", "", angleOffset, "")

            mMsgInfo.PostMessage(s)
            mMsgInfo.PostMessage("")

            'move to location
            success = Me.MoveStage3Axis(target.X, target.Y, target.Z, PauseForVisualCheck)
            If Not success Then Return False

            Return True
        End Function

        Public Function MoveStageAfterPickup() As Boolean
            Dim success As Boolean
            Dim angleOffset As Double
            'move to 
            success = MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, -50, False)

            angleOffset = mPara.Alignment.PartInTrayOffsetAngle

            'make angle correction
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, angleOffset, True)
            If Not success Then Return False


            Return True
        End Function

        Public Function RotateAfterViewProcess() As Boolean
            Dim success As Boolean
            Dim angleOffset As Double
            angleOffset = mPara.Alignment.PartOffsetAngle
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, angleOffset, True)
            Return True

        End Function

        Public Function MoveStageToNamedPosition(ByVal sPosition As String, ByVal PauseForVisualCheck As Boolean) As Boolean
            Dim v As Double
            Dim s As String
            Dim PackageOffsetAngle As Double
            dim PackageOffsetXY As iXpsStage.Position2D
            Dim success, isAlignment, isEpoxy As Boolean
            Dim vPosition, vPositionAlign, offset As iXpsStage.Position3D
            Dim ePosition, ePositionAlign As iXpsStage.StagePositionEnum

            'parse position
            Try
                ePosition = CType(sPosition, iXpsStage.StagePositionEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong position identification: " + sPosition + "Error: " + ex.Message)
                Return False
            End Try

            'ack
            s = mStage.GetStagePositionLabel(ePosition)
            s = "    Move stage to " + s + " position ..."
            mMsgData.PostMessage(s)

            'flag
            isAlignment = mStage.IsAlignPosition(ePosition)
            isEpoxy = mStage.IsEpoxyPosition(ePosition)

            'we will only do visual check for alignment
            PauseForVisualCheck = PauseForVisualCheck And (isAlignment Or isEpoxy)

            'get main stage position
            vPosition = mStage.GetConfiguredStagePosition(ePosition)

            'get offset
            Select Case ePosition
                Case iXpsStage.StagePositionEnum.Lens1Align, iXpsStage.StagePositionEnum.Lens1Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(0)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(0)

                Case iXpsStage.StagePositionEnum.Lens2Align, iXpsStage.StagePositionEnum.Lens2Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(1)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(1)

                Case iXpsStage.StagePositionEnum.Lens3Align, iXpsStage.StagePositionEnum.Lens3Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(2)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(2)

                Case iXpsStage.StagePositionEnum.Lens4Align, iXpsStage.StagePositionEnum.Lens4Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(3)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(3)

                Case iXpsStage.StagePositionEnum.Bs1Align, iXpsStage.StagePositionEnum.Bs1Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(1)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(1)


                    'Add by Ming to Apply Omux Offset to Align Process

                    'PackageOffsetXY.X = mPara.Alignment.OmuxOffsetXY.X
                    'PackageOffsetXY.Y = mPara.Alignment.PackageOffsetXY(1).Y

                    'PackageOffsetAngle = 0.5 * (mPara.Alignment.PackageOffsetAngle(0) + mPara.Alignment.PackageOffsetAngle(1))
                    'PackageOffsetXY.X = 0.5 * (mPara.Alignment.PackageOffsetXY(0).X + mPara.Alignment.PackageOffsetXY(1).X)
                    'PackageOffsetXY.Y = 0.5 * (mPara.Alignment.PackageOffsetXY(0).Y + mPara.Alignment.PackageOffsetXY(1).Y)

                Case iXpsStage.StagePositionEnum.PbsAlign, iXpsStage.StagePositionEnum.PbsEpoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(1)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(1)

                    'Add by Ming to Apply Omux Offset to Align Process

                    'PackageOffsetXY.X = mPara.Alignment.OmuxOffsetXY.X
                    'PackageOffsetXY.Y = mPara.Alignment.PackageOffsetXY(1).Y


                    'PackageOffsetAngle = 0.5 * (mPara.Alignment.PackageOffsetAngle(1) + mPara.Alignment.PackageOffsetAngle(2))
                    'PackageOffsetXY.X = 0.5 * (mPara.Alignment.PackageOffsetXY(1).X + mPara.Alignment.PackageOffsetXY(2).X)
                    'PackageOffsetXY.Y = 0.5 * (mPara.Alignment.PackageOffsetXY(1).Y + mPara.Alignment.PackageOffsetXY(2).Y)

                Case iXpsStage.StagePositionEnum.Bs2Align, iXpsStage.StagePositionEnum.Bs2Epoxy
                    PackageOffsetAngle = mPara.Alignment.PackageOffsetAngle(1)
                    PackageOffsetXY = mPara.Alignment.PackageOffsetXY(1)

                    'Add by Ming to Apply Omux Offset to Align Process

                    'PackageOffsetXY.X = mPara.Alignment.OmuxOffsetXY.X
                    'PackageOffsetXY.Y = mPara.Alignment.PackageOffsetXY(1).Y


                    'PackageOffsetAngle = 0.5 * (mPara.Alignment.PackageOffsetAngle(2) + mPara.Alignment.PackageOffsetAngle(3))
                    'PackageOffsetXY.X = 0.5 * (mPara.Alignment.PackageOffsetXY(2).X + mPara.Alignment.PackageOffsetXY(3).X)
                    'PackageOffsetXY.Y = 0.5 * (mPara.Alignment.PackageOffsetXY(2).Y + mPara.Alignment.PackageOffsetXY(3).Y)
            End Select

            'add offset
            If isAlignment Then
                'totate angle first
                s = ""
                s += ControlChars.CrLf + "      Apply angle offset: package = " + PackageOffsetAngle.ToString("0.00")
                s += ControlChars.CrLf + "                             part = " + mPara.Alignment.PartOffsetAngle.ToString("0.00")
                s += ControlChars.CrLf + "      Move angle now ...  "
                mMsgData.PostMessage(s)

                v = mPara.Alignment.PartOffsetAngle - mPara.Alignment.CcdAngle
                If v > 0.5 Then
                    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, v, True)
                    If Not success Then Return False
                End If


                'add XY offset to the next move
                s = ""
                s += ControlChars.CrLf + "      Apply stage offset: package = " + PackageOffsetXY.ToString()
                s += ControlChars.CrLf + "                             part = " + mPara.Alignment.PartOffsetXY.ToString()
                mMsgData.PostMessage(s)
                vPosition.X += (-PackageOffsetXY.X - mPara.Alignment.PartOffsetXY.X)
                vPosition.Y += (-PackageOffsetXY.Y - mPara.Alignment.PartOffsetXY.Y)
            End If

            'epoxy
            If isEpoxy Then
                'angle offset is already done
                s = "      Apply angle offset is not needed for epoxy application"
                mMsgData.PostMessage(s)

                'add XY offset to the next move
                'If Me.HaveValidAlignmentPosition() Then
                '    ePositionAlign = mStage.GetAlignmentPsotionEnum(ePosition)
                '    vPositionAlign = mStage.GetConfiguredStagePosition(ePositionAlign)

                '    offset.X = (mAlignData(ProcessStepEnum.InitialAlign).StageX - vPositionAlign.X)
                '    offset.Y = (mAlignData(ProcessStepEnum.InitialAlign).StageY - vPositionAlign.Y)
                '    offset.Z = (mAlignData(ProcessStepEnum.InitialAlign).StageZ - vPositionAlign.Z)

                '    s = ""
                '    s += ControlChars.CrLf + "      Apply YXZ offset based on the alignment position"
                '    s += ControlChars.CrLf + "      Offset = " + offset.ToString()
                '    mMsgData.PostMessage(s)

                '    vPosition.X += offset.X
                '    vPosition.Y += offset.Y
                '    vPosition.Z += offset.Z

                'Else
                s = ""
                s += ControlChars.CrLf + "      Apply YZ offset based on the vision system. No alignment result found!"
                s += ControlChars.CrLf + "      Package = " + PackageOffsetXY.ToString()
                s += ControlChars.CrLf + "         Part = " + mPara.Alignment.PartOffsetXY.ToString()

                vPosition.X += (-PackageOffsetXY.X)
                vPosition.Y += (-PackageOffsetXY.Y)
                'End If

            End If

            'do move now
            success = Me.MoveStage3Axis(vPosition.X, vPosition.Y, vPosition.Z, PauseForVisualCheck)
            If Not success Then Return False

            Return success
        End Function

        Private Function HaveValidAlignmentPosition() As Boolean
            If Double.IsNaN(mAlignData(ProcessStepEnum.InitialAlign).StageX) Then Return False
            If Double.IsNaN(mAlignData(ProcessStepEnum.InitialAlign).StageY) Then Return False
            If Double.IsNaN(mAlignData(ProcessStepEnum.InitialAlign).StageZ) Then Return False
            Return True
        End Function
#End Region

#Region "Force Gauge, Z Touch"
        Public Enum zTouchType
            AlignLens = fPartTray.PartEnum.Lens
            AlighnBS = 1
            PickupParts = 2
            ApplyEpoxy
            AllOther
        End Enum

        Public Function zTouchWithForceGauge(ByVal sStage As String, ByVal sType As String) As Boolean
            Dim eStage As iXpsStage.StageEnum
            Dim eType As zTouchType

            Try
                eStage = CType(sStage, iXpsStage.StageEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong stage selection: " + sStage + "Error: " + ex.Message)
                Return False
            End Try

            Try
                eType = CType(sType, zTouchType)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong zTouch part type selection: " + sType + "Error: " + ex.Message)
                Return False
            End Try

            'do work
            Return Me.zTouchWithForceGauge(eStage, eType)
        End Function

        Public Function zTouchWithForceGauge(ByVal eStage As iXpsStage.StageEnum, ByVal eType As zTouchType) As Boolean
            Dim s, fmt As String
            Dim i, iMAx As Integer
            Dim success As Boolean
            Dim x As Instrument.iOmegaDP40
            Dim axis As iXpsStage.AxisNameEnum
            Dim Z0, Z1, dZ, Force0, Force1, dF, Ztouch As Double
            Dim zMin, zMax As Double

            Dim MinStep As Double = 0.001   '0.001 mm

            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,7:0.0000}", "{1,7:0.0000}", "{2,7:0.0}", "{3,7:0.0}", "{4}")

            'ack
            mMsgInfo.PostMessage("    Move tool down until part touch the bottom ... ")
            mMsgInfo.PostMessage(String.Format(fmt, "StageZ", "DeltaZ", "Force", "DelatF", ""))

            'get assignment
            Select Case eStage
                Case iXpsStage.StageEnum.Main
                    x = mTool.mInst.ForceGaugeMain
                    axis = iXpsStage.AxisNameEnum.StageZ
                Case iXpsStage.StageEnum.Hexapod
                    x = mTool.mInst.ForceGaugeHexapod
                    'axis = TODD
                Case Else
                    Return False
            End Select

            'initial value
            Z0 = mStage.GetStagePosition(axis)
            Force0 = mTool.mUtility.GetForceGuageReading(eStage, mPara.zTouchSense.Samples)
            mMsgInfo.PostMessage(String.Format(fmt, Z0, 0, Force0, 0, "Initial"))

            'slow down stage
            'mStage.SetStageVelocity(axis, mPara.zTouchSense.Velocity)

            'move tool down
            While True
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit While
                End If

                'move one step
                success = Me.MoveStage1Axis(axis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, mPara.zTouchSense.StepSize, False)
                If Not success Then Exit While

                'get data
                Force1 = mTool.mUtility.GetForceGuageReading(eStage, mPara.zTouchSense.Samples)
                dF = (Force1 - Force0)
                Z1 = mStage.GetStagePosition(axis)
                dZ = (Z1 - Z0)

                'show
                mMsgInfo.PostMessage(String.Format(fmt, Z1, dZ, Force1, dF, "Move arm down"))

                'check force
                If Math.Abs(dF) > mPara.zTouchSense.ForceChangeThreshold Then
                    s = "      ^ Force change {0:0.0000} > {1:0.0000} threshold"
                    s = String.Format(s, dF, mPara.zTouchSense.ForceChangeThreshold)
                    mMsgInfo.PostMessage(s)
                    success = True
                    Exit While
                End If

                'check exit - travle too large
                If Math.Abs(dZ) > mPara.zTouchSense.MaxMove Then
                    s = "X   Travel {0:0.0000} > {1:0.0000} spec max. Check stage!"
                    s = String.Format(s, dZ, mPara.zTouchSense.MaxMove)
                    mMsgInfo.PostMessage(s)
                    success = False
                    Exit While
                End If

                Force0 = Force1
            End While

            'move tool up
            If success Then
                iMAx = Convert.ToInt32(mPara.zTouchSense.StepSize / MinStep)
                i = 0
                For i = 0 To iMAx
                    'check stop
                    If mTool.CheckStop() Then
                        success = False
                        Exit For
                    End If

                    'do move up now
                    success = Me.MoveStage1Axis(axis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -MinStep, False)
                    If Not success Then Exit For

                    'get status
                    Force1 = mTool.mUtility.GetForceGuageReading(eStage, mPara.zTouchSense.Samples)
                    dF = (Force1 - Force0)
                    Z1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
                    dZ = (Z1 - Z0)

                    mMsgInfo.PostMessage(String.Format(fmt, Z1, dZ, Force1, dF, "Move arm up"))

                    'check force
                    If Math.Abs(dF) > mPara.zTouchSense.ForceChangeThreshold Then
                        s = "      ^ Force change {0:0.0000} < {1:0.0000} threshold. Found the first 1um gap"
                        s = String.Format(s, dF, mPara.zTouchSense.ForceChangeThreshold)
                        mMsgInfo.PostMessage(s)
                        success = True
                        Exit For
                    End If
                Next
            End If

            'organize result
            If success Then
                If i >= iMAx Then
                    mMsgInfo.PostMessage("!   Part is still touching. But the same position was not touching initially!")
                End If
                Ztouch = Z1
                'second, offset for the bond line
                Select Case eType
                    Case zTouchType.AlighnBS
                        'for beam splitter, we will not move it up and down, keep a uniform bond line
                        zMin = Z1 - mPara.zTouchSense.BondLineBS
                        zMax = Z1 - mPara.zTouchSense.BondLineBS

                    Case zTouchType.AlignLens
                        'for lens, we have a range of acceptance
                        zMax = Z1 - mPara.zTouchSense.BondLineLensMin
                        zMin = Z1 - mPara.zTouchSense.BondLineLensMax

                    Case zTouchType.ApplyEpoxy
                        zMin = Z1 - mPara.zTouchSense.GapForApplyEpoxy
                        zMax = Z1 - mPara.zTouchSense.GapForApplyEpoxy

                    Case zTouchType.PickupParts
                        zMin = Z1 - mPara.zTouchSense.GapForPartPickup
                        zMax = Z1 - mPara.zTouchSense.GapForPartPickup

                    Case Else
                        zMin = Z1 - mPara.zTouchSense.GapForOther
                        zMax = Z1 - mPara.zTouchSense.GapForOther

                End Select

                'move the part to the center
                Z1 = 0.5 * (zMin + zMax)
                success = Me.MoveStage1Axis(axis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z1, False)

                'final result
                If success Then
                    Force1 = mTool.mUtility.GetForceGuageReading(eStage, mPara.zTouchSense.Samples)
                    dF = (Force1 - Force0)
                    Z1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
                    dZ = (Z1 - Z0)

                    mMsgInfo.PostMessage(String.Format(fmt, Z1, dZ, Force1, dF, "At the middle of the gap/bondline control limit"))

                    mPara.UpdateStageLimitForZ(zMin, zMax, Ztouch)
                    mMsgInfo.PostMessage(String.Format(fmt, mPara.Alignment.StageMinZ, "", "", "", "Z Lo limit for alignment"))
                    mMsgInfo.PostMessage(String.Format(fmt, mPara.Alignment.StageMaxZ, "", "", "", "Z Hi limit for alignment"))
                End If

            End If

            'recover stage speed
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            'done
            Return success
        End Function
#End Region

#Region "save position and move back"
        Public HaveSavedPosition As Boolean
        Private mSavedStagePosition As iXpsStage.Position3D

        Public Function SaveCurrentStagePosition() As Boolean
            Dim s, fmt As String
            Dim height As Double


            'ack
            mMsgData.PostMessage("    Save optimized stage position for coming back")

            'read positions
            mSavedStagePosition.X = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            mSavedStagePosition.Y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            mSavedStagePosition.Z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            'read touch Z
            height = (mPara.Alignment.StageTouchZ - mSavedStagePosition.Z) * 1000


            'show it
            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,8:0.0000}", "{2,8:0.0000}")
            s = ""
            s += String.Format(fmt, "Stage X", "Stage Y", "Stage Z") + ControlChars.CrLf
            s += String.Format(fmt, mSavedStagePosition.X, mSavedStagePosition.Y, mSavedStagePosition.Z)
            mMsgData.PostMessage(s)
            mMsgInfo.PostMessage(s)
            fmt = "       The gap between aligned position and the substrate is {0:0.00}"
            s = ""
            s += String.Format(fmt, height)
            mMsgData.PostMessage(s)
            mMsgInfo.PostMessage(s)

            'flag
            Me.HaveSavedPosition = True

            Return True
        End Function

        Public Function MoveStageBackToSavedPosition(ByVal DoVisualCheck As Boolean) As Boolean
            Dim s, fmt As String
            Dim success As Boolean
            Dim x, y, z, v As Double
            Dim i As Integer

            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,8:0.0000}", "{2,8:0.0000}", "{3}")

            'ack
            mMsgData.PostMessage("    Moved back to saved positions")

            'check 
            If Not Me.HaveSavedPosition Then
                mMsgData.PostMessage("X   No position saved previously to go back")
                Return False
            End If

            'show header and the saved data
            s = String.Format(fmt, "Stage X", "Stage Y", "Stage Z", "Action")
            s += ControlChars.CrLf
            s += String.Format(fmt, mSavedStagePosition.X, mSavedStagePosition.Y, mSavedStagePosition.Z, "Saved Position, our target")
            mMsgData.PostMessage(s)

            'move stage XYZ, but Z to the safe position
            success = Me.MoveStage3Axis(mSavedStagePosition.X, mSavedStagePosition.Y, mStage.ZforSafeMove, False)
            If Not success Then Return False

            'read back - note that all the read back here are added not only for information, but also for the delay to slow down the Z move
            x = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            s = String.Format(fmt, x, y, z, "Move stage with Z offset to the safe position")
            mMsgData.PostMessage(s)

            'move stage Z up to visual check position
            If DoVisualCheck Then
                v = mStage.ZforVisualCheck
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, v, False)
                If Not success Then Return False

                x = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
                y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                s = String.Format(fmt, x, y, z, "Move Stage Z for visual check")
                mMsgData.PostMessage(s)

                'messge for visual check
                success = Me.ConfirmVisualCheck(mSavedStagePosition)
                If Not success Then Return False
            End If

            'move stage Z further up just below the saved saved
            v = mSavedStagePosition.Z - mPara.zSlowMove.StepCount * mPara.zSlowMove.StepSize
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, v, False)
            If Not success Then Return False

            System.Threading.Thread.Sleep(mPara.zSlowMove.StepDelay)

            x = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            s = String.Format(fmt, x, y, z, "Move Stage Z above package to avoid touching epoxy")
            mMsgData.PostMessage(s)

            'set Z stage to slow velocity
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.zSlowMove.Velocity)

            'move Z down slowly
            s = "        Move Z down in {0} steps at {1:0.000} mm per step"
            mMsgData.PostMessage(String.Format(s, mPara.zSlowMove.StepCount, mPara.zSlowMove.StepSize))
            For i = 1 To mPara.zSlowMove.StepCount
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, mPara.zSlowMove.StepSize, False)
                If Not success Then Return False

                System.Threading.Thread.Sleep(mPara.zSlowMove.StepDelay)

                x = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
                y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                s = String.Format(fmt, x, y, z, "Moving Stage Z down slowly, Step " + i.ToString())
                mMsgData.PostMessage(s)
            Next

            'get the final XYZ
            x = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            y = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            z = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            s = String.Format(fmt, x, y, z, v, "Final Position")
            mMsgData.PostMessage(s)

            'recover the stage velocity
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, 20)

            'done
            Return True

        End Function

        
#End Region

#Region "utility functions"
        Private Function ConfirmVisualCheck(ByVal position As iXpsStage.Position3D) As Boolean
            Dim f As fStageAdjust
            Dim s As String
            Dim r As DialogResult

            s = "Please visually check the part position to make sure it is safe to move the stage up."
            f = New fStageAdjust(mStage, s, position)

            f.ShowDialog(mTool.mParent)
            r = f.DialogResult

            If (r = DialogResult.OK) Then
                mMsgData.PostMessage("      Move Z stage up is confirmed to be safe.")
            Else
                mMsgData.PostMessage("X   User confirmed unsafe XY position for moving Z stage up!")
            End If

            f.Dispose()
            f = Nothing

            Return (r = DialogResult.OK)
        End Function

        Private Function ValidateStagePosition(ByVal axis As iXpsStage.AxisNameEnum, ByVal TargetPosition As Double) As Boolean
            Dim Min, Max As Double
            Dim s As String

            mStage.GetStageTravelLimit(axis, Min, Max)
            s = String.Format("[{0:0.000}, {1:0.000}]", Min, Max)

            If TargetPosition < Min Then
                mMsgData.PostMessage("X   Target position below the travel limit " + s)
                Return False
            ElseIf TargetPosition > Max Then
                mMsgData.PostMessage("X   Target position above the travel limit " + s)
                Return False
            Else
                Return True
            End If
        End Function

        Public Function WaitStageToStop(ByVal Text As String, ByVal ExtraWait As Boolean) As Boolean
            Dim elapse As Double
            Dim tStart As Date

            Const fmt As String = "{0}, elapse {1:0.00} second"

            'monitor the motion process
            tStart = Date.Now
            While mStage.StageMoving
                'check stop
                If mTool.CheckStop() Then
                    'halt current one
                    mStage.HaltMotion()
                    'done
                    Return False
                End If
                'relax and show data
                System.Threading.Thread.Sleep(100)
                elapse = Date.Now.Subtract(tStart).TotalSeconds
                mMsgData.PostMessage(String.Format(fmt, Text, elapse), w2.w2MessageService.MessageDisplayStyle.NewStatus)
            End While

            'add additional wait for stage to be ready
            'While Not mStage.IsStageReadyAll
            '    'relax and show data
            '    System.Threading.Thread.Sleep(100)
            '    elapse = Date.Now.Subtract(tStart).TotalSeconds
            '    mMsgData.PostMessage(String.Format(fmt, Text, elapse), w2.w2MessageService.MessageDisplayStyle.NewStatus)
            'End While

            'extra
            If ExtraWait Then System.Threading.Thread.Sleep(500)

            Return True
        End Function

        Private Sub ShowStageError(ByVal stage As String)
            Dim s As String
            s = ""
            s += ControlChars.CrLf + "X   Failed to move stage: " + stage
            s += ControlChars.CrLf + "              Error Code: " + mStage.XPSController.LastError
            mMsgData.PostMessage(s)
        End Sub
#End Region
        Public Function UnloadPart() As Boolean
            'turn of the LDD
            Dim s As String
            Dim vPosition As iXpsStage.Position3D

            Dim ePosition As iXpsStage.StagePositionEnum
            'check whether the vacuum of Lens and PBS are on
            ePosition = iXpsStage.StagePositionEnum.LoadUnload
            vPosition = mStage.GetConfiguredStagePosition(ePosition)

            s = mStage.GetStagePositionLabel(ePosition)
            s = "    Move stage to " + s + " position ..."
            mMsgData.PostMessage(s)


            'move to safe position
            'raise Z up
            Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, -50, False)
            'move X out
            Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, vPosition.X, False)
            'move Y left
            Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, vPosition.Y, False)

            'move probe up
            Me.MoveStage1Axis(iXpsStage.AxisNameEnum.Probe, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, 5, False)

            s = "Unload finished"
            mMsgData.PostMessage(s)
            MessageBox.Show("Process Finished", " ", MessageBoxButtons.OK)
            Return True
        End Function


        Public Function HomeAllStages() As Boolean
            HomeStage(iXpsStage.AxisNameEnum.StageX)
            Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, -80, False)

            HomeStage(iXpsStage.AxisNameEnum.StageY)

            HomeStage(iXpsStage.AxisNameEnum.StageZ)
            Return True
        End Function

        Private Sub HomeStage(ByVal axis As Integer)
            Dim success As Boolean

            mTool.Instruments.XPS.Axis = axis

            If mTool.Instruments.XPS.StageReady() Then Return

            success = mTool.Instruments.XPS.InitializeMotion()
            If success Then success = mTool.Instruments.XPS.HomeNoWait()

            If success Then
                Me.WaitStageToStop("Wait stage homing to finish", True)
            Else
                Me.MessageService.PostMessage("X   Motor Error: " + mTool.Instruments.XPS.LastError)
            End If
        End Sub


    End Class

End Class




