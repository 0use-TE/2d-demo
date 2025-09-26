using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using Godot;
using System;

public partial class CheckTargetRadiusTask : BTCondition
{
    public override void _Setup()
    {
    }
    public override Status _Tick(double delta)
    {
       var _ai = Blackboard.Get<AIBase>();

        var targetContext = _ai.TargetContext;
        if(targetContext.CurrentTarget.TargetNode==null)
        {
            _ai.LoggerBTNode(this, $"角色目标为null");
            return Status.Failure;
        }
        else
        {
            _ai.LoggerBTNode(this, $"检测到角色{targetContext.CurrentTarget.TargetNode?.Name}");
            return Status.Success;
        }
    }
}
