update "HET_LOCAL_AREA" set "CONCURRENCY_CONTROL_NUMBER" = "CONCURRENCY_CONTROL_NUMBER" + 1, "END_DATE" = now() where "NAME" = 'Un-Assigned';

insert into "HET_ROLLOVER_PROGRESS"
select a."DISTRICT_ID", null
from "HET_DISTRICT" a
where not exists (select 1
from "HET_ROLLOVER_PROGRESS" b
where b."DISTRICT_ID" = a."DISTRICT_ID");

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.het_log TO het_application_proxy;
