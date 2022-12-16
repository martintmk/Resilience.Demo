namespace Resilience;

public sealed class NullResilienceStrategyProvider : IResilienceStrategyProvider
{
    public static readonly NullResilienceStrategyProvider Instance = new();

    private NullResilienceStrategyProvider()
    {
    }

    public IResilienceStrategy GetResilienceStrategy(string strategyName) => NullResilienceStrategy.Instance;

    public IResilienceStrategy GetResilienceStrategy(string strategyName, string strategyKey) => NullResilienceStrategy.Instance;
}
