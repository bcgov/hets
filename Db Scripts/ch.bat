@ECHO OFF

ECHO Delete previous log file
del create_log.txt

ECHO Create HETS database
psql -h localhost -p 5433 -U postgres -d postgres -a -q -f .\CREATE\CREATE_0_HETS_DB.sql >> create_log.txt

ECHO Create Tables / Indexes / Etc.
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_1_HETS_DB_DDL-tables.sql >> create_log.txt
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_2_HETS_DB_DDL-fk_ix.sql >> create_log.txt

ECHO Setup grants and add users
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_3_HETS_DB_DDL-grants.sql >> create_log.txt
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_4_HETS_DB-add-user.sql >> create_log.txt

ECHO Add additional constraints
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_5_HETS_DB-constraints.sql >> create_log.txt

ECHO Add triggers
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_6_HETS_DB-trigger-function.sql >> create_log.txt
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_7_HETS_DB-before-triggers.sql >> create_log.txt
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_8_HETS_DB-after-triggers.sql >> create_log.txt

ECHO Add default data
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\CREATE\CREATE_9_HETS_DB-default-values.sql >> create_log.txt

pause