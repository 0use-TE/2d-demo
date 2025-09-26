using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Characters.AIPerception.Core;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.Characters.Core.Context;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Misc.Enums;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

[Meta(typeof(IAutoNode))]
public partial class TestAi : CharacterBase
{
    public override void _Notification(int what) => this.Notify(what);
    [Node(nameof(BTPlayer))]
    private BTPlayer BTPlayer { get; set; } = default!;

    [Node(nameof(NavigationAgent2D))]
    private NavigationAgent2D NavigationAgent { get; set; } =default!;
    [Node(nameof (Area2D))]
    private Area2D Area2D { get; set; } =default!;
    private TargetContext TargetContext { get; set; } = new TargetContext();
    private Timer? _timer;
    private IList<CharacterBase> _characters=new List<CharacterBase>();

    private IList<ITargetPerception> _targetPerceptions = new List<ITargetPerception>();
    [Dependency]
    private MapContext MapContext => this.DependOn<MapContext>();

    public override void _Ready()
    {
        base._Ready();
        TeamType = E_TeamType.Enemy;
        _timer = new Timer()
        {
            Autostart = true,
            WaitTime = 1
        };
        _timer.Timeout += _timer_Timeout;
        AddChild(_timer);
        Area2D.BodyEntered += Area2D_BodyEntered;
        Area2D.BodyExited += Area2D_BodyExited;

        //配置感知
        _targetPerceptions.
    }

    private void Area2D_BodyEntered(Node2D body)
    {
        if (body is CharacterBase character)
        {
            if (character == this)
                return;
            if (character.TeamType != TeamType)
            {
                _logger.LogInformationWithNodeName(this, $"角色{character.Name}进入了攻击范围!");
                _characters.Add(character);
            }
            else
            {
                _logger.LogInformationWithNodeName(this, $"非敌对角色{character.Name}进入了攻击范围!");
            }
        }
        else
        {
            _logger.LogInformationWithNodeName(this, $"非角色{body.Name}进入了攻击范围!");
        }
    }
    private void Area2D_BodyExited(Node2D body)
    {
        if (body is CharacterBase character)
        {
            if (character.TeamType != TeamType)
            {
                _logger.LogInformationWithNodeName(this, $"角色{character.Name}退出了攻击范围!");
                _characters.Remove(character);
            }
            else
            {
                _logger.LogInformationWithNodeName(this, $"非敌对角色{character.Name}退出了攻击范围!");
            }
        }
        else
        {
            _logger.LogInformationWithNodeName(this, $"非角色{body.Name}退出了攻击范围!");
        }
    }


    private void _timer_Timeout()
    {
        //检测策略
        foreach(var item in _targetPerceptions)
        {
            item.TargetPerception(_characters, _logger, TargetContext);
        }
    }
}
