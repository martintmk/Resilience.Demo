namespace Resilience;

public interface IResilienceStrategyBuilder
{
    ResilienceStrategyBuilderProperties Properties { get; set; }

    IResilienceStrategyBuilder AddStrategy(IResilienceStrategy strategy, ResilienceStrategyProperties? properties = null);

    IResilienceStrategyBuilder AddStrategy(Func<ResilienceStrategyBuilderContext, IResilienceStrategy> factory, ResilienceStrategyProperties? properties = null);

    IResilienceStrategy Create(ResilienceStrategyInstanceProperties? properties = null);
}
