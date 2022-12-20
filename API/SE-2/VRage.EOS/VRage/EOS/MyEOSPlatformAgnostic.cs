using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.Platform;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyEOSPlatformAgnostic : IMyEOSPlatform, IMyNetworkingInvite
	{
		public long AllocatedMemory => 0L;

		public string SessionNameAttributeKey => "NO_SESSION_HANDLE";

		public Result Initialize(string productName)
		{
			return PlatformInterface.Initialize(new InitializeOptions
			{
				ProductName = productName,
				ProductVersion = "1.0"
			});
		}

		public RelayControl GetRelayControl()
		{
			return RelayControl.AllowRelays;
		}

		public bool CreateOrJoinSession(string connectionString, ref string sessionName)
		{
			return false;
		}

		public void DisconnectSession()
		{
		}

		public string RetrieveConnectionStringFromSession(string dataProtocol)
		{
			return null;
		}

		public bool IsInviteSupported()
		{
			return false;
		}

		public void OpenInviteOverlay()
		{
		}
	}
}
