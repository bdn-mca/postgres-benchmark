using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark.Benchmarks.PgMsSmallData;

[WarmupCount(0)]
public class SmallBenchmarkStoredProcService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [Benchmark]
    public async Task StoredProcedure()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw(GetSqlCommand()).ToListAsync();
    }

    [GlobalCleanup]
    public async Task CleanTable()
    {
        if (BenchmarkDbType == DatabaseType.PostgreSqlWindows ||
            BenchmarkDbType == DatabaseType.PostgreSqlLinux ||
            BenchmarkDbType == DatabaseType.PostgreSqlCitus)
        {
            using var pgCtx = new PgBenchmarkDbContext(BenchmarkDbType);
            await pgCtx.Set<BenchmarkSpEntity>().FromSqlRaw("delete from public.benchmark").ToListAsync();
            await pgCtx.Set<BenchmarkSpEntity>().FromSqlRaw("vacuum full public.benchmark").ToListAsync();
        }
        else if (BenchmarkDbType == DatabaseType.MsSql)
        {
            using var msCtx = new MsBenchmarkDbContext();
            await msCtx.Set<BenchmarkSpEntity>().FromSqlRaw("delete from Benchmark").ToListAsync();
        }
    }

    private string GetSqlCommand()
    {
        return BenchmarkDbType switch
        {
            DatabaseType.MsSql
                => "exec MsSqlBenchmarkInsert 100",
            DatabaseType.PostgreSqlWindows or DatabaseType.PostgreSqlLinux or DatabaseType.PostgreSqlCitus
                => "call public.postgres_benchmark_insert(100)",
            _ => throw new ArgumentException("Unknown value.", nameof(BenchmarkDbType)),
        };
    }
}
