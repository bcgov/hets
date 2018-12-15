@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous log file
del release1.2log.txt

ECHO Executing Release 1.2 update script
psql -h localhost -U postgres -d hets -a -q -f .\UPDATE\RELEASE1.2.sql >> release1.2log.txt

pause