SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;


-- HETS-908 (BVT: Printed Rental Agreement)
-- Add the User's City so it can be automatically inserted on new agreements
ALTER TABLE public."HET_USER" ADD COLUMN "AGREEMENT_CITY" character varying(255);