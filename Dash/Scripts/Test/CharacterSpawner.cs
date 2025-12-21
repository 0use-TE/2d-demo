using Dash.Scripts.Misc.Extensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;

namespace Dash.Scripts.Test;

public partial class TestCharacterSpawner : Node2D
{
    [ExportGroup("Spawn Settings")]
    [Export] private PackedScene? TestCharacter;

    [Export] private int MaxCount { get; set; } = 10; // 最大生成数量
    [Export] private float SpawnInterval { get; set; } = 2.0f; // 生成间隔(秒)

    [Inject] private ILogger<TestCharacterSpawner> _logger = default!;

    private double _timer = 0; // 用于手动计时

    public override void _Ready()
    {
        // 如果想在开始时立即生成第一个，可以将 _timer 初始化为 SpawnInterval
        _timer = 0;
    }

    public override void _Process(double delta)
    {
        // 1. 检查当前已生成的子节点数量是否超过限制
        // 注意：这里统计的是当前节点下的子节点数量
        if (GetChildCount() >= MaxCount)
        {
            return;
        }

        // 2. 计时逻辑
        _timer += delta;
        if (_timer >= SpawnInterval)
        {
            SpawnEnemy();
            _timer = 0; // 重置计时器
        }
    }

    private void SpawnEnemy()
    {
        if (TestCharacter == null)
        {
            _logger.LogWarning("没有给角色赋值，无法生成!");
            return;
        }

        var character = TestCharacter.Instantiate<Node2D>();
        if (character != null)
        {
            // 设置位置为生成器的位置
            character.Position = Position;

            // 将敌人添加为当前生成器的子物体
            AddChild(character);

            _logger.LogInfoWithNode(this, $"生成了新敌人: {character.Name}, 当前总数: {GetChildCount()}");
        }
    }
}

