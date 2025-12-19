using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core.Stats;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDemo.Scripts.Entity.AI
{
    [Obsolete]
    [Meta(typeof(IAutoNode))]
    public partial class AIBTInitValueNode : BTPlayer
    {
        public override void _Notification(int what) => this.Notify(what);
        [Dependency]
        public EntityStat EntityStat => this.DependOn<EntityStat>();
        [Inject]
        public ILogger<AIBTInitValueNode> Logger { get; set; } = default!;
        public void OnResolved()
        {
            Logger.LogInfoWithNode(this, "初始化角色数据!");
            Blackboard.Set("MoveSpeed", EntityStat.MoveSpeed);
        }
    }
}
