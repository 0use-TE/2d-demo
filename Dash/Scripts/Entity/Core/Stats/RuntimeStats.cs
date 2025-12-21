using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Dash.Scripts.Entity.Core.Stats
{
    public class RuntimeStats
    {
        // 基础属性：运行时需要频繁变动
        public string Name { get; set; } = "暂无";
        public float MaxHp { get; set; }
        public float CurrentHp { get; set; }
        public float MoveSpeed { get; set; }

        // 攻击属性：通常运行时不会改变，可以直接引用 ConfigData 里的项
        // 或者如果你有“强化技能”逻辑，这里也可以存一个副本
        public List<AttackStat> Attacks { get; private set; } = new List<AttackStat>();
        public RuntimeStats() { }

        public RuntimeStats(EntityStat config)
        {
            if (config == null) return;

            // 初始化数值
            MaxHp = config.MaxHp;
            CurrentHp = config.MaxHp;
            MoveSpeed = config.MoveSpeed;
            Name = config.Name;
            // 攻击列表建议：
            // 如果攻击数值不会在运行时被永久修改，直接引用即可：
            Attacks = new (config.Attacks);

        }
    }
}
