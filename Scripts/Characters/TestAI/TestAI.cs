using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.CompositeNodes;
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
	[Export]
	private float _maxRadius=200;
	[Export]
	private float _speed=50;
	private bool _isAttack;
	private int _attackIndex;

	protected override void ConfigureStateMachine()
	{
		//StateMachine
		TeamType = E_TeamType.Enemy;
		_enemyFollow = new EnemyFollow(StateMachine);
		_enemyIdle = new EnemyIdle(StateMachine);

		_enemyIdle.AddEnter(() => _isIdle = true)
			.AddExit(() => _isIdle = false);

		_enemyFollow.AddEnter(() => _isWalk = true)
			.AddExit(() => _isWalk = false);

		StateMachine.SetInitialState(_enemyIdle);
	}
    /*
	Root (Selector)
└── Sequence
    ├── Condition: DetectTarget (Success/Failure)
    ├── AnimationNode: SwitchAnimation (Success)
    └── Parallel (AllRunning, AnyFailure)
         ├── Condition: CheckTargetDistance (Running/Failure)
         └── Action: FollowTarget (Running)
	 */
    protected override void ConfigureBehaviourTree()
    {
		BehaviorTree.BuildTree()
			.Selector()
				//Follow分支
				.Parallel(ParallelPolicy.Any, ParallelPolicy.Any)
					.SwitchAnimation(_enemyFollow)
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
