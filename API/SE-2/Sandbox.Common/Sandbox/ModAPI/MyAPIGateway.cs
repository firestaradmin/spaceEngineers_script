using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// This is entry point for entire scripting possibilities in game
	/// </summary>
	public static class MyAPIGateway
	{
		/// <summary>
		/// Event triggered on gui control created.
		/// </summary>
		[Obsolete("Use IMyGui.GuiControlCreated")]
		public static Action<object> GuiControlCreated;

		/// <summary>
		/// IMyPlayerCollection contains all players that are in world 
		/// </summary>
		public static IMyPlayerCollection Players;

		/// <summary>
		/// IMyCubeBuilder represents building hand 
		/// </summary>
		public static IMyCubeBuilder CubeBuilder;

		/// <summary>
		/// IMyTerminalActionsHelper is helper for terminal actions and allows to access terminal 
		/// </summary>
		public static IMyTerminalActionsHelper TerminalActionsHelper;

		/// <summary>
		/// IMyTerminalControls allows access to adding and removing controls from a block's terminal screen
		/// </summary>
		public static IMyTerminalControls TerminalControls;

		/// <summary>
		/// IMyUtilities contains mainly I/O, serialization, mod interaction functions
		/// </summary>
		public static IMyUtilities Utilities;

		/// <summary>
		/// IMyMultiplayer  contains multiplayer related things
		/// </summary>
		public static IMyMultiplayer Multiplayer;

		/// <summary>
		/// IMyParallelTask allows to run tasks on background threads 
		/// </summary>
		public static IMyParallelTask Parallel;

		/// <summary>
		/// IMyPhysics contains physics related things (CastRay, etc.)
		/// </summary>
		public static IMyPhysics Physics;

		/// <summary>
		/// IMyGui exposes some useful values from the GUI systems
		/// </summary>
		public static IMyGui Gui;

		/// <summary>
		/// Allows you spawn prefabs 
		/// </summary>
		public static IMyPrefabManager PrefabManager;

		/// <summary>
		/// Provides mod access to control compilation of ingame scripts
		/// </summary>
		public static IMyIngameScripting IngameScripting;

		/// <summary>
		/// IMyInput allows accessing direct input device states
		/// </summary>
		public static IMyInput Input;

		/// <summary>
		/// IMyContractSystem allows you add or edit contracts
		/// </summary>
		public static IMyContractSystem ContractSystem;

		private static IMyEntities m_entitiesStorage;

		private static IMySession m_sessionStorage;

		/// <summary>
		/// Provides access to the Grid Group system
		/// </summary>
		public static IMyGridGroups GridGroups;

		public static IMyReflection Reflection;

		public static IMySpectatorTools SpectatorTools;

		/// <summary>
		/// Allows you to use some reflection tools
		/// </summary>
		public static IMyReflection Reflection;

		/// <summary>
		/// Gives you access to spectator tools. 
		/// </summary>
		public static IMySpectatorTools SpectatorTools;

		/// <summary>
		/// Interface for controlling projectile behavior
		/// </summary>
		public static IMyProjectiles Projectiles;

		/// <summary>
		/// Interface for controlling missiles
		/// </summary>
		public static IMyMissiles Missiles;

		/// <summary>
		/// Provides access for checking installation state of DLCs, and if DLC required by a definition is present.
		/// </summary>
		public static IMyDLCs DLC;

		/// <summary>
		/// IMySession represents session object e.g. current world and its settings
		/// </summary>
		public static IMySession Session
		{
			get
			{
				return m_sessionStorage;
			}
			set
			{
				m_sessionStorage = value;
			}
		}

		/// <summary>
		/// IMyEntities represents all objects that currently in world 
		/// </summary>
		public static IMyEntities Entities
		{
			get
			{
				return m_entitiesStorage;
			}
			set
			{
				m_entitiesStorage = value;
				if (Entities != null)
				{
					MyAPIGatewayShortcuts.RegisterEntityUpdate = Entities.RegisterForUpdate;
					MyAPIGatewayShortcuts.UnregisterEntityUpdate = Entities.UnregisterForUpdate;
				}
				else
				{
					MyAPIGatewayShortcuts.RegisterEntityUpdate = null;
					MyAPIGatewayShortcuts.UnregisterEntityUpdate = null;
				}
			}
		}

		[Conditional("DEBUG")]
		[Obsolete]
		public static void GetMessageBoxPointer(ref IntPtr pointer)
		{
			IntPtr hModule = LoadLibrary("user32.dll");
			pointer = GetProcAddress(hModule, "MessageBoxW");
		}

		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string dllname);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

		[Obsolete]
		public static void Clean()
		{
			Session = null;
			Entities = null;
			Players = null;
			CubeBuilder = null;
			if (IngameScripting != null)
			{
				IngameScripting.Clean();
			}
			IngameScripting = null;
			TerminalActionsHelper = null;
			Utilities = null;
			Parallel = null;
			Physics = null;
			Multiplayer = null;
			PrefabManager = null;
			Input = null;
			TerminalControls = null;
			GridGroups = null;
		}

		[Obsolete]
		public static StringBuilder DoorBase(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in name)
			{
				if (c == ' ')
				{
					stringBuilder.Append(c);
				}
				byte b = (byte)c;
				for (int j = 0; j < 8; j++)
				{
					stringBuilder.Append(((b & 0x80u) != 0) ? "Door" : "Base");
					b = (byte)(b << 1);
				}
			}
			return stringBuilder;
		}
	}
}
