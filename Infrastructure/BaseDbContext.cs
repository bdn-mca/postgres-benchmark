using Microsoft.EntityFrameworkCore;

namespace PostgreSqlBenchmark.Infrastructure;

internal abstract class BaseDbContext : DbContext
{
    protected BaseDbContext()
        :base()
    {
    }

    public virtual DbSet<BenchmarkDbEntity> BenchmarkDbEntities { get; set; }
}
