FROM tran-hets-tools/server
# Dockerfile for package HETSAPI TEST

RUN apt-get update \
 && apt-get upgrade -y --force-yes \
 && apt-get -y --force-yes install postgresql-9.4 postgresql-client-9.4 \
 && rm -rf /var/lib/apt/lists/*;

# create database and user.

RUN createdb hets
RUN createuser -s hets 
RUN postgres --command 'CREATE ROLE hets WITH LOGIN ENCRYPTED PASSWORD 'hets';'
RUN postgres --command 'GRANT ALL PRIVILEGES ON DATABASE hets TO hets;'
 
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:8080 
ENV DATABASE_SERVICE_NAME localhost
ENV POSTGRESQL_USER hets
ENV POSTGRESQL_PASSWORD hets
ENV POSTGRESQL_DATABASE hets
 
WORKDIR /app/Server/src/HETSAPI/
RUN dotnet test
