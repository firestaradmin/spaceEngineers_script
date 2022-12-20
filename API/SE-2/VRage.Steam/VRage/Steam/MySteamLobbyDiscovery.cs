using System;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamLobbyDiscovery : IMyLobbyDiscovery, IDisposable
	{
		private readonly Callback<GameLobbyJoinRequested_t> m_gameLobbyJoinRequested;

		private readonly MySteamNetworking m_steamNetworking;

		public bool Supported => true;

		public bool FriendSupport => true;

		public bool ContinueToLobbySupported => true;

		public event MyLobbyJoinRequested OnJoinLobbyRequested;

		public MySteamLobbyDiscovery(MySteamNetworking steamNetworking)
		{
			m_steamNetworking = steamNetworking;
			m_gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(HandleLobbyChange);
			MySteamLobby.OnInvitedToLobby += MySteamLobby_OnInvitedToLobby;
		}

		public void Dispose()
		{
			MySteamLobby.OnInvitedToLobby -= MySteamLobby_OnInvitedToLobby;
			m_gameLobbyJoinRequested?.Unregister();
		}

		private void MySteamLobby_OnInvitedToLobby(IMyLobby lobbyToJoin, ulong invitedBy)
		{
			this.OnJoinLobbyRequested?.Invoke(lobbyToJoin, invitedBy, SteamFriends.GetFriendPersonaName((CSteamID)invitedBy));
		}

		private void HandleLobbyChange(GameLobbyJoinRequested_t param)
		{
			MySteamLobby orCreate = MySteamLobby.GetOrCreate((ulong)param.m_steamIDLobby);
			orCreate.RequestData();
			this.OnJoinLobbyRequested?.Invoke(orCreate, m_steamNetworking.Service.UserId, m_steamNetworking.Service.UserName);
		}

		public bool OnInvite(string protocolData)
		{
			return false;
		}

		public void JoinLobby(ulong lobbyId, MyJoinResponseDelegate responseDelegate)
		{
			MyServiceManager.Instance.AddService((IMyNetworking)m_steamNetworking);
			MySteamLobby.GetOrCreate(lobbyId).Join(responseDelegate);
		}

		public IMyLobby CreateLobby(ulong lobbyId)
		{
			MyServiceManager.Instance.AddService((IMyNetworking)m_steamNetworking);
			return MySteamLobby.GetOrCreate(lobbyId);
		}

		public void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse)
		{
			MyServiceManager.Instance.AddService((IMyNetworking)m_steamNetworking);
			MySteamLobby.TryCreateLobby(type, maxPlayers, createdResponse);
		}

		public void AddPublicLobbies(List<IMyLobby> lobbyList)
		{
			MySteamLobby.AddPublicLobbies(lobbyList);
		}

		public void AddFriendLobbies(List<IMyLobby> lobbyList)
		{
			MySteamLobby.AddFriendLobbies(lobbyList);
		}

		public void RequestLobbyList(Action<bool> completed)
		{
			MySteamLobby.RefreshLobbies(completed);
		}

		public void AddLobbyFilter(string key, string value)
		{
			SteamMatchmaking.AddRequestLobbyListStringFilter(key, value, ELobbyComparison.k_ELobbyComparisonEqual);
		}
	}
}
