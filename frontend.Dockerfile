FROM tran-schoolbus-tools/client
# Dockerfile for the application front end

# compile the client
WORKDIR /opt/app-root/
# copy the full source for the client
COPY Client /opt/app-root/
RUN /bin/bash -c './node_modules/.bin/gulp --production --commit=$OPENSHIFT_BUILD_COMMIT'
