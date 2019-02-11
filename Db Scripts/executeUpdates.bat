@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous log file
del log.txt

ECHO R1.3
psql -h localhost -U postgres -d postgres -a -q -f .\UPDATE\RELEASE1.3.sql > log.txt

ECHO R1.4
psql -h localhost -U postgres -d postgres -a -q -f .\UPDATE\RELEASE1.4.sql >> log.txt

ECHO R1.5
psql -h localhost -U postgres -d postgres -a -q -f .\UPDATE\RELEASE1.5.sql >> log.txt


pause