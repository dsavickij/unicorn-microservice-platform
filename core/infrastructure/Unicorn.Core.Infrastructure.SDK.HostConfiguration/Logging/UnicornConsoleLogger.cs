using Microsoft.Extensions.Logging;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.Logging;

internal class UnicornConsoleLogger : ILogger
{
    private readonly string _categoryName;

    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel) => LogLevelColorConfiguration.LogLevels.ContainsKey(logLevel);

    public UnicornConsoleLogger(string categoryName) => _categoryName = categoryName;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (LogLevelColorConfiguration.EventId == 0 || LogLevelColorConfiguration.EventId == eventId.Id)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = LogLevelColorConfiguration.LogLevels[logLevel];

            Console.Write($"[{TimeOnly.FromDateTime(DateTime.Now)}][{logLevel}][EventId: {eventId.Id}]");
            Console.WriteLine($"[{_categoryName}] {formatter(state, exception)}");

            Console.ForegroundColor = originalColor;
        }
    }
}
