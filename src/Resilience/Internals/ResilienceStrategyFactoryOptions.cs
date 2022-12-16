namespace Resilience.Internals;

internal class ResilienceStrategyFactoryOptions
{
    public List<Func<ResilienceStrategyBuilderContext, IResilienceStrategy>> Strategies { get; } = new();
}
