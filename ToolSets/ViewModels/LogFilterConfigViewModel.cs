using Avalonia.Platform.Storage;
using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSets.Services;
using ToolSets.Shared;

namespace ToolSets.ViewModels
{
    internal class LogFilterConfigViewModel:ViewModelBase,IDialogAware
    {
        private readonly FileDialogService _dialogService;
        private readonly ILogFilterService _logFilterService;
        private string? _dllPath;
        private IReadOnlyList<FilePickerFileType> _filters = [new FilePickerFileType("程序集") { Patterns = ["*.dll"] }];

        public LogFilterConfigViewModel(FileDialogService dialogService, ILogFilterService logFilterService)
        {
            _dialogService = dialogService;
            _logFilterService = logFilterService;
            _dllPath = _logFilterService.GetDllPath();

            OpenSelectDllDialogCommand = new DelegateCommand(async () => await ExecuteOpenSelectDllDialog());
            SaveCommand = new DelegateCommand(() => RequestClose.Invoke(new DialogResult(ButtonResult.OK)));
        }

        public string? DllPath
        {
            get => _dllPath;
            set => SetProperty(ref _dllPath, value);
        }

        public DelegateCommand OpenSelectDllDialogCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DialogCloseListener RequestClose { get; set; }

        private async Task ExecuteOpenSelectDllDialog()
        {
           using  var file = await _dialogService.OpenFileAsync("选择程序集", _filters);
            if (file != null)
            {
                DllPath = file.Path.LocalPath;
                _logFilterService.SaveDllPath(DllPath);
                Debug.WriteLine($"已选择 DLL: {DllPath}");
            }
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
