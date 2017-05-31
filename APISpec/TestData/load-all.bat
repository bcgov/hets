echo off

IF %1.==. GOTO USAGE

REM Note - order matters in the loading of the data

REM The following data will be loaded on all systems - Dev/Test/Prod
call load.bat "cities\cities_city.json" api/cities/bulk %1
call load.bat "ServiceAreas\ServiceAreas_ServiceArea.json" api/serviceareas/bulk %1
call load.bat "EquipmentTypes\EquipmentTypes_EquipmentType.json" api/equipmenttypes/bulk %1

IF %1.==test. GOTO TestProd
IF %1.==prod. GOTO TestProd

REM The rest of the tables are only loaded into Dev and other servers
call load.bat "LocalAreas\LocalAreas_LocalArea.json" api/localareas/bulk %1
call load.bat "Contacts\Contacts_Contact.json" api/contacts/bulk %1
call load.bat "Owners\Owners_Owner.json" api/owners/bulk %1
call load.bat "DistrictEquipmentTypes\DistrictEquipmentTypes_DistrictEquipmentType.json" api/districtEquipmentTypes/bulk %1
call load.bat "Equipment\Equipment_Equipment.json" api/equipment/bulk %1
call load.bat "Project\Project_Project.json" api/projects/bulk %1
call load.bat "RentalRequest\RentalRequest_RentalRequest.json" api/rentalrequests/bulk %1

REM 
REM // update the seniority scores.

call load.bat recalc 1 %1
call load.bat recalc 2 %1
call load.bat recalc 3 %1

GOTO End1

:USAGE

echo Incorrect syntax
echo USAGE: %0 ^<dev^|test^|prod^|server^>
echo Example: %0 dev
echo Example: %0 http://localhost:55217

goto End1

:TestProd
echo NOTE: Only reference tables are loaded into Test and Prod

:End1
