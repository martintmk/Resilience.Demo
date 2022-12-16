using Microsoft.Extensions.DependencyInjection;
using Resilience.Internals;

namespace Resilience;

public static class ResilienceStrategyBuilderExtensions
{
    public static IResilienceStrategyBuilder AddStrategy(this IResilienceStrategyBuilder builder, IResilienceStrategy strategy)
    {
        return builder.AddStrategy(_ => strategy);
    }

    public static IResilienceStrategyBuilder AddStrategy(this IResilienceStrategyBuilder builder, Func<ResilienceStrategyBuilderContext, IResilienceStrategy> factory)
    {
        builder.Services
            .AddOptions<ResilienceStrategyFactoryOptions>(builder.StrategyName)
            .Configure<IServiceProvider>((options, provider) => options.Strategies.Add(factory));

        return builder;
    }
}
