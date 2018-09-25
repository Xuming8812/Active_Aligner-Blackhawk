Option Explicit On
Option Strict On

Imports PI
Namespace Instrument
    'there are two classes here, one for PI C887 (GCS2) and one for PI C843 (GCS843) controller
    Public Class iPiGCS2
        Inherits iMotionController

#Region "additional DLL"
        Private Declare Function PI_qSPI Lib "PI_GCS2_DLL.dll" (ByVal iID As Integer, ByVal Axis As String, ByRef Values() As Double) As Integer
        Private Declare Function PI_SPI Lib "PI_GCS2_DLL.dll" (ByVal iID As Integer, ByVal Axis As String, ByRef Values() As Double) As Integer


#End Region

        Public Enum AxisEnum
            All = -1
            X
            Y
            Z
            U
            V
            W
        End Enum

        Private mID As Integer
        Private mError As Integer
        Private mAxisString As String

        Public Sub New(ByVal AxisCount As Integer)
            MyBase.New(AxisCount)
        End Sub

        Public Overrides Function Initialize(ByVal sConnection As String, ByVal RaiseError As Boolean) As Boolean
            Dim s As String
            Dim iPort As Integer

            Select Case True
                Case sConnection.StartsWith("COM")
                    s = sConnection.Substring(3)
                    iPort = Convert.ToInt32(s)
                    mID = GCS2.ConnectRS232(iPort, 115200)

                Case sConnection.Contains(".")
                    mID = GCS2.ConnectTCPIP(sConnection, 50000)

                Case Else
                    MessageBox.Show("Unrecognized connection string: " + sConnection, "PI CGS")
                    Return False
            End Select

            If (mID = -1) And RaiseError Then
                MessageBox.Show("Error connecting to PI controller at " + sConnection, "PI CGS")
                Return False
            Else
                'we are fine
            End If

            'following were used for PIVOT debug, 
            'Me.StartHome()
            'Dim v1, v2, v3 As Double
            ''Me.SetPivot(0, 0, 0)
            'Me.ReadPivot(v1, v2, v3)

            Dim r0, s0, t0 As Double
            Me.ReadPivot(r0, s0, t0)
            Me.SetPivot(1.8, 1.5, 1.7)

            Return(GCS2.IsConnected(mID) = 1)
        End Function

        Public Overrides Sub Close()
            GCS2.CloseConnection(mID)
        End Sub

        Public Overrides ReadOnly Property StageReady() As Boolean
            Get
                Dim data(0) As Integer
                mError = GCS2.IsControllerReady(mID, data(0))
                If (mError = 1) And (data(0) = 1) Then
                    mError = GCS843.qFRF(mID, mAxisString, data)
                    'Return (mError = 1) And (data(0) = 1)
                    Return data(0) = 1
                Else
                    Return False
                End If
            End Get
        End Property

        Public Function ReadPivot(ByRef R As Double, ByRef S As Double, ByRef T As Double) As Boolean
            Dim reply As System.Text.StringBuilder = New System.Text.StringBuilder()
            Dim i As Integer
            
            mError = GCS2.GcsCommandset(mID, "SPI?")
            System.Threading.Thread.Sleep(100)
            mError = GCS2.GcsGetAnswerSize(mID, i)
            For k As Integer = 0 To 2
                mError = GCS2.GcsGetAnswer(mID, reply, i)
                Dim s1 = reply.ToString
                If s1.Contains("R") Then
                    s1 = s1.Replace("R=", "")
                    R = Val(s1)
                End If

                If s1.Contains("S") Then
                    s1 = s1.Replace("S=", "")
                    S = Val(s1)
                End If

                If s1.Contains("T") Then
                    s1 = s1.Replace("T=", "")
                    T = Val(s1)
                End If

            Next

            Return (mError = 1)
        End Function

        Public Function SetPivot(ByVal R As Double, ByVal S As Double, ByVal T As Double) As Boolean
            'mError = PI_SPI(mID, "R", New Double() {R})
            'If (mError = 1) Then mError = PI_SPI(mID, "S", New Double() {S})
            'If (mError = 1) Then mError = PI_SPI(mID, "T", New Double() {T})
            'Return (mError = 1)
            Dim head, command As String
            head = "SPI "
            command = head & "R " & R
            mError = GCS2.GcsCommandset(mID, command)

            command = head & "S " & S
            mError = GCS2.GcsCommandset(mID, command)

            command = head & "T " & T
            mError = GCS2.GcsCommandset(mID, command)

            Return (mError = 1)
        End Function

        Public Overrides Property Axis() As Integer
            Get
                Return mAxis
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then Return
                If value > mAxisCount Then Return
                mAxis = value
                Select Case mAxis
                    Case 0 : mAxisString = ""  ' for all connected axis
                    Case 1 : mAxisString = "X"
                    Case 2 : mAxisString = "Y"
                    Case 3 : mAxisString = "Z"
                    Case 4 : mAxisString = "U"
                    Case 5 : mAxisString = "V"
                    Case 6 : mAxisString = "W"
                End Select
            End Set
        End Property

        Public Overrides ReadOnly Property AxisName() As String
            Get
                Return mAxisString
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentPosition As Double
            Get
                Dim v As Double() = New Double(0) {}
                mError = GCS2.qPOS(mID, mAxisString, v)
                If mError = 1 Then
                    Return v(0)
                Else
                    Return Double.NaN
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StageMoving As Boolean
            Get
                Dim value As Integer() = New Integer(0) {}
                mError = GCS2.IsMoving(mID, mAxisString, value)
                If (mError = 1) And (value(0) = 1) Then Return True

                'homing will cause not ready 
                If Not Me.StageReady() Then Return True

                Return False
            End Get
        End Property

        Public Overrides Function InitializeMotion() As Boolean
            mError = GCS2.INI(mID, mAxisString)
            Return (mError = 1)
        End Function

        Protected Overrides Function StartHome() As Boolean
            mError = GCS2.FRF(mID, mAxisString)
            Return (mError = 1)
        End Function

        Protected Overrides Function StartMove(ByVal Method As iMotionController.MoveToTargetMethodEnum, ByVal Target As Double) As Boolean
            'check ready
            If Me.StageMoving Then Return False

            'move
            Select Case Method
                Case MoveToTargetMethodEnum.Absolute
                    mError = GCS2.MOV(mID, mAxisString, New Double() {Target})
                Case MoveToTargetMethodEnum.Relative
                    mError = GCS2.MVR(mID, mAxisString, New Double() {Target})
            End Select

            'return
            Return (mError = 1)
        End Function

        Public Overrides Property DriveEnabled As Boolean
            Get
                Dim data() As Integer = New Integer() {}
                mError = GCS2.qSVO(mID, mAxisString, data)
                Return (mError = 1) And (data(0) = 1)
            End Get
            Set(ByVal value As Boolean)
                Dim data(0) As Integer
                data(0) = 1
                GCS2.SVO(mID, mAxisString, data)
            End Set
        End Property

        Public Overrides Function HaltMotion() As Boolean
            mError = GCS2.HLT(mID, mAxisString)
            Return mError = 1
        End Function

        Public Overrides Function KillMotion() As Boolean
            Return Me.KillMotionAllAxis()
        End Function

        Public Overrides Function KillMotionAllAxis() As Boolean
            mError = GCS2.STP(mID)
            Return mError = 1
        End Function

#Region "system error and info"
        Public Overrides ReadOnly Property ControllerVersion() As String
            Get
                Dim s As New System.Text.StringBuilder(256)

                mError = GCS2.qIDN(mID, s, s.Capacity)
                If mError = 1 Then
                    Return s.ToString()
                Else
                    mError = GCS2.GetError(mID)
                    Return Me.LastError
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StageData() As iMotionController.StageInformation
            Get
                Dim x As StageInformation
                Dim s As New System.Text.StringBuilder(400)
                mError = GCS2.qCST(mID, mAxisString, s, s.Length)
                If mError = 1 Then
                    x.ModelNumber = s.ToString()
                    x.Partnumber = ""
                    x.SerialNumber = ""
                Else
                    x = Nothing
                End If
                Return x
            End Get
        End Property

        Public ReadOnly Property LastErrorCode() As Integer
            Get
                Return mError
            End Get
        End Property

        Public Overrides ReadOnly Property LastError() As String
            Get
                Dim s As New System.Text.StringBuilder(1024)
                GCS2.TranslateError(mError, s, s.Capacity)
                Return s.ToString()
            End Get
        End Property
#End Region

#Region "Aux - GPIO and ADC/DAC"
        Public Overloads Overrides ReadOnly Property DigitInput(ByVal Port As Integer) As Integer
            Get

            End Get
        End Property

        Public Overloads Overrides Property DigitOutput(ByVal Port As Integer) As Integer
            Get

            End Get
            Set(ByVal value As Integer)

            End Set
        End Property

        Public Overrides Function ReadAnalogyInput(ByVal Channel As Integer) As Double
            Dim CH As Integer() = New Integer() {Channel}
            Dim data(0) As Integer
            mError = GCS2.qTAD(mID, CH, data, 1)
            Return data(0)
        End Function

        Public Overrides Function SetAnalogyOutput(ByVal Channel As Integer, ByVal Value As Double) As Boolean

        End Function
#End Region

#Region "velocity acceleration"
        Public Overrides Property Acceleration As Double
            Get

            End Get
            Set(ByVal value As Double)

            End Set
        End Property

        Public Overrides Property AccelerationMaximum As Double
            Get

            End Get
            Set(ByVal value As Double)

            End Set
        End Property

        Public Overrides Property Deceleration As Double
            Get

            End Get
            Set(ByVal value As Double)

            End Set
        End Property

        Public Overrides Property Velocity As Double
            Get

            End Get
            Set(ByVal value As Double)

            End Set
        End Property

        Public Overrides Property VelocityMaximum As Double
            Get

            End Get
            Set(ByVal value As Double)

            End Set
        End Property
#End Region
    End Class

    Public Class iPiGCS843
        Inherits iMotionController

#Region "additional DLL"
        Private Declare Function C843_OpenUserStagesEditDialog Lib "C843_GCS_DLL.dll" (ByVal iID As Integer) As Integer
        'Private Declare Function PI_qSPI Lib "PI_GCS2_DLL.dll" (ByVal iID As Integer, ByVal Axis As String, ByRef Values() As Double) As Integer
        'Private Declare Function PI_SPI Lib "PI_GCS2_DLL.dll" (ByVal iID As Integer, ByVal Axis As String, ByRef Values() As Double) As Integer

#End Region


        Private mID As Integer
        Private mError As Integer
        Private mAxisString As String

        Public Sub New(ByVal AxisCount As Integer)
            MyBase.New(AxisCount)
        End Sub

        Public Overrides Function Initialize(sConnection As String, RaiseError As Boolean) As Boolean
            Dim iBoard As Integer

            If Integer.TryParse(sConnection, iBoard) Then
                mID = GCS843.Connect(iBoard)
            Else
                MessageBox.Show("Invalid board number " + sConnection, "PI CGS for C843 Controller")
                Return False
            End If

            If (mID = -1) And RaiseError Then
                MessageBox.Show("Error connecting to PI controller at " + sConnection, "PI CGS")
                Return False
            Else
                mError = GCS843.CST(mID, "1", "M-116.DGH")
                If mError <> 1 Then Return False

                Dim names As System.Text.StringBuilder = New System.Text.StringBuilder()
                mError = GCS843.qCST(mID, mAxisString, names, 1024)
                If mError <> 1 Then Return False

                mError = GCS843.INI(mID, "")
                If mError <> 1 Then Return False

            End If

            Return (GCS843.IsConnected(mID) = 1)
        End Function

        Public Overrides Sub Close()
            GCS843.CloseConnection(mID)
        End Sub

        Public Sub OpenEditDialog()
            C843_OpenUserStagesEditDialog(mID)
        End Sub

        Public Overrides ReadOnly Property StageReady() As Boolean
            Get
                Dim data(0) As Integer
                mError = GCS843.qREF(mID, mAxisString, data)
                Return (mError = 1) And (data(0) = 1)
            End Get
        End Property

        Public Overrides Property Axis() As Integer
            Get
                Return mAxis
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then Return
                If value > mAxisCount Then Return
                mAxis = value
                mAxisString = mAxis.ToString()
            End Set
        End Property

        Public Overrides ReadOnly Property AxisName As String
            Get
                Return mAxisString
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentPosition As Double
            Get
                Dim v(0) As Double
                mError = GCS843.qPOS(mID, mAxisString, v)
                If mError = 1 Then
                    Return v(0)
                Else
                    Return Double.NaN
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StageMoving As Boolean
            Get
                Dim value(0) As Integer
                mError = GCS843.IsMoving(mID, mAxisString, value)

                If (mError = 1) And (value(0) = 1) Then
                    Return True
                Else
                    mError = GCS843.IsReferencing(mID, mAxisString, value)
                    Return (mError = 1) And (value(0) = 1)
                End If
            End Get
        End Property

        Public Overrides Function InitializeMotion() As Boolean
            mError = GCS843.INI(mID, mAxisString)
            Return (mError = 1)
        End Function

        Protected Overrides Function StartHome() As Boolean
            mError = GCS843.REF(mID, mAxisString)
            Return mError = 1
        End Function

        Protected Overrides Function StartMove(Method As iMotionController.MoveToTargetMethodEnum, Target As Double) As Boolean
            'check ready
            If Me.StageMoving Then Return False

            Dim value(0) As Double
            value(0) = Target

            'move
            Select Case Method
                Case MoveToTargetMethodEnum.Absolute
                    mError = GCS843.MOV(mID, mAxisString, value)
                Case MoveToTargetMethodEnum.Relative
                    mError = GCS843.MVR(mID, mAxisString, value)
            End Select

            'return
            Return (mError = 1)
        End Function

        Public Overrides Property DriveEnabled As Boolean
            Get
                Dim data(0) As Integer
                mError = GCS843.qSVO(mID, mAxisString, data)
                Return (mError = 1) And (data(0) = 1)
            End Get
            Set(value As Boolean)
                Dim data(0) As Integer
                data(0) = 1
                GCS843.SVO(mID, mAxisString, data)
            End Set
        End Property

        Public Overrides Function HaltMotion() As Boolean
            mError = GCS843.HLT(mID, mAxisString)
            Return mError = 1
        End Function

        Public Overrides Function KillMotion() As Boolean
            Return Me.KillMotionAllAxis()
        End Function

        Public Overrides Function KillMotionAllAxis() As Boolean
            mError = GCS843.STP(mID)
            Return mError = 1
        End Function

#Region "system error and info"
        Public Overrides ReadOnly Property ControllerVersion() As String
            Get
                Dim s As New System.Text.StringBuilder(256)

                mError = GCS843.qIDN(mID, s, s.Capacity)
                If mError = 1 Then
                    Return s.ToString()
                Else
                    mError = GCS843.GetError(mID)
                    Return Me.LastError
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property StageData() As iMotionController.StageInformation
            Get
                Dim x As StageInformation
                Dim s As New System.Text.StringBuilder(400)
                mError = GCS843.qCST(mID, mAxisString, s, s.Length)
                If mError = 1 Then
                    x.ModelNumber = s.ToString()
                    x.Partnumber = ""
                    x.SerialNumber = ""
                Else
                    x = Nothing
                End If
                Return x
            End Get
        End Property

        Public ReadOnly Property LastErrorCode() As Integer
            Get
                Return mError
            End Get
        End Property

        Public Overrides ReadOnly Property LastError() As String
            Get
                Dim s As New System.Text.StringBuilder(1024)
                GCS843.TranslateError(mError, s, s.Capacity)
                Return s.ToString()
            End Get
        End Property
#End Region

#Region "Aux - GPIO and ADC/DAC"
        Public Overloads Overrides ReadOnly Property DigitInput(Port As Integer) As Integer
            Get

            End Get
        End Property

        Public Overloads Overrides Property DigitOutput(Port As Integer) As Integer
            Get

            End Get
            Set(value As Integer)

            End Set
        End Property

        Public Overrides Function ReadAnalogyInput(Channel As Integer) As Double
            Return Double.NaN
        End Function

        Public Overrides Function SetAnalogyOutput(Channel As Integer, Value As Double) As Boolean

        End Function
#End Region

#Region "velocity acceleration"
        Public Overrides Property Acceleration As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property AccelerationMaximum As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property Deceleration As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property Velocity As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property

        Public Overrides Property VelocityMaximum As Double
            Get

            End Get
            Set(value As Double)

            End Set
        End Property
#End Region
    End Class
End Namespace
