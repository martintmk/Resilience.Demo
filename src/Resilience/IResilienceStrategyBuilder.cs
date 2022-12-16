using Microsoft.Extensions.DependencyInjection;

namespace Resilience;

public interface IResilienceStrategyBuilder
{
    public string StrategyName { get; }

    public IServiceCollection Services { get; }
}
