FROM registry.access.redhat.com/rhscl/postgresql-95-rhel7
# This image provides a postgres installation from which to run backups 

# Set the workdir to be root
WORKDIR /

# Load the backup script into the container
COPY backup.sh /

# Copy the restore script into the container [sh /restore.sh]
COPY restore.sh /backups/restore
COPY connections.sql /backups/restore

# execute a backup
CMD sh /backup.sh

