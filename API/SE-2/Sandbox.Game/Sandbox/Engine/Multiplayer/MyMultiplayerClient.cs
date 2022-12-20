using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
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
	internal sealed class MyMultiplayerClient : MyMultiplayerClientBase
	{
		private string m_worldName;

		private MyGameModeEnum m_gameMode;

		private float m_inventoryMultiplier;

		private float m_blocksInventoryMultiplier;

		private float m_assemblerMultiplier;

		private float m_refineryMultiplier;

		private float m_welderMultiplier;

		private float m_grinderMultiplier;

		private string m_hostName;

		private string m_hostAnalyticsId;

		private ulong m_worldSize;

		private int m_appVersion;

		private int m_membersLimit;

		private string m_dataHash;

		private string m_serverPasswordSalt;

		private bool m_sessionClosed;

		private readonly List<ulong> m_members = new List<ulong>();

		private readonly Dictionary<ulong, MyConnectedClientData> m_memberData = new Dictionary<ulong, MyConnectedClientData>();

		public Action OnJoin;

		private List<MyObjectBuilder_Checkpoint.ModItem> m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();

		protected override bool IsServerInternal => false;

		public override string WorldName
		{
			get
			{
				return m_worldName;
			}
			set
			{
				m_worldName = value;
			}
		}

		public override MyGameModeEnum GameMode
		{
			get
			{
				return m_gameMode;
			}
			set
			{
				m_gameMode = value;
			}
		}

		public override float InventoryMultiplier
		{
			get
			{
				return m_inventoryMultiplier;
			}
			set
			{
				m_inventoryMultiplier = value;
			}
		}

		public override float BlocksInventoryMultiplier
		{
			get
			{
				return m_blocksInventoryMultiplier;
			}
			set
			{
				m_blocksInventoryMultiplier = value;
			}
		}

		public override float AssemblerMultiplier
		{
			get
			{
				return m_assemblerMultiplier;
			}
			set
			{
				m_assemblerMultiplier = value;
			}
		}

		public override float RefineryMultiplier
		{
			get
			{
				return m_refineryMultiplier;
			}
			set
			{
				m_refineryMultiplier = value;
			}
		}

		public override float WelderMultiplier
		{
			get
			{
				return m_welderMultiplier;
			}
			set
			{
				m_welderMultiplier = value;
			}
		}

		public override float GrinderMultiplier
		{
			get
			{
				return m_grinderMultiplier;
			}
			set
			{
				m_grinderMultiplier = value;
			}
		}

		public override string HostName
		{
			get
			{
				return m_hostName;
			}
			set
			{
				m_hostName = value;
			}
		}

		public string HostAnalyticsId => m_hostAnalyticsId;

		public override ulong WorldSize
		{
			get
			{
				return m_worldSize;
			}
			set
			{
				m_worldSize = value;
			}
		}

		public override int AppVersion
		{
			get
			{
				return m_appVersion;
			}
			set
			{
				m_appVersion = value;
			}
		}

		public override string DataHash
		{
			get
			{
				return m_dataHash;
			}
			set
			{
				m_dataHash = value;
			}
		}

		public override int MaxPlayers => 65536;

		public override int ModCount { get; protected set; }

		public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get
			{
				return m_mods;
			}
			set
			{
				m_mods = value;
				ModCount = m_mods.Count;
			}
		}

		public override int ViewDistance { get; set; }

		public override bool Scenario { get; set; }

		public override string ScenarioBriefing { get; set; }

		public override DateTime ScenarioStartTime { get; set; }

		public MyGameServerItem Server { get; private set; }

		public override bool ExperimentalMode { get; set; }

		public override IEnumerable<ulong> Members => m_members;

		public override int MemberCount => m_members.Count;

		public override bool IsSomeoneElseConnected => MemberCount >= 3;

		public override ulong LobbyId => 0uL;

		public override int MemberLimit
		{
			get
			{
				return m_membersLimit;
			}
			set
			{
				m_membersLimit = value;
			}
		}

		public string DisconnectReason { get; private set; }

		internal MyMultiplayerClient(MyGameServerItem server, MySyncLayer syncLayer)
			: base(syncLayer)
		{
			SyncLayer.RegisterClientEvents(this);
			SyncLayer.TransportLayer.IsBuffering = true;
			Server = server;
			base.ServerId = server.SteamID;
			base.ClientLeft += MyMultiplayerClient_ClientLeft;
			syncLayer.TransportLayer.Register(MyMessageId.JOIN_RESULT, 0, OnJoinResult);
			syncLayer.TransportLayer.Register(MyMessageId.WORLD_DATA, 0, OnWorldData);
			syncLayer.TransportLayer.Register(MyMessageId.CLIENT_CONNECTED, 0, OnClientConnected);
			base.ClientJoined += MyMultiplayerClient_ClientJoined;
			base.HostLeft += MyMultiplayerClient_HostLeft;
			MyGameService.ConnectToServer(server, delegate(JoinResult joinResult)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					OnConnectToServer(joinResult);
				}, "OnConnectToServer");
			});
			MyGameService.Peer2Peer.ConnectionFailed += Peer2Peer_ConnectionFailed;
		}

		private void OnConnectToServer(JoinResult joinResult)
		{
			if (!m_sessionClosed)
			{
				if (joinResult == JoinResult.OK)
				{
					MyGameService.Networking.Chat.UpdateChatAvailability();
					SendPlayerData(MyGameService.UserName);
				}
				else
				{
					JoinResultMsg joinResultMsg = default(JoinResultMsg);
					joinResultMsg.JoinResult = joinResult;
					JoinResultMsg msg = joinResultMsg;
					OnUserJoined(ref msg);
				}
				DisconnectReason = null;
			}
		}

		private void OnWorldData(MyPacket packet)
		{
			ServerDataMsg msg = base.ReplicationLayer.OnWorldData(packet);
			OnServerData(ref msg);
			packet.Return();
		}

		private void OnJoinResult(MyPacket packet)
		{
			JoinResultMsg msg = base.ReplicationLayer.OnJoinResult(packet);
			OnUserJoined(ref msg);
			packet.Return();
		}

		private void OnClientConnected(MyPacket packet)
		{
			ConnectedClientDataMsg msg = base.ReplicationLayer.OnClientConnected(packet);
			Sync.ClientConnected(packet.Sender.Id.Value, msg.Name);
			OnConnectedClient(ref msg, msg.ClientId);
			packet.Return();
		}

		public override void Dispose()
		{
			if (!m_sessionClosed)
			{
				CloseClient();
			}
			base.Dispose();
		}

		private void MyMultiplayerClient_HostLeft()
		{
			CloseSession();
			MySessionLoader.UnloadAndExitToMenu();
			MyGuiScreenServerReconnector.ReconnectToLastSession();
		}

		private void MyMultiplayerClient_ClientLeft(ulong user, MyChatMemberStateChangeEnum stateChange)
		{
			if (user == base.ServerId)
			{
				RaiseHostLeft();
				return;
			}
			if (m_members.Contains(user))
			{
				m_members.Remove(user);
				MyConnectedClientData value;
				string text = (m_memberData.TryGetValue(user, out value) ? value.Name : "Unknown");
				MySandboxGame.Log.WriteLineAndConsole("Player disconnected: " + text + " (" + EndpointId.Format(user) + ")");
				if (MySandboxGame.IsGameReady && Sync.MyId != user)
				{
					MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientDisconnected, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					myHudNotification.SetTextFormatArguments(text);
					MyHud.Notifications.Add(myHudNotification);
				}
			}
			m_memberData.Remove(user);
		}

		private void MyMultiplayerClient_ClientJoined(ulong user, string userName)
		{
			if (!m_members.Contains(user))
			{
				m_members.Add(user);
			}
		}

		private void Peer2Peer_ConnectionFailed(ulong remoteUserId, string error)
		{
			DisconnectReason = error;
			if (remoteUserId == base.ServerId)
			{
				RaiseHostLeft();
			}
		}

		public bool IsCorrectVersion()
		{
			return m_appVersion == (int)MyFinalBuildConstants.APP_VERSION;
		}

		public override void DisconnectClient(ulong userId)
		{
			if (!m_sessionClosed)
			{
				CloseClient();
			}
		}

		public override void BanClient(ulong client, bool ban)
		{
			MyControlBanClientMsg myControlBanClientMsg = default(MyControlBanClientMsg);
			myControlBanClientMsg.BannedClient = client;
			myControlBanClientMsg.Banned = ban;
			MyControlBanClientMsg message = myControlBanClientMsg;
			SendControlMessage(base.ServerId, ref message);
		}

		private void CloseClient()
		{
			MyControlDisconnectedMsg myControlDisconnectedMsg = default(MyControlDisconnectedMsg);
			myControlDisconnectedMsg.Client = Sync.MyId;
			MyControlDisconnectedMsg message = myControlDisconnectedMsg;
			SendControlMessage(base.ServerId, ref message);
			OnJoin = null;
			Thread.Sleep(200);
			CloseSession();
		}

		private void CloseSession()
		{
			OnJoin = null;
			if (MyGameService.Networking != null)
			{
				MyGameService.Peer2Peer.ConnectionFailed -= Peer2Peer_ConnectionFailed;
				MyGameService.Peer2Peer.CloseSession(base.ServerId);
				MyGameService.DisconnectFromServer();
			}
			m_sessionClosed = true;
		}

		public override ulong GetOwner()
		{
			return base.ServerId;
		}

		public override MyLobbyType GetLobbyType()
		{
			return MyLobbyType.Public;
		}

		public override void SetLobbyType(MyLobbyType myLobbyType)
		{
		}

		public override void SetMemberLimit(int limit)
		{
			m_membersLimit = limit;
		}

		public override void OnChatMessage(ref ChatMsg msg)
		{
			bool flag = false;
			if (m_memberData.ContainsKey(msg.Author) && (m_memberData[msg.Author].IsAdmin || flag))
			{
				MyClientDebugCommands.Process(msg.Text, msg.Author);
			}
			RaiseChatMessageReceived(msg.Author, msg.Text, (ChatChannel)msg.Channel, msg.TargetId, msg.CustomAuthorName ?? string.Empty);
		}

		public override void SendChatMessage(string text, ChatChannel channel, long targetId, string customAuthor)
		{
			if (channel != ChatChannel.GlobalScripted)
			{
				ChatMsg chatMsg = default(ChatMsg);
				chatMsg.Text = text;
				chatMsg.Author = Sync.MyId;
				chatMsg.Channel = (byte)channel;
				chatMsg.TargetId = targetId;
				chatMsg.CustomAuthorName = string.Empty;
				ChatMsg msg = chatMsg;
				OnChatMessage(ref msg);
				MyMultiplayerBase.SendChatMessage(ref msg);
			}
		}

		private void OnServerData(ref ServerDataMsg msg)
		{
			m_worldName = msg.WorldName;
			m_gameMode = msg.GameMode;
			m_inventoryMultiplier = msg.InventoryMultiplier;
			m_blocksInventoryMultiplier = msg.BlocksInventoryMultiplier;
			m_assemblerMultiplier = msg.AssemblerMultiplier;
			m_refineryMultiplier = msg.RefineryMultiplier;
			m_welderMultiplier = msg.WelderMultiplier;
			m_grinderMultiplier = msg.GrinderMultiplier;
			m_hostName = msg.HostName;
			m_hostAnalyticsId = msg.ServerAnalyticsId;
			m_worldSize = msg.WorldSize;
			m_appVersion = msg.AppVersion;
			m_membersLimit = msg.MembersLimit;
			m_dataHash = msg.DataHash;
			m_serverPasswordSalt = msg.ServerPasswordSalt;
		}

		private void OnUserJoined(ref JoinResultMsg msg)
		{
			if (msg.JoinResult == JoinResult.OK)
			{
				base.IsServerExperimental = msg.ServerExperimental;
				if (OnJoin != null)
				{
					OnJoin();
					OnJoin = null;
				}
				return;
			}
			if (msg.JoinResult == JoinResult.NotInGroup)
			{
				MySessionLoader.UnloadAndExitToMenu();
				Dispose();
				ulong groupId = Server.GetGameTagByPrefixUlong("groupId");
				string clanName = MyGameService.GetClanName(groupId);
				MyGuiScreenMessageBox obj = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerErrorNotInGroup), clanName)));
				obj.ResultCallback = delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyGameService.OpenOverlayUser(groupId);
					}
				};
				MyGuiSandbox.AddScreen(obj);
				return;
			}
			if (msg.JoinResult == JoinResult.BannedByAdmins)
			{
				MySessionLoader.UnloadAndExitToMenu();
				Dispose();
				ulong admin = msg.Admin;
				if (admin != 0L)
				{
					MyGuiScreenMessageBox obj2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorBannedByAdminsWithDialog));
					obj2.ResultCallback = delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							MyGameService.OpenOverlayUser(admin);
						}
					};
					MyGuiSandbox.AddScreen(obj2);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorBannedByAdmins)));
				}
				return;
			}
			StringBuilder messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorConnectionFailed);
			switch (msg.JoinResult)
			{
			case JoinResult.AlreadyJoined:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorAlreadyJoined);
				break;
			case JoinResult.ServerFull:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorServerFull);
				break;
			case JoinResult.SteamServersOffline:
				messageText = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MultiplayerErrorSteamServersOffline), MySession.GameServiceName);
				break;
			case JoinResult.TicketInvalid:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketInvalid);
				break;
			case JoinResult.GroupIdInvalid:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorGroupIdInvalid);
				break;
			case JoinResult.KickedRecently:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorKickedByAdmins);
				break;
			case JoinResult.TicketCanceled:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketCanceled);
				break;
			case JoinResult.TicketAlreadyUsed:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketAlreadyUsed);
				break;
			case JoinResult.LoggedInElseWhere:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorLoggedInElseWhere);
				break;
			case JoinResult.NoLicenseOrExpired:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorNoLicenseOrExpired);
				break;
			case JoinResult.UserNotConnected:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorUserNotConnected);
				break;
			case JoinResult.VACBanned:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorVACBanned);
				break;
			case JoinResult.VACCheckTimedOut:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorVACCheckTimedOut);
				break;
			case JoinResult.FamilySharing:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerError_FamilyShareDisabled);
				break;
			case JoinResult.PasswordRequired:
				MyGuiSandbox.AddScreen(new MyGuiScreenServerPassword(delegate(string password)
				{
					SendPasswordHash(password);
				}));
				return;
			case JoinResult.WrongPassword:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorWrongPassword);
				break;
			case JoinResult.ExperimentalMode:
				messageText = ((!MySandboxGame.Config.ExperimentalMode) ? MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental) : MyTexts.Get(MyCommonTexts.MultiplayerErrorNotExperimental));
				break;
			case JoinResult.ProfilingNotAllowed:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorProfilingNotAllowed);
				break;
			case JoinResult.NotFound:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorNotFound);
				break;
			case JoinResult.IncorrectTime:
				messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorIncorrectTime);
				break;
			}
			Dispose();
			MySessionLoader.UnloadAndExitToMenu();
			StringBuilder messageCaption4 = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, messageCaption4));
		}

		public void SendPasswordHash(string password)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(m_serverPasswordSalt))
			{
				MyLog.Default.Error("Empty password salt on the server.");
				return;
			}
<<<<<<< HEAD
			byte[] salt = Convert.FromBase64String(m_serverPasswordSalt.PadRight(m_serverPasswordSalt.Length + (4 - m_serverPasswordSalt.Length % 4) % 4, '='));
			byte[] bytes = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(20);
=======
			byte[] array = Convert.FromBase64String(m_serverPasswordSalt);
			byte[] bytes = ((DeriveBytes)new Rfc2898DeriveBytes(password, array, 10000)).GetBytes(20);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyControlSendPasswordHashMsg myControlSendPasswordHashMsg = default(MyControlSendPasswordHashMsg);
			myControlSendPasswordHashMsg.PasswordHash = bytes;
			MyControlSendPasswordHashMsg message = myControlSendPasswordHashMsg;
			SendControlMessage(base.ServerId, ref message);
		}

		public void LoadMembersFromWorld(List<MyObjectBuilder_Client> clients)
		{
			if (clients == null)
			{
				return;
			}
			foreach (MyObjectBuilder_Client client in clients)
			{
				m_memberData.Add(client.SteamId, new MyConnectedClientData
				{
					Name = client.Name,
					IsAdmin = client.IsAdmin,
					ServiceName = client.ClientService
				});
				RaiseClientJoined(client.SteamId, client.Name);
			}
		}

		public override string GetMemberName(ulong steamUserID)
		{
			m_memberData.TryGetValue(steamUserID, out var value);
			return value.Name;
		}

		public override string GetMemberServiceName(ulong steamUserID)
		{
			m_memberData.TryGetValue(steamUserID, out var value);
			return value.ServiceName;
		}

		private void OnConnectedClient(ref ConnectedClientDataMsg msg, EndpointId playerId)
		{
			MySandboxGame.Log.WriteLineAndConsole(string.Concat("Client connected: ", msg.Name, " (", playerId, ")"));
			if (MySandboxGame.IsGameReady && playerId.Value != base.ServerId && Sync.MyId != playerId.Value && msg.Join)
			{
				MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientConnected, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
				myHudNotification.SetTextFormatArguments(msg.Name);
				MyHud.Notifications.Add(myHudNotification);
			}
			m_memberData[playerId.Value] = new MyConnectedClientData
			{
				Name = msg.Name,
				IsAdmin = msg.IsAdmin,
				IsProfiling = msg.IsProfiling,
				ServiceName = msg.ServiceName
			};
			MyGameService.Networking.Chat.WarmupPlayerCache(playerId.Value, IsCrossMember(playerId.Value));
			RaiseClientJoined(playerId.Value, msg.Name);
		}

		private void SendPlayerData(string clientName)
		{
			ConnectedClientDataMsg connectedClientDataMsg = default(ConnectedClientDataMsg);
			connectedClientDataMsg.Name = clientName;
			connectedClientDataMsg.Join = true;
			connectedClientDataMsg.ExperimentalMode = ExperimentalMode;
			connectedClientDataMsg.IsProfiling = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
			connectedClientDataMsg.ServiceName = MyGameService.Service.ServiceName;
			ConnectedClientDataMsg msg = connectedClientDataMsg;
			byte[] array = new byte[1024];
			if (!MyGameService.GetAuthSessionTicket(out var _, array, out var length))
			{
				MySessionLoader.UnloadAndExitToMenu();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorConnectionFailed)));
			}
			else
			{
				msg.Token = new byte[length];
				Array.Copy(array, msg.Token, length);
				base.ReplicationLayer.SendClientConnected(ref msg);
			}
		}

		protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
		{
			if (data.BannedClient == Sync.MyId && (bool)data.Banned)
			{
				MySessionLoader.UnloadAndExitToMenu();
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionKicked), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextYouHaveBeenBanned)));
				return;
			}
			if ((bool)data.Banned)
			{
				AddBannedClient(data.BannedClient);
			}
			else
			{
				RemoveBannedClient(data.BannedClient);
			}
			if (m_members.Contains(data.BannedClient) && (bool)data.Banned)
			{
				RaiseClientLeft(data.BannedClient, MyChatMemberStateChangeEnum.Banned);
			}
		}

		public override void StartProcessingClientMessagesWithEmptyWorld()
		{
			if (!Sync.Clients.HasClient(base.ServerId))
			{
				Sync.Clients.AddClient(base.ServerId, HostName);
			}
			base.StartProcessingClientMessagesWithEmptyWorld();
			if (Sync.Clients.LocalClient == null)
			{
				Sync.Clients.SetLocalSteamId(Sync.MyId, !Sync.Clients.HasClient(Sync.MyId), MyGameService.UserName);
			}
		}

		protected override void OnAllMembersData(ref AllMembersDataMsg msg)
		{
			if (msg.Clients != null)
			{
				foreach (MyObjectBuilder_Client client in msg.Clients)
				{
					if (!m_memberData.ContainsKey(client.SteamId))
					{
						m_memberData.Add(client.SteamId, new MyConnectedClientData
						{
							Name = client.Name,
							IsAdmin = client.IsAdmin,
							ServiceName = client.ClientService
						});
					}
					if (!m_members.Contains(client.SteamId))
					{
						m_members.Add(client.SteamId);
					}
					if (!Sync.Clients.HasClient(client.SteamId))
					{
						Sync.Clients.AddClient(client.SteamId, client.Name);
					}
				}
			}
			ProcessAllMembersData(ref msg);
		}
	}
}
