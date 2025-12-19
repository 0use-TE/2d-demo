using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.Core.Stats
{
    [GlobalClass]
    public partial class EntityStat:Resource
    {
        [ExportGroup("Base Stats")]
        [Export] public float MaxHp = 100.0f;
        [Export] public float MoveSpeed = 50.0f;
        
        [ExportGroup("Abilities")]
        [Export] public Array<AttackStat> Attacks=new Array<AttackStat>();
    }
}
