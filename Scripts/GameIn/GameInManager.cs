using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Entity.Core;
using DDemo.Scripts.GameHander;
using DDemo.Scripts.GameIn.EnvironmentContext;
using DDemo.Scripts.Test.LoggerExtensions;
using Godot;
using Godot.DependencyInjection.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDemo.Scripts.GameIn
{
	[Meta(typeof(IAutoNode))]
	public partial class GameInManager : Node2D,
		IProvide<PlayerContext>,
		IProvide<AIUnitContext>,
		IProvide<MapContext>
	{
		public override void _Notification(int what) => this.Notify(what);
		[Export]
		private Node2D? TestMainBase { get; set; }
		[Inject]
		private ILogger<GameInManager> _logger=default!;

		private PlayerContext _playerContext = new PlayerContext();
		private AIUnitContext _aiUnitContext = new AIUnitContext();
		private MapContext _mapContext = new MapContext();
		PlayerContext IProvide<PlayerContext>.Value() => _playerContext;
		AIUnitContext IProvide<AIUnitContext>.Value() => _aiUnitContext;
		MapContext IProvide<MapContext>.Value() => _mapContext;
            
		public void OnReady()
		{
			if(TestMainBase != null) 
			_mapContext.TargetPos.Add(TestMainBase);
		}

        public void Setup()
        {
            // Call the this.Provide() method once your dependencies have been initialized.
            this.Provide();
        }
    }
}
