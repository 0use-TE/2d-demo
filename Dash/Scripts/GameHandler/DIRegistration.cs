using Dash.Scripts.GameHandler.Services;
using GameDevTools.Share.ShareModel.LogFilter;
using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            GD.PushError("Ouse");
            services.AddMessagePipe(options =>
            {

            });

            var filterService = new LogFilterService();
            services.AddSingleton<ILogFilterService>(filterService);
            services.AddLogging(builder =>
            {
                builder.AddFilter((captuee, level) => true);
            });
        }
    }
}