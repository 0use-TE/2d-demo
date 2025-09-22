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
            logger.LogInformation($"节点类型:{node.GetType()}信息:{content}");
        }
    }
}
