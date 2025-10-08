using DDemo.Scripts.Misc.Enums;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.Entity.Core
{
    public abstract partial class BuildingBase : StaticBody2D, IBuilding
    {
        public E_TeamType TeamType { get; set; }

        public virtual void TakeDamage(Node2D attacker, int attackValue)
        {

        }
    }
}
