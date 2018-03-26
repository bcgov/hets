
------------ to create constraints
-- Have to create a unique index first, then the constraint using index
--
alter table "HET_LOCAL_AREA" drop constraint if exists "HET_LOCA_LOCAL_AREA_NUMBER_UK";
drop index if exists "HET_LOCA_LOCAL_AREA_NUMBER_UK";

CREATE UNIQUE INDEX "HET_LOCA_LOCAL_AREA_NUMBER_UK"
    ON public."HET_LOCAL_AREA" 
    ("LOCAL_AREA_NUMBER")
    TABLESPACE pg_default;
    
alter table "HET_LOCAL_AREA" 
add constraint "HET_LOCA_LOCAL_AREA_NUMBER_UK" UNIQUE using index "HET_LOCA_LOCAL_AREA_NUMBER_UK"
;


alter table "HET_USER" drop constraint if exists "HET_USR_GUID_UK";
drop index if exists "HET_USR_GUID_UK";

CREATE UNIQUE INDEX "HET_USR_GUID_UK"
    ON public."HET_USER" 
    ("GUID")
    TABLESPACE pg_default;

alter table "HET_USER" 
add constraint "HET_USR_GUID_UK" UNIQUE using index "HET_USR_GUID_UK"
;

alter table "HET_ROLE" drop constraint if exists "HET_ROLE_NAME_UK";
drop index if exists "HET_ROLE_NAME_UK";

CREATE UNIQUE INDEX "HET_ROLE_NAME_UK"
    ON public."HET_ROLE" 
    ("NAME")
    TABLESPACE pg_default;

alter table "HET_ROLE" 
add constraint "HET_ROLE_NAME_UK" UNIQUE using index "HET_ROLE_NAME_UK"
;

alter table "HET_PERMISSION" drop constraint if exists "HET_PRM_CODE_UK";
drop index if exists "HET_PRM_CODE_UK";

CREATE UNIQUE INDEX "HET_PRM_CODE_UK"
    ON public."HET_PERMISSION" 
    ("CODE")
    TABLESPACE pg_default;

alter table "HET_PERMISSION" 
add constraint "HET_PRM_CODE_UK" UNIQUE using index "HET_PRM_CODE_UK"
;

alter table "HET_PERMISSION" drop constraint if exists "HET_PRM_NAME_UK";
drop index if exists "HET_PRM_NAME_UK";

CREATE UNIQUE INDEX "HET_PRM_NAME_UK"
    ON public."HET_PERMISSION" 
    ("NAME")
    TABLESPACE pg_default;

alter table "HET_PERMISSION" 
add constraint "HET_PRM_NAME_UK" UNIQUE using index "HET_PRM_NAME_UK"
;
/*
-- the table below fails in DEV due to duplicates
-- as per these select statements, the duplicates are due to empty strings. There are no nulls (nulls would not cause a problem if they were there anyway)

select '--'||"REGISTERED_COMPANY_NUMBER"||'--' from "HET_OWNER" c where exists (select 1 from "HET_OWNER" c2 where c."REGISTERED_COMPANY_NUMBER" = c2."REGISTERED_COMPANY_NUMBER" and c."OWNER_ID" != c2."OWNER_ID") order by 1

select count (*) from "HET_OWNER" c where "REGISTERED_COMPANY_NUMBER" is null;
*/

alter table "HET_OWNER" drop constraint if exists "HET_OWN_REGISTERED_COMPANY_NUMBER_UK";
drop index if exists "HET_OWN_REGISTERED_COMPANY_NUMBER_UK";

CREATE UNIQUE INDEX "HET_OWN_REGISTERED_COMPANY_NUMBER_UK"
    ON public."HET_OWNER" 
    ("REGISTERED_COMPANY_NUMBER")
    TABLESPACE pg_default;

alter table "HET_PERMISSION" 
add constraint "HET_OWN_REGISTERED_COMPANY_NUMBER_UK" UNIQUE using index "HET_OWN_REGISTERED_COMPANY_NUMBER_UK"
;

/*
-- the table below fails in DEV due to duplicates
-- as per these select statements, the duplicates are due to empty strings AND real duplicates. 

select '--'||"WORK_SAFE_BCPOLICY_NUMBER"||'--' from "HET_OWNER" c where exists (select 1 from "HET_OWNER" c2 where c."WORK_SAFE_BCPOLICY_NUMBER" = c2."WORK_SAFE_BCPOLICY_NUMBER" and c."OWNER_ID" != c2."OWNER_ID") order by 1

select count (*) from "HET_OWNER" c where "WORK_SAFE_BCPOLICY_NUMBER" is null;
*/


alter table "HET_OWNER" drop constraint if exists "HET_OWN_WORK_SAFE_BCPOLICY_NUMBER_UK";
drop index if exists "HET_OWN_WORK_SAFE_BCPOLICY_NUMBER_UK";

CREATE UNIQUE INDEX "HET_OWN_WORK_SAFE_BCPOLICY_NUMBER_UK"
    ON public."HET_OWNER" 
    ("WORK_SAFE_BCPOLICY_NUMBER")
    TABLESPACE pg_default;

alter table "HET_OWNER" 
add constraint "HET_OWN_WORK_SAFE_BCPOLICY_NUMBER_UK" UNIQUE using index "HET_OWN_WORK_SAFE_BCPOLICY_NUMBER_UK"
;

/*
-- the table below fails in DEV due to duplicates
-- as per these select statements, the duplicates are due to empty strings AND real duplicates. 

select '--'||"NUMBER"||'--' from "HET_RENTAL_AGREEMENT" c where exists (select 1 from "HET_RENTAL_AGREEMENT" c2 where c."NUMBER" = c2."NUMBER" and c."RENTAL_AGREEMENT_ID" != c2."RENTAL_AGREEMENT_ID") order by 1

select count (*) from "HET_RENTAL_AGREEMENT" c where "NUMBER" is null;
*/

alter table "HET_RENTAL_AGREEMENT" drop constraint if exists "HET_RNTAG_NUMBER_UK";
drop index if exists "HET_RNTAG_NUMBER_UK";

CREATE UNIQUE INDEX "HET_RNTAG_NUMBER_UK"
    ON public."HET_RENTAL_AGREEMENT" 
    ("NUMBER")
    TABLESPACE pg_default;

alter table "HET_RENTAL_AGREEMENT" 
add constraint "HET_RNTAG_NUMBER_UK" UNIQUE using index "HET_RNTAG_NUMBER_UK"
;



