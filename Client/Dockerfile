FROM dotnet-20-rhel7:latest

# Dockerfile for HETS Client

# install rlwrap
USER 0
RUN yum install -y readline-devel bzip2  && \
    yum clean all -y

# install Node.js
ENV NVM_DIR /usr/local/nvm
ENV NODE_VERSION  v8.9.1
	
RUN  rm -R /opt/rh/rh-nodejs6 \
    && mkdir /opt/rh/rh-nodejs6 \
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
    && yum install -y curl \
    && yum clean all -y \
    && mkdir -p /opt/app-root \
    && chmod -R a+rwx /opt/app-root \
    && chown -R 1001:0 /opt/app-root && fix-permissions /opt/app-root

# Run container by default as user with id 1001 (default)
USER 1001

env PATH "$PATH:/usr/local/nvm/versions/node/v8.9.1/bin/" 	

# copy Node.js packages

COPY /package.json /app/out/src/
WORKDIR /app/out/src

ENV NODE_ENV production

RUN npm install

# compile will happen at the next build stage (frontend)
# otherwise there is no git commit in the environment.

