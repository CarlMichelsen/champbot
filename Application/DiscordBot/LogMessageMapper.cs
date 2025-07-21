using Discord;
using Microsoft.Extensions.Logging;

namespace Application.DiscordBot;

public static class LogMessageMapper
{
    private static readonly Dictionary<LogSeverity, LogLevel> SeverityMap = new()
    {
        { LogSeverity.Critical, LogLevel.Critical },
        { LogSeverity.Error, LogLevel.Error },
        { LogSeverity.Warning, LogLevel.Warning },
        { LogSeverity.Info, LogLevel.Information },
        { LogSeverity.Verbose, LogLevel.Debug },
        { LogSeverity.Debug, LogLevel.Trace },
    };

    public static void Log(this ILogger logger, LogMessage logMessage)
    {
        var logLevel = SeverityMap.GetValueOrDefault(logMessage.Severity, LogLevel.Information);
        logger.Log(
            logLevel,
            logMessage.Exception,
            "[{Source}] {Message}",
            logMessage.Source,
            logMessage.Message);
    }
}