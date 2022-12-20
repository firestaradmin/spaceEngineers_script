using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamInvite : IMyNetworkingInvite
	{
		private readonly MySteamNetworking m_steamNetworking;

		public MySteamInvite(MySteamNetworking steamNetworking)
		{
			m_steamNetworking = steamNetworking;
		}

		public bool IsInviteSupported()
		{
			return true;
		}

		public void OpenInviteOverlay()
		{
			if (m_steamNetworking.Service.IsActive)
			{
				if (MySteamLobby.ActiveLobby != null)
				{
					SteamFriends.ActivateGameOverlayInviteDialog((CSteamID)MySteamLobby.ActiveLobby.LobbyId);
				}
				else
				{
					SteamFriends.ActivateGameOverlay("");
				}
			}
		}
	}
}
