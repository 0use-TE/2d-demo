using CharacterModule.BehaviourTree;
using CharacterModule.StateMachineModule;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.GameIn;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc.Enums;
using Godot;
using Godot.DependencyInjection.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public NavigationAgent2D NavigationAgent2D { get; private set; } = default!;

		public E_TeamType TeamType { get;protected set; } = E_TeamType.Neutral;

		[Dependency]
		protected PlayerContext PlayerContext => this.DependOn<PlayerContext>();
		[Dependency]
		protected AIUnitContext AIUnitContext =>this.DependOn<AIUnitContext>();

        public TargetContext TargetContext { get; private set; } =new TargetContext();

        public override void _Ready()
		{
			base._Ready();
			NavigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
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
				blackboard.Save(TargetContext);

				blackboard.Save(_logger);
			});

            ConfigureStateMachine();
            ConfigureBehaviourTree();
        }
        
		protected abstract void ConfigureStateMachine();
		protected abstract void ConfigureBehaviourTree();

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			base._Process(delta);
		}
		public override void _PhysicsProcess(double delta)
		{
			//BT tick
			BehaviorTree?.Tick(delta);
			//StateMachine's process.
			StateMachine.PhysicsProcess(delta);
		}
	}
}
