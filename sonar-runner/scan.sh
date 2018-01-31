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
cd Common/src/HETSCommon/obj
snyk test --json | snyk-to-html -o $PATHXML/hetscommon.html
cd $PATHXML
cd FrontEnd/src/FrontEnd/obj
snyk test --json | snyk-to-html -o $PATHXML/frontend.html
cd $PATHXML
cd PDF/src/PDF.Server/obj
snyk test --json | snyk-to-html -o $PATHXML/pdfserver.html
cd $PATHXML
cd Server/src/HETSAPI/obj
snyk test --json | snyk-to-html -o $PATHXML/hetsapi.html
cd $PATHXML
cd Client
npm install
snyk test --json | snyk-to-html -o $PATHXML/client.html

