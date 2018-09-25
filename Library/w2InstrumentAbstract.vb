Option Explicit On
Option Strict On

Namespace Instrument

#Region "enum"
    'common enumation
    Public Enum PowerUnitEnum
        Unknown = -1
        W = 0
        uW = 1
        mW = 2
        dBm = 3
        dB = 4
        Relative = 5
    End Enum

    Public Enum MediumEnum
        Air = 1
        Vacuum = 2
    End Enum

    Public Enum TriggleModeEnum
        [Single]
        Continuous
    End Enum

    Public Enum TemperatureUnitEnum
        None = -1

        DegC = 0
        DegF = 1
        Kelvin = 2
    End Enum
#End Region

    ''' <summary>
    ''' All physics are in MKS unit
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Physics
        Public Shared Cvacuum As Double = 299792458.0#
        Public Shared RefractiveIndexAir As Double = 1.00026407#

        Shared Function GHzTonm(ByVal Frequency_GHz As Double) As Double
            If Frequency_GHz > 0 Then
                Return (Cvacuum / RefractiveIndexAir / Frequency_GHz)
            Else
                Return 0.0
            End If
        End Function

        Shared Function nmToGHz(ByVal Wavelength_nm As Double) As Double
            If Wavelength_nm > 0 Then
                Return (Cvacuum / RefractiveIndexAir / Wavelength_nm)
            Else
                Return 0.0
            End If
        End Function

        Shared Function mWTodBm(ByVal Power_mW As Double) As Double
            Return 10.0 * Math.Log10(Power_mW)
        End Function

        Shared Function dBmTomW(ByVal Power_dBm As Double) As Double
            Return 10.0 ^ (0.1 * Power_dBm)
        End Function

        Shared Function Thermistor_TemperatureToResistance(ByVal Temperature_DegC As Double) As Double
            Dim v, alpha, beta As Double

            Const A As Double = 0.001129148
            Const B As Double = 0.000234125
            Const C As Double = 0.0000000876741

            v = 273.15 + Temperature_DegC
            alpha = 0.5 * (A - 1.0 / v) / C

            beta = (B / 3.0 / C) ^ 3
            beta += alpha ^ 2
            beta = Math.Sqrt(beta)

            v = (beta - alpha) ^ (1 / 3)
            v -= (beta + alpha) ^ (1 / 3)

            Return Math.Exp(v)
        End Function


    End Class

    ''' <summary>
    ''' Abstract instrument class for all instrument hardwares
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class iInstrumentGeneric

        ''' <summary>
        ''' This is the class entry point beside the constructer to initialize the hardware
        ''' </summary>
        ''' <param name="InstrumentAddress">GPIB board address, or COM port, USB ID</param>
        ''' <param name="AdditionalInfo">GPIB instrument address, COM baud rate, and etc</param>
        ''' <param name="RaiseError">if error message shall be shown when error encountered</param>
        ''' <returns>True if successful</returns>
        ''' <remarks></remarks>
        Public MustOverride Function Initialize(ByVal InstrumentAddress As Integer, _
                                                ByVal AdditionalInfo As Integer, _
                                                ByVal RaiseError As Boolean) As Boolean

        'overloaded initialization short cuts
        Public Overridable Function Initialize(ByVal AdrsInstrument As Integer) As Boolean
            Return Me.Initialize(0, AdrsInstrument, False)
        End Function

        Public Overridable Function Initialize(ByVal AdrsInstrument As Integer, ByVal RaiseError As Boolean) As Boolean
            Return Me.Initialize(0, AdrsInstrument, RaiseError)
        End Function

        Public MustOverride ReadOnly Property Name() As String

        Public MustOverride Property Connected() As Boolean

        'duplicated method for Connected Set property, 
        'this is mainly to emulate the .NET serial port method
        Public Sub Open()
            Me.Connected = True
        End Sub
        Public Sub Close()
            Me.Connected = False
        End Sub

        'these are only used for inherated classes
        Public MustOverride Function QueryValue(ByVal sCmd As String) As Double
        Public MustOverride Function QueryString(ByVal sCmd As String) As String
        Public MustOverride Sub SendCmd(ByVal sCmd As String)
    End Class


    ''' <summary>
    ''' RS232 interface
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class iRS232Generic
        Inherits iInstrumentGeneric

        Protected mPort As IO.Ports.SerialPort

        Public MustOverride Overloads Function Initialize(ByVal sPort As String, ByVal BaudRate As Integer, ByVal RaiseError As Boolean) As Boolean

        Public Overloads Function Initialize(ByVal sPort As String, ByVal BuadRate As Integer) As Boolean
            Return Me.Initialize(sPort, BuadRate, False)
        End Function

        Public Overloads Overrides Function Initialize(ByVal COMAdrs As Integer, ByVal BaudRate As Integer, ByVal RaiseError As Boolean) As Boolean
            Return Me.Initialize("COM" & COMAdrs, BaudRate, RaiseError)
        End Function


        Public MustOverride Overrides ReadOnly Property Name() As String

        Public MustOverride Overrides Function QueryString(ByVal sCmd As String) As String
        Public MustOverride Overrides Function QueryValue(ByVal sCmd As String) As Double
        Public MustOverride Overrides Sub SendCmd(ByVal sCmd As String)

        Public Overrides Property Connected() As Boolean
            Get
                If mPort Is Nothing Then
                    Return False
                Else
                    Return mPort.IsOpen
                End If
            End Get
            Set(ByVal value As Boolean)
                If mPort Is Nothing Then Return
                If value Then
                    If Not mPort.IsOpen Then mPort.Open()
                Else
                    mPort.Close()
                End If
            End Set
        End Property

#Region "shared functions"
        Private Shared mSorter As System.Comparison(Of String) = AddressOf ComPortSorter

        Private Shared Function ComPortSorter(ByVal x As String, ByVal y As String) As Integer
            Dim cpr As New System.Collections.CaseInsensitiveComparer

            If x.StartsWith("COM") And x.StartsWith("COM") Then
                Dim Vx As Double = Val(x.Substring(3))
                Dim Vy As Double = Val(y.Substring(3))
                Return cpr.Compare(Vx, Vy)
            Else
                Return cpr.Compare(x, y)
            End If
        End Function

        Public Shared ReadOnly Property PortNamesAll() As String()
            Get
                Dim sData As String() = New String() {}
                Array.Resize(sData, My.Computer.Ports.SerialPortNames.Count)
                My.Computer.Ports.SerialPortNames.CopyTo(sData, 0)
                Array.Sort(sData, mSorter)
                Return sData
            End Get
        End Property

        Public Shared ReadOnly Property PortNamesAvailable() As String()
            Get
                Dim sList As New List(Of String)
                Dim port As New IO.Ports.SerialPort

                For Each s As String In PortNamesAll()
                    port.PortName = s
                    Try
                        port.Open()
                    Catch ex As Exception
                        Continue For
                    End Try
                    sList.Add(s)
                    port.Close()
                Next

                Dim sData As String() = New String() {}
                Array.Resize(sData, sList.Count)
                sList.CopyTo(sData, 0)
                Return sData
            End Get
        End Property

#End Region
    End Class

End Namespace
