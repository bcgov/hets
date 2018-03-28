-- Table: public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"

DROP TABLE IF EXISTS public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST";

CREATE TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
(
    "RENTAL_REQUEST_ROTATION_LIST_HIST_ID" integer NOT NULL DEFAULT nextval('"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass),
	"RENTAL_REQUEST_ROTATION_LIST_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "ASKED_DATE_TIME" timestamp without time zone,
    "EQUIPMENT_ID" integer,
    "IS_FORCE_HIRE" boolean,
    "NOTE" character varying(2048) COLLATE pg_catalog."default",
    "OFFER_RESPONSE" text COLLATE pg_catalog."default",
    "OFFER_RESPONSE_NOTE" character varying(2048) COLLATE pg_catalog."default",
    "RENTAL_AGREEMENT_ID" integer,
    "RENTAL_REQUEST_ID" integer,
    "ROTATION_LIST_SORT_ORDER" integer NOT NULL,
    "WAS_ASKED" boolean,
    "OFFER_REFUSAL_REASON" character varying(50) COLLATE pg_catalog."default",
    "OFFER_RESPONSE_DATETIME" timestamp without time zone,
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
    CONSTRAINT "HET_RENTAL_REQUEST_ROTATION_LIST_HIST_PK" PRIMARY KEY ("RENTAL_REQUEST_ROTATION_LIST_HIST_ID")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
    OWNER to "user6DA";