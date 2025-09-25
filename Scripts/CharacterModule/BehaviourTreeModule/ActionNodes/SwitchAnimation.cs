using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTreeModule.ActionNodes
{
	internal class SwitchAnimation : BehaviourNode
	{
		private bool _isOneShot;
		private StateMachine _stateMachine = default!;
		private BaseState _newState;
		private ILogger? _logger;
		public SwitchAnimation(BaseState newState, bool isOneShot = false)
		{
			_newState = newState;
			_isOneShot = isOneShot;
		}
		protected override void OnBlackboardCreated()
		{
			_stateMachine = Blackboard.Load<StateMachine>() ?? throw new ArgumentNullException("黑板中没有找到状态机!");
			_logger = Blackboard.Load<ILogger>();
		}
		public override NodeState Tick(double delta)
		{
			var currentState = _stateMachine.GetCurrentState();

			if (currentState.GetType() != _newState.GetType())
			{
				_logger?.LogInformation($"切换到了动画:{_newState.GetType().Name}");
				_stateMachine.ChangeState(_newState);
			}
			return NodeState.Running;
		}
	}
}
