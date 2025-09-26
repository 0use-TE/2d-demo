using Godot;
using System;

public partial class FollowTargetTask : BTAction
{
    public override Status _Tick(double delta)
    {
        return Status.Running;
    }
}
