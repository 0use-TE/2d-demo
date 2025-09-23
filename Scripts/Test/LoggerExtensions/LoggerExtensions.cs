using CharacterModule.BehaviourTree.Core;
using Godot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Test.LoggerExtensions
{
    public static class LoggerExtensions
    {
        public static void LogBehaviourTreeNodeInformation(this ILogger logger,BehaviourNode node,string content)
        {
            logger.LogInformation($"\nBT节点类型:{node.GetType().Name}\n信息:{content}\n");
        }

        public static void LogInformationWithNodeName(this ILogger logger, Node node, string content, bool logTime = false)
        {
            var msg = $"\nGodot节点类型:{node.Name}\n信息:{content}";
            if (logTime)
                msg += $"\n时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
            msg += "\n";
            logger.LogInformation(msg);
        }
    }
}
