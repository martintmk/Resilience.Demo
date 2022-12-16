namespace Resilience;

public class ResilienceStrategyBuilderContext
{
    internal ResilienceStrategyBuilderContext(string strategyName, string strategyKey, IServiceProvider serviceProvider)
    {
        StrategyName = strategyName;
        StrategyKey = strategyKey;
        ServiceProvider = serviceProvider;
    }

    public string StrategyName { get; }

    public string StrategyKey { get; }

    public IServiceProvider ServiceProvider { get; }
}
