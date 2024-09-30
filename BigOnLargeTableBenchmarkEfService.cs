using BenchmarkDotNet.Attributes;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
public class BigOnLargeTableBenchmarkEfService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [Params(3_000_001, 10_000_001)]
    public int InitialTableSize { get; set; }

    [Params(1000, 20_000)]
    public int BulkAddSize { get; set; }

    internal List<BenchmarkDbEntity> Entities { get; set; } = [];

    [IterationSetup]
    public void CleanAndPopulateEntities()
    {
        Entities.Clear();
        for (int i = 1; i <= BulkAddSize; i++)
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
    public async Task EfCoreBulkAdd()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.BulkInsertAsync(Entities,
            new BulkConfig
            {
                BatchSize = BulkAddSize + 1000
            });
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
        if (BenchmarkDbType == DatabaseType.PostgreSql)
        {
            using var pgCtx = new PgBenchmarkDbContext();
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
