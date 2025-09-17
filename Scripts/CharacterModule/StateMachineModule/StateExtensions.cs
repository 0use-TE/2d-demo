namespace CharacterModule.StateMachineModule
{
    public static class StateExtensions
    {
        public static BaseState AddTransitions(
            this BaseState state,
            params (Func<bool> condition, BaseState nextState)[] transitions)
        {
            foreach (var (condition, nextState) in transitions)
            {
                state.Transitions.Add(condition, nextState);
            }
            return state;
        }
        public static BaseState AddTransitions(this BaseState state, Func<bool> transition, BaseState nextState)
        {
            state.Transitions.Add(transition, nextState);
            return state;
        }
        public static BaseState AddEnter(this BaseState state, Action action)
        {
            state.OnEnter += action;
            return state;
        }
        public static BaseState AddExit(this BaseState state, Action action)
        {
            state.OnExit += action;
            return state;
        }
        public static BaseState AddPhysicsProcess(this BaseState state, Action<double> action)
        {
            state.OnPhysicsProcess += action;
            return state;
        }
    }
}
