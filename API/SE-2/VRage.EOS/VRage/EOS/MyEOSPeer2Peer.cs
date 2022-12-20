using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.P2P;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.EOS
{
	internal class MyEOSPeer2Peer : IMyPeer2Peer, IDisposable
	{
		private struct OpToken : IDisposable
		{
			private MyEOSPeer2Peer m_instance;

			public OpToken(MyEOSPeer2Peer instance)
			{
				m_instance = instance;
				m_instance.m_p2POperationInProgress = true;
			}

			public void Dispose()
			{
				m_instance.m_p2POperationInProgress = false;
			}
		}

		private readonly MyEOSNetworking m_networking;

		private readonly P2PInterface m_p2p;

		private ProductUserId m_localUserId;

		private ulong m_connectionRequestNotificationId;

		private ulong m_connectionClosedNotificationId;

		private SocketId m_socketId = new SocketId
		{
			SocketName = "TEST"
		};

		private readonly Dictionary<ProductUserId, ulong> m_productToServiceId = new Dictionary<ProductUserId, ulong>();

		private readonly Dictionary<ulong, ProductUserId> m_serviceToProductId = new Dictionary<ulong, ProductUserId>();

		public ProductUserId ServerUserId;

		private const string SOCKET_NAME = "TEST";

		private bool m_isServer;

		private bool m_disconnecting;

		private static readonly TimeSpan LobbyMemberValidationTimeout = TimeSpan.FromSeconds(5.0);

		private List<(ProductUserId User, DateTime ConnectTime)> m_pendingClients = new List<(ProductUserId, DateTime)>();

		private readonly MyP2PQoSAdapter m_adapter;

		private bool m_p2POperationInProgress;

		private HashSet<ulong> m_peersAwaitingRemoval = new HashSet<ulong>();

		public int MTUSize => 1170;

		public int NetworkUpdateLatency => 8;

		public string DetailedStats => m_adapter.GetStatsReadable();

		public IEnumerable<(string Name, double Value)> Stats => m_adapter.GetStats();

		public IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats => m_adapter.GetClientStats();

		public event Action<ulong> SessionRequest;

		public event Action<ulong, string> ConnectionFailed;

		public MyEOSPeer2Peer(MyEOSNetworking networking, byte[] channels)
		{
			m_networking = networking;
			m_localUserId = new ProductUserId(IntPtr.Zero);
			m_networking.Users.OnUserChanged += OnUserChanged;
			m_p2p = m_networking.Platform.GetP2PInterface();
			m_p2p.SetRelayControl(new SetRelayControlOptions
			{
				RelayControl = m_networking.EOSPlatform.GetRelayControl()
			});
			networking.Platform.GetLobbyInterface().AddNotifyLobbyMemberStatusReceived(new AddNotifyLobbyMemberStatusReceivedOptions(), null, MemberStatusReceived);
			m_adapter = new MyP2PQoSAdapter(m_p2p, m_localUserId, m_socketId, networking);
			foreach (byte channel in channels)
			{
				RequestChannel(channel);
			}
		}

		private void MemberStatusReceived(LobbyMemberStatusReceivedCallbackInfo data)
		{
			if (data.TargetUserId != m_localUserId)
			{
				m_networking.Log($"PUID [{data.TargetUserId.GetIdString()}] status changed: {data.CurrentStatus}.");
			}
			else
			{
				m_networking.Log($"Our status changed: {data.CurrentStatus}.");
			}
			if (data.CurrentStatus == LobbyMemberStatus.Joined)
			{
				CheckClientReadiness();
			}
		}

		private void OnUserChanged()
		{
			if (m_connectionRequestNotificationId != 0L)
			{
				m_p2p.RemoveNotifyPeerConnectionRequest(m_connectionRequestNotificationId);
			}
			if (m_connectionClosedNotificationId != 0L)
			{
				m_p2p.RemoveNotifyPeerConnectionClosed(m_connectionClosedNotificationId);
			}
			m_adapter.ClearAll();
			m_localUserId = m_networking.Users.ProductUserId;
			m_adapter.LocalId = m_localUserId;
			if (!m_localUserId.IsValid())
			{
				return;
			}
			m_p2p.QueryNATType(new QueryNATTypeOptions(), null, delegate(OnQueryNATTypeCompleteInfo data)
			{
				if (data.ResultCode != 0)
				{
					MyLog.Default.WriteLine("EOS P2P - Query NAT type failed.");
				}
			});
			AddNotifyPeerConnectionRequestOptions options = new AddNotifyPeerConnectionRequestOptions
			{
				LocalUserId = m_localUserId,
				SocketId = m_socketId
			};
			m_connectionRequestNotificationId = m_p2p.AddNotifyPeerConnectionRequest(options, null, OnIncomingConnectionRequest);
			AddNotifyPeerConnectionClosedOptions options2 = new AddNotifyPeerConnectionClosedOptions
			{
				LocalUserId = m_localUserId,
				SocketId = m_socketId
			};
			m_connectionClosedNotificationId = m_p2p.AddNotifyPeerConnectionClosed(options2, null, OnConnectionClosed);
		}

		private void OnConnectionClosed(OnRemoteConnectionClosedInfo data)
		{
			ulong remoteId;
			if (m_p2POperationInProgress)
			{
				m_networking.InvokeOnNetworkThread(delegate
				{
					OnConnectionClosed(data);
				});
				m_adapter.NotifyDisconnected(data.RemoteUserId);
			}
			else if (m_productToServiceId.TryGetValue(data.RemoteUserId, out remoteId))
			{
				lock (m_peersAwaitingRemoval)
				{
					m_peersAwaitingRemoval.Remove(remoteId);
				}
				m_networking.Log("PUID [" + data.RemoteUserId.GetIdString() + "] disconnected.");
				m_adapter.RemovePeer(data.RemoteUserId);
				m_productToServiceId.Remove(data.RemoteUserId);
				m_serviceToProductId.Remove(remoteId);
				if (data.RemoteUserId == ServerUserId)
				{
					ServerUserId = null;
				}
				if (!m_disconnecting)
				{
					m_networking.InvokeOnMainThread(delegate
					{
						this.ConnectionFailed.InvokeIfNotNull(remoteId, "Remote Disconnected");
					});
				}
			}
			else
			{
				m_networking.Log("Disconnected from unknown peer PUID [" + data.RemoteUserId.GetIdString() + "].");
			}
		}

		internal void RaiseConnectionFailed(ProductUserId id, string reason)
		{
			if (m_productToServiceId.TryGetValue(id, out var remoteId))
			{
				m_networking.InvokeOnMainThread(delegate
				{
					this.ConnectionFailed.InvokeIfNotNull(remoteId, reason);
				});
			}
		}

		private void OnIncomingConnectionRequest(OnIncomingConnectionRequestInfo data)
		{
			ProductUserId remoteUserId = data.RemoteUserId;
			if (m_productToServiceId.ContainsKey(remoteUserId))
			{
				return;
			}
			m_networking.Log("Connection request from PUID [" + remoteUserId.GetIdString() + "].");
			if (m_p2POperationInProgress)
			{
				m_networking.InvokeOnNetworkThread(delegate
				{
					OnIncomingConnectionRequest(data);
				});
				return;
			}
			MyEOSGameServer eOSGameServer = m_networking.EOSGameServer;
			if (eOSGameServer == null)
			{
				return;
			}
			if (!eOSGameServer.IsLobbyMember(remoteUserId))
			{
				WaitForClientReadiness(remoteUserId);
			}
			else if (m_isServer)
			{
				string text = eOSGameServer.GetMemberAttribute(remoteUserId, "MEMBER_SERVICE_ID")?.AsUtf8;
				if (text == null)
				{
					m_networking.Log("Connection request from PUID [" + remoteUserId.GetIdString() + "] waiting until lobby modification arrives.");
					WaitForClientReadiness(remoteUserId);
				}
				else
				{
					AcceptAndValidateClient(remoteUserId, text);
				}
			}
		}

		private void WaitForClientReadiness(ProductUserId userId)
		{
			m_pendingClients.Add((userId, DateTime.Now));
			m_networking.InvokeOnNetworkThread(CheckClientReadiness, LobbyMemberValidationTimeout);
		}

		private void CheckClientReadiness()
		{
			MyEOSGameServer eOSGameServer = m_networking.EOSGameServer;
			for (int num = m_pendingClients.Count - 1; num >= 0; num--)
			{
				var (productUserId, dateTime) = m_pendingClients[num];
				string serviceUserIdStr;
				if (eOSGameServer.IsLobbyMember(productUserId) && (serviceUserIdStr = eOSGameServer.GetMemberAttribute(productUserId, "MEMBER_SERVICE_ID")?.AsUtf8) != null)
				{
					m_pendingClients.RemoveAtFast(num);
					AcceptAndValidateClient(productUserId, serviceUserIdStr);
				}
				else if (dateTime + LobbyMemberValidationTimeout < DateTime.Now)
				{
					m_pendingClients.RemoveAtFast(num);
					RejectConnection(productUserId);
				}
			}
		}

		private void AcceptAndValidateClient(ProductUserId userId, string serviceUserIdStr)
		{
			MyEOSGameServer eOSGameServer = m_networking.EOSGameServer;
			ulong serviceUserId;
			ExternalAccountType serviceKind;
			try
			{
				serviceUserId = ulong.Parse(serviceUserIdStr);
				serviceKind = (ExternalAccountType)eOSGameServer.GetMemberAttribute(userId, "MEMBER_SERVICE_KIND").AsInt64.Value;
			}
			catch
			{
				m_networking.Error("Could not validate service id for client PUID [" + userId.GetIdString() + "].");
				RejectConnection(userId);
				return;
			}
			ConnectInterface accounts = m_networking.Platform.GetConnectInterface();
			accounts.QueryProductUserIdMappings(new QueryProductUserIdMappingsOptions
			{
				LocalUserId = m_localUserId,
				ProductUserIds = new ProductUserId[1] { userId }
			}, null, delegate(QueryProductUserIdMappingsCallbackInfo xdata)
			{
				bool flag = false;
				if (xdata.ResultCode == Result.Success && accounts.GetProductUserIdMapping(new GetProductUserIdMappingOptions
				{
					LocalUserId = m_localUserId,
					AccountIdType = serviceKind,
					TargetProductUserId = userId
				}, out var outBuffer) == Result.Success)
				{
					flag = serviceKind == ExternalAccountType.Xbl || ulong.Parse(outBuffer) == serviceUserId;
				}
				if (flag)
				{
					OnVerifiedPeerConnected(serviceUserId, userId);
				}
				else
				{
					m_networking.Error("Could not validate service id for client PUID [" + userId.GetIdString() + "].");
					RejectConnection(userId);
				}
			});
		}

		private void OnVerifiedPeerConnected(ulong serviceUserId, ProductUserId eosUserID)
		{
			m_networking.Log("PUID[" + eosUserID.GetIdString() + "]/ServiceId[" + EndpointId.Format(serviceUserId) + "] Added to known endpoints.");
			lock (m_peersAwaitingRemoval)
			{
				m_peersAwaitingRemoval.Remove(serviceUserId);
			}
			m_adapter.AddPeer(eosUserID);
			m_productToServiceId.Add(eosUserID, serviceUserId);
			m_serviceToProductId.Add(serviceUserId, eosUserID);
			m_networking.InvokeOnMainThread(delegate
			{
				this.SessionRequest.InvokeIfNotNull(serviceUserId);
			});
		}

		private void RejectConnection(ProductUserId eosUserID)
		{
			m_p2p.CloseConnection(new CloseConnectionOptions
			{
				LocalUserId = m_localUserId,
				RemoteUserId = eosUserID,
				SocketId = m_socketId
			});
		}

		public bool AcceptSession(ulong remotePeerId)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				AcceptSessionInternal(remotePeerId);
			});
			return true;
		}

		private void AcceptSessionInternal(ulong remotePeerId)
		{
			lock (m_productToServiceId)
			{
				if (m_serviceToProductId.TryGetValue(remotePeerId, out var value))
				{
					m_networking.Log("Accept connection from PUID [" + value.GetIdString() + "].");
					AcceptConnectionOptions options = new AcceptConnectionOptions
					{
						LocalUserId = m_localUserId,
						RemoteUserId = value,
						SocketId = new SocketId
						{
							SocketName = "TEST"
						}
					};
					m_p2p.AcceptConnection(options);
				}
				else
				{
					m_networking.InvokeOnMainThread(delegate
					{
						this.ConnectionFailed.InvokeIfNotNull(remotePeerId, "Endpoint not found.");
					});
				}
			}
		}

		public bool CloseSession(ulong remotePeerId)
		{
			bool flag;
			lock (m_peersAwaitingRemoval)
			{
				flag = m_peersAwaitingRemoval.Add(remotePeerId);
			}
			if (flag)
			{
				m_networking.InvokeOnNetworkThread(delegate
				{
					CheckAndEnqueueCloseSession(remotePeerId);
				});
			}
			return true;
		}

		private void CheckAndEnqueueCloseSession(ulong remotePeerId)
		{
			bool flag;
			lock (m_peersAwaitingRemoval)
			{
				flag = m_peersAwaitingRemoval.Contains(remotePeerId);
			}
			lock (m_productToServiceId)
			{
				if (flag && m_serviceToProductId.TryGetValue(remotePeerId, out var value))
				{
					m_networking.Log($"Intentionally closing connection to PUID [{value.GetIdString()}] in {5} seconds.");
					m_networking.InvokeOnNetworkThread(delegate
					{
						CloseSessionInternal(remotePeerId);
					}, TimeSpan.FromSeconds(5.0));
				}
			}
		}

		private void CloseSessionInternal(ulong remotePeerId)
		{
			lock (m_productToServiceId)
			{
				bool flag;
				lock (m_peersAwaitingRemoval)
				{
					flag = m_peersAwaitingRemoval.Remove(remotePeerId);
				}
				if (flag && m_serviceToProductId.TryGetValue(remotePeerId, out var value))
				{
					m_networking.Log("Intentionally closing connection to PUID [" + value.GetIdString() + "].");
					m_productToServiceId.Remove(value);
					m_serviceToProductId.Remove(remotePeerId);
					m_adapter.RemovePeer(value);
					CloseConnectionOptions options = new CloseConnectionOptions
					{
						LocalUserId = m_localUserId,
						RemoteUserId = value,
						SocketId = m_socketId
					};
					m_p2p.CloseConnection(options);
					if (value == ServerUserId)
					{
						ServerUserId = null;
					}
				}
			}
		}

		public bool RequestChannel(int channel)
		{
			lock (m_productToServiceId)
			{
				if (channel > 255)
				{
					return false;
				}
				m_adapter.ReserveChannel((byte)channel);
				return true;
			}
		}

		public bool SendPacket(ulong remoteUser, byte[] data, int byteCount, MyP2PMessageEnum msgType, int channel)
		{
			lock (m_productToServiceId)
			{
				if (!m_serviceToProductId.TryGetValue(remoteUser, out var value))
				{
					return false;
				}
				int num;
				switch (msgType)
				{
				default:
					throw new ArgumentOutOfRangeException("msgType", msgType, null);
				case MyP2PMessageEnum.Unreliable:
				case MyP2PMessageEnum.UnreliableNoDelay:
				case MyP2PMessageEnum.ReliableWithBuffering:
					num = ((msgType == MyP2PMessageEnum.ReliableWithBuffering) ? 1 : 0);
					break;
				case MyP2PMessageEnum.Reliable:
					num = 1;
					break;
				}
				bool flag = (byte)num != 0;
				bool highPriority = msgType == MyP2PMessageEnum.Reliable;
				if (!flag && byteCount > 1170)
				{
					return false;
				}
				using (Operation())
				{
					return m_adapter.Send(MemoryExtensions.AsSpan(data, 0, byteCount), (byte)channel, value, highPriority, flag);
				}
			}
		}

		public bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel)
		{
			lock (m_productToServiceId)
			{
				using (Operation())
				{
					if (m_adapter.TryReadMessage(channel, MemoryExtensions.AsSpan(buffer, 0, (int)dataSize), out var size, out var peer))
					{
						if (!m_productToServiceId.TryGetValue(peer, out remoteUser))
						{
							return false;
						}
						dataSize = size;
						remoteUser = m_productToServiceId[peer];
						return true;
					}
				}
				dataSize = 0u;
				remoteUser = 0uL;
				return false;
			}
		}

		public bool IsPacketAvailable(out uint msgSize, int channel)
		{
			lock (m_productToServiceId)
			{
				using (Operation())
				{
					if (!m_adapter.HasMessage(channel, out msgSize))
					{
						m_networking.Platform.Tick();
						return m_adapter.HasMessage(channel, out msgSize);
					}
					return true;
				}
			}
		}

		public void ProcessQueues()
		{
			lock (m_productToServiceId)
			{
				if (m_localUserId.IsValid())
				{
					using (Operation())
					{
						m_adapter.ProcessQueues();
					}
				}
			}
		}

		private OpToken Operation()
		{
			return new OpToken(this);
		}

		public bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state)
		{
			lock (m_productToServiceId)
			{
				if (m_serviceToProductId.ContainsKey(remoteUser))
				{
					m_p2p.GetNATType(new GetNATTypeOptions(), out var outNATType);
					state.ConnectionActive = true;
					state.UsingRelay = outNATType == NATType.Strict;
				}
				else
				{
					state.ConnectionActive = false;
				}
				return false;
			}
		}

		public void SetServer(bool server)
		{
			m_isServer = server;
		}

		public void BeginFrameProcessing()
		{
		}

		public void EndFrameProcessing()
		{
		}

		public void DisconnectAll()
		{
			m_adapter.ClearAll();
		}

		public void Dispose()
		{
			if (m_connectionRequestNotificationId != 0L)
			{
				m_p2p.RemoveNotifyPeerConnectionRequest(m_connectionRequestNotificationId);
			}
			if (m_connectionClosedNotificationId != 0L)
			{
				m_p2p.RemoveNotifyPeerConnectionClosed(m_connectionClosedNotificationId);
			}
			if (m_localUserId.IsValid())
			{
				m_p2p.CloseConnections(new CloseConnectionsOptions
				{
					LocalUserId = m_localUserId
				});
			}
			m_adapter.Dispose();
		}

		public void SetHost(ProductUserId hostProductUserId, ulong hostServiceUserId)
		{
			m_networking.Log("Host [" + hostProductUserId.GetIdString() + "] set.");
			m_disconnecting = false;
			m_productToServiceId.Add(hostProductUserId, hostServiceUserId);
			m_serviceToProductId.Add(hostServiceUserId, hostProductUserId);
			m_adapter.AddPeer(hostProductUserId);
			ServerUserId = hostProductUserId;
		}

		public void ClearHost()
		{
			if (ServerUserId != null)
			{
				m_networking.Log("Host [" + ServerUserId.GetIdString() + "] cleared.");
				m_serviceToProductId.Remove(m_productToServiceId[ServerUserId]);
				m_productToServiceId.Remove(ServerUserId);
				m_adapter.RemovePeer(ServerUserId);
				ServerUserId = null;
			}
		}

		public void SetDisconnecting()
		{
			m_disconnecting = true;
		}
	}
}
