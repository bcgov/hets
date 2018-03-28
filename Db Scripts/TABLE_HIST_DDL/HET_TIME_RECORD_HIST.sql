-- Table: public."HET_TIME_RECORD_HIST"

DROP TABLE IF EXISTS public."HET_TIME_RECORD_HIST";

CREATE TABLE public."HET_TIME_RECORD_HIST"
(
    "TIME_RECORD_HIST_ID" integer NOT NULL DEFAULT nextval('"HET_TIME_RECORD_HIST_ID_seq"'::regclass),
	"TIME_RECORD_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "ENTERED_DATE" timestamp without time zone,
    "HOURS" real,
    "RENTAL_AGREEMENT_RATE_ID" integer,
    "WORKED_DATE" timestamp without time zone NOT NULL,
    "TIME_PERIOD" character varying(20) COLLATE pg_catalog."default",
    "DB_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "RENTAL_AGREEMENT_ID" integer,
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 1,
	"APP_CREATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_CREATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_CREATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
    "APP_LAST_UPDATE_USER_GUID" character varying(255) COLLATE pg_catalog."default",
    "APP_LAST_UPDATE_USERID" character varying(255) COLLATE pg_catalog."default",
    "DB_CREATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    "DB_LAST_UPDATE_USER_ID" character varying(63) COLLATE pg_catalog."default",
    CONSTRAINT "HET_TIME_RECORD_HIST_PK" PRIMARY KEY ("TIME_RECORD_HIST_ID")
   
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_TIME_RECORD_HIST"
    OWNER to "user6DA";