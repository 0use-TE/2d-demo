using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GameDevTools.Extensions;
using GameDevTools.Services.DataPersistenceServices;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation.Regions;

namespace GameDevTools.ViewModels
{
    internal class LogFilterViewModel : ViewModelBase, IViewModelDataInit,INavigationAware
    {
        private class LogFilterSaveModel
        {
            public string SelectDllPath { get; set; } = "暂无选择";
            public string SelectDllName { get; set; } = "暂无选择";
        };

        public AsyncDelegateCommand<IEnumerable<IStorageItem>> SelectFileCommad { get; set; }
        private readonly ILogger<LogFilterViewModel> _logger;
        private readonly IJsonPersistenceService _jsonPersistenceService;
        private IStorageItem? _selectStorageItem;
        private const string LogFilterKeyName = "LogFilter";
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

        public bool IsLoaded { get; private set; }

        public LogFilterViewModel(ILogger<LogFilterViewModel> logger, IJsonPersistenceService jsonPersistenceService)
        {
            _logger = logger;
            _jsonPersistenceService = jsonPersistenceService;
            SelectFileCommad = new AsyncDelegateCommand<IEnumerable<IStorageItem>>(async (files) =>
            {
                try
                {
                    var file = files.FirstOrDefault();
                    if (file == null)
                    {
                        _logger.LogWarningWithArea(E_LogArea.LogFilter, "找不到文件!");
                        return;
                    }

                    _selectStorageItem = file;
                    SelectDllPath = file.Path.LocalPath;
                    SelectDllName = file.Name;
                    _logger.LogInformationWithArea(E_LogArea.LogFilter, "选择了Dll:{name}", file.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogErrorWithArea(E_LogArea.LogFilter, ex, "发生异常错误");
                }
            });
        }

        public void Load()
        {
            var saveModel = _jsonPersistenceService.Load<LogFilterSaveModel>(LogFilterKeyName);
            SelectDllName = saveModel.SelectDllName;
            SelectDllPath = saveModel.SelectDllPath;
        }

        public void Save()
        {
            var saveModel = new LogFilterSaveModel
            {
                SelectDllPath = SelectDllPath,
                SelectDllName = SelectDllName
            };
            _jsonPersistenceService.Save(LogFilterKeyName, saveModel);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(!IsLoaded )
            {
                Load();
                IsLoaded = true;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Save();
        }
    }
}
