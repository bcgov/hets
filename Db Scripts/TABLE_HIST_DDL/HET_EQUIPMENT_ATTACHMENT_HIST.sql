-- Table: public."HET_EQUIPMENT_ATTACHMENT_HIST"

DROP TABLE IF EXISTS public."HET_EQUIPMENT_ATTACHMENT_HIST";

CREATE TABLE public."HET_EQUIPMENT_ATTACHMENT_HIST"
(
    "EQUIPMENT_ATTACHMENT_HIST_ID" integer NOT NULL DEFAULT nextval('"HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"'::regclass),
	"EQUIPMENT_ATTACHMENT_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "DESCRIPTION" character varying(2048) COLLATE pg_catalog."default",
    "EQUIPMENT_ID" integer,
    "DB_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "TYPE_NAME" character varying(100) COLLATE pg_catalog."default",
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 1,
	"APP_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_CREATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "DB_CREATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    CONSTRAINT "HET_EQUIPMENT_ATTACHMENT_HIST_PK" PRIMARY KEY ("EQUIPMENT_ATTACHMENT_HIST_ID")
   
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_EQUIPMENT_ATTACHMENT_HIST"
    OWNER to "user6DA";
