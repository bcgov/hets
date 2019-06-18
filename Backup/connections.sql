do $$
begin 
    IF EXISTS (SELECT datname FROM pg_database WHERE datname = 'hets') THEN
	  -- kill sessions
	  PERFORM pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'hets';
	END IF;
end
$$