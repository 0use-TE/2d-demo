using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.Characters.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
	public class EnemyIsInDistance : BehaviourNode
	{
		private float _distance;
		private AIBase ?_ai;
		public EnemyIsInDistance(float distance)
		{
			_distance=distance;
		}
        protected override void OnBlackboardCreated()
        {
			_ai = Blackboard.Load<AIBase>();
        }

		public override NodeState Tick(double delta)
		{
			return NodeState.Success;
		}
	}
}
