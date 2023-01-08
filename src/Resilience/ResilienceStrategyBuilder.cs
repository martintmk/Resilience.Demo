using Resilience.Strategies;

namespace Resilience;

public class ResilienceStrategyBuilder : IResilienceStrategyBuilder
{
    private readonly List<Entry> _entries = new();

    public ResilienceStrategyBuilderProperties Properties { get; set; } = new();

    public IResilienceStrategyBuilder AddStrategy(IResilienceStrategy strategy, ResilienceStrategyProperties? properties = null) => AddStrategy(_ => strategy, properties);

    public IResilienceStrategyBuilder AddStrategy(Func<ResilienceStrategyBuilderContext, IResilienceStrategy> factory, ResilienceStrategyProperties? properties = null)
    {
        _entries.Add(new Entry(factory, properties ?? new ResilienceStrategyProperties()));

        return this;
    }

    public IResilienceStrategy Create(ResilienceStrategyInstanceProperties? properties = null)
    {
        properties ??= new ResilienceStrategyInstanceProperties();

        var strategies = new List<DelegatingResilienceStrategy>();

        foreach (var entry in _entries)
        {
            var context = new ResilienceStrategyBuilderContext(Properties, entry.Properties, properties);   
            var strategy = entry.Factory(context);

            if (strategy is DelegatingResilienceStrategy delegatingResilienceStrategy)
            {
                strategies.Add(delegatingResilienceStrategy);
            }
            else
            {
                strategies.Add(new DelegatingStrategyWrapper(strategy));
            }
        }

        for (int i = 0; i < strategies.Count - 1; i++)
        {
            strategies[i].Next = strategies[i + 1];
        }

        return strategies[0];
    }

    private record class Entry(Func<ResilienceStrategyBuilderContext, IResilienceStrategy> Factory, ResilienceStrategyProperties Properties);
}
