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

namespace ToolSets.ViewModels
{
    internal class LogFilterConfigViewModel:ViewModelBase,IDialogAware
    {
        private FileDialogService _dialogService;
        private string? _dllPath;
        private IReadOnlyList<FilePickerFileType> _filters = [new FilePickerFileType("程序集") { Patterns = ["*.dll"] }];

        public LogFilterConfigViewModel(FileDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public DelegateCommand OpenSelectDllDIalog => new DelegateCommand(async () =>
        {
            var file = await _dialogService.OpenFileAsync("选择程序集", _filters);
            Debug.WriteLine(file?.Path.LocalPath);
            _dllPath = file?.Path.LocalPath;
        });

        public DialogCloseListener RequestClose { get; set; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
