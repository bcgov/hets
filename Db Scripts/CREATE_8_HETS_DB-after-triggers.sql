-- Trigger: het_eqp_ar_iud_tr

-- DROP TRIGGER het_eqp_ar_iud_tr ON public."HET_EQUIPMENT";

CREATE TRIGGER het_eqp_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_EQUIPMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_eqp_ar_iud_tr();

-- *******************************************************************	
-- Trigger: het_eqpatt_ar_iud_tr

-- DROP TRIGGER het_eqpatt_ar_iud_tr ON public."HET_EQUIPMENT_ATTACHMENT";

CREATE TRIGGER het_eqpatt_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_EQUIPMENT_ATTACHMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_eqpatt_ar_iud_tr();	
	
-- *******************************************************************
-- Trigger: het_note_ar_iud_tr

-- DROP TRIGGER het_note_ar_iud_tr ON public."HET_NOTE";

CREATE TRIGGER het_note_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_NOTE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_note_ar_iud_tr();	
	
-- *******************************************************************
-- Trigger: het_rntag_ar_iud_tr

-- DROP TRIGGER het_rntag_ar_iud_tr ON public."HET_RENTAL_AGREEMENT";

CREATE TRIGGER het_rntag_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntag_ar_iud_tr();	
	
-- *******************************************************************
-- Trigger: het_rntagc_ar_iud_tr

-- DROP TRIGGER het_rntagc_ar_iud_tr ON public."HET_RENTAL_AGREEMENT_CONDITION";

CREATE TRIGGER het_rntagc_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_CONDITION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntagc_ar_iud_tr();	
	
-- *******************************************************************
-- Trigger: het_rntagr_ar_iud_tr

-- DROP TRIGGER het_rntagr_ar_iud_tr ON public."HET_RENTAL_AGREEMENT_RATE";

CREATE TRIGGER het_rntagr_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_RATE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntagr_ar_iud_tr();

-- *******************************************************************
-- Trigger: het_timrec_ar_iud_tr

-- DROP TRIGGER het_timrec_ar_iud_tr ON public."HET_TIME_RECORD";

CREATE TRIGGER het_timrec_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_TIME_RECORD"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_timrec_ar_iud_tr();

-- *******************************************************************
-- Trigger: het_rntrrl_ar_iud_tr

-- DROP TRIGGER het_rntrrl_ar_iud_tr ON public."HET_RENTAL_REQUEST_ROTATION_LIST";

CREATE TRIGGER het_rntrrl_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_REQUEST_ROTATION_LIST"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntrrl_ar_iud_tr();



	