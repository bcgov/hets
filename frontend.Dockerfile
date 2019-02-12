FROM tran-hets-tools/client
# Dockerfile for the application front end

# Switch to root for package installs
USER 0

# compile the client
WORKDIR /opt/app-root/

# copy the full source for the client
COPY Client /opt/app-root/

# Install newer version of Node 
ENV NVM_DIR /usr/local/nvm
ENV NODE_VERSION  v10.13.0

RUN touch ~/.bash_profile \
    && curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.33.6/install.sh | bash \
    && . $NVM_DIR/nvm.sh \
    && nvm ls-remote \
    && nvm install $NODE_VERSION \
    && nvm alias default $NODE_VERSION \
    && nvm use default \
    && npm install -g autorest
	
RUN pwd

RUN ls -la

   
# build the client app   
RUN /bin/bash -c './node_modules/.bin/gulp --production --commit=$OPENSHIFT_BUILD_COMMIT'   
   
# modify 
RUN chown -R 1001:0 /opt/app-root/ && fix-permissions /opt/app-root/

User 1001