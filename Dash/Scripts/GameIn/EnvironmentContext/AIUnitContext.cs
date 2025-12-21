using Dash.Scripts.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.GameIn.EnvironmentContext
{
    public class AIUnitContext
    {
        public IList<AIBase> AIUnits { get; set; } = new List<AIBase>();

    }
}
