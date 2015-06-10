# this script generates the Elite-DB in the "..\data\"-folder of this file
# for starting the server again after the DB is created simply type 
#
#              .\bin\mysqld.exe --defaults-file=Elite.ini --console
#

set EliteDBName=Elite_DB
set ROOT_PW=EliteAdmin
set USER_PW=Elite

SET SOURCE_DIR=%~dp0
SET SOURCE_DRIVE=%~d0

#SET MYSQL_DIR=%1
#SET MYSQL_DATADIR=%2
#SET LOGFILE=.log\Install.log

# goto current working dir
%SCRIPT_DRIVE%
cd %SCRIPT_LOCATION%..

mkdir data

if exist .\share\all.sql del .\share\all.sql 
type .\share\mysql_system_tables.sql .\share\mysql_system_tables_data.sql .\share\fill_help_tables.sql > .\share\all.sql
.\bin\mysqld.exe --defaults-file=Elite.ini --bootstrap --console  < .\share\all.sql 
if exist .\share\all.sql del .\share\all.sql

start .\bin\mysqld.exe --defaults-file=Elite.ini --console

.\bin\mysql -u root --execute="UPDATE mysql.user SET Password = PASSWORD('%ROOT_PW%') WHERE User = 'root'; FLUSH PRIVILEGES;"

.\bin\mysql -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.user WHERE User <> 'root';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="DELETE FROM mysql.db WHERE User <> 'root';"

.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE SCHEMA IF NOT EXISTS `%EliteDBName%` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;"
                    
.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER 'RN_User'@'localhost' IDENTIFIED BY '%USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant Insert, Select, Update, Delete On `%EliteDBName%`.* To 'RN_User'@'localhost';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="CREATE USER 'RN_User'@'127.0.0.1' IDENTIFIED BY '%USER_PW%';"
.\bin\mysql -u root --password=%ROOT_PW% --execute="Grant Insert, Select, Update, Delete On `%EliteDBName%`.* To 'RN_User'@'127.0.0.1';"

.\bin\mysql -u root --password=%ROOT_PW% < .\script\create_Elite_DB.sql

pause
