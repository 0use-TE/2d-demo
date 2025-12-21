using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GameDevTools.Extensions;

/// <summary>
/// 顺序不可以随意修改！
/// </summary>
public enum E_LogArea
{
    LogFilter,
    FileIO
}

public static class LoggerExtensions
{
    /// <summary>
    /// 核心方法：带 Area 的通用日志
    /// </summary>
    public static void LogWithArea(
        this ILogger logger,
        LogLevel logLevel,
        E_LogArea area,
        string message,
        params object[] args)
    {
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["Area"] = area.ToString()
        }))
        {
            logger.Log(logLevel, message, args);
        }
    }

    public static void LogTraceWithArea(this ILogger logger, E_LogArea area, string message, params object[] args)
        => logger.LogWithArea(LogLevel.Trace, area, message, args);

    public static void LogDebugWithArea(this ILogger logger, E_LogArea area, string message, params object[] args)
        => logger.LogWithArea(LogLevel.Debug, area, message, args);

    public static void LogInformationWithArea(this ILogger logger, E_LogArea area, string message, params object[] args)
        => logger.LogWithArea(LogLevel.Information, area, message, args);

    public static void LogWarningWithArea(this ILogger logger, E_LogArea area, string message, params object[] args)
        => logger.LogWithArea(LogLevel.Warning, area, message, args);

    public static void LogErrorWithArea(this ILogger logger, E_LogArea area, Exception exception, string message, params object[] args)
    {
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["Area"] = area.ToString()
        }))
        {
            logger.LogError(exception, message, args);
        }
    }

    public static void LogCriticalWithArea(this ILogger logger, E_LogArea area, string message, params object[] args)
        => logger.LogWithArea(LogLevel.Critical, area, message, args);
}
