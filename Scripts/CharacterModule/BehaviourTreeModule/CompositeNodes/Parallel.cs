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
        All, // 所有子节点成功时返回 Success，任一失败时返回 Failure
        Any // 任一子节点成功时返回 Success，所有失败时返回 Failure
    }
	public enum RunningPolicy
	{
		WaitAny, // 只要有一个子节点 Running，就保持 Running
		WaitAll  // 所有子节点结束后，再根据成功/失败策略返回
	}
	// Parallel 节点
	public class Parallel : CompositeNode
    {
        private ParallelPolicy successPolicy;
        private ParallelPolicy failurePolicy;
		private RunningPolicy runningPolicy;

		// 构造函数
		public Parallel(ParallelPolicy successPolicy, ParallelPolicy failurePolicy, RunningPolicy runningPolicy = RunningPolicy.WaitAny)
		{
			this.successPolicy = successPolicy;
			this.failurePolicy = failurePolicy;
			this.runningPolicy = runningPolicy;
		}


		public override NodeState Tick(double delta)
		{
			bool hasRunning = false;
			int successCount = 0;
			int failureCount = 0;

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

			// 先判断失败策略
			if (failurePolicy == ParallelPolicy.Any && failureCount > 0)
				return NodeState.Failure;
			if (failurePolicy == ParallelPolicy.All && failureCount == children.Count)
				return NodeState.Failure;

			// 再判断成功策略
			if (successPolicy == ParallelPolicy.Any && successCount > 0)
				return NodeState.Success;
			if (successPolicy == ParallelPolicy.All && successCount == children.Count)
				return NodeState.Success;

			// 处理 Running 策略
			if (hasRunning)
			{
				if (runningPolicy == RunningPolicy.WaitAny)
				{
					// 只要有一个在 Running，就返回 Running
					return NodeState.Running;
				}
				else if (runningPolicy == RunningPolicy.WaitAll)
				{
					// 必须等所有结束才返回结果
					return NodeState.Running;
				}
			}

			// 如果没有 Running，兜底 -> Failure
			return NodeState.Failure;
		}
	}
}
