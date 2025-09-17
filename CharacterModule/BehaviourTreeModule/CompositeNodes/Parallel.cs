using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.CompositeNodes
{
    // 并行策略
    public enum ParallelPolicy
    {
        AllSuccess, // 所有子节点成功时返回 Success，任一失败时返回 Failure
        AnySuccess  // 任一子节点成功时返回 Success，所有失败时返回 Failure
    }

    // Parallel 节点
    public class Parallel : CompositeNode
    {
        private ParallelPolicy successPolicy;
        private ParallelPolicy failurePolicy;

        // 构造函数
        public Parallel(ParallelPolicy successPolicy, ParallelPolicy failurePolicy)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
        }

        public override NodeState Tick(double delta)
        {
            bool hasRunning = false;
            int successCount = 0;
            int failureCount = 0;

            // 并行执行所有子节点
            foreach (var child in children)
            {
                var status = child.Tick(delta);
                switch (status)
                {
                    case NodeState.Success:
                        successCount++;
                        break;
                    case NodeState.Failure:
                        failureCount++;
                        break;
                    case NodeState.Running:
                        hasRunning = true;
                        break;
                }
            }

            // 检查 Failure Policy
            if (failurePolicy == ParallelPolicy.AnySuccess && failureCount > 0)
            {
                return NodeState.Failure; // 任一失败返回 Failure
            }
            if (failurePolicy == ParallelPolicy.AllSuccess && failureCount == children.Count)
            {
                return NodeState.Failure; // 所有失败返回 Failure
            }

            // 检查 Success Policy
            if (successPolicy == ParallelPolicy.AnySuccess && successCount > 0)
            {
                return NodeState.Success; // 任一成功返回 Success
            }
            if (successPolicy == ParallelPolicy.AllSuccess && successCount == children.Count)
            {
                return NodeState.Success; // 所有成功返回 Success
            }

            // 如果有子节点 Running，则返回 Running
            return hasRunning ? NodeState.Running : NodeState.Failure;
        }

        // 工厂方法
        public static ICompositeNode Create(ParallelPolicy successPolicy = ParallelPolicy.AllSuccess, ParallelPolicy failurePolicy = ParallelPolicy.AnySuccess)
        {
            return new Parallel(successPolicy, failurePolicy);
        }

        // Fluent API：设置策略
        public Parallel SetSuccessPolicy(ParallelPolicy policy)
        {
            successPolicy = policy;
            return this;
        }

        public Parallel SetFailurePolicy(ParallelPolicy policy)
        {
            failurePolicy = policy;
            return this;
        }
    }
}
