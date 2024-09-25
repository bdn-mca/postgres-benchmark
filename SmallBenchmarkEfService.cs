using BenchmarkDotNet.Attributes;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
public class SmallBenchmarkEfService
{
    [Benchmark]
    public async Task MsEfCoreSingleAdd()
    {
        using var ctx = new MsBenchmarkDbContext();
        for (int i = 1; i < 101; i++)
        {
            ctx.BenchmarkDbEntities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = "MS EfCore-SingleAdd",
                Type = 3,
                Properties = null,
            });
        }
        await ctx.SaveChangesAsync();
    }

    [Benchmark]
    public async Task MsEfCoreRangeAdd()
    {
        List<BenchmarkDbEntity> entities = new List<BenchmarkDbEntity>();
        for (int i = 1; i < 101; i++)
        {
            entities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = "MS EfCore-RangeAdd",
                Type = 4,
                Properties = null,
            });
        }
        using var ctx = new MsBenchmarkDbContext();
        ctx.BenchmarkDbEntities.AddRange(entities);
        await ctx.SaveChangesAsync();
    }

    [Benchmark]
    public async Task PgEfCoreSingleAdd()
    {
        using var ctx = new PgBenchmarkDbContext();
        for (int i = 1; i < 101; i++)
        {
            ctx.BenchmarkDbEntities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = "PG EfCore-SingleAdd",
                Type = 5,
                Properties = null,
            });
        }
        await ctx.SaveChangesAsync();
    }

    [Benchmark]
    public async Task PgEfCoreRangeAdd()
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
                Name = "PG EfCore-RangeAdd",
                Type = 6,
                Properties = null,
            });
        }
        using var ctx = new PgBenchmarkDbContext();
        ctx.BenchmarkDbEntities.AddRange(entities);
        await ctx.SaveChangesAsync();
    }
}
