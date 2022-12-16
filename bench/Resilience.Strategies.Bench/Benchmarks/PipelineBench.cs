using BenchmarkDotNet.Attributes;

namespace Resilience.Strategies.Bench.Benchmarks;

public class PipelineBench
{
    private object? _strategy;

    [GlobalSetup]
    public void Setup()
    {
        _strategy = Helper.CreatePipeline(Technology, Components);
    }

    [Params(2, 5, 10)]
    public int Components { get; set; }

    [Params(ResilienceTechnology.ResiliencePrototype, ResilienceTechnology.Polly)]
    public ResilienceTechnology Technology { get; set; }

    [Benchmark]
    public ValueTask ExecutePipeline() => _strategy!.ExecuteAsync(Technology);
}
