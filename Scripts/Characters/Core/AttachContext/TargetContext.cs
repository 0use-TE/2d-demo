using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core.Context
{
    public class TargetContext
    {
		/// <summary>
		/// 范围内的角色目标
		/// </summary>
		public Target? CharacterTarget { get; set; }   
		/// <summary>
		/// 范围内的建筑目标
		/// </summary>
		public Target? BuildingTarget { get; set; }
		/// <summary>
		/// 固定的大本营目标
		/// </summary>
		public Target? BaseTarget { get; set; }        

	}

}
