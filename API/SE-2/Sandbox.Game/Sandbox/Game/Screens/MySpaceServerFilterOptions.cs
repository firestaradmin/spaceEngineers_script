using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
	public class MySpaceServerFilterOptions : MyServerFilterOptions
	{
		public const byte SPACE_BOOL_OFFSET = 128;

		public MySpaceServerFilterOptions()
		{
		}

		public MySpaceServerFilterOptions(MyObjectBuilder_ServerFilterOptions ob)
			: base(ob)
		{
		}

		public MyFilterRange GetFilter(MySpaceNumericOptionEnum key)
		{
			return (MyFilterRange)base.Filters[(byte)key];
		}

		public MyFilterBool GetFilter(MySpaceBoolOptionEnum key)
		{
			return (MyFilterBool)base.Filters[(byte)key];
		}

		protected override Dictionary<byte, IMyFilterOption> CreateFilters()
		{
			Dictionary<byte, IMyFilterOption> dictionary = new Dictionary<byte, IMyFilterOption>();
			foreach (byte value in Enum.GetValues(typeof(MySpaceNumericOptionEnum)))
			{
				dictionary.Add(value, new MyFilterRange());
			}
			foreach (byte value2 in Enum.GetValues(typeof(MySpaceBoolOptionEnum)))
			{
				dictionary.Add(value2, new MyFilterBool());
			}
			return dictionary;
		}

		public override bool FilterServer(MyCachedServerItem server)
		{
			MyObjectBuilder_SessionSettings settings = server.Settings;
			if (settings == null)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by no settings");
				return false;
			}
			if (!GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(settings.InventorySizeMultiplier))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by InventorySizeMultiplier");
				return false;
			}
			if (!GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility).IsMatch((float)settings.EnvironmentHostility))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by EnvironmentHostility");
				return false;
			}
			MyFilterRange filter = GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
			if (!filter.IsMatch(settings.AssemblerEfficiencyMultiplier) || !filter.IsMatch(settings.AssemblerSpeedMultiplier) || !filter.IsMatch(settings.RefinerySpeedMultiplier))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by ProductionMultipliers");
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Spectator).IsMatch(settings.EnableSpectator))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.CopyPaste).IsMatch(settings.EnableCopyPaste))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.ThrusterDamage).IsMatch(settings.ThrusterDamage))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.PermanentDeath).IsMatch(settings.PermanentDeath))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Weapons).IsMatch(settings.WeaponsEnabled))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.CargoShips).IsMatch(settings.CargoShipsEnabled))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.BlockDestruction).IsMatch(settings.DestructibleBlocks))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Scripts).IsMatch(settings.EnableIngameScripts))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Oxygen).IsMatch(settings.EnableOxygen))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.ThirdPerson).IsMatch(settings.Enable3rdPersonView))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Encounters).IsMatch(settings.EnableEncounters))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Airtightness).IsMatch(settings.EnableOxygenPressurization))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.UnsupportedStations).IsMatch(settings.StationVoxelSupport))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.VoxelDestruction).IsMatch(settings.EnableVoxelDestruction))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Drones).IsMatch(settings.EnableDrones))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Wolves).IsMatch(settings.EnableWolfs))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.Spiders).IsMatch(settings.EnableSpiders))
			{
				return false;
			}
			if (!GetFilter(MySpaceBoolOptionEnum.RespawnShips).IsMatch(settings.EnableRespawnShips))
			{
				return false;
			}
			if (server.Rules == null || !GetFilter(MySpaceBoolOptionEnum.ExternalServerManagement).IsMatch(server.Rules.ContainsKey("SM")))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by ExternalServerManagement");
				return false;
			}
			return true;
		}

		public override bool FilterLobby(IMyLobby lobby)
		{
			if (!GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f)))
			{
				return false;
			}
			MyFilterRange filter = GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
			if (!filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f)) || !filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f)))
			{
				return false;
			}
			return true;
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MySessionSearchFilter GetNetworkFilter(MySupportedPropertyFilters supportedFilters, string searchText)
		{
			MySessionSearchFilterHelper mySessionSearchFilterHelper = MySessionSearchFilterHelper.Create(supportedFilters);
			if (!string.IsNullOrWhiteSpace(searchText))
			{
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_NAMES", MySearchCondition.Contains, searchText);
			}
			if (SameVersion)
			{
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_DATA", MySearchCondition.Equal, MyFinalBuildConstants.APP_VERSION);
			}
			if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
			{
				mySessionSearchFilterHelper.AddCustomConditional("CONSOLE_COMPATIBLE", MySearchCondition.Equal, "1");
			}
			if (CheckPlayer)
			{
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_PLAYER_COUNT", MySearchCondition.GreaterOrEqual, PlayerCount.Min);
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_PLAYER_COUNT", MySearchCondition.LesserOrEqual, PlayerCount.Max);
			}
			if (SurvivalMode != CreativeMode)
			{
				if (SurvivalMode)
				{
					mySessionSearchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, "gamemodeS");
				}
				if (CreativeMode)
				{
					mySessionSearchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, "gamemodeC");
				}
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES && SameData)
			{
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, "datahash" + MyDataIntegrityChecker.GetHashBase64());
			}
			if (Ping > -1)
			{
				mySessionSearchFilterHelper.AddConditional("SERVER_PROP_PING", MySearchCondition.LesserOrEqual, Ping);
			}
			return mySessionSearchFilterHelper.Filter;
		}
	}
}
