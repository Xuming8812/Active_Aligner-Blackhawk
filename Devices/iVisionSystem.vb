Option Explicit On
Option Strict On
Option Infer Off

Imports System.Runtime.InteropServices

Namespace Instrument


    Public Class iVisionSystem
        'Private Declare Sub ShowDlg Lib "VisionDLL.dll" ()
        'extern "C" __declspec(dllimport) void   Vision(int Scene,int IndexImage,double& x,double& y,double &angle,int &error);
        'Private Declare Function Vision Lib "VisionDLL.dll" (ByVal Scene As Int32, ByVal ImageIndex As Int32, ByRef X As Double, ByRef Y As Double, ByRef Angle As Double, ByRef ErrorCode As Int32) As Integer

        <DllImport("VisionDLL.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)> _
        Private Shared Sub Vision(ByVal Scene As Int32, ByVal ImageIndex As Int32, ByVal bOnlineTest As Int32, ByVal bSaveImage As Int32, ByRef X As Double, ByRef Y As Double, ByRef Angle As Double, ByRef ErrorCode As Int32)
        End Sub

        Private mError As Int32

        Public Function Initialize() As Boolean
            Dim x, y, angle As Double
            Dim s As String

            'mError = 10
            'angle = 65

            'this is used for testing DLL communication. 
            'Vision(99, 1, x, y, angle, mError)
            'Vision(1, 1, x, y, angle, mError)

            'Initialize
            Vision(0, 1, 1, 1, x, y, angle, mError)
            'Vision(1, 1, 1, 1, x, y, angle, mError)
            'If mError = 0 Then
            '    'read reconfigure file
            '    Vision(99, 1, x, y, angle, mError)
            'End If

            If mError = 0 Then
                Return True
            Else
                s = Me.GetErrorString()
                MessageBox.Show(s, "Vision DLL")
                Return False
            End If
        End Function

        Public Sub Close()
            Dim x, y, angle As Double
            Vision(100, 1, 1, 1, x, y, angle, mError)
        End Sub

        Public Function GetVisionData(ByVal Scene As Int32, ByVal ImageIndex As Int32, ByVal bOnlineTest As Int32, ByVal bSaveImage As Int32, ByRef X As Double, ByRef Y As Double, ByRef Angle As Double) As Boolean
            'Scene=1
            Vision(Scene, ImageIndex, bOnlineTest, bSaveImage, X, Y, Angle, mError)
            Return (mError = 0)
        End Function

        Public Function GetErrorString() As String
            Select Case mError
                Case 0
                    Return "No Error"
                Case 1
                    Return "Failed Initialization"
                Case 2
                    Return "Failed Image Process"
                Case 4
                    Return "No part found"
                Case Else
                    Return "Unknown Error"

            End Select

        End Function
    End Class
End Namespace


