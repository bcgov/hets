@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous log file
del log.txt

ECHO Drop database if it exists
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_0_HETS_DB.sql > log.txt
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_1_HETS_DB.sql >> log.txt
psql -h localhost -U postgres -d postgres -a -q -f .\DROP\DROP_2_HETS_DB-role.sql >> log.txt

ECHO Create HETS database
psql -h localhost -U postgres -d postgres -a -q -f .\CREATE\CREATE_0_HETS_DB.sql >> log.txt

ECHO Create Tables / Indexes / Etc.
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_1_HETS_DB_DDL-tables.sql >> log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_2_HETS_DB_DDL-fk_ix.sql >> log.txt

ECHO Setup grants and add users (DEV)
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_3_HETS_DB_DDL-grants.sql >> log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_4_HETS_DB-add-user.sql >> log.txt

ECHO Add additional constraints
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_5_HETS_DB-constraints.sql >> log.txt

ECHO Add triggers
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_6_HETS_DB-trigger-function.sql >> log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_7_HETS_DB-before-triggers.sql >> log.txt
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_8_HETS_DB-after-triggers.sql >> log.txt

ECHO Add default data
psql -h localhost -U postgres -d hets -a -q -f .\CREATE\CREATE_9_HETS_DB-default-values.sql >> log.txt

pause