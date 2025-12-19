using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.Entity.Core.AttachContext
{
    public class Target
    {
        public Node2D? TargetNode { get; set; }
        public bool IsPos { get; set; }
        public bool IsValid { get; set; }
        public Vector2 Position { get; set; }
    }

}
