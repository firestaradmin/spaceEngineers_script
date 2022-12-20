using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI
{
	public interface IMyMultiplayer
	{
		bool MultiplayerActive { get; }

		bool IsServer { get; }

		ulong ServerId { get; }

		ulong MyId { get; }

		string MyName { get; }

		IMyPlayerCollection Players { get; }

		bool IsServerPlayer(IMyNetworkClient player);

		void SendEntitiesCreated(List<MyObjectBuilder_EntityBase> objectBuilders);

		bool SendMessageToServer(ushort id, byte[] message, bool reliable = true);

		bool SendMessageToOthers(ushort id, byte[] message, bool reliable = true);

		bool SendMessageTo(ushort id, byte[] message, ulong recipient, bool reliable = true);

		void JoinServer(string address);

		[Obsolete("Use RegisterSecureMessageHandler && UnregisterSecureMessageHandler pair instead", false)]
		void RegisterMessageHandler(ushort id, Action<byte[]> messageHandler);

		[Obsolete("Use RegisterSecureMessageHandler && UnregisterSecureMessageHandler pair instead", false)]
		void UnregisterMessageHandler(ushort id, Action<byte[]> messageHandler);

		/// <summary>
		/// Allows you do reliable checks WHO have sent message to you.
<<<<<<< HEAD
		/// Action - HandlerId, Package, Player SteamID or 0 for Dedicated server, Sent message comes from server
		/// </summary>
		/// <param name="id">Unique handler id</param>
		/// <param name="messageHandler">Call function</param>        
=======
		/// </summary>
		/// <param name="id">Uniq handler id</param>
		/// <param name="messageHandler">Call function</param>
		/// <param name="ushort">HandlerId</param>
		/// <param name="byte[][]">Package</param>
		/// <param name="ulong">Player SteamID or 0 for Dedicated server</param>
		/// <param name="bool">Sent message comes from server</param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void RegisterSecureMessageHandler(ushort id, Action<ushort, byte[], ulong, bool> messageHandler);

		void UnregisterSecureMessageHandler(ushort id, Action<ushort, byte[], ulong, bool> messageHandler);

		void ReplicateEntityForClient(long entityId, ulong steamId);
	}
}
