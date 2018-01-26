PATHXML=$(pwd -P)
DT=$(TZ='America/Vancouver' date +%Y%m%d)
cd ..
mono /usr/lib/sonar-scanner/SonarQube.Scanner.MSBuild.exe begin /s:$PATHXML/SonarQube.Analysis.xml /n:"HETS - Pipeline DOTNET" /v:"$DT.$BUILD_NUMBER"
dotnet restore
dotnet build
dotnet test
mono /usr/lib/sonar-scanner/SonarQube.Scanner.MSBuild.exe end /s:$PATHXML/SonarQube.Analysis.xml
