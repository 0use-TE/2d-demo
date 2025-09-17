using CharacterModule.StateMachineModule;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	 public abstract partial class PlayerBase : CharacterBase, IPlayer
	{
		public override void _Ready()
		{
			base._Ready();
			BuildStateMachine();
		}

		protected abstract void BuildStateMachine();

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
