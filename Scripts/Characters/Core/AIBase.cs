using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	public partial class AIBase : CharacterBase, IAI
	{
		public BehaviorTree ?BehaviorTree { get; private set; } 
		public NavigationAgent2D NavigationAgent2D { get; private set; } = default!;
		public override void _Ready()
		{
			base._Ready();
			NavigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
		}
	}
}
