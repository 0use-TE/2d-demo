using DDemo.Scripts.Events;
using DDemo.Scripts.Misc.Extensions;
using DDemo.Scripts.PooledObjectPolicy;
using Godot;
using Godot.DependencyInjection.Attributes;
using MessagePipe;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
namespace DDemo.Scripts.UI;

internal partial class TestHpui : Control
{
    [Export] public PackedScene HpBarPrefab { get; set; } = default!;

    [Inject] public ISubscriber<EntitySpotted> _spottedSub = default!;
    [Inject] public ISubscriber<EntityGone> _goneSub = default!;
    [Inject] public ISubscriber<EntityHealthChanged> _healthSub = default!;
    [Inject] public ILogger<TestHpui> Logger = default!;

    // 注入提供者
    [Inject] public ObjectPoolProvider PoolProvider { get; set; } = default!;

    private readonly Dictionary<Node2D, HpBar> _activeBars = new();
    private IDisposable _disposable = default!;

    // 声明本地池
    private ObjectPool<HpBar> _hpBarPool = default!;

    public override void _Ready()
    {
        // --- 1. 初始化对象池 ---
        // 使用自定义 Policy 告诉池子如何处理 HpBar
        var policy = new HpBarPooledObjectPolicy(HpBarPrefab);

        _hpBarPool = PoolProvider.Create(policy);

        var bag = DisposableBag.CreateBuilder();

        // --- 2. 订阅逻辑修改 ---

        // 订阅发现敌人
        _spottedSub.Subscribe(x =>
        {
            if (!_activeBars.ContainsKey(x.Entity))
            {
                // 【修改】从池子借，而不是 Instantiate
                var bar = _hpBarPool.Get();

                AddChild(bar); // 借出来后挂载到 UI 树
                bar.Visible = true; // 确保可见
                bar.Initialize(x.Entity, x.CurrentHp, x.MaxHp);

                _activeBars[x.Entity] = bar;
                Logger.LogInfoWithNode(this, $"[Pool] 为实体{x.Entity.Name}租用了血条");
            }
        }).AddTo(bag);

        // 订阅敌人消失
        _goneSub.Subscribe(x =>
        {
            if (_activeBars.Remove(x.Entity, out var bar))
            {
                // 【修改】还回池子，而不是 QueueFree
                _hpBarPool.Return(bar);
                Logger.LogInfoWithNode(this, $"[Pool] 回收了实体{x.Entity.Name}的血条");
            }
        }).AddTo(bag);

        // 订阅血量更新（保持不变）
        _healthSub.Subscribe(x =>
        {
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
        // 退出时，字典里的活跃对象建议清理
        _activeBars.Clear();
    }
}