SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;

create role het_application_proxy;

--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: -
--

GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- Name: SEQUENCE "HET_DIGITAL_FILE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_DIGITAL_FILE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_DIGITAL_FILE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_DIGITAL_FILE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_CONDITION_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_CONDITION_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_CONDITION_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_CONDITION_TYPE" TO het_application_proxy;


--
-- Name: TABLE "HET_CONTACT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_CONTACT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_CONTACT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_CONTACT_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_CONTACT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_CONTACT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_DISTRICT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_DISTRICT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_DISTRICT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_DISTRICT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_DISTRICT_EQUIPMENT_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_DISTRICT_EQUIPMENT_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_DISTRICT_EQUIPMENT_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_DISTRICT_EQUIPMENT_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_EQUIPMENT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_EQUIPMENT" TO het_application_proxy;


--
-- Name: TABLE "HET_EQUIPMENT_ATTACHMENT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_EQUIPMENT_ATTACHMENT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_EQUIPMENT_ATTACHMENT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_ATTACHMENT_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_ATTACHMENT_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_EQUIPMENT_ATTACHMENT_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_EQUIPMENT_ATTACHMENT_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_EQUIPMENT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_EQUIPMENT_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_EQUIPMENT_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_EQUIPMENT_HIST" TO het_application_proxy;


--
-- Name: TABLE "HET_EQUIPMENT_STATUS_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_EQUIPMENT_STATUS_TYPE" TO het_application_proxy;

--
-- Name: TABLE "HET_EQUIPMENT_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_EQUIPMENT_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_EQUIPMENT_STATUS_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_STATUS_TYPE_ID_seq" TO het_application_proxy;
--
-- Name: SEQUENCE "HET_EQUIPMENT_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_EQUIPMENT_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_HISTORY"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_HISTORY" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_HISTORY_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_HISTORY_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_IMPORT_MAP"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_IMPORT_MAP" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_IMPORT_MAP_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_IMPORT_MAP_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_LOCAL_AREA"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_LOCAL_AREA" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_LOCAL_AREA_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_LOCAL_AREA_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_LOCAL_AREA_ROTATION_LIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_LOCAL_AREA_ROTATION_LIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_LOCAL_AREA_ROTATION_LIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_LOCAL_AREA_ROTATION_LIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_MIME_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_MIME_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_MIME_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_MIME_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_NOTE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_NOTE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_NOTE_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_NOTE_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_NOTE_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_NOTE_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_NOTE_NOTE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_NOTE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_BUSINESS"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_BUSINESS" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_BUSINESS_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_BUSINESS_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_BUSINESS_USER"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_BUSINESS_USER" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_BUSINESS_USER_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_BUSINESS_USER_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_OWNER"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_OWNER" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_OWNER_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_OWNER_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_OWNER_STATUS_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_OWNER_STATUS_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_OWNER_STATUS_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_OWNER_STATUS_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_PERSON"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_PERSON" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_PERSON_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_PERSON_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_PERMISSION"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_PERMISSION" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_PERMISSION_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_PERMISSION_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_PROJECT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_PROJECT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_PROJECT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_PROJECT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_PROJECT_STATUS_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_PROJECT_STATUS_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_PROJECT_STATUS_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_PROJECT_STATUS_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_PROVINCIAL_RATE_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_PROVINCIAL_RATE_TYPE" TO het_application_proxy;


--
-- Name: TABLE "HET_RATE_PERIOD_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RATE_PERIOD_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RATE_PERIOD_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RATE_PERIOD_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_REGION"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_REGION" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_REGION_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_REGION_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_CONDITION"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_CONDITION" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_CONDITION_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_CONDITION_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_CONDITION_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_CONDITION_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_CONDITION_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_HIST" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_RATE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_RATE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_RATE_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_RATE_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_RATE_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_RATE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_RATE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_AGREEMENT_STATUS_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_AGREEMENT_STATUS_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_STATUS_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_AGREEMENT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_AGREEMENT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_REQUEST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_REQUEST" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_REQUEST_ATTACHMENT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_REQUEST_ATTACHMENT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_REQUEST_ATTACHMENT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_REQUEST_ATTACHMENT_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_REQUEST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_REQUEST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_REQUEST_ROTATION_LIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_REQUEST_ROTATION_LIST_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_REQUEST_ROTATION_LIST_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_RENTAL_REQUEST_STATUS_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_RENTAL_REQUEST_STATUS_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_RENTAL_REQUEST_STATUS_TYPE" TO het_application_proxy;


--
-- Name: TABLE "HET_ROLE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_ROLE" TO het_application_proxy;


--
-- Name: TABLE "HET_ROLE_PERMISSION"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_ROLE_PERMISSION" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_ROLE_PERMISSION_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_ROLE_PERMISSION_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_ROLE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_ROLE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_SENIORITY_AUDIT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_SENIORITY_AUDIT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_SENIORITY_AUDIT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_SENIORITY_AUDIT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_SERVICE_AREA"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_SERVICE_AREA" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_SERVICE_AREA_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_SERVICE_AREA_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_TIME_PERIOD_TYPE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_TIME_PERIOD_TYPE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_TIME_PERIOD_TYPE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_TIME_PERIOD_TYPE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_TIME_RECORD"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_TIME_RECORD" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_TIME_RECORD_HIST_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_TIME_RECORD_HIST_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_TIME_RECORD_HIST"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,UPDATE ON TABLE public."HET_TIME_RECORD_HIST" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_TIME_RECORD_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_TIME_RECORD_ID_seq" TO het_application_proxy;

--
-- Name: TABLE "HET_USER"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_USER" TO het_application_proxy;


--
-- Name: TABLE "HET_USER_DISTRICT"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_USER_DISTRICT" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_USER_DISTRICT_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_USER_DISTRICT_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_USER_FAVOURITE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_USER_FAVOURITE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_USER_FAVOURITE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_USER_FAVOURITE_ID_seq" TO het_application_proxy;


--
-- Name: TABLE "HET_USER_ROLE"; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public."HET_USER_ROLE" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_USER_ROLE_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_USER_ROLE_ID_seq" TO het_application_proxy;


--
-- Name: SEQUENCE "HET_USER_ID_seq"; Type: ACL; Schema: public; Owner: -
--

GRANT ALL PRIVILEGES ON SEQUENCE public."HET_USER_ID_seq" TO het_application_proxy;


--
-- Name: TABLE counter; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.counter TO het_application_proxy;


--
-- Name: SEQUENCE counter_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.counter_id_seq TO het_application_proxy;


--
-- Name: TABLE hash; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.hash TO het_application_proxy;


--
-- Name: SEQUENCE hash_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.hash_id_seq TO het_application_proxy;


--
-- Name: TABLE job; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.job TO het_application_proxy;


--
-- Name: SEQUENCE job_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.job_id_seq TO het_application_proxy;


--
-- Name: TABLE jobparameter; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.jobparameter TO het_application_proxy;


--
-- Name: SEQUENCE jobparameter_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.jobparameter_id_seq TO het_application_proxy;


--
-- Name: TABLE jobqueue; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.jobqueue TO het_application_proxy;


--
-- Name: SEQUENCE jobqueue_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.jobqueue_id_seq TO het_application_proxy;


--
-- Name: TABLE list; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.list TO het_application_proxy;


--
-- Name: SEQUENCE list_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.list_id_seq TO het_application_proxy;


--
-- Name: TABLE lock; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.lock TO het_application_proxy;


--
-- Name: TABLE schema; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.schema TO het_application_proxy;


--
-- Name: TABLE server; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.server TO het_application_proxy;


--
-- Name: TABLE set; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.set TO het_application_proxy;


--
-- Name: SEQUENCE set_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.set_id_seq TO het_application_proxy;


--
-- Name: TABLE state; Type: ACL; Schema: public; Owner: -
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE public.state TO het_application_proxy;


--
-- Name: SEQUENCE state_id_seq; Type: ACL; Schema: public; Owner: -
--

GRANT USAGE ON SEQUENCE public.state_id_seq TO het_application_proxy;


--
-- PostgreSQL database dump complete
--

