using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.ActionNodes
{
    // 动作节点
    public class ActionNode : BehaviourNode
    {
        private Func<double, NodeState> action;

        public ActionNode(Func<double, NodeState> action)
        {
            this.action = action;
        }

        public override NodeState Tick(double delta)
        {
            return action?.Invoke(delta) ?? NodeState.Failure;
        }


    }
}