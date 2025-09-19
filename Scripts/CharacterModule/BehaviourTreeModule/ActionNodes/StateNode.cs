using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;

namespace CharacterModule.BehaviourTree.ActionNodes
{
    public class StateNode : BehaviourNode
    {
        //StateMachine
        private StateMachine? _stateMachine;
        private BaseState _state;
        public StateNode( BaseState baseState)
        {
            _state = baseState;
        }
        public override NodeState Tick(double delta)
        {
            SwitchState();
            return NodeState.Success;
        }
        protected override void OnBlackboardCreated()
        {
            _stateMachine = Blackboard.Load<StateMachine>();
        }
        private void SwitchState()
        {
            var currentState = _stateMachine?.GetCurrentState();
            if (currentState != _state)
            {
                _stateMachine?.ChangeState(_state);
            }
            else
            {

            }
        }
    }
}
