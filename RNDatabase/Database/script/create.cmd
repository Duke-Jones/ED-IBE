


K:
cd K:\RegulatedNoise_db\RegulatedNoise\MySQL


mkdir data

if exist .\share\all.sql del .\share\all.sql
type .\share\mysql_system_tables.sql .\share\mysql_system_tables_data.sql .\share\fill_help_tables.sql > .\share\all.sql
.\bin\mysqld.exe --defaults-file=Elite.ini --bootstrap --console  < .\share\all.sql
if exist .\share\all.sql del .\share\all.sql

.\bin\mysqld.exe --defaults-file=Elite.ini --console

set DBName=mysql
set ROOT_PW=EliteAdmin
set USER_PW=Elite


.\bin\mysql -u root --execute="UPDATE mysql.user SET Password = PASSWORD('%ROOT_PW%') WHERE User = 'root'; FLUSH PRIVILEGES;"

.\bin\mysql -u root -p%ROOT_PW% --execute="DELETE FROM mysql.user WHERE User <> 'root';"
.\bin\mysql -u root -p%ROOT_PW% --execute="DELETE FROM mysql.db WHERE User <> 'root';"

.\bin\mysql -u root -p%ROOT_PW% --execute="CREATE SCHEMA IF NOT EXISTS `Elite_DB` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;"

.\bin\mysql -u root -p%ROOT_PW% --execute="CREATE USER 'RN_User'@'localhost' IDENTIFIED BY '%USER_PW%';"
.\bin\mysql -u root -p%ROOT_PW% --execute="Grant Insert, Select, Update, Delete On `Elite_DB`.* To 'RN_User'@'localhost';"
.\bin\mysql -u root -p%ROOT_PW% --execute="CREATE USER 'RN_User'@'127.0.0.1' IDENTIFIED BY '%USER_PW%';"
.\bin\mysql -u root -p%ROOT_PW% --execute="Grant Insert, Select, Update, Delete On `Elite_DB`.* To 'RN_User'@'127.0.0.1';"

