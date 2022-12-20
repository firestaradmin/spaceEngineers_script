using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.ExceptionServices;
using EmptyKeys.UserInterface;
using ParallelTasks;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using SpaceEngineers.Game;
using SpaceEngineers.Game.Achievements;
using SpaceEngineers.Game.GUI;
using VRage;
using VRage.EOS;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Mod.Io;
using VRage.Platform.Windows;
using VRage.Steam;
using VRage.UserInterface;
using VRage.Utils;
using VRageRender;

namespace SpaceEngineers
{
	internal static class MyProgram
	{
		private static MyCommonProgramStartup m_startup;

		private static IMyRender m_renderer;

		private static uint AppId = 244850u;

		private static void Main(string[] args)
		{
			Exception ex = null;
			try
			{
				InitTexts();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			SpaceEngineersGame.SetupBasicGameInfo();
			m_startup = new MyCommonProgramStartup(args);
			string appDataPath = m_startup.GetAppDataPath();
			MyVRageWindows.Init(MyPerGameSettings.BasicGameInfo.ApplicationName, MySandboxGame.Log, appDataPath, detectLeaks: false, performInit: false);
			InitGameSettings_PC();
			InitGameSettings_Common();
			if (m_startup.PerformReporting(out var crashInfo))
			{
				return;
			}
			if (crashInfo.IsExperimental && crashInfo.IsOutOfMemory)
			{
				MySandboxGame.ExperimentalOutOfMemoryCrash = true;
			}
			if (!MyVRage.Platform.System.IsSingleInstance)
			{
				MyErrorReporter.ReportAppAlreadyRunning(MyPerGameSettings.BasicGameInfo.GameName);
				return;
			}
			MyInitializer.InvokeBeforeRun(AppId, MyPerGameSettings.BasicGameInfo.ApplicationName, MyVRage.Platform.System.GetAppDataPath(), addDateToLog: true, 3, OnConfigChanged);
			if (ex != null)
			{
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			MyVRage.Platform.Init();
			MyInitializer.InitCheckSum();
			m_startup.PerformColdStart();
			m_startup.PerformAutoconnect();
			m_startup.InitSplashScreen();
			if (!m_startup.Check64Bit())
			{
				return;
			}
			MyPlatformGameSettings.VERBOSE_NETWORK_LOGGING |= m_startup.VerboseNetworkLogging();
			IMyGameService myGameService = MySteamGameService.Create(Sandbox.Engine.Platform.Game.IsDedicated, AppId);
			MyServiceManager.Instance.AddService(myGameService);
			MyServerDiscoveryAggregator myServerDiscoveryAggregator = new MyServerDiscoveryAggregator();
			MySteamGameService.InitNetworking(isDedicated: false, myGameService, MyPerGameSettings.GameName, myServerDiscoveryAggregator);
			MyEOSService.InitNetworking(isDedicated: false, MyPerGameSettings.GameName, myGameService, "xyza7891964JhtVD93nm3nZp8t1MbnhC", "AKGM16qoFtct0IIIA8RCqEIYG4d4gXPPDNpzGuvlhLA", "24b1cd652a18461fa9b3d533ac8d6b5b", "1958fe26c66d4151a327ec162e4d49c8", "07c169b3b641401496d352cad1c905d6", "https://retail.epicgames.com/", MyEOSService.CreatePlatform(), MyPlatformGameSettings.VERBOSE_NETWORK_LOGGING, MyNetworking.CollectNetworkParameters(args), myServerDiscoveryAggregator, MyMultiplayer.Channels);
			MyServiceManager.Instance.AddService((IMyServerDiscovery)myServerDiscoveryAggregator);
			IMyUGCService ugc = MySteamUgcService.Create(AppId, myGameService);
			MyGameService.WorkshopService.AddAggregate(ugc);
			IMyMicrophoneService serviceInstance = MySteamGameService.CreateMicrophone();
			MyServiceManager.Instance.AddService(serviceInstance);
			IMyUGCService myUGCService = MyModIoService.Create(MyServiceManager.Instance.GetService<IMyGameService>(), "spaceengineers", "264", "1fb4489996a5e8ffc6ec1135f9985b5b", "331", "f2b64abe55452252b030c48adc0c1f0e", MyPlatformGameSettings.UGC_TEST_ENVIRONMENT, Sync.IsDedicated);
			myUGCService.IsConsentGiven = MySandboxGame.Config.ModIoConsent;
			MyGameService.WorkshopService.AddAggregate(myUGCService);
			MySpaceEngineersAchievements.Initialize();
			MySandboxGame.IsRenderUpdateSyncEnabled = m_startup.IsRenderUpdateSyncEnabled();
			MySandboxGame.IsVideoRecordingEnabled = m_startup.IsVideoRecordingEnabled();
			MySandboxGame.PerformNotInteractiveReport = m_startup.PerformNotInteractiveReport;
			SpaceEngineersGame.SetupPerGameSettings();
			SpaceEngineersGame.SetupAnalytics();
			MySandboxGame.InitMultithreading();
			MyVRage.Platform.System.OnThreadpoolInitialized();
			if (m_startup.IsGenerateDx11MipCache())
			{
				MyDX11Render.GenerateDx11MipCache();
				Environment.Exit(0);
				return;
			}
			try
			{
				InitializeRender();
			}
			catch (MyRenderException ex3)
			{
				MyVRage.Platform.Windows.MessageBox(ex3.Message, "Error", MessageBoxOptions.OkOnly);
				return;
			}
			if (!m_startup.CheckSteamRunning())
			{
				return;
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyFileSystem.InitUserSpecific(MyGameService.UserId.ToString());
			}
			ActivateABTests();
			string path = Path.Combine(MyFileSystem.UserDataPath, "Promo");
			if (Directory.Exists(path))
			{
				IEnumerable<string> files = MyFileSystem.GetFiles(path);
				foreach (string item in files)
				{
					File.Delete(item);
				}
			}
			SpaceEngineersGame spaceEngineersGame = new SpaceEngineersGame(args);
			try
			{
				spaceEngineersGame.Run(customRenderLoop: false, m_startup.DisposeSplashScreen);
			}
			finally
			{
				spaceEngineersGame.Dispose();
			}
			MyGameService.ShutDown();
			MyInitializer.InvokeAfterRun();
			MyVRage.Done();
		}

		private static void ActivateABTests()
		{
			if (MyNewGameScreenABTestHelper.Instance.IsActive() && !MyNewGameScreenABTestHelper.Instance.IsApplied())
			{
				MyNewGameScreenABTestHelper.Instance.ApplyTest();
			}
		}

		private static void InitTexts()
		{
			MyLanguage.ObtainCurrentOSCulture();
			string rootDirectory = Path.Combine(MyFileSystem.RootPath, "Content\\Data\\Localization\\CoreTexts");
			HashSet<MyLanguagesEnum> outSupportedLanguages = new HashSet<MyLanguagesEnum>();
			MyTexts.LoadSupportedLanguages(rootDirectory, outSupportedLanguages);
			MyLanguagesEnum osLanguageCurrentOfficial = MyLanguage.GetOsLanguageCurrentOfficial();
			MyTexts.Languages.TryGetValue(osLanguageCurrentOfficial, out var value);
			if (value == null)
			{
				MyTexts.Languages.TryGetValue(MyLanguagesEnum.English, out value);
				if (value == null)
				{
					return;
				}
			}
			MyTexts.LoadTexts(rootDirectory, value.CultureName, value.SubcultureName);
		}

		private static void InitializeRender()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_renderer = new MyNullRender();
			}
			else
			{
				MyRenderPresetEnum renderQualityHint = MyVRage.Platform.Render.GetRenderQualityHint();
				MyPerformanceSettings defaults = MyGuiScreenOptionsGraphics.GetPreset(renderQualityHint);
				bool force = renderQualityHint > MyRenderPresetEnum.CUSTOM;
				MyRenderProxy.Settings.User = MyVideoSettingsManager.GetGraphicsSettingsFromConfig(ref defaults, force).PerformanceSettings.RenderSettings;
				MyRenderProxy.Settings.EnableAnsel = MyPlatformGameSettings.ENABLE_ANSEL;
				MyRenderProxy.Settings.EnableAnselWithSprites = MyPlatformGameSettings.ENABLE_ANSEL_WITH_SPRITES;
				MyStringId graphicsRenderer = MySandboxGame.Config.GraphicsRenderer;
				if (graphicsRenderer == MySandboxGame.DirectX11RendererKey)
				{
					m_renderer = new MyDX11Render(MyRenderProxy.Settings);
					if (!m_renderer.IsSupported)
					{
						MySandboxGame.Log.WriteLine("DirectX 11 renderer not supported. No renderer to revert back to.");
						m_renderer = null;
					}
				}
				if (m_renderer == null)
				{
					throw new MyRenderException("The current version of the game requires a Dx11 card. \\n For more information please see : http://blog.marekrosa.org/2016/02/space-engineers-news-full-source-code_26.html", MyRenderExceptionEnum.GpuNotSupported);
				}
				MySandboxGame.Config.GraphicsRenderer = graphicsRenderer;
			}
			Engine engine = new MyEngine();
			MyRenderProxy.Initialize(m_renderer);
		}

		private static void InitGameSettings_Common()
		{
			if (MyVRage.Platform.System.SimulationQuality == SimulationQuality.Normal)
			{
				MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = false;
			}
		}

		private static void InitGameSettings_PC()
		{
		}

		private static void InitGameSettings_XBox()
		{
			MyPlatformGameSettings.FEEDBACK_ON_EXIT = false;
			MyPlatformGameSettings.LIMITED_MAIN_MENU = true;
			MyPlatformGameSettings.CONSOLE_COMPATIBLE = true;
			MyPlatformGameSettings.DYNAMIC_REPLICATION_RADIUS = true;
			MyInput.EnableModifierKeyEmulation = true;
			MyParallelEntityUpdateOrchestrator.WorkerPriority = WorkPriority.Normal;
			MyPlatformGameSettings.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR = 5;
			MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION = false;
			MyPerGameSettings.BasicGameInfo.GameAcronym = "SEX";
			MyPlatformGameSettings.IsIgnorePcuAllowed = false;
			MyPlatformGameSettings.IsMultilineEditableByGamepad = true;
			MyPlatformGameSettings.SUPPORT_COMMUNITY_TRANSLATIONS = false;
			MyPlatformGameSettings.ENABLE_LOGOS = true;
			MyPlatformGameSettings.ENABLE_LOGOS_ASAP = true;
			MyPlatformGameSettings.PREFER_ONLINE = true;
			MyPlatformGameSettings.CLOUD_ALWAYS_ENABLED = true;
			MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE = false;
			MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT = true;
			MyPlatformGameSettings.GAME_SAVES_TO_CLOUD = true;
			MyPlatformGameSettings.GAME_CONFIG_TO_CLOUD = true;
			MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD = true;
			MyPlatformGameSettings.ENABLE_NEWGAME_SCREEN_ABTEST = false;
			MyPlatformGameSettings.WORKSHOP_BROWSER_ITEMS_PER_PAGE = 9u;
			MyPlatformGameSettings.ITEM_TOOLTIP_SCALE = 0.8f;
			MyPlatformGameSettings.CONTROLLER_DEFAULT_ON_START = true;
			MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING = true;
			MyPlatformGameSettings.VOICE_CHAT_3D_SOUND = false;
			MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN = true;
			MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION = true;
			MyPerGameSettings.JoinScreenBannerTexture = "Textures\\GUI\\nitrado.png";
			MyPerGameSettings.JoinScreenBannerTextureHighlight = "Textures\\GUI\\nitrado_highlight.png";
			MyPerGameSettings.JoinScreenBannerURL = "ms-windows-store://pdp/?productid=9p2cpmvcw0lx";
			MyPlatformGameSettings.VoxelTrashRemovalSettings value = default(MyPlatformGameSettings.VoxelTrashRemovalSettings);
			value.Age = 24;
			value.RevertAsteroids = true;
			value.MinDistanceFromGrid = 3000;
			value.MinDistanceFromPlayer = 1000;
			value.RevertCloseToNPCGrids = true;
			MyPlatformGameSettings.FORCED_VOXEL_TRASH_REMOVAL_SETTINGS = value;
			MyPlatformGameSettings.ADDITIONAL_BLOCK_LIMITS = ImmutableArray.Create(("Warhead", (short)25));
			MyPerGameSettings.ChangeLogUrl = "https://mirror.keenswh.com/news/SpaceEngineersChangelogXboxEnglish.xml";
		}

		private static void OnConfigChanged()
		{
			int num = 100000;
			int num2 = 600000;
			int maxSafePlayers_Remote = 16;
			bool experimentalMode = MySandboxGame.Config.ExperimentalMode;
			int num3 = 8;
			MyPlatformGameSettings.LOBBY_MAX_PLAYERS = (experimentalMode ? 16 : num3);
			MyObjectBuilder_SessionSettings.MaxSafePlayers = num3;
			MyObjectBuilder_SessionSettings.MaxSafePCU = num;
			MyObjectBuilder_SessionSettings.MaxSafePlayers_Remote = maxSafePlayers_Remote;
			MyObjectBuilder_SessionSettings.MaxSafePCU_Remote = num2;
			if (experimentalMode)
			{
				MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX = MyVRage.Platform.System.GetExperimentalPCULimit(num);
				MyPlatformGameSettings.SERVER_TOTAL_PCU_MAX = null;
				MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX = MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
			}
			else
			{
				MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX = num;
				MyPlatformGameSettings.SERVER_TOTAL_PCU_MAX = num2;
				MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX = MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
			}
			MyFakes.VOICE_CHAT_MIC_SENSITIVITY = MySandboxGame.Config.MicSensitivity;
			MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION = MySandboxGame.Config.AutomaticVoiceChatActivation;
		}
	}
}
