-- Trigger: het_eqp_ar_iud_tr

-- DROP TRIGGER het_eqp_ar_iud_tr ON public."HET_EQUIPMENT";

CREATE TRIGGER het_eqp_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_EQUIPMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_eqp_ar_iud_tr();