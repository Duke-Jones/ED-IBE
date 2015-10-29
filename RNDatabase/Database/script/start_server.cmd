rem @echo off
rem 
rem this script is starting the server
rem 

SET SOURCE_DIR=%~dp0
SET SOURCE_DRIVE=%~d0

%SOURCE_DRIVE%
cd %SOURCE_DIR%..

.\bin\mysqld.exe --defaults-file=Elite.ini --console
