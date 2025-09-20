using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Godot;
using Microsoft.Extensions.Logging;
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
		private AIBase _ai=default!;
		private TargetContext? _targetContext;

		private ILogger _logger=default!;
		public EnemyIsInDistance(float distance)
		{
			_distance=distance;
		}
		protected override void OnBlackboardCreated()
		{
			_ai = Blackboard.Load<AIBase>()??throw new NullReferenceException("AI没有存入黑板");
			_targetContext=Blackboard.Load<TargetContext>();
			_logger = Blackboard.Load<ILogger>()!;
		}

		public override NodeState Tick(double delta)
		{
            if (_targetContext?.PrimaryTarget == null)
                _logger.LogInformation("目标未设置");
            
			if (_ai.NavigationAgent2D.IsNavigationFinished())
				return NodeState.Failure;



			if(_targetContext?.PrimaryTarget?.Position.DistanceTo(_ai.Position)<_distance)
				return NodeState.Success;
		
			return NodeState.Failure;
		}
	}
}
