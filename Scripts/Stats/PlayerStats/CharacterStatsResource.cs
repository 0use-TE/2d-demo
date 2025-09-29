using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Stats.PlayerStats
{
    [GlobalClass]
    public partial class CharacterStatsResource : Resource
    {
        [Export]
        public float HP { get; set; }
        [Export] 
        public float VerticalMoveSpeed { get; set; }
        [Export] 
        public float HorizontalMoveSpeed { get; set; }
        [Export]
        public float MeleeAttackMoveSpeed { get; set; }
        [Export]
        public float RemoteAttackMoveSpeed { get; set; }
    }
}
