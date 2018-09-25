Option Explicit On
Option Strict On

Public Class ucOnOff
    Public Enum StateEnum
        [On]
        Off
        Unknown
    End Enum

    Private mScale As Double

    Public Event StateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Public Sub New()
        InitializeComponent()
        lblStatus.BackColor = Color.Gray
        optOn.Checked = False
        optOff.Checked = False
        Me.IconScale = 0.5
    End Sub

    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            MyBase.Font = value
            optOn.Font = value
            optOff.Font = value
            Me.ResizeCtrl()
        End Set
    End Property

    Public Property IconScale() As Double
        Get
            Return mScale
        End Get
        Set(ByVal value As Double)
            mScale = value
            Me.ResizeCtrl()
        End Set
    End Property

    Public Property IconBorderStyle() As System.Windows.Forms.BorderStyle
        Get
            Return lblStatus.BorderStyle
        End Get
        Set(ByVal value As System.Windows.Forms.BorderStyle)
            lblStatus.BorderStyle = value
        End Set
    End Property

    Public Property State() As StateEnum
        Get
            Select Case True
                Case optOn.Checked
                    Return StateEnum.On
                Case optOff.Checked
                    Return StateEnum.Off
                Case Else
                    Return StateEnum.Unknown
            End Select
        End Get
        Set(ByVal value As StateEnum)
            Select Case value
                Case StateEnum.On
                    optOn.Checked = True
                Case StateEnum.Off
                    optOff.Checked = True
                Case Else
                    optOn.Checked = False
                    optOff.Checked = False
            End Select
        End Set
    End Property

    Private Sub ResizeCtrl()
        lblStatus.Width = Convert.ToInt32(mScale * optOn.Height)
        lblStatus.Height = lblStatus.Width
        lblStatus.Top = optOn.Top + (optOn.Height - lblStatus.Height) \ 2

        optOn.Left = lblStatus.Left + lblStatus.Width + 20
        optOff.Left = optOn.Left + optOn.Width + 10

        Me.MinimumSize = New Size(optOff.Left + optOn.Width + lblStatus.Left, _
                                  optOff.Top + optOn.Height + optOff.Top)
        Me.MaximumSize = Me.MinimumSize
    End Sub

    Private Sub opt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optOn.CheckedChanged, optOff.CheckedChanged
        Static LastState As StateEnum = StateEnum.Unknown

        'changes?
        If Me.State <> LastState Then
            'save state
            LastState = Me.State
            'set status
            Select Case LastState
                Case StateEnum.On
                    lblStatus.BackColor = Color.LawnGreen
                Case StateEnum.Off
                    lblStatus.BackColor = Color.Red
                Case Else
                    lblStatus.BackColor = Color.Gray
            End Select
            'event
            RaiseEvent StateChanged(Me, New System.EventArgs())
        End If

    End Sub


End Class
