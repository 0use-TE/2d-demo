using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using GameDevTools.Extensions;
using GameDevTools.Services.DataPersistenceServices;
using GameDevTools.Share.ShareModel.LogFilter;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace GameDevTools.ViewModels;

internal class LogFilterViewModel : BindableBase, IViewModelDataInit, INavigationAware
{
    // 内部模型：用于列表显示和勾选状态
    // 在 ViewModel 内部或同级
    public class NodeFilterItem : BindableBase
    {
        public Action? OnChanged { get; init; }
        public string FullName { get; init; } = string.Empty;
        public bool IsEnabled
        {
            get => field;
            set
            {
                if (SetProperty(ref field, value))
                {
                    // 只要变了，就执行通知逻辑
                    OnChanged?.Invoke();
                }
            }
        } = false;
        // 辅助方法：快速转为 DTO
        public NodeFilterDto ToDto() => new() { FullName = FullName, IsEnabled = IsEnabled };
    }

    private readonly ILogger<LogFilterViewModel> _logger;
    private readonly IJsonPersistenceService _jsonPersistenceService;
    private const string LogFilterKeyName = "LogFilter";
    private List<NodeFilterItem> _allNodes = new(); // 原始完整数据
    public DelegateCommand SaveCommand { get; set; }
    // --- 属性绑定 (使用 field 关键字) ---
    public bool IsDirty
    {
        get => field;
        set
        {
            if (SetProperty(ref field, value))
                SaveCommand.RaiseCanExecuteChanged();
        }
    }

    public string SelectDllPath
    {
        get => field;
        set => SetProperty(ref field, value);
    } = "";

    public string SelectDllName
    {
        get => field;
        set => SetProperty(ref field, value);
    } = "";

    public string? FilterDllName
    {
        get => field;
        set
        {
            if (SetProperty(ref field, value))
            {
                RefreshDisplayList();
            }
        }
    }

    // 界面绑定的过滤后的集合
    public ObservableCollection<NodeFilterItem> DisplayNodes { get; } = new();

    // --- 命令 ---
    public DelegateCommand<IEnumerable<IStorageItem>> SelectFileCommad { get; }
    public DelegateCommand ScanGodotDllInfoCommand { get; }
    public DelegateCommand SelectAllCommand { get; }
    public DelegateCommand ClearAllCommand { get; }

    public LogFilterViewModel(ILogger<LogFilterViewModel> logger, IJsonPersistenceService jsonPersistenceService)
    {
        _logger = logger;
        _jsonPersistenceService = jsonPersistenceService;

        SelectFileCommad = new DelegateCommand<IEnumerable<IStorageItem>>(async (files) => await SelectFileAsync(files));
        ScanGodotDllInfoCommand = new DelegateCommand(async () => await ScanGodotDllInfoAsync());

        // 全部选择和清空逻辑
        SelectAllCommand = new DelegateCommand(() => BatchSetEnabled(true));
        ClearAllCommand = new DelegateCommand(() => BatchSetEnabled(false));
        SaveCommand = new DelegateCommand(Save, () => IsDirty);
    }

    private void BatchSetEnabled(bool isEnabled)
    {
        // 仅对当前过滤显示的项进行操作
        foreach (var node in DisplayNodes)
        {
            node.IsEnabled = isEnabled;
        }
    }

    private void RefreshDisplayList()
    {
        DisplayNodes.Clear();
        var filtered = string.IsNullOrWhiteSpace(FilterDllName)
            ? _allNodes
            : _allNodes.Where(x => x.FullName.Contains(FilterDllName, StringComparison.OrdinalIgnoreCase));

        foreach (var item in filtered)
        {
            DisplayNodes.Add(item);
        }
    }

    private async Task ScanGodotDllInfoAsync()
    {
        if (string.IsNullOrEmpty(SelectDllPath) || !File.Exists(SelectDllPath))
        {
            _logger.LogWarningWithArea(E_LogArea.LogFilter, "DLL 路径无效，无法扫描");
            return;
        }

        await Task.Run(() =>
        {
            // 1. 准备加载上下文
            var resolver = new AssemblyDependencyResolver(SelectDllPath);
            var context = new AssemblyLoadContext("ScanContext", isCollectible: true);

            context.Resolving += (ctx, name) =>
            {
                string? path = resolver.ResolveAssemblyToPath(name);
                if (path != null) return ctx.LoadFromAssemblyPath(path);

                var dir = Path.GetDirectoryName(SelectDllPath);
                var dllName = $"{name.Name}.dll";
                var fallbackPath = Path.Combine(dir!, dllName);
                return File.Exists(fallbackPath) ? ctx.LoadFromAssemblyPath(fallbackPath) : null;
            };

            try
            {
                var assembly = context.LoadFromAssemblyPath(SelectDllPath);

                // 2. 获取类型，处理加载异常
                IEnumerable<Type> types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null)!;
                    _logger.LogWarningWithArea(E_LogArea.LogFilter, "部分依赖缺失，已加载可用类型");
                }

                // 3. 筛选 Node 类名
                var scannedNames = types
                    .Where(t => t.IsClass && !t.IsAbstract && IsDerivedFrom(t, "Godot.Node"))
                    .Select(t => t.FullName ?? t.Name)
                    .Distinct()
                    .ToList();

                // 4. 回到 UI 线程进行数据合并与增量更新
                Dispatcher.UIThread.Post(() =>
                {
                    // A. 记录旧状态
                    var oldStateMap = _allNodes.ToDictionary(x => x.FullName, x => x.IsEnabled);

                    // B. 构建新列表，并注入 OnChanged 回调
                    var mergedResults = scannedNames.Select(name => new NodeFilterItem
                    {
                        FullName = name,
                        IsEnabled = oldStateMap.TryGetValue(name, out bool wasEnabled) && wasEnabled,
                        OnChanged = () => IsDirty = true
                    }).ToList();

                    // C. 智能判断：如果扫描前后的 FullName 列表完全一致，则不需要标记 IsDirty
                    bool structureChanged = !scannedNames.SequenceEqual(_allNodes.Select(x => x.FullName));

                    // 更新底层数据源
                    _allNodes = mergedResults;

                    // 触发 UI 列表刷新
                    RefreshDisplayList();

                    if (structureChanged)
                        IsDirty = true; // 结构变了（比如扫描到了新类），需要保存

                    _logger.LogInformationWithArea(E_LogArea.LogFilter, "扫描完成，同步了 {count} 个 Node 类", _allNodes.Count);
                });
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithArea(E_LogArea.LogFilter, ex, "反射扫描失败");
            }
            finally
            {
                // 卸载上下文释放 DLL 占用
                context.Unload();
            }
        });
    }
    private bool IsDerivedFrom(Type? type, string targetBaseName)
    {
        while (type != null)
        {
            if (type.FullName == targetBaseName) return true;
            type = type.BaseType;
        }

        return false;
    }

    private async Task SelectFileAsync(IEnumerable<IStorageItem> files)
    {
        var file = files.FirstOrDefault();
        if (file == null) return;
        SelectDllPath = file.Path.LocalPath;
        SelectDllName = file.Name;
        _logger.LogInformationWithArea(E_LogArea.LogFilter, "选择了Dll:{name}", file.Name);
    }

    public void Load()
    {
        var saveModel = _jsonPersistenceService.Load<LogFilterSaveModel>(LogFilterKeyName);
        if (saveModel != null)
        {
            SelectDllName = saveModel.SelectDllName;
            SelectDllPath = saveModel.SelectDllPath;

            // 将 纯数据 List 转为 可绑定的对象
            _allNodes = saveModel.SavedNodes.Select(dto => new NodeFilterItem
            {
                FullName = dto.FullName,
                IsEnabled = dto.IsEnabled,
                OnChanged = () => IsDirty = true
            }).ToList();

            RefreshDisplayList();
        }
    }

    public void Save()
    {
        var saveModel = new LogFilterSaveModel
        {
            SelectDllPath = SelectDllPath,
            SelectDllName = SelectDllName,
            // 将 Observable 列表转为 纯数据 List
            SavedNodes = _allNodes.Select(x => x.ToDto()).ToList()
        };
        _jsonPersistenceService.Save(LogFilterKeyName, saveModel);
        IsDirty = false;
    }

    public bool IsLoaded { get; private set; }
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (!IsLoaded)
        {
            Load();
            IsLoaded = true;
        }
    }
    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) => Save();
}
