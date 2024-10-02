# MsSql - PostgreSql benchmark
Benchmarking `PostgreSql` vs. `MS SQL 2022` performance with EF Core 8 and Stored procedures.
- Windows: both database servers installed on local machine with default settings.
- Linux: official PostgreSql docker
- Citus: official Citus docker

# Getting started
1. Pull docker images of PostgreSql and Citus, and run them on different ports
2. Manually create the tables and stored procedures on all database instances you intend to benchmark
    - You can find the scripts in the [Database](https://github.com/bdn-mca/postgres-benchmark/tree/main/Database) folder
3. Update the connection strings in the [PgBenchmarkDbContext](https://github.com/bdn-mca/postgres-benchmark/blob/main/Infrastructure/PgBenchmarkDbContext.cs)
4. Build and Run in `Release` configuration

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
| EfCoreBulk | 2000      | MsSql           | False        | 23.410 s |  2.2910 s |  3.3581 s | 24.616 s |
| EfCoreBulk | 2000      | MsSql           | True         | 14.133 s |  0.1933 s |  0.1714 s | 14.152 s |
| EfCoreBulk | 2000      | PostgreSql      | False        | 30.493 s | 12.1064 s | 18.1203 s | 24.354 s |
| EfCoreBulk | 2000      | PostgreSql      | True         |  5.557 s |  0.1809 s |  0.2414 s |  5.522 s |
| EfCoreBulk | 50000     | MsSql           | False        | 13.818 s |  1.7076 s |  2.5559 s | 14.669 s |
| EfCoreBulk | 50000     | MsSql           | True         |  8.291 s |  0.1210 s |  0.1011 s |  8.301 s |
| EfCoreBulk | 50000     | PostgreSql      | False        | 31.014 s | 12.3816 s | 18.5322 s | 24.330 s |
| EfCoreBulk | 50000     | PostgreSql      | True         |  5.208 s |  0.0931 s |  0.0871 s |  5.206 s |
| EfCoreBulk | 200000    | MsSql           | False        | 12.640 s |  1.8502 s |  2.7694 s | 12.461 s |
| EfCoreBulk | 200000    | MsSql           | True         |  7.755 s |  0.1044 s |  0.0926 s |  7.727 s |
| EfCoreBulk | 200000    | PostgreSql      | False        | 30.666 s | 12.4444 s | 18.6263 s | 24.037 s |
| EfCoreBulk | 200000    | PostgreSql      | True         |  5.210 s |  0.0656 s |  0.0614 s |  5.203 s |

#### [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 23.410 s, StdErr = 0.624 s (2.66%), N = 29, StdDev = 3.358 s
Min = 15.324 s, Q1 = 22.378 s, Median = 24.616 s, Q3 = 25.652 s, Max = 27.357 s
IQR = 3.274 s, LowerFence = 17.468 s, UpperFence = 30.562 s
ConfidenceInterval = [21.119 s; 25.701 s] (CI 99.9%), Margin = 2.291 s (9.79% of Mean)
Skewness = -1.17, Kurtosis = 3.21, MValue = 2.31
-------------------- Histogram --------------------
[14.515 s ; 18.275 s) | @@@
[18.275 s ; 21.144 s) | @@@@
[21.144 s ; 23.570 s) | @
[23.570 s ; 26.439 s) | @@@@@@@@@@@@@@@@@@
[26.439 s ; 28.792 s) | @@@
---------------------------------------------------
```

#### [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 14.133 s, StdErr = 0.046 s (0.32%), N = 14, StdDev = 0.171 s
Min = 13.870 s, Q1 = 14.022 s, Median = 14.152 s, Q3 = 14.229 s, Max = 14.513 s
IQR = 0.207 s, LowerFence = 13.711 s, UpperFence = 14.539 s
ConfidenceInterval = [13.940 s; 14.327 s] (CI 99.9%), Margin = 0.193 s (1.37% of Mean)
Skewness = 0.4, Kurtosis = 2.5, MValue = 2
-------------------- Histogram --------------------
[13.821 s ; 14.607 s) | @@@@@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=2000, BenchmarkDbType=PostgreSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 30.493 s, StdErr = 3.308 s (10.85%), N = 30, StdDev = 18.120 s
Min = 5.444 s, Q1 = 15.880 s, Median = 24.354 s, Q3 = 49.638 s, Max = 58.814 s
IQR = 33.758 s, LowerFence = -34.757 s, UpperFence = 100.275 s
ConfidenceInterval = [18.387 s; 42.599 s] (CI 99.9%), Margin = 12.106 s (39.70% of Mean)
Skewness = 0.27, Kurtosis = 1.46, MValue = 3.29
-------------------- Histogram --------------------
[ 4.659 s ; 19.967 s) | @@@@@@@@@@@@@@
[19.967 s ; 25.820 s) | @
[25.820 s ; 43.894 s) | @@@@@
[43.894 s ; 59.202 s) | @@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=2000, BenchmarkDbType=PostgreSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.557 s, StdErr = 0.048 s (0.87%), N = 25, StdDev = 0.241 s
Min = 5.202 s, Q1 = 5.368 s, Median = 5.522 s, Q3 = 5.735 s, Max = 6.062 s
IQR = 0.367 s, LowerFence = 4.816 s, UpperFence = 6.286 s
ConfidenceInterval = [5.376 s; 5.738 s] (CI 99.9%), Margin = 0.181 s (3.25% of Mean)
Skewness = 0.48, Kurtosis = 2.23, MValue = 2
-------------------- Histogram --------------------
[5.093 s ; 5.258 s) | @
[5.258 s ; 5.475 s) | @@@@@@@@@@
[5.475 s ; 5.767 s) | @@@@@@@@@@
[5.767 s ; 6.095 s) | @@@@
---------------------------------------------------
```

#### [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.818 s, StdErr = 0.467 s (3.38%), N = 30, StdDev = 2.556 s
Min = 9.021 s, Q1 = 11.811 s, Median = 14.669 s, Q3 = 16.082 s, Max = 16.959 s
IQR = 4.271 s, LowerFence = 5.404 s, UpperFence = 22.489 s
ConfidenceInterval = [12.110 s; 15.525 s] (CI 99.9%), Margin = 1.708 s (12.36% of Mean)
Skewness = -0.45, Kurtosis = 1.73, MValue = 2.93
-------------------- Histogram --------------------
[ 8.677 s ; 10.841 s) | @@@@
[10.841 s ; 13.000 s) | @@@@@@@@@
[13.000 s ; 14.940 s) | @@
[14.940 s ; 17.100 s) | @@@@@@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.291 s, StdErr = 0.028 s (0.34%), N = 13, StdDev = 0.101 s
Min = 8.146 s, Q1 = 8.216 s, Median = 8.301 s, Q3 = 8.354 s, Max = 8.506 s
IQR = 0.139 s, LowerFence = 8.007 s, UpperFence = 8.563 s
ConfidenceInterval = [8.170 s; 8.412 s] (CI 99.9%), Margin = 0.121 s (1.46% of Mean)
Skewness = 0.33, Kurtosis = 2.27, MValue = 2
-------------------- Histogram --------------------
[8.089 s ; 8.562 s) | @@@@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=50000, BenchmarkDbType=PostgreSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 31.014 s, StdErr = 3.384 s (10.91%), N = 30, StdDev = 18.532 s
Min = 5.401 s, Q1 = 16.639 s, Median = 24.330 s, Q3 = 49.556 s, Max = 60.223 s
IQR = 32.917 s, LowerFence = -32.737 s, UpperFence = 98.932 s
ConfidenceInterval = [18.633 s; 43.396 s] (CI 99.9%), Margin = 12.382 s (39.92% of Mean)
Skewness = 0.29, Kurtosis = 1.47, MValue = 3.29
-------------------- Histogram --------------------
[ 4.952 s ; 22.205 s) | @@@@@@@@@@@@@@
[22.205 s ; 37.861 s) | @@@@@
[37.861 s ; 44.871 s) | @
[44.871 s ; 60.527 s) | @@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=50000, BenchmarkDbType=PostgreSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.208 s, StdErr = 0.022 s (0.43%), N = 15, StdDev = 0.087 s
Min = 5.035 s, Q1 = 5.163 s, Median = 5.206 s, Q3 = 5.264 s, Max = 5.340 s
IQR = 0.101 s, LowerFence = 5.012 s, UpperFence = 5.415 s
ConfidenceInterval = [5.114 s; 5.301 s] (CI 99.9%), Margin = 0.093 s (1.79% of Mean)
Skewness = -0.36, Kurtosis = 2.19, MValue = 2
-------------------- Histogram --------------------
[4.999 s ; 5.129 s) | @@
[5.129 s ; 5.349 s) | @@@@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=200000, BenchmarkDbType=MsSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.640 s, StdErr = 0.506 s (4.00%), N = 30, StdDev = 2.769 s
Min = 8.448 s, Q1 = 9.952 s, Median = 12.461 s, Q3 = 15.282 s, Max = 16.608 s
IQR = 5.330 s, LowerFence = 1.957 s, UpperFence = 23.277 s
ConfidenceInterval = [10.790 s; 14.490 s] (CI 99.9%), Margin = 1.850 s (14.64% of Mean)
Skewness = 0.06, Kurtosis = 1.48, MValue = 3.82
-------------------- Histogram --------------------
[ 7.278 s ;  8.625 s) | @
[ 8.625 s ; 11.280 s) | @@@@@@@@@@
[11.280 s ; 13.620 s) | @@@@@@@@
[13.620 s ; 14.434 s) |
[14.434 s ; 16.773 s) | @@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=200000, BenchmarkDbType=MsSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.755 s, StdErr = 0.025 s (0.32%), N = 14, StdDev = 0.093 s
Min = 7.635 s, Q1 = 7.678 s, Median = 7.727 s, Q3 = 7.816 s, Max = 7.913 s
IQR = 0.138 s, LowerFence = 7.471 s, UpperFence = 8.023 s
ConfidenceInterval = [7.650 s; 7.859 s] (CI 99.9%), Margin = 0.104 s (1.35% of Mean)
Skewness = 0.52, Kurtosis = 1.69, MValue = 2
-------------------- Histogram --------------------
[7.606 s ; 7.944 s) | @@@@@@@@@@@@@@
---------------------------------------------------
```

#### [BatchSize=200000, BenchmarkDbType=PostgreSql, CleanupTable=False]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 30.666 s, StdErr = 3.401 s (11.09%), N = 30, StdDev = 18.626 s
Min = 5.343 s, Q1 = 15.582 s, Median = 24.037 s, Q3 = 49.209 s, Max = 60.687 s
IQR = 33.627 s, LowerFence = -34.858 s, UpperFence = 99.650 s
ConfidenceInterval = [18.222 s; 43.110 s] (CI 99.9%), Margin = 12.444 s (40.58% of Mean)
Skewness = 0.31, Kurtosis = 1.49, MValue = 2.53
-------------------- Histogram --------------------
[ 4.752 s ; 25.486 s) | @@@@@@@@@@@@@@@
[25.486 s ; 44.310 s) | @@@@@
[44.310 s ; 60.045 s) | @@@@@@@@@
[60.045 s ; 68.555 s) | @
---------------------------------------------------
```

#### [BatchSize=200000, BenchmarkDbType=PostgreSql, CleanupTable=True]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.210 s, StdErr = 0.016 s (0.30%), N = 15, StdDev = 0.061 s
Min = 5.073 s, Q1 = 5.175 s, Median = 5.203 s, Q3 = 5.249 s, Max = 5.314 s
IQR = 0.074 s, LowerFence = 5.063 s, UpperFence = 5.361 s
ConfidenceInterval = [5.145 s; 5.276 s] (CI 99.9%), Margin = 0.066 s (1.26% of Mean)
Skewness = -0.16, Kurtosis = 2.7, MValue = 2
-------------------- Histogram --------------------
[5.041 s ; 5.149 s) | @
[5.149 s ; 5.347 s) | @@@@@@@@@@@@@@
---------------------------------------------------
```

#### Outliers
- BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed, 4 outliers were detected (15.32 s..16.58 s, 31.36 s)
- BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed (14.96 s)
- BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 5 outliers were removed (7.05 s..8.64 s)
- BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed (8.71 s, 8.74 s)
- BulkBenchmarkEfService.EfCoreBulk: InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed (8.10 s)

## Small inserts on already large table
Table consisting of millions of rows receives non-bulk small inserts of 100s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start

| Method         | InitialTableSize | BenchmarkDbType | Mean     | Error     | StdDev    |
|--------------- |----------------- |---------------- |---------:|----------:|----------:|
| EfCoreRangeAdd | 3000001          | MsSql           | 6.784 ms | 0.1094 ms | 0.0970 ms |
| EfCoreRangeAdd | 3000001          | PostgreSql      | 5.693 ms | 0.0764 ms | 0.0714 ms |
| EfCoreRangeAdd | 10000001         | MsSql           | 6.915 ms | 0.1250 ms | 0.1169 ms |
| EfCoreRangeAdd | 10000001         | PostgreSql      | 7.283 ms | 0.0696 ms | 0.0581 ms |

#### [InitialTableSize=3.000.001, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.784 ms, StdErr = 0.026 ms (0.38%), N = 14, StdDev = 0.097 ms
Min = 6.548 ms, Q1 = 6.751 ms, Median = 6.801 ms, Q3 = 6.835 ms, Max = 6.964 ms
IQR = 0.084 ms, LowerFence = 6.624 ms, UpperFence = 6.962 ms
ConfidenceInterval = [6.674 ms; 6.893 ms] (CI 99.9%), Margin = 0.109 ms (1.61% of Mean)
Skewness = -0.62, Kurtosis = 3.52, MValue = 2
-------------------- Histogram --------------------
[6.495 ms ; 6.727 ms) | @@@
[6.727 ms ; 7.017 ms) | @@@@@@@@@@@
---------------------------------------------------
```

#### [InitialTableSize=3.000.001, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.693 ms, StdErr = 0.018 ms (0.32%), N = 15, StdDev = 0.071 ms
Min = 5.593 ms, Q1 = 5.639 ms, Median = 5.693 ms, Q3 = 5.736 ms, Max = 5.838 ms
IQR = 0.097 ms, LowerFence = 5.493 ms, UpperFence = 5.882 ms
ConfidenceInterval = [5.617 ms; 5.769 ms] (CI 99.9%), Margin = 0.076 ms (1.34% of Mean)
Skewness = 0.26, Kurtosis = 2.09, MValue = 2
-------------------- Histogram --------------------
[5.555 ms ; 5.756 ms) | @@@@@@@@@@@@@
[5.756 ms ; 5.876 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=10.000.001, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.915 ms, StdErr = 0.030 ms (0.44%), N = 15, StdDev = 0.117 ms
Min = 6.788 ms, Q1 = 6.814 ms, Median = 6.887 ms, Q3 = 6.985 ms, Max = 7.150 ms
IQR = 0.171 ms, LowerFence = 6.557 ms, UpperFence = 7.241 ms
ConfidenceInterval = [6.790 ms; 7.040 ms] (CI 99.9%), Margin = 0.125 ms (1.81% of Mean)
Skewness = 0.67, Kurtosis = 1.96, MValue = 2
-------------------- Histogram --------------------
[6.775 ms ; 6.980 ms) | @@@@@@@@@@@
[6.980 ms ; 7.212 ms) | @@@@
---------------------------------------------------
```

#### [InitialTableSize=10.000.001, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.283 ms, StdErr = 0.016 ms (0.22%), N = 13, StdDev = 0.058 ms
Min = 7.190 ms, Q1 = 7.251 ms, Median = 7.281 ms, Q3 = 7.306 ms, Max = 7.411 ms
IQR = 0.055 ms, LowerFence = 7.168 ms, UpperFence = 7.389 ms
ConfidenceInterval = [7.213 ms; 7.352 ms] (CI 99.9%), Margin = 0.070 ms (0.96% of Mean)
Skewness = 0.42, Kurtosis = 2.77, MValue = 2
-------------------- Histogram --------------------
[7.157 ms ; 7.443 ms) | @@@@@@@@@@@@@
---------------------------------------------------
```

#### Outliers
- SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: WarmupCount=0 -> 1 outlier  was  removed, 2 outliers were detected (6.55 ms, 7.03 ms)
- SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: WarmupCount=0 -> 2 outliers were removed (7.49 ms, 8.34 ms)

## Bulk inserts on already large table
Table consisting of millions of rows receives bulk inserts of 1000s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start
- `BulkAddSize` - number of rows added in each iteration in bulk

| Method        | InitialTableSize | BulkAddSize | BenchmarkDbType | Mean       | Error      | StdDev     | Median     |
|-------------- |----------------- |------------ |---------------- |-----------:|-----------:|-----------:|-----------:|
| EfCoreBulkAdd | 3000001          | 1000        | MsSql           |  19.438 ms |  0.3853 ms |  0.5144 ms |  19.435 ms |
| EfCoreBulkAdd | 3000001          | 1000        | PostgreSql      |   8.046 ms |  0.1597 ms |  0.4208 ms |   8.000 ms |
| EfCoreBulkAdd | 3000001          | 20000       | MsSql           | 191.824 ms |  3.7038 ms |  3.8035 ms | 191.060 ms |
| EfCoreBulkAdd | 3000001          | 20000       | PostgreSql      | 137.397 ms | 10.0203 ms | 29.2297 ms | 134.005 ms |
| EfCoreBulkAdd | 10000001         | 1000        | MsSql           |  22.330 ms |  0.4331 ms |  0.4814 ms |  22.265 ms |
| EfCoreBulkAdd | 10000001         | 1000        | PostgreSql      |  18.882 ms |  0.3776 ms |  1.0080 ms |  18.746 ms |
| EfCoreBulkAdd | 10000001         | 20000       | MsSql           | 257.900 ms | 28.1699 ms | 81.2766 ms | 202.850 ms |
| EfCoreBulkAdd | 10000001         | 20000       | PostgreSql      | 294.051 ms |  4.6315 ms |  4.1057 ms | 294.559 ms |

#### [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 19.438 ms, StdErr = 0.103 ms (0.53%), N = 25, StdDev = 0.514 ms
Min = 18.484 ms, Q1 = 19.256 ms, Median = 19.435 ms, Q3 = 19.923 ms, Max = 20.373 ms
IQR = 0.667 ms, LowerFence = 18.255 ms, UpperFence = 20.924 ms
ConfidenceInterval = [19.052 ms; 19.823 ms] (CI 99.9%), Margin = 0.385 ms (1.98% of Mean)
Skewness = -0.24, Kurtosis = 2.26, MValue = 2
-------------------- Histogram --------------------
[18.430 ms ; 18.892 ms) | @@@@@
[18.892 ms ; 19.644 ms) | @@@@@@@@@@@@@
[19.644 ms ; 20.379 ms) | @@@@@@@
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.046 ms, StdErr = 0.047 ms (0.58%), N = 81, StdDev = 0.421 ms
Min = 7.220 ms, Q1 = 7.780 ms, Median = 8.000 ms, Q3 = 8.309 ms, Max = 9.136 ms
IQR = 0.529 ms, LowerFence = 6.987 ms, UpperFence = 9.102 ms
ConfidenceInterval = [7.886 ms; 8.205 ms] (CI 99.9%), Margin = 0.160 ms (1.99% of Mean)
Skewness = 0.55, Kurtosis = 2.82, MValue = 2
-------------------- Histogram --------------------
[7.220 ms ; 7.485 ms) | @@@@@
[7.485 ms ; 7.773 ms) | @@@@@@@@@@@@@
[7.773 ms ; 8.028 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@
[8.028 ms ; 8.297 ms) | @@@@@@@@@@@@@@
[8.297 ms ; 8.595 ms) | @@@@@@@@@@@
[8.595 ms ; 8.850 ms) | @@@@@@@
[8.850 ms ; 9.188 ms) | @@@
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 191.824 ms, StdErr = 0.922 ms (0.48%), N = 17, StdDev = 3.804 ms
Min = 187.277 ms, Q1 = 188.806 ms, Median = 191.060 ms, Q3 = 192.569 ms, Max = 200.824 ms
IQR = 3.762 ms, LowerFence = 183.162 ms, UpperFence = 198.212 ms
ConfidenceInterval = [188.120 ms; 195.527 ms] (CI 99.9%), Margin = 3.704 ms (1.93% of Mean)
Skewness = 1.1, Kurtosis = 3.29, MValue = 2
-------------------- Histogram --------------------
[185.336 ms ; 192.139 ms) | @@@@@@@@@@@@
[192.139 ms ; 198.312 ms) | @@@
[198.312 ms ; 202.766 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 137.397 ms, StdErr = 2.953 ms (2.15%), N = 98, StdDev = 29.230 ms
Min = 87.454 ms, Q1 = 112.771 ms, Median = 134.005 ms, Q3 = 162.857 ms, Max = 200.093 ms
IQR = 50.087 ms, LowerFence = 37.640 ms, UpperFence = 237.988 ms
ConfidenceInterval = [127.376 ms; 147.417 ms] (CI 99.9%), Margin = 10.020 ms (7.29% of Mean)
Skewness = 0.19, Kurtosis = 1.96, MValue = 3.22
-------------------- Histogram --------------------
[ 79.133 ms ;  90.840 ms) | @@@
[ 90.840 ms ; 108.089 ms) | @@@@@@@@@@@@@
[108.089 ms ; 124.732 ms) | @@@@@@@@@@@@@@@@@@@@@@@
[124.732 ms ; 133.045 ms) | @@@@@@@
[133.045 ms ; 152.627 ms) | @@@@@@@@@@@@@@@@@@@@@
[152.627 ms ; 169.269 ms) | @@@@@@@@@@@@@@@
[169.269 ms ; 187.095 ms) | @@@@@@@@@@@@@@
[187.095 ms ; 204.732 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 22.330 ms, StdErr = 0.110 ms (0.49%), N = 19, StdDev = 0.481 ms
Min = 21.305 ms, Q1 = 22.073 ms, Median = 22.265 ms, Q3 = 22.598 ms, Max = 23.403 ms
IQR = 0.525 ms, LowerFence = 21.286 ms, UpperFence = 23.385 ms
ConfidenceInterval = [21.896 ms; 22.763 ms] (CI 99.9%), Margin = 0.433 ms (1.94% of Mean)
Skewness = 0.3, Kurtosis = 3.16, MValue = 2
-------------------- Histogram --------------------
[21.069 ms ; 21.542 ms) | @
[21.542 ms ; 22.171 ms) | @@@@@
[22.171 ms ; 23.073 ms) | @@@@@@@@@@@
[23.073 ms ; 23.640 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 18.882 ms, StdErr = 0.111 ms (0.59%), N = 83, StdDev = 1.008 ms
Min = 17.171 ms, Q1 = 18.112 ms, Median = 18.746 ms, Q3 = 19.575 ms, Max = 21.233 ms
IQR = 1.463 ms, LowerFence = 15.917 ms, UpperFence = 21.770 ms
ConfidenceInterval = [18.505 ms; 19.260 ms] (CI 99.9%), Margin = 0.378 ms (2.00% of Mean)
Skewness = 0.51, Kurtosis = 2.29, MValue = 2.4
-------------------- Histogram --------------------
[17.089 ms ; 17.725 ms) | @@@@@@
[17.725 ms ; 18.331 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@
[18.331 ms ; 19.125 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@
[19.125 ms ; 19.700 ms) | @@@@@@@
[19.700 ms ; 20.306 ms) | @@@@@@@@@@@@
[20.306 ms ; 20.989 ms) | @@@@@@@
[20.989 ms ; 21.536 ms) | @
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 257.900 ms, StdErr = 8.295 ms (3.22%), N = 96, StdDev = 81.277 ms
Min = 194.455 ms, Q1 = 199.368 ms, Median = 202.850 ms, Q3 = 311.124 ms, Max = 480.400 ms
IQR = 111.756 ms, LowerFence = 31.734 ms, UpperFence = 478.758 ms
ConfidenceInterval = [229.730 ms; 286.070 ms] (CI 99.9%), Margin = 28.170 ms (10.92% of Mean)
Skewness = 1.07, Kurtosis = 2.78, MValue = 2.58
-------------------- Histogram --------------------
[183.381 ms ; 229.976 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[229.976 ms ; 272.481 ms) | @
[272.481 ms ; 319.076 ms) | @@@@@@@@@@@@@@
[319.076 ms ; 359.927 ms) | @@@@@@@@@
[359.927 ms ; 392.259 ms) | @@@
[392.259 ms ; 438.854 ms) | @@@@@@@@
[438.854 ms ; 490.234 ms) | @@
---------------------------------------------------
```

#### [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=PostgreSql]
```
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 294.051 ms, StdErr = 1.097 ms (0.37%), N = 14, StdDev = 4.106 ms
Min = 286.450 ms, Q1 = 291.013 ms, Median = 294.559 ms, Q3 = 296.411 ms, Max = 299.806 ms
IQR = 5.397 ms, LowerFence = 282.917 ms, UpperFence = 304.507 ms
ConfidenceInterval = [289.419 ms; 298.682 ms] (CI 99.9%), Margin = 4.631 ms (1.58% of Mean)
Skewness = -0.18, Kurtosis = 1.79, MValue = 2
-------------------- Histogram --------------------
[284.214 ms ; 292.302 ms) | @@@@@
[292.302 ms ; 302.042 ms) | @@@@@@@@@
---------------------------------------------------
```

#### Outliers
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 3 outliers were removed (20.99 ms..68.69 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 4 outliers were removed (9.39 ms..53.90 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed (202.60 ms, 237.96 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 2 outliers were removed (343.03 ms, 436.07 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 3 outliers were removed, 4 outliers were detected (21.31 ms, 25.42 ms..67.09 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 4 outliers were removed (23.51 ms..235.03 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 4 outliers were removed (785.97 ms..981.66 ms)
- BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: InvocationCount=1, UnrollFactor=1, WarmupCount=0 -> 1 outlier  was  removed (342.87 ms)

# PostgreSql Linux vs. PostgreSql Citus
Trying the same benchmarks on `PostgreSql 17` (from the official docker) vs. `PostgreSql Citus` (from the official docker).

## Small stored procedures
Stored procedures inserting 100 items.

| Method          | BenchmarkDbType | Mean     | Error     | StdDev    |
|---------------- |---------------- |---------:|----------:|----------:|
| StoredProcedure | PostgreSqlLinux | 3.144 ms | 0.0629 ms | 0.1067 ms |
| StoredProcedure | PostgreSqlCitus | 3.109 ms | 0.0621 ms | 0.0870 ms |

## Small EF Core
Inserting 100 items (non-bulk) with EF Core.

| Method          | BenchmarkDbType | Mean     | Error     | StdDev    |
|---------------- |---------------- |---------:|----------:|----------:|
| EfCoreSingleAdd | PostgreSqlLinux | 8.397 ms | 0.1645 ms | 0.1760 ms |
| EfCoreRangeAdd  | PostgreSqlLinux | 8.602 ms | 0.1691 ms | 0.2682 ms |
| EfCoreSingleAdd | PostgreSqlCitus | 9.257 ms | 0.1776 ms | 0.2181 ms |
| EfCoreRangeAdd  | PostgreSqlCitus | 9.061 ms | 0.1760 ms | 0.2410 ms |

## Bulk stored procedure
Inserting 1.000.000 items with stored procedure, using `generate_series`.
Do not `CleanupTable` - meaning does not delete and vacuum between operations.

| Method          | BenchmarkDbType | CleanupTable | Mean    | Error   | StdDev  |
|---------------- |---------------- |------------- |--------:|--------:|--------:|
| StoredProcedure | PostgreSqlLinux | False        | 18.43 s | 5.835 s | 8.733 s |
| StoredProcedure | PostgreSqlCitus | False        | 17.91 s | 5.473 s | 8.192 s |

```
[BenchmarkDbType=PostgreSqlLinux, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 18.433 s, StdErr = 1.594 s (8.65%), N = 30, StdDev = 8.733 s
Min = 5.577 s, Q1 = 11.752 s, Median = 16.222 s, Q3 = 25.942 s, Max = 32.824 s
IQR = 14.189 s, LowerFence = -9.532 s, UpperFence = 47.225 s
ConfidenceInterval = [12.598 s; 24.267 s] (CI 99.9%), Margin = 5.835 s (31.65% of Mean)
Skewness = 0.13, Kurtosis = 1.52, MValue = 3.5
-------------------- Histogram --------------------
[ 1.888 s ;  8.931 s) | @@@@
[ 8.931 s ; 16.309 s) | @@@@@@@@@@@@
[16.309 s ; 23.716 s) | @@
[23.716 s ; 31.093 s) | @@@@@@@@@@@
[31.093 s ; 36.513 s) | @
---------------------------------------------------

[BenchmarkDbType=PostgreSqlCitus, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 17.914 s, StdErr = 1.496 s (8.35%), N = 30, StdDev = 8.192 s
Min = 5.953 s, Q1 = 11.215 s, Median = 17.036 s, Q3 = 25.703 s, Max = 31.240 s
IQR = 14.487 s, LowerFence = -10.516 s, UpperFence = 47.434 s
ConfidenceInterval = [12.441 s; 23.388 s] (CI 99.9%), Margin = 5.473 s (30.55% of Mean)
Skewness = 0.07, Kurtosis = 1.45, MValue = 3.83
-------------------- Histogram --------------------
[ 5.899 s ; 12.820 s) | @@@@@@@@@@@@
[12.820 s ; 20.154 s) | @@@@@@
[20.154 s ; 22.833 s) |
[22.833 s ; 29.754 s) | @@@@@@@@@@@
[29.754 s ; 34.701 s) | @
---------------------------------------------------
```

## Bulk EF Core
Inserting 1.000.000 items with EF Core by using EF.BulkExtensions.

| Method     | BenchmarkDbType | CleanupTable | BatchSize | Mean    | Error   | StdDev  | Median   |
|----------- |---------------- |------------- |---------- |--------:|--------:|--------:|---------:|
| EfCoreBulk | PostgreSqlLinux | False        | 2000      | 13.22 s | 4.870 s | 7.289 s |  9.950 s |
| EfCoreBulk | PostgreSqlLinux | False        | 50000     | 13.15 s | 4.736 s | 7.088 s |  9.647 s |
| EfCoreBulk | PostgreSqlLinux | False        | 200000    | 13.02 s | 4.537 s | 6.791 s | 10.009 s |
| EfCoreBulk | PostgreSqlCitus | False        | 2000      | 13.04 s | 4.628 s | 6.927 s |  9.332 s |
| EfCoreBulk | PostgreSqlCitus | False        | 50000     | 13.14 s | 4.781 s | 7.155 s |  9.676 s |
| EfCoreBulk | PostgreSqlCitus | False        | 200000    | 13.24 s | 4.747 s | 7.105 s |  9.592 s |

```
[BenchmarkDbType=PostgreSqlLinux, CleanupTable=False, BatchSize=2000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.224 s, StdErr = 1.331 s (10.06%), N = 30, StdDev = 7.289 s
Min = 3.967 s, Q1 = 7.923 s, Median = 9.950 s, Q3 = 19.141 s, Max = 26.945 s
IQR = 11.218 s, LowerFence = -8.904 s, UpperFence = 35.968 s
ConfidenceInterval = [8.353 s; 18.094 s] (CI 99.9%), Margin = 4.870 s (36.83% of Mean)
Skewness = 0.54, Kurtosis = 1.79, MValue = 2.8
-------------------- Histogram --------------------
[ 0.888 s ;  4.785 s) | @@
[ 4.785 s ; 10.943 s) | @@@@@@@@@@@@@@@
[10.943 s ; 15.011 s) | @
[15.011 s ; 22.046 s) | @@@@@@@
[22.046 s ; 28.204 s) | @@@@@
---------------------------------------------------

[BenchmarkDbType=PostgreSqlLinux, CleanupTable=False, BatchSize=50000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.155 s, StdErr = 1.294 s (9.84%), N = 30, StdDev = 7.088 s
Min = 3.874 s, Q1 = 7.765 s, Median = 9.647 s, Q3 = 19.947 s, Max = 27.136 s
IQR = 12.182 s, LowerFence = -10.507 s, UpperFence = 38.220 s
ConfidenceInterval = [8.419 s; 17.890 s] (CI 99.9%), Margin = 4.736 s (36.00% of Mean)
Skewness = 0.45, Kurtosis = 1.68, MValue = 2.4
-------------------- Histogram --------------------
[ 3.569 s ;  9.556 s) | @@@@@@@@@@@@@@@
[ 9.556 s ; 15.546 s) | @@@@@
[15.546 s ; 23.416 s) | @@@@@@@@
[23.416 s ; 30.130 s) | @@
---------------------------------------------------

[BenchmarkDbType=PostgreSqlLinux, CleanupTable=False, BatchSize=200000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.023 s, StdErr = 1.240 s (9.52%), N = 30, StdDev = 6.791 s
Min = 3.921 s, Q1 = 7.769 s, Median = 10.009 s, Q3 = 19.509 s, Max = 24.837 s
IQR = 11.740 s, LowerFence = -9.841 s, UpperFence = 37.120 s
ConfidenceInterval = [8.486 s; 17.560 s] (CI 99.9%), Margin = 4.537 s (34.84% of Mean)
Skewness = 0.39, Kurtosis = 1.57, MValue = 2.8
-------------------- Histogram --------------------
[ 3.749 s ;  9.486 s) | @@@@@@@@@@@@@@@
[ 9.486 s ; 13.712 s) | @@
[13.712 s ; 19.483 s) | @@@@@
[19.483 s ; 25.219 s) | @@@@@@@@
---------------------------------------------------

[BenchmarkDbType=PostgreSqlCitus, CleanupTable=False, BatchSize=2000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.036 s, StdErr = 1.265 s (9.70%), N = 30, StdDev = 6.927 s
Min = 4.118 s, Q1 = 7.816 s, Median = 9.332 s, Q3 = 19.520 s, Max = 25.132 s
IQR = 11.705 s, LowerFence = -9.741 s, UpperFence = 37.077 s
ConfidenceInterval = [8.408 s; 17.664 s] (CI 99.9%), Margin = 4.628 s (35.50% of Mean)
Skewness = 0.46, Kurtosis = 1.57, MValue = 2.88
-------------------- Histogram --------------------
[ 3.867 s ; 10.644 s) | @@@@@@@@@@@@@@@@
[10.644 s ; 16.496 s) | @@@@
[16.496 s ; 18.628 s) | @
[18.628 s ; 24.480 s) | @@@@@@@@
[24.480 s ; 28.058 s) | @
---------------------------------------------------

[BenchmarkDbType=PostgreSqlCitus, CleanupTable=False, BatchSize=50000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.137 s, StdErr = 1.306 s (9.94%), N = 30, StdDev = 7.155 s
Min = 4.208 s, Q1 = 7.935 s, Median = 9.676 s, Q3 = 19.533 s, Max = 25.896 s
IQR = 11.598 s, LowerFence = -9.463 s, UpperFence = 36.931 s
ConfidenceInterval = [8.357 s; 17.918 s] (CI 99.9%), Margin = 4.781 s (36.39% of Mean)
Skewness = 0.52, Kurtosis = 1.68, MValue = 2.88
-------------------- Histogram --------------------
[ 4.095 s ; 10.140 s) | @@@@@@@@@@@@@@@@
[10.140 s ; 12.448 s) | @
[12.448 s ; 18.493 s) | @@@@@
[18.493 s ; 25.971 s) | @@@@@@@@
---------------------------------------------------

[BenchmarkDbType=PostgreSqlCitus, CleanupTable=False, BatchSize=200000]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.244 s, StdErr = 1.297 s (9.79%), N = 30, StdDev = 7.105 s
Min = 3.887 s, Q1 = 7.874 s, Median = 9.592 s, Q3 = 19.778 s, Max = 26.724 s
IQR = 11.904 s, LowerFence = -9.983 s, UpperFence = 37.635 s
ConfidenceInterval = [8.497 s; 17.991 s] (CI 99.9%), Margin = 4.747 s (35.84% of Mean)
Skewness = 0.47, Kurtosis = 1.65, MValue = 2.88
-------------------- Histogram --------------------
[ 3.790 s ;  9.792 s) | @@@@@@@@@@@@@@@@
[ 9.792 s ; 12.912 s) | @
[12.912 s ; 18.923 s) | @@@@
[18.923 s ; 24.925 s) | @@@@@@@@
[24.925 s ; 29.725 s) | @
---------------------------------------------------
```