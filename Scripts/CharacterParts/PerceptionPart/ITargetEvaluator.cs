using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
	public interface ITargetEvaluator
	{
		void Evaluate(AIBase ai, TargetContext targetContext,IList<CharacterBase> characters,MapContext mapContext, ILogger logger);
	}
}
