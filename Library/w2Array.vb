Option Explicit On
Option Strict On

Public Class w2Array

#Region "Simple algebra"
    Public Shared Function Abs(ByVal Input As Double()) As Double()
        Dim Output As Double()
        Dim i, ii As Integer
        ii = Input.Length - 1
        ReDim Output(ii)
        For i = 0 To ii
            Output(i) = Math.Abs(Input(i))
        Next
        Return Output
    End Function

    Public Shared Function Abs(ByVal Input As List(Of Double)) As Double()
        Return Abs(Input.ToArray())
    End Function

    Public Shared Function ArrayDelta(ByVal Input As Double(), Optional ByVal Offset As Integer = 1) As Double()
        Dim Output As Double()
        Dim i, ii As Integer

        Dim sign As Double = 1
        If Offset < 0 Then
            sign = -1
            Offset = -Offset
        End If

        ii = Input.Length - 1 - Offset
        ReDim Output(ii)
        For i = 0 To ii
            Output(i) = (Input(i + Offset) - Input(i)) * sign
        Next

        Return Output
    End Function

    Public Shared Function ArrayDelta(ByVal Input As List(Of Double), Optional ByVal Offset As Integer = 1) As Double()
        Return ArrayDelta(Input.ToArray())
    End Function

    Public Shared Function ArrayDeltaAbs(ByVal Input As Double(), Optional ByVal Offset As Integer = 1) As Double()
        Dim Output As Double()
        Dim i, ii As Integer

        If Offset < 0 Then Offset = -Offset

        ii = Input.Length - 1 - Offset
        ReDim Output(ii)
        For i = 0 To ii
            Output(i) = Math.Abs(Input(i + Offset) - Input(i))
        Next

        Return Output
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <param name="Offset"></param>
    ''' <returns></returns>
    ''' <remarks>Percentage is normalized to the larger of the two</remarks>
    Public Shared Function ArrayDeltaPercentageAbs(ByVal Input As Double(), Optional ByVal Offset As Integer = 1) As Double()
        Dim Output As Double()
        Dim i, ii As Integer

        If Offset < 0 Then Offset = -Offset

        ii = Input.Length - 1 - Offset
        ReDim Output(ii)
        For i = 0 To ii
            Output(i) = Input(i + Offset) - Input(i)
            Output(i) /= Math.Max(Math.Abs(Input(i)), Math.Abs(Input(i + Offset)))
            Output(i) *= 100.0
            Output(i) = Math.Abs(Output(i))
        Next

        Return Output
    End Function

    Public Shared Function ArrayDeltaPercentage(ByVal Input As Double(), Optional ByVal Offset As Integer = 1) As Double()
        Dim Output As Double()
        Dim i, ii As Integer

        Dim sign As Double = 1
        If Offset < 0 Then
            sign = -1
            Offset = -Offset
        End If

        ii = Input.Length - 1 - Offset
        ReDim Output(ii)
        For i = 0 To ii
            Output(i) = sign * (Input(i + Offset) - Input(i))
            Output(i) /= Input(i)
            Output(i) *= 100.0
        Next

        Return Output
    End Function

    Public Shared Function Add(ByVal Input As Double(), ByVal Value As Double) As Double()
        Dim i, ii As Integer
        ii = Input.Length - 1

        Dim Output(ii) As Double
        For i = 0 To ii
            Output(i) = Input(i) + Value
        Next

        Return Output
    End Function

    Public Shared Function Add(ByVal A As Double(), ByVal B As Double()) As Double()
        Dim i, ii As Integer
        ii = Math.Min(A.Length - 1, B.Length - 1)

        Dim Output(ii) As Double
        For i = 0 To ii
            Output(i) = A(i) + B(i)
        Next

        Return Output
    End Function

    Public Shared Function Add(ByVal Input As List(Of Double), ByVal Value As Double) As Double()
        Return Add(Input.ToArray(), Value)
    End Function

    Public Shared Function Subtract(ByVal input As Double(), ByVal Value As Double) As Double()
        Return Add(input, -Value)
    End Function

    Public Shared Function Subtract(ByVal A As Double(), ByVal B As Double()) As Double()
        Dim i, ii As Integer
        ii = Math.Min(A.Length - 1, B.Length - 1)

        Dim Output(ii) As Double
        For i = 0 To ii
            Output(i) = A(i) - B(i)
        Next

        Return Output
    End Function

    Public Shared Function Subtract(ByVal input As List(Of Double), ByVal Value As Double) As Double()
        Return Add(input, -Value)
    End Function

    ''' <summary>
    ''' Replace data in the input array with the NullValue if the data is 
    ''' inside range of [Min, Max] inclusive
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <param name="NullValue">the value used to replac the nulled data</param>
    ''' <param name="Min"></param>
    ''' <param name="Max"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NullDataInArray(ByVal Input() As Double, ByVal NullValue As Double, _
                                           ByVal Min As Double, ByVal Max As Double) As Double()
        Dim i, ii As Integer
        ii = Input.Length - 1

        Dim Output(ii) As Double
        For i = 0 To ii
            If Input(i) >= Min AndAlso Input(i) <= Max Then
                Output(i) = NullValue
            Else
                Output(i) = Input(i)
            End If
        Next

        Return Output
    End Function
#End Region

#Region "Mean, Min, Max"
    Public Shared Function AbsoluteMaximum(ByVal Input As Double()) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return Math.Max(Math.Abs(Min), Max)
    End Function

    Public Shared Function AbsoluteMaximum(ByVal Input As List(Of Double)) As Double
        Return AbsoluteMaximum(Input.ToArray())
    End Function

    Public Shared Function AbsoluteMinimum(ByVal Input As Double()) As Double
        Return Minimum(Abs(Input))
    End Function

    Public Shared Function AbsoluteMinimum(ByVal Input As List(Of Double)) As Double
        Return AbsoluteMaximum(Input.ToArray())
    End Function

    Public Shared Function AbsoluteMaximumSigned(ByVal Input As Double()) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        If Math.Abs(Min) < Max Then
            Return Max
        Else
            Return Min
        End If
    End Function

    Public Shared Function AbsoluteMaximumSigned(ByVal Input As List(Of Double)) As Double
        Return AbsoluteMaximumSigned(Input.ToArray())
    End Function

    Public Shared Function Maximum(ByVal Input As Double()) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return Max
    End Function

    Public Shared Function Maximum(ByVal Input As List(Of Double)) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return Max
    End Function

    Public Shared Function Mean(ByVal Input As Double()) As Double
        If Input.Length = 0 Then Return 0
        Dim Sum As Double = 0
        Dim Num As Integer = 0
        For Each v As Double In Input
            If Not Double.IsNaN(v) Then
                Sum += v
                Num += 1
            End If
        Next
        Return Sum / Num
    End Function

    Public Shared Function Mean(ByVal Input As List(Of Double)) As Double
        Return Mean(Input.ToArray())
    End Function

    Public Shared Function Minimum(ByVal Input As Double()) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return Min
    End Function

    Public Shared Function Minimum(ByVal Input As List(Of Double)) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return Min
    End Function

    Public Shared Sub MinMax(ByVal Input As Double(), ByRef Min As Double, ByRef iMin As Integer, _
                                                      ByRef Max As Double, ByRef iMax As Integer)
        Dim i As Integer = 0
        For Each v As Double In Input
            If Not Double.IsNaN(v) Then
                If i = 0 Then
                    'initialize value
                    'i = 1
                    Min = v : iMin = i
                    Max = v : iMax = i
                Else
                    'check min, max now
                    If v < Min Then
                        Min = v : iMin = i
                    ElseIf v > Max Then
                        Max = v : iMax = i
                    End If
                End If
                i += 1
            End If
        Next
    End Sub

    Public Shared Sub MinMax(ByVal Input As Double(), ByRef Min As Double, ByRef Max As Double)
        Dim i, j As Integer
        MinMax(Input, Min, i, Max, j)
    End Sub

    Public Shared Sub MinMax(ByVal Input As List(Of Double), ByRef Min As Double, ByRef Max As Double)
        MinMax(Input.ToArray(), Min, Max)
    End Sub

    Public Shared Sub MinMaxAverage(ByVal Input As Double(), ByRef Min As Double, ByRef Max As Double, ByRef Average As Double)
        Dim i As Integer = 0

        Average = 0.0
        For Each v As Double In Input
            If Not Double.IsNaN(v) Then
                If i = 0 Then
                    'initialize value
                    Min = v
                    Max = v
                Else
                    'check min, max now
                    If v < Min Then
                        Min = v
                    ElseIf v > Max Then
                        Max = v
                    End If
                End If
                'sum
                Average += v
                i += 1
            End If
        Next
        Average = Average / i
    End Sub

    Public Shared Sub MinMaxAverage(ByVal Input As Array, ByRef Min As Double, ByRef Max As Double, ByRef Average As Double)
        Dim i As Integer = 0

        Average = 0.0
        For Each v As Double In Input
            If Not Double.IsNaN(v) Then
                If i = 0 Then
                    'initialize value
                    Min = v
                    Max = v
                Else
                    'check min, max now
                    If v < Min Then
                        Min = v
                    ElseIf v > Max Then
                        Max = v
                    End If
                End If
                'sum
                Average += v
                i += 1
            End If
        Next
        Average = Average / i
    End Sub

    Public Shared Sub MinMaxAverage(ByVal Input As List(Of Double), ByRef Min As Double, ByRef Max As Double, ByRef Average As Double)
        MinMaxAverage(Input.ToArray(), Min, Max, Average)
    End Sub

    Public Shared Function Range(ByVal Input As Double()) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return (Max - Min)
    End Function

    Public Shared Function Range(ByVal Input As List(Of Double)) As Double
        Dim Min, Max As Double
        MinMax(Input, Min, Max)
        Return (Max - Min)
    End Function
#End Region

#Region "Mean, StdEv, R"
    Public Shared Function StandardDeviation(ByVal Input As Double()) As Double
        Dim S0 As Double = 0.0
        Dim S1 As Double = 0.0
        Dim S2 As Double = 0.0
        For Each v As Double In Input
            If Not Double.IsNaN(v) Then
                S0 += 1
                S1 += v
                S2 += v ^ 2
            End If
        Next
        Return Math.Sqrt(S0 * S2 - S1 ^ 2) / S0
    End Function

    Public Shared Sub MeanAndStandardDeviation(ByVal Input As Double(), ByRef Mean As Double, ByRef Stdev As Double)
        Dim S0 As Double = 0.0
        Dim S1 As Double = 0.0
        Dim S2 As Double = 0.0
        For Each v As Double In Input
            S0 += 1
            S1 += v
            S2 += v ^ 2
        Next

        Mean = S1 / S0
        Stdev = Math.Sqrt(S0 * S2 - S1 ^ 2) / S0
    End Sub

    'calculate the "R"
    Public Shared Function CalculateR(ByVal vData As Double(), ByVal MeanSquareError As Double) As Double
        Dim v, va As Double

        'mean
        va = w2Array.Mean(vData)
        'variance
        v = 0
        For i As Integer = 0 To vData.Length - 1
            v += (vData(i) - va) ^ 2
        Next
        'R
        Return 1.0 - MeanSquareError * vData.Length / v
    End Function
#End Region

#Region "Counting"
    Public Shared Function SingChanges(ByVal Input() As Double) As Integer
        Dim i, ii As Integer
        Dim c As Integer

        ii = Input.Length - 2
        c = 0
        For i = 0 To ii
            If Math.Sign(Input(i)) * Math.Sign(Input(i + 1)) < 0 Then c += 1
        Next

        Return c
    End Function

    Public Shared Function SingChanges(ByVal Input As List(Of Double)) As Integer
        Return SingChanges(Input.ToArray())
    End Function
#End Region

#Region "Not used"
    'Public Shared Function ListToArray(ByVal aList As List(Of Double)) As Double()
    '    Dim aArray As Double() = New Double() {}
    '    Array.Resize(aArray, aList.Count)
    '    aList.CopyTo(aArray)
    '    Return aArray
    'End Function

    'Public Shared Function ToDouble(ByVal Input() As Integer) As Double()
    '    Return Array.ConvertAll(Input, New System.Converter(Of Integer, Double)(AddressOf Convert.ToDouble))
    'End Function
#End Region

#Region "Digital filter"
    Public Shared Function LowPassFilter(ByVal Coef As Double, ByVal Input As Double()) As Double()
        Dim i, ii As Integer
        'copy the data
        Dim Output As Double() = Input
        ii = Output.Length - 1
        'forward
        For i = 1 To ii
            Output(i) = Coef * Output(i) + (1.0 - Coef) * Output(i - 1)
        Next
        'backward
        For i = ii - 1 To 0 Step -1
            Output(i) = Coef * Output(i) + (1.0 - Coef) * Output(i + 1)
        Next
        'return
        Return Output
    End Function

#End Region

#Region "Sort/random"
    Public Shared Sub SortUnique(ByVal Data As List(Of Double), ByRef QuiqueData As Double(), ByRef Count As Double())
        'copy to a new list and then sort
        Dim L As New List(Of Double)
        L.AddRange(Data)
        L.Sort()
        'check unique
        Dim x As New List(Of Double)
        Dim y As New List(Of Double)
        Dim v As Double = L(0)
        Dim c As Integer = 1
        For i As Integer = 1 To L.Count - 1
            If L(i) = v Then
                c += 1
            Else
                x.Add(v)
                y.Add(c)

                v = L(i)
                c = 1
            End If
        Next
        'last
        x.Add(v)
        y.Add(c)
        'return
        QuiqueData = x.ToArray()
        Count = y.ToArray()
    End Sub

    Public Shared Sub SortByKeyUniqueMemberCount(ByVal Key As List(Of Double), ByVal Data As List(Of Double), _
                                                 ByRef UniqueKey As Double(), ByRef UniqueMemberCount As Double())
        'copy array
        Dim x As Double() = Key.ToArray()
        Dim y As Double() = Data.ToArray()
        'sort array
        Array.Sort(x, y)
        'average those are the same
        Dim Lx As New List(Of Double)
        Dim Ly As New List(Of Double)
        Dim L As New List(Of Double)
        Dim xKey As Double
        'Dim Num As Integer
        Dim i, ii As Integer

        ii = x.Length - 1
        xKey = x(0)
        L.Add(y(0))
        For i = 1 To ii
            If x(i) = xKey Then
                If Not L.Contains(y(i)) Then L.Add(y(i))
            Else
                'save last
                Lx.Add(xKey)
                Ly.Add(L.Count)
                'prepare for next
                xKey = x(i)
                L.Clear()
                L.Add(y(i))
            End If
        Next
        'add the last pair
        Lx.Add(xKey)
        Ly.Add(L.Count)

        'return
        UniqueKey = Lx.ToArray()
        UniqueMemberCount = Ly.ToArray()
    End Sub


    Public Shared Sub SortByKeyAverage(ByVal Key As List(Of Double), ByVal Data As List(Of Double), _
                                       ByRef UniqueKey As Double(), ByRef DataAverage As Double())
        'copy array
        Dim x As Double() = Key.ToArray()
        Dim y As Double() = Data.ToArray()
        'sort array
        Array.Sort(x, y)
        'average those are the same
        Dim Lx As New List(Of Double)
        Dim Ly As New List(Of Double)
        Dim aKey, Sum As Double
        Dim Num As Integer
        Dim i, ii As Integer

        ii = x.Length - 1
        aKey = x(0)
        Sum = y(0)
        Num = 1
        For i = 1 To ii
            If x(i) = aKey Then
                'sum
                Sum += y(i)
                Num += 1
            Else
                'save last
                Lx.Add(aKey)
                Ly.Add(Sum / Num)
                'prepare for next
                aKey = x(i)
                Sum = y(i)
                Num = 1
            End If
        Next
        'add the last pair
        Lx.Add(aKey)
        Ly.Add(Sum / Num)

        'return
        UniqueKey = Lx.ToArray()
        DataAverage = Ly.ToArray()
    End Sub

    Public Shared Sub SortByKeyMinMax(ByVal Key As List(Of Double), ByVal Data As List(Of Double), _
                                      ByRef UniqueKey As Double(), ByRef DataMin As Double(), ByRef DataMax As Double())
        'copy array
        Dim x As Double() = Key.ToArray()
        Dim y As Double() = Data.ToArray()
        'sort array
        Array.Sort(x, y)
        'average those are the same
        Dim Lx As New List(Of Double)
        Dim Lmin As New List(Of Double)
        Dim Lmax As New List(Of Double)
        Dim aKey As Double
        Dim Min, Max As Double
        Dim i, ii As Integer

        ii = x.Length - 1
        aKey = x(0)
        Min = y(0)
        Max = y(0)
        For i = 1 To ii
            If x(i) = aKey Then
                'check
                If y(i) > Max Then
                    Max = y(i)
                ElseIf y(i) < Min Then
                    Min = y(i)
                End If
            Else
                'save last
                Lx.Add(aKey)
                Lmin.Add(Min)
                Lmax.Add(Max)
                'prepare for next
                aKey = x(i)
                Min = y(i)
                Max = y(i)
            End If
        Next
        'add the last pair
        Lx.Add(aKey)
        Lmin.Add(Min)
        Lmax.Add(Max)

        UniqueKey = Lx.ToArray()
        DataMin = Lmin.ToArray()
        DataMax = Lmax.ToArray()
    End Sub

    Public Shared Sub SortByKeyRange(ByVal Key As List(Of Double), ByVal Data As List(Of Double), _
                                     ByRef UniqueKey As Double(), ByRef DataRange As Double())
        Dim Min As Double() = New Double() {}
        Dim Max As Double() = New Double() {}
        SortByKeyMinMax(Key, Data, UniqueKey, Min, Max)
        DataRange = Subtract(Max, Min)
    End Sub

    Public Shared Function Shuffle(Of T)(ByVal OldList As List(Of T)) As List(Of T)
        Dim k, i, ii As Integer
        Dim tmp As T
        Dim Ran As New Random

        'copy the old list to new list to preserve the oldlist
        Dim NewList As New List(Of T)(OldList)

        'shuffle the new list, 
        ii = NewList.Count - 1
        For i = ii To 0 Step -1
            k = Ran.Next(i)
            tmp = NewList(i)
            NewList(i) = NewList(k)
            NewList(k) = tmp
        Next

        Return NewList
    End Function

    Public Shared Function Shuffle(Of T)(ByVal OldArray As T()) As T()
        Return Shuffle(New List(Of T)(OldArray)).ToArray()
    End Function

#End Region

#Region "get value from non-integer index"
    Public Shared Function GetValueAtIndex(ByVal Data() As Double, ByVal Index As Double) As Double
        Dim i As Integer
        i = Convert.ToInt32(Math.Floor(Index))
        Return Data(i) + (Index - i) * (Data(i + 1) - Data(i))
    End Function
#End Region

#Region "get a value from a curve"
    ''' <summary>
    ''' Given a curve defined by X() and Y(), get all the Y values corresponding to X0
    ''' using linear interpolation bteween two nearest neighbours
    ''' </summary>
    ''' <param name="X0"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns>zero length array if nothing is found</returns>
    ''' <remarks></remarks>
    Public Shared Function GetYAll(ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double) As Double()
        Dim i, ii As Integer
        Dim v1, v2 As Double
        Dim Y0 As New List(Of Double)

        ii = X.Length - 1
        For i = 0 To ii - 1
            v1 = X0 - X(i)
            If v1 = 0 Then
                Y0.Add(Y(i))
                Continue For
            End If

            v2 = X0 - X(i + 1)
            If v2 = 0 Then Continue For 'let the next iteration handle this, otherwise it will be double counted

            If v1 * v2 < 0 Then
                v1 *= Y(i) - Y(i + 1)
                v1 /= X(i) - X(i + 1)       'X(i) - X(i + 1) <> 0 because v1 * v2 < 0
                v1 += Y(i)
                Y0.Add(v1)
            End If
        Next

        Return Y0.ToArray()
    End Function

    ''' <summary>
    ''' Given a curve defined by X() and Y(), get the first Y values corresponding to X0
    ''' </summary>
    ''' <param name="X0"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns>return NaN if nothing is found</returns>
    ''' <remarks></remarks>
    Public Shared Function GetYFirst(ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double) As Double
        Return GetY(0, X0, X, Y, True)
    End Function

    ''' <summary>
    ''' Given a curve defined by X() and Y(), get the first Y values corresponding to X0
    ''' in the range from StartIndex to the end of the array
    ''' </summary>
    ''' <param name="StartIndex"></param>
    ''' <param name="X0"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetYFirst(ByVal StartIndex As Integer, ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double) As Double
        Return GetY(StartIndex, X0, X, Y, True)
    End Function

    ''' <summary>
    ''' Given a curve defined by X() and Y(), get the last Y values corresponding to X0
    ''' </summary>
    ''' <param name="X0"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns>return NaN if nothing is found</returns>
    ''' <remarks></remarks>
    Public Shared Function GetYLast(ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double) As Double
        Return GetY(X.Length, X0, X, Y, False)
    End Function

    ''' <summary>
    ''' Given a curve defined by X() and Y(), get the last Y values corresponding to X0
    ''' in the array range from 0 to EndIndex
    ''' </summary>
    ''' <param name="EndIndex"></param>
    ''' <param name="X0"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetYLast(ByVal EndIndex As Integer, ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double) As Double
        Return GetY(EndIndex, X0, X, Y, False)
    End Function

    Private Shared Function GetY(ByVal StartIndex As Integer, ByVal X0 As Double, ByVal X() As Double, ByVal Y() As Double, ByVal SearchForward As Boolean) As Double
        Dim i, j As Integer
        Dim iMax As Integer
        Dim iStart, iStop, iStep As Integer
        Dim v1, v2 As Double

        iMax = X.Length - 2         '-1 for upper bound, -1 again for (i+1) 


        If StartIndex < 0 Then
            StartIndex = 0
        ElseIf StartIndex > iMax Then
            StartIndex = iMax
        End If
        iStart = StartIndex

        If SearchForward Then
            iStop = iMax
            iStep = 1
        Else
            iStop = 0
            iStep = -1
        End If

        For i = iStart To iStop Step iStep
            v1 = X0 - X(i)
            If v1 = 0 Then Return Y(i)

            v2 = X0 - X(i + 1)
            If v2 = 0 Then Return Y(i + 1)

            If v1 * v2 < 0 Then
                v1 *= Y(i) - Y(i + 1)
                v1 /= X(i) - X(i + 1)       'X(i) - X(i + 1) <> 0 because v1 * v2 < 0
                v1 += Y(i)
                Return v1
            End If
        Next

        'X0 outside iStart, iStop
        'use nearest end points to estimate Y0
        'note that iMax is not the end points
        If iStop = iMax Then iStop = iMax + 1
        If iStart = iMax Then iStart = iMax + 1

        v1 = X0 - X(iStart)
        v2 = X0 - X(iStop)
        If Math.Abs(v1) <= Math.Abs(v2) Then
            i = iStart
            j = iStart + iStep
        Else
            i = iStop
            j = iStop - iStep
        End If

        v1 = X0 - X(i)
        v1 *= Y(i) - Y(j)
        v1 /= X(i) - X(j)       'X(i) - X(i + 1) <> 0 because = 0 case is already treated
        v1 += Y(i)
        Return v1

    End Function

#End Region

#Region "Scan parameter list"
    Public Shared Function BuildScanList(ByVal vStart As Double, ByVal vStop As Double, ByVal vStep As Double) As Double()
        Dim vList As New List(Of Double)
        Dim v1, v2 As Double
        Dim v As Double

        Select Case vStep
            Case 0
                Return New Double() {vStart, vStop}

            Case Is > 0
                v1 = Math.Min(vStart, vStop)
                v2 = Math.Max(vStart, vStop)
                v = v1
                While v <= v2
                    vList.Add(v)
                    v += vStep
                End While

            Case Is < 0
                v1 = Math.Max(vStart, vStop)
                v2 = Math.Min(vStart, vStop)
                v = v1
                While v >= v2
                    vList.Add(v)
                    v += vStep
                End While
        End Select

        'make sure end is included
        If vList.Count > 0 Then
            If vList(vList.Count - 1) <> v2 Then vList.Add(v2)
        End If

        'return
        Return vList.ToArray()
    End Function

    Public Shared Function ValidateScanParameter(ByVal vStart As Double, ByVal vStop As Double, _
                                                 ByVal vStep As Double, ByVal ShowError As Boolean) As Boolean
        Dim s As String = ""
        Select Case vStep
            Case 0
                s = "Step cannot be zero!"
            Case Is > 0
                If vStart > vStop Then
                    s = "Inconsistent setting: Step > 0 but Start > Stop"
                End If
            Case Is < 0
                If vStart < vStop Then
                    s = "Inconsistent setting: Step < 0 but Start < Stop"
                End If
        End Select

        If s = "" Then
            Return True
        Else
            If ShowError Then
                s += ControlChars.CrLf + "Start: " & vStart
                s += ControlChars.CrLf + "Stop:  " & vStop
                s += ControlChars.CrLf + "Step:  " & vStep
                MessageBox.Show(s, "Scan parameter", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return False
        End If
    End Function
#End Region

#Region "trajectory"
    Public Shared Sub GetSpiral(ByVal Length As Integer, ByVal StepSize As Double, ByVal X0 As Double, ByVal Y0 As Double, ByRef X() As Double, ByRef Y() As Double)
        Dim i, ii As Integer
        Dim k, kk As Integer

        ii = Length - 1
        ReDim X(ii), Y(ii)

        X(0) = X0
        Y(0) = Y0

        i = 1
        kk = 1
        While i <= ii
            'move x first
            For k = 1 To kk
                X(i) = X(i - 1) + StepSize
                Y(i) = Y(i - 1)
                i += 1
                If i > ii Then Return
            Next
            'move y now
            For k = 1 To kk
                X(i) = X(i - 1)
                Y(i) = Y(i - 1) + StepSize
                i += 1
                If i > ii Then Return
            Next
            'next 
            StepSize = -1.0 * StepSize
            kk += 1
        End While
    End Sub

    Public Shared Sub GetSpiralMatrixIndex(ByVal Length As Integer, ByVal StepSizeSign As Double, ByRef X() As Integer, ByRef Y() As Integer)
        Dim i, ii As Integer
        Dim k, kk As Integer
        Dim MinX, MinY As Integer
        Dim StepSize As Integer

        ii = Length - 1
        ReDim X(ii), Y(ii)

        X(0) = 0
        Y(0) = 0
        StepSize = Math.Sign(StepSizeSign)
        If StepSize = 0 Then StepSize = 1

        MinX = X(0)
        MinY = Y(0)

        i = 1
        kk = 1
        While i <= ii
            'move x first
            For k = 1 To kk
                X(i) = X(i - 1) + StepSize
                Y(i) = Y(i - 1)
                If X(i) < MinX Then MinX = X(i)
                If Y(i) < MinY Then MinY = Y(i)
                i += 1
                If i > ii Then Exit While
            Next
            'move y now
            For k = 1 To kk
                X(i) = X(i - 1)
                Y(i) = Y(i - 1) + StepSize
                If X(i) < MinX Then MinX = X(i)
                If Y(i) < MinY Then MinY = Y(i)
                i += 1
                If i > ii Then Exit While
            Next
            'next 
            StepSize = -StepSize
            kk += 1
        End While

        'now we need to offset all the array so that they starts with (0, 0)
        For i = 0 To ii
            X(i) -= MinX
            Y(i) -= MinY
        Next
    End Sub
#End Region

#Region "string related"
    Public Shared Function Concatenate(ByVal Delimiter As String, ByVal ParamArray Parts As Object()) As String
        Dim x, y As Object
        Dim aArray As Array
        Dim s As String = ""
        For Each x In Parts
            If TypeOf x Is Array Then
                aArray = CType(x, Array)
                For Each y In aArray
                    If TypeOf y Is Double Then y = CType(y, Single)
                    s += (Delimiter + y.ToString)
                Next y
            Else
                If TypeOf x Is Double Then x = CType(x, Single)
                s += (Delimiter + x.ToString)
            End If
        Next x
        If s.StartsWith(Delimiter) Then s = s.Substring(Delimiter.Length)
        Return s
    End Function

    Public Shared Function Concatenate(ByVal Delimiter As String, ByVal fmt As String, ByVal Data As Double()) As String
        Dim v As Double
        Dim s As String = ""

        For Each v In Data
            If fmt.StartsWith("{") Then
                s += Delimiter + String.Format(fmt, v)
            Else
                s += Delimiter + v.ToString(fmt)
            End If
        Next
        If s.StartsWith(Delimiter) Then s = s.Substring(Delimiter.Length)
        Return s
    End Function

    'Public Shared Function SplitToInteger(ByVal s As String, ByVal Delimiter As Char) As Integer()
    '    Dim sData As String()
    '    Dim vData As Integer() = New Integer() {}
    '    Dim i, ii As Integer
    '    Dim k As Integer

    '    If s = "" Then Return vData


    '    sData = 
    '    ii = sData.Length - 1
    '    ReDim vData(ii)
    '    k = 0
    '    For i = 0 To ii
    '        Try
    '            vData(k) = Integer.Parse(sData(i))
    '            k += 1
    '        Catch ex As Exception
    '            'ignore
    '        End Try
    '    Next

    '    If k <> 0 Then k = k - 1
    '    If k < ii Then ReDim vData(k)

    '    Return vData
    'End Function
#End Region
End Class
