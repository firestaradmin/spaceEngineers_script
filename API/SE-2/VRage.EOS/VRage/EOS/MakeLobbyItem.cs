using Epic.OnlineServices.Lobby;

namespace VRage.EOS
{
	public delegate T MakeLobbyItem<T>(LobbyDetails details, LobbyDetailsInfo info = null);
}
