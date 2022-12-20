using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public interface IMyLobbyDiscovery
	{
		bool Supported { get; }

		bool FriendSupport { get; }

		bool ContinueToLobbySupported { get; }

		event MyLobbyJoinRequested OnJoinLobbyRequested;

		bool OnInvite(string protocolData);

		void JoinLobby(ulong lobbyId, MyJoinResponseDelegate responseDelegate);

		IMyLobby CreateLobby(ulong lobbyId);

		void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse);

		void RequestLobbyList(Action<bool> completed);

		void AddPublicLobbies(List<IMyLobby> lobbyList);

		void AddFriendLobbies(List<IMyLobby> lobbyList);

		void AddLobbyFilter(string key, string value);
	}
}
