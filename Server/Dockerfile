# Build context: Server/
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:6ba9644da07bb5b560a402acec9ca38711a0762969f1b61da261df5c9f6f85da AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY HetsApi/HetsApi.csproj HetsApi/packages.lock.json HetsApi/
COPY HetsCommon/HetsCommon.csproj HetsCommon/packages.lock.json HetsCommon/
COPY HetsData/HetsData.csproj HetsData/packages.lock.json HetsData/
COPY HetsReport/HetsReport.csproj HetsReport/packages.lock.json HetsReport/
RUN dotnet restore HetsApi/HetsApi.csproj --locked-mode

COPY . .
RUN dotnet publish HetsApi/HetsApi.csproj --configuration Release \
    --no-restore --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:ea721098d6ea2894ff9bd8387821a1659d4765d6c014b8d648bb1da16f016f5a AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
COPY container-entrypoint.sh /usr/local/bin/hets-api-start

# OpenShift assigns an arbitrary UID in group 0. The mounted configuration
# replaces the published defaults, matching the previous S2I startup script.
RUN mkdir -p /opt/app-root/configmap \
    && chgrp -R 0 /app /opt/app-root \
    && chmod -R g=u /app /opt/app-root \
    && chmod 755 /usr/local/bin/hets-api-start
USER 1001:0
EXPOSE 8080
ENTRYPOINT ["/usr/local/bin/hets-api-start"]
