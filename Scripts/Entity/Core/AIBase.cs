using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.AI.AIPerception.Core;
using DDemo.Scripts.Entity.AI.AIPerception.Imps;
using DDemo.Scripts.Entity.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc;
using DDemo.Scripts.Misc.Enums;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
    [Meta(typeof(IAutoNode))]
    public abstract partial class AIBase:CharacterBase
    {
        public override void _Notification(int what) => this.Notify(what);

        [Node(nameof(BTPlayer))]
        protected BTPlayer BTPlayer { get; set; } = default!;

        [Node(nameof(Area2D))]
        private Area2D Area2D { get; set; } = default!;

        private Timer? _timer;
        private IList<CharacterBase> _characters = new List<CharacterBase>();
        private IList<BuildingBase> _buildings = new List<BuildingBase>();
        private IList<ITargetPerception> _targetPerceptions = new List<ITargetPerception>();

        [Dependency]
        public MapContext MapContext => this.DependOn<MapContext>();

        [Node(nameof(NavigationAgent2D))]
        public NavigationAgent2D NavigationAgent2D { get; set; } = default!;
        public TargetContext TargetContext { get; set; } = new TargetContext();

        public override void _Ready()
        {
            base._Ready();
            TeamType = E_TeamType.Enemy;
            _timer = new Timer()
            {
                Autostart = true,
                WaitTime = GlobalConstant.PerceptionIntervel
            };
            _timer.Timeout += _timer_Timeout;
            AddChild(_timer);
            Area2D.BodyEntered += Area2D_BodyEntered;
            Area2D.BodyExited += Area2D_BodyExited;
            //配置感知
            ConfigurateTargetPenetration(_targetPerceptions);
            BTPlayer.Blackboard.Set(this);
        }

        public void OnResolved()
        {
            //配置黑板
            //存储AI本身
        }
        private void Area2D_BodyEntered(Node2D body)
        {
            if (body is CharacterBase character)
            {
                if (character == this)
                    return;
                if (character.TeamType != TeamType)
                {
                    ILogger.LogInformationWithNodeName(this, $"角色{character.Name}进入了攻击范围!");
                    _characters.Add(character);
                }
                else
                {
                    ILogger.LogInformationWithNodeName(this, $"非敌对角色{character.Name}进入了攻击范围!");
                }
            }
            else
            {
                ILogger.LogInformationWithNodeName(this, $"非角色{body.Name}进入了攻击范围!");
            }
        }
        private void Area2D_BodyExited(Node2D body)
        {
            if (body is CharacterBase character)
            {
                if (character.TeamType != TeamType)
                {
                    ILogger.LogInformationWithNodeName(this, $"角色{character.Name}退出了攻击范围!");
                    if (TargetContext.CurrentTarget.TargetNode == character)
                        TargetContext.CurrentTarget.TargetNode =null;

                    _characters.Remove(character);
                }
                else
                {
                    ILogger.LogInformationWithNodeName(this, $"非敌对角色{character.Name}退出了攻击范围!");
                }
            }
            else
            {
                ILogger.LogInformationWithNodeName(this, $"非角色{body.Name}退出了攻击范围!");
            }
        }

        protected abstract  void ConfigurateTargetPenetration(IList<ITargetPerception> targetPerceptions);

        private void _timer_Timeout()
        {
            //检测策略
            foreach (var item in _targetPerceptions)
            {
                ILogger.LogInformationWithNodeName(this, $"执行了策略{item.GetType().Name}");
                item.TargetPerception(this, _characters, _buildings, MapContext, ILogger, TargetContext);
            }
        }
        public void FacingTarget()
        {

        }
    }
}
