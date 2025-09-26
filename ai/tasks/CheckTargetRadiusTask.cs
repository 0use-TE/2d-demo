using Godot;
using System;

public partial class CheckTargetRadiusTask : BTCondition
{
    public override Status _Tick(double delta)
    {
        var player = Blackboard.Get("Player").As<Node2D>();
        if(player!=null)
        {
            GD.Print("检测到了玩家");
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }
}
