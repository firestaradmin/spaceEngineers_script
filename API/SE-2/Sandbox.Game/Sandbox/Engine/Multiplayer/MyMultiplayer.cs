using System;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Network;
using VRage.Replication;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[StaticEventOwner]
	public static class MyMultiplayer
	{
		protected sealed class OnTeleport_003C_003ESystem_UInt64_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, ulong, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong userId, in Vector3D location, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnTeleport(userId, location);
			}
		}

		public const int CONTROL_CHANNEL = 0;

		public const int GAME_EVENT_CHANNEL = 2;

		public static byte[] Channels = new byte[2] { 0, 2 };

		public const string HOST_NAME_TAG = "host";

		public const string WORLD_NAME_TAG = "world";

		public const string HOST_STEAM_ID_TAG = "host_steamId";

		public const string WORLD_SIZE_TAG = "worldSize";

		public const string APP_VERSION_TAG = "appVersion";

		public const string GAME_MODE_TAG = "gameMode";

		public const string DATA_HASH_TAG = "dataHash";

		public const string MOD_COUNT_TAG = "mods";

		public const string MOD_ITEM_TAG = "mod";

		public const string VIEW_DISTANCE_TAG = "view";

		public const string INVENTORY_MULTIPLIER_TAG = "inventoryMultiplier";

		public const string BLOCKS_INVENTORY_MULTIPLIER_TAG = "blocksInventoryMultiplier";

		public const string ASSEMBLER_MULTIPLIER_TAG = "assemblerMultiplier";

		public const string REFINERY_MULTIPLIER_TAG = "refineryMultiplier";

		public const string WELDER_MULTIPLIER_TAG = "welderMultiplier";

		public const string GRINDER_MULTIPLIER_TAG = "grinderMultiplier";

		public const string SCENARIO_TAG = "scenario";

		public const string SCENARIO_BRIEFING_TAG = "scenarioBriefing";

		public const string SCENARIO_START_TIME_TAG = "scenarioStartTime";

		public const string EXPERIMENTAL_MODE_TAG = "experimentalMode";

		public const string SESSION_CONFIG_TAG = "sc";

		private static MyReplicationSingle m_replicationOffline;

		public static MyMultiplayerBase Static
		{
			get
			{
				return (MyMultiplayerBase)MyMultiplayerMinimalBase.Instance;
			}
			set
			{
				MyMultiplayerMinimalBase.Instance = value;
			}
		}

		public static MyReplicationLayerBase ReplicationLayer
		{
			get
			{
				if (Static == null)
				{
					InitOfflineReplicationLayer(dispatchRegistration: false);
					return m_replicationOffline;
				}
				return Static.ReplicationLayer;
			}
		}

		public static Task? InitOfflineReplicationLayer(bool dispatchRegistration = true)
		{
			MyReplicationSingle replicationLayer;
			if (m_replicationOffline == null)
			{
				replicationLayer = new MyReplicationSingle(new EndpointId(Sync.MyId));
				m_replicationOffline = replicationLayer;
				if (dispatchRegistration)
				{
					return Parallel.Start(Register, WorkPriority.VeryLow);
				}
				Register();
			}
			return null;
			void Register()
			{
				replicationLayer.RegisterFromGameAssemblies();
			}
		}

		public static MyMultiplayerHostResult HostLobby(MyLobbyType lobbyType, int maxPlayers, MySyncLayer syncLayer)
		{
			MyMultiplayerHostResult ret = new MyMultiplayerHostResult();
			MyMultiplayerLobby mpLobby = null;
			mpLobby = new MyMultiplayerLobby(syncLayer, lobbyType, (uint)maxPlayers, delegate(IMyLobby lobby, bool success, MyLobbyStatusCode reason)
			{
				bool flag = Static != mpLobby;
				if (!ret.Cancelled || flag)
				{
					if (success && (flag || lobby.OwnerId != Sync.MyId))
					{
						success = false;
						lobby.Leave();
					}
					if (success)
					{
						lobby.LobbyType = lobbyType;
						mpLobby.SetLobby(lobby);
						mpLobby.ExperimentalMode = true;
					}
					else if (!flag)
					{
						Static = null;
					}
					ret.RaiseDone(success, reason, Static);
				}
				else
				{
					Static = null;
				}
			});
			Static = mpLobby;
			return ret;
		}

		public static MyMultiplayerJoinResult JoinLobby(ulong lobbyId)
		{
			MyMultiplayerJoinResult ret = new MyMultiplayerJoinResult();
			MyGameService.JoinLobby(lobbyId, delegate(bool success, IMyLobby lobby, MyLobbyStatusCode response)
			{
				if (!ret.Cancelled)
				{
					if (success && response == MyLobbyStatusCode.Success && lobby.OwnerId == Sync.MyId)
					{
						response = MyLobbyStatusCode.DoesntExist;
						lobby.Leave();
					}
					success = success && response == MyLobbyStatusCode.Success;
					ret.RaiseJoined(success, lobby, response, (!success) ? null : (Static = new MyMultiplayerLobbyClient(lobby, new MySyncLayer(new MyTransportLayer(2)))));
				}
			});
			return ret;
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction);
		/// </summary>
		/// <param name="action"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent(Func<IMyEventOwner, Action> action, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, IMyEventOwner>(null, null, action, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg);
		/// </summary>
		/// <param name="arg2"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2>(Func<IMyEventOwner, Action<T2>> action, T2 arg2, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, IMyEventOwner>(null, null, action, arg2, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg2, arg3);
		/// </summary>
		/// <param name="arg3"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2, T3>(Func<IMyEventOwner, Action<T2, T3>> action, T2 arg2, T3 arg3, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, IMyEventOwner>(null, null, action, arg2, arg3, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg2, arg3, arg4);
		/// </summary>
		/// <param name="arg4"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2, T3, T4>(Func<IMyEventOwner, Action<T2, T3, T4>> action, T2 arg2, T3 arg3, T4 arg4, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, IMyEventOwner>(null, null, action, arg2, arg3, arg4, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg2, arg3, arg4, arg5);
		/// </summary>
		/// <param name="arg5"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2, T3, T4, T5>(Func<IMyEventOwner, Action<T2, T3, T4, T5>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, IMyEventOwner>(null, null, action, arg2, arg3, arg4, arg5, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg2, arg3, arg4, arg5, arg6);
		/// </summary>
		/// <param name="arg6"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2, T3, T4, T5, T6>(Func<IMyEventOwner, Action<T2, T3, T4, T5, T6>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, T6, IMyEventOwner>(null, null, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint, position);
		}

		/// <summary>
		/// Raises static multiplayer event.
		/// Usage: MyMultiplayer.RaiseStaticEvent(s =&gt; MyClass.MyStaticFunction, arg2, arg3, arg4, arg5, arg6);
		/// </summary>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		/// <param name="arg6"></param>
		/// <param name="arg7"></param>
		/// <param name="position"></param>
		public static void RaiseStaticEvent<T2, T3, T4, T5, T6, T7>(Func<IMyEventOwner, Action<T2, T3, T4, T5, T6, T7>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, EndpointId targetEndpoint = default(EndpointId), Vector3D? position = null)
		{
			ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, T6, T7, IMyEventOwner>(null, null, action, arg2, arg3, arg4, arg5, arg6, arg7, targetEndpoint, position);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="arg1"></param>
		public static void RaiseEvent<T1>(T1 arg1, Func<T1, Action> action, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, IMyEventOwner>(arg1, null, action, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		public static void RaiseEvent<T1, T2>(T1 arg1, Func<T1, Action<T2>> action, T2 arg2, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, IMyEventOwner>(arg1, null, action, arg2, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		public static void RaiseEvent<T1, T2, T3>(T1 arg1, Func<T1, Action<T2, T3>> action, T2 arg2, T3 arg3, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, T3, IMyEventOwner>(arg1, null, action, arg2, arg3, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		public static void RaiseEvent<T1, T2, T3, T4>(T1 arg1, Func<T1, Action<T2, T3, T4>> action, T2 arg2, T3 arg3, T4 arg4, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, T3, T4, IMyEventOwner>(arg1, null, action, arg2, arg3, arg4, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg5"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg1"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		public static void RaiseEvent<T1, T2, T3, T4, T5>(T1 arg1, Func<T1, Action<T2, T3, T4, T5>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, IMyEventOwner>(arg1, null, action, arg2, arg3, arg4, arg5, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg6"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="arg1"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		public static void RaiseEvent<T1, T2, T3, T4, T5, T6>(T1 arg1, Func<T1, Action<T2, T3, T4, T5, T6>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6, IMyEventOwner>(arg1, null, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint);
		}

		/// <summary>
		/// Raises multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="arg7"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		/// <param name="arg5"></param>
		/// <param name="arg6"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		public static void RaiseEvent<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, Func<T1, Action<T2, T3, T4, T5, T6, T7>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6, T7, IMyEventOwner>(arg1, null, action, arg2, arg3, arg4, arg5, arg6, arg7, targetEndpoint);
		}

		/// <summary>
		/// Raises blocking multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="arg6"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		public static void RaiseBlockingEvent<T1, T2, T3, T4, T5, T6>(T1 arg1, T6 arg6, Func<T1, Action<T2, T3, T4, T5>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner where T6 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent(arg1, arg6, action, arg2, arg3, arg4, arg5, targetEndpoint);
		}

		/// <summary>
		/// Raises blocking multiplayer event.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="arg7"></param>
		/// <param name="action"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		/// <param name="arg6"></param>
		/// <param name="targetEndpoint">Target of the event. When broadcasting, it's exclude endpoint.</param>
		public static void RaiseBlockingEvent<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T7 arg7, Func<T1, Action<T2, T3, T4, T5, T6>> action, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, EndpointId targetEndpoint = default(EndpointId)) where T1 : IMyEventOwner where T7 : IMyEventOwner
		{
			ReplicationLayer.RaiseEvent(arg1, arg7, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint);
		}

		internal static MyReplicationServer GetReplicationServer()
		{
			if (Static != null)
			{
				return Static.ReplicationLayer as MyReplicationServer;
			}
			return null;
		}

		internal static MyReplicationClient GetReplicationClient()
		{
			if (Static != null)
			{
				return Static.ReplicationLayer as MyReplicationClient;
			}
			return null;
		}

		public static void ReplicateImmediatelly(IMyReplicable replicable, IMyReplicable dependency = null)
		{
			GetReplicationServer()?.ForceReplicable(replicable, dependency);
		}

		public static void RemoveForClientIfIncomplete(IMyEventProxy obj)
		{
			GetReplicationServer()?.RemoveForClientIfIncomplete(obj);
		}

		public static void TeleportControlledEntity(Vector3D location)
		{
			RaiseStaticEvent((IMyEventOwner x) => OnTeleport, MySession.Static.LocalHumanPlayer.Id.SteamId, location);
		}

<<<<<<< HEAD
		[Event(null, 479)]
=======
		[Event(null, 443)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnTeleport(ulong userId, Vector3D location)
		{
			if (Sync.IsValidEventOnServer && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				MyEventContext.ValidationFailed();
				return;
			}
			MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(userId));
			if (playerById.Controller.ControlledEntity == null)
			{
				return;
			}
			MyCharacter myCharacter;
			if ((myCharacter = playerById.Controller.ControlledEntity as MyCharacter) != null)
			{
				if (myCharacter.IsOnLadder)
				{
					myCharacter.GetOffLadder();
				}
				MyCockpit myCockpit;
				if ((myCockpit = myCharacter.IsUsing as MyCockpit) != null)
				{
					myCockpit.RemovePilot();
				}
			}
			MyEntity topMostParent = playerById.Controller.ControlledEntity.Entity.GetTopMostParent();
			MatrixD worldMatrix = topMostParent.WorldMatrix;
			worldMatrix.Translation = location;
			topMostParent.Teleport(worldMatrix);
		}

		/// <summary>
		/// Gets multiplayer statistics in formatted string. Use only for Debugging.
		/// </summary>
		/// <returns>Formatted multiplayer statistics.</returns>
		public static string GetMultiplayerStats()
		{
			if (Static != null)
			{
				return Static.ReplicationLayer.GetMultiplayerStat();
			}
			return string.Empty;
		}
	}
}
