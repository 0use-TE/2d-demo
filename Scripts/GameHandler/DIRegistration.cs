using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using ToolSets.Shared;

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

            Log.Logger = new LoggerConfiguration()
                .CreateLogger();
            
            services.AddMessagePipe(options => {

            });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(dispose: true);
#if TOOLS
                //builder.AddFilter((category, logLevel) =>
                //{
                //    var rules = LogFilterService.LoadRules();
                //    var rule = rules
                //        .FirstOrDefault(r => r.TypeName == category);
                //    if (rule == null || !rule.IsEnabled)
                //        return false;
                //    return true;
                //});
#endif
            });
            //Godot Services
            services.AddGodotServices();
        }
    }
}