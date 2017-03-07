## Legacy Export Files ##

The legacy export contains the following files, in a zip file:
- Area.xml
- Block.xml
- Equip.xml
- Equip_Type.xml
- HETS_Audit.xml
- HETS_Shift.xml
- Owner.xml
- Rotation_Doc.xml
- User_HETS.xml
- Area1.xml
- Dump_Truck.xml
- Equip_Attach.xml
- Equip_Usage.xml
- HETS_City.xml
- List_Resource.xml
- Project.xml
- Service_Area.xml

# Mappings #

## Area ##

| Legacy Field    | New Object | New Field       | Notes |
| ------------    | ---------- | ---------       | ----- |
| Area_Id         | LocalArea  |                 |       |
| Area_Cd         | LocalArea  |                 |       |
| Area_Desc       | LocalArea  | Name            |       |
| Service_Area_Id | LocalArea  | ServiceArea     |       | 
| Created_Dt      | LocalArea  | CreateTimestamp |       |
| Created_By      | LocalArea  | CreateUserid    |       |

## Block ##

| Legacy Field        | New Object | New Field       | Notes |
| ------------        | ---------- | ---------       | ----- |
| Area_Id             |            |                 |       |
| Equip_Type_Id       |            |                 |       |
| Block_Num           |            |                 |       |
| Cycle_Num           |            |                 |       |
| Max_Cycle           |            |                 |       |
| Last_Hired_Equip_Id |            |                 |       |
| Block_Name          |            |                 |       |
| Closed              |            |                 |       |
| Closed_Comments     |            |                 |       |
| Created_Dt          |            |                 |       |
| Created_By          |            |                 |       |

## Equip ##

| Legacy Field        | New Table | New Field          | Notes |
| ------------        | --------- | ---------          | ----- |
| Equip_Id            |           |
| Area_Id             | Equipment | LocalArea 
| Equip_Type_Id       | Equipment | EquipmentType
| Owner_Popt_Id       | Equipment | Owner
| Equip_Cd            | Equipment | EquipmentCode
| Approved_Dt         | Equipment | ApprovedDate
| Received_Dt         | Equipment | ReceivedDate
| Addr1               |           | 
| Addr2               |           | 
| Addr3               |           |
| Addr4               |           |
| City                |           |
| Postal              |           |
| Block_Num           | Equipment | BlockNumber
| Comment             | Equipment |
| Cycle_Hrs_Wrk       | Equipment |
| Frozen_Out          | Equipment | 
| Last_Dt             | Equipment | LastVerifiedDate 
| Licence             | Equipment | LicensePlate
| Make                | Equipment | Make
| Model               | Equipment | Model
| Year                | Equipment | Year
| Type                | Equipment | Type
| Num_Years           | Equipment |
| Operator            | Equipment | Operator
| Pay_Rate            | Equipment | PayRate
| Project_Id          |
| Refuse_Rate         | Equipment | RefuseRate
| Seniority           | Equipment | Seniority
| Serial_Num          | Equipment | SerialNumber
| Size                | Equipment | Size
| Working             | 
| Year_End_Reg        |
| Prev_Reg_Area       |
| YTD                 |
| YTD1                |
| YTD3                |
| Status_Cd           | Equipment | Status
| Archive_Cd          | Equipment | ArchiveCode
| Archive_Reason      | Equipment | ArchiveReason
| Reg_Dump_Trk        | 
| Created_Dt          | Equipment | CreateTimestamp
| Created_By          | Equipment | CreateUserid
| Modified_Dt         |
| Modified_By         |

## EquipAttach ##

| Legacy Field        | New Table           | New Field            | Notes |
| ------------        | ---------           | ---------            | ----- |
| Equip_Id            | Equipment           | EquipmentAttachments |       |
| Attach_Seq_Num      | EquipmentAttachment | Attachment           |       |
| Attach_Desc         | EquipmentAttachment | Description          |       |
| Created_Dt          | EquipmentAttachment | CreateTimestamp      |       |
| Created_By          | EquipmentAttachment | CreateUserid         |       |

## EquipType ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Equip_Type_Id          | 
| SubSystem_Id           | 
| Service_Area_Id        | EquipmentType       | LocalArea
| Equip_Type_Cd          | EquipmentType       | Name
| Equip_Type_Desc        | EquipmentType       | Description
| Equip_Rental_Rate_No   | EquipmentType       | EquipRentalRateNo
| Equip_Rental_Rate_Page | EquipmentType       | EquipRentalRatePage
| Max_Hours              | EquipmentType       | MaxHours
| Extend_Hours           | EquipmentType       | ExtendHours
| Max_Hours_Sub          | EquipmentType       | MaxHoursSub
| Second_Blk             | EquipmentType       | Blocks
| Created_Dt             | EquipmentType       | CreateTimestamp
| Created_Dt             | EquipmentType       | CreateUserid

## EquipUsage ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Equip_Id               | 
| Project_Id             |
| Service_Area_Id        | 
| Worked_Dt              | TimeRecord          | WorkedDate
| Entered_Dt             | TimeRecord          | EnteredDate
| Hours                  | TimeRecord          | Hours
| Rate                   | TimeRecord          | RentalAgreementRate
| Hours2                 | TimeRecord          | Hours
| Rate2                  | TimeRecord          | RentalAgreementRate
| Hours3                 | TimeRecord          | Hours
| Rate3                  | TimeRecord          | RentalAgreementRate
| Created_Dt             | TimeRecord          | CreateTimestamp
| Created_By             | TimeRecord          | CreateUserid

## HETS_Audit ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Created_By             |
| Created_Dt             |
| Action                 |
| Reason                 |

## HETS_City ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Service_Area_Id        |                     |                 |       |
| Seq_Num                |                     |                 |       |
| City                   | City                | Name            |       |


## Owner ##

| Legacy Field             | New Table           | New Field       | Notes |
| ------------             | ---------           | ---------       | ----- |
| Popt_Id                  |
| Area_Id                  | Owner               | LocalArea
| Owner_Cd                 |
| Owner_First_Name         | Owner               | OrganizationName 
| Owner_Last_Name          | Owner               | OrganizationName
| Contact_Person           | Owner               | PrimaryContact
| Local_To_Area            | Owner               | 
| Maintenance_Contractor   | Owner               | IsMaintenanceContractor
| Comment                  | Owner               | Notes
| WCB_Num                  | 
| WCB_Expiry_Dt            |
| CGL_company              |
| CGL_Policy               |
| CGL_Start_Dt             |
| CGL_End_Dt               | Owner               | CGLEndDate
| Status_Cd                | Owner               | Status
| Archive_Cd               | Owner               | ArchiveCode
| Service_Area_Id          | 
| Selected_Service_Area_Id |
| Created_By               | Owner               |
| Created_Dt               | Owner               |
| Modified_By              | Owner               |
| Modified_Dt              | Owner               |

## Project ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Project_Id             |
| Service_Area_Id        | Project             | LocalArea
| Project_Num            | Project             | ProvincialProjectNumber 
| Job_Desc1              | Project             | Name
| Job_Desc2              | Project             | Notes
| Created_Dt             |                     | CreateTimestamp
| Created_By             | 

## Rotation_Doc ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Equip_Id               |
| Note_Dt                |
| Created_Dt             |
| Service_Area_Id        | 
| Project_Id             |
| Note_Type              |
| Reason                 |
| Note_Id                |
| Created_By             |

## Service_Area ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- |
| Service_Area_Id        | 
| Service_Area_Cd        | ServiceArea         | MinistryServiceAreaID
| Service_Area_Desc      | ServiceArea         | Name
| District_Area_Id       | ServiceArea         | District
| Last_Year_End_Shift    |                     |  
| Address                |                     |
| Phone                  |                     |
| Fax                    |                     | 
| FiscalStart            | ServiceArea         | StartDate
| FiscalEnd              |                     |
| Created_Dt             | ServiceArea         |       
| Created_By             | ServiceArea         | 

## User_HETS ##

| Legacy Field           | New Table           | New Field       | Notes |
| ------------           | ---------           | ---------       | ----- | 
| Popt_ID                | 
| Service_Area_Id        | User                | District
| User_Cd                | User                | SmUserId
| User_Cd                | User                | SmAuthorizationDirectory     
| Authority              | User                | UserRoles
| Default_Service_Area   | 
| Created_Dt             | User                |    
| Created_By             | User                |    
| Modified_Dt            | User                |
| Modified_By            | User                | 

# Flow #

- User starts the import process
- User selects the districts to import
- Cities are updated
- Service Areas in that district are updated
- Users in those service areas are imported / updated if the modified date is newer
- Projects in those service areas are imported
- Owners in those service areas are imported
- Equipment Types are updated
- Equipment in the service areas are imported