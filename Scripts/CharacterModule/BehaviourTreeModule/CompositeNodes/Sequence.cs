using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.CompositeNodes
{
    // Sequence 节点
    public class Sequence : CompositeNode
    {
        private int currentChildIndex = 0;

        public override NodeState Tick(double delta)
        {
            // 从当前子节点开始执行
            for (int i = currentChildIndex; i < children.Count; i++)
            {
                var status = children[i].Tick(delta);
                if (status == NodeState.Running)
                {
                    currentChildIndex = i; // 记录当前运行的子节点
                    return NodeState.Running;
                }
                if (status == NodeState.Failure)
                {
                    currentChildIndex = 0; // 重置索引
                    return NodeState.Failure;
                }
            }

            // 所有子节点成功，重置索引
            currentChildIndex = 0;
            return NodeState.Success;
        }
    }
}
