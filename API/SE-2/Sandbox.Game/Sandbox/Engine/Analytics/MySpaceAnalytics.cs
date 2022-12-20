using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
<<<<<<< HEAD
=======
using Sandbox.Game.GameSystems;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Analytics;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Replication;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Engine.Analytics
{
	[StaticEventOwner]
	public sealed class MySpaceAnalytics : MyAnalyticsManager
	{
		protected sealed class NotifyAnalyticsIds_003C_003ESystem_String_0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string userId, in string sessionId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				NotifyAnalyticsIds(userId, sessionId);
			}
		}

		private static MySpaceAnalytics m_instance;

		private bool m_firstRun;

		private bool m_isSessionStarted;

		private bool m_isSessionEnded;

		private string m_hashedUserID;

		private string m_sessionID;

		private string m_clientVersion;

		private string m_platform;

		private Dictionary<string, MyProduct> m_products;
<<<<<<< HEAD

		private MySessionStartEvent m_defaultSessionData;
=======

		private MySessionStartEvent m_defaultSessionData;

		private MyWorldStartEvent m_defaultWorldData;

		private MyDamageInformation m_lastDamageInformation = new MyDamageInformation
		{
			Type = MyStringHash.NullOrEmpty
		};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyWorldStartEvent m_defaultWorldData;

<<<<<<< HEAD
		private MyDamageInformation m_lastDamageInformation = new MyDamageInformation
		{
			Type = MyStringHash.NullOrEmpty
		};

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyWorldEntryEnum m_worldEntry;

		private DateTime m_loadingStartedAt;

		private DateTime m_worldStartTime;

		private string m_worldSessionID;

		private int m_serverMostConcurrentPlayers;

		private IMyAnalytics m_heartbeatTracker;
<<<<<<< HEAD
=======

		private TimeSpan m_heartbeatPeriod;

		private MyHeartbeatEvent m_heartbeatEvent;

		private bool m_isHeartbeatEnabled;

		private DateTime m_lastHeartbeatSent;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private TimeSpan m_heartbeatPeriod;

		private MyHeartbeatEvent m_heartbeatEvent;

		private bool m_isHeartbeatEnabled;

		private DateTime m_lastHeartbeatSent;

		/// <summary>
		/// Gets a singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static MySpaceAnalytics Instance
		{
			get
			{
				if (m_instance == null)
				{
					MySpaceAnalytics value = new MySpaceAnalytics();
					Interlocked.CompareExchange(ref m_instance, value, null);
				}
				return m_instance;
			}
		}

		public string UserId => m_hashedUserID;

		private MySpaceAnalytics()
		{
		}

		public void StartSessionAndIdentifyPlayer(ulong userId, bool firstTimeRun)
		{
			m_hashedUserID = HashUserId(userId);
			StartSession(firstTimeRun);
		}

		public void StartSessionAndIdentifyPlayer(string hashedUserId, bool firstTimeRun)
		{
			m_hashedUserID = hashedUserId;
			StartSession(firstTimeRun);
		}

<<<<<<< HEAD
		public void ReportDropContainer(bool competetive)
		{
			MyDropContainerEvent analyticsEvent = new MyDropContainerEvent(m_defaultWorldData)
			{
=======
		public void ReportGoodBotQuestion(ResponseType responseType, string question, string responseId)
		{
			MyGoodBotEvent analyticsEvent = new MyGoodBotEvent(m_defaultWorldData)
			{
				GoodBot_ResponseType = responseType,
				GoodBot_Question = question,
				GoodBot_ResponseID = responseId
			};
			ReportCurrentEvent(analyticsEvent);
		}

		public void ReportDropContainer(bool competetive)
		{
			MyDropContainerEvent analyticsEvent = new MyDropContainerEvent(m_defaultWorldData)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Competetive = competetive
			};
			ReportCurrentEvent(analyticsEvent);
		}

		public void ReportBannerClick(string caption, uint packageID)
		{
			MyBannerClickEvent analyticsEvent = new MyBannerClickEvent(m_defaultSessionData)
			{
				banner_caption = caption,
				banner_package_id = packageID
			};
			ReportCurrentEvent(analyticsEvent);
		}

		public void ReportSessionEnd(string sessionEndReason)
		{
			if (TryEndSession(sessionEndReason, out var sessionEndEvent, out var _))
			{
				ReportCurrentEvent(sessionEndEvent);
			}
		}

		public void ReportSessionEndByCrash(Exception exception)
<<<<<<< HEAD
		{
			EndSessionAndReportLater("GameCrash", exception);
		}

		public void StoreWorldLoadingStartTime()
		{
			m_loadingStartedAt = DateTime.UtcNow;
		}

		public void SetWorldEntry(MyWorldEntryEnum entry)
		{
=======
		{
			EndSessionAndReportLater("GameCrash", exception);
		}

		public void StoreWorldLoadingStartTime()
		{
			m_loadingStartedAt = DateTime.UtcNow;
		}

		public void SetWorldEntry(MyWorldEntryEnum entry)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_worldEntry = entry;
		}

		public void ReportWorldStart(MyObjectBuilder_SessionSettings settings)
		{
			if (m_isSessionStarted)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => NotifyAnalyticsIds, m_hashedUserID, m_sessionID);
				m_worldSessionID = Guid.NewGuid().ToString();
				m_serverMostConcurrentPlayers = 0;
				m_defaultWorldData = GatherWorldStartData(settings);
				m_worldStartTime = DateTime.UtcNow;
				ReportCurrentEvent(m_defaultWorldData);
			}
		}

		public void ReportWorldEnd()
		{
			if (m_isSessionStarted)
			{
				MyWorldEndEvent myWorldEndEvent = GatherWorldEndData();
				if (myWorldEndEvent.WorldStartProperties == null)
				{
					m_worldSessionID = null;
					return;
				}
				ReportCurrentEvent(myWorldEndEvent);
				m_worldSessionID = null;
			}
		}

		public void SetLastDamageInformation(MyDamageInformation lastDamageInformation)
		{
			if (!(lastDamageInformation.Type == default(MyStringHash)))
			{
				m_lastDamageInformation = lastDamageInformation;
			}
		}

		public void OnSuspending()
		{
			EndSessionAndReportLater("Suspended");
		}

		public void OnResuming()
		{
			StartSession(firstTimeRun: false);
			if (MySession.Static != null)
			{
				MySession.Static.ResetStatistics();
				ReportWorldStart(MySession.Static.Settings);
			}
		}

		public void RegisterHeartbeatTracker(IMyAnalytics heartbeatTracker, TimeSpan heartbeatPeriod)
		{
			m_heartbeatTracker = heartbeatTracker ?? throw new ArgumentNullException("heartbeatTracker can't be null");
			m_heartbeatPeriod = heartbeatPeriod;
			m_lastHeartbeatSent = DateTime.MinValue;
		}

		public void Update(MyTimeSpan elapsedTime)
		{
			TrySendHeartbeat();
			if (MySession.Static != null && MyMultiplayer.Static != null)
			{
				int num = ((MyMultiplayer.Static is MyMultiplayerClient) ? 1 : 0);
				m_serverMostConcurrentPlayers = Math.Max(m_serverMostConcurrentPlayers, MyMultiplayer.Static.MemberCount - num);
			}
		}

		private void StartSession(bool firstTimeRun)
		{
			if (!m_isSessionStarted)
			{
				if (m_hashedUserID == null)
				{
					throw new Exception("UserID not set before SessionStart");
				}
				((IMyAnalytics)this).ReportPostponedEvents();
				m_firstRun = firstTimeRun;
				m_defaultSessionData = GatherSessionStartData();
				m_products = GetProducts();
				m_clientVersion = MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString();
				m_platform = MyPerGameSettings.BasicGameInfo.GameAcronym;
				m_sessionID = Guid.NewGuid().ToString();
				ReportCurrentEvent(m_defaultSessionData);
				m_isSessionEnded = false;
				m_isSessionStarted = true;
				MyLog.Default.WriteLine("Analytics session started: UserID = (" + m_hashedUserID + ") SessionID = (" + m_sessionID + ")");
				StartHeartbeat();
<<<<<<< HEAD
			}
		}

		private void ReportCurrentEvent(MyAnalyticsEvent analyticsEvent)
		{
			((IMyAnalytics)this).ReportEvent((IMyAnalyticsEvent)analyticsEvent, DateTime.UtcNow, m_sessionID, m_hashedUserID, m_clientVersion, m_platform, (Exception)null);
		}

		private void EndSessionAndReportLater(string reason, Exception exception = null)
		{
			if (MySession.Static != null && m_isSessionStarted)
			{
				MyWorldEndEvent analyticsEvent = GatherWorldEndData();
				((IMyAnalytics)this).ReportEventLater((IMyAnalyticsEvent)analyticsEvent, DateTime.UtcNow, m_sessionID, m_hashedUserID, m_clientVersion, m_platform, (Exception)null);
			}
			if (TryEndSession(reason, out var sessionEndEvent, out var sessionId))
			{
				((IMyAnalytics)this).ReportEventLater((IMyAnalyticsEvent)sessionEndEvent, DateTime.UtcNow + TimeSpan.FromMilliseconds(1.0), sessionId, m_hashedUserID, m_clientVersion, m_platform, exception);
			}
		}

		private Dictionary<string, MyProduct> GetProducts()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
=======
			}
		}

		private void ReportCurrentEvent(MyAnalyticsEvent analyticsEvent)
		{
			((IMyAnalytics)this).ReportEvent((IMyAnalyticsEvent)analyticsEvent, DateTime.UtcNow, m_sessionID, m_hashedUserID, m_clientVersion, m_platform, (Exception)null);
		}

		private void EndSessionAndReportLater(string reason, Exception exception = null)
		{
			if (MySession.Static != null && m_isSessionStarted)
			{
				MyWorldEndEvent analyticsEvent = GatherWorldEndData();
				((IMyAnalytics)this).ReportEventLater((IMyAnalyticsEvent)analyticsEvent, DateTime.UtcNow, m_sessionID, m_hashedUserID, m_clientVersion, m_platform, (Exception)null);
			}
			if (TryEndSession(reason, out var sessionEndEvent, out var sessionId))
			{
				((IMyAnalytics)this).ReportEventLater((IMyAnalyticsEvent)sessionEndEvent, DateTime.UtcNow + TimeSpan.FromMilliseconds(1.0), sessionId, m_hashedUserID, m_clientVersion, m_platform, exception);
			}
		}

		private Dictionary<string, MyProduct> GetProducts()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Dictionary<string, MyProduct> dictionary = new Dictionary<string, MyProduct>();
				if (MyGameService.IsProductOwned(MyGameService.AppId, out var purchaseTime))
				{
					MyProduct myProduct = new MyProduct
					{
						ProductID = MyGameService.AppId.ToString(),
						ProductName = "MainGame"
					};
					if (purchaseTime.HasValue)
					{
						myProduct.PurchaseTimestamp = purchaseTime.Value;
					}
					dictionary.Add(myProduct.ProductName, myProduct);
				}
				{
					foreach (KeyValuePair<uint, MyDLCs.MyDLC> dLC in MyDLCs.DLCs)
					{
						if (MyGameService.IsProductOwned(dLC.Key, out var purchaseTime2))
						{
							MyProduct myProduct2 = new MyProduct
							{
								ProductID = dLC.Key.ToString(),
								ProductName = dLC.Value.Name,
								PackageID = dLC.Value.PackageId,
								StoreID = dLC.Value.StoreId
							};
							if (purchaseTime2.HasValue)
							{
								myProduct2.PurchaseTimestamp = purchaseTime2.Value;
							}
							dictionary.Add(myProduct2.ProductName, myProduct2);
						}
					}
					return dictionary;
				}
			}
			return null;
		}

		private MySessionStartEvent GatherSessionStartData()
		{
			MySessionStartEvent mySessionStartEvent = new MySessionStartEvent();
			mySessionStartEvent.client_version = MyPerGameSettings.BasicGameInfo.GameVersion.ToString();
			mySessionStartEvent.client_branch = MyGameService.BranchNameFriendly;
			mySessionStartEvent.cpu_info = MyVRage.Platform.System.GetInfoCPU(out var _, out var physicalCores);
<<<<<<< HEAD
			mySessionStartEvent.cpu_number_of_threads = Environment.ProcessorCount;
=======
			mySessionStartEvent.cpu_number_of_threads = Environment.get_ProcessorCount();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			mySessionStartEvent.cpu_number_of_cores = physicalCores;
			mySessionStartEvent.os_memory = MyVRage.Platform.System.GetTotalPhysicalMemory() / 1024uL / 1024uL;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyAdapterInfo myAdapterInfo = MyVideoSettingsManager.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
				mySessionStartEvent.gpu_name = myAdapterInfo.Name;
				mySessionStartEvent.gpu_memory = (int)(myAdapterInfo.VRAM / 1024uL / 1024uL);
				mySessionStartEvent.gpu_driver_version = myAdapterInfo.DriverVersion;
			}
<<<<<<< HEAD
			mySessionStartEvent.os_info = Environment.OSVersion.VersionString;
			mySessionStartEvent.os_architecture = (Environment.Is64BitOperatingSystem ? "64bit" : "32bit");
=======
			mySessionStartEvent.os_info = Environment.get_OSVersion().get_VersionString();
			mySessionStartEvent.os_architecture = (Environment.get_Is64BitOperatingSystem() ? "64bit" : "32bit");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			mySessionStartEvent.is_first_run = m_firstRun;
			mySessionStartEvent.is_dedicated = Sandbox.Engine.Platform.Game.IsDedicated;
			mySessionStartEvent.userLanguage = MyLanguage.CurrentLanguage.ToString();
			mySessionStartEvent.userLocale = MyLanguage.CurrentCultureName.ToString();
			mySessionStartEvent.os_culture = MyLanguage.GetCurrentOSCulture();
			mySessionStartEvent.region_iso2 = MyVRage.Platform.System.TwoLetterISORegionName;
			mySessionStartEvent.region_iso3 = MyVRage.Platform.System.ThreeLetterISORegionName;
			float.TryParse(MyVRage.Platform.System.RegionLongitude, out var result);
			float.TryParse(MyVRage.Platform.System.RegionLatitude, out var result2);
			mySessionStartEvent.region_location = new double[2] { result, result2 };
			mySessionStartEvent.audio_hud_warnings = MySandboxGame.Config.HudWarnings;
			mySessionStartEvent.speed_based_ship_sounds = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				mySessionStartEvent.display_resolution = MySandboxGame.Config.ScreenWidth + " x " + MySandboxGame.Config.ScreenHeight;
				mySessionStartEvent.display_window_mode = MyVideoSettingsManager.CurrentDeviceSettings.WindowMode.ToString();
				mySessionStartEvent.graphics_anisotropic_filtering = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering.ToString();
				mySessionStartEvent.graphics_antialiasing_mode = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode.ToString();
				mySessionStartEvent.graphics_shadow_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality.ToString();
				mySessionStartEvent.graphics_texture_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality.ToString();
				mySessionStartEvent.graphics_voxel_texture_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelTextureQuality.ToString();
				mySessionStartEvent.graphics_voxel_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality.ToString();
				mySessionStartEvent.graphics_grass_density_factor = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor;
				mySessionStartEvent.graphics_grass_draw_distance = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance;
				mySessionStartEvent.graphics_flares_intensity = MyVideoSettingsManager.CurrentGraphicsSettings.FlaresIntensity;
				mySessionStartEvent.graphics_voxel_shader_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality.ToString();
				mySessionStartEvent.graphics_alphamasked_shader_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality.ToString();
				mySessionStartEvent.graphics_atmosphere_shader_quality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality.ToString();
				mySessionStartEvent.graphics_distance_fade = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.DistanceFade;
				mySessionStartEvent.audio_music_volume = MySandboxGame.Config.MusicVolume;
				mySessionStartEvent.audio_sound_volume = MySandboxGame.Config.GameVolume;
				mySessionStartEvent.audio_mute_when_not_in_focus = MySandboxGame.Config.EnableMuteWhenNotInFocus;
				mySessionStartEvent.experimental_mode = MySandboxGame.Config.ExperimentalMode;
				mySessionStartEvent.building_mode = MySandboxGame.Config.CubeBuilderBuildingMode;
				mySessionStartEvent.controls_hints = MySandboxGame.Config.ControlsHints;
				mySessionStartEvent.goodbot_hints = MySandboxGame.Config.GoodBotHints;
				mySessionStartEvent.rotation_hints = MySandboxGame.Config.RotationHints;
				mySessionStartEvent.enable_steam_cloud = MySandboxGame.Config.EnableSteamCloud;
				mySessionStartEvent.enable_trading = MySandboxGame.Config.EnableTrading;
				mySessionStartEvent.gdpr_consent = MySandboxGame.Config.GDPRConsent;
				mySessionStartEvent.area_interaction = MySandboxGame.Config.AreaInteraction;
				mySessionStartEvent.mod_io_consent = MySandboxGame.Config.ModIoConsent;
				mySessionStartEvent.show_crosshair = MySandboxGame.Config.ShowCrosshair;
			}
			return mySessionStartEvent;
		}

		private MyWorldStartEvent GatherWorldStartData(MyObjectBuilder_SessionSettings settings)
		{
			MyWorldStartEvent myWorldStartEvent = new MyWorldStartEvent(m_defaultSessionData);
			myWorldStartEvent.WorldSessionID = m_worldSessionID;
			myWorldStartEvent.scenario_name = MyCampaignManager.Static.ActiveCampaignName;
			myWorldStartEvent.entry = m_worldEntry.ToString();
			myWorldStartEvent.game_mode = settings.GameMode.ToString();
			myWorldStartEvent.online_mode = settings.OnlineMode.ToString();
			myWorldStartEvent.world_type = MySession.Static.Scenario.Id.SubtypeName;
			myWorldStartEvent.worldName = MySession.Static.Name;
			myWorldStartEvent.server_is_dedicated = MyMultiplayer.Static is MyMultiplayerClient;
			MyMultiplayerClient myMultiplayerClient;
			myWorldStartEvent.server_id = (((myMultiplayerClient = MyMultiplayer.Static as MyMultiplayerClient) != null) ? myMultiplayerClient.HostAnalyticsId : "");
			myWorldStartEvent.server_max_number_of_players = ((MyMultiplayer.Static == null) ? 1 : MyMultiplayer.Static.MemberLimit);
			myWorldStartEvent.server_current_number_of_players = ((MyMultiplayer.Static == null) ? 1 : MyMultiplayer.Static.MemberCount);
			myWorldStartEvent.is_hosting_player = MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer;
			if (MyMultiplayer.Static != null)
			{
				if (MySession.Static != null && MySession.Static.LocalCharacter != null && MyMultiplayer.Static.HostName.Equals(MySession.Static.LocalCharacter.DisplayNameText))
				{
					myWorldStartEvent.multiplayer_type = "Host";
				}
				else if (MyMultiplayer.Static.HostName.Equals("Dedicated server"))
				{
					myWorldStartEvent.multiplayer_type = "Dedicated server";
				}
				else
				{
					myWorldStartEvent.multiplayer_type = "Client";
				}
			}
			else
			{
				myWorldStartEvent.multiplayer_type = "Off-line";
			}
			myWorldStartEvent.active_mods = GetModList();
			myWorldStartEvent.active_mods_count = MySession.Static.Mods.Count;
			long value = (long)Math.Ceiling((DateTime.UtcNow - m_loadingStartedAt).TotalSeconds);
			myWorldStartEvent.loading_duration = value;
			myWorldStartEvent.networking_type = MyGameService.Networking?.ServiceName;
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			bool flag = MyCampaignManager.Static != null && MyCampaignManager.Static.ActiveCampaign != null;
			myWorldStartEvent.is_campaign_mission = flag;
			myWorldStartEvent.is_official_campaign = flag && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla;
<<<<<<< HEAD
			myWorldStartEvent.level_script_count = ((component != null && component.RunningLevelScriptNames != null) ? component.RunningLevelScriptNames.Count() : 0);
			myWorldStartEvent.state_machine_count = ((component != null && component.SMManager != null && component.SMManager.MachineDefinitions != null) ? component.SMManager.MachineDefinitions.Count : 0);
=======
			myWorldStartEvent.level_script_count = ((component != null && component.RunningLevelScriptNames != null) ? Enumerable.Count<string>((IEnumerable<string>)component.RunningLevelScriptNames) : 0);
			myWorldStartEvent.state_machine_count = ((component != null && component.SMManager != null && component.SMManager.MachineDefinitions != null) ? component.SMManager.MachineDefinitions.Count : 0);
			m_lastCampaignProgressionTime = 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myWorldStartEvent.voxel_support = settings.StationVoxelSupport;
			myWorldStartEvent.destructible_blocks = settings.DestructibleBlocks;
			myWorldStartEvent.destructible_voxels = settings.EnableVoxelDestruction;
			myWorldStartEvent.jetpack = settings.EnableJetpack;
			myWorldStartEvent.hostility = settings.EnvironmentHostility.ToString();
			myWorldStartEvent.drones = settings.EnableDrones;
			myWorldStartEvent.wolfs = settings.EnableWolfs;
			myWorldStartEvent.spiders = settings.EnableSpiders;
			myWorldStartEvent.encounters = settings.EnableEncounters;
			myWorldStartEvent.oxygen = settings.EnableOxygen;
			myWorldStartEvent.pressurization = settings.EnableOxygenPressurization;
			myWorldStartEvent.realistic_sounds = settings.RealisticSound;
			myWorldStartEvent.tool_shake = settings.EnableToolShake;
			myWorldStartEvent.multiplier_inventory = settings.InventorySizeMultiplier;
			myWorldStartEvent.multiplier_welding_speed = settings.WelderSpeedMultiplier;
			myWorldStartEvent.multiplier_grinding_speed = settings.GrinderSpeedMultiplier;
			myWorldStartEvent.multiplier_refinery_speed = settings.RefinerySpeedMultiplier;
			myWorldStartEvent.multiplier_assembler_speed = settings.AssemblerSpeedMultiplier;
			myWorldStartEvent.multiplier_assembler_efficiency = settings.AssemblerEfficiencyMultiplier;
			myWorldStartEvent.max_floating_objects = settings.MaxFloatingObjects;
			myWorldStartEvent.weather_system = settings.WeatherSystem;
			return myWorldStartEvent;
		}

		private MyWorldEndEvent GatherWorldEndData()
		{
			MyWorldEndEvent myWorldEndEvent = new MyWorldEndEvent(m_defaultWorldData);
			MyFpsManager.PrepareMinMax();
			MySession @static = MySession.Static;
			if (@static != null)
			{
				double totalSeconds = @static.ElapsedPlayTime.TotalSeconds;
				myWorldEndEvent.world_session_duration = (uint)totalSeconds;
				myWorldEndEvent.entire_world_duration = (uint)@static.ElapsedGameTime.TotalSeconds;
				myWorldEndEvent.fps_average = ((totalSeconds == 0.0) ? null : new uint?((uint)((double)MyFpsManager.GetSessionTotalFrames() / totalSeconds)));
				myWorldEndEvent.fps_minimum = (uint)MyFpsManager.GetMinSessionFPS();
				myWorldEndEvent.fps_maximum = (uint)MyFpsManager.GetMaxSessionFPS();
				myWorldEndEvent.ups_average = (uint)((double)MyGameStats.Static.UpdateCount / totalSeconds);
				myWorldEndEvent.simspeed_client_average = ((totalSeconds == 0.0) ? null : new double?((double)@static.SessionSimSpeedPlayer / totalSeconds));
				myWorldEndEvent.simspeed_server_average = ((totalSeconds == 0.0) ? null : new double?((double)@static.SessionSimSpeedServer / totalSeconds));
			}
			int[] percentileValues = MyGeneralStats.Static.PercentileValues;
			IEnumerable<(string Name, double[] Value, bool Bytes)> aggregatedStats = MyGeneralStats.Static.AggregatedStats;
			myWorldEndEvent.NetworkStats = new Dictionary<string, double>();
			foreach (var item in aggregatedStats)
			{
				for (int i = 0; i < percentileValues.Length; i++)
				{
					double num = item.Value[i];
					if (item.Bytes)
					{
						num *= 0.0009765625;
					}
					myWorldEndEvent.NetworkStats[$"{item.Name}.{percentileValues[i]}"] = num;
				}
			}
			MyMultiplayerClient myMultiplayerClient;
			if ((myMultiplayerClient = MyMultiplayerMinimalBase.Instance as MyMultiplayerClient) != null && !string.IsNullOrEmpty(myMultiplayerClient.DisconnectReason))
			{
				myWorldEndEvent.world_exit_reason = myMultiplayerClient.DisconnectReason;
			}
			if (MyCampaignManager.Static != null)
			{
				_ = MyCampaignManager.Static.ActiveCampaign;
			}
			int num2 = 0;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.Toolbar != null)
			{
				for (int j = 0; j < localCharacter.Toolbar.ItemCount; j++)
				{
					if (localCharacter.Toolbar.GetItemAtIndex(j) != null)
					{
						num2++;
					}
				}
			}
			myWorldEndEvent.toolbar_used_slots = (uint)num2;
			myWorldEndEvent.toolbar_page_switches = MySession.Static.ToolbarPageSwitches;
			myWorldEndEvent.total_blocks_created = MySession.Static.TotalBlocksCreated;
			myWorldEndEvent.total_damage_dealt = MySession.Static.TotalDamageDealt;
			myWorldEndEvent.time_grinding_blocks = (uint)MySession.Static.TimeGrindingBlocks.TotalSeconds;
			myWorldEndEvent.time_grinding_friendly_blocks = (uint)MySession.Static.TimeGrindingFriendlyBlocks.TotalSeconds;
			myWorldEndEvent.time_grinding_neutral_blocks = (uint)MySession.Static.TimeGrindingNeutralBlocks.TotalSeconds;
			myWorldEndEvent.time_grinding_enemy_blocks = (uint)MySession.Static.TimeGrindingEnemyBlocks.TotalSeconds;
			myWorldEndEvent.time_piloting_big_ships = (uint)MySession.Static.TimePilotingBigShip.TotalSeconds;
			myWorldEndEvent.time_piloting_small_ships = (uint)MySession.Static.TimePilotingSmallShip.TotalSeconds;
			myWorldEndEvent.time_on_foot_all = (uint)MySession.Static.TimeOnFoot.TotalSeconds;
			myWorldEndEvent.time_using_jetpack = (uint)MySession.Static.TimeOnJetpack.TotalSeconds;
			myWorldEndEvent.time_on_foot_stations = (uint)MySession.Static.TimeOnStation.TotalSeconds;
			myWorldEndEvent.time_on_foot_ships = (uint)MySession.Static.TimeOnShips.TotalSeconds;
			myWorldEndEvent.time_on_foot_asteroids = (uint)MySession.Static.TimeOnAsteroids.TotalSeconds;
			myWorldEndEvent.time_on_foot_planets = (uint)MySession.Static.TimeOnPlanets.TotalSeconds;
			myWorldEndEvent.time_sprinting = (uint)MySession.Static.TimeSprinting.TotalSeconds;
			myWorldEndEvent.time_using_input_mouse = (uint)MySession.Static.TimeUsingMouseInput.TotalSeconds;
			myWorldEndEvent.time_using_input_gamepad = (uint)MySession.Static.TimeUsingGamepadInput.TotalSeconds;
			myWorldEndEvent.time_camera_grid_first_person = (uint)MySession.Static.TimeInCameraGridFirstPerson.TotalSeconds;
			myWorldEndEvent.time_camera_grid_third_person = (uint)MySession.Static.TimeInCameraGridThirdPerson.TotalSeconds;
			myWorldEndEvent.time_camera_character_first_person = (uint)MySession.Static.TimeInCameraCharFirstPerson.TotalSeconds;
			myWorldEndEvent.time_camera_character_third_person = (uint)MySession.Static.TimeInCameraCharThirdPerson.TotalSeconds;
			myWorldEndEvent.time_camera_tool_first_person = (uint)MySession.Static.TimeInCameraToolFirstPerson.TotalSeconds;
			myWorldEndEvent.time_camera_tool_third_person = (uint)MySession.Static.TimeInCameraToolThirdPerson.TotalSeconds;
			myWorldEndEvent.time_camera_weapon_first_person = (uint)MySession.Static.TimeInCameraWeaponFirstPerson.TotalSeconds;
			myWorldEndEvent.time_camera_weapon_third_person = (uint)MySession.Static.TimeInCameraWeaponThirdPerson.TotalSeconds;
			myWorldEndEvent.time_camera_building_first_person = (uint)MySession.Static.TimeInCameraBuildingFirstPerson.TotalSeconds;
			myWorldEndEvent.time_camera_building_third_person = (uint)MySession.Static.TimeInCameraBuildingThirdPerson.TotalSeconds;
			myWorldEndEvent.TotalAmountMined = new Dictionary<string, int>();
			foreach (KeyValuePair<string, MyFixedPoint> item2 in MySession.Static.AmountMined)
			{
				myWorldEndEvent.TotalAmountMined[item2.Key] = (int)item2.Value.RawValue;
			}
			myWorldEndEvent.time_in_ship_builder_mode = (uint)MySession.Static.TimeInBuilderMode.TotalSeconds;
			myWorldEndEvent.time_creative_tools_enabled = (uint)MySession.Static.TimeCreativeToolsEnabled.TotalSeconds;
			myWorldEndEvent.total_blocks_created_from_ship = MySession.Static.TotalBlocksCreatedFromShips;
			myWorldEndEvent.server_most_concurrent_players = (uint)Math.Max(1, m_serverMostConcurrentPlayers);
			return myWorldEndEvent;
		}

		private bool TryEndSession(string sessionEndReason, out MySessionEndEvent sessionEndEvent, out string sessionId)
		{
			if (m_isSessionEnded)
			{
				sessionEndEvent = null;
				sessionId = null;
				return false;
			}
			sessionEndEvent = new MySessionEndEvent(m_defaultSessionData)
			{
				game_quit_reason = sessionEndReason,
				session_duration = MySandboxGame.TotalTimeInMilliseconds / 1000
			};
			sessionId = m_sessionID;
			m_isSessionEnded = true;
			m_isSessionStarted = false;
			PauseHeartbeat();
			return true;
		}

		private string HashUserId(ulong userId)
		{
			Random random = new Random((int)userId);
			byte[] array = new byte[16];
			random.NextBytes(array);
			return new Guid(array).ToString();
		}

		private static string GetModList()
		{
			string text = string.Empty;
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in MySession.Static.Mods)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
<<<<<<< HEAD
				text = (string.IsNullOrEmpty(mod.FriendlyName) ? (string.IsNullOrEmpty(mod.Name) ? (text + mod.PublishedFileId) : (text + mod.Name.Replace(",", ""))) : (text + mod.FriendlyName.Replace(",", "")));
=======
				text += mod.FriendlyName.Replace(",", "");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return text;
		}

		private void PauseHeartbeat()
		{
			m_isHeartbeatEnabled = false;
		}

		private void StartHeartbeat()
		{
			m_isHeartbeatEnabled = true;
		}

		private bool TrySendHeartbeat()
		{
			if (!m_isHeartbeatEnabled || m_heartbeatTracker == null)
			{
				return false;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (m_lastHeartbeatSent + m_heartbeatPeriod > utcNow)
			{
				return false;
			}
			m_heartbeatEvent = m_heartbeatEvent ?? GatherHeartbeatData();
			m_heartbeatTracker.ReportEvent(m_heartbeatEvent, DateTime.UtcNow, m_sessionID, m_hashedUserID, m_clientVersion, m_platform);
			m_lastHeartbeatSent = utcNow;
			return true;
		}

		private MyHeartbeatEvent GatherHeartbeatData()
		{
			return new MyHeartbeatEvent
			{
				Region_ISO2 = m_defaultSessionData.region_iso2,
				Region_ISO3 = m_defaultSessionData.region_iso3
			};
		}

<<<<<<< HEAD
		[Event(null, 723)]
=======
		[Event(null, 724)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void NotifyAnalyticsIds(string userId, string sessionId)
		{
			MyLog.Default.WriteLine($"Analytics ids for user ({MyEventContext.Current.Sender}): User = ({userId}) Session = ({sessionId}).");
		}
	}
}
