Option Explicit On
Option Strict On
Option Infer Off

Partial Public Class BlackHawkFunction
    Partial Public Class StageFunctions

        'this is 1 based
        Private Const mBaseChannel As Integer = 2

#Region "beam scan utility structure"
        'we make the Axis lebel matching the satge 

        Public Structure BeamWaistData
            Public Position As Double
            Public WidthYY As Double
            Public WidthZZ As Double
            Public FWHMYY As Double
            Public FWHMZZ As Double
            Public PeakValue As Double
            Public PeakYY As Double
            Public PeakZZ As Double

            Public Function GetTableHeader() As String
                Return w2String.Concatenate(ControlChars.Tab, "Position", " Peak Y", " Peak Z", "Width Y", "Width Z", "Peak Power")
            End Function

            Public Function GetTableData() As String
                Const fmt As String = "{0,8:0.00}" + ControlChars.Tab + "{1,8:0.00000}" + ControlChars.Tab + "{2,8:0.00000}" + ControlChars.Tab + "{3,8:0.00000}" + ControlChars.Tab + "{4,8:0.00000}" + ControlChars.Tab + "{5,8:0.00}"
                Return String.Format(fmt, Position, PeakYY, PeakZZ, WidthYY, WidthZZ, PeakValue)
            End Function
        End Structure

        Public Structure CollimatedBeamData
            Public Sub New(ByVal DataA As BeamWaistData, ByVal DataB As BeamWaistData)
                'divide peak intensity by 1000
                'DataA.PeakValue *= 0.001
                'DataB.PeakValue *= 0.001

                'note that near position actually have large stage position
                If DataA.Position > DataB.Position Then
                    DataNear = DataA
                    DataFar = DataB
                Else
                    DataNear = DataB
                    DataFar = DataA
                End If

                AveragePeakValue = 0.5 * (DataNear.PeakValue + DataFar.PeakValue)
                WidthDiffYY = DataFar.WidthYY - DataNear.WidthYY
                WidthDiffZZ = DataFar.WidthZZ - DataNear.WidthZZ

                PeakSteeringYY = DataFar.PeakYY - DataNear.PeakYY
                PeakSteeringZZ = DataFar.PeakZZ - DataNear.PeakZZ
            End Sub

            Public DataNear As BeamWaistData
            Public DataFar As BeamWaistData
            Public ReadOnly AveragePeakValue As Double
            Public ReadOnly WidthDiffYY As Double
            Public ReadOnly WidthDiffZZ As Double
            Public ReadOnly PeakSteeringYY As Double
            Public ReadOnly PeakSteeringZZ As Double
        End Structure
#End Region

#Region "Diaplay format"
        Private ReadOnly mFormatBeamData As String = w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,8:0.0000}", "{2,8:0.00}", "{3,8:0.0000}", "{4,8:0.0000}", "{5,8:0.00}", "{6,8:0.0000}", "{7,8:0.0000}")

        Private Function GetFormattedString_BeamDataHeader(ByVal AddSpace As Boolean) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            s += String.Format(mFormatBeamData, "Lens Y", "Lens Z", "NanoScan", "PeakYY", "PeakZZ", "BeamPow", "DelatYY", "DeltaZZ")
            Return s
        End Function

        Private Function GetFormattedString_BeamData(ByVal AddSpace As Boolean, ByVal LensY As Double, ByVal LensZ As Double, ByVal BeamData As BeamWaistData) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            s += String.Format(mFormatBeamData, LensY, LensZ, BeamData.Position, BeamData.PeakYY, BeamData.PeakZZ, BeamData.PeakValue, "", "")
            Return s
        End Function

        Private Function GetFormattedString_BeamData(ByVal AddSpace As Boolean, ByVal LensY As Double, ByVal LensZ As Double, ByVal BeamData As CollimatedBeamData) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            s += String.Format(mFormatBeamData, LensY, LensZ, BeamData.DataNear.Position, BeamData.DataNear.PeakYY, BeamData.DataNear.PeakZZ, BeamData.DataNear.PeakValue, "", "")
            s += ControlChars.CrLf
            If AddSpace Then s += "      "
            s += String.Format(mFormatBeamData, LensY, LensZ, BeamData.DataFar.Position, BeamData.DataFar.PeakYY, BeamData.DataFar.PeakZZ, BeamData.DataFar.PeakValue, BeamData.PeakSteeringYY, BeamData.PeakSteeringZZ)
            Return s
        End Function

        Private ReadOnly mFormatBeamOverlap As String = w2String.Concatenate(ControlChars.Tab, "{0,8:0}", "{1,3:0}", "{2,8:00000}", "{3,8:00000}", "{4,8:0.0000}", "{5,8:0.0000}", "{6,8:0.0000}", "{7,8:0.0000}", "{8,8:0.0000}")

        Private Function GetFormattedString_BeamOverlapHeader(ByVal AddSpace As Boolean) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            s += String.Format(mFormatBeamData, "Condition", "CH", "PealYY", "PeakZZ", "DeltaYY", "DeltaZZ", "StageY", "StageZ", "StangeAngle")
            Return s
        End Function

        Private Function GetFormattedString_BeamOverlapBase(ByVal AddSpace As Boolean, ByVal sCh As String, ByVal PeakYY As Double, ByVal PeakZZ As Double) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            s += String.Format(mFormatBeamData, "Aligned", sCh, PeakYY, PeakZZ, "", "", "", "", "")
            Return s
        End Function

        Private Function GetFormattedString_BeamOverlap(ByVal AddSpace As Boolean, ByVal ch As Integer, ByVal PeakYY As Double, ByVal PeakZZ As Double, ByVal DeltaYY As Double, ByVal DeltaZZ As Double, _
                                                                                                        ByVal StageY As Double, ByVal StageZ As Double, ByVal StageAngle As Double) As String
            Dim s As String
            s = ""
            If AddSpace Then s = "      "
            If Double.IsNaN(StageAngle) Then
                s += String.Format(mFormatBeamData, "Active", ch, PeakYY, PeakZZ, DeltaYY, DeltaZZ, StageY, StageZ, "")
            Else
                s += String.Format(mFormatBeamData, "Active", ch, PeakYY, PeakZZ, DeltaYY, DeltaZZ, StageY, StageZ, StageAngle)
            End If
            Return s
        End Function

#End Region

#Region "beam scan function"
        Public Function MoveBeamGage(ByVal Position As Double, ByVal ShowDetail As Boolean) As Boolean
            Dim s As String
            Dim v As Double
            Dim success As Boolean

            'ack
            s = "    Move beam gage to " + Position.ToString
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgInfo.PostMessage(s)

            'get current
            v = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)

            'we do not need high accuracy for the beam scan position
            If Math.Abs(Position - v) < 0.1 Then
                s = "      Stage already in position"
                mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
                If ShowDetail Then mMsgInfo.PostMessage(s)
                Return True
            End If

            'move stage
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Position, False)
            If Not success Then Return False

            'relax a bit to get more stable reading
            System.Threading.Thread.Sleep(300)

            Return success
        End Function

        Public Function GetBeamScanData(ByVal Position As Double, ByVal ShowDetail As Boolean, ByVal CheckData As Boolean, ByRef Data As BeamWaistData) As Boolean
            Dim success As Boolean
            Dim s, fmt As String
            Dim xData As Instrument.iBeamProfiler.SimpleData

            'ack
            s = "    Measuring beam scan data at fixed location ..."
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgInfo.PostMessage(s)

            'move stage and wait
            If Double.IsNaN(Position) Then
                'no target position, just read the current position
                Position = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)
            Else
                'move stage
                success = Me.MoveBeamGage(Position, ShowDetail)
                If Not success Then Return False
            End If

            'get xData
            If ShowDetail Then
                s = "      Get beam scan data"
                mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
                If ShowDetail Then mMsgInfo.PostMessage(s)
            End If

            'xData = mTool.mInst.NanoScan.AcquireData(10, Instrument.iBeamProfiler.DataAcquisitionMode.AllSummaryData)
            xData = mTool.mInst.NanoScan.AcquireData(mPara.BeamScan.Samples, Instrument.iBeamProfiler.DataAcquisitionMode.AllSummaryData)
            If Not xData.Valid Then
                mMsgInfo.PostMessage("X   Error reading data from beam scan. Error code = " + mTool.mInst.NanoScan.GetLastError())
                Return False
            End If
            If mTool.CheckStop() Then Return False

            'pass data out
            Data.Position = Position

            'changed by Ming, judged by Total power,unit in mW
            Data.PeakValue = xData.TotalEnergy

            '!!!--------------------------------------------------------------------'
            '!!!we will make changes here to match Beam Scan axis with stage axis!!!'
            '!!!apply sign if needed                                             !!!' 
            '!!!--------------------------------------------------------------------'
            Data.WidthZZ = xData.FWHMY
            Data.WidthYY = xData.FWHMX

            'Data.FWHMZZ = xData.FWHMY
            'Data.FWHMYY = xData.FWHMX
            'changed by Ming, centroid position is used for its stablity.
            Data.PeakZZ = xData.CentroidY
            Data.PeakYY = xData.CentroidX


            'show xData
            If ShowDetail Then
                fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,6:0.000}", "{1,7:0.0000}", "{2,7:0.0000}", "{3,7:0.0000}", "{4,7:0.0000}", "{5,7:0.0}", "{6,8:0}")
                s = String.Format(fmt, "Pos(mm)", "Width X", "Width Y", " Peak X", " Peak Y", " Peak E", "Total Energy")
                s += ControlChars.CrLf
                s += String.Format(fmt, Position, xData.D4SigmaX, xData.D4SigmaY, xData.PeakLocationX, xData.PeakLocationY, xData.PeakPower, xData.TotalEnergy)
                mMsgInfo.PostMessage(s)

                fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,6:0.00}", "{1,6:0.00}", "{2,6:0.0}", "{3}", "{4}")
                s = String.Format(fmt, " Gain", "  Exp", "BlkLel", "F.InCal", "B.InCal")
                s += ControlChars.CrLf
                s += String.Format(fmt, xData.Gain, xData.Exposure, xData.BlackLevel, xData.IsFarmeCalibrated, xData.IsBaselineCalibrated)
                mMsgInfo.PostMessage(s)
            End If

            'pass fail?
            If CheckData Then
                If Data.PeakValue < mPara.Alignment.MinPower Then
                    s = "X   Beam peak intensity {0:0.00} < {1:0.00} minimum intensity requirement!"
                    s = String.Format(s, Data.PeakValue, mPara.Alignment.MinPower)
                    mMsgInfo.PostMessage(s)
                    Return False
                End If

                If Data.PeakZZ < mPara.Alignment.MinZZ Or Data.PeakZZ > mPara.Alignment.MaxZZ Then
                    s = "X   Peak location Z {0:0.0000} outside the boundary [{1:0.0000}, {2:0.0000}]"
                    s = String.Format(s, Data.PeakZZ, mPara.Alignment.MinZZ, mPara.Alignment.MaxZZ)
                    mMsgInfo.PostMessage(s)
                    Return False
                End If

                If Data.PeakYY < mPara.Alignment.MinYY Or xData.PeakLocationY > mPara.Alignment.MaxYY Then
                    s = "X   Peak location Y {0:0.0000} outside the boundary [{1:0.0000}, {2:0.0000}]"
                    s = String.Format(s, Data.PeakYY, mPara.Alignment.MinYY, mPara.Alignment.MaxYY)
                    mMsgInfo.PostMessage(s)
                    Return False
                End If
            End If
         
            Return True
        End Function

        Public Function GetBeamCollimationData(ByVal Position1 As Double, ByVal Position2 As Double, ByVal ShowDetail As Boolean, ByVal CheckSpec As Boolean, ByRef Data As CollimatedBeamData) As Boolean
            Dim success As Boolean
            Dim s, fmt As String
            Dim Position0 As Double
            Dim DataA, DataB As BeamWaistData

            'ack
            s = "    Measure collimating beam data ... "
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)
            If ShowDetail Then mMsgInfo.PostMessage(s)

            'validate position
            If Double.IsNaN(Position1) Then Position1 = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanNear).X
            If Double.IsNaN(Position2) Then Position2 = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X

            'get current position and check which one is closer
            Position0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)
            If Math.Abs(Position1 - Position0) > Math.Abs(Position2 - Position0) Then
                Position0 = Position2
                Position2 = Position1
                Position1 = Position0
            End If

            'get data at position 1
            success = Me.GetBeamScanData(Position1, False, False, DataA)
            If Not success Then Return False

            'get data at position 2
            success = Me.GetBeamScanData(Position2, False, False, DataB)
            If Not success Then Return False

            'summarize data
            Data = New CollimatedBeamData(DataA, DataB)

            'show data
            If ShowDetail Then
                fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0}", "{1,7:0.00}", "{2,7:0.00}", "{3,7:0.00}")
                s = ""
                s += ControlChars.CrLf + String.Format(fmt, "Item", "Near", "Far", "Extra", "")
                s += ControlChars.CrLf + String.Format(fmt, "Meas Pos", Data.DataNear.Position, Data.DataFar.Position, "")
                s += ControlChars.CrLf + String.Format(fmt, "Width Y", Data.DataNear.WidthYY, Data.DataFar.WidthYY, Data.WidthDiffYY)
                s += ControlChars.CrLf + String.Format(fmt, "Width Z", Data.DataNear.WidthZZ, Data.DataFar.WidthZZ, Data.WidthDiffZZ)
                s += ControlChars.CrLf + String.Format(fmt, "Peak Int", Data.DataNear.PeakValue, Data.DataFar.PeakValue, Data.AveragePeakValue)
                s += ControlChars.CrLf + String.Format(fmt, "Peak Y", Data.DataNear.PeakYY, Data.DataFar.PeakYY, Data.PeakSteeringYY)
                s += ControlChars.CrLf + String.Format(fmt, "Peak Z", Data.DataNear.PeakZZ, Data.DataFar.PeakZZ, Data.PeakSteeringZZ)
                mMsgInfo.PostMessage(s)
            End If

            'check spec
            success = True
            If CheckSpec Then
                fmt = "      Peak Settring {0} = {1,7:0.0000}, Spec {2,7:0.000}"

                mMsgInfo.PostMessage("")

                s = String.Format(fmt, "Y", Data.PeakSteeringYY, mPara.Alignment.MaxSteeringY)
                If Math.Abs(Data.PeakSteeringYY) > mPara.Alignment.MaxSteeringY Then
                    s = mMsgInfo.MarkFailed(s)
                    success = False
                End If
                mMsgInfo.PostMessage(s)

                s = String.Format(fmt, "Z", Data.PeakSteeringZZ, mPara.Alignment.MaxSteeringZ)
                If Math.Abs(Data.PeakSteeringYY) > mPara.Alignment.MaxSteeringZ Then
                    s = mMsgInfo.MarkFailed(s)
                    success = False
                End If
                mMsgInfo.PostMessage(s)
            End If

            'return 
            Return success
        End Function

#Region "simplified utility function"
        Public Function GetBeamScanData(ByVal CheckData As Boolean) As Boolean
            Dim data As BeamWaistData
            Return Me.GetBeamScanData(Double.NaN, True, CheckData, data)
        End Function

        Public Function GetBeamCollimationData(ByVal CheckSpec As Boolean) As Boolean
            Dim data As CollimatedBeamData
            Return Me.GetBeamCollimationData(Double.NaN, Double.NaN, True, CheckSpec, data)
        End Function
#End Region
#End Region

#Region "lens alignment"
        Public Function CenterBeamScan(ByVal BeamScanPosition As Double) As Boolean
            Dim success As Boolean
            Dim s As String
            Dim Data As BeamWaistData
            Dim OffsetY, OffsetZ As Double

            'ack
            s = "    Center beam scan reading ... "
            mMsgInfo.PostMessage(s)

            'turn CH2 on
            'mTool.mInst.LDD.TurnSingleChannelOn(mBaseChannel)

            'get beam scan slider X position
            'If Double.IsNaN(BeamScanPosition) Then BeamScanPosition = mPara.Alignment.BeamScanXList(0)

            'get beam scan data at near field
            success = Me.GetBeamScanData(Double.NaN, False, False, Data)
            If Not success Then Return False

            OffsetY = (4500 - Data.PeakYY) * 0.001
            OffsetZ = (4500 - Data.PeakZZ) * 0.001

            s = "      " + Data.GetTableHeader()
            s += ControlChars.CrLf + "      Initial reading"
            s += ControlChars.CrLf + "      " + Data.GetTableData()
            mMsgInfo.PostMessage(s)

            'move beam scan to center beam
            s = "Center beam on beam scan"
            mMsgInfo.PostMessage(s, w2.w2MessageService.MessageDisplayStyle.NewStatus)

            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.BeamScanY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -OffsetY)
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.BeamScanZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -OffsetZ)
            success = Me.WaitStageToStop("wait for beam scan statge move", True)
            If Not success Then Return False

            'read beam scan data again
            success = Me.GetBeamScanData(Double.NaN, False, True, Data)
            If Not success Then Return False

            s = ControlChars.CrLf + "      After centering beam scan Y and Z"
            s += ControlChars.CrLf + "      " + Data.GetTableData()
            mMsgInfo.PostMessage(s)

            Return True
        End Function
        'Simple Version of AlignlensForIntersity by delete adjusting current
        Public Function AlignLensForSmallestBeam(ByVal Channel As Integer) As Boolean
            Dim s, sStep As String
            Dim n As Integer
            Dim X, X0, XStep, LastPeak As Double
            Dim data As BeamWaistData
            Dim success, reversed, peakFound As Boolean
            Dim peakposition As Double

            'ack
            mMsgInfo.PostMessage("    Adjust lens X position for peak intensity ... ")
            'turn laser on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)
            'do adjustment
            s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,7:0.00}", "{2}")
            mMsgInfo.PostMessage(String.Format(s, "Lens X  ", "Width  ", "Adjustment"))

            'step condition
            XStep = mPara.Alignment.StageStepX

            'reduce stage speed
            sStep = "Initial"
            reversed = False
            peakFound = False

            X0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)

            'do loop
            n = 0
            While True
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit While
                End If

                'get data
                X = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Exit While

                'show data
                mMsgInfo.PostMessage(String.Format(s, X, data.WidthYY, sStep))
                If peakFound Then
                    success = True
                    s = "    Peak position is at {0:0.0000}"
                    s = String.Format(s, peakposition)
                    mMsgInfo.PostMessage(s)
                    s = "    Move back to peak position..."
                    mMsgInfo.PostMessage(s)
                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, peakposition)
                    Exit While
                End If

                'check intensity - adjust laser current first
                Select Case True

                    Case n = 0
                        'first X adjustment, change flag
                        sStep = "Lens X first"

                    Case data.WidthYY < LastPeak
                        'peak still increasing, keep going
                        sStep = "Lens X " + Math.Sign(XStep).ToString("0")
                        If Not reversed Then
                            If data.WidthYY < 600 Then
                                XStep = mPara.Alignment.StageStepX
                            Else
                                XStep = XStep * 1.2
                            End If
                        Else
                            XStep = XStep * 1.5
                        End If
                        If data.WidthYY < 400 Then
                            peakFound = True
                        End If
                        peakposition = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)

                    Case data.WidthYY > LastPeak
                        'peak decrease

                        If n = 1 Then
                            n = 0
                            XStep = -XStep
                            sStep = "Lens X large step reverse, first step decrease"
                        Else
                            'move back, at reduced step size
                            XStep = -Math.Sign(XStep) * mPara.Alignment.StageStepFineX
                            'do not do reverse twice
                            If reversed Then
                                peakFound = True
                                mMsgInfo.PostMessage("<- Signal drop again", w2.w2MessageService.MessageDisplayStyle.ContinuingMessage)
                                sStep = "<- Back to peak"

                            Else
                                'peak decreasing, reverse X
                                reversed = True
                                sStep = "Lens X " + Math.Sign(XStep).ToString("0")
                            End If
                        End If
                End Select

                'check range
                If Math.Abs(X + XStep - X0) > mPara.Alignment.MaxStageMoveX Then
                    s = "X   Lens X has moved away from initial position by {0:0.0000}, more than it is allowed {1:0.0000} "
                    s = String.Format(s, (X + XStep - X0), mPara.Alignment.MaxStageMoveX)
                    mMsgInfo.PostMessage(s)
                    success = False
                    Exit While
                End If

                'move X now
                success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, XStep)
                If Not success Then
                    Me.ShowStageError("Stage X")
                    Exit While
                End If
                success = Me.WaitStageToStop("Wait for Stage X to stop", True)
                If Not success Then Exit While

                'next 
                LastPeak = data.WidthYY
                n += 1
            End While

            'recover stage speed
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageX)

            'done
            Return success


        End Function

        Public Function AlignLensForTotalEnergy(ByVal Channel As Integer) As Boolean
            Dim s, sStep As String
            Dim n As Integer
            Dim X, X0, XStep, LastPeak As Double
            Dim data As BeamWaistData
            Dim success, reversed, peakFound As Boolean
            Dim axis As iXpsStage.AxisNameEnum
            Dim eAxis As String
            Dim i, k, R As Integer
            Dim peakPosition As Double
            'ack

            mMsgInfo.PostMessage("    Adjust lens  position for total energy ... ")

            'turn laser on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'step condition
            XStep = mPara.Alignment.StageStepXforEnergy

            For i = 0 To 3

                Select Case i
                    Case 0
                        XStep = mPara.Alignment.StageStepXforEnergy
                        axis = iXpsStage.AxisNameEnum.StageX
                        eAxis = " X "
                        s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,7:0.00}", "{2}")
                        mMsgInfo.PostMessage(String.Format(s, "Lens X  ", "Energy  ", "Adjustment"))
                    Case 1
                        XStep = mPara.Alignment.StageStepXforEnergy
                        axis = iXpsStage.AxisNameEnum.StageY
                        eAxis = " Y "
                        s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,7:0.00}", "{2}")
                        mMsgInfo.PostMessage(String.Format(s, "Lens Y  ", "Energy  ", "Adjustment"))
                    Case 2
                        XStep = mPara.Alignment.StageStepXforEnergy
                        axis = iXpsStage.AxisNameEnum.StageZ
                        eAxis = " Z "
                        s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,7:0.00}", "{2}")
                        mMsgInfo.PostMessage(String.Format(s, "Lens Z  ", "Energy  ", "Adjustment"))
                    Case 3
                        XStep = mPara.Alignment.StageStepXforEnergy
                        axis = iXpsStage.AxisNameEnum.StageX
                        eAxis = " X "
                        s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,7:0.00}", "{2}")
                        mMsgInfo.PostMessage(String.Format(s, "Lens X  ", "Energy  ", "Adjustment"))
                End Select

                n = 0
                X0 = mStage.GetStagePosition(axis)

                sStep = "Initial"
                reversed = False
                peakFound = False
                peakPosition = mStage.GetStagePosition(axis)
                'do loop
                While True
                    'check stop
                    If mTool.CheckStop() Then
                        success = False
                        Exit While
                    End If

                    'get data
                    X = mStage.GetStagePosition(axis)
                    success = Me.GetBeamScanData(Double.NaN, False, False, data)
                    If Not success Then Exit While
                    'show data
                    mMsgInfo.PostMessage(String.Format(s, X, data.PeakValue, sStep))
                    If peakFound Then
                        success = True
                        s = "    Peak position is at {0:0.0000}"
                        s = String.Format(s, peakPosition)
                        mMsgInfo.PostMessage(s)
                        s = "    Move back to peak position..."
                        mMsgInfo.PostMessage(s)
                        success = mStage.MoveStageNoWait(axis, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, peakPosition)
                        Exit While
                    End If

                    'check intensity - adjust laser current first
                    Select Case True

                        Case n = 0
                            'first X adjustment, change flag
                            sStep = "Lens" + eAxis + "first"

                        Case data.PeakValue > LastPeak
                            'peak still increasing, keep going
                            sStep = "Lens" + eAxis + Math.Sign(XStep).ToString("0")

                            If Not reversed Then
                                XStep = 1.2 * XStep
                            Else
                                XStep = 1.5 * XStep
                            End If

                            If data.PeakValue > 15 Then
                                peakFound = True
                            End If
                            peakPosition = mStage.GetStagePosition(axis)
                        Case data.PeakValue < LastPeak
                            'move back, at reduced step size
                            If n = 1 Then
                                n = 0
                                XStep = -XStep
                                sStep = "Lens" + eAxis + "large step reverse, first step decrease"
                            Else
                                XStep = -Math.Sign(XStep) * mPara.Alignment.StageStepFineXforEnergy
                                'do not do reverse twice
                                If reversed Then
                                    peakFound = True
                                    mMsgInfo.PostMessage("<- Signal drop again", w2.w2MessageService.MessageDisplayStyle.ContinuingMessage)
                                    sStep = "<- Back to peak"

                                Else
                                    'peak decreasing, reverse X
                                    reversed = True
                                    sStep = "Lens" + eAxis + Math.Sign(XStep).ToString("0")
                                End If
                            End If
                    End Select

                    'check range
                    If Math.Abs(X + XStep - X0) > mPara.Alignment.MaxStageMoveX Then
                        s = "X   Lens" + eAxis + "has moved away from initial position by {0:0.0000}, more than it is allowed {1:0.0000} "
                        s = String.Format(s, (X + XStep - X0), mPara.Alignment.MaxStageMoveX)
                        mMsgInfo.PostMessage(s)
                        success = False
                        Exit While
                    End If

                    'move X now
                    success = mStage.MoveStageNoWait(axis, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, XStep)
                    If Not success Then
                        Me.ShowStageError("Stage" + eAxis)
                        Exit While
                    End If
                    success = Me.WaitStageToStop("Wait for Stage" + eAxis + "to stop", True)
                    If Not success Then Exit While

                    'next 
                    LastPeak = data.PeakValue

                    n += 1
                End While

            Next

            'done
            Return success


        End Function

        'this function can be simplified if we verify adjust laser current is not needed
        Public Function AlignLensForIntensity(ByVal Channel As Integer, ByVal UseNearSide As Boolean) As Boolean
            Dim s, sStep As String
            Dim n As Integer
            Dim Current, Current0, CurrentStep As Double
            Dim X, X0, XStep, LastPeak As Double
            Dim data As BeamWaistData
            Dim success, adjustCurrent, reversed, peakFound As Boolean

            Const CurrentStepMin As Double = 1.0

            'ack
            mMsgInfo.PostMessage("    Adjust lens X position for peak intensity ... ")

            'turn laser on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'first move the beam gage to near side
            If UseNearSide Then
                mMsgInfo.PostMessage("    Move beam scan to near side")
                X = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanNear).X
            Else
                mMsgInfo.PostMessage("    Move beam scan to far side")
                X = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            End If

            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, X, False)
            If Not success Then Return False

            'do adjustment
            s = "      " + w2String.Concatenate(ControlChars.Tab, "{0,7:0.0000}", "{1,7:0.00}", "{2,7:0.00}", "{3}")
            mMsgInfo.PostMessage(String.Format(s, "Lens X", " Current", "Peak Value", "Adjustment"))

            'step condition
            CurrentStep = mPara.LaserDiode.CurrentStep
            XStep = mPara.Alignment.StageStepX

            'reduce stage speed
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageX, mPara.Alignment.StageSpeed)

            sStep = "Initial"
            reversed = False
            peakFound = False

            X0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            Current0 = mTool.mInst.LDD.Current(Channel)
            Current = Current0

            'do loop
            n = 0
            While True
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit While
                End If

                'get data
                X = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Exit While

                'show data
                mMsgInfo.PostMessage(String.Format(s, X, Current, data.PeakValue, sStep))
                If peakFound Then
                    success = True
                    Exit While
                End If

                'check intensity - adjust laser current first
                adjustCurrent = False

                Select Case True
                    Case (data.PeakValue < mPara.Alignment.MinPower) And UseNearSide
                        'if the current was decreased, cut the step by half
                        If Current < Current0 Then CurrentStep *= 0.5
                        If CurrentStep < CurrentStepMin Then CurrentStep = CurrentStepMin
                        Current += CurrentStep
                        sStep = "Current +"
                        adjustCurrent = True

                    Case (data.PeakValue > mPara.Alignment.MaxPower) And UseNearSide
                        'if the current was increased, cut the step by half
                        If Current > Current0 Then CurrentStep *= 0.5
                        If CurrentStep < CurrentStepMin Then CurrentStep = CurrentStepMin
                        Current -= CurrentStep
                        sStep = "Current -"
                        adjustCurrent = True

                    Case n = 0
                        'first X adjustment, change flag
                        n = 1
                        sStep = "Lens X first"

                    Case data.PeakValue > LastPeak
                        'peak still increasing, keep going
                        sStep = "Lens X " + Math.Sign(XStep).ToString("0")

                    Case data.PeakValue < LastPeak
                        'move back, at reduced step size
                        XStep = -Math.Sign(XStep) * mPara.Alignment.StageStepFineX
                        'do not do reverse twice
                        If reversed Then
                            peakFound = True
                            mMsgInfo.PostMessage("<- Signal drop again", w2.w2MessageService.MessageDisplayStyle.ContinuingMessage)
                            sStep = "<- Back to peak"
                        Else
                            'peak decreasing, reverse X
                            reversed = True
                            sStep = "Lens X " + Math.Sign(XStep).ToString("0")
                        End If
                End Select

                If adjustCurrent Then
                    'check range
                    If Current < mPara.LaserDiode.MinCurrent Then
                        mMsgInfo.PostMessage("X   Required laser current is too low to make sense. Please adjust beam scan gain!")
                        success = False
                        Exit While
                    End If
                    If Current > mPara.LaserDiode.MaxCurrent Then
                        mMsgInfo.PostMessage("X   Required laser current is too high to make sense. Please adjust beam gage integration time!")
                        success = False
                        Exit While
                    End If

                    'apply current
                    mTool.mInst.LDD.Current(Channel) = Current
                    System.Threading.Thread.Sleep(50)

                Else
                    'check range
                    If Math.Abs(X + XStep - X0) > mPara.Alignment.MaxStageMoveX Then
                        s = "X   Lens X has moved away from initial position by {0:0.0000}, more than it is allowed {1:0.0000} "
                        s = String.Format(s, (X + XStep - X0), mPara.Alignment.MaxStageMoveX)
                        mMsgInfo.PostMessage(s)
                        success = False
                        Exit While
                    End If

                    'move X now
                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageX, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, XStep)
                    If Not success Then
                        Me.ShowStageError("Stage X")
                        Exit While
                    End If
                    success = Me.WaitStageToStop("Wait for Stage X to stop", True)
                    If Not success Then Exit While

                End If

                'next 
                LastPeak = data.PeakValue
            End While

            'recover stage speed
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageX)

            'done
            Return success
        End Function

        Public Function AlignLensForBeamSteering_Coarse(ByVal Channel As Integer) As Boolean
            Dim success As Boolean
            Dim s As String
            Dim i, ii, ZCapMin, ZCapMax As Integer
            Dim BSx, Y0, Y1, Y2, Z0, Z1, Z2, gain, L As Double
            Dim Data1, Data2, DataAdjusted, DataCorr As BeamWaistData
            Dim DataX As CollimatedBeamData
            Dim deltaY(4), deltaZ(4) As Double

            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.BeamScanX, 80)
            'ack
            s = "    Coarse alignment by centering beam along the rail ..."
            mMsgInfo.PostMessage(s)
            mMsgInfo.PostMessage(Me.GetFormattedString_BeamDataHeader(True))

            'turn laser on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'get current lens position
            Y1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            Z1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            'save it for future reference 
            Y0 = Y1
            Z0 = Z1

            'get beam scan data at near field
            BSx = mPara.Alignment.BeamScanXList(0)
            success = Me.GetBeamScanData(BSx, False, False, Data1)

            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
            success = Me.GetBeamScanData(BSx, False, False, DataCorr)
            deltaY(0) = DataCorr.PeakYY - Data1.PeakYY
            deltaZ(0) = DataCorr.PeakZZ - Data1.PeakZZ
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001)
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001)


            If Not success Then Return False

            s = "      Initial at position = 0"
            s += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y1, Z1, Data1)
            mMsgInfo.PostMessage(s)

            'do loop to slowly move bean scan back - start from 1, we already did i = 0
            ii = mPara.Alignment.BeamScanXList.Length - 1
            For i = 1 To ii
                mStage.SetStageVelocity(iXpsStage.AxisNameEnum.BeamScanX, 80)
                'get beam scan data at next position
                BSx = mPara.Alignment.BeamScanXList(i)
                success = Me.GetBeamScanData(BSx, False, False, Data2)
                If Not success Then Return False

                'get collimation data based on this and previous data, stage is still at Y1 and Z1
                DataX = New CollimatedBeamData(Data1, Data2)
                s = "      At position = " + i.ToString()
                s += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y1, Z1, Data2)
                mMsgInfo.PostMessage(s)

                'continue to the next position if the steering is in spec
                If Math.Abs(DataX.PeakSteeringYY) < mPara.Alignment.MinSteeringY And Math.Abs(DataX.PeakSteeringZZ) < mPara.Alignment.MinSteeringZ Then
                    mMsgInfo.PostMessage("      Collimation good, continue to next position")

                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
                    success = Me.GetBeamScanData(BSx, False, False, DataCorr)
                    deltaY(i) = DataCorr.PeakYY - Data2.PeakYY
                    deltaZ(i) = DataCorr.PeakZZ - Data2.PeakZZ
                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001)
                    success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001)
                    'we will keep use the previous Data1 which is at nearer side, this effectively skip the alignment for this position
                    Continue For
                End If

                'adjust stage YZ for steering - because L is different for each step, the coef is not normalized, it is "raw data", basically the lens forcal length
                'we are making sure beam scan axis is matching stage axis
                'in this configuration, a positive beam steering => stage is below focal point => delta move is positive

                L = Math.Abs(DataX.DataFar.Position - DataX.DataNear.Position)

                gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensY)
                Y2 = Y1 + DataX.PeakSteeringYY / gain / 1000
                If Math.Abs(Y2 - Y1) > 0.01 Then
                    Y2 = Y1 + Math.Sign(DataX.PeakSteeringYY) * 0.01
                End If
                gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensZ)
                Z2 = Z1 + DataX.PeakSteeringZZ / gain / 1000
                If Math.Abs(Z2 - Z1) > 0.01 Then
                    Z2 = Z1 + Math.Sign(DataX.PeakSteeringZZ) * 0.01
                End If
                'move to the new position - we will ignore previous zcap failures
                'initial positions are passed to check if the amount of move is safe
                ZCapMax = 0
                ZCapMin = 0
                success = Me.MoveStageForLensAlignment(Y2, Z2, Y0, Z0, ZCapMin, ZCapMax)
                If Not success Then Exit For

                'get the actual stage position - Z may be bounded by the boneline control
                Y2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                Z2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                'get a beam data - now beam scan position move, no position is passed           -------!!!---------Note that because beam gage does not move with stage, the beam scan data moves with stage move
                '                                                                               -------!!!---------But we are OK here becasue Data1 and Data2 are measured with the same stage position!
                '                                                                               -------!!!---------One way to check this is that Data1 here should be close to Data2 previously after adding DetlaY and DeltaZ
                success = Me.GetBeamScanData(Double.NaN, False, False, DataAdjusted)
                If Not success Then Exit For

                '*** we may need to change sign here, depending on how the axis for stage and beam scan are "aligned"
                deltaY(i) = (DataAdjusted.PeakYY - Data2.PeakYY) / (Y2 - Y1) / 1000
                deltaZ(i) = (DataAdjusted.PeakZZ - Data2.PeakZZ) / (Z2 - Z1) / 1000

                DataCorr = DataAdjusted


                s = "      After adjustment"
                s += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y2, Z2, DataCorr) + ControlChars.Tab + "Actual position"
                's += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y2, Z2, DataCorr) + ControlChars.Tab + "If beam scan moved with package"
                mMsgInfo.PostMessage(s)

                gain = deltaY(i - 1) - deltaY(i)                  'please verify that the sign of gain is positiove
                If gain < 0 Then
                    gain = -gain
                End If
                mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensY, gain)
                gain = deltaZ(i - 1) - deltaZ(i)
                If gain < 0 Then
                    gain = -gain
                End If
                mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensZ, gain)

                'rotate buffer for the next process
                Data1 = DataCorr
                Y1 = Y2
                Z1 = Z2
            Next

            success = mPara.SaveAlignCoefToFile()

            Return success

        End Function

        'this process trys to align the stage so that beam at near and far sides hits the same position
        Public Function AlignLensForSteering_Fine(ByVal Channel As Integer) As Boolean
            Dim success As Boolean
            Dim s, sInSpec As String
            Dim k, ZCapMin, ZCapMax As Integer
            Dim v, L As Double
            Dim Y0, Y1, Y2, Z0, Z1, Z2 As Double
            Dim DeltaY, DeltaZ As Double
            Dim data1, data2 As CollimatedBeamData

            'Dim dataNear, dataFar As BeamWaistData
            Dim nearBeamDeltaY(10), nearBeamDeltaZ(10), farBeamDeltaY(10), farBeamDeltaZ(10) As Double

            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.BeamScanX, 80)
            'data
            sInSpec = "      Beam steering in psec: |DeltaY| < {0:0.000}, |DeltaZ| < {1:0.000}"
            sInSpec = String.Format(sInSpec, mPara.Alignment.MaxSteeringY, mPara.Alignment.MaxSteeringZ)

            'ack
            mMsgInfo.PostMessage("    Adjust lens Y and Z position for beam steering ... ")

            'turn laser on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'header
            s = Me.GetFormattedString_BeamDataHeader(True)
            mMsgInfo.PostMessage(s)

            'get the current position and beam steering
            Y1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            Z1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

            'calculate the gain of near field,added by Ming

            'success = Me.GetBeamScanData(mPara.Alignment.BeamScanXList(0), False, False, dataNear)
            'success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
            'success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, 0.001)
            'success = Me.GetBeamScanData(mPara.Alignment.BeamScanXList(0), False, False, dataFar)
            'BeamDeltaY(0) = dataFar.PeakYY - dataNear.PeakYY
            'BeamDeltaZ(0) = dataFar.PeakZZ - dataNear.PeakZZ
            'success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001)
            'success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -0.001

            success = Me.GetBeamCollimationData(Double.NaN, Double.NaN, False, False, data1)
            If Not success Then Return False

            'save position for later reference
            Y0 = Y1
            Z0 = Z1

            'show data
            s = "      Initial"
            s += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y1, Z1, data1)
            mMsgInfo.PostMessage(s)

            'exit if steering is in spec
            If Math.Abs(data1.PeakSteeringYY) < mPara.Alignment.MaxSteeringY And Math.Abs(data1.PeakSteeringZZ) < mPara.Alignment.MaxSteeringZ Then
                mMsgInfo.PostMessage(sInSpec)
                Return True
            End If

            'slowdown motor speed
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, mPara.Alignment.StageSpeed)
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.Alignment.StageSpeed)

            'do an initial step move - note that the gain for Y is the opposite

            v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY)
            DeltaY = data1.PeakSteeringYY / v / 1000

            v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ)
            DeltaZ = data1.PeakSteeringZZ / v / 1000

            k = 0
            ZCapMin = 0
            ZCapMax = 0
            While True
                'check stop
                If mTool.CheckStop() Then Return False
                mStage.SetStageVelocity(iXpsStage.AxisNameEnum.BeamScanX, 80)

                'get new position
                Y2 = Y1 + DeltaY
                Z2 = Z1 + DeltaZ

                'move to the new position
                success = Me.MoveStageForLensAlignment(Y2, Z2, Y0, Z0, ZCapMin, ZCapMax)
                If Not success Then Exit While

                'get the current position - actual
                Y2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                Z2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                'get beam scan data
                success = Me.GetBeamCollimationData(Double.NaN, Double.NaN, False, False, data2)
                If Not success Then Exit While

                'show data
                s = "      Adjustment " + k.ToString()
                s += ControlChars.CrLf + Me.GetFormattedString_BeamData(True, Y2, Z2, data2)
                mMsgInfo.PostMessage(s)

                'exit if steering is in spec
                If Math.Abs(data2.PeakSteeringYY) < mPara.Alignment.MaxSteeringY And Math.Abs(data2.PeakSteeringZZ) < mPara.Alignment.MaxSteeringZ Then
                    mMsgInfo.PostMessage(sInSpec)
                    success = True
                    Exit While
                ElseIf Math.Abs(data2.PeakSteeringYY) < mPara.Alignment.MaxSteeringY And Math.Abs(data2.PeakSteeringZZ) > mPara.Alignment.MaxSteeringZ Then
                    k += 1
                    nearBeamDeltaZ(k) = (data2.DataNear.PeakZZ - data1.DataNear.PeakZZ) / (Y2 - Y1) / 1000
                    farBeamDeltaZ(k) = (data2.DataFar.PeakZZ - data1.DataFar.PeakZZ) / (Z2 - Z1) / 1000
                    v = nearBeamDeltaZ(k) - farBeamDeltaZ(k)
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ, v)
                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ)
                    DeltaZ = data2.PeakSteeringZZ / v / 1000

                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY)
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY, v)
                    DeltaY = 0


                ElseIf Math.Abs(data2.PeakSteeringYY) > mPara.Alignment.MaxSteeringY And Math.Abs(data2.PeakSteeringZZ) < mPara.Alignment.MaxSteeringZ Then
                    k += 1
                    nearBeamDeltaY(k) = (data2.DataNear.PeakYY - data1.DataNear.PeakYY) / (Y2 - Y1) / 1000
                    farBeamDeltaY(k) = (data2.DataFar.PeakYY - data1.DataFar.PeakYY) / (Y2 - Y1) / 1000
                    v = nearBeamDeltaY(k) - farBeamDeltaY(k)
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY, v)
                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY)
                    DeltaY = data2.PeakSteeringYY / v / 1000

                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ)
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ, v)
                    DeltaZ = 0


                Else
                    k += 1
                    nearBeamDeltaY(k) = (data2.DataNear.PeakYY - data1.DataNear.PeakYY) / (Y2 - Y1) / 1000
                    nearBeamDeltaZ(k) = (data2.DataNear.PeakZZ - data1.DataNear.PeakZZ) / (Z2 - Z1) / 1000
                    farBeamDeltaY(k) = (data2.DataFar.PeakYY - data1.DataFar.PeakYY) / (Y2 - Y1) / 1000
                    farBeamDeltaZ(k) = (data2.DataFar.PeakZZ - data1.DataFar.PeakZZ) / (Z2 - Z1) / 1000
                    v = nearBeamDeltaY(k) - farBeamDeltaY(k)                  'please verify that the sign of gain is positiove
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY, v)
                    v = nearBeamDeltaZ(k) - farBeamDeltaZ(k)
                    mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ, v)

                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineY)
                    DeltaY = data2.PeakSteeringYY / v / 1000

                    v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.LensfineZ)
                    DeltaZ = data2.PeakSteeringZZ / v / 1000
                End If



                'calculate the new delta 


                'rotate buffer
                Y1 = Y2
                Z1 = Z2

                'time out
                If k = 5 Then
                    mMsgInfo.PostMessage("X   Still cannot find optimized position after multiple trials")
                    success = False
                    Exit While
                End If
            End While
            'recover speed
            'Me.MoveStage1Axis(iXpsStage.AxisNameEnum.BeamScanX, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, mPara.Alignment.BeamScanXList(0), False)
            mPara.SaveAlignCoefToFile()

            Return success
        End Function

        Public Function AlignLensForSteering_Overlap(ByVal Channel As Integer) As Boolean
            Dim success As Boolean
            Dim s, sInSpec As String
            Dim i, ZCapMin, ZCapMax As Integer
            Dim Data1, Data2 As BeamWaistData
            Dim Y0, Z0, Y1, Z1, Y2, Z2, Yset, Zset, Gain, L As Double
            Dim BeamZZ1, BeamYY1, BeamZZ2, BeamYY2 As Double

            sInSpec = "      Beam steering in psec: |DeltaY| < {0:0.000}, |DeltaZ| < {1:0.000}"
            sInSpec = String.Format(sInSpec, mPara.Alignment.MaxSteeringY, mPara.Alignment.MaxSteeringZ)

            'ack
            s = "    Align lens for overlap ... "
            mMsgInfo.PostMessage(s)

            'we do not need to do CH2
            If Channel = mBaseChannel Then
                s = "      CH {0:0} is base channel and self aligned. No overlap aligment is needed."
                s = String.Format(s, mBaseChannel)
                mMsgInfo.PostMessage(s)
                Return True
            End If

            'header
            s = Me.GetFormattedString_BeamDataHeader(True)
            mMsgInfo.PostMessage(s)

            'get beam scan position at far side and move it
            L = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            success = Me.MoveBeamGage(L, False)
            If Not success Then Return False

            'L = (mPara.Alignment.BeamScanXOffset + L)

            'slow down motor speed for alignment
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, mPara.Alignment.StageSpeed)
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.Alignment.StageSpeed)

            'loop
            i = 0
            ZCapMin = 0
            ZCapMax = 0
            While True
                'check stop
                If mTool.CheckStop() Then Return False

                'info
                mMsgInfo.PostMessage("      Alignment" + i.ToString())

                If i = 0 Then
                    'get current stage position, save for future reference
                    Y0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                    Z0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
                Else
                    'move to the new position
                    success = Me.MoveStageForLensAlignment(Yset, Zset, Y0, Z0, ZCapMin, ZCapMax)
                    If Not success Then Exit While
                End If

                'since beam scan does not move together with package, we need to re-measure the beam position everytime
                success = Me.GetBeamCenter_AlignedChannels(Channel, BeamYY2, BeamZZ2)
                If Not success Then Exit While

                'get the current position and beam steering - do this after beam scan measurement to get a more stable stage position
                Y2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                Z2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                'get beam gage data for the target channel
                mTool.mInst.LDD.TurnSingleChannelOn(Channel)
                success = Me.GetBeamScanData(Double.NaN, False, False, Data2)
                If Not success Then Exit While

                s = Me.GetFormattedString_BeamOverlap(True, Channel, Data2.PeakYY, Data2.PeakZZ, Data2.PeakYY - BeamYY2, Data2.PeakZZ - BeamZZ2, Y2, Z2, Double.NaN)
                mMsgInfo.PostMessage(s)

                'exit if steering is in spec
                If Math.Abs(Data2.PeakYY - BeamYY2) < mPara.Alignment.MaxSteeringY And Math.Abs(Data2.PeakZZ - BeamZZ2) < mPara.Alignment.MaxSteeringZ Then
                    mMsgInfo.PostMessage(sInSpec)
                    success = True
                    Exit While
                End If

                'estimate the next move
                If i = 0 Then
                    'calculate the next position, for the first time, we use the previous coef 
                    'Gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.lensYoverlap)
                    Gain = 1000
                    Yset = Y2 + (Data2.PeakYY - BeamYY2) / Gain / 1000
                    'Gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.lensZoverlap)
                    Gain = 1200
                    Zset = Z2 + (Data2.PeakZZ - BeamZZ2) / Gain / 1000
                Else
                    'for the new one, we have two sets of data to calculate the new coef
                    'initial diff is (Data1.PeakY - BeamY1), new diff is (Data2.PeakY - BeamY2), the stage move was (Y2-Y1)

                    Gain = (Data2.PeakYY - BeamYY2) - (Data1.PeakYY - BeamYY1)
                    If Gain <> 0 Then
                        'the gain should be very consistent, we only allow a small adjustment
                        Gain = -(Data2.PeakYY - Data1.PeakYY) / (Y2 - Y1) / 1000
                        'If Gain < 0 Then
                        '    s = "X   estimated coef has the wrong sign!"
                        'End If
                        mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.lensYoverlap, Gain)
                        Gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.lensYoverlap)
                        Yset = Y2 + (Data2.PeakYY - BeamYY2) / Gain / 1000
                    Else
                        mMsgInfo.PostMessage("X   Beam is not moving with the stage Y move")
                        success = False
                        Exit While
                    End If

                    Gain = (Data2.PeakZZ - BeamZZ2) - (Data1.PeakZZ - BeamZZ1)
                    If Gain <> 0 Then
                        Gain = -(Data2.PeakZZ - Data1.PeakZZ) / (Z2 - Z1) / 1000
                        'If Gain < 0 Then
                        '    s = "X   estimated coef has the wrong sign!"
                        'End If
                        mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.lensZoverlap, Gain)
                        Gain = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.lensZoverlap)
                        Zset = Z2 + (Data2.PeakZZ - BeamZZ2) / Gain / 1000
                    Else
                        mMsgInfo.PostMessage("X   Beam is not moving with the stage Z move")
                        success = False
                        Exit While
                    End If
                End If

                'rotate buffer
                Y1 = Y2
                Z1 = Z2
                Data1 = Data2
                BeamYY1 = BeamYY2
                BeamZZ1 = BeamZZ2

                'time out
                i += 1
                If i = 10 Then
                    mMsgInfo.PostMessage("X   Still cannot find optimized position after multiple trials")
                    success = False
                    Exit While
                End If
            End While

            'recover speed
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageY)
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)
            mPara.SaveOverlapCoefToFile()
            Return success
        End Function

        Private Function MoveStageForLensAlignment(ByVal Y As Double, ByVal Z As Double, ByVal Y0 As Double, ByVal Z0 As Double, ByRef ZCapMin As Integer, ByRef ZCapMax As Integer) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim DeltaY, DeltaZ As Double

            'get the amount of move
            DeltaY = Y - Y0
            DeltaZ = Z - Z0

            'check range
            If Math.Abs(DeltaY) > mPara.Alignment.MaxStageMoveY Then
                s = "X   Required X adjustment {0:0.0000} is more than what is allowed {1:0.0000} "
                s = String.Format(s, DeltaY, mPara.Alignment.MaxStageMoveY)
                mMsgInfo.PostMessage(s)
                Return False
            End If

            If Math.Abs(DeltaZ) > mPara.Alignment.MaxStageMoveZ Then
                s = "X   Required Z adjustment {0:0.0000} is more than what is allowed {1:0.0000} "
                s = String.Format(s, DeltaZ, mPara.Alignment.MaxStageMoveZ)
                mMsgInfo.PostMessage(s)
                Return False
            End If

            'cap Z limt and try again
            If Z < mPara.Alignment.StageMinZ Or Z > mPara.Alignment.StageMaxZ Then
                s = "!   Required Z position {0:0.0000} is beyond the Z limit {1:0.0000, 2:0.000} per bond line thickness spec"
                s = String.Format(s, Z, mPara.Alignment.StageMinZ, mPara.Alignment.StageMaxZ)
                mMsgInfo.PostMessage(s)

                If Z < mPara.Alignment.StageMinZ Then
                    ZCapMin += 1
                    Z = mPara.Alignment.StageMinZ
                End If

                If Z > mPara.Alignment.StageMaxZ Then
                    ZCapMax += 1
                    Z = mPara.Alignment.StageMaxZ
                End If

                If ZCapMin > 1 Or ZCapMax > 1 Then
                    mMsgInfo.PostMessage("X   We have capped Z range once, still cannot pass. Try rotate lens!")
                    Return False
                End If
            End If

            'move the stage Y and Z
            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Y)
            If Not success Then
                Me.ShowStageError("Stage Y")
                Return False
            End If

            success = mStage.MoveStageNoWait(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z)
            If Not success Then
                Me.ShowStageError("Stage Z")
                Return False
            End If

            'wait 
            success = Me.WaitStageToStop("Wait Stage Y and Z to finish the move", False)
            If Not success Then Return False

            'wait for a while until beam is stable on the detector
            mTool.WaitForTime(1.0, "Wait for stage and beam to stable!")

            'done
            Return success
        End Function

        Private Function GetBeamCenter_AlignedChannels(ByVal NewChannel As Integer, ByRef YY As Double, ByRef ZZ As Double) As Boolean
            Dim i As Integer
            Dim s As String
            Dim success As Boolean
            Dim data As BeamWaistData
            Dim Channels As New List(Of Integer)
            'Added by Ming to simplify the poccess, we only need to CH2 info
            NewChannel = 1
            'get all aligned channels
            Channels = New List(Of Integer)
            Channels.Add(2)
            'Channels.Add(mBaseChannel)
            For i = 1 To NewChannel - 1
                If i = mBaseChannel Then Continue For
                Channels.Add(i)
            Next

            'get average beam center for the aligned channels
            YY = 0.0
            ZZ = 0.0
            For Each i In Channels
                mTool.mInst.LDD.TurnSingleChannelOn(i)

                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Return False

                s = Me.GetFormattedString_BeamOverlapBase(True, i.ToString(), data.PeakYY, data.PeakZZ)
                mMsgInfo.PostMessage(s)

                YY += data.PeakYY
                ZZ += data.PeakZZ
            Next

            If Channels.Count > 1 Then
                YY /= Channels.Count
                ZZ /= Channels.Count

                s = Me.GetFormattedString_BeamOverlapBase(True, "AVE", data.PeakYY, data.PeakZZ)
                mMsgInfo.PostMessage(s)
            End If

            Return True
        End Function

        Private Function GetBeamCenter_AlignedChannels_GoodOne(ByVal NewChannel As Integer, ByRef YY As Double, ByRef ZZ As Double) As Boolean
            Dim i As Integer
            Dim s As String
            Dim success As Boolean
            Dim data As BeamWaistData
            Dim Channels As New List(Of Integer)

            'get all aligned channels
            Channels = New List(Of Integer)
            Channels.Add(mBaseChannel)
            For i = 1 To NewChannel - 1
                If i = mBaseChannel Then Continue For
                Channels.Add(i)
            Next

            'get average beam center for the aligned channels
            YY = 0.0
            ZZ = 0.0
            For Each i In Channels
                mTool.mInst.LDD.TurnSingleChannelOn(i)

                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Return False

                s = Me.GetFormattedString_BeamOverlapBase(True, i.ToString(), data.PeakYY, data.PeakZZ)
                mMsgInfo.PostMessage(s)

                YY += data.PeakYY
                ZZ += data.PeakZZ
            Next

            If Channels.Count > 1 Then
                YY /= Channels.Count
                ZZ /= Channels.Count

                s = Me.GetFormattedString_BeamOverlapBase(True, "AVE", data.PeakYY, data.PeakZZ)
                mMsgInfo.PostMessage(s)
            End If

            Return True
        End Function
#End Region

#Region "lens align ZY scan, angle adjust"
        Public Function AlignLensForSteering_FinePlusScan(ByVal Channel As Integer, ByVal RangeY As Double, ByVal YStep As Double, ByVal RangeZ As Double, ByVal ZStep As Double) As Boolean
            Dim success As Boolean
            'fast fine adjustment
            success = Me.AlignLensForSteering_Fine(Channel)
            If success Then Return True
            'fail safe
            success = Me.AlignLensByScan_NearFar(Channel, RangeY, YStep, RangeZ, ZStep)
            Return success
        End Function

        Public Function AlignLensForSteering_OverlapPlusScan(ByVal Channel As Integer, ByVal RangeY As Double, ByVal YStep As Double, ByVal RangeZ As Double, ByVal ZStep As Double) As Boolean
            Dim success As Boolean
            'fast fine adjustment
            success = Me.AlignLensForSteering_Overlap(Channel)
            If success Then Return True
            'fail safe
            success = Me.AlignLensByScan_Overlap(Channel, RangeY, YStep, RangeZ, ZStep)
            Return success
        End Function

        Public Function AlignLensByScan_NearFar(ByVal Channel As Integer, ByVal RangeY As Double, ByVal YStep As Double, ByVal RangeZ As Double, ByVal ZStep As Double) As Boolean
            Dim s, fmt As String
            Dim success As Boolean
            Dim k, kStart, kStop, kStep As Integer
            Dim iY, iiY, iZ, iiZ, iMinY, iMinZ As Integer
            Dim v, vX(1), Y0, Z0 As Double
            Dim deltaY, deltaZ, delta, deltaMin As Double
            Dim Y, Z As List(Of Double)
            Dim BeamY()()(), BeamZ()()() As Double
            Dim BeamData As BeamWaistData

            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,8:0.0000}", "{2,8:0.0000}", "{3,8:0.0000}", "{4,8:0.0000}", _
                                                                    "{5,8:0.0000}", "{6,8:0.0000}", "{7,8:0.0000}", "{8,8:0.0000}")

            'ack
            mMsgInfo.PostMessage("   Align lens by scanning stage positions for best beam position offset at near and far ... ")

            'turn channel on
            'mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'validate
            s = "!   {0} Scan range {1:0.0000} > max spec {2:0.0000}. Bound the range to the max."
            If RangeY > mPara.Alignment.MaxStageMoveY Then
                mMsgInfo.PostMessage(String.Format(s, "Y", RangeY, mPara.Alignment.MaxStageMoveY))
                RangeY = mPara.Alignment.MaxStageMoveY
            End If

            If RangeZ > mPara.Alignment.MaxStageMoveZ Then
                mMsgInfo.PostMessage(String.Format(s, "Z", RangeZ, mPara.Alignment.MaxStageMoveZ))
                RangeZ = mPara.Alignment.MaxStageMoveZ
            End If

            'build array
            Y0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY) - 0.5 * RangeY
            Z0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ) - 0.5 * RangeZ

            Y = New List(Of Double)
            v = Y0
            While (v - Y0) <= RangeY
                Y.Add(v)
                v += YStep
            End While

            Z = New List(Of Double)
            v = Z0
            While (v - Z0) <= RangeZ
                Z.Add(v)
                v += ZStep
            End While

            'dimension
            iiY = Y.Count - 1
            iiZ = Z.Count - 1
            ReDim BeamY(1)
            ReDim BeamZ(1)
            For k = 0 To 1
                ReDim BeamY(k)(iiY)
                ReDim BeamZ(k)(iiY)
                For iY = 0 To iiY
                    ReDim BeamY(k)(iY)(iiZ)
                    ReDim BeamZ(k)(iY)(iiZ)
                Next
            Next

            'check current beam scan position to make least amount of move
            v = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)
            vX(0) = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanNear).X
            vX(1) = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            If Math.Abs(v - vX(0)) < Math.Abs(v - vX(1)) Then
                kStart = 0
                kStop = 1
            Else
                kStart = 1
                kStop = 0
            End If
            kStep = kStop - kStart

            'do loop now
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, mPara.Alignment.StageSpeed)
            'mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.Alignment.StageSpeed)
            deltaMin = 200
            For k = kStart To kStop Step kStep
                'header
                s = ControlChars.CrLf + String.Format(fmt, "Stage Y", "Stage Z", "BeamY0", "BeamZ0", "BeamYn", "BeamZn", "DeltaY", "DeltaZ", "DeltaR")
                mMsgInfo.PostMessage(s)

                'move beam scan
                mMsgInfo.PostMessage("      Beam Scan at " + vX(k).ToString("0.000"))
                success = Me.MoveBeamGage(vX(k), False)
                If Not success Then Exit For

                For iY = 0 To iiY
                    'move stage Y
                    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Y(iY), False)
                    If Not success Then Exit For

                    For iZ = 0 To iiZ
                        'check stop
                        If mTool.CheckStop() Then
                            success = False
                            Exit For
                        End If

                        'move stage Z
                        success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z(iZ), False)
                        If Not success Then Exit For

                        'get beam scan data
                        success = Me.GetBeamScanData(Double.NaN, False, False, BeamData)
                        If Not success Then Return False
                        BeamY(k)(iY)(iZ) = BeamData.PeakYY
                        BeamZ(k)(iY)(iZ) = BeamData.PeakZZ

                        If k = 0 Then
                            'display data
                            s = String.Format(fmt, Y(iY), Z(iZ), BeamY(k)(iY)(iZ), BeamZ(k)(iY)(iZ), "", "", "", "", "")
                        Else
                            'calculate delta and keep track of the mim - we are using the distance in YZ plane
                            deltaY = BeamY(k)(iY)(iZ) - BeamY(0)(iY)(iZ)
                            deltaZ = BeamZ(k)(iY)(iZ) - BeamZ(0)(iY)(iZ)
                            delta = Math.Sqrt(deltaY ^ 2 + deltaZ ^ 2)
                            s = String.Format(fmt, Y(iY), Z(iZ), BeamY(0)(iY)(iZ), BeamZ(0)(iY)(iZ), BeamY(k)(iY)(iZ), BeamZ(k)(iY)(iZ), deltaY, deltaZ, delta)
                            If delta > deltaMin Then
                                deltaMin = delta
                                iMinY = iY
                                iMinZ = iZ
                                s += ControlChars.Tab + "<- New Min"
                            End If
                        End If
                        mMsgInfo.PostMessage(s)

                    Next iZ

                    If Not success Then Exit For
                Next iY

                If Not success Then Exit For
            Next k

            If success Then
                s = ControlChars.CrLf + "      Found min beam steering {0,8:0.0000} at stage (Y, Z) = ({1,8:0.0000}, {2,8:0.0000})"
                s = String.Format(s, deltaMin, Y(iMinY), Z(iMinZ))
                mMsgInfo.PostMessage(s)

                'check spec
                If deltaMin < mPara.Alignment.MaxSteeringY And deltaMin < mPara.Alignment.MaxSteeringZ Then
                    success = True
                    s = "      ^ Setting meet the spec."
                Else
                    success = False
                    s = "X   Min steering is out of spec "
                End If
                mMsgInfo.PostMessage(s)

                'move stage now
                If success Then success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Y(iMinY), False)
                If success Then success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z(iMinZ), False)
            End If

            'recover stage speed
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageY)
            'mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            Return success
        End Function

        Public Function AlignLensByScan_Overlap(ByVal Channel As Integer, ByVal RangeY As Double, ByVal YStep As Double, ByVal RangeZ As Double, ByVal ZStep As Double) As Boolean
            Dim s, fmt As String
            Dim success As Boolean
            Dim k, iY, iiY, iZ, iiZ, iMinY, iMinZ As Integer
            Dim v, Y0, Z0 As Double
            Dim deltaY, deltaZ, delta, deltaMin As Double
            Dim Y, Z As List(Of Double)
            Dim BeamY()()(), BeamZ()()() As Double
            Dim BeamData As BeamWaistData

            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,8:0.0000}", "{1,8:0.0000}", "{2,8:0.0000}", "{3,8:0.0000}", "{4,8:0.0000}", _
                                                                    "{5,8:0.0000}", "{6,8:0.0000}", "{7,8:0.0000}", "{8,8:0.0000}")

            'ack
            mMsgInfo.PostMessage("   Align lens by scanning stage positions for best beam position offset at near and far ... ")

            'turn channel on
            mTool.mInst.LDD.TurnSingleChannelOn(Channel)

            'validate
            s = "!   {0} Scan range {1:0.0000} > max spec {2:0.0000}. Bound the range to the max."
            If RangeY > mPara.Alignment.MaxStageMoveY Then
                mMsgInfo.PostMessage(String.Format(s, "Y", RangeY, mPara.Alignment.MaxStageMoveY))
                RangeY = mPara.Alignment.MaxStageMoveY
            End If

            If RangeZ > mPara.Alignment.MaxStageMoveZ Then
                mMsgInfo.PostMessage(String.Format(s, "Z", RangeZ, mPara.Alignment.MaxStageMoveZ))
                RangeZ = mPara.Alignment.MaxStageMoveZ
            End If

            'build array
            Y0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY) - 0.5 * RangeY
            Z0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ) - 0.5 * RangeZ

            Y = New List(Of Double)
            v = Y0
            While (v - Y0) <= RangeY
                Y.Add(v)
                v += YStep
            End While

            Z = New List(Of Double)
            v = Z0
            While (v - Z0) <= RangeZ
                Z.Add(v)
                v += ZStep
            End While

            'dimension
            iiY = Y.Count - 1
            iiZ = Z.Count - 1
            ReDim BeamY(1)
            ReDim BeamZ(1)
            For k = 0 To 1
                ReDim BeamY(k)(iiY)
                ReDim BeamZ(k)(iiY)
                For iY = 0 To iiY
                    ReDim BeamY(k)(iY)(iiZ)
                    ReDim BeamZ(k)(iY)(iiZ)
                Next
            Next

            'header
            s = ControlChars.CrLf + String.Format(fmt, "Stage Y", "Stage Z", "BeamRefY", "BeamRefZ", "BeamChY", "BeamChZ", "DeltaY", "DeltaZ", "DeltaR")
            mMsgInfo.PostMessage(s)

            'move beam scan to far size
            v = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            success = Me.MoveBeamGage(v, False)
            If Not success Then Return False

            'loop through stage positions
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, mPara.Alignment.StageSpeed)
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.Alignment.StageSpeed)

            deltaMin = Double.MaxValue
            For iY = 0 To iiY
                'move stage Y
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Y(iY), False)
                If Not success Then Exit For

                For iZ = 0 To iiZ
                    'check stop
                    If mTool.CheckStop() Then
                        success = False
                        Exit For
                    End If

                    'move stage Z
                    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z(iZ), False)
                    If Not success Then Exit For

                    'get data for the aligned channels
                    success = Me.GetBeamCenter_AlignedChannels(Channel, BeamY(0)(iY)(iZ), BeamZ(0)(iY)(iZ))
                    If Not success Then Return False

                    'get beam scan data this channel
                    mTool.mInst.LDD.TurnSingleChannelOn(Channel)
                    success = Me.GetBeamScanData(Double.NaN, False, False, BeamData)
                    If Not success Then Return False
                    BeamY(1)(iY)(iZ) = BeamData.PeakYY
                    BeamZ(1)(iY)(iZ) = BeamData.PeakZZ

                    'calculate delta and keep track of the mim - we are using the distance in YZ plane
                    deltaY = BeamY(1)(iY)(iZ) - BeamY(0)(iY)(iZ)
                    deltaZ = BeamZ(1)(iY)(iZ) - BeamZ(0)(iY)(iZ)
                    delta = Math.Sqrt(deltaY ^ 2 + deltaZ ^ 2)
                    s = String.Format(fmt, Y(iY), Z(iZ), BeamY(0)(iY)(iZ), BeamZ(0)(iY)(iZ), BeamY(1)(iY)(iZ), BeamZ(1)(iY)(iZ), deltaY, deltaZ, delta)
                    If delta > deltaMin Then
                        deltaMin = delta
                        iMinY = iY
                        iMinZ = iZ
                        s += ControlChars.Tab + "<- New Min"
                    End If

                    mMsgInfo.PostMessage(s)
                Next iZ

                If Not success Then Exit For
            Next iY

            If success Then
                s = ControlChars.CrLf + "      Found min beam overlap {0,8:0.0000} at stage (Y, Z) = ({1,8:0.0000}, {2,8:0.0000})"
                s = String.Format(s, deltaMin, Y(iMinY), Z(iMinZ))
                mMsgInfo.PostMessage(s)

                'check spec
                'check spec
                If deltaMin < mPara.Alignment.MaxSteeringY And deltaMin < mPara.Alignment.MaxSteeringZ Then
                    success = True
                    s = "      ^ Setting meet the spec."
                Else
                    success = False
                    s = "X   Min steering is out of spec "
                End If
                mMsgInfo.PostMessage(s)

                'move stage now
                If success Then success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Y(iMinY), False)
                If success Then success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, Z(iMinZ), False)
            End If

            'recover stage speed
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageY)
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            Return success
        End Function

        Public Function AlignLensOverlapByAngle(ByVal Channel As Integer, ByVal Range As Double, ByVal StepSize As Double) As Boolean
            Dim s As String
            Dim i, ii, iMin As Integer
            Dim success As Boolean
            Dim v, BaseYY, BaseZZ As Double
            Dim StageY, StageZ, Angle0 As Double
            Dim DeltaYY, DeltaZZ, DeltaMin As Double
            Dim AngleList As List(Of Double)
            Dim BeamData As BeamWaistData

            'ack
            mMsgInfo.PostMessage("    Adjust lens angle to improve beam over ... ")

            'we do not need to do CH2
            If Channel = mBaseChannel Then
                s = "      CH {0:0} is base channel and self aligned. No overlap aligment is needed."
                s = String.Format(s, mBaseChannel)
                mMsgInfo.PostMessage(s)
                Return True
            End If

            'bound range
            s = "!   {0} Scan range {1:0.0000} > max spec {2:0.0000}. Bound the range to the max."
            If Range > mPara.Alignment.MaxStageMoveAngle Then
                mMsgInfo.PostMessage(String.Format(s, "Angle", Range, mPara.Alignment.MaxStageMoveAngle))
                Range = mPara.Alignment.MaxStageMoveAngle
            End If

            'get stage position
            StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            Angle0 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)

            'build array
            Angle0 -= 0.5 * Range

            AngleList = New List(Of Double)
            v = Angle0
            While (v - Angle0) <= Range
                AngleList.Add(v)
                v += StepSize
            End While

            'get the bease - we are only adjusting the lens angle, stage XYZ remains the same, thus beam posiiton for the ref channels shall remain the same
            s = Me.GetFormattedString_BeamOverlapHeader(True)
            mMsgInfo.PostMessage(s)

            success = Me.GetBeamCenter_AlignedChannels(Channel, BaseYY, BaseZZ)
            If Not success Then Return False

            'loop 
            ii = AngleList.Count - 1

            For i = 0 To ii
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit For
                End If

                'move stage
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, AngleList(i), False)
                If Not success Then Exit For

                'get beam scan data
                mTool.mInst.LDD.TurnSingleChannelOn(Channel)
                success = Me.GetBeamScanData(Double.NaN, False, False, BeamData)
                DeltaYY = BeamData.WidthYY - BaseYY
                DeltaZZ = BeamData.WidthZZ - BaseZZ

                'show overlap
                s = Me.GetFormattedString_BeamOverlap(True, Channel, BeamData.FWHMYY, BeamData.FWHMZZ, DeltaYY, DeltaZZ, StageY, StageZ, AngleList(i))

                'keep track of min
                v = Math.Abs(DeltaYY)
                If i = 0 Or v < DeltaMin Then
                    iMin = i
                    DeltaMin = v
                    s += ControlChars.Tab + " <- Y overlap improved"
                End If

                mMsgInfo.PostMessage(s)
            Next

            If success Then
                s = "      Found minimum YY overlap {0:0.0000} at at angle {1:0.0000}"
                s = String.Format(s, DeltaMin, AngleList(iMin))

                'move stage to the best angle, in two steps, first to the min then the best - this is to get better repeatability
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, AngleList(0), False)
                If success Then
                    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Absolute, AngleList(iMin), False)
                End If
            End If


            Return success
        End Function

        Public Function MeasureOverlapPitch(ByVal Channel As Integer) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim i, iStart, iStop, iStep As Integer
            Dim v, scale, StageY, StageZ, StageAngle, pitchYY, pitchZZ As Double
            Dim BeamScanX(1), BeamYY(1), BeamZZ(1), RefYY(1), RefZZ(1) As Double
            Dim data As BeamWaistData
            Dim spec As Double

            '2-item array
            Const iNear As Integer = 0
            Const iFar As Integer = 1

            'ack
            mMsgInfo.PostMessage("   Measure beam overlap at near and far side, calculate pitch at pigtail ... ")

            'we do not need to do CH2
            If Channel = mBaseChannel Then
                s = "      CH {0:0} is base channel and self aligned. No overlap aligment is needed."
                s = String.Format(s, mBaseChannel)
                mMsgInfo.PostMessage(s)
                Return True
            End If

            'get stage info
            StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            StageAngle = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)

            'get beam scan position and decide where to move
            v = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)
            BeamScanX(iFar) = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            BeamScanX(iNear) = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanNear).X

            s = Me.GetFormattedString_BeamDataHeader(True)
            mMsgInfo.PostMessage(s)

            If Math.Abs(v - BeamScanX(iFar)) < Math.Abs(v - BeamScanX(iNear)) Then
                'current position closer to far position
                iStart = iFar
                iStop = iNear
            Else
                'current position closer to near position
                iStart = iNear
                iStop = iFar
            End If
            iStep = (iStop - iStart)

            For i = iStart To iStop Step iStep
                'move to the closer side
                success = Me.MoveBeamGage(BeamScanX(i), False)
                If Not success Then Return False

                'get the ref
                success = Me.GetBeamCenter_AlignedChannels(Channel, RefYY(i), RefZZ(i))
                If Not success Then Return False

                'get active one
                mTool.mInst.LDD.TurnSingleChannelOn(Channel)
                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Return False

                BeamYY(i) = data.PeakYY
                BeamZZ(i) = data.PeakZZ

                'show next
                If i = iNear Then
                    s = "      Overlap at near side" + ControlChars.CrLf
                Else
                    s = "      Overlap at  far side" + ControlChars.CrLf
                End If

                s += Me.GetFormattedString_BeamOverlap(True, Channel, BeamYY(i), BeamZZ(i), (BeamYY(i) - RefYY(i)), (BeamZZ(i) - RefZZ(i)), StageY, StageZ, StageAngle)
                mMsgInfo.PostMessage(s)
            Next

            'calculate the "pitch"
            scale = Math.Abs((mPara.Alignment.BeamScanXOffset) / (BeamScanX(iFar) - BeamScanX(iNear)))

            v = (BeamYY(iFar) - RefYY(iFar)) - (BeamYY(iNear) - RefYY(iNear))
            pitchYY = (BeamYY(iNear) - RefYY(iNear)) - v * scale

            v = (BeamZZ(iFar) - RefZZ(iFar)) - (BeamZZ(iNear) - RefZZ(iNear))
            pitchZZ = (BeamZZ(iNear) - RefZZ(iNear)) - v * scale

            v = Math.Sqrt(pitchYY ^ 2 + pitchZZ ^ 2)

            'show final result
            s = "      Overlap at pigtail Y = {0,8:0.0000}, Z = {1,8:0.0000}, Overall = {2,8:0.0000}"
            s = String.Format(s, pitchYY, pitchZZ, v)
            mMsgInfo.PostMessage(s)

            spec = mPara.Alignment.MaxPitchError * 1000
            'check spec
            If v < spec Then
                success = True
                s = "      ^ pitch error in spec"
            Else
                success = False
                s = "X   Pitch error out of psec. Spec <= " + spec.ToString("0.0000")
            End If
            mMsgInfo.PostMessage(s)

            'this in theory should not fail. If fail, we can do angle adjustment in near side to reduce DeltaYY and some Z adjuistment to reduce ZZ. 
            'we will see what happens in real 

            mMsgInfo.PostMessage(s)
            Return success
        End Function

        Public Function MeasureAngleAndPitch() As Boolean
            Dim success As Boolean
            Dim s, header1, header2 As String
            Dim i, ii As Integer
            Dim BeamScanX(4), Xposition(4), XpositionActual(4) As Double
            Dim v As Double
            Dim data As BeamWaistData
            Dim BeamYY(4, 4), BeamZZ(4, 4), BeamSizeYY(4, 4), BeamSizeZZ(4, 4) As Double
            Dim Xmean(4), Ymean(4), Zmean(4), XYmean(4), XXmean(4), XZmean(4) As Double
            Dim kAngleY(4), bAngleY(4), kAngleZ(4), bAngleZ(4), kSizeY(4), bSizeY(4), kSizeZ(4), bSizeZ(4) As Double
            Dim Power(4, 4) As Double



            mMsgInfo.PostMessage("   Measure beam pitch and angle at far and near side for all channels...")

            v = mStage.GetStagePosition(iXpsStage.AxisNameEnum.BeamScanX)
            BeamScanX = {390, 300, 200, 100, 0}

            For i = 0 To 4
                Xposition(i) = 390 + 55 - BeamScanX(i)
                XpositionActual(i) = Xposition(i) * 1000
            Next

            header1 = "       CH{0}  {1,7:0.00}  {2,7:0.00}  {3,7:0.00}  {4,7:0.00} {5,4:0.00}"

            For i = 0 To 4
                'move to the closer side
                success = Me.MoveBeamGage(BeamScanX(i), False)
                If Not success Then Return False
                header2 = "Measuring beam info at {0,6:0.00}"
                s = String.Format(header2, Xposition(i))
                mMsgInfo.PostMessage(s)
                mMsgInfo.PostMessage("       CH   PosY     PosZ     SizeY    SizeZ   Power")

                For ii = 1 To 4

                    mTool.mInst.LDD.TurnSingleChannelOn(ii)

                    System.Threading.Thread.Sleep(200)
                    success = Me.GetBeamScanData(Double.NaN, False, False, data)
                    If Not success Then
                        BeamYY(i, ii) = 0
                        BeamZZ(i, ii) = 0
                        BeamSizeYY(i, ii) = 0
                        BeamSizeZZ(i, ii) = 0
                        Power(i, ii) = 0
                    Else
                        BeamYY(i, ii) = data.PeakYY
                        BeamZZ(i, ii) = data.PeakZZ
                        BeamSizeYY(i, ii) = data.WidthYY
                        BeamSizeZZ(i, ii) = data.WidthZZ
                        Power(i, ii) = data.PeakValue
                    End If
                    s = String.Format(header1, ii, BeamYY(i, ii), BeamZZ(i, ii), BeamSizeYY(i, ii), BeamSizeZZ(i, ii), Power(i, ii))
                    mMsgInfo.PostMessage(s)
                Next
            Next


            'Calculate k and b for angle
            For ii = 1 To 4
                For i = 0 To 4
                    Xmean(ii) = Xmean(ii) + XpositionActual(i)
                    Ymean(ii) = Ymean(ii) + BeamYY(i, ii)
                    Zmean(ii) = Zmean(ii) + BeamZZ(i, ii)
                    XXmean(ii) = XXmean(ii) + XpositionActual(i) * XpositionActual(i)
                    XYmean(ii) = XYmean(ii) + BeamYY(i, ii) * XpositionActual(i)
                    XZmean(ii) = XZmean(ii) + BeamZZ(i, ii) * XpositionActual(i)
                Next
                Xmean(ii) /= 5
                Ymean(ii) /= 5
                Zmean(ii) /= 5
                XXmean(ii) /= 5
                XYmean(ii) /= 5
                XZmean(ii) /= 5

                kAngleY(ii) = (XYmean(ii) - Xmean(ii) * Ymean(ii)) / (XXmean(ii) - Xmean(ii) * Xmean(ii))
                bAngleY(ii) = Ymean(ii) - kAngleY(ii) * Xmean(ii)

                kAngleZ(ii) = (XZmean(ii) - Xmean(ii) * Zmean(ii)) / (XXmean(ii) - Xmean(ii) * Xmean(ii))
                bAngleZ(ii) = Zmean(ii) - kAngleZ(ii) * Xmean(ii)
            Next
            'Calculate k and b for beam size
            ReDim Xmean(4), Ymean(4), Zmean(4), XXmean(4), XYmean(4), XZmean(4)
            For ii = 1 To 4
                For i = 0 To 4
                    Xmean(ii) = Xmean(ii) + XpositionActual(i)
                    Ymean(ii) = Ymean(ii) + BeamSizeYY(i, ii)
                    Zmean(ii) = Zmean(ii) + BeamSizeZZ(i, ii)
                    XXmean(ii) = XXmean(ii) + XpositionActual(i) * XpositionActual(i)
                    XYmean(ii) = XYmean(ii) + BeamSizeYY(i, ii) * XpositionActual(i)
                    XZmean(ii) = XZmean(ii) + BeamSizeZZ(i, ii) * XpositionActual(i)
                Next
                Xmean(ii) /= 5
                Ymean(ii) /= 5
                Zmean(ii) /= 5
                XXmean(ii) /= 5
                XYmean(ii) /= 5
                XZmean(ii) /= 5

                kSizeY(ii) = (XYmean(ii) - Xmean(ii) * Ymean(ii)) / (XXmean(ii) - Xmean(ii) * Xmean(ii))
                bSizeY(ii) = Ymean(ii) - kSizeY(ii) * Xmean(ii)

                kSizeZ(ii) = (XZmean(ii) - Xmean(ii) * Zmean(ii)) / (XXmean(ii) - Xmean(ii) * Xmean(ii))
                bSizeZ(ii) = Zmean(ii) - kSizeZ(ii) * Xmean(ii)
            Next


            mMsgInfo.PostMessage("   Measure completed and the results are shown as below:")

            header1 = "             AngleY     AngleZ     PitchY      PitchZ       Ty         Tz        Power"
            mMsgInfo.PostMessage(header1)
            header1 = "      CH {0}:  {1,7:0.0000}    {2,7:0.0000}    {3,8:0.00}    {4,8:0.00}    {5,7:0.0000}    {6,7:0.0000}    {7,4:0.00}"
            For ii = 1 To 4
                If ii = 2 Then
                    s = String.Format(header1, ii, Math.Atan(kAngleY(ii)), Math.Atan(kAngleZ(ii)), bAngleY(ii), bAngleZ(ii), Math.Atan(kSizeY(ii)), Math.Atan(kSizeZ(ii)), Power(4, ii))
                Else
                    s = String.Format(header1, ii, Math.Atan(kAngleY(ii)), Math.Atan(kAngleZ(ii)), (bAngleY(ii) - bAngleY(2)), (bAngleZ(ii) - bAngleZ(2)), Math.Atan(kSizeY(ii)), Math.Atan(kSizeZ(ii)), Power(4, ii))
                End If

                mMsgInfo.PostMessage(s)
            Next

            Return success
        End Function




#End Region

#Region "PBS alignment"
        Public Function AlignPbsByAngle(ByVal PbsChannel As Integer) As Boolean
            Dim LensChannels() As Integer
            Dim data0, data1, data2 As BeamWaistData()
            Dim k, i, ii, CH As Integer
            Dim v, angle1, angle2, deltaY(), deltaZ(), deltaAngle() As Double
            Dim s, fmt, hdr, sInSpec As String
            Dim success As Boolean

            Const zMove As Double = 2.0
            Const MaxAngleChange As Double = 3.0

            fmt = "      " + w2String.Concatenate(ControlChars.Tab, "{0,3:0}", "{1,8:000}", "{2,8:0000}", "{3,8:0.0000}", "{4,8:0.0000}", "{5,8:0.0000}", "{6,8:0.0000}", "{7,8:0.0000}")
            hdr = String.Format(fmt, "CH", "Angle", "DeltaY", "DeltaZ", "BeamYn", "BeamZn", "BeamY0", "BeamZ0")

            sInSpec = "      Beam position change DeltaY = {0:0.0000} in spec {1:0.0000}}"

            'ack
            s = "    Align PBS" + PbsChannel.ToString() + " to ensure normal incident ... "
            mMsgInfo.PostMessage(s)

            'get channels
            LensChannels = Me.GetLensChannels(PbsChannel)
            If LensChannels.Length = 0 Then
                s = "X   Invalid PSB channel to be aligned. Valid number is between [1, 3]"
                mMsgInfo.PostMessage(s)
                Return False
            End If

            'dimension
            ii = LensChannels.Length - 1
            ReDim data0(ii), data1(ii), data2(ii), deltaY(ii), deltaZ(ii), deltaAngle(ii)

            'get beam scan position at far side and move it
            v = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            success = Me.MoveBeamGage(v, False)
            If Not success Then Return False

            'lift the part and measure the original beam position
            mMsgInfo.PostMessage("      Life Z stage and measure the original beam position w/o PBS")
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, -zMove, False)
            If Not success Then Return False

            mMsgInfo.PostMessage(hdr)
            angle1 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)
            For i = 0 To ii
                CH = LensChannels(i)
                mTool.mInst.LDD.TurnSingleChannelOn(CH)
                success = Me.GetBeamScanData(Double.NaN, False, False, data0(i))
                If Not success Then Return False

                s = String.Format(fmt, CH, angle1, "", "", "", "", data0(i).PeakYY, data0(i).PeakZZ)
                mMsgInfo.PostMessage(s)
            Next

            'move PBS back
            mMsgInfo.PostMessage("      Put Z stage back and measure the beam position with PBS ")
            success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, zMove, False)
            If Not success Then Return False

            'loop 
            k = 0
            While True
                'check stop
                If mTool.CheckStop() Then
                    success = False
                    Exit While
                End If

                'rotate angle now
                If k = 0 Then
                    success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.AngleMain, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, deltaAngle.Average(), False)
                    If Not success Then Exit While
                End If

                'get alignment data
                mMsgInfo.PostMessage(hdr)
                angle2 = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)
                For i = 0 To ii
                    CH = LensChannels(i)
                    mTool.mInst.LDD.TurnSingleChannelOn(CH)
                    success = Me.GetBeamScanData(Double.NaN, False, False, data2(i))
                    If Not success Then Return False

                    deltaY(i) = data2(i).PeakYY - data0(i).PeakYY
                    deltaZ(i) = data2(i).PeakZZ - data0(i).PeakZZ
                    s = String.Format(fmt, CH, angle2, deltaY(i), deltaZ(i), data2(i).PeakYY, data2(i).PeakZZ, data1(i).PeakYY, data1(i).PeakZZ)
                    mMsgInfo.PostMessage(s)
                Next
                If deltaY.Length > 1 Then
                    s = String.Format(fmt, "Ave", angle2, deltaY.Average(), deltaZ.Average(), "", "", "", "")
                End If

                'check exit
                If k > 0 Then
                    v = deltaY.Average()
                    If Math.Abs(v) < mPara.Alignment.MaxSteeringY Then
                        s = String.Format(sInSpec, v, mPara.Alignment.MaxSteeringY)
                        mMsgInfo.PostMessage(s)
                        success = True

                        'note the Z probelm if any
                        v = deltaZ.Average()
                        If Math.Abs(v) < mPara.Alignment.MaxSteeringZ Then
                            sInSpec = "      Beam position change DeltaZ = {0:0.0000} in  spec {1:0.0000}}"
                        Else
                            sInSpec = "!     Beam position change DeltaZ = {0:0.0000} off spec {1:0.0000}}. But there is no Z adjustment capability here."
                        End If
                        s = String.Format(sInSpec, v, mPara.Alignment.MaxSteeringZ)
                        mMsgInfo.PostMessage(s)

                        Exit While
                    End If
                End If

                'change angle if needed
                If k > 0 Then
                    'we have (Angle2 - Angle1) ==> (data2(i).PeakY - data1(i).PeakY)
                    'we just update the new ceofficient
                    For i = 0 To ii
                        v = (angle2 - angle1) / (data2(i).PeakYY - data1(i).PeakYY)
                        mPara.UpdateAlignCoef(BlackHawkParameters.AlignCoefEnum.PbsAngle, v)
                    Next
                End If
                v = mPara.AlignCoef(BlackHawkParameters.AlignCoefEnum.PbsAngle)
                For i = 0 To ii
                    deltaAngle(i) = -deltaY(i) / v
                Next

                'validate the angle change
                v = deltaAngle.Average()
                If Math.Abs(v) > MaxAngleChange Then
                    s = "X   Required angle change {0:0.000} is more than the maximum allowed {1:0.000}"
                    s = String.Format(s, v, MaxAngleChange)
                    MessageBox.Show(s)
                    success = False
                    Return False
                ElseIf v = 0 Then
                    s = "X   Required angle change is zero on average, we cannot make all channels overlap"
                    MessageBox.Show(s)
                    success = False
                    Return False
                End If

                'rotate buffer
                data1 = data2
                angle1 = angle2

                'next
                k += 1
                If k = 10 Then
                    mMsgInfo.PostMessage("X   Still cannot find optimized position after multiple trials")
                    success = False
                    Exit While
                End If
            End While

            Return success
        End Function

        Private Function GetLensChannels(ByVal PbsChannel As Integer) As Integer()
            Select Case PbsChannel
                Case 1
                    Return New Integer() {mBaseChannel}
                Case 2
                    Return New Integer() {1, mBaseChannel}
                Case 3
                    Return New Integer() {3}
                Case Else
                    Return New Integer() {}
            End Select
        End Function
#End Region

#Region "Record data, post epoxy alignment"
#Region "alignment result"
        Private Enum ProcessStepEnum
            InitialAlign
            PostEpoxy
            PostUvCure
        End Enum

        Private Structure AlignmentResult
            Public HaveData As Boolean

            Public StageX As Double
            Public StageY As Double
            Public StageZ As Double
            Public StageAngle As Double

            Public IsAbsoluteBeamPosition As Boolean
            Public BeamPeak As Double
            Public BeamCenterYY As Double
            Public BeamCenterZZ As Double

            Public Overloads Shared Operator -(ByVal A As AlignmentResult, ByVal B As AlignmentResult) As AlignmentResult
                Dim C As AlignmentResult

                C.StageAngle = A.StageAngle - B.StageAngle
                C.StageX = A.StageX - B.StageX
                C.StageY = A.StageY - B.StageY
                C.StageZ = A.StageZ - B.StageZ

                C.BeamPeak = A.BeamPeak - B.BeamPeak
                C.BeamCenterYY = A.BeamCenterYY - B.BeamCenterYY
                C.BeamCenterZZ = A.BeamCenterZZ - B.BeamCenterZZ

                Return C
            End Operator

            Public ReadOnly Property TableHeader() As String
                Get
                    If IsAbsoluteBeamPosition Then
                        Return w2String.Concatenate(ControlChars.Tab, "Proc Step", "CH", "PeakYY", "PeakZZ", "PeakPow", "StageX", "StageY", "StageZ", "Angle")
                    Else
                        Return w2String.Concatenate(ControlChars.Tab, "Proc Step", "CH", "DeltaYY", "DeltaZZ", "PeakPow", "StageX", "StageY", "StageZ", "Angle")
                    End If

                End Get
            End Property

            Public Function GetTableText(ByVal sStep As String, ByVal sCH As String) As String
                Dim s As String
                s = w2String.Concatenate(ControlChars.Tab, "{0,14}", "{1,3:0}", "{2,8:0.00000}", "{3,8:0.00000}", "{4,8:0.000}", "{5,8:0.0000}", "{6,8:0.0000}", "{7,8:0.0000}", "{8,8:0.000}")
                Return String.Format(s, sStep, sCH, BeamCenterYY, BeamCenterZZ, BeamPeak, StageX, StageY, StageZ, StageAngle)
            End Function

        End Structure

        Private mAlignData([Enum].GetValues(GetType(ProcessStepEnum)).Length - 1) As AlignmentResult

        Public Sub ClearRecordedData()
            Dim i As Integer
            For i = 0 To mAlignData.Length - 1
                mAlignData(i).HaveData = False
            Next i
        End Sub
#End Region

        Public Function RecordData(ByVal sStep As String, ByVal sPart As String) As Boolean
            Dim s As String
            Dim i As Integer
            Dim eStep As ProcessStepEnum
            Dim ePart As iXpsStage.PartEnum

            'valid enumeration passdown
            Try
                eStep = CType(sStep, ProcessStepEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong process step identification: " + sStep + "Error: " + ex.Message)
                Return False
            End Try

            'valid enumeration passdown
            Try
                ePart = CType(sPart, iXpsStage.PartEnum)
            Catch ex As Exception
                mMsgInfo.PostMessage("X   Wrong part identification: " + sStep + "Error: " + ex.Message)
                Return False
            End Try

            'validate old data
            For i = 0 To eStep - 1
                If Not mAlignData(i).HaveData Then
                    s = CType(i, ProcessStepEnum).ToString()
                    mMsgInfo.PostMessage("X   There is no data for the prior step: " + s)
                    Return False
                End If
            Next

            'actual work
            Select Case ePart
                'PBS
                Case iXpsStage.PartEnum.BS1
                    Return Me.RecordData_Pbs(eStep, 1)
                Case iXpsStage.PartEnum.PBS
                    Return Me.RecordData_Pbs(eStep, 2)
                Case iXpsStage.PartEnum.BS2
                    Return Me.RecordData_Pbs(eStep, 3)

                    'Lens
                Case iXpsStage.PartEnum.Lens1
                    Return Me.RecordData_Lens(eStep, 1)
                Case iXpsStage.PartEnum.Lens2
                    Return Me.RecordData_Lens(eStep, 2)
                Case iXpsStage.PartEnum.Lens3
                    Return Me.RecordData_Lens(eStep, 3)
                Case iXpsStage.PartEnum.Lens4
                    Return Me.RecordData_Lens(eStep, 4)

                    'error
                Case Else
                    mMsgInfo.PostMessage("X   Wrong part identification: " + sStep)
                    Return False
            End Select


        End Function


        Private Function RecordData_Pbs(ByVal eStep As ProcessStepEnum, ByVal iChannel As Integer) As Boolean
            Dim s As String
            Dim success As Boolean
            Dim v As Double
            Dim i, ii As Integer
            Dim CH, LensChannels() As Integer
            Dim data() As BeamWaistData
            Dim change As AlignmentResult

            'ack
            s = "    Record alignment data for PBS" + iChannel.ToString() + " Step " + eStep.ToString()
            mMsgInfo.PostMessage(s)

            'get lens channels
            LensChannels = Me.GetLensChannels(iChannel)
            If LensChannels.Length = 0 Then
                s = "X   Invalid PSB channel to be aligned. Valid number is between [1, 3]"
                mMsgInfo.PostMessage(s)
                Return False
            End If
            ii = LensChannels.Length - 1
            ReDim data(ii)

            'set flag
            mAlignData(eStep).HaveData = True
            mAlignData(eStep).IsAbsoluteBeamPosition = True

            'get stage position
            mAlignData(eStep).StageX = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            mAlignData(eStep).StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            mAlignData(eStep).StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            mAlignData(eStep).StageAngle = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)

            'get beam scan position at far side and move it
            v = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            success = Me.MoveBeamGage(v, False)
            If Not success Then Return False

            'get beam data
            mMsgInfo.PostMessage("      " + mAlignData(eStep).TableHeader)
            mMsgInfo.PostMessage("      Current measurement")

            change.BeamPeak = 0
            change.BeamCenterYY = 0
            change.BeamCenterZZ = 0

            For i = 0 To ii
                'measure
                CH = LensChannels(i)
                mTool.mInst.LDD.TurnSingleChannelOn(CH)
                success = Me.GetBeamScanData(Double.NaN, False, False, data(i))
                If Not success Then Return False
                'pass down data
                mAlignData(eStep).BeamPeak = data(i).PeakValue
                mAlignData(eStep).BeamCenterYY = data(i).PeakYY
                mAlignData(eStep).BeamCenterZZ = data(i).PeakZZ
                'show
                s = mAlignData(eStep).GetTableText(eStep.ToString(), CH.ToString())
                mMsgInfo.PostMessage("      " + s)
                'sum for average
                change.BeamPeak += mAlignData(eStep).BeamPeak
                change.BeamCenterYY += mAlignData(eStep).BeamCenterYY
                change.BeamCenterZZ += mAlignData(eStep).BeamCenterYY
            Next

            'average and show if needed
            If data.Length > 1 Then
                mAlignData(eStep).BeamPeak = change.BeamPeak / data.Length
                mAlignData(eStep).BeamCenterYY = change.BeamCenterYY / data.Length
                mAlignData(eStep).BeamCenterZZ = change.BeamCenterZZ / data.Length

                s = mAlignData(eStep).GetTableText(eStep.ToString(), "AVE")
                mMsgInfo.PostMessage("      " + s)
            End If

            'we decided not to do the post epoxy re-alignment, so just record and show the data the data
            mMsgInfo.PostMessage("      Data comparison among steps")
            For i = 0 To eStep
                s = mAlignData(i).GetTableText(eStep.ToString(), "")
                mMsgInfo.PostMessage("      " + s)

                'change
                If i = eStep And eStep <> ProcessStepEnum.InitialAlign Then
                    change = mAlignData(i) - mAlignData(ProcessStepEnum.InitialAlign)
                    s = change.GetTableText("Change", "")
                    mMsgInfo.PostMessage("      " + s)
                End If
            Next

            Return True
        End Function

        Private Function RecordData_Lens(eStep As ProcessStepEnum, ByVal iChannel As Integer) As Boolean
            Dim s As String
            Dim i As Integer
            Dim success As Boolean
            Dim v, baseYY, baseZZ As Double
            Dim specYY, specZZ As Double
            Dim data As BeamWaistData
            Dim change As AlignmentResult

            'ack
            s = "    Record alignment data for Lens" + iChannel.ToString() + " Step " + eStep.ToString()
            mMsgInfo.PostMessage(s)

            'get beam scan position at far side and move it
            v = mStage.GetConfiguredBeamScanPosition(iXpsStage.StagePositionEnum.BeamScanFar).X
            success = Me.MoveBeamGage(v, False)
            If Not success Then Return False

            'get stage position
            mAlignData(eStep).StageX = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageX)
            mAlignData(eStep).StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
            mAlignData(eStep).StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)
            mAlignData(eStep).StageAngle = mStage.GetStagePosition(iXpsStage.AxisNameEnum.AngleMain)

            'get beam scan data
            If iChannel = mBaseChannel Then
                'base channel absolute value only
                s = "      This is a base channel and only absolute position are recorded"
                mMsgInfo.PostMessage(s)

                mAlignData(eStep).HaveData = True
                mAlignData(eStep).IsAbsoluteBeamPosition = True
                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Return False

                mAlignData(eStep).BeamPeak = data.PeakValue
                mAlignData(eStep).BeamCenterYY = data.PeakYY
                mAlignData(eStep).BeamCenterZZ = data.PeakZZ

                s = "      " + mAlignData(eStep).TableHeader
                s += ControlChars.CrLf + "      " + mAlignData(eStep).GetTableText(eStep.ToString(), iChannel.ToString())
                mMsgInfo.PostMessage(s)

            Else
                s = "      Compare data between aligned and active channels"
                mMsgInfo.PostMessage(s)

                s = Me.GetFormattedString_BeamOverlapHeader(True)
                mMsgInfo.PostMessage(s)

                'get base data
                success = Me.GetBeamCenter_AlignedChannels(iChannel, baseYY, baseZZ)
                If Not success Then Return False

                'get beam gage data for the target channel
                mTool.mInst.LDD.TurnSingleChannelOn(iChannel)
                success = Me.GetBeamScanData(Double.NaN, False, False, data)
                If Not success Then Return False

                'consolidate data
                mAlignData(eStep).HaveData = True
                mAlignData(eStep).IsAbsoluteBeamPosition = False
                mAlignData(eStep).BeamPeak = data.PeakValue
                mAlignData(eStep).BeamCenterYY = data.PeakYY - baseYY
                mAlignData(eStep).BeamCenterZZ = data.PeakZZ - baseZZ

                'show data
                s = "      Get data for this channel"
                s = Me.GetFormattedString_BeamOverlap(True, iChannel, data.PeakYY, data.PeakZZ, mAlignData(eStep).BeamCenterYY, mAlignData(eStep).BeamCenterZZ, _
                                                                      mAlignData(eStep).StageY, mAlignData(eStep).StageZ, Double.NaN)
                mMsgInfo.PostMessage(s)

                s = "      Summary: "
                s += ControlChars.CrLf + "      " + mAlignData(eStep).TableHeader
                s += ControlChars.CrLf + "      " + mAlignData(eStep).GetTableText(eStep.ToString(), iChannel.ToString())
                mMsgInfo.PostMessage(s)

            End If

            'post epoxy aligment - check spec and try to do some adjustment
            If eStep = ProcessStepEnum.PostEpoxy Then
                If mAlignData(eStep).IsAbsoluteBeamPosition Then
                    'delta is referenced to last alignment
                    baseYY = mAlignData(eStep).BeamCenterYY - mAlignData(ProcessStepEnum.InitialAlign).BeamCenterYY
                    baseZZ = mAlignData(eStep).BeamCenterZZ - mAlignData(ProcessStepEnum.InitialAlign).BeamCenterZZ
                    'collimation channel or CH2, collimation is less important than overlap, and we have PBS steering beam any way, do not over tight the spec to kill ourself, just tight the spec by 2x
                    specYY = 0.5 * mPara.Alignment.MaxSteeringY
                    specZZ = 0.5 * mPara.Alignment.MaxSteeringZ
                Else
                    'this is very smiple, we just want to make sure that the beam difference is in spec
                    baseYY = mAlignData(eStep).BeamCenterYY
                    baseZZ = mAlignData(eStep).BeamCenterZZ
                    specYY = mPara.Alignment.MaxSteeringY
                    specZZ = mPara.Alignment.MaxSteeringZ
                End If
                'check spec
                If (Math.Abs(baseYY) < specYY) And (Math.Abs(baseZZ) < specZZ) Then
                    'data meet spec, fine
                Else
                    success = Me.AlignLensPostEpoxy(iChannel, baseYY, baseZZ)
                    If Not success Then Return False
                End If
            End If

            'show final data
            mMsgInfo.PostMessage(ControlChars.CrLf + "      Data comparison among steps")
            For i = 0 To eStep
                Select Case i
                    Case 0
                        eStep = ProcessStepEnum.InitialAlign
                    Case 1
                        eStep = ProcessStepEnum.PostEpoxy
                    Case 2
                        eStep = ProcessStepEnum.PostUvCure
                End Select
                s = mAlignData(i).GetTableText((eStep).ToString(), "AVE")
                mMsgInfo.PostMessage("      " + s)

                'change, not needed if data is already relative
                If mAlignData(eStep).IsAbsoluteBeamPosition Then
                    If i = eStep And eStep <> ProcessStepEnum.InitialAlign Then
                        change = mAlignData(i) - mAlignData(ProcessStepEnum.InitialAlign)
                        s = change.GetTableText("Change", "AVE")
                        mMsgInfo.PostMessage("      " + s)
                    End If
                End If
            Next

            Return True
        End Function

        Private Function AlignLensPostEpoxy(ByVal iChannel As Integer, ByVal deltaYY As Double, ByVal deltaZZ As Double) As Boolean
            Dim s As String
            Dim i, ii As Integer
            Dim success As Boolean
            Dim vStep, deltaY, deltaZ As Double
            Dim baseYY, baseZZ, specYY, specZZ As Double
            Dim data As BeamWaistData

            Const eStep As ProcessStepEnum = ProcessStepEnum.PostEpoxy

            'limit step size
            vStep = mPara.Alignment.PostEpoxyStepSize
            If vStep > 0.001 Then vStep = 0.001
            If vStep < 0.0001 Then vStep = 0.0001

            'ack
            s = ControlChars.CrLf + "      Post epoxy fine adjustment for best alignment ... "
            mMsgInfo.PostMessage(s)

            'reduce stage speed
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageY, mPara.Alignment.StageSpeed)
            mStage.SetStageVelocity(iXpsStage.AxisNameEnum.StageZ, mPara.Alignment.StageSpeed)

            ii = Convert.ToInt32(mPara.Alignment.postEpoxyMaxMove / vStep)
            For i = 0 To ii
                'stage position increase will reduce beam position or reducing deltaYY
                deltaY = vStep * Math.Sign(deltaYY)
                deltaZ = vStep * Math.Sign(deltaZZ)

                'move stage now
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageY, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, deltaY, False)
                If Not success Then Exit For
                success = Me.MoveStage1Axis(iXpsStage.AxisNameEnum.StageZ, Instrument.iMotionController.MoveToTargetMethodEnum.Relative, deltaZ, False)
                If Not success Then Exit For

                'get beam data
                If mAlignData(eStep).IsAbsoluteBeamPosition Then
                    'for self aligned channel, we will use the reference data
                    success = Me.GetBeamScanData(Double.NaN, False, False, data)
                    If Not success Then Exit For

                    'assign data
                    mAlignData(eStep).BeamPeak = data.PeakValue
                    mAlignData(eStep).BeamCenterYY = data.PeakYY
                    mAlignData(eStep).BeamCenterZZ = data.PeakZZ
                    mAlignData(eStep).StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                    mAlignData(eStep).StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                    'calculate the delta - relative to initial alignment
                    deltaYY = mAlignData(eStep).BeamCenterYY - mAlignData(ProcessStepEnum.InitialAlign).BeamCenterYY
                    deltaZZ = mAlignData(eStep).BeamCenterZZ - mAlignData(ProcessStepEnum.InitialAlign).BeamCenterZZ

                    'spec
                    specYY = 0.5 * mPara.Alignment.MaxSteeringY
                    specZZ = 0.5 * mPara.Alignment.MaxSteeringZ

                    'show data
                    s = "      Beam data relative to initial alignment"
                    s += ControlChars.CrLf + "      " + mAlignData(eStep).TableHeader
                    s += ControlChars.CrLf + "      " + mAlignData(ProcessStepEnum.InitialAlign).GetTableText(ProcessStepEnum.InitialAlign.ToString(), iChannel.ToString())
                    s += ControlChars.CrLf + "      " + mAlignData(eStep).GetTableText(eStep.ToString(), iChannel.ToString())
                    mMsgInfo.PostMessage(s)

                Else
                    'no reference point, we will use aligned channel
                    s = "      Compare data between aligned and active channels"
                    mMsgInfo.PostMessage(s)

                    s = Me.GetFormattedString_BeamOverlapHeader(True)
                    mMsgInfo.PostMessage(s)

                    'get base data
                    success = Me.GetBeamCenter_AlignedChannels(iChannel, baseYY, baseZZ)
                    If Not success Then Return False

                    'get beam gage data for the target channel
                    mTool.mInst.LDD.TurnSingleChannelOn(iChannel)
                    success = Me.GetBeamScanData(Double.NaN, False, False, data)
                    If Not success Then Return False

                    'consolidate data
                    mAlignData(eStep).BeamPeak = data.PeakValue
                    mAlignData(eStep).BeamCenterYY = data.PeakYY - baseYY
                    mAlignData(eStep).BeamCenterZZ = data.PeakZZ - baseZZ
                    mAlignData(eStep).StageY = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageY)
                    mAlignData(eStep).StageZ = mStage.GetStagePosition(iXpsStage.AxisNameEnum.StageZ)

                    deltaYY = mAlignData(eStep).BeamCenterYY
                    deltaZZ = mAlignData(eStep).BeamCenterZZ
                    specYY = mPara.Alignment.MaxSteeringY
                    specZZ = mPara.Alignment.MaxSteeringZ

                    'show beam data
                    s = Me.GetFormattedString_BeamOverlap(True, iChannel, data.PeakYY, data.PeakZZ, mAlignData(eStep).BeamCenterYY, mAlignData(eStep).BeamCenterZZ, _
                                                                mAlignData(eStep).StageY, mAlignData(eStep).StageZ, Double.NaN)
                    mMsgInfo.PostMessage(s)

                    'show recorded data - this is delta not
                    s = "      Beam data relative to aligned and cured channels"
                    s += ControlChars.CrLf + "      " + mAlignData(eStep).TableHeader
                    s += ControlChars.CrLf + "      " + mAlignData(eStep).GetTableText(eStep.ToString(), iChannel.ToString())
                    mMsgInfo.PostMessage(s)
                End If

                'check spec
                success = ((Math.Abs(deltaYY) < specYY) And (Math.Abs(deltaZZ) < specZZ))
                If success Then Exit For
            Next

            'recover stage speed
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageY)
            mStage.RecoverStageDefaultVelocity(iXpsStage.AxisNameEnum.StageZ)

            'done
            Return success

        End Function
#End Region
    End Class

End Class