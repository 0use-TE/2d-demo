using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using Godot;
using System;

public partial class CheckTargetInAttackRadiusTask : BTCondition
{
    [Export]
    private float _attackRadius;
    public override Status _Tick(double delta)
    {
        var ai = Blackboard.Get<AIBase>();
        var targetContext = ai.TargetContext;
        var targetNode= targetContext.CurrentTarget.TargetNode;

        if (targetNode!= null)
        {
            if (targetNode.GlobalPosition.DistanceTo(ai.GlobalPosition)<=_attackRadius)
            {
                ai.LoggerBTNode(this, $"目标{targetNode.Name}在攻击范围内!");
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}
