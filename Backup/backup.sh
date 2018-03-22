#!/bin/bash
# Postgresql automated backup script
# See README.md for documentation on this script

FINAL_BACKUP_DIR=$BACKUP_DIR"`date +\%Y-\%m-\%d`/"
DBFILE=$FINAL_BACKUP_DIR"$POSTGRESQL_DATABASE`date +\%Y-\%m-\%d-%H-%M`"
echo "Making backup directory in $FINAL_BACKUP_DIR"
 
if ! mkdir -p $FINAL_BACKUP_DIR; then
	echo "Cannot create backup directory in $FINAL_BACKUP_DIR." 1>&2
	exit 1;
fi;

export PGPASSWORD=$POSTGRESQL_PASSWORD

while true; do

	if ! pg_dump -Fp -h "$DATABASE_SERVICE_NAME" -U "$POSTGRESQL_USER" "$POSTGRESQL_DATABASE" | gzip > $DBFILE.sql.gz.in_progress; then
		echo "[!!ERROR!!] Failed to backup database $POSTGRESQL_DATABASE" 
	else
		mv $DBFILE.sql.gz.in_progress $DBFILE.sql.gz
		echo "Database backup written to $DBFILE.sql.gz"
		
		# cull backups older than 31 days.  (SB-331)
		find $BACKUP_DIR* -type d -ctime +31 | xargs rm -rf
	fi;

	# 24 hrs
	sleep 1d

done