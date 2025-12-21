using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Dash.Scripts.Entity.AI.AIPerception.Core;
using Dash.Scripts.Entity.AI.AIPerception.Imps;
using Dash.Scripts.Entity.Core;
using Dash.Scripts.GameIn.EnvironmentContext;
using Dash.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Dash.Scripts.Entity.AI;
[Meta(typeof(IAutoNode))]
public partial class TestAi : AIBase
{
    public override void _Notification(int what) => this.Notify(what);

    protected override void ConfigurateTargetPenetration(IList<ITargetPerception> targetPerceptions)
    {
        targetPerceptions.Add(new TestTargetPerceptionImp());
    }

}
