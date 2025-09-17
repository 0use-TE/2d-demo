using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.ActionNodes;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.PerceptionPart.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart.BehaviourNodes
{
    internal class PerceptionPlayerConditionNode : BehaviourNode
    {
        private  IPerception _perception;
        private float _distance;
        public PerceptionPlayerConditionNode(float distance)
        {
            _distance = distance;
        }

		public override NodeState Tick(double delta)
        {
            if(_perception == null)
                throw new Exception("Perception is null");
			return _perception.Perception()? NodeState.Success : NodeState.Failure;
        }
    }
}
