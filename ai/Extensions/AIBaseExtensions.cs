using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.ai.Extensions
{
    public static class AIBaseExtensions
    {
        public static void Logger(this AIBase ai, string message)
        {
            ai.ILogger.LogInformationWithNodeName(ai, message);
        }
        public static void LoggerBTNode(this AIBase ai, BTTask btTask, string message)
        {
            ai.ILogger.LogInformation($"\n行为树节点{btTask.GetType().Name}\n{message}\n");
        }
    }
}
