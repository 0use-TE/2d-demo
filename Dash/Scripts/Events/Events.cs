using Dash.Scripts.Entity.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Events
{
    /// 发现敌人：携带引用和数值
    public record EntitySpottedEvent(Node2D Entity, float CurrentHp, float MaxHp);
    /// <summary>
    /// 敌人消失：只需要知道是谁消失了
    /// </summary>
    /// <param name="Entity"></param>
    public record EntityGoneEvent(Node2D Entity);
    /// <summary>
    /// 血量变化：用于更新现有的血条
    /// </summary>
    /// <param name="Entity"></param>
    /// <param name="CurrentHp"></param>
    /// <param name="MaxHp"></param>
    public record EntityHealthChangedEvent(Node2D Entity, float CurrentHp, float MaxHp);
    /// <summary>
    /// 实体死亡
    /// </summary>
    /// <param name="Entity"></param>
    public record EntityDeadEvent(IEntity Entity);

}
