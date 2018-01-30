This example demonstrates how to analyze a simple Java project with Gradle.

Prerequisites
=============
* [SonarQube](http://www.sonarqube.org/downloads/) 6.7+
* Visual Studio
* [SonarQube.Scanner.MSBuild.exe](https://docs.sonarqube.org/display/SCAN/Scanning+on+Windows)
* [Gradle](http://www.gradle.org/) 2.1 or higher

Usage
=====
* Analyze the project with SonarQube using Gradle:

        ./gradlew sonarqube [-Dsonar.host.url=... -Dsonar.jdbc.url=... -Dsonar.jdbc.username=... -Dsonar.jdbc.password=...]
        
Local Scan
=============
To run the scans on your local development machine and report the results on the SonarQube Server you can do the following:

***Running the scan for all content except C# ***
* Change your ```build.gradle``` (this is mandatory, otherwise you will overwrite an existing project):
```
        property "sonar.host.url", "https://sonarqube-tran-hets-tools.pathfinder.gov.bc.ca"
        property "sonar.projectName", "HETS - OTHER - <INSERT YOUR NAME HERE>"
        property "sonar.projectKey", "org.sonarqube:bcgov-hets-other-<INSERT YOUR NAME HERE>"
~~~
* In the sonar-runner directory run: ```gradlew sonarqube```

***Running the scan for all C# content***
* Install ```SonarQube.Scanner.MSBuild.exe```, follow the instructions under "On CI Server" [here](https://docs.sonarqube.org/display/SCAN/Scanning+on+Windows) all but step 6B.
* Update the ```scanDotNet.cmd``` command to point to the right path for MSBUILD. The path should end with ```15.0\Bin\amd64\MSBuild.exe``` otherwise the build will not succeed.
* Be aware that the system is trying to use the Sonarqube.Analysis file that is located in the directory where you installed ```SonarQube.Scanner.MSBuild.exe```.
* 
