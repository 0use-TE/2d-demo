using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.AI.AIPerception.Core
{
    public interface ITargetPerception
    {
        void TargetPerception(AIBase ai,IList<CharacterBase> characters,IList<BuildingBase> buildings,MapContext mapContext,ILogger logger,TargetContext targetContext);
    }
}
