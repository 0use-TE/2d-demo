using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.PerceptionPart.Core
{
    internal class EnemyPerception : IPerception
    {
        private int _distance = 32*5;
        public EnemyPerception()
        {

        }
        public bool Perception()
        {
            return true;
        }
    }
}
