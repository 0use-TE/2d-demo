using DDemo.Scripts.GameIn;
using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
            //Godot Logger
            services.AddGodotLogger();
            //Godot Services
            services.AddGodotServices();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		}
    }
}