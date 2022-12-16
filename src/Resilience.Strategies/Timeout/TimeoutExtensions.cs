using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Resilience.Strategies.Timeout;

public static class TimeoutExtensions
{
    public static IResilienceStrategyBuilder AddTimeout(this IResilienceStrategyBuilder builder, string strategyName)
    {
        return builder.AddTimeout(strategyName, _ => { });
    }

    public static IResilienceStrategyBuilder AddTimeout(this IResilienceStrategyBuilder builder, string strategyName, Action<TimeoutStrategyOptions> configure)
    {
        var name = builder.StrategyName + "-" + strategyName;

        _ = builder.Services.Configure(name, configure);

        return builder.AddStrategy(context => new TimeoutStrategy(context.ServiceProvider.GetRequiredService<IOptionsMonitor<TimeoutStrategyOptions>>().Get(name)));
    }
}
