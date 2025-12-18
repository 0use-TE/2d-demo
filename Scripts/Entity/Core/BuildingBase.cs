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
    public abstract partial class BuildingBase : StaticBody2D, IBuilding
    {
        public E_TeamType TeamType { get; set; }
        public EntityStat? ConfigData { get; set; }

        public RuntimeStats RuntimeStats { get; set; } = default!;
        public override void _Ready()
        {
            base._Ready();

            // 在 Ready 时，ConfigData 已经由 Godot 引擎完成注入
            if (ConfigData != null)
            {
                RuntimeStats= new RuntimeStats(ConfigData);
            }
            else
            {
                RuntimeStats = new RuntimeStats();
                GD.PrintErr($"{Name}: BuildingBase 缺少 ConfigData 配置！");
            }
        }
        public virtual void TakeDamage(Node2D attacker, float attackValue)
        {

        }
    }
}
