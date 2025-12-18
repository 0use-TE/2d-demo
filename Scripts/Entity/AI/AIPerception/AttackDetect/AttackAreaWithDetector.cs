using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using DDemo.Scripts.Entity.Core.Stats;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDemo.Scripts.Entity.AI.AIPerception.AttackDetect
{
    [Meta(typeof(IAutoNode))]
    internal partial class AttackAreaWithDetector : AttackArea
    {
        public override void _Notification(int what) => this.Notify(what);

        [Node(nameof(Detector))]
        private Area2D Detector { get; set; } = default!;

        public bool InAttackAreaDetector { get; private set; }
        private readonly HashSet<CharacterBase> _detectedEnemies = new();

        private AttackStat? attackStat;

        public override void _Ready()
        {
            base._Ready();
            // 敌人进入/离开检测范围
            Detector.BodyEntered += OnDetectorEntered;
            Detector.BodyExited += OnDetectorExited;

            // 攻击命中
        }
        private void OnDetectorEntered(Node2D body)
        {
            if (body == CharacterBase) return;
            if (body is CharacterBase character && character.TeamType != CharacterBase.TeamType)
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

    }
}
