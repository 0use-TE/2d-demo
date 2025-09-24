using CharacterModule.BehaviourTree.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTreeModule.Core
{
	// 基础装饰节点
	public abstract class DecoratorNode : BehaviourNode, IDecoratorNode
	{
		public INode? Child { get; private set; }
		private ILogger? _logger;
		protected override void OnBlackboardCreated()
		{
			_logger = Blackboard.Load<ILogger>();
		}
		public ICompositeNode AddChild(INode child)
		{
			if(Child!=null)
			{
				_logger?.LogInformation("装饰节点只能有一个子节点");
				return this;
			}
			Child= child;
			child.SetParent(this);
			return this;
		}

		public ICompositeNode? GetParent()
		{
			return _parent;
		}
	}
}
