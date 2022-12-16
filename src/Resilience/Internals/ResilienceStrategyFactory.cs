using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resilience;
using Resilience.Internals;
using Resilience.Internals.Strategies;

internal class ResilienceStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResilienceStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IResilienceStrategy CreateResilienceStrategy(string name, string key)
    {
        var factories = _serviceProvider.GetRequiredService<IOptionsMonitor<ResilienceStrategyFactoryOptions>>().Get(name).Strategies;
        var strategies = new List<DelegatingResilienceStrategy>
        {
            new InitializeResilienceContextStrategy(name, key)
        };

        var context = new ResilienceStrategyBuilderContext(name, key, _serviceProvider);

        foreach (var factory in factories)
        {
            var strategy = factory(context);

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
}
