using DDemo.Scripts.UI;
using Godot;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDemo.Scripts.PooledObjectPolicy
{
    public class HpBarPooledObjectPolicy : IPooledObjectPolicy<HpBar>
    {
        private readonly PackedScene _scene;

        public HpBarPooledObjectPolicy(PackedScene scene)
        {
            _scene = scene;
        }

        public HpBar Create()
        {
            // 池子为空时，实例化新节点
            return _scene.Instantiate<HpBar>();
        }

        public bool Return(HpBar obj)
        {
            // 回收时的重置操作
            obj.Visible = false;
            if (obj.GetParent() != null)
            {
                obj.GetParent().RemoveChild(obj); // 从场景树移除但不销毁
            }
            return true;
        }
    }
}
