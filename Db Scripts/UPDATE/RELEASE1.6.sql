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


-- HETS-1073 - Allow users to identify "Sets" in the rental agreement
ALTER TABLE public."HET_RENTAL_AGREEMENT_RATE" ADD COLUMN "SET" boolean default false;

-- HETS-1077 - Identify alternate to current PDF engine for generated PDFs'
CREATE SEQUENCE public."HET_BATCH_REPORT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
CREATE TABLE public."HET_BATCH_REPORT"
(
    "REPORT_ID" integer DEFAULT nextval('public."HET_BATCH_REPORT_ID_seq"'::regclass) NOT NULL,
	"REPORT_NAME" character varying(100) COLLATE pg_catalog."default",
    "REPORT_LINK" character varying(500) COLLATE pg_catalog."default",
	"START_DATE" timestamp without time zone,
	"END_DATE" timestamp without time zone,
	"COMPLETE" boolean,
    "APP_CREATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
	"APP_CREATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_CREATE_USERID" character varying(255) COLLATE pg_catalog."default",    
	"APP_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,    
	"APP_LAST_UPDATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",    	
    "APP_LAST_UPDATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,		
	"DB_CREATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
	"DB_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
	"DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "DB_LAST_UPDATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_HET_BATCH_REPORT" PRIMARY KEY ("REPORT_ID")  
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_BATCH_REPORT"
    OWNER to postgres;

GRANT ALL ON TABLE public."HET_BATCH_REPORT" TO postgres;

