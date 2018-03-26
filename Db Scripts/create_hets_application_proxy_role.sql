create role het_application_proxy;

-- sequences
grant usage on public."counter_id_seq" to het_application_proxy;
grant usage on public."hash_id_seq" to het_application_proxy;
grant usage on public."HET_ATTACHMENT_ATTACHMENT_id_seq" to het_application_proxy;
grant usage on public."HET_CITY_CITY_id_seq" to het_application_proxy;
grant usage on public."HET_CONDITION_TYPE_CONDITION_TYPE_id_seq" to het_application_proxy;
grant usage on public."HET_CONTACT_CONTACT_id_seq" to het_application_proxy;
grant usage on public."HET_DISTRICT_DISTRICT_id_seq" to het_application_proxy;
grant usage on public."HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ATTACHMENT_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_ATTACHMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_COPY_EQUIPMENT_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_COPY_HIST_EQUIPMENT_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_EQUIPMENT_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_TYPE_EQUIPMENT_TYPE_id_seq" to het_application_proxy;
grant usage on public."HET_HISTORY_HISTORY_id_seq" to het_application_proxy;
grant usage on public."HET_IMPORT_MAP_IMPORT_MAP_id_seq" to het_application_proxy;
grant usage on public."HET_LOCAL_AREA_LOCAL_AREA_id_seq" to het_application_proxy;
grant usage on public."HET_LOCAL_AREA_ROTATION_LIST_LOCAL_AREA_ROTATION_LIST_id_seq" to het_application_proxy;
grant usage on public."HET_NOTE_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_NOTE_NOTE_id_seq" to het_application_proxy;
grant usage on public."HET_OWNER_OWNER_id_seq" to het_application_proxy;
grant usage on public."HET_PERMISSION_PERMISSION_id_seq" to het_application_proxy;
grant usage on public."HET_PROJECT_PROJECT_id_seq" to het_application_proxy;
grant usage on public."HET_REGION_REGION_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_CONDITION_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_CONDITIO_RENTAL_AGREEMENT_CONDITION_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_RATE_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_RATE_RENTAL_AGREEMENT_RATE_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_REQUEST_ATTACHMENT_RENTAL_REQUEST_ATTACHMENT_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_REQUEST_RENTAL_REQUEST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_REQUEST_ROTATION_L_RENTAL_REQUEST_ROTATION_LIST__seq to het_application_proxy;
grant usage on public."HET_ROLE_PERMISSION_ROLE_PERMISSION_id_seq" to het_application_proxy;
grant usage on public."HET_ROLE_ROLE_id_seq" to het_application_proxy;
grant usage on public."HET_SENIORITY_AUDIT_SENIORITY_AUDIT_id_seq" to het_application_proxy;
grant usage on public."HET_SERVICE_AREA_SERVICE_AREA_id_seq" to het_application_proxy;
grant usage on public."HET_TIME_RECORD_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_TIME_RECORD_TIME_RECORD_id_seq" to het_application_proxy;
grant usage on public."HET_USER_DISTRICT_USER_DISTRICT_id_seq" to het_application_proxy;
grant usage on public."HET_USER_FAVOURITE_USER_FAVOURITE_id_seq" to het_application_proxy;
grant usage on public."HET_USER_ROLE_USER_ROLE_id_seq" to het_application_proxy;
grant usage on public."HET_USER_USER_id_seq" to het_application_proxy;
grant usage on public."job_id_seq" to het_application_proxy;
grant usage on public."jobparameter_id_seq" to het_application_proxy;
grant usage on public."jobqueue_id_seq" to het_application_proxy;
grant usage on public."list_id_seq" to het_application_proxy;
grant usage on public."set_id_seq" to het_application_proxy;
grant usage on public."state_id_seq" to het_application_proxy;
-- sequences for HIST tables
grant usage on public."HET_EQUIPMENT_ATTACHMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_EQUIPMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_NOTE_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_CONDITION_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_AGREEMENT_RATE_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST_id_seq" to het_application_proxy;
grant usage on public."HET_TIME_RECORD_HIST_id_seq" to het_application_proxy;


-- tables
grant select, insert, update, delete on public."HET_ATTACHMENT" to het_application_proxy;
grant select, insert, update, delete on public."HET_CITY" to het_application_proxy;
grant select, insert, update, delete on public."HET_CONDITION_TYPE" to het_application_proxy;
grant select, insert, update, delete on public."HET_CONTACT" to het_application_proxy;
grant select, insert, update, delete on public."HET_DISTRICT_EQUIPMENT_TYPE" to het_application_proxy;
grant select, insert, update, delete on public."HET_DISTRICT" to het_application_proxy;
grant select, insert, update, delete on public."HET_EQUIPMENT_ATTACHMENT" to het_application_proxy;
grant select, insert, update, delete on public."HET_EQUIPMENT" to het_application_proxy;
grant select, insert, update, delete on public."HET_EQUIPMENT_TYPE" to het_application_proxy;
grant select, insert, update, delete on public."HET_HISTORY" to het_application_proxy;
grant select, insert, update, delete on public."HET_IMPORT_MAP" to het_application_proxy;
grant select, insert, update, delete on public."HET_LOCAL_AREA_ROTATION_LIST" to het_application_proxy;
grant select, insert, update, delete on public."HET_LOCAL_AREA" to het_application_proxy;
grant select, insert, update, delete on public."HET_NOTE" to het_application_proxy;
grant select, insert, update, delete on public."HET_OWNER" to het_application_proxy;
grant select, insert, update, delete on public."HET_PERMISSION" to het_application_proxy;
grant select, insert, update, delete on public."HET_PROJECT" to het_application_proxy;
grant select, insert, update, delete on public."HET_PROVINCIAL_RATE_TYPE" to het_application_proxy;
grant select, insert, update, delete on public."HET_REGION" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_AGREEMENT_CONDITION" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_AGREEMENT_RATE" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_AGREEMENT" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_REQUEST_ATTACHMENT" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_REQUEST_ROTATION_LIST" to het_application_proxy;
grant select, insert, update, delete on public."HET_RENTAL_REQUEST" to het_application_proxy;
grant select, insert, update, delete on public."HET_ROLE_PERMISSION" to het_application_proxy;
grant select, insert, update, delete on public."HET_ROLE" to het_application_proxy;
grant select, insert, update, delete on public."HET_SENIORITY_AUDIT" to het_application_proxy;
grant select, insert, update, delete on public."HET_SERVICE_AREA" to het_application_proxy;
grant select, insert, update, delete on public."HET_TIME_RECORD" to het_application_proxy;
grant select, insert, update, delete on public."HET_USER_DISTRICT" to het_application_proxy;
grant select, insert, update, delete on public."HET_USER_FAVOURITE" to het_application_proxy;
grant select, insert, update, delete on public."HET_USER_ROLE" to het_application_proxy;
grant select, insert, update, delete on public."HET_USER" to het_application_proxy;
grant select, insert, update, delete on public."counter" to het_application_proxy;
grant select, insert, update, delete on public."__EFMigrationsHistory" to het_application_proxy;
grant select, insert, update, delete on public."hash" to het_application_proxy;
grant select, insert, update, delete on public."jobparameter" to het_application_proxy;
grant select, insert, update, delete on public."jobqueue" to het_application_proxy;
grant select, insert, update, delete on public."job" to het_application_proxy;
grant select, insert, update, delete on public."list" to het_application_proxy;
grant select, insert, update, delete on public."lock" to het_application_proxy;
grant select, insert, update, delete on public."schema" to het_application_proxy;
grant select, insert, update, delete on public."server" to het_application_proxy;
grant select, insert, update, delete on public."set" to het_application_proxy;
grant select, insert, update, delete on public."state" to het_application_proxy;
-- now the HIST tables, without delete
grant select, insert, update on public."HET_EQUIPMENT_ATTACHMENT_HIST" to het_application_proxy;
grant select, insert, update on public."HET_EQUIPMENT_HIST" to het_application_proxy;
grant select, insert, update on public."HET_NOTE_HIST" to het_application_proxy;
grant select, insert, update on public."HET_RENTAL_AGREEMENT_CONDITION_HIST" to het_application_proxy;
grant select, insert, update on public."HET_RENTAL_AGREEMENT_HIST" to het_application_proxy;
grant select, insert, update on public."HET_RENTAL_AGREEMENT_RATE_HIST" to het_application_proxy;
grant select, insert, update on public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" to het_application_proxy;
grant select, insert, update on public."HET_TIME_RECORD_HIST" to het_application_proxy;


-- now grant the role to the proxy account in each environment
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_user WHERE username = 'TRDBHETD') THEN
        grant het_application_proxy to TRDBHETD;
    ELSIF EXISTS (SELECT 1 FROM pg_user WHERE username = 'TRDBHETT') THEN
        grant het_application_proxy to TRDBHETT;
    ELSIF EXISTS (SELECT 1 FROM pg_user WHERE username = 'TRDBHETP') THEN
        grant het_application_proxy to TRDBHETP;    
    END IF;
END
$$;


