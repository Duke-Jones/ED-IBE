@echo off
set DEBUG = 0

if [%1] EQU [/forceinstall] (
	REM the installationscript will redirect all messages into a log-file
	@echo on
) else (
	REM show messages only if wanted
	if [%DEBUG%] EQU [1] @echo on
)

Setlocal EnableDelayedExpansion

set EliteDBName=Elite_DB
set ROOT_PW=EliteAdmin
set RN_USER=RN_User
set RN_USER_PW=Elite
set RN_USER_PRIV=Insert, Select, Update, Delete, Create Temporary Tables, Create View, Drop
set SQL_HOSTS=localhost 127.0.0.1 ::1 %computername% ASTERISK

REM "super" permission is needed for performance reasons while inserting big data plenties
set RN_USER_PRIV_GLOB=Super	

SET SOURCE_DIR=%~dp0

REM if parameter 1 is /forceinstall ?  then don't ask
if [%1] EQU [/forceinstall] goto start

REM ask before delete something
echo. 
echo. 
echo This script generates the Elite-DB in the "..\data\"-folder of this file.
echo For starting the server again after the DB is created simply type : 
echo. 
echo            %MYSQL_PATH%\bin\mysqld.exe --defaults-file=Elite.ini --console
echo                                     or call
echo                                start_server.cmd
echo. 
echo Warning: the complete database will be deleted and new generated !!! ALL DATA WILL BE LOST !!!

set /p userinput=If you want to to proceed type (without quotes) "kill'em all" :
if "%userinput%" NEQ "kill'em all" goto end

:start

REM goto current working dir (one level up from this script)
cd /D %SOURCE_DIR%..
SET SOURCE_DIR=%cd%

if [%DEBUG%] EQU [1] pause

REM get the optional dirs in the installation routine (destination)
if not [%2] EQU [] (
   SET DESTINATION_DIR=%~2
) else (
   SET DESTINATION_DIR=%SOURCE_DIR%
)

REM get the optional dirs in the installation routine (source)
if not [%3] EQU [] (
   SET MYSQL_PATH=%~3
) else (
   SET MYSQL_PATH=%SOURCE_DIR%
)

if [%DEBUG%] EQU [1] echo SOURCE_DIR = %SOURCE_DIR%
if [%DEBUG%] EQU [1] echo DESTINATION_DIR = %DESTINATION_DIR%
if [%DEBUG%] EQU [1] pause

REM shut down the server if runnin'
"%MYSQL_PATH%\bin\mysqladmin" -u root --password=%ROOT_PW% shutdown
timeout /t 5

REM delete old data dir if existing and (re)create
if exist %DESTINATION_DIR%\data del /s /f /q %DESTINATION_DIR%\data & rd /s /q %DESTINATION_DIR%\data
timeout /t 1
mkdir %DESTINATION_DIR%\data

REM create script for creating the database
if exist %DESTINATION_DIR%\all.sql del %DESTINATION_DIR%\all.sql 
type "%MYSQL_PATH%\share\mysql_system_tables.sql" "%MYSQL_PATH%\share\mysql_system_tables_data.sql" "%MYSQL_PATH%\share\fill_help_tables.sql" > "%DESTINATION_DIR%\all.sql"

REM create the database
"%MYSQL_PATH%\bin\mysqld.exe" --defaults-file="%DESTINATION_DIR%\Elite.ini" --bootstrap --console  < "%DESTINATION_DIR%\all.sql"
del "%DESTINATION_DIR%\all.sql"

if [%DEBUG%] EQU [1] pause
if [%DEBUG%] EQU [1] pause

rem start sql-server first time
start /D "%MYSQL_PATH%" bin\mysqld.exe --defaults-file="%DESTINATION_DIR%\Elite.ini" --console

if [%DEBUG%] EQU [1] pause 

REM prepare root-user and delete all other waste-accounts
"%MYSQL_PATH%\bin\mysql.exe" -u root --execute="UPDATE mysql.user SET Password = PASSWORD('%ROOT_PW%') WHERE User = 'root'; FLUSH PRIVILEGES;"
"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.user WHERE User <> 'root';"
"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.db WHERE User <> 'root';"

REM create elite schema
"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="CREATE SCHEMA IF NOT EXISTS `%EliteDBName%` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;"

REM add user-account for expectable locations
for %%h in (%SQL_HOSTS%) do (
	if [%%h] EQU [ASTERISK] (
		set GRANT_LOCATION=*  
	) Else (
		set GRANT_LOCATION=%%h
	)
	
	"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'!GRANT_LOCATION!' IDENTIFIED BY '%RN_USER_PW%';"
	"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'!GRANT_LOCATION!';"
	
	REM "super" permission is needed for performance reasons while inserting big data plenties	
	"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'!GRANT_LOCATION!';"
) 

REM create the Elite database itself
"%MYSQL_PATH%\bin\mysql.exe" -u root --password=%ROOT_PW% < "%DESTINATION_DIR%\script\create_Elite_DB.sql"

REM go back into script directory
cd script

if [%DEBUG%] EQU [1] pause

REM shut down the server if it's a installation
if [%1] EQU [/forceinstall] (
   "%MYSQL_PATH%\bin\mysqladmin" -u root --password=%ROOT_PW% shutdown
   timeout /t 5
)

:end

echo. 
echo finished !
echo. 
