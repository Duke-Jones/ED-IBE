rem @echo off
rem 
rem this script is starting the server
rem 

SET SOURCE_DIR=%~dp0
SET SOURCE_DRIVE=%~d0

%SCRIPT_DRIVE%
cd %SCRIPT_LOCATION%..


.\bin\mysqld.exe --defaults-file=Elite.ini --console
