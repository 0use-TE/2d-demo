using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;

/// <summary>
/// 想要触发AnimationFinished回到，需要满足以下条件
/// 第一是动画必须设置为单次
/// </summary>
[Meta(typeof(IAutoConnect	))]
public partial class TestAnimationEndSignal : Node2D
{
	public override void _Notification(int what) => this.Notify(what);
	[Node(nameof(AnimationPlayer))]
	public AnimationPlayer AnimationPlayer { get; set; } = default!;
	[Inject]
	private ILogger<TestAnimationEndSignal> _logger=default!;

	public override void _Ready()
	{
		AnimationPlayer.AnimationFinished += AnimationPlayer_AnimationFinished;
	}

	private void AnimationPlayer_AnimationFinished(StringName animName)
	{
		_logger.LogInformationWithNodeName(this, $"动画:{animName}已经播放完了!");
	}
}
