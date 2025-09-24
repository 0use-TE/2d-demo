using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.Core;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts
{
    internal class CheckTargetDistanceBTNode : AIBehaviourNode
    {
        private float _minDistance;
        public override NodeState Tick(double delta)
        {
            if (_targetContext.CharacterTarget?.TargetNode == null)
            {
                return NodeState.Failure;
            }
            else
            {
                return NodeState.Running;
            }
        }
    }
}
