using Godot;
using Godot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace DDemo.Scripts.GameHander
{
    public partial class DIRegistration : Node2D, IServicesConfigurator
    {

        public void ConfigureServices(IServiceCollection services)
        {
            //Godot Logger
            services.AddGodotLogger();
            //Godot Services
            services.AddGodotServices();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //Used for create object pools

        }
    }
}