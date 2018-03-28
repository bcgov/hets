-- Trigger: het_timrec_ar_iud_tr

-- DROP TRIGGER het_timrec_ar_iud_tr ON public."HET_TIME_RECORD";

CREATE TRIGGER het_timrec_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_TIME_RECORD"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_timrec_ar_iud_tr();