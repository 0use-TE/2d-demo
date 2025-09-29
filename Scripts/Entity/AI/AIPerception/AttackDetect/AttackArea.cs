using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DDemo.Scripts.Entity.AI.AIPerception.AttackDetect
{
    [Meta(typeof(IAutoNode))]
    public partial class AttackArea : Node2D, IAttackEnemy
    {
        public override void _Notification(int what) => this.Notify(what);

        [Dependency]
        private AIBase Self => this.DependOn<AIBase>();

        [Node(nameof(Detector))]
        private Area2D Detector { get; set; } = default!;

        [Node(nameof(HitBox))]
        private Area2D HitBox { get; set; } = default!;

        [Export]
        public bool InAttackAreaDetector { get; private set; }

        private readonly HashSet<CharacterBase> _detectedEnemies = new();

        public override void _Ready()
        {
            // 敌人进入/离开检测范围
            Detector.BodyEntered += OnDetectorEntered;
            Detector.BodyExited += OnDetectorExited;

            // 攻击命中
            HitBox.Monitoring = false; // 默认关闭
        }

        private void OnDetectorEntered(Node2D body)
        {
            if (body == Self) return;
            if (body is CharacterBase character && character.TeamType != Self.TeamType)
            {
                _detectedEnemies.Add(character);
                InAttackAreaDetector = true;
            }
        }

        private void OnDetectorExited(Node2D body)
        {
            if (body is CharacterBase character && _detectedEnemies.Contains(character))
            {
                _detectedEnemies.Remove(character);
                if (_detectedEnemies.Count == 0)
                    InAttackAreaDetector = false;
            }
        }


        /// <summary>
        /// 动画帧直接调用，用于执行攻击逻辑
        /// </summary>
        public void AttackEnemy()
        {

        }
    }
}

