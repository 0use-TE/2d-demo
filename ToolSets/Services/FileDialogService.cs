using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ToolSets.Views;

namespace ToolSets.Services
{
    public class FileDialogService
    {
        private readonly MainWindow _mainWindow;
        public FileDialogService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public async Task<IStorageFile?> OpenFileAsync(string title,IReadOnlyList<FilePickerFileType> filePickerFileTypes)
        {
            var topLevel = _mainWindow;
            if(topLevel==null)
            {
                Debug.WriteLine("topLevel is null");
                return  null;
            }

            // 启动异步操作以打开对话框。
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = filePickerFileTypes
            });

            if (files.Count >= 1)
            {
                return files[0];
            }
        
            return null;
        }

        
    }
}
