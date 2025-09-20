using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Characters.Core
{
    public interface ITarget
    {
        Node2D TargetNode { get; }
        bool IsValid { get; }
        Vector2 Position { get; }
    }

}
