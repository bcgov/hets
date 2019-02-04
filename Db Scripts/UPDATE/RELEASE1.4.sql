SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;


--HETS-979 - Delete equipment type containing only Archived equipment
-- Add a "DELETED" column
ALTER TABLE public."HET_DISTRICT_EQUIPMENT_TYPE" ADD COLUMN "DELETED" boolean NOT NULL DEFAULT false;