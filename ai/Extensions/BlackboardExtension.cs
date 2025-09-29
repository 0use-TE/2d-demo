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
        public static void Set<T>(this Blackboard blackboard, T value) where T:GodotObject
        {
            var key = typeof(T).FullName!;
            blackboard.SetVar(key, value);

        }

        public static T Get<[MustBeVariant]T>(this Blackboard blackboard) where T : GodotObject
        {
            var key = typeof(T).FullName!;
            return blackboard.GetVar(key).As<T>();
        }
    }
}
