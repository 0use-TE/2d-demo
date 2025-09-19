using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Godot;
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
		private PlayerContext? _playerContext;
		public EnemyIsInDistance(float distance)
		{
			_distance=distance;
		}
		protected override void OnBlackboardCreated()
		{
			_ai = Blackboard.Load<AIBase>();
			_playerContext=Blackboard.Load<PlayerContext>();
		}

		public override NodeState Tick(double delta)
		{
			var player = _playerContext.Players.First();
			if (player != null)
			{
				if(_ai.Position.DistanceTo(player.Position)<_distance)
				{
					return NodeState.Success;
				}
			}
			return NodeState.Failure;
		}
	}
}
