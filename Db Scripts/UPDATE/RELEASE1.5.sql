SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;


--HETS-1006 - Go-Live: Add additional fields for projects
ALTER TABLE public."HET_PROJECT" ADD COLUMN "FISCAL_YEAR" character varying(10) NOT NULL DEFAULT '2018/2019';
ALTER TABLE public."HET_PROJECT" ADD COLUMN "RESPONSIBILITY_CENTRE" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "SERVICE_LINE" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "STOB" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "PRODUCT" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "BUSINESS_FUNCTION" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "WORK_ACTIVITY" character varying(255);
ALTER TABLE public."HET_PROJECT" ADD COLUMN "COST_TYPE" character varying(255);