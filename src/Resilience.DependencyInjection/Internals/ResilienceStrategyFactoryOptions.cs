namespace Resilience.DependencyInjection.Internals;

internal class ResilienceStrategyFactoryOptions
{
    public List<Action<IServiceProvider, IResilienceStrategyBuilder>> ConfigureActions { get; } = new();
}
