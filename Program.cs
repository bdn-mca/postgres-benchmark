using BenchmarkDotNet.Running;
using PostgreSqlBenchmark;

BenchmarkRunner.Run<SmallBenchmarkStoredProcService>();
BenchmarkRunner.Run<SmallBenchmarkEfService>();

BenchmarkRunner.Run<BulkBenchmarkEfService>();
BenchmarkRunner.Run<BulkBenchmarkStoredProcService>();