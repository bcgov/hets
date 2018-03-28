-- Table: public."HET_RENTAL_AGREEMENT_RATE_HIST"

DROP TABLE IF EXISTS public."HET_RENTAL_AGREEMENT_RATE_HIST";

CREATE TABLE public."HET_RENTAL_AGREEMENT_RATE_HIST"
(
    "RENTAL_AGREEMENT_RATE_HIST_ID" integer NOT NULL DEFAULT nextval('"HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"'::regclass),
	"RENTAL_AGREEMENT_RATE_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "COMMENT" character varying(2048) COLLATE pg_catalog."default",
    "COMPONENT_NAME" character varying(150) COLLATE pg_catalog."default",
    "DB_CREATE_TIMESTAMP" timestamp without time zone NOT NULL,
    "APP_CREATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "IS_ATTACHMENT" boolean,
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone NOT NULL,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50) COLLATE pg_catalog."default",
    "PERCENT_OF_EQUIPMENT_RATE" real,
    "RATE" real,
    "RATE_PERIOD" character varying(50) COLLATE pg_catalog."default",
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
    "IS_INCLUDED_IN_TOTAL" boolean NOT NULL DEFAULT false,
    CONSTRAINT "HET_RENTAL_AGREEMENT_RATE_HIST_PK" PRIMARY KEY ("RENTAL_AGREEMENT_RATE_HIST_ID")

)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."HET_RENTAL_AGREEMENT_RATE_HIST"
    OWNER to "user6DA";