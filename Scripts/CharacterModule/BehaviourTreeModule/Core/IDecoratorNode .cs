using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTreeModule.Core
{
	internal interface IDecoratorNode:ICompositeNode
	{
		INode? Child { get; }
	}
}
