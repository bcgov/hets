FROM tran-hets-tools/server
# Dockerfile for package HETSAPI TEST
 
ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:8080 

WORKDIR /app/Server/src/HETSAPI
# restore packages
RUN dotnet restore

# wipe out the test database
RUN dotnet ef database drop -f

# initialize the test database 
RUN dotnet ef database update 

WORKDIR /app/Server/test
RUN dotnet restore
RUN dotnet test
