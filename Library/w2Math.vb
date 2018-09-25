Option Explicit On
Option Strict On

Namespace w2Math
    Public Class Matrix

        Shared Function DotProduct(ByVal X() As Double, ByVal Y() As Double) As Double
            Dim i, ii As Integer
            ii = Math.Min(X.Length, Y.Length) - i

            Dim v As Double = 0.0
            For i = 0 To ii
                v += (X(i) * Y(i))
            Next

            Return v
        End Function

        Shared Function DotProduct(ByVal X(,) As Double, ByVal Y() As Double) As Double()
            Dim i, j, ii As Integer
            ii = Y.Length - 1

            Dim Z(ii) As Double
            For i = 0 To ii
                Z(i) = 0
                For j = 0 To ii
                    Z(i) += (X(i, j) * Y(j))
                Next
            Next

            Return Z
        End Function

        ''' <summary>
        ''' This is Gauss-Jardan matrix inversion for solving the linear equation.
        ''' It is a modified version of that from Numeric Recipes 
        ''' </summary>
        ''' <param name="A">Input matrix</param>
        ''' <returns>Inversed matrix </returns>
        ''' <remarks></remarks>
        Shared Function Inverse(ByVal A(,) As Double) As Double(,)
            If A.GetLength(0) <> A.GetLength(1) Then Return Nothing
            Dim ii As Integer = A.GetLength(0) - 1

            'copy matrix 
            Dim X(ii, ii) As Double
            Array.Copy(A, X, A.Length)

            'local variables
            Dim big, tmp, pivinv As Double
            Dim indcol(ii), indrow(ii), piv(ii) As Integer
            Dim i, j, k, col, row As Integer

            'fill the pivotal as zero
            For j = 0 To ii
                piv(j) = 0
            Next j

            'loop
            For i = 0 To ii
                'search for povotal 
                big = 0.0
                For j = 0 To ii
                    If piv(j) <> 1 Then
                        For k = 0 To ii
                            If piv(k) = 0 Then
                                If Math.Abs(X(j, k)) >= big Then
                                    big = Math.Abs(X(j, k))
                                    row = j
                                    col = k
                                End If
                            ElseIf piv(k) > 1 Then
                                ' check for singular
                                Return Nothing
                            End If
                        Next
                    End If
                Next

                piv(col) += 1

                If row <> col Then
                    For j = 0 To ii
                        tmp = X(row, j)
                        X(row, j) = X(col, j)
                        X(col, j) = tmp
                    Next
                End If
                indrow(i) = row
                indcol(i) = col

                'divide pivot row by the pivot element
                If X(col, col) = 0 Then Return Nothing ' check for singular
                pivinv = 1.0 / X(col, col)
                X(col, col) = 1.0

                For j = 0 To ii
                    X(col, j) *= pivinv
                Next

                'reduce the row except for the pivot
                For j = 0 To ii
                    If j <> col Then
                        tmp = X(j, col)
                        X(j, col) = 0.0
                        For k = 0 To ii
                            X(j, k) -= (X(col, k) * tmp)
                        Next
                    End If
                Next
            Next

            'swap value
            For j = ii To 0 Step -1
                If indrow(j) <> indcol(j) Then
                    For k = 0 To ii
                        tmp = X(k, indrow(j))
                        X(k, indrow(j)) = X(k, indcol(j))
                        X(k, indcol(j)) = tmp
                    Next
                End If
            Next

            Return X
        End Function
    End Class

    Public Class CurveFit

#Region "arbitary function"
        ''' <summary>
        ''' Delegate function for the fit
        ''' Calculate Y, and dY/dCeof from X, and Coef
        ''' </summary>
        ''' <param name="X">x value of the function</param>
        ''' <param name="Coef">function coefficient array</param>
        ''' <param name="Y">resulting y</param>
        ''' <param name="dYdCeof">resulting dY/dCeof array</param>
        ''' <remarks></remarks>
        Delegate Sub FitFunction(ByVal X As Double, ByVal Coef() As Double, ByRef Y As Double, ByRef dYdCeof() As Double)

        ''' <summary>
        ''' This is the so called Levenberg-Marquardt Method for nonlinear curve fitting.
        ''' It is a modified version of that from Numeric Recipes. 
        ''' The the equation being fitted is passed over using Delegate!        
        ''' W2 08/12/2003  Original
        ''' W2 11/30/2007  Modified for .NET2005 taking the advantage of delegate
        ''' </summary>
        ''' <param name="X">Input: data array for x</param>
        ''' <param name="Y">Input: data array for y</param>
        ''' <param name="Coef">Input/Output: in as initial coefficient gass, out as the final best result</param>
        ''' <param name="ChiSq">Output: the final Chi square of the fit</param>
        ''' <param name="Tolerance">Input: fit stop paramter, fit stops when the relative difference the Chi square between iteration is smaller than this number </param>
        ''' <returns>Total number of iteration performed</returns>
        ''' <remarks></remarks>
        ''' 
        Public Shared Function NonlinearFit(ByVal X() As Double, ByVal Y() As Double, ByRef Coef() As Double, _
                                            ByRef ChiSq As Double, ByVal Tolerance As Double, ByVal TheFunction As FitFunction) As Integer

            Const MaxTrial As Integer = 2000

            Dim M, j As Integer
            Dim lamda As Double
            Dim alpha1(,), beta1() As Double
            Dim alpha2(,), beta2() As Double
            Dim ChiSq1, ChiSq2 As Double
            Dim dA() As Double
            Dim ATry() As Double
            Dim Trial As Integer
            Dim bLoop As Boolean

            'get ready
            M = Coef.Length - 1
            ReDim ATry(M)
            alpha1 = New Double(,) {}
            beta1 = New Double() {}
            alpha2 = New Double(,) {}
            beta2 = New Double() {}

            'do 1st try chi square
            MRQCof(X, Y, Coef, alpha1, beta1, ChiSq1, TheFunction)

            bLoop = True
            Trial = 0
            lamda = 0.001
            While bLoop And Trial < MaxTrial
                'update counter
                Trial += 1

                'Marquardt insight
                For j = 0 To M
                    alpha1(j, j) *= (1 + lamda)
                Next j

                'get next set of A
                dA = Matrix.DotProduct(Matrix.Inverse(alpha1), beta1)
                For j = 0 To M
                    ATry(j) = Coef(j) + dA(j)
                Next j

                'get next chi square
                Call MRQCof(X, Y, ATry, alpha2, beta2, ChiSq2, TheFunction)

                'compare result
                If ChiSq2 <= ChiSq1 Then
                    'exit loop if no significant change is seen
                    bLoop = (ChiSq1 - ChiSq2) > Tolerance * ChiSq1
                    'reduce the parameter
                    lamda = lamda * 0.01
                    'replace results for next try
                    ChiSq1 = ChiSq2
                    Coef = ATry
                    beta1 = beta2
                    alpha1 = alpha2
                Else
                    'exit loop if lamda is already very small
                    bLoop = (lamda > 1.0E-50)
                    'increase the parameter
                    lamda = lamda * 10.0#
                End If

            End While

            ChiSq = ChiSq1
            Return Trial
        End Function

        Private Shared Sub MRQCof(ByVal X() As Double, ByVal Y() As Double, ByRef A() As Double, _
                                  ByRef alpha(,) As Double, ByRef beta() As Double, ByRef ChiSq As Double, _
                                  ByVal TheFunction As FitFunction)
            Dim i, j, k As Integer
            Dim N, M As Integer

            Dim dYdA() As Double = New Double() {}
            Dim YFit As Double
            Dim dY As Double

            N = UBound(X)
            M = UBound(A)

            ReDim alpha(M, M)
            ReDim beta(M)

            ChiSq = 0.0
            For i = 0 To N
                'get Yfit and dY/dA
                TheFunction(X(i), A, YFit, dYdA)

                'calculate alpha, beta, and Chi Square
                dY = Y(i) - YFit
                ChiSq += (dY ^ 2.0)

                For j = 0 To M
                    For k = 0 To j
                        alpha(j, k) += (dYdA(j) * dYdA(k))
                    Next k
                    beta(j) += (dYdA(j) * dY)
                Next j
            Next i

            'alpha is a symmetric matrix, fill the other half
            For j = 1 To M
                For k = 1 To j - 1
                    alpha(k, j) = alpha(j, k)
                Next k
            Next j

        End Sub

#End Region

#Region "Linear fit"
        Public Shared Sub LinearFit(ByVal X() As Double, ByVal Y() As Double, _
                                    ByRef Coef() As Double, ByRef RSquared As Double)
            LinearFit(X, Y, Nothing, Coef, Nothing, Nothing, Nothing, Nothing, RSquared)
        End Sub

        Public Shared Sub LinearFit(ByVal X() As Double, ByVal Y() As Double, _
                                    ByRef Coef() As Double, ByVal YFit() As Double, ByRef RSquared As Double)
            LinearFit(X, Y, Nothing, Coef, Nothing, YFit, Nothing, Nothing, RSquared)
        End Sub

        Public Shared Sub LinearFit(ByVal X() As Double, ByVal Y() As Double, _
                                    ByRef Coef() As Double, _
                                    ByRef MSE As Double, ByRef RSquared As Double)
            LinearFit(X, Y, Nothing, Coef, Nothing, Nothing, MSE, Nothing, RSquared)
        End Sub

        Public Shared Sub LinearFit(ByVal X() As Double, ByVal Y() As Double, _
                                    ByRef Coef() As Double, ByVal YFit() As Double, _
                                    ByRef MSE As Double, ByRef RSquared As Double)
            LinearFit(X, Y, Nothing, Coef, Nothing, YFit, MSE, Nothing, RSquared)
        End Sub

        Public Shared Sub LinearFit(ByVal X() As Double, ByVal Y() As Double, ByVal Ystdev() As Double, _
                                    ByRef Coef() As Double, ByRef CoefStdev() As Double, _
                                    ByRef Yfit() As Double, ByRef mse As Double, _
                                    ByRef ChiSquare As Double, ByRef RSquared As Double)
            Dim ss, sx, sy As Double
            Dim w As Double
            Dim t, sxoss, st2 As Double
            Dim a, b As Double
            Dim i, ii, n As Integer
            Dim NoStdev As Boolean

            n = X.Length
            ii = n - 1

            NoStdev = (Ystdev Is Nothing)
            If NoStdev Then
                ReDim Ystdev(ii)
                For i = 0 To ii
                    Ystdev(i) = 1.0
                Next
            End If

            ss = 0.0
            sx = 0.0
            sy = 0.0
            st2 = 0.0
            b = 0.0
            For i = 0 To ii
                w = Ystdev(i) * Ystdev(i)
                w = 1.0 / w
                ss += w
                sx += X(i) * w
                sy += Y(i) * w
            Next

            sxoss = sx / ss
            For i = 0 To ii
                t = (X(i) - sxoss) / Ystdev(i)
                st2 += t * t
                b += t * Y(i) / Ystdev(i)
            Next

            b /= st2
            a = (sy - sx * b) / ss

            'get fitted y
            ReDim Yfit(ii)
            For i = 0 To ii
                Yfit(i) = a + b * X(i)
            Next

            'get chi square
            ChiSquare = 0.0
            mse = 0.0
            RSquared = 0.0
            For i = 0 To ii
                w = Y(i) - Yfit(i)
                mse += w * w

                w /= Ystdev(i)
                ChiSquare += w * w

                w = Ystdev(i) * Ystdev(i)
                w = Y(i) / w - sy / n
                RSquared += w * w
            Next
            RSquared = 1.0 - ChiSquare * n / RSquared
            mse = mse / n

            'retrun coef
            Coef = New Double() {a, b}

            'return error for ceof
            a = sx * sx / ss / st2
            a = Math.Sqrt((1 + a) / ss)
            b = Math.Sqrt(1.0 / st2)

            If NoStdev Then
                w = Math.Sqrt(ChiSquare / (n - 1))
                a = a * w
                b = b * w
            End If

            CoefStdev = New Double() {a, b}
        End Sub
#End Region

#Region "Polynomial fit"
        Public Shared Sub PolynomialFit(ByVal X() As Double, ByVal Y() As Double, ByVal Order As Integer, ByRef Coef() As Double)
            Dim i, ii As Integer
            Dim j, k As Integer
            Dim C, P, G, Q As Double
            Dim D1, D2 As Double
            Dim B(), T(), S() As Double

            ReDim Coef(Order)
            ReDim B(Order)
            ReDim T(Order)
            ReDim S(Order)

            ii = X.Length - 1

            '------------------------Q1(x) = 1 -----------
            B(0) = 1.0
            D1 = X.Length

            'average Y and X
            C = 0.0
            P = 0.0
            For i = 0 To ii
                C += Y(i)
                P += X(i)
            Next
            C /= D1
            P /= D1

            Coef(0) = C * B(0)

            '------------------------Q2(x) = (x - P)
            T(0) = -P
            T(1) = 1.0

            D2 = 0.0
            C = 0.0
            G = 0.0
            For i = 0 To ii
                Q = X(i) - P
                D2 += Q * Q
                C += Y(i) * Q
                G += X(i) * Q * Q
            Next
            C /= D2
            P = G / D2
            Q = D2 / D1

            D1 = D2
            Coef(0) += C * T(0)
            Coef(1) = C * T(1)

            '------------------------the rest of Q(x)
            For j = 2 To Order
                S(0) = -P * T(0) - Q * B(0)

                S(j - 1) = -P * T(j - 1) + T(j - 2)
                If j > 2 Then
                    For k = j - 1 To 1 Step -1
                        S(k) = -P * T(k) + T(k - 1) - Q * B(k)
                    Next
                End If

                S(j) = T(j - 1)

                D2 = 0.0
                C = 0.0
                P = 0.0
                For i = 0 To ii
                    Q = S(j)
                    For k = j - 1 To 0 Step -1
                        Q = Q * X(i) + S(k)
                    Next

                    D2 += Q * Q
                    C += Y(i) * Q
                    P += X(i) * Q * Q
                Next

                C /= D2
                P /= D2
                Q = D2 / D1
                D1 = D2

                Coef(j) = C * S(j)
                T(j) = S(j)
                For k = j - 1 To 0 Step -1
                    Coef(k) = C * S(k) + Coef(k)
                    B(k) = T(k)
                    T(k) = S(k)
                Next
            Next
        End Sub

        Public Shared Sub PolynomialFit(ByVal X() As Double, ByVal Y() As Double, ByVal Order As Integer, _
                                        ByRef Coef() As Double, ByRef Yfit() As Double, _
                                        ByRef mse As Double, ByRef RSquared As Double)
            Dim i, ii, k As Integer
            Dim yAve As Double

            'do fit
            PolynomialFit(X, Y, Order, Coef)

            'dimension
            ii = Y.Length - 1
            ReDim Yfit(ii)

            'average
            yAve = 0.0
            For i = 0 To ii
                yAve += Y(i)
            Next
            yAve /= Y.Length

            'get mes, and R squared
            mse = 0.0
            RSquared = 0.0
            For i = 0 To ii
                'calculate YFit backwards, this is fater
                Yfit(i) = Coef(Order)
                For k = Order - 1 To 0 Step -1
                    Yfit(i) = Yfit(i) * X(i) + Coef(k)
                Next
                'sum square error
                mse += (Y(i) - Yfit(i)) ^ 2
                'sum variance
                RSquared = (Y(i) - yAve) ^ 2
            Next

            RSquared = 1.0 - mse / RSquared
            mse /= (Y.Length)
        End Sub
#End Region
    End Class

    Public Class CheckSum
        Private CRC8_Code_Table As Byte() = New Byte() _
        {&H0, &H7, &HE, &H9, &H1C, &H1B, &H12, &H15, &H38, &H3F, &H36, &H31, &H24, &H23, &H2A, &H2D, _
         &H70, &H77, &H7E, &H79, &H6C, &H6B, &H62, &H65, &H48, &H4F, &H46, &H41, &H54, &H53, &H5A, &H5D, _
         &HE0, &HE7, &HEE, &HE9, &HFC, &HFB, &HF2, &HF5, &HD8, &HDF, &HD6, &HD1, &HC4, &HC3, &HCA, &HCD, _
         &H90, &H97, &H9E, &H99, &H8C, &H8B, &H82, &H85, &HA8, &HAF, &HA6, &HA1, &HB4, &HB3, &HBA, &HBD, _
         &HC7, &HC0, &HC9, &HCE, &HDB, &HDC, &HD5, &HD2, &HFF, &HF8, &HF1, &HF6, &HE3, &HE4, &HED, &HEA, _
         &HB7, &HB0, &HB9, &HBE, &HAB, &HAC, &HA5, &HA2, &H8F, &H88, &H81, &H86, &H93, &H94, &H9D, &H9A, _
         &H27, &H20, &H29, &H2E, &H3B, &H3C, &H35, &H32, &H1F, &H18, &H11, &H16, &H3, &H4, &HD, &HA, _
         &H57, &H50, &H59, &H5E, &H4B, &H4C, &H45, &H42, &H6F, &H68, &H61, &H66, &H73, &H74, &H7D, &H7A, _
         &H89, &H8E, &H87, &H80, &H95, &H92, &H9B, &H9C, &HB1, &HB6, &HBF, &HB8, &HAD, &HAA, &HA3, &HA4, _
         &HF9, &HFE, &HF7, &HF0, &HE5, &HE2, &HEB, &HEC, &HC1, &HC6, &HCF, &HC8, &HDD, &HDA, &HD3, &HD4, _
         &H69, &H6E, &H67, &H60, &H75, &H72, &H7B, &H7C, &H51, &H56, &H5F, &H58, &H4D, &H4A, &H43, &H44, _
         &H19, &H1E, &H17, &H10, &H5, &H2, &HB, &HC, &H21, &H26, &H2F, &H28, &H3D, &H3A, &H33, &H34, _
         &H4E, &H49, &H40, &H47, &H52, &H55, &H5C, &H5B, &H76, &H71, &H78, &H7F, &H6A, &H6D, &H64, &H63, _
         &H3E, &H39, &H30, &H37, &H22, &H25, &H2C, &H2B, &H6, &H1, &H8, &HF, &H1A, &H1D, &H14, &H13, _
         &HAE, &HA9, &HA0, &HA7, &HB2, &HB5, &HBC, &HBB, &H96, &H91, &H98, &H9F, &H8A, &H8D, &H84, &H83, _
         &HDE, &HD9, &HD0, &HD7, &HC2, &HC5, &HCC, &HCB, &HE6, &HE1, &HE8, &HEF, &HFA, &HFD, &HF4, &HF3}


        Public Shared Function CRC8(ByVal Data As String) As String
            Dim e As New System.Text.ASCIIEncoding
            Dim b() As Byte

            'convert string to byte
            b = e.GetBytes(Data)

            'get CRC byte
            b(0) = CRC8(b)

            'get char
            Return BitConverter.ToChar(b, 0)
        End Function

        Public Shared Function CRC8(ByVal Data() As Byte) As Byte
            Dim i, ii, k As Integer
            Dim crc, temp As Integer

            ii = Data.Length - 1

            crc = &H0
            For i = 0 To ii
                temp = Data(i)
                For k = 0 To 7
                    If ((crc Xor temp) And &H1) = &H1 Then
                        crc = crc Xor &H18
                        crc >>= 1
                        crc = crc Or &H80
                    Else
                        crc >>= 1
                    End If
                    temp >>= 1
                Next
            Next

            crc = crc And &HFF    'take the LSB
            Return Convert.ToByte(crc)
        End Function

        Public Shared Function CRC16(ByVal Data() As Byte) As UShort
            Dim i, ii, k As Integer
            Dim crc, temp As Integer

            ii = Data.Length - 1

            crc = &HFFFF
            For i = 0 To ii
                crc = crc Xor Data(i)
                For k = 0 To 7
                    temp = crc >> 1
                    If (crc And &H1) = &H1 Then
                        crc = temp Xor &HA001
                    Else
                        crc = temp
                    End If
                Next
            Next

            crc = crc And &HFFFF    'take two low byte

            Return Convert.ToUInt16(crc)
        End Function

        Public Function CRC8Ex(ByVal Data() As Byte, ByVal Length As Integer) As Byte
            Dim crc As Byte
            Dim i, ii As Integer

            ii = Length - 1
            For i = 0 To ii
                crc = (crc Xor Data(i))
                crc = CRC8_Code_Table(crc)
            Next i

            Return crc
        End Function
    End Class

    Public Class RotatingBuffer
        Private mData() As Double
        Private mLength As Integer
        Private mIndex As Integer

        Public Sub New(ByVal Length As Integer)
            mLength = Length
            Me.Clear()
        End Sub

        Public Sub Add(ByVal Value As Double)
            'full?
            If mIndex = mLength Then mIndex = 0
            'add data
            mData(mIndex) = Value
            'next
            mIndex += 1
        End Sub

        Public Sub Clear()
            ReDim mData(mLength - 1)
            mIndex = 0
        End Sub

        Public ReadOnly Property Length() As Integer
            Get
                Return mLength
            End Get
        End Property

        Public ReadOnly Property Data() As Double()
            Get
                Return mData
            End Get
        End Property

        Public ReadOnly Property Data(ByVal Index As Integer) As Double
            Get
                Return mData(Index)
            End Get
        End Property

        Public ReadOnly Property Average() As Double
            Get
                Return w2Array.Mean(mData)
            End Get
        End Property

        Public ReadOnly Property Range() As Double
            Get
                Return w2Array.Range(mData)
            End Get
        End Property

        Public ReadOnly Property Min() As Double
            Get
                Return w2Array.Minimum(mData)
            End Get
        End Property

        Public ReadOnly Property Max() As Double
            Get
                Return w2Array.Maximum(mData)
            End Get
        End Property

        Public ReadOnly Property StandardDeviation() As Double
            Get
                Return w2Array.StandardDeviation(mData)
            End Get
        End Property
    End Class

End Namespace