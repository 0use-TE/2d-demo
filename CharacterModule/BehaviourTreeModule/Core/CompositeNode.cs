using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CharacterModule.BehaviourTree.Core
{
    // 组合节点抽象基类
    public abstract class CompositeNode : BehaviourNode, ICompositeNode
    {
        protected List<INode> children = new List<INode>();

        public ICompositeNode AddChild(INode child)
        {
            children.Add(child);
            child.SetParent(this);
            return this;
        }

        public ICompositeNode GetParent()
        {
            return _parent;
        }
    }
}
