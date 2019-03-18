FROM registry.access.redhat.com/rhscl/postgresql-95-rhel7
# This image provides a postgres installation from which to run backups 

# Set the workdir to be root
WORKDIR /

# Load the backup script into the container and make it executable
COPY backup.sh /
# RUN chmod -R a+rwx /backup.sh

# Copy the restore script into the container and make it executable
COPY restore.sh /
chmod a+rwx /restore.sh

# execute a backup
CMD sh /backup.sh

