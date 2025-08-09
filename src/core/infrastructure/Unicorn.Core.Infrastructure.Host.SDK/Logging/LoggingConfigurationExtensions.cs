﻿using Microsoft.Extensions.Logging;

namespace Unicorn.Core.Infrastructure.Host.SDK.Logging;

public static class LoggingConfigurationExtensions
{
    public static void ConfigureLogging(this ILoggingBuilder builder)
    {
        builder.ClearProviders();

        builder.AddDebug();
        builder.AddApplicationInsights();
        builder.AddProvider(new UnicornConsoleLoggerProvider());
    }
}
