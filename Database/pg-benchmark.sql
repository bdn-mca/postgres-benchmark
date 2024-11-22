﻿-- Table: public.benchmark

-- DROP TABLE IF EXISTS public.benchmark;

CREATE TABLE IF NOT EXISTS public.benchmark
(
    id integer generated always as identity,
    created_on timestamp with time zone NOT NULL,
    deleted_on timestamp with time zone,
    name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    type integer NOT NULL,
    properties character varying(200) COLLATE pg_catalog."default",
    uid uuid NOT NULL,
    amount numeric(28, 8) NOT NULL,
    CONSTRAINT benchmark_pkey PRIMARY KEY (id)
)

ALTER TABLE IF EXISTS public.benchmark
    OWNER to postgres;
-- Index: benchmark_type_idx

-- DROP INDEX IF EXISTS public.benchmark_type_idx;

CREATE INDEX IF NOT EXISTS benchmark_type_idx
    ON public.benchmark USING btree
    (type ASC NULLS LAST)
    TABLESPACE pg_default;

cluster public.benchmark using benchmark_pkey;