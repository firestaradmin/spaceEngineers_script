using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using EmptyKeys.UserInterface;
using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Planet;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
<<<<<<< HEAD
using Sandbox.Game.Entities.Interfaces;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Factions.Definitions;
using VRage.Game.GUI;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.SessionComponents;
using VRage.Game.Voxels;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Render.Particles;
using VRage.Scripting;
using VRage.Serialization;
using VRage.UserInterface;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
	/// <summary>
	/// Base class for all session types (single, coop, mmo, sandbox)
	/// </summary>
	[StaticEventOwner]
	public sealed class MySession : IMyNetObject, IMyEventOwner, IMySession
	{
		private class ComponentComparer : IComparer<MySessionComponentBase>
		{
			public int Compare(MySessionComponentBase x, MySessionComponentBase y)
			{
				int priority = x.Priority;
				int num = priority.CompareTo(y.Priority);
				if (num == 0)
				{
					return string.Compare(x.GetType().FullName, y.GetType().FullName, StringComparison.Ordinal);
				}
				return num;
			}
		}

		public enum MyHitIndicatorTarget
		{
			Character,
			Headshot,
			Kill,
			Grid,
			Other,
			Friend,
			None
		}

		private class GatherVoxelMaterialsWork : IPrioritizedWork, IWork
		{
			private List<MyVoxelBase> m_voxelMaps;

			private HashSet<string> m_target;

			private BoundingSphereD m_sphere;

			private Action m_completion;

			private int m_index;

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public WorkOptions Options
			{
				get
				{
					WorkOptions result = default(WorkOptions);
					result.MaximumThreads = m_voxelMaps.Count;
					result.TaskType = MyProfiler.TaskType.Voxels;
					return result;
				}
			}

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public WorkPriority Priority => WorkPriority.VeryLow;

			public GatherVoxelMaterialsWork(List<MyVoxelBase> voxelMaps, HashSet<string> target, BoundingSphereD sphere, Action completion)
			{
				m_voxelMaps = voxelMaps;
				m_target = target;
				m_sphere = sphere;
				m_completion = completion;
				m_index = -1;
			}

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void DoWork(WorkData workData = null)
			{
				int num = Interlocked.Increment(ref m_index);
				if (num < m_voxelMaps.Count)
				{
					MyVoxelBase voxel = m_voxelMaps[num];
					GetVoxelMaterials(m_target, voxel, 7, m_sphere.Center, (float)MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_FAR);
					GetVoxelMaterials(m_target, voxel, 1, m_sphere.Center, (float)MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_CLOSE);
					if (num == m_voxelMaps.Count - 1)
					{
						m_completion?.Invoke();
					}
				}
			}
		}

		public enum LimitResult
		{
			Passed,
			MaxGridSize,
			NoFaction,
			BlockTypeLimit,
			MaxBlocksPerPlayer,
			PCU
		}

		protected sealed class OnCreativeToolsEnabled_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCreativeToolsEnabled(value);
			}
		}

		protected sealed class OnCrash_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCrash();
			}
		}

		protected sealed class HitIndicatorActivationInternal_003C_003ESandbox_Game_World_MySession_003C_003EMyHitIndicatorTarget : ICallSite<IMyEventOwner, MyHitIndicatorTarget, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyHitIndicatorTarget type, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HitIndicatorActivationInternal(type);
			}
		}

		protected sealed class OnPromoteLevelSet_003C_003ESystem_UInt64_0023VRage_Game_ModAPI_MyPromoteLevel : ICallSite<IMyEventOwner, ulong, MyPromoteLevel, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong steamId, in MyPromoteLevel level, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPromoteLevelSet(steamId, level);
			}
		}

		protected sealed class OnServerSaving_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool saveStarted, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnServerSaving(saveStarted);
			}
		}

		protected sealed class OnServerPerformanceWarning_003C_003ESystem_String_0023VRage_MySimpleProfiler_003C_003EProfilingBlockType : ICallSite<IMyEventOwner, string, MySimpleProfiler.ProfilingBlockType, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string key, in MySimpleProfiler.ProfilingBlockType type, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnServerPerformanceWarning(key, type);
			}
		}

		protected sealed class SetSpectatorPositionFromServer_003C_003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetSpectatorPositionFromServer(position);
			}
		}

		protected sealed class CameraControllerSet_003C_003EVRage_Game_MyCameraControllerEnum_0023System_Int64 : ICallSite<IMyEventOwner, MyCameraControllerEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyCameraControllerEnum type, in long entityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CameraControllerSet(type, entityId);
			}
		}

		protected sealed class OnRequestVicinityInformation_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestVicinityInformation(entityId);
			}
		}

		protected sealed class OnVicinityInformation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_String_003E_0023System_Collections_Generic_List_00601_003CSystem_String_003E_0023System_Collections_Generic_List_00601_003CSystem_String_003E : ICallSite<IMyEventOwner, List<string>, List<string>, List<string>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<string> voxels, in List<string> models, in List<string> armorModels, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnVicinityInformation(voxels, models, armorModels);
			}
		}

		private static readonly ComponentComparer SessionComparer;

		private readonly CachingDictionary<Type, MySessionComponentBase> m_sessionComponents = new CachingDictionary<Type, MySessionComponentBase>();

		private readonly Dictionary<int, SortedSet<MySessionComponentBase>> m_sessionComponentsForUpdate = new Dictionary<int, SortedSet<MySessionComponentBase>>();

		private readonly List<MySessionComponentBase> m_sessionComponentForDraw = new List<MySessionComponentBase>();

		private readonly List<MySessionComponentBase> m_sessionComponentForDrawAsync = new List<MySessionComponentBase>();

		private HashSet<string> m_componentsToLoad;

		public HashSet<string> SessionComponentEnabled = new HashSet<string>();

		public HashSet<string> SessionComponentDisabled = new HashSet<string>();

		private const string SAVING_FOLDER = ".new";

		public const int MIN_NAME_LENGTH = 5;

		public const int MAX_NAME_LENGTH = 90;

		public const int MAX_DESCRIPTION_LENGTH = 7999;

		internal MySpectatorCameraController Spectator = new MySpectatorCameraController();

		internal MyTimeSpan m_timeOfSave;

		internal DateTime m_lastTimeMemoryLogged;

		private Dictionary<string, short> EmptyBlockTypeLimitDictionary = new Dictionary<string, short>();

		private static MySession m_static;

		public int RequiresDX = 9;

		private bool m_isSaveInProgress;
<<<<<<< HEAD
=======

		private bool m_isSnapshotSaveInProgress;

		public MyObjectBuilder_SessionSettings Settings;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private bool m_isSnapshotSaveInProgress;

<<<<<<< HEAD
		public MyObjectBuilder_SessionSettings Settings;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool? m_isRunningExperimentalCache;

		public MyScriptManager ScriptManager;

		public List<Tuple<string, MyBlueprintItemInfo>> BattleBlueprints;

		public Dictionary<ulong, MyPromoteLevel> PromotedUsers = new Dictionary<ulong, MyPromoteLevel>();

		public MyScenarioDefinition Scenario;

		public BoundingBoxD? WorldBoundaries;

		public readonly MyVoxelMaps VoxelMaps = new MyVoxelMaps();

		public readonly MyFactionCollection Factions = new MyFactionCollection();

		public MyPlayerCollection Players = new MyPlayerCollection();

		public MyPerPlayerData PerPlayerData = new MyPerPlayerData();

		public readonly MyToolBarCollection Toolbars = new MyToolBarCollection();

		internal MyVirtualClients VirtualClients = new MyVirtualClients();

		internal MyCameraCollection Cameras = new MyCameraCollection();

		public MyGpsCollection Gpss = new MyGpsCollection();

		public MyBlockLimits NPCBlockLimits;

		public MyBlockLimits GlobalBlockLimits;

		public MyBlockLimits SessionBlockLimits;

		public int TotalSessionPCU;

		public bool ServerSaving;

		private AdminSettingsEnum m_adminSettings;

		private Dictionary<ulong, AdminSettingsEnum> m_remoteAdminSettings = new Dictionary<ulong, AdminSettingsEnum>();

		private bool m_streamingInProgress;

		private List<MyUpdateCallback> m_updateCallbacks = new List<MyUpdateCallback>();

		private static bool m_showMotD;

		public Dictionary<string, MyFixedPoint> AmountMined = new Dictionary<string, MyFixedPoint>();

		private bool m_cameraAwaitingEntity;

		private IMyCameraController m_cameraController = MySpectatorCameraController.Static;

		public ulong WorldSizeInBytes;

		private int m_gameplayFrameCounter;

		private const int FRAMES_TO_CONSIDER_READY = 10;

		private int m_framesToReady;

		public HashSet<ulong> CreativeTools = new HashSet<ulong>();

		private bool m_updateAllowed;

		private List<MySessionComponentBase> m_loadOrder = new List<MySessionComponentBase>();

		private static int m_profilerDumpDelay;

		private DateTime m_streamingIndicatorShowTime = DateTime.MaxValue;

		private MyMultiplayerHostResult m_serverRequest;

		private DateTime m_streamingIndicatorShowTime = DateTime.MaxValue;

		private MyMultiplayerHostResult m_serverRequest;

		public const float ADAPTIVE_LOAD_THRESHOLD = 90f;

		private int m_simQualitySwitchFrames;

		private int m_lastQualitySwitchFrame;

		private const int ConsecutiveFramesToShowWarning = 300;
<<<<<<< HEAD

		private MyOxygenProviderSystemHelper m_oxygenHelper = new MyOxygenProviderSystemHelper();

=======

		private MyOxygenProviderSystemHelper m_oxygenHelper = new MyOxygenProviderSystemHelper();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static string GameServiceName
		{
			get
			{
				if (MyGameService.Service == null)
				{
					return "";
				}
				return MyGameService.Service.ServiceName;
			}
		}

		public static MySession Static
		{
			get
			{
				return m_static;
			}
			set
			{
				m_static = value;
				MyVRage.Platform.SessionReady = value != null;
			}
		}

		public DateTime GameDateTime
		{
			get
			{
				return new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc) + ElapsedGameTime;
			}
			set
			{
				ElapsedGameTime = value - new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}
		}

		public TimeSpan ElapsedGameTime { get; set; }
<<<<<<< HEAD

		public DateTime InGameTime { get; set; }

=======

		public DateTime InGameTime { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Name { get; set; }

		public string Description { get; set; }

		public string Password { get; set; }

		public ulong? WorkshopId { get; private set; }

		public string CurrentPath { get; set; }

		public float SessionSimSpeedPlayer { get; private set; }

		public float SessionSimSpeedServer { get; private set; }

		public bool CameraOnCharacter { get; set; }

		public uint AutoSaveInMinutes
		{
			get
			{
				if (MyFakes.ENABLE_AUTOSAVE && Settings != null)
				{
					return Settings.AutoSaveInMinutes;
				}
				return 0u;
			}
		}

		public bool IsAdminMenuEnabled => IsUserModerator(Sync.MyId);

		public bool CreativeMode => Settings.GameMode == MyGameModeEnum.Creative;

		public bool SurvivalMode => Settings.GameMode == MyGameModeEnum.Survival;

		public bool InfiniteAmmo
		{
			get
			{
				if (!Settings.InfiniteAmmo)
				{
					return Settings.GameMode == MyGameModeEnum.Creative;
				}
				return true;
			}
		}

		public bool EnableContainerDrops
		{
			get
			{
				if (Settings.EnableContainerDrops)
				{
					return Settings.GameMode == MyGameModeEnum.Survival;
				}
				return false;
			}
		}

		public int MinDropContainerRespawnTime => Settings.MinDropContainerRespawnTime * 60;

		public int MaxDropContainerRespawnTime => Settings.MaxDropContainerRespawnTime * 60;

		public bool AutoHealing => Settings.AutoHealing;

		public bool ThrusterDamage => Settings.ThrusterDamage;

		public bool WeaponsEnabled => Settings.WeaponsEnabled;

		public bool CargoShipsEnabled => Settings.CargoShipsEnabled;

		public bool DestructibleBlocks => Settings.DestructibleBlocks;

		public bool EnableIngameScripts => Settings.EnableIngameScripts;

		public bool Enable3RdPersonView => Settings.Enable3rdPersonView;

		public bool EnableToolShake => Settings.EnableToolShake;

		public bool ShowPlayerNamesOnHud => Settings.ShowPlayerNamesOnHud;

		public bool EnableConvertToStation => Settings.EnableConvertToStation;

		public short MaxPlayers => Settings.MaxPlayers;

		public short MaxFloatingObjects => Settings.MaxFloatingObjects;

		public short MaxBackupSaves => Settings.MaxBackupSaves;

		public int MaxGridSize => Settings.MaxGridSize;

		public int MaxBlocksPerPlayer => Settings.MaxBlocksPerPlayer;

		public Dictionary<string, short> BlockTypeLimits
		{
			get
			{
				if (Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
				{
					return EmptyBlockTypeLimitDictionary;
				}
				return Settings.BlockTypeLimits.Dictionary;
			}
		}

		public bool EnableRemoteBlockRemoval => Settings.EnableRemoteBlockRemoval;

		public float InventoryMultiplier => Settings.InventorySizeMultiplier;

		public float CharactersInventoryMultiplier => Settings.InventorySizeMultiplier;

		public float BlocksInventorySizeMultiplier => Settings.BlocksInventorySizeMultiplier;

		public bool SimplifiedSimulation => MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE ?? Settings.SimplifiedSimulation;

		public float RefinerySpeedMultiplier => Settings.RefinerySpeedMultiplier;

		public float AssemblerSpeedMultiplier => Settings.AssemblerSpeedMultiplier;

		public float AssemblerEfficiencyMultiplier => Settings.AssemblerEfficiencyMultiplier;

		public float WelderSpeedMultiplier => Settings.WelderSpeedMultiplier;

		public float GrinderSpeedMultiplier => Settings.GrinderSpeedMultiplier;

		public float HackSpeedMultiplier => Settings.HackSpeedMultiplier;

		public MyOnlineModeEnum OnlineMode => Settings.OnlineMode;

		public MyEnvironmentHostilityEnum EnvironmentHostility => Settings.EnvironmentHostility;

		public bool StartInRespawnScreen => Settings.StartInRespawnScreen;

		public bool EnableVoxelDestruction => Settings.EnableVoxelDestruction;

		public MyBlockLimitsEnabledEnum BlockLimitsEnabled => Settings.BlockLimitsEnabled;

		public int TotalPCU => Settings.TotalPCU;

		public int PiratePCU => Settings.PiratePCU;

		public int MaxFactionsCount
		{
			get
			{
				if (BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					return Settings.MaxFactionsCount;
				}
				return Math.Max(1, Settings.MaxFactionsCount);
			}
		}

		public bool ResearchEnabled
		{
			get
			{
				if (Settings.EnableResearch)
				{
					return !CreativeMode;
				}
				return false;
			}
		}

		public string CustomLoadingScreenImage { get; set; }

		public string CustomLoadingScreenText { get; set; }

		public string CustomSkybox { get; set; }

		public ulong SharedToolbar { get; set; }

		public bool IsRunningExperimental
		{
			get
			{
				if (!m_isRunningExperimentalCache.HasValue)
				{
					if (Sync.IsServer)
					{
						m_isRunningExperimentalCache = MySandboxGame.Config.ExperimentalMode;
					}
					else
					{
						m_isRunningExperimentalCache = MyMultiplayer.Static.IsServerExperimental;
					}
				}
				return m_isRunningExperimentalCache.Value;
			}
		}

		public bool EnableSpiders => Settings.EnableSpiders;

		public bool EnableWolfs => Settings.EnableWolfs;

		public int TotalBotLimit => Settings.TotalBotLimit;

		public bool EnableScripterRole => Settings.EnableScripterRole;

		public bool IsScenario => Settings.Scenario;

		public bool LoadedAsMission { get; private set; }

		public bool PersistentEditMode { get; private set; }

		public List<MyObjectBuilder_Checkpoint.ModItem> Mods { get; set; }

		BoundingBoxD IMySession.WorldBoundaries
		{
			get
			{
				if (!WorldBoundaries.HasValue)
				{
					return BoundingBoxD.CreateInvalid();
				}
				return WorldBoundaries.Value;
			}
		}

		public MySyncLayer SyncLayer { get; private set; }

		public MyChatSystem ChatSystem => GetComponent<MyChatSystem>();

		public static bool ShowMotD
		{
			get
			{
				return m_showMotD;
			}
			set
			{
				m_showMotD = value;
			}
		}

		public TimeSpan ElapsedPlayTime { get; private set; }

		public TimeSpan TimeOnFoot { get; private set; }

		public TimeSpan TimeSprinting { get; private set; }

		public TimeSpan TimeOnJetpack { get; private set; }

		public TimeSpan TimePilotingSmallShip { get; private set; }

		public TimeSpan TimePilotingBigShip { get; private set; }

		public TimeSpan TimeOnStation { get; private set; }

		public TimeSpan TimeOnShips { get; private set; }

		public TimeSpan TimeOnAsteroids { get; private set; }

		public TimeSpan TimeOnPlanets { get; private set; }

		public TimeSpan TimeInBuilderMode { get; private set; }

		public TimeSpan TimeCreativeToolsEnabled { get; private set; }

		public TimeSpan TimeUsingMouseInput { get; private set; }

		public TimeSpan TimeUsingGamepadInput { get; private set; }

		public TimeSpan TimeInCameraGridFirstPerson { get; private set; }

		public TimeSpan TimeInCameraGridThirdPerson { get; private set; }

		public TimeSpan TimeInCameraCharFirstPerson { get; private set; }

		public TimeSpan TimeInCameraCharThirdPerson { get; private set; }

		public TimeSpan TimeInCameraToolFirstPerson { get; private set; }

		public TimeSpan TimeInCameraToolThirdPerson { get; private set; }

		public TimeSpan TimeInCameraWeaponFirstPerson { get; private set; }

		public TimeSpan TimeInCameraWeaponThirdPerson { get; private set; }

		public TimeSpan TimeInCameraBuildingFirstPerson { get; private set; }

		public TimeSpan TimeInCameraBuildingThirdPerson { get; private set; }

		public TimeSpan TimeGrindingBlocks { get; private set; }

		public TimeSpan TimeGrindingFriendlyBlocks { get; private set; }

		public TimeSpan TimeGrindingNeutralBlocks { get; private set; }

		public TimeSpan TimeGrindingEnemyBlocks { get; private set; }

		public float PositiveIntegrityTotal { get; set; }

		public float NegativeIntegrityTotal { get; set; }

		public ulong VoxelHandVolumeChanged { get; set; }

		public uint TotalDamageDealt { get; set; }

		public uint TotalBlocksCreated { get; set; }

		public uint TotalBlocksCreatedFromShips { get; set; }

		public uint ToolbarPageSwitches { get; set; }

		public IMyWeatherEffects WeatherEffects => Static.GetComponent<MySectorWeatherComponent>();

		public MyPlayer LocalHumanPlayer => Sync.Clients?.LocalClient?.FirstPlayer;

		IMyPlayer IMySession.LocalHumanPlayer => LocalHumanPlayer;

		public MyEntity TopMostControlledEntity
		{
			get
			{
				MyEntity myEntity = ControlledEntity?.Entity;
				MyEntity myEntity2 = myEntity?.GetTopMostParent();
				if (myEntity2 == null || Sync.Players.GetControllingPlayer(myEntity) != Sync.Players.GetControllingPlayer(myEntity2))
				{
					return myEntity;
				}
				return myEntity2;
			}
		}

		public Sandbox.Game.Entities.IMyControllableEntity ControlledEntity => LocalHumanPlayer?.Controller.ControlledEntity;

<<<<<<< HEAD
		public MyCubeGrid ControlledGrid
		{
			get
			{
				Sandbox.Game.Entities.IMyControllableEntity controlledEntity = ControlledEntity;
				if (controlledEntity == null)
				{
					return null;
				}
				MyCubeBlock myCubeBlock;
				if ((myCubeBlock = controlledEntity as MyCubeBlock) != null)
				{
					return myCubeBlock.CubeGrid;
				}
				return null;
			}
		}

		public MyCharacter LocalCharacter => LocalHumanPlayer?.Character;

		public IEnumerable<MyCharacter> SavedCharacters => LocalHumanPlayer?.SavedCharacters;

=======
		public MyCharacter LocalCharacter => LocalHumanPlayer?.Character;

		public IEnumerable<MyCharacter> SavedCharacters => LocalHumanPlayer?.SavedCharacters;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public long LocalCharacterEntityId => LocalCharacter?.EntityId ?? 0;

		public long LocalPlayerId => LocalHumanPlayer?.Identity.IdentityId ?? 0;

		public bool IsCameraAwaitingEntity
		{
			get
			{
				return m_cameraAwaitingEntity;
			}
			set
			{
				m_cameraAwaitingEntity = value;
			}
		}

		public IMyCameraController CameraController
		{
			get
			{
				return m_cameraController;
			}
			set
			{
				if (m_cameraController == value)
				{
					return;
				}
				IMyCameraController cameraController = m_cameraController;
				m_cameraController = value;
				if (Static == null)
				{
					return;
				}
				if (this.CameraAttachedToChanged != null)
				{
					this.CameraAttachedToChanged(cameraController, m_cameraController);
				}
				if (cameraController != null)
				{
					cameraController.OnReleaseControl(m_cameraController);
					if (cameraController.Entity != null)
					{
						cameraController.Entity.OnClosing -= OnCameraEntityClosing;
					}
				}
				m_cameraController.OnAssumeControl(cameraController);
				if (m_cameraController.Entity != null)
				{
					m_cameraController.Entity.OnClosing += OnCameraEntityClosing;
				}
				m_cameraController.ForceFirstPersonCamera = false;
			}
		}

		public bool IsValid => true;

		public int GameplayFrameCounter => m_gameplayFrameCounter;

		public bool Ready { get; private set; }

		public bool IsUnloading { get; private set; }

		public MyEnvironmentHostilityEnum? PreviousEnvironmentHostility { get; set; }

		/// <summary>
		/// Checks if the local player has access to creative tools.
		/// </summary>
		public bool HasCreativeRights => HasPlayerCreativeRights(Sync.MyId);

		public bool IsCopyPastingEnabled
		{
			get
			{
				if (!CreativeToolsEnabled(Sync.MyId) || !HasCreativeRights)
				{
					if (CreativeMode)
					{
						return Settings.EnableCopyPaste;
					}
					return false;
				}
				return true;
			}
		}

		public MyGameFocusManager GameFocusManager { get; private set; }

		public AdminSettingsEnum AdminSettings
		{
			get
			{
				return m_adminSettings;
			}
			set
			{
				m_adminSettings = value;
			}
		}

		public Dictionary<ulong, AdminSettingsEnum> RemoteAdminSettings
		{
			get
			{
				return m_remoteAdminSettings;
			}
			set
			{
				m_remoteAdminSettings = value;
			}
		}

		public bool StreamingInProgress
		{
			get
			{
				return m_streamingInProgress;
			}
			set
			{
				if (m_streamingInProgress != value)
				{
					m_streamingInProgress = value;
					if (m_streamingInProgress)
					{
						MyHud.PushRotatingWheelVisible();
						MyHud.RotatingWheelText = MyTexts.Get(MySpaceTexts.LoadingWheel_Streaming);
					}
					else
					{
						MyHud.PopRotatingWheelVisible();
						MyHud.RotatingWheelText = MyHud.Empty;
					}
				}
			}
		}

		public bool IsSaveInProgress
		{
			get
			{
				if (!m_isSaveInProgress)
				{
					return m_isSnapshotSaveInProgress;
				}
				return true;
			}
		}

		public bool IsServer
		{
			get
			{
				if (!Sync.IsServer)
				{
					return MyMultiplayer.Static == null;
				}
				return true;
			}
		}

		public MyGameDefinition GameDefinition { get; set; }

		public int AppVersionFromSave { get; private set; }

		public string ThumbPath => Path.Combine(CurrentPath, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);

		public bool MultiplayerAlive { get; set; }

		public bool MultiplayerDirect { get; set; }

		public double MultiplayerLastMsg { get; set; }

		public MyTimeSpan MultiplayerPing { get; set; }
<<<<<<< HEAD

		public bool HighSimulationQuality { get; private set; } = true;

=======

		public bool HighSimulationQuality { get; private set; } = true;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public bool HighSimulationQualityNotification
		{
			get
			{
				if (Settings.AdaptiveSimulationQuality && (!Sync.IsServer || !HighSimulationQuality) && GameplayFrameCounter - m_lastQualitySwitchFrame >= 300)
				{
					if (!Sync.IsServer)
					{
						return !(Sync.ServerCPULoadSmooth > 90f);
					}
					return false;
				}
				return true;
			}
		}

		IMyVoxelMaps IMySession.VoxelMaps => VoxelMaps;

		IMyCameraController IMySession.CameraController => CameraController;

		float IMySession.AssemblerEfficiencyMultiplier => AssemblerEfficiencyMultiplier;

		float IMySession.AssemblerSpeedMultiplier => AssemblerSpeedMultiplier;

		bool IMySession.AutoHealing => AutoHealing;

		uint IMySession.AutoSaveInMinutes => AutoSaveInMinutes;

		bool IMySession.CargoShipsEnabled => CargoShipsEnabled;

		bool IMySession.ClientCanSave => false;

		bool IMySession.CreativeMode => CreativeMode;

		string IMySession.CurrentPath => CurrentPath;

		string IMySession.Description
		{
			get
			{
				return Description;
			}
			set
			{
				Description = value;
			}
		}

		TimeSpan IMySession.ElapsedPlayTime => ElapsedPlayTime;

		bool IMySession.EnableCopyPaste => IsCopyPastingEnabled;

		MyEnvironmentHostilityEnum IMySession.EnvironmentHostility => EnvironmentHostility;

		DateTime IMySession.GameDateTime
		{
			get
			{
				return GameDateTime;
			}
			set
			{
				GameDateTime = value;
			}
		}

		float IMySession.GrinderSpeedMultiplier => GrinderSpeedMultiplier;

		float IMySession.HackSpeedMultiplier => HackSpeedMultiplier;

		float IMySession.InventoryMultiplier => InventoryMultiplier;

		float IMySession.CharactersInventoryMultiplier => CharactersInventoryMultiplier;

		float IMySession.BlocksInventorySizeMultiplier => BlocksInventorySizeMultiplier;

		bool IMySession.IsCameraAwaitingEntity
		{
			get
			{
				return IsCameraAwaitingEntity;
			}
			set
			{
				IsCameraAwaitingEntity = value;
			}
		}

		bool IMySession.IsCameraControlledObject => IsCameraControlledObject();

		bool IMySession.IsCameraUserControlledSpectator => IsCameraUserControlledSpectator();

		short IMySession.MaxFloatingObjects => MaxFloatingObjects;

		short IMySession.MaxBackupSaves => MaxBackupSaves;

		short IMySession.MaxPlayers => MaxPlayers;

		bool IMySession.MultiplayerAlive
		{
			get
			{
				return MultiplayerAlive;
			}
			set
			{
				MultiplayerAlive = value;
			}
		}

		bool IMySession.MultiplayerDirect
		{
			get
			{
				return MultiplayerDirect;
			}
			set
			{
				MultiplayerDirect = value;
			}
		}

		double IMySession.MultiplayerLastMsg
		{
			get
			{
				return MultiplayerLastMsg;
			}
			set
			{
				MultiplayerLastMsg = value;
			}
		}

		string IMySession.Name
		{
			get
			{
				return Name;
			}
			set
			{
				Name = value;
			}
		}

		float IMySession.NegativeIntegrityTotal
		{
			get
			{
				return NegativeIntegrityTotal;
			}
			set
			{
				NegativeIntegrityTotal = value;
			}
		}

		MyOnlineModeEnum IMySession.OnlineMode => OnlineMode;

		string IMySession.Password
		{
			get
			{
				return Password;
			}
			set
			{
				Password = value;
			}
		}

		float IMySession.PositiveIntegrityTotal
		{
			get
			{
				return PositiveIntegrityTotal;
			}
			set
			{
				PositiveIntegrityTotal = value;
			}
		}

		float IMySession.RefinerySpeedMultiplier => RefinerySpeedMultiplier;

		bool IMySession.ShowPlayerNamesOnHud => ShowPlayerNamesOnHud;

		bool IMySession.SurvivalMode => SurvivalMode;

		bool IMySession.ThrusterDamage => ThrusterDamage;

		string IMySession.ThumbPath => ThumbPath;

		TimeSpan IMySession.TimeOnBigShip => TimePilotingBigShip;

		TimeSpan IMySession.TimeOnFoot => TimeOnFoot;

		TimeSpan IMySession.TimeOnJetpack => TimeOnJetpack;

		TimeSpan IMySession.TimeOnSmallShip => TimePilotingSmallShip;

		bool IMySession.WeaponsEnabled => WeaponsEnabled;

		float IMySession.WelderSpeedMultiplier => WelderSpeedMultiplier;

		ulong? IMySession.WorkshopId => WorkshopId;

		IMyPlayer IMySession.Player => LocalHumanPlayer;

		VRage.Game.ModAPI.Interfaces.IMyControllableEntity IMySession.ControlledObject => ControlledEntity;

		MyObjectBuilder_SessionSettings IMySession.SessionSettings => Settings;

		IMyFactionCollection IMySession.Factions => Factions;

		IMyCamera IMySession.Camera => MySector.MainCamera;

		double IMySession.CameraTargetDistance
		{
			get
			{
				return GetCameraTargetDistance();
			}
			set
			{
				SetCameraTargetDistance(value);
			}
		}

		public IMyConfig Config => MySandboxGame.Config;

		IMyDamageSystem IMySession.DamageSystem => MyDamageSystem.Static;

		IMyGpsCollection IMySession.GPS => Static.Gpss;

		[Obsolete("Use HasCreativeRights")]
		bool IMySession.HasAdminPrivileges => HasCreativeRights;

		MyPromoteLevel IMySession.PromoteLevel => GetUserPromoteLevel(Sync.MyId);

		bool IMySession.HasCreativeRights => HasCreativeRights;

		Version IMySession.Version => MyFinalBuildConstants.APP_VERSION;

		IMyOxygenProviderSystem IMySession.OxygenProviderSystem => m_oxygenHelper;

		int IMySession.TotalBotLimit => TotalBotLimit;

		public event Action<ulong, MyPromoteLevel> OnUserPromoteLevelChanged;

		public event Action OnLocalPlayerSkinOrColorChanged;

		public event Action<IMyCameraController, IMyCameraController> CameraAttachedToChanged;

		/// <summary>
		/// Called after session is created, but before it's loaded.
		/// MySession.Static.Statis is valid when raising OnLoading.
		/// </summary>
		public static event Action OnLoading;

		public static event Action OnUnloading;

		public static event Action AfterLoading;

		public static event Action BeforeLoading;

		public static event Action OnUnloaded;

		public event Action OnReady;

		public event Action<MyObjectBuilder_Checkpoint> OnSavingCheckpoint;

		event Action IMySession.OnSessionReady
		{
			add
			{
				Static.OnReady += value;
			}
			remove
			{
				Static.OnReady -= value;
			}
		}

		event Action IMySession.OnSessionLoading
		{
			add
			{
				OnLoading += value;
			}
			remove
			{
				OnLoading -= value;
			}
		}

		private void PrepareBaseSession(List<MyObjectBuilder_Checkpoint.ModItem> mods, MyScenarioDefinition definition = null)
		{
			MyGeneralStats.Static.LoadData();
			ScriptManager.Init(null);
			MyDefinitionManager.Static.LoadData(mods);
			LoadGameDefinition(definition?.GameDefinition ?? MyGameDefinition.Default);
			Scenario = definition;
			if (definition != null)
			{
				WorldBoundaries = definition.WorldBoundaries;
				MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(definition.Environment);
			}
			MySector.InitEnvironmentSettings();
			MyModAPIHelper.Initialize();
			LoadDataComponents();
			InitDataComponents();
			MyModAPIHelper.Initialize();
		}

		private void PrepareBaseSession(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
			MyGeneralStats.Static.LoadData();
			if (MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
			{
				MyVRage.Platform.Scripting.CompileIngameScriptAsync("Dummy", string.Empty, out var _, string.Empty, "Program", typeof(MyGridProgram).Name);
			}
			MyGuiTextures.Static.Unload();
			ScriptManager.Init(checkpoint.ScriptManagerData);
			MyDefinitionManager.Static.LoadData(checkpoint.Mods);
			PreloadModels(sector);
			MyLocalCache.PreloadLocalInventoryConfig();
			if (MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				PreloadVicinityCache(checkpoint.VicinityVoxelCache, checkpoint.VicinityModelsCache, checkpoint.VicinityArmorModelsCache);
				MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()?.DrawLoading();
			}
			VirtualClients.Init();
			LoadGameDefinition(checkpoint);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyGuiManager.InitFonts();
			}
			MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>(checkpoint.Scenario, out Scenario);
			WorldBoundaries = checkpoint.WorldBoundaries;
			if (!WorldBoundaries.HasValue && Scenario != null)
			{
				WorldBoundaries = Scenario.WorldBoundaries;
			}
			MySector.InitEnvironmentSettings(sector.Environment);
			if (MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				string text = ((!string.IsNullOrEmpty(CustomSkybox)) ? CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture);
				MyRenderProxy.PreloadTextures(new string[1] { text }, TextureType.CubeMap);
			}
			MyModAPIHelper.Initialize();
			LoadDataComponents();
			LoadObjectBuildersComponents(checkpoint.SessionComponents);
			MyModAPIHelper.Initialize();
			if (Sync.IsDedicated && MySessionComponentAnimationSystem.Static != null)
			{
				MySessionComponentAnimationSystem.Static.SetUpdateOrder(MyUpdateOrder.NoUpdate);
			}
		}

		private void RegisterComponentsFromAssemblies()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName[] array = Enumerable.ToArray<AssemblyName>(Enumerable.Select<IGrouping<string, AssemblyName>, AssemblyName>(Enumerable.GroupBy<AssemblyName, string>((IEnumerable<AssemblyName>)executingAssembly.GetReferencedAssemblies(), (Func<AssemblyName, string>)((AssemblyName x) => x.Name)), (Func<IGrouping<string, AssemblyName>, AssemblyName>)((IGrouping<string, AssemblyName> y) => Enumerable.First<AssemblyName>((IEnumerable<AssemblyName>)y))));
			m_componentsToLoad = new HashSet<string>();
			m_componentsToLoad.UnionWith((IEnumerable<string>)GameDefinition.SessionComponents.Keys);
			m_componentsToLoad.RemoveWhere((Predicate<string>)((string x) => SessionComponentDisabled.Contains(x)));
			m_componentsToLoad.UnionWith((IEnumerable<string>)SessionComponentEnabled);
			AssemblyName[] array2 = array;
			foreach (AssemblyName assemblyName in array2)
			{
				try
				{
					if (!assemblyName.Name.Contains("Sandbox") && !assemblyName.Name.Equals("VRage.Game"))
<<<<<<< HEAD
					{
						continue;
					}
					Assembly assembly = Assembly.Load(assemblyName);
					object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
					if (customAttributes.Length != 0)
					{
=======
					{
						continue;
					}
					Assembly assembly = Assembly.Load(assemblyName);
					object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
					if (customAttributes.Length != 0)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						AssemblyProductAttribute assemblyProductAttribute = customAttributes[0] as AssemblyProductAttribute;
						if (assemblyProductAttribute.Product == "Sandbox" || assemblyProductAttribute.Product == "VRage.Game")
						{
							RegisterComponentsFromAssembly(assembly);
						}
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("Error while resolving session components assemblies");
					MyLog.Default.WriteLine(ex.ToString());
				}
			}
			try
			{
				foreach (KeyValuePair<MyModContext, HashSet<MyStringId>> item in ScriptManager.ScriptsPerMod)
				{
					MyStringId key = Enumerable.First<MyStringId>((IEnumerable<MyStringId>)item.Value);
					RegisterComponentsFromAssembly(ScriptManager.Scripts[key], modAssembly: true, item.Key);
				}
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLine("Error while loading modded session components");
				MyLog.Default.WriteLine(ex2.ToString());
			}
			try
			{
				foreach (IPlugin plugin in MyPlugins.Plugins)
				{
					RegisterComponentsFromAssembly(plugin.GetType().Assembly, modAssembly: true);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				RegisterComponentsFromAssembly(MyPlugins.GameAssembly);
			}
			catch (Exception ex4)
			{
				MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
				MyLog.Default.WriteLine(ex4.ToString());
			}
			try
			{
				RegisterComponentsFromAssembly(MyPlugins.UserAssemblies);
			}
			catch (Exception ex5)
			{
				MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
				MyLog.Default.WriteLine(ex5.ToString());
			}
			RegisterComponentsFromAssembly(executingAssembly);
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				if (value.ModContext == null || value.ModContext.IsBaseGame)
				{
					m_sessionComponentForDrawAsync.Add(value);
				}
				else
				{
					m_sessionComponentForDraw.Add(value);
				}
			}
		}

		public T GetComponent<T>() where T : MySessionComponentBase
		{
			m_sessionComponents.TryGetValue(typeof(T), out var value);
			return value as T;
		}

		public void RegisterComponent(MySessionComponentBase component, MyUpdateOrder updateOrder, int priority)
		{
			m_sessionComponents[component.ComponentType] = component;
			component.Session = this;
			AddComponentForUpdate(updateOrder, component);
			m_sessionComponents.ApplyChanges();
		}

		public void UnregisterComponent(MySessionComponentBase component)
		{
			component.Session = null;
			m_sessionComponents.Remove(component.ComponentType);
		}

		public void RegisterComponentsFromAssembly(Assembly[] assemblies, bool modAssembly = false, MyModContext context = null)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					RegisterComponentsFromAssembly(assembly, modAssembly, context);
				}
			}
		}

		public void RegisterComponentsFromAssembly(Assembly assembly, bool modAssembly = false, MyModContext context = null)
		{
			if (assembly == null)
			{
				return;
			}
			MySandboxGame.Log.WriteLine("Registered modules from: " + assembly.FullName);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (Attribute.IsDefined(type, typeof(MySessionComponentDescriptor)))
				{
					TryRegisterSessionComponent(type, modAssembly, context);
				}
			}
		}

		private void TryRegisterSessionComponent(Type type, bool modAssembly, MyModContext context)
		{
			try
			{
				MyDefinitionId? definition = null;
				MySessionComponentBase mySessionComponentBase = (MySessionComponentBase)Activator.CreateInstance(type);
				if (mySessionComponentBase.IsRequiredByGame || modAssembly || GetComponentInfo(type, out definition))
				{
					RegisterComponent(mySessionComponentBase, mySessionComponentBase.UpdateOrder, mySessionComponentBase.Priority);
					GetComponentInfo(type, out definition);
					mySessionComponentBase.Definition = definition;
					mySessionComponentBase.ModContext = context;
				}
			}
			catch (Exception)
			{
				MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
			}
		}

		private bool GetComponentInfo(Type type, out MyDefinitionId? definition)
		{
			string text = null;
			if (m_componentsToLoad.Contains(type.Name))
			{
				text = type.Name;
			}
			else if (m_componentsToLoad.Contains(type.FullName))
			{
				text = type.FullName;
			}
			if (text != null)
			{
				GameDefinition.SessionComponents.TryGetValue(text, out definition);
				return true;
			}
			definition = null;
			return false;
		}

		public void AddComponentForUpdate(MyUpdateOrder updateOrder, MySessionComponentBase component)
		{
			for (int i = 0; i <= 2; i++)
			{
				if (((uint)updateOrder & (uint)(1 << i)) != 0)
				{
					SortedSet<MySessionComponentBase> value = null;
					if (!m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
					{
						m_sessionComponentsForUpdate.Add(1 << i, value = new SortedSet<MySessionComponentBase>((IComparer<MySessionComponentBase>)SessionComparer));
					}
					value.Add(component);
				}
			}
		}

		public void SetComponentUpdateOrder(MySessionComponentBase component, MyUpdateOrder order)
		{
			for (int i = 0; i <= 2; i++)
			{
				SortedSet<MySessionComponentBase> value = null;
				if (((uint)order & (uint)(1 << i)) != 0)
				{
					if (!m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
					{
						value = new SortedSet<MySessionComponentBase>();
						m_sessionComponentsForUpdate.Add(i, value);
					}
					value.Add(component);
				}
				else if (m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
				{
					value.Remove(component);
				}
			}
		}

		public void LoadObjectBuildersComponents(List<MyObjectBuilder_SessionComponent> objectBuilderData)
		{
			foreach (MyObjectBuilder_SessionComponent objectBuilderDatum in objectBuilderData)
			{
				Type key;
				if ((key = MySessionComponentMapping.TryGetMappedSessionComponentType(objectBuilderDatum.GetType())) != null && m_sessionComponents.TryGetValue(key, out var value))
				{
					value.Init(objectBuilderDatum);
				}
			}
			InitDataComponents();
		}

		private void InitDataComponents()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				if (!value.Initialized)
				{
					MyObjectBuilder_SessionComponent sessionComponent = null;
					if (value.ObjectBuilderType != MyObjectBuilderType.Invalid)
					{
						sessionComponent = (MyObjectBuilder_SessionComponent)Activator.CreateInstance(value.ObjectBuilderType);
					}
					value.Init(sessionComponent);
				}
			}
		}

		public void LoadDataComponents()
		{
			MyTimeOfDayHelper.Reset();
			RaiseOnLoading();
			Sync.Clients.SetLocalSteamId(Sync.MyId, !(MyMultiplayer.Static is MyMultiplayerClient), MyGameService.UserName);
			Sync.Players.RegisterEvents();
			SetAsNotReady();
			HashSet<MySessionComponentBase> val = new HashSet<MySessionComponentBase>();
			do
			{
				m_sessionComponents.ApplyChanges();
				foreach (MySessionComponentBase value in m_sessionComponents.Values)
				{
					if (!val.Contains(value))
					{
						LoadComponent(value);
						val.Add(value);
					}
				}
			}
			while (m_sessionComponents.HasChanges());
		}

		private void LoadComponent(MySessionComponentBase component)
		{
			if (component.Loaded)
			{
				return;
			}
			Type[] dependencies = component.Dependencies;
			foreach (Type key in dependencies)
			{
				m_sessionComponents.TryGetValue(key, out var value);
				if (value != null)
				{
					LoadComponent(value);
				}
			}
			if (!m_loadOrder.Contains(component))
			{
				m_loadOrder.Add(component);
				component.LoadData();
				component.AfterLoadData();
				return;
			}
			string text = $"Circular dependency: {component.DebugName}";
			MySandboxGame.Log.WriteLine(text);
			throw new Exception(text);
		}

		public void UnloadDataComponents(bool beforeLoadWorld = false)
		{
			MySessionComponentBase mySessionComponentBase = null;
			try
			{
				for (int num = m_loadOrder.Count - 1; num >= 0; num--)
				{
					mySessionComponentBase = m_loadOrder[num];
					mySessionComponentBase.UnloadDataConditional();
				}
			}
			catch (Exception innerException)
			{
				if (mySessionComponentBase != null)
				{
					IMyModContext modContext = mySessionComponentBase.ModContext;
					if (mySessionComponentBase?.ModContext != null && !modContext.IsBaseGame)
					{
						throw new ModCrashedException(innerException, modContext);
					}
				}
				throw;
			}
			MySessionComponentMapping.Clear();
			m_sessionComponents.Clear();
			m_loadOrder.Clear();
			foreach (SortedSet<MySessionComponentBase> value in m_sessionComponentsForUpdate.Values)
			{
				value.Clear();
			}
			m_sessionComponentsForUpdate.Clear();
			m_sessionComponentForDraw.Clear();
			m_sessionComponentForDrawAsync.Clear();
			if (!beforeLoadWorld)
			{
				Sync.Players.UnregisterEvents();
				Sync.Clients.Clear();
				MyNetworkReader.Clear();
			}
			Ready = false;
		}

		public void BeforeStartComponents()
		{
			TotalDamageDealt = 0u;
			TotalBlocksCreated = 0u;
			ToolbarPageSwitches = 0u;
			ElapsedPlayTime = default(TimeSpan);
			m_timeOfSave = MySandboxGame.Static.TotalTime;
			MyFpsManager.Reset();
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.BeforeStart();
			}
			if (MySpaceAnalytics.Instance != null)
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(Guid.NewGuid().ToString(), firstTimeRun: true);
				}
				MySpaceAnalytics.Instance.ReportWorldStart(Settings);
			}
		}

		public void UpdateComponents()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			SortedSet<MySessionComponentBase> value = null;
			Enumerator<MySessionComponentBase> enumerator;
			if (m_sessionComponentsForUpdate.TryGetValue(1, out value))
			{
				enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySessionComponentBase current = enumerator.get_Current();
						if (current.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
						{
							current.UpdateBeforeSimulation();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.Simulate();
			}
			if (m_sessionComponentsForUpdate.TryGetValue(2, out value))
			{
				enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySessionComponentBase current2 = enumerator.get_Current();
						if (current2.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
						{
							current2.Simulate();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (!m_sessionComponentsForUpdate.TryGetValue(4, out value))
<<<<<<< HEAD
			{
				return;
			}
			foreach (MySessionComponentBase item3 in value)
			{
				if (item3.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
				{
					item3.UpdateAfterSimulation();
=======
			{
				return;
			}
			enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySessionComponentBase current3 = enumerator.get_Current();
					if (current3.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
					{
						current3.UpdateAfterSimulation();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UpdateComponentsWhilePaused()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			SortedSet<MySessionComponentBase> value = null;
			Enumerator<MySessionComponentBase> enumerator;
			if (m_sessionComponentsForUpdate.TryGetValue(1, out value))
			{
				enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySessionComponentBase current = enumerator.get_Current();
						if (current.UpdateOnPause)
						{
							current.UpdateBeforeSimulation();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (m_sessionComponentsForUpdate.TryGetValue(2, out value))
			{
				enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySessionComponentBase current2 = enumerator.get_Current();
						if (current2.UpdateOnPause)
						{
							current2.Simulate();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (!m_sessionComponentsForUpdate.TryGetValue(4, out value))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MySessionComponentBase item3 in value)
			{
				if (item3.UpdateOnPause)
				{
					item3.UpdateAfterSimulation();
=======
			enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySessionComponentBase current3 = enumerator.get_Current();
					if (current3.UpdateOnPause)
					{
						current3.UpdateAfterSimulation();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public static float GetPlayerDistance(MyEntity entity, ICollection<MyPlayer> players)
		{
			Vector3D translation = entity.WorldMatrix.Translation;
			float num = float.MaxValue;
			foreach (MyPlayer player in players)
			{
				Sandbox.Game.Entities.IMyControllableEntity controlledEntity = player.Controller.ControlledEntity;
				if (controlledEntity != null)
				{
					float num2 = Vector3.DistanceSquared(controlledEntity.Entity.WorldMatrix.Translation, translation);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return (float)Math.Sqrt(num);
		}

		public static float GetOwnerLoginTimeSeconds(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 0)
			{
				return 0f;
			}
			return GetIdentityLoginTimeSeconds(grid.BigOwners[0]);
		}

		public static float GetIdentityLoginTimeSeconds(long identityId)
		{
			MyIdentity myIdentity = Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return 0f;
			}
			return (int)(DateTime.Now - myIdentity.LastLoginTime).TotalSeconds;
		}

		public static float GetOwnerLogoutTimeSeconds(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 0)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 1)
			{
				return GetIdentityLogoutTimeSeconds(grid.BigOwners[0]);
			}
			float num = float.MaxValue;
			foreach (long bigOwner in grid.BigOwners)
			{
				float identityLogoutTimeSeconds = GetIdentityLogoutTimeSeconds(bigOwner);
				if (identityLogoutTimeSeconds < num)
				{
					num = identityLogoutTimeSeconds;
				}
			}
			return num;
		}

		public static float GetIdentityLogoutTimeSeconds(long identityId)
		{
			if (Static.Players.TryGetPlayerId(identityId, out var result) && Static.Players.GetPlayerById(result) != null)
			{
				return 0f;
			}
			MyIdentity myIdentity = Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return 0f;
			}
			if (Static.Players.IdentityIsNpc(identityId))
			{
				return 0f;
			}
			return (int)(DateTime.Now - myIdentity.LastLogoutTime).TotalSeconds;
		}

		public bool IsSettingsExperimental(bool remote = false)
		{
			if (GetSettingsExperimentalReason(remote, null) != (MyObjectBuilder_SessionSettings.ExperimentalReason)0L)
			{
				return !MyCampaignManager.Static.IsCampaignRunning;
			}
			return false;
		}

<<<<<<< HEAD
		public MyObjectBuilder_SessionSettings.ExperimentalReason GetSettingsExperimentalReason(bool remote, MyObjectBuilder_Checkpoint checkpoint)
		{
			MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = Settings.GetExperimentalReason(remote);
			if (MySandboxGame.Config.ExperimentalMode && (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServerExperimental))
=======
		public bool IsSettingsExperimental(bool remote = false)
		{
			if (GetSettingsExperimentalReason(remote, null) != (MyObjectBuilder_SessionSettings.ExperimentalReason)0L)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ExperimentalTurnedOnInConfiguration;
			}
<<<<<<< HEAD
			if (MySandboxGame.InsufficientHardware)
			{
=======
			return false;
		}

		public MyObjectBuilder_SessionSettings.ExperimentalReason GetSettingsExperimentalReason(bool remote, MyObjectBuilder_Checkpoint checkpoint)
		{
			MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = Settings.GetExperimentalReason(remote);
			if (MySandboxGame.Config.ExperimentalMode && (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServerExperimental))
			{
				experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ExperimentalTurnedOnInConfiguration;
			}
			if (MySandboxGame.InsufficientHardware)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.InsufficientHardware;
			}
			if (Mods.Count > 0)
			{
				experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Mods;
			}
			if (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Plugins != null && MySandboxGame.ConfigDedicated.Plugins.Count != 0)
			{
				experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Plugins;
			}
			bool flag = false;
			if (checkpoint != null)
			{
<<<<<<< HEAD
				MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault();
=======
				MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>(Enumerable.OfType<MyObjectBuilder_CampaignSessionComponent>((IEnumerable)checkpoint.SessionComponents));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myObjectBuilder_CampaignSessionComponent != null)
				{
					flag |= MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaign(myObjectBuilder_CampaignSessionComponent);
				}
			}
			if (flag)
			{
				experimentalReason = (MyObjectBuilder_SessionSettings.ExperimentalReason)0L;
			}
			return experimentalReason;
		}

		public MyPromoteLevel GetUserPromoteLevel(ulong steamId)
		{
			if (Static.OnlineMode == MyOnlineModeEnum.OFFLINE)
			{
				return MyPromoteLevel.Owner;
			}
			if (Static.OnlineMode != 0 && steamId == Sync.ServerId)
			{
				return MyPromoteLevel.Owner;
			}
			Static.PromotedUsers.TryGetValue(steamId, out var value);
			return value;
		}

		public bool IsUserScripter(ulong steamId)
		{
			if (!EnableScripterRole)
			{
				return true;
			}
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Scripter;
		}

		public bool IsUserModerator(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Moderator;
		}

		public bool IsUserSpaceMaster(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.SpaceMaster;
		}

		/// <summary>
		/// Checks if a given player is an admin.
		/// </summary>
		/// <param name="steamId"></param>
		/// <returns></returns>
		public bool IsUserAdmin(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Admin;
		}

		public bool IsUserOwner(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Owner;
		}

		public bool CreativeToolsEnabled(ulong user)
		{
			if (CreativeTools.Contains(user))
			{
				return HasPlayerCreativeRights(user);
			}
			return false;
		}

		public void EnableCreativeTools(ulong user, bool value)
		{
			if (value && HasCreativeRights)
			{
				CreativeTools.Add(user);
			}
			else
			{
				CreativeTools.Remove(user);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnCreativeToolsEnabled, value);
		}

<<<<<<< HEAD
		[Event(null, 873)]
=======
		[Event(null, 823)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnCreativeToolsEnabled(bool value)
		{
			ulong value2 = MyEventContext.Current.Sender.Value;
			if (value && Static.HasPlayerCreativeRights(value2))
			{
				Static.CreativeTools.Add(value2);
			}
			else
			{
				Static.CreativeTools.Remove(value2);
			}
		}

<<<<<<< HEAD
		[Event(null, 887)]
=======
		[Event(null, 837)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void OnCrash()
		{
		}

		public bool IsCopyPastingEnabledForUser(ulong user)
		{
			if (!CreativeToolsEnabled(user) || !HasPlayerCreativeRights(user))
			{
				if (CreativeMode)
				{
					return Settings.EnableCopyPaste;
				}
				return false;
			}
			return true;
		}

		public static void HitIndicatorActivation(MyHitIndicatorTarget type, ulong shooter)
		{
			if (shooter != 0L && (Sync.MyId == shooter || Sync.Clients.HasClient(shooter)))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HitIndicatorActivationInternal, type, new EndpointId(shooter));
			}
		}

<<<<<<< HEAD
		[Event(null, 986)]
=======
		[Event(null, 930)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		public static void HitIndicatorActivationInternal(MyHitIndicatorTarget type)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenHudSpace>()?.ActivateHitIndicator(type);
		}

		public bool SetUserPromoteLevel(ulong steamId, MyPromoteLevel level)
		{
			if (level < MyPromoteLevel.None || level > MyPromoteLevel.Admin)
			{
				throw new ArgumentOutOfRangeException("level", level, null);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnPromoteLevelSet, steamId, level);
			return true;
		}

<<<<<<< HEAD
		[Event(null, 1008)]
=======
		[Event(null, 948)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void OnPromoteLevelSet(ulong steamId, MyPromoteLevel level)
		{
			if (level == MyPromoteLevel.None)
			{
				Static.PromotedUsers.Remove(steamId);
			}
			else
			{
				Static.PromotedUsers[steamId] = level;
			}
			if (Static.RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				if (!Static.IsUserAdmin(steamId))
				{
					Static.RemoteAdminSettings[steamId] = value & ~AdminSettingsEnum.AdminOnly;
					if (steamId == Sync.MyId)
					{
						Static.AdminSettings = Static.RemoteAdminSettings[steamId];
					}
				}
				else if (!Static.IsUserModerator(steamId))
				{
					Static.RemoteAdminSettings[steamId] = AdminSettingsEnum.None;
					if (steamId == Sync.MyId)
					{
						Static.AdminSettings = Static.RemoteAdminSettings[steamId];
					}
				}
			}
			if (Static.OnUserPromoteLevelChanged != null)
			{
				Static.OnUserPromoteLevelChanged(steamId, level);
			}
		}

		public bool CanPromoteUser(ulong requester, ulong target)
		{
			MyPromoteLevel userPromoteLevel = GetUserPromoteLevel(requester);
			MyPromoteLevel userPromoteLevel2 = GetUserPromoteLevel(target);
			if (userPromoteLevel2 < MyPromoteLevel.Admin)
			{
				if (userPromoteLevel >= userPromoteLevel2)
				{
					return userPromoteLevel >= MyPromoteLevel.Admin;
				}
				return false;
			}
			return false;
		}

		public bool CanDemoteUser(ulong requester, ulong target)
		{
			MyPromoteLevel userPromoteLevel = GetUserPromoteLevel(requester);
			MyPromoteLevel userPromoteLevel2 = GetUserPromoteLevel(target);
			if (userPromoteLevel2 > MyPromoteLevel.None && userPromoteLevel2 < MyPromoteLevel.Owner)
			{
				if (userPromoteLevel >= userPromoteLevel2)
				{
					return userPromoteLevel >= MyPromoteLevel.Admin;
				}
				return false;
			}
			return false;
		}

		public void SetAsNotReady()
		{
			m_framesToReady = 10;
			Ready = false;
		}

		public bool HasPlayerCreativeRights(ulong steamId)
		{
			if (MyMultiplayer.Static != null && !IsUserSpaceMaster(steamId))
			{
				return CreativeMode;
			}
			return true;
		}

		public bool HasPlayerSpectatorRights(ulong steamId)
		{
			if (!CreativeMode && !Settings.EnableSpectator && !IsUserAdmin(steamId))
			{
				if (IsUserModerator(steamId))
				{
					return CreativeToolsEnabled(steamId);
				}
				return false;
			}
			return true;
		}

		private void RaiseOnLoading()
		{
			MySession.OnLoading?.Invoke();
		}

<<<<<<< HEAD
		[Event(null, 1088)]
=======
		[Event(null, 1016)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnServerSaving(bool saveStarted)
		{
			Static.ServerSaving = saveStarted;
		}

<<<<<<< HEAD
		/// <summary>
		/// Show performance warning from server
		/// </summary>
		[Event(null, 1097)]
=======
		[Event(null, 1025)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnServerPerformanceWarning(string key, MySimpleProfiler.ProfilingBlockType type)
		{
			MySimpleProfiler.ShowServerPerformanceWarning(key, type);
		}

		/// <summary>
		/// Send performance warnings to clients
		/// </summary>
		private void PerformanceWarning(MySimpleProfiler.MySimpleProfilingBlock block)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnServerPerformanceWarning, block.Name, block.Type);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sandbox.Game.World.MySession" /> class.
		/// </summary>
		private MySession(MySyncLayer syncLayer, bool registerComponents = true)
		{
			if (syncLayer == null)
			{
				MyLog.Default.WriteLine("MySession.Static.MySession() - sync layer is null");
			}
			SyncLayer = syncLayer;
			ElapsedGameTime = default(TimeSpan);
			Spectator.Reset();
			MyCubeGrid.ResetInfoGizmos();
			m_timeOfSave = MyTimeSpan.Zero;
			ElapsedGameTime = default(TimeSpan);
			Ready = false;
			MultiplayerLastMsg = 0.0;
			MultiplayerAlive = true;
			MultiplayerDirect = true;
			AppVersionFromSave = MyFinalBuildConstants.APP_VERSION;
			Factions.FactionStateChanged += OnFactionsStateChanged;
			ScriptManager = new MyScriptManager();
			GC.Collect(2, GCCollectionMode.Forced);
			MySandboxGame.Log.WriteLine(string.Format("GC Memory: {0} B", GC.GetTotalMemory(forceFullCollection: false).ToString("##,#")));
			MySandboxGame.Log.WriteLine(string.Format("Process Memory: {0} B", Process.GetCurrentProcess().get_PrivateMemorySize64().ToString("##,#")));
			GameFocusManager = new MyGameFocusManager();
		}

		private MySession()
			: this(Sandbox.Engine.Platform.Game.IsDedicated ? MyMultiplayer.Static.SyncLayer : new MySyncLayer(new MyTransportLayer(2)))
		{
		}

		static MySession()
		{
			SessionComparer = new ComponentComparer();
			m_showMotD = false;
			if (MyAPIGatewayShortcuts.GetMainCamera == null)
			{
				MyAPIGatewayShortcuts.GetMainCamera = GetMainCamera;
			}
			if (MyAPIGatewayShortcuts.GetWorldBoundaries == null)
			{
				MyAPIGatewayShortcuts.GetWorldBoundaries = GetWorldBoundaries;
			}
			if (MyAPIGatewayShortcuts.GetLocalPlayerPosition == null)
			{
				MyAPIGatewayShortcuts.GetLocalPlayerPosition = GetLocalPlayerPosition;
			}
		}

		/// <summary>
		/// Starts multiplayer server with current world
		/// </summary>
		internal void StartServer(MyMultiplayerBase multiplayer)
		{
			multiplayer.WorldName = Name;
			multiplayer.GameMode = Settings.GameMode;
			multiplayer.WorldSize = WorldSizeInBytes;
			multiplayer.AppVersion = MyFinalBuildConstants.APP_VERSION;
			multiplayer.DataHash = MyDataIntegrityChecker.GetHashBase64();
			multiplayer.InventoryMultiplier = Settings.InventorySizeMultiplier;
			multiplayer.BlocksInventoryMultiplier = Settings.BlocksInventorySizeMultiplier;
			multiplayer.AssemblerMultiplier = Settings.AssemblerEfficiencyMultiplier;
			multiplayer.RefineryMultiplier = Settings.RefinerySpeedMultiplier;
			multiplayer.WelderMultiplier = Settings.WelderSpeedMultiplier;
			multiplayer.GrinderMultiplier = Settings.GrinderSpeedMultiplier;
			multiplayer.MemberLimit = Settings.MaxPlayers;
			multiplayer.Mods = Mods;
			multiplayer.ViewDistance = Settings.ViewDistance;
			multiplayer.SyncDistance = Settings.SyncDistance;
			multiplayer.Scenario = IsScenario;
			multiplayer.ExperimentalMode = IsSettingsExperimental();
			MyCachedServerItem.SendSettingsToSteam();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				(multiplayer as MyDedicatedServerBase).SendGameTagsToSteam();
				MySimpleProfiler.ShowPerformanceWarning += PerformanceWarning;
			}
			if (multiplayer is MyMultiplayerLobby)
			{
				((MyMultiplayerLobby)multiplayer).HostSteamId = MyMultiplayer.Static.ServerId;
			}
			Static.Gpss.RegisterChat(multiplayer);
		}

		private void DisconnectMultiplayer()
		{
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.Disconnect();
			}
		}

		private void UnloadMultiplayer()
		{
			if (MyMultiplayer.Static != null)
			{
				Gpss.UnregisterChat(MyMultiplayer.Static);
				MyMultiplayer.Static.Dispose();
				SyncLayer = null;
			}
		}

		private void LoadGameDefinition(MyDefinitionId? gameDef = null)
		{
			if (!gameDef.HasValue)
			{
				gameDef = MyGameDefinition.Default;
			}
			Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>(gameDef.Value);
			if (Static.GameDefinition == null)
			{
				Static.GameDefinition = MyGameDefinition.DefaultDefinition;
			}
			RegisterComponentsFromAssemblies();
		}

		private void LoadGameDefinition(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.GameDefinition.IsNull())
			{
				LoadGameDefinition();
				return;
			}
			Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>(checkpoint.GameDefinition);
			SessionComponentDisabled = checkpoint.SessionComponentDisabled;
			SessionComponentEnabled = checkpoint.SessionComponentEnabled;
			RegisterComponentsFromAssemblies();
			ShowMotD = true;
		}

		private void CheckUpdate()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			bool flag = true;
			if (IsPausable())
			{
				flag = !MySandboxGame.IsPaused && MySandboxGame.Static.IsActive;
			}
			if (m_updateAllowed == flag)
			{
				return;
			}
			m_updateAllowed = flag;
			if (!m_updateAllowed)
			{
				MyLog.Default.WriteLine("Updating stopped.");
				SortedSet<MySessionComponentBase> value = null;
				if (!m_sessionComponentsForUpdate.TryGetValue(4, out value))
<<<<<<< HEAD
				{
					return;
				}
				foreach (MySessionComponentBase item in value)
				{
					item.UpdatingStopped();
=======
				{
					return;
				}
				Enumerator<MySessionComponentBase> enumerator = value.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().UpdatingStopped();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			else
			{
				MyLog.Default.WriteLine("Updating continues.");
			}
		}

		private void CheckSimSpeed()
		{
			if (!Settings.AdaptiveSimulationQuality)
			{
				return;
			}
			bool num = !HighSimulationQuality;
			bool flag = MySandboxGame.Static.CPULoadSmooth > 90f;
			if (num != flag)
			{
				m_simQualitySwitchFrames--;
				if (m_simQualitySwitchFrames <= 0)
				{
					HighSimulationQuality = !flag;
					m_simQualitySwitchFrames = 30;
					m_lastQualitySwitchFrame = GameplayFrameCounter - 30;
				}
			}
			else
			{
				m_simQualitySwitchFrames = Math.Min(m_simQualitySwitchFrames + 1, 30);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Updates resource.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Update(MyTimeSpan updateTime)
		{
			if (m_updateAllowed && MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.UpdateClientStateGroups();
			}
			CheckUpdate();
			CheckSimSpeed();
			CheckProfilerDump();
			if (MySandboxGame.Config.SyncRendering)
			{
				Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(-1.0));
			}
			Parallel.RunCallbacks();
			TimeSpan elapsedTimespan = new TimeSpan(0, 0, 0, 0, 16);
			if (m_updateAllowed || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (MySandboxGame.IsPaused)
				{
					return;
				}
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.ReplicationLayer.UpdateBefore();
				}
				UpdateComponents();
				Gpss.Update();
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.ReplicationLayer.UpdateAfter();
					MyMultiplayer.Static.Tick();
				}
				if ((CameraController == null || !CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
				{
					MyThirdPersonSpectator.Static.Update();
				}
				if (IsServer)
				{
					Players.SendDirtyBlockLimits();
				}
				ElapsedGameTime += (MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan);
				if (m_lastTimeMemoryLogged + TimeSpan.FromSeconds(30.0) < DateTime.UtcNow)
				{
					MyVRage.Platform.System.GetGCMemory(out var allocated, out var used);
					MySandboxGame.Log.WriteLine($"GC Memory: {allocated} / {used} MB");
					m_lastTimeMemoryLogged = DateTime.UtcNow;
				}
				if (AutoSaveInMinutes != 0 && MySandboxGame.IsGameReady && updateTime.TimeSpan - m_timeOfSave.TimeSpan > TimeSpan.FromMinutes(AutoSaveInMinutes))
				{
					MySandboxGame.Log.WriteLine("Autosave initiated");
					MyCharacter localCharacter = LocalCharacter;
					bool flag = (localCharacter != null && !localCharacter.IsDead) || localCharacter == null;
					MySandboxGame.Log.WriteLine("Character state: " + flag);
					flag &= Sync.IsServer;
					MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer);
					flag &= !MyAsyncSaving.InProgress;
					MySandboxGame.Log.WriteLine("MyAsyncSaving.InProgress: " + MyAsyncSaving.InProgress);
					if (flag)
					{
						MySandboxGame.Log.WriteLineAndConsole("Autosave");
						MyAsyncSaving.Start(delegate
						{
							MySector.ResetEyeAdaptation = true;
						});
					}
					m_timeOfSave = updateTime;
				}
				if (MySandboxGame.IsGameReady && m_framesToReady > 0)
				{
					m_framesToReady--;
					if (m_framesToReady == 0)
					{
						Ready = true;
						MyAudio.Static.PlayMusic(new MyMusicTrack
						{
							TransitionCategory = MyStringId.GetOrCompute("Default")
						});
						this.OnReady.InvokeIfNotNull();
						this.OnReady = null;
						MySimpleProfiler.Reset(resetFrameCounter: true);
						if (Sandbox.Engine.Platform.Game.IsDedicated)
						{
							MyGameService.GameServer.SetGameReady(state: true);
<<<<<<< HEAD
							if (!Console.IsInputRedirected && MySandboxGame.IsConsoleVisible)
=======
							if (!Console.get_IsInputRedirected() && MySandboxGame.IsConsoleVisible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								MyLog.Default.WriteLineAndConsole("Game ready... Press Ctrl+C to exit");
							}
							else
							{
								MyLog.Default.WriteLineAndConsole("Game ready... ");
							}
						}
					}
				}
				if (Sync.MultiplayerActive && !Sync.IsServer)
				{
					CheckMultiplayerStatus();
				}
				m_gameplayFrameCounter++;
			}
			else if (MySandboxGame.IsPaused && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				UpdateComponentsWhilePaused();
			}
			UpdateStatistics(ref elapsedTimespan);
			DebugDraw();
			for (int num = m_updateCallbacks.Count - 1; num >= 0; num--)
			{
				m_updateCallbacks[num].Update();
				if (m_updateCallbacks[num].ToBeRemoved)
				{
					m_updateCallbacks.RemoveAtFast(num);
				}
			}
		}

		public void AddUpdateCallback(MyUpdateCallback callback)
		{
			m_updateCallbacks.Add(callback);
		}

		private static void CheckProfilerDump()
		{
			m_profilerDumpDelay--;
			if (m_profilerDumpDelay == 0)
			{
				MyRenderProxy.GetRenderProfiler().Dump();
				VRage.Profiler.MyRenderProfiler.SetLevel(0);
			}
			else if (m_profilerDumpDelay < 0)
			{
				m_profilerDumpDelay = -1;
			}
		}

		private void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				_ = MyDebugDrawSettings.DEBUG_DRAW_CONTROLLED_ENTITIES;
				if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_DataProvider);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_Access);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_FULLCELLS)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.FullCells);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES_SCALED)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodesScaled);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACROLEAVES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroLeaves);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRO_SCALED)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroScaled);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACROLEAVES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroLeaves);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_ENCOUNTERS)
				{
					MyEncounterGenerator.Static.DebugDraw();
				}
			}
		}

		private void CheckMultiplayerStatus()
		{
			MultiplayerAlive = MyMultiplayer.Static.IsConnectionAlive;
			MultiplayerDirect = MyMultiplayer.Static.IsConnectionDirect;
			if (Sync.IsServer)
			{
				MultiplayerLastMsg = 0.0;
				return;
			}
			MultiplayerLastMsg = (DateTime.UtcNow - MyMultiplayer.Static.LastMessageReceived).TotalSeconds;
			MyReplicationClient myReplicationClient = MyMultiplayer.ReplicationLayer as MyReplicationClient;
			if (myReplicationClient == null)
			{
				return;
			}
			MultiplayerPing = myReplicationClient.Ping;
			DateTime now = DateTime.Now;
			if (myReplicationClient.PendingStreamingRelicablesCount > 0)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(0.5);
				DateTime dateTime = now + timeSpan;
				if (m_streamingIndicatorShowTime > dateTime)
				{
					m_streamingIndicatorShowTime = dateTime;
				}
<<<<<<< HEAD
			}
			else
			{
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(1.0);
				if (now < m_streamingIndicatorShowTime || now >= m_streamingIndicatorShowTime + timeSpan2)
				{
					m_streamingIndicatorShowTime = DateTime.MaxValue;
				}
			}
=======
			}
			else
			{
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(1.0);
				if (now < m_streamingIndicatorShowTime || now >= m_streamingIndicatorShowTime + timeSpan2)
				{
					m_streamingIndicatorShowTime = DateTime.MaxValue;
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			StreamingInProgress = m_streamingIndicatorShowTime <= now;
		}

		public bool IsPausable()
		{
			return !Sync.MultiplayerActive;
		}

		private void UpdateStatistics(ref TimeSpan elapsedTimespan)
		{
			ElapsedPlayTime += (MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan);
			SessionSimSpeedPlayer += (float)((double)MyPhysics.SimulationRatio * elapsedTimespan.TotalSeconds);
			SessionSimSpeedServer += (float)((double)Sync.ServerSimulationRatio * elapsedTimespan.TotalSeconds);
			if (LocalHumanPlayer != null && LocalHumanPlayer.Character != null)
			{
				if (ControlledEntity is MyCharacter)
				{
					if (((MyCharacter)ControlledEntity).GetCurrentMovementState() == MyCharacterMovementEnum.Flying)
<<<<<<< HEAD
					{
						TimeOnJetpack += elapsedTimespan;
					}
					else
					{
						TimeOnFoot += elapsedTimespan;
						if (((MyCharacter)ControlledEntity).IsSprinting)
						{
							TimeSprinting += elapsedTimespan;
						}
					}
					MyCharacterSoundComponent soundComp = ((MyCharacter)ControlledEntity).SoundComp;
					if (soundComp != null)
					{
						if (soundComp.StandingOnGrid != null)
						{
							if (soundComp.StandingOnGrid.IsStatic)
							{
								TimeOnStation += elapsedTimespan;
							}
							else
							{
								TimeOnShips += elapsedTimespan;
							}
						}
						if (soundComp.StandingOnVoxel != null)
						{
							if (soundComp.StandingOnVoxel is MyVoxelPhysics && ((MyVoxelPhysics)soundComp.StandingOnVoxel).RootVoxel is MyPlanet)
							{
								TimeOnPlanets += elapsedTimespan;
							}
							else
							{
								TimeOnAsteroids += elapsedTimespan;
							}
						}
					}
					if (((MyCharacter)ControlledEntity).IsInFirstPersonView)
					{
						TimeInCameraCharFirstPerson += elapsedTimespan;
					}
					else
					{
=======
					{
						TimeOnJetpack += elapsedTimespan;
					}
					else
					{
						TimeOnFoot += elapsedTimespan;
						if (((MyCharacter)ControlledEntity).IsSprinting)
						{
							TimeSprinting += elapsedTimespan;
						}
					}
					MyCharacterSoundComponent soundComp = ((MyCharacter)ControlledEntity).SoundComp;
					if (soundComp != null)
					{
						if (soundComp.StandingOnGrid != null)
						{
							if (soundComp.StandingOnGrid.IsStatic)
							{
								TimeOnStation += elapsedTimespan;
							}
							else
							{
								TimeOnShips += elapsedTimespan;
							}
						}
						if (soundComp.StandingOnVoxel != null)
						{
							if (soundComp.StandingOnVoxel is MyVoxelPhysics && ((MyVoxelPhysics)soundComp.StandingOnVoxel).RootVoxel is MyPlanet)
							{
								TimeOnPlanets += elapsedTimespan;
							}
							else
							{
								TimeOnAsteroids += elapsedTimespan;
							}
						}
					}
					if (((MyCharacter)ControlledEntity).IsInFirstPersonView)
					{
						TimeInCameraCharFirstPerson += elapsedTimespan;
					}
					else
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						TimeInCameraCharThirdPerson += elapsedTimespan;
					}
					IMyEntity equippedTool = ((MyCharacter)ControlledEntity).EquippedTool;
					if (equippedTool is IMyHandheldGunObject<MyGunBase>)
					{
						if (((MyCharacter)ControlledEntity).IsInFirstPersonView)
						{
							TimeInCameraWeaponFirstPerson += elapsedTimespan;
						}
						else
						{
							TimeInCameraWeaponThirdPerson += elapsedTimespan;
						}
					}
					else if (equippedTool is MyBlockPlacerBase)
					{
						if (((MyCharacter)ControlledEntity).IsInFirstPersonView)
						{
							TimeInCameraBuildingFirstPerson += elapsedTimespan;
						}
						else
						{
							TimeInCameraBuildingThirdPerson += elapsedTimespan;
						}
					}
					else if (equippedTool is IMyHandheldGunObject<MyToolBase>)
					{
						if (((MyCharacter)ControlledEntity).IsInFirstPersonView)
						{
							TimeInCameraToolFirstPerson += elapsedTimespan;
						}
						else
						{
							TimeInCameraToolThirdPerson += elapsedTimespan;
						}
						if (((IMyHandheldGunObject<MyToolBase>)equippedTool).IsShooting && equippedTool is MyAngleGrinder && ((MyAngleGrinder)equippedTool).HasHitBlock)
						{
							TimeGrindingBlocks += elapsedTimespan;
							MyCubeGrid cubeGrid = ((MyAngleGrinder)equippedTool).GetTargetBlock().CubeGrid;
							long num = ((cubeGrid.BigOwners.Count > 0) ? cubeGrid.BigOwners[0] : ((cubeGrid.SmallOwners.Count > 0) ? cubeGrid.SmallOwners[0] : 0));
							switch ((num != 0L) ? ((MyCharacter)ControlledEntity).GetRelationTo(num) : MyRelationsBetweenPlayerAndBlock.NoOwnership)
							{
							case MyRelationsBetweenPlayerAndBlock.Owner:
							case MyRelationsBetweenPlayerAndBlock.FactionShare:
							case MyRelationsBetweenPlayerAndBlock.Friends:
								TimeGrindingFriendlyBlocks += elapsedTimespan;
								break;
							case MyRelationsBetweenPlayerAndBlock.NoOwnership:
							case MyRelationsBetweenPlayerAndBlock.Neutral:
								TimeGrindingNeutralBlocks += elapsedTimespan;
								break;
							default:
								TimeGrindingEnemyBlocks += elapsedTimespan;
								break;
							}
						}
					}
				}
				else if (ControlledEntity is MyCockpit)
				{
					if (((MyCockpit)ControlledEntity).IsLargeShip())
					{
						TimePilotingBigShip += elapsedTimespan;
					}
					else
					{
						TimePilotingSmallShip += elapsedTimespan;
					}
					if (((MyCockpit)ControlledEntity).BuildingMode)
<<<<<<< HEAD
					{
						TimeInBuilderMode += elapsedTimespan;
					}
					if (((MyCockpit)ControlledEntity).IsInFirstPersonView)
					{
=======
					{
						TimeInBuilderMode += elapsedTimespan;
					}
					if (((MyCockpit)ControlledEntity).IsInFirstPersonView)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						TimeInCameraGridFirstPerson += elapsedTimespan;
					}
					else
					{
						TimeInCameraGridThirdPerson += elapsedTimespan;
					}
				}
			}
			if (CreativeToolsEnabled(Sync.MyId))
			{
				TimeCreativeToolsEnabled += elapsedTimespan;
			}
			if (MyInput.Static.IsJoystickLastUsed)
			{
				TimeUsingGamepadInput += elapsedTimespan;
			}
			else
			{
				TimeUsingMouseInput += elapsedTimespan;
			}
		}

		public void ResetStatistics()
		{
			AmountMined = new Dictionary<string, MyFixedPoint>();
			ElapsedPlayTime = TimeSpan.FromMinutes(0.0);
			TimeOnJetpack = TimeSpan.FromMinutes(0.0);
			TimeOnFoot = TimeSpan.FromMinutes(0.0);
			TimeSprinting = TimeSpan.FromMinutes(0.0);
			TimeOnStation = TimeSpan.FromMinutes(0.0);
			TimeOnShips = TimeSpan.FromMinutes(0.0);
			TimeOnPlanets = TimeSpan.FromMinutes(0.0);
			TimeOnAsteroids = TimeSpan.FromMinutes(0.0);
			TimeInCameraCharFirstPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraCharThirdPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraWeaponFirstPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraWeaponThirdPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraBuildingFirstPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraBuildingThirdPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraToolFirstPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraToolThirdPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraGridFirstPerson = TimeSpan.FromMinutes(0.0);
			TimeInCameraGridThirdPerson = TimeSpan.FromMinutes(0.0);
			TimePilotingBigShip = TimeSpan.FromMinutes(0.0);
			TimePilotingSmallShip = TimeSpan.FromMinutes(0.0);
			TimeInBuilderMode = TimeSpan.FromMinutes(0.0);
			TimeCreativeToolsEnabled = TimeSpan.FromMinutes(0.0);
			TimeUsingGamepadInput = TimeSpan.FromMinutes(0.0);
			TimeUsingMouseInput = TimeSpan.FromMinutes(0.0);
			TimeGrindingBlocks = TimeSpan.FromMinutes(0.0);
			TimeGrindingFriendlyBlocks = TimeSpan.FromMinutes(0.0);
			TimeGrindingNeutralBlocks = TimeSpan.FromMinutes(0.0);
			TimeGrindingEnemyBlocks = TimeSpan.FromMinutes(0.0);
			PositiveIntegrityTotal = 0f;
			NegativeIntegrityTotal = 0f;
			VoxelHandVolumeChanged = 0uL;
			TotalDamageDealt = 0u;
			TotalBlocksCreated = 0u;
			TotalBlocksCreatedFromShips = 0u;
			ToolbarPageSwitches = 0u;
		}

		public void HandleInput()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.HandleInput();
			}
		}

		public void Draw()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.Draw();
			}
		}

		public void DrawAsync()
		{
			foreach (MySessionComponentBase item in m_sessionComponentForDrawAsync)
			{
				item.Draw();
			}
		}

		public void DrawSync()
		{
			foreach (MySessionComponentBase item in m_sessionComponentForDraw)
			{
				item.Draw();
			}
		}

		public static bool IsCompatibleVersion(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint == null)
			{
				return false;
			}
			return checkpoint.AppVersion <= (int)MyFinalBuildConstants.APP_VERSION;
		}

		/// <summary>
		/// Initializes a new single player session and start new game with parameters
		/// </summary>
		public static void Start(string name, string description, string password, MyObjectBuilder_SessionSettings settings, List<MyObjectBuilder_Checkpoint.ModItem> mods, MyWorldGenerator.Args generationArgs)
		{
			MyLog.Default.WriteLineAndConsole("Starting world " + name);
			MyEntityContainerEventExtensions.InitEntityEvents();
			Static = new MySession();
			Static.Name = name;
			Static.Mods = mods;
			Static.Description = description;
			Static.Password = password;
			Static.Settings = settings;
			Static.Scenario = generationArgs.Scenario;
			double num = settings.WorldSizeKm * 500;
			if (num > 0.0)
			{
				Static.WorldBoundaries = new BoundingBoxD(new Vector3D(0.0 - num, 0.0 - num, 0.0 - num), new Vector3D(num, num, num));
			}
			MyVisualScriptLogicProvider.Init();
			Static.InGameTime = generationArgs.Scenario.GameDate;
			Static.RequiresDX = (generationArgs.Scenario.HasPlanets ? 11 : 9);
			if (Static.OnlineMode != 0)
			{
				Static.StartServerRequest();
				if (Static.WaitForServerRequest(out var statusCode))
				{
					Static.ShowLoadingError(lobbyFailed: true, statusCode);
					return;
				}
			}
			Static.IsCameraAwaitingEntity = true;
			string text = MyUtils.StripInvalidChars(name);
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			while (Directory.Exists(Static.CurrentPath))
			{
				Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
			}
			Static.PrepareBaseSession(mods, generationArgs.Scenario);
			MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(generationArgs.Scenario.Environment);
			MyWorldGenerator.GenerateWorld(generationArgs);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyToolBarCollection.RequestCreateToolbar(new MyPlayer.PlayerId(Sync.MyId, 0));
			}
			string scenario = generationArgs.Scenario.DisplayNameText.ToString();
			Static.LogSettings(scenario, generationArgs.AsteroidAmount);
			if (generationArgs.Scenario.SunDirection.IsValid())
			{
				MySector.SunProperties.SunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
				MySector.SunProperties.BaseSunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
			}
			MyPrefabManager.FinishedProcessingGrids.Reset();
			if (MyPrefabManager.PendingGrids > 0)
			{
				MyPrefabManager.FinishedProcessingGrids.WaitOne();
			}
			Parallel.RunCallbacks();
			MyEntities.UpdateOnceBeforeFrame();
			Static.BeforeStartComponents();
			Static.Save();
			Static.SessionSimSpeedPlayer = 0f;
			Static.SessionSimSpeedServer = 0f;
			MySpectatorCameraController.Static.InitLight(isLightOn: false);
		}

		internal static void LoadMultiplayer(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			if (MyFakes.ENABLE_PRELOAD_CHARACTER_ANIMATIONS)
			{
				PreloadAnimations("Models\\Characters\\Animations");
			}
			Static = new MySession(multiplayerSession.SyncLayer);
			Static.Mods = world.Checkpoint.Mods;
			Static.Settings = world.Checkpoint.Settings;
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), contentFolder: false, createIfNotExists: false);
			if (!MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>(world.Checkpoint.Scenario, out Static.Scenario))
			{
				Static.Scenario = Enumerable.FirstOrDefault<MyScenarioDefinition>((IEnumerable<MyScenarioDefinition>)MyDefinitionManager.Static.GetScenarioDefinitions());
			}
			Static.WorldBoundaries = world.Checkpoint.WorldBoundaries;
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			Static.LoadMembersFromWorld(world, multiplayerSession);
			MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
			Static.PrepareBaseSession(world.Checkpoint, world.Sector);
			if (MyFakes.MP_SYNC_CLUSTERTREE)
			{
				MyPhysics.DeserializeClusters(world.Clusters);
			}
			try
<<<<<<< HEAD
			{
				foreach (MyObjectBuilder_Planet planet in world.Planets)
				{
					MyPlanet myPlanet = new MyPlanet();
					MyPlanetStorageProvider myPlanetStorageProvider = new MyPlanetStorageProvider();
					myPlanetStorageProvider.Init(generator: MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(planet.PlanetGenerator)), seed: planet.Seed, radius: planet.Radius, loadTextures: true);
					VRage.Game.Voxels.IMyStorage storage = new MyOctreeStorage(myPlanetStorageProvider, myPlanetStorageProvider.StorageSize);
					myPlanet.Init(planet, storage);
					MyEntities.Add(myPlanet);
				}
			}
			catch (MyPlanetWhitelistException ex)
			{
				MyLog.Default.Error("Error during loading session:" + ex);
				throw new MyLoadingTooManyPlanetsException(ex);
=======
			{
				foreach (MyObjectBuilder_Planet planet in world.Planets)
				{
					MyPlanet myPlanet = new MyPlanet();
					MyPlanetStorageProvider myPlanetStorageProvider = new MyPlanetStorageProvider();
					myPlanetStorageProvider.Init(generator: MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(planet.PlanetGenerator)), seed: planet.Seed, radius: planet.Radius, loadTextures: true);
					VRage.Game.Voxels.IMyStorage storage = new MyOctreeStorage(myPlanetStorageProvider, myPlanetStorageProvider.StorageSize);
					myPlanet.Init(planet, storage);
					MyEntities.Add(myPlanet);
				}
			}
			catch (MyPlanetWhitelistException)
			{
				throw new MyLoadingTooManyPlanetsException();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			_ = world.Checkpoint.ControlledObject;
			world.Checkpoint.ControlledObject = -1L;
			if (multiplayerSession != null)
			{
				Static.Gpss.RegisterChat(multiplayerSession);
			}
			Static.CameraController = MySpectatorCameraController.Static;
			Static.LoadWorld(world.Checkpoint, world.Sector);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			MyGeneralStats.Clear();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CacheGuiTexturePaths();
			}
			Static.BeforeStartComponents();
			MySpaceBindingCreator.CreateBindingDefault();
			MyRenderProxy.IsPlayerSpaceMaster = true;
			if (Static != null)
			{
				MyRenderProxy.IsPlayerSpaceMaster = Static.IsUserSpaceMaster(MyGameService.UserId);
			}
		}

		public static void LoadMission(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, bool persistentEditMode)
		{
			LoadMission(sessionPath, checkpoint, checkpointSizeInBytes, checkpoint.SessionName, checkpoint.Description);
			Static.PersistentEditMode = persistentEditMode;
			Static.LoadedAsMission = true;
		}

		public static void LoadMission(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, string name, string description)
		{
			MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
			Load(sessionPath, checkpoint, checkpointSizeInBytes);
			Static.Name = name;
			Static.Description = description;
			string text = MyUtils.StripInvalidChars(checkpoint.SessionName);
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			while (Directory.Exists(Static.CurrentPath))
			{
				Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
			}
		}

		public static void Load(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, bool saveLastStates = true, bool allowXml = true)
		{
			PrioritizedScheduler prioritizedScheduler = Parallel.Scheduler as PrioritizedScheduler;
<<<<<<< HEAD
			prioritizedScheduler?.SuspendThreads(WorkPriority.VeryLow);
=======
			prioritizedScheduler.SuspendThreads(WorkPriority.VeryLow);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			try
			{
				MyLog.Default.WriteLineAndConsole("Loading session: " + sessionPath);
				Static = new MySession
				{
					Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName),
					Description = checkpoint.Description,
					Mods = checkpoint.Mods,
					Settings = checkpoint.Settings,
					CurrentPath = sessionPath
				};
				Static.Settings.EnableIngameScripts &= MyVRage.Platform.Scripting.IsRuntimeCompilationSupported;
				MyObjectBuilder_SessionSettings.ExperimentalReason settingsExperimentalReason = Static.GetSettingsExperimentalReason(remote: false, checkpoint);
				MyLog.Default.WriteLineAndConsole("Experimental mode: " + ((settingsExperimentalReason != (MyObjectBuilder_SessionSettings.ExperimentalReason)0L) ? "Yes" : "No"));
				MyLog.Default.WriteLineAndConsole("Experimental mode reason: " + settingsExperimentalReason);
				MyLog.Default.WriteLineAndConsole("Console compatibility: " + (MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "Yes" : "No"));
				MyEntityIdentifier.Reset();
				bool needsXml = false;
				ulong sizeInBytes;
				MyObjectBuilder_Sector myObjectBuilder_Sector = MyLocalCache.LoadSector(sessionPath, checkpoint.CurrentSector, allowXml, out sizeInBytes, out needsXml);
				if (myObjectBuilder_Sector == null)
				{
					if (!allowXml && needsXml)
					{
						throw new MyLoadingNeedXMLException();
					}
					throw new ApplicationException("Sector could not be loaded");
				}
				if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.OnlineMode != 0)
				{
					Static.StartServerRequest();
				}
				ulong voxelsSizeInBytes = GetVoxelsSizeInBytes(sessionPath);
				Static.WorldSizeInBytes = checkpointSizeInBytes + sizeInBytes + voxelsSizeInBytes;
				MyCubeGrid.Preload();
				MySession.BeforeLoading?.Invoke();
				MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(checkpoint, myObjectBuilder_Sector);
				Static.PrepareBaseSession(checkpoint, myObjectBuilder_Sector);
				MyVisualScriptLogicProvider.Init();
				if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.OnlineMode != 0 && !Static.WaitForServerRequest(out var statusCode))
				{
					Static.ShowLoadingError(lobbyFailed: true, statusCode);
					return;
				}
				Static.LoadWorld(checkpoint, myObjectBuilder_Sector);
<<<<<<< HEAD
				prioritizedScheduler?.ResumeThreads(WorkPriority.VeryLow);
=======
				prioritizedScheduler.ResumeThreads(WorkPriority.VeryLow);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (Sync.IsServer)
				{
					Static.InitializeFactions();
				}
				if (saveLastStates)
				{
					MyLocalCache.SaveLastSessionInfo(sessionPath, isOnline: false, isLobby: false, Static.Name, null, 0);
				}
				Static.LogSettings();
				MyHud.Notifications.Get(MyNotificationSingletons.WorldLoaded).SetTextFormatArguments(Static.Name);
				MyHud.Notifications.Add(MyNotificationSingletons.WorldLoaded);
				if (!MyFakes.LOAD_UNCONTROLLED_CHARACTERS && !MySessionComponentReplay.Static.HasAnyData)
				{
					Static.RemoveUncontrolledCharacters();
				}
				MyGeneralStats.Clear();
				MyHudChat.ResetChatSettings();
				Static.BeforeStartComponents();
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					CacheGuiTexturePaths();
				}
				RaiseAfterLoading();
				MySpaceBindingCreator.CreateBindingDefault();
				if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.LocalCharacter != null)
				{
					foreach (MyCharacter savedCharacter in Static.SavedCharacters)
					{
						MyLocalCache.LoadInventoryConfig(savedCharacter);
					}
				}
				Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.SendEntitiesToClients();
				MyLog.Default.WriteLineAndConsole("Session loaded");
			}
<<<<<<< HEAD
			catch (Exception ex)
			{
				MyLog.Default.Error("Error during loading session:" + ex);
				throw;
			}
			finally
			{
				prioritizedScheduler?.ResumeThreads(WorkPriority.VeryLow);
			}
		}

		private static void PreloadModels(MyObjectBuilder_Sector sector)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (sector.SectorObjects != null)
			{
				foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
				{
					if (!(sectorObject is MyObjectBuilder_CubeGrid))
					{
						continue;
					}
					foreach (MyObjectBuilder_CubeBlock cubeBlock in (sectorObject as MyObjectBuilder_CubeGrid).CubeBlocks)
					{
						if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out var blockDefinition) && !string.IsNullOrEmpty(blockDefinition.Model))
						{
							hashSet.Add(blockDefinition.Model);
						}
					}
				}
			}
			string[] blockModels = hashSet.ToArray();
			WorkPriority priority = WorkPriority.Normal;
			Parallel.Start(delegate
			{
				WorkOptions value2 = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload For");
				value2.MaximumThreads = int.MaxValue;
				Parallel.For(0, blockModels.Length, delegate(int i)
				{
					MyModels.GetModelOnlyData(blockModels[i]);
				}, priority, value2);
			}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload"), priority);
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.HasModelVariants)
				{
					if (physicalItemDefinition.Models != null)
					{
						string[] models2 = physicalItemDefinition.Models;
						foreach (string item in models2)
						{
							hashSet2.Add(item);
						}
					}
				}
				else if (!string.IsNullOrEmpty(physicalItemDefinition.Model))
				{
					hashSet2.Add(physicalItemDefinition.Model);
				}
			}
			foreach (MyDebrisDefinition debrisDefinition in MyDefinitionManager.Static.GetDebrisDefinitions())
			{
				if (!string.IsNullOrEmpty(debrisDefinition.Model))
				{
					hashSet2.Add(debrisDefinition.Model);
				}
			}
			WorkPriority animPriority = WorkPriority.VeryLow;
			string[] models = hashSet2.ToArray();
			Parallel.Start(delegate
			{
=======
			finally
			{
				prioritizedScheduler.ResumeThreads(WorkPriority.VeryLow);
			}
		}

		private static void PreloadModels(MyObjectBuilder_Sector sector)
		{
			HashSet<string> val = new HashSet<string>();
			if (sector.SectorObjects != null)
			{
				foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
				{
					if (!(sectorObject is MyObjectBuilder_CubeGrid))
					{
						continue;
					}
					foreach (MyObjectBuilder_CubeBlock cubeBlock in (sectorObject as MyObjectBuilder_CubeGrid).CubeBlocks)
					{
						if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out var blockDefinition) && !string.IsNullOrEmpty(blockDefinition.Model))
						{
							val.Add(blockDefinition.Model);
						}
					}
				}
			}
			string[] blockModels = Enumerable.ToArray<string>((IEnumerable<string>)val);
			WorkPriority priority = WorkPriority.Normal;
			Parallel.Start(delegate
			{
				WorkOptions value2 = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload For");
				value2.MaximumThreads = int.MaxValue;
				Parallel.For(0, blockModels.Length, delegate(int i)
				{
					MyModels.GetModelOnlyData(blockModels[i]);
				}, priority, value2);
			}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Block Models Preload"), priority);
			HashSet<string> val2 = new HashSet<string>();
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.HasModelVariants)
				{
					if (physicalItemDefinition.Models != null)
					{
						string[] models2 = physicalItemDefinition.Models;
						foreach (string text in models2)
						{
							val2.Add(text);
						}
					}
				}
				else if (!string.IsNullOrEmpty(physicalItemDefinition.Model))
				{
					val2.Add(physicalItemDefinition.Model);
				}
			}
			foreach (MyDebrisDefinition debrisDefinition in MyDefinitionManager.Static.GetDebrisDefinitions())
			{
				if (!string.IsNullOrEmpty(debrisDefinition.Model))
				{
					val2.Add(debrisDefinition.Model);
				}
			}
			WorkPriority animPriority = WorkPriority.VeryLow;
			string[] models = Enumerable.ToArray<string>((IEnumerable<string>)val2);
			Parallel.Start(delegate
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				WorkOptions value = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Models Preload For");
				value.MaximumThreads = int.MaxValue;
				Parallel.For(0, models.Length, delegate(int i)
				{
					MyModels.GetModelOnlyData(models[i]);
				}, animPriority, value);
				MyCharacter.Preload(animPriority);
			}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "Models Preload"), animPriority);
		}

		private static void CacheGuiTexturePaths()
		{
			foreach (MyFactionIconsDefinition allDefinition in MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>())
			{
				if (allDefinition.Icons == null || allDefinition.Icons.Length == 0)
				{
					continue;
				}
				for (int i = 0; i < allDefinition.Icons.Length; i++)
				{
					if (!string.IsNullOrEmpty(allDefinition.Icons[i]))
					{
						ImageManager.Instance.AddImage(allDefinition.Icons[i]);
					}
				}
			}
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.Icons != null && physicalItemDefinition.Icons.Length != 0 && !string.IsNullOrEmpty(physicalItemDefinition.Icons[0]))
				{
					ImageManager.Instance.AddImage(physicalItemDefinition.Icons[0]);
				}
			}
			foreach (MyPrefabDefinition value in MyDefinitionManager.Static.GetPrefabDefinitions().Values)
			{
				if (!value.Context.IsBaseGame)
				{
					string modPath = value.Context.ModPath;
					if (value.Icons != null && value.Icons.Length == 1 && !string.IsNullOrEmpty(value.Icons[0]))
					{
						value.Icons[0] = Path.Combine(modPath, value.Icons[0]);
						ImageManager.Instance.AddImage(value.Icons[0]);
					}
					if (!string.IsNullOrEmpty(value.TooltipImage))
					{
						value.TooltipImage = Path.Combine(modPath, value.TooltipImage);
						ImageManager.Instance.AddImage(value.TooltipImage);
					}
				}
			}
		}

		private static void PreloadAnimations(string relativeDirectory)
		{
			IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, relativeDirectory), "*.mwm", MySearchOption.AllDirectories);
<<<<<<< HEAD
			if (files == null || !files.Any())
=======
			if (files == null || !Enumerable.Any<string>(files))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			foreach (string item in files)
			{
				MyModels.GetModelOnlyAnimationData(item.Replace(MyFileSystem.ContentPath, string.Empty).TrimStart(new char[1] { Path.DirectorySeparatorChar }));
			}
		}

		internal static void CreateWithEmptyWorld(MyMultiplayerBase multiplayerSession)
		{
			Static = new MySession(multiplayerSession.SyncLayer, registerComponents: false);
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			Static.Gpss.RegisterChat(multiplayerSession);
			Static.CameraController = MySpectatorCameraController.Static;
			Static.Settings = new MyObjectBuilder_SessionSettings();
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			Static.PrepareBaseSession(new List<MyObjectBuilder_Checkpoint.ModItem>());
			multiplayerSession.StartProcessingClientMessagesWithEmptyWorld();
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			MyLocalCache.ClearLastSessionInfo();
			if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.LocalHumanPlayer == null)
			{
				Sync.Players.RequestNewPlayer(Sync.MyId, 0, MyGameService.UserName, null, realPlayer: true, initialPlayer: true);
			}
			MyGeneralStats.Clear();
		}

		internal void LoadMultiplayerWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			Static.UnloadDataComponents(beforeLoadWorld: true);
			MyDefinitionManager.Static.UnloadData();
			Static.Mods = world.Checkpoint.Mods;
			Static.Settings = world.Checkpoint.Settings;
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), contentFolder: false, createIfNotExists: false);
			if (!MyDefinitionManager.Static.TryGetDefinition<MyScenarioDefinition>(world.Checkpoint.Scenario, out Static.Scenario))
			{
				Static.Scenario = Enumerable.FirstOrDefault<MyScenarioDefinition>((IEnumerable<MyScenarioDefinition>)MyDefinitionManager.Static.GetScenarioDefinitions());
			}
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
			Static.PrepareBaseSession(world.Checkpoint, world.Sector);
			_ = world.Checkpoint.ControlledObject;
			world.Checkpoint.ControlledObject = -1L;
			Static.Gpss.RegisterChat(multiplayerSession);
			Static.CameraController = MySpectatorCameraController.Static;
			Static.LoadWorld(world.Checkpoint, world.Sector);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			MyLocalCache.ClearLastSessionInfo();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CacheGuiTexturePaths();
			}
			Static.BeforeStartComponents();
		}

		private void LoadMembersFromWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			if (multiplayerSession is MyMultiplayerClient)
			{
				(multiplayerSession as MyMultiplayerClient).LoadMembersFromWorld(world.Checkpoint.Clients);
			}
		}

		private void RemoveUncontrolledCharacters()
		{
<<<<<<< HEAD
			if (!Sync.IsServer)
			{
				return;
			}
			foreach (MyCharacter item in MyEntities.GetEntities().OfType<MyCharacter>())
			{
				if (item.ControllerInfo.Controller != null && (!item.ControllerInfo.IsRemotelyControlled() || item.GetCurrentMovementState() == MyCharacterMovementEnum.Died))
				{
					continue;
				}
				MyLargeTurretBase myLargeTurretBase = ControlledEntity as MyLargeTurretBase;
				if (myLargeTurretBase == null || myLargeTurretBase.Pilot != item)
				{
=======
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			if (!Sync.IsServer)
			{
				return;
			}
			foreach (MyCharacter item in Enumerable.OfType<MyCharacter>((IEnumerable)MyEntities.GetEntities()))
			{
				if (item.ControllerInfo.Controller != null && (!item.ControllerInfo.IsRemotelyControlled() || item.GetCurrentMovementState() == MyCharacterMovementEnum.Died))
				{
					continue;
				}
				MyLargeTurretBase myLargeTurretBase = ControlledEntity as MyLargeTurretBase;
				if (myLargeTurretBase == null || myLargeTurretBase.Pilot != item)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyRemoteControl myRemoteControl = ControlledEntity as MyRemoteControl;
					if (myRemoteControl == null || myRemoteControl.Pilot != item)
					{
						item.Close();
					}
				}
			}
<<<<<<< HEAD
			foreach (MyCubeGrid item2 in MyEntities.GetEntities().OfType<MyCubeGrid>())
			{
				foreach (MySlimBlock block in item2.GetBlocks())
				{
					MyCockpit myCockpit = block.FatBlock as MyCockpit;
					if (myCockpit != null && !(myCockpit is MyCryoChamber) && myCockpit.Pilot != null && myCockpit.Pilot != LocalCharacter)
					{
						myCockpit.Pilot.Close();
						myCockpit.ClearSavedpilot();
=======
			foreach (MyCubeGrid item2 in Enumerable.OfType<MyCubeGrid>((IEnumerable)MyEntities.GetEntities()))
			{
				Enumerator<MySlimBlock> enumerator3 = item2.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						MyCockpit myCockpit = enumerator3.get_Current().FatBlock as MyCockpit;
						if (myCockpit != null && !(myCockpit is MyCryoChamber) && myCockpit.Pilot != null && myCockpit.Pilot != LocalCharacter)
						{
							myCockpit.Pilot.Close();
							myCockpit.ClearSavedpilot();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator3).Dispose();
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Creates new Steam lobby
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void StartServerRequest()
		{
			if (MyGameService.IsOnline)
			{
				UnloadMultiplayer();
				MyNetworkMonitor.StartSession();
				m_serverRequest = MyMultiplayer.HostLobby(GetLobbyType(Static.OnlineMode), Static.MaxPlayers, Static.SyncLayer);
				m_serverRequest.Done += OnMultiplayerHost;
			}
			else
			{
				m_serverRequest = null;
			}
		}

		private bool WaitForServerRequest(out MyLobbyStatusCode statusCode)
		{
			if (m_serverRequest != null)
			{
				m_serverRequest.Wait();
				statusCode = m_serverRequest.StatusCode;
				return m_serverRequest.Success;
			}
			statusCode = MyLobbyStatusCode.NoUser;
			return false;
		}

		private static MyLobbyType GetLobbyType(MyOnlineModeEnum onlineMode)
		{
			return onlineMode switch
			{
				MyOnlineModeEnum.FRIENDS => MyLobbyType.FriendsOnly, 
				MyOnlineModeEnum.PUBLIC => MyLobbyType.Public, 
				MyOnlineModeEnum.PRIVATE => MyLobbyType.Private, 
				_ => MyLobbyType.Private, 
			};
		}

		private static void OnMultiplayerHost(bool success, MyLobbyStatusCode reason, MyMultiplayerBase multiplayer)
		{
			if (success)
			{
				Static?.StartServer(multiplayer);
			}
			else
			{
				Static?.UnloadMultiplayer();
			}
		}

		private void LoadWorld(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
			MySandboxGame.Static.SessionCompatHelper.FixSessionObjectBuilders(checkpoint, sector);
			MyEntities.MemoryLimitAddFailureReset();
			ElapsedGameTime = new TimeSpan(checkpoint.ElapsedGameTime);
			InGameTime = checkpoint.InGameTime;
			Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName);
			Description = checkpoint.Description;
			PromotedUsers = ((checkpoint.PromotedUsers != null) ? checkpoint.PromotedUsers.Dictionary : new Dictionary<ulong, MyPromoteLevel>());
			CreativeTools = checkpoint.CreativeTools ?? new HashSet<ulong>();
			m_remoteAdminSettings.Clear();
			if (checkpoint?.AllPlayersData != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item in checkpoint.AllPlayersData.Dictionary)
				{
					ulong clientId = item.Key.GetClientId();
					AdminSettingsEnum adminSettingsEnum = (AdminSettingsEnum)item.Value.RemoteAdminSettings;
					if (checkpoint.RemoteAdminSettings != null && checkpoint.RemoteAdminSettings.Dictionary.TryGetValue(clientId, out var value))
					{
						adminSettingsEnum = (AdminSettingsEnum)value;
					}
					if (!MyPlatformGameSettings.IsIgnorePcuAllowed)
					{
						adminSettingsEnum &= ~AdminSettingsEnum.IgnorePcu;
						adminSettingsEnum &= ~AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
					}
					m_remoteAdminSettings[clientId] = adminSettingsEnum;
					if (!Sync.IsDedicated && clientId == Sync.MyId)
					{
						m_adminSettings = adminSettingsEnum;
					}
					if (!PromotedUsers.TryGetValue(clientId, out var value2))
					{
						value2 = MyPromoteLevel.None;
					}
					if (item.Value.PromoteLevel > value2)
					{
						PromotedUsers[clientId] = item.Value.PromoteLevel;
					}
					if (!CreativeTools.Contains(clientId) && item.Value.CreativeToolsEnabled)
					{
						CreativeTools.Add(clientId);
					}
				}
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				foreach (KeyValuePair<ulong, MyPromoteLevel> item2 in Enumerable.ToList<KeyValuePair<ulong, MyPromoteLevel>>(Enumerable.Where<KeyValuePair<ulong, MyPromoteLevel>>((IEnumerable<KeyValuePair<ulong, MyPromoteLevel>>)PromotedUsers, (Func<KeyValuePair<ulong, MyPromoteLevel>, bool>)((KeyValuePair<ulong, MyPromoteLevel> e) => e.Value == MyPromoteLevel.Owner))))
				{
					PromotedUsers.Remove(item2.Key);
				}
				foreach (string administrator in MySandboxGame.ConfigDedicated.Administrators)
				{
					if (ulong.TryParse(administrator, out var result2))
					{
						PromotedUsers[result2] = MyPromoteLevel.Owner;
					}
				}
			}
			WorkshopId = checkpoint.WorkshopId;
			Password = checkpoint.Password;
			PreviousEnvironmentHostility = checkpoint.PreviousEnvironmentHostility;
			RequiresDX = checkpoint.RequiresDX;
			CustomLoadingScreenImage = checkpoint.CustomLoadingScreenImage;
			CustomLoadingScreenText = checkpoint.CustomLoadingScreenText;
			CustomSkybox = checkpoint.CustomSkybox;
			AppVersionFromSave = checkpoint.AppVersion;
			MyToolbarComponent.InitCharacterToolbar(checkpoint.CharacterToolbar);
			LoadCameraControllerSettings(checkpoint);
			TotalSessionPCU = 0;
			NPCBlockLimits = new MyBlockLimits(Static.PiratePCU, 0);
			GlobalBlockLimits = new MyBlockLimits(Static.TotalPCU, 0);
			SessionBlockLimits = new MyBlockLimits(Static.TotalPCU, 0);
			MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
			MyPlayer.PlayerId? savingPlayerId = null;
			if (TryFindSavingPlayerId(checkpoint, out playerId) && (!IsScenario || Static.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				savingPlayerId = playerId;
			}
			if (Sync.IsServer || (!IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME))
			{
				Sync.Players.LoadIdentities(checkpoint, savingPlayerId);
			}
			if (!IsScenario || !Static.Settings.StartInRespawnScreen)
			{
				Sync.Players.LoadConnectedPlayers(checkpoint, savingPlayerId);
			}
			else
			{
				Static.Settings.StartInRespawnScreen = false;
			}
			Sync.Players.RespawnComponent.InitFromCheckpoint(checkpoint);
			Toolbars.LoadToolbars(checkpoint);
			if (checkpoint.Factions != null && (Sync.IsServer || (!IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME)))
			{
				Static.Factions.Init(checkpoint.Factions);
			}
			if (Settings.ProceduralDensity == 0f)
<<<<<<< HEAD
			{
				if (MyVRage.Platform.System.IsMemoryLimited)
				{
					MyStorageBase.ResetCache();
					GC.Collect(GC.MaxGeneration);
				}
				MyVRage.Platform.System.OnSessionStarted(SessionType.ExtendedEntities);
			}
			else
			{
				MyVRage.Platform.System.OnSessionStarted(SessionType.Normal);
			}
			if (!MyEntities.Load(sector.SectorObjects, out var errorMessage))
			{
=======
			{
				if (MyVRage.Platform.System.IsMemoryLimited)
				{
					MyStorageBase.ResetCache();
					GC.Collect(GC.MaxGeneration);
				}
				MyVRage.Platform.System.OnSessionStarted(SessionType.ExtendedEntities);
			}
			else
			{
				MyVRage.Platform.System.OnSessionStarted(SessionType.Normal);
			}
			if (!MyEntities.Load(sector.SectorObjects, out var errorMessage))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ShowLoadingError(lobbyFailed: false, MyLobbyStatusCode.Error, errorMessage);
				return;
			}
			if (!MyCampaignManager.Static.IsScenarioRunning)
			{
				if ((MyPlatformGameSettings.CONSOLE_COMPATIBLE || !MySandboxGame.Config.ExperimentalMode) && !Sandbox.Engine.Platform.Game.IsDedicated)
				{
					foreach (MyIdentity allIdentity in Players.GetAllIdentities())
					{
						if (!Players.IdentityIsNpc(allIdentity.IdentityId) && allIdentity.BlockLimits.IsOverLimits)
						{
							MyStringId value3 = ((!MySandboxGame.Config.ExperimentalMode) ? MyCommonTexts.SaveGameErrorExperimental : MyCommonTexts.SaveGameErrorOverBlockLimits);
							ShowLoadingError(lobbyFailed: false, MyLobbyStatusCode.Error, value3);
							return;
						}
					}
				}
				if (Sync.IsServer && !MyPlatformGameSettings.IsIgnorePcuAllowed && BlockLimitsEnabled == MyBlockLimitsEnabledEnum.GLOBALLY)
				{
					OnReady += TestUncountedPCUs;
				}
			}
			Parallel.RunCallbacks();
			MySandboxGame.Static.SessionCompatHelper.AfterEntitiesLoad(sector.AppVersion);
			MyGlobalEvents.LoadEvents(sector.SectorEvents);
			MySpectatorCameraController.Static.InitLight(checkpoint.SpectatorIsLightOn);
			if (Sync.IsServer)
			{
				MySpectatorCameraController.Static.SetViewMatrix(MatrixD.Invert(checkpoint.SpectatorPosition.GetMatrix()));
				MySpectatorCameraController.Static.SpeedModeLinear = checkpoint.SpectatorSpeed.X;
				MySpectatorCameraController.Static.SpeedModeAngular = checkpoint.SpectatorSpeed.Y;
			}
			if (!IsScenario || !Static.Settings.StartInRespawnScreen)
			{
				Sync.Players.LoadControlledEntities(checkpoint.ControlledEntities, checkpoint.ControlledObject, savingPlayerId);
			}
			LoadCamera(checkpoint);
			if (CreativeMode && !Sandbox.Engine.Platform.Game.IsDedicated && LocalHumanPlayer != null && LocalHumanPlayer.Character != null && LocalHumanPlayer.Character.IsDead)
			{
				MyPlayerCollection.RequestLocalRespawn();
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.OnSessionReady();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated && LocalHumanPlayer == null)
			{
				Sync.Players.RequestNewPlayer(Sync.MyId, 0, MyGameService.UserName, null, realPlayer: true, initialPlayer: true);
			}
			else if (ControlledEntity == null && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyLog.Default.WriteLine("ControlledObject was null, respawning character");
				m_cameraAwaitingEntity = true;
				MyPlayerCollection.RequestLocalRespawn();
			}
			SharedToolbar = checkpoint.SharedToolbar;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyPlayer.PlayerId pid = new MyPlayer.PlayerId(Sync.MyId, 0);
				MyToolbar myToolbar = Toolbars.TryGetPlayerToolbar(pid);
				if (checkpoint.SharedToolbar != 0L)
				{
					MyPlayer.PlayerId pid2 = new MyPlayer.PlayerId(checkpoint.SharedToolbar, 0);
					MyToolbar myToolbar2 = Toolbars.TryGetPlayerToolbar(pid2);
					if (myToolbar2 != null)
					{
						myToolbar = myToolbar2;
					}
				}
				if (myToolbar == null)
				{
					MyToolBarCollection.RequestCreateToolbar(pid);
					MyToolbarComponent.InitCharacterToolbar(Scenario.DefaultToolbar);
				}
				else
				{
					MyToolbarComponent.InitCharacterToolbar(myToolbar.GetObjectBuilder());
				}
				GetComponent<MyRadialMenuComponent>().InitDefaultLastUsed(Scenario.DefaultToolbar);
			}
			Gpss.LoadGpss(checkpoint);
			MyRenderProxy.RebuildCullingStructure();
			Settings.ResetOwnership = false;
			if (!CreativeMode)
			{
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = false;
<<<<<<< HEAD
			}
			if (!MySandboxGame.Config.SyncRendering)
			{
				MyRenderProxy.WaitForFrame();
			}
			MyRenderProxy.CollectGarbage();
			void TestUncountedPCUs()
			{
				if (Ready)
				{
					int num = NPCBlockLimits.PCUBuilt + GlobalBlockLimits.PCUBuilt;
					if (TotalSessionPCU - num > 50000)
					{
						if (MyEntities.HasEntitiesToDelete())
						{
							MySandboxGame.Static.Invoke(TestUncountedPCUs, "TestUncountedPCUs");
						}
						else
						{
							MyLog.Default.WriteLine($"Warning: World has too many uncounted PCUs: {TotalSessionPCU};{num}");
							MyStringId message;
							if (IsRunningExperimental)
							{
								message = MySpaceTexts.Notification_TooManyUncounterPCUWarning;
							}
							else
							{
								MySessionLoader.UnloadAndExitToMenu();
								message = MySpaceTexts.Notification_TooManyUncounterPCUError;
							}
							MyWorkshopItem workshopItem = MySessionLoader.LastLoadedSessionWorkshopItem;
							MyGuiScreenBase screen = ((workshopItem == null) ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(message)) : MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder(MyTexts.GetString(message) + "\n" + MyTexts.GetString(MySpaceTexts.Notification_TooManyUncounterPCUReport)), null, null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
							{
								if (result == MyGuiScreenMessageBox.ResultEnum.YES)
								{
									workshopItem.Report("[" + message.String + "] " + MyTexts.GetString(message));
								}
							}));
							MyGuiSandbox.AddScreen(screen);
						}
					}
				}
			}
=======
			}
			if (!MySandboxGame.Config.SyncRendering)
			{
				MyRenderProxy.WaitForFrame();
			}
			MyRenderProxy.CollectGarbage();
			void TestUncountedPCUs()
			{
				if (Ready)
				{
					int num = NPCBlockLimits.PCUBuilt + GlobalBlockLimits.PCUBuilt;
					if (TotalSessionPCU - num > 50000)
					{
						if (MyEntities.HasEntitiesToDelete())
						{
							MySandboxGame.Static.Invoke(TestUncountedPCUs, "TestUncountedPCUs");
						}
						else
						{
							MyLog.Default.WriteLine($"Warning: World has too many uncounted PCUs: {TotalSessionPCU};{num}");
							MyStringId message;
							if (IsRunningExperimental)
							{
								message = MySpaceTexts.Notification_TooManyUncounterPCUWarning;
							}
							else
							{
								MySessionLoader.UnloadAndExitToMenu();
								message = MySpaceTexts.Notification_TooManyUncounterPCUError;
							}
							MyWorkshopItem workshopItem = MySessionLoader.LastLoadedSessionWorkshopItem;
							MyGuiScreenBase screen = ((workshopItem == null) ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, MyTexts.Get(message)) : MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder(MyTexts.GetString(message) + "\n" + MyTexts.GetString(MySpaceTexts.Notification_TooManyUncounterPCUReport)), null, null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
							{
								if (result == MyGuiScreenMessageBox.ResultEnum.YES)
								{
									workshopItem.Report("[" + message.String + "] " + MyTexts.GetString(message));
								}
							}));
							MyGuiSandbox.AddScreen(screen);
						}
					}
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private bool TryFindSavingPlayerId(MyObjectBuilder_Checkpoint checkpoint, out MyPlayer.PlayerId playerId)
		{
			playerId = default(MyPlayer.PlayerId);
			if (!MyFakes.REUSE_OLD_PLAYER_IDENTITY)
			{
				return false;
			}
			if (!Sync.IsServer || Sync.Clients.Count != 1)
			{
				return false;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			if (checkpoint?.AllPlayersData == null)
			{
				return false;
			}
			if (checkpoint.Identities == null)
			{
				return false;
			}
			bool flag = false;
			MyObjectBuilder_Identity myObjectBuilder_Identity = checkpoint.Identities.Find((MyObjectBuilder_Identity i) => i.CharacterEntityId == checkpoint.ControlledObject);
			if (myObjectBuilder_Identity == null)
			{
				if (checkpoint.ControlledEntities.Dictionary.TryGetValue(checkpoint.ControlledObject, out var value))
				{
					foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item in checkpoint.AllPlayersData.Dictionary)
					{
						if (item.Key.GetClientId() == value.GetClientId() && item.Key.SerialId == value.SerialId)
						{
							playerId = new MyPlayer.PlayerId(value.GetClientId(), value.SerialId);
						}
						if (item.Key.GetClientId() == Sync.MyId && item.Key.SerialId == 0)
						{
							flag = true;
						}
					}
				}
				return !flag;
			}
			foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item2 in checkpoint.AllPlayersData.Dictionary)
			{
				if (item2.Value.IdentityId == myObjectBuilder_Identity.IdentityId)
				{
					playerId = new MyPlayer.PlayerId(item2.Key.GetClientId(), item2.Key.SerialId);
				}
				if (item2.Key.GetClientId() == Sync.MyId && item2.Key.SerialId == 0)
				{
					flag = true;
				}
			}
			return !flag;
		}

		private void LoadCamera(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.SpectatorDistance > 0f)
			{
				MyThirdPersonSpectator.Static.UpdateAfterSimulation();
				MyThirdPersonSpectator.Static.ResetViewerDistance(checkpoint.SpectatorDistance);
			}
			MySandboxGame.Log.WriteLine("Checkpoint.CameraAttachedTo: " + checkpoint.CameraEntity);
			IMyEntity myEntity = null;
			MyCameraControllerEnum myCameraControllerEnum = checkpoint.CameraController;
			if (!Static.Enable3RdPersonView && myCameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				myCameraControllerEnum = (checkpoint.CameraController = MyCameraControllerEnum.Entity);
			}
			if (checkpoint.CameraEntity == 0L && ControlledEntity != null)
			{
				myEntity = ControlledEntity as MyEntity;
				if (myEntity != null)
				{
					IMyPilotable myPilotable;
					if ((myPilotable = ControlledEntity as IMyPilotable) != null && myPilotable.CanHavePreviousControlledEntity)
					{
						myEntity = myPilotable.GetPreviousCameraEntity;
					}
					else if (!(ControlledEntity is IMyCameraController))
					{
						myEntity = null;
						myCameraControllerEnum = MyCameraControllerEnum.Spectator;
					}
				}
			}
			else if (!MyEntities.EntityExists(checkpoint.CameraEntity))
			{
				myEntity = ControlledEntity as MyEntity;
				if (myEntity != null)
				{
					myCameraControllerEnum = MyCameraControllerEnum.Entity;
					if (!(ControlledEntity is IMyCameraController))
					{
						myEntity = null;
						myCameraControllerEnum = MyCameraControllerEnum.Spectator;
					}
				}
				else
				{
					MyLog.Default.WriteLine("ERROR: Camera entity from checkpoint does not exists!");
					myCameraControllerEnum = MyCameraControllerEnum.Spectator;
				}
			}
			else
			{
				myEntity = MyEntities.GetEntityById(checkpoint.CameraEntity);
			}
			if (myCameraControllerEnum == MyCameraControllerEnum.Spectator && myEntity != null)
			{
				myCameraControllerEnum = MyCameraControllerEnum.Entity;
			}
			MyEntityCameraSettings cameraSettings = null;
			bool flag = false;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && (myCameraControllerEnum == MyCameraControllerEnum.Entity || myCameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator) && myEntity != null)
			{
				MyPlayer.PlayerId pid = ((LocalHumanPlayer == null) ? new MyPlayer.PlayerId(Sync.MyId, 0) : LocalHumanPlayer.Id);
				if (Static.Cameras.TryGetCameraSettings(pid, myEntity.EntityId, myEntity is MyCharacter && LocalCharacter == myEntity, out cameraSettings) && !cameraSettings.IsFirstPerson)
				{
					myCameraControllerEnum = MyCameraControllerEnum.ThirdPersonSpectator;
					flag = true;
				}
			}
			Static.IsCameraAwaitingEntity = false;
			SetCameraController(myCameraControllerEnum, myEntity);
			if (flag)
			{
				MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
				MyThirdPersonSpectator.Static.ResetViewerDistance(cameraSettings.Distance);
			}
		}

		private void LoadCameraControllerSettings(MyObjectBuilder_Checkpoint checkpoint)
		{
			Cameras.LoadCameraCollection(checkpoint);
		}

		internal static void FixIncorrectSettings(MyObjectBuilder_SessionSettings settings)
		{
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
			if (settings.RefinerySpeedMultiplier <= 0f)
			{
				settings.RefinerySpeedMultiplier = myObjectBuilder_SessionSettings.RefinerySpeedMultiplier;
			}
			if (settings.AssemblerSpeedMultiplier <= 0f)
			{
				settings.AssemblerSpeedMultiplier = myObjectBuilder_SessionSettings.AssemblerSpeedMultiplier;
			}
			if (settings.AssemblerEfficiencyMultiplier <= 0f)
			{
				settings.AssemblerEfficiencyMultiplier = myObjectBuilder_SessionSettings.AssemblerEfficiencyMultiplier;
			}
			if (settings.InventorySizeMultiplier <= 0f)
			{
				settings.InventorySizeMultiplier = myObjectBuilder_SessionSettings.InventorySizeMultiplier;
			}
			if (settings.WelderSpeedMultiplier <= 0f)
			{
				settings.WelderSpeedMultiplier = myObjectBuilder_SessionSettings.WelderSpeedMultiplier;
			}
			if (settings.GrinderSpeedMultiplier <= 0f)
			{
				settings.GrinderSpeedMultiplier = myObjectBuilder_SessionSettings.GrinderSpeedMultiplier;
			}
			if (settings.HackSpeedMultiplier <= 0f)
			{
				settings.HackSpeedMultiplier = myObjectBuilder_SessionSettings.HackSpeedMultiplier;
			}
			if (!settings.PermanentDeath.HasValue)
			{
				settings.PermanentDeath = true;
			}
			settings.ViewDistance = MathHelper.Clamp(settings.ViewDistance, 1000, 50000);
			settings.SyncDistance = MathHelper.Clamp(settings.SyncDistance, 1000, 20000);
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				settings.Scenario = false;
				settings.ScenarioEditMode = false;
			}
			if (Static != null && Static.Scenario != null)
			{
				settings.WorldSizeKm = ((!Static.Scenario.HasPlanets) ? settings.WorldSizeKm : 0);
			}
			if (Static != null && !Static.WorldBoundaries.HasValue && settings.WorldSizeKm > 0)
			{
				double num = settings.WorldSizeKm * 500;
				if (num > 0.0)
				{
					Static.WorldBoundaries = new BoundingBoxD(new Vector3D(0.0 - num, 0.0 - num, 0.0 - num), new Vector3D(num, num, num));
				}
			}
		}

		private void ShowLoadingError(bool lobbyFailed = false, MyLobbyStatusCode statusCode = MyLobbyStatusCode.Error, MyStringId? errorMessage = null)
		{
			StringBuilder stringBuilder;
			if (!lobbyFailed)
			{
				stringBuilder = ((!MyEntities.MemoryLimitAddFailure) ? MyTexts.Get(errorMessage ?? MyCommonTexts.MessageBoxTextErrorLoadingEntities) : MyTexts.Get(MyCommonTexts.MessageBoxTextMemoryLimitReachedDuringLoad));
			}
			else
			{
				stringBuilder = MyJoinGameHelper.GetErrorMessage(statusCode);
				MyLog.Default.WriteLine(string.Concat("ShowLoadingError: ", statusCode, " / ", stringBuilder));
			}
			throw new MyLoadingException(stringBuilder.ToString());
		}

		/// <summary>
		/// Make sure there's character, will be removed when dead/respawn done
		/// In creative mode, there will be respawn too
		/// </summary>
		internal void FixMissingCharacter()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				bool num = ControlledEntity != null && ControlledEntity is MyCockpit;
<<<<<<< HEAD
				bool flag = MyEntities.GetEntities().OfType<MyCharacter>().Any();
=======
				bool flag = Enumerable.Any<MyCharacter>(Enumerable.OfType<MyCharacter>((IEnumerable)MyEntities.GetEntities()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				bool flag2 = ControlledEntity != null && ControlledEntity is MyRemoteControl && (ControlledEntity as MyRemoteControl).WasControllingCockpitWhenSaved();
				bool flag3 = ControlledEntity != null && ControlledEntity is MyLargeTurretBase && (ControlledEntity as MyLargeTurretBase).WasControllingCockpitWhenSaved();
				if (!num && !flag && !flag2 && !flag3)
				{
					MyPlayerCollection.RequestLocalRespawn();
				}
			}
		}

		public MyCameraControllerEnum GetCameraControllerEnum()
		{
			if (CameraController == MySpectatorCameraController.Static)
			{
				switch (MySpectatorCameraController.Static.SpectatorCameraMovement)
				{
				case MySpectatorCameraMovementEnum.UserControlled:
					return MyCameraControllerEnum.Spectator;
				case MySpectatorCameraMovementEnum.ConstantDelta:
					return MyCameraControllerEnum.SpectatorDelta;
				case MySpectatorCameraMovementEnum.None:
					return MyCameraControllerEnum.SpectatorFixed;
				case MySpectatorCameraMovementEnum.Orbit:
					return MyCameraControllerEnum.SpectatorOrbit;
				}
			}
			else
			{
				if (CameraController == MyThirdPersonSpectator.Static)
				{
					return MyCameraControllerEnum.ThirdPersonSpectator;
				}
				if (CameraController is MyEntity || CameraController is MyEntityRespawnComponentBase)
				{
					if ((!CameraController.IsInFirstPersonView && !CameraController.ForceFirstPersonCamera) || !CameraController.EnableFirstPersonView)
					{
						return MyCameraControllerEnum.ThirdPersonSpectator;
					}
					return MyCameraControllerEnum.Entity;
				}
			}
			return MyCameraControllerEnum.Spectator;
		}

<<<<<<< HEAD
		[Event(null, 3399)]
=======
		[Event(null, 3147)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Client]
		[Reliable]
		public static void SetSpectatorPositionFromServer(Vector3D position)
		{
			Static.SetCameraController(MyCameraControllerEnum.Spectator, null, position);
		}

		public void SetCameraController(MyCameraControllerEnum cameraControllerEnum, IMyEntity cameraEntity = null, Vector3D? position = null)
		{
			if (cameraEntity != null && Spectator.Position == Vector3.Zero)
			{
				Spectator.Position = cameraEntity.GetPosition() + cameraEntity.WorldMatrix.Forward * 4.0 + cameraEntity.WorldMatrix.Up * 2.0;
				Spectator.SetTarget(cameraEntity.GetPosition(), cameraEntity.PositionComp.WorldMatrixRef.Up);
				Spectator.Initialized = true;
			}
			CameraOnCharacter = cameraEntity is MyCharacter;
			switch (cameraControllerEnum)
			{
			case MyCameraControllerEnum.Entity:
			{
				MyEntityRespawnComponentBase component;
				if (cameraEntity is IMyCameraController)
				{
					Static.CameraController = (IMyCameraController)cameraEntity;
				}
<<<<<<< HEAD
				else if (cameraEntity != null && cameraEntity.Components.TryGet<MyEntityRespawnComponentBase>(out component))
=======
				else if (cameraEntity.Components.TryGet<MyEntityRespawnComponentBase>(out component))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					Static.CameraController = component;
				}
				else
				{
					Static.CameraController = LocalCharacter;
				}
				break;
			}
			case MyCameraControllerEnum.Spectator:
				Static.CameraController = MySpectatorCameraController.Static;
				if (Static.ControlledEntity != null && Static.ControlledEntity is MyCockpit)
				{
					((MyCockpit)Static.ControlledEntity).RemoveControlNotifications();
				}
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorFixed:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.None;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorDelta:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.ConstantDelta;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorFreeMouse:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.ThirdPersonSpectator:
				if (cameraEntity != null)
				{
					Static.CameraController = (IMyCameraController)cameraEntity;
				}
				Static.CameraController.IsInFirstPersonView = false;
				break;
			case MyCameraControllerEnum.SpectatorOrbit:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.Orbit;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CameraControllerSet, cameraControllerEnum, cameraEntity?.EntityId ?? 0);
		}

		[Event(null, 3550)]
		[Reliable]
		[Server]
		public static void CameraControllerSet(MyCameraControllerEnum type, long entityId)
		{
			Static.Players.CameraControllerSet(type, entityId, MyEventContext.Current.Sender.Value);
		}

		public void SetEntityCameraPosition(MyPlayer.PlayerId pid, IMyEntity cameraEntity)
		{
			if (LocalHumanPlayer == null || LocalHumanPlayer.Id != pid)
			{
				return;
			}
			if (Cameras.TryGetCameraSettings(pid, cameraEntity.EntityId, cameraEntity is MyCharacter && LocalCharacter == cameraEntity, out var cameraSettings))
			{
				if (!cameraSettings.IsFirstPerson)
				{
					SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, cameraEntity);
					MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
					MyThirdPersonSpectator.Static.ResetViewerDistance(cameraSettings.Distance);
				}
			}
			else if (GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || (cameraEntity is Sandbox.ModAPI.IMyShipController && ((Sandbox.ModAPI.IMyShipController)cameraEntity).IsDefault3rdView))
			{
				SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, cameraEntity);
				MyThirdPersonSpectator.Static.RecalibrateCameraPosition(cameraEntity is MyCharacter);
				MyThirdPersonSpectator.Static.ResetSpring();
				MyThirdPersonSpectator.Static.UpdateZoom();
			}
		}

		public bool IsCameraControlledObject()
		{
			return ControlledEntity == Static.CameraController;
		}

		public bool IsCameraUserControlledSpectator()
		{
			if (MySpectatorCameraController.Static != null)
			{
				if (Static.CameraController == MySpectatorCameraController.Static)
				{
					if (MySpectatorCameraController.Static.SpectatorCameraMovement != 0 && MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.Orbit)
					{
						return MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.FreeMouse;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		public bool IsCameraUserAnySpectator()
		{
			if (MySpectatorCameraController.Static != null)
			{
				if (Static.CameraController == MySpectatorCameraController.Static)
				{
					return MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.None;
				}
				return false;
			}
			return true;
		}

		public float GetCameraTargetDistance()
		{
			return (float)MyThirdPersonSpectator.Static.GetViewerDistance();
		}

		public void SetCameraTargetDistance(double distance)
		{
			MyThirdPersonSpectator.Static.ResetViewerDistance((distance == 0.0) ? null : new double?(distance));
		}

		public void SaveControlledEntityCameraSettings(bool isFirstPerson)
		{
			if (ControlledEntity != null && LocalHumanPlayer != null)
			{
				MyCharacter myCharacter = ControlledEntity as MyCharacter;
				if (myCharacter == null || !myCharacter.IsDead)
				{
					Cameras.SaveEntityCameraSettings(LocalHumanPlayer.Id, ControlledEntity.Entity.EntityId, isFirstPerson, MyThirdPersonSpectator.Static.GetViewerDistance(), myCharacter != null && LocalCharacter == ControlledEntity, ControlledEntity.HeadLocalXAngle, ControlledEntity.HeadLocalYAngle);
				}
			}
		}

		public bool Save(string customSaveName = null)
		{
			m_isSnapshotSaveInProgress = true;
			if (!Save(out var snapshot, customSaveName))
			{
				m_isSnapshotSaveInProgress = false;
				return false;
			}
			bool num = snapshot.Save(null, null);
			if (num)
			{
				WorldSizeInBytes = snapshot.SavedSizeInBytes;
			}
			m_isSnapshotSaveInProgress = false;
			return num;
		}

		public bool Save(out MySessionSnapshot snapshot, string customSaveName = null)
		{
			m_isSaveInProgress = true;
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnServerSaving, arg2: true);
			}
			snapshot = new MySessionSnapshot();
			MySandboxGame.Log.WriteLine("Saving world - START");
			using (MySandboxGame.Log.IndentUsing())
			{
				string saveName = customSaveName ?? Name;
				if (customSaveName != null)
				{
					if (!Path.IsPathRooted(customSaveName))
					{
						string directoryName = Path.GetDirectoryName(CurrentPath);
						if (Directory.Exists(directoryName))
						{
							CurrentPath = Path.Combine(directoryName, customSaveName);
						}
						else
						{
							CurrentPath = MyLocalCache.GetSessionSavesPath(customSaveName, contentFolder: false);
						}
					}
					else
					{
						CurrentPath = customSaveName;
						saveName = Path.GetFileName(customSaveName);
					}
				}
				snapshot.TargetDir = CurrentPath;
				snapshot.SavingDir = Path.Combine(snapshot.TargetDir, ".new");
				try
				{
					MySandboxGame.Log.WriteLine("Making world state snapshot.");
					LogMemoryUsage("Before snapshot.");
					snapshot.CheckpointSnapshot = GetCheckpoint(saveName);
					snapshot.SectorSnapshot = GetSector();
					snapshot.CompressedVoxelSnapshots = VoxelMaps.GetVoxelMapsData(includeChanged: true, compressed: true);
					snapshot.VicinityGatherTask = GatherVicinityInformation(snapshot.CheckpointSnapshot);
					Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = new Dictionary<string, VRage.Game.Voxels.IMyStorage>();
					snapshot.VoxelSnapshots = VoxelMaps.GetVoxelMapsData(includeChanged: true, compressed: false, voxelStorageNameCache);
					snapshot.VoxelStorageNameCache = voxelStorageNameCache;
					LogMemoryUsage("After snapshot.");
					SaveDataComponents();
				}
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLine(ex);
					m_isSaveInProgress = false;
					return false;
				}
				finally
				{
					SaveEnded();
				}
				LogMemoryUsage("Directory cleanup");
			}
			MySandboxGame.Log.WriteLine("Saving world - END");
			m_isSaveInProgress = false;
			return true;
		}

		public void SaveEnded()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnServerSaving, arg2: false);
			}
		}

		public void SaveDataComponents()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				SaveComponent(value);
			}
		}

		private void SaveComponent(MySessionComponentBase component)
		{
			component.SaveData();
		}

		public MyObjectBuilder_World GetWorld(bool includeEntities = true, bool isClientRequest = false)
		{
			return new MyObjectBuilder_World
			{
				Checkpoint = GetCheckpoint(Name, isClientRequest),
				Sector = GetSector(includeEntities),
				VoxelMaps = (includeEntities ? new SerializableDictionary<string, byte[]>(Static.GetVoxelMapsArray(includeChanged: false)) : new SerializableDictionary<string, byte[]>())
			};
		}

		public MyObjectBuilder_Sector GetSector(bool includeEntities = true)
		{
			MyObjectBuilder_Sector myObjectBuilder_Sector = null;
			myObjectBuilder_Sector = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Sector>();
			if (includeEntities)
			{
				myObjectBuilder_Sector.SectorObjects = MyEntities.Save();
			}
			myObjectBuilder_Sector.SectorEvents = MyGlobalEvents.GetGlobalEventsObjectBuilder();
			myObjectBuilder_Sector.Environment = MySector.GetEnvironmentSettings();
			myObjectBuilder_Sector.AppVersion = MyFinalBuildConstants.APP_VERSION;
			return myObjectBuilder_Sector;
		}

		public MyObjectBuilder_Checkpoint GetCheckpoint(string saveName, bool isClientRequest = false)
		{
			MatrixD matrix = MatrixD.Invert(MySpectatorCameraController.Static.GetViewMatrix());
			MyCameraControllerEnum cameraControllerEnum = GetCameraControllerEnum();
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Checkpoint>();
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = MyObjectBuilderSerializer.Clone(Settings) as MyObjectBuilder_SessionSettings;
			myObjectBuilder_SessionSettings.ScenarioEditMode = myObjectBuilder_SessionSettings.ScenarioEditMode || PersistentEditMode;
			myObjectBuilder_Checkpoint.SessionName = saveName;
			myObjectBuilder_Checkpoint.Description = Description;
			myObjectBuilder_Checkpoint.Password = Password;
			myObjectBuilder_Checkpoint.LastSaveTime = DateTime.Now;
			myObjectBuilder_Checkpoint.WorkshopId = WorkshopId;
			myObjectBuilder_Checkpoint.ElapsedGameTime = ElapsedGameTime.Ticks;
			myObjectBuilder_Checkpoint.InGameTime = InGameTime;
			myObjectBuilder_Checkpoint.Settings = myObjectBuilder_SessionSettings;
			myObjectBuilder_Checkpoint.Mods = Mods;
			myObjectBuilder_Checkpoint.CharacterToolbar = MyToolbarComponent.CharacterToolbar.GetObjectBuilder();
			myObjectBuilder_Checkpoint.Scenario = Scenario.Id;
			myObjectBuilder_Checkpoint.WorldBoundaries = WorldBoundaries;
			myObjectBuilder_Checkpoint.PreviousEnvironmentHostility = PreviousEnvironmentHostility;
			myObjectBuilder_Checkpoint.RequiresDX = RequiresDX;
			myObjectBuilder_Checkpoint.CustomLoadingScreenImage = CustomLoadingScreenImage;
			myObjectBuilder_Checkpoint.CustomLoadingScreenText = CustomLoadingScreenText;
			myObjectBuilder_Checkpoint.CustomSkybox = CustomSkybox;
			myObjectBuilder_Checkpoint.GameDefinition = GameDefinition.Id;
			myObjectBuilder_Checkpoint.SessionComponentDisabled = SessionComponentDisabled;
			myObjectBuilder_Checkpoint.SessionComponentEnabled = SessionComponentEnabled;
			myObjectBuilder_Checkpoint.SharedToolbar = SharedToolbar;
			myObjectBuilder_Checkpoint.PromotedUsers = null;
			myObjectBuilder_Checkpoint.RemoteAdminSettings = null;
			myObjectBuilder_Checkpoint.CreativeTools = null;
			Sync.Players.SavePlayers(myObjectBuilder_Checkpoint, RemoteAdminSettings, PromotedUsers, CreativeTools);
			Toolbars.SaveToolbars(myObjectBuilder_Checkpoint);
			Cameras.SaveCameraCollection(myObjectBuilder_Checkpoint);
			Gpss.SaveGpss(myObjectBuilder_Checkpoint);
			if (MyFakes.SHOW_FACTIONS_GUI)
			{
				myObjectBuilder_Checkpoint.Factions = Factions.GetObjectBuilder();
			}
			else
			{
				myObjectBuilder_Checkpoint.Factions = null;
			}
			myObjectBuilder_Checkpoint.Identities = Sync.Players.SaveIdentities();
			myObjectBuilder_Checkpoint.RespawnCooldowns = new List<MyObjectBuilder_Checkpoint.RespawnCooldownItem>();
			Sync.Players.RespawnComponent.SaveToCheckpoint(myObjectBuilder_Checkpoint);
			myObjectBuilder_Checkpoint.ControlledEntities = Sync.Players.SerializeControlledEntities();
			myObjectBuilder_Checkpoint.SpectatorPosition = new MyPositionAndOrientation(ref matrix);
			myObjectBuilder_Checkpoint.SpectatorSpeed = new SerializableVector2(MySpectatorCameraController.Static.SpeedModeLinear, MySpectatorCameraController.Static.SpeedModeAngular);
			myObjectBuilder_Checkpoint.SpectatorIsLightOn = MySpectatorCameraController.Static.IsLightOn;
			myObjectBuilder_Checkpoint.SpectatorDistance = (float)MyThirdPersonSpectator.Static.GetViewerDistance();
			myObjectBuilder_Checkpoint.CameraController = cameraControllerEnum;
			if (cameraControllerEnum == MyCameraControllerEnum.Entity)
			{
				myObjectBuilder_Checkpoint.CameraEntity = ((MyEntity)CameraController).EntityId;
			}
			if (ControlledEntity != null)
			{
				myObjectBuilder_Checkpoint.ControlledObject = ControlledEntity.Entity.EntityId;
				if (!(ControlledEntity is MyCharacter))
				{
				}
			}
			else
			{
				myObjectBuilder_Checkpoint.ControlledObject = -1L;
			}
			myObjectBuilder_Checkpoint.AppVersion = MyFinalBuildConstants.APP_VERSION;
			if (isClientRequest)
			{
				myObjectBuilder_Checkpoint.Clients = SaveMembers();
			}
			myObjectBuilder_Checkpoint.NonPlayerIdentities = Sync.Players.SaveNpcIdentities();
			SaveSessionComponentObjectBuilders(myObjectBuilder_Checkpoint, isClientRequest);
			myObjectBuilder_Checkpoint.ScriptManagerData = ScriptManager.GetObjectBuilder();
			if (this.OnSavingCheckpoint != null)
			{
				this.OnSavingCheckpoint(myObjectBuilder_Checkpoint);
			}
			return myObjectBuilder_Checkpoint;
		}

		public static void RequestVicinityCache(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestVicinityInformation, entityId);
		}

<<<<<<< HEAD
		[Event(null, 3919)]
=======
		[Event(null, 3597)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnRequestVicinityInformation(long entityId)
		{
			SendVicinityInformation(entityId, MyEventContext.Current.Sender);
		}

		public static void SendVicinityInformation(long entityId, EndpointId client)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			if (entityById == null)
			{
				return;
			}
			BoundingSphereD bs = new BoundingSphereD(entityById.PositionComp.WorldMatrixRef.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
			HashSet<string> voxelMaterials = new HashSet<string>();
			HashSet<string> models = new HashSet<string>();
			HashSet<string> armorModels = new HashSet<string>();
			Static.GatherVicinityInformation(bs, voxelMaterials, models, armorModels, delegate
			{
<<<<<<< HEAD
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnVicinityInformation, voxelMaterials.ToList(), models.ToList(), armorModels.ToList(), client);
			});
		}

		[Event(null, 3945)]
=======
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnVicinityInformation, Enumerable.ToList<string>((IEnumerable<string>)voxelMaterials), Enumerable.ToList<string>((IEnumerable<string>)models), Enumerable.ToList<string>((IEnumerable<string>)armorModels), client);
			});
		}

		[Event(null, 3620)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void OnVicinityInformation(List<string> voxels, List<string> models, List<string> armorModels)
		{
			PreloadVicinityCache(voxels, models, armorModels);
		}

		private static void PreloadVicinityCache(List<string> voxels, List<string> models, List<string> armorModels)
		{
			if (voxels != null && voxels.Count > 0)
			{
				byte[] array = new byte[voxels.Count];
				int num = 0;
				foreach (string voxel in voxels)
				{
					if (voxel != null)
					{
						MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxel);
						if (voxelMaterialDefinition != null)
						{
							array[num++] = voxelMaterialDefinition.Index;
						}
						else
						{
							array[num++] = 0;
						}
					}
				}
				MyRenderProxy.PreloadVoxelMaterials(array);
			}
			if (models != null && models.Count > 0)
			{
				MyRenderProxy.PreloadModels(models, forInstancedComponent: true);
			}
			if (armorModels != null && armorModels.Count > 0)
			{
				MyRenderProxy.PreloadModels(armorModels, forInstancedComponent: false);
			}
		}

		private Task GatherVicinityInformation(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || !MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING)
			{
				return default(Task);
			}
			if (checkpoint.VicinityArmorModelsCache == null)
			{
				checkpoint.VicinityArmorModelsCache = new List<string>();
			}
			else
			{
				checkpoint.VicinityArmorModelsCache.Clear();
			}
			if (checkpoint.VicinityModelsCache == null)
			{
				checkpoint.VicinityModelsCache = new List<string>();
			}
			else
			{
				checkpoint.VicinityModelsCache.Clear();
			}
			if (checkpoint.VicinityVoxelCache == null)
			{
				checkpoint.VicinityVoxelCache = new List<string>();
			}
			else
			{
				checkpoint.VicinityVoxelCache.Clear();
			}
			if (LocalCharacter != null)
			{
				BoundingSphereD bs = new BoundingSphereD(LocalCharacter.WorldMatrix.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
				HashSet<string> voxelMaterials = new HashSet<string>();
				HashSet<string> models = new HashSet<string>();
				HashSet<string> armorModels = new HashSet<string>();
				return GatherVicinityInformation(bs, voxelMaterials, models, armorModels, delegate
				{
					if (LocalCharacter.CurrentWeapon != null)
					{
						checkpoint.VicinityArmorModelsCache.Add(LocalCharacter.CurrentWeapon.PhysicalItemDefinition.Model);
					}
					MyDefinitionManager.Static.Characters.TryGetValue(LocalCharacter.ModelName, out var result);
					if (!string.IsNullOrEmpty(result.Model))
					{
						checkpoint.VicinityArmorModelsCache.Add(result.Model);
					}
<<<<<<< HEAD
					checkpoint.VicinityArmorModelsCache.AddRange(armorModels);
					checkpoint.VicinityModelsCache.AddRange(models);
					checkpoint.VicinityVoxelCache.AddRange(voxelMaterials);
=======
					checkpoint.VicinityArmorModelsCache.AddRange((IEnumerable<string>)armorModels);
					checkpoint.VicinityModelsCache.AddRange((IEnumerable<string>)models);
					checkpoint.VicinityVoxelCache.AddRange((IEnumerable<string>)voxelMaterials);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				});
			}
			return default(Task);
		}

		public Task GatherVicinityInformation(BoundingSphereD bs, HashSet<string> voxelMaterials, HashSet<string> models, HashSet<string> armorModels, Action completion)
		{
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref bs, list);
			List<MyVoxelBase> list2 = null;
			foreach (MyEntity item in list)
			{
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				MyVoxelBase myVoxelBase;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.RenderData != null && myCubeGrid.RenderData.Cells != null)
					{
						foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in myCubeGrid.Render.RenderData.Cells)
						{
							if (cell.Value.CubeParts == null)
							{
								continue;
							}
							foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
							{
								AddAllModels(cubePart.Key.Model, armorModels);
							}
						}
					}
					Enumerator<MySlimBlock> enumerator4 = myCubeGrid.CubeBlocks.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							MySlimBlock current3 = enumerator4.get_Current();
							if (current3.FatBlock != null && current3.FatBlock.Model != null)
							{
								AddAllModels(current3.FatBlock.Model, models);
							}
						}
					}
<<<<<<< HEAD
				}
				else if ((myVoxelBase = item as MyVoxelBase) != null && !(myVoxelBase is MyVoxelPhysics))
				{
					list2 = list2 ?? new List<MyVoxelBase>();
					list2.Add(myVoxelBase);
=======
					finally
					{
						((IDisposable)enumerator4).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else if ((myVoxelBase = item as MyVoxelBase) != null && !(myVoxelBase is MyVoxelPhysics))
				{
					list2 = list2 ?? new List<MyVoxelBase>();
					list2.Add(myVoxelBase);
				}
			}
			if (list2 != null)
			{
				return Parallel.Start(new GatherVoxelMaterialsWork(list2, voxelMaterials, bs, completion));
			}
<<<<<<< HEAD
			if (list2 != null)
			{
				return Parallel.Start(new GatherVoxelMaterialsWork(list2, voxelMaterials, bs, completion));
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			completion.InvokeIfNotNull();
			return default(Task);
		}

		private static void GetVoxelMaterials(HashSet<string> voxelMaterials, MyVoxelBase voxel, int lod, Vector3D center, float radius)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			MyShapeSphere shape = new MyShapeSphere
			{
				Center = center,
				Radius = radius
			};
			Enumerator<byte> enumerator = voxel.GetMaterialsInShape(shape, lod).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					byte current = enumerator.get_Current();
					MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(current);
					if (voxelMaterialDefinition != null)
					{
						voxelMaterials.Add(voxelMaterialDefinition.Id.SubtypeName);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void AddAllModels(MyModel model, HashSet<string> models)
		{
			if (!string.IsNullOrEmpty(model.AssetName))
			{
				models.Add(model.AssetName);
			}
		}

		private void SaveSessionComponentObjectBuilders(MyObjectBuilder_Checkpoint checkpoint, bool isClientRequest)
		{
			checkpoint.SessionComponents = new List<MyObjectBuilder_SessionComponent>();
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				if (!isClientRequest || !value.IsServerOnly)
				{
					MyObjectBuilder_SessionComponent objectBuilder = value.GetObjectBuilder();
					if (objectBuilder != null)
					{
						checkpoint.SessionComponents.Add(objectBuilder);
					}
				}
			}
		}

		public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged)
		{
			return VoxelMaps.GetVoxelMapsArray(includeChanged);
		}

		public List<MyObjectBuilder_Planet> GetPlanetObjectBuilders()
		{
			List<MyObjectBuilder_Planet> list = new List<MyObjectBuilder_Planet>();
			foreach (MyVoxelBase instance in VoxelMaps.Instances)
			{
				MyPlanet myPlanet = instance as MyPlanet;
				if (myPlanet != null)
				{
					list.Add(myPlanet.GetObjectBuilder() as MyObjectBuilder_Planet);
				}
			}
			return list;
		}

		internal List<MyObjectBuilder_Client> SaveMembers(bool forceSave = false)
		{
			if (MyMultiplayer.Static == null)
			{
				return null;
			}
			if (!forceSave && Enumerable.Count<ulong>(MyMultiplayer.Static.Members) == 1)
			{
				using IEnumerator<ulong> enumerator = MyMultiplayer.Static.Members.GetEnumerator();
				enumerator.MoveNext();
				if (enumerator.Current == Sync.MyId)
				{
					return null;
				}
			}
			List<MyObjectBuilder_Client> list = new List<MyObjectBuilder_Client>();
			foreach (ulong member in MyMultiplayer.Static.Members)
			{
				MyObjectBuilder_Client myObjectBuilder_Client = new MyObjectBuilder_Client();
				myObjectBuilder_Client.SteamId = member;
				myObjectBuilder_Client.Name = MyMultiplayer.Static.GetMemberName(member);
				myObjectBuilder_Client.IsAdmin = Static.IsUserAdmin(member);
				myObjectBuilder_Client.ClientService = MyMultiplayer.Static.GetMemberServiceName(member);
				list.Add(myObjectBuilder_Client);
			}
			return list;
		}

		public void GameOver()
		{
			GameOver(MyCommonTexts.MP_YouHaveBeenKilled);
		}

		public void GameOver(MyStringId? customMessage)
		{
		}

		public void Unload()
		{
			IsUnloading = true;
			MySession.OnUnloading.InvokeIfNotNull();
<<<<<<< HEAD
			Factions.FactionStateChanged -= OnFactionsStateChanged;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGuiScreenProgress myGuiScreenProgress = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_UnloadingWorld));
			MyScreenManager.AddScreenNow(myGuiScreenProgress);
			myGuiScreenProgress.DrawPaused();
			try
<<<<<<< HEAD
			{
				bool flag;
				do
				{
					flag = Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(16.0));
					try
					{
						Parallel.RunCallbacks();
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine("Exception occurred while unloading session");
						MyLog.Default.WriteLine(ex);
					}
					MyVRage.Platform.Update();
					MyGameService.Update();
				}
				while (!(flag & !MySandboxGame.Static.ProcessInvoke()));
				MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase)(((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()) ?? ((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>())), isUnloading: true);
				MyGuiSandbox.Update(16);
				MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase)(((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()) ?? ((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>())), isUnloading: true);
				MySandboxGame.IsPaused = false;
				if (MyHud.RotatingWheelVisible)
				{
					MyHud.PopRotatingWheelVisible();
				}
				Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = false;
				MySpectatorCameraController.Static.CleanLight();
				if (MySpaceAnalytics.Instance != null)
				{
					MySpaceAnalytics.Instance.ReportWorldEnd();
				}
				MySandboxGame.Log.WriteLine("MySession::Unload START");
				MySessionSnapshot.WaitForSaving();
				MySandboxGame.Log.WriteLine("AutoSaveInMinutes: " + AutoSaveInMinutes);
				MySandboxGame.Log.WriteLine("MySandboxGame.IsDedicated: " + Sandbox.Engine.Platform.Game.IsDedicated);
				MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer);
				if (Sandbox.Engine.Platform.Game.IsDedicated && MySandboxGame.ConfigDedicated.RestartSave)
				{
					MySandboxGame.Log.WriteLineAndConsole("Autosave in unload");
					Save();
				}
				MySandboxGame.Static.ClearInvokeQueue();
				MyDroneAIDataStatic.Reset();
				MyAudio.Static.StopUpdatingAll3DCues();
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MyAudio.Static.ChangeGlobalVolume(1f, 0f);
				MyAudio.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
				MyEntity3DSoundEmitter.LastTimePlaying.Clear();
				MyEntity3DSoundEmitter.ClearEntityEmitters();
				Ready = false;
				VoxelMaps.Clear();
				MySandboxGame.Config.Save();
				if (LocalHumanPlayer != null)
				{
					MyShipController myShipController;
					if (LocalHumanPlayer.Character?.CurrentRemoteControl != null && (myShipController = LocalHumanPlayer.Character?.CurrentRemoteControl as MyShipController) != null)
					{
						myShipController.Use();
					}
					if (LocalHumanPlayer.Controller != null)
					{
						LocalHumanPlayer.Controller.SaveCamera();
					}
				}
				MyRenderProxy.ClearPersistentBillboards();
				DisconnectMultiplayer();
				UnloadDataComponents();
				UnloadMultiplayer();
				MyTerminalControlFactory.Unload();
				MyDefinitionManager.Static.UnloadData();
				MyDefinitionManager.Static.PreloadDefinitions();
				if (!Sync.IsDedicated)
				{
					MyGuiManager.InitFonts();
				}
				MyInput.Static.ClearBlacklist();
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					(EmptyKeys.UserInterface.Engine.Instance.AssetManager as MyAssetManager)?.UnloadGeneratedTextures();
				}
				MyDefinitionErrors.Clear();
				MyResourceDistributorComponent.Clear();
				MyRenderProxy.UnloadData();
				MyHud.Questlog.CleanDetails();
				MyHud.Questlog.Visible = false;
				MyAPIGateway.Clean();
				MyOxygenProviderSystem.ClearOxygenGenerators();
				MyDynamicAABBTree.Dispose();
				MyDynamicAABBTreeD.Dispose();
				MyParticleEffectsLibrary.Close();
				Factions.Clear();
				if (MyVRage.Platform.System.IsMemoryLimited)
				{
					MyModels.UnloadModdedModels();
				}
				MyUseObjectFactory.RestoreBaseGamePreset();
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
				MySandboxGame.Log.WriteLine("MySession::Unload END");
				if (MyCubeBuilder.AllPlayersColors != null)
				{
					MyCubeBuilder.AllPlayersColors.Clear();
				}
				MySession.OnUnloaded.InvokeIfNotNull();
				MyVRage.Platform.System.OnSessionUnloaded();
				Parallel.Scheduler.WaitForTasksToFinish(new TimeSpan(-1L));
				Parallel.Clean();
				if (HkHandle.GetHandlesAmount() > 0)
				{
					MyLog.Default.Error($"Shouldn't have any unclosed handles. Closing HkHandles {HkHandle.GetHandlesAmount()}");
				}
				MyCharacter.ResetOnCharacterDiedEvent();
			}
			finally
			{
=======
			{
				bool flag;
				do
				{
					flag = Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(16.0));
					try
					{
						Parallel.RunCallbacks();
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine("Exception occurred while unloading session");
						MyLog.Default.WriteLine(ex);
					}
					MyVRage.Platform.Update();
					MyGameService.Update();
				}
				while (!(flag & !MySandboxGame.Static.ProcessInvoke()));
				MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase)(((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()) ?? ((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>())), isUnloading: true);
				MyGuiSandbox.Update(16);
				MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase)(((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>()) ?? ((object)MyScreenManager.GetFirstScreenOfType<MyGuiScreenProgress>())), isUnloading: true);
				MySandboxGame.IsPaused = false;
				if (MyHud.RotatingWheelVisible)
				{
					MyHud.PopRotatingWheelVisible();
				}
				Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = false;
				MySpectatorCameraController.Static.CleanLight();
				if (MySpaceAnalytics.Instance != null)
				{
					MySpaceAnalytics.Instance.ReportWorldEnd();
				}
				MySandboxGame.Log.WriteLine("MySession::Unload START");
				MySessionSnapshot.WaitForSaving();
				MySandboxGame.Log.WriteLine("AutoSaveInMinutes: " + AutoSaveInMinutes);
				MySandboxGame.Log.WriteLine("MySandboxGame.IsDedicated: " + Sandbox.Engine.Platform.Game.IsDedicated);
				MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer);
				if ((SaveOnUnloadOverride.HasValue && SaveOnUnloadOverride.Value) || (!SaveOnUnloadOverride.HasValue && AutoSaveInMinutes != 0 && Sandbox.Engine.Platform.Game.IsDedicated))
				{
					MySandboxGame.Log.WriteLineAndConsole("Autosave in unload");
					Save();
				}
				MySandboxGame.Static.ClearInvokeQueue();
				MyDroneAIDataStatic.Reset();
				MyAudio.Static.StopUpdatingAll3DCues();
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MyAudio.Static.ChangeGlobalVolume(1f, 0f);
				MyAudio.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
				MyEntity3DSoundEmitter.LastTimePlaying.Clear();
				MyEntity3DSoundEmitter.ClearEntityEmitters();
				Ready = false;
				VoxelMaps.Clear();
				MySandboxGame.Config.Save();
				if (LocalHumanPlayer != null && LocalHumanPlayer.Controller != null)
				{
					LocalHumanPlayer.Controller.SaveCamera();
				}
				DisconnectMultiplayer();
				UnloadDataComponents();
				UnloadMultiplayer();
				MyTerminalControlFactory.Unload();
				MyDefinitionManager.Static.UnloadData();
				MyDefinitionManager.Static.PreloadDefinitions();
				if (!Sync.IsDedicated)
				{
					MyGuiManager.InitFonts();
				}
				MyInput.Static.ClearBlacklist();
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					(EmptyKeys.UserInterface.Engine.Instance.AssetManager as MyAssetManager)?.UnloadGeneratedTextures();
				}
				MyDefinitionErrors.Clear();
				MyResourceDistributorComponent.Clear();
				MyRenderProxy.UnloadData();
				MyHud.Questlog.CleanDetails();
				MyHud.Questlog.Visible = false;
				MyAPIGateway.Clean();
				MyOxygenProviderSystem.ClearOxygenGenerators();
				MyDynamicAABBTree.Dispose();
				MyDynamicAABBTreeD.Dispose();
				MyParticleEffectsLibrary.Close();
				Factions.Clear();
				if (MyVRage.Platform.System.IsMemoryLimited)
				{
					MyModels.UnloadModdedModels();
				}
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
				MySandboxGame.Log.WriteLine("MySession::Unload END");
				if (MyCubeBuilder.AllPlayersColors != null)
				{
					MyCubeBuilder.AllPlayersColors.Clear();
				}
				MySession.OnUnloaded.InvokeIfNotNull();
				MyVRage.Platform.System.OnSessionUnloaded();
				Parallel.Scheduler.WaitForTasksToFinish(new TimeSpan(-1L));
				Parallel.Clean();
			}
			finally
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myGuiScreenProgress.CloseScreen();
			}
			IsUnloading = false;
		}

		/// <summary>
		/// Initializes faction collection.
		/// </summary>
		private void InitializeFactions()
		{
			Factions.CreateDefaultFactions();
		}

		public static void InitiateDump()
		{
			VRage.Profiler.MyRenderProfiler.SetLevel(-1);
			m_profilerDumpDelay = 60;
		}

		private static ulong GetVoxelsSizeInBytes(string sessionPath)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			ulong num = 0uL;
			string[] files = Directory.GetFiles(sessionPath, "*.vx2", (SearchOption)0);
			for (int i = 0; i < files.Length; i++)
			{
<<<<<<< HEAD
				FileInfo fileInfo = new FileInfo(files[i]);
				num += (ulong)fileInfo.Length;
=======
				FileInfo val = new FileInfo(files[i]);
				num += (ulong)val.get_Length();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return num;
		}

		private void LogMemoryUsage(string msg)
		{
			MySandboxGame.Log.WriteMemoryUsage(msg);
		}

		private void LogSettings(string scenario = null, int asteroidAmount = 0)
		{
			MyLog log = MySandboxGame.Log;
			log.WriteLine("MySession.Static.LogSettings - START", LoggingOptions.SESSION_SETTINGS);
			using (log.IndentUsing(LoggingOptions.SESSION_SETTINGS))
			{
				log.WriteLine("Name = " + Name, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("Description = " + Description, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("GameDateTime = " + GameDateTime, LoggingOptions.SESSION_SETTINGS);
				if (scenario != null)
				{
					log.WriteLine("Scenario = " + scenario, LoggingOptions.SESSION_SETTINGS);
					log.WriteLine("AsteroidAmount = " + asteroidAmount, LoggingOptions.SESSION_SETTINGS);
				}
				log.WriteLine("Password = " + Password, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("CurrentPath = " + CurrentPath, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("WorkshopId = " + WorkshopId, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("CameraController = " + CameraController, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("ThumbPath = " + ThumbPath, LoggingOptions.SESSION_SETTINGS);
				Settings.LogMembers(log, LoggingOptions.SESSION_SETTINGS);
			}
			log.WriteLine("MySession.Static.LogSettings - END", LoggingOptions.SESSION_SETTINGS);
		}

		public bool GetVoxelHandAvailable(MyCharacter character)
		{
			MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(character);
			if (playerFromCharacter == null)
			{
				return false;
			}
			return GetVoxelHandAvailable(playerFromCharacter.Client.SteamUserId);
		}

		public bool GetVoxelHandAvailable(ulong user)
		{
			if (Settings.EnableVoxelHand)
			{
				if (SurvivalMode)
				{
					return CreativeToolsEnabled(user);
				}
				return true;
			}
			return false;
		}

		private void OnFactionsStateChanged(MyFactionStateChange change, long fromFactionId, long toFactionId, long playerId, long sender)
		{
			string text = null;
			if (change == MyFactionStateChange.FactionMemberKick && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenKickedFromFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberAcceptJoin && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenAcceptedToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouCannotJoinToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && LocalPlayerId == sender)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextApplicantCannotJoinToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberAcceptJoin && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity != null)
				{
					string displayName = myIdentity.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerJoined), displayName);
				}
			}
			else if (change == MyFactionStateChange.FactionMemberLeave && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity2 = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity2 != null)
				{
					string displayName2 = myIdentity2.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerLeft), displayName2);
				}
			}
			else if (change == MyFactionStateChange.FactionMemberSendJoin && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity3 = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity3 != null)
				{
					string displayName3 = myIdentity3.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerApplied), displayName3);
				}
			}
			if (text != null)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), text);
			}
		}

		private static IMyCamera GetMainCamera()
		{
			return MySector.MainCamera;
		}

		private static BoundingBoxD GetWorldBoundaries()
		{
			if (Static == null || !Static.WorldBoundaries.HasValue)
			{
				return default(BoundingBoxD);
			}
			return Static.WorldBoundaries.Value;
		}

		private static Vector3D GetLocalPlayerPosition()
		{
			if (Static != null && Static.LocalHumanPlayer != null)
			{
				return Static.LocalHumanPlayer.GetPosition();
			}
			return default(Vector3D);
		}

		public short GetBlockTypeLimit(string blockType)
		{
			int num = 1;
			switch (BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
				return 0;
			case MyBlockLimitsEnabledEnum.GLOBALLY:
				num = 1;
				break;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				num = 1;
				break;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				num = 1;
				break;
			}
			if (!BlockTypeLimits.TryGetValue(blockType, out var value))
			{
				return 0;
			}
			if (value > 0 && value / num == 0)
			{
				return 1;
			}
			return (short)(value / num);
		}

		private static void RaiseAfterLoading()
		{
			MySession.AfterLoading?.Invoke();
		}

		public LimitResult IsWithinWorldLimits(out string failedBlockType, long ownerID, string blockName, int pcuToBuild, int blocksToBuild = 0, int blocksCount = 0, Dictionary<string, int> blocksPerType = null)
		{
			failedBlockType = null;
			if (BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				return LimitResult.Passed;
			}
			ulong num = Players.TryGetSteamId(ownerID);
			if (num != 0L && Static.IsUserAdmin(num))
			{
				AdminSettingsEnum? adminSettingsEnum = null;
				if (num == Sync.MyId)
				{
					adminSettingsEnum = Static.AdminSettings;
				}
				else if (Static.RemoteAdminSettings.ContainsKey(num))
				{
					adminSettingsEnum = Static.RemoteAdminSettings[num];
				}
				if (((uint?)adminSettingsEnum & 0x40u) != 0)
				{
					return LimitResult.Passed;
				}
			}
			MyIdentity myIdentity = Players.TryGetIdentity(ownerID);
			if (MaxGridSize != 0 && blocksCount + blocksToBuild > MaxGridSize)
			{
				return LimitResult.MaxGridSize;
			}
			if (myIdentity != null)
			{
				MyBlockLimits blockLimits = myIdentity.BlockLimits;
				if (BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && Factions.GetPlayerFaction(myIdentity.IdentityId) == null)
				{
					return LimitResult.NoFaction;
				}
				if (blockLimits != null)
				{
					if (MaxBlocksPerPlayer != 0 && blockLimits.BlocksBuilt + blocksToBuild > blockLimits.MaxBlocks)
					{
						return LimitResult.MaxBlocksPerPlayer;
					}
					if (TotalPCU != 0 && pcuToBuild > blockLimits.PCU)
					{
						return LimitResult.PCU;
					}
					if (blocksPerType != null)
					{
						MyBlockLimits.MyTypeLimitData myTypeLimitData = default(MyBlockLimits.MyTypeLimitData);
						foreach (KeyValuePair<string, short> blockTypeLimit2 in BlockTypeLimits)
						{
							if (blocksPerType.ContainsKey(blockTypeLimit2.Key))
							{
								int num2 = blocksPerType[blockTypeLimit2.Key];
<<<<<<< HEAD
								if (blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit2.Key, out var value))
=======
								if (blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit2.Key, ref myTypeLimitData))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									num2 += myTypeLimitData.BlocksBuilt;
								}
								if (num2 > GetBlockTypeLimit(blockTypeLimit2.Key))
								{
									return LimitResult.BlockTypeLimit;
								}
							}
						}
					}
					else if (blockName != null)
					{
						short blockTypeLimit = GetBlockTypeLimit(blockName);
						if (blockTypeLimit > 0)
						{
<<<<<<< HEAD
							if (blockLimits.BlockTypeBuilt.TryGetValue(blockName, out var value2))
=======
							MyBlockLimits.MyTypeLimitData myTypeLimitData2 = default(MyBlockLimits.MyTypeLimitData);
							if (blockLimits.BlockTypeBuilt.TryGetValue(blockName, ref myTypeLimitData2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								blocksToBuild += myTypeLimitData2.BlocksBuilt;
							}
							if (blocksToBuild > blockTypeLimit)
							{
								return LimitResult.BlockTypeLimit;
							}
						}
					}
				}
			}
			return LimitResult.Passed;
		}

		public bool CheckLimitsAndNotify(long ownerID, string blockName, int pcuToBuild, int blocksToBuild = 0, int blocksCount = 0, Dictionary<string, int> blocksPerType = null)
		{
			string failedBlockType;
			LimitResult limitResult = IsWithinWorldLimits(out failedBlockType, ownerID, blockName, pcuToBuild, blocksToBuild, blocksCount, blocksPerType);
			if (limitResult != 0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				MyHud.Notifications.Add(GetNotificationForLimitResult(limitResult));
				return false;
			}
			return true;
		}

		public static MyNotificationSingletons GetNotificationForLimitResult(LimitResult result)
		{
			return result switch
			{
				LimitResult.MaxGridSize => MyNotificationSingletons.LimitsGridSize, 
				LimitResult.NoFaction => MyNotificationSingletons.LimitsNoFaction, 
				LimitResult.BlockTypeLimit => MyNotificationSingletons.LimitsPerBlockType, 
				LimitResult.MaxBlocksPerPlayer => MyNotificationSingletons.LimitsPlayer, 
				LimitResult.PCU => MyNotificationSingletons.LimitsPCU, 
				_ => MyNotificationSingletons.LimitsPCU, 
			};
		}

		public bool CheckResearchAndNotify(long identityId, MyDefinitionId id)
		{
			if (Static.Settings.EnableResearch && !MySessionComponentResearch.Static.CanUse(identityId, id) && !Static.CreativeMode && !Static.CreativeToolsEnabled(Static.Players.TryGetSteamId(identityId)))
			{
				if (Static.LocalCharacter != null && identityId == Static.LocalCharacter.GetPlayerIdentityId())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.BlockNotResearched);
				}
				return false;
			}
			return true;
		}

		public bool CheckDLCAndNotify(MyDefinitionBase definition)
		{
			MyHudNotificationBase myHudNotificationBase = MyHud.Notifications.Get(MyNotificationSingletons.MissingDLC);
			MyDLCs.MyDLC firstMissingDefinitionDLC = GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
			if (firstMissingDefinitionDLC == null)
			{
				return true;
			}
			myHudNotificationBase.SetTextFormatArguments(MyTexts.Get(firstMissingDefinitionDLC.DisplayName));
			MyHud.Notifications.Add(myHudNotificationBase);
			return false;
		}

		private void OnCameraEntityClosing(MyEntity entity)
		{
			SetCameraController(MyCameraControllerEnum.Spectator);
		}

		public static void PerformPlatformPatchBeforeLoad(MyObjectBuilder_Checkpoint checkpoint, MyGameModeEnum? forceGameMode, MyOnlineModeEnum? forceOnlineMode)
		{
			if (checkpoint != null)
			{
				PerformPlatformPatchBeforeLoad(checkpoint.Settings, forceGameMode, forceOnlineMode);
				if (!MyPlatformGameSettings.IsModdingAllowed)
				{
					checkpoint.Mods.Clear();
				}
			}
		}

		public static void PerformPlatformPatchBeforeLoad(MyObjectBuilder_WorldConfiguration worldConfig, MyGameModeEnum? forceGameMode, MyOnlineModeEnum? forceOnlineMode)
		{
			if (worldConfig != null)
			{
				PerformPlatformPatchBeforeLoad(worldConfig.Settings, forceGameMode, forceOnlineMode);
				if (!MyPlatformGameSettings.IsModdingAllowed)
				{
					worldConfig.Mods.Clear();
				}
			}
		}

		public static void PerformPlatformPatchBeforeLoad(MyObjectBuilder_SessionSettings settings, MyGameModeEnum? forceGameMode, MyOnlineModeEnum? forceOnlineMode)
		{
			FixIncorrectSettings(settings);
			if (forceGameMode.HasValue)
			{
				settings.GameMode = forceGameMode.Value;
			}
			if (forceOnlineMode.HasValue)
			{
				settings.OnlineMode = forceOnlineMode.Value;
			}
			if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
			{
				settings.MaxPlanets = Math.Min(settings.MaxPlanets, 3);
				settings.PredefinedAsteroids = false;
				settings.UseConsolePCU = true;
				settings.VoxelGeneratorVersion = 5;
				settings.EnableContainerDrops = false;
				settings.OffensiveWordsFiltering = true;
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					settings.EnableIngameScripts = false;
					if (!settings.Scenario)
					{
						int? num = ((settings.OnlineMode == MyOnlineModeEnum.OFFLINE) ? MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX : MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX);
						if (num.HasValue)
						{
							int totalPCU = Math.Min(settings.TotalPCU, num.Value);
							if (settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
							{
								totalPCU = num.Value;
								settings.BlockLimitsEnabled = MyBlockLimitsEnabledEnum.GLOBALLY;
							}
							settings.TotalPCU = totalPCU;
						}
					}
				}
			}
			if (MyPlatformGameSettings.FORCED_VOXEL_TRASH_REMOVAL_SETTINGS.HasValue)
			{
				MyPlatformGameSettings.VoxelTrashRemovalSettings value = MyPlatformGameSettings.FORCED_VOXEL_TRASH_REMOVAL_SETTINGS.Value;
				settings.VoxelTrashRemovalEnabled = true;
				settings.VoxelAgeThreshold = value.Age;
				settings.VoxelGridDistanceThreshold = value.MinDistanceFromGrid;
				settings.VoxelPlayerDistanceThreshold = value.MinDistanceFromPlayer;
				if (value.RevertAsteroids)
				{
					settings.TrashFlags |= MyTrashRemovalFlags.RevertAsteroids;
				}
				if (value.RevertCloseToNPCGrids)
				{
					settings.TrashFlags |= MyTrashRemovalFlags.RevertCloseToNPCGrids;
				}
			}
			if (!MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING)
			{
				settings.TrashRemovalEnabled = true;
			}
			Dictionary<string, short> dictionary = settings.BlockTypeLimits.Dictionary;
			ImmutableArray<(string, short)>.Enumerator enumerator = MyPlatformGameSettings.ADDITIONAL_BLOCK_LIMITS.GetEnumerator();
			while (enumerator.MoveNext())
			{
				(string, short) current = enumerator.Current;
				string item = current.Item1;
				short item2 = current.Item2;
				short valueOrDefault = dictionary.GetValueOrDefault(item, short.MaxValue);
				dictionary[item] = Math.Min(item2, valueOrDefault);
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated && settings.OnlineMode != 0 && (settings.MaxPlayers == 0 || settings.MaxPlayers > MyPlatformGameSettings.LOBBY_MAX_PLAYERS))
			{
				settings.MaxPlayers = (short)MyPlatformGameSettings.LOBBY_MAX_PLAYERS;
			}
		}

		public void InvokeLocalPlayerSkinOrColorChanged()
		{
			this.OnLocalPlayerSkinOrColorChanged.InvokeIfNotNull();
		}

<<<<<<< HEAD
		public bool TryGetAdminSettings(ulong steamId, out MyAdminSettingsEnum adminSettings)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				adminSettings = (MyAdminSettingsEnum)value;
				return true;
			}
			adminSettings = MyAdminSettingsEnum.None;
			return false;
		}

		public bool IsUserInvulnerable(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.Invulnerable) != 0;
			}
			return false;
		}

		public bool IsUserShowAllPlayers(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.ShowPlayers) != 0;
			}
			return false;
		}

		public bool IsUserUseAllTerminals(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.UseTerminals) != 0;
			}
			return false;
		}

		public bool IsUserUntargetable(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.Untargetable) != 0;
			}
			return false;
		}

		public bool IsUserKeepOriginalOwnershipOnPaste(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.KeepOriginalOwnershipOnPaste) != 0;
			}
			return false;
		}

		public bool IsUserIgnoreSafeZones(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.IgnoreSafeZones) != 0;
			}
			return false;
		}

		public bool IsUserIgnorePCULimit(ulong steamId)
		{
			if (RemoteAdminSettings.TryGetValue(steamId, out var value))
			{
				return (value & AdminSettingsEnum.IgnorePcu) != 0;
			}
			return false;
		}

		public MyObjectBuilder_Checkpoint.ModItem? GetMod(WorkshopId workshopId)
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in Static.Mods)
			{
				if (mod.PublishedFileId == workshopId.Id && mod.PublishedServiceName == workshopId.ServiceName)
				{
					return mod;
				}
			}
			return null;
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void IMySession.BeforeStartComponents()
		{
			BeforeStartComponents();
		}

		void IMySession.Draw()
		{
			Draw();
		}

		void IMySession.GameOver()
		{
			GameOver();
		}

		void IMySession.GameOver(MyStringId? customMessage)
		{
			GameOver(customMessage);
		}

		MyObjectBuilder_Checkpoint IMySession.GetCheckpoint(string saveName)
		{
			return GetCheckpoint(saveName);
		}

		MyObjectBuilder_Sector IMySession.GetSector()
		{
			return GetSector();
		}

		Dictionary<string, byte[]> IMySession.GetVoxelMapsArray()
		{
			return GetVoxelMapsArray(includeChanged: true);
		}

		MyObjectBuilder_World IMySession.GetWorld()
		{
			return GetWorld();
		}

		bool IMySession.IsPausable()
		{
			return IsPausable();
		}

		void IMySession.RegisterComponent(MySessionComponentBase component, MyUpdateOrder updateOrder, int priority)
		{
			RegisterComponent(component, updateOrder, priority);
		}

		bool IMySession.Save(string customSaveName)
		{
			return Save(customSaveName);
		}

		void IMySession.SetAsNotReady()
		{
			SetAsNotReady();
		}

		void IMySession.SetCameraController(MyCameraControllerEnum cameraControllerEnum, IMyEntity cameraEntity, Vector3D? position)
		{
			SetCameraController(cameraControllerEnum, cameraEntity, position);
		}

		void IMySession.Unload()
		{
			Unload();
		}

		void IMySession.UnloadDataComponents()
		{
			UnloadDataComponents();
		}

		void IMySession.UnloadMultiplayer()
		{
			UnloadMultiplayer();
		}

		void IMySession.UnregisterComponent(MySessionComponentBase component)
		{
			UnregisterComponent(component);
		}

		void IMySession.Update(MyTimeSpan time)
		{
			Update(time);
		}

		void IMySession.UpdateComponents()
		{
			UpdateComponents();
		}

		bool IMySession.IsUserAdmin(ulong steamId)
		{
			return Static.IsUserAdmin(steamId);
		}

		[Obsolete("Use GetUserPromoteLevel")]
		bool IMySession.IsUserPromoted(ulong steamId)
		{
			return Static.IsUserSpaceMaster(steamId);
		}

		MyPromoteLevel IMySession.GetUserPromoteLevel(ulong steamId)
		{
			return GetUserPromoteLevel(steamId);
		}
	}
}
