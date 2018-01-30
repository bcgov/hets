set datetimef=%time:~0,2%%time:~3,2%%time:~6,2%
cd ..
SonarQube.Scanner.MSBuild.exe begin /k:"org.sonarqube:bcgov-hets-%USERNAME%" /n:"HETS - DOTNET - %USERNAME%" /v:"%date%.%datetimef%"
"D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe" /t:Rebuild
SonarQube.Scanner.MSBuild.exe end
cd sonar-runner
