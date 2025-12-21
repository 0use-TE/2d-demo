using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Misc.Enums
{
    public enum E_TeamType
    {
        /// <summary>
        /// 中立（中立生物）
        /// </summary>
        Neutral,
        /// <summary>
        /// 秩序（玩家 + 玩家的盟友/召唤物）
        /// </summary>
        Order, 
        /// <summary>
        /// 混乱（敌人、怪物）
        /// </summary>
        Chaos
    }
    public enum E_ControllerType
    {
        AI,     // 电脑控制
        Human, // 真人操作
    }
}
