Option Explicit On
Option Strict On
Option Infer Off

Imports NanoScan

Namespace Instrument
    Public Class iNewportNanoScan
        Inherits iBeamProfiler


        Private mNS As NanoScan.INanoScan
        Private mError As Integer

        Public Overrides Function Initialize(sDataSource As String) As Boolean
            Dim nDevice As Short

            'this will take time
            mNS = New NanoScan.NsAs

            'get device
            mError = mNS.NsAsGetNumDevices(nDevice)
            If mError <> 0 Then
                MessageBox.Show("Error initializing Nano Scan tool. Error Cdoe = " + Me.GetLastError(), "Nano Scan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If nDevice < 1 Then
                MessageBox.Show("No NanoScan device found", "Nano Scan", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            mNS.NsAsShowWindow = True
            mNS.NsAsDataAcquisition = True
            System.Threading.Thread.Sleep(100)
            mNS.NsAsAutoROI = True
            mError = mNS.NsAsAutoFind()
            mNS.NsAsDataAcquisition = True


            Return True
        End Function

        Public Overrides Function Close() As Boolean
            mNS.NsAsDataAcquisition = False
            System.Runtime.InteropServices.Marshal.ReleaseComObject(mNS)
            Return True
        End Function

        Public Overrides Function AcquireData(ByVal Samples As Integer, ByVal Mode As DataAcquisitionMode) As iBeamProfiler.SimpleData
            Dim iMode As Integer
            Dim iShort As Short
            Dim v As Single
            Dim data As SimpleData

            Const ROI As Integer = 0

            'select parameters to extract
            iMode = NsAsParameterSelection.NSAS_SELECT_PARAM_POS_CENTR + NsAsParameterSelection.NSAS_SELECT_PARAM_TOTAL_POWER + NsAsParameterSelection.NSAS_SELECT_PARAM_WIDTH_1

            If Mode = DataAcquisitionMode.PeakWidthEnergyEllipicity Then
                iMode += NsAsParameterSelection.NSAS_SELECT_PARAM_ELLIPTIC
            End If

            If Mode = DataAcquisitionMode.AllSummaryData Then

            End If

            If Mode = DataAcquisitionMode.DataAndFrame Then

            End If

            'set up measurement
            mError = mNS.NsAsSelectParameters(iMode)

            'set flag, zero data
            data.SampleCount = Samples
            data.HaveFrameInfo = False

            data.FrameData = Nothing
            data.DoubleData = Nothing

            ' Power Energy Data
            data.TotalEnergy = 0
            data.AveragePowerDensity = 0
            data.PeakPower = 0
            data.Minimum = 0

            ' Spatial Data
            data.CentroidX = 0
            data.CentroidY = 0
            data.PeakLocationX = 0
            data.PeakLocationY = 0
            data.D4SigmaX = 0
            data.D4SigmaY = 0
            data.D4SigmaDiameter = 0
            data.FWHMX = 0
            data.FWHMY = 0



            data.Orientation = 0
            data.Ellipticity = 0
            data.Eccentricity = 0


            For iMode = 1 To Samples
                mError = mNS.NsAsAcquireSync1Rev()
                If mError <> 0 Then Exit For

                mError = mNS.NsAsRunComputation()
                'If mError <> 0 Then Exit For

                ' Power Energy Data
                mError = mNS.NsAsGetTotalPower(v)
                If mError <> 0 Then Exit For
                data.TotalEnergy += v

                'mError = mNS.NsAsGetPower(ROI, v)
                'If mError <> 0 Then Exit For
                'data.PeakPower += v

                ' Spatial Data
                mError = mNS.NsAsGetCentroidPosition(0, ROI, v)
                If mError <> 0 Then Exit For
                data.CentroidX += v

                mError = mNS.NsAsGetCentroidPosition(1, ROI, v)
                If mError <> 0 Then Exit For
                data.CentroidY += v

                'mError = mNS.NsAsGetPeakPosition(0, ROI, v)
                'If mError <> 0 Then Exit For
                'data.PeakLocationX += v

                'mError = mNS.NsAsGetPeakPosition(1, ROI, v)
                'If mError <> 0 Then Exit For
                'data.PeakLocationY += v

                'mError = mNS.NsAsGetBeamWidth4Sigma(0, ROI, v)
                'If mError <> 0 Then Exit For
                'data.D4SigmaX += v

                'mError = mNS.NsAsGetBeamWidth4Sigma(1, ROI, v)
                'If mError <> 0 Then Exit For
                'data.D4SigmaY += v

                'mError = mNS.NsAsGetBeamWidth(0, ROI, 50.0F, v)
                'If mError <> 0 Then Exit For
                'data.FWHMX += v

                'mError = mNS.NsAsGetBeamWidth(1, ROI, 50.0F, v)
                'If mError <> 0 Then Exit For
                'data.FWHMY += v

                'Add by Ming to get 13.5% Width
                mError = mNS.NsAsGetBeamWidth(0, ROI, 13.5F, v)
                If mError <> 0 Then Exit For
                data.FWHMX += v

                mError = mNS.NsAsGetBeamWidth(1, ROI, 13.5F, v)
                If mError <> 0 Then Exit For
                data.FWHMY += v

                'If Mode = DataAcquisitionMode.PeakWidthEnergyEllipicity Then
                '    mError = mNS.NsAsGetBeamEllipticity(ROI, v)
                '    If mError <> 0 Then Exit For
                '    data.Ellipticity += v
                'End If

                'Public AveragePowerDensity As Double
                'Public Minimum As Double
                'Public Orientation As Double
                'Public Ellipticity As Double
                'Public Eccentricity As Double

            Next

            data.Valid = (mError = 0)

            'average data once we are done
            ' Power Energy Data
            data.TotalEnergy /= data.SampleCount
            data.AveragePowerDensity /= data.SampleCount
            data.PeakPower /= data.SampleCount
            data.Minimum /= data.SampleCount

            ' Spatial Data - convert to mm unit

            'Changed By Ming to convert to um unit
            data.CentroidX /= data.SampleCount
            data.CentroidY /= data.SampleCount
            'data.PeakLocationX /= data.SampleCount
            'data.PeakLocationY /= data.SampleCount
            'data.D4SigmaX /= data.SampleCount
            'data.D4SigmaY /= data.SampleCount
            data.FWHMX /= data.SampleCount
            data.FWHMY /= data.SampleCount
            'data.D4SigmaDiameter = Math.Sqrt(data.D4SigmaX ^ 2 + data.D4SigmaY ^ 2)

            'data.CentroidX /= (1000 * data.SampleCount)
            'data.CentroidY /= (1000 * data.SampleCount)
            'data.PeakLocationX /= (1000 * data.SampleCount)
            'data.PeakLocationY /= (1000 * data.SampleCount)
            'data.D4SigmaX /= (1000 * data.SampleCount)
            'data.D4SigmaY /= (1000 * data.SampleCount)
            'data.D4SigmaDiameter = Math.Sqrt(data.D4SigmaX ^ 2 + data.D4SigmaY ^ 2)

            data.Orientation /= data.SampleCount
            data.Ellipticity /= data.SampleCount
            data.Eccentricity /= data.SampleCount

            'use radius
            'data.D4SigmaX /= 2
            'data.D4SigmaY /= 2

            'get condition info
            mError = mNS.NsAsGetGain(0, iShort)
            data.Gain = iShort
            'data.Exposure = Me.Exposure
            'data.BlackLevel = Me.BlackLevel

            data.IsFarmeCalibrated = False
            data.IsBaselineCalibrated = False

            'return data
            data.TimeStamp = Date.Now
            Return data

        End Function

        Public Overrides Function GetLastError() As String
            Return "0x" + Convert.ToString(mError, 16)
        End Function

        Public Property PowerUnit As NanoScan.NsAsPowerUnits
            Get
                Return CType(mNS.NsAsPowerUnits, NanoScan.NsAsPowerUnits)
            End Get
            Set(ByVal value As NanoScan.NsAsPowerUnits)
                mNS.NsAsPowerUnits = Convert.ToInt16(value)
            End Set
        End Property

#Region "config"
        Public Overrides Property AutoGain As Boolean
            Get
                Return False
            End Get
            Set(value As Boolean)
                'do nothing
            End Set
        End Property

        Public Overrides Property Gain As Double
            Get
                Dim g1, g2 As Short
                mError = mNS.NsAsGetGain(0, g1)
                mError = mNS.NsAsGetGain(1, g2)
                Return 0.5 * (g1 + g2)
            End Get
            Set(ByVal value As Double)
                Dim g As Short
                g = Convert.ToInt16(value)
                mError = mNS.NsAsSetGain(0, g)
                mError = mNS.NsAsSetGain(1, g)
            End Set
        End Property

        Public Overrides Property SamplingFrequency As Double
            Get
                Dim v As Single
                mError = mNS.NsAsGetRotationFrequency(v)
                Return v
            End Get
            Set(ByVal value As Double)
                mError = mNS.NsAsSetRotationFrequency(Convert.ToSingle(value))
            End Set
        End Property

        Public Overrides ReadOnly Property SamplingFrequencyMeasured() As Double
            Get
                Dim v As Single
                mError = mNS.NsAsGetMeasuredRotationFreq(v)
                Return v
            End Get
        End Property

        Public Overrides Property SamplingResolution As Double
            Get
                Dim vx, vy As Single
                mError = mNS.NsAsGetSamplingResolution(0, vx)
                mError = mNS.NsAsGetSamplingResolution(1, vy)
                Return 0.5 * (vx + vy)
            End Get
            Set(ByVal value As Double)
                mError = mNS.NsAsSetSamplingResolution(Convert.ToSingle(value))
            End Set
        End Property

        Public Overrides Property Wavelenth_nm As Double
#End Region
    End Class

    Public Class iThorlabsBP200BeamScan
        Inherits iBeamProfiler


        Private mBP2 As Thorlabs.BP2.TLBP2
        Private mStatus As Integer

        Public Overrides Function Initialize(sDataSource As String) As Boolean
            Dim Devices As Thorlabs.BP2.BP2_device()
            Dim iDevices As UInt32

            If Not sDataSource.StartsWith("USB0::") Then
                mBP2 = New Thorlabs.BP2.TLBP2(New IntPtr())
                Try
                    mStatus = mBP2.get_connected_devices(Nothing, iDevices)
                Catch ex As Exception
                    MessageBox.Show("Failed to get conencted Thorlabs BP200 series beam scan: " + ex.Message, "BP200", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try

                If mStatus <> 0 Then
                    MessageBox.Show("Failed to get conencted Thorlabs BP200 series beam scan", "BP200", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If

                If iDevices = 0 Then
                    MessageBox.Show("No BP200 beam scan found", "BP200", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If

                ReDim Devices(Convert.ToInt32(iDevices) - 1)
                mStatus = mBP2.get_connected_devices(Devices, iDevices)

                sDataSource = Devices(0).ResourceString
                'mBP2.Dispose()
                'mBP2 = Nothing
            End If

            mBP2 = New Thorlabs.BP2.TLBP2(sDataSource, False, False)
            'Me.AutoGain = True

            'Dim iError As Short
            'Dim s As New System.Text.StringBuilder
            'mStatus = mBP2.self_test(iError, s)

            Dim sampleCount As UShort
            Dim sampleResolution As Double
            mStatus = mBP2.set_drum_speed_ex(10.0, sampleCount, sampleResolution)
            mStatus = mBP2.set_position_correction(True)
            mStatus = mBP2.set_auto_gain(True)
            mStatus = mBP2.set_speed_correction(True)

            Return True
        End Function

        Public Overrides Function Close() As Boolean
            mBP2.Dispose()
            mBP2 = Nothing
            Return True
        End Function

        Public Overrides Function AcquireData(Samples As Integer, Mode As iBeamProfiler.DataAcquisitionMode) As iBeamProfiler.SimpleData
            Dim NewSample(1) As Short
            'Dim iStatus As UShort
            Dim data As SimpleData
            Dim SlitData(3) As Thorlabs.BP2.BP2_slit_data
            Dim Result(3) As Thorlabs.BP2.BP2_calculations
            Dim power, PowerData(2128 - 1) As Double
            Dim powerSaturation As Single

            'null data
            data.Valid = False
            data.SampleCount = Samples
            data.HaveFrameInfo = False
            data.FrameData = Nothing
            data.DoubleData = Nothing

            'get status
            'While True
            '    Dim deviceStatus As UShort
            '    mStatus = mBP2.get_device_status(deviceStatus)
            '    If (mStatus <> 0) Then Return data
            '    If (deviceStatus And 2) = 2 Then Exit While
            'End While

            'get daya
            Try
                mStatus = mBP2.get_slit_scan_data(SlitData, Result, power, powerSaturation, Nothing)
                If (mStatus <> 0) Then Return data

                'assign data
                data.Valid = True
                data.TotalEnergy = power

                data.CentroidX = Result(0).CentroidPos
                data.CentroidY = Result(1).CentroidPos

                data.PeakLocationX = Result(0).PeakPosition
                data.PeakLocationY = Result(1).PeakPosition
                data.PeakPower = 0.5 * (Result(0).PeakIntensity + Result(1).PeakIntensity)

                data.D4SigmaX = Result(0).BeamWidthClip
                data.D4SigmaY = Result(1).BeamWidthClip

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            'data.HaveFrameInfo = True
            'data.FrameData(0) = SlitData(0).SlitSamplesIntensities

            Return data

        End Function

        Public Overrides Function GetLastError() As String
            Dim s As New System.Text.StringBuilder
            mStatus = mBP2.error_query(mStatus, s)
            Return s.ToString
        End Function

#Region "config"
        Public Overrides Property AutoGain As Boolean
            Get
                Dim b As Boolean
                mStatus = mBP2.get_auto_gain(b)
                Return b And mStatus = 0
            End Get
            Set(value As Boolean)
                mStatus = mBP2.set_auto_gain(value)
            End Set
        End Property

        Public Overrides Property Gain As Double
            Get
                Dim bGain(1), pGain As Byte
                mStatus = mBP2.get_gains(bGain, pGain)
                Return (bGain(0) + bGain(1) + pGain) / 3
            End Get
            Set(value As Double)
                Dim min, max As Byte
                Dim bGain(1), pGain As Byte

                mStatus = mBP2.get_gain_range(min, max)
                pGain = Convert.ToByte(value)
                If pGain < min Then pGain = min
                If pGain > max Then pGain = max
                bGain(0) = pGain
                bGain(1) = pGain

                mStatus = mBP2.set_gains(bGain, pGain)
            End Set
        End Property

        Public Overrides Property SamplingFrequency As Double
            Get
                Dim freq As Double
                mStatus = mBP2.get_drum_speed(freq)
                Return freq
            End Get
            Set(value As Double)
                Dim min, max As Double
                mStatus = mBP2.get_drum_speed_range(min, max)
                If value < min Then value = min
                If value > max Then value = max
                mStatus = mBP2.set_drum_speed(value)
            End Set
        End Property

        Public Overrides ReadOnly Property SamplingFrequencyMeasured As Double
            Get
                Return Me.SamplingFrequency
            End Get
        End Property

        Public Overrides Property SamplingResolution As Double



        Public Overrides Property Wavelenth_nm As Double
            Get
                Dim v As Double
                mStatus = mBP2.get_wavelength(v)
                Return v
            End Get
            Set(value As Double)
                Dim min, max As UShort
                mStatus = mBP2.get_wavelength_range(min, max)
                If value < min Then value = min
                If value > max Then value = max
                mStatus = mBP2.set_wavelength(value)
            End Set
        End Property
#End Region

#Region "Thorlabs specific"
        Public ReadOnly Property SerialNumber() As String
            Get
                Dim s As New System.Text.StringBuilder()
                mStatus = mBP2.get_serial_number(s)
                Return s.ToString()
            End Get
        End Property

        Public Sub Reset()
            mStatus = mBP2.reset()
        End Sub

#End Region

    End Class

    Public MustInherit Class iBeamProfiler
        Public Structure DoublePoint
            Public Sub New(ByVal X0 As Double, ByVal Y0 As Double)
                X = X0
                Y = Y0
            End Sub
            Public X As Double
            Public Y As Double
        End Structure

        Public Structure SimpleData
            Public Valid As Boolean
            Public SampleCount As Integer

            Public TimeStamp As DateTime

            Public HaveFrameInfo As Boolean
            Public FrameSize As Point
            Public FrameData()() As Double
            Public DoubleData()() As Double

            ' Power Energy Data
            Public TotalEnergy As Double
            Public AveragePowerDensity As Double
            Public PeakPower As Double
            Public Minimum As Double

            ' Spatial Data
            Public CentroidX As Double
            Public CentroidY As Double
            Public PeakLocationX As Double
            Public PeakLocationY As Double

            Public D4SigmaX As Double
            Public D4SigmaY As Double
            Public D4SigmaDiameter As Double

            Public FWHMX As Double
            Public FWHMY As Double


            Public Orientation As Double
            Public Ellipticity As Double
            Public Eccentricity As Double
            'Public CrossSectionArea As Double
            'Public CursorToCrosshair As Double
            'Public CentroidToCrosshair As Double

            'condition
            Public Gain As Double
            Public Exposure As Double
            Public BlackLevel As Double
            Public IsFarmeCalibrated As Boolean
            Public IsBaselineCalibrated As Boolean
        End Structure

        Public Enum DataAcquisitionMode
            PeakWidthEnergy
            PeakWidthEnergyEllipicity
            AllSummaryData
            DataAndFrame
        End Enum

        Public MustOverride Function Initialize(ByVal sDataSource As String) As Boolean
        Public MustOverride Function Close() As Boolean
        Public MustOverride Function AcquireData(ByVal Samples As Integer, ByVal Mode As DataAcquisitionMode) As SimpleData
        Public MustOverride Function GetLastError() As String

        'config
        Public MustOverride Property AutoGain As Boolean
        Public MustOverride Property Gain As Double
        Public MustOverride Property SamplingFrequency As Double
        Public MustOverride ReadOnly Property SamplingFrequencyMeasured As Double
        Public MustOverride Property SamplingResolution As Double
        Public MustOverride Property Wavelenth_nm As Double


    End Class
End Namespace


