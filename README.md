# postgres-benchmark
Benchmarking postgres performance with EF Core and Stored procedures vs. the same in MS SQL.

# Getting started
- Manually create the tables and stored procedures
- Contains 2 services, one for small number of items (100), and another for bulk (1.000.000). You can comment out the one you don't want to benchmark.

# Benchmarks

## Small stored procedures
Stored procedures inserting 100 items.

| Method            | Mean     | Error     | StdDev    |
|------------------ |---------:|----------:|----------:|
| MsStoredProcedure | 1.710 ms | 0.0304 ms | 0.0455 ms |
| PgStoredProcedure | 1.196 ms | 0.0232 ms | 0.0228 ms |

## Small entity framework (not bulk)
Inserting 100 items with entity framework, without using bulk extensions.
One is adding entities one by one in a foreach, the other is adding a range from a list.

| Method            | Mean     | Error     | StdDev    |
|------------------ |---------:|----------:|----------:|
| MsEfCoreSingleAdd | 7.331 ms | 0.1289 ms | 0.1206 ms |
| MsEfCoreRangeAdd  | 7.014 ms | 0.1022 ms | 0.0853 ms |
| PgEfCoreSingleAdd | 5.889 ms | 0.0525 ms | 0.0410 ms |
| PgEfCoreRangeAdd  | 5.868 ms | 0.0762 ms | 0.0675 ms |

## Bulk entity framework
Inserting 1.000.000 items with entity framework, using bulk extensions.
The BatchSize parameter is the EFCore.Extensions BatchSize property.

| Method       | BatchSize | Mean    | Error    | StdDev  |
|------------- |---------- |--------:|---------:|--------:|
| MsEfCoreBulk | 2000      | 15.58 s |  5.545 s | 0.304 s |
| PgEfCoreBulk | 2000      | 11.34 s | 16.317 s | 2.525 s |
| MsEfCoreBulk | 50000     | 10.41 s |  0.603 s | 0.033 s |
| PgEfCoreBulk | 50000     | 17.36 s | 10.294 s | 1.593 s |

## Bulk stored procedures
Inserting 1.000.000 items with stored procedures, using general_series.

| Method            | Mean     | Error    | StdDev  |
|------------------ |---------:|---------:|--------:|
| MsStoredProcedure |  9.287 s |  8.576 s | 1.327 s |
| PgStoredProcedure | 30.875 s | 23.716 s | 3.670 s |