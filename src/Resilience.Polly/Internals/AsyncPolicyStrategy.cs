using Polly;

namespace Resilience.Polly.Internals;

internal class AsyncPolicyStrategy : DelegatingResilienceStrategy
{
    private readonly IAsyncPolicy _policy;

    public AsyncPolicyStrategy(IAsyncPolicy policy)
    {
        _policy = policy;
    }

    public override ValueTask<T> ExecuteAsync<T, TState>(Func<ResilienceContext, TState, ValueTask<T>> execution, ResilienceContext context, TState state)
    {
        return base.ExecuteAsync(async (context, state) =>
        {
            if (context.IsSynchronous)
            {
                throw new InvalidOperationException("This strategy does not allow synchronous executions because underlying Polly 'IAsyncPolicy' does not support it.");
            }

            var pollyContext = new Context();

            return await _policy.ExecuteAsync(async (c, t) => await execution(context, state), pollyContext, context.CancellationToken);
        },
        context,
        state);
    }
}
