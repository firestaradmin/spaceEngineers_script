using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using VRage.GameServices;

namespace VRage.EOS
{
	public interface IMyEOSPlatform : IMyNetworkingInvite
	{
		long AllocatedMemory { get; }

		string SessionNameAttributeKey { get; }

		Result Initialize(string productName);

		RelayControl GetRelayControl();

		string RetrieveConnectionStringFromSession(string dataProtocol);

		bool CreateOrJoinSession(string connectionString, ref string sessionName);

		void DisconnectSession();
	}
}
