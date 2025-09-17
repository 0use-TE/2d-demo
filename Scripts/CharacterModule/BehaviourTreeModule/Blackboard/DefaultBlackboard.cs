using CharacterModule.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.Blackboard
{
    public class DefaultBlackboard : IBlackboard
    {
        private Dictionary<string, object?> data = new Dictionary<string, object?>();
        public void Clear()
        {
            data.Clear();
        }

        public bool HasKey(string key)
        {
            return data.ContainsKey(key);
        }

        public T? Load<T>(string key)
        {
            if (data.TryGetValue(key, out var value))
                return (T?)value;

            return default;
        }

        public T? Load<T>()
        {
            var key= typeof(T).Name.ToString();
            return Load<T>(key);
        }

        public void Save<T>(string key, T value)
        {
            data[key] = value;
        }

        public void Save<T>(T value)
        {
            var key = typeof(T).Name.ToString();
            Save(key,value);
        }
    }
}
