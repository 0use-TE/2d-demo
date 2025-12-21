using GameDevTools.Share;
using GameDevTools.Share.ShareModel.LogFilter;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Dash.Scripts.GameHandler.Services
{
    public class LogFilterService : ILogFilterService, IDisposable
    {
        // 使用 ImmutableHashSet 保证读取时的线程安全，无需 lock
        private ImmutableHashSet<string> _enabledNames = ImmutableHashSet<string>.Empty;
        private readonly string _configPath;
        private readonly FileSystemWatcher? _watcher;

        public LogFilterService()
        {
            // 1. 从共享项目获取路径
            _configPath = SharedPathProvider.GetConfigFilePath("LogFilter");

            // 2. 初始加载
            LoadConfig();

            // 3. 启动文件监听 (实现运行时热更新)
            var directory = Path.GetDirectoryName(_configPath);
            if (Directory.Exists(directory))
            {
                _watcher = new FileSystemWatcher(directory, Path.GetFileName(_configPath));
                _watcher.NotifyFilter = NotifyFilters.LastWrite;
                _watcher.Changed += (s, e) => LoadConfig(); // 文件一变就重载
                _watcher.EnableRaisingEvents = true;
            }
        }

        private void LoadConfig()
        {
            if (!File.Exists(_configPath)) return;

            try
            {
                // 防止 Avalonia 正在写入时导致的 IO 占用错误，尝试读取
                using var stream = File.Open(_configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var model = JsonSerializer.Deserialize<LogFilterSaveModel>(stream);

                if (model?.SavedNodes != null)
                {
                    // 只提取开启的 FullName 进 HashSet
                    var names = model.SavedNodes
                        .Where(x => x.IsEnabled)
                        .Select(x => x.FullName)
                        .ToImmutableHashSet();

                    _enabledNames = names;
                }
            }
            catch (IOException)
            {
                // 写入冲突时，可以稍后重试或忽略本次更新
            }
        }

        public bool IsAllowed(string categoryName)
        {
            // 如果配置为空，默认全部放行（或者根据你的喜好改为全部拦截）
            if (_enabledNames.IsEmpty) return false;

            return !_enabledNames.Contains(categoryName);
        }

        public void Dispose() => _watcher?.Dispose();
    }
}
