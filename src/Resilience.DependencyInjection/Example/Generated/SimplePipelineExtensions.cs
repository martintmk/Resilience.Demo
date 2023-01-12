// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resilience.Strategies.Retry;
using Resilience.Strategies.Timeout;

namespace Resilience.DependencyInjection.Example.Generated;

// Generated Code
internal static partial class SimplePipelineExtensions
{
    public static IServiceCollection AddSimplePipeline(this IServiceCollection services, Action<SimplePipelineOptions>? configure = null, IConfigurationSection? section = null)
    {
        var builder = services.AddOptions<SimplePipelineOptions>("SimplePipeline");

        if (configure != null)
        {
            builder.Configure(configure);
        }

        if (section != null)
        {
            builder.Bind(section);
        }

        services.AddResilienceStrategy("SimplePipeline", (serviceProvider, builder) =>
        {
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<SimplePipelineOptions>>().Get("SimplePipeline");

            builder.AddTimeout(options.TotalTimeout);
            builder.AddRetry(options.Retry);
            builder.AddTimeout(options.AttemptTimeout);
        });

        return services;
    }
}
