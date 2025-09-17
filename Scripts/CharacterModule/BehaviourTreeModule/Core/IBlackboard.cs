using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModule.BehaviourTree.Core
{
    public interface IBlackboard
    {
        void Save<T>(string key, T value);
        void Save<T>(T value);
        T? Load<T>(string key);
        T? Load<T>();
        bool HasKey(string key);
        void Clear();
    }
}
