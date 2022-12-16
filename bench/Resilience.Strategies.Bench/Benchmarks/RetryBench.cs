using BenchmarkDotNet.Attributes;

namespace Resilience.Strategies.Bench.Benchmarks;

public class RetryBench
{
    private object? _strategy;

    [GlobalSetup]
    public void Setup()
    {
        _strategy = Helper.CreateRetries(Technology);
    }

    [Params(ResilienceTechnology.ResiliencePrototype, ResilienceTechnology.Polly)]
    public ResilienceTechnology Technology { get; set; }

    [Benchmark]
    public ValueTask ExecuteRetry() => _strategy!.ExecuteAsync(Technology);
}
