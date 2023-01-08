namespace Resilience;

public class ResilienceStrategyBuilderContext
{
    public ResilienceStrategyBuilderContext(
        ResilienceStrategyBuilderProperties builderProperties,
        ResilienceStrategyProperties strategyProperties,
        ResilienceStrategyInstanceProperties instanceProperties)
    {
        BuilderProperties = builderProperties;
        StrategyProperties = strategyProperties;
        InstanceProperties = instanceProperties;
    }

    public ResilienceStrategyBuilderProperties BuilderProperties { get; }

    public ResilienceStrategyProperties StrategyProperties { get; }

    public ResilienceStrategyInstanceProperties InstanceProperties { get; }
}
