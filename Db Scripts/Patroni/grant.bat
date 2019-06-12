
ECHO GRANT to het_application_proxy;
psql -h localhost -p 5433 -U postgres -d hets -a -q -f ..\CREATE\CREATE_3_HETS_DB_DDL-grants.sql >> create_log.txt
ECHO Make sure the user ID is correct for your environment in grant-user.sql 
psql -h localhost -p 5433 -U postgres -d hets -a -q -f .\grant-user.sql >> create_log.txt