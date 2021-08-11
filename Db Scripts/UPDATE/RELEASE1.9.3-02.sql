CREATE SEQUENCE public."HET_RENTAL_REQUEST_SENIORITY_LIST_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;

CREATE TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" (
	"RENTAL_REQUEST_SENIORITY_LIST_ID" int4 NOT NULL DEFAULT nextval('"HET_RENTAL_REQUEST_SENIORITY_LIST_ID_seq"'::regclass),
	"RENTAL_REQUEST_ID" int4 NULL,
	"EQUIPMENT_ID" int4 NULL,
	"TYPE" varchar(50) NULL,
	"EQUIPMENT_CODE" varchar(25) NULL,
	"MAKE" varchar(50) NULL,
	"MODEL" varchar(50) NULL,
	"YEAR" varchar(15) NULL,
	"RECEIVED_DATE" timestamp NOT NULL,
	"YEARS_OF_SERVICE" float4 NULL,
	"LICENCE_PLATE" varchar(20) NULL,
	"SERIAL_NUMBER" varchar(100) NULL,
	"SIZE" varchar(128) NULL,
	"SENIORITY" float4 NULL,
	"SENIORITY_EFFECTIVE_DATE" timestamp NULL,
	"TO_DATE" timestamp NULL,
	"NUMBER_IN_BLOCK" int4 NULL,
	"BLOCK_NUMBER" int4 NULL,
	"SERVICE_HOURS_LAST_YEAR" float4 NULL,
	"SERVICE_HOURS_THREE_YEARS_AGO" float4 NULL,
	"SERVICE_HOURS_TWO_YEARS_AGO" float4 NULL,
	"IS_SENIORITY_OVERRIDDEN" bool NULL,
	"SENIORITY_OVERRIDE_REASON" varchar(2048) NULL,
	"APPROVED_DATE" timestamp NULL,
	"EQUIPMENT_STATUS_TYPE_ID" int4 NOT NULL,
	"STATUS_COMMENT" varchar(255) NULL,
	"ARCHIVE_DATE" timestamp NULL,
	"ARCHIVE_CODE" varchar(50) NULL,
	"ARCHIVE_REASON" varchar(2048) NULL,
	"LAST_VERIFIED_DATE" timestamp NOT NULL,
	"INFORMATION_UPDATE_NEEDED_REASON" varchar(2048) NULL,
	"IS_INFORMATION_UPDATE_NEEDED" bool NULL,
	"DISTRICT_EQUIPMENT_TYPE_ID" int4 NULL,
	"LOCAL_AREA_ID" int4 NULL,
	"OPERATOR" varchar(255) NULL,
	"OWNER_ID" int4 NULL,
	"PAY_RATE" float4 NULL,
	"REFUSE_RATE" varchar(255) NULL,
	"LEGAL_CAPACITY" varchar(150) NULL,
	"LICENCED_GVW" varchar(150) NULL,
	"PUP_LEGAL_CAPACITY" varchar(150) NULL,
	"APP_CREATE_USER_DIRECTORY" varchar(50) NULL,
	"APP_CREATE_USER_GUID" varchar(255) NULL,
	"APP_CREATE_USERID" varchar(255) NULL,
	"APP_CREATE_TIMESTAMP" timestamp NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
	"APP_LAST_UPDATE_USER_DIRECTORY" varchar(50) NULL,
	"APP_LAST_UPDATE_USER_GUID" varchar(255) NULL,
	"APP_LAST_UPDATE_USERID" varchar(255) NULL,
	"APP_LAST_UPDATE_TIMESTAMP" timestamp NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
	"DB_CREATE_USER_ID" varchar(63) NULL,
	"DB_CREATE_TIMESTAMP" timestamp NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
	"DB_LAST_UPDATE_TIMESTAMP" timestamp NOT NULL DEFAULT '0001-01-01 00:00:00'::timestamp without time zone,
	"DB_LAST_UPDATE_USER_ID" varchar(63) NULL,
	"CONCURRENCY_CONTROL_NUMBER" int4 NOT NULL DEFAULT 0,
	CONSTRAINT "PK_HET_RENTAL_REQUEST_SENIORITY_LIST" PRIMARY KEY ("RENTAL_REQUEST_SENIORITY_LIST_ID")
);

CREATE INDEX "IX_HET_RENTAL_REQUEST_SENIORITY_LIST_EQUIPMENT_ID" ON public."HET_RENTAL_REQUEST_SENIORITY_LIST" USING btree ("EQUIPMENT_ID");
CREATE INDEX "IX_HET_RENTAL_REQUEST_SENIORITY_LIST_RENTAL_REQUEST_ID" ON public."HET_RENTAL_REQUEST_SENIORITY_LIST" USING btree ("RENTAL_REQUEST_ID");

-- Table Triggers

create trigger het_rntrsl_br_iu_tr before
insert
    or
update
    on
    public."HET_RENTAL_REQUEST_SENIORITY_LIST" for each row execute procedure het_validate_init_audit_cols();


-- public."HET_RENTAL_REQUEST_SENIORITY_LIST" foreign keys

ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_DISTRICT_EQUIPMENT_TYPE_ID" FOREIGN KEY ("DISTRICT_EQUIPMENT_TYPE_ID") REFERENCES public."HET_DISTRICT_EQUIPMENT_TYPE"("DISTRICT_EQUIPMENT_TYPE_ID");
ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_LOCAL_AREA_ID" FOREIGN KEY ("LOCAL_AREA_ID") REFERENCES public."HET_LOCAL_AREA"("LOCAL_AREA_ID");
ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_OWNER_ID" FOREIGN KEY ("OWNER_ID") REFERENCES public."HET_OWNER"("OWNER_ID");
ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_STATUS_TYPE_ID" FOREIGN KEY ("EQUIPMENT_STATUS_TYPE_ID") REFERENCES public."HET_EQUIPMENT_STATUS_TYPE"("EQUIPMENT_STATUS_TYPE_ID");
ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_EQUIPMENT_ID" FOREIGN KEY ("EQUIPMENT_ID") REFERENCES public."HET_EQUIPMENT"("EQUIPMENT_ID");
ALTER TABLE public."HET_RENTAL_REQUEST_SENIORITY_LIST" ADD CONSTRAINT "FK_HET_RENTAL_REQUEST_SENIORITY_LIST_RENTAL_REQUEST_ID" FOREIGN KEY ("RENTAL_REQUEST_ID") REFERENCES public."HET_RENTAL_REQUEST"("RENTAL_REQUEST_ID");

INSERT INTO public."HET_RENTAL_REQUEST_SENIORITY_LIST"
	("RENTAL_REQUEST_ID", "EQUIPMENT_ID", "TYPE", "EQUIPMENT_CODE", "MAKE", "MODEL", "YEAR", "RECEIVED_DATE", "YEARS_OF_SERVICE", "LICENCE_PLATE", "SERIAL_NUMBER", "SIZE", "SENIORITY", "SENIORITY_EFFECTIVE_DATE", "TO_DATE", "NUMBER_IN_BLOCK", "BLOCK_NUMBER", "SERVICE_HOURS_LAST_YEAR", "SERVICE_HOURS_THREE_YEARS_AGO", "SERVICE_HOURS_TWO_YEARS_AGO", "IS_SENIORITY_OVERRIDDEN", "SENIORITY_OVERRIDE_REASON", "APPROVED_DATE", "EQUIPMENT_STATUS_TYPE_ID", "STATUS_COMMENT", "ARCHIVE_DATE", "ARCHIVE_CODE", "ARCHIVE_REASON", "LAST_VERIFIED_DATE", "INFORMATION_UPDATE_NEEDED_REASON", "IS_INFORMATION_UPDATE_NEEDED", "DISTRICT_EQUIPMENT_TYPE_ID", "LOCAL_AREA_ID", "OPERATOR", "OWNER_ID", "PAY_RATE", "REFUSE_RATE", "LEGAL_CAPACITY", "LICENCED_GVW", "PUP_LEGAL_CAPACITY", "APP_CREATE_USERID", "APP_CREATE_TIMESTAMP", "APP_LAST_UPDATE_USERID", "APP_LAST_UPDATE_TIMESTAMP", "CONCURRENCY_CONTROL_NUMBER")
	SELECT r."RENTAL_REQUEST_ID", h."EQUIPMENT_ID", h."TYPE", h."EQUIPMENT_CODE", h."MAKE", h."MODEL", h."YEAR", h."RECEIVED_DATE", h."YEARS_OF_SERVICE", h."LICENCE_PLATE", h."SERIAL_NUMBER", h."SIZE", h."SENIORITY", h."SENIORITY_EFFECTIVE_DATE", h."TO_DATE", h."NUMBER_IN_BLOCK", h."BLOCK_NUMBER", h."SERVICE_HOURS_LAST_YEAR", h."SERVICE_HOURS_THREE_YEARS_AGO", h."SERVICE_HOURS_TWO_YEARS_AGO", h."IS_SENIORITY_OVERRIDDEN", h."SENIORITY_OVERRIDE_REASON", h."APPROVED_DATE", h."EQUIPMENT_STATUS_TYPE_ID", h."STATUS_COMMENT", h."ARCHIVE_DATE", h."ARCHIVE_CODE", h."ARCHIVE_REASON", h."LAST_VERIFIED_DATE", h."INFORMATION_UPDATE_NEEDED_REASON", h."IS_INFORMATION_UPDATE_NEEDED", h."DISTRICT_EQUIPMENT_TYPE_ID", h."LOCAL_AREA_ID", h."OPERATOR", h."OWNER_ID", h."PAY_RATE", h."REFUSE_RATE", h."LEGAL_CAPACITY", h."LICENCED_GVW", h."PUP_LEGAL_CAPACITY", 
		   'SYSTEM_HETS' as "APP_CREATE_USERID", NOW() as "APP_CREATE_TIMESTAMP", 'SYSTEM_HETS' as "APP_LAST_UPDATE_USERID", NOW() as "APP_LAST_UPDATE_TIMESTAMP", 1 as "CONCURRENCY_CONTROL_NUMBER"
    FROM public."HET_EQUIPMENT_HIST" h, public."HET_RENTAL_REQUEST" r 
	WHERE h."LOCAL_AREA_ID" = r."LOCAL_AREA_ID" AND h."DISTRICT_EQUIPMENT_TYPE_ID" = r."DISTRICT_EQUIPMENT_TYPE_ID" AND h."EQUIPMENT_STATUS_TYPE_ID" = 4
	  AND r."DB_CREATE_TIMESTAMP" between h."EFFECTIVE_DATE" AND COALESCE(h."END_DATE", NOW());

alter table public."HET_RENTAL_REQUEST_ROTATION_LIST" add column "BLOCK_NUMBER" int4;
alter table public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" add column "BLOCK_NUMBER" int4;

CREATE OR REPLACE FUNCTION public.het_rntrrl_ar_iud_tr()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
            (
			 "RENTAL_REQUEST_ROTATION_LIST_HIST_ID",
             "RENTAL_REQUEST_ROTATION_LIST_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ASKED_DATE_TIME",
			 "EQUIPMENT_ID",
			 "IS_FORCE_HIRE",
			 "NOTE",
			 "OFFER_RESPONSE",
			 "OFFER_RESPONSE_NOTE",
			 "RENTAL_AGREEMENT_ID",
			 "RENTAL_REQUEST_ID",
			 "ROTATION_LIST_SORT_ORDER",
			 "WAS_ASKED",
			 "OFFER_REFUSAL_REASON",
			 "OFFER_RESPONSE_DATETIME",
			 "BLOCK_NUMBER",
			 "CONCURRENCY_CONTROL_NUMBER",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "APP_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID",
			 "APP_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_GUID",
			 "APP_LAST_UPDATE_USERID",
			 "DB_CREATE_USER_ID",
			 "DB_LAST_UPDATE_USER_ID"
            )
            VALUES( 
              nextval('"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass),
              NEW."RENTAL_REQUEST_ROTATION_LIST_ID",
              l_current_timestamp,
              NULL,
			  NEW."ASKED_DATE_TIME",
			  NEW."EQUIPMENT_ID",
			  NEW."IS_FORCE_HIRE",
			  NEW."NOTE",
			  NEW."OFFER_RESPONSE",
			  NEW."OFFER_RESPONSE_NOTE",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."RENTAL_REQUEST_ID",
			  NEW."ROTATION_LIST_SORT_ORDER",
			  NEW."WAS_ASKED",
			  NEW."OFFER_REFUSAL_REASON",
			  NEW."OFFER_RESPONSE_DATETIME",
			  NEW."BLOCK_NUMBER",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."APP_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_GUID",
			  NEW."APP_CREATE_USERID",
			  NEW."APP_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_GUID",
			  NEW."APP_LAST_UPDATE_USERID",
			  NEW."DB_CREATE_USER_ID",
			  NEW."DB_LAST_UPDATE_USER_ID"
             );
             RETURN NEW;
        ELSIF (TG_OP = 'UPDATE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" rntrrlhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntrrlhis."RENTAL_REQUEST_ROTATION_LIST_ID" = NEW."RENTAL_REQUEST_ROTATION_LIST_ID"
            AND rntrrlhis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
            (
			 "RENTAL_REQUEST_ROTATION_LIST_HIST_ID",
             "RENTAL_REQUEST_ROTATION_LIST_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ASKED_DATE_TIME",
			 "EQUIPMENT_ID",
			 "IS_FORCE_HIRE",
			 "NOTE",
			 "OFFER_RESPONSE",
			 "OFFER_RESPONSE_NOTE",
			 "RENTAL_AGREEMENT_ID",
			 "RENTAL_REQUEST_ID",
			 "ROTATION_LIST_SORT_ORDER",
			 "WAS_ASKED",
			 "OFFER_REFUSAL_REASON",
			 "OFFER_RESPONSE_DATETIME",
			 "BLOCK_NUMBER",
			 "CONCURRENCY_CONTROL_NUMBER",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "APP_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID",
			 "APP_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_GUID",
			 "APP_LAST_UPDATE_USERID",
			 "DB_CREATE_USER_ID",
			 "DB_LAST_UPDATE_USER_ID"
            )
            VALUES( 
              nextval('"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass),
              NEW."RENTAL_REQUEST_ROTATION_LIST_ID",
              l_current_timestamp,
              NULL,
			  NEW."ASKED_DATE_TIME",
			  NEW."EQUIPMENT_ID",
			  NEW."IS_FORCE_HIRE",
			  NEW."NOTE",
			  NEW."OFFER_RESPONSE",
			  NEW."OFFER_RESPONSE_NOTE",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."RENTAL_REQUEST_ID",
			  NEW."ROTATION_LIST_SORT_ORDER",
			  NEW."WAS_ASKED",
			  NEW."OFFER_REFUSAL_REASON",
			  NEW."OFFER_RESPONSE_DATETIME",
			  NEW."BLOCK_NUMBER",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."APP_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_GUID",
			  NEW."APP_CREATE_USERID",
			  NEW."APP_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_GUID",
			  NEW."APP_LAST_UPDATE_USERID",
			  NEW."DB_CREATE_USER_ID",
			  NEW."DB_LAST_UPDATE_USER_ID"
             );
     	RETURN NEW;
        ELSIF (TG_OP = 'DELETE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" rntrrlhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntrrlhis."RENTAL_REQUEST_ROTATION_LIST_ID" = OLD."RENTAL_REQUEST_ROTATION_LIST_ID"
            AND rntrrlhis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$function$
;

update public."HET_RENTAL_REQUEST_ROTATION_LIST" a
set "BLOCK_NUMBER" = b."BLOCK_NUMBER", "CONCURRENCY_CONTROL_NUMBER" = a."CONCURRENCY_CONTROL_NUMBER" + 1
from public."HET_RENTAL_REQUEST_SENIORITY_LIST" b 
where b."RENTAL_REQUEST_ID" = a."RENTAL_REQUEST_ID" and b."EQUIPMENT_ID" = a."EQUIPMENT_ID" ;