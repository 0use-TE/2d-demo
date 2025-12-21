using Dash.AI.Extensions;
using Dash.Scripts.Entity.Core;
using Godot;
using System;
/// <summary>
/// 存在攻击目标跟随，返回Running，否则返回Failure
/// </summary>
public partial class FollowTargetTask : BTAction
{
    private AIBase _ai = default!;
    private double _repathTimer = 0;
    private Vector2 _lastTargetPos = Vector2.Zero;
    public override void _Enter()
    {
        _ai = Blackboard.Get<AIBase>();
    }
    public override Status _Tick(double delta)
    {
        var targetNode = _ai.TargetContext.CurrentTarget.TargetNode;

        if (IsInstanceValid(targetNode))
        {
            _repathTimer += delta;
            Vector2 currentTargetPos = targetNode.GlobalPosition;

            // 策略：每 0.2 秒更新一次路径，或者目标移动超过 30 像素时更新
            if (_repathTimer >= 0.2f || _lastTargetPos.DistanceTo(currentTargetPos) > 30f)
            {
                _ai.NavigationAgent2D.TargetPosition = currentTargetPos;
                _lastTargetPos = currentTargetPos;
                _repathTimer = 0;
            }

            // 导航逻辑
            if (!_ai.NavigationAgent2D.IsTargetReached())
            {
                var nextPos = _ai.NavigationAgent2D.GetNextPathPosition();
                var direction = (nextPos - _ai.GlobalPosition).Normalized();

                _ai.SetVelocity(direction * _ai.RuntimeStats.MoveSpeed);
                _ai.MoveAndSlide();

                return Status.Running;
            }

            return Status.Success; // 到达目的地
        }

        return Status.Failure;
    }
}
