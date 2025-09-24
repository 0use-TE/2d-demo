using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Blackboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.Core
{
    public abstract class BehaviourNode:INode
    {
        protected ICompositeNode? _parent;
        public IBlackboard Blackboard { get; set; } = new DefaultBlackboard();

        public  virtual  void SetParent(ICompositeNode parent)
        {
            _parent = parent;
            Blackboard= parent.Blackboard;
            OnBlackboardCreated();
        }
        protected virtual void OnBlackboardCreated()
        {

        }

        public abstract NodeState Tick(double delta);
    }
}
