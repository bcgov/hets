SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: HET_DIGITAL_FILE_DIGITAL_FILE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_DIGITAL_FILE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_ATTACHMENT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_DIGITAL_FILE" (
    "DIGITAL_FILE_ID" integer DEFAULT nextval('public."HET_DIGITAL_FILE_ID_seq"'::regclass) NOT NULL,
    "DESCRIPTION" character varying(2048),
    "FILE_NAME" character varying(2048),
    "TYPE" character varying(255),
    "MIME_TYPE_ID" integer NOT NULL,
    "FILE_CONTENTS" bytea,
    "EQUIPMENT_ID" integer,
    "OWNER_ID" integer,
    "PROJECT_ID" integer,
    "RENTAL_REQUEST_ID" integer,
    "APP_CREATE_USER_DIRECTORY" character varying(50),
    "APP_CREATE_USER_GUID" character varying(255),
    "APP_CREATE_USERID" character varying(255),
    "APP_CREATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50),
    "APP_LAST_UPDATE_USER_GUID" character varying(255),
    "APP_LAST_UPDATE_USERID" character varying(255),
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_CREATE_USER_ID" character varying(63),
    "DB_CREATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_LAST_UPDATE_USER_ID" character varying(63),
    "CONCURRENCY_CONTROL_NUMBER" integer DEFAULT 0 NOT NULL
);

--
--
-- Name: HET_CONDITION_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_CONDITION_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_CONDITION_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_CONDITION_TYPE" (
    "CONDITION_TYPE_ID" integer DEFAULT nextval('public."HET_CONDITION_TYPE_ID_seq"'::regclass) NOT NULL,
    "DISTRICT_ID" integer,
    "CONDITION_TYPE_CODE" character varying(20),
    "DESCRIPTION" character varying(2048),
    "ACTIVE" boolean NOT NULL,
    "APP_CREATE_USER_DIRECTORY" character varying(50),
    "APP_CREATE_USER_GUID" character varying(255),
    "APP_CREATE_USERID" character varying(255),
    "APP_CREATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "APP_LAST_UPDATE_USER_DIRECTORY" character varying(50),
    "APP_LAST_UPDATE_USER_GUID" character varying(255),
    "APP_LAST_UPDATE_USERID" character varying(255),
    "APP_LAST_UPDATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_CREATE_USER_ID" character varying(63),
    "DB_CREATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_LAST_UPDATE_TIMESTAMP" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
    "DB_LAST_UPDATE_USER_ID" character varying(63),
    "CONCURRENCY_CONTROL_NUMBER" integer DEFAULT 0 NOT NULL
);

--
-- Name: HET_CONTACT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_CONTACT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_CONTACT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_CONTACT" (
    "CONTACT_ID" integer DEFAULT nextval('public."HET_CONTACT_ID_seq"'::regclass) NOT NULL,
    "SURNAME" character varying(50),
	"GIVEN_NAME" character varying(50),
    "ROLE" character varying(100),
	"NOTES" character varying(512),       
	"EMAIL_ADDRESS" character varying(255),
    "MOBILE_PHONE_NUMBER" character varying(20),
	"WORK_PHONE_NUMBER" character varying(20),
	"FAX_PHONE_NUMBER" character varying(20),    
    "ADDRESS1" character varying(80),
    "ADDRESS2" character varying(80),
    "CITY" character varying(100),    
    "POSTAL_CODE" character varying(15),
    "PROVINCE" character varying(50),    
    "OWNER_ID" integer,
    "PROJECT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_DISTRICT_DISTRICT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_DISTRICT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_DISTRICT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_DISTRICT" (
    "DISTRICT_ID" integer DEFAULT nextval('public."HET_DISTRICT_ID_seq"'::regclass) NOT NULL,
    "DISTRICT_NUMBER" integer,
	"NAME" character varying(150),
	"START_DATE" timestamp without time zone NOT NULL,
	"END_DATE" timestamp without time zone,
    "MINISTRY_DISTRICT_ID" integer NOT NULL,    
    "REGION_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0   
);


--
-- Name: HET_DISTRICT_STATUS; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_DISTRICT_STATUS" (
    "DISTRICT_ID" integer,
    "CURRENT_FISCAL_YEAR" integer,
	"NEXT_FISCAL_YEAR" integer,
	"ROLLOVER_START_DATE" timestamp without time zone,
	"ROLLOVER_END_DATE" timestamp without time zone,
	"LOCAL_AREA_COUNT" integer,
	"DISTRICT_EQUIPMENT_TYPE_COUNT" integer,
	"LOCAL_AREA_COMPLETE_COUNT" integer,
	"DISTRICT_EQUIPMENT_TYPE_COMPLETE_COUNT" integer,
	"PROGRESS_PERCENTAGE" integer,
	"DISPLAY_ROLLOVER_MESSAGE" boolean NOT NULL, 
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);


--
-- Name: HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_DISTRICT_EQUIPMENT_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_DISTRICT_EQUIPMENT_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_DISTRICT_EQUIPMENT_TYPE" (
    "DISTRICT_EQUIPMENT_TYPE_ID" integer DEFAULT nextval('public."HET_DISTRICT_EQUIPMENT_TYPE_ID_seq"'::regclass) NOT NULL,
    "DISTRICT_EQUIPMENT_NAME" character varying(255),
    "DISTRICT_ID" integer,
    "EQUIPMENT_TYPE_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_EQUIPMENT_EQUIPMENT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT" (
    "EQUIPMENT_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_ID_seq"'::regclass) NOT NULL,
	"TYPE" character varying(50),
    "EQUIPMENT_CODE" character varying(25),
	"MAKE" character varying(50),
    "MODEL" character varying(50),
	"YEAR" character varying(15),
    "RECEIVED_DATE" timestamp without time zone NOT NULL,
	"YEARS_OF_SERVICE" real,
	"LICENCE_PLATE" character varying(20),
	"SERIAL_NUMBER" character varying(100),
    "SIZE" character varying(128),
	"SENIORITY" real,
	"SENIORITY_EFFECTIVE_DATE" timestamp without time zone,
	"TO_DATE" timestamp without time zone,    		            
    "NUMBER_IN_BLOCK" integer,
    "BLOCK_NUMBER" integer,    
    "SERVICE_HOURS_LAST_YEAR" real,
    "SERVICE_HOURS_THREE_YEARS_AGO" real,
    "SERVICE_HOURS_TWO_YEARS_AGO" real,        
    "IS_SENIORITY_OVERRIDDEN" boolean,
    "SENIORITY_OVERRIDE_REASON" character varying(2048),  
    "APPROVED_DATE" timestamp without time zone,
	"EQUIPMENT_STATUS_TYPE_ID" integer NOT NULL,
	"STATUS_COMMENT" character varying(255),
	"ARCHIVE_DATE" timestamp without time zone,
	"ARCHIVE_CODE" character varying(50),
	"ARCHIVE_REASON" character varying(2048),
	"LAST_VERIFIED_DATE" timestamp without time zone NOT NULL,
    "INFORMATION_UPDATE_NEEDED_REASON" character varying(2048),
    "IS_INFORMATION_UPDATE_NEEDED" boolean,       
    "DISTRICT_EQUIPMENT_TYPE_ID" integer,    
    "LOCAL_AREA_ID" integer,	
    "OPERATOR" character varying(255),
    "OWNER_ID" integer,
    "PAY_RATE" real,    
    "REFUSE_RATE" character varying(255),    	      	
    "LEGAL_CAPACITY" character varying(150),
    "LICENCED_GVW" character varying(150),
    "PUP_LEGAL_CAPACITY" character varying(150),
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ATTACHMENT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_ATTACHMENT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT_ATTACHMENT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT_ATTACHMENT" (
    "EQUIPMENT_ATTACHMENT_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_ATTACHMENT_ID_seq"'::regclass) NOT NULL,
    "TYPE_NAME" character varying(100),
	"DESCRIPTION" character varying(2048),
    "EQUIPMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT_ATTACHMENT_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT_ATTACHMENT_HIST" (
    "EQUIPMENT_ATTACHMENT_HIST_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"'::regclass) NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
	"EQUIPMENT_ATTACHMENT_ID" integer NOT NULL,
    "TYPE_NAME" character varying(100),
	"DESCRIPTION" character varying(2048),
	"EQUIPMENT_ID" integer,	
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_EQUIPMENT_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT_HIST" (
    "EQUIPMENT_HIST_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_HIST_ID_seq"'::regclass) NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,	
	"EQUIPMENT_ID" integer NOT NULL,
    "TYPE" character varying(50),
    "EQUIPMENT_CODE" character varying(25),
	"MAKE" character varying(50),
    "MODEL" character varying(50),
	"YEAR" character varying(15),
    "RECEIVED_DATE" timestamp without time zone NOT NULL,
	"YEARS_OF_SERVICE" real,
	"LICENCE_PLATE" character varying(20),
	"SERIAL_NUMBER" character varying(100),
    "SIZE" character varying(128),
	"SENIORITY" real,
	"SENIORITY_EFFECTIVE_DATE" timestamp without time zone,
	"TO_DATE" timestamp without time zone,    		            
    "NUMBER_IN_BLOCK" integer,
    "BLOCK_NUMBER" integer,    
    "SERVICE_HOURS_LAST_YEAR" real,
    "SERVICE_HOURS_THREE_YEARS_AGO" real,
    "SERVICE_HOURS_TWO_YEARS_AGO" real,        
    "IS_SENIORITY_OVERRIDDEN" boolean,
    "SENIORITY_OVERRIDE_REASON" character varying(2048),  
    "APPROVED_DATE" timestamp without time zone,
    "EQUIPMENT_STATUS_TYPE_ID" integer NOT NULL,
	"STATUS_COMMENT" character varying(255),
	"ARCHIVE_DATE" timestamp without time zone,
	"ARCHIVE_CODE" character varying(50),
	"ARCHIVE_REASON" character varying(2048),
	"LAST_VERIFIED_DATE" timestamp without time zone NOT NULL,
    "INFORMATION_UPDATE_NEEDED_REASON" character varying(2048),
    "IS_INFORMATION_UPDATE_NEEDED" boolean,       
    "DISTRICT_EQUIPMENT_TYPE_ID" integer,    
    "LOCAL_AREA_ID" integer,	
    "OPERATOR" character varying(255),
    "OWNER_ID" integer,
    "PAY_RATE" real,    
    "REFUSE_RATE" character varying(255),    	      	
    "LEGAL_CAPACITY" character varying(150),
    "LICENCED_GVW" character varying(150),
    "PUP_LEGAL_CAPACITY" character varying(150),
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_EQUIPMENT_STATUS_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_STATUS_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT_STATUS_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT_STATUS_TYPE" (
    "EQUIPMENT_STATUS_TYPE_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_STATUS_TYPE_ID_seq"'::regclass) NOT NULL,
    "EQUIPMENT_STATUS_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_EQUIPMENT_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_EQUIPMENT_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_EQUIPMENT_TYPE" (
    "EQUIPMENT_TYPE_ID" integer DEFAULT nextval('public."HET_EQUIPMENT_TYPE_ID_seq"'::regclass) NOT NULL,
    "NAME" character varying(150),
    "BLUE_BOOK_RATE_NUMBER" real,
    "BLUE_BOOK_SECTION" real,
    "IS_DUMP_TRUCK" boolean NOT NULL,
	"NUMBER_OF_BLOCKS" integer NOT NULL,
	"EXTEND_HOURS" real,    
    "MAXIMUM_HOURS" real,
    "MAX_HOURS_SUB" real,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_HISTORY_HISTORY_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_HISTORY_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
--
-- Name: HET_HISTORY; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_HISTORY" (
    "HISTORY_ID" integer DEFAULT nextval('public."HET_HISTORY_ID_seq"'::regclass) NOT NULL,
	"CREATED_DATE" timestamp without time zone,
	"HISTORY_TEXT" character varying(2048),
    "EQUIPMENT_ID" integer,    
    "OWNER_ID" integer,
    "PROJECT_ID" integer,
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_IMPORT_MAP_IMPORT_MAP_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_IMPORT_MAP_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_IMPORT_MAP; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_IMPORT_MAP" (
    "IMPORT_MAP_ID" integer DEFAULT nextval('public."HET_IMPORT_MAP_ID_seq"'::regclass) NOT NULL,
    "OLD_TABLE" text,
	"OLD_KEY" character varying(250),	
    "NEW_TABLE" text,
	"NEW_KEY" integer NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_LOCAL_AREA_LOCAL_AREA_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_LOCAL_AREA_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_LOCAL_AREA; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_LOCAL_AREA" (
    "LOCAL_AREA_ID" integer DEFAULT nextval('public."HET_LOCAL_AREA_ID_seq"'::regclass) NOT NULL,
    "LOCAL_AREA_NUMBER" integer NOT NULL,
	"NAME" character varying(150),    
    "END_DATE" timestamp without time zone,    
    "START_DATE" timestamp without time zone DEFAULT '0001-01-01 00:00:00'::timestamp without time zone NOT NULL,
	"SERVICE_AREA_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_LOCAL_AREA_ROTATION_LIST_LOCAL_AREA_ROTATION_LIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_LOCAL_AREA_ROTATION_LIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_LOCAL_AREA_ROTATION_LIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_LOCAL_AREA_ROTATION_LIST" (
    "LOCAL_AREA_ROTATION_LIST_ID" integer DEFAULT nextval('public."HET_LOCAL_AREA_ROTATION_LIST_ID_seq"'::regclass) NOT NULL,
    "LOCAL_AREA_ID" integer,
	"DISTRICT_EQUIPMENT_TYPE_ID" integer,
	"ASK_NEXT_BLOCK1_ID" integer,
    "ASK_NEXT_BLOCK1_SENIORITY" real,
    "ASK_NEXT_BLOCK2_ID" integer,
    "ASK_NEXT_BLOCK2_SENIORITY" real,
    "ASK_NEXT_BLOCK_OPEN_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_MIME_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_MIME_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_MIME_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_MIME_TYPE" (
    "MIME_TYPE_ID" integer DEFAULT nextval('public."HET_MIME_TYPE_ID_seq"'::regclass) NOT NULL,
    "MIME_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_NOTE_NOTE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_NOTE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_NOTE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_NOTE" (
    "NOTE_ID" integer DEFAULT nextval('public."HET_NOTE_ID_seq"'::regclass) NOT NULL,
    "TEXT" character varying(2048),
    "IS_NO_LONGER_RELEVANT" boolean,
    "EQUIPMENT_ID" integer,
	"OWNER_ID" integer,
    "PROJECT_ID" integer,    
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_NOTE_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_NOTE_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_NOTE_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_NOTE_HIST" (
    "NOTE_HIST_ID" integer DEFAULT nextval('public."HET_NOTE_HIST_ID_seq"'::regclass) NOT NULL,
    "NOTE_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,	
	"TEXT" character varying(2048),
    "IS_NO_LONGER_RELEVANT" boolean,
    "EQUIPMENT_ID" integer,    
    "OWNER_ID" integer,
    "PROJECT_ID" integer,    
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);


--
-- Name: HET_BUSINESS_BUSINESS_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_BUSINESS_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_BUSINESS; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_BUSINESS" (
    "BUSINESS_ID" integer DEFAULT nextval('public."HET_BUSINESS_ID_seq"'::regclass) NOT NULL,
    "BCEID_LEGAL_NAME" character varying(150),
    "BCEID_DOING_BUSINESS_AS" character varying(150),
	"BCEID_BUSINESS_NUMBER" character varying(50),
	"BCEID_BUSINESS_GUID" character varying(50),	
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0        
);


--
-- Name: HET_BUSINESS_USER_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_BUSINESS_USER_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_BUSINESS; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_BUSINESS_USER" (
    "BUSINESS_USER_ID" integer DEFAULT nextval('public."HET_BUSINESS_USER_ID_seq"'::regclass) NOT NULL,
    "BCEID_USER_ID" character varying(150),
	"BCEID_GUID" character varying(50),
	"BCEID_DISPLAY_NAME" character varying(150),
    "BCEID_FIRST_NAME" character varying(150),
	"BCEID_LAST_NAME" character varying(150),
	"BCEID_EMAIL" character varying(150),	
	"BCEID_TELEPHONE" character varying(150),	
	"BUSINESS_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0        
);


--
-- Name: HET_BUSINESS_USER_ROLE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_BUSINESS_USER_ROLE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_BUSINESS_USER_ROLE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_BUSINESS_USER_ROLE" (
    "BUSINESS_USER_ROLE_ID" integer DEFAULT nextval('public."HET_BUSINESS_USER_ROLE_ID_seq"'::regclass) NOT NULL, 
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "EXPIRY_DATE" timestamp without time zone,
    "BUSINESS_USER_ID" integer,
	"ROLE_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);


--
-- Name: HET_OWNER_OWNER_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_OWNER_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_OWNER; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_OWNER" (
    "OWNER_ID" integer DEFAULT nextval('public."HET_OWNER_ID_seq"'::regclass) NOT NULL,
    "ORGANIZATION_NAME" character varying(150),
    "OWNER_CODE" character varying(20),
    "DOING_BUSINESS_AS" character varying(150),
	"SURNAME" character varying(50),
	"GIVEN_NAME" character varying(50),
    "REGISTERED_COMPANY_NUMBER" character varying(150),
    "ADDRESS1" character varying(80),
    "ADDRESS2" character varying(80),
    "CITY" character varying(100),
    "POSTAL_CODE" character varying(15),
    "PROVINCE" character varying(50),        
	"OWNER_STATUS_TYPE_ID" integer NOT NULL,
	"STATUS_COMMENT" character varying(255),
	"ARCHIVE_DATE" timestamp without time zone,
	"ARCHIVE_CODE" character varying(50),
	"ARCHIVE_REASON" character varying(2048),
	"LOCAL_AREA_ID" integer,    
    "PRIMARY_CONTACT_ID" integer,    
    "CGL_POLICY_NUMBER" character varying(50),    
	"CGLEND_DATE" timestamp without time zone,
    "WORK_SAFE_BCPOLICY_NUMBER" character varying(50),    
    "WORK_SAFE_BCEXPIRY_DATE" timestamp without time zone,
    "IS_MAINTENANCE_CONTRACTOR" boolean,    
    "MEETS_RESIDENCY" boolean NOT NULL,
	"BUSINESS_ID" integer,
	"SHARED_KEY" character varying(50),   
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0        
);

--
-- Name: HET_OWNER_STATUS_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_OWNER_STATUS_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_OWNER_STATUS_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_OWNER_STATUS_TYPE" (
    "OWNER_STATUS_TYPE_ID" integer DEFAULT nextval('public."HET_OWNER_STATUS_TYPE_ID_seq"'::regclass) NOT NULL,
    "OWNER_STATUS_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_PERMISSION_PERMISSION_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_PERMISSION_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_PERMISSION; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_PERMISSION" (
    "PERMISSION_ID" integer DEFAULT nextval('public."HET_PERMISSION_ID_seq"'::regclass) NOT NULL,
    "CODE" character varying(50),
    "NAME" character varying(150),
	"DESCRIPTION" character varying(2048),
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_PERSON_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_PERSON_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_PERSON; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_PERSON" (
    "PERSON_ID" integer DEFAULT nextval('public."HET_PERSON_ID_seq"'::regclass) NOT NULL,
    "SURNAME" character varying(50) NOT NULL,
    "FIRST_NAME" character varying(50),
    "MIDDLE_NAMES" character varying(200),
    "NAME_SUFFIX" character varying(50),
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_PROJECT_PROJECT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_PROJECT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_PROJECT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_PROJECT" (
    "PROJECT_ID" integer DEFAULT nextval('public."HET_PROJECT_ID_seq"'::regclass) NOT NULL,  
    "PROVINCIAL_PROJECT_NUMBER" character varying(150),
    "NAME" character varying(100),
    "PROJECT_STATUS_TYPE_ID" integer NOT NULL,
	"INFORMATION" character varying(2048),    
    "DISTRICT_ID" integer,
	"PRIMARY_CONTACT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0	
);

--
-- Name: HET_PROJECT_STATUS_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_PROJECT_STATUS_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_PROJECT_STATUS_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_PROJECT_STATUS_TYPE" (
    "PROJECT_STATUS_TYPE_ID" integer DEFAULT nextval('public."HET_PROJECT_STATUS_TYPE_ID_seq"'::regclass) NOT NULL,
    "PROJECT_STATUS_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_PROVINCIAL_RATE_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_PROVINCIAL_RATE_TYPE" (
    "RATE_TYPE" character varying(20) NOT NULL,
    "ACTIVE" boolean NOT NULL,
    "DESCRIPTION" character varying(200),
	"PERIOD_TYPE" character varying(20),
    "RATE" real,
	"OVERTIME" boolean NOT NULL,
    "IS_INCLUDED_IN_TOTAL" boolean NOT NULL,
    "IS_PERCENT_RATE" boolean NOT NULL,
    "IS_RATE_EDITABLE" boolean NOT NULL,    
    "IS_IN_TOTAL_EDITABLE" boolean DEFAULT false NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RATE_PERIOD_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RATE_PERIOD_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RATE_PERIOD_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RATE_PERIOD_TYPE" (
    "RATE_PERIOD_TYPE_ID" integer DEFAULT nextval('public."HET_RATE_PERIOD_TYPE_ID_seq"'::regclass) NOT NULL,
    "RATE_PERIOD_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_REGION_REGION_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_REGION_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_REGION; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_REGION" (
    "REGION_ID" integer DEFAULT nextval('public."HET_REGION_ID_seq"'::regclass) NOT NULL,
    "NAME" character varying(150),
	"REGION_NUMBER" integer,
    "MINISTRY_REGION_ID" integer NOT NULL,        
	"START_DATE" timestamp without time zone NOT NULL,
	"END_DATE" timestamp without time zone,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT" (
    "RENTAL_AGREEMENT_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_ID_seq"'::regclass) NOT NULL,
    "NUMBER" character varying(30),
	"ESTIMATE_HOURS" integer,
    "ESTIMATE_START_WORK" timestamp without time zone,
    "NOTE" character varying(2048),    
    "EQUIPMENT_RATE" real,    
    "RATE_COMMENT" character varying(2048),
    "RATE_PERIOD_TYPE_ID" integer NOT NULL,	
	"DATED_ON" timestamp without time zone,    
    "RENTAL_AGREEMENT_STATUS_TYPE_ID" integer NOT NULL,
	"EQUIPMENT_ID" integer,
    "PROJECT_ID" integer,
	"DISTRICT_ID" integer,
	"RENTAL_REQUEST_ID" integer,
	"RENTAL_REQUEST_ROTATION_LIST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_AGREEMENT_CONDITIO_RENTAL_AGREEMENT_CONDITION_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_CONDITION_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
--
-- Name: HET_RENTAL_AGREEMENT_CONDITION; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_CONDITION" (
    "RENTAL_AGREEMENT_CONDITION_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_CONDITION_ID_seq"'::regclass) NOT NULL,
    "COMMENT" character varying(2048),
    "CONDITION_NAME" character varying(150),
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT_CONDITION_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_CONDITION_HIST" (
    "RENTAL_AGREEMENT_CONDITION_HIST_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq"'::regclass) NOT NULL,
    "RENTAL_AGREEMENT_CONDITION_ID" integer NOT NULL,
	"EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,    
	"COMMENT" character varying(2048),
    "CONDITION_NAME" character varying(150),
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_AGREEMENT_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_HIST" (
    "RENTAL_AGREEMENT_HIST_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_HIST_ID_seq"'::regclass) NOT NULL,
    "RENTAL_AGREEMENT_ID" integer NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "NUMBER" character varying(30),
	"ESTIMATE_HOURS" integer,
    "ESTIMATE_START_WORK" timestamp without time zone,
    "NOTE" character varying(2048),    
    "EQUIPMENT_RATE" real,    
    "RATE_COMMENT" character varying(2048),
    "RATE_PERIOD_TYPE_ID" integer NOT NULL,	
	"DATED_ON" timestamp without time zone,    
    "RENTAL_AGREEMENT_STATUS_TYPE_ID" integer NOT NULL,
	"EQUIPMENT_ID" integer,
    "PROJECT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_RATE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT_RATE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_RATE" (
    "RENTAL_AGREEMENT_RATE_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_RATE_ID_seq"'::regclass) NOT NULL,
    "COMMENT" character varying(2048),
    "COMPONENT_NAME" character varying(150),
    "RATE" real,
    "OVERTIME" boolean DEFAULT false,
	"ACTIVE" boolean DEFAULT false,
    "IS_INCLUDED_IN_TOTAL" boolean DEFAULT false NOT NULL,    
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT_RATE_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_RATE_HIST" (
    "RENTAL_AGREEMENT_RATE_HIST_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"'::regclass) NOT NULL,
    "RENTAL_AGREEMENT_RATE_ID" integer NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "COMMENT" character varying(2048),
    "COMPONENT_NAME" character varying(150),
    "RATE" real,
    "OVERTIME" boolean DEFAULT false,
	"ACTIVE" boolean DEFAULT false,
    "IS_INCLUDED_IN_TOTAL" boolean DEFAULT false NOT NULL,    
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_AGREEMENT_STATUS_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_AGREEMENT_STATUS_TYPE" (
    "RENTAL_AGREEMENT_STATUS_TYPE_ID" integer DEFAULT nextval('public."HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq"'::regclass) NOT NULL,
    "RENTAL_AGREEMENT_STATUS_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_REQUEST_RENTAL_REQUEST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_REQUEST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_REQUEST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_REQUEST" (
    "RENTAL_REQUEST_ID" integer DEFAULT nextval('public."HET_RENTAL_REQUEST_ID_seq"'::regclass) NOT NULL,
    "EQUIPMENT_COUNT" integer NOT NULL,
    "EXPECTED_START_DATE" timestamp without time zone,
    "EXPECTED_END_DATE" timestamp without time zone,
    "EXPECTED_HOURS" integer,    
    "FIRST_ON_ROTATION_LIST_ID" integer,
    "RENTAL_REQUEST_STATUS_TYPE_ID" integer NOT NULL,
	"DISTRICT_EQUIPMENT_TYPE_ID" integer,
	"LOCAL_AREA_ID" integer,
    "PROJECT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ATTACHMENT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_REQUEST_ATTACHMENT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_REQUEST_ATTACHMENT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_REQUEST_ATTACHMENT" (
    "RENTAL_REQUEST_ATTACHMENT_ID" integer DEFAULT nextval('public."HET_RENTAL_REQUEST_ATTACHMENT_ID_seq"'::regclass) NOT NULL,
    "ATTACHMENT" character varying(150),
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_RENTAL_REQUEST_ROTATION_L_RENTAL_REQUEST_ROTATION_LIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_REQUEST_ROTATION_LIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST" (
    "RENTAL_REQUEST_ROTATION_LIST_ID" integer DEFAULT nextval('public."HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq"'::regclass) NOT NULL,
    "ROTATION_LIST_SORT_ORDER" integer NOT NULL,
	"ASKED_DATE_TIME" timestamp without time zone,            
    "WAS_ASKED" boolean,
	"OFFER_RESPONSE" text,
    "OFFER_RESPONSE_NOTE" character varying(2048),
	"OFFER_REFUSAL_REASON" character varying(50),
    "OFFER_RESPONSE_DATETIME" timestamp without time zone,
	"IS_FORCE_HIRE" boolean,
	"NOTE" character varying(2048),
	"EQUIPMENT_ID" integer,    
    "RENTAL_AGREEMENT_ID" integer,
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0        
);

--
-- Name: HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
--
-- Name: HET_RENTAL_REQUEST_ROTATION_LIST_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" (
    "RENTAL_REQUEST_ROTATION_LIST_HIST_ID" integer DEFAULT nextval('public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass) NOT NULL,
    "RENTAL_REQUEST_ROTATION_LIST_ID" integer NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "ROTATION_LIST_SORT_ORDER" integer NOT NULL,
	"ASKED_DATE_TIME" timestamp without time zone,            
    "WAS_ASKED" boolean,
	"OFFER_RESPONSE" text,
    "OFFER_RESPONSE_NOTE" character varying(2048),
	"OFFER_REFUSAL_REASON" character varying(50),
    "OFFER_RESPONSE_DATETIME" timestamp without time zone,
	"IS_FORCE_HIRE" boolean,
	"NOTE" character varying(2048),
	"EQUIPMENT_ID" integer,    
    "RENTAL_AGREEMENT_ID" integer,
    "RENTAL_REQUEST_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0   
);

--
-- Name: HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_RENTAL_REQUEST_STATUS_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_RENTAL_REQUEST_STATUS_TYPE" (
    "RENTAL_REQUEST_STATUS_TYPE_ID" integer DEFAULT nextval('public."HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq"'::regclass) NOT NULL,
    "RENTAL_REQUEST_STATUS_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_ROLE_ROLE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_ROLE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_ROLE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_ROLE" (
    "ROLE_ID" integer DEFAULT nextval('public."HET_ROLE_ID_seq"'::regclass) NOT NULL,
    "NAME" character varying(255),
	"DESCRIPTION" character varying(2048),
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_ROLE_PERMISSION_ROLE_PERMISSION_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_ROLE_PERMISSION_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_ROLE_PERMISSION; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_ROLE_PERMISSION" (
    "ROLE_PERMISSION_ID" integer DEFAULT nextval('public."HET_ROLE_PERMISSION_ID_seq"'::regclass) NOT NULL,
    "PERMISSION_ID" integer,
    "ROLE_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_SENIORITY_AUDIT_SENIORITY_AUDIT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_SENIORITY_AUDIT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_SENIORITY_AUDIT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_SENIORITY_AUDIT" (
    "SENIORITY_AUDIT_ID" integer DEFAULT nextval('public."HET_SENIORITY_AUDIT_ID_seq"'::regclass) NOT NULL,
    "START_DATE" timestamp without time zone NOT NULL,
	"END_DATE" timestamp without time zone NOT NULL,
	"OWNER_ORGANIZATION_NAME" character varying(150),
	"SENIORITY" real,	
	"BLOCK_NUMBER" integer,
    "IS_SENIORITY_OVERRIDDEN" boolean,
    "SENIORITY_OVERRIDE_REASON" character varying(2048),	
	"SERVICE_HOURS_LAST_YEAR" real,
    "SERVICE_HOURS_THREE_YEARS_AGO" real,
    "SERVICE_HOURS_TWO_YEARS_AGO" real,	
    "EQUIPMENT_ID" integer,    
    "LOCAL_AREA_ID" integer,
    "OWNER_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_SERVICE_AREA_SERVICE_AREA_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_SERVICE_AREA_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_SERVICE_AREA; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_SERVICE_AREA" (
    "SERVICE_AREA_ID" integer DEFAULT nextval('public."HET_SERVICE_AREA_ID_seq"'::regclass) NOT NULL, 
    "NAME" character varying(150),
	"AREA_NUMBER" integer,
    "MINISTRY_SERVICE_AREA_ID" integer NOT NULL,
	"FISCAL_START_DATE" timestamp without time zone NOT NULL,    
	"FISCAL_END_DATE" timestamp without time zone,    
	"ADDRESS" character varying(255),
	"PHONE" character varying(50),
	"FAX" character varying(50),
	"SUPPORTING_DOCUMENTS" character varying(500),
    "DISTRICT_ID" integer,	
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_TIME_PERIOD_TYPE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_TIME_PERIOD_TYPE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_TIME_PERIOD_TYPE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_TIME_PERIOD_TYPE" (
    "TIME_PERIOD_TYPE_ID" integer DEFAULT nextval('public."HET_TIME_PERIOD_TYPE_ID_seq"'::regclass) NOT NULL,
    "TIME_PERIOD_TYPE_CODE" character varying(20) NOT NULL,
    "DESCRIPTION" character varying(2048) NOT NULL,
    "SCREEN_LABEL" character varying(200),
    "DISPLAY_ORDER" integer,
    "IS_ACTIVE" boolean DEFAULT true NOT NULL,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_TIME_RECORD_TIME_RECORD_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_TIME_RECORD_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_TIME_RECORD; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_TIME_RECORD" (
    "TIME_RECORD_ID" integer DEFAULT nextval('public."HET_TIME_RECORD_ID_seq"'::regclass) NOT NULL,
    "ENTERED_DATE" timestamp without time zone,
    "WORKED_DATE" timestamp without time zone NOT NULL,
    "TIME_PERIOD_TYPE_ID" integer NOT NULL,
	"HOURS" real,
    "RENTAL_AGREEMENT_RATE_ID" integer,
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: HET_TIME_RECORD_HIST_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_TIME_RECORD_HIST_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_TIME_RECORD_HIST; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_TIME_RECORD_HIST" (
    "TIME_RECORD_HIST_ID" integer DEFAULT nextval('public."HET_TIME_RECORD_HIST_ID_seq"'::regclass) NOT NULL,
    "TIME_RECORD_ID" integer NOT NULL,
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "END_DATE" timestamp without time zone,
    "ENTERED_DATE" timestamp without time zone,
    "WORKED_DATE" timestamp without time zone NOT NULL,
    "TIME_PERIOD_TYPE_ID" integer NOT NULL,
	"HOURS" real,
    "RENTAL_AGREEMENT_RATE_ID" integer,
    "RENTAL_AGREEMENT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);
	
--
-- Name: HET_USER_USER_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_USER_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
--
-- Name: HET_USER; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_USER" (
    "USER_ID" integer DEFAULT nextval('public."HET_USER_ID_seq"'::regclass) NOT NULL, 
    "SURNAME" character varying(50),
	"GIVEN_NAME" character varying(50),
	"INITIALS" character varying(10),	
	"SM_USER_ID" character varying(255),
	"SM_AUTHORIZATION_DIRECTORY" character varying(255),
	"GUID" character varying(255),
	"EMAIL" character varying(255),	
	"ACTIVE" boolean NOT NULL,
    "DISTRICT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_USER_DISTRICT_USER_DISTRICT_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_USER_DISTRICT_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_USER_DISTRICT; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_USER_DISTRICT" (
    "USER_DISTRICT_ID" integer DEFAULT nextval('public."HET_USER_DISTRICT_ID_seq"'::regclass) NOT NULL, 
    "IS_PRIMARY" boolean NOT NULL,	   
    "USER_ID" integer,
	"DISTRICT_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_USER_FAVOURITE_USER_FAVOURITE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_USER_FAVOURITE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_USER_FAVOURITE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_USER_FAVOURITE" (
    "USER_FAVOURITE_ID" integer DEFAULT nextval('public."HET_USER_FAVOURITE_ID_seq"'::regclass) NOT NULL, 
    "TYPE" character varying(150),    
    "NAME" character varying(150),
    "VALUE" character varying(2048),    
	"IS_DEFAULT" boolean,
	"USER_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0
);

--
-- Name: HET_USER_ROLE_USER_ROLE_ID_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."HET_USER_ROLE_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: HET_USER_ROLE; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HET_USER_ROLE" (
    "USER_ROLE_ID" integer DEFAULT nextval('public."HET_USER_ROLE_ID_seq"'::regclass) NOT NULL, 
    "EFFECTIVE_DATE" timestamp without time zone NOT NULL,
    "EXPIRY_DATE" timestamp without time zone,
    "USER_ID" integer,
	"ROLE_ID" integer,
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
    "CONCURRENCY_CONTROL_NUMBER" integer NOT NULL DEFAULT 0    
);

--
-- Name: counter_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."counter_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: counter; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.counter (
    id integer DEFAULT nextval('public."counter_id_seq"'::regclass) NOT NULL, 
    key character varying(100) NOT NULL,
    value smallint NOT NULL,
    expireat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: hash_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."hash_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: hash; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.hash (
    id integer DEFAULT nextval('public."hash_id_seq"'::regclass) NOT NULL, 
    key character varying(100) NOT NULL,
    field character varying(100) NOT NULL,
    value text,
    expireat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: job_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."job_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: job; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.job (
    id integer DEFAULT nextval('public."job_id_seq"'::regclass) NOT NULL, 
    stateid integer,
    statename character varying(20),
    invocationdata text NOT NULL,
    arguments text NOT NULL,
    createdat timestamp without time zone NOT NULL,
    expireat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: jobparameter_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."jobparameter_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: jobparameter; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.jobparameter (
    id integer DEFAULT nextval('public."jobparameter_id_seq"'::regclass) NOT NULL, 
    jobid integer NOT NULL,
    name character varying(40) NOT NULL,
    value text,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: jobqueue_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."jobqueue_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: jobqueue; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.jobqueue (
    id integer DEFAULT nextval('public."jobqueue_id_seq"'::regclass) NOT NULL, 
    jobid integer NOT NULL,
    queue character varying(20) NOT NULL,
    fetchedat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: list_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."list_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: list; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.list (
    id integer DEFAULT nextval('public."list_id_seq"'::regclass) NOT NULL, 
    key character varying(100) NOT NULL,
    value text,
    expireat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: lock; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.lock (
    resource character varying(100) NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL,
    acquired timestamp without time zone
);

--
-- Name: schema; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.schema (
    version integer NOT NULL
);

--
-- Name: server; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.server (
    id character varying(100) NOT NULL,
    data text,
    lastheartbeat timestamp without time zone NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: set_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."set_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: set; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.set (
    id integer DEFAULT nextval('public."set_id_seq"'::regclass) NOT NULL, 
    key character varying(100) NOT NULL,
    score double precision NOT NULL,
    value text NOT NULL,
    expireat timestamp without time zone,
    updatecount integer DEFAULT 0 NOT NULL
);

--
-- Name: state_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."state_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: state; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.state (
    id integer DEFAULT nextval('public."state_id_seq"'::regclass) NOT NULL, 
    jobid integer NOT NULL,
    name character varying(20) NOT NULL,
    reason character varying(100),
    createdat timestamp without time zone NOT NULL,
    data text,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: HET_EQUIPMENT_ATTACHMENT_HIST HET_EQUIPMENT_ATTACHMENT_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT_ATTACHMENT_HIST"
    ADD CONSTRAINT "HET_EQUIPMENT_ATTACHMENT_HIST_PK" PRIMARY KEY ("EQUIPMENT_ATTACHMENT_HIST_ID");


--
-- Name: HET_EQUIPMENT_HIST HET_EQUIPMENT_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT_HIST"
    ADD CONSTRAINT "HET_EQUIPMENT_HIST_PK" PRIMARY KEY ("EQUIPMENT_HIST_ID");


--
-- Name: HET_NOTE_HIST HET_NOTE_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_NOTE_HIST"
    ADD CONSTRAINT "HET_NOTE_HIST_PK" PRIMARY KEY ("NOTE_HIST_ID");


--
-- Name: HET_RENTAL_AGREEMENT_CONDITION_HIST HET_RENTAL_AGREEMENT_CONDITION_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_CONDITION_HIST"
    ADD CONSTRAINT "HET_RENTAL_AGREEMENT_CONDITION_HIST_PK" PRIMARY KEY ("RENTAL_AGREEMENT_CONDITION_HIST_ID");


--
-- Name: HET_RENTAL_AGREEMENT_HIST HET_RENTAL_AGREEMENT_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_HIST"
    ADD CONSTRAINT "HET_RENTAL_AGREEMENT_HIST_PK" PRIMARY KEY ("RENTAL_AGREEMENT_HIST_ID");


--
-- Name: HET_RENTAL_AGREEMENT_RATE_HIST HET_RENTAL_AGREEMENT_RATE_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_RATE_HIST"
    ADD CONSTRAINT "HET_RENTAL_AGREEMENT_RATE_HIST_PK" PRIMARY KEY ("RENTAL_AGREEMENT_RATE_HIST_ID");


--
-- Name: HET_RENTAL_REQUEST_ROTATION_LIST_HIST HET_RENTAL_REQUEST_ROTATION_LIST_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
    ADD CONSTRAINT "HET_RENTAL_REQUEST_ROTATION_LIST_HIST_PK" PRIMARY KEY ("RENTAL_REQUEST_ROTATION_LIST_HIST_ID");

--
-- Name: HET_TIME_RECORD_HIST HET_TIME_RECORD_HIST_PK; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_TIME_RECORD_HIST"
    ADD CONSTRAINT "HET_TIME_RECORD_HIST_PK" PRIMARY KEY ("TIME_RECORD_HIST_ID");

--
-- Name: HET_DIGITAL_FILE PK_HET_DIGITAL_FILE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_DIGITAL_FILE"
    ADD CONSTRAINT "PK_HET_DIGITAL_FILE" PRIMARY KEY ("DIGITAL_FILE_ID");

--
-- Name: HET_CONDITION_TYPE PK_HET_CONDITION_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_CONDITION_TYPE"
    ADD CONSTRAINT "PK_HET_CONDITION_TYPE" PRIMARY KEY ("CONDITION_TYPE_ID");


--
-- Name: HET_CONTACT PK_HET_CONTACT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_CONTACT"
    ADD CONSTRAINT "PK_HET_CONTACT" PRIMARY KEY ("CONTACT_ID");


--
-- Name: HET_DISTRICT PK_HET_DISTRICT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_DISTRICT"
    ADD CONSTRAINT "PK_HET_DISTRICT" PRIMARY KEY ("DISTRICT_ID");


--
-- Name: HET_DISTRICT_EQUIPMENT_TYPE PK_HET_DISTRICT_EQUIPMENT_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_DISTRICT_EQUIPMENT_TYPE"
    ADD CONSTRAINT "PK_HET_DISTRICT_EQUIPMENT_TYPE" PRIMARY KEY ("DISTRICT_EQUIPMENT_TYPE_ID");


--
-- Name: HET_EQUIPMENT PK_HET_EQUIPMENT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT"
    ADD CONSTRAINT "PK_HET_EQUIPMENT" PRIMARY KEY ("EQUIPMENT_ID");


--
-- Name: HET_EQUIPMENT_ATTACHMENT PK_HET_EQUIPMENT_ATTACHMENT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT_ATTACHMENT"
    ADD CONSTRAINT "PK_HET_EQUIPMENT_ATTACHMENT" PRIMARY KEY ("EQUIPMENT_ATTACHMENT_ID");


--
-- Name: HET_EQUIPMENT_STATUS_TYPE PK_HET_EQUIPMENT_STATUS_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT_STATUS_TYPE"
    ADD CONSTRAINT "PK_HET_EQUIPMENT_STATUS_TYPE" PRIMARY KEY ("EQUIPMENT_STATUS_TYPE_ID");


--
-- Name: HET_EQUIPMENT_TYPE PK_HET_EQUIPMENT_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_EQUIPMENT_TYPE"
    ADD CONSTRAINT "PK_HET_EQUIPMENT_TYPE" PRIMARY KEY ("EQUIPMENT_TYPE_ID");


--
-- Name: HET_HISTORY PK_HET_HISTORY; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_HISTORY"
    ADD CONSTRAINT "PK_HET_HISTORY" PRIMARY KEY ("HISTORY_ID");


--
-- Name: HET_IMPORT_MAP PK_HET_IMPORT_MAP; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_IMPORT_MAP"
    ADD CONSTRAINT "PK_HET_IMPORT_MAP" PRIMARY KEY ("IMPORT_MAP_ID");


--
-- Name: HET_LOCAL_AREA PK_HET_LOCAL_AREA; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_LOCAL_AREA"
    ADD CONSTRAINT "PK_HET_LOCAL_AREA" PRIMARY KEY ("LOCAL_AREA_ID");


--
-- Name: HET_LOCAL_AREA_ROTATION_LIST PK_HET_LOCAL_AREA_ROTATION_LIST; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_LOCAL_AREA_ROTATION_LIST"
    ADD CONSTRAINT "PK_HET_LOCAL_AREA_ROTATION_LIST" PRIMARY KEY ("LOCAL_AREA_ROTATION_LIST_ID");


--
-- Name: HET_MIME_TYPE PK_HET_MIME_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_MIME_TYPE"
    ADD CONSTRAINT "PK_HET_MIME_TYPE" PRIMARY KEY ("MIME_TYPE_ID");


--
-- Name: HET_NOTE PK_HET_NOTE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_NOTE"
    ADD CONSTRAINT "PK_HET_NOTE" PRIMARY KEY ("NOTE_ID");

	
--
-- Name: HET_BUSINESS PK_HET_BUSINESS; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_BUSINESS"
    ADD CONSTRAINT "PK_HET_BUSINESS" PRIMARY KEY ("BUSINESS_ID");


--
-- Name: HET_BUSINESS_USER PK_HET_BUSINESS_USER; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_BUSINESS_USER"
    ADD CONSTRAINT "PK_HET_BUSINESS_USER" PRIMARY KEY ("BUSINESS_USER_ID");
		
		
--
-- Name: HET_BUSINESS_USER_ROLE PK_HET_BUSINESS_USER_ROLE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_BUSINESS_USER_ROLE"
    ADD CONSTRAINT "PK_PK_HET_BUSINESS_USER_ROLE" PRIMARY KEY ("BUSINESS_USER_ROLE_ID");

	
--
-- Name: HET_OWNER PK_HET_OWNER; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_OWNER"
    ADD CONSTRAINT "PK_HET_OWNER" PRIMARY KEY ("OWNER_ID");


--
-- Name: HET_OWNER_STATUS_TYPE PK_HET_OWNER_STATUS_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_OWNER_STATUS_TYPE"
    ADD CONSTRAINT "PK_HET_OWNER_STATUS_TYPE" PRIMARY KEY ("OWNER_STATUS_TYPE_ID");


--
-- Name: HET_PERMISSION PK_HET_PERMISSION; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_PERMISSION"
    ADD CONSTRAINT "PK_HET_PERMISSION" PRIMARY KEY ("PERMISSION_ID");


--
-- Name: HET_PERSON PK_HET_PERSON; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_PERSON"
    ADD CONSTRAINT "PK_HET_PERSON" PRIMARY KEY ("PERSON_ID");


--
-- Name: HET_PROJECT PK_HET_PROJECT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_PROJECT"
    ADD CONSTRAINT "PK_HET_PROJECT" PRIMARY KEY ("PROJECT_ID");


--
-- Name: HET_PROJECT_STATUS_TYPE PK_HET_PROJECT_STATUS_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_PROJECT_STATUS_TYPE"
    ADD CONSTRAINT "PK_HET_PROJECT_STATUS_TYPE" PRIMARY KEY ("PROJECT_STATUS_TYPE_ID");


--
-- Name: HET_PROVINCIAL_RATE_TYPE PK_HET_PROVINCIAL_RATE_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_PROVINCIAL_RATE_TYPE"
    ADD CONSTRAINT "PK_HET_PROVINCIAL_RATE_TYPE" PRIMARY KEY ("RATE_TYPE");


--
-- Name: HET_RATE_PERIOD_TYPE PK_HET_RATE_PERIOD_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RATE_PERIOD_TYPE"
    ADD CONSTRAINT "PK_HET_RATE_PERIOD_TYPE" PRIMARY KEY ("RATE_PERIOD_TYPE_ID");


--
-- Name: HET_REGION PK_HET_REGION; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_REGION"
    ADD CONSTRAINT "PK_HET_REGION" PRIMARY KEY ("REGION_ID");


--
-- Name: HET_RENTAL_AGREEMENT PK_HET_RENTAL_AGREEMENT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT"
    ADD CONSTRAINT "PK_HET_RENTAL_AGREEMENT" PRIMARY KEY ("RENTAL_AGREEMENT_ID");


--
-- Name: HET_RENTAL_AGREEMENT_CONDITION PK_HET_RENTAL_AGREEMENT_CONDITION; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_CONDITION"
    ADD CONSTRAINT "PK_HET_RENTAL_AGREEMENT_CONDITION" PRIMARY KEY ("RENTAL_AGREEMENT_CONDITION_ID");


--
-- Name: HET_RENTAL_AGREEMENT_RATE PK_HET_RENTAL_AGREEMENT_RATE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_RATE"
    ADD CONSTRAINT "PK_HET_RENTAL_AGREEMENT_RATE" PRIMARY KEY ("RENTAL_AGREEMENT_RATE_ID");


--
-- Name: HET_RENTAL_AGREEMENT_STATUS_TYPE PK_HET_RENTAL_AGREEMENT_STATUS_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_AGREEMENT_STATUS_TYPE"
    ADD CONSTRAINT "PK_HET_RENTAL_AGREEMENT_STATUS_TYPE" PRIMARY KEY ("RENTAL_AGREEMENT_STATUS_TYPE_ID");


--
-- Name: HET_RENTAL_REQUEST PK_HET_RENTAL_REQUEST; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_REQUEST"
    ADD CONSTRAINT "PK_HET_RENTAL_REQUEST" PRIMARY KEY ("RENTAL_REQUEST_ID");


--
-- Name: HET_RENTAL_REQUEST_ATTACHMENT PK_HET_RENTAL_REQUEST_ATTACHMENT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_REQUEST_ATTACHMENT"
    ADD CONSTRAINT "PK_HET_RENTAL_REQUEST_ATTACHMENT" PRIMARY KEY ("RENTAL_REQUEST_ATTACHMENT_ID");


--
-- Name: HET_RENTAL_REQUEST_ROTATION_LIST PK_HET_RENTAL_REQUEST_ROTATION_LIST; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_REQUEST_ROTATION_LIST"
    ADD CONSTRAINT "PK_HET_RENTAL_REQUEST_ROTATION_LIST" PRIMARY KEY ("RENTAL_REQUEST_ROTATION_LIST_ID");


--
-- Name: HET_RENTAL_REQUEST_STATUS_TYPE PK_HET_RENTAL_REQUEST_STATUS_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_RENTAL_REQUEST_STATUS_TYPE"
    ADD CONSTRAINT "PK_HET_RENTAL_REQUEST_STATUS_TYPE" PRIMARY KEY ("RENTAL_REQUEST_STATUS_TYPE_ID");


--
-- Name: HET_ROLE PK_HET_ROLE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_ROLE"
    ADD CONSTRAINT "PK_HET_ROLE" PRIMARY KEY ("ROLE_ID");


--
-- Name: HET_ROLE_PERMISSION PK_HET_ROLE_PERMISSION; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_ROLE_PERMISSION"
    ADD CONSTRAINT "PK_HET_ROLE_PERMISSION" PRIMARY KEY ("ROLE_PERMISSION_ID");


--
-- Name: HET_SENIORITY_AUDIT PK_HET_SENIORITY_AUDIT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_SENIORITY_AUDIT"
    ADD CONSTRAINT "PK_HET_SENIORITY_AUDIT" PRIMARY KEY ("SENIORITY_AUDIT_ID");


--
-- Name: HET_SERVICE_AREA PK_HET_SERVICE_AREA; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_SERVICE_AREA"
    ADD CONSTRAINT "PK_HET_SERVICE_AREA" PRIMARY KEY ("SERVICE_AREA_ID");


--
-- Name: HET_TIME_PERIOD_TYPE PK_HET_TIME_PERIOD_TYPE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_TIME_PERIOD_TYPE"
    ADD CONSTRAINT "PK_HET_TIME_PERIOD_TYPE" PRIMARY KEY ("TIME_PERIOD_TYPE_ID");


--
-- Name: HET_TIME_RECORD PK_HET_TIME_RECORD; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_TIME_RECORD"
    ADD CONSTRAINT "PK_HET_TIME_RECORD" PRIMARY KEY ("TIME_RECORD_ID");


--
-- Name: HET_USER PK_HET_USER; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_USER"
    ADD CONSTRAINT "PK_HET_USER" PRIMARY KEY ("USER_ID");


--
-- Name: HET_USER_DISTRICT PK_HET_USER_DISTRICT; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_USER_DISTRICT"
    ADD CONSTRAINT "PK_HET_USER_DISTRICT" PRIMARY KEY ("USER_DISTRICT_ID");


--
-- Name: HET_USER_FAVOURITE PK_HET_USER_FAVOURITE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_USER_FAVOURITE"
    ADD CONSTRAINT "PK_HET_USER_FAVOURITE" PRIMARY KEY ("USER_FAVOURITE_ID");


--
-- Name: HET_USER_ROLE PK_HET_USER_ROLE; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HET_USER_ROLE"
    ADD CONSTRAINT "PK_HET_USER_ROLE" PRIMARY KEY ("USER_ROLE_ID");


--
-- Name: counter counter_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.counter
    ADD CONSTRAINT counter_pkey PRIMARY KEY (id);


--
-- Name: hash hash_key_field_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.hash
    ADD CONSTRAINT hash_key_field_key UNIQUE (key, field);


--
-- Name: hash hash_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.hash
    ADD CONSTRAINT hash_pkey PRIMARY KEY (id);


--
-- Name: job job_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (id);


--
-- Name: jobparameter jobparameter_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.jobparameter
    ADD CONSTRAINT jobparameter_pkey PRIMARY KEY (id);


--
-- Name: jobqueue jobqueue_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.jobqueue
    ADD CONSTRAINT jobqueue_pkey PRIMARY KEY (id);


--
-- Name: list list_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.list
    ADD CONSTRAINT list_pkey PRIMARY KEY (id);


--
-- Name: lock lock_resource_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.lock
    ADD CONSTRAINT lock_resource_key UNIQUE (resource);


--
-- Name: schema schema_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.schema
    ADD CONSTRAINT schema_pkey PRIMARY KEY (version);


--
-- Name: server server_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.server
    ADD CONSTRAINT server_pkey PRIMARY KEY (id);


--
-- Name: set set_key_value_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.set
    ADD CONSTRAINT set_key_value_key UNIQUE (key, value);


--
-- Name: set set_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.set
    ADD CONSTRAINT set_pkey PRIMARY KEY (id);


--
-- Name: state state_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);
