-- ********************************************************************	
-- FUNCTION: public.het_validate_init_audit_cols()
CREATE FUNCTION public.het_validate_init_audit_cols() 
    RETURNS trigger
    LANGUAGE 'plpgsql'
    VOLATILE NOT LEAKPROOF
AS $BODY$

	DECLARE
    l_current_timestamp timestamp;
	BEGIN
    	l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
            NEW."DB_CREATE_TIMESTAMP" := l_current_timestamp;
            NEW."DB_CREATE_USER_ID" := current_user;
			NEW."DB_LAST_UPDATE_TIMESTAMP" := l_current_timestamp;
            NEW."DB_LAST_UPDATE_USER_ID" := current_user;
           	RETURN NEW;
        ELSIF (TG_OP = 'UPDATE') THEN
         	IF (OLD."DB_CREATE_USER_ID" <> NEW."DB_CREATE_USER_ID") THEN
            	RAISE 'DB_CREATE_USER_ID CANNOT BE MODIFIED DURING AN UPDATE %', NEW."DB_CREATE_USER_ID" USING ERRCODE = '20010';
            END IF;
        	IF (OLD."DB_CREATE_TIMESTAMP" <> NEW."DB_CREATE_TIMESTAMP") THEN
            	RAISE 'DB_CREATE_TIMESTAMP CANNOT BE MODIFIED DURING AN UPDATE %', NEW."DB_CREATE_TIMESTAMP" USING ERRCODE = '20020';
            END IF;
            IF (OLD."APP_CREATE_USERID" <> NEW."APP_CREATE_USERID") THEN
            	RAISE 'APP_CREATE_USERID CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_USERID" USING ERRCODE = '20140';
            END IF;
        	IF (OLD."APP_CREATE_TIMESTAMP" <> NEW."APP_CREATE_TIMESTAMP") THEN
            	RAISE 'APP_CREATE_TIMESTAMP CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_TIMESTAMP" USING ERRCODE = '20150';
            END IF;
            IF (NEW."APP_CREATE_USER_GUID" IS NOT NULL AND OLD."APP_CREATE_USER_GUID" IS NOT NULL) AND 
            	(NEW."APP_CREATE_USER_GUID" <> OLD."APP_CREATE_USER_GUID")THEN
            	RAISE 'APP_CREATE_USER_GUID CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_USER_GUID" USING ERRCODE = '20160';
            END IF;
            IF (NEW."APP_CREATE_USER_GUID" IS NOT NULL AND OLD."APP_CREATE_USER_GUID" IS NULL) THEN
            	RAISE 'APP_CREATE_USER_GUID CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_USER_GUID" USING ERRCODE = '20160';
            END IF;
            IF (NEW."APP_CREATE_USER_GUID" IS NULL AND OLD."APP_CREATE_USER_GUID" IS NOT NULL) THEN
            	RAISE 'APP_CREATE_USER_GUID CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_USER_GUID" USING ERRCODE = '20160';
            END IF;
            IF (OLD."APP_CREATE_USER_DIRECTORY" <> NEW."APP_CREATE_USER_DIRECTORY") THEN
            	RAISE 'APP_CREATE_USER_DIRECTORY CANNOT BE MODIFIED DURING AN UPDATE %', NEW."APP_CREATE_USER_DIRECTORY" USING ERRCODE = '20170';
            END IF;
            IF(NEW."CONCURRENCY_CONTROL_NUMBER" <> OLD."CONCURRENCY_CONTROL_NUMBER"+1) THEN
            	RAISE 'Concurrency Failure %', NEW."CONCURRENCY_CONTROL_NUMBER" USING ERRCODE = '20180';
            END IF;
            NEW."DB_LAST_UPDATE_TIMESTAMP" := l_current_timestamp;
            NEW."DB_LAST_UPDATE_USER_ID" := current_user;
           
            RETURN NEW;
         END IF;     
   	END;

$BODY$;

ALTER FUNCTION public.het_validate_init_audit_cols()
    OWNER TO "postgres";

-- ********************************************************************	
-- FUNCTION: public.het_eqp_ar_iud_tr()

CREATE FUNCTION public.het_eqp_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
  
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_EQUIPMENT_HIST"
            ("EQUIPMENT_HIST_ID",
             "EQUIPMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
             "APPROVED_DATE",
             "ARCHIVE_DATE",
             "ARCHIVE_REASON",
             "BLOCK_NUMBER",
             "ARCHIVE_CODE",
             "DISTRICT_EQUIPMENT_TYPE_ID",
             "EQUIPMENT_CODE",
             "LOCAL_AREA_ID",
             "MAKE",
             "MODEL",
             "OPERATOR",
             "OWNER_ID",
             "PAY_RATE",
             "RECEIVED_DATE",
             "REFUSE_RATE",
             "LICENCE_PLATE",
             "SENIORITY",
             "SERIAL_NUMBER",
             "SIZE",
             "STATUS",
             "TO_DATE",
             "YEARS_OF_SERVICE",
             "SERVICE_HOURS_LAST_YEAR",
             "SERVICE_HOURS_THREE_YEARS_AGO",
             "SERVICE_HOURS_TWO_YEARS_AGO",
             "YEAR",
             "LAST_VERIFIED_DATE",
             "INFORMATION_UPDATE_NEEDED_REASON",
             "IS_INFORMATION_UPDATE_NEEDED",
             "SENIORITY_EFFECTIVE_DATE",
             "IS_SENIORITY_OVERRIDDEN",
             "SENIORITY_OVERRIDE_REASON",
             "NUMBER_IN_BLOCK",
             "DB_CREATE_TIMESTAMP",
             "DB_CREATE_USER_ID",
             "DB_LAST_UPDATE_TIMESTAMP",
             "DB_LAST_UPDATE_USER_ID",
             "APP_CREATE_TIMESTAMP",
             "APP_CREATE_USERID",
             "APP_CREATE_USER_GUID",
             "APP_CREATE_USER_DIRECTORY",
             "APP_LAST_UPDATE_TIMESTAMP",
             "APP_LAST_UPDATE_USERID",
             "APP_LAST_UPDATE_USER_GUID",
             "APP_LAST_UPDATE_USER_DIRECTORY",
			 "TYPE",
			 "STATUS_COMMENT",
			 "LEGAL_CAPACITY",
			 "LICENCED_GVW",
			 "PUP_LEGAL_CAPACITY",
             "CONCURRENCY_CONTROL_NUMBER"
			 
            )
            VALUES( 
              nextval('"HET_EQUIPMENT_HIST_ID_seq"'::regclass),
              NEW."EQUIPMENT_ID",
              l_current_timestamp,
              NULL,
              NEW."APPROVED_DATE",
              NEW."ARCHIVE_DATE",
              NEW."ARCHIVE_REASON",
              NEW."BLOCK_NUMBER",
              NEW."ARCHIVE_CODE",
              NEW."DISTRICT_EQUIPMENT_TYPE_ID",
              NEW."EQUIPMENT_CODE",
              NEW."LOCAL_AREA_ID",
              NEW."MAKE",
              NEW."MODEL",
              NEW."OPERATOR",
              NEW."OWNER_ID",
              NEW."PAY_RATE",
              NEW."RECEIVED_DATE",
              NEW."REFUSE_RATE",
              NEW."LICENCE_PLATE",
              NEW."SENIORITY",
              NEW."SERIAL_NUMBER",
              NEW."SIZE",
              NEW."STATUS",
              NEW."TO_DATE",
              NEW."YEARS_OF_SERVICE",
              NEW."SERVICE_HOURS_LAST_YEAR",
              NEW."SERVICE_HOURS_THREE_YEARS_AGO",
              NEW."SERVICE_HOURS_TWO_YEARS_AGO",
              NEW."YEAR",
              NEW."LAST_VERIFIED_DATE",
              NEW."INFORMATION_UPDATE_NEEDED_REASON",
              NEW."IS_INFORMATION_UPDATE_NEEDED",
              NEW."SENIORITY_EFFECTIVE_DATE",
              NEW."IS_SENIORITY_OVERRIDDEN",
              NEW."SENIORITY_OVERRIDE_REASON",
              NEW."NUMBER_IN_BLOCK",
              NEW."DB_CREATE_TIMESTAMP",
              NEW."DB_CREATE_USER_ID",
              NEW."DB_LAST_UPDATE_TIMESTAMP",
              NEW."DB_LAST_UPDATE_USER_ID",
              NEW."APP_CREATE_TIMESTAMP",
              NEW."APP_CREATE_USERID",
              NEW."APP_CREATE_USER_GUID",
              NEW."APP_CREATE_USER_DIRECTORY",
              NEW."APP_LAST_UPDATE_TIMESTAMP",
              NEW."APP_LAST_UPDATE_USERID",
              NEW."APP_LAST_UPDATE_USER_GUID",
              NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."TYPE",
			  NEW."STATUS_COMMENT",
			  NEW."LEGAL_CAPACITY",
			  NEW."LICENCED_GVW",
			  NEW."PUP_LEGAL_CAPACITY",
              NEW."CONCURRENCY_CONTROL_NUMBER"
             );
             RETURN NEW;
        ELSIF (TG_OP = 'UPDATE') THEN
        	---- First update the previously active row
            UPDATE public."HET_EQUIPMENT_HIST" eqphis
            SET "END_DATE" = l_current_timestamp
            WHERE eqphis."EQUIPMENT_ID" = NEW."EQUIPMENT_ID"
            AND eqphis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_EQUIPMENT_HIST"
            ("EQUIPMENT_HIST_ID",
             "EQUIPMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
             "APPROVED_DATE",
             "ARCHIVE_DATE",
             "ARCHIVE_REASON",
             "BLOCK_NUMBER",
             "ARCHIVE_CODE",
             "DISTRICT_EQUIPMENT_TYPE_ID",
             "EQUIPMENT_CODE",
             "LOCAL_AREA_ID",
             "MAKE",
             "MODEL",
             "OPERATOR",
             "OWNER_ID",
             "PAY_RATE",
             "RECEIVED_DATE",
             "REFUSE_RATE",
             "LICENCE_PLATE",
             "SENIORITY",
             "SERIAL_NUMBER",
             "SIZE",
             "STATUS",
             "TO_DATE",
             "YEARS_OF_SERVICE",
             "SERVICE_HOURS_LAST_YEAR",
             "SERVICE_HOURS_THREE_YEARS_AGO",
             "SERVICE_HOURS_TWO_YEARS_AGO",
             "YEAR",
             "LAST_VERIFIED_DATE",
             "INFORMATION_UPDATE_NEEDED_REASON",
             "IS_INFORMATION_UPDATE_NEEDED",
             "SENIORITY_EFFECTIVE_DATE",
             "IS_SENIORITY_OVERRIDDEN",
             "SENIORITY_OVERRIDE_REASON",
             "NUMBER_IN_BLOCK",
             "DB_CREATE_TIMESTAMP",
             "DB_CREATE_USER_ID",
             "DB_LAST_UPDATE_TIMESTAMP",
             "DB_LAST_UPDATE_USER_ID",
             "APP_CREATE_TIMESTAMP",
             "APP_CREATE_USERID",
             "APP_CREATE_USER_GUID",
             "APP_CREATE_USER_DIRECTORY",
             "APP_LAST_UPDATE_TIMESTAMP",
             "APP_LAST_UPDATE_USERID",
             "APP_LAST_UPDATE_USER_GUID",
             "APP_LAST_UPDATE_USER_DIRECTORY",
			 "TYPE",
			 "STATUS_COMMENT",
			 "LEGAL_CAPACITY",
			 "LICENCED_GVW",
			 "PUP_LEGAL_CAPACITY",
             "CONCURRENCY_CONTROL_NUMBER"
            )
            VALUES( 
              nextval('"HET_EQUIPMENT_HIST_ID_seq"'::regclass),
              NEW."EQUIPMENT_ID",
              l_current_timestamp,
              NULL,
              NEW."APPROVED_DATE",
              NEW."ARCHIVE_DATE",
              NEW."ARCHIVE_REASON",
              NEW."BLOCK_NUMBER",
              NEW."ARCHIVE_CODE",
              NEW."DISTRICT_EQUIPMENT_TYPE_ID",
              NEW."EQUIPMENT_CODE",
              NEW."LOCAL_AREA_ID",
              NEW."MAKE",
              NEW."MODEL",
              NEW."OPERATOR",
              NEW."OWNER_ID",
              NEW."PAY_RATE",
              NEW."RECEIVED_DATE",
              NEW."REFUSE_RATE",
              NEW."LICENCE_PLATE",
              NEW."SENIORITY",
              NEW."SERIAL_NUMBER",
              NEW."SIZE",
              NEW."STATUS",
              NEW."TO_DATE",
              NEW."YEARS_OF_SERVICE",
              NEW."SERVICE_HOURS_LAST_YEAR",
              NEW."SERVICE_HOURS_THREE_YEARS_AGO",
              NEW."SERVICE_HOURS_TWO_YEARS_AGO",
              NEW."YEAR",
              NEW."LAST_VERIFIED_DATE",
              NEW."INFORMATION_UPDATE_NEEDED_REASON",
              NEW."IS_INFORMATION_UPDATE_NEEDED",
              NEW."SENIORITY_EFFECTIVE_DATE",
              NEW."IS_SENIORITY_OVERRIDDEN",
              NEW."SENIORITY_OVERRIDE_REASON",
              NEW."NUMBER_IN_BLOCK",
              NEW."DB_CREATE_TIMESTAMP",
              NEW."DB_CREATE_USER_ID",
              NEW."DB_LAST_UPDATE_TIMESTAMP",
              NEW."DB_LAST_UPDATE_USER_ID",
              NEW."APP_CREATE_TIMESTAMP",
              NEW."APP_CREATE_USERID",
              NEW."APP_CREATE_USER_GUID",
              NEW."APP_CREATE_USER_DIRECTORY",
              NEW."APP_LAST_UPDATE_TIMESTAMP",
              NEW."APP_LAST_UPDATE_USERID",
              NEW."APP_LAST_UPDATE_USER_GUID",
              NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."TYPE",
			  NEW."STATUS_COMMENT",
			  NEW."LEGAL_CAPACITY",
			  NEW."LICENCED_GVW",
			  NEW."PUP_LEGAL_CAPACITY",
              NEW."CONCURRENCY_CONTROL_NUMBER"
             );
     	RETURN NEW;
        ELSIF (TG_OP = 'DELETE') THEN
        	---- First update the previously active row
            UPDATE public."HET_EQUIPMENT_HIST" eqphis
            SET "END_DATE" = l_current_timestamp
            WHERE eqphis."EQUIPMENT_ID" = OLD."EQUIPMENT_ID"
            AND eqphis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_eqp_ar_iud_tr()
    OWNER TO "postgres";
	
-- ********************************************************************	
-- FUNCTION: public.het_eqpatt_ar_iud_tr()

CREATE FUNCTION public.het_eqpatt_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_EQUIPMENT_ATTACHMENT_HIST"
            (
			 "EQUIPMENT_ATTACHMENT_HIST_ID",
             "EQUIPMENT_ATTACHMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "DESCRIPTION",
			 "EQUIPMENT_ID",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "TYPE_NAME",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"'::regclass),
              NEW."EQUIPMENT_ATTACHMENT_ID",
              l_current_timestamp,
              NULL,
              NEW."DESCRIPTION",
			  NEW."EQUIPMENT_ID",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."TYPE_NAME",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_EQUIPMENT_ATTACHMENT_HIST" eqpatthis
            SET "END_DATE" = l_current_timestamp
            WHERE eqpatthis."EQUIPMENT_ATTACHMENT_ID" = NEW."EQUIPMENT_ATTACHMENT_ID"
            AND eqpatthis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_EQUIPMENT_ATTACHMENT_HIST"
            (
			 "EQUIPMENT_ATTACHMENT_HIST_ID",
             "EQUIPMENT_ATTACHMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "DESCRIPTION",
			 "EQUIPMENT_ID",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "TYPE_NAME",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"'::regclass),
              NEW."EQUIPMENT_ATTACHMENT_ID",
              l_current_timestamp,
              NULL,
              NEW."DESCRIPTION",
			  NEW."EQUIPMENT_ID",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."TYPE_NAME",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_EQUIPMENT_ATTACHMENT_HIST" eqpatthis
            SET "END_DATE" = l_current_timestamp
            WHERE eqpatthis."EQUIPMENT_ATTACHMENT_ID" = OLD."EQUIPMENT_ATTACHMENT_ID"
            AND eqpatthis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_eqpatt_ar_iud_tr()
    OWNER TO "postgres";
	
-- ********************************************************************
-- FUNCTION: public.het_note_ar_iud_tr()

CREATE FUNCTION public.het_note_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_NOTE_HIST"
            (
			 "NOTE_HIST_ID",
             "NOTE_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "EQUIPMENT_ID",
			 "IS_NO_LONGER_RELEVANT",
			 "OWNER_ID",
			 "PROJECT_ID",
			 "TEXT",
			 "RENTAL_REQUEST_ID",
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
              nextval('"HET_NOTE_HIST_ID_seq"'::regclass),
              NEW."NOTE_ID",
              l_current_timestamp,
              NULL,
			  NEW."EQUIPMENT_ID",
			  NEW."IS_NO_LONGER_RELEVANT",
			  NEW."OWNER_ID",
			  NEW."PROJECT_ID",
			  NEW."TEXT",
			  NEW."RENTAL_REQUEST_ID",
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
            UPDATE public."HET_NOTE_HIST" notehis
            SET "END_DATE" = l_current_timestamp
            WHERE notehis."NOTE_ID" = NEW."NOTE_ID"
            AND notehis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_NOTE_HIST"
            (
			 "NOTE_HIST_ID",
             "NOTE_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "EQUIPMENT_ID",
			 "IS_NO_LONGER_RELEVANT",
			 "OWNER_ID",
			 "PROJECT_ID",
			 "TEXT",
			 "RENTAL_REQUEST_ID",
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
              nextval('"HET_NOTE_HIST_ID_seq"'::regclass),
              NEW."NOTE_ID",
              l_current_timestamp,
              NULL,
			  NEW."EQUIPMENT_ID",
			  NEW."IS_NO_LONGER_RELEVANT",
			  NEW."OWNER_ID",
			  NEW."PROJECT_ID",
			  NEW."TEXT",
			  NEW."RENTAL_REQUEST_ID",
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
            UPDATE public."HET_NOTE_HIST" notehis
            SET "END_DATE" = l_current_timestamp
            WHERE notehis."NOTE_ID" = OLD."NOTE_ID"
            AND notehis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_note_ar_iud_tr()
    OWNER TO "postgres";
	
-- ********************************************************************
-- FUNCTION: public.het_rntag_ar_iud_tr()

CREATE FUNCTION public.het_rntag_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_RENTAL_AGREEMENT_HIST"
            (
			 "RENTAL_AGREEMENT_HIST_ID",
             "RENTAL_AGREEMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "EQUIPMENT_ID",
			 "PROJECT_ID",
			 "STATUS",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DATED_ON",
			 "EQUIPMENT_RATE",
			 "ESTIMATE_HOURS",
			 "ESTIMATE_START_WORK",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "NOTE",
			 "NUMBER",
			 "RATE_COMMENT",
			 "RATE_PERIOD",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_RENTAL_AGREEMENT_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_ID",
              l_current_timestamp,
              NULL,
			  NEW."EQUIPMENT_ID",
			  NEW."PROJECT_ID",
			  NEW."STATUS",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DATED_ON",
			  NEW."EQUIPMENT_RATE",
			  NEW."ESTIMATE_HOURS",
		 	  NEW."ESTIMATE_START_WORK",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."NOTE",
			  NEW."NUMBER",
			  NEW."RATE_COMMENT",
			  NEW."RATE_PERIOD",
			  NEW."CONCURRENCY_CONTROL_NUMBER",	
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
            UPDATE public."HET_RENTAL_AGREEMENT_HIST" rntaghis
            SET "END_DATE" = l_current_timestamp
            WHERE rntaghis."RENTAL_AGREEMENT_ID" = NEW."RENTAL_AGREEMENT_ID"
            AND rntaghis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_RENTAL_AGREEMENT_HIST"
            (
			 "RENTAL_AGREEMENT_HIST_ID",
             "RENTAL_AGREEMENT_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "EQUIPMENT_ID",
			 "PROJECT_ID",
			 "STATUS",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DATED_ON",
			 "EQUIPMENT_RATE",
			 "ESTIMATE_HOURS",
			 "ESTIMATE_START_WORK",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "NOTE",
			 "NUMBER",
			 "RATE_COMMENT",
			 "RATE_PERIOD",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_RENTAL_AGREEMENT_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_ID",
              l_current_timestamp,
              NULL,
			  NEW."EQUIPMENT_ID",
			  NEW."PROJECT_ID",
			  NEW."STATUS",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DATED_ON",
			  NEW."EQUIPMENT_RATE",
			  NEW."ESTIMATE_HOURS",
		 	  NEW."ESTIMATE_START_WORK",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."NOTE",
			  NEW."NUMBER",
			  NEW."RATE_COMMENT",
			  NEW."RATE_PERIOD",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_RENTAL_AGREEMENT_HIST" rntaghis
            SET "END_DATE" = l_current_timestamp
            WHERE rntaghis."RENTAL_AGREEMENT_ID" = OLD."RENTAL_AGREEMENT_ID"
            AND rntaghis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_rntag_ar_iud_tr()
    OWNER TO "postgres";
	
-- ********************************************************************	
-- FUNCTION: public.het_rntagc_ar_iud_tr()

CREATE FUNCTION public.het_rntagc_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_RENTAL_AGREEMENT_CONDITION_HIST"
            (
			 "RENTAL_AGREEMENT_CONDITION_HIST_ID",
             "RENTAL_AGREEMENT_CONDITION_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "COMMENT",
			 "CONDITION_NAME",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "RENTAL_AGREEMENT_ID",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_CONDITION_ID",
              l_current_timestamp,
              NULL,
			  NEW."COMMENT", 
			  NEW."CONDITION_NAME", 
			  NEW."DB_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_DIRECTORY", 
			  NEW."DB_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY", 
			  NEW."RENTAL_AGREEMENT_ID", 
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_RENTAL_AGREEMENT_CONDITION_HIST" rntagchis
            SET "END_DATE" = l_current_timestamp
            WHERE rntagchis."RENTAL_AGREEMENT_CONDITION_ID" = NEW."RENTAL_AGREEMENT_CONDITION_ID"
            AND rntagchis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_RENTAL_AGREEMENT_CONDITION_HIST"
            (
			 "RENTAL_AGREEMENT_CONDITION_HIST_ID",
             "RENTAL_AGREEMENT_CONDITION_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "COMMENT",
			 "CONDITION_NAME",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "RENTAL_AGREEMENT_ID",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_CONDITION_ID",
              l_current_timestamp,
              NULL,
			  NEW."COMMENT", 
			  NEW."CONDITION_NAME", 
			  NEW."DB_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_DIRECTORY", 
			  NEW."DB_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY", 
			  NEW."RENTAL_AGREEMENT_ID", 
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_RENTAL_AGREEMENT_CONDITION_HIST" rntagchis
            SET "END_DATE" = l_current_timestamp
            WHERE rntagchis."RENTAL_AGREEMENT_CONDITION_ID" = OLD."RENTAL_AGREEMENT_CONDITION_ID"
            AND rntagchis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_rntagc_ar_iud_tr()
    OWNER TO "postgres";
	
-- ********************************************************************	
-- FUNCTION: public.het_rntagr_ar_iud_tr()

CREATE FUNCTION public.het_rntagr_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_RENTAL_AGREEMENT_RATE_HIST"
            (
			 "RENTAL_AGREEMENT_RATE_HIST_ID",
             "RENTAL_AGREEMENT_RATE_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "COMMENT",
			 "COMPONENT_NAME", 
			 "DB_CREATE_TIMESTAMP", 
			 "APP_CREATE_USER_DIRECTORY", 
			 "IS_ATTACHMENT", 
			 "DB_LAST_UPDATE_TIMESTAMP", 
			 "APP_LAST_UPDATE_USER_DIRECTORY", 
			 "PERCENT_OF_EQUIPMENT_RATE",
			 "RATE", 
			 "RATE_PERIOD", 
			 "RENTAL_AGREEMENT_ID", 
			 "CONCURRENCY_CONTROL_NUMBER",
			 "APP_CREATE_TIMESTAMP", 
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID", 
			 "APP_LAST_UPDATE_TIMESTAMP", 
			 "APP_LAST_UPDATE_USER_GUID", 
			 "APP_LAST_UPDATE_USERID", 
			 "DB_CREATE_USER_ID", 
			 "DB_LAST_UPDATE_USER_ID", 
			 "IS_INCLUDED_IN_TOTAL" 
            )
            VALUES( 
              nextval('"HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_RATE_ID",
              l_current_timestamp,
              NULL,
			  NEW."COMMENT", 
			  NEW."COMPONENT_NAME", 
			  NEW."DB_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_DIRECTORY", 
			  NEW."IS_ATTACHMENT", 
			  NEW."DB_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY", 
			  NEW."PERCENT_OF_EQUIPMENT_RATE", 
			  NEW."RATE", 
			  NEW."RATE_PERIOD", 
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."CONCURRENCY_CONTROL_NUMBER",			  
			  NEW."APP_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_GUID", 
			  NEW."APP_CREATE_USERID", 
			  NEW."APP_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_GUID", 
			  NEW."APP_LAST_UPDATE_USERID", 
			  NEW."DB_CREATE_USER_ID", 
			  NEW."DB_LAST_UPDATE_USER_ID", 
			  NEW."IS_INCLUDED_IN_TOTAL"
             );
             RETURN NEW;
        ELSIF (TG_OP = 'UPDATE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_AGREEMENT_RATE_HIST" rntagrhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntagrhis."RENTAL_AGREEMENT_RATE_ID" = NEW."RENTAL_AGREEMENT_RATE_ID"
            AND rntagrhis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_RENTAL_AGREEMENT_RATE_HIST"
            (
			 "RENTAL_AGREEMENT_RATE_HIST_ID",
             "RENTAL_AGREEMENT_RATE_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "COMMENT",
			 "COMPONENT_NAME", 
			 "DB_CREATE_TIMESTAMP", 
			 "APP_CREATE_USER_DIRECTORY", 
			 "IS_ATTACHMENT", 
			 "DB_LAST_UPDATE_TIMESTAMP", 
			 "APP_LAST_UPDATE_USER_DIRECTORY", 
			 "PERCENT_OF_EQUIPMENT_RATE",
			 "RATE", 
			 "RATE_PERIOD", 
			 "RENTAL_AGREEMENT_ID", 
			 "CONCURRENCY_CONTROL_NUMBER",
			 "APP_CREATE_TIMESTAMP", 
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID", 
			 "APP_LAST_UPDATE_TIMESTAMP", 
			 "APP_LAST_UPDATE_USER_GUID", 
			 "APP_LAST_UPDATE_USERID", 
			 "DB_CREATE_USER_ID", 
			 "DB_LAST_UPDATE_USER_ID", 
			 "IS_INCLUDED_IN_TOTAL" 
            )
            VALUES( 
              nextval('"HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"'::regclass),
              NEW."RENTAL_AGREEMENT_RATE_ID",
              l_current_timestamp,
              NULL,
			  NEW."COMMENT", 
			  NEW."COMPONENT_NAME", 
			  NEW."DB_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_DIRECTORY", 
			  NEW."IS_ATTACHMENT", 
			  NEW."DB_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY", 
			  NEW."PERCENT_OF_EQUIPMENT_RATE", 
			  NEW."RATE", 
			  NEW."RATE_PERIOD", 
			  NEW."RENTAL_AGREEMENT_ID", 
			  NEW."CONCURRENCY_CONTROL_NUMBER",
			  NEW."APP_CREATE_TIMESTAMP", 
			  NEW."APP_CREATE_USER_GUID", 
			  NEW."APP_CREATE_USERID", 
			  NEW."APP_LAST_UPDATE_TIMESTAMP", 
			  NEW."APP_LAST_UPDATE_USER_GUID", 
			  NEW."APP_LAST_UPDATE_USERID", 
			  NEW."DB_CREATE_USER_ID", 
			  NEW."DB_LAST_UPDATE_USER_ID", 
			  NEW."IS_INCLUDED_IN_TOTAL"
             );
     	RETURN NEW;
        ELSIF (TG_OP = 'DELETE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_AGREEMENT_RATE_HIST" rntagrhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntagrhis."RENTAL_AGREEMENT_RATE_ID" = OLD."RENTAL_AGREEMENT_RATE_ID"
            AND rntagrhis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_rntagr_ar_iud_tr()
    OWNER TO "postgres";

-- ********************************************************************	
-- FUNCTION: public.het_timrec_ar_iud_tr()

CREATE FUNCTION public.het_timrec_ar_iud_tr() 
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_TIME_RECORD_HIST"
            (
			 "TIME_RECORD_HIST_ID",
             "TIME_RECORD_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ENTERED_DATE",
			 "HOURS",
			 "RENTAL_AGREEMENT_RATE_ID",
			 "WORKED_DATE",
			 "TIME_PERIOD",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "RENTAL_AGREEMENT_ID",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_TIME_RECORD_HIST_ID_seq"'::regclass),
              NEW."TIME_RECORD_ID",
              l_current_timestamp,
              NULL,
			  NEW."ENTERED_DATE",
			  NEW."HOURS",
			  NEW."RENTAL_AGREEMENT_RATE_ID",
		 	  NEW."WORKED_DATE",
			  NEW."TIME_PERIOD",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_TIME_RECORD_HIST" timrechis
            SET "END_DATE" = l_current_timestamp
            WHERE timrechis."TIME_RECORD_ID" = NEW."TIME_RECORD_ID"
            AND timrechis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_TIME_RECORD_HIST"
            (
			 "TIME_RECORD_HIST_ID",
             "TIME_RECORD_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ENTERED_DATE",
			 "HOURS",
			 "RENTAL_AGREEMENT_RATE_ID",
			 "WORKED_DATE",
			 "TIME_PERIOD",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "RENTAL_AGREEMENT_ID",
			 "CONCURRENCY_CONTROL_NUMBER",
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
              nextval('"HET_TIME_RECORD_HIST_ID_seq"'::regclass),
              NEW."TIME_RECORD_ID",
              l_current_timestamp,
              NULL,
			  NEW."ENTERED_DATE",
			  NEW."HOURS",
			  NEW."RENTAL_AGREEMENT_RATE_ID",
		 	  NEW."WORKED_DATE",
			  NEW."TIME_PERIOD",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
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
            UPDATE public."HET_TIME_RECORD_HIST" timrechis
            SET "END_DATE" = l_current_timestamp
            WHERE timrechis."TIME_RECORD_ID" = OLD."TIME_RECORD_ID"
            AND timrechis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

ALTER FUNCTION public.het_timrec_ar_iud_tr()
    OWNER TO "postgres";
	
-- *******************************************************************
-- FUNCTION: public.het_rntrrl_ar_iud_tr()

CREATE FUNCTION public.het_rntrrl_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$

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

$BODY$;	

ALTER FUNCTION public.het_rntrrl_ar_iud_tr()
    OWNER TO "postgres";
	
	
	