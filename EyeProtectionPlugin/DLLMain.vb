Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO
Imports System.Drawing

Public Class dllmain

#Region " Plugin Constructor "

    Public Shared Function Start() As String
        Try
            StartPlugin()
            Return String.Empty
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Shared Function AutoExecute() As Boolean
        Return False
    End Function

    Public Shared Function RequiredAc() As Boolean
        Return False
    End Function

    Public Shared Function Visible() As Boolean
        Return True
    End Function

    Public Shared Function LoadSync() As Boolean
        Return True
    End Function

    Public Shared Function Title() As String
        Return "Eye Protector"
    End Function

    Public Shared Function Description() As String

        Dim Info As String = <a><![CDATA[
Protect your Eyes!

The brightness of your screen, in the long run, 
irritates your eyes, and erodes your vision, 
use Eyes Protection to protect yourself!

...

]]></a>.Value

        Return Info.ToString
    End Function

    Public Shared Function Icon() As Image
        Return My.Resources.icons8_security_cameras_48__2_
    End Function

#End Region


    Private Shared Sub StartPlugin()

        Dim CachePath As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\Local\DesktopOrganizer\")

        Dim PluginsDir As String = IO.Path.Combine(CachePath, "Plugins")

        Dim ProcessPath As String = IO.Path.Combine(PluginsDir, "EyeProtection\EyeProtection.exe")

        If IO.File.Exists(ProcessPath) = True Then

            Process.Start(ProcessPath)

        Else

            Throw New FileLoadException("File No Fount!" & vbNewLine & ProcessPath)

        End If



    End Sub

End Class
