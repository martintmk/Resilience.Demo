namespace Resilience.Strategies.Timeout;

public static class TimeoutExtensions
{
    public static IResilienceStrategyBuilder AddTimeout(this IResilienceStrategyBuilder builder, Action< TimeoutStrategyOptions> configure, ResilienceStrategyProperties? properties = null)
    {
        var options = new TimeoutStrategyOptions();
        configure(options);

        return builder.AddTimeout(options, properties);
    }

    public static IResilienceStrategyBuilder AddTimeout(this IResilienceStrategyBuilder builder, TimeoutStrategyOptions options, ResilienceStrategyProperties? properties = null)
    {
        return builder.AddStrategy(context => new TimeoutStrategy(options), properties);
    }
}
