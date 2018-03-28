-- Trigger: het_eqpatt_ar_iud_tr

-- DROP TRIGGER het_eqpatt_ar_iud_tr ON public."HET_EQUIPMENT_ATTACHMENT";

CREATE TRIGGER het_eqpatt_ar_iud_tr
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."HET_EQUIPMENT_ATTACHMENT"
    FOR EACH ROW
    EXECUTE PROCEDURE public.het_eqpatt_ar_iud_tr();