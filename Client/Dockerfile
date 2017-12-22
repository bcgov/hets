FROM dotnet-20-rhel7:latest

# Dockerfile for HETS Client

# install rlwrap
USER 0
RUN yum install -y readline-devel bzip2  && \
    yum clean all -y
	
USER 1001
# copy Node.js packages

COPY /package.json /app/out/src/
WORKDIR /app/out/src

ENV NODE_ENV production

RUN npm install

# compile will happen at the next build stage (frontend)
# otherwise there is no git commit in the environment.

