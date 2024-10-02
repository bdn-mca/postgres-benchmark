# MsSql - PostgreSql benchmark
Benchmarking `PostgreSql` vs. `MS SQL` performance with EF Core and Stored procedures.

## Machine information
```
Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
Intel Core i5-10400 CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
```

## Benchmarked databases
Database servers installed with default settings.

- MsSql 2022 (16.0.1125.1) Developer edition
  - Installed locally on the machine
- PostgreSql 16.4, compiled by Visual C++ build 1940, 64-bit
  - Installed locally on the machine
- PostgreSql 17.0 (Debian 17.0-1.pgdg110+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 10.2.1-6) 10.2.1 20210110, 64-bit
  - Docker (1d8819794c610d6812ac46fe93b04e8ce0c4ae8dd8b8797739c8fd44b28bf243)
- PostgreSql Citus 12.1.5 on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
  - Docker (9aa2f9a36235002f60bd7e1a53e34a0849656b75a04e8ed0c80191e0eb5ed12d)

## Package versions
```
.NET SDK 8.0.401
BenchmarkDotNet v0.14.0
```

# Getting started
1. Set up the database servers you want to benchmark
    - Either install the versions you want to benchmark locally, or pull docker images and run them on different ports
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

| Method          | BenchmarkDbType   | Mean     | Error     | StdDev    |
|---------------- |------------------ |---------:|----------:|----------:|
| EfCoreSingleAdd | MsSql             | 7.150 ms | 0.1422 ms | 0.3352 ms |
| EfCoreRangeAdd  | MsSql             | 6.871 ms | 0.1299 ms | 0.1390 ms |
| EfCoreSingleAdd | PostgreSqlWindows | 5.795 ms | 0.1142 ms | 0.1270 ms |
| EfCoreRangeAdd  | PostgreSqlWindows | 5.712 ms | 0.1118 ms | 0.0991 ms |
| EfCoreSingleAdd | PostgreSqlLinux   | 8.355 ms | 0.1612 ms | 0.2412 ms |
| EfCoreRangeAdd  | PostgreSqlLinux   | 8.288 ms | 0.1298 ms | 0.1275 ms |
| EfCoreSingleAdd | PostgreSqlCitus   | 8.781 ms | 0.1527 ms | 0.1429 ms |
| EfCoreRangeAdd  | PostgreSqlCitus   | 8.868 ms | 0.1721 ms | 0.2113 ms |

```
SmallBenchmarkEfService.EfCoreSingleAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.150 ms, StdErr = 0.041 ms (0.58%), N = 66, StdDev = 0.335 ms
Min = 6.688 ms, Q1 = 6.893 ms, Median = 7.066 ms, Q3 = 7.363 ms, Max = 8.035 ms
IQR = 0.470 ms, LowerFence = 6.189 ms, UpperFence = 8.068 ms
ConfidenceInterval = [7.008 ms; 7.292 ms] (CI 99.9%), Margin = 0.142 ms (1.99% of Mean)
Skewness = 0.81, Kurtosis = 2.64, MValue = 2.36
-------------------- Histogram --------------------
[6.579 ms ; 6.722 ms) | @
[6.722 ms ; 6.948 ms) | @@@@@@@@@@@@@@@@@@
[6.948 ms ; 7.273 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@
[7.273 ms ; 7.512 ms) | @@@@@
[7.512 ms ; 7.730 ms) | @@@@@@@@@@
[7.730 ms ; 7.947 ms) | @@@
[7.947 ms ; 8.144 ms) | @
---------------------------------------------------

SmallBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.871 ms, StdErr = 0.033 ms (0.48%), N = 18, StdDev = 0.139 ms
Min = 6.643 ms, Q1 = 6.756 ms, Median = 6.867 ms, Q3 = 6.972 ms, Max = 7.151 ms
IQR = 0.216 ms, LowerFence = 6.432 ms, UpperFence = 7.295 ms
ConfidenceInterval = [6.741 ms; 7.001 ms] (CI 99.9%), Margin = 0.130 ms (1.89% of Mean)
Skewness = 0.2, Kurtosis = 1.92, MValue = 2
-------------------- Histogram --------------------
[6.574 ms ; 6.841 ms) | @@@@@@@@@
[6.841 ms ; 7.016 ms) | @@@@@@@
[7.016 ms ; 7.180 ms) | @@
---------------------------------------------------

SmallBenchmarkEfService.EfCoreSingleAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.795 ms, StdErr = 0.029 ms (0.50%), N = 19, StdDev = 0.127 ms
Min = 5.641 ms, Q1 = 5.704 ms, Median = 5.762 ms, Q3 = 5.880 ms, Max = 6.039 ms
IQR = 0.176 ms, LowerFence = 5.440 ms, UpperFence = 6.143 ms
ConfidenceInterval = [5.681 ms; 5.909 ms] (CI 99.9%), Margin = 0.114 ms (1.97% of Mean)
Skewness = 0.51, Kurtosis = 1.86, MValue = 2
-------------------- Histogram --------------------
[5.639 ms ; 5.764 ms) | @@@@@@@@@@
[5.764 ms ; 5.934 ms) | @@@@@@
[5.934 ms ; 6.071 ms) | @@@
---------------------------------------------------

SmallBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.712 ms, StdErr = 0.026 ms (0.46%), N = 14, StdDev = 0.099 ms
Min = 5.607 ms, Q1 = 5.628 ms, Median = 5.683 ms, Q3 = 5.768 ms, Max = 5.904 ms
IQR = 0.140 ms, LowerFence = 5.417 ms, UpperFence = 5.978 ms
ConfidenceInterval = [5.600 ms; 5.824 ms] (CI 99.9%), Margin = 0.112 ms (1.96% of Mean)
Skewness = 0.65, Kurtosis = 1.97, MValue = 2
-------------------- Histogram --------------------
[5.590 ms ; 5.822 ms) | @@@@@@@@@@@@
[5.822 ms ; 5.958 ms) | @@
---------------------------------------------------

SmallBenchmarkEfService.EfCoreSingleAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.355 ms, StdErr = 0.044 ms (0.53%), N = 30, StdDev = 0.241 ms
Min = 8.058 ms, Q1 = 8.177 ms, Median = 8.304 ms, Q3 = 8.495 ms, Max = 8.977 ms
IQR = 0.318 ms, LowerFence = 7.699 ms, UpperFence = 8.973 ms
ConfidenceInterval = [8.193 ms; 8.516 ms] (CI 99.9%), Margin = 0.161 ms (1.93% of Mean)
Skewness = 0.75, Kurtosis = 2.81, MValue = 2
-------------------- Histogram --------------------
[8.038 ms ; 8.241 ms) | @@@@@@@@@@@@
[8.241 ms ; 8.571 ms) | @@@@@@@@@@@@@@
[8.571 ms ; 8.852 ms) | @@@
[8.852 ms ; 9.079 ms) | @
---------------------------------------------------

SmallBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.288 ms, StdErr = 0.032 ms (0.38%), N = 16, StdDev = 0.127 ms
Min = 8.057 ms, Q1 = 8.171 ms, Median = 8.327 ms, Q3 = 8.380 ms, Max = 8.479 ms
IQR = 0.209 ms, LowerFence = 7.858 ms, UpperFence = 8.693 ms
ConfidenceInterval = [8.158 ms; 8.417 ms] (CI 99.9%), Margin = 0.130 ms (1.57% of Mean)
Skewness = -0.43, Kurtosis = 1.72, MValue = 2
-------------------- Histogram --------------------
[8.039 ms ; 8.256 ms) | @@@@@
[8.256 ms ; 8.546 ms) | @@@@@@@@@@@
---------------------------------------------------

SmallBenchmarkEfService.EfCoreSingleAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.781 ms, StdErr = 0.037 ms (0.42%), N = 15, StdDev = 0.143 ms
Min = 8.558 ms, Q1 = 8.684 ms, Median = 8.769 ms, Q3 = 8.855 ms, Max = 9.101 ms
IQR = 0.171 ms, LowerFence = 8.428 ms, UpperFence = 9.111 ms
ConfidenceInterval = [8.629 ms; 8.934 ms] (CI 99.9%), Margin = 0.153 ms (1.74% of Mean)
Skewness = 0.53, Kurtosis = 2.58, MValue = 2
-------------------- Histogram --------------------
[8.482 ms ; 8.727 ms) | @@@@@@
[8.727 ms ; 9.135 ms) | @@@@@@@@@
---------------------------------------------------

SmallBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.868 ms, StdErr = 0.045 ms (0.51%), N = 22, StdDev = 0.211 ms
Min = 8.640 ms, Q1 = 8.707 ms, Median = 8.779 ms, Q3 = 9.024 ms, Max = 9.324 ms
IQR = 0.317 ms, LowerFence = 8.231 ms, UpperFence = 9.500 ms
ConfidenceInterval = [8.696 ms; 9.040 ms] (CI 99.9%), Margin = 0.172 ms (1.94% of Mean)
Skewness = 0.8, Kurtosis = 2.2, MValue = 2
-------------------- Histogram --------------------
[8.639 ms ; 8.837 ms) | @@@@@@@@@@@@@@
[8.837 ms ; 9.206 ms) | @@@@@@
[9.206 ms ; 9.423 ms) | @@
---------------------------------------------------
```

## Bulk stored procedures
Inserting 1.000.000 items with stored procedures, using `general_series`.
`CleanupTable` tells if the data in the `benchmark` table was deleted (for PG additionally vacuumed) after each iteration.

| Method          | BenchmarkDbType   | CleanupTable | Mean     | Error     | StdDev    |
|---------------- |------------------ |------------- |---------:|----------:|----------:|
| StoredProcedure | MsSql             | False        | 14.339 s |  1.7154 s |  2.4601 s |
| StoredProcedure | PostgreSqlWindows | False        | 34.574 s | 11.5119 s | 17.2305 s |
| StoredProcedure | PostgreSqlLinux   | False        | 17.197 s |  5.1044 s |  7.6400 s |
| StoredProcedure | PostgreSqlCitus   | False        | 17.435 s |  5.0567 s |  7.5686 s |
| StoredProcedure | MsSql             | True         |  6.701 s |  0.1428 s |  0.2138 s |
| StoredProcedure | PostgreSqlWindows | True         |  8.331 s |  0.1543 s |  0.1715 s |
| StoredProcedure | PostgreSqlLinux   | True         |  5.409 s |  0.1244 s |  0.1823 s |
| StoredProcedure | PostgreSqlCitus   | True         |  5.489 s |  0.1788 s |  0.2677 s |

```
BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=MsSql, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 14.339 s, StdErr = 0.465 s (3.24%), N = 28, StdDev = 2.460 s
Min = 7.578 s, Q1 = 13.599 s, Median = 15.320 s, Q3 = 16.076 s, Max = 16.898 s
IQR = 2.477 s, LowerFence = 9.883 s, UpperFence = 19.791 s
ConfidenceInterval = [12.624 s; 16.055 s] (CI 99.9%), Margin = 1.715 s (11.96% of Mean)
Skewness = -1.43, Kurtosis = 4.07, MValue = 3
-------------------- Histogram --------------------
[ 6.851 s ;  8.978 s) | @@
[ 8.978 s ; 11.404 s) | @@
[11.404 s ; 12.298 s) |
[12.298 s ; 14.794 s) | @@@@@@
[14.794 s ; 16.920 s) | @@@@@@@@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=MsSql, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.701 s, StdErr = 0.039 s (0.58%), N = 30, StdDev = 0.214 s
Min = 6.336 s, Q1 = 6.565 s, Median = 6.642 s, Q3 = 6.864 s, Max = 7.189 s
IQR = 0.299 s, LowerFence = 6.117 s, UpperFence = 7.312 s
ConfidenceInterval = [6.559 s; 6.844 s] (CI 99.9%), Margin = 0.143 s (2.13% of Mean)
Skewness = 0.33, Kurtosis = 2.33, MValue = 2
-------------------- Histogram --------------------
[6.288 s ; 6.469 s) | @@@
[6.469 s ; 6.650 s) | @@@@@@@@@@@@@
[6.650 s ; 6.923 s) | @@@@@@@@@@
[6.923 s ; 7.122 s) | @@@
[7.122 s ; 7.279 s) | @
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlWindows, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 34.574 s, StdErr = 3.146 s (9.10%), N = 30, StdDev = 17.231 s
Min = 7.687 s, Q1 = 20.524 s, Median = 32.183 s, Q3 = 51.342 s, Max = 61.146 s
IQR = 30.818 s, LowerFence = -25.702 s, UpperFence = 97.569 s
ConfidenceInterval = [23.062 s; 46.086 s] (CI 99.9%), Margin = 11.512 s (33.30% of Mean)
Skewness = 0.04, Kurtosis = 1.5, MValue = 3.09
-------------------- Histogram --------------------
[ 0.409 s ; 16.064 s) | @@@@
[16.064 s ; 30.620 s) | @@@@@@@@@@@
[30.620 s ; 44.642 s) | @@@@
[44.642 s ; 59.198 s) | @@@@@@@@@@
[59.198 s ; 68.424 s) | @
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlWindows, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.331 s, StdErr = 0.039 s (0.47%), N = 19, StdDev = 0.172 s
Min = 7.994 s, Q1 = 8.225 s, Median = 8.356 s, Q3 = 8.430 s, Max = 8.733 s
IQR = 0.206 s, LowerFence = 7.916 s, UpperFence = 8.739 s
ConfidenceInterval = [8.176 s; 8.485 s] (CI 99.9%), Margin = 0.154 s (1.85% of Mean)
Skewness = 0.34, Kurtosis = 2.94, MValue = 2
-------------------- Histogram --------------------
[7.909 s ; 8.213 s) | @@@@
[8.213 s ; 8.382 s) | @@@@@@@@@
[8.382 s ; 8.754 s) | @@@@@@
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlLinux, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 17.197 s, StdErr = 1.395 s (8.11%), N = 30, StdDev = 7.640 s
Min = 6.839 s, Q1 = 10.729 s, Median = 15.621 s, Q3 = 23.653 s, Max = 30.238 s
IQR = 12.924 s, LowerFence = -8.657 s, UpperFence = 43.039 s
ConfidenceInterval = [12.092 s; 22.301 s] (CI 99.9%), Margin = 5.104 s (29.68% of Mean)
Skewness = 0.19, Kurtosis = 1.53, MValue = 2.46
-------------------- Histogram --------------------
[ 6.803 s ; 13.257 s) | @@@@@@@@@@@@@
[13.257 s ; 20.307 s) | @@@@@@
[20.307 s ; 28.110 s) | @@@@@@@@@
[28.110 s ; 33.465 s) | @@
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlLinux, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.409 s, StdErr = 0.034 s (0.63%), N = 29, StdDev = 0.182 s
Min = 4.948 s, Q1 = 5.318 s, Median = 5.374 s, Q3 = 5.515 s, Max = 5.819 s
IQR = 0.197 s, LowerFence = 5.023 s, UpperFence = 5.810 s
ConfidenceInterval = [5.285 s; 5.534 s] (CI 99.9%), Margin = 0.124 s (2.30% of Mean)
Skewness = 0.11, Kurtosis = 3.32, MValue = 2
-------------------- Histogram --------------------
[4.870 s ; 5.026 s) | @
[5.026 s ; 5.132 s) |
[5.132 s ; 5.310 s) | @@@@@
[5.310 s ; 5.466 s) | @@@@@@@@@@@@@@@
[5.466 s ; 5.652 s) | @@@@@
[5.652 s ; 5.826 s) | @@@
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlCitus, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 17.435 s, StdErr = 1.382 s (7.93%), N = 30, StdDev = 7.569 s
Min = 7.093 s, Q1 = 10.547 s, Median = 15.790 s, Q3 = 25.352 s, Max = 29.717 s
IQR = 14.805 s, LowerFence = -11.661 s, UpperFence = 47.560 s
ConfidenceInterval = [12.378 s; 22.492 s] (CI 99.9%), Margin = 5.057 s (29.00% of Mean)
Skewness = 0.12, Kurtosis = 1.4, MValue = 2.83
-------------------- Histogram --------------------
[ 6.723 s ; 13.983 s) | @@@@@@@@@@@@
[13.983 s ; 21.582 s) | @@@@@@
[21.582 s ; 27.975 s) | @@@@@@@@@@@
[27.975 s ; 32.914 s) | @
---------------------------------------------------

BulkBenchmarkStoredProcService.StoredProcedure: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BenchmarkDbType=PostgreSqlCitus, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.489 s, StdErr = 0.049 s (0.89%), N = 30, StdDev = 0.268 s
Min = 5.016 s, Q1 = 5.284 s, Median = 5.464 s, Q3 = 5.643 s, Max = 6.012 s
IQR = 0.359 s, LowerFence = 4.745 s, UpperFence = 6.182 s
ConfidenceInterval = [5.310 s; 5.668 s] (CI 99.9%), Margin = 0.179 s (3.26% of Mean)
Skewness = 0.22, Kurtosis = 2.2, MValue = 3.33
-------------------- Histogram --------------------
[4.932 s ; 5.158 s) | @@@
[5.158 s ; 5.456 s) | @@@@@@@@@@@@
[5.456 s ; 5.576 s) | @@
[5.576 s ; 5.874 s) | @@@@@@@@@@
[5.874 s ; 6.125 s) | @@@
---------------------------------------------------
```

## Bulk Entity Framework Core
Inserting 1.000.000 items with EF Core, using EF.BulkExtensions.
- `CleanupTable` tells if the data in the `benchmark` table was deleted (for PG additionally vacuumed) after each iteration.
- `BatchSize` is the EFCore.Extensions BatchSize property - meaning the insert is done multiple times with the determined batch number of items.

| Method     | BatchSize | BenchmarkDbType   | CleanupTable | Mean     | Error     | StdDev    | Median   |
|----------- |---------- |------------------ |------------- |---------:|----------:|----------:|---------:|
| EfCoreBulk | 2000      | MsSql             | False        | 21.978 s |  2.2255 s |  3.3310 s | 23.364 s |
| EfCoreBulk | 2000      | PostgreSqlWindows | False        | 29.272 s | 11.9565 s | 17.8959 s | 22.649 s |
| EfCoreBulk | 2000      | PostgreSqlLinux   | False        | 12.509 s |  4.3857 s |  6.5643 s |  9.652 s |
| EfCoreBulk | 2000      | PostgreSqlCitus   | False        | 12.578 s |  4.4073 s |  6.5966 s |  9.537 s |
| EfCoreBulk | 50000     | MsSql             | False        | 12.593 s |  1.8104 s |  2.7097 s | 12.119 s |
| EfCoreBulk | 50000     | PostgreSqlWindows | False        | 28.879 s | 11.8316 s | 17.7089 s | 22.128 s |
| EfCoreBulk | 50000     | PostgreSqlLinux   | False        | 12.232 s |  4.2335 s |  6.3365 s |  9.586 s |
| EfCoreBulk | 50000     | PostgreSqlCitus   | False        | 12.527 s |  4.4458 s |  6.6543 s |  9.440 s |
| EfCoreBulk | 200000    | MsSql             | False        | 12.632 s |  1.8151 s |  2.7167 s | 12.465 s |
| EfCoreBulk | 200000    | PostgreSqlWindows | False        | 29.629 s | 12.3954 s | 18.5528 s | 22.163 s |
| EfCoreBulk | 200000    | PostgreSqlLinux   | False        | 12.526 s |  4.1181 s |  6.1638 s | 10.523 s |
| EfCoreBulk | 200000    | PostgreSqlCitus   | True         |  3.514 s |  0.0791 s |  0.1184 s |  3.475 s |
| EfCoreBulk | 2000      | MsSql             | True         | 13.996 s |  0.1972 s |  0.1647 s | 14.016 s |
| EfCoreBulk | 2000      | PostgreSqlWindows | True         |  5.223 s |  0.0971 s |  0.0997 s |  5.251 s |
| EfCoreBulk | 2000      | PostgreSqlLinux   | True         |  3.396 s |  0.0582 s |  0.0544 s |  3.381 s |
| EfCoreBulk | 2000      | PostgreSqlCitus   | True         |  3.442 s |  0.0629 s |  0.0673 s |  3.413 s |
| EfCoreBulk | 50000     | MsSql             | True         |  8.267 s |  0.1061 s |  0.0886 s |  8.274 s |
| EfCoreBulk | 50000     | PostgreSqlWindows | True         |  5.298 s |  0.0653 s |  0.0611 s |  5.289 s |
| EfCoreBulk | 50000     | PostgreSqlLinux   | True         |  3.380 s |  0.0653 s |  0.0641 s |  3.371 s |
| EfCoreBulk | 50000     | PostgreSqlCitus   | True         |  3.436 s |  0.0655 s |  0.0581 s |  3.424 s |
| EfCoreBulk | 200000    | MsSql             | True         |  7.715 s |  0.1061 s |  0.0886 s |  7.694 s |
| EfCoreBulk | 200000    | PostgreSqlWindows | True         |  5.322 s |  0.0949 s |  0.0888 s |  5.315 s |
| EfCoreBulk | 200000    | PostgreSqlLinux   | True         |  3.507 s |  0.0552 s |  0.0567 s |  3.500 s |
| EfCoreBulk | 200000    | PostgreSqlCitus   | False        | 13.116 s |  4.4901 s |  6.7205 s |  9.666 s |

```
BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 21.978 s, StdErr = 0.608 s (2.77%), N = 30, StdDev = 3.331 s
Min = 14.899 s, Q1 = 20.084 s, Median = 23.364 s, Q3 = 24.715 s, Max = 25.459 s
IQR = 4.631 s, LowerFence = 13.138 s, UpperFence = 31.661 s
ConfidenceInterval = [19.753 s; 24.204 s] (CI 99.9%), Margin = 2.225 s (10.13% of Mean)
Skewness = -0.77, Kurtosis = 2.29, MValue = 3.29
-------------------- Histogram --------------------
[14.646 s ; 17.460 s) | @@@@
[17.460 s ; 18.805 s) | @
[18.805 s ; 21.619 s) | @@@@@@@@
[21.619 s ; 22.719 s) |
[22.719 s ; 25.533 s) | @@@@@@@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=MsSql, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.996 s, StdErr = 0.046 s (0.33%), N = 13, StdDev = 0.165 s
Min = 13.615 s, Q1 = 13.951 s, Median = 14.016 s, Q3 = 14.082 s, Max = 14.290 s
IQR = 0.131 s, LowerFence = 13.754 s, UpperFence = 14.280 s
ConfidenceInterval = [13.799 s; 14.193 s] (CI 99.9%), Margin = 0.197 s (1.41% of Mean)
Skewness = -0.58, Kurtosis = 3.17, MValue = 2
-------------------- Histogram --------------------
[13.523 s ; 13.892 s) | @@@
[13.892 s ; 14.382 s) | @@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 29.272 s, StdErr = 3.267 s (11.16%), N = 30, StdDev = 17.896 s
Min = 5.284 s, Q1 = 15.184 s, Median = 22.649 s, Q3 = 46.081 s, Max = 58.283 s
IQR = 30.897 s, LowerFence = -31.161 s, UpperFence = 92.425 s
ConfidenceInterval = [17.315 s; 41.228 s] (CI 99.9%), Margin = 11.956 s (40.85% of Mean)
Skewness = 0.31, Kurtosis = 1.47, MValue = 3.14
-------------------- Histogram --------------------
[ 4.380 s ; 19.498 s) | @@@@@@@@@@@@@@
[19.498 s ; 33.480 s) | @@@@
[33.480 s ; 43.236 s) | @@
[43.236 s ; 58.355 s) | @@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.223 s, StdErr = 0.024 s (0.46%), N = 17, StdDev = 0.100 s
Min = 5.016 s, Q1 = 5.193 s, Median = 5.251 s, Q3 = 5.278 s, Max = 5.370 s
IQR = 0.085 s, LowerFence = 5.065 s, UpperFence = 5.406 s
ConfidenceInterval = [5.126 s; 5.320 s] (CI 99.9%), Margin = 0.097 s (1.86% of Mean)
Skewness = -0.62, Kurtosis = 2.47, MValue = 2
-------------------- Histogram --------------------
[4.996 s ; 5.098 s) | @@@
[5.098 s ; 5.210 s) | @@
[5.210 s ; 5.421 s) | @@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.509 s, StdErr = 1.198 s (9.58%), N = 30, StdDev = 6.564 s
Min = 3.657 s, Q1 = 7.632 s, Median = 9.652 s, Q3 = 19.329 s, Max = 24.419 s
IQR = 11.698 s, LowerFence = -9.915 s, UpperFence = 36.876 s
ConfidenceInterval = [8.123 s; 16.895 s] (CI 99.9%), Margin = 4.386 s (35.06% of Mean)
Skewness = 0.49, Kurtosis = 1.67, MValue = 2.53
-------------------- Histogram --------------------
[ 0.884 s ;  5.049 s) | @@
[ 5.049 s ; 10.595 s) | @@@@@@@@@@@@@@@
[10.595 s ; 16.474 s) | @@@@
[16.474 s ; 23.746 s) | @@@@@@@@
[23.746 s ; 27.192 s) | @
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.396 s, StdErr = 0.014 s (0.41%), N = 15, StdDev = 0.054 s
Min = 3.323 s, Q1 = 3.365 s, Median = 3.381 s, Q3 = 3.415 s, Max = 3.507 s
IQR = 0.050 s, LowerFence = 3.290 s, UpperFence = 3.491 s
ConfidenceInterval = [3.338 s; 3.454 s] (CI 99.9%), Margin = 0.058 s (1.71% of Mean)
Skewness = 0.63, Kurtosis = 2.49, MValue = 2
-------------------- Histogram --------------------
[3.294 s ; 3.361 s) | @@@@
[3.361 s ; 3.536 s) | @@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.578 s, StdErr = 1.204 s (9.58%), N = 30, StdDev = 6.597 s
Min = 3.981 s, Q1 = 7.581 s, Median = 9.537 s, Q3 = 18.579 s, Max = 25.221 s
IQR = 10.998 s, LowerFence = -8.917 s, UpperFence = 35.077 s
ConfidenceInterval = [8.171 s; 16.985 s] (CI 99.9%), Margin = 4.407 s (35.04% of Mean)
Skewness = 0.47, Kurtosis = 1.65, MValue = 2.8
-------------------- Histogram --------------------
[ 1.195 s ;  5.187 s) | @@
[ 5.187 s ; 10.760 s) | @@@@@@@@@@@@@@@
[10.760 s ; 16.617 s) | @@@
[16.617 s ; 23.067 s) | @@@@@@@@@
[23.067 s ; 28.007 s) | @
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=2000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.442 s, StdErr = 0.016 s (0.46%), N = 18, StdDev = 0.067 s
Min = 3.377 s, Q1 = 3.399 s, Median = 3.413 s, Q3 = 3.485 s, Max = 3.630 s
IQR = 0.086 s, LowerFence = 3.271 s, UpperFence = 3.613 s
ConfidenceInterval = [3.379 s; 3.505 s] (CI 99.9%), Margin = 0.063 s (1.83% of Mean)
Skewness = 1.25, Kurtosis = 3.89, MValue = 2
-------------------- Histogram --------------------
[3.372 s ; 3.477 s) | @@@@@@@@@@@@
[3.477 s ; 3.664 s) | @@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.593 s, StdErr = 0.495 s (3.93%), N = 30, StdDev = 2.710 s
Min = 9.042 s, Q1 = 10.032 s, Median = 12.119 s, Q3 = 15.267 s, Max = 16.474 s
IQR = 5.235 s, LowerFence = 2.180 s, UpperFence = 23.119 s
ConfidenceInterval = [10.782 s; 14.403 s] (CI 99.9%), Margin = 1.810 s (14.38% of Mean)
Skewness = 0.09, Kurtosis = 1.36, MValue = 2.77
-------------------- Histogram --------------------
[ 8.954 s ; 11.671 s) | @@@@@@@@@@@@@
[11.671 s ; 14.385 s) | @@@@@@
[14.385 s ; 16.674 s) | @@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=MsSql, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.267 s, StdErr = 0.025 s (0.30%), N = 13, StdDev = 0.089 s
Min = 8.107 s, Q1 = 8.240 s, Median = 8.274 s, Q3 = 8.349 s, Max = 8.368 s
IQR = 0.109 s, LowerFence = 8.077 s, UpperFence = 8.512 s
ConfidenceInterval = [8.161 s; 8.373 s] (CI 99.9%), Margin = 0.106 s (1.28% of Mean)
Skewness = -0.53, Kurtosis = 1.83, MValue = 2
-------------------- Histogram --------------------
[8.057 s ; 8.404 s) | @@@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 28.879 s, StdErr = 3.233 s (11.20%), N = 30, StdDev = 17.709 s
Min = 5.078 s, Q1 = 14.666 s, Median = 22.128 s, Q3 = 45.916 s, Max = 57.858 s
IQR = 31.250 s, LowerFence = -32.208 s, UpperFence = 92.790 s
ConfidenceInterval = [17.048 s; 40.711 s] (CI 99.9%), Margin = 11.832 s (40.97% of Mean)
Skewness = 0.33, Kurtosis = 1.48, MValue = 2.86
-------------------- Histogram --------------------
[ 4.394 s ; 19.354 s) | @@@@@@@@@@@@@@
[19.354 s ; 33.320 s) | @@@@
[33.320 s ; 43.663 s) | @@@
[43.663 s ; 58.623 s) | @@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.298 s, StdErr = 0.016 s (0.30%), N = 15, StdDev = 0.061 s
Min = 5.216 s, Q1 = 5.245 s, Median = 5.289 s, Q3 = 5.348 s, Max = 5.402 s
IQR = 0.103 s, LowerFence = 5.090 s, UpperFence = 5.504 s
ConfidenceInterval = [5.232 s; 5.363 s] (CI 99.9%), Margin = 0.065 s (1.23% of Mean)
Skewness = 0.28, Kurtosis = 1.61, MValue = 2
-------------------- Histogram --------------------
[5.193 s ; 5.434 s) | @@@@@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.232 s, StdErr = 1.157 s (9.46%), N = 30, StdDev = 6.337 s
Min = 3.699 s, Q1 = 7.505 s, Median = 9.586 s, Q3 = 17.484 s, Max = 24.508 s
IQR = 9.979 s, LowerFence = -7.464 s, UpperFence = 32.453 s
ConfidenceInterval = [7.999 s; 16.466 s] (CI 99.9%), Margin = 4.234 s (34.61% of Mean)
Skewness = 0.53, Kurtosis = 1.86, MValue = 2.43
-------------------- Histogram --------------------
[ 1.023 s ;  4.704 s) | @@
[ 4.704 s ; 10.057 s) | @@@@@@@@@@@@@@
[10.057 s ; 15.825 s) | @@@@
[15.825 s ; 21.178 s) | @@@@@@@
[21.178 s ; 27.184 s) | @@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.380 s, StdErr = 0.016 s (0.47%), N = 16, StdDev = 0.064 s
Min = 3.308 s, Q1 = 3.332 s, Median = 3.371 s, Q3 = 3.410 s, Max = 3.551 s
IQR = 0.078 s, LowerFence = 3.216 s, UpperFence = 3.526 s
ConfidenceInterval = [3.315 s; 3.445 s] (CI 99.9%), Margin = 0.065 s (1.93% of Mean)
Skewness = 1.06, Kurtosis = 3.62, MValue = 2
-------------------- Histogram --------------------
[3.307 s ; 3.393 s) | @@@@@@@@@
[3.393 s ; 3.585 s) | @@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.527 s, StdErr = 1.215 s (9.70%), N = 30, StdDev = 6.654 s
Min = 3.632 s, Q1 = 7.575 s, Median = 9.440 s, Q3 = 18.702 s, Max = 24.598 s
IQR = 11.127 s, LowerFence = -9.115 s, UpperFence = 35.392 s
ConfidenceInterval = [8.082 s; 16.973 s] (CI 99.9%), Margin = 4.446 s (35.49% of Mean)
Skewness = 0.51, Kurtosis = 1.74, MValue = 2.4
-------------------- Histogram --------------------
[ 0.821 s ;  4.958 s) | @@
[ 4.958 s ; 12.078 s) | @@@@@@@@@@@@@@@
[12.078 s ; 17.699 s) | @@@@@
[17.699 s ; 24.664 s) | @@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=50000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.436 s, StdErr = 0.016 s (0.45%), N = 14, StdDev = 0.058 s
Min = 3.374 s, Q1 = 3.400 s, Median = 3.424 s, Q3 = 3.449 s, Max = 3.591 s
IQR = 0.049 s, LowerFence = 3.327 s, UpperFence = 3.522 s
ConfidenceInterval = [3.371 s; 3.502 s] (CI 99.9%), Margin = 0.066 s (1.91% of Mean)
Skewness = 1.26, Kurtosis = 4, MValue = 2
-------------------- Histogram --------------------
[3.371 s ; 3.512 s) | @@@@@@@@@@@@@
[3.512 s ; 3.622 s) | @
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=MsSql, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.632 s, StdErr = 0.496 s (3.93%), N = 30, StdDev = 2.717 s
Min = 8.328 s, Q1 = 10.139 s, Median = 12.465 s, Q3 = 15.623 s, Max = 16.313 s
IQR = 5.484 s, LowerFence = 1.913 s, UpperFence = 23.849 s
ConfidenceInterval = [10.817 s; 14.447 s] (CI 99.9%), Margin = 1.815 s (14.37% of Mean)
Skewness = -0.06, Kurtosis = 1.45, MValue = 3.82
-------------------- Histogram --------------------
[ 7.180 s ;  8.527 s) | @
[ 8.527 s ; 10.822 s) | @@@@@@@@@@
[10.822 s ; 13.566 s) | @@@@@@@@
[13.566 s ; 14.380 s) |
[14.380 s ; 16.675 s) | @@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=MsSql, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.715 s, StdErr = 0.025 s (0.32%), N = 13, StdDev = 0.089 s
Min = 7.582 s, Q1 = 7.644 s, Median = 7.694 s, Q3 = 7.807 s, Max = 7.844 s
IQR = 0.162 s, LowerFence = 7.401 s, UpperFence = 8.050 s
ConfidenceInterval = [7.609 s; 7.822 s] (CI 99.9%), Margin = 0.106 s (1.38% of Mean)
Skewness = 0.07, Kurtosis = 1.29, MValue = 2
-------------------- Histogram --------------------
[7.580 s ; 7.872 s) | @@@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 29.629 s, StdErr = 3.387 s (11.43%), N = 30, StdDev = 18.553 s
Min = 5.271 s, Q1 = 14.933 s, Median = 22.163 s, Q3 = 47.787 s, Max = 59.561 s
IQR = 32.854 s, LowerFence = -34.348 s, UpperFence = 97.068 s
ConfidenceInterval = [17.234 s; 42.024 s] (CI 99.9%), Margin = 12.395 s (41.84% of Mean)
Skewness = 0.32, Kurtosis = 1.43, MValue = 3.2
-------------------- Histogram --------------------
[ 5.157 s ; 20.830 s) | @@@@@@@@@@@@@@@
[20.830 s ; 37.935 s) | @@@@
[37.935 s ; 43.951 s) | @
[43.951 s ; 59.625 s) | @@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlWindows, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.322 s, StdErr = 0.023 s (0.43%), N = 15, StdDev = 0.089 s
Min = 5.097 s, Q1 = 5.304 s, Median = 5.315 s, Q3 = 5.377 s, Max = 5.468 s
IQR = 0.072 s, LowerFence = 5.196 s, UpperFence = 5.485 s
ConfidenceInterval = [5.227 s; 5.416 s] (CI 99.9%), Margin = 0.095 s (1.78% of Mean)
Skewness = -0.75, Kurtosis = 3.56, MValue = 2
-------------------- Histogram --------------------
[5.050 s ; 5.185 s) | @
[5.185 s ; 5.291 s) | @@
[5.291 s ; 5.503 s) | @@@@@@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.526 s, StdErr = 1.125 s (8.98%), N = 30, StdDev = 6.164 s
Min = 3.881 s, Q1 = 7.566 s, Median = 10.523 s, Q3 = 18.183 s, Max = 23.226 s
IQR = 10.617 s, LowerFence = -8.359 s, UpperFence = 34.108 s
ConfidenceInterval = [8.408 s; 16.644 s] (CI 99.9%), Margin = 4.118 s (32.88% of Mean)
Skewness = 0.4, Kurtosis = 1.64, MValue = 2.31
-------------------- Histogram --------------------
[ 1.277 s ;  6.078 s) | @@@
[ 6.078 s ; 11.285 s) | @@@@@@@@@@@@@
[11.285 s ; 17.228 s) | @@@@@@
[17.228 s ; 23.484 s) | @@@@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlLinux, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.507 s, StdErr = 0.014 s (0.39%), N = 17, StdDev = 0.057 s
Min = 3.418 s, Q1 = 3.477 s, Median = 3.500 s, Q3 = 3.538 s, Max = 3.662 s
IQR = 0.060 s, LowerFence = 3.387 s, UpperFence = 3.628 s
ConfidenceInterval = [3.452 s; 3.563 s] (CI 99.9%), Margin = 0.055 s (1.57% of Mean)
Skewness = 0.93, Kurtosis = 3.9, MValue = 2
-------------------- Histogram --------------------
[3.389 s ; 3.517 s) | @@@@@@@@@@@
[3.517 s ; 3.691 s) | @@@@@@
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=False]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 13.116 s, StdErr = 1.227 s (9.35%), N = 30, StdDev = 6.721 s
Min = 4.720 s, Q1 = 7.687 s, Median = 9.666 s, Q3 = 19.653 s, Max = 24.211 s
IQR = 11.967 s, LowerFence = -10.263 s, UpperFence = 37.603 s
ConfidenceInterval = [8.626 s; 17.606 s] (CI 99.9%), Margin = 4.490 s (34.23% of Mean)
Skewness = 0.39, Kurtosis = 1.42, MValue = 3.13
-------------------- Histogram --------------------
[ 4.475 s ; 10.153 s) | @@@@@@@@@@@@@@@@
[10.153 s ; 14.800 s) | @@
[14.800 s ; 17.746 s) | @
[17.746 s ; 23.424 s) | @@@@@@@@@@
[23.424 s ; 27.049 s) | @
---------------------------------------------------

BulkBenchmarkEfService.EfCoreBulk: Job-QSSRVS(InvocationCount=1, MaxIterationCount=30, UnrollFactor=1, WarmupCount=0) [BatchSize=200000, BenchmarkDbType=PostgreSqlCitus, CleanupTable=True]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 3.514 s, StdErr = 0.022 s (0.62%), N = 30, StdDev = 0.118 s
Min = 3.380 s, Q1 = 3.431 s, Median = 3.475 s, Q3 = 3.600 s, Max = 3.761 s
IQR = 0.169 s, LowerFence = 3.178 s, UpperFence = 3.853 s
ConfidenceInterval = [3.435 s; 3.593 s] (CI 99.9%), Margin = 0.079 s (2.25% of Mean)
Skewness = 0.84, Kurtosis = 2.35, MValue = 2.22
-------------------- Histogram --------------------
[3.380 s ; 3.480 s) | @@@@@@@@@@@@@@@@@@
[3.480 s ; 3.554 s) | @@@
[3.554 s ; 3.698 s) | @@@@@
[3.698 s ; 3.811 s) | @@@@
---------------------------------------------------
```

## Small inserts on already large table
Table consisting of millions of rows receives non-bulk small inserts of 100s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start

| Method         | InitialTableSize | BenchmarkDbType   | Mean      | Error     | StdDev    |
|--------------- |----------------- |------------------ |----------:|----------:|----------:|
| EfCoreRangeAdd | 3000001          | MsSql             |  7.052 ms | 0.0630 ms | 0.0559 ms |
| EfCoreRangeAdd | 3000001          | PostgreSqlWindows |  5.900 ms | 0.1124 ms | 0.1155 ms |
| EfCoreRangeAdd | 3000001          | PostgreSqlLinux   |  8.493 ms | 0.1392 ms | 0.1234 ms |
| EfCoreRangeAdd | 3000001          | PostgreSqlCitus   |  9.012 ms | 0.1750 ms | 0.1873 ms |
| EfCoreRangeAdd | 10000001         | MsSql             |  6.985 ms | 0.1372 ms | 0.1580 ms |
| EfCoreRangeAdd | 10000001         | PostgreSqlWindows |  7.492 ms | 0.1473 ms | 0.1447 ms |
| EfCoreRangeAdd | 10000001         | PostgreSqlLinux   | 11.321 ms | 0.2163 ms | 0.2491 ms |
| EfCoreRangeAdd | 10000001         | PostgreSqlCitus   | 12.194 ms | 0.2407 ms | 0.4341 ms |

```
SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=3000001, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.052 ms, StdErr = 0.015 ms (0.21%), N = 14, StdDev = 0.056 ms
Min = 6.995 ms, Q1 = 7.010 ms, Median = 7.029 ms, Q3 = 7.089 ms, Max = 7.155 ms
IQR = 0.079 ms, LowerFence = 6.892 ms, UpperFence = 7.208 ms
ConfidenceInterval = [6.989 ms; 7.115 ms] (CI 99.9%), Margin = 0.063 ms (0.89% of Mean)
Skewness = 0.8, Kurtosis = 1.96, MValue = 2
-------------------- Histogram --------------------
[6.964 ms ; 7.185 ms) | @@@@@@@@@@@@@@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=3000001, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 5.900 ms, StdErr = 0.028 ms (0.47%), N = 17, StdDev = 0.115 ms
Min = 5.697 ms, Q1 = 5.808 ms, Median = 5.932 ms, Q3 = 5.983 ms, Max = 6.047 ms
IQR = 0.175 ms, LowerFence = 5.546 ms, UpperFence = 6.245 ms
ConfidenceInterval = [5.787 ms; 6.012 ms] (CI 99.9%), Margin = 0.112 ms (1.91% of Mean)
Skewness = -0.38, Kurtosis = 1.75, MValue = 2
-------------------- Histogram --------------------
[5.670 ms ; 5.786 ms) | @@@
[5.786 ms ; 6.049 ms) | @@@@@@@@@@@@@@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=3000001, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.493 ms, StdErr = 0.033 ms (0.39%), N = 14, StdDev = 0.123 ms
Min = 8.329 ms, Q1 = 8.391 ms, Median = 8.465 ms, Q3 = 8.575 ms, Max = 8.695 ms
IQR = 0.184 ms, LowerFence = 8.115 ms, UpperFence = 8.851 ms
ConfidenceInterval = [8.353 ms; 8.632 ms] (CI 99.9%), Margin = 0.139 ms (1.64% of Mean)
Skewness = 0.29, Kurtosis = 1.6, MValue = 2
-------------------- Histogram --------------------
[8.303 ms ; 8.762 ms) | @@@@@@@@@@@@@@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=3000001, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 9.012 ms, StdErr = 0.044 ms (0.49%), N = 18, StdDev = 0.187 ms
Min = 8.789 ms, Q1 = 8.843 ms, Median = 9.022 ms, Q3 = 9.112 ms, Max = 9.428 ms
IQR = 0.269 ms, LowerFence = 8.439 ms, UpperFence = 9.515 ms
ConfidenceInterval = [8.837 ms; 9.187 ms] (CI 99.9%), Margin = 0.175 ms (1.94% of Mean)
Skewness = 0.46, Kurtosis = 2.18, MValue = 2
-------------------- Histogram --------------------
[8.770 ms ; 8.957 ms) | @@@@@@@@
[8.957 ms ; 9.226 ms) | @@@@@@@@
[9.226 ms ; 9.448 ms) | @@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=10000001, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 6.985 ms, StdErr = 0.035 ms (0.51%), N = 20, StdDev = 0.158 ms
Min = 6.640 ms, Q1 = 6.903 ms, Median = 6.968 ms, Q3 = 7.114 ms, Max = 7.262 ms
IQR = 0.211 ms, LowerFence = 6.587 ms, UpperFence = 7.430 ms
ConfidenceInterval = [6.848 ms; 7.122 ms] (CI 99.9%), Margin = 0.137 ms (1.96% of Mean)
Skewness = -0.38, Kurtosis = 2.41, MValue = 2
-------------------- Histogram --------------------
[6.634 ms ; 6.787 ms) | @@@
[6.787 ms ; 7.036 ms) | @@@@@@@@@
[7.036 ms ; 7.188 ms) | @@@@@@@
[7.188 ms ; 7.338 ms) | @
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=10000001, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 7.492 ms, StdErr = 0.036 ms (0.48%), N = 16, StdDev = 0.145 ms
Min = 7.273 ms, Q1 = 7.398 ms, Median = 7.458 ms, Q3 = 7.602 ms, Max = 7.766 ms
IQR = 0.204 ms, LowerFence = 7.093 ms, UpperFence = 7.907 ms
ConfidenceInterval = [7.345 ms; 7.640 ms] (CI 99.9%), Margin = 0.147 ms (1.97% of Mean)
Skewness = 0.31, Kurtosis = 1.87, MValue = 2
-------------------- Histogram --------------------
[7.198 ms ; 7.360 ms) | @@@
[7.360 ms ; 7.571 ms) | @@@@@@@@
[7.571 ms ; 7.841 ms) | @@@@@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=10000001, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 11.321 ms, StdErr = 0.056 ms (0.49%), N = 20, StdDev = 0.249 ms
Min = 10.684 ms, Q1 = 11.205 ms, Median = 11.274 ms, Q3 = 11.449 ms, Max = 11.709 ms
IQR = 0.244 ms, LowerFence = 10.838 ms, UpperFence = 11.815 ms
ConfidenceInterval = [11.105 ms; 11.537 ms] (CI 99.9%), Margin = 0.216 ms (1.91% of Mean)
Skewness = -0.43, Kurtosis = 3.09, MValue = 2
-------------------- Histogram --------------------
[10.563 ms ; 10.804 ms) | @
[10.804 ms ; 11.201 ms) | @@@@
[11.201 ms ; 11.442 ms) | @@@@@@@@@@
[11.442 ms ; 11.722 ms) | @@@@@
---------------------------------------------------

SmallOnLargeTableBenchmarkEfService.EfCoreRangeAdd: Job-AOFWSP(WarmupCount=0) [InitialTableSize=10000001, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 12.194 ms, StdErr = 0.068 ms (0.56%), N = 41, StdDev = 0.434 ms
Min = 11.650 ms, Q1 = 11.822 ms, Median = 12.084 ms, Q3 = 12.506 ms, Max = 13.472 ms
IQR = 0.684 ms, LowerFence = 10.795 ms, UpperFence = 13.533 ms
ConfidenceInterval = [11.953 ms; 12.435 ms] (CI 99.9%), Margin = 0.241 ms (1.97% of Mean)
Skewness = 0.81, Kurtosis = 3.03, MValue = 2
-------------------- Histogram --------------------
[11.638 ms ; 11.968 ms) | @@@@@@@@@@@@@@@@
[11.968 ms ; 12.336 ms) | @@@@@@@@@@@@
[12.336 ms ; 12.755 ms) | @@@@@@@@@
[12.755 ms ; 13.103 ms) | @@@
[13.103 ms ; 13.637 ms) | @
---------------------------------------------------
```

## Bulk inserts on already large table
Table consisting of millions of rows receives bulk inserts of 1000s of items.
- `InitialTableSize` - number of rows in the table before the benchmark of inserts start
- `BulkAddSize` - number of rows added in each iteration in bulk

| Method        | InitialTableSize | BulkAddSize | BenchmarkDbType   | Mean       | Error      | StdDev     |
|-------------- |----------------- |------------ |------------------ |-----------:|-----------:|-----------:|
| EfCoreBulkAdd | 3000001          | 1000        | MsSql             |  19.032 ms |  0.3575 ms |  0.3169 ms |
| EfCoreBulkAdd | 3000001          | 1000        | PostgreSqlWindows |   8.245 ms |  0.1645 ms |  0.3846 ms |
| EfCoreBulkAdd | 3000001          | 1000        | PostgreSqlLinux   |  11.531 ms |  0.3244 ms |  0.9411 ms |
| EfCoreBulkAdd | 3000001          | 1000        | PostgreSqlCitus   |  11.290 ms |  0.2244 ms |  0.5989 ms |
| EfCoreBulkAdd | 3000001          | 20000       | MsSql             | 190.907 ms |  3.1599 ms |  4.2184 ms |
| EfCoreBulkAdd | 3000001          | 20000       | PostgreSqlWindows | 135.502 ms | 10.1063 ms | 29.3201 ms |
| EfCoreBulkAdd | 3000001          | 20000       | PostgreSqlLinux   | 111.166 ms |  5.4028 ms | 15.6744 ms |
| EfCoreBulkAdd | 3000001          | 20000       | PostgreSqlCitus   | 110.332 ms |  5.2428 ms | 15.0425 ms |
| EfCoreBulkAdd | 10000001         | 1000        | MsSql             |  19.573 ms |  0.3914 ms |  0.5613 ms |
| EfCoreBulkAdd | 10000001         | 1000        | PostgreSqlWindows |  18.980 ms |  0.3796 ms |  0.6445 ms |
| EfCoreBulkAdd | 10000001         | 1000        | PostgreSqlLinux   |  18.358 ms |  0.5065 ms |  1.4451 ms |
| EfCoreBulkAdd | 10000001         | 1000        | PostgreSqlCitus   |  18.780 ms |  0.3727 ms |  0.9884 ms |
| EfCoreBulkAdd | 10000001         | 20000       | MsSql             | 198.287 ms |  2.8690 ms |  2.5433 ms |
| EfCoreBulkAdd | 10000001         | 20000       | PostgreSqlWindows | 305.107 ms |  5.9458 ms |  7.3020 ms |
| EfCoreBulkAdd | 10000001         | 20000       | PostgreSqlLinux   | 173.434 ms |  3.4311 ms |  7.2373 ms |
| EfCoreBulkAdd | 10000001         | 20000       | PostgreSqlCitus   | 176.827 ms |  3.4869 ms |  6.2875 ms |

```
BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 19.032 ms, StdErr = 0.085 ms (0.44%), N = 14, StdDev = 0.317 ms
Min = 18.518 ms, Q1 = 18.812 ms, Median = 18.997 ms, Q3 = 19.292 ms, Max = 19.651 ms
IQR = 0.480 ms, LowerFence = 18.091 ms, UpperFence = 20.013 ms
ConfidenceInterval = [18.675 ms; 19.390 ms] (CI 99.9%), Margin = 0.357 ms (1.88% of Mean)
Skewness = 0.2, Kurtosis = 1.99, MValue = 2
-------------------- Histogram --------------------
[18.346 ms ; 18.755 ms) | @@
[18.755 ms ; 19.824 ms) | @@@@@@@@@@@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 8.245 ms, StdErr = 0.048 ms (0.58%), N = 65, StdDev = 0.385 ms
Min = 7.389 ms, Q1 = 7.967 ms, Median = 8.204 ms, Q3 = 8.469 ms, Max = 9.344 ms
IQR = 0.502 ms, LowerFence = 7.214 ms, UpperFence = 9.221 ms
ConfidenceInterval = [8.081 ms; 8.410 ms] (CI 99.9%), Margin = 0.165 ms (2.00% of Mean)
Skewness = 0.55, Kurtosis = 3.33, MValue = 2.86
-------------------- Histogram --------------------
[7.367 ms ; 7.615 ms) | @@
[7.615 ms ; 7.876 ms) | @@@@@@
[7.876 ms ; 8.127 ms) | @@@@@@@@@@@@@@@@@@@@@@@
[8.127 ms ; 8.360 ms) | @@@@@@@
[8.360 ms ; 8.645 ms) | @@@@@@@@@@@@@@@@@@
[8.645 ms ; 8.896 ms) | @@@@@@@
[8.896 ms ; 9.210 ms) |
[9.210 ms ; 9.470 ms) | @@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 11.531 ms, StdErr = 0.096 ms (0.83%), N = 97, StdDev = 0.941 ms
Min = 10.049 ms, Q1 = 10.859 ms, Median = 11.280 ms, Q3 = 11.970 ms, Max = 14.139 ms
IQR = 1.110 ms, LowerFence = 9.194 ms, UpperFence = 13.635 ms
ConfidenceInterval = [11.206 ms; 11.855 ms] (CI 99.9%), Margin = 0.324 ms (2.81% of Mean)
Skewness = 0.82, Kurtosis = 3.01, MValue = 3.5
-------------------- Histogram --------------------
[ 9.780 ms ; 10.159 ms) | @@
[10.159 ms ; 10.733 ms) | @@@@@@@@@@@@
[10.733 ms ; 11.452 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[11.452 ms ; 11.989 ms) | @@@@@@@@@@@@@@@@@@@@@@
[11.989 ms ; 12.350 ms) | @@@
[12.350 ms ; 12.888 ms) | @@@@@@@@@@@@@
[12.888 ms ; 13.518 ms) | @@@@
[13.518 ms ; 14.203 ms) | @@@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 11.290 ms, StdErr = 0.066 ms (0.58%), N = 83, StdDev = 0.599 ms
Min = 10.327 ms, Q1 = 10.847 ms, Median = 11.117 ms, Q3 = 11.677 ms, Max = 13.186 ms
IQR = 0.830 ms, LowerFence = 9.602 ms, UpperFence = 12.921 ms
ConfidenceInterval = [11.066 ms; 11.514 ms] (CI 99.9%), Margin = 0.224 ms (1.99% of Mean)
Skewness = 0.76, Kurtosis = 3.32, MValue = 2.59
-------------------- Histogram --------------------
[10.147 ms ; 10.695 ms) | @@@@@@@@@@
[10.695 ms ; 11.146 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[11.146 ms ; 11.376 ms) | @@@@
[11.376 ms ; 11.736 ms) | @@@@@@@@@@@@@@@@@
[11.736 ms ; 12.117 ms) | @@@@@@@@@@
[12.117 ms ; 12.464 ms) | @@@@@
[12.464 ms ; 12.871 ms) | @
[12.871 ms ; 13.232 ms) | @@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 190.907 ms, StdErr = 0.844 ms (0.44%), N = 25, StdDev = 4.218 ms
Min = 185.073 ms, Q1 = 187.915 ms, Median = 189.726 ms, Q3 = 192.772 ms, Max = 201.994 ms
IQR = 4.857 ms, LowerFence = 180.630 ms, UpperFence = 200.057 ms
ConfidenceInterval = [187.747 ms; 194.067 ms] (CI 99.9%), Margin = 3.160 ms (1.66% of Mean)
Skewness = 1.19, Kurtosis = 3.82, MValue = 2
-------------------- Histogram --------------------
[183.180 ms ; 190.470 ms) | @@@@@@@@@@@@@@@
[190.470 ms ; 195.027 ms) | @@@@@@@
[195.027 ms ; 199.642 ms) | @
[199.642 ms ; 203.887 ms) | @@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 135.502 ms, StdErr = 2.977 ms (2.20%), N = 97, StdDev = 29.320 ms
Min = 84.700 ms, Q1 = 112.770 ms, Median = 139.428 ms, Q3 = 159.356 ms, Max = 194.012 ms
IQR = 46.586 ms, LowerFence = 42.891 ms, UpperFence = 229.235 ms
ConfidenceInterval = [125.396 ms; 145.609 ms] (CI 99.9%), Margin = 10.106 ms (7.46% of Mean)
Skewness = -0, Kurtosis = 1.87, MValue = 3.23
-------------------- Histogram --------------------
[ 83.553 ms ; 100.304 ms) | @@@@@@@@@@@@@@@
[100.304 ms ; 120.946 ms) | @@@@@@@@@@@@@@@@@@@@@
[120.946 ms ; 138.002 ms) | @@@@@@@@@@
[138.002 ms ; 154.753 ms) | @@@@@@@@@@@@@@@@@@@@@@@@
[154.753 ms ; 175.629 ms) | @@@@@@@@@@@@@@@@@@@
[175.629 ms ; 190.286 ms) | @@@@@@@
[190.286 ms ; 202.388 ms) | @
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 111.166 ms, StdErr = 1.591 ms (1.43%), N = 97, StdDev = 15.674 ms
Min = 87.838 ms, Q1 = 98.190 ms, Median = 107.132 ms, Q3 = 125.043 ms, Max = 163.679 ms
IQR = 26.853 ms, LowerFence = 57.911 ms, UpperFence = 165.323 ms
ConfidenceInterval = [105.763 ms; 116.569 ms] (CI 99.9%), Margin = 5.403 ms (4.86% of Mean)
Skewness = 0.68, Kurtosis = 3.1, MValue = 2.6
-------------------- Histogram --------------------
[ 83.361 ms ;  93.473 ms) | @@@@@@@@@
[ 93.473 ms ; 102.428 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[102.428 ms ; 111.883 ms) | @@@@@@@@@@@@@@
[111.883 ms ; 122.816 ms) | @@@@@@@@@@@@@@@@@@
[122.816 ms ; 133.818 ms) | @@@@@@@@@@@@@@@@@@@@@
[133.818 ms ; 139.734 ms) | @@
[139.734 ms ; 148.751 ms) | @
[148.751 ms ; 154.745 ms) |
[154.745 ms ; 163.700 ms) | @@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=3000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 110.332 ms, StdErr = 1.543 ms (1.40%), N = 95, StdDev = 15.043 ms
Min = 86.265 ms, Q1 = 97.906 ms, Median = 108.601 ms, Q3 = 121.851 ms, Max = 155.050 ms
IQR = 23.945 ms, LowerFence = 61.989 ms, UpperFence = 157.769 ms
ConfidenceInterval = [105.089 ms; 115.575 ms] (CI 99.9%), Margin = 5.243 ms (4.75% of Mean)
Skewness = 0.38, Kurtosis = 2.32, MValue = 5.26
-------------------- Histogram --------------------
[ 81.938 ms ;  90.691 ms) | @@@@
[ 90.691 ms ;  99.344 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@
[ 99.344 ms ; 104.099 ms) | @@@
[104.099 ms ; 112.753 ms) | @@@@@@@@@@@@@@@@@@@@@
[112.753 ms ; 117.039 ms) | @@@@@@
[117.039 ms ; 125.693 ms) | @@@@@@@@@@@@@@@@@@
[125.693 ms ; 129.128 ms) | @
[129.128 ms ; 137.782 ms) | @@@@@@@@@@@@@@
[137.782 ms ; 146.436 ms) |
[146.436 ms ; 150.723 ms) |
[150.723 ms ; 159.377 ms) | @
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 19.573 ms, StdErr = 0.106 ms (0.54%), N = 28, StdDev = 0.561 ms
Min = 18.718 ms, Q1 = 19.259 ms, Median = 19.383 ms, Q3 = 19.680 ms, Max = 21.008 ms
IQR = 0.422 ms, LowerFence = 18.626 ms, UpperFence = 20.313 ms
ConfidenceInterval = [19.182 ms; 19.965 ms] (CI 99.9%), Margin = 0.391 ms (2.00% of Mean)
Skewness = 0.97, Kurtosis = 2.93, MValue = 2
-------------------- Histogram --------------------
[18.639 ms ; 19.147 ms) | @@@@
[19.147 ms ; 19.632 ms) | @@@@@@@@@@@@@@@@@
[19.632 ms ; 20.200 ms) | @
[20.200 ms ; 20.685 ms) | @@@@@
[20.685 ms ; 21.251 ms) | @
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 18.980 ms, StdErr = 0.106 ms (0.56%), N = 37, StdDev = 0.645 ms
Min = 17.392 ms, Q1 = 18.686 ms, Median = 18.925 ms, Q3 = 19.448 ms, Max = 20.068 ms
IQR = 0.761 ms, LowerFence = 17.545 ms, UpperFence = 20.589 ms
ConfidenceInterval = [18.601 ms; 19.360 ms] (CI 99.9%), Margin = 0.380 ms (2.00% of Mean)
Skewness = -0.35, Kurtosis = 2.69, MValue = 2
-------------------- Histogram --------------------
[17.261 ms ; 17.768 ms) | @@
[17.768 ms ; 18.507 ms) | @@@@@@
[18.507 ms ; 19.176 ms) | @@@@@@@@@@@@@@@@
[19.176 ms ; 19.740 ms) | @@@@@@@@@
[19.740 ms ; 20.322 ms) | @@@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 18.358 ms, StdErr = 0.149 ms (0.81%), N = 94, StdDev = 1.445 ms
Min = 16.230 ms, Q1 = 17.308 ms, Median = 18.072 ms, Q3 = 19.186 ms, Max = 22.125 ms
IQR = 1.878 ms, LowerFence = 14.491 ms, UpperFence = 22.002 ms
ConfidenceInterval = [17.851 ms; 18.864 ms] (CI 99.9%), Margin = 0.507 ms (2.76% of Mean)
Skewness = 0.93, Kurtosis = 3, MValue = 2.07
-------------------- Histogram --------------------
[16.051 ms ; 16.816 ms) | @@@@@@@
[16.816 ms ; 17.650 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
[17.650 ms ; 18.556 ms) | @@@@@@@@@@@@@@@@@@@@@@@@@@@
[18.556 ms ; 19.638 ms) | @@@@@@@@@@@@@@
[19.638 ms ; 20.638 ms) | @@@@@@
[20.638 ms ; 21.719 ms) | @@@@@@@@
[21.719 ms ; 22.542 ms) | @@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=1000, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 18.780 ms, StdErr = 0.109 ms (0.58%), N = 82, StdDev = 0.988 ms
Min = 16.506 ms, Q1 = 18.230 ms, Median = 18.785 ms, Q3 = 19.343 ms, Max = 21.244 ms
IQR = 1.113 ms, LowerFence = 16.561 ms, UpperFence = 21.012 ms
ConfidenceInterval = [18.407 ms; 19.152 ms] (CI 99.9%), Margin = 0.373 ms (1.98% of Mean)
Skewness = -0.02, Kurtosis = 2.88, MValue = 2
-------------------- Histogram --------------------
[16.408 ms ; 17.071 ms) | @@@@
[17.071 ms ; 17.666 ms) | @@@@@@
[17.666 ms ; 18.282 ms) | @@@@@@@@@@@
[18.282 ms ; 18.879 ms) | @@@@@@@@@@@@@@@@@@@@@@@@
[18.879 ms ; 19.640 ms) | @@@@@@@@@@@@@@@@@@@@@@@@
[19.640 ms ; 20.338 ms) | @@@@@@@@@@
[20.338 ms ; 20.782 ms) |
[20.782 ms ; 21.379 ms) | @@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=MsSql]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 198.287 ms, StdErr = 0.680 ms (0.34%), N = 14, StdDev = 2.543 ms
Min = 194.719 ms, Q1 = 196.448 ms, Median = 197.505 ms, Q3 = 199.997 ms, Max = 204.413 ms
IQR = 3.549 ms, LowerFence = 191.125 ms, UpperFence = 205.320 ms
ConfidenceInterval = [195.418 ms; 201.156 ms] (CI 99.9%), Margin = 2.869 ms (1.45% of Mean)
Skewness = 0.78, Kurtosis = 2.91, MValue = 2
-------------------- Histogram --------------------
[193.334 ms ; 199.265 ms) | @@@@@@@@@
[199.265 ms ; 205.798 ms) | @@@@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlWindows]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 305.107 ms, StdErr = 1.557 ms (0.51%), N = 22, StdDev = 7.302 ms
Min = 293.450 ms, Q1 = 299.806 ms, Median = 304.359 ms, Q3 = 309.528 ms, Max = 321.530 ms
IQR = 9.722 ms, LowerFence = 285.223 ms, UpperFence = 324.111 ms
ConfidenceInterval = [299.162 ms; 311.053 ms] (CI 99.9%), Margin = 5.946 ms (1.95% of Mean)
Skewness = 0.3, Kurtosis = 2.28, MValue = 2
-------------------- Histogram --------------------
[292.140 ms ; 298.280 ms) | @@@@
[298.280 ms ; 305.120 ms) | @@@@@@@@@
[305.120 ms ; 315.504 ms) | @@@@@@@@
[315.504 ms ; 324.950 ms) | @
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlLinux]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 173.434 ms, StdErr = 0.985 ms (0.57%), N = 54, StdDev = 7.237 ms
Min = 157.831 ms, Q1 = 167.465 ms, Median = 173.117 ms, Q3 = 178.401 ms, Max = 188.747 ms
IQR = 10.937 ms, LowerFence = 151.060 ms, UpperFence = 194.806 ms
ConfidenceInterval = [170.003 ms; 176.865 ms] (CI 99.9%), Margin = 3.431 ms (1.98% of Mean)
Skewness = 0.14, Kurtosis = 2.34, MValue = 2
-------------------- Histogram --------------------
[155.318 ms ; 159.824 ms) | @
[159.824 ms ; 164.632 ms) | @@@@
[164.632 ms ; 170.096 ms) | @@@@@@@@@@@@@
[170.096 ms ; 175.122 ms) | @@@@@@@@@@@@@@@
[175.122 ms ; 182.161 ms) | @@@@@@@@@@@@@@@@
[182.161 ms ; 189.043 ms) | @@@@@
---------------------------------------------------

BigOnLargeTableBenchmarkEfService.EfCoreBulkAdd: Job-MRSSUO(InvocationCount=1, UnrollFactor=1, WarmupCount=0) [InitialTableSize=10000001, BulkAddSize=20000, BenchmarkDbType=PostgreSqlCitus]
Runtime = .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2; GC = Concurrent Workstation
Mean = 176.827 ms, StdErr = 0.982 ms (0.56%), N = 41, StdDev = 6.288 ms
Min = 160.084 ms, Q1 = 173.298 ms, Median = 177.584 ms, Q3 = 180.199 ms, Max = 188.136 ms
IQR = 6.901 ms, LowerFence = 162.946 ms, UpperFence = 190.551 ms
ConfidenceInterval = [173.340 ms; 180.314 ms] (CI 99.9%), Margin = 3.487 ms (1.97% of Mean)
Skewness = -0.38, Kurtosis = 2.92, MValue = 2
-------------------- Histogram --------------------
[157.690 ms ; 163.332 ms) | @
[163.332 ms ; 168.578 ms) | @@@
[168.578 ms ; 174.156 ms) | @@@@@@@
[174.156 ms ; 178.943 ms) | @@@@@@@@@@@@@@@@@
[178.943 ms ; 184.841 ms) | @@@@@@@@@
[184.841 ms ; 190.529 ms) | @@@@
---------------------------------------------------
```
