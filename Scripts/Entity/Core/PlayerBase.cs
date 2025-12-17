using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using DDemo.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
    [Meta(typeof(IAutoNode))]

    public abstract partial class PlayerBase : CharacterBase, IPlayer
	{
        public override void _Notification(int what) => this.Notify(what);

		public StateMachine StateMachine { get; private set; } = new StateMachine();
		private BlackboardPlan ?_blackboardPlan;
		[Export]
		private BlackboardPlan BlackboardPlan {
			get => _blackboardPlan??throw new NullReferenceException("没有设置数值呢，请点击角色为其添加黑板😋");
			set => _blackboardPlan = value;
		}


		private Blackboard _blackboard=default!;

		public T GetVar<T>(string key)
		{
			return _blackboard.GetVar<T>(key);
        }
        public override void _Ready()
		{
			base._Ready();
			//创建黑板
			_blackboard=BlackboardPlan.CreateBlackboard(this);

			//设置阵营
			TeamType = E_TeamType.Player;
            ConfigureStateMachine();
            AnimationPlayer.AnimationFinished += AnimationPlayer_AnimationFinished;
        }
		protected abstract void AnimationPlayer_AnimationFinished(StringName animName);

        protected abstract void ConfigureStateMachine();

		/// <summary>
		/// Called once per frame
		/// </summary>
		/// <param name="delta"></param>
		public override void _Process(double delta)
		{
			base._Process(delta);
			StateMachine?.Process(delta);
		}
		public override void _PhysicsProcess(double delta)
		{
			StateMachine.PhysicsProcess(delta);
			MoveAndSlide();
		}
	}
}
