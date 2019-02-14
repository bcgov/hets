SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;


--HETS-1070 - Remove conversion from user roles
DELETE FROM public."HET_USER_ROLE"
WHERE "ROLE_ID" IN
(
    SELECT "ROLE_ID"
    FROM public."HET_ROLE"
    WHERE "NAME" = '5-Data Conversion'
);

DELETE FROM public."HET_ROLE_PERMISSION"
WHERE "ROLE_ID" IN
(
    SELECT "ROLE_ID"
    FROM public."HET_ROLE"
    WHERE "NAME" = '5-Data Conversion'
);

DELETE FROM public."HET_ROLE_PERMISSION"
WHERE "PERMISSION_ID" IN
(
    SELECT "PERMISSION_ID"
    FROM public."HET_PERMISSION"
    WHERE "CODE" = 'ImportData'
);

DELETE FROM public."HET_PERMISSION"
WHERE "CODE" = 'ImportData';

DELETE FROM public."HET_ROLE"
WHERE "NAME" = '5-Data Conversion';

UPDATE public."HET_ROLE"
SET "NAME" = '5-Business BCeID User',
    "CONCURRENCY_CONTROL_NUMBER" = "CONCURRENCY_CONTROL_NUMBER" + 1
WHERE "NAME" = '6-Business BCeID User';