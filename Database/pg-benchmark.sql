-- Table: public.benchmark

-- DROP TABLE IF EXISTS public.benchmark;

CREATE TABLE IF NOT EXISTS public.benchmark
(
    id bigint NOT NULL DEFAULT 1,
    created_on timestamp with time zone NOT NULL,
    deleted_on timestamp with time zone,
    name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    type integer NOT NULL,
    properties character varying(200) COLLATE pg_catalog."default",
    uid uuid NOT NULL,
    CONSTRAINT benchmark_pkey PRIMARY KEY (uid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.benchmark
    OWNER to postgres;
-- Index: benchmark_type_idx

-- DROP INDEX IF EXISTS public.benchmark_type_idx;

CREATE INDEX IF NOT EXISTS benchmark_type_idx
    ON public.benchmark USING btree
    (type ASC NULLS LAST)
    TABLESPACE pg_default;