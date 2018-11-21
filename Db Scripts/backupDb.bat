@ECHO OFF

CD "C:\Workspace\hets\Db Scripts"

ECHO Delete previous backup file
del hets.backup.bak

ECHO Backup database
pg_dump --host="localhost" --port="5432" --username="postgres" --no-password --dbname="hets" --blobs --format="c" --compress="9" --file="hets.backup.bak"

pause