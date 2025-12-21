using Dash.AI.Extensions;
using Dash.Scripts.Entity.Core;
using Godot;
using System;
/// <summary>
/// 存在攻击目标跟随，返回Running，否则返回Failure
/// </summary>
public partial class FollowTargetTask : BTAction
{
  
    public override Status _Tick(double delta)
    {
        var _ai = Blackboard.Get<AIBase>();
        var moveSpeed = _ai.RuntimeStats.MoveSpeed;
        var targetContext = _ai.TargetContext;
        if (targetContext.CurrentTarget.TargetNode != null)
        {
            _ai.NavigationAgent2D.TargetPosition = targetContext.CurrentTarget.TargetNode.GlobalPosition;
            var nextPos = _ai.NavigationAgent2D.GetNextPathPosition();
            var direction = (nextPos - _ai.GlobalPosition).Normalized();

            _ai.LoggerBTNode(this, $"正在跟踪敌人{targetContext.CurrentTarget.TargetNode.Name}");
            _ai.SetVelocity(direction * moveSpeed);
            _ai.MoveAndSlide();

            return Status.Running;
        }
        else
        {
            return Status.Failure;
        }
    }
}
