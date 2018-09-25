Option Strict On
Option Explicit On

Public Class w2PeakDetector
    Private Const DefaultCoef As Double = 0.5
    Private Const DefaultExclusion As Integer = 8

    Private mValue As List(Of Double)
    Private mFV As List(Of Double)
    Private mDV As List(Of Double)
    Private mCoef As Double
    Private mExclusion As Integer
    Private mHavePeak As Boolean
    Private mHaveValley As Boolean
    Private mMin As Double
    Private mMax As Double

    Public Sub New()
        mValue = New List(Of Double)
        mFV = New List(Of Double)
        mDV = New List(Of Double)
        mCoef = DefaultCoef
        mExclusion = DefaultExclusion
    End Sub

#Region "properties"
    Public ReadOnly Property Data() As Double()
        Get
            Return mValue.ToArray()
        End Get
    End Property

    Public Property LowPassCoefficient() As Double
        Get
            Return mCoef
        End Get
        Set(ByVal value As Double)
            mCoef = value
        End Set
    End Property

    Public Property Exclusion() As Integer
        Get
            Return mExclusion
        End Get
        Set(ByVal value As Integer)
            mExclusion = value
        End Set
    End Property

    Public ReadOnly Property HavePeak() As Boolean
        Get
            Return mHavePeak
        End Get
    End Property

    Public ReadOnly Property HaveValley() As Boolean
        Get
            Return mHaveValley
        End Get
    End Property

    Public ReadOnly Property FirstPeakIndex() As Integer
        Get
            If Not mHavePeak Then Return -1
            Return GetIndex(True)
        End Get
    End Property

    Public ReadOnly Property FirstValleyIndex() As Integer
        Get
            If Not mHaveValley Then Return -1
            Return GetIndex(False)
        End Get
    End Property

    Private Function GetIndex(ByVal NeedPeak As Boolean) As Integer
        Dim i, ii As Integer
        Dim v(), dV() As Double

        ii = mFV.Count - 1
        ReDim v(ii)
        ReDim dV(ii)

        'do a backward filtering
        v(ii) = mFV(ii)
        For i = ii - 1 To 1 Step -1       'mFV(0) is singularity, ignore
            v(i) = mCoef * mFV(i) + (1.0 - mCoef) * v(i + 1)
        Next

        'calculate the difference, note the valid value is from index 1 to ii-1
        dV(1) = v(2) - v(1)
        For i = 2 To ii - 2
            dV(i) = v(i + 1) - v(i)
            If dV(i) * dV(i - 1) <= 0 Then
                'dV(i) = V(i+1) - v(i), so i is the center, return i will reflect the peak/valley
                If dV(i) < 0 OrElse dV(i - 1) > 0 Then
                    If NeedPeak Then Return i
                Else
                    If (Not NeedPeak) Then Return i
                End If
            End If
        Next

        Return -1
    End Function

    Public ReadOnly Property Min() As Double
        Get
            Return mMin
        End Get
    End Property

    Public ReadOnly Property Max() As Double
        Get
            Return mMax
        End Get
    End Property

#End Region

    Public Sub Add(ByVal value As Double)
        Dim i As Integer

        'add the original value
        mValue.Add(value)

        'get min/max
        Select Case True
            Case mValue.Count = 1
                mMin = value
                mMax = value
            Case mMin > value
                mMin = value
            Case mMax < value
                mMax = value
        End Select
        
        'calculate the filtered value, and its delta
        i = mValue.Count - 1
        If i = 0 Then
            mFV.Add(value)
        Else
            value = mCoef * value + (1.0 - mCoef) * mFV(i - 1)
            mFV.Add(value)
            'calculate delat
            If i > 1 Then
                'first i-1 equals 1, mFV(0) is a singularity without filtering, ignore 
                value = mFV(i) - mFV(i - 1)
                mDV.Add(value)
            End If
            'check peak valley
            If i > mExclusion Then
                i = mDV.Count - 1
                If i > 0 Then
                    If (mDV(i) * mDV(i - 1)) <= 0 Then
                        If mDV(i) < 0 OrElse mDV(i - 1) > 0 Then
                            mHavePeak = True            'peak makes last point negative
                        Else
                            mHaveValley = True          'valley makes last point positive
                        End If
                    End If
                End If
            End If

        End If
    End Sub

    Public Sub Add(ByVal values() As Double)
        For Each value As Double In values
            Me.Add(values)
        Next
    End Sub

    Public Sub ClearPeak()
        mHavePeak = False
    End Sub

    Public Sub ClearValley()
        mHaveValley = False
    End Sub

    Public Sub Clear()
        mValue.Clear()
        mFV.Clear()
        mDV.Clear()
        mHavePeak = False
        mHaveValley = False
    End Sub

#Region "peak valley functions"
    Public Function GetPeakIndexes() As Double()
        Dim p() As Double = New Double() {}
        Dim v() As Double = New Double() {}

        GetIndexes(mValue.ToArray(), mCoef, p, v)
        Return p
    End Function

    Public Function GetValleyIndexes() As Double()
        Dim p() As Double = New Double() {}
        Dim v() As Double = New Double() {}

        GetIndexes(mValue.ToArray(), mCoef, p, v)
        Return v
    End Function

    Public Sub GetIndexes(ByRef PeakIndex() As Double, ByRef ValleyIndex() As Double)
        GetIndexes(mValue.ToArray(), mCoef, PeakIndex, ValleyIndex)
    End Sub
#End Region

#Region "shared functions"
    Public Shared Function GetPeakIndexes(ByVal value() As Double) As Double()
        Return GetPeakIndexes(value, DefaultCoef)
    End Function

    Public Shared Function GetPeakIndexes(ByVal value() As Double, ByVal FilterCoef As Double) As Double()
        Dim p() As Double = New Double() {}
        Dim v() As Double = New Double() {}

        GetIndexes(value, FilterCoef, p, v)
        Return p
    End Function

    Public Shared Function GetValleyIndexes(ByVal value() As Double) As Double()
        Return GetValleyIndexes(value, DefaultCoef)
    End Function

    Public Shared Function GetValleyIndexes(ByVal value() As Double, ByVal FilterCoef As Double) As Double()
        Dim p() As Double = New Double() {}
        Dim v() As Double = New Double() {}

        GetIndexes(value, FilterCoef, p, v)
        Return v
    End Function

    Public Shared Sub GetIndexes(ByVal value() As Double, ByRef PeakIndex() As Double, ByRef ValleyIndex() As Double)
        GetIndexes(value, DefaultCoef, PeakIndex, ValleyIndex)
    End Sub

    Public Shared Sub GetIndexes(ByVal value() As Double, ByVal FilterCoef As Double, ByRef PeakIndex() As Double, ByRef ValleyIndex() As Double)
        Dim i, ii As Integer
        Dim x(), dx() As Double
        Dim iZero As Double
        Dim p, v As New List(Of Double)

        ii = value.Length - 1
        If ii < 2 Then Return

        ReDim x(ii)
        ReDim dx(ii)

        'forward filtering
        x(0) = value(0)
        For i = 1 To ii
            x(i) = FilterCoef * value(i) + (1.0 - FilterCoef) * x(i - 1)
        Next

        'backward filtering
        For i = ii - 1 To 1 Step -1
            x(i) = FilterCoef * x(i) + (1.0 - FilterCoef) * x(i + 1)
        Next

        'get difference
        For i = 1 To ii - 2
            dx(i) = x(i + 1) - x(i)
        Next

        'check zero crossing
        For i = 2 To ii - 2
            If dx(i) * dx(i - 1) < 0 Then
                'get the partial index where d will be zero
                iZero = dx(i) - dx(i - 1)
                iZero = dx(i - 1) / iZero
                iZero = (i - 1) - iZero
                iZero += 0.5                'add the zero because if the two are equal, the peak is in the middle of the two
                If dx(i) < 0 OrElse dx(i - 1) > 0 Then
                    p.Add(iZero)
                Else
                    v.Add(iZero)
                End If
            End If
        Next

        'return
        PeakIndex = p.ToArray()
        ValleyIndex = v.ToArray()
    End Sub

#Region "Dominant"
    Public Shared Function GetDominantPeakIndex(ByVal Value() As Double) As Double
        Dim p, v As Double
        GetDominantPeakValleyIndex(Value, DefaultCoef, p, v)
        Return p
    End Function

    Public Shared Function GetDominantValleyIndex(ByVal Value() As Double) As Double
        Dim p, v As Double
        GetDominantPeakValleyIndex(Value, DefaultCoef, p, v)
        Return v
    End Function

    Public Shared Function GetDominantPeakIndex(ByVal Value() As Double, ByVal FilterCoef As Double) As Double
        Dim p, v As Double
        GetDominantPeakValleyIndex(Value, FilterCoef, p, v)
        Return p
    End Function

    Public Shared Function GetDominantValleyIndex(ByVal Value() As Double, ByVal FilterCoef As Double) As Double
        Dim p, v As Double
        GetDominantPeakValleyIndex(Value, FilterCoef, p, v)
        Return v
    End Function
    Public Shared Sub GetDominantPeakValleyIndex(ByVal value() As Double, ByRef PeakIndex As Double, ByRef ValleyIndex As Double)
        GetDominantPeakValleyIndex(value, DefaultCoef, PeakIndex, ValleyIndex)
    End Sub

    Public Shared Sub GetDominantPeakValleyIndex(ByVal value() As Double, ByVal FilterCoef As Double, ByRef PeakIndex As Double, ByRef ValleyIndex As Double)
        Dim Min, Max As Double
        Dim iMin, iMax As Integer
        Dim i, idx As Integer
        Dim Diff, MinDiff As Double
        Dim p() As Double = New Double() {}
        Dim v() As Double = New Double() {}

        'get simple peak and valley
        w2Array.MinMax(value, Min, iMin, Max, iMax)

        'get all peak and valley
        GetIndexes(value, FilterCoef, p, v)

        'find the peak index closest to iMax
        Select Case p.Length
            Case 0
                PeakIndex = Double.NaN
            Case 1
                PeakIndex = p(0)
            Case Else
                For i = 0 To p.Length - 1
                    Diff = Math.Abs(p(i) - iMax)
                    If i = 0 Then
                        MinDiff = Diff
                        idx = i
                    ElseIf MinDiff > Diff Then
                        MinDiff = Diff
                        idx = i
                    End If
                Next
                PeakIndex = p(idx)
        End Select

        'find the valley index closest to iMin
        Select Case v.Length
            Case 0
                ValleyIndex = Double.NaN
            Case 1
                ValleyIndex = v(0)
            Case Else
                For i = 0 To v.Length - 1
                    Diff = Math.Abs(v(i) - iMin)
                    If i = 0 Then
                        MinDiff = Diff
                        idx = i
                    ElseIf MinDiff > Diff Then
                        MinDiff = Diff
                        idx = i
                    End If
                Next
                ValleyIndex = v(idx)
        End Select

    End Sub
#End Region
#End Region
End Class
