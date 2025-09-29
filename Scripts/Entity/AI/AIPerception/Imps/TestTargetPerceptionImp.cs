using DDemo.Scripts.Entity.AI.AIPerception.Core;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.AI.AIPerception.Imps
{
    internal class TestTargetPerceptionImp : ITargetPerception
    {
        public void TargetPerception(AIBase ai,IList<CharacterBase> characters,IList<BuildingBase> buildings, MapContext mapContext, ILogger logger, TargetContext targetContext)
        {

            var nearestCharacter = characters
             .OrderBy(c => c.GlobalPosition.DistanceTo(ai.GlobalPosition))
             .FirstOrDefault();
            if (nearestCharacter != null)
            {
                logger.LogInformationWithNodeName(ai, $"角色{nearestCharacter.Name}被设置为了角色攻击目标!");
                targetContext.CurrentTarget.TargetNode = nearestCharacter;
            }
            else
            {
                var manBaseNode = mapContext.TargetPos.FirstOrDefault();
                if (manBaseNode != null)
                {
                    logger.LogInformationWithNodeName(ai, $"大本营{manBaseNode.Name}被设置为了角色攻击目标!");
                    targetContext.CurrentTarget.TargetNode = manBaseNode;
                }
            }
            logger.LogInformationWithNodeName(ai, $"找不到任何攻击目标!");
        }
    }
}
