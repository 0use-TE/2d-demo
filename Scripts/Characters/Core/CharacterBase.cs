using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Misc.Enums;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	[Meta(typeof(IAutoNode))]
public	abstract partial class CharacterBase: CharacterBody2D, ICharacter 
	{
		public override void _Notification(int what) => this.Notify(what);
		public CharacterBody2D CharacterBody2D { get; private set; } = default!;

		// 后台字段
		private AnimatedSprite2D _animatedSprite2D = default!;
		private AnimationPlayer _animationPlayer = default!;
		private AnimationTree _animationTree = default!;

		[Node(nameof(AnimatedSprite2D))]
		public AnimatedSprite2D AnimatedSprite2D
		{
			get => _animatedSprite2D;
			private set => _animatedSprite2D = value ?? throw new InvalidOperationException(
				$"AnimatedSprite2D 没有正确注入！请检查角色实例是否放置了该节点。");
		}

		[Node(nameof(AnimationPlayer))]
		public AnimationPlayer AnimationPlayer
		{
			get => _animationPlayer;
			private set => _animationPlayer = value ?? throw new InvalidOperationException(
				"AnimationPlayer 没有正确注入！请检查角色实例是否放置了该节点。");
		}

		[Node(nameof(AnimationTree))]
		public AnimationTree AnimationTree
		{
			get => _animationTree;
			private set => _animationTree = value ?? throw new InvalidOperationException(
				"AnimationTree 没有正确注入！请检查角色实例是否放置了该节点。");
		}
		public StateMachine StateMachine { get; private set; } = new StateMachine();

        public E_TeamType TeamType { get; set; }

        protected ILogger _logger = default!;

        [Inject]
        public ILoggerFactory _loggerFactory = default!;


        public int FacingDirection { get; set; } = 1; // 1表示向右，-1表示向左
		public override void _Ready()
		{
			base._Ready();
			_logger = _loggerFactory.CreateLogger(GetType());
			CharacterBody2D = this;
			AnimationTree.AnimationFinished += AnimationTree_AnimationFinished; 
		}

		protected abstract void AnimationTree_AnimationFinished(StringName animName);

		public void AddVelocity(float? x = null, float? y = null)
		{
			var velocity = Velocity;
			if (x.HasValue) velocity.X = x.Value;
			if (y.HasValue) velocity.Y = y.Value;
			Velocity = velocity;
		}
		public override void _Process(double delta)
		{
			_logger.LogInformationWithNodeName(this, "当前状态:" + StateMachine.GetCurrentState().GetType().Name);
			base._Process(delta);
			Filp();
		}
		
		public void Filp()
		{
			if (Velocity.X > 0 && FacingDirection < 0)
			{
				FacingDirection = 1;
				AnimatedSprite2D.FlipH = false;
			}
			else if (Velocity.X < 0 && FacingDirection > 0)
			{
				FacingDirection = -1;
				AnimatedSprite2D.FlipH = true;
			}
		}

		public void SetVelocity(float? x = null, float? y = null)
		{
			var velocity = Velocity;
			if (x.HasValue) velocity.X = x.Value;
			if (y.HasValue) velocity.Y = y.Value;
			Velocity = velocity;
		}
	}
}
