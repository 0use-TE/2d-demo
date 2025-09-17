using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.ActionNodes;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.PerceptionPart.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart.BehaviourNodes
{
    internal class PerceptionPlayerConditionNode : BehaviourNode
    {
        private readonly IPerception _perception;

        public PerceptionPlayerConditionNode()
        {
            _perception = new EnemyPerception();
        }

        public override NodeState Tick(double delta)
        {
            return _perception.Perception()? NodeState.Success : NodeState.Failure;
        }
    }
}
