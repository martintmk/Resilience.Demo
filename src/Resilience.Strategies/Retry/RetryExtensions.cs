using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Resilience.Strategies.Retry;

public static class RetryExtensions
{
    public static IResilienceStrategyBuilder AddRetry(this IResilienceStrategyBuilder builder) => builder.AddRetry("default");

    public static IResilienceStrategyBuilder AddRetry(this IResilienceStrategyBuilder builder, Action<RetryStrategyOptions> configure) => builder.AddRetry("default", configure);

    public static IResilienceStrategyBuilder AddRetry(this IResilienceStrategyBuilder builder, string strategyName) => builder.AddRetry(strategyName, _ => { });

    public static IResilienceStrategyBuilder AddRetry(this IResilienceStrategyBuilder builder, string strategyName, Action<RetryStrategyOptions> configure)
    {
        var name = builder.StrategyName + "-" + strategyName;

        _ = builder.Services.Configure(name, configure);

        return builder.AddStrategy(context => new RetryStrategy(context.ServiceProvider.GetRequiredService<IOptionsMonitor<RetryStrategyOptions>>().Get(name)));
    }
}
