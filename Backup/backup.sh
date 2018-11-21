#!/bin/bash
# Postgresql automated backup script
# See README.md for documentation on this script

export NUM_BACKUPS="${NUM_BACKUPS:-31}"
export BACKUP_PERIOD="${BACKUP_PERIOD:-1d}"

while true; do
	FINAL_BACKUP_DIR=$BACKUP_DIR"`date +\%Y-\%m-\%d`/"
	DBFILE=$FINAL_BACKUP_DIR"$POSTGRESQL_DATABASE`date +\%Y-\%m-\%d-%H-%M`"
	echo "Making backup directory in $FINAL_BACKUP_DIR"
	
	if ! mkdir -p $FINAL_BACKUP_DIR; then
		echo "Cannot create backup directory in $FINAL_BACKUP_DIR." 1>&2
		exit 1;
	fi;	

	if ! pg_dump --host="$DATABASE_SERVICE_NAME" --port="5432" --username="postgres" --dbname="$POSTGRESQL_DATABASE" --blobs --format="c" --compress="9" --file="$DBFILE.bak"; then
		echo "[!!ERROR!!] Failed to backup database $POSTGRESQL_DATABASE" 
	else
		echo "Database backup written to $DBFILE.bak"
		
		# cull backups to a limit of NUM_BACKUPS
		find ${BACKUP_DIR}* | grep bak | sort -r | sed "1,${NUM_BACKUPS}d" | xargs rm -rf
		
		# cull backup folders to a limit of NUM_BACKUPS
		find /backups | grep 20 | grep -v bak | grep -v trash | sort -r | sed "1,${NUM_BACKUPS}d" | xargs rm -rf
	fi;
	echo "Current Backups:"
	ls -alh ${BACKUP_DIR}/*/*.bak*
	echo "===================="

	# 24 hrs
	sleep ${BACKUP_PERIOD}

done