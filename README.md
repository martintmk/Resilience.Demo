# Introduction

The resilience demo is a proof of concept (POC) showcase of how the unified and non-allocating resilience API can look like.

## API

At the heart of the POC is the `IResilienceStrategy` interface that is responsible for execution of user code. It's one interface that handles all Polly scenarios:

- `ISyncPolicy`
- `IAsyncPolicy`
- `ISyncPolicy<T>`
- `IAsyncPolicy<T>`

``` csharp
public interface IResilienceStrategy
{
    ValueTask<T> ExecuteAsync<T, TState>(Func<ResilienceContext, TState, ValueTask<T>> execution, ResilienceContext context, TState state);
}
```

The `ResilienceContext` is defined as:

``` csharp
public class ResilienceContext
{
    public CancellationToken CancellationToken { get; set; }

    public bool IsSynchronous { get; set; }

    public bool IsVoid { get; set; }

    public bool ContinueOnCapturedContext { get; set; }

    // omitted for simplicity
}
```

The `IResilienceStrategy` unifies the 4 different policies used in Polly. User actions are executed under a single API. The are many extension
methods for this interface that cover different scenarios:

- Synchronous void methods.
- Synchronous methods with result.
- Asynchronous void methods.
- Asynchronous methods with result.

For example, synchronous `Execute` extension:

``` csharp
public static void Execute(this IResilienceStrategy strategy, Action execute)
{
    var context = ResilienceContext.Get();
    context.IsSynchronous = true;
    context.IsVoid = true;

    try
    {
        strategy.ExecuteAsync(static (context, state) =>
        {
            state();
            return new ValueTask<VoidResult>(VoidResult.Instance);
        }, 
        context, execute).GetAwaiter().GetResult();
    }
    finally
    {
        ResilienceContext.Return(context);
    }
}
```

In the preceding example:

- We rent `ResilienceContext` from pool.
- We store the information about the execution mode by setting the `IsSynchronous` and `IsVoid` properties to the context.
- We pass the user delegate, and use the `State` to avoid closure allocation.
- We block the execution.
- We return `ResilienceContext` to the pool.

Underlying implementation decides how to execute this delegate by reading the `ResilienceContext`:

``` csharp
internal class DelayStrategy : DelegatingResilienceStrategy
{
    public async override ValueTask<T> ExecuteAsync<T, TState>(Func<ResilienceContext, TState, ValueTask<T>> execution, ResilienceContext context, TState state)
    {
        if (context.IsSynchronous)
        {
            Thread.Sleep(1000);
        }
        else
        {
            await Task.Delay(1000);
        }

        return await execution(context, state);
    }
}
```

In the preceding example:

- For synchronous execution we are using `Thread.Sleep`.
- For asynchronous execution we are using `Task.Delay`.

This way, the responsibility of how to execute method is lifted from the user and instead passed to the policy. User knows only the `IResilienceStrategy` interface. User uses only a single strategy to execute all scenarios. Previously, user had to decide whether to use sync vs async, typed vs non-typed policies.

The life of extensibility author is also simplified as he only maintains one implementation of strategy instead of multiple ones. See the duplications in [`Polly.Retry`](https://github.com/App-vNext/Polly/tree/master/src/Polly/Retry).

### Creation and access to `IResilienceStrategy`

This API integrates natively with DI and exposes a single `AddResilienceStrategy` extension method that returns a `IResilienceStrategyBuilder`. You can chain various extensions for `IResilienceStrategyBuilder` and build a pipeline of strategies:

``` csharp
IServiceCollection services = ... ;

services
    .AddResilienceStrategy("my-strategy")
    .AddRetry()
    .AddCircuitBreaker()
    .AddTimeout(options => { ... });
```

To retrieve a resilience strategy:

``` csharp
IResilienceStrategyProvider provider = ... ;
IResilienceStrategy strategy = provider.GetResilienceStrategy("my-strategy");
```

### Extensibility

The resilience extensibility is simple. We just expose two `AddStrategy` extensions for `IResilienceStrategyBuilder`:

``` csharp
public interface IResilienceStrategyBuilder
{
    public string StrategyName { get; }

    public IServiceCollection Services { get; }
}

public class ResilienceStrategyBuilderContext
{
    public string StrategyName { get; }

    public string StrategyKey { get; }

    public IServiceProvider ServiceProvider { get; }
}

public static class ResilienceStrategyBuilderExtensions
{
    public static IResilienceStrategyBuilder AddStrategy(this IResilienceStrategyBuilder builder, IResilienceStrategy strategy);

    public static IResilienceStrategyBuilder AddStrategy(this IResilienceStrategyBuilder builder, Func<ResilienceStrategyBuilderContext, IResilienceStrategy> factory);
}
```

The extensibility author uses `AddStrategy` extension to add support for custom strategies. Use  `IServiceCollection` to register any custom services and `IServiceProvider` to resolve them from DI.

### Handling of different result types

The resilience strategy can handle many different result types and exceptions as retry strategy sample demonstrates:

``` csharp
var options = new RetryStrategyOptions();
options
    .ShouldRetry
    .Add<HttpResponseMessage>(m => m.StatusCode == HttpStatusCode.InternalServerError) // inspecting the result
    .Add(HttpStatusCode.InternalServerError) // particular value for other type
    .Add<MyResult>(v => v.IsError)
    .Add<MyResult>((v, context) => IsError(context)) // retrieve data from context for evaluation
    .AddException<InvalidOperationException>() // exceptions
    .AddException<HttpRequestMessageException>() // more exceptions
    .Add<MyResult>((v, context) => await IsErrorAsync(v, context)); // async predicates
```

In the preceding sample retry strategy handles 3 different types of results. This allows sharing of the retry strategy across the different result types. It is also possible to retrieve additional details from the `ResilienceContext` when handling the result.

### Packages

The POC exposes the following resilience packages:

- `Resilience.Abstractions`: contains `IResilienceStrategy` + extensions, `IResilienceStrategyProvider`.
- `Resilience`: contains implementations, `IResiliencePipelineBuilder` + extensions.
- `Resilience.Polly`: contains extensions and integration points with Polly.
- `Resilience.Strategies`: contains implementations of built-in strategies (retry, bulkhead, timeout, hedging).
