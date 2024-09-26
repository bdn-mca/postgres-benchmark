using BenchmarkDotNet.Attributes;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
public class SmallBenchmarkEfService
{
    [ParamsAllValues]
    public DatabaseType BenchmarkDbType { get; set; }

    [Benchmark]
    public async Task EfCoreSingleAdd()
    {
        using IBenchmarkDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        for (int i = 1; i < 101; i++)
        {
            ctx.BenchmarkDbEntities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = $"{BenchmarkDbType.ToString()}-SingleAdd",
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
                Name = $"{BenchmarkDbType.ToString()}-RangeAdd",
                Type = 4,
                Properties = null,
            });
        }
        using IBenchmarkDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        ctx.BenchmarkDbEntities.AddRange(entities);
        await ctx.SaveChangesAsync();
    }
}
