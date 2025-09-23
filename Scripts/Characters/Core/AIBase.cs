using CharacterModule.BehaviourTree;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Godot;

namespace DDemo.Scripts.Characters.Core
{
    /// <summary>
    /// AI后面判断需要各种感知组件的支持（即环境上下文），目前是通过GameInManager暴露的(通过AutoInject),
	/// 先描述下AI需要的信息，首先是玩家(攻击谁)，其次是队伍信息（友军，敌军），其他信息(比如地图，)
    /// </summary>
    [Meta(typeof(IAutoNode))]
    public abstract partial class AIBase: CharacterBase, IAI 
	{
        public override void _Notification(int what) => this.Notify(what);

        public BehaviorTree BehaviorTree { get; protected set; } = default!;
		[Node(nameof(NavigationAgent2D))]
		public NavigationAgent2D NavigationAgent2D { get; private set; } = default!;
		[Dependency]
		protected PlayerContext PlayerContext => this.DependOn<PlayerContext>();
		[Dependency]
		protected AIUnitContext AIUnitContext =>this.DependOn<AIUnitContext>();
		[Dependency]
		protected MapContext MapContext=>this.DependOn<MapContext>();
        public TargetContext TargetContext { get; private set; } =new TargetContext();

        public override void _Ready()
		{
			base._Ready();
			BehaviorTree = BehaviorTree.CreateTree();
		}

        public void OnResolved()
        {
			BehaviorTree.ConfigurateBlackboard(blackboard =>
			{
                blackboard.Save(this);
				blackboard.Save(StateMachine);
                blackboard.Save(PlayerContext);
				blackboard.Save(AIUnitContext);
				blackboard.Save(MapContext);
				//
				blackboard.Save(TargetContext);

				blackboard.Save(_logger);
			});

            ConfigureStateMachine();
            ConfigureBehaviourTree();
        }
        
		protected abstract void ConfigureStateMachine();
		protected abstract void ConfigureBehaviourTree();

		public override void _Process(double delta)
		{
			base._Process(delta);
		}
		public override void _PhysicsProcess(double delta)
		{
			//BT tick
			BehaviorTree.Tick(delta);
		}
	}
}
