
using CharacterModule.StateMachineModule;

namespace DDemo.Scripts.Characters.Players
{
    public partial class Player
    {
        internal class PlayerIdleState : BaseState
        {
            public PlayerIdleState(StateMachine sm) : base(sm) { }
        }
        internal class PlayerWalkState : BaseState
        {
            public PlayerWalkState(StateMachine sm) : base(sm) { }
        }
        internal class PlayerMeleeAttackState : BaseState
        {
            public PlayerMeleeAttackState(StateMachine sm) : base(sm) { }
        }

        internal class PlayerRemoteAttackState : BaseState
        {
            public PlayerRemoteAttackState(StateMachine sm) : base(sm) { }
        }
    }
}

