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

namespace DDemo.Scripts.Characters.Core
{
    [Meta(typeof(IAutoNode))]

    public abstract partial class PlayerBase : CharacterBase, IPlayer
	{
        public override void _Notification(int what) => this.Notify(what);

        private AnimationTree _animationTree = default!;
        public StateMachine StateMachine { get; private set; } = new StateMachine();

        [Node(nameof(AnimationTree))]
        public AnimationTree AnimationTree
        {
            get => _animationTree;
            private set => _animationTree = value ?? throw new InvalidOperationException(
                "AnimationTree 没有正确注入！请检查角色实例是否放置了该节点。");
        }
        public override void _Ready()
		{
			base._Ready();
			//设置阵营
			TeamType = Misc.Enums.E_TeamType.Player;
            AnimationTree.AnimationFinished += AnimationTree_AnimationFinished;
            ConfigureStateMachine();
        }
        protected abstract void AnimationTree_AnimationFinished(StringName animName);


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
