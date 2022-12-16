using Microsoft.Extensions.DependencyInjection;

namespace Resilience.Internals;

internal class ResilienceStrategyBuilder : IResilienceStrategyBuilder
{
    public ResilienceStrategyBuilder(IServiceCollection services, string pipelineName)
    {
        Services = services;
        StrategyName = pipelineName;
    }

    public string StrategyName { get; }

    public IServiceCollection Services { get; }
}
