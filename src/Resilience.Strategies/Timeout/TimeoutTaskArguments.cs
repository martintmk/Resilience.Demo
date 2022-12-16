namespace Resilience.Strategies.Timeout;

public readonly struct TimeoutTaskArguments
{
    public TimeoutTaskArguments(ResilienceContext context)
    {
        Context = context;
    }

    public ResilienceContext Context { get; }
}
