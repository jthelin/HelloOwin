@setlocal
@echo off

set CMDHOME=%~dp0
@REM Remove trailing backslash \
set CMDHOME=%CMDHOME:~0,-1%

start "HelloOwinServer" %CMDHOME%\HelloOwinServer\bin\Debug\HelloOwinServer.exe
