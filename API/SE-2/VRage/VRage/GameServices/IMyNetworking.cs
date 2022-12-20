namespace VRage.GameServices
{
	public interface IMyNetworking
	{
		string ServiceName { get; }

		IMyPeer2Peer Peer2Peer { get; }

		IMyNetworkingChat Chat { get; }

		IMyNetworkingInvite Invite { get; }

		string ProductName { get; }
	}
}
