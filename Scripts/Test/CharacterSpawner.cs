using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;

namespace DDemo.Scripts.Test;

[GlobalClass]
public partial class CharacterSpawner : Node2D
{
    [Export]
    private int Count { get; set; }
    [Export]
    private PackedScene ?TestCharacter;
    [Inject]
    private ILogger<CharacterSpawner> _logger=default!;
    public override void _Ready()
    {
        for (var i = 0; i < Count; i++)
        {
            var character = TestCharacter?.Instantiate<Node2D>();
            if (character != null)
            {
                character.Position = Position;
                AddChild(character);
            }
            else
            {
                _logger.LogWarning("没有给角色赋值!");
            }
        }

    }
}

