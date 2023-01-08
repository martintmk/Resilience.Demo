using Resilience.Strategies.Retry;
using Resilience.Strategies.Timeout;
using Polly.Timeout;
using Polly;

namespace Resilience.Strategies.Bench;

internal static partial class Helper
{
    public static object CraeteSimplePipeline(ResilienceTechnology technology)
    {
        var delay = TimeSpan.FromSeconds(10);
        var innerTimeout = TimeSpan.FromSeconds(10);
        var outerTimeout = TimeSpan.FromSeconds(30);

        return technology switch
        {
            ResilienceTechnology.Polly =>
                Policy.WrapAsync(
                    Policy.TimeoutAsync(innerTimeout, TimeoutStrategy.Optimistic, (_, _, _) => Task.CompletedTask).AsAsyncPolicy<int>(),
                    Policy
                        .HandleResult(10)
                        .Or<InvalidOperationException>()
                        .WaitAndRetryAsync(3, attempt => delay, (result, time) => Task.CompletedTask),
                    Policy.TimeoutAsync(outerTimeout, TimeoutStrategy.Optimistic, (_, _, _) => Task.CompletedTask).AsAsyncPolicy<int>()),

            ResilienceTechnology.ResiliencePrototype => CreateStrategy(builder =>
            {
                builder.AddTimeout(
                    options =>
                    {
                        options.TimeoutInterval = outerTimeout;
                        options.OnTimeout.Add(args => new ValueTask());
                    },
                    new ResilienceStrategyProperties { StartegyName = "outer" });

                builder.AddRetry(
                    options =>
                    {
                        options.ShouldRetry
                            .Add(10)
                            .AddException<InvalidOperationException>();

                        options.RetryCount = 3;
                        options.RetryDelayGenerator = attempt => delay;
                        options.OnRetry.Add((args) => default(ValueTask));
                    },
                    new ResilienceStrategyProperties { StartegyName = "retries" });

                builder.AddTimeout(
                    options =>
                    {
                        options.TimeoutInterval = innerTimeout;
                        options.OnTimeout.Add(args => new ValueTask());
                    },
                    new ResilienceStrategyProperties { StartegyName = "inner" });
            }),
            _ => throw new NotImplementedException()
        };
    }
}
