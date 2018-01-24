cd ..
SonarQube.Scanner.MSBuild.exe begin /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-apispec-%USERNAME%" /n:"HETS - APISpec - %USERNAME%" /v:"1.0"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd sonar-runner
cd ..\FrontEnd
SonarQube.Scanner.MSBuild.exe begin /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-frontend-%USERNAME%" /n:"HETS - FrontEnd - %USERNAME%" /v:"3.0"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd ..\sonar-runner
cd ..\Server
SonarQube.Scanner.MSBuild.exe begin /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-server-%USERNAME%" /n:"HETS - Server - %USERNAME%" /v:"3.0"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd ..\sonar-runner
cd ..\PDF\src\PDF.Server
SonarQube.Scanner.MSBuild.exe begin /d:sonar.url=https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca /k:"org.sonarqube:bcgov-hets-pdf-%USERNAME%" /n:"HETS - PDF - %USERNAME%" /v:"3.0"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd ..\sonar-runner
