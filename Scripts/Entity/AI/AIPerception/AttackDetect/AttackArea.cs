using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDemo.ai.Extensions;
namespace DDemo.Scripts.Entity.AI.AIPerception.AttackDetect
{
    [Meta(typeof(IAutoNode))]
    public partial class AttackArea : Node2D, IAttackEnemy
    {
        public override void _Notification(int what) => this.Notify(what);

        [Dependency]
        private AIBase AI => this.DependOn<AIBase>();

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
            if (body == AI) return;
            if (body is CharacterBase character && character.TeamType != AI.TeamType)
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
        public async void AttackEnemy()
        {
            HitBox.Monitoring = true;
            // ✅ 确保等到下一次物理帧
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            var objs = HitBox.GetOverlappingBodies();

            foreach (var obj in objs)
            {
                AI.Logger($"检测到刚体{obj.Name}");

                if (obj is IDamageable damageable)
                {
                    damageable.TakeDamage(AI,10);
                }
            }

        }
    }
}

