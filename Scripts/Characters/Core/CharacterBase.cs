using CharacterModule.StateMachineModule;
using DDemo.Scripts.Misc.Enums;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using PlatformExplorer.PlayerScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
public	abstract partial class CharacterBase: CharacterBody2D, ICharacter 
	{
		public CharacterBody2D CharacterBody2D { get; private set; } = default!;

		public AnimatedSprite2D AnimatedSprite2D { get; private set; } = default!;

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
			AnimatedSprite2D = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
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
            if (_loggerFactory == null)
            {
                _logger.LogInformation("进入");
            }

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
