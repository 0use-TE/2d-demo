using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;
using System;

namespace CharacterModule.BehaviourTree
{
    public enum NodeState
    {
        Success,
        Failure,
        Running
    }
    public class BehaviorTree
    {
        private readonly IRoot _root;
        public BehaviorTree(IRoot root)
        {
            _root = root;
        }   
        public void Tick(double delta)
        {
            _root.Tick(delta);
        }
        public static BehaviorTree CreateTree()
        {
            return new BehaviorTree(new BehaviourRoot());
        }
        public BehaviorTree ConfigurateBlackboard(Action<IBlackboard> configAction)
        {
            configAction.Invoke(_root.Blackboard);
            return this;
        }
        public BehaviorTree ConfigurateStateMachine(StateMachine stateMachine)
        {
            _root.Blackboard.Save(stateMachine);
            return this;
        }
        public ICompositeNode BuildTree()
        {
            return _root;
        }
    }
}
