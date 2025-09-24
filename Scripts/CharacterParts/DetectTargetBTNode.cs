using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using DDemo.Scripts.CharacterParts.Extensions;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts
{
    internal class DetectTargetBTNode : AIBehaviourNode
    {



        public override NodeState Tick(double delta)
        {
			var target = _targetContext.CharacterTarget?.TargetNode;
			if (target != null)
			{
				_logger.LogBehaviourTreeNodeInformation(this, $"目标{target.Name}在跟随范围内");
				return NodeState.Success;
			}
			else
			{
				_logger.LogBehaviourTreeNodeInformation(this, $"暂未设置要跟随的角色目标");
				return NodeState.Failure;
			}
		}
    }
}
