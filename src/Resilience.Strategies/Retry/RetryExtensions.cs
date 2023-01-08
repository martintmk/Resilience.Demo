namespace Resilience.Strategies.Retry;

public static class RetryExtensions
{
    public static IResilienceStrategyBuilder AddRetry(
        this IResilienceStrategyBuilder builder,
        Action<RetryStrategyOptions> configure,
        ResilienceStrategyProperties? properties = null)
    {
        var options = new RetryStrategyOptions();
        configure(options);

        return builder.AddRetry(options, properties);
    }

    public static IResilienceStrategyBuilder AddRetry(
        this IResilienceStrategyBuilder builder,
        RetryStrategyOptions options,
        ResilienceStrategyProperties? properties = null)
    {
        return builder.AddStrategy(context => new RetryStrategy(options), properties);
    }
}
