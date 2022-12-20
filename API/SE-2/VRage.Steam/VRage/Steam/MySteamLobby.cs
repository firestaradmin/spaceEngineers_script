using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Steamworks;
using VRage.GameServices;
using VRage.Library.Collections;

namespace VRage.Steam
{
	internal class MySteamLobby : IMyLobby
	{
		private static Callback<LobbyCreated_t> m_lobbyCreateResult;

		private static Callback<LobbyMatchList_t> m_lobbyListResult;

		private static Callback<LobbyEnter_t> m_lobbyEnterResult;

		private static Callback<LobbyDataUpdate_t> m_lobbyDataUpdateResult;

		private static Callback<LobbyChatMsg_t> m_lobbyChatMessageResult;

		private static Callback<LobbyChatUpdate_t> m_lobbyChatUpdateResult;

		private static Callback<LobbyGameCreated_t> m_lobbyGameCreatedResult;

		private static Callback<LobbyInvite_t> m_lobbyInviteResult;

		private static Callback<LobbyKicked_t> m_lobbyKickedResult;

		private static Dictionary<ulong, MySteamLobby> m_lobbiesByIds;

		private CSteamID m_lobbyId;

		private static Action<bool> OnRefreshCompleted;

		private static MyLobbyCreated OnLobbyCreated;

		private static MyJoinResponseDelegate OnLobbyJoined;

		internal static MySteamLobby ActiveLobby { get; private set; }

		public ulong LobbyId
		{
			get
			{
				return (ulong)m_lobbyId;
			}
			private set
			{
				m_lobbyId = (CSteamID)value;
			}
		}

		public bool IsValid => m_lobbyId.IsValid();

		public ulong OwnerId => (ulong)SteamMatchmaking.GetLobbyOwner(m_lobbyId);

		public MyLobbyType LobbyType
		{
			get
			{
				string data = GetData("LobbyType");
				MyLobbyType result = MyLobbyType.Public;
				if (!Enum.TryParse<MyLobbyType>(data, out result))
				{
					return MyLobbyType.Public;
				}
				return result;
			}
			set
			{
				SetData("LobbyType", value.ToString());
			}
		}

		public ConnectionStrategy ConnectionStrategy => ConnectionStrategy.Normal;

		public int MemberCount => SteamMatchmaking.GetNumLobbyMembers(m_lobbyId);

		public int MemberLimit
		{
			get
			{
				return SteamMatchmaking.GetLobbyMemberLimit(m_lobbyId);
			}
			set
			{
				SteamMatchmaking.SetLobbyMemberLimit(m_lobbyId, value);
			}
		}

		public IEnumerable<ulong> MemberList
		{
			get
			{
				for (int i = 0; i < MemberCount; i++)
				{
					yield return (ulong)SteamMatchmaking.GetLobbyMemberByIndex(m_lobbyId, i);
				}
			}
		}

		public bool IsChatAvailable => true;

		public event MessageReceivedDelegate OnChatReceived;

		public event MyLobbyChatUpdated OnChatUpdated;

		public static event Action<IMyLobby, ulong> OnInvitedToLobby;

		public event KickedDelegate OnKicked;

		public event MyLobbyDataUpdated OnDataReceived;

		internal static MySteamLobby GetOrCreate(ulong lobbyId)
		{
			if (m_lobbiesByIds.TryGetValue(lobbyId, out var value))
			{
				return value;
			}
			value = new MySteamLobby(lobbyId);
			m_lobbiesByIds[lobbyId] = value;
			return value;
		}

		static MySteamLobby()
		{
			m_lobbiesByIds = new Dictionary<ulong, MySteamLobby>();
			OnRefreshCompleted = null;
			OnLobbyCreated = null;
			OnLobbyJoined = null;
			MySteamLobby.OnInvitedToLobby = null;
			m_lobbyListResult = Callback<LobbyMatchList_t>.Create(HandleLobbyResponse);
			m_lobbyCreateResult = Callback<LobbyCreated_t>.Create(HandleLobbyCreation);
			m_lobbyEnterResult = Callback<LobbyEnter_t>.Create(HandleLobbyJoin);
			m_lobbyDataUpdateResult = Callback<LobbyDataUpdate_t>.Create(HandleLobbyDataUpdate);
			m_lobbyChatMessageResult = Callback<LobbyChatMsg_t>.Create(HandleChatReceived);
			m_lobbyChatUpdateResult = Callback<LobbyChatUpdate_t>.Create(HandleChatUpdated);
			m_lobbyGameCreatedResult = Callback<LobbyGameCreated_t>.Create(OnGameCreated);
			m_lobbyInviteResult = Callback<LobbyInvite_t>.Create(HandleLobbyInvite);
			m_lobbyKickedResult = Callback<LobbyKicked_t>.Create(HandleKick);
		}

		private MySteamLobby(ulong lobbyID)
		{
			LobbyId = lobbyID;
		}

		public static void RefreshLobbies(Action<bool> completed)
		{
			OnRefreshCompleted = completed;
			SteamMatchmaking.RequestLobbyList();
		}

		private static void HandleLobbyResponse(LobbyMatchList_t result)
		{
			Dictionary<ulong, MySteamLobby> dictionary = new Dictionary<ulong, MySteamLobby>(m_lobbiesByIds);
			for (int i = 0; i < result.m_nLobbiesMatching; i++)
			{
				CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
				GetOrCreate((ulong)lobbyByIndex).RequestData();
				if (dictionary.ContainsKey((ulong)lobbyByIndex))
				{
					dictionary.Remove((ulong)lobbyByIndex);
				}
			}
			foreach (KeyValuePair<ulong, MySteamLobby> item in dictionary)
			{
				m_lobbiesByIds.Remove(item.Key);
			}
			if (OnRefreshCompleted != null)
			{
				OnRefreshCompleted(obj: true);
			}
		}

		public static void AddPublicLobbies(List<IMyLobby> lobbies)
		{
			foreach (KeyValuePair<ulong, MySteamLobby> lobbiesById in m_lobbiesByIds)
			{
				MySteamLobby value = lobbiesById.Value;
				if (value.LobbyType == MyLobbyType.Public)
				{
					lobbies.Add(value);
				}
			}
		}

		public static void AddFriendLobbies(List<IMyLobby> lobbies)
		{
			foreach (KeyValuePair<ulong, MySteamLobby> lobbiesById in m_lobbiesByIds)
			{
				MySteamLobby value = lobbiesById.Value;
				if (value.LobbyType == MyLobbyType.FriendsOnly)
				{
					lobbies.Add(value);
				}
			}
		}

		public static void TryCreateLobby(MyLobbyType lobbyType, uint maxMembers, MyLobbyCreated callback)
		{
			OnLobbyJoined = null;
			OnLobbyCreated = callback;
			SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, (int)maxMembers);
		}

		private static void HandleLobbyCreation(LobbyCreated_t result)
		{
			if (result.m_eResult != EResult.k_EResultOK)
			{
				if (OnLobbyCreated != null)
				{
					OnLobbyCreated(null, success: false, MyLobbyStatusCode.Error);
				}
				return;
			}
			MySteamLobby orCreate = GetOrCreate(result.m_ulSteamIDLobby);
			string personaName = SteamFriends.GetPersonaName();
			orCreate.SetData("name", personaName + "'s game");
			if (OnLobbyCreated != null)
			{
				OnLobbyCreated(orCreate, success: true, MyLobbyStatusCode.Success);
			}
		}

		public static void TryJoinLobby(ulong lobbyId, MyJoinResponseDelegate callback)
		{
			OnLobbyCreated = null;
			OnLobbyJoined = callback;
			SteamMatchmaking.JoinLobby((CSteamID)lobbyId);
		}

		private static void HandleLobbyJoin(LobbyEnter_t result)
		{
			MyLobbyStatusCode eChatRoomEnterResponse = (MyLobbyStatusCode)result.m_EChatRoomEnterResponse;
			if (eChatRoomEnterResponse != MyLobbyStatusCode.Success)
			{
				if (OnLobbyJoined != null)
				{
					OnLobbyJoined(successFlag: false, null, eChatRoomEnterResponse);
				}
				return;
			}
			MySteamLobby mySteamLobby = (ActiveLobby = GetOrCreate(result.m_ulSteamIDLobby));
			ulong ownerId = mySteamLobby.OwnerId;
			ulong num = (ulong)SteamUser.GetSteamID();
			if (mySteamLobby.LobbyType == MyLobbyType.FriendsOnly && ownerId != num && !SteamFriends.HasFriend((CSteamID)ownerId, EFriendFlags.k_EFriendFlagImmediate))
			{
				mySteamLobby.Leave();
				if (OnLobbyJoined != null)
				{
					OnLobbyJoined(successFlag: false, null, MyLobbyStatusCode.FriendsOnly);
				}
			}
			else if (OnLobbyJoined != null)
			{
				OnLobbyJoined(successFlag: true, mySteamLobby, eChatRoomEnterResponse);
			}
		}

		private static void HandleChatReceived(LobbyChatMsg_t result)
		{
			MySteamLobby orCreate = GetOrCreate(result.m_ulSteamIDLobby);
			if (orCreate.OnChatReceived == null)
			{
				return;
			}
			byte[] array = new byte[4096];
			CSteamID pSteamIDUser;
			EChatEntryType peChatEntryType;
			int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(orCreate.m_lobbyId, (int)result.m_iChatID, out pSteamIDUser, array, 4096, out peChatEntryType);
			using (BitStream bitStream = new BitStream())
			{
				bitStream.ResetRead(array, 0, lobbyChatEntry * 8, copy: false);
				string message = bitStream.ReadString();
				long targetId = bitStream.ReadInt64();
				byte channel = bitStream.ReadByte();
				bool num = bitStream.ReadBool();
				string customAuthor = null;
				if (num)
				{
					customAuthor = bitStream.ReadString();
				}
				orCreate.OnChatReceived?.Invoke((ulong)pSteamIDUser, message, channel, targetId, customAuthor);
			}
		}

		private static void HandleChatUpdated(LobbyChatUpdate_t result)
		{
			MySteamLobby orCreate = GetOrCreate(result.m_ulSteamIDLobby);
			if (orCreate.OnChatUpdated != null)
			{
				orCreate.OnChatUpdated(orCreate, result.m_ulSteamIDUserChanged, result.m_ulSteamIDMakingChange, (MyChatMemberStateChangeEnum)result.m_rgfChatMemberStateChange, MyLobbyStatusCode.Success);
			}
		}

		public unsafe bool SendChatMessage(string text, byte channel, long targetId, string customAuthor)
		{
			using (BitStream bitStream = new BitStream(text.Length * 2 + 100))
			{
				bitStream.ResetWrite();
				bitStream.WriteString(text);
				bitStream.WriteInt64(targetId);
				bitStream.WriteByte(channel);
				if (customAuthor == null)
				{
					bitStream.WriteBool(value: false);
				}
				else
				{
					bitStream.WriteBool(value: true);
					bitStream.WriteString(customAuthor);
				}
				int bytePosition = bitStream.BytePosition;
				byte[] array = new byte[bytePosition];
				try
				{
					fixed (byte* destination = array)
					{
						Unsafe.CopyBlockUnaligned(destination, (void*)bitStream.DataPointer, (uint)bytePosition);
					}
				}
				finally
				{
				}
				return SteamMatchmaking.SendLobbyChatMsg(m_lobbyId, array, bytePosition);
			}
		}

		private static void OnGameCreated(LobbyGameCreated_t result)
		{
			GetOrCreate(result.m_ulSteamIDLobby);
		}

		private static void HandleLobbyInvite(LobbyInvite_t result)
		{
			MySteamLobby orCreate = GetOrCreate(result.m_ulSteamIDLobby);
			MySteamLobby.OnInvitedToLobby?.Invoke(orCreate, result.m_ulSteamIDUser);
		}

		private static void HandleKick(LobbyKicked_t result)
		{
			GetOrCreate(result.m_ulSteamIDLobby).OnKicked?.Invoke(result.m_ulSteamIDAdmin, result.m_bKickedDueToDisconnect == 1);
		}

		public ulong GetMemberByIndex(int index)
		{
			return (ulong)SteamMatchmaking.GetLobbyMemberByIndex(m_lobbyId, index);
		}

		public void Join(MyJoinResponseDelegate reponseDelegate)
		{
			TryJoinLobby(LobbyId, reponseDelegate);
		}

		public string GetMemberName(ulong userId)
		{
			return SteamFriends.GetFriendPersonaName((CSteamID)userId);
		}

		public void Leave()
		{
			if (ActiveLobby == this)
			{
				ActiveLobby = null;
			}
			SteamMatchmaking.LeaveLobby(m_lobbyId);
		}

		private static void HandleLobbyDataUpdate(LobbyDataUpdate_t result)
		{
			MySteamLobby orCreate = GetOrCreate(result.m_ulSteamIDLobby);
			if (orCreate.OnDataReceived != null)
			{
				orCreate.OnDataReceived((result.m_bSuccess == 1) ? true : false, orCreate, result.m_ulSteamIDMember);
			}
		}

		public bool RequestData()
		{
			return SteamMatchmaking.RequestLobbyData(m_lobbyId);
		}

		public string GetData(string key)
		{
			return SteamMatchmaking.GetLobbyData(m_lobbyId, key);
		}

		public bool SetData(string key, string value, bool important = true)
		{
			return SteamMatchmaking.SetLobbyData(m_lobbyId, key, value);
		}

		public bool DeleteData(string key)
		{
			return SteamMatchmaking.DeleteLobbyData(m_lobbyId, key);
		}

		public override string ToString()
		{
			return $"[{LobbyId}] Owner Id: {OwnerId}, Lobby type: {LobbyType}";
		}
	}
}
