using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Dash.Scripts.Entity.Core.AttackSystem.Core;
using Dash.Scripts.Entity.Core.Stats;
using Dash.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.Core
{
    public interface IEntity : IDamageable
    {
        E_TeamType TeamType { get; set; }
        E_ControllerType ControllerType { get; set; }
        [Export]
        EntityStat? ConfigData { get; set; }
        RuntimeStats RuntimeStats { get; }
    }
}
