using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ToolSets.Shared
{
    // 自定义 AssemblyLoadContext，用于加载和卸载程序集
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string _dependencyDir;

        public CustomAssemblyLoadContext(string dependencyDir) : base(isCollectible: true)
        {
            _dependencyDir = dependencyDir;
            // 注册依赖解析事件
            this.Resolving += ResolveDependency;
        }

        private Assembly ResolveDependency(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            // 尝试从依赖目录加载缺失的程序集
            string assemblyPath = Path.Combine(_dependencyDir, $"{assemblyName.Name}.dll");
            if (File.Exists(assemblyPath))
            {
                try
                {
                    return context.LoadFromAssemblyPath(assemblyPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载依赖 {assemblyName.Name} 失败: {ex.Message}");
                }
            }
            return null;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // 默认不处理，依赖 Resolving 事件
            return null;
        }
    }
}
