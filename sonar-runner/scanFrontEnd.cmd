cd ..
SonarQube.Scanner.MSBuild.exe begin /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-%USERNAME%" /n:"HETS - %USERNAME%" /v:"1.0"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd sonar-runner
