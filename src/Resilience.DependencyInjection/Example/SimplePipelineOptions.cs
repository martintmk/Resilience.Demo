// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Resilience.Strategies.Retry;
using Resilience.Strategies.Timeout;

namespace Resilience.DependencyInjection.Example;

[ResilienceStrategy]
public class SimplePipelineOptions
{
    public TimeoutStrategyOptions TotalTimeout { get; set; } = new();

    public RetryStrategyOptions Retry { get; set; } = new();

    public TimeoutStrategyOptions AttemptTimeout { get; set; } = new();
}
