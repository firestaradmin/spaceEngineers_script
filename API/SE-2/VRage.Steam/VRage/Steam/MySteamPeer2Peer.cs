using System;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamPeer2Peer : IMyPeer2Peer
	{
		private Callback<P2PSessionRequest_t> m_sessionRequest;

		private Callback<P2PSessionConnectFail_t> m_connectionFailed;

		private Callback<P2PSessionRequest_t> m_sessionRequestGameServer;

		private Callback<P2PSessionConnectFail_t> m_connectionFailedGameServer;

		private bool m_server;

		public int MTUSize => 1200;

		public int NetworkUpdateLatency => 2;

		public string DetailedStats => "";

		public IEnumerable<(string Name, double Value)> Stats
		{
			get
			{
				yield break;
			}
		}

		public IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats
		{
			get
			{
				yield break;
			}
		}

		public event Action<ulong> SessionRequest;

		public event Action<ulong, string> ConnectionFailed;

		public MySteamPeer2Peer()
		{
			m_sessionRequestGameServer = Callback<P2PSessionRequest_t>.CreateGameServer(HandleSessionRequest);
			m_connectionFailedGameServer = Callback<P2PSessionConnectFail_t>.CreateGameServer(HandleConnectionFailure);
			m_sessionRequest = Callback<P2PSessionRequest_t>.Create(HandleSessionRequest);
			m_connectionFailed = Callback<P2PSessionConnectFail_t>.Create(HandleConnectionFailure);
		}

		public void SetServer(bool server)
		{
			m_server = server;
		}

		private void HandleSessionRequest(P2PSessionRequest_t param)
		{
			this.SessionRequest?.Invoke((ulong)param.m_steamIDRemote);
		}

		private void HandleConnectionFailure(P2PSessionConnectFail_t param)
		{
			Action<ulong, string> connectionFailed = this.ConnectionFailed;
			if (connectionFailed != null)
			{
				ulong arg = (ulong)param.m_steamIDRemote;
				MyP2PSessionErrorEnum eP2PSessionError = (MyP2PSessionErrorEnum)param.m_eP2PSessionError;
				connectionFailed(arg, eP2PSessionError.ToString());
			}
		}

		public bool AcceptSession(ulong remotePeerId)
		{
			if (m_server)
			{
				return SteamGameServerNetworking.AcceptP2PSessionWithUser((CSteamID)remotePeerId);
			}
			return SteamNetworking.AcceptP2PSessionWithUser((CSteamID)remotePeerId);
		}

		public bool CloseSession(ulong remotePeerId)
		{
			if (m_server)
			{
				return SteamGameServerNetworking.CloseP2PSessionWithUser((CSteamID)remotePeerId);
			}
			return SteamNetworking.CloseP2PSessionWithUser((CSteamID)remotePeerId);
		}

		public bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel)
		{
			if (m_server)
			{
				return SteamGameServerNetworking.SendP2PPacket((CSteamID)remoteUser, data, (uint)byteCount, (EP2PSend)msgType, channel);
			}
			return SteamNetworking.SendP2PPacket((CSteamID)remoteUser, data, (uint)byteCount, (EP2PSend)msgType, channel);
		}

		public bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel)
		{
			CSteamID psteamIDRemote;
			bool result = ((!m_server) ? SteamNetworking.ReadP2PPacket(buffer, (uint)buffer.Length, out dataSize, out psteamIDRemote, channel) : SteamGameServerNetworking.ReadP2PPacket(buffer, (uint)buffer.Length, out dataSize, out psteamIDRemote, channel));
			remoteUser = (ulong)psteamIDRemote;
			return result;
		}

		public bool IsPacketAvailable(out uint msgSize, int channel)
		{
			if (m_server)
			{
				return SteamGameServerNetworking.IsP2PPacketAvailable(out msgSize, channel);
			}
			if (MySteamService.Static.IsActive)
			{
				return SteamNetworking.IsP2PPacketAvailable(out msgSize, channel);
			}
			msgSize = 0u;
			return false;
		}

		public bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state)
		{
			if ((!m_server) ? SteamNetworking.GetP2PSessionState((CSteamID)remoteUser, out var pConnectionState) : SteamGameServerNetworking.GetP2PSessionState((CSteamID)remoteUser, out pConnectionState))
			{
				state.BytesQueuedForSend = pConnectionState.m_nBytesQueuedForSend;
				state.Connecting = pConnectionState.m_bConnecting == 1;
				state.ConnectionActive = pConnectionState.m_bConnectionActive == 1;
				state.LastSessionError = (MyP2PSessionErrorEnum)pConnectionState.m_eP2PSessionError;
				state.PacketsQueuedForSend = pConnectionState.m_nPacketsQueuedForSend;
				state.RemoteIP = pConnectionState.m_nRemoteIP;
				state.RemotePort = pConnectionState.m_nRemotePort;
				state.UsingRelay = pConnectionState.m_bUsingRelay == 1;
				return true;
			}
			return false;
		}

		public void BeginFrameProcessing()
		{
		}

		public void EndFrameProcessing()
		{
		}
	}
}
