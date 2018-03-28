-- Table: public."HET_EQUIPMENT_HIST"

DROP TABLE IF EXISTS public."HET_EQUIPMENT_HIST";

CREATE TABLE public."HET_EQUIPMENT_HIST"
(

    "EQUIPMENT_HIST_ID" integer NOT NULL DEFAULT nextval('"HET_EQUIPMENT_HIST_ID_seq"'::regclass),
	"EQUIPMENT_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "APPROVED_DATE" timestamp without time zone,
    "ARCHIVE_DATE" timestamp without time zone,
    "ARCHIVE_REASON" character varying(2048) COLLATE pg_catalog."default",
    "BLOCK_NUMBER" integer,
    "ARCHIVE_CODE" character varying(50) COLLATE pg_catalog."default",
    "DISTRICT_EQUIPMENT_TYPE_ID" integer,
    "EQUIPMENT_CODE" character varying(25) COLLATE pg_catalog."default",
    "LOCAL_AREA_ID" integer,
    "MAKE" character varying(50) COLLATE pg_catalog."default",
    "MODEL" character varying(50) COLLATE pg_catalog."default",
    "OPERATOR" character varying(255) COLLATE pg_catalog."default",
    "OWNER_ID" integer,
    "PAY_RATE" real,
    "RECEIVED_DATE" timestamp without time zone NOT NULL,
    "REFUSE_RATE" character varying(255) COLLATE pg_catalog."default",
    "LICENCE_PLATE" character varying(20) COLLATE pg_catalog."default",
    "SENIORITY" real,
    "SERIAL_NUMBER" character varying(100) COLLATE pg_catalog."default",
    "SIZE" character varying(128) COLLATE pg_catalog."default",
    "STATUS" character varying(50) COLLATE pg_catalog."default",
    "TO_DATE" timestamp without time zone,
    "YEARS_OF_SERVICE" real,
    "SERVICE_HOURS_LAST_YEAR" real,
    "SERVICE_HOURS_THREE_YEARS_AGO" real,
    "SERVICE_HOURS_TWO_YEARS_AGO" real,
    "YEAR" character varying(15) COLLATE pg_catalog."default",
    "LAST_VERIFIED_DATE" timestamp without time zone NOT NULL,
    "INFORMATION_UPDATE_NEEDED_REASON" character varying(2048) COLLATE pg_catalog."default",
    "IS_INFORMATION_UPDATE_NEEDED" boolean,
    "SENIORITY_EFFECTIVE_DATE" timestamp without time zone,
    "IS_SENIORITY_OVERRIDDEN" boolean,
    "SENIORITY_OVERRIDE_REASON" character varying(2048) COLLATE pg_catalog."default",
    "NUMBER_IN_BLOCK" integer,
	"TYPE" character varying(50) COLLATE pg_catalog."default",
    "STATUS_COMMENT" character varying(255) COLLATE pg_catalog."default",
	"LEGAL_CAPACITY" character varying(150) COLLATE pg_catalog."default",
    "LICENCED_GVW" character varying(150) COLLATE pg_catalog."default",
    "PUP_LEGAL_CAPACITY" character varying(150) COLLATE pg_catalog."default",
	"CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 1,
    "DB_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "APP_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_CREATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "DB_CREATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    CONSTRAINT "HET_EQUIPMENT_HIST_PK" PRIMARY KEY ("EQUIPMENT_HIST_ID")
    
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_EQUIPMENT_HIST"
    OWNER to "user6DA";