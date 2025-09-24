

using CharacterModule.BehaviourTree.Core;
using System;

namespace DDemo.Scripts.CharacterParts.Extensions
{
	public static class BlackboardExtensions
	{
		/// <summary>
		/// 从黑板加载指定类型数据，如果为空则抛出自定义异常
		/// </summary>
		public static T LoadOrThrow<T>(this IBlackboard blackboard)
		{
			var value = blackboard.Load<T>();
			if (value == null)
			{
				throw new NullReferenceException(
					$"黑板中没有找到 {typeof(T).Name} 实例"
				);
			}
			return value;
		}
	}
}
