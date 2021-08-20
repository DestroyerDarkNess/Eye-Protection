Imports System.ComponentModel
Imports System.Management
Imports System.Runtime.InteropServices

Public Class ParentControl

    Private MainTransparent As New Protector

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ParentControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainTransparent.Show()
        RanbowBar1.StartProgress()
        SetHotKey()
        Guna.UI.Lib.GraphicsHelper.ShadowForm(Me)
        GuiRuntime()
        ParentMonitor()
        '  Dim MousePos As Point = Cursor.Position
        ' Me.Location = New Point(MousePos.X - Me.Width, MousePos.Y - Me.Height)
    End Sub

    Private Sub GuiRuntime()
        AddDragger(Me)
        AddDragger(Panel1)
        AddDragger(Panel2)
        AddDragger(Label1)
        AddDragger(Label2)
        AddDragger(Label3)
    End Sub

    Public Sub AddDragger(ByVal cControl As Control)
        Dim NewDragC As New Guna.UI.WinForms.GunaDragControl With {.TargetControl = cControl}
    End Sub

    Private Sub GunaMetroTrackBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles GunaMetroTrackBar1.Scroll
        MainTransparent.Opacity = GunaMetroTrackBar1.Value * 1 / 100
    End Sub

    Private Sub GunaGradientButton1_Click(sender As Object, e As EventArgs) Handles GunaGradientButton1.Click
        If Not ColorDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            If Not ColorDialog1.Color = Color.Fuchsia Then
                MainTransparent.BackColor = ColorDialog1.Color
            End If
        End If
    End Sub

    Private Sub GunaControlBox1_Click(sender As Object, e As EventArgs) Handles GunaControlBox1.Click
        Me.Hide()
    End Sub

    Private WithEvents Hotkey As GlobalHotkey = Nothing

    Public Sub SetHotKey()
        Try
            Hotkey = New GlobalHotkey(GlobalHotkey.KeyModifier.Ctrl Or GlobalHotkey.KeyModifier.Alt, Keys.Escape)
            Hotkey.Tag = "Closing Eye Protection"
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub

    Private Sub HotKey_Press(ByVal sender As GlobalHotkey, ByVal e As GlobalHotkey.HotKeyEventArgs) Handles Hotkey.Press
        If MainTransparent.Visible = True Then
            Me.Hide()
        Else
            Me.Show()
        End If
    End Sub

    Private Sub ParentControl_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Hotkey.Unregister()
    End Sub

#Region " Key Monitor "

    <DllImport("user32.dll")>
    Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function

    Private CurrentProc As Process = Process.GetCurrentProcess

    Private WithEvents Timer1 As New Timer With {.Enabled = True}

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If GetAsyncKeyState(Keys.F2) <> 0 Then
                If Me.Visible = True Then
                    Me.Hide()
                Else
                    Me.Show()
                End If
            End If

            Dim ProcessList As Process() = Process.GetProcessesByName(CurrentProc.ProcessName)

            For Each Proc As Process In ProcessList
                If Not Proc.Id = CurrentProc.Id Then
                    Proc.Kill()
                    Me.Show()
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Function CursorInArea() As Boolean
        Try
            Dim MousePos As Point = Cursor.Position
            Dim DialogPos As Point = Me.Location
            Dim RegionX As Integer = DialogPos.X + Me.Width
            Dim RegionY As Integer = DialogPos.Y + Me.Height

            If MousePos.X >= DialogPos.X AndAlso MousePos.X <= RegionX Then
                Return True
            End If

            If MousePos.Y >= DialogPos.Y AndAlso MousePos.Y <= RegionY Then
                Return True
            End If

            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

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

#Region " Parent Monitor "

    Public Sub ParentMonitor()
        Dim tskThread As New Task(AddressOf ParentWait, TaskCreationOptions.LongRunning)
        tskThread.Start()
    End Sub

    Private Sub ParentWait()
        Try
            Using parentProcess As Process = GetParentProcess()
                If String.Equals(parentProcess.MainModule.ModuleName, "explorer.exe", StringComparison.OrdinalIgnoreCase) = False Then
                    parentProcess.WaitForExit()
                    Application.Exit()
                End If
            End Using

        Catch ex As Exception
            Me.Close()
        End Try
    End Sub

    Private Function GetParentProcess() As Process
        Dim parentProcess As Process = Nothing

        Using currentProcess As Process = Process.GetCurrentProcess()
            Dim filter As String = String.Format("ProcessId={0}", currentProcess.Id)
            Dim query As SelectQuery = New SelectQuery("Win32_Process", filter)

            Using searcher As ManagementObjectSearcher = New ManagementObjectSearcher(query)

                Using results As ManagementObjectCollection = searcher.[Get]()

                    If results.Count > 0 Then
                        If results.Count > 1 Then Throw New InvalidOperationException()
                        Dim resultEnumerator As IEnumerator = results.GetEnumerator()
                        Dim fMoved As Boolean = resultEnumerator.MoveNext()

                        Using wmiProcess As ManagementObject = CType(resultEnumerator.Current, ManagementObject)
                            Dim parentProcessId As PropertyData = wmiProcess.Properties("ParentProcessId")
                            Dim pid As UInteger = CUInt(parentProcessId.Value)
                            parentProcess = Process.GetProcessById(CInt(pid))
                        End Using
                    End If
                End Using
            End Using
        End Using

        Return parentProcess
    End Function

#End Region

End Class