-- PROCEDURE: public.postgres_benchmark_cleanup()

-- DROP PROCEDURE IF EXISTS public.postgres_benchmark_cleanup();

CREATE OR REPLACE PROCEDURE public.postgres_benchmark_cleanup(
	)
LANGUAGE 'sql'
AS $BODY$
delete from benchmark;
vacuum full benchmark;
$BODY$;
ALTER PROCEDURE public.postgres_benchmark_cleanup()
    OWNER TO postgres;
