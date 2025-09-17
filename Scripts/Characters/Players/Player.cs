using CharacterModule.StateMachineModule;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
namespace PlatformExplorer.PlayerScript;
public partial class Player : CharacterBody2D
{
	[Inject]
	private ILogger<Player> _logger = default!;
	private CharacterBody2D? _body;
	private AnimatedSprite2D ?_animatedSprite;

	//Congifure Args
	[Export]
	private float _horizontalSpeed = 32 * 2.5f;       // 水平移动速度
	[Export]
	private float _verticalSpeed = 32 * 2.5f;       // 水平移动速度
	
	private float _meleeAttackMoveSpeed = 32 * 0;       // 近战攻击移动速度
	private float _remoteAttackMoveSpeed = 32 * 0.05f;       // 近战攻击移动速度
	//Input-Related
	private PlayerInput _playerInput = new PlayerInput();

	//State-Related
	private StateMachine ?_playerStateMachine;
	private PlayerIdleState ?_playerIdleState;
	private PlayerWalkState ?_playerWalkState;
	//Attack
	private PlayerMeleeAttackState ?_playerMeleeAttackState;
	private PlayerRemoteAttackState? _playerRemoteAttackState;


	//Animation Flag
	private bool _isIdle;
	private bool _isWalk;

	private bool _isAttack;
	private int _attackIndex = 0;

	public override void _Ready()
	{
		_body = this;
		_animatedSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));

		_playerStateMachine = new StateMachine();
		_playerIdleState = new PlayerIdleState(_playerStateMachine);
		_playerWalkState = new PlayerWalkState(_playerStateMachine);
		_playerMeleeAttackState = new PlayerMeleeAttackState(_playerStateMachine);
		_playerRemoteAttackState = new PlayerRemoteAttackState(_playerStateMachine);

		//Idle
		_playerIdleState.AddEnter(() => _isIdle = true).AddEnter(() => SetVelocity(0, 0)).
				AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) > 0.1f || Mathf.Abs(_playerInput.Vertical) > .1f, _playerWalkState).
				AddTransitions(() => _playerInput.MeleeAttack, _playerMeleeAttackState).
				AddTransitions(() => _playerInput.RemoteAttack, _playerRemoteAttackState).
				AddExit(() => _isIdle = false);
		//Walk
		_playerWalkState.AddEnter(() => _isWalk = true).
			AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) < 0.1f && Mathf.Abs(_playerInput.Vertical) < .1f, _playerIdleState).
			AddTransitions(() => _playerInput.MeleeAttack, _playerMeleeAttackState).
			AddTransitions(() => _playerInput.RemoteAttack, _playerRemoteAttackState).
			AddPhysicsProcess((delta) => SetVelocity(_playerInput.Horizontal * _horizontalSpeed, _playerInput.Vertical * _verticalSpeed)).
			AddExit(() => _isWalk = false);

		//MeleeAttack
		_playerMeleeAttackState.AddEnter(() => _isAttack = true).AddEnter(() => SetVelocity(0, 0)).
			AddEnter(() => _attackIndex = Random.Shared.Next(0, 2)).
			AddTransitions(() => !_isAttack, _playerIdleState).
			AddPhysicsProcess((delta) => SetVelocity(_playerInput.Horizontal * _meleeAttackMoveSpeed, _playerInput.Vertical * _meleeAttackMoveSpeed));

		//RemoteAttack
		_playerRemoteAttackState.AddEnter(() => _isAttack = true).AddEnter(() => _attackIndex = 2).
			AddTransitions(() => !_isAttack, _playerIdleState).
			AddPhysicsProcess((delta) => SetVelocity(_playerInput.Horizontal * _remoteAttackMoveSpeed, 0));

		//Set Initial State
		_playerStateMachine.SetInitialState(_playerIdleState);
		_logger.LogInformation("Player Ready");
	}
	/// <summary>
	/// Called once per frame
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		_playerStateMachine?.Process(delta);
		if (_playerInput.Horizontal > 0.1)
			_animatedSprite.FlipH = false;
		else if (_playerInput.Horizontal < -0.1)
			_animatedSprite.FlipH = true;
	}

	/// <summary>
	/// Animation finished  callbacks
	/// </summary>
	public void OnAnimationFinished()
	{
		_attackIndex = 0;
		_isAttack = false;
	}
	public override void _PhysicsProcess(double delta)
	{
		_playerStateMachine.PhysicsProcess(delta);

		MoveAndSlide();
	}

	private void SetVelocity(float? x = null, float? y = null)
	{
		var velocity = Velocity;
		if (x.HasValue) velocity.X = x.Value;
		if (y.HasValue) velocity.Y = y.Value;
		Velocity = velocity;
	}

	private void AddVelocity(float? x = null, float? y = null)
	{
		var velocity = Velocity;
		if (x.HasValue) velocity.X += x.Value;
		if (y.HasValue) velocity.Y += y.Value;
		Velocity = velocity;
	}
}
