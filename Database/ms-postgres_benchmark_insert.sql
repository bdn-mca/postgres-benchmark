CREATE PROCEDURE MsSqlBenchmarkInsert @itemsCount integer
AS

insert into Benchmark
select VALUE, getdate(), null, 'StoredProcedure', 2, null, newid ()
from generate_series(1, @itemsCount);