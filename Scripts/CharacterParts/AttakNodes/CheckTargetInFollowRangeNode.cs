using CharacterModule.BehaviourTree;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.AttakNodes
{
    public class CheckTargetInFollowRangeNode : AIBehaviourNode
    {
        private float _attackRange;
        private E_TargetType _targetType;
        public CheckTargetInFollowRangeNode(float attackRange, E_TargetType targetType)
        {
            _attackRange = attackRange;
            _targetType = targetType;
        }

        public override NodeState Tick(double delta)
        {
            var target = _targetContext.GetTarget(_targetType);

            if (target?.TargetNode != null)
            {
                if(_ai.GlobalPosition.DistanceTo(target.TargetNode.GlobalPosition)<_attackRange)
                {
                    _logger.LogBehaviourTreeNodeInformation(this, $"{target.TargetNode.Name}处在攻击范围");
                    return NodeState.Failure;
                }
                else
                {
                    return NodeState.Running;
                }

            }
            else
            {
                return NodeState.Failure;
            }

        }
    }
}
