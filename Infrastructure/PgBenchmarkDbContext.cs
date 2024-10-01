using Microsoft.EntityFrameworkCore;

namespace PostgreSqlBenchmark.Infrastructure;

internal class PgBenchmarkDbContext : BaseDbContext
{
    internal const string WindowsConnectionString = @"Host=localhost;Port:5432;Database=postgres;Username=postgres;Password=a123456!;Include Error Detail=true";
    internal const string LinuxConnectionString = @"Host=localhost;Port:5501;Database=postgres;Username=postgres;Password=a123456!;Include Error Detail=true";
    internal const string LinuxCitusConnectionString = @"Host=localhost;Port:5500;Database=postgres;Username=postgres;Password=a123456!;Include Error Detail=true";

    private readonly DatabaseType databaseType;

    public PgBenchmarkDbContext(DatabaseType databaseType)
        : base()
    {
        this.databaseType = databaseType;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(GetConnectionString(), pgOptions =>
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

    private string GetConnectionString()
    {
        switch (databaseType)
        {
            case DatabaseType.PostgreSqlWindows:
                return WindowsConnectionString;
            case DatabaseType.PostgreSqlLinux:
                return LinuxConnectionString;
            case DatabaseType.PostgreSqlCitus:
                return LinuxCitusConnectionString;
            default:
                throw new ArgumentException("Unknown value.", nameof(databaseType));
        }
    }
}
