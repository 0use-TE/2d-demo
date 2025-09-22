using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
    public class TargetIsInAttackRadius : BehaviourNode
    {
        private readonly float _distance;
        private ILogger _logger=default!;
        private AIBase _ai=default!;
        private TargetContext _targetContext=default!;
        public TargetIsInAttackRadius(float distance)
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
                _logger.LogInformation("没有设置攻击目标");
                return NodeState.Failure;
            }

            if (_ai.Position.DistanceTo(_targetContext.PrimaryTarget.Position) < _distance)
            {
                _logger.LogInformation($"敌人{_targetContext.PrimaryTarget?.TargetNode?.Name}进入攻击范围!");
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}
