-- FUNCTION: public.het_eqpatt_ar_iud_tr()

DROP FUNCTION IF EXISTS public.het_eqpatt_ar_iud_tr() CASCADE;

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
    OWNER TO "user6DA";
