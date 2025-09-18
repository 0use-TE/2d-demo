using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Level.Core
{
	internal interface IUnitContext
	{
		/// <summary>
		/// 玩家
		/// </summary>
		IList<IUnit> Player { get; }

		//IList<IUnit> Enemy { get; }
		///// <summary>
		///// 玩家一方的AI
		///// </summary>
		//IList<IUnit> Neutral { get; }
		///// <summary>
		///// 中立单位、环境
		///// </summary>
		//IList<IUnit> Ally { get; }
	}
}
