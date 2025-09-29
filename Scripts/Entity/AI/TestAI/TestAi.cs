using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.AI.AIPerception.Core;
using DDemo.Scripts.Entity.AI.AIPerception.Imps;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

[Meta(typeof(IAutoNode))]
public partial class TestAi : AIBase
{
    public override void _Notification(int what) => this.Notify(what);

    protected override void ConfigurateTargetPenetration(IList<ITargetPerception> targetPerceptions)
    {
        targetPerceptions.Add(new TestTargetPerceptionImp());
    }
}
