using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using DDemo.Scripts.Entity.Core.Stats;
using DDemo.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
    public interface IEntity:IDamageable
    {
        E_TeamType TeamType { get; set; }
        [Export] 
        EntityStat? ConfigData { get; set; }
    }
}
