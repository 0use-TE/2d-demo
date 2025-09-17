using CharacterModule.BehaviourTree;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
	public interface IAI:ICharacter
	{
		BehaviorTree? BehaviorTree { get; }
		NavigationAgent2D NavigationAgent2D { get; }
	}
}
