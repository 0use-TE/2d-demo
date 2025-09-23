using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.CharacterParts.PerceptionPart;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Misc.Enums;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
namespace DDemo.Scripts.Characters.TestAI;
[Meta(typeof(IAutoNode))]
public partial class TestAI : AIBase
{
	public override void _Notification(int what) => this.Notify(what);

	private EnemyIdle _enemyIdle;
	private EnemyFollow _enemyFollow;

	private MeleeAttack _meleeAttack;
	// 
	private bool _isIdle=true;
	private bool _isWalk;

	private bool _isAttack;
	private int _attackIndex;

	protected override void ConfigureStateMachine()
	{
		//StateMachine
		TeamType = E_TeamType.Enemy;
	}

    protected override void ConfigureBehaviourTree()
    {
		BehaviorTree.BuildTree()
			.Selector()
				.Sequence()
					.AddChild(new AcquireTargetNode(100))
					.AddChild(new FollowTargetNode(32*2))
				.End();
				
    }

	/// <summary>
	/// Animation finished  callbacks
	/// </summary>
	protected override void AnimationTree_AnimationFinished(StringName animName)
	{
		_logger.LogInformationWithNodeName(this, $"动画(类型为单次):{animName} 播放结束!");
		_attackIndex = 0;
		_isAttack = false;
	}
}
