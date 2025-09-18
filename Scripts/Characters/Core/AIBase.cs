using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using DDemo.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	public abstract partial class AIBase : CharacterBase, IAI
	{
		public BehaviorTree BehaviorTree { get; protected set; } = default!;
		public NavigationAgent2D NavigationAgent2D { get; private set; } = default!;

		public E_TeamType TeamType { get;protected set; } = E_TeamType.Neutral;

		public override void _Ready()
		{

			base._Ready();
			NavigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
			BehaviorTree = BehaviorTree.CreateTree().ConfigurateStateMachine(StateMachine)
				.ConfigurateBlackboard(blackboard =>
				{
					blackboard.Save(this);
				});
			ConfigureStateMachine();
			ConfigureBehaviourTree();
		}
		protected abstract void ConfigureStateMachine();
		protected abstract void ConfigureBehaviourTree();

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			base._Process(delta);
		}
		public override void _PhysicsProcess(double delta)
		{
			//BT tick
			BehaviorTree?.Tick(delta);
			//StateMachine's process.
			StateMachine.PhysicsProcess(delta);
		}

	}
}
