using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTreeModule.CompositeNodes
{
    /// <summary>
    /// 子节点按照顺序执行，前面可以立即中断，没有中断，最后返回Runing
    /// </summary>
    public class RuningExitParallel : CompositeNode
    {

        public override NodeState Tick(double delta)
        {
            if(children.Count==0)
            {
                return NodeState.Failure;
            }

            for(int i = 0; i < children.Count; i++)
            {
                var tick= children[i].Tick(delta);
                if(tick==NodeState.Failure)
                    return NodeState.Failure;
                if(tick== NodeState.Success)
                    return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}
