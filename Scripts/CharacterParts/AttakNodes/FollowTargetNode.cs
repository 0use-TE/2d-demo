using CharacterModule.BehaviourTree;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.AttakNodes
{
    internal class FollowTargetNode : AIBehaviourNode
    {
        private E_TargetType _targetType;
        private float _followSpeed;
        public FollowTargetNode(E_TargetType targetType, float followSpeed)
        {
            _targetType = targetType;
            _followSpeed = followSpeed;
        }

        public override NodeState Tick(double delta)
        {
            var target = _targetContext.GetTarget(_targetType);
            var targetNode = target?.TargetNode;

            if (targetNode != null)
            {
                // 1. 设置目标点
                _ai.NavigationAgent2D.TargetPosition = targetNode.GlobalPosition;

                // 2. 获取下一个拐点
                Vector2 nextPoint = _ai.NavigationAgent2D.GetNextPathPosition();

                // 3. 计算移动方向
                Vector2 direction = (nextPoint - _ai.GlobalPosition).Normalized();

                // 4. 应用速度
                _ai.Velocity = direction * _followSpeed;

                // 5. 更新位置 
                _ai.MoveAndSlide();

                _logger.LogBehaviourTreeNodeInformation(this, $"正在跟随角色{targetNode.Name}");
                return NodeState.Running;
            }
            else
            {
                // 没有目标 → 失败
                return NodeState.Failure;
            }
        }

    }
}
