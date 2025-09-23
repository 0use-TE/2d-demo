using Chickensoft.Introspection;
using Godot;
using Godot.DependencyInjection.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;
using Chickensoft.AutoInject;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Test.LoggerExtensions;
namespace DDemo.Scripts.Test
{
	[Meta(typeof(IAutoNode))]
	public partial class TestDebug:Node2D
	{

        public override void _Notification(int what) => this.Notify(what);
		[Inject]
		public ILogger<TestDebug> Logger { get; set; } = default!;

		[Dependency]
		private PlayerContext _playerContext => this.DependOn<PlayerContext>();
        public void OnResolved()
		{

        }
        public override void _Process(double delta)
		{
        }
    }
}
