using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public interface IMyPeer2Peer
	{
		int MTUSize { get; }

		int NetworkUpdateLatency { get; }

		string DetailedStats { get; }

		IEnumerable<(string Name, double Value)> Stats { get; }

		IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats { get; }

		event Action<ulong> SessionRequest;

		event Action<ulong, string> ConnectionFailed;

		bool AcceptSession(ulong remotePeerId);

		bool CloseSession(ulong remotePeerId);

		bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel);

		bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel);

		bool IsPacketAvailable(out uint msgSize, int channel);

		bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state);

		void SetServer(bool server);

		void BeginFrameProcessing();

		void EndFrameProcessing();
	}
}
