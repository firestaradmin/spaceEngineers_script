using System;
using System.Collections.Generic;
using System.Threading;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI.Physics;
using VRage;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.ModAPI
{
	public static class MyModAPIHelper
	{
		[StaticEventOwner]
		public class MyMultiplayer : IMyMultiplayer
		{
			protected sealed class ModMessageServerReliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E_0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in ulong recipient, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageServerReliable(id, message, recipient);
				}
			}

			protected sealed class ModMessageServerUnreliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E_0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in ulong recipient, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageServerUnreliable(id, message, recipient);
				}
			}

			protected sealed class ModMessageClientReliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E_0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in ulong recipient, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageClientReliable(id, message, recipient);
				}
			}

			protected sealed class ModMessageClientUnreliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E_0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in ulong recipient, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageClientUnreliable(id, message, recipient);
				}
			}

			protected sealed class ModMessageBroadcastReliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageBroadcastReliable(id, message);
				}
			}

			protected sealed class ModMessageBroadcastUnreliable_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageBroadcastUnreliable(id, message);
				}
			}

			protected sealed class ModMessageBroadcastReliableFromServer_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageBroadcastReliableFromServer(id, message);
				}
			}

			protected sealed class ModMessageBroadcastUnreliableFromServer_003C_003ESystem_UInt16_0023System_Byte_003C_0023_003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ushort id, in byte[] message, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ModMessageBroadcastUnreliableFromServer(id, message);
				}
			}

			protected sealed class ReplicateEntity_Implmentation_003C_003ESystem_Int64_0023System_UInt64 : ICallSite<IMyEventOwner, long, ulong, DBNull, DBNull, DBNull, DBNull>
			{
				public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in ulong steamId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
				{
					ReplicateEntity_Implmentation(entityId, steamId);
				}
			}

			public static MyMultiplayer Static;

			private const int UNRELIABLE_MAX_SIZE = 1024;

			private static Dictionary<ushort, List<Action<byte[]>>> m_registeredListeners;

			private static Dictionary<ushort, List<Action<ushort, byte[], ulong, bool>>> m_registeredSecureListeners;

			public bool MultiplayerActive => Sync.MultiplayerActive;

			public bool IsServer => Sync.IsServer;

			public ulong ServerId => Sync.ServerId;

			public ulong MyId => Sync.MyId;

			public string MyName => Sync.MyName;

			public IMyPlayerCollection Players => Sync.Players;

			static MyMultiplayer()
			{
				m_registeredListeners = new Dictionary<ushort, List<Action<byte[]>>>();
				m_registeredSecureListeners = new Dictionary<ushort, List<Action<ushort, byte[], ulong, bool>>>();
				Static = new MyMultiplayer();
			}

			public bool IsServerPlayer(IMyNetworkClient player)
			{
				if (player is MyNetworkClient)
				{
					return (player as MyNetworkClient).IsGameServer();
				}
				return false;
			}

			public void SendEntitiesCreated(List<MyObjectBuilder_EntityBase> objectBuilders)
			{
			}

			public bool SendMessageToServer(ushort id, byte[] message, bool reliable)
			{
				if (!reliable && message.Length > 1024)
				{
					return false;
				}
				if (reliable)
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageServerReliable, id, message, Sync.ServerId);
				}
				else
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageServerUnreliable, id, message, Sync.ServerId);
				}
				return true;
			}

			public bool SendMessageToOthers(ushort id, byte[] message, bool reliable)
			{
				if (!reliable && message.Length > 1024)
				{
					return false;
				}
				if (IsServer)
				{
					if (reliable)
					{
						Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageBroadcastReliableFromServer, id, message);
					}
					else
					{
						Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageBroadcastUnreliableFromServer, id, message);
					}
				}
				else if (reliable)
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageBroadcastReliable, id, message);
				}
				else
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageBroadcastUnreliable, id, message);
				}
				return true;
			}

			public bool SendMessageTo(ushort id, byte[] message, ulong recipient, bool reliable)
			{
				if (!reliable && message.Length > 1024)
				{
					return false;
				}
				if (reliable)
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageClientReliable, id, message, recipient, new EndpointId(recipient));
				}
				else
				{
					Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ModMessageClientUnreliable, id, message, recipient, new EndpointId(recipient));
				}
				return true;
			}

			public void JoinServer(string address)
			{
				if ((!Sandbox.Engine.Platform.Game.IsDedicated || !IsServer) && MySandboxGame.Static != null)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						MySessionLoader.UnloadAndExitToMenu();
						MyGameService.OnPingServerResponded += MySandboxGame.Static.ServerResponded;
						MyGameService.OnPingServerFailedToRespond += MySandboxGame.Static.ServerFailedToRespond;
						MyGameService.PingServer(address);
					}, "UnloadAndExitToMenu");
				}
			}

			public void RegisterMessageHandler(ushort id, Action<byte[]> messageHandler)
			{
				if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
				{
					throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
				}
				List<Action<byte[]>> value = null;
				if (m_registeredListeners.TryGetValue(id, out value))
				{
					value.Add(messageHandler);
					return;
				}
				m_registeredListeners[id] = new List<Action<byte[]>>();
				m_registeredListeners[id].Add(messageHandler);
			}

			public void UnregisterMessageHandler(ushort id, Action<byte[]> messageHandler)
			{
				if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
				{
					throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
				}
				List<Action<byte[]>> value = null;
				if (m_registeredListeners.TryGetValue(id, out value))
				{
					value.Remove(messageHandler);
				}
			}

			public void RegisterSecureMessageHandler(ushort id, Action<ushort, byte[], ulong, bool> messageHandler)
			{
<<<<<<< HEAD
				if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
=======
				if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
				}
				List<Action<ushort, byte[], ulong, bool>> value = null;
				if (m_registeredSecureListeners.TryGetValue(id, out value))
				{
					value.Add(messageHandler);
					return;
				}
				m_registeredSecureListeners[id] = new List<Action<ushort, byte[], ulong, bool>>();
				m_registeredSecureListeners[id].Add(messageHandler);
			}

			public void UnregisterSecureMessageHandler(ushort id, Action<ushort, byte[], ulong, bool> messageHandler)
			{
<<<<<<< HEAD
				if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
=======
				if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
				}
				List<Action<ushort, byte[], ulong, bool>> value = null;
				if (m_registeredSecureListeners.TryGetValue(id, out value))
				{
					value.Remove(messageHandler);
				}
			}

<<<<<<< HEAD
			[Event(null, 285)]
=======
			[Event(null, 242)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Reliable]
			[Server]
			private static void ModMessageServerReliable(ushort id, byte[] message, ulong recipient)
			{
				HandleMessageClient(id, message, recipient);
			}

<<<<<<< HEAD
			[Event(null, 291)]
=======
			[Event(null, 248)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Server]
			private static void ModMessageServerUnreliable(ushort id, byte[] message, ulong recipient)
			{
				HandleMessageClient(id, message, recipient);
			}

<<<<<<< HEAD
			[Event(null, 297)]
=======
			[Event(null, 254)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Reliable]
			[Server]
			[Client]
			private static void ModMessageClientReliable(ushort id, byte[] message, ulong recipient)
			{
				HandleMessageClient(id, message, recipient);
			}

<<<<<<< HEAD
			[Event(null, 303)]
=======
			[Event(null, 260)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Server]
			[Client]
			private static void ModMessageClientUnreliable(ushort id, byte[] message, ulong recipient)
			{
				HandleMessageClient(id, message, recipient);
			}

<<<<<<< HEAD
			[Event(null, 309)]
=======
			[Event(null, 266)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Reliable]
			[Server]
			[BroadcastExcept]
			private static void ModMessageBroadcastReliable(ushort id, byte[] message)
			{
				HandleMessage(id, message);
			}

<<<<<<< HEAD
			[Event(null, 315)]
=======
			[Event(null, 272)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Server]
			[BroadcastExcept]
			private static void ModMessageBroadcastUnreliable(ushort id, byte[] message)
			{
				HandleMessage(id, message);
			}

<<<<<<< HEAD
			[Event(null, 321)]
=======
			[Event(null, 278)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Reliable]
			[BroadcastExcept]
			private static void ModMessageBroadcastReliableFromServer(ushort id, byte[] message)
			{
				HandleMessage(id, message);
			}

<<<<<<< HEAD
			[Event(null, 327)]
=======
			[Event(null, 284)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[BroadcastExcept]
			private static void ModMessageBroadcastUnreliableFromServer(ushort id, byte[] message)
			{
				HandleMessage(id, message);
			}

			private static void HandleMessageClient(ushort id, byte[] message, ulong recipient)
			{
				if (recipient == Sync.MyId)
				{
					HandleMessage(id, message);
				}
			}

			private static void HandleMessage(ushort id, byte[] message)
			{
				List<Action<byte[]>> value = null;
				if (m_registeredListeners.TryGetValue(id, out value) && value != null)
				{
					foreach (Action<byte[]> item in value)
					{
						item(message);
					}
				}
				List<Action<ushort, byte[], ulong, bool>> value2 = null;
				if (!m_registeredSecureListeners.TryGetValue(id, out value2) || value2 == null || (!MyEventContext.Current.IsLocallyInvoked && (MyEventContext.Current.Sender.IsNull || !MyEventContext.Current.Sender.IsValid)))
				{
					return;
				}
				ulong arg;
				bool arg2;
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					arg = (Sync.IsDedicated ? 0 : MySession.Static.LocalHumanPlayer.Id.SteamId);
					arg2 = MySession.Static.IsServer;
				}
				else
				{
					ulong value3 = MyEventContext.Current.Sender.Value;
					arg2 = value3 == Sync.ServerId;
					if (!Sync.Clients.TryGetClient(value3, out var client))
					{
						return;
					}
					arg = client.SteamUserId;
				}
				foreach (Action<ushort, byte[], ulong, bool> item2 in value2)
				{
					item2(id, message, arg, arg2);
				}
			}

			public void ReplicateEntityForClient(long entityId, ulong steamId)
			{
				Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReplicateEntity_Implmentation, entityId, steamId);
			}

<<<<<<< HEAD
			[Event(null, 399)]
=======
			[Event(null, 356)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			[Reliable]
			[Server]
			private static void ReplicateEntity_Implmentation(long entityId, ulong steamId)
			{
			}
		}

		public static void Initialize()
		{
			Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = true;
			MyAPIGateway.Session = MySession.Static;
			MyAPIGateway.Entities = new MyEntitiesHelper_ModAPI();
			MyAPIGateway.Players = Sync.Players;
			MyAPIGateway.CubeBuilder = MyCubeBuilder.Static;
			MyAPIGateway.IngameScripting = new MyVRageIngameScriptingAdapter(MyVRage.Platform.Scripting.GetModAPIScriptingHandle());
			MyAPIGateway.TerminalActionsHelper = MyTerminalControlFactoryHelper.Static;
			MyAPIGateway.Utilities = MyAPIUtilities.Static;
			MyAPIGateway.Parallel = MyParallelTask.Static;
			MyAPIGateway.Physics = MyPhysics.Static;
			MyAPIGateway.Multiplayer = MyMultiplayer.Static;
			MyAPIGateway.PrefabManager = MyPrefabManager.Static;
			MyAPIGateway.Input = (VRage.ModAPI.IMyInput)MyInput.Static;
			MyAPIGateway.TerminalControls = MyTerminalControls.Static;
			MyAPIGateway.Gui = new MyGuiModHelpers();
			MyAPIGateway.GridGroups = new MyGridGroupsHelper();
			MyAPIGateway.Reflection = new ScriptingReflection();
			MyAPIGateway.ContractSystem = ((MySession.Static == null) ? null : MySession.Static.GetComponent<MySessionComponentContractSystem>());
			MyAPIGateway.SpectatorTools = ((MySession.Static == null) ? null : MySession.Static.GetComponent<MySessionComponentSpectatorTools>());
<<<<<<< HEAD
			MyAPIGateway.Projectiles = ((MySession.Static == null) ? null : MySession.Static.GetComponent<MyProjectiles>());
			MyAPIGateway.Missiles = ((MySession.Static == null) ? null : MySession.Static.GetComponent<MyMissiles>());
			MyAPIGateway.DLC = MySession.Static?.GetComponent<MySessionComponentDLC>();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
