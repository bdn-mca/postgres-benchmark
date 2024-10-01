using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
[MaxIterationCount(30)]
public class BulkBenchmarkStoredProcService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [ParamsAllValues]
    public bool CleanupTable { get; set; }

    [Benchmark]
    public async Task StoredProcedure()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.Set<BenchmarkSpEntity>().FromSqlRaw(GetSqlCommand()).ToListAsync();
    }

    private string GetSqlCommand()
    {
        return BenchmarkDbType switch
        {
            DatabaseType.MsSql
                => "exec MsSqlBenchmarkInsert 1000000",
            DatabaseType.PostgreSqlWindows or DatabaseType.PostgreSqlLinux or DatabaseType.PostgreSqlCitus
                => "call public.postgres_benchmark_insert(1000000)",
            _ => throw new ArgumentException("Unknown value.", nameof(BenchmarkDbType)),
        };
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        if (CleanupTable)
        {
            CleanTable();
        }
    }

    [GlobalCleanup]
    public void CleanTable()
    {
        if (BenchmarkDbType.HasFlag(DatabaseType.PostgreSqlWindows | DatabaseType.PostgreSqlLinux | DatabaseType.PostgreSqlCitus))
        {
            using var pgCtx = new PgBenchmarkDbContext(BenchmarkDbType);
            pgCtx.Set<BenchmarkSpEntity>().FromSqlRaw("delete from public.benchmark").ToList();
            pgCtx.Set<BenchmarkSpEntity>().FromSqlRaw("vacuum full public.benchmark").ToList();
        }
        else if (BenchmarkDbType == DatabaseType.MsSql)
        {
            using var msCtx = new MsBenchmarkDbContext();
            msCtx.Set<BenchmarkSpEntity>().FromSqlRaw("delete from Benchmark").ToList();
        }
    }
}
