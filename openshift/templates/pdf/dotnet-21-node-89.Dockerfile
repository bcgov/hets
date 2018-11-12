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

# Install wget
RUN yum -y --setopt=tsflags=nodocs install wget && \
    yum clean all -y
	
# Install epel-release	
RUN wget https://dl.fedoraproject.org/pub/epel/epel-release-latest-7.noarch.rpm	&& \
    yum -y install epel-release-latest-7.noarch.rpm && \   
    yum clean all -y
		
# Install libfontconfig
RUN yum install -y fontconfig freetype freetype-devel fontconfig-devel libstdc++ && \
    yum clean all -y

# Install liberation-fonts
RUN yum -y install liberation-* && \    
    yum clean all -y	
		
# Install cabextract
RUN wget http://ftp.tu-chemnitz.de/pub/linux/dag/redhat/el7/en/x86_64/rpmforge/RPMS/cabextract-1.4-1.el7.rf.x86_64.rpm && \
    yum -y install cabextract-1.4-1.el7.rf.x86_64.rpm && \    
    yum clean all -y		

# Install xorg-x11-fonts
RUN yum -y install xorg-x11-font-utils fontpackages-filesystem ipa-gothic-fonts && \
    yum -y install xorg-x11-fonts-100dpi xorg-x11-fonts-75dpi xorg-x11-fonts-misc && \
    yum -y install xorg-x11-fonts-Type1 xorg-x11-utils && \
    yum clean all -y	

# Install microsoft fonts	
RUN wget https://downloads.sourceforge.net/project/mscorefonts2/rpms/msttcore-fonts-installer-2.6-1.noarch.rpm && \
	yum -y install ./msttcore-fonts-installer-2.6-1.noarch.rpm && \
    yum clean all -y			

# Install additional chrome dependencies
RUN yum -y install adwaita-cursor-theme adwaita-icon-theme alsa-lib at at-spi2-atk at-spi2-core atk && \
    yum -y install avahi-libs bc cairo cairo-gobject colord-libs cups-client cups-libs && \
    yum -y install dconf desktop-file-utils ed emacs-filesystem file file-libs fribidi && \
	yum -y install gdk-pixbuf2 glib-networking glib2 gnutls graphite2 gsettings-desktop-schemas && \
	yum -y install gtk-update-icon-cache gtk3 harfbuzz hicolor-icon-theme hostname jasper-libs && \
	yum -y install jbigkit-libs json-glib lcms2 libXScrnSaver libXcomposite libXcursor libXft && \
	yum -y install libappindicator-gtk3 libdbusmenu libdbusmenu-gtk3 libepoxy libgusb libindicator-gtk3 && \
	yum -y install libjpeg-turbo libmodman libpipeline libproxy libsoup libthai libtiff libusbx && \
	yum -y install libwayland-cursor libwayland-egl libxkbcommon m4 mailx make man-db && \	
	yum -y install mariadb-libs nettle pango patch pixman postfix psmisc redhat-lsb-core && \
	yum -y install redhat-lsb-submod-security rest spax systemd-sysv sysvinit-tools && \
	yum -y install time trousers xdg-utils xkeyboard-config && \	
    yum clean all -y	
				
# Install libstdc++6
RUN yum -y install libstdc++.so.6 && \
    yum clean all -y	
				
# Install chrome
RUN wget http://orion.lcg.ufrj.br/RPMS/myrpms/google/google-chrome-stable-56.0.2924.76-1.x86_64.rpm && \
    rpm -ih --nodeps ./google-chrome-stable-56.0.2924.76-1.x86_64.rpm && \
    yum clean all -y				
	
ENV chrome:launchOptions:args --no-sandbox	
	
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

RUN rm -rf /var/cache/yum

RUN chmod -R a+rwx /usr/local/nvm
RUN mkdir -p /opt/app-root
RUN chmod -R a+rwx /opt/app-root
RUN chown -R 1001:0 /opt/app-root && fix-permissions /opt/app-root

RUN echo $PATH | sed -e 's|/opt/rh/rh-nodejs8/root/usr/bin:||g'

# Run container by default as user with id 1001 (default)
USER 1001

env PATH "$PATH:/usr/local/nvm/versions/node/v10.13.0/bin" 

# Directory with the sources is set as the working directory.
WORKDIR /opt/app-root/src

# Set the default CMD to print the usage of the language image.
CMD /usr/libexec/s2i/usage

# Display installed versions
RUN google-chrome-stable --version
RUN node --version
RUN npm --version