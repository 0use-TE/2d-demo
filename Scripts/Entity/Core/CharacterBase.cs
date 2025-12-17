using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using DDemo.Scripts.Events;
using DDemo.Scripts.Misc.Enums;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using MessagePipe;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
    [Meta(typeof(IAutoNode))]
    public abstract partial class CharacterBase : CharacterBody2D, ICharacter
    {
        public override void _Notification(int what) => this.Notify(what);
        public CharacterBody2D CharacterBody2D { get; private set; } = default!;
        [Node("VisibleOnScreenNotifier2D")]
        public VisibleOnScreenNotifier2D VisibilityNotifier { get; private set; } = default!;
        [Inject] public IPublisher<EnemySpotted> _spottedPub = default!;
        [Inject] public IPublisher<EnemyGone> _gonePub = default!;

        // 后台字段
        private AnimationPlayer _animationPlayer = default!;
        private AnimatedSprite2D _animatedSprite2D = default!;
        [Node(nameof(AnimationPlayer))]
        public AnimationPlayer AnimationPlayer
        {
            get => _animationPlayer;
            private set => _animationPlayer = value ?? throw new InvalidOperationException(
                "AnimationPlayer 没有正确注入！请检查角色实例是否放置了该节点。");
        }
        [Node(nameof(AnimatedSprite2D))]
        public AnimatedSprite2D AnimatedSprite2D
        {
            get => _animatedSprite2D;
            private set => _animatedSprite2D = value ?? throw new InvalidOperationException(
                "AnimatedSprite2D 没有正确注入！请检查角色实例是否放置了该节点。");
        }

        public E_TeamType TeamType { get; set; }
        public ILogger Logger { get; set; } = default!;
        [Inject]
        public ILoggerFactory _loggerFactory = default!;

        public int FacingDirection { get; set; } = 1; // 1表示向右，-1表示向左
        public override void _Ready()
        {
            base._Ready();
            Logger = _loggerFactory.CreateLogger(GetType());
            CharacterBody2D = this;
        }
        public void OnResolved()
        {
            // 进入屏幕发信号
            VisibilityNotifier.ScreenEntered += () =>
            {
                Logger.LogInformationWithNodeName(this, "角色进入了屏幕!");
                _spottedPub.Publish(new EnemySpotted(this, 90, 100));
            };
            // 离开屏幕发信号
            VisibilityNotifier.ScreenExited += () =>
            {
                Logger.LogInformationWithNodeName(this, "角色退出了屏幕!");
                _gonePub.Publish(new EnemyGone(this));
            };
            if (VisibilityNotifier.IsOnScreen())
            {
                Logger.LogInformationWithNodeName(this, "角色初始化时进入了屏幕!");
                _spottedPub.Publish(new EnemySpotted(this, 50, 100));
            }
        }
        public void AddVelocity(float? x = null, float? y = null)
        {
            var velocity = Velocity;
            if (x.HasValue) velocity.X = x.Value;
            if (y.HasValue) velocity.Y = y.Value;
            Velocity = velocity;
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            Flip();
        }
        public void Flip()
        {
            //朝右
            if (FacingDirection > 0 && Velocity.X < -0.1)
            {
                FacingDirection = -1;
                AnimatedSprite2D.FlipH = true;
            }

            if (FacingDirection < 0 && Velocity.X > 0.1)
            {
                FacingDirection = 1;
                AnimatedSprite2D.FlipH = false;
            }
        }
        public void SetVelocity(float? x = null, float? y = null)
        {
            var velocity = Velocity;
            if (x.HasValue) velocity.X = x.Value;
            if (y.HasValue) velocity.Y = y.Value;
            Velocity = velocity;
        }


        public virtual void TakeDamage(Node2D attacker, int attackValue)
        {

        }

    }
}
