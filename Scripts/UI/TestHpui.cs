using DDemo.Scripts.Events;
using DDemo.Scripts.Misc.Extensions;
using Godot;
using Godot.Collections;
using Godot.DependencyInjection.Attributes;
using MessagePipe;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace DDemo.Scripts.UI;

public partial class TestHpui : Control
{
    [Export] public PackedScene HpBarPrefab { get; set; } = default!;

    // 注入订阅器
    [Inject] public ISubscriber<EntitySpotted> _spottedSub = default!;
    [Inject] public ISubscriber<EntityGone> _goneSub = default!;
    [Inject] public ISubscriber<EntityHealthChanged> _healthSub = default!;
    [Inject] public ILogger<TestHpui> Logger=default!;
    // 字典：Key是敌人节点，Value是对应的UI血条
    private readonly Dictionary<Node2D, HpBar> _activeBars = new();
    private IDisposable _disposable = default!;

    public override void _Ready()
    {
        // 使用 MessagePipe 的 DisposableBag 或者把多个订阅合并
        var bag = DisposableBag.CreateBuilder();

        // 1. 订阅发现敌人
        _spottedSub.Subscribe(x => {
            if (!_activeBars.ContainsKey(x.Entity))
            {
                var bar = HpBarPrefab.Instantiate<HpBar>();
                AddChild(bar);
                bar.Initialize(x.Entity, x.CurrentHp, x.MaxHp);
                _activeBars[x.Entity] = bar;
                Logger.LogInfoWithNode(this, $"为实体{x.Entity.Name}添加了血条");
            }
        }).AddTo(bag);

        // 2. 订阅敌人消失
        _goneSub.Subscribe(x => {
            if (_activeBars.TryGetValue(x.Entity, out var bar))
            {
                bar.QueueFree();
                _activeBars.Remove(x.Entity);
            }
            Logger.LogInfoWithNode(this, $"为实体{x.Entity.Name}移除了血条");
        }).AddTo(bag);

        // 3. 订阅血量更新
        _healthSub.Subscribe(x => {
            if (_activeBars.TryGetValue(x.Entity, out var bar))
            {
                bar.UpdateHealth(x.CurrentHp, x.MaxHp);
            }
        }).AddTo(bag);

        _disposable = bag.Build();
    }

    public override void _ExitTree()
    {
        _disposable?.Dispose();
    }
}