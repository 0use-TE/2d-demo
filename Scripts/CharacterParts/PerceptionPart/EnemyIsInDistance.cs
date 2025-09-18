using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
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
		public EnemyIsInDistance(float distance)
		{
			_distance=distance;
		}
		public override NodeState Tick(double delta)
		{
			return NodeState.Success;
		}
	}
}
