-- PROCEDURE: public.postgres_benchmark_insert(integer)

-- DROP PROCEDURE IF EXISTS public.postgres_benchmark_insert(integer);

CREATE OR REPLACE PROCEDURE public.postgres_benchmark_insert(
	IN itemscount integer)
LANGUAGE 'sql'
AS $BODY$

insert into benchmark
select id, now(), null, 'StoredProcedure', 2, null, gen_random_uuid ()
from generate_series(1, itemsCount) as id;

$BODY$;
ALTER PROCEDURE public.postgres_benchmark_insert(integer)
    OWNER TO postgres;
