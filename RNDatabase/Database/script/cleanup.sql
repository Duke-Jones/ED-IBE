


K:
cd K:\RegulatedNoise_db\RegulatedNoise\MySQL

mkdir data

type .\share\mysql_system_tables.sql .\share\mysql_system_tables_data.sql .\share\fill_help_tables.sql > .\share\all.sql

.\bin\mysqld.exe --defaults-file=Elite.ini --bootstrap --console  < .\share\all_sql.sql

.\bin\mysqld.exe --defaults-file=Elite.ini --console --skip-grant-tables

set DBName=mysql
mysql -u root -pEliteAdmin %DBName% < ..\cleanup.sql

 UPDATE mysql.user SET Password = PASSWORD('newpwd')
    ->     WHERE User = 'root';
mysql> FLUSH PRIVILEGES;