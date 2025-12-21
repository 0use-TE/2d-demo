using Dash.Scripts.GameHandler.Services;
using GameDevTools.Share.ShareModel.LogFilter;
using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Serilog;
using System;
using System.Linq;
using System.Reflection;

namespace Dash.Scripts.GameHander
{
    public partial class DIRegistration : Node2D, IServicesConfigurator
    {
        /// <summary>
        /// 注册全局均使用的服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Godot Services
            services.AddGodotServices();

            services.AddSingleton<DefaultObjectPoolProvider>();

            services.AddMessagePipe(options =>
            {

            });

            var filterService = new LogFilterService();
            services.AddSingleton<ILogFilterService>(filterService);
            services.AddLogging(builder =>
            {
                builder.AddFilter((capegory, level) =>
                {
                    return filterService.IsAllowed(capegory);
                });
            });
        }
    }
}