using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core.AttackSystem.Imps
{
    public partial class TestAttackDeterctorImp : Area2D, IAttackDetector
    {

        private CharacterBase _character = default!;
        [Inject]
        private ILogger<TestAttackDeterctorImp> _logger=default!;
        public override void _Ready()
        {
            _character=GetParent<CharacterBase>();
        }
        
        public void AttackDetector()
        {
            var overlaps = GetOverlappingBodies();
            foreach (var body in overlaps)
            {
                if (body is CharacterBase attackTarget)
                {
                    if (attackTarget.TeamType != _character.TeamType)
                    {
                        if (attackTarget is IDamageable damageable)
                        {
                            _logger.LogInformation($"检测到了攻击对象{attackTarget.Name}");
                            damageable.TakeDamage(_character);
                        }
                    }
                }
            }
        }
    }
}
