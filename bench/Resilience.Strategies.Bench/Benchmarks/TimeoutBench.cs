using BenchmarkDotNet.Attributes;

namespace Resilience.Strategies.Bench.Benchmarks;

public class TimeoutBench
{
    private object? _strategy;

    [GlobalSetup]
    public void Setup()
    {
        _strategy = Helper.CreateTimeout(Technology);
    }

    [Params(ResilienceTechnology.ResiliencePrototype, ResilienceTechnology.Polly)]
    public ResilienceTechnology Technology { get; set; }

    [Benchmark]
    public ValueTask ExecuteTimeout() => _strategy!.ExecuteAsync(Technology);
}
