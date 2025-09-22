
using CharacterModule.StateMachineModule;

namespace PlatformExplorer.BehaviorTreeTest
{
	public class EnemyIdle : BaseState
	{
		public EnemyIdle(StateMachine stateMachine) : base(stateMachine)
		{
		}
	}
	public class EnemyFollow : BaseState
	{
		public EnemyFollow(StateMachine stateMachine) : base(stateMachine)
		{
		}
	}
    public class MeleeAttack : BaseState
    {
        public MeleeAttack(StateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}
