using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
    public class TargetContext
    {
        public ITarget? PrimaryTarget { get; set; }   // 主要目标，比如玩家
        public ITarget? SecondaryTarget { get; set; } // 次要目标，比如玩家的建筑物
    }

}
