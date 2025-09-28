using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
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

        public override void _Ready()
		{
			base._Ready();
			//设置阵营
			TeamType = Misc.Enums.E_TeamType.Player;
            ConfigureStateMachine();
			InitChatacterStats();
            AnimationPlayer.AnimationFinished += AnimationPlayer_AnimationFinished;
        }
		public abstract void InitChatacterStats();

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
