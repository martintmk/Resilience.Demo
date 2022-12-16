using Microsoft.Extensions.ObjectPool;

namespace Resilience;

public sealed class ResilienceContext
{
    private static readonly ObjectPool<ResilienceContext> s_pool = ObjectPool.Create<ResilienceContext>();

    public ResilienceContext()
    {
    }

    public CancellationToken CancellationToken { get; set; }

    public bool IsSynchronous { get; set; }

    public Type ResultType { get; set; } = typeof(VoidResult);

    public bool IsVoid { get; set; }

    public bool ContinueOnCapturedContext { get; set; }

    public string StrategyName { get; set; } = string.Empty;

    public string StrategyKey { get; set; } = string.Empty;

    public static ResilienceContext Get(CancellationToken cancellationToken = default)
    {
        var context = s_pool.Get();
        context.CancellationToken = cancellationToken;
        return context;
    }

    public static void Return(ResilienceContext context)
    {
        context.Reset();

        s_pool.Return(context);
    }

    private void Reset()
    {
        IsVoid = false;
        IsSynchronous = false;
        ContinueOnCapturedContext = false;
        StrategyKey = string.Empty;
        StrategyName = string.Empty;
    }
}
