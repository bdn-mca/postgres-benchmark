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
            case DatabaseType.PostgreSql:
                return new PgBenchmarkDbContext();
            default:
                throw new ArgumentException("Unknown value.", nameof(dbType));
        }
    }
}
