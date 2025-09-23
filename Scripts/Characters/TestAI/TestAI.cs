using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.CharacterParts.PerceptionPart;
using DDemo.Scripts.Characters.Core;
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
	private bool _isIdle;
	private bool _isWalk;

	private bool _isAttack;
	private int _attackIndex;

	protected override void ConfigureStateMachine()
	{
		//StateMachine
		TeamType = DDemo.Scripts.Misc.Enums.E_TeamType.Enemy;

		_enemyIdle = new EnemyIdle(StateMachine);
		_enemyFollow = new EnemyFollow(StateMachine);

		_enemyIdle.AddEnter(() => _isIdle = true)
			.AddPhysicsProcess((delta) =>
			{

			})
			.AddExit(() => _isIdle = false);

		_enemyFollow.AddEnter(() => _isWalk = true)
			.AddPhysicsProcess((delta =>
			{
			var enemy=	TargetContext?.PrimaryTarget?.TargetNode;

				if (enemy == null) 
					return;

				// 动态更新目标位置（持续追踪玩家）
				NavigationAgent2D.TargetPosition = enemy.GlobalPosition;

				// 获取导航下一步的路径点
				if (NavigationAgent2D.IsNavigationFinished()) return;

				var nextPosition = NavigationAgent2D.GetNextPathPosition();
				var direction = (nextPosition - GlobalPosition).Normalized();

				Velocity = direction * 32;
				MoveAndSlide();

			}))
			.AddExit(() => _isWalk = false);

		_meleeAttack=new MeleeAttack(StateMachine);

		_meleeAttack.AddEnter(
			() => {
				_isAttack = true;
				_logger.LogInformation("进入轻攻击");
			}).
			AddEnter(()=>SetVelocity(0,0))
			.AddEnter(()=>_attackIndex=Random.Shared.Next(2));

		StateMachine.SetInitialState(_enemyIdle);

	}
//    Selector
//├── Sequence
//│   ├── Condition(!_isAttack)
//│   ├── AcquireTargetNode(200)
//│   └── Selector
//│       ├── Sequence
//│       │   ├── TargetIsInAttackRadius(30)
//│       │   └── SwitchState(_meleeAttack)
//│       └── Sequence
//│           └── SwitchState(_enemyFollow)
//└── SwitchState(_enemyIdle)
    protected override void ConfigureBehaviourTree()
    {
		BehaviorTree.BuildTree()
			.Selector()
				.Sequence()
					.Condition((delta => !_isAttack)) // 顶层检查
					.AddChild(new AcquireTargetNode(200))
					.Selector()
						.Sequence()
							.AddChild(new TargetIsInAttackRadius(30))
							.SwitchState(_meleeAttack, true)
						.End()
						.Sequence()
							.SwitchState(_enemyFollow)
						.End()
					.End()
				.End();
    }
	public override void _Process(double delta)
	{
		base._Process(delta);
		_logger.LogInformationWithNodeName(this, "当前状态:"+StateMachine.GetCurrentState().GetType().ToString());
	}

    /// <summary>
    /// Animation finished  callbacks
    /// </summary>
    public void OnAnimationFinished()
    {
		_logger.LogInformationWithNodeName(this,"攻击(_isAttack)被取消!");
        _attackIndex = 0;
        _isAttack = false;
    }
}
