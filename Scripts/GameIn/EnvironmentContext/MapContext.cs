using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.GameIn.EnvironmentContext
{
    public class MapContext
    {
        public IList<Node2D> SpawnPos { get; set; }  =new List<Node2D>();
        public IList<Node2D> TargetPos { get; set; }=new List<Node2D>();    

    }
}
