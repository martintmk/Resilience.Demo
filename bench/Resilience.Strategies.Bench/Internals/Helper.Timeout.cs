using Resilience.Strategies.Timeout;
using Polly.Timeout;
using Polly;

namespace Resilience.Strategies.Bench;

internal static partial class Helper
{
    public static object CreateTimeout(ResilienceTechnology technology)
    {
        var timeout = TimeSpan.FromSeconds(30);

        return technology switch
        {
            ResilienceTechnology.Polly => Policy.TimeoutAsync(timeout, TimeoutStrategy.Optimistic, (_, _, _) => Task.CompletedTask).AsAsyncPolicy<int>(),

            ResilienceTechnology.ResiliencePrototype => CreateStrategy(builder =>
            {
                builder.AddTimeout("dummy", options =>
                {
                    options.TimeoutInterval = timeout;
                    options.OnTimeout.Add(args => new ValueTask());
                });
            }),
            _ => throw new NotImplementedException()
        };
    }
}
