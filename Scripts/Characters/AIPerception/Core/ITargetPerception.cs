using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.AIPerception.Core
{
    internal interface ITargetPerception
    {
        void TargetPerception(IList<CharacterBase> characters,ILogger logger,TargetContext targetContext);
    }
}
