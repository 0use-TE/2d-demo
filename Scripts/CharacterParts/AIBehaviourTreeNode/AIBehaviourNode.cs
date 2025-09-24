using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;
using DDemo.Scripts.CharacterParts.Extensions;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.AIBehaviourTreeNode
{
	public abstract class AIBehaviourNode : BehaviourNode
	{
		protected ILogger _logger = default!;
		protected PlayerContext _playerContext = default!;
		protected TargetContext _targetContext = default!;
		protected AIUnitContext _unitContext = default!;
		protected AIBase _ai = default!;

		protected override void OnBlackboardCreated()
		{
			// 从黑板获取一次依赖，并缓存到字段
			_ai = Blackboard.LoadOrThrow<AIBase>();
			_logger = Blackboard.LoadOrThrow<ILogger>();
			_playerContext = Blackboard.LoadOrThrow<PlayerContext>();
			_targetContext = Blackboard.LoadOrThrow<TargetContext>();
			_unitContext = Blackboard.LoadOrThrow<AIUnitContext>();
		}
	}

}
