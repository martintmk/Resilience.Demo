using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Resilience;

public class ResilienceStrategyBuilderProperties
{
    public string StrategyName { get; set; } = string.Empty;

    public ILoggerFactory LoggerFactory { get; set; } = NullLoggerFactory.Instance;
}
