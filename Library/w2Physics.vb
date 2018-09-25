Option Explicit On
Option Strict On
Option Infer Off

Namespace w2Physics
    Public Class BeamWaistCalculator
#Region "utility"
        Public Structure BeamWaistInfo
            Public Sub New(ByVal NewW0 As Double, ByVal NewZ1 As Double, ByVal NewZ2 As Double, ByVal NewW1 As Double, ByVal NewW2 As Double)
                W0 = NewW0

                Z1 = NewZ1
                Z2 = NewZ2

                W1 = NewW1
                W2 = NewW2
            End Sub
            Public W0 As Double
            Public Z1 As Double
            Public Z2 As Double

            Public W1 As Double
            Public W2 As Double
        End Structure

        Public Structure BeamWaistInfo2D
            Public Enum DataSelectionOption
                UseX
                UseY
                'UseAverage
            End Enum
            Public X As BeamWaistInfo
            Public Y As BeamWaistInfo

            'Public ReadOnly Property Average As BeamWaistInfo
            '    Get
            '        Dim v As BeamWaistInfo
            '        v.W0 = 0.5 * (X.W0 + Y.W0)
            '        v.W1 = 0.5 * (X.W1 + Y.W1)
            '        v.W2 = 0.5 * (X.W2 + Y.W2)
            '        v.Z1 = 0.5 * (X.Z1 + Y.Z1)
            '        v.Z2 = 0.5 * (X.Z2 + Y.Z2)
            '        Return v
            '    End Get
            'End Property

            Public Function GetActiveBeamWaistInfo(ByVal Selection As DataSelectionOption) As BeamWaistInfo
                Select Case Selection
                    Case DataSelectionOption.UseX
                        Return X
                    Case DataSelectionOption.UseY
                        Return Y
                        'Case DataSelectionOption.UseAverage
                        '    Return Me.Average
                    Case Else
                        Return Nothing
                End Select
            End Function
        End Structure
#End Region

        Public Property Lamda As Double = 0.00155

        Public Sub New()
        End Sub

        Public Sub New(ByVal Wavelength As Double)
            Lamda = Wavelength
        End Sub

        Public Function CalculateWaist(ByVal DeltaZ As Double, ByVal W1 As Double, ByVal W2 As Double) As BeamWaistInfo
            Dim A, B, C As Double
            Dim W1S, W2S As Double
            Dim LamdaZpi As Double
            Dim v, v1, v2 As Double
            Dim result As BeamWaistInfo

            'passdown
            result.W1 = W1
            result.W2 = W2

            'calculate
            LamdaZpi = Lamda * DeltaZ / Math.PI
            W1S = W1 ^ 2
            W2S = W2 ^ 2

            v = (W1S - W2S) / LamdaZpi / 2
            A = 1 + v ^ 2
            B = -0.5 * (W1S + W2S)
            C = (LamdaZpi / 2) ^ 2

            v = Math.Sqrt(B ^ 2 - 4 * A * C)
            v -= B
            v /= (2 * A)
            v = Math.Sqrt(v)

            result.W0 = v

            A = Math.PI * v / Lamda
            v = v ^ 2
            v1 = A * Math.Sqrt(W1S - v)
            v2 = A * Math.Sqrt(W2S - v)

            result.Z1 = Math.Min(v1, v2)
            result.Z2 = Math.Max(v1, v2)


            Return result
        End Function

        Public Function GetWidthAtZ(ByVal W0 As Double, ByVal Z As Double) As Double
            Dim v As Double

            v = Lamda * Z / Math.PI / W0 ^ 2
            v = 1 + v ^ 2
            v = W0 * Math.Sqrt(v)

            Return v
        End Function

        Public Function GetZfromWidth(ByVal W0 As Double, ByVal W As Double) As Double
            Dim v As Double

            v = Math.Sqrt(W ^ 2 - W0 ^ 2)
            v = Math.PI * W0 / Lamda / v

            Return v
        End Function

    End Class

    Public Class LIV
        Public Shared Function FitThresholdCurrent(ByVal Current() As Double, ByVal Power() As Double) As Double
            Dim i, ii As Integer
            Dim idx As Double
            Dim D1, D2 As Double()

            ii = Current.Length - 1
            ReDim D1(ii), D2(ii)

            '1st derivative
            For i = 0 To ii - 1
                D1(i) = Power(i + 1) - Power(i)
                D1(i) /= (Current(i + 1) - Current(i))
            Next

            '2nd derivative
            For i = 1 To ii - 1
                D2(i) = D1(i) - D1(i - 1)
            Next

            'get peak from 2nd derivativee,
            idx = w2PeakDetector.GetDominantPeakIndex(D2)
            Return w2Array.GetValueAtIndex(Current, idx)
        End Function

        Public Shared Function FitThresholdCurrent(ByVal Current() As Double, ByVal Power() As Double, ByVal StartCurrent As Double, ByVal EndCurrent As Double) As Double
            Dim i, ii As Integer
            Dim idx, diff, max As Double
            Dim peakIndex() As Double
            Dim D1, D2 As List(Of Double)

            ii = Current.Length - 1
            D1 = New List(Of Double)
            D2 = New List(Of Double)

            '1st derivative
            For i = 0 To ii - 1
                If Current(i) >= StartCurrent And Current(i + 1) <= EndCurrent Then
                    diff = Power(i + 1) - Power(i)
                    diff /= (Current(i + 1) - Current(i))
                    D1.Add(diff)
                End If
            Next

            '2nd derivative
            For i = 1 To D1.Count - 1
                D2.Add(D1(i) - D1(i - 1))
            Next

            'get peak from 2nd derivativee,
            'idx = w2PeakDetector.GetDominantPeakIndex(D2.ToArray())
            peakIndex = w2PeakDetector.GetPeakIndexes(D2.ToArray())
            max = Double.MinValue
            For Each i In peakIndex
                If D2(i) > max Then max = D2(i)
            Next

            For Each i In peakIndex
                If D2(i) > 0.3 * max Then
                    idx = i
                    Exit For
                End If
            Next

            Return w2Array.GetValueAtIndex(Current, idx)
        End Function
    End Class
End Namespace