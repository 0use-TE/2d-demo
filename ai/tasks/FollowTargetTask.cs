using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using Godot;
using System;

public partial class FollowTargetTask : BTAction
{
    [Export]
    private float _speed;
    public override void _Setup()
    {

    }
    public override Status _Tick(double delta)
    {
            var    _ai = Blackboard.Get<AIBase>();

        var targetContext = _ai.TargetContext;
        if (targetContext.CharacterTarget.TargetNode != null)
        {
            _ai.NavigationAgent2D.TargetPosition = targetContext.CharacterTarget.TargetNode.GlobalPosition;
            var nextPos = _ai.NavigationAgent2D.GetNextPathPosition();
            var direction = (nextPos - _ai.GlobalPosition).Normalized();

            _ai.LoggerBTNode(this, $"正在跟踪敌人{targetContext.CharacterTarget.TargetNode.Name}");
            _ai.SetVelocity(direction * _speed);
            _ai.MoveAndSlide();

            return Status.Running;
        }
        else
        {
            return Status.Failure;
        }
    }
}
