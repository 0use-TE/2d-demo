using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.Stats;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using System;
namespace DDemo.Scripts.Entity.Players;
[Meta(typeof(IAutoConnect))]
public partial class Player : PlayerBase
{
	public override void _Notification(int what) => this.Notify(what);
    //Input-Related
    private PlayerInput _playerInput = new PlayerInput();

	//State-Related
	private PlayerIdleState ?_playerIdleState;
	private PlayerWalkState ?_playerWalkState;
	//Attack
	private PlayerMeleeAttackState ?_playerMeleeAttackState;
	private PlayerRemoteAttackState? _playerRemoteAttackState;
	private bool _isAttack;
	private int _attackIndex = 0;

	protected override void ConfigureStateMachine()
	{

		_playerIdleState = new PlayerIdleState(StateMachine);
		_playerWalkState = new PlayerWalkState(StateMachine);
		_playerMeleeAttackState = new PlayerMeleeAttackState(StateMachine);
		_playerRemoteAttackState = new PlayerRemoteAttackState(StateMachine);

		//Idle
		_playerIdleState.AddEnter(() => AnimationPlayer.Play("Idle")).AddEnter(() => SetVelocity(0, 0)).
				AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) > 0.1f || Mathf.Abs(_playerInput.Vertical) > .1f, _playerWalkState).
				AddTransitions(() => _playerInput.MeleeAttack, _playerMeleeAttackState).
				AddTransitions(() => _playerInput.RemoteAttack, _playerRemoteAttackState);
		//Walk
		_playerWalkState.AddEnter(() => AnimationPlayer.Play("Walk")).
			AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) < 0.1f && Mathf.Abs(_playerInput.Vertical) < .1f, _playerIdleState).
			AddTransitions(() => _playerInput.MeleeAttack, _playerMeleeAttackState).
			AddTransitions(() => _playerInput.RemoteAttack, _playerRemoteAttackState).
			AddPhysicsProcess((delta) => SetVelocity(_playerInput.Horizontal * GetVar<float>(StatStrings.HorizontalMoveSpeed), _playerInput.Vertical * GetVar<float>(StatStrings.HorizontalMoveSpeed)));

		//MeleeAttack
		_playerMeleeAttackState.AddEnter(() => _isAttack = true).AddEnter(() => SetVelocity(0, 0))
			.AddEnter(() => _attackIndex = Random.Shared.Next(0, 2))
			.AddEnter(() => AnimationPlayer.Play("Attack" + (_attackIndex + 1)))
			.AddTransitions(() => !_isAttack, _playerIdleState);

		//RemoteAttack
		_playerRemoteAttackState.AddEnter(() => _isAttack = true).AddEnter(() => _attackIndex = 2)
			.AddEnter(() => AnimationPlayer.Play("Attack" + (_attackIndex + 1)))
			.AddTransitions(() => !_isAttack, _playerIdleState);

		//Set Initial State
		StateMachine.SetInitialState(_playerIdleState);
	}
    protected override void AnimationPlayer_AnimationFinished(StringName animName)
	{
        Logger.LogInformationWithNodeName(this, $"动画{animName}播放结束!");
        _isAttack = false;
        _attackIndex = 0;
    }
}
