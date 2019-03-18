#!/bin/bash
# Postgresql automated restore script
# See README.md for documentation on this script

export RESTORE_DIR="${RESTORE_DIR}"
export FULL_RESTORE_FILE="${RESTORE_DIR}/hetsDbBackup.bak"

if [ "$1" != "" ]; then
    echo "Restoring backup file: $1"
else
    echo "Backup directory and file is required."
	echo "Example execution: ./restore.sh 2019-03-17/hets2019-03-18-16-28.bak"
	exit
fi

# create restore folder
if [ -d "$RESTORE_DIR" ]
then
	echo "*** clearing restore directory"
	rm -rf "$RESTORE_DIR/*.*" 
else
	echo "*** creating restore directory"
	mkdir "$RESTORE_DIR"
fi

# check if the file exists
export FULL_BACKUP_FILE="${BACKUP_DIR}/$1"

if [ -f "$FULL_BACKUP_FILE" ]
then
	echo "*** copying file to the restore directory: $RESTORE_DIR"
else
	echo "*** backup not found"
	exit
fi

# copy file to the restore directory
cp "$FULL_BACKUP_FILE" "$FULL_RESTORE_FILE"

if [ -f "$FULL_BACKUP_FILE" ]
then
	echo "*** restoring backup file"
else
	echo "*** error copying backup file"
	exit
fi

# restore database
export PGPASSWORD=$POSTGRESQL_PASSWORD

if ! pg_restore --clean --if-exists --host="$DATABASE_SERVICE_NAME" --port="5432" --username="$POSTGRESQL_USER" --dbname="$POSTGRESQL_DATABASE" --format="c" "$FULL_RESTORE_FILE"; then
	echo "[!!ERROR!!] Failed to restore database $PGDB" 
else
	echo "*** database restore complete"	
fi;
	
exit