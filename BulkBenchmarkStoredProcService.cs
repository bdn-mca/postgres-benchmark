using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
[MinIterationCount(2)]
[MaxIterationCount(4)]
public class BulkBenchmarkStoredProcService
{
    [Benchmark]
    public async Task MsStoredProcedure()
    {
        using var ctx = new MsBenchmarkDbContext();
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw("exec MsSqlBenchmarkInsert 1000000").ToListAsync();
    }

    [Benchmark]
    public async Task PgStoredProcedure()
    {
        using var ctx = new PgBenchmarkDbContext();
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw("call public.postgres_benchmark_insert(1000000)").ToListAsync();
    }
}
