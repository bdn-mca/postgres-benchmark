using BenchmarkDotNet.Running;
using PostgreSqlBenchmark.Benchmarks.PgAggregates;
using PostgreSqlBenchmark.Benchmarks.PgMsBigData;
using PostgreSqlBenchmark.Benchmarks.PgMsSmallData;

// small data
BenchmarkRunner.Run<SmallBenchmarkStoredProcService>();
BenchmarkRunner.Run<SmallBenchmarkEfService>();

// bulk
BenchmarkRunner.Run<BulkBenchmarkStoredProcService>();
BenchmarkRunner.Run<BulkBenchmarkEfService>();

// on large table
BenchmarkRunner.Run<SmallOnLargeTableBenchmarkEfService>();
BenchmarkRunner.Run<BigOnLargeTableBenchmarkEfService>();

// pg aggregates
BenchmarkRunner.Run<PgAggregatesSumBenchmarkService>();