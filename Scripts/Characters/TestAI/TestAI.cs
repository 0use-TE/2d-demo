using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using DDemo.Scripts.CharacterParts.PerceptionPart.BehaviourNodes;
using Godot;
using Microsoft.Extensions.Logging;
namespace PlatformExplorer.BehaviorTreeTest;

public partial class TestAI : CharacterBody2D
{


	private NavigationAgent2D _navigationAgent2D;
	private AnimatedSprite2D _animatedSprite;

	private StateMachine _enemyStateMachine;
	private EnemyIdle _enemyIdle;
	private EnemyFollow _enemyFollow;

	// 
	private bool _isIdle;
	private bool _isWalk;

	private bool test=true;

	//Tree
	private BehaviorTree _tree;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_navigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
		_animatedSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		
		//StateMachine
		_enemyStateMachine = new StateMachine();
		_enemyIdle = new EnemyIdle(_enemyStateMachine);
		_enemyFollow = new EnemyFollow(_enemyStateMachine);

		_enemyIdle.AddEnter(() => _isIdle = true)
			.AddPhysicsProcess((delta) =>
			{
				GD.Print("Idle");
			})
			.AddExit(() => _isIdle = false);

		_enemyFollow.AddEnter(() => _isWalk = true)
			.AddPhysicsProcess((delta =>
			{
				var player = GetTree().GetCurrentScene().FindChild("Player") as CharacterBody2D;
				if (player == null) return;

				// 动态更新目标位置（持续追踪玩家）
				_navigationAgent2D.TargetPosition = player.GlobalPosition;

				// 获取导航下一步的路径点
				if (_navigationAgent2D.IsNavigationFinished()) return;

				var nextPosition = _navigationAgent2D.GetNextPathPosition();
				var direction = (nextPosition - GlobalPosition).Normalized();

				Velocity = direction * 32*2;
				MoveAndSlide();

			}))
			.AddExit(() => _isWalk = false);

		_enemyStateMachine.SetInitialState(_enemyIdle);


		_tree= BehaviorTree.CreateTree()
			.ConfigurateStateMachine(_enemyStateMachine);

		_tree.BuildTree()
			.Selector()
				.Sequence()
					.AddChild(new PerceptionPlayerConditionNode()) //感知玩家
					//跟随玩家
					.SwitchState(_enemyFollow)
			.End()
					.SwitchState(_enemyIdle);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (Input.IsKeyPressed(Key.X))
			//test = !test;

		if (Velocity.X> 0.1)
			_animatedSprite.FlipH = false;
		else if (Velocity.X< -0.1)
			_animatedSprite.FlipH = true;
	}
	public override void _PhysicsProcess(double delta)
	{
		//BT tick
		_tree.Tick(delta);
		//StateMachine's process.
		_enemyStateMachine.PhysicsProcess(delta);
	}

}
