	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core.Context
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
		public Target? MainBaseTarget { get; set; }
		public Target? GetTarget(E_TargetType targetType)
		{
            var target = targetType switch
            {
                E_TargetType.Character => CharacterTarget,
                E_TargetType.Building => BuildingTarget,
                E_TargetType.MainBase => MainBaseTarget,
                _ => null // 兜底，避免没有匹配情况
            };
			return target;
        }

	}
}
