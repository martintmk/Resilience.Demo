namespace Resilience.Strategies.Timeout;

public class TimeoutStrategyOptions : DelegatingResilienceStrategy
{
    public TimeSpan TimeoutInterval { get; set; } = TimeSpan.FromSeconds(30);

    public Events<TimeoutTaskArguments> OnTimeout { get; set; } = new();
}
