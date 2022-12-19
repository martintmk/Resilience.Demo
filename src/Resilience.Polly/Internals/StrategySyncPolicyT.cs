using Polly;

namespace Resilience.Polly.Internals;

internal class StrategySyncPolicy<T> : Policy<T>, ISyncPolicy<T>
{
    private readonly IResilienceStrategy _strategy;

    public StrategySyncPolicy(IResilienceStrategy strategy)
    {
        _strategy = strategy;
    }

    protected override T Implementation(Func<Context, CancellationToken, T> action, Context context, CancellationToken cancellationToken)
    {
        var resilienceContext = ResilienceContext.Get(cancellationToken);
        resilienceContext.ContinueOnCapturedContext = true;
        resilienceContext.IsSynchronous = true;
        resilienceContext.IsVoid = false;
        resilienceContext.ResultType = typeof(T);

        resilienceContext.Update(context);

        return _strategy.ExecuteAsync(
            (context, state) =>
            {
                var result = state.action(state.context, context.CancellationToken);
                return new ValueTask<T>(result);
            },
            resilienceContext,
            (action, context)).GetAwaiter().GetResult();
    }
}
