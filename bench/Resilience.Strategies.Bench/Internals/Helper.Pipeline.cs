using Polly;

namespace Resilience.Strategies.Bench;

internal static partial class Helper
{
    public static object CreatePipeline(ResilienceTechnology technology, int count)
    {
        return technology switch
        {
            ResilienceTechnology.Polly => Policy.WrapAsync(Enumerable.Repeat(0, count).Select(_ => Policy.NoOpAsync<int>()).ToArray()),

            ResilienceTechnology.ResiliencePrototype => CreateStrategy(builder =>
            {
                for (int i = 0; i < count; i++)
                {
                    builder.AddStrategy(new EmptyResilienceStrategy());
                }
            }),
            _ => throw new NotImplementedException()
        };
    }
}
