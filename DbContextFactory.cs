using PostgreSqlBenchmark.Infrastructure;

namespace PostgreSqlBenchmark;
internal class DbContextFactory
{
    public static BaseDbContext GetBenchmarkDbContext(DatabaseType dbType)
    {
        switch (dbType)
        {
            case DatabaseType.MsSql:
                return new MsBenchmarkDbContext();
            case DatabaseType.PostgreSqlWindows:
            case DatabaseType.PostgreSqlLinux:
            case DatabaseType.PostgreSqlCitus:
                return new PgBenchmarkDbContext(dbType);
            default:
                throw new ArgumentException("Unknown value.", nameof(dbType));
        }
    }
}
