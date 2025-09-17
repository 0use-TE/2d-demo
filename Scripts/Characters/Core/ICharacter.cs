using CharacterModule.StateMachineModule;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	public interface ICharacter
	{
		CharacterBody2D CharacterBody2D { get; }
		AnimatedSprite2D AnimatedSprite2D { get;  }
		StateMachine StateMachine { get; }
		int FacingDirection { get; set; } 
		void SetVelocity(float? x=null, float? y=null);
		void AddVelocity(float? x = null, float? y = null);
		void Filp();
	}
}
