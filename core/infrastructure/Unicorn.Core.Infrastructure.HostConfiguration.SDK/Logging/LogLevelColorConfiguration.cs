using Microsoft.Extensions.Logging;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Logging;

internal static class LogLevelColorConfiguration
{
    public static int EventId { get; set; }

    public static Dictionary<LogLevel, ConsoleColor> LogLevels { get; } = new ()
    {
        [LogLevel.Trace] = ConsoleColor.DarkGray,
        [LogLevel.Debug] = ConsoleColor.Gray,
        [LogLevel.Information] = ConsoleColor.White,
        [LogLevel.Warning] = ConsoleColor.Yellow,
        [LogLevel.Error] = ConsoleColor.Red,
    };
}
