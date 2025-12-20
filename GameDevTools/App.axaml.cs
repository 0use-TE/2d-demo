using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using GameDevTools.Services;
using GameDevTools.Services.DataPersistences;
using GameDevTools.ViewModels;
using GameDevTools.Views;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
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
                .CreateLogger();

            var serviceColllection = new ServiceCollection();
            serviceColllection.AddSingleton<INotificationService, NotificationService>();
            serviceColllection.AddSingleton<IJsonPersistenceService, JsonPersistenceService>();
            //Logging
            serviceColllection.AddLogging(builder =>
            {
                builder.AddSerilog(dispose: true);
            });

            //Pupulate ServiceCollection To DryIoc
            containerRegistry.GetContainer().Populate(serviceColllection);

            // Register you Services, Views, Dialogs, etc.

        }
        protected override void OnInitialized()
        {
            //参数化NotificationHost
            Container.Resolve<INotificationService>().SetHostWindow((MainWindow as Window) ?? throw new InvalidOperationException("主窗口设置失败!"));
            base.OnInitialized();
        }
    }
}
