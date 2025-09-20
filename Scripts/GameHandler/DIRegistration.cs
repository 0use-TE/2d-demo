using DDemo.Scripts.GameIn;
using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Godot;
using System;
using System.Reflection;
namespace DDemo.Scripts.GameHander
{
    public partial class DIRegistration : Node2D, IServicesConfigurator
    {
        /// <summary>
        /// 注册全局均使用的服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Log congiguration
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Godot()
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            //Godot Services
            services.AddGodotServices();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        }
    }
}