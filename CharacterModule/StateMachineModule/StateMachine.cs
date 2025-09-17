namespace CharacterModule.StateMachineModule
{
    // 状态机管理器
    public class StateMachine
    {

        private BaseState _currentState;
        public BaseState GetCurrentState() => _currentState;
        public StateMachine()
        {
            _currentState = new EmptyState(this);
        }
        public void SetInitialState(BaseState initialState)
        {
            _currentState = initialState;
            _currentState.Enter();
        }

        // 切换状态
        public void ChangeState(BaseState nextState)
        {
            _currentState.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        public void Process(double delta) => _currentState?.Process(delta);
        public void PhysicsProcess(double delta) => _currentState?.PhysicsProcess(delta);
    }
}
