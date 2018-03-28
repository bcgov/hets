-- Trigger: het_rntrrl_ar_iud_tr

-- DROP TRIGGER het_rntrrl_ar_iud_tr ON public."HET_RENTAL_REQUEST_ROTATION_LIST";

CREATE TRIGGER het_rntrrl_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_REQUEST_ROTATION_LIST"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntrrl_ar_iud_tr();