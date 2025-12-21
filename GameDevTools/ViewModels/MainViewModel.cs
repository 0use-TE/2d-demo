using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Notifications;
using GameDevTools.Services;
using GameDevTools.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation.Regions;

namespace GameDevTools.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        public DelegateCommand NavigateToLogFilter { get; set; }
        public MainViewModel(IContainerProvider containerProvider,IRegionManager regionManager)
        {
            _containerProvider = containerProvider;
            _regionManager = regionManager;
            NavigateToLogFilter = new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("MainView", nameof(LogFilterView));
            });
        }
    }
}
