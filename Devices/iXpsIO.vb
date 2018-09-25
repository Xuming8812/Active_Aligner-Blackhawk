Option Explicit On
Option Strict On
Option Infer Off

Public Class iXpsIO
    Public Enum PostLightStatusEnum
        Running
        Fail
        Standby
    End Enum

    Public Structure IoLineConfig
        Public Valid As Boolean
        Public Port As Integer
        Public Line As Integer

        Public Shared Function Parse(ByVal s As String) As IoLineConfig
            Dim x As IoLineConfig
            Dim data() As String
            data = s.Trim().Split(","c)
            Try
                'NOTE: XPS line is 1 based, internal bit is 0 based, we will subtract 1 here to match the line with bit
                x.Port = Convert.ToInt32(data(0))
                x.Line = Convert.ToInt32(data(1)) - 1
                x.Valid = True
            Catch ex As Exception
                x.Valid = False
            End Try
            Return x
        End Function
    End Structure

    Private Enum XpsIoChannelIndex
        'do not change the name including case here. These names are uased as Key in the configuration file
        DoorInterlock
        EmergencyStop

        LampRunning
        LampFail
        LampStandby

        'these 4 are analogy reading of the vacuum or pressure
        VacuumReadingInput
        VacuumReadingOutputLens
        VacuumReadingOutputBs
        CdaReadingInput

        'vacuum IO
        VacuumMain
        VacuumMainReadback

        VacuumHexapod
        VacuumHexapodReadback

        VacuumPackage
        VacuumPackageReadback

        VacuumOrCda
       

        'epoxy 
        CdaEpoxy
        EpoxyValve

        'probe
        CdaProbe

        'spare
        'SpareOutput1   
        SpareOutput2
        SpareOutput3
        SpareOutput4
        SpareOutput5
        SpareOutput6

        'SpareInput1
        'SpareInput2
        'SpareInput3
        SpareInput4
        SpareInput5
        SpareInput6
        SpareInput7
        SpareInput8
    End Enum

    Private mPara() As IoLineConfig
    Private mXPS As Instrument.iXPS

    Public ReadOnly Property HaveController As Boolean
        Get
            Return (mXPS IsNot Nothing)
        End Get
    End Property

    Public Function Initialize(ByRef hXPS As Instrument.iXPS, ByRef hConfig As w2.w2IniFileXML) As Boolean
        Dim i, ii As Integer
        Dim s, key As String
        Dim xType As Type

        Const section As String = "XpsIoTable"

        mXPS = hXPS

        'read the parameter
        xType = GetType(XpsIoChannelIndex)
        ii = [Enum].GetValues(xType).Length - 1
        ReDim mPara(ii)
        For i = 0 To ii
            key = [Enum].GetName(xType, i)
            s = hConfig.ReadParameter(section, key, "")
            mPara(i) = IoLineConfig.Parse(s)
            If Not mPara(i).Valid Then
                s = "Cannot parse the XPS IO configuration for " + key
                MessageBox.Show(s, "IO Config", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Next

        ' set GPIO 3 to defautl state "TRUE"
        If mXPS IsNot Nothing Then
            mXPS.DigitOutput(3, 0) = False
            mXPS.DigitOutput(3, 1) = True
            mXPS.DigitOutput(3, 2) = True
            mXPS.DigitOutput(3, 3) = True
            mXPS.DigitOutput(3, 4) = True
            mXPS.DigitOutput(3, 5) = True
        End If

        Return True
    End Function

    Public ReadOnly Property DoorClosed() As Boolean
        Get
            Return mXPS.DigitInput(mPara(XpsIoChannelIndex.DoorInterlock).Port, mPara(XpsIoChannelIndex.DoorInterlock).Line)
        End Get
    End Property

    Public ReadOnly Property EmergencyStop() As Boolean
        Get
            Return mXPS.DigitInput(mPara(XpsIoChannelIndex.EmergencyStop).Port, mPara(XpsIoChannelIndex.EmergencyStop).Line)
        End Get
    End Property

    Public Sub SetPostLight(ByVal value As PostLightStatusEnum)
        If mXPS Is Nothing Then Return
        mXPS.DigitOutput(mPara(XpsIoChannelIndex.LampFail).Port, mPara(XpsIoChannelIndex.LampFail).Line) = (value = PostLightStatusEnum.Fail)
        mXPS.DigitOutput(mPara(XpsIoChannelIndex.LampRunning).Port, mPara(XpsIoChannelIndex.LampRunning).Line) = (value = PostLightStatusEnum.Running)
        mXPS.DigitOutput(mPara(XpsIoChannelIndex.LampStandby).Port, mPara(XpsIoChannelIndex.LampStandby).Line) = (value = PostLightStatusEnum.Standby)
    End Sub

#Region "vacuum IO"
    Public Enum VacuumLine
        Main = iXpsStage.StageEnum.Main
        Hexapod = iXpsStage.StageEnum.Hexapod
        Package = 2
        LineInput = 3
    End Enum

    Private Function GetVacuumLineXpsIoChannelIndex(ByVal eLine As VacuumLine) As Integer
        Dim index As XpsIoChannelIndex
        Select Case eLine
            Case VacuumLine.Hexapod
                index = XpsIoChannelIndex.VacuumHexapod
            Case VacuumLine.Main
                index = XpsIoChannelIndex.VacuumMain
            Case VacuumLine.Package
                index = XpsIoChannelIndex.VacuumPackage
            Case Else
                Return -1
        End Select

        Return index
    End Function

    Public Property VacuumEnabled(ByVal eLine As VacuumLine) As Boolean
        Get
            Dim index As Integer
            index = Me.GetVacuumLineXpsIoChannelIndex(eLine)
            If index < 0 Then Return False
            Return Not mXPS.DigitOutput(mPara(index).Port, mPara(index).Line)
        End Get
        Set(value As Boolean)
            Dim index As Integer
            index = Me.GetVacuumLineXpsIoChannelIndex(eLine)
            If index < 0 Then Return
            mXPS.DigitOutput(mPara(index).Port, mPara(index).Line) = Not value
        End Set
    End Property

    Public ReadOnly Property VacuumEnabledReadback(ByVal eLine As VacuumLine) As Boolean
        Get
            Dim index As XpsIoChannelIndex
            Select Case eLine
                Case VacuumLine.Hexapod
                    index = XpsIoChannelIndex.VacuumHexapodReadback
                Case VacuumLine.Main
                    index = XpsIoChannelIndex.VacuumMainReadback
                Case VacuumLine.Package
                    'index = XpsIoChannelIndex.VacuumPackageReadback
                    'we do not have feedback line for the package, just use the set value
                    Return Me.VacuumEnabled(VacuumLine.Package)
                Case Else
                    Return False
            End Select
            Return Not mXPS.DigitOutput(mPara(index).Port, mPara(index).Line)
        End Get
    End Property

    Public Property VacuumLinePressurized() As Boolean
        Get
            Return mXPS.DigitOutput(mPara(XpsIoChannelIndex.VacuumOrCda).Port, mPara(XpsIoChannelIndex.VacuumOrCda).Line)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara(XpsIoChannelIndex.VacuumOrCda).Port, mPara(XpsIoChannelIndex.VacuumOrCda).Line) = value
        End Set
    End Property
#End Region

#Region "Probe"
    Public Property ProbePositionOn() As Boolean
        Get
            Return mXPS.DigitOutput(mPara(XpsIoChannelIndex.CdaProbe).Port, mPara(XpsIoChannelIndex.CdaProbe).Line)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara(XpsIoChannelIndex.CdaProbe).Port, mPara(XpsIoChannelIndex.CdaProbe).Line) = value
        End Set
    End Property
#End Region

#Region "Epoxy/injector"
    Public Property EpoxyMoveOut() As Boolean
        Get
            Return mXPS.DigitOutput(mPara(XpsIoChannelIndex.CdaEpoxy).Port, mPara(XpsIoChannelIndex.CdaEpoxy).Line)
        End Get
        Set(ByVal value As Boolean)
            mXPS.DigitOutput(mPara(XpsIoChannelIndex.CdaEpoxy).Port, mPara(XpsIoChannelIndex.CdaEpoxy).Line) = value
        End Set
    End Property

    Public Sub EpoxyTriggerOnce()
        'open contact to arm the trigger
        mXPS.DigitOutput(mPara(XpsIoChannelIndex.EpoxyValve).Port, mPara(XpsIoChannelIndex.EpoxyValve).Line) = True
        System.Threading.Thread.Sleep(100)
        'trun it on
        mXPS.DigitOutput(mPara(XpsIoChannelIndex.EpoxyValve).Port, mPara(XpsIoChannelIndex.EpoxyValve).Line) = False
    End Sub

#End Region

#Region "Vacuum/Pressure reading"
    Public ReadOnly Property VacuumLevel(ByVal eLine As VacuumLine) As Double
        Get
            Dim index As XpsIoChannelIndex

            Select Case eLine
                Case VacuumLine.Hexapod
                    index = XpsIoChannelIndex.VacuumReadingOutputBs
                Case VacuumLine.Main
                    index = XpsIoChannelIndex.VacuumReadingOutputLens
                Case VacuumLine.LineInput
                    index = XpsIoChannelIndex.VacuumReadingInput
                Case VacuumLine.Package
                    Return Double.NaN
                Case Else
                    Return Double.NaN
            End Select

            Return Me.GetVacuumPressure(mPara(index).Line)
        End Get
    End Property

    Public ReadOnly Property CompressAirPressure() As Double
        Get
            Return Me.GetVacuumPressure(mPara(XpsIoChannelIndex.CdaReadingInput).Line)
        End Get
    End Property

    Private Function GetVacuumPressure(ByVal Channel As Integer) As Double
        Dim V As Double

        Const V1 As Double = 2.8371
        Const V2 As Double = 3.943
        Const P1 As Double = -48.8
        Const P2 As Double = -77.4

        'convert analogy voltage to pressure in kPa , channel + 1 to accomodate the bit mask
        V = mXPS.ReadAnalogyInput(Channel + 1)

        V -= V1
        V /= (V2 - V1)
        V *= (P2 - P1)
        V += P1

        If Channel = mPara(XpsIoChannelIndex.CdaReadingInput).Line Then
            V *= -0.01
        End If

        Return V
    End Function
#End Region

End Class
