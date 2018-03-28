-- FUNCTION: public.het_rntagr_ar_iud_tr()

DROP FUNCTION IF EXISTS public.het_rntagr_ar_iud_tr() CASCADE;

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
    OWNER TO "user6DA";
