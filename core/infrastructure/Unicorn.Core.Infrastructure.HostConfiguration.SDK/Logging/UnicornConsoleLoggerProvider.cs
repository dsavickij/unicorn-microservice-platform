using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Logging;

internal sealed class UnicornConsoleLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, UnicornConsoleLogger> _loggers = new ();

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, categoryName => new UnicornConsoleLogger(categoryName));

    public void Dispose() => _loggers.Clear();
}
