using Dash.Scripts.Entity.Core;
using Dash.Scripts.Entity.Core.AttachContext;
using Dash.Scripts.GameIn.EnvironmentContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.AI.AIPerception.Core
{
    public interface ITargetPerception
    {
        void TargetPerception(AIBase ai,IList<CharacterBase> characters,IList<BuildingBase> buildings,MapContext mapContext,ILogger logger,TargetContext targetContext);
    }
}
