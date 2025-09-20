using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.CharacterParts.PerceptionPart;
using DDemo.Scripts.Characters.Core;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
namespace PlatformExplorer.BehaviorTreeTest;

[Meta(typeof(IAutoNode))]
public partial class TestAI : AIBase
{
    public override void _Notification(int what) => this.Notify(what);

    private EnemyIdle _enemyIdle;
	private EnemyFollow _enemyFollow;

	// 
	private bool _isIdle;
	private bool _isWalk;

	protected override void ConfigureStateMachine()
	{
		//StateMachine
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
			var player=	TargetContext?.PrimaryTarget?.TargetNode;

				if (player == null) 
					return;

				// 动态更新目标位置（持续追踪玩家）
				NavigationAgent2D.TargetPosition = player.GlobalPosition;

				// 获取导航下一步的路径点
				if (NavigationAgent2D.IsNavigationFinished()) return;

				var nextPosition = NavigationAgent2D.GetNextPathPosition();
				var direction = (nextPosition - GlobalPosition).Normalized();

				Velocity = direction * 32;
				MoveAndSlide();

			}))
			.AddExit(() => _isWalk = false);

		StateMachine.SetInitialState(_enemyIdle);

	}
	protected override void ConfigureBehaviourTree()
	{
		BehaviorTree.BuildTree()
		.Selector()
			.Sequence()
				.AddChild(new EnemyIsInDistance(128f))//感知玩家
				.SwitchState(_enemyFollow)   //跟随玩家
		.End()
				.SwitchState(_enemyIdle);
	}

}
