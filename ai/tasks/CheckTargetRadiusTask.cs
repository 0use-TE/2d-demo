using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using Godot;
using System;
/// <summary>
/// 当有攻击目标返回Sucess，无攻击目标返回Failure
/// </summary>
public partial class CheckTargetRadiusTask : BTCondition
{
    private AIBase _ai = default!;
    public override void _Setup()
    {
       _ai = Blackboard.Get<AIBase>();
    }
    public override Status _Tick(double delta)
    {
        var targetContext = _ai.TargetContext;
        if(targetContext.CurrentTarget.TargetNode==null)
        {
            _ai.LoggerBTNode(this, $"攻击目标为null");
            return Status.Failure;
        }
        else
        {
            _ai.LoggerBTNode(this, $"检测到攻击目标{targetContext.CurrentTarget.TargetNode?.Name}");
            return Status.Success;
        }
    }
}
