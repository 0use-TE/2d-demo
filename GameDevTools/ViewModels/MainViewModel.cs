using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Notifications;
using GameDevTools.Services;
using Prism.Commands;

namespace GameDevTools.ViewModels
{
    internal class MainViewModel:ViewModelBase
    {
        public ViewModelBase Page { get; set; } = new LogFilterViewModel();
        public MainViewModel()
        {
        
        }
    }
}
