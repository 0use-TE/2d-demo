using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Stats.PlayerStats
{
    internal class CharacterStatsDto
    {
        public int HP;
        public float VerticalMoveSpeed { get; set; }
        public float HorizontalMoveSpeed { get; set; }
        public float MeleeAttackMoveSpeed { get; set; }
        public float RemoteAttackMoveSpeed { get; set; }
    }
}
