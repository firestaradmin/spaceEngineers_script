using System;
using System.Collections.Generic;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyEOSLobbyDiscovery : IMyLobbyDiscovery
	{
		public bool Supported => false;

		public bool FriendSupport => false;

		public bool ContinueToLobbySupported { get; }

		public event MyLobbyJoinRequested OnJoinLobbyRequested;

		public void JoinLobby(ulong lobbyId, MyJoinResponseDelegate responseDelegate)
		{
		}

		public IMyLobby CreateLobby(ulong lobbyId)
		{
			return null;
		}

		public void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse)
		{
		}

		public void RequestLobbyList(Action<bool> completed)
		{
		}

		public void AddPublicLobbies(List<IMyLobby> lobbyList)
		{
		}

		public void AddFriendLobbies(List<IMyLobby> lobbyList)
		{
		}

		public void AddLobbyFilter(string key, string value)
		{
		}

		public bool OnInvite(string protocolData)
		{
			return false;
		}
	}
}
