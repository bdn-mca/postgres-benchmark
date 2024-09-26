using Microsoft.EntityFrameworkCore;

namespace PostgreSqlBenchmark.Infrastructure;

internal class PgBenchmarkDbContext : BaseDbContext
{
    const string ConnectionString = @"Host=localhost;Database=postgres;Username=postgres;Password=a123456!;Include Error Detail=true";

    public PgBenchmarkDbContext()
        : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(ConnectionString, pgOptions =>
        {
            pgOptions.CommandTimeout(300);
        });
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BenchmarkDbEntity>()
            .ToTable("benchmark", "public");

        modelBuilder.Entity<BenchmarkSpEntity>(e => e.HasNoKey());
    }
}
