Imports System.Drawing

Namespace Core
    Public Class Utils

#Region " Move Process Window "

        ' [ Move Process Window ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        '
        ' Move the notepad window at 10,50 (X,Y)
        ' Move_Process_Window("notepad.exe", 10, 50)
        '
        ' Move the notepad window at 10 (X) and preserving the original (Y) process window position
        ' Move_Process_Window("notepad.exe", 10, Nothing)

        <System.Runtime.InteropServices.DllImport("user32.dll")>
        Shared Function GetWindowRect(hWnd As IntPtr, ByRef rc As Rectangle) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll")>
        Shared Function MoveWindow(hWnd As IntPtr, x As Integer, y As Integer, Width As Integer, Height As Integer, repaint As Boolean) As Boolean
        End Function

        Public Shared Sub Move_Process_Window(ByVal ProcessName As String, ByVal X As Integer, ByVal Y As Integer)

            ProcessName = If(ProcessName.ToLower.EndsWith(".exe"),
                             ProcessName.Substring(0, ProcessName.LastIndexOf(".")),
                             ProcessName)

            Dim rect As Rectangle = Nothing
            Dim proc As Process = Nothing

            Try
                ' Find the process
                proc = Process.GetProcessesByName(ProcessName).First

                ' Store the process Main Window positions and sizes into the Rectangle.
                GetWindowRect(proc.MainWindowHandle, rect)

                ' Move the Main Window
                MoveWindow(proc.MainWindowHandle,
                           If(Not X = Nothing, X, rect.Left),
                           If(Not Y = Nothing, Y, rect.Top),
                           (rect.Width - rect.Left),
                           (rect.Height - rect.Top),
                           True)

            Catch ex As InvalidOperationException
                Throw New Exception("Process not found.")
            Finally
                rect = Nothing
                If proc IsNot Nothing Then proc.Dispose()

            End Try

        End Sub

#End Region

    End Class
End Namespace

