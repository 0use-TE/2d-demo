using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.Extensions;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
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
			var targetNode = _targetContext?.PrimaryTarget?.TargetNode;
			if (targetNode == null)
			{
				_logger.LogBehaviourTreeNodeInformation(this, "主目标或者主目标节点未设置");
				return NodeState.Failure;
			}

			// 设置目标位置给 NavigationAgent2D
			_ai.NavigationAgent2D.TargetPosition = targetNode.GlobalPosition;

			// 计算距离
			Vector2 toTarget = targetNode.GlobalPosition - _ai.GlobalPosition;
			if (toTarget.Length() < 32.0f)
			{
				_logger.LogBehaviourTreeNodeInformation(this, "已到达目标附近");
				_ai.NavigationAgent2D.TargetPosition = _ai.GlobalPosition; // 停止移动
				return NodeState.Success;
			}

			// 获取 NavigationAgent2D 的下一步速度向量
			Vector2 nextVelocity = _ai.NavigationAgent2D.GetNextPathPosition() - _ai.GlobalPosition;

			nextVelocity = nextVelocity.Normalized() * _speed;
			_ai.Velocity = nextVelocity;
			_ai.MoveAndSlide();

			_logger.LogBehaviourTreeNodeInformation(this, $"跟踪目标: 速度 {_ai.Velocity}, 当前距离 {toTarget.Length()}");

			return NodeState.Running;
		}


	}
}
