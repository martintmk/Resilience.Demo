﻿using BenchmarkDotNet.Attributes;

namespace Resilience.Strategies.Bench.Benchmarks;

public class SimplePipelineBench
{
    private object? _strategy;

    [GlobalSetup]
    public void Setup()
    {
        _strategy = Helper.CraeteSimplePipeline(Technology);
    }

    [Params(ResilienceTechnology.ResiliencePrototype, ResilienceTechnology.Polly)]
    public ResilienceTechnology Technology { get; set; }

    [Benchmark]
    public ValueTask ExecuteSimplePipeline() => _strategy!.ExecuteAsync(Technology);
}
