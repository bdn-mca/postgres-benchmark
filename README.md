# MsSql - PostgreSql benchmark
Benchmarking PostgreSql 16 vs. MS SQL 2022 performance with EF Core and Stored procedures.

# Getting started
- Manually create the tables and stored procedures
  - You can find the scripts in the [Database](https://github.com/bdn-mca/postgres-benchmark/tree/main/Database) folder
- Build and Run in `Release` configuration

# Benchmarks
## Legend
- Error           : Half of 99.9% confidence interval
- StdDev          : Standard deviation of all measurements

## Small stored procedures
Stored procedures inserting 100 items.

| Method          | BenchmarkDbType | Mean     | Error     | StdDev    | Median   |
|---------------- |---------------- |---------:|----------:|----------:|---------:|
| StoredProcedure | MsSql           | 1.939 ms | 0.0627 ms | 0.1839 ms | 1.889 ms |
| StoredProcedure | PostgreSql      | 1.220 ms | 0.0242 ms | 0.0645 ms | 1.206 ms |

#### Small stored procedure [BenchmarkDbType=MsSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 1.939 ms, StdErr = 0.018 ms (0.95%), N = 99, StdDev = 0.184 ms
Min = 1.651 ms, Q1 = 1.812 ms, Median = 1.889 ms, Q3 = 2.054 ms, Max = 2.422 ms
IQR = 0.241 ms, LowerFence = 1.451 ms, UpperFence = 2.416 ms
ConfidenceInterval = [1.876 ms; 2.002 ms] (CI 99.9%), Margin = 0.063 ms (3.23% of Mean)
Skewness = 0.73, Kurtosis = 2.74, MValue = 2.13

```
-------------------- Histogram --------------------
[1.599 ms ; 1.687 ms) | @@
[1.687 ms ; 1.804 ms) | @@@@@@@@@@@@@@@@@@@@
[1.804 ms ; 1.909 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[1.909 ms ; 2.020 ms) | @@@@@@@@@@@@@@@@
[2.020 ms ; 2.143 ms) | @@@@@@@@@@@@@@@@
[2.143 ms ; 2.239 ms) | @@@@
[2.239 ms ; 2.343 ms) | @@@@@@@@
[2.343 ms ; 2.440 ms) | @@
---------------------------------------------------
```

#### Small stored procedure [BenchmarkDbType=PostgreSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 1.220 ms, StdErr = 0.007 ms (0.58%), N = 83, StdDev = 0.064 ms
Min = 1.125 ms, Q1 = 1.172 ms, Median = 1.206 ms, Q3 = 1.255 ms, Max = 1.412 ms
IQR = 0.083 ms, LowerFence = 1.048 ms, UpperFence = 1.380 ms
ConfidenceInterval = [1.195 ms; 1.244 ms] (CI 99.9%), Margin = 0.024 ms (1.98% of Mean)
Skewness = 0.92, Kurtosis = 3.36, MValue = 2

```
-------------------- Histogram --------------------
[1.106 ms ; 1.150 ms) | @@@@@@@
[1.150 ms ; 1.189 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@
[1.189 ms ; 1.232 ms) | @@@@@@@@@@@@@@@@@@@@@
[1.232 ms ; 1.272 ms) | @@@@@@@@@@@
[1.272 ms ; 1.314 ms) | @@@@@@@@@@
[1.314 ms ; 1.372 ms) | @@@@@
[1.372 ms ; 1.431 ms) | @@
---------------------------------------------------
```

#### Outliers
SmallBenchmarkStoredProcService.StoredProcedure: WarmupCount=0 -> 1 outlier  was  removed (2.46 ms)
SmallBenchmarkStoredProcService.StoredProcedure: WarmupCount=0 -> 5 outliers were removed (1.44 ms..1.99 ms)

## Small EF Core (not bulk)
Inserting 100 items with entity framework, without using bulk extensions.
- One uses `DbSet.Add` and is adding entities one by one in a foreach
- The other uses `DbSet.AddRange` and is adding a range from a list in one go

| Method           | BenchmarkDbType | Mean     | Error     | StdDev    |
|----------------- |---------------- |---------:|----------:|----------:|
| SingleAdd        | MsSql           | 7.212 ms | 0.1389 ms | 0.1299 ms |
| RangeAdd         | MsSql           | 7.148 ms | 0.1317 ms | 0.2050 ms |
| SingleAdd        | PostgreSql      | 6.140 ms | 0.1227 ms | 0.2116 ms |
| RangeAdd         | PostgreSql      | 6.095 ms | 0.1205 ms | 0.1767 ms |

#### Small EF Core Single Add: [BenchmarkDbType=MsSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.212 ms, StdErr = 0.034 ms (0.47%), N = 15, StdDev = 0.130 ms
Min = 7.028 ms, Q1 = 7.134 ms, Median = 7.178 ms, Q3 = 7.300 ms, Max = 7.445 ms
IQR = 0.166 ms, LowerFence = 6.885 ms, UpperFence = 7.549 ms
ConfidenceInterval = [7.073 ms; 7.351 ms] (CI 99.9%), Margin = 0.139 ms (1.93% of Mean)
Skewness = 0.38, Kurtosis = 1.78, MValue = 2

```
-------------------- Histogram --------------------
[6.959 ms ; 7.111 ms) | @@@
[7.111 ms ; 7.477 ms) | @@@@@@@@@@@@
---------------------------------------------------
```

#### Small EF Core Range Add: [BenchmarkDbType=MsSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.148 ms, StdErr = 0.036 ms (0.51%), N = 32, StdDev = 0.205 ms
Min = 6.917 ms, Q1 = 6.998 ms, Median = 7.065 ms, Q3 = 7.227 ms, Max = 7.624 ms
IQR = 0.230 ms, LowerFence = 6.653 ms, UpperFence = 7.572 ms
ConfidenceInterval = [7.017 ms; 7.280 ms] (CI 99.9%), Margin = 0.132 ms (1.84% of Mean)
Skewness = 0.96, Kurtosis = 2.76, MValue = 2

```
-------------------- Histogram --------------------
[6.912 ms ; 7.081 ms) | @@@@@@@@@@@@@@@@@
[7.081 ms ; 7.290 ms) | @@@@@@@
[7.290 ms ; 7.460 ms) | @@@@@
[7.460 ms ; 7.661 ms) | @@@
---------------------------------------------------
```

#### Small EF Core Single Add: [BenchmarkDbType=PostgreSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.140 ms, StdErr = 0.034 ms (0.56%), N = 38, StdDev = 0.212 ms
Min = 5.755 ms, Q1 = 5.976 ms, Median = 6.146 ms, Q3 = 6.293 ms, Max = 6.635 ms
IQR = 0.317 ms, LowerFence = 5.500 ms, UpperFence = 6.768 ms
ConfidenceInterval = [6.017 ms; 6.262 ms] (CI 99.9%), Margin = 0.123 ms (2.00% of Mean)
Skewness = 0.14, Kurtosis = 2.31, MValue = 2

```
-------------------- Histogram --------------------
[5.742 ms ; 5.910 ms) | @@@@@
[5.910 ms ; 6.076 ms) | @@@@@@@@@@@
[6.076 ms ; 6.330 ms) | @@@@@@@@@@@@@@@@
[6.330 ms ; 6.522 ms) | @@@@@
[6.522 ms ; 6.718 ms) | @
---------------------------------------------------
```

#### Small EF Core Range Add: [BenchmarkDbType=PostgreSql]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.095 ms, StdErr = 0.033 ms (0.54%), N = 29, StdDev = 0.177 ms
Min = 5.781 ms, Q1 = 5.947 ms, Median = 6.083 ms, Q3 = 6.209 ms, Max = 6.447 ms
IQR = 0.262 ms, LowerFence = 5.555 ms, UpperFence = 6.602 ms
ConfidenceInterval = [5.974 ms; 6.215 ms] (CI 99.9%), Margin = 0.121 ms (1.98% of Mean)
Skewness = 0.15, Kurtosis = 2.21, MValue = 2

```
-------------------- Histogram --------------------
[5.723 ms ; 5.890 ms) | @@@
[5.890 ms ; 6.069 ms) | @@@@@@@@@
[6.069 ms ; 6.220 ms) | @@@@@@@@@@@@
[6.220 ms ; 6.448 ms) | @@@@@
---------------------------------------------------
```

#### Outliers
SmallBenchmarkEfService.EfCoreSingleAdd: WarmupCount=0  -> 1 outlier  was  removed (7.87 ms)
SmallBenchmarkEfService.MsEfCoreRangeAdd: WarmupCount=0 -> 1 outlier  was  removed (7.74 ms)
SmallBenchmarkEfService.MsEfCoreRangeAdd: WarmupCount=0 -> 1 outlier  was  removed (9.53 ms)

## Bulk stored procedures
Inserting 1.000.000 items with stored procedures, using `general_series`.
`CleanupTable` tells if the data in the `benchmark` table was deleted (for PG additionally vacuumed) after each iteration.

| Method          | BenchmarkDbType | CleanupTable | Mean     | Error     | StdDev    |
|---------------- |---------------- |------------- |---------:|----------:|----------:|
| StoredProcedure | MsSql           | False        | 14.232 s |  2.8237 s |  4.1390 s |
| StoredProcedure | MsSql           | True         |  6.717 s |  0.2145 s |  0.3211 s |
| StoredProcedure | PostgreSql      | False        | 34.746 s | 11.5699 s | 17.3172 s |
| StoredProcedure | PostgreSql      | True         |  8.366 s |  0.1627 s |  0.1741 s |

#### Bulk stored procedure [BenchmarkDbType=MsSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 14.232 s, StdErr = 0.769 s (5.40%), N = 29, StdDev = 4.139 s
Min = 7.258 s, Q1 = 12.793 s, Median = 16.215 s, Q3 = 16.598 s, Max = 21.659 s
IQR = 3.805 s, LowerFence = 7.086 s, UpperFence = 22.304 s
ConfidenceInterval = [11.408 s; 17.056 s] (CI 99.9%), Margin = 2.824 s (19.84% of Mean)
Skewness = -0.56, Kurtosis = 2.12, MValue = 3

```
-------------------- Histogram --------------------
[ 6.053 s ;  9.589 s) | @@@@@@@
[ 9.589 s ; 11.141 s) |
[11.141 s ; 13.979 s) | @@
[13.979 s ; 17.516 s) | @@@@@@@@@@@@@@@@@@
[17.516 s ; 19.183 s) |
[19.183 s ; 22.719 s) | @@
---------------------------------------------------
```

#### Bulk stored procedure [BenchmarkDbType=MsSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.717 s, StdErr = 0.059 s (0.87%), N = 30, StdDev = 0.321 s
Min = 6.185 s, Q1 = 6.425 s, Median = 6.770 s, Q3 = 6.914 s, Max = 7.287 s
IQR = 0.489 s, LowerFence = 5.692 s, UpperFence = 7.647 s
ConfidenceInterval = [6.502 s; 6.931 s] (CI 99.9%), Margin = 0.215 s (3.19% of Mean)
Skewness = -0.13, Kurtosis = 1.89, MValue = 2.77

```
-------------------- Histogram --------------------
[6.177 s ; 6.449 s) | @@@@@@@@@
[6.449 s ; 6.733 s) | @@@@
[6.733 s ; 7.004 s) | @@@@@@@@@@@@@
[7.004 s ; 7.298 s) | @@@@
---------------------------------------------------
```

#### Bulk stored procedure [BenchmarkDbType=PostgreSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 34.746 s, StdErr = 3.162 s (9.10%), N = 30, StdDev = 17.317 s
Min = 7.929 s, Q1 = 20.534 s, Median = 32.434 s, Q3 = 51.746 s, Max = 59.714 s
IQR = 31.212 s, LowerFence = -26.283 s, UpperFence = 98.563 s
ConfidenceInterval = [23.176 s; 46.315 s] (CI 99.9%), Margin = 11.570 s (33.30% of Mean)
Skewness = 0.01, Kurtosis = 1.5, MValue = 3

```
-------------------- Histogram --------------------
[ 0.614 s ; 16.791 s) | @@@@@
[16.791 s ; 31.420 s) | @@@@@@@@@@
[31.420 s ; 46.251 s) | @@@@@
[46.251 s ; 60.880 s) | @@@@@@@@@@
---------------------------------------------------
```

#### Bulk stored procedure [BenchmarkDbType=PostgreSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.366 s, StdErr = 0.041 s (0.49%), N = 18, StdDev = 0.174 s
Min = 8.070 s, Q1 = 8.264 s, Median = 8.335 s, Q3 = 8.515 s, Max = 8.655 s
IQR = 0.251 s, LowerFence = 7.887 s, UpperFence = 8.892 s
ConfidenceInterval = [8.203 s; 8.529 s] (CI 99.9%), Margin = 0.163 s (1.94% of Mean)
Skewness = 0.06, Kurtosis = 1.8, MValue = 2

```
-------------------- Histogram --------------------
[7.983 s ; 8.183 s) | @@
[8.183 s ; 8.358 s) | @@@@@@@@@
[8.358 s ; 8.662 s) | @@@@@@@
---------------------------------------------------
```

#### Outliers
BulkBenchmarkStoredProcService.StoredProcedure: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed (23.64 s)
BulkBenchmarkStoredProcService.StoredProcedure: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 4 outliers were removed (9.58 s..10.18 s)

## Bulk Entity Framework Core
Inserting 1.000.000 items with EF Core, using EF.BulkExtensions.
- `CleanupTable` tells if the data in the `benchmark` table was deleted (for PG additionally vacuumed) after each iteration.
- `BatchSize` is the EFCore.Extensions BatchSize property - meaning the insert is done multiple times with the determined batch number of items.

| Method     | BatchSize | BenchmarkDbType | CleanupTable | Mean     | Error     | StdDev    | Median   |
|----------- |---------- |---------------- |------------- |---------:|----------:|----------:|---------:|
| EfCoreBulk | 2000      | MsSql           | False        | 23.840 s |  2.2654 s |  3.3907 s | 25.082 s |
| EfCoreBulk | 2000      | MsSql           | True         | 14.088 s |  0.2732 s |  0.2556 s | 13.998 s |
| EfCoreBulk | 2000      | PostgreSql      | False        | 30.771 s | 12.8518 s | 19.2359 s | 23.212 s |
| EfCoreBulk | 2000      | PostgreSql      | True         |  5.444 s |  0.0626 s |  0.0586 s |  5.423 s |
| EfCoreBulk | 50000     | MsSql           | False        | 16.129 s |  1.9089 s |  2.8572 s | 17.407 s |
| EfCoreBulk | 50000     | MsSql           | True         |  8.657 s |  0.1659 s |  0.1844 s |  8.589 s |
| EfCoreBulk | 50000     | PostgreSql      | False        | 31.127 s | 12.5795 s | 18.8285 s | 24.009 s |
| EfCoreBulk | 50000     | PostgreSql      | True         |  5.415 s |  0.0872 s |  0.0773 s |  5.421 s |

#### Bulk EF Core [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 23.840 s, StdErr = 0.619 s (2.60%), N = 30, StdDev = 3.391 s
Min = 14.613 s, Q1 = 23.104 s, Median = 25.082 s, Q3 = 26.042 s, Max = 27.686 s
IQR = 2.938 s, LowerFence = 18.697 s, UpperFence = 30.449 s
ConfidenceInterval = [21.574 s; 26.105 s] (CI 99.9%), Margin = 2.265 s (9.50% of Mean)
Skewness = -1.44, Kurtosis = 4.15, MValue = 2.5

```
-------------------- Histogram --------------------
[13.181 s ; 16.161 s) | @@
[16.161 s ; 19.219 s) | @
[19.219 s ; 22.084 s) | @@@@
[22.084 s ; 23.826 s) | @
[23.826 s ; 26.691 s) | @@@@@@@@@@@@@@@@@@@@@
[26.691 s ; 29.118 s) | @
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 14.088 s, StdErr = 0.066 s (0.47%), N = 15, StdDev = 0.256 s
Min = 13.726 s, Q1 = 13.931 s, Median = 13.998 s, Q3 = 14.325 s, Max = 14.455 s
IQR = 0.393 s, LowerFence = 13.341 s, UpperFence = 14.915 s
ConfidenceInterval = [13.815 s; 14.361 s] (CI 99.9%), Margin = 0.273 s (1.94% of Mean)
Skewness = 0.25, Kurtosis = 1.46, MValue = 2

```
-------------------- Histogram --------------------
[13.590 s ; 14.019 s) | @@@@@@@@@
[14.019 s ; 14.478 s) | @@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=2000, BenchmarkDbType=PostgreSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 30.771 s, StdErr = 3.512 s (11.41%), N = 30, StdDev = 19.236 s
Min = 5.170 s, Q1 = 15.441 s, Median = 23.212 s, Q3 = 51.885 s, Max = 60.232 s
IQR = 36.444 s, LowerFence = -39.225 s, UpperFence = 106.550 s
ConfidenceInterval = [17.919 s; 43.623 s] (CI 99.9%), Margin = 12.852 s (41.77% of Mean)
Skewness = 0.31, Kurtosis = 1.43, MValue = 2.67

```
-------------------- Histogram --------------------
[ 5.056 s ; 21.306 s) | @@@@@@@@@@@@@@@
[21.306 s ; 41.244 s) | @@@@@
[41.244 s ; 61.794 s) | @@@@@@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=2000, BenchmarkDbType=PostgreSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.444 s, StdErr = 0.015 s (0.28%), N = 15, StdDev = 0.059 s
Min = 5.358 s, Q1 = 5.406 s, Median = 5.423 s, Q3 = 5.489 s, Max = 5.563 s
IQR = 0.083 s, LowerFence = 5.281 s, UpperFence = 5.614 s
ConfidenceInterval = [5.381 s; 5.506 s] (CI 99.9%), Margin = 0.063 s (1.15% of Mean)
Skewness = 0.46, Kurtosis = 1.98, MValue = 2

```
-------------------- Histogram --------------------
[5.352 s ; 5.594 s) | @@@@@@@@@@@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 16.129 s, StdErr = 0.522 s (3.23%), N = 30, StdDev = 2.857 s
Min = 8.958 s, Q1 = 16.550 s, Median = 17.407 s, Q3 = 17.842 s, Max = 18.213 s
IQR = 1.292 s, LowerFence = 14.612 s, UpperFence = 19.780 s
ConfidenceInterval = [14.220 s; 18.038 s] (CI 99.9%), Margin = 1.909 s (11.84% of Mean)
Skewness = -1.51, Kurtosis = 3.71, MValue = 2

```
-------------------- Histogram --------------------
[ 8.816 s ; 11.230 s) | @@@@
[11.230 s ; 13.823 s) | @@
[13.823 s ; 15.925 s) |
[15.925 s ; 18.339 s) | @@@@@@@@@@@@@@@@@@@@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.657 s, StdErr = 0.042 s (0.49%), N = 19, StdDev = 0.184 s
Min = 8.363 s, Q1 = 8.517 s, Median = 8.589 s, Q3 = 8.855 s, Max = 8.939 s
IQR = 0.339 s, LowerFence = 8.009 s, UpperFence = 9.363 s
ConfidenceInterval = [8.491 s; 8.823 s] (CI 99.9%), Margin = 0.166 s (1.92% of Mean)
Skewness = 0.26, Kurtosis = 1.43, MValue = 2

```
-------------------- Histogram --------------------
[8.272 s ; 8.453 s) | @
[8.453 s ; 8.635 s) | @@@@@@@@@@
[8.635 s ; 8.962 s) | @@@@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=50000, BenchmarkDbType=PostgreSql, CleanupTable=False]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 31.127 s, StdErr = 3.438 s (11.04%), N = 30, StdDev = 18.828 s
Min = 5.771 s, Q1 = 16.059 s, Median = 24.009 s, Q3 = 50.338 s, Max = 61.805 s
IQR = 34.279 s, LowerFence = -35.359 s, UpperFence = 101.756 s
ConfidenceInterval = [18.548 s; 43.707 s] (CI 99.9%), Margin = 12.580 s (40.41% of Mean)
Skewness = 0.31, Kurtosis = 1.49, MValue = 3

```
-------------------- Histogram --------------------
[ 4.931 s ; 20.838 s) | @@@@@@@@@@@@@@
[20.838 s ; 37.584 s) | @@@@@
[37.584 s ; 46.595 s) | @@
[46.595 s ; 62.501 s) | @@@@@@@@@
---------------------------------------------------
```

#### Bulk EF Core [BatchSize=50000, BenchmarkDbType=PostgreSql, CleanupTable=True]

Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.415 s, StdErr = 0.021 s (0.38%), N = 14, StdDev = 0.077 s
Min = 5.288 s, Q1 = 5.374 s, Median = 5.421 s, Q3 = 5.480 s, Max = 5.515 s
IQR = 0.106 s, LowerFence = 5.216 s, UpperFence = 5.638 s
ConfidenceInterval = [5.327 s; 5.502 s] (CI 99.9%), Margin = 0.087 s (1.61% of Mean)
Skewness = -0.36, Kurtosis = 1.69, MValue = 2

```
-------------------- Histogram --------------------
[5.246 s ; 5.530 s) | @@@@@@@@@@@@@@
---------------------------------------------------
```

#### Outliers
BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 3 outliers were detected (14.61 s..17.53 s)
BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 6 outliers were detected (8.96 s..13.09 s)
BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed (5.68 s, 5.68 s)

## Small inserts on already large table
Table consisting of millions of rows receives non-bulk small inserts of 100s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start

| Method         | InitialTableSize | BenchmarkDbType | Mean     | Error     | StdDev    |
|--------------- |----------------- |---------------- |---------:|----------:|----------:|
| EfCoreRangeAdd | 3000001          | MsSql           | 7.364 ms | 0.1278 ms | 0.1195 ms |
| EfCoreRangeAdd | 3000001          | PostgreSql      | 5.615 ms | 0.0790 ms | 0.0739 ms |
| EfCoreRangeAdd | 10000001         | MsSql           | 7.599 ms | 0.1425 ms | 0.3677 ms |
| EfCoreRangeAdd | 10000001         | PostgreSql      | 7.194 ms | 0.1040 ms | 0.0973 ms |

#### [InitialTableSize=3.000.001, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.364 ms, StdErr = 0.031 ms (0.42%), N = 15, StdDev = 0.120 ms
Min = 7.082 ms, Q1 = 7.301 ms, Median = 7.371 ms, Q3 = 7.452 ms, Max = 7.528 ms
IQR = 0.151 ms, LowerFence = 7.075 ms, UpperFence = 7.679 ms
ConfidenceInterval = [7.236 ms; 7.492 ms] (CI 99.9%), Margin = 0.128 ms (1.74% of Mean)
Skewness = -0.7, Kurtosis = 2.7, MValue = 2
```
-------------------- Histogram --------------------
[7.019 ms ; 7.208 ms) | @
[7.208 ms ; 7.592 ms) | @@@@@@@@@@@@@@
---------------------------------------------------
```

#### [InitialTableSize=3.000.001, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.615 ms, StdErr = 0.019 ms (0.34%), N = 15, StdDev = 0.074 ms
Min = 5.485 ms, Q1 = 5.557 ms, Median = 5.612 ms, Q3 = 5.668 ms, Max = 5.746 ms
IQR = 0.111 ms, LowerFence = 5.392 ms, UpperFence = 5.834 ms
ConfidenceInterval = [5.536 ms; 5.694 ms] (CI 99.9%), Margin = 0.079 ms (1.41% of Mean)
Skewness = -0.06, Kurtosis = 1.82, MValue = 2
```
-------------------- Histogram --------------------
[5.446 ms ; 5.582 ms) | @@@@@
[5.582 ms ; 5.786 ms) | @@@@@@@@@@
---------------------------------------------------
```

#### [InitialTableSize=10.000.001, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.599 ms, StdErr = 0.042 ms (0.55%), N = 78, StdDev = 0.368 ms
Min = 6.795 ms, Q1 = 7.373 ms, Median = 7.522 ms, Q3 = 7.699 ms, Max = 8.676 ms
IQR = 0.326 ms, LowerFence = 6.883 ms, UpperFence = 8.189 ms
ConfidenceInterval = [7.456 ms; 7.741 ms] (CI 99.9%), Margin = 0.142 ms (1.87% of Mean)
Skewness = 0.97, Kurtosis = 3.95, MValue = 2
```
-------------------- Histogram --------------------
[6.682 ms ; 6.943 ms) | @
[6.943 ms ; 7.169 ms) | @@@@@
[7.169 ms ; 7.440 ms) | @@@@@@@@@@@@@@@@@@
[7.440 ms ; 7.666 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[7.666 ms ; 7.915 ms) | @@@@@@@@@
[7.915 ms ; 8.106 ms) | @@@
[8.106 ms ; 8.282 ms) | @@
[8.282 ms ; 8.508 ms) | @@@@@
[8.508 ms ; 8.749 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=10.000.001, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.194 ms, StdErr = 0.025 ms (0.35%), N = 15, StdDev = 0.097 ms
Min = 7.086 ms, Q1 = 7.111 ms, Median = 7.169 ms, Q3 = 7.250 ms, Max = 7.399 ms
IQR = 0.138 ms, LowerFence = 6.904 ms, UpperFence = 7.457 ms
ConfidenceInterval = [7.090 ms; 7.298 ms] (CI 99.9%), Margin = 0.104 ms (1.45% of Mean)
Skewness = 0.59, Kurtosis = 2.04, MValue = 2
```
-------------------- Histogram --------------------
[7.056 ms ; 7.451 ms) | @@@@@@@@@@@@@@@
---------------------------------------------------
```

#### Outliers
- SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: WarmupCount=0 -> 13 outliers were removed (8.87 ms..14.26 ms)

## Bulk inserts on already large table
Table consisting of millions of rows receives bulk inserts of 1000s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start
- `BulkAddSize` - number of rows added in each iteration in bulk

| Method        | InitialTableSize | BulkAddSize | BenchmarkDbType | Mean       | Error      | StdDev     | Median     |
|-------------- |----------------- |------------ |---------------- |-----------:|-----------:|-----------:|-----------:|
| EfCoreBulkAdd | 3000001          | 1000        | MsSql           |  19.321 ms |  0.3846 ms |  0.5001 ms |  19.261 ms |
| EfCoreBulkAdd | 3000001          | 1000        | PostgreSql      |   7.281 ms |  0.1450 ms |  0.4114 ms |   7.204 ms |
| EfCoreBulkAdd | 3000001          | 20000       | MsSql           | 195.782 ms |  3.7992 ms |  4.6657 ms | 194.447 ms |
| EfCoreBulkAdd | 3000001          | 20000       | PostgreSql      | 124.061 ms |  9.3142 ms | 27.4632 ms | 118.442 ms |
| EfCoreBulkAdd | 10000001         | 1000        | MsSql           |  19.548 ms |  0.3695 ms |  0.5416 ms |  19.461 ms |
| EfCoreBulkAdd | 10000001         | 1000        | PostgreSql      |  17.669 ms |  0.3505 ms |  0.6231 ms |  17.602 ms |
| EfCoreBulkAdd | 10000001         | 20000       | MsSql           | 233.629 ms | 13.3769 ms | 38.8087 ms | 205.016 ms |
| EfCoreBulkAdd | 10000001         | 20000       | PostgreSql      | 284.598 ms |  5.6219 ms |  5.5215 ms | 284.365 ms |

#### [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 19.321 ms, StdErr = 0.102 ms (0.53%), N = 24, StdDev = 0.500 ms
Min = 18.603 ms, Q1 = 18.971 ms, Median = 19.261 ms, Q3 = 19.634 ms, Max = 20.464 ms
IQR = 0.662 ms, LowerFence = 17.978 ms, UpperFence = 20.627 ms
ConfidenceInterval = [18.936 ms; 19.705 ms] (CI 99.9%), Margin = 0.385 ms (1.99% of Mean)
Skewness = 0.62, Kurtosis = 2.46, MValue = 2
```
-------------------- Histogram --------------------
[18.375 ms ; 18.824 ms) | @@
[18.824 ms ; 19.279 ms) | @@@@@@@@@@@
[19.279 ms ; 19.963 ms) | @@@@@@@@@
[19.963 ms ; 20.692 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.281 ms, StdErr = 0.043 ms (0.59%), N = 93, StdDev = 0.411 ms
Min = 6.485 ms, Q1 = 6.948 ms, Median = 7.204 ms, Q3 = 7.533 ms, Max = 8.271 ms
IQR = 0.585 ms, LowerFence = 6.070 ms, UpperFence = 8.411 ms
ConfidenceInterval = [7.136 ms; 7.426 ms] (CI 99.9%), Margin = 0.145 ms (1.99% of Mean)
Skewness = 0.49, Kurtosis = 2.4, MValue = 2.54
```
-------------------- Histogram --------------------
[6.422 ms ; 6.673 ms) | @@@
[6.673 ms ; 6.865 ms) | @@@@@@@@
[6.865 ms ; 7.104 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@
[7.104 ms ; 7.350 ms) | @@@@@@@@@@@@@@@@@@@@@@
[7.350 ms ; 7.655 ms) | @@@@@@@@@@@@@@@@
[7.655 ms ; 7.897 ms) | @@@@@
[7.897 ms ; 8.135 ms) | @@@@@@@@@@@@
[8.135 ms ; 8.390 ms) | @
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 195.782 ms, StdErr = 0.995 ms (0.51%), N = 22, StdDev = 4.666 ms
Min = 189.669 ms, Q1 = 192.082 ms, Median = 194.447 ms, Q3 = 199.017 ms, Max = 206.446 ms
IQR = 6.935 ms, LowerFence = 181.680 ms, UpperFence = 209.419 ms
ConfidenceInterval = [191.983 ms; 199.581 ms] (CI 99.9%), Margin = 3.799 ms (1.94% of Mean)
Skewness = 0.58, Kurtosis = 2.22, MValue = 2
```
-------------------- Histogram --------------------
[189.456 ms ; 193.827 ms) | @@@@@@@@@@
[193.827 ms ; 200.695 ms) | @@@@@@@@@
[200.695 ms ; 206.499 ms) | @@@
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 124.061 ms, StdErr = 2.746 ms (2.21%), N = 100, StdDev = 27.463 ms
Min = 76.796 ms, Q1 = 99.442 ms, Median = 118.442 ms, Q3 = 146.410 ms, Max = 180.275 ms
IQR = 46.968 ms, LowerFence = 28.990 ms, UpperFence = 216.863 ms
ConfidenceInterval = [114.747 ms; 133.376 ms] (CI 99.9%), Margin = 9.314 ms (7.51% of Mean)
Skewness = 0.31, Kurtosis = 1.88, MValue = 2.83
```
-------------------- Histogram --------------------
[ 69.031 ms ;  78.380 ms) | @
[ 78.380 ms ;  93.767 ms) | @@@@@@@@@@
[ 93.767 ms ; 109.298 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@
[109.298 ms ; 126.281 ms) | @@@@@@@@@@@@@@@@@@@@@
[126.281 ms ; 138.697 ms) | @@@@@@@
[138.697 ms ; 154.228 ms) | @@@@@@@@@@@@@@@@
[154.228 ms ; 171.206 ms) | @@@@@@@@@@@@@@@@
[171.206 ms ; 188.040 ms) | @@@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 19.548 ms, StdErr = 0.101 ms (0.51%), N = 29, StdDev = 0.542 ms
Min = 18.865 ms, Q1 = 19.202 ms, Median = 19.461 ms, Q3 = 19.777 ms, Max = 21.554 ms
IQR = 0.575 ms, LowerFence = 18.340 ms, UpperFence = 20.640 ms
ConfidenceInterval = [19.179 ms; 19.918 ms] (CI 99.9%), Margin = 0.370 ms (1.89% of Mean)
Skewness = 1.66, Kurtosis = 7.02, MValue = 2
```
-------------------- Histogram --------------------
[18.633 ms ; 19.132 ms) | @@@@
[19.132 ms ; 19.594 ms) | @@@@@@@@@@@@@@@
[19.594 ms ; 20.186 ms) | @@@@@@@@
[20.186 ms ; 20.937 ms) | @
[20.937 ms ; 21.785 ms) | @
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 17.669 ms, StdErr = 0.099 ms (0.56%), N = 40, StdDev = 0.623 ms
Min = 15.958 ms, Q1 = 17.340 ms, Median = 17.602 ms, Q3 = 18.057 ms, Max = 19.044 ms
IQR = 0.716 ms, LowerFence = 16.266 ms, UpperFence = 19.132 ms
ConfidenceInterval = [17.318 ms; 18.019 ms] (CI 99.9%), Margin = 0.351 ms (1.98% of Mean)
Skewness = -0.07, Kurtosis = 3.26, MValue = 2
```
-------------------- Histogram --------------------
[15.719 ms ; 16.197 ms) | @
[16.197 ms ; 16.679 ms) |
[16.679 ms ; 17.260 ms) | @@@@@@
[17.260 ms ; 17.738 ms) | @@@@@@@@@@@@@@@@@@@
[17.738 ms ; 18.361 ms) | @@@@@@@@@@
[18.361 ms ; 19.060 ms) | @@@@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 233.629 ms, StdErr = 3.940 ms (1.69%), N = 97, StdDev = 38.809 ms
Min = 196.242 ms, Q1 = 200.885 ms, Median = 205.016 ms, Q3 = 269.738 ms, Max = 319.568 ms
IQR = 68.853 ms, LowerFence = 97.606 ms, UpperFence = 373.017 ms
ConfidenceInterval = [220.252 ms; 247.005 ms] (CI 99.9%), Margin = 13.377 ms (5.73% of Mean)
Skewness = 0.61, Kurtosis = 1.82, MValue = 2.8
```
-------------------- Histogram --------------------
[194.198 ms ; 216.370 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[216.370 ms ; 233.896 ms) |
[233.896 ms ; 255.195 ms) | @@@@@@
[255.195 ms ; 277.367 ms) | @@@@@@@@@@@@@@@@@@@@@@
[277.367 ms ; 304.854 ms) | @@@@@@@@@@
[304.854 ms ; 330.654 ms) | @@@@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=PostgreSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 284.598 ms, StdErr = 1.380 ms (0.49%), N = 16, StdDev = 5.521 ms
Min = 276.250 ms, Q1 = 280.463 ms, Median = 284.365 ms, Q3 = 287.949 ms, Max = 297.599 ms
IQR = 7.486 ms, LowerFence = 269.234 ms, UpperFence = 299.178 ms
ConfidenceInterval = [278.976 ms; 290.220 ms] (CI 99.9%), Margin = 5.622 ms (1.98% of Mean)
Skewness = 0.55, Kurtosis = 2.67, MValue = 2
```
-------------------- Histogram --------------------
[275.633 ms ; 281.385 ms) | @@@@@@
[281.385 ms ; 288.584 ms) | @@@@@@@@
[288.584 ms ; 294.462 ms) | @
[294.462 ms ; 300.475 ms) | @
---------------------------------------------------
```

#### Outliers
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed (22.12 ms, 65.75 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 3 outliers were removed (9.53 ms..52.79 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed (239.35 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 8 outliers were removed (21.86 ms..89.90 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed, 3 outliers were detected (15.96 ms, 19.91 ms, 63.75 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 3 outliers were removed (698.70 ms..891.15 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 4 outliers were removed (318.10 ms..348.67 ms)