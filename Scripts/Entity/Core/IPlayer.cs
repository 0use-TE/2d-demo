using CharacterModule.StateMachineModule;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
	public interface IPlayer:ICharacter
	{
        StateMachine StateMachine { get; }

    }
}
