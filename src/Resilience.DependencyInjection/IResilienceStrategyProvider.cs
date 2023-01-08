namespace Resilience.DependencyInjection;

public interface IResilienceStrategyProvider
{
    IResilienceStrategy GetResilienceStrategy(string strategyName);

    IResilienceStrategy GetResilienceStrategy(string strategyName, ResilienceStrategyInstanceProperties properties);
}
