using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Dash.Scripts.Entity.Core.AttackSystem.Core;
using Dash.Scripts.Entity.Core.Stats;
using Dash.Scripts.Events;
using Dash.Scripts.Misc.Enums;
using Dash.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using MessagePipe;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.Core
{
    [Meta(typeof(IAutoNode))]
    public abstract partial class CharacterBase : CharacterBody2D, ICharacter, IProvide<IEntity>
    {
        public override void _Notification(int what) => this.Notify(what);
        public CharacterBody2D CharacterBody2D { get; private set; } = default!;
        [Node("VisibleOnScreenNotifier2D")]
        public VisibleOnScreenNotifier2D VisibilityNotifier { get; private set; } = default!;
        [Inject] public IPublisher<EntitySpotted> _spottedPub = default!;
        [Inject] public IPublisher<EntityGone> _gonePub = default!;
        [Inject] public IPublisher<EntityHealthChanged> _healthChanged = default!;
        IEntity IProvide<IEntity>.Value() => this;

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
        [Export]
        public EntityStat? ConfigData { get; set; }
        [Node(nameof(MeleeAttackDetectNodes))]
        public Node2D MeleeAttackDetectNodes { get; set; } = default!;
        public RuntimeStats RuntimeStats { get; set; } = default!;

        public override void _Ready()
        {
            base._Ready();
            Logger = _loggerFactory.CreateLogger(GetType());
            CharacterBody2D = this;

            // 在 Ready 时，ConfigData 已经由 Godot 引擎完成注入
            if (ConfigData != null)
            {
                RuntimeStats = new RuntimeStats(ConfigData);
            }
            else
            {
                RuntimeStats = new RuntimeStats();
                GD.PrintErr($"{Name}: BuildingBase 缺少 ConfigData 配置！");
            }
            // Call the this.Provide() method once your dependencies have been initialized.
            this.Provide();
        }
        public void OnResolved()
        {
            // 进入屏幕发信号
            VisibilityNotifier.ScreenEntered += () =>
            {
                Logger.LogInfoWithNode(this, "角色进入了屏幕!");
                _spottedPub.Publish(new EntitySpotted(this, RuntimeStats.CurrentHp, RuntimeStats.MaxHp));
            };
            // 离开屏幕发信号
            VisibilityNotifier.ScreenExited += () =>
            {
                Logger.LogInfoWithNode(this, "角色退出了屏幕!");
                _gonePub.Publish(new EntityGone(this));
            };

            // 手动触发一次初始状态
            if (VisibilityNotifier.IsOnScreen())
            {
                Logger.LogInfoWithNode(this, "角色初始在屏幕上!");
                _spottedPub.Publish(new EntitySpotted(this, RuntimeStats.CurrentHp, RuntimeStats.MaxHp));
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
            if (FacingDirection > 0 && MeleeAttackDetectNodes.Scale.X < 0)
            {
                MeleeAttackDetectNodes.Scale = new Vector2(1, MeleeAttackDetectNodes.Scale.Y);
            }
            else if (FacingDirection < 0 && MeleeAttackDetectNodes.Scale.X > 0)
            {
                MeleeAttackDetectNodes.Scale = new Vector2(-1, MeleeAttackDetectNodes.Scale.Y);
            }
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

        /// <summary>
        /// 受到攻击
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="attackValue"></param>
        public virtual void TakeDamage(IEntity attacker, float attackValue)
        {
            RuntimeStats.CurrentHp -= attackValue;

            _healthChanged.Publish(new EntityHealthChanged(this, RuntimeStats.CurrentHp, RuntimeStats.MaxHp));
        }

    }
}
