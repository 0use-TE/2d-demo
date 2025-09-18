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
namespace DDemo.Scripts.Test
{
	[Meta(typeof())]
	public partial class TestDebug:Node2D
	{

        [Inject]
		private ILogger<TestDebug> _logger = default!;
		[Inject]
		private SceneTree SceneTree = default!;
		public override void _Process(double delta)
		{
		}
	}
}
