-- Trigger: het_df_br_iu_tr

-- DROP TRIGGER het_df_br_iu_tr ON public."HET_ATTACHMENT";

CREATE TRIGGER het_df_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_DIGITAL_FILE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_cndtyp_br_iu_tr

-- DROP TRIGGER het_cndtyp_br_iu_tr ON public."HET_CONDITION_TYPE";

CREATE TRIGGER het_cndtyp_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_CONDITION_TYPE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_cntct_br_iu_tr

-- DROP TRIGGER het_cntct_br_iu_tr ON public."HET_CONTACT";

CREATE TRIGGER het_cntct_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_CONTACT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_dist_br_iu_tr

-- DROP TRIGGER het_dist_br_iu_tr ON public."HET_DISTRICT";

CREATE TRIGGER het_dist_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_DISTRICT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_distet_br_iu_tr

-- DROP TRIGGER het_distet_br_iu_tr ON public."HET_DISTRICT_EQUIPMENT_TYPE";

CREATE TRIGGER het_distet_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_DISTRICT_EQUIPMENT_TYPE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_eqp_br_iu_tr

-- DROP TRIGGER het_eqp_br_iu_tr ON public."HET_EQUIPMENT";

CREATE TRIGGER het_eqp_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_EQUIPMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_eqpatt_br_iu_tr

-- DROP TRIGGER het_eqpatt_br_iu_tr ON public."HET_EQUIPMENT_ATTACHMENT";

CREATE TRIGGER het_eqpatt_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_EQUIPMENT_ATTACHMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_eqptyp_br_iu_tr

-- DROP TRIGGER het_eqptyp_br_iu_tr ON public."HET_EQUIPMENT_TYPE";

CREATE TRIGGER het_eqptyp_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_EQUIPMENT_TYPE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();

-- Trigger: het_hist_br_iu_tr

-- DROP TRIGGER het_hist_br_iu_tr ON public."HET_HISTORY";

CREATE TRIGGER het_hist_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_HISTORY"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();

-- Trigger: het_loca_br_iu_tr

-- DROP TRIGGER het_loca_br_iu_tr ON public."HET_LOCAL_AREA";

CREATE TRIGGER het_loca_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_LOCAL_AREA"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_locarl_br_iu_tr

-- DROP TRIGGER het_locarl_br_iu_tr ON public."HET_LOCAL_AREA_ROTATION_LIST";

CREATE TRIGGER het_locarl_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_LOCAL_AREA_ROTATION_LIST"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();

-- Trigger: het_note_br_iu_tr

-- DROP TRIGGER het_note_br_iu_tr ON public."HET_NOTE";

CREATE TRIGGER het_note_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_NOTE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();

-- Trigger: het_bus_br_iu_tr

-- DROP TRIGGER het_bus_br_iu_tr ON public."HET_BUSINESS";

CREATE TRIGGER het_bus_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_BUSINESS"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_bususr_br_iu_tr

-- DROP TRIGGER het_bususr_br_iu_tr ON public."HET_BUSINESS_USER";

CREATE TRIGGER het_bususr_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_BUSINESS_USER"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_bususrole_br_iu_tr

-- DROP TRIGGER het_bususrole_br_iu_tr ON public."HET_BUSINESS_USER_ROLE";

CREATE TRIGGER het_bususrole_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_BUSINESS_USER_ROLE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();
			

-- Trigger: het_own_br_iu_tr

-- DROP TRIGGER het_own_br_iu_tr ON public."HET_OWNER";

CREATE TRIGGER het_own_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_OWNER"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_prm_br_iu_tr

-- DROP TRIGGER het_prm_br_iu_tr ON public."HET_PERMISSION";

CREATE TRIGGER het_prm_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_PERMISSION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_proj_br_iu_tr

-- DROP TRIGGER het_proj_br_iu_tr ON public."HET_PROJECT";

CREATE TRIGGER het_proj_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_PROJECT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_prvrt_br_iu_tr

-- DROP TRIGGER het_prvrt_br_iu_tr ON public."HET_PROVINCIAL_RATE_TYPE";

CREATE TRIGGER het_prvrt_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_PROVINCIAL_RATE_TYPE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_reg_br_iu_tr

-- DROP TRIGGER het_reg_br_iu_tr ON public."HET_REGION";

CREATE TRIGGER het_reg_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_REGION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntag_br_iu_tr

-- DROP TRIGGER het_rntag_br_iu_tr ON public."HET_RENTAL_AGREEMENT";

CREATE TRIGGER het_rntag_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntagc_br_iu_tr

-- DROP TRIGGER het_rntagc_br_iu_tr ON public."HET_RENTAL_AGREEMENT_CONDITION";

CREATE TRIGGER het_rntagc_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_CONDITION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntagr_br_iu_tr

-- DROP TRIGGER het_rntagr_br_iu_tr ON public."HET_RENTAL_AGREEMENT_RATE";

CREATE TRIGGER het_rntagr_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_RATE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntr_br_iu_tr

-- DROP TRIGGER het_rntr_br_iu_tr ON public."HET_RENTAL_REQUEST";

CREATE TRIGGER het_rntr_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_REQUEST"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntra_br_iu_tr

-- DROP TRIGGER het_rntra_br_iu_tr ON public."HET_RENTAL_REQUEST_ATTACHMENT";

CREATE TRIGGER het_rntra_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_REQUEST_ATTACHMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rntrrl_br_iu_tr

-- DROP TRIGGER het_rntrrl_br_iu_tr ON public."HET_RENTAL_REQUEST_ROTATION_LIST";

CREATE TRIGGER het_rntrrl_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_RENTAL_REQUEST_ROTATION_LIST"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_role_br_iu_tr

-- DROP TRIGGER het_role_br_iu_tr ON public."HET_ROLE";

CREATE TRIGGER het_role_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_ROLE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_rlprm_br_iu_tr

-- DROP TRIGGER het_rlprm_br_iu_tr ON public."HET_ROLE_PERMISSION";

CREATE TRIGGER het_rlprm_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_ROLE_PERMISSION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_snra_br_iu_tr

-- DROP TRIGGER het_snra_br_iu_tr ON public."HET_SENIORITY_AUDIT";

CREATE TRIGGER het_snra_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_SENIORITY_AUDIT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_sera_br_iu_tr

-- DROP TRIGGER het_sera_br_iu_tr ON public."HET_SERVICE_AREA";

CREATE TRIGGER het_sera_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_SERVICE_AREA"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_timrec_br_iu_tr

-- DROP TRIGGER het_timrec_br_iu_tr ON public."HET_TIME_RECORD";

CREATE TRIGGER het_timrec_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_TIME_RECORD"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_usr_br_iu_tr

-- DROP TRIGGER het_usr_br_iu_tr ON public."HET_USER";

CREATE TRIGGER het_usr_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_USER"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_usrfav_br_iu_tr

-- DROP TRIGGER het_usrfav_br_iu_tr ON public."HET_USER_FAVOURITE";

CREATE TRIGGER het_usrfav_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_USER_FAVOURITE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();


-- Trigger: het_usrrl_br_iu_tr

-- DROP TRIGGER het_usrrl_br_iu_tr ON public."HET_USER_ROLE";

CREATE TRIGGER het_usrrl_br_iu_tr
    BEFORE INSERT OR UPDATE 
    ON public."HET_USER_ROLE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_validate_init_audit_cols();
