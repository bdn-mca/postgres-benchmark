namespace PostgreSqlBenchmark.Infrastructure;
internal class BenchmarkDbEntity
{
    public Guid Uid { get; set; }

    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public string? Properties { get; set; }
}
