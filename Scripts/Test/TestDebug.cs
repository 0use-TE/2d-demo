using Chickensoft.Introspection;
using Godot;
using Godot.DependencyInjection.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chickensoft.AutoInject;
using DDemo.Scripts.Characters.Core;
using DDemo.Scripts.GameIn.EnvironmentContext;
namespace DDemo.Scripts.Test
{
	[Meta(typeof(IAutoNode))]
	public partial class TestDebug:Node2D
	{
        public override void _Notification(int what) => this.Notify(what);

		[Inject]
		public ILogger<TestDebug> Logger { get; set; } = default!;
        [Node(nameof(Node2D))]
		private Node2D Node2D { get; set; } = default!;

		[Dependency]
		private PlayerContext _playerContext => this.DependOn<PlayerContext>();
        public void OnResolved()
        {
        }
        public override void _Process(double delta)
		{
            Logger.LogInformation("I am in Debug's Scene");

        }
    }
}
