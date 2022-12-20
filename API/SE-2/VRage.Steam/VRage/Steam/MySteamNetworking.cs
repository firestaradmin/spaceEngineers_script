using System;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamNetworking : IMyNetworking, IDisposable
	{
		public readonly MySteamService Service;

		private MySteamGameServer m_gameServer;

		private readonly MySteamLobbyDiscovery m_steamLobbyDiscovery;

		private readonly MySteamServerDiscovery m_steamServerDiscovery;

		public string ServiceName => "Steam";

		public IMyPeer2Peer Peer2Peer { get; }

		public IMyNetworkingChat Chat { get; }

		public IMyNetworkingInvite Invite { get; }

		public string ProductName { get; }

		public MySteamNetworking(bool isDedicated, MySteamService service, MyServerDiscoveryAggregator serverDiscoveryAggregator, bool serverDiscovery, bool lobbyDiscovery, string productName)
		{
			ProductName = productName;
			Service = service;
			if (isDedicated)
			{
				m_gameServer = new MySteamGameServer(this);
				MyServiceManager.Instance.AddService((IMyGameServer)m_gameServer);
			}
			Invite = new MySteamInvite(this);
			Peer2Peer = new MySteamPeer2Peer();
			if (!isDedicated)
			{
				if (serverDiscovery)
				{
					m_steamServerDiscovery = new MySteamServerDiscovery(this);
					if (serverDiscoveryAggregator != null)
					{
						serverDiscoveryAggregator.AddAggregate(m_steamServerDiscovery);
					}
					else
					{
						MyServiceManager.Instance.AddService((IMyServerDiscovery)m_steamServerDiscovery);
					}
				}
				if (lobbyDiscovery)
				{
					m_steamLobbyDiscovery = new MySteamLobbyDiscovery(this);
					MyServiceManager.Instance.AddService((IMyLobbyDiscovery)m_steamLobbyDiscovery);
				}
			}
			Chat = new SimpleNetworkingChat(service, MyGameServiceConstants.MAX_CHAT_MESSAGE_SIZE);
			service.OnUpdate += Update;
			service.OnUpdateNetworkThread += UpdateNetworkThread;
		}

		private void Update()
		{
			m_gameServer?.RunCallbacks();
			m_steamServerDiscovery?.Update();
		}

		private void UpdateNetworkThread(bool sessionEnabled)
		{
			m_gameServer?.Update();
		}

		public void Dispose()
		{
			m_steamLobbyDiscovery?.Dispose();
			m_steamServerDiscovery?.Dispose();
			m_gameServer?.Dispose();
			m_gameServer = null;
			Service.OnUpdate -= Update;
			Service.OnUpdateNetworkThread -= UpdateNetworkThread;
		}
	}
}
