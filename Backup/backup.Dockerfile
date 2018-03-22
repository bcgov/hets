FROM registry.access.redhat.com/rhscl/postgresql-95-rhel7
# This image provides a postgres installation from which to run backups 

# Set the workdir to be root
WORKDIR /

# Load the backup script into the container and make it executable
COPY backup.sh /
# RUN chmod -R a+rwx /backup.sh

# Set the default CMD to print the usage of the language image.
CMD sh /backup.sh

