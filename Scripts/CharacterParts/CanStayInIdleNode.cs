using CharacterModule.BehaviourTree;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using DDemo.Scripts.Test.LoggerExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts
{
    internal class CanStayInIdleNode : AIBehaviourNode
    {
        public override NodeState Tick(double delta)
        {
            var targetNode= _targetContext.CharacterTarget?.TargetNode;
            if (targetNode==null)
            {
                _logger.LogBehaviourTreeNodeInformation(this, $"没有敌人,将保持在Idle");
                return NodeState.Running;
            }
            else
            {
                _logger.LogBehaviourTreeNodeInformation(this, $"发现敌人{targetNode.Name}，将退出节点{this.GetType().Name}");
                return NodeState.Failure;
            }
        }
    }
}
