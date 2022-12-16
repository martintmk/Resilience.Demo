using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Resilience.Internals;

namespace Resilience;

public static class ResilienceServiceCollectionExtensions
{
    public static IResilienceStrategyBuilder AddResilienceStrategy(this IServiceCollection services, string name)
    {
        services.TryAddSingleton<IResilienceStrategyProvider, ResilienceStrategyProvider>();
        services.TryAddSingleton<ResilienceStrategyFactory>();

        return new ResilienceStrategyBuilder(services, name);
    }
}
