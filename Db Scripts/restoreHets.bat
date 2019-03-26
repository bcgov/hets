@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous log file
del restore_log.txt

ECHO Drop current hets database
psql -q -h localhost -U postgres -d postgres -a -q -c "do $$ begin PERFORM pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'hets'; end $$" >> restore_log.txt
psql -q -h localhost -U postgres -d postgres -a -q -c "DROP DATABASE IF EXISTS hets;" >> restore_log.txt

ECHO Create empty hets database
psql -q -h localhost -U postgres -d postgres -a -q -c "CREATE DATABASE hets WITH ENCODING = 'UTF8';" >> restore_log.txt
psql -q -h localhost -U postgres -d postgres -a -q -c "SET client_encoding = 'UTF8';" >> restore_log.txt

ECHO Restore database from backup
pg_restore --host="localhost" --port="5432" --username="postgres" --dbname="hets" --format="c" ".\BACKUP\hetsDbBackup.bak" >> restore_log.txt

pause