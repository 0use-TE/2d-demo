using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Scripts.GameIn.EnvironmentContext
{
    internal class EntityInCameraContext
    {
        public IList<PhysicsBody2D> Entities { get; set; }=new List<PhysicsBody2D>();
    }
}
