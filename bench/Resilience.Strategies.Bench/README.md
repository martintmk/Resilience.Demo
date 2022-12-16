# Benchmark results

```text
ResilienceStrategyFactoryBenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
Intel Core i9-10885H CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100-rc.2.22477.23
  [Host] : .NET 6.0.10 (6.0.1022.47605), X64 RyuJIT
```

## PIPELINES

|          Method | Components |          Technology |       Mean |    Error |   StdDev |     Median |   Gen0 | Allocated |
|---------------- |----------- |-------------------- |-----------:|---------:|---------:|-----------:|-------:|----------:|
| ExecutePipeline |          2 |               Polly |   322.3 ns |  1.90 ns |  2.72 ns |   322.0 ns | 0.0811 |     680 B |
| ExecutePipeline |          2 | ResiliencePrototype |   100.3 ns |  1.11 ns |  1.63 ns |   101.1 ns |      - |         - |
| ExecutePipeline |          5 |               Polly |   994.2 ns |  5.65 ns |  7.91 ns |   991.4 ns | 0.2460 |    2072 B |
| ExecutePipeline |          5 | ResiliencePrototype |   125.8 ns |  2.80 ns |  3.73 ns |   128.1 ns |      - |         - |
| ExecutePipeline |         10 |               Polly | 2,110.9 ns | 22.29 ns | 33.36 ns | 2,112.6 ns | 0.5226 |    4392 B |
| ExecutePipeline |         10 | ResiliencePrototype |   153.4 ns |  2.14 ns |  3.13 ns |   154.9 ns |      - |         - |

## RETRIES

|       Method |          Technology |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------- |-------------------- |---------:|---------:|---------:|-------:|----------:|
| ExecuteRetry |               Polly | 198.0 ns | 26.41 ns | 39.53 ns | 0.0496 |     416 B |
| ExecuteRetry | ResiliencePrototype | 299.7 ns | 12.81 ns | 18.77 ns |      - |         - |

## TIMEOUT

|         Method |          Technology |     Mean |    Error |   StdDev |   Median |   Gen0 | Allocated |
|--------------- |-------------------- |---------:|---------:|---------:|---------:|-------:|----------:|
| ExecuteTimeout |               Polly | 536.9 ns | 11.39 ns | 16.69 ns | 538.5 ns | 0.1230 |    1032 B |
| ExecuteTimeout | ResiliencePrototype | 215.9 ns |  2.96 ns |  4.15 ns | 213.6 ns |      - |         - |

## SIMPLE PIPELINE (Outer Timeout - Retries - Inner Timeout)

|                Method |          Technology |       Mean |    Error |   StdDev |   Gen0 | Allocated |
|---------------------- |-------------------- |-----------:|---------:|---------:|-------:|----------:|
| ExecuteSimplePipeline |               Polly | 2,348.7 ns | 247.3 ns | 346.7 ns | 0.3738 |    3136 B |
| ExecuteSimplePipeline | ResiliencePrototype |   902.1 ns | 114.7 ns | 171.7 ns |      - |         - |
