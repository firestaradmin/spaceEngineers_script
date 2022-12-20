using System;
using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
	public class MySyncLayer
	{
		private class DefaultProtoSerializer<T>
		{
			public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>(MyObjectBuilderSerializer.Serializer);
		}

		internal readonly MyTransportLayer TransportLayer;

		internal readonly MyClientCollection Clients;

		internal MySyncLayer(MyTransportLayer transportLayer)
		{
			TransportLayer = transportLayer;
			Clients = new MyClientCollection();
		}

		internal void RegisterClientEvents(MyMultiplayerBase multiplayer)
		{
			multiplayer.ClientJoined += OnClientJoined;
			multiplayer.ClientLeft += OnClientLeft;
			foreach (ulong member in multiplayer.Members)
			{
				if (member != Sync.MyId)
				{
					OnClientJoined(member, multiplayer.GetMemberName(member));
				}
			}
		}

		private void OnClientJoined(ulong steamUserId, string userName)
		{
			if (!Clients.HasClient(steamUserId))
			{
				Clients.AddClient(steamUserId, userName);
			}
		}

		private void OnClientLeft(ulong steamUserId, MyChatMemberStateChangeEnum leaveReason)
		{
			Clients.RemoveClient(steamUserId);
		}

		public static bool CheckSendPermissions(ulong target, MyMessagePermissions permission)
		{
			return permission switch
			{
				MyMessagePermissions.FromServer | MyMessagePermissions.ToServer => Sync.ServerId == target || Sync.IsServer, 
				MyMessagePermissions.FromServer => Sync.IsServer, 
				MyMessagePermissions.ToServer => Sync.ServerId == target, 
				_ => false, 
			};
		}

		public static bool CheckReceivePermissions(ulong sender, MyMessagePermissions permission)
		{
			return permission switch
			{
				MyMessagePermissions.FromServer | MyMessagePermissions.ToServer => Sync.ServerId == sender || Sync.IsServer, 
				MyMessagePermissions.FromServer => Sync.ServerId == sender, 
				MyMessagePermissions.ToServer => Sync.IsServer, 
				_ => false, 
			};
		}

		internal static ISerializer<TMsg> GetSerializer<TMsg>() where TMsg : struct
		{
			if (Attribute.IsDefined(typeof(TMsg), typeof(ProtoContractAttribute)))
			{
				return CreateProto<TMsg>();
			}
			if (BlittableHelper<TMsg>.IsBlittable)
			{
				return (ISerializer<TMsg>)Activator.CreateInstance(typeof(BlitSerializer<>).MakeGenericType(typeof(TMsg)));
			}
			return null;
		}

		private static ISerializer<TMsg> CreateProto<TMsg>()
		{
			return DefaultProtoSerializer<TMsg>.Default;
		}

		private static ISerializer<TMsg> CreateBlittable<TMsg>() where TMsg : unmanaged
		{
			return BlitSerializer<TMsg>.Default;
		}
	}
}
