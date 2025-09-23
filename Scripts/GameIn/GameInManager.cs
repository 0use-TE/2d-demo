using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using DDemo.Scripts.Characters.Core;
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
			//获取所有玩家，开发场景使用，正常都是游戏外部创建玩家传入的
			#region
			_playerContext.Players = GetTree().GetNodesInGroup("Player").OfType<PlayerBase>().ToList();
			#endregion
		}

        public void Setup()
        {
            // Call the this.Provide() method once your dependencies have been initialized.
            this.Provide();
        }
    }
}
