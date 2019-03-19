#!/bin/bash
# Postgresql automated backup script
# See README.md for documentation on this script

export NUM_BACKUPS="${NUM_BACKUPS:-31}"
export BACKUP_PERIOD="${BACKUP_PERIOD:-1d}"

# move the restore scripts to the correct folder (can't be done at deployment time)
export RESTORE_DIR="${RESTORE_DIR}"
export RESTORE_SCRIPT_FILE="/restore.sh"
export RESTORE_SQL_FILE="/connections.sql"
export FULL_SCRIPT_FILE="${RESTORE_DIR}restore.sh"
export FULL_SQL_FILE="${RESTORE_DIR}connections.sql"

if [ -d "$RESTORE_DIR" ]
then
	echo "*** clearing restore directory"
	rm -rf "$RESTORE_DIR/*.*" 
else
	echo "*** creating restore directory"
	mkdir "$RESTORE_DIR"
fi

cp -f $RESTORE_SCRIPT_FILE $FULL_SCRIPT_FILE
cp -f $RESTORE_SQL_FILE $FULL_SQL_FILE

# execute the backup - then wait for 24 hours
while true; do
	FINAL_BACKUP_DIR=$BACKUP_DIR"`date +\%Y-\%m-\%d`/"
	DBFILE=$FINAL_BACKUP_DIR"$POSTGRESQL_DATABASE`date +\%Y-\%m-\%d-%H-%M`"
	echo "Making backup directory in $FINAL_BACKUP_DIR"
	
	if ! mkdir -p $FINAL_BACKUP_DIR; then
		echo "Cannot create backup directory in $FINAL_BACKUP_DIR." 1>&2
		exit 1;
	fi;	
	
	export PGPASSWORD=$POSTGRESQL_PASSWORD

	if ! pg_dump --host="$DATABASE_SERVICE_NAME" --port="5432" --username="$POSTGRESQL_USER" --dbname="$POSTGRESQL_DATABASE" --blobs --format="c" --compress="9" --file="$DBFILE.bak"; then
		echo "[!!ERROR!!] Failed to backup database $POSTGRESQL_DATABASE" 
	else
		echo "Database backup written to $DBFILE.bak"
		
		# cull backups to a limit of NUM_BACKUPS
		find ${BACKUP_DIR}* | grep bak | sort -r | sed "1,${NUM_BACKUPS}d" | xargs rm -rf
		
		# cull backup folders to a limit of NUM_BACKUPS
		find /backups | grep 20 | grep -v bak | grep -v trash | sort -r | sed "1,${NUM_BACKUPS}d" | xargs rm -rf
	fi;
	echo "Current Backups:"
	ls -alh ${BACKUP_DIR}*/*.bak*
	echo "===================="

	# 24 hrs
	sleep ${BACKUP_PERIOD}

done