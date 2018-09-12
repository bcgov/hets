do $$
begin

	SET statement_timeout = 0;
	SET lock_timeout = 0;
	SET client_encoding = 'UTF8';
	SET standard_conforming_strings = on;
	SET check_function_bodies = false;
	SET client_min_messages = warning;
	PERFORM pg_catalog.set_config('search_path', '', false);


	IF EXISTS (SELECT datname FROM pg_database WHERE datname = 'hets') THEN
	  -- kill sessions
	  PERFORM pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'hets';
	END IF;
	

end
$$
