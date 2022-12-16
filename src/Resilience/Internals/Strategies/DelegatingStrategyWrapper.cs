namespace Resilience.Internals.Strategies;

internal sealed class DelegatingStrategyWrapper : DelegatingResilienceStrategy
{
    private readonly IResilienceStrategy strategy;

    public DelegatingStrategyWrapper(IResilienceStrategy strategy) => this.strategy = strategy;

    public override ValueTask<T> ExecuteAsync<T, TState>(Func<ResilienceContext, TState, ValueTask<T>> execution, ResilienceContext context, TState state)
    {
        return strategy.ExecuteAsync(static (context, state) => state.Next.ExecuteAsync(state.execution, context, state.state), context, (Next, execution, state));
    }
}
