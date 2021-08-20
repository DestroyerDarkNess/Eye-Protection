Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Protector

    <DllImport("user32", EntryPoint:="SetLayeredWindowAttributes")>
    Public Shared Function SetLayeredWindowAttributes(ByVal Handle As IntPtr, ByVal crKey As Integer, ByVal bAlpha As Byte, ByVal dwFlags As Integer) As Integer
    End Function

    Private Sub Protector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SetLayered(2)
            SetWindowStyle.SetWindowStyle(Me.Handle, SetWindowStyle.WindowLongFlags.GWL_EXSTYLE, SetWindowStyle.WindowStyles.WS_EX_TRANSPARENT Or SetWindowStyle.WindowStyles.WS_EX_LAYERED)
            Timer1.Enabled = True
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub

    Public Sub SetLayered(ByVal LWA_ALPHA As Integer)
        SetLayeredWindowAttributes(Me.Handle, 0, 128, LWA_ALPHA)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Me.TopMost = True
    End Sub

#Region " No Windows Focus "

    Private Const SW_SHOWNOACTIVATE As Integer = 4
    Private Const HWND_TOPMOST As Integer = -1
    Private Const SWP_NOACTIVATE As UInteger = &H10

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function ShowWindow(ByVal hWnd As System.IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Public Shared Sub ShowInactiveTopmost(ByVal frm As System.Windows.Forms.Form)
        Try
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE)
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, SWP_NOACTIVATE)
        Catch ex As System.Exception
        End Try
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Private Const WS_EX_TOPMOST As Integer = &H8
    Private Const WS_THICKFRAME As Integer = &H40000
    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_EX_TOOLWINDOW As Integer = &H80

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim createParamsA As CreateParams = MyBase.CreateParams
            createParamsA.ExStyle = createParamsA.ExStyle Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST
            Return createParamsA
        End Get
    End Property

#End Region

End Class
