using Epic.OnlineServices.Lobby;

namespace VRage.EOS
{
	internal static class LobbyMemberStatusExtensions
	{
		public static bool IsConnected(this LobbyMemberStatus self)
		{
			if (self != 0)
			{
				return self == LobbyMemberStatus.Promoted;
			}
			return true;
		}
	}
}
