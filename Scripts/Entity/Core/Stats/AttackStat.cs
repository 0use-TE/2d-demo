using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core.Stats
{
    [GlobalClass]
    public partial class AttackStat : Resource
    {
        [Export] public string SkillName = "Basic Attack";
        [Export] public float Damage = 10.0f;
        [Export] public float Cooldown = 1.0f;
    }
}
