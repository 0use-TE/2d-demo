using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.ai.Extensions
{
    public static class BlackboardExtensions
    {
        public static void Set<[MustBeVariant] T>(this Blackboard blackboard, T value)
        {
            var key = typeof(T).FullName!;
            blackboard.Set(key, Variant.From(value));
        }

        public static T Get<[MustBeVariant] T>(this Blackboard blackboard)
        {
            var key = typeof(T).FullName!;
            var variant = blackboard.Get(key);
            return variant.As<T>();
        }
    }
}
