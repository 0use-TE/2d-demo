using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using CharacterModule.BehaviourTreeModule.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTreeModule.DecorateNodes
{
	public class Decorator_Inverter: DecoratorNode
	{
		public override NodeState Tick(double delta)
		{
			var status = Child?.Tick(delta);

			return status switch
			{
				NodeState.Success => NodeState.Failure,
				NodeState.Failure => NodeState.Success,
				NodeState.Running => NodeState.Running,
				_ => NodeState.Failure
			};
		}
	}
}
