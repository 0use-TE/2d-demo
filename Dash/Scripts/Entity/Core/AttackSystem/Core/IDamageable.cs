using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.Core.AttackSystem.Core
{
    public interface IDamageable
    {
        void TakeDamage(IEntity attacker,float attackValue);
    }
}
