using Polly;
using Resilience.Strategies.Retry;

namespace Resilience.Strategies.Bench;

internal static partial class Helper
{
    public static object CreateRetries(ResilienceTechnology technology)
    {
        var delay = TimeSpan.FromSeconds(10);

        return technology switch
        {
            ResilienceTechnology.Polly =>
                Policy
                    .HandleResult(10)
                    .Or<InvalidOperationException>()
                    .WaitAndRetryAsync(3, attempt => delay, (result, time) => Task.CompletedTask),

            ResilienceTechnology.ResiliencePrototype => CreateStrategy(builder =>
            {
                builder.AddRetry(options =>
                {
                    options.ShouldRetry
                        .Add(10)
                        .AddException<InvalidOperationException>();

                    options.RetryCount = 3;
                    options.RetryDelayGenerator = attempt => delay;
                    options.OnRetry.Add((args) => default(ValueTask));
                });
            }),
            _ => throw new NotImplementedException()
        };
    }
}
