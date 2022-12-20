using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Multiplayer
{
	/// <summary>
	/// Container of multiplayer classes
	/// </summary>
	public sealed class MyMultiplayerLobby : MyMultiplayerServerBase
	{
		private IMyLobby m_lobby;

		private bool m_serverDataValid;

		protected override bool IsServerInternal => true;

		public override string WorldName
		{
			get
			{
				return GetLobbyWorldName(m_lobby);
			}
			set
			{
				m_lobby.SetData("world", value ?? "Unnamed");
			}
		}

		public override MyGameModeEnum GameMode
		{
			get
			{
				return GetLobbyGameMode(m_lobby);
			}
			set
			{
				IMyLobby lobby = m_lobby;
				int num = (int)value;
				lobby.SetData("gameMode", num.ToString());
			}
		}

		public override float InventoryMultiplier
		{
			get
			{
				return GetLobbyFloat("inventoryMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("inventoryMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override float BlocksInventoryMultiplier
		{
			get
			{
				return GetLobbyFloat("blocksInventoryMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("blocksInventoryMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override float AssemblerMultiplier
		{
			get
			{
				return GetLobbyFloat("assemblerMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("assemblerMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override float RefineryMultiplier
		{
			get
			{
				return GetLobbyFloat("refineryMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("refineryMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override float WelderMultiplier
		{
			get
			{
				return GetLobbyFloat("welderMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("welderMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override float GrinderMultiplier
		{
			get
			{
				return GetLobbyFloat("grinderMultiplier", m_lobby, 1f);
			}
			set
			{
				m_lobby.SetData("grinderMultiplier", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override string HostName
		{
			get
			{
				return GetLobbyHostName(m_lobby);
			}
			set
			{
				m_lobby.SetData("host", value);
			}
		}

		public override ulong WorldSize
		{
			get
			{
				return GetLobbyWorldSize(m_lobby);
			}
			set
			{
				m_lobby.SetData("worldSize", value.ToString());
			}
		}

		public override int AppVersion
		{
			get
			{
				return GetLobbyAppVersion(m_lobby);
			}
			set
			{
				m_lobby.SetData("appVersion", value.ToString());
			}
		}

		public override string DataHash
		{
			get
			{
				return m_lobby.GetData("dataHash");
			}
			set
			{
				m_lobby.SetData("dataHash", value);
			}
		}

		public static int MAX_PLAYERS => MyPlatformGameSettings.LOBBY_MAX_PLAYERS;

		public override int MaxPlayers => MAX_PLAYERS;

		public override int ModCount
		{
			get
			{
				return GetLobbyModCount(m_lobby);
			}
			protected set
			{
				m_lobby.SetData("mods", value.ToString());
			}
		}

		public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get
			{
				return GetLobbyMods(m_lobby);
			}
			set
			{
				ModCount = value.Count;
				int num = ModCount - 1;
				foreach (MyObjectBuilder_Checkpoint.ModItem item in value)
				{
					string value2 = item.PublishedFileId + "_" + item.PublishedServiceName + "_" + item.FriendlyName;
					m_lobby.SetData("mod" + num--, value2, important: false);
				}
			}
		}

		public override int ViewDistance
		{
			get
			{
				return GetLobbyViewDistance(m_lobby);
			}
			set
			{
				m_lobby.SetData("view", value.ToString());
			}
		}

		public override int SyncDistance
		{
			get
			{
				return MyLayers.GetSyncDistance();
			}
			set
			{
				MyLayers.SetSyncDistance(value);
			}
		}

		public override bool Scenario
		{
			get
			{
				return GetLobbyBool("scenario", m_lobby, defValue: false);
			}
			set
			{
				m_lobby.SetData("scenario", value.ToString());
			}
		}

		public override string ScenarioBriefing
		{
			get
			{
				return m_lobby.GetData("scenarioBriefing");
			}
			set
			{
				m_lobby.SetData("scenarioBriefing", (value == null || value.Length < 1) ? " " : value);
			}
		}

		public override DateTime ScenarioStartTime
		{
			get
			{
				return GetLobbyDateTime("scenarioStartTime", m_lobby, DateTime.MinValue);
			}
			set
			{
				m_lobby.SetData("scenarioStartTime", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public ulong HostSteamId
		{
			get
			{
				return GetLobbyULong("host_steamId", m_lobby, 0uL);
			}
			set
			{
				m_lobby.SetData("host_steamId", value.ToString());
			}
		}

		public override bool ExperimentalMode
		{
			get
			{
				return GetLobbyBool("experimentalMode", m_lobby, defValue: false);
			}
			set
			{
				m_lobby.SetData("experimentalMode", value.ToString());
			}
		}

		public override IEnumerable<ulong> Members => m_lobby.MemberList;

		public override int MemberCount => m_lobby.MemberCount;

		public override bool IsSomeoneElseConnected => MemberCount >= 2;

		public override ulong LobbyId => m_lobby.LobbyId;

		public override int MemberLimit
		{
			get
			{
				return m_lobby.MemberLimit;
			}
			set
			{
				SetMemberLimit(value);
			}
		}

		public override bool IsLobby => true;

		public event MyLobbyDataUpdated OnLobbyDataUpdated;

		public MyMultiplayerLobby(MySyncLayer syncLayer, MyLobbyType lobbyType, uint maxPlayers, MyLobbyCreated action)
			: base(syncLayer, new EndpointId(Sync.MyId))
		{
			MyGameService.CreateLobby(lobbyType, maxPlayers, action);
			MyGameService.Networking.Chat.UpdateChatAvailability();
		}

		public void SetLobby(IMyLobby lobby)
		{
			m_lobby = lobby;
			base.ServerId = m_lobby.OwnerId;
			SyncLayer.RegisterClientEvents(this);
			HostName = MyGameService.UserName;
			lobby.OnChatUpdated += Matchmaking_LobbyChatUpdate;
			lobby.OnChatReceived += Matchmaking_LobbyChatMsg;
			lobby.OnDataReceived += lobby_OnDataReceived;
			base.ClientLeft += MyMultiplayerLobby_ClientLeft;
			AcceptMemberSessions();
		}

		private void lobby_OnDataReceived(bool success, IMyLobby lobby, ulong memberOrLobby)
		{
			this.OnLobbyDataUpdated?.Invoke(success, lobby, memberOrLobby);
		}

		private void MyMultiplayerLobby_ClientLeft(ulong userId, MyChatMemberStateChangeEnum stateChange)
		{
			if (userId == base.ServerId)
			{
				MyGameService.Peer2Peer.CloseSession(userId);
			}
			if (stateChange == MyChatMemberStateChangeEnum.Kicked || stateChange == MyChatMemberStateChangeEnum.Banned)
			{
				MyControlKickClientMsg myControlKickClientMsg = default(MyControlKickClientMsg);
				myControlKickClientMsg.KickedClient = userId;
				myControlKickClientMsg.Kicked = true;
				myControlKickClientMsg.Add = false;
				MyControlKickClientMsg message = myControlKickClientMsg;
				MyLog.Default.WriteLineAndConsole("Player " + GetMemberName(userId) + " kicked");
				SendControlMessageToAll(ref message, 0uL);
			}
			MySandboxGame.Log.WriteLineAndConsole("Player left: " + GetMemberName(userId) + " (" + userId + ")");
		}

		public bool IsCorrectVersion()
		{
			return IsLobbyCorrectVersion(m_lobby);
		}

		private void Matchmaking_LobbyChatUpdate(IMyLobby lobby, ulong changedUser, ulong makingChangeUser, MyChatMemberStateChangeEnum stateChange, MyLobbyStatusCode reason)
		{
			if (lobby.LobbyId != m_lobby.LobbyId)
			{
				return;
			}
			if (stateChange == MyChatMemberStateChangeEnum.Entered)
			{
				if (MyVisualScriptLogicProvider.PlayerConnectRequest != null)
				{
					JoinResult result = JoinResult.OK;
					MyVisualScriptLogicProvider.PlayerConnectRequest(changedUser, ref result);
					if (result != 0)
					{
						RaiseClientLeft(changedUser, MyChatMemberStateChangeEnum.Disconnected);
						return;
					}
				}
				MyGameService.Networking.Chat.WarmupPlayerCache(changedUser, IsCrossMember(changedUser));
				string memberName = GetMemberName(changedUser);
				MySandboxGame.Log.WriteLineAndConsole("Player entered: " + memberName + " (" + changedUser + ")");
				MyGameService.Peer2Peer.AcceptSession(changedUser);
				if (Sync.Clients == null || !Sync.Clients.HasClient(changedUser))
				{
					RaiseClientJoined(changedUser, memberName);
					if (Scenario && changedUser != Sync.MyId)
					{
						SendAllMembersDataToClient(changedUser);
					}
				}
				if (MySandboxGame.IsGameReady && changedUser != base.ServerId)
				{
					MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientConnected, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					myHudNotification.SetTextFormatArguments(memberName);
					MyHud.Notifications.Add(myHudNotification);
				}
			}
			else
			{
				if (Sync.Clients == null || Sync.Clients.HasClient(changedUser))
				{
					RaiseClientLeft(changedUser, stateChange);
				}
				if (changedUser == base.ServerId)
				{
					RaiseHostLeft();
					StringBuilder errorMessage = MyJoinGameHelper.GetErrorMessage(reason);
					MyLog.Default.WriteLine(string.Concat("Matchmaking_LobbyChatUpdate: ", reason, " / ", errorMessage));
					MySessionLoader.UnloadAndExitToMenu();
					StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, errorMessage, messageCaption));
				}
				else if (MySandboxGame.IsGameReady)
				{
					MyHudNotification myHudNotification2 = new MyHudNotification(MyCommonTexts.NotificationClientDisconnected, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					string memberName2 = GetMemberName(changedUser);
					myHudNotification2.SetTextFormatArguments(memberName2);
					MyHud.Notifications.Add(myHudNotification2);
				}
			}
		}

		private void Matchmaking_LobbyChatMsg(ulong memberId, string message, byte channel, long targetId, string author)
		{
			RaiseChatMessageReceived(memberId, message, (ChatChannel)channel, targetId, author);
		}

		private void AcceptMemberSessions()
		{
			foreach (ulong member in m_lobby.MemberList)
			{
				if (member != Sync.MyId && member == base.ServerId)
				{
					MyGameService.Peer2Peer.AcceptSession(member);
				}
			}
		}

		public override void DisconnectClient(ulong userId)
		{
			MyLog.Default.WriteLineAndConsole("User forcibly disconnected " + GetMemberName(userId));
			RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Disconnected);
		}

		public override void BanClient(ulong userId, bool banned)
		{
		}

		public override void Tick()
		{
			base.Tick();
			if (!m_serverDataValid)
			{
				if (AppVersion == 0)
				{
					MySession.Static.StartServer(this);
				}
				m_serverDataValid = true;
			}
		}

		public override void SendChatMessage(string text, ChatChannel channel, long targetId, string customAuthor)
		{
			m_lobby.SendChatMessage(text, (byte)channel, targetId, customAuthor);
		}

		public override void Dispose()
		{
			if (m_lobby != null)
			{
				m_lobby.OnChatUpdated -= Matchmaking_LobbyChatUpdate;
				m_lobby.OnChatReceived -= Matchmaking_LobbyChatMsg;
				m_lobby.OnDataReceived -= lobby_OnDataReceived;
				if (m_lobby.IsValid)
				{
					CloseMemberSessions();
					m_lobby.Leave();
				}
			}
			base.Dispose();
			m_lobby = null;
		}

		public override ulong GetOwner()
		{
			return m_lobby.OwnerId;
		}

		public override MyLobbyType GetLobbyType()
		{
			return m_lobby.LobbyType;
		}

		public override void SetLobbyType(MyLobbyType type)
		{
			m_lobby.LobbyType = type;
		}

		public override void SetMemberLimit(int limit)
		{
			m_lobby.MemberLimit = limit;
		}

		public static bool IsLobbyCorrectVersion(IMyLobby lobby)
		{
			return GetLobbyAppVersion(lobby) == (int)MyFinalBuildConstants.APP_VERSION;
		}

		public static MyGameModeEnum GetLobbyGameMode(IMyLobby lobby)
		{
			if (int.TryParse(lobby.GetData("gameMode"), out var result))
			{
				return (MyGameModeEnum)result;
			}
			return MyGameModeEnum.Creative;
		}

		public static float GetLobbyFloat(string key, IMyLobby lobby, float defValue)
		{
			if (float.TryParse(lobby.GetData(key), NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defValue;
		}

		public static int GetLobbyInt(string key, IMyLobby lobby, int defValue)
		{
			if (int.TryParse(lobby.GetData(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return defValue;
		}

		public static DateTime GetLobbyDateTime(string key, IMyLobby lobby, DateTime defValue)
		{
			if (DateTime.TryParse(lobby.GetData(key), CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
			{
				return result;
			}
			return defValue;
		}

		public static long GetLobbyLong(string key, IMyLobby lobby, long defValue)
		{
			if (long.TryParse(lobby.GetData(key), out var result))
			{
				return result;
			}
			return defValue;
		}

		public static ulong GetLobbyULong(string key, IMyLobby lobby, ulong defValue)
		{
			if (ulong.TryParse(lobby.GetData(key), out var result))
			{
				return result;
			}
			return defValue;
		}

		public static bool GetLobbyBool(string key, IMyLobby lobby, bool defValue)
		{
			if (bool.TryParse(lobby.GetData(key), out var result))
			{
				return result;
			}
			return defValue;
		}

		public static string GetLobbyWorldName(IMyLobby lobby)
		{
			return lobby.GetData("world");
		}

		public static ulong GetLobbyWorldSize(IMyLobby lobby)
		{
			if (ulong.TryParse(lobby.GetData("worldSize"), out var result))
			{
				return result;
			}
			return 0uL;
		}

		public static string GetLobbyHostName(IMyLobby lobby)
		{
			return lobby.GetData("host");
		}

		public static int GetLobbyAppVersion(IMyLobby lobby)
		{
			if (!int.TryParse(lobby.GetData("appVersion"), out var result))
			{
				return 0;
			}
			return result;
		}

		public static ulong GetLobbyHostSteamId(IMyLobby lobby)
		{
			return GetLobbyULong("host_steamId", lobby, 0uL);
		}

		public static string GetDataHash(IMyLobby lobby)
		{
			return lobby.GetData("dataHash");
		}

		public static bool HasSameData(IMyLobby lobby)
		{
			string dataHash = GetDataHash(lobby);
			if (dataHash == "")
			{
				return true;
			}
			if (dataHash == MyDataIntegrityChecker.GetHashBase64())
			{
				return true;
			}
			return false;
		}

		public static int GetLobbyModCount(IMyLobby lobby)
		{
			return GetLobbyInt("mods", lobby, 0);
		}

		public static List<MyObjectBuilder_Checkpoint.ModItem> GetLobbyMods(IMyLobby lobby)
		{
			int lobbyModCount = GetLobbyModCount(lobby);
			List<MyObjectBuilder_Checkpoint.ModItem> list = new List<MyObjectBuilder_Checkpoint.ModItem>(lobbyModCount);
			for (int i = 0; i < lobbyModCount; i++)
			{
				string text = lobby.GetData("mod" + i) ?? string.Empty;
				int num = text.IndexOf("_");
				if (num != -1)
				{
					ulong.TryParse(text.Substring(0, num), out var result);
					string text2 = text.Substring(num + 1);
					num = text2.IndexOf("_");
					if (num != -1)
					{
						string publishedServiceName = text2.Substring(0, num);
						string text3 = text2.Substring(num + 1);
						list.Add(new MyObjectBuilder_Checkpoint.ModItem(text3, result, publishedServiceName, text3));
						continue;
					}
				}
				MySandboxGame.Log.WriteLineAndConsole("Failed to parse mod details from LobbyData. '" + text + "'");
			}
			return list;
		}

		public static int GetLobbyViewDistance(IMyLobby lobby)
		{
			return GetLobbyInt("view", lobby, 20000);
		}

		public static bool GetLobbyScenario(IMyLobby lobby)
		{
			return GetLobbyBool("scenario", lobby, defValue: false);
		}

		public static string GetLobbyScenarioBriefing(IMyLobby lobby)
		{
			return lobby.GetData("scenarioBriefing");
		}

		public override string GetMemberName(ulong steamUserID)
		{
			return m_lobby.GetMemberName(steamUserID);
		}

		public override string GetMemberServiceName(ulong steamUserID)
		{
			return MyGameService.Service.ServiceName;
		}

		protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
		{
		}
	}
}
