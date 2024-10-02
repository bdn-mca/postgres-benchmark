using BenchmarkDotNet.Attributes;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
public class SmallOnLargeTableBenchmarkEfService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [Params(3_000_001, 10_000_001)]
    public int InitialTableSize { get; set; }

    [Benchmark]
    public async Task EfCoreRangeAdd()
    {
        List<BenchmarkDbEntity> entities = [];
        for (int i = 1; i < 101; i++)
        {
            entities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = $"{BenchmarkDbType.ToString()}-RangeAdd",
                Type = 4,
                Properties = null,
            });
        }
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        ctx.BenchmarkDbEntities.AddRange(entities);
        await ctx.SaveChangesAsync();
    }

    [GlobalSetup]
    public async Task PopulateTable()
    {
        List<BenchmarkDbEntity> entities = [];
        for (int i = 1; i < InitialTableSize; i++)
        {
            entities.Add(new BenchmarkDbEntity
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
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.BulkInsertAsync(entities,
            new BulkConfig
            {
                BatchSize = 200_000
            });
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
}
