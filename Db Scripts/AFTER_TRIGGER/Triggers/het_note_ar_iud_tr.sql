-- Trigger: het_note_ar_iud_tr

-- DROP TRIGGER het_note_ar_iud_tr ON public."HET_NOTE";

CREATE TRIGGER het_note_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_NOTE"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_note_ar_iud_tr();