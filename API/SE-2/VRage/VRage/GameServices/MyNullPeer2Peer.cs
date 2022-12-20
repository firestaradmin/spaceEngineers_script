using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MyNullPeer2Peer : IMyPeer2Peer
	{
		public int MTUSize => 1200;

		public int NetworkUpdateLatency { get; }

		/// <inheritdoc />
		public string DetailedStats => "";

		/// <inheritdoc />
		public IEnumerable<(string Name, double Value)> Stats
		{
			get
			{
				yield break;
			}
		}

		/// <inheritdoc />
		public IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats
		{
			get
			{
				yield break;
			}
		}

		public event Action<ulong> SessionRequest
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<ulong, string> ConnectionFailed
		{
			add
			{
			}
			remove
			{
			}
		}

		public bool AcceptSession(ulong remotePeerId)
		{
			return false;
		}

		public bool CloseSession(ulong remotePeerId)
		{
			return false;
		}

		/// <inheritdoc />
		public bool RequestChannel(int channel)
		{
			return true;
		}

		public bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel)
		{
			return false;
		}

		public bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel)
		{
			dataSize = 0u;
			remoteUser = 0uL;
			return false;
		}

		public bool IsPacketAvailable(out uint msgSize, int channel)
		{
			msgSize = 0u;
			return false;
		}

		public bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state)
		{
			state = default(MyP2PSessionState);
			return false;
		}

		public void SetServer(bool server)
		{
		}

		public void BeginFrameProcessing()
		{
		}

		public void EndFrameProcessing()
		{
		}
	}
}
