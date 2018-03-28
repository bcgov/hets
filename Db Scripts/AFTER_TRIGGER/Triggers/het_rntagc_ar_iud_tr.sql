-- Trigger: het_rntagc_ar_iud_tr

-- DROP TRIGGER het_rntagc_ar_iud_tr ON public."HET_RENTAL_AGREEMENT_CONDITION";

CREATE TRIGGER het_rntagc_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_CONDITION"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntagc_ar_iud_tr();