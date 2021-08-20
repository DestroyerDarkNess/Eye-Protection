Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Windows.Forms

Public Class ColorBar
    Inherits UserControl

    Public Const MinSmoothness As Integer = 0
    Public Const MaxSmoothness As Integer = 7

    Public Const MinThickness As Single = 0.1
    Public Const MaxThickness As Single = 0.5

    Private Const BorderWidth As Integer = 2
    Private lstDefault As List(Of Color)
    Private lstBrushes As List(Of SolidBrush)

    Public Enum enumOrientation
        Horizontal
        Vertical
        Circular
    End Enum

    Public Enum BarStyle
        Flow
        Expand
        Block
    End Enum

    Private m_ColorList As List(Of Color)
    Private m_Style As BarStyle
    Private m_Value As Integer
    Private m_Minimum As Integer
    Private m_Maximum As Integer
    Private m_Smoothness As Integer
    Private m_Orientation As enumOrientation
    Private m_Reversed As Boolean = False
    Private m_WidthThickness As Single
    Private m_HeightThickness As Single

    Public Property WidthThickness() As Single
        Get
            Return m_WidthThickness
        End Get
        Set(ByVal value As Single)
            If (value <> m_WidthThickness) Then
                If value < MinThickness Then value = MinThickness
                If value > MaxThickness Then value = MaxThickness
                m_WidthThickness = value
                Me.Invalidate(False)
            End If
        End Set
    End Property

    Public Property HeightThickness() As Single
        Get
            Return m_HeightThickness
        End Get
        Set(ByVal value As Single)
            If (value <> m_HeightThickness) Then
                If value < MinThickness Then value = MinThickness
                If value > MaxThickness Then value = MaxThickness
                m_HeightThickness = value
                Me.Invalidate(False)
            End If
        End Set
    End Property

    Public Property Orientation() As enumOrientation
        Get
            Return m_Orientation
        End Get
        Set(ByVal value As enumOrientation)
            m_Orientation = value
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Reversed() As Boolean
        Get
            Return m_Reversed
        End Get
        Set(ByVal value As Boolean)
            If value <> m_Reversed Then
                m_Reversed = value
                lstBrushes.Reverse()
                Me.Invalidate(False)
            End If
        End Set
    End Property

    Public Property ColorList() As List(Of Color)
        Get
            Return m_ColorList
        End Get
        Set(ByVal value As List(Of Color))
            m_ColorList = value
            If Not m_ColorList Is Nothing Then
                If m_ColorList.Count < 2 Then
                    BuildColorList(lstDefault)
                Else
                    BuildColorList(m_ColorList)
                End If
            Else
                BuildColorList(lstDefault)
            End If
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Style() As BarStyle
        Get
            Return m_Style
        End Get
        Set(ByVal value As BarStyle)
            m_Style = value
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Value() As Integer
        Get
            Return m_Value
        End Get
        Set(ByVal value As Integer)
            m_Value = value
            If m_Value < m_Minimum Then m_Value = m_Minimum
            If m_Value > m_Maximum Then m_Value = m_Maximum
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Minimum() As Integer
        Get
            Return m_Minimum
        End Get
        Set(ByVal value As Integer)
            m_Minimum = value
            If m_Minimum > m_Maximum Then Swap(m_Minimum, m_Maximum)
            If m_Value < m_Minimum Then m_Value = m_Minimum
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Maximum() As Integer
        Get
            Return m_Maximum
        End Get
        Set(ByVal value As Integer)
            m_Maximum = value
            If m_Minimum > m_Maximum Then Swap(m_Minimum, m_Maximum)
            If m_Value > m_Maximum Then m_Value = m_Maximum
            Me.Invalidate(False)
        End Set
    End Property

    Public Property Smoothness() As Integer
        Get
            Return m_Smoothness
        End Get
        Set(ByVal value As Integer)
            If value < ColorBar.MinSmoothness Then value = MinSmoothness
            If value > ColorBar.MaxSmoothness Then value = MaxSmoothness
            m_Smoothness = value
            If Not m_ColorList Is Nothing Then
                BuildColorList(m_ColorList)
            Else
                BuildColorList(lstDefault)
            End If
            Me.Invalidate(False)
        End Set
    End Property

    Private Sub Swap(ByRef val1 As Integer, ByRef val2 As Integer)
        Dim temp As Integer
        temp = val1
        val1 = val2
        val2 = temp
    End Sub

    Public Sub New()

        '  InitializeComponent()

        '   Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.Opaque, True)
        Me.UpdateStyles()

        lstDefault = New List(Of Color)
        lstDefault.Add(Color.Red)
        lstDefault.Add(Color.Orange)
        lstDefault.Add(Color.Yellow)
        lstDefault.Add(Color.Green)
        lstDefault.Add(Color.Cyan)
        lstDefault.Add(Color.Blue)
        lstDefault.Add(Color.Indigo)
        lstDefault.Add(Color.Violet)

        Minimum = 0
        Maximum = 100
        Smoothness = 0
        Value = Minimum
        Style = BarStyle.Expand
        Orientation = enumOrientation.Horizontal
        Reversed = False
        WidthThickness = 0.2
        HeightThickness = 0.2

    End Sub

    Private Function InterpolateColors(ByVal color1 As Color, ByVal color2 As Color) As Color

        Return Color.FromArgb(CInt((CInt(color1.R) + CInt(color2.R)) / 2), CInt((CInt(color1.G) + CInt(color2.G)) / 2), CInt((CInt(color1.B) + CInt(color2.B)) / 2))

    End Function

    Private Sub BuildColorList(ByRef lstAdd As List(Of Color))

        Dim c As Color
        Dim lstColors As New List(Of Color)

        lstBrushes = New List(Of SolidBrush)

        For Each c In lstAdd
            lstColors.Add(c)
        Next

        Dim idx As Integer ' lstColors index
        Dim cnt As Integer ' lstColors item count
        Dim sdc As Integer ' sub-divide count

        For sdc = 0 To m_Smoothness Step 1
            idx = 0
            cnt = lstColors.Count - 1
            While idx < cnt
                lstColors.Insert(idx + 1, InterpolateColors(lstColors(idx), lstColors(idx + 1)))
                idx += 2
                cnt += 1
            End While
        Next sdc

        For Each c In lstColors
            lstBrushes.Add(New SolidBrush(c))
        Next

    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = &H14 Then ' ignore WM_ERASEBKGND
            Return
        End If
        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Me.Invalidate(False)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        MyBase.OnPaint(e)

        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), Me.ClientRectangle)

        Dim percentComplete As Single = CSng((m_Value - m_Minimum) / (m_Maximum - m_Minimum))

        If percentComplete <= 0.0F Then Exit Sub
        If percentComplete > 1.0F Then percentComplete = 1.0F

        Dim fullWidth As Single ' width = length

        If m_Orientation = enumOrientation.Horizontal Then
            fullWidth = CSng(Me.ClientRectangle.Width - BorderWidth)
        Else
            fullWidth = CSng(Me.ClientRectangle.Height - BorderWidth)
        End If

        Dim totalWidth As Single = fullWidth * percentComplete

        Dim barWidth As Single
        If m_Style = BarStyle.Expand Then
            barWidth = totalWidth
        Else
            If m_Style = BarStyle.Flow Or m_Style = BarStyle.Block Then
                barWidth = fullWidth
            End If
        End If
        barWidth /= CSng(lstBrushes.Count)

        Dim height As Single
        Dim halfBorder As Single = CSng(BorderWidth / 2)
        Dim idxColor As Integer = 0
        Dim x As Single

        Select Case m_Orientation

            Case enumOrientation.Horizontal

                height = CSng(Me.ClientRectangle.Height - BorderWidth)

                For x = halfBorder To totalWidth Step barWidth
                    e.Graphics.FillRectangle(lstBrushes(idxColor), x, halfBorder, barWidth, height)
                    If barWidth > 4 And Me.Style = BarStyle.Block Then
                        ControlPaint.DrawBorder(e.Graphics, New Rectangle(CInt(x), CInt(halfBorder), CInt(barWidth), CInt(height)), Color.Gray, ButtonBorderStyle.Outset)
                    End If
                    If idxColor < lstBrushes.Count Then
                        idxColor += 1
                    End If
                Next

                If (x < (Me.ClientRectangle.Width - halfBorder)) And percentComplete = 1.0 Then
                    If idxColor < lstBrushes.Count Then
                        e.Graphics.FillRectangle(lstBrushes(idxColor), x, halfBorder, ((Me.ClientRectangle.Width - halfBorder) - x), height)
                    End If
                End If

            Case enumOrientation.Vertical

                height = CSng(Me.ClientRectangle.Width - BorderWidth)

                For x = halfBorder To totalWidth Step barWidth
                    e.Graphics.FillRectangle(lstBrushes(idxColor), halfBorder, Me.ClientRectangle.Bottom - barWidth - x, height, barWidth)
                    If barWidth > 4 And Me.Style = BarStyle.Block Then
                        ControlPaint.DrawBorder(e.Graphics, New Rectangle(CInt(halfBorder), CInt(Me.ClientRectangle.Bottom - barWidth - x), CInt(height), CInt(barWidth)), Color.Gray, ButtonBorderStyle.Outset)
                    End If
                    If idxColor < lstBrushes.Count Then
                        idxColor += 1
                    End If
                Next

                If (x < (Me.ClientRectangle.Top - halfBorder)) And percentComplete = 1.0 Then
                    If idxColor < lstBrushes.Count Then
                        e.Graphics.FillRectangle(lstBrushes(idxColor), halfBorder, x, height, x - (Me.ClientRectangle.Top - halfBorder))
                    End If
                End If

            Case enumOrientation.Circular

                Const PI_OVER_180 As Single = 0.0174532924F
                Dim x1, y1, x2, y2, x3, y3, x4, y4 As Single
                Dim cx As Single = CSng(Me.ClientRectangle.Width / 2)
                Dim cy As Single = CSng(Me.ClientRectangle.Height / 2)
                Dim r1 As Single = CSng(Me.ClientRectangle.Width / 2)
                Dim r2 As Single = CSng((Me.ClientRectangle.Width / 2) - (r1 * m_WidthThickness))
                Dim r3 As Single = CSng(Me.ClientRectangle.Height / 2)
                Dim r4 As Single = CSng((Me.ClientRectangle.Height / 2) - (r3 * m_HeightThickness))
                Dim angle As Single = 0.0F
                Dim angleStep As Single
                Dim endAngle As Single = 360.0F * percentComplete
                Dim points(3) As PointF

                If m_Style = BarStyle.Expand Then
                    angleStep = (360.0F * percentComplete) / lstBrushes.Count
                Else
                    If m_Style = BarStyle.Flow Or m_Style = BarStyle.Block Then
                        angleStep = 360.0F / lstBrushes.Count
                    End If
                End If

                x1 = CSng(r1 * (Math.Sin(PI_OVER_180 * angle)) + cx)
                y1 = CSng(r3 * (Math.Cos(PI_OVER_180 * angle)) + cy)
                x2 = CSng(r2 * (Math.Sin(PI_OVER_180 * angle)) + cx)
                y2 = CSng(r4 * (Math.Cos(PI_OVER_180 * angle)) + cy)

                Do

                    angle += angleStep

                    x3 = CSng(r1 * (Math.Sin(PI_OVER_180 * angle)) + cx)
                    y3 = CSng(r3 * (Math.Cos(PI_OVER_180 * angle)) + cy)
                    x4 = CSng(r2 * (Math.Sin(PI_OVER_180 * angle)) + cx)
                    y4 = CSng(r4 * (Math.Cos(PI_OVER_180 * angle)) + cy)

                    points(0).X = x1
                    points(0).Y = y1
                    points(1).X = x3
                    points(1).Y = y3
                    points(2).X = x4
                    points(2).Y = y4
                    points(3).X = x2
                    points(3).Y = y2

                    x1 = x3
                    y1 = y3
                    x2 = x4
                    y2 = y4

                    If idxColor < lstBrushes.Count Then
                        e.Graphics.FillPolygon(lstBrushes(idxColor), points)
                        idxColor += 1
                    End If

                Loop Until angle >= endAngle

        End Select

    End Sub

End Class
