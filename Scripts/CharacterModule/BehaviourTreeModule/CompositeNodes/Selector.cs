using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.CompositeNodes
{
    public class Selector:CompositeNode
    {
        private int currentChildIndex = 0;

        public override NodeState Tick(double delta)
        {
            // 从当前子节点开始尝试
            for (int i = currentChildIndex; i < children.Count; i++)
            {
                var status = children[i].Tick(delta);
                if (status == NodeState.Running)
                {
                    currentChildIndex = i; // 记录当前运行的子节点
                    return NodeState.Running;
                }
                if (status == NodeState.Success)
                {
                    currentChildIndex = 0; // 重置索引
                    return NodeState.Success;
                }
            }

            // 所有子节点失败，重置索引
            currentChildIndex = 0;
            return NodeState.Failure;
        }
    }

}
