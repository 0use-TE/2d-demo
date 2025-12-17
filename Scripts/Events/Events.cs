using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Events
{
    // 发现敌人：携带引用和数值
    public record EnemySpotted(Node2D Entity, int CurrentHp, int MaxHp);
    // 敌人消失：只需要知道是谁消失了
    public record EnemyGone(Node2D Entity);
    // 血量变化：用于更新现有的血条
    public record EnemyHealthChanged(Node2D Entity, int CurrentHp, int MaxHp);
}
