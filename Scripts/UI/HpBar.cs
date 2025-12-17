using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;
namespace DDemo.Scripts.UI;

[Meta(typeof(IAutoNode))]
public partial class HpBar : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Node("TextureProgressBar")]
    public TextureProgressBar ProgressBar { get; private set; } = default!;

    private Node2D _target = default!;
    public void Initialize(Node2D target, int current, int max)
    {
        _target = target;
        UpdateHealth(current, max);
    }

    public override void _Process(double delta)
    {
        // 核心：追踪位置
        if (IsInstanceValid(_target))
        {
            // 获取敌人在屏幕上的位置
            Vector2 screenPos = _target.GetGlobalTransformWithCanvas().Origin;
            // 偏移量：显示在头顶
            GlobalPosition = screenPos + new Vector2(-ProgressBar.Size.X / 2, -60);
        }
    }

    public void UpdateHealth(int current, int max)
    {
        ProgressBar.MaxValue = max;
        var tween = CreateTween();
        tween.TweenProperty(ProgressBar, "value", current, 0.25f)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.Out);
    }
}