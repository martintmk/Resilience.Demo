namespace Resilience;

public interface IResilienceStrategyProvider
{
    IResilienceStrategy GetResilienceStrategy(string strategyName);

    IResilienceStrategy GetResilienceStrategy(string strategyName, string strategyKey);
}
