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
	public interface ICharacter
	{
        E_TeamType TeamType { get; set; }

		CharacterBody2D CharacterBody2D { get; }
		AnimationPlayer AnimationPlayer { get; }
		AnimatedSprite2D AnimatedSprite2D { get;  }
		AnimationTree AnimationTree { get; }
		StateMachine StateMachine { get; }
		int FacingDirection { get; set; } 
		void SetVelocity(float? x=null, float? y=null);
		void AddVelocity(float? x = null, float? y = null);
		void Filp();
	}
}
