using VRage.GameServices;

namespace VRage.Steam
{
	public static class MySteamGameService
	{
		private static MySteamNetworking m_networking;

		public static IMyGameService Create(bool isDedicated, uint appId)
		{
			return new MySteamService(isDedicated, appId);
		}

		public static IMyNetworking InitNetworking(bool isDedicated, IMyGameService service, string productName, MyServerDiscoveryAggregator serverDiscoveryAggregator, bool serverDiscovery = true, bool lobbyDiscovery = true)
		{
			m_networking = new MySteamNetworking(isDedicated, (MySteamService)service, serverDiscoveryAggregator, serverDiscovery, lobbyDiscovery, productName);
			return m_networking;
		}

		public static IMyMicrophoneService CreateMicrophone()
		{
			return new MySteamMicrophone();
		}
	}
}
