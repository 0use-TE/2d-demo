using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Dash.Scripts.Entity.Core.Stats;
using Dash.Scripts.GameIn.EnvironmentContext;
using Dash.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dash.Scripts.Entity.AI
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
