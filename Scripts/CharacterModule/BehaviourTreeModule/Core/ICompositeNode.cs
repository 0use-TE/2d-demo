using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.Core
{
    public interface ICompositeNode:INode
    {
        ICompositeNode AddChild(INode child);
        ICompositeNode? GetParent();
    }
}
