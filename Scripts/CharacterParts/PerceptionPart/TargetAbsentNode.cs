using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.Extensions;
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
	public class TargetAbsentNode : BehaviourNode
	{
		private TargetContext _targetContext=default!;
		private ILogger _logger = default!;
		protected override void OnBlackboardCreated()
		{
			_targetContext=Blackboard.LoadOrThrow<TargetContext>();
			_logger=Blackboard.LoadOrThrow<ILogger>();
		}
		public override NodeState Tick(double delta)
		{
			if(_targetContext.PrimaryTarget==null||_targetContext.SecondaryTarget==null)
			{
				_logger.LogBehaviourTreeNodeInformation(this, $"目标不存在");
				return NodeState.Success;
			}
			else
			{
				_logger.LogBehaviourTreeNodeInformation(this, $"目标存在了....");
				return NodeState.Failure;
			}
		}
	}
}
