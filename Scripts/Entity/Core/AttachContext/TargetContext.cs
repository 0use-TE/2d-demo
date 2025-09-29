	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core.Context
{
	public enum E_TargetType
	{
		MainBase,
		Character,
		Building
	}
	public class TargetContext
	{
		/// <summary>
		/// 当前的目标
		/// </summary>
		public Target CurrentTarget { get; set; } = new Target();
	}
}
