using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.ActionNodes
{
    public class ConditionNode : BehaviourNode
    {
        private Func<double, bool> _condition;
        public ConditionNode(Func<double, bool> condition)
        {
            _condition = condition;
        }

        public override NodeState Tick(double delta)
        {
            return _condition(delta) ? NodeState.Success : NodeState.Failure;
        }
    }
}
