// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Polly;

namespace Resilience.Polly.Internals;

internal class AsyncPolicyStrategy<TResult> : DelegatingResilienceStrategy
{
    private readonly IAsyncPolicy<TResult> _policy;

    public AsyncPolicyStrategy(IAsyncPolicy<TResult> policy)
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

            if (context.ResultType != typeof(TResult))
            {
                throw new InvalidOperationException($"This strategy only supports executions that returns the '{typeof(TResult).Name}' results.");
            }


            var pollyContext = new Context();
            var result = (await _policy.ExecuteAsync(async (c, t) => (TResult)(object)await execution(context, state), pollyContext, context.CancellationToken))!;

            return (T)(object)result;
        },
        context,
        state);
    }
}

