PATHXML=$(pwd -P)
DT=$(TZ='America/Vancouver' date +%Y%m%d)
export JAVA_HOME="/usr/lib/jvm/java-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64"
cd ..
mono /usr/lib/sonar-scanner/SonarQube.Scanner.MSBuild.exe begin /s:$PATHXML/SonarQube.Analysis.xml /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-pipeline-dotnet" /n:"HETS - Pipeline DOTNET" /v:"$DT.$BUILD_NUMBER"
dotnet restore
dotnet build
dotnet test
mono /usr/lib/sonar-scanner/SonarQube.Scanner.MSBuild.exe end

PATHXML=$(pwd -P)
