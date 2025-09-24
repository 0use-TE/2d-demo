using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts
{
    internal class FollowTargetBTNode : AIBehaviourNode
    {
		private float _speed;
		public FollowTargetBTNode(float speed)
		{
			_speed = speed;
		}

		public override NodeState Tick(double delta)
		{
			// 获取目标节点
			var targetNode = _targetContext.CharacterTarget?.TargetNode;

			if (targetNode != null)
			{
				// 计算目标位置
				var targetPosition = targetNode.GlobalPosition;

				// 计算当前位置到目标位置的方向
				Vector2 direction = (targetPosition - _ai.GlobalPosition).Normalized();

				Vector2 velocity = direction * _speed;

				// 使用刚体的 move_and_slide 方法来平滑移动
				_ai.Velocity = velocity; 

				// 确保刚体能用物理引擎计算出平滑的运动
				_ai.MoveAndSlide();

				return NodeState.Running; // 仍在移动，继续运行
			}
			else
			{
				// 如果没有目标，返回失败
				return NodeState.Failure;
			}
		}
	}
}
