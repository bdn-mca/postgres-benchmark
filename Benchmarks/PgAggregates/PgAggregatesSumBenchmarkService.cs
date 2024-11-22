using BenchmarkDotNet.Attributes;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark.Benchmarks.PgAggregates;

[WarmupCount(0)]
public class PgAggregatesSumBenchmarkService
{
    [Params(DatabaseType.MsSql, DatabaseType.PostgreSqlLinux, DatabaseType.PostgreSqlCitus)]
    public DatabaseType BenchmarkDbType { get; set; }

    [Params(50_000, 250_000, 1_000_000)]
    public int InitialTableSize { get; set; }

    public int Types { get; set; } = 20;

    public int TypeFilter { get; set; } = 5;

    [Benchmark]
    public async Task ExecuteCount()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.Set<BenchmarkCountSpEntity>().FromSqlRaw(GetCountBenchmarkQuery()).ToListAsync();
    }

    [Benchmark]
    public async Task ExecuteAvg()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.Set<BenchmarkAggregateSpEntity>().FromSqlRaw(GetAggregateBenchmarkQuery("avg")).ToListAsync();
    }

    [Benchmark]
    public async Task ExecuteSum()
    {
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.Set<BenchmarkAggregateSpEntity>().FromSqlRaw(GetAggregateBenchmarkQuery("sum")).ToListAsync();
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
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Name = BenchmarkDbType.ToString(),
                Type = new Random().Next(1, 20),
                Properties = null,
                Amount = (decimal)(new Random().NextDouble() * 100),
            });
        }
        using BaseDbContext ctx = DbContextFactory.GetBenchmarkDbContext(BenchmarkDbType);
        await ctx.BulkInsertAsync(entities,
            new BulkConfig
            {
                BatchSize = 200_000
            });

        // await ctx.Set<BenchmarkSpEntity>().FromSqlRaw(GetGlobalSetupCommand()).ToListAsync();
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
            await msCtx.Set<BenchmarkSpEntity>().FromSqlRaw("delete from benchmark").ToListAsync();
        }
    }

    private string GetCountBenchmarkQuery()
    {
        return BenchmarkDbType switch
        {
            DatabaseType.MsSql
                => $"select [Type] as [Type], count(*) as Result from benchmark where [Type] = {TypeFilter} group by [Type] order by [Type]",
            DatabaseType.PostgreSqlWindows or DatabaseType.PostgreSqlLinux or DatabaseType.PostgreSqlCitus
                => $"select type as Type, count(*) as Result from public.benchmark where type = {TypeFilter} group by type order by type",
            _ => throw new ArgumentException("Unknown value.", nameof(BenchmarkDbType)),
        };
    }

    private string GetAggregateBenchmarkQuery(string operation)
    {
        return BenchmarkDbType switch
        {
            DatabaseType.MsSql
                => $"select [Type] as [Type], {operation}(Amount) as Result from benchmark where [Type] = {TypeFilter} group by [Type] order by [Type]",
            DatabaseType.PostgreSqlWindows or DatabaseType.PostgreSqlLinux or DatabaseType.PostgreSqlCitus
                => $"select type as Type, {operation}(amount) as Result from public.benchmark where type = {TypeFilter} group by type order by type",
            _ => throw new ArgumentException("Unknown value.", nameof(BenchmarkDbType)),
        };
    }
}
