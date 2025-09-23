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
            var rulesDict = new Dictionary<string, LogFilterRule>();
            try
            {
                // 验证DLL文件是否存在
                if (!File.Exists(assemblyPath))
                {
                    Console.WriteLine($"DLL文件不存在: {assemblyPath}");
                    return _filterRules;
                }

                // 加载DLL文件
                byte[] assemblyBytes = File.ReadAllBytes(assemblyPath);
                var assembly = Assembly.Load(assemblyBytes);
                if (assembly == null)
                {
                    Console.WriteLine("加载程序集失败!");
                    return _filterRules;
                }

                Console.WriteLine($"加载到了程序集: {assembly.FullName}");

                // 获取类型，处理ReflectionTypeLoadException
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
                    types = ex.Types.Where(t => t != null).ToArray(); // 只处理成功加载的类型
                }

                // 找出所有继承自Godot.Node2D且包含ILogger或ILoggerFactory的基类
                var baseLoggerTypes = new HashSet<Type>();

                foreach (var type in types)
                {
                    if (type == null || !typeof(Godot.Node2D).IsAssignableFrom(type))
                        continue;

                    // 检查字段
                    bool hasLogger = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(f => typeof(ILogger).IsAssignableFrom(f.FieldType) ||
                                  typeof(ILoggerFactory).IsAssignableFrom(f.FieldType));

                    // 检查属性
                    if (!hasLogger)
                    {
                        hasLogger = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                            .Any(p => typeof(ILogger).IsAssignableFrom(p.PropertyType) ||
                                      typeof(ILoggerFactory).IsAssignableFrom(p.PropertyType));
                    }

                    if (hasLogger)
                    {
                        baseLoggerTypes.Add(type);
                    }
                }

                // 找出所有继承自带ILogger基类的子类
                foreach (var type in types)
                {
                    if (type == null || !typeof(Godot.Node2D).IsAssignableFrom(type))
                        continue;

                    // 是否继承自某个带ILogger的基类
                    if (baseLoggerTypes.Any(baseType => baseType.IsAssignableFrom(type)))
                    {
                        string typeName = type.FullName ?? string.Empty;
                        if (!string.IsNullOrEmpty(typeName) && !rulesDict.ContainsKey(typeName))
                        {
                            rulesDict[typeName] = new LogFilterRule
                            {
                                TypeName = typeName,
                                FieldOrPropertyName = string.Empty,
                                IsEnabled = true,
                                LogLevel = "Information"
                            };
                        }
                    }
                }

                var newRules = rulesDict.Values.ToList();
                UpdateFilterRules(newRules);
                SaveDllPath(assemblyPath);
                return _filterRules;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"扫描程序集失败: {ex.Message}");
                if (ex is ReflectionTypeLoadException rtle)
                {
                    foreach (var loaderEx in rtle.LoaderExceptions)
                    {
                        Console.WriteLine($"  - 详细错误: {loaderEx?.Message}");
                    }
                }
                return _filterRules;
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
