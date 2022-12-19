using Polly;

namespace Resilience.Polly.Internals;

internal class StrategyAsyncPolicy : AsyncPolicy
{
    private readonly IResilienceStrategy _strategy;

    public StrategyAsyncPolicy(IResilienceStrategy strategy)
    {
        _strategy = strategy;
    }

    protected override async Task<TResult> ImplementationAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Context context,
        CancellationToken cancellationToken,
        bool continueOnCapturedContext)
    {
        var resilienceContext = ResilienceContext.Get(cancellationToken);
        resilienceContext.ContinueOnCapturedContext = continueOnCapturedContext;
        resilienceContext.IsSynchronous = false;
        resilienceContext.IsVoid = false;

        resilienceContext.Update(context);

        return await _strategy.ExecuteAsync(
            async (context, state) => await state.action(state.context, context.CancellationToken).ConfigureAwait(false),
            resilienceContext,
            (action, context));
    }
}
