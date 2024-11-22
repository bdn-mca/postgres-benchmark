using Microsoft.EntityFrameworkCore;

namespace PostgreSqlBenchmark.Infrastructure;

internal class MsBenchmarkDbContext : BaseDbContext
{
    const string ConnectionString = @"Server=.;Database=Benchmark;Trusted_Connection=True;TrustServerCertificate=true";

    public MsBenchmarkDbContext()
        : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(ConnectionString, msOptions =>
        {
            msOptions.CommandTimeout(300);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BenchmarkDbEntity>()
            .ToTable("benchmark", "dbo");

        modelBuilder.Entity<BenchmarkSpEntity>(e => e.HasNoKey());
        modelBuilder.Entity<BenchmarkCountSpEntity>(e => e.HasNoKey());
        modelBuilder.Entity<BenchmarkAggregateSpEntity>(e => e.HasNoKey());
    }
}
