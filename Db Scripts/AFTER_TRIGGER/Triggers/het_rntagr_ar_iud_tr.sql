-- Trigger: het_rntagr_ar_iud_tr

-- DROP TRIGGER het_rntagr_ar_iud_tr ON public."HET_RENTAL_AGREEMENT_RATE";

CREATE TRIGGER het_rntagr_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT_RATE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntagr_ar_iud_tr();