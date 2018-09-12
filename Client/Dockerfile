FROM dotnet-20-rhel7:latest

# Dockerfile for HETS Client

# install rlwrap
USER 0
RUN yum install -y readline-devel bzip2 git && \
    yum clean all -y

# install Node.js
ENV NVM_DIR /usr/local/nvm
ENV NODE_VERSION  v8.9.1
	
RUN mkdir /opt/rh/rh-nodejs6 \
    && touch /opt/rh/rh-nodejs6/enable \
    && chmod a+rx /opt/rh/rh-nodejs6/enable \
    && touch ~/.bash_profile \
    && curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.33.6/install.sh | bash \
    && . $NVM_DIR/nvm.sh \
    && nvm ls-remote \
    && nvm install $NODE_VERSION \
    && nvm alias default $NODE_VERSION \
    && nvm use default \
    && chmod -R a+rwx /usr/local/nvm \    
    && mkdir -p /app/out/src/ \
    && chmod -R a+rwx /app/out/src/ \
    && chown -R 1001:0 /app/out/src/ && fix-permissions /app/out/src/

# Run container by default as user with id 1001 (default)
USER 1001

env PATH "$PATH:/usr/local/nvm/versions/node/v8.9.1/bin/" 	

# copy Node.js packages

COPY /package.json /opt/app-root/
WORKDIR /opt/app-root/

ENV NODE_ENV production

# compile will happen at the next build stage (frontend)
# otherwise there is no git commit in the environment.

