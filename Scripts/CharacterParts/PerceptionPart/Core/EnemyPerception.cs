using CharacterModule.BehaviourTree;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart.Core
{
    internal class EnemyPerception : IPerception
    {
        private float _distance;
        public EnemyPerception(float distance)
        {
            _distance = distance;
        }
        public bool Perception()
        {
            //判断逻辑
            return true;
        }
    }
}
