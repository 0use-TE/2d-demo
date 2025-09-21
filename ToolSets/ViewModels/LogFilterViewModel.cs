using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace ToolSets.ViewModels
{
    internal class LogFilterViewModel : ViewModelBase
    {

        private string? _dllPath;
        public DelegateCommand OpenSelectDllDIalog => new DelegateCommand(() =>
        {
            //打开文件对话框，选择dll
        });
        public LogFilterViewModel()
        {

        }

    }
}
