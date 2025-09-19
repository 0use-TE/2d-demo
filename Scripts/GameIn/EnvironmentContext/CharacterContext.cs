using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.GameIn.EnvironmentContext
{
    public class CharacterContext
    {
        private readonly AIUnitContext _aiUnitContext;
        private readonly PlayerContext _playerContext;

        public CharacterContext(AIUnitContext aiUnitContext, PlayerContext playerContext)
        {
            _aiUnitContext = aiUnitContext;
            _playerContext = playerContext;
        }
    }
}
