using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resilience;
using Resilience.DependencyInjection.Internals;

internal class ResilienceStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResilienceStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IResilienceStrategy CreateResilienceStrategy(string name, ResilienceStrategyInstanceProperties properties)
    {
        var actions = _serviceProvider.GetRequiredService<IOptionsMonitor<ResilienceStrategyFactoryOptions>>().Get(name).ConfigureActions;
        var builder = _serviceProvider.GetRequiredService<IResilienceStrategyBuilder>();

        foreach (var action in actions)
        {
            action.Invoke(_serviceProvider, builder);
        }

        return builder.Create(properties);
    }
}
