using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
	public class TestTargetEvaluator : ITargetEvaluator
	{
		public void Evaluate(AIBase ai, TargetContext targetContext, IList<CharacterBase> characters, MapContext mapContext, ILogger logger)
		{
			// 先设置一个初始目标距离为最大值
			CharacterBase? closestEnemy = null;
			float closestDistance = float.MaxValue;

			// 获取 AI 当前位置
			var aiPosition = ai.GlobalPosition;

			// 遍历所有敌人，选择距离最近的
			foreach (var character in characters)
			{
				// 计算当前敌人与 AI 的距离
				var distance = aiPosition.DistanceTo(character.GlobalPosition);

				// 如果是最近的敌人，更新
				if (distance < closestDistance)
				{
					closestEnemy = character;
					closestDistance = distance;
				}
			}

			// 如果找到了最近的敌人，设置为攻击目标
			if (closestEnemy != null)
			{
				targetContext.CharacterTarget = new Target { TargetNode = closestEnemy }; // 更新目标
				logger.LogInformationWithNodeName(ai, $"选择了角色 {closestEnemy.Name} 作为攻击目标，距离为 {closestDistance} 米");
			}
			else
			{
				// 没有敌人时，清空目标
				targetContext.CharacterTarget = null;
				logger.LogInformationWithNodeName(ai, "没有找到可攻击的敌人");
			}

			//设置目标点
			if (targetContext.MainBaseTarget?.TargetNode == null)
			{
				var firstTarget = mapContext.TargetPos.FirstOrDefault();
				if (firstTarget != null)
				{
					logger.LogInformationWithNodeName(ai, $"设置BaseTarget为{firstTarget.Name}");
					targetContext.MainBaseTarget = new Target { TargetNode =firstTarget  };
				}
				else
				{
					logger.LogInformationWithNodeName(ai, $"没有任何BaseTarget");
				}
			}
			else
			{
				logger.LogInformationWithNodeName(ai, $"已经设置了BaseTarget");
			}
		}
	}
}
