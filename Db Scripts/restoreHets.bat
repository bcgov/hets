@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous log file
del restore_log.txt

ECHO Drop database if it exists
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_0_HETS_DB.sql > restore_log.txt
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_1_HETS_DB.sql >> restore_log.txt
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_2_HETS_DB-role.sql >> restore_log.txt

ECHO Create HETS database
psql -h localhost -U postgres -d postgres -a -q -f .\CREATE\CREATE_0_HETS_DB.sql >> restore_log.txt

ECHO Create Tables / Indexes / Etc.
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_1_HETS_DB_DDL-tables.sql >> restore_log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_2_HETS_DB_DDL-fk_ix.sql >> restore_log.txt

ECHO Setup grants and add users
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_3_HETS_DB_DDL-grants.sql >> restore_log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_4_HETS_DB-add-user.sql >> restore_log.txt

ECHO Add additional constraints
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_5_HETS_DB-constraints.sql >> restore_log.txt

ECHO Add triggers
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_6_HETS_DB-trigger-function.sql >> restore_log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_7_HETS_DB-before-triggers.sql >> restore_log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_8_HETS_DB-after-triggers.sql >> restore_log.txt

ECHO Restore database from backup
pg_restore --clean --host="localhost" --port="5432" --username="postgres" --dbname="hets" --format="c" ".\BACKUP\hets2019-02-14-04-04.bak" >> restore_log.txt

pause