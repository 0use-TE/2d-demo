using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using GameDevTools.Services;
using GameDevTools.Services.DataPersistenceServices;
using GameDevTools.ViewModels;
using GameDevTools.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Container.DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation.Regions;
using Serilog;

namespace GameDevTools
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            // Required when overriding Initialize
            base.Initialize();
        }

        protected override AvaloniaObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Debug()
                .CreateLogger();

            var serviceColllection = new ServiceCollection();
            serviceColllection.AddSingleton<INotificationService, NotificationService>();
            serviceColllection.AddSingleton<IJsonPersistenceService, JsonPersistenceService>();
            //Logging
            serviceColllection.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            });

            //Pupulate ServiceCollection To DryIoc
            containerRegistry.GetContainer().Populate(serviceColllection);

            // Register you Services, Views, Dialogs, etc.
            containerRegistry.RegisterForNavigation<LogFilterView>();

            //添加ViewLocator
            var viewLocator = Container.Resolve<ViewLocator>();
            DataTemplates.Add(viewLocator);

        }
        protected override void OnInitialized()
        {
            //参数化NotificationHost
            Container.Resolve<INotificationService>().SetHostWindow((MainWindow as Window) ?? throw new InvalidOperationException("主窗口设置失败!"));
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app)
            {
                app.Exit += App_Exit;
            }
            //显示Windows
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate("MainView", nameof(LogFilterView));
        }

        private void App_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            var regionManager = Container.Resolve<IRegionManager>();

            foreach (var region in regionManager.Regions)
            {
                // 1. 遍历区域内所有的 View
                foreach (var view in region.Views)
                {
                    // 2. 尝试从 View 的 DataContext 获取 VM
                    // 这里判断是否实现了 IViewModelDataInit 接口
                    if (view is AvaloniaObject avaloniaObj &&
                        avaloniaObj.GetValue(Control.DataContextProperty) is IViewModelDataInit vm)
                        vm.Save();
                }
            }
        }
    }
}
