using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;

public partial class TestRaycast : Node2D
{
    [Export]
    private float _distance;

    public override void _Process(double delta)
    {
        var world2D = GetWorld2D();
        var space = world2D.DirectSpaceState;

        // 构造一个圆形碰撞体（检测区域）
        var circle = new CircleShape2D { Radius = _distance };

        // 设置圆心位置（AI 自己的位置）
        var transform = new Transform2D(0, GlobalPosition);

        // 排除自身，避免检测命中自己
        var exclude = new Godot.Collections.Array<Rid>
        {
            new Rid(this),
        };

        // 在物理空间中检测与圆相交的对象
        var results = space.IntersectShape(new PhysicsShapeQueryParameters2D
        {
            Shape = circle,
            Transform = transform,
            Margin = 0.1f,
        });

        if (results.Count > 0)
        {
            GD.Print($"检测到 {results.Count} 个对象:");
            foreach (var res in results)
            {
                var dict = res;
                var collider= dict["collider"];
               var node= collider.As<Node2D>();
                GD.Print(node.Name);
            }
        }
        else
        {
            GD.Print("没有检测到对象");
        }
    }
}
