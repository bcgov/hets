Postgres Backup/Restore in OpenShift
----------------
An OpenShift Deployment called "backup" in the HETS projects (Dev, Test, Prod) runs the backups of the Postgres database. The following are the instructions for running the backups and a restore.

Deployment / Configuration
----------------
The OpenShift Deployment Template for this application will automatically deploy the Backup app as described below.

The following environment variables are used by the Backup app.

**NOTE**: THESE ENVIRONMENT VARIABLES MUST MATCH THE VARIABLES USED BY THE postgresql DEPLOYMENT DESCRIPTOR.

| Name | Purpose |
| ---- | ------- |
| DATABASE_SERVICE_NAME | hostname for the database to backup |
| POSTGRESQL_USER | database user for the backup |
| POSTGRESQL_PASSWORD | database password for the backup |
| POSTGRESQL_DATABASE | database to backup | 
| BACKUP_DIR | directory to store the backups |
| RESTORE_DIR | directory to store the backup to be restored |

The BACKUP_DIR & RESTORE_DIR must be set to a location that has persistent storage.

Backup
------
The purpose of the backup app is to do automatic backups.  Deploy the Backup app to do daily backups.  Viewing the Logs for the Backup App will show a record of backups that have been completed.

The Backup app performs the following sequence of operations:

1. Create a directory that will be used to store the backup.
2. Use the `pg_dump` and `gzip` commands to make a backup.
3. Cull backups more than $NUM_BACKUPS (default 31 - configured in deployment script)
4. Sleep for a day and repeat

Note that we are just using a simple "sleep" to run the backup periodically. More elegent solutions were looked at briefly, but there was not a lot of time or benefit, so OpenShift Scheduled Jobs, cron and so on are not used. With some more effort they likely could be made to work.

A separate pod is used vs. having the backups run from the Postgres Pod for fault tolerent purposes - to keep the backups separate from the database storage.  We don't want to, for example, lose the storage of the database, or have the database and backups storage fill up, and lose both the database and the backups.

Immediate Backup:
-----------------
To execute a backup right now, check the logs of the Backup pod to make sure a backup isn't run right now (pretty unlikely...), and then deploy the "backup" using OpenShift "deploy" capabilities.


Restore
-------
These steps perform a restore of a backup. This process must be executed from the postgres pod.

1. Log into the OpenShift Console and log into OpenShift on the command shell window
2. Scale to 0 all Apps that use the database connection.
   1. This is necessary as the Apps will need to restart to pull data from the restored backup.
   2. In HETS this is just **server**
   3. It is recommended that you also scale down to 0 **frontend** so that users know the application is unavailable while the database restore is underway.       
3. Restart the **postgres** pod as a quick way of closing any other database connections from users using port forward or that have rsh'd to directly connect to the database.
4. Open a Terminal window on the **postgres** pod
   1. locate the backup to restore (change to the directory)
   
       cd /backups/2019-03-18
	   
    2. execute the script identifying the file to resore (example below):
	
       sh /backups/restore/restore.sh 2019-03-18/hets2019-03-18-20-55.bak
   
5. From the Openshift Console restart the app:
    1. Scale up the Server app and wait for it to finish starting up.  View the logs for the Server app to verify there were no startup issues.
    2. Scale up the FrontEnd app and wait for it to finish starting up.  View the logs for the Server app to verify there were no startup issues.
6.  Verify full application functionality.

Done!
