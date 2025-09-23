using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.Extensions;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Microsoft.Extensions.Logging;
using System;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
	public class FollowTargetNode : BehaviourNode
	{
		private TargetContext _targetContext=default!;
		private AIBase _ai=default!;
		private ILogger _logger = default!;
		private float _speed;
		public FollowTargetNode(float  speed)
		{
			_speed = speed;
		}
		protected override void OnBlackboardCreated()
		{
			// 从黑板中加载目标上下文，如果未设置则抛出异常
			_targetContext = Blackboard.LoadOrThrow<TargetContext>();
			// 从黑板中加载AI实例，如果未设置则抛出异常
			_ai = Blackboard.LoadOrThrow<AIBase>();
			_logger = Blackboard.LoadOrThrow<ILogger>();
		}
		public override NodeState Tick(double delta)
		{
			if (_targetContext?.PrimaryTarget?.TargetNode == null)
			{
				_logger.LogBehaviourTreeNodeInformation(this, "主目标或者是主目标的节点没有设置");
				return NodeState.Failure;
			}

			var targetPos = _targetContext.PrimaryTarget.TargetNode.Position;
			var currentPos = _ai.Position;
			var direction = targetPos - currentPos;
			var distance = direction.Length();

			if (distance < 0.01f)
			{
				// 已到达目标
				_ai.NavigationAgent2D.TargetPosition = targetPos;
				_logger.LogBehaviourTreeNodeInformation(this, "AI已经到达目标!");
				return NodeState.Success;
			}

			direction = direction.Normalized();
			var moveDistance = (float)(_speed * delta);
			var nextPos = distance <= moveDistance ? targetPos : currentPos + direction * moveDistance;

			_ai.Position = nextPos;
			_logger.LogBehaviourTreeNodeInformation(this, $"{_ai.Name}正在Follow");
			return NodeState.Running;
		}
	}
}
