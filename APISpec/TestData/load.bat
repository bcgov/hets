@ECHO OFF

SET dev=http://server-tran-hets-dev.pathfinder.gov.bc.ca
SET test=http://server-tran-hets-test.pathfinder.gov.bc.ca

IF %3.==. GOTO USAGE

SET server=%3
IF %3==dev SET server=%dev%
IF %3==test SET server=%test%

REM curl -c cookie %server%/api/authentication/dev/token?userId=SCURRAN
curl -c cookie %server%/api/authentication/dev/token/SCURRAN
curl -b cookie -v -H "Content-Type: application/json" -X POST --data-binary "@%1" %server%/%2

GOTO End1

:USAGE
ECHO Incorrect syntax
ECHO USAGE load.bat ^<JSON filename^> ^<endpoint^> ^<server URL^>
ECHO Example: load.bat regions.json api/regions dev
ECHO Where server URL is one of dev, test or a full URL
ECHO Note: Do not put a / before the endpoint
ECHO The dev server is: %dev%
ECHO The dev server is: %test%

:End1
