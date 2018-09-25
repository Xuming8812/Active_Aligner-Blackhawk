Option Explicit On
Option Strict On

Namespace Instrument
    Public MustInherit Class iMotionController
        Public Enum MoveToTargetMethodEnum
            Absolute
            Relative
        End Enum

        Public Structure StageInformation
            Public ModelNumber As String
            Public SerialNumber As String
            Public Partnumber As String

            Public Overrides Function ToString() As String
                Return (ModelNumber + " " + Partnumber + " " + SerialNumber)
            End Function
        End Structure

        Protected mAxisCount As Integer
        Protected mAxis As Integer

        Public Sub New()
            Me.New(1)
        End Sub

        Public Sub New(ByVal AxisCount As Integer)
            mAxisCount = AxisCount
            mAxis = 1
        End Sub

        'Public Property Axis() As Integer
        '    Get
        '        Return mAxis
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < 1 Then Return
        '        If value > mAxisCount Then Return
        '        mAxis = value
        '    End Set
        'End Property

        Public MustOverride Property Axis() As Integer
        Public MustOverride ReadOnly Property AxisName() As String

        Public MustOverride Function Initialize(ByVal sConnection As String, ByVal RaiseError As Boolean) As Boolean
        Public MustOverride Sub Close()
        Public MustOverride ReadOnly Property ControllerVersion() As String

        Public MustOverride ReadOnly Property StageReady() As Boolean

        'Public MustOverride ReadOnly Property LastErrorCode() As Integer
        Public MustOverride ReadOnly Property LastError() As String

        'Public MustOverride ReadOnly Property PivotPoint() As String


#Region "configuration"
        Public MustOverride Property DriveEnabled() As Boolean
        Public MustOverride ReadOnly Property CurrentPosition() As Double

        Public MustOverride Property Velocity() As Double
        Public MustOverride Property VelocityMaximum() As Double
         
        Public MustOverride Property Acceleration() As Double
        Public MustOverride Property Deceleration() As Double
        Public MustOverride Property AccelerationMaximum() As Double

        Public MustOverride ReadOnly Property StageData() As StageInformation
#End Region

#Region "motion"
        Public MustOverride Function InitializeMotion() As Boolean
        Protected MustOverride Function StartHome() As Boolean
        Protected MustOverride Function StartMove(ByVal Method As MoveToTargetMethodEnum, ByVal Target As Double) As Boolean

        Public MustOverride Function KillMotionAllAxis() As Boolean
        Public MustOverride Function KillMotion() As Boolean
        Public MustOverride Function HaltMotion() As Boolean

        Public MustOverride ReadOnly Property StageMoving() As Boolean

        Public Function HomeNoWait() As Boolean
            If Me.StageMoving() Then Return False
            Return Me.StartHome()
        End Function

        Public Function MoveNoWait(ByVal Method As MoveToTargetMethodEnum, ByVal Target As Double) As Boolean
            If Me.StageMoving() Then Return False
            Return Me.StartMove(Method, Target)
        End Function

        Public Function HomeAndWait() As Boolean
            If Not Me.HomeNoWait() Then Return False
            System.Threading.Thread.Sleep(300)
            While Me.StageMoving
                System.Threading.Thread.Sleep(300)
            End While
            Return True
        End Function

        Public Function MoveAndWait(ByVal Method As MoveToTargetMethodEnum, ByVal Target As Double) As Boolean
            If Not Me.MoveNoWait(Method, Target) Then Return False
            System.Threading.Thread.Sleep(300)
            While Me.StageMoving
                System.Threading.Thread.Sleep(300)
            End While
            Return True
        End Function
#End Region

#Region "GPIO"
        Public MustOverride Function ReadAnalogyInput(ByVal Channel As Integer) As Double
        Public MustOverride Function SetAnalogyOutput(ByVal Channel As Integer, ByVal Value As Double) As Boolean

        Public MustOverride ReadOnly Property DigitInput(ByVal Port As Integer) As Integer
        Public MustOverride Property DigitOutput(ByVal Port As Integer) As Integer

        Public ReadOnly Property DigitInput(ByVal Port As Integer, ByVal Bit As Integer) As Boolean
            Get
                Dim Value As Integer
                Dim Mask As Integer

                Value = Me.DigitInput(Port)
                Mask = (1 << Bit)
                Return (Value And Mask) = Mask
            End Get
        End Property

        Public Property DigitOutput(ByVal Port As Integer, ByVal Bit As Integer) As Boolean
            Get
                Dim Value As Integer
                Dim Mask As Integer

                Value = Me.DigitOutput(Port)
                Mask = (1 << Bit)
                Return (Value And Mask) = Mask
            End Get
            Set(ByVal value As Boolean)
                Dim Data As Integer
                Dim Mask As Integer
                'get current value
                Data = Me.DigitOutput(Port)
                'set this bit
                Mask = (1 << Bit)
                'get the new value
                If value Then
                    Data = (Data Or Mask)           'the mask bit is set to 1 by OR
                Else
                    Data = (Data And (Not Mask))    'not Mask is 1110111, the AND put the 0 to 0
                End If
                'send
                Me.DigitOutput(Port) = Data
            End Set
        End Property
#End Region
    End Class
End Namespace

