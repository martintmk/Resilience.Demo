using System.Collections.Concurrent;
using Resilience.DependencyInjection;

namespace Resilience.DependencyInjection.Internals;

internal class ResilienceStrategyProvider : IResilienceStrategyProvider
{
    private readonly ResilienceStrategyFactory _factory;
    private readonly ConcurrentDictionary<string, IResilienceStrategy> _pipelines = new();

    public ResilienceStrategyProvider(ResilienceStrategyFactory factory)
    {
        _factory = factory;
    }

    public IResilienceStrategy GetResilienceStrategy(string strategyName)
    {
        return _pipelines.GetOrAdd(strategyName, _ => _factory.CreateResilienceStrategy(strategyName, new ResilienceStrategyInstanceProperties()));
    }

    public IResilienceStrategy GetResilienceStrategy(string strategyName, ResilienceStrategyInstanceProperties properties) => throw new NotImplementedException();
}
