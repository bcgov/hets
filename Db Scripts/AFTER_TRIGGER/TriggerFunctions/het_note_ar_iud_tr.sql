-- FUNCTION: public.het_note_ar_iud_tr()

DROP FUNCTION IF EXISTS public.het_note_ar_iud_tr() CASCADE;

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
    OWNER TO "user6DA";
