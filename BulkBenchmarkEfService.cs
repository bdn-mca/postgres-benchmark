using BenchmarkDotNet.Attributes;
using EFCore.BulkExtensions;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

[WarmupCount(0)]
[MinIterationCount(2)]
[MaxIterationCount(4)]
public class BulkBenchmarkEfService
{
    [Params(2000, 50000)]
    public int BatchSize { get; set; }

    [Benchmark]
    public async Task MsEfCoreBulk()
    {
        List<BenchmarkDbEntity> entities = [];
        for (int i = 1; i < 1_000_001; i++)
        {
            entities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = "MS EfCore",
                Type = 1,
                Properties = null,
            });
        }
        using var ctx = new MsBenchmarkDbContext();
        await ctx.BulkInsertAsync(entities,
            new BulkConfig
            {
                BatchSize = BatchSize
            });
    }

    [Benchmark]
    public async Task PgEfCoreBulk()
    {
        List<BenchmarkDbEntity> entities = [];
        for (int i = 1; i < 1_000_001; i++)
        {
            entities.Add(new BenchmarkDbEntity
            {
                Uid = Guid.NewGuid(),
                Id = i,
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = "PG EfCore",
                Type = 2,
                Properties = null,
            });
        }
        using var ctx = new PgBenchmarkDbContext();
        await ctx.BulkInsertAsync(entities,
            new BulkConfig
            {
                BatchSize = BatchSize
            });
    }
}
