-- FUNCTION: public.het_rntag_ar_iud_tr()

DROP FUNCTION IF EXISTS public.het_rntag_ar_iud_tr() CASCADE;

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
    OWNER TO "user6DA";
