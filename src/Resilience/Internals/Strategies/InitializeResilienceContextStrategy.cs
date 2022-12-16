namespace Resilience.Internals.Strategies;

internal class InitializeResilienceContextStrategy : DelegatingResilienceStrategy
{
    private readonly string _pipelineName;
    private readonly string _pipelineKey;

    public InitializeResilienceContextStrategy(string pipelineName, string pipelineKey)
    {
        _pipelineName = pipelineName;
        _pipelineKey = pipelineKey;
    }

    public override async ValueTask<T> ExecuteAsync<T, TState>(Func<ResilienceContext, TState, ValueTask<T>> execution, ResilienceContext context, TState state)
    {
        var prevName = context.StrategyName;
        var prevKey = context.StrategyKey;


        context.StrategyName = _pipelineName;
        context.StrategyKey = _pipelineKey;

        try
        {
            return await base.ExecuteAsync(execution, context, state);
        }
        finally
        {
            context.StrategyName = prevName;
            context.StrategyKey = prevKey;
        }
    }

}
