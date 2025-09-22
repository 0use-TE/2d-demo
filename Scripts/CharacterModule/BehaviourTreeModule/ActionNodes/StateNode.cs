using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;
using System.ComponentModel;

namespace CharacterModule.BehaviourTree.ActionNodes
{
    public class StateNode : BehaviourNode
    {
        //StateMachine
        private StateMachine? _stateMachine;
        private BaseState _state;
        private bool _canRepeate;
        public StateNode( BaseState baseState,bool canRepeate)
        {
            _state = baseState;
            _canRepeate = canRepeate;
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
            if (currentState == null) return;

            if (currentState.GetType() == _state.GetType() && !_canRepeate)
                return;

            _stateMachine?.ChangeState(_state);
        }

    }
}
