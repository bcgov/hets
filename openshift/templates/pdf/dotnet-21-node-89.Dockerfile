FROM registry.access.redhat.com/dotnet/dotnet-21-rhel7
# This image provides a .NET Core 2.1 and upgraded Node environment you can use 
# to run your .NET applications.

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# This setting is a workaround for issues with dotnet and certain docker versions
ENV LTTNG_UST_REGISTER_TIMEOUT 0

# Switch to root for package installs
USER 0

# Install git
RUN yum install -y bzip2 git && \
    yum clean all -y

# Install libfontconfig
RUN yum install -y fontconfig freetype freetype-devel fontconfig-devel libstdc++ && \
    yum clean all -y

# Install liberation-fonts
RUN yum -y install liberation-* && \
    yum -y install mstcore-fonts-2.0-3.noarch.rpm
    yum clean all -y		
	
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
	
RUN yum -y --setopt=tsflags=nodocs install wget && \
    wget https://dl.google.com/linux/direct/google-chrome-stable_current_x86_64.rpm && \
    yum -y install ./google-chrome-stable_current_x86_64.rpm && \
    yum clean all -y		

ENV chrome:launchOptions:args --no-sandbox	

RUN rm -rf /var/cache/yum

RUN chmod -R a+rwx /usr/local/nvm
RUN mkdir -p /opt/app-root
RUN chmod -R a+rwx /opt/app-root
RUN chown -R 1001:0 /opt/app-root && fix-permissions /opt/app-root

RUN sed -i -e 's+/opt/rh/rh-nodejs8/root/usr/bin:/opt/rh/rh-dotnet21/root/usr/bin:+ ' $PATH

# Run container by default as user with id 1001 (default)
USER 1001

env PATH "$PATH:/usr/local/nvm/versions/node/v10.13.0/bin" 

# Directory with the sources is set as the working directory.
WORKDIR /opt/app-root/src

# Set the default CMD to print the usage of the language image.
CMD /usr/libexec/s2i/usage

# Display installed versions
RUN google-chrome --version
RUN node --version
RUN npm --version