using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core.Context
{
    public class TargetContext
    {
        public Target? PrimaryTarget { get; set; }   // 主要目标，比如玩家
        public Target? SecondaryTarget { get; set; } // 次要目标，比如玩家的建筑物
    }

}
