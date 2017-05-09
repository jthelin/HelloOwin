@setlocal
@echo off

set CMDHOME=%~dp0
@REM Remove trailing backslash \
set CMDHOME=%CMDHOME:~0,-1%

%CMDHOME%\HelloOwinClient\bin\Debug\HelloOwinClient.exe
