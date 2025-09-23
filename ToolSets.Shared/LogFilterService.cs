using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToolSets.Shared
{
    public class LogFilterService : ILogFilterService
    {
        private static string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ToolSets", "log_filters.json");
        private static string _dllPathConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ToolSets", "dll_path.json");
        private List<LogFilterRule> _filterRules = [];
        public LogFilterService()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
            LoadFilterRules();
        }
        public List<LogFilterRule> ScanAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"DLL文件不存在: {assemblyPath}");
                return _filterRules;
            }
            var direcory= Path.GetDirectoryName(assemblyPath);
            if (direcory==null)
            {

                return _filterRules;
            }

            // 创建自定义 AssemblyLoadContext
            var context = new CustomAssemblyLoadContext(direcory);

            try
            {
                // 加载程序集
                var assembly = context.LoadFromAssemblyPath(assemblyPath);
                Console.WriteLine($"加载到了程序集: {assembly.FullName}");

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Console.WriteLine("部分类型加载失败:");
                    foreach (var loaderEx in ex.LoaderExceptions)
                    {
                        Console.WriteLine($"  - 错误: {loaderEx?.Message}");
                    }
                    types = ex.Types.Where(t => t != null).ToArray()!;
                }

                // 1. 找到所有目标类型
                var newTypeNames = types
                    .Where(t => t != null && typeof(Godot.Node2D).IsAssignableFrom(t))
                    .Where(t =>
                        t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                            .Any(f => typeof(ILogger).IsAssignableFrom(f.FieldType) ||
                                      typeof(ILoggerFactory).IsAssignableFrom(f.FieldType)) ||
                        t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                            .Any(p => typeof(ILogger).IsAssignableFrom(p.PropertyType) ||
                                      typeof(ILoggerFactory).IsAssignableFrom(p.PropertyType))
                    )
                    .Select(t => t.FullName!)
                    .ToList();

				// 2. 把旧规则转字典，方便查找
				var dict = _filterRules.ToDictionary(r => r.TypeName, r => r);

				// 3. 同步删除不存在的类型
				foreach (var key in dict.Keys.ToList())
				{
					if (!newTypeNames.Contains(key))
						dict.Remove(key);
				}

				// 4. 只添加新的，不覆盖旧的状态
				foreach (var typeName in newTypeNames)
				{
					if (!dict.ContainsKey(typeName))
					{
						dict[typeName] = new LogFilterRule
						{
							TypeName = typeName,
							FieldOrPropertyName = string.Empty,
							IsEnabled = true, // 默认启用
							LogLevel = "Information"
						};
					}
				}

				// 5. 更新到 _filterRules
				_filterRules = dict.Values.ToList();
                SaveFilterRules(_filterRules);

				return _filterRules;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"扫描程序集失败: {ex.Message}");
                return _filterRules;
            }
            finally
            {
                // 卸载 AssemblyLoadContext
                context.Unload();
            }
        }
        public List<LogFilterRule> GetFilterRules()
        {
            return _filterRules;
        }

        public void SaveFilterRules(List<LogFilterRule> rules)
        {
            _filterRules = rules;
            var json = JsonSerializer.Serialize(rules, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }

        public void UpdateFilterRules(List<LogFilterRule> newRules)
        {
            LoadFilterRules();
            _filterRules = _filterRules
                .Where(existing => newRules.Any(newRule =>
                    newRule.TypeName == existing.TypeName &&
                    newRule.FieldOrPropertyName == existing.FieldOrPropertyName))
                .ToList();

            foreach (var newRule in newRules)
            {
                if (!_filterRules.Any(r => r.TypeName == newRule.TypeName && r.FieldOrPropertyName == newRule.FieldOrPropertyName))
                {
                    _filterRules.Add(newRule);
                }
            }

            SaveFilterRules(_filterRules);
        }

        public static IList<LogFilterRule> LoadRules()
        {
            var json = File.ReadAllText(_configPath);
            var filterRules = JsonSerializer.Deserialize<List<LogFilterRule>>(json) ?? [];
            return filterRules;
        }

        public string? GetDllPath()
        {
            if (File.Exists(_dllPathConfig))
            {
                var json = File.ReadAllText(_dllPathConfig);
                return JsonSerializer.Deserialize<string>(json);
            }
            return null;
        }

        public void SaveDllPath(string path)
        {
            var json = JsonSerializer.Serialize(path, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dllPathConfig, json);
        }

        private void LoadFilterRules()
        {
            if (File.Exists(_configPath))
            {
                var json = File.ReadAllText(_configPath);
                _filterRules = JsonSerializer.Deserialize<List<LogFilterRule>>(json) ?? [];
            }
        }
    }
}
