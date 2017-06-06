@ECHO OFF

SET dev=http://server-tran-hets-dev.pathfinder.gov.bc.ca
SET test=http://server-tran-hets-test.pathfinder.gov.bc.ca
SET prod=http://server-tran-hets-prod.pathfinder.gov.bc.ca

IF %3.==. GOTO USAGE

SET server=%3
IF %3==dev SET server=%dev%
IF %3==test SET server=%test%
IF %3==prod SET server=%prod%

REM curl -c cookie %server%/api/authentication/dev/token?userId=SCURRAN
REM curl -c cookie %server%/api/authentication/dev/token?userId=TMcTesterson
echo on
curl -c cookie %server%/api/authentication/dev/token/SCURRAN
IF %1==recalc GOTO RECALC

curl -b cookie -v -H "Content-Type: application/json" -X POST --data-binary "@%1" %server%/%2
@echo off

GOTO End1

:RECALC
curl -b cookie -v %server%/api/equipment/recalcSeniority?region=%2

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
