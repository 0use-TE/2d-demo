using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.ai.Extensions;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.Entity.Core.AttackSystem.Core;
using DDemo.Scripts.Entity.Core.Stats;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DDemo.Scripts.Entity.AI.AIPerception.AttackDetect
{
    [Meta(typeof(IAutoNode))]
    public partial class AttackArea : Node2D, IAttackEnemy
    {
        public override void _Notification(int what) => this.Notify(what);
        [Export]
        public int AttackID { get; set; }
        [Node(nameof(HitBox))]
        private Area2D HitBox { get; set; } = default!;

        [Dependency]
        protected  CharacterBase CharacterBase => this.DependOn<CharacterBase>();
        [Inject]
        public ILogger<AttackArea> Logger { get; set; } = default!;
        public override void _Ready()
        {
            base._Ready();
            HitBox.Monitoring = false; // 默认关闭

        }
        /// <summary>
        /// 动画帧直接调用，用于执行攻击逻辑
        /// </summary>
        public async void AttackEnemy()
        {
            Logger.LogInformationWithNodeName(this, $"{CharacterBase.GetType().Name}尝试发动攻击");

            if (AttackID < 0 || AttackID > CharacterBase.RuntimeStats.Attacks.Count)
            {
                Logger.LogErrWithPush(this, "攻击Id不在列表范围内!");
                return;
            }

            var data = CharacterBase.RuntimeStats.Attacks[AttackID];
            var damage = data.Damage;
            HitBox.Monitoring = true;
            // ✅ 确保等到下一次物理帧
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            var objs = HitBox.GetOverlappingBodies();
            foreach (var obj in objs)
            {
                if (obj == CharacterBase) continue;

                if (obj is IDamageable damageable)
                {
                    Logger.LogInformationWithNodeName(this, $"{CharacterBase.GetType().Name}对敌人{obj.Name}造成了{damage}");
                    damageable.TakeDamage(CharacterBase, damage);
                }
            }
        }

    }
}

