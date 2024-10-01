namespace PostgreSqlBenchmark;

[Flags]
public enum DatabaseType
{
    MsSql = 1,
    PostgreSqlWindows = 2,
    PostgreSqlLinux = 4,
    PostgreSqlCitus = 8
}
