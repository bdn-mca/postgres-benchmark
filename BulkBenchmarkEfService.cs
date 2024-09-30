using BenchmarkDotNet.Attributes;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
[MaxIterationCount(30)]
public class BulkBenchmarkEfService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [ParamsAllValues]
    public bool CleanupTable { get; set; }

    [Params(2000, 50000, 200_000)]
    public int BatchSize { get; set; }

    internal List<BenchmarkDbEntity> Entities { get; set; } = [];

    [IterationSetup]
    public void PrepareList()
    {
        Entities.Clear();
        for (int i = 1; i < 1_000_001; i++)
        {
            Entities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = BenchmarkDbType.ToString(),
                Type = 1,
                Properties = null,
            });
        }
    }

    [Benchmark]
    public async Task EfCoreBulk()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.BulkInsertAsync(Entities,
            new BulkConfig
            {
                BatchSize = BatchSize
            });
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        if (CleanupTable)
        {
            CleanTable();
        }
        else if (BenchmarkDbType == DatabaseType.PostgreSql)
        {
            using var pgCtx = new PgBenchmarkDbContext();
            pgCtx.Set<BenchmarkSpEntity>().FromSqlRaw("cluster").ToList();
        }
    }

    [GlobalCleanup]
    public void CleanTable()
    {
        if (BenchmarkDbType == DatabaseType.PostgreSql)
        {
            using var pgCtx = new PgBenchmarkDbContext();
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
