using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
public class SmallBenchmarkStoredProcService
{
    [Benchmark]
    public async Task MsStoredProcedure()
    {
        using var ctx = new MsBenchmarkDbContext();
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw("exec MsSqlBenchmarkInsert 100").ToListAsync();
    }

    [Benchmark]
    public async Task PgStoredProcedure()
    {
        using var ctx = new PgBenchmarkDbContext();
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw("call public.postgres_benchmark_insert(100)").ToListAsync();
    }
}
