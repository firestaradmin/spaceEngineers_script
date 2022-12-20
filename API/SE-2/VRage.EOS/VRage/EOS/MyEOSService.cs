using System.Collections.Generic;
using VRage.GameServices;

namespace VRage.EOS
{
	public static class MyEOSService
	{
		private static MyEOSNetworking m_instance;

		public static IMyGameService Create()
		{
			return new MyEOSGameService();
		}

		public static void InitNetworking(bool isDedicated, string productName, IMyGameService service, string clientId, string clientSecret, string productId, string sandboxId, string deploymentId, string encryptionUrl, IMyEOSPlatform platform, bool verboseLogging, IEnumerable<string> parameters, MyServerDiscoveryAggregator serverDiscoveryAggregator, byte[] channels)
		{
			m_instance = new MyEOSNetworking(isDedicated, productName, service, clientId, clientSecret, productId, sandboxId, deploymentId, encryptionUrl, platform, verboseLogging, parameters, serverDiscoveryAggregator, channels);
		}

		public static IMyEOSPlatform CreatePlatform()
		{
			return new MyEOSPlatformAgnostic();
		}
	}
}
