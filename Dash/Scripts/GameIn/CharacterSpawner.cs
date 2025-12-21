using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Dash.Scripts.Entity.Core;
using Dash.Scripts.Events;
using Dash.Scripts.GameIn.EnvironmentContext;
using Dash.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using MessagePipe;
using Microsoft.Extensions.Logging;

namespace Dash.Scripts.GameIn;

[Meta(typeof(IAutoNode))] // 启用自动节点依赖注入
public partial class CharacterSpawner : Node2D
{
    public override void _Notification(int what) => this.Notify(what);

    [ExportGroup("Spawn Settings")]
    [Export] private PackedScene? TestCharacter;

    [Export] private int SpawnCount { get; set; } = 10; // 最大生成数量
    [Export] private float SpawnInterval { get; set; } = 2.0f; // 生成间隔(秒)

    [Inject] private ILogger<CharacterSpawner> _logger = default!;
    [Inject] private ISubscriber<EntityDeadEvent> _entityDeadEvent= default!;
    [Dependency]
    private AIUnitContext _aiUnitContext => this.DependOn<AIUnitContext>();


    private double _timer = 0; // 用于手动计时

    public override void _Ready()
    {
        // 如果想在开始时立即生成第一个，可以将 _timer 初始化为 SpawnInterval
        _timer = 0;
    }

    public override void _Process(double delta)
    {

        // 2. 计时逻辑
        _timer += delta;
        if (_timer >= SpawnInterval && SpawnCount-- > 0)
        {
            SpawnEnemy();
            _timer = 0; // 重置计时器
        }
    }

    private void SpawnEnemy()
    {
        if (TestCharacter == null)
        {
            _logger.LogWaringWithPush(this, "没有给角色赋值，无法生成!");
            return;
        }

        if (TestCharacter?.Instantiate() is not AIBase ai)
        {
            _logger.LogErrWithPush(this,"错误：拖入的场景没有挂载 AIBase 脚本！");
            return;
        }

        if (ai != null)
        {
            // 设置位置为生成器的位置
            ai.Position = Vector2.Zero;
            // 将敌人添加为当前生成器的子物体
            AddChild(ai);
            _aiUnitContext.AIUnits.Add(ai);
            _logger.LogInfoWithNode(this, $"生成了新敌人: {ai.Name}");
        }
    }
}

