Public Class RanbowBar
    Inherits Panel

    Private ColorBar1 As New ColorBar With {.Smoothness = 7, .Value = .Maximum, .Visible = False, .Name = "ColorBar1"}
    Private ColorBar2 As New ColorBar With {.Smoothness = 7, .Value = .Maximum, .Visible = False, .Name = "ColorBar2", .Reversed = True}
    Private WithEvents Timer1 As New System.Windows.Forms.Timer With {.Interval = 1, .Enabled = False}

    Private _BarOrientation As Orientation = Orientation.Horizontal
    Public Property BarOrientation As Integer
        Get
            Return _BarOrientation
        End Get
        Set(value As Integer)
            _BarOrientation = value
            Invalidate()
        End Set
    End Property

    Public Sub New()

        '  Me.Controls.Add(ColorBar1)
        '   Me.Controls.Add(ColorBar2)
        ResetPoint()
    End Sub


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        MyBase.OnPaint(e)

        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), Me.ClientRectangle)

        ColorBar1.Size = Me.Size
        ColorBar2.Size = Me.Size
        ColorBar1.Orientation = _BarOrientation
        ColorBar2.Orientation = _BarOrientation



    End Sub

    Private Sub ResetPoint()

        Select Case _BarOrientation

            Case Orientation.Horizontal

                ColorBar1.Location = New Point(0 - ColorBar1.Width, 0)
                ColorBar2.Location = New Point(ColorBar1.Location.X - ColorBar2.Width, 0)

            Case Orientation.Vertical

                ColorBar1.Location = New Point(0, 0 - ColorBar1.Height)
                ColorBar2.Location = New Point(0, ColorBar1.Location.Y - ColorBar2.Height)

        End Select

    End Sub

    Public Sub StartProgress()
        ColorBar1.Visible = True
        ColorBar2.Visible = True
        Timer1.Enabled = True
    End Sub

    Public Sub StopProgress()
        ColorBar1.Visible = False
        ColorBar2.Visible = False
        Timer1.Enabled = False
    End Sub

    Dim BackControlList As New List(Of ColorBar)
    Dim BackColorBar1 As ColorBar = Nothing

    Dim Reversed As Boolean = True
    Dim ID As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case _BarOrientation

            Case Orientation.Horizontal

                If BackColorBar1 Is Nothing Then

                    BackColorBar1 = New ColorBar With {.Smoothness = 7, .Value = .Maximum, .Visible = True, .Reversed = False, .WidthThickness = 0.0}
                    BackColorBar1.Name = ID
                    BackColorBar1.BorderStyle = BorderStyle.None
                    BackColorBar1.Size = Me.Size
                    Me.Controls.Add(BackColorBar1)
                    ID += 1
                    BackColorBar1.Location = New Point(0, 0) '- BackColorBar1.Width

                End If

                If BackColorBar1.Location.X = 0 Then

                    Dim UltimateControl As Control = Me.Controls(Me.Controls.Count - 1)
                    BackControlList.Add(BackColorBar1)
                    BackColorBar1 = New ColorBar With {.Smoothness = 7, .Value = .Maximum, .Visible = True, .Reversed = Reversed, .WidthThickness = 0.0}
                    BackColorBar1.Name = ID
                    BackColorBar1.BorderStyle = BorderStyle.None
                    BackColorBar1.BringToFront()
                    Reversed = Not Reversed
                    BackColorBar1.Size = Me.Size
                    Me.Controls.Add(BackColorBar1)
                    ID += 1
                    BackColorBar1.Location = New Point(UltimateControl.Location.X - BackColorBar1.Width, 0)

                End If

                For Each ControlEx As Control In BackControlList
                    ControlEx.Location = New Point(ControlEx.Location.X + 1, 0)
                Next

                BackColorBar1.Location = New Point(BackColorBar1.Location.X + 1, 0)

            Case Orientation.Vertical

                Dim IsLimit As Boolean = (ColorBar2.Location.Y = Me.Height)

                If IsLimit = True Then

                    ResetPoint()

                Else

                    ColorBar1.Location = New Point(0, ColorBar1.Location.Y + 1)
                    ColorBar2.Location = New Point(0, ColorBar2.Location.Y + 2)

                End If

        End Select
    End Sub

End Class
