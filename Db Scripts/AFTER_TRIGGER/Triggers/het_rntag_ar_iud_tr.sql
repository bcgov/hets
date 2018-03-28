-- Trigger: het_rntag_ar_iud_tr

-- DROP TRIGGER het_rntag_ar_iud_tr ON public."HET_RENTAL_AGREEMENT";

CREATE TRIGGER het_rntag_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_RENTAL_AGREEMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_rntag_ar_iud_tr();