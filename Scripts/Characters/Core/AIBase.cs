using CharacterModule.BehaviourTree;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.CharacterParts.PerceptionPart;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using System.Collections.Generic;

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
		[Node(nameof(Area2D))]
		protected Area2D Area2D { get; private set; } = default!;
        public TargetContext TargetContext { get; private set; } =new TargetContext();
		private IList<CharacterBase> _characters=new List<CharacterBase>();

		private IList<ITargetEvaluator> targetEvaluators = new List<ITargetEvaluator>();
		private Timer? _evaluationTimer;
		public override void _Ready()
		{
			base._Ready();
			BehaviorTree = BehaviorTree.CreateTree();
			Area2D.BodyEntered += Area2D_BodyEntered; ;
			Area2D.BodyExited += Area2D_BodyExited; ;

			_evaluationTimer = new Timer()
			{
				Autostart = true,
				WaitTime = GlobalConstant.PerceptionIntervel,
				OneShot = false
			};
			_evaluationTimer.Timeout += OnEvaluationTimeout;
			AddChild(_evaluationTimer);
		}

		private void Area2D_BodyExited(Node2D body)
		{
			if (body is CharacterBase character)
			{
				if (character.TeamType != TeamType)
				{
					_logger.LogInformationWithNodeName(this, $"角色{character.Name}退出了攻击范围");
					_characters.Remove(character);
				}
			}
			else
			{
				_logger.LogInformationWithNodeName(this, $"刚体{body.Name}退出了攻击范围");
			}
		
		}

		private void Area2D_BodyEntered(Node2D body)
		{
			if (body is CharacterBase character)
			{
				if (character.TeamType != TeamType)
				{
					_logger.LogInformationWithNodeName(this, $"角色{character.Name}进入了攻击范围");
					_characters.Add(character);
				}
			}
			else
			{
				_logger.LogInformationWithNodeName(this, $"刚体{body.Name}进入了攻击范围");
			}
		}

		/// <summary>
		/// 用于决策AI的
		/// </summary>
		private void OnEvaluationTimeout()
		{
			foreach (var evaluator in targetEvaluators)
			{
				_logger.LogInformationWithNodeName(this, $"执行了策略{evaluator.GetType().Name}");
				evaluator.Evaluate(this,TargetContext,_characters, MapContext,_logger);
			}
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
			ConfigurateTargetEvaluator(targetEvaluators);
		}
        
		protected abstract void ConfigureStateMachine();
		protected abstract void ConfigureBehaviourTree();
		protected abstract void ConfigurateTargetEvaluator(IList<ITargetEvaluator> targetEvaluators);


		public override void _Process(double delta)
		{
			base._Process(delta);
		}
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			//BT tick
			BehaviorTree.Tick(delta);
		}
	}
}
