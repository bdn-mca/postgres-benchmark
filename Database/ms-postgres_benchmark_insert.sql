CREATE OR ALTER PROCEDURE MsSqlBenchmarkInsert
    @itemsCount integer
AS

insert into Benchmark
select getdate(), null, 'StoredProcedure', VALUE%20, null, newid (), rand() * 1000
from generate_series(1, @itemsCount);

