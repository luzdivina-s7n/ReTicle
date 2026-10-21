@echo off
set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
"%CSC%" /nologo /optimize+ /target:winexe /out:rtc.exe /win32icon:app.ico rtc.cs
echo Done.
