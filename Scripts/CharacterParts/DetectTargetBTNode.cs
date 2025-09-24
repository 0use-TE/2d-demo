using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts
{
    internal class DetectTargetBTNode : BehaviourNode
    {
        private ILogger _logger=default!;
        protected override void OnBlackboardCreated()
        {
            _logger=Blackboard.LoadOrThrow<ILogger>();
        }
        public override NodeState Tick(double delta)
        {

        }
    }
}
