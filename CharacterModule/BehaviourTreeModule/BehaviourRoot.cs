using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree
{
    public class BehaviourRoot : CompositeNode,IRoot
    {
        public override NodeState Tick(double delta)
        {
            if (children.Count == 0)
            {
                return NodeState.Failure; // 如果没有子节点，返回失败
            }
            var status = children[0].Tick(delta); // 只执行第一个子节点
            return status;
        }
    }
}
