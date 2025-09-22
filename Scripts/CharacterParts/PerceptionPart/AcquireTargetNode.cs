using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using Chickensoft.Collections;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart
{
    internal class AcquireTargetNode : BehaviourNode
    {
        private float _distance;
        private AIBase _ai = default!;
        private TargetContext _targetContext = default!;
        private ILogger _logger=default!;
        public AcquireTargetNode(float distance)
        {
            _distance = distance;
        }
        protected override void OnBlackboardCreated()
        {
            _ai = Blackboard.Load<AIBase>() ?? throw new NullReferenceException("AI没有存入黑板");
            _targetContext = Blackboard.Load<TargetContext>()!;
            _logger = Blackboard.Load<ILogger>()!;
        }
        public override NodeState Tick(double delta)
        {

            var world2D = _ai.GetWorld2D();
            var space = world2D.DirectSpaceState;

            var circle = new CircleShape2D { Radius = _distance };
            var transform = new Transform2D(0, _ai.GlobalPosition);

            var exclude = new Godot.Collections.Array<Rid> { new Rid(_ai) };

            var results = space.IntersectShape(new PhysicsShapeQueryParameters2D
            {
                Shape = circle,
                Transform = transform,
                Margin = 0.1f,
                Exclude = exclude
            });

            CharacterBase? nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    var collider = item["collider"];
                    try
                    {
                        // 转换为 Node2D
                        var node = collider.As<Node2D>();

                        // 判断是否是 CharacterBase
                        if (node is CharacterBase otherAI)
                        {

                            // 必须阵营不同，才算敌人

                            if (otherAI.TeamType != _ai.TeamType)
                            {
                                float dist = _ai.GlobalPosition.DistanceTo(otherAI.GlobalPosition);
                                if (dist < nearestDistance)
                                {
                                    nearestDistance = dist;
                                    nearestEnemy = otherAI;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "检测敌人时出错");
                    }
                }
            }

            // 设置到上下文
            if (nearestEnemy != null)
            {
                _logger.LogBehaviourTreeNodeInformation(this, $"成功检测到了攻击目标{_targetContext?.PrimaryTarget?.TargetNode?.Name}");


                _targetContext!.PrimaryTarget= new Target
                {
                     TargetNode=nearestEnemy
                };
                return NodeState.Success;
            }

            _targetContext.PrimaryTarget = null;
            return NodeState.Failure;
        }
    }
}
