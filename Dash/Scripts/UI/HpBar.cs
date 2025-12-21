using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using System;

namespace Dash.Scripts.UI;

[Meta(typeof(IAutoNode))]
public partial class HpBar : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Node("TextureProgressBar")]
    public TextureProgressBar ProgressBar { get; private set; } = default!;

    private Node2D _target = default!;
    private float _yOffset;

    public void Initialize(Node2D target, float current, float max)
    {
        _target = target;
        // 在初始化时计算高度偏移
        _yOffset = CalculateTargetHeight(target);

        UpdateHealth(current, max);
    }

    public override void _Process(double delta)
    {
        if (IsInstanceValid(_target))
        {
            // 获取敌人在屏幕上的位置（Canvas 坐标）
            Vector2 screenPos = _target.GetGlobalTransformWithCanvas().Origin;

            // 核心：即使在这里，也需要考虑 target 的实时 Scale 
            // 如果你希望血条高度随缩放动态变化，就用 _yOffset * _target.Scale.Y
            float dynamicOffset = _yOffset * _target.GlobalScale.Y;

            // 居中对齐并应用偏移
            GlobalPosition = screenPos + new Vector2(-ProgressBar.Size.X / 2, -dynamicOffset / 2);
        }
        else
        {
            // 目标消失，
            QueueFree();
        }
    }

    private float CalculateTargetHeight(Node2D target)
    {
        // 1. 尝试寻找动画节点 (AnimatedSprite2D)
        var animSprite = target.FindChild("AnimatedSprite2D") as AnimatedSprite2D;
        if (animSprite != null && animSprite.SpriteFrames != null)
        {
            var texture = animSprite.SpriteFrames.GetFrameTexture(animSprite.Animation, 0);
            if (texture != null)
            {
                // 注意：这里返回的是未缩放的原始高度 
                // 假设 Pivot 在脚底，所以直接用高度；如果 Pivot 在中心，则需除以 2
                return texture.GetSize().Y - 20;
            }
        }

        return 60.0f;
    }

    public void UpdateHealth(float current, float max)
    {
        ProgressBar.MaxValue = max;
        var tween = CreateTween();
        tween.TweenProperty(ProgressBar, "value", current, 0.25f)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.Out);
    }
}