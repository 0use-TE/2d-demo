using Chickensoft.AutoInject;
using Chickensoft.Introspection;
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
    [Meta(typeof(IAutoNode))]
    public abstract partial class BuildingBase : StaticBody2D, IBuilding,IProvide<IEntity>
    {
        public override void _Notification(int what) => this.Notify(what);

        public E_TeamType TeamType { get; set; }
        public EntityStat? ConfigData { get; set; }

        public RuntimeStats RuntimeStats { get; set; } = default!;
        IEntity IProvide<IEntity>.Value() => this;
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
            this.Provide();
        }
        /// <summary>
        /// 暂未思考建筑物
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="attackValue"></param>
        public virtual void TakeDamage(IEntity attacker, float attackValue)
        {

        }
    }
}
