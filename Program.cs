using BenchmarkDotNet.Running;
using PostgreSqlBenchmark;

BenchmarkRunner.Run<SmallBenchmarkStoredProcService>();
BenchmarkRunner.Run<SmallBenchmarkEfService>();
BenchmarkRunner.Run<BulkBenchmarkStoredProcService>();
BenchmarkRunner.Run<BulkBenchmarkEfService>();