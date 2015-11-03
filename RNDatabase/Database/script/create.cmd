rem @echo off

echo. 
echo. 
echo This script generates the Elite-DB in the "..\data\"-folder of this file.
echo For starting the server again after the DB is created simply type : 
echo. 
echo            .\bin\mysqld.exe --defaults-file=Elite.ini --console
echo. 
echo Warning: the complete database will be deleted and new generated !!! ALL DATA WILL BE LOST !!!
set /p userinput=If you want to to proceed type (without quotes) "kill'em all" :

if "%userinput%" NEQ "kill'em all" goto end

set EliteDBName=Elite_DB
set ROOT_PW=EliteAdmin
set RN_USER=RN_User
set RN_USER_PW=Elite
set RN_USER_PRIV=Insert, Select, Update, Delete, Create Temporary Tables, Create View, Drop

rem "super" permission is needed for performance reasons while inserting big data plenties
set RN_USER_PRIV_GLOB=Super	

SET SOURCE_DIR=%~dp0
SET SOURCE_DRIVE=%~d0

REM SET MYSQL_DIR=%1
REM SET MYSQL_DATADIR=%2
REM SET LOGFILE=.log\Install.log

REM goto current working dir
%SOURCE_DRIVE%
cd %SOURCE_DIR%..

if not exist data goto no_delete_required
rmdir /S /Q data 
timeout /t 2

:no_delete_required

mkdir .\data

if exist .\share\all.sql del .\share\all.sql 
type .\share\mysql_system_tables.sql .\share\mysql_system_tables_data.sql .\share\fill_help_tables.sql > .\share\all.sql
.\bin\mysqld.exe --defaults-file=Elite.ini --bootstrap --console  < .\share\all.sql 
if exist .\share\all.sql del .\share\all.sql

start .\bin\mysqld.exe --defaults-file=Elite.ini --console

.\bin\mysql -u root --execute="UPDATE mysql.user SET Password = PASSWORD('%ROOT_PW%') WHERE User = 'root'; FLUSH PRIVILEGES;"

.\bin\mysql -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.user WHERE User <> 'root';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.db WHERE User <> 'root';"

.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE SCHEMA IF NOT EXISTS `%EliteDBName%` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;"

                    
.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'localhost' IDENTIFIED BY '%RN_USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'localhost';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'localhost';"

.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'127.0.0.1' IDENTIFIED BY '%RN_USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'127.0.0.1';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'127.0.0.1';"

.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'::1' IDENTIFIED BY '%RN_USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'::1';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'::1';"

.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'%computername%' IDENTIFIED BY '%RN_USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'%computername%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'%computername%';"

rem set GRANT_LOCATION=*
rem .\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER '%RN_USER%'@'%GRANT_LOCATION%' IDENTIFIED BY '%RN_USER_PW%';"
rem .\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV% On `%EliteDBName%`.* To '%RN_USER%'@'%GRANT_LOCATION%';"
rem .\bin\mysql -u root --password=%ROOT_PW% --execute="Grant %RN_USER_PRIV_GLOB% On *.* To '%RN_USER%'@'%GRANT_LOCATION%';"


.\bin\mysql -u root --password=%ROOT_PW% < .\script\create_Elite_DB.sql


cd script

pause

:end
