using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
    public class TargetIsInAttackRadiusNode : BehaviourNode
    {
        private readonly float _distance;
        private ILogger _logger=default!;
        private AIBase _ai=default!;
        private TargetContext _targetContext=default!;
        public TargetIsInAttackRadiusNode(float distance)
        {
            _distance = distance;
        }
        protected override void OnBlackboardCreated()
        {
            _logger = Blackboard.Load<ILogger>()!;
            _ai = Blackboard.Load<AIBase>()!;
            _targetContext = Blackboard.Load<TargetContext>()!;
        }

        public override NodeState Tick(double delta)
        {
            if(_targetContext.PrimaryTarget==null)
            {
                _logger.LogBehaviourTreeNodeInformation(this,"没有设置主目标");
                return NodeState.Failure;
            }

            if(_targetContext.PrimaryTarget.TargetNode==null)
            {
                _logger.LogBehaviourTreeNodeInformation(this, "没有设置主目标的节点");
                return NodeState.Failure;
            }

            if (_ai.Position.DistanceTo(_targetContext.PrimaryTarget.TargetNode.Position) < _distance)
            {
                var targetNode = _targetContext.PrimaryTarget.TargetNode;
                if (targetNode == null)
                {
                    _logger.LogBehaviourTreeNodeInformation(this, $"主要目标没有设置目标节点!");
                    return NodeState.Failure;
                }
                _logger.LogBehaviourTreeNodeInformation(this, $"敌人{targetNode.Name}进入攻击范围!");
                return NodeState.Success;
            }
            else
            {
                _logger.LogBehaviourTreeNodeInformation(this, $"目标位置{_targetContext.PrimaryTarget.TargetNode.Position}\n当前位置{_ai.Position}");
                return NodeState.Failure;
            }
        }
    }
}
