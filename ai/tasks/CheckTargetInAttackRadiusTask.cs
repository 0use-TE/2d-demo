using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using Godot;
using System;
/// <summary>
/// 检查攻击目标是否在在攻击范围内，在返回success,否则返回Failure
/// </summary>
public partial class CheckTargetInAttackRadiusTask : BTCondition
{
    private AIBase ai=default!;
    private float attack1Radius;
    public override void _Setup()
    {
        ai = Blackboard.Get<AIBase>();
        attack1Radius = Blackboard.GetVar("Attack1Radius").As<float>();
    }
    public override Status _Tick(double delta)
    {
        var targetContext = ai.TargetContext;
        var targetNode= targetContext.CurrentTarget.TargetNode;

        if (targetNode!= null)
        {
            if (targetNode.GlobalPosition.DistanceTo(ai.GlobalPosition)<=attack1Radius)
            {
                ai.LoggerBTNode(this, $"目标{targetNode.Name}在攻击范围内!");
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}
