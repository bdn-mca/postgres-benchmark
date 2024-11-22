CREATE OR ALTER PROCEDURE MsSqlBenchmarkInsert
    @itemsCount integer
AS

insert into Benchmark
select VALUE, getdate(), null, 'StoredProcedure', VALUE%5, null, newid ()
from generate_series(1, @itemsCount);

CREATE OR ALTER PROCEDURE MsSqlBenchmarkInsert
    @itemsCount integer,
    @typesCount integer
AS

insert into Benchmark
select VALUE, getdate(), null, 'StoredProcedure', VALUE%@typesCount, null, newid ()
from generate_series(1, @itemsCount);