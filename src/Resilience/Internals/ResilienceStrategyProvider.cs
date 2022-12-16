using System.Collections.Concurrent;

namespace Resilience.Internals;

internal class ResilienceStrategyProvider : IResilienceStrategyProvider
{
    private readonly ResilienceStrategyFactory _factory;
    private readonly ConcurrentDictionary<string, IResilienceStrategy> _pipelines = new();

    public ResilienceStrategyProvider(ResilienceStrategyFactory factory)
    {
        _factory = factory;
    }

    public IResilienceStrategy GetResilienceStrategy(string pipelineName)
    {
        return _pipelines.GetOrAdd(pipelineName, _ => _factory.CreateResilienceStrategy(pipelineName, string.Empty));
    }

    public IResilienceStrategy GetResilienceStrategy(string pipelineName, string pipelineKey) => throw new NotImplementedException();
}
