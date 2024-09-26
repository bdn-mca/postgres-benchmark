using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;

internal interface IBenchmarkDbContext : IDisposable
{
    DbSet<BenchmarkDbEntity> BenchmarkDbEntities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
