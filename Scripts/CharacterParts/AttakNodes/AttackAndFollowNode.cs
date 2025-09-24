using CharacterModule.BehaviourTree;
using CharacterModule.BehaviourTree.CompositeNodes;
using CharacterModule.BehaviourTree.Core;
using CharacterModule.StateMachineModule;
using DDemo.Scripts.CharacterParts.AIBehaviourTreeNode;
using DDemo.Scripts.Characters.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.CharacterParts.AttakNodes
{
    public class AttackAndFollowNode : CompositeNode
    {
        private  readonly Selector _rootNode;
        public AttackAndFollowNode(E_TargetType targetType,float followSpeed, float attackRange,BaseState followState,BaseState attackState)
        {
            _rootNode = new Selector();

        }
        public override NodeState Tick(double delta)
        {
            return _rootNode.Tick(delta);
        }
    }
}
