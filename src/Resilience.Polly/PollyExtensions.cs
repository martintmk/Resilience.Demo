using Polly;
using Resilience.Polly.Internals;

namespace Resilience.Polly;

public static class PollyExtensions
{
    // IResilienceStrategy -> Polly extensions

    public static IAsyncPolicy AsAsyncPolicy(this IResilienceStrategy strategy) => new StrategyAsyncPolicy(strategy);

    public static IAsyncPolicy<T> AsAsyncPolicy<T>(this IResilienceStrategy strategy) => new StrategyAsyncPolicy<T>(strategy);

    public static ISyncPolicy AsSyncPolicy(this IResilienceStrategy strategy) => new StrategySyncPolicy(strategy);

    public static ISyncPolicy<T> AsSyncPolicy<T>(this IResilienceStrategy strategy) => new StrategySyncPolicy<T>(strategy);

    // Polly -> IResilienceStrategy

    public static IResilienceStrategy AsResilienceStrategy(this IAsyncPolicy policy) => new AsyncPolicyStrategy(policy);

    public static IResilienceStrategy AsResilienceStrategy<T>(this IAsyncPolicy<T> policy) => new AsyncPolicyStrategy<T>(policy);

    public static ResilienceContext Update(this ResilienceContext context, Context pollyContext)
    {
        context.StrategyKey = pollyContext.PolicyKey;
        context.StrategyName = pollyContext.PolicyWrapKey;

        // TODO: more stuff later

        return context;
    }
}
