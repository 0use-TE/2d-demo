namespace CharacterModule.StateMachineModule
{
    public abstract class BaseState
    {
        //每个状态都要填写过渡条件,回调里面干什么
        public event Action? OnEnter;
        public event Action? OnExit;
        public event Action<double>? OnPhysicsProcess;
        public Dictionary<Func<bool>, BaseState> Transitions = new Dictionary<Func<bool>, BaseState>();
        private StateMachine _stateMachine;
        public BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void Enter() => OnEnter?.Invoke();
        public void Process(double delta)
        {
            foreach (var transition in Transitions)
            {
                if (transition.Key())
                {
                    //切换状态
                    _stateMachine.ChangeState(transition.Value);
                    break;
                }
            }
        }
        public void PhysicsProcess(double delta) => OnPhysicsProcess?.Invoke(delta);
        public void Exit() => OnExit?.Invoke();
    }
}
