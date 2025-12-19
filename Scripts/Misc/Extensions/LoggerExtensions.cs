using Godot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Misc.Extensions;

public static class LoggerExtensions
{
    // --- Trace / Debug: 用于开发期间的琐碎信息 ---
    public static void LogDebugWithNode(this ILogger logger, Node node, string content, [CallerMemberName] string member = "")
    {
        LogToAll(logger, LogLevel.Debug, node, content, member);
    }

    // --- Information: 重要状态变更 ---
    public static void LogInfoWithNode(this ILogger logger, Node node, string content, [CallerMemberName] string member = "")
    {
        LogToAll(logger, LogLevel.Information, node, content, member);
    }

    // --- Warning: 异常但不影响运行的情况 ---
    public static void LogWarningWithNode(this ILogger logger, Node node, string content, [CallerMemberName] string member = "")
    {
        string msg = FormatMessage("WARN", node, content, member);
        GD.PushWarning(msg); // 在 Godot 编辑器 Debugger 面板显示黄色警告
        logger.LogWarning(msg);
    }

    // --- Error: 导致逻辑中断的错误 ---
    public static void LogErrWithPush(this ILogger logger, Node node, string content, [CallerMemberName] string member = "")
    {
        string msg = FormatMessage("ERROR", node, content, member);
        GD.PushError(msg); // 在 Godot 编辑器 Debugger 面板显示红色错误
        logger.LogError(msg);
    }
    // --- Error: 导致逻辑中断的错误 ---
    public static void LogWaringWithPush(this ILogger logger, Node node, string content, [CallerMemberName] string member = "")
    {
        string msg = FormatMessage("Waring", node, content, member);
        GD.PushWarning(msg); // 在 Godot 编辑器 Debugger 面板显示黄色警告
        logger.LogWarning(msg);
    }

    // --- 核心格式化逻辑 ---
    private static void LogToAll(ILogger logger, LogLevel level, Node node, string content, string member)
    {
        string prefix = level.ToString().ToUpper();
        string msg = FormatMessage(prefix, node, content, member);
        logger.Log(level, msg);
    }

    private static string FormatMessage(string level, Node node, string content, string member)
    {
        string time = DateTime.Now.ToString("HH:mm:ss.fff");
        string nodeInfo = node != null
            ? $"[{node.Name} @ {node.GetPath()} (ID:{node.GetInstanceId()})]"
            : "[Null Node]";

        // 格式: [时间][等级][节点路径][方法名] | 信息
        return $"[{time}][{level}]{nodeInfo}[{member}] | {content}";
    }
}
