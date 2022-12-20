using System;
using System.Collections.Generic;
using Sandbox.Engine.Platform;
using VRage;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	/// <summary>
	/// Game Service helper class
	/// </summary>
	public static class MyGameService
	{
		private class MyNullAchievement : IMyAchievement
		{
			public bool IsUnlocked => false;

			public int StatValueInt { get; set; }

			public float StatValueFloat { get; set; }

			public int StatValueConditionBitField { get; set; }

			public event Action OnStatValueChanged;

			public event Action OnUnlocked;

			public void Unlock()
			{
				this.OnUnlocked.InvokeIfNotNull();
			}

			public void IndicateProgress(uint value)
			{
				bool num = StatValueInt != (int)value;
				StatValueInt = (int)value;
				if (num)
				{
					this.OnStatValueChanged.InvokeIfNotNull();
				}
			}
		}

		private static IMyGameService m_gameServiceCache;

		private static IMyNetworking m_networkingCache;
<<<<<<< HEAD

		private static IMyGameServer m_gameServer;

		private static IMyInventoryService m_inventoryCache;

=======

		private static IMyGameServer m_gameServer;

		private static IMyInventoryService m_inventoryCache;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static IMyServerDiscovery m_serverDiscovery;

		private static IMyLobbyDiscovery m_lobbyDiscovery;

		private static IMyMicrophoneService m_microphoneCache;

		public static MyUGCAggregator WorkshopService { get; private set; }

		public static bool AtLeastOneUGCServiceConsented
		{
			get
			{
				foreach (IMyUGCService aggregate in WorkshopService.GetAggregates())
				{
					if (aggregate.IsConsentGiven)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static IMyGameService Service
		{
			get
			{
				EnsureGameService();
				return m_gameServiceCache;
			}
		}

		public static IMyNetworking Networking => EnsureNetworking();

		public static IMyServerDiscovery ServerDiscovery
		{
			get
			{
				EnsureServerDiscovery();
				return m_serverDiscovery;
			}
		}

		public static IMyLobbyDiscovery LobbyDiscovery
		{
			get
			{
				EnsureLobbyDiscovery();
				return m_lobbyDiscovery;
			}
		}

		public static IMyGameServer GameServer
		{
			get
			{
				if (!EnsureGameServer())
				{
					return null;
				}
				return m_gameServer;
			}
		}

		public static IMyPeer2Peer Peer2Peer => EnsureNetworking()?.Peer2Peer;

		public static uint AppId
		{
			get
			{
				if (!EnsureGameService())
				{
					return 0u;
				}
				return m_gameServiceCache.AppId;
			}
		}

		public static bool IsActive
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsActive;
				}
				return false;
			}
		}

		public static bool IsOnline
		{
			get
			{
				if (!EnsureGameService() || !m_gameServiceCache.IsOnline)
				{
					if (EnsureGameServer() && m_gameServer != null)
					{
						return m_gameServer.Running;
					}
					return false;
				}
				return true;
			}
		}

		public static bool IsOverlayBrowserAvailable
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsOverlayBrowserAvailable;
				}
				return false;
			}
		}

		public static bool IsOverlayEnabled
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsOverlayEnabled;
				}
				return false;
			}
		}

		public static bool OwnsGame
		{
			get
			{
				if (EnsureGameService() && IsActive)
				{
					return m_gameServiceCache.OwnsGame;
				}
				return false;
			}
		}

		public static ulong UserId
		{
			get
			{
				if (!EnsureGameService())
				{
					return ulong.MaxValue;
				}
				return m_gameServiceCache.UserId;
			}
			set
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.UserId = value;
				}
			}
		}

		public static string UserName
		{
			get
			{
				string text = null;
				if (EnsureGameService())
				{
					text = m_gameServiceCache.UserName;
				}
				return text ?? string.Empty;
			}
		}

		public static MyGameServiceUniverse UserUniverse
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.UserUniverse;
				}
				return MyGameServiceUniverse.Invalid;
			}
		}

		public static string BranchName
		{
			get
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					return "DedicatedServer";
				}
				if (!IsActive)
				{
					return "SVN";
				}
				if (!EnsureGameService() || string.IsNullOrEmpty(m_gameServiceCache.BranchName))
				{
					return "";
				}
				return m_gameServiceCache.BranchName;
			}
		}

		public static int RecycleTokens
		{
			get
			{
				if (EnsureInventoryService())
				{
					return m_inventoryCache.RecycleTokens;
				}
				return 0;
			}
		}

		public static string BranchNameFriendly
		{
			get
			{
				if (string.IsNullOrEmpty(BranchName))
				{
					return "default";
				}
				return BranchName;
			}
		}

		public static ICollection<MyGameInventoryItem> InventoryItems
		{
			get
			{
				if (!EnsureInventoryService())
				{
					return null;
				}
				return m_inventoryCache.InventoryItems;
			}
		}

		public static IDictionary<int, MyGameInventoryItemDefinition> Definitions
		{
			get
			{
				if (!EnsureInventoryService())
				{
					return null;
				}
				return m_inventoryCache.Definitions;
			}
		}

		public static bool HasGameServer
		{
			get
			{
				if (!EnsureGameServer())
				{
					return false;
				}
				return m_gameServer != null;
			}
		}

		public static bool IsTextChatAvailable => EnsureNetworking()?.Chat.IsTextChatAvailable ?? false;

		public static bool IsVoiceChatAvailable => EnsureNetworking()?.Chat.IsVoiceChatAvailable ?? false;

		public static event MyLobbyJoinRequested LobbyJoinRequested
		{
			add
			{
				if (EnsureLobbyDiscovery())
				{
					m_lobbyDiscovery.OnJoinLobbyRequested += value;
				}
			}
			remove
			{
				if (EnsureLobbyDiscovery())
				{
					m_lobbyDiscovery.OnJoinLobbyRequested -= value;
				}
			}
		}

		public static event MyServerChangeRequested ServerChangeRequested
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnServerChangeRequested += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnServerChangeRequested -= value;
				}
			}
		}

		public static event EventHandler<MyGameServerItem> OnPingServerResponded
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnPingServerResponded += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnPingServerResponded -= value;
				}
			}
		}

		public static event EventHandler OnPingServerFailedToRespond
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnPingServerFailedToRespond += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnPingServerFailedToRespond -= value;
				}
			}
		}

		public static event EventHandler<int> OnFavoritesServerListResponded
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnFavoritesServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnFavoritesServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnFavoritesServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnFavoritesServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnHistoryServerListResponded
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnHistoryServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnHistoryServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnHistoryServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnHistoryServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnLANServerListResponded
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnLANServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnLANServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnLANServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnLANServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnDedicatedServerListResponded
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnDedicatedServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnDedicatedServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse
		{
			add
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnDedicatedServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureServerDiscovery())
				{
					m_serverDiscovery.OnDedicatedServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler InventoryRefreshed
		{
			add
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.InventoryRefreshed += value;
				}
			}
			remove
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.InventoryRefreshed -= value;
				}
			}
		}

		public static event EventHandler<MyGameItemsEventArgs> CheckItemDataReady
		{
			add
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.CheckItemDataReady += value;
				}
			}
			remove
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.CheckItemDataReady -= value;
				}
			}
		}

		public static event EventHandler<MyGameItemsEventArgs> ItemsAdded
		{
			add
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.ItemsAdded += value;
				}
			}
			remove
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.ItemsAdded -= value;
				}
			}
		}

		public static event EventHandler NoItemsReceived
		{
			add
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.NoItemsReceived += value;
				}
			}
			remove
			{
				if (EnsureInventoryService())
				{
					m_inventoryCache.NoItemsReceived -= value;
				}
			}
		}

		public static event Action<bool> OnOverlayActivated
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnOverlayActivated += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnOverlayActivated -= value;
				}
			}
		}

		public static event Action<uint> OnDLCInstalled
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDLCInstalled += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDLCInstalled -= value;
				}
			}
		}

		public static event Action<bool> OnUserChanged
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnUserChanged += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnUserChanged -= value;
				}
			}
		}

		static MyGameService()
		{
			WorkshopService = new MyUGCAggregator();
			MyServiceManager.Instance.OnChanged += OnServiceChanged;
		}

		private static void OnServiceChanged(object obj)
		{
			ClearCache();
		}

		public static void ClearCache()
		{
			m_gameServiceCache = null;
			m_networkingCache = null;
			m_gameServer = null;
			m_inventoryCache = null;
			m_serverDiscovery = null;
			m_lobbyDiscovery = null;
			m_microphoneCache = null;
		}

		private static bool EnsureGameService()
		{
			if (m_gameServiceCache == null)
			{
				m_gameServiceCache = MyServiceManager.Instance.GetService<IMyGameService>();
			}
			return m_gameServiceCache != null;
		}

		private static bool EnsureInventoryService()
		{
			if (m_inventoryCache == null)
			{
				m_inventoryCache = MyServiceManager.Instance.GetService<IMyInventoryService>();
			}
			return m_inventoryCache != null;
		}

		private static bool EnsureLobbyDiscovery()
		{
			if (m_lobbyDiscovery == null)
			{
				m_lobbyDiscovery = MyServiceManager.Instance.GetService<IMyLobbyDiscovery>();
			}
			return m_lobbyDiscovery != null;
		}

		private static bool EnsureServerDiscovery()
<<<<<<< HEAD
		{
			if (m_serverDiscovery == null)
			{
				m_serverDiscovery = MyServiceManager.Instance.GetService<IMyServerDiscovery>();
			}
			return m_serverDiscovery != null;
		}

		private static IMyNetworking EnsureNetworking()
		{
			IMyNetworking myNetworking = m_networkingCache;
			if (myNetworking == null)
			{
				myNetworking = (m_networkingCache = MyServiceManager.Instance.GetService<IMyNetworking>());
				m_networkingCache?.Chat?.UpdateChatAvailability();
			}
			return myNetworking;
		}

		private static bool EnsureMicrophone()
		{
			if (m_microphoneCache == null)
			{
				m_microphoneCache = MyServiceManager.Instance.GetService<IMyMicrophoneService>();
			}
			return m_microphoneCache != null;
		}

		private static bool EnsureGameServer()
		{
			if (m_gameServer == null)
			{
				m_gameServer = MyServiceManager.Instance.GetService<IMyGameServer>();
			}
=======
		{
			if (m_serverDiscovery == null)
			{
				m_serverDiscovery = MyServiceManager.Instance.GetService<IMyServerDiscovery>();
			}
			return m_serverDiscovery != null;
		}

		private static IMyNetworking EnsureNetworking()
		{
			IMyNetworking myNetworking = m_networkingCache;
			if (myNetworking == null)
			{
				myNetworking = (m_networkingCache = MyServiceManager.Instance.GetService<IMyNetworking>());
				m_networkingCache?.Chat?.UpdateChatAvailability();
			}
			return myNetworking;
		}

		private static bool EnsureMicrophone()
		{
			if (m_microphoneCache == null)
			{
				m_microphoneCache = MyServiceManager.Instance.GetService<IMyMicrophoneService>();
			}
			return m_microphoneCache != null;
		}

		private static bool EnsureGameServer()
		{
			if (m_gameServer == null)
			{
				m_gameServer = MyServiceManager.Instance.GetService<IMyGameServer>();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return m_gameServer != null;
		}

		public static IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
			if (!EnsureInventoryService())
			{
				return null;
			}
			return m_inventoryCache.GetDefinitionsForSlot(slot);
		}

		public static void OpenOverlayUrl(string url)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OpenOverlayUrl(url);
			}
		}

		public static void OpenInviteOverlay()
		{
			EnsureNetworking()?.Invite.OpenInviteOverlay();
		}

		public static bool IsInviteSupported()
		{
			return EnsureNetworking()?.Invite.IsInviteSupported() ?? false;
		}

		internal static void SetNotificationPosition(NotificationPosition notificationPosition)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SetNotificationPosition(notificationPosition);
			}
		}

		public static void ShutDown()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ShutDown();
			}
		}

		public static IMyAchievement GetAchievement(string achievementName, string statName, float statMaxValue)
		{
			if (!EnsureGameService())
			{
				return new MyNullAchievement();
			}
			IMyAchievement myAchievement = m_gameServiceCache.GetAchievement(achievementName, statName, statMaxValue);
			if (myAchievement == null)
			{
				MyLog.Default.Error("Achievement " + achievementName + " is not implemented in game service " + m_gameServiceCache.ServiceName);
				myAchievement = new MyNullAchievement();
			}
			return myAchievement;
		}

		public static bool IsAppInstalled(uint appId)
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return false;
			}
			return m_gameServiceCache.IsAppInstalled(appId);
		}

		public static void OpenDlcInShop(uint dlcId)
		{
			if (EnsureGameService() && m_gameServiceCache.IsActive)
			{
				m_gameServiceCache.OpenDlcInShop(dlcId);
			}
		}

		public static void OpenInventoryItemInShop(int itemId)
		{
			if (EnsureGameService() && m_gameServiceCache.IsActive)
			{
				m_gameServiceCache.OpenInventoryItemInShop(itemId);
			}
		}

		public static bool IsDlcInstalled(uint dlcId)
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return false;
			}
			return m_gameServiceCache.IsDlcInstalled(dlcId);
		}

		public static int GetDLCCount()
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return 0;
			}
			return m_gameServiceCache.GetDLCCount();
		}

		public static bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize)
		{
			if (!EnsureGameService())
			{
				dlcId = 0u;
				available = false;
				name = null;
				return false;
			}
			return m_gameServiceCache.GetDLCDataByIndex(index, out dlcId, out available, out name, nameBufferSize);
		}

		public static void OpenOverlayUser(ulong id)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OpenOverlayUser(id);
			}
		}

		public static bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
		{
			length = 0u;
			ticketHandle = 0u;
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.GetAuthSessionTicket(out ticketHandle, buffer, out length);
		}

		public static void PingServer(string connectionString)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.PingServer(connectionString);
			}
		}

		public static void OnThreadpoolInitialized()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OnThreadpoolInitialized();
			}
		}

		public static void LoadStats()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.LoadStats();
			}
		}

		public static void ResetAllStats(bool achievementsToo)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ResetAllStats(achievementsToo);
			}
		}

		public static void StoreStats()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.StoreStats();
			}
		}

		public static void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.GetServerRules(serverItem, completedAction, failedAction);
			}
		}

		public static string GetPersonaName(ulong userId)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetPersonaName(userId);
		}

		public static bool HasFriend(ulong userId)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.HasFriend(userId);
		}

		public static string GetClanName(ulong groupId)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetClanName(groupId);
		}

		public static void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.GetPlayerDetails(serverItem, completedAction, failedAction);
			}
		}

		public static void AddFavoriteGame(string connectionString)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.AddFavoriteGame(connectionString);
			}
		}

		public static void AddFavoriteGame(MyGameServerItem serverItem)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.AddFavoriteGame(serverItem);
			}
		}

		public static void RemoveFavoriteGame(MyGameServerItem serverItem)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RemoveFavoriteGame(serverItem);
			}
		}

		public static void AddHistoryGame(MyGameServerItem serverItem)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.AddHistoryGame(serverItem);
			}
		}

		public static void Update()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.Update();
			}
			WorkshopService.Update();
			if (EnsureInventoryService())
			{
				m_inventoryCache.Update();
			}
		}

		public static bool IsUserInGroup(ulong groupId)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.IsUserInGroup(groupId);
		}

		public static bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
		{
			totalBytes = (availableBytes = 0uL);
			if (!EnsureGameService())
			{
				return true;
			}
			return m_gameServiceCache.GetRemoteStorageQuota(out totalBytes, out availableBytes);
		}

		public static int GetRemoteStorageFileCount()
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.GetRemoteStorageFileCount();
		}

		public static string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
		{
			fileSizeInBytes = 0;
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetRemoteStorageFileNameAndSize(fileIndex, out fileSizeInBytes);
		}

		public static bool IsRemoteStorageFilePersisted(string file)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.IsRemoteStorageFilePersisted(file);
		}

		public static bool RemoteStorageFileForget(string file)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.RemoteStorageFileForget(file);
		}

		internal static void RequestFavoritesServerList(MySessionSearchFilter filterOps)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RequestFavoritesServerList(filterOps);
			}
		}

		internal static void CancelFavoritesServersRequest()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.CancelFavoritesServersRequest();
			}
		}

		internal static MyGameServerItem GetFavoritesServerDetails(int server)
		{
			if (!EnsureServerDiscovery())
			{
				return null;
			}
			return m_serverDiscovery.GetFavoritesServerDetails(server);
		}

		internal static void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RequestServerItems(connectionStrings, filter, resultCallback);
			}
		}

		internal static void RequestInternetServerList(MySessionSearchFilter filterOps)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RequestInternetServerList(filterOps);
			}
		}

		internal static void CancelInternetServersRequest()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.CancelInternetServersRequest();
			}
		}

		internal static MyGameServerItem GetDedicatedServerDetails(int server)
		{
			if (!EnsureServerDiscovery())
			{
				return null;
			}
			return m_serverDiscovery.GetDedicatedServerDetails(server);
		}

		internal static void RequestHistoryServerList(MySessionSearchFilter filterOps)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RequestHistoryServerList(filterOps);
			}
		}

		internal static MyGameServerItem GetHistoryServerDetails(int server)
		{
			if (!EnsureServerDiscovery())
			{
				return null;
			}
			return m_serverDiscovery.GetHistoryServerDetails(server);
		}

		internal static void CancelHistoryServersRequest()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.CancelHistoryServersRequest();
			}
		}

		public static void RequestLANServerList()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.RequestLANServerList();
			}
		}

		public static MyGameServerItem GetLANServerDetails(int server)
		{
			if (!EnsureServerDiscovery())
			{
				return null;
			}
			return m_serverDiscovery.GetLANServerDetails(server);
		}

		public static void CancelLANServersRequest()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.CancelLANServersRequest();
			}
		}

		internal static ulong CreatePublishedFileUpdateRequest(ulong publishedFileId)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.CreatePublishedFileUpdateRequest(publishedFileId);
		}

		internal static void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFileTags(updateHandle, tags);
			}
		}

		internal static void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFileFile(updateHandle, steamItemFileName);
			}
		}

		internal static void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFilePreviewFile(updateHandle, steamPreviewFileName);
			}
		}

		internal static void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CommitPublishedFileUpdate(updateHandle, onCallResult);
			}
		}

		internal static void FileDelete(string steamItemFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileDelete(steamItemFileName);
			}
		}

		internal static void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.PublishWorkshopFile(file, previewFile, title, description, longDescription, visibility, tags, onCallResult);
			}
		}

		internal static void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SubscribePublishedFile(publishedFileId, onCallResult);
			}
		}

		internal static bool FileExists(string fileName)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.FileExists(fileName);
		}

		internal static int GetFileSize(string fileName)
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.GetFileSize(fileName);
		}

		internal static ulong FileWriteStreamOpen(string fileName)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.FileWriteStreamOpen(fileName);
		}

		internal static void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileWriteStreamWriteChunk(handle, buffer, size);
			}
		}

		internal static void FileWriteStreamClose(ulong handle)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileWriteStreamClose(handle);
			}
		}

		internal static void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileShare(file, onCallResult);
			}
		}

		internal static void GetAllInventoryItems()
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.GetAllItems();
			}
		}

		internal static MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId)
		{
			if (!EnsureInventoryService())
			{
				return null;
			}
			return m_inventoryCache.GetInventoryItemDefinition(assetModifierId);
		}

		internal static bool HasInventoryItemWithDefinitionId(int id)
		{
			if (!EnsureInventoryService())
			{
				return false;
			}
			return m_inventoryCache.HasInventoryItemWithDefinitionId(id);
		}

		internal static bool HasInventoryItem(ulong id)
		{
			if (!EnsureInventoryService())
			{
				return false;
			}
			return m_inventoryCache.HasInventoryItem(id);
		}

		internal static bool HasInventoryItem(string assetModifierId)
		{
			if (!EnsureInventoryService())
			{
				return false;
			}
			return m_inventoryCache.HasInventoryItem(assetModifierId);
		}

		internal static void TriggerPersonalContainer()
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.TriggerPersonalContainer();
			}
		}

		internal static void TriggerCompetitiveContainer()
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.TriggerCompetitiveContainer();
			}
		}

		internal static void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.GetItemCheckData(item, completedAction);
			}
		}

		internal static void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.GetItemsCheckData(items, completedAction);
			}
		}

		internal static List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			if (!EnsureInventoryService())
			{
				checkResult = false;
				return null;
			}
			return m_inventoryCache.CheckItemData(checkData, out checkResult);
		}

		internal static void ConsumeItem(MyGameInventoryItem item)
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.ConsumeItem(item);
			}
		}

		internal static void JoinLobby(ulong lobbyId, MyJoinResponseDelegate reponseDelegate)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.JoinLobby(lobbyId, reponseDelegate);
			}
		}

		internal static IMyLobby CreateLobby(ulong lobbyId)
		{
			if (!EnsureLobbyDiscovery())
			{
				return null;
			}
			return m_lobbyDiscovery.CreateLobby(lobbyId);
		}

		internal static void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.CreateLobby(type, maxPlayers, createdResponse);
			}
		}

		internal static void AddFriendLobbies(List<IMyLobby> lobbies)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.AddFriendLobbies(lobbies);
			}
		}

		internal static void AddPublicLobbies(List<IMyLobby> lobbies)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.AddPublicLobbies(lobbies);
			}
		}

		internal static void RequestLobbyList(Action<bool> completed)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.RequestLobbyList(completed);
			}
		}

		internal static void AddLobbyFilter(string key, string value)
		{
			if (EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.AddLobbyFilter(key, value);
			}
		}

		internal static int GetChatMaxMessageSize()
		{
			return EnsureNetworking()?.Chat.GetChatMaxMessageSize() ?? MyGameServiceConstants.MAX_CHAT_MESSAGE_SIZE;
		}

		internal static MyGameServiceAccountType GetServerAccountType(ulong steamId)
		{
			if (!EnsureGameService())
			{
				return MyGameServiceAccountType.Invalid;
			}
			return m_gameServiceCache.GetServerAccountType(steamId);
		}

		internal static void SetServerModTemporaryDirectory()
		{
			if (EnsureGameServer())
			{
				m_gameServer.SetServerModTemporaryDirectory();
			}
		}

		public static void InitializeVoiceRecording()
		{
			if (EnsureMicrophone())
			{
				m_microphoneCache.InitializeVoiceRecording();
			}
		}

		internal static void DisposeVoiceRecording()
		{
			if (EnsureMicrophone())
			{
				m_microphoneCache.DisposeVoiceRecording();
			}
		}

		public static void StartVoiceRecording()
		{
			if (EnsureMicrophone())
			{
				m_microphoneCache.StartVoiceRecording();
			}
		}

		public static void StopVoiceRecording()
		{
			if (EnsureMicrophone())
			{
				m_microphoneCache.StopVoiceRecording();
			}
		}

		public static byte[] GetVoiceDataFormat()
		{
			if (!EnsureMicrophone())
			{
				return null;
			}
			return m_microphoneCache.GetVoiceDataFormat();
		}

		public static bool IsMicrophoneFilteringSilence()
		{
			if (!EnsureMicrophone())
			{
				return false;
			}
			return m_microphoneCache.FiltersSilence;
		}

		public static MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size)
		{
			if (!EnsureMicrophone())
			{
				size = 0u;
				return MyVoiceResult.NotInitialized;
			}
			return m_microphoneCache.GetAvailableVoice(ref buffer, out size);
		}

		public static MyPlayerChatState GetPlayerMutedState(ulong playerId)
		{
			if (IsPlayerMutedInCloud(playerId))
			{
				return MyPlayerChatState.Muted;
			}
			return EnsureNetworking()?.Chat.GetPlayerChatState(playerId) ?? MyPlayerChatState.Silent;
		}

		internal static bool IsPlayerMutedInCloud(ulong playerId)
		{
			if (EnsureGameService())
			{
				return m_gameServiceCache.IsPlayerMuted(playerId);
			}
			return false;
		}

		public static void UpdateMutedPlayersFromCloud(Action onDone = null)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdateMutedPlayers(onDone);
			}
			else
			{
				onDone.InvokeIfNotNull();
			}
		}

		internal static void SetPlayerMuted(ulong playerId, bool muted)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SetPlayerMuted(playerId, muted);
			}
			EnsureNetworking()?.Chat.SetPlayerMuted(playerId, muted);
		}

		internal static bool RecycleItem(MyGameInventoryItem item)
		{
			if (!EnsureInventoryService())
			{
				return false;
			}
			return m_inventoryCache.RecycleItem(item);
		}

		internal static bool CraftSkin(MyGameInventoryItemQuality quality)
		{
			if (!EnsureInventoryService())
			{
				return false;
			}
			return m_inventoryCache.CraftSkin(quality);
		}

		internal static uint GetCraftingCost(MyGameInventoryItemQuality quality)
		{
			if (!EnsureInventoryService())
			{
				return 0u;
			}
			return m_inventoryCache.GetCraftingCost(quality);
		}

		internal static uint GetRecyclingReward(MyGameInventoryItemQuality quality)
		{
			if (!EnsureInventoryService())
			{
				return 0u;
			}
			return m_inventoryCache.GetRecyclingReward(quality);
		}

		public static int GetFriendsCount()
		{
			if (!EnsureGameService())
			{
				return -1;
			}
			return m_gameServiceCache.GetFriendsCount();
		}

		public static ulong GetFriendIdByIndex(int index)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.GetFriendIdByIndex(index);
		}

		public static string GetFriendNameByIndex(int index)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetFriendNameByIndex(index);
		}

		public static void SaveToCloudAsync(string fileName, byte[] buffer, Action<CloudResult> completedAction)
		{
			if (!EnsureGameService())
			{
				completedAction?.Invoke(CloudResult.Failed);
			}
			m_gameServiceCache.SaveToCloudAsync(fileName, buffer, completedAction);
		}

		public static CloudResult SaveToCloud(string fileName, byte[] buffer)
		{
			if (!EnsureGameService())
			{
				return CloudResult.Failed;
			}
			return m_gameServiceCache.SaveToCloud(fileName, buffer);
		}

		public static CloudResult SaveToCloud(string containerName, List<MyCloudFile> files)
		{
			if (!EnsureGameService())
			{
				return CloudResult.Failed;
			}
			return m_gameServiceCache.SaveToCloud(containerName, files);
		}

		public static List<MyCloudFileInfo> GetCloudFiles(string directoryFilter)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetCloudFiles(directoryFilter);
		}

		public static byte[] LoadFromCloud(string fileName)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.LoadFromCloud(fileName);
		}

		public static bool LoadFromCloudAsync(string fileName, Action<byte[]> onDone)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.LoadFromCloudAsync(fileName, onDone);
		}

		public static bool DeleteFromCloud(string fileName)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.DeleteFromCloud(fileName);
		}

		public static bool IsUpdateAvailable()
		{
			return false;
		}

		public static MyWorkshopItem CreateWorkshopItem(string serviceName)
		{
			return GetUGC(serviceName)?.CreateWorkshopItem();
		}

		public static MyWorkshopItemPublisher CreateWorkshopPublisher(string serviceName)
		{
			return GetUGC(serviceName)?.CreateWorkshopPublisher();
		}

		public static MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return WorkshopService.CreateWorkshopPublisher(item);
		}

		public static MyWorkshopQuery CreateWorkshopQuery(string serviceName)
		{
			return GetUGC(serviceName)?.CreateWorkshopQuery();
		}

		public static IMyUGCService GetUGC(string serviceName)
		{
			return WorkshopService.GetAggregate(serviceName);
		}

		public static bool IsProductOwned(uint productId, out DateTime? purchaseTime)
		{
			purchaseTime = null;
			if (!EnsureGameService())
			{
				return false;
<<<<<<< HEAD
			}
			return m_gameServiceCache.IsProductOwned(productId, out purchaseTime);
		}

		public static void SuspendWorkshopDownloads()
		{
			WorkshopService.SuspendWorkshopDownloads();
		}

		public static void ResumeWorkshopDownloads()
		{
			WorkshopService.ResumeWorkshopDownloads();
		}

		public static void AddUnownedItems()
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.AddUnownedItems();
			}
		}

		public static void OpenInShop(string shopUrl)
		{
			string[] array = shopUrl.Split(new char[1] { ':' });
			if (array.Length == 2 && uint.TryParse(array[1], out var result))
			{
				if (array[0] == "app")
				{
					OpenDlcInShop(result);
				}
				else
				{
					OpenInventoryItemInShop((int)result);
				}
			}
		}

		public static void Trace(bool enable)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.Trace(enable);
			}
		}

		public static void AddDlcPackages(List<MyDlcPackage> packages)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddDlcPackages(packages);
			}
		}

		public static void ConnectToServer(MyGameServerItem server, Action<JoinResult> onDone)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.Connect(server, onDone);
			}
		}

		public static void DisconnectFromServer()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.Disconnect();
			}
		}

		public static void UpdateNetworkThread(bool sessionEnabled)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdateNetworkThread(sessionEnabled);
=======
			}
			return m_gameServiceCache.IsProductOwned(productId, out purchaseTime);
		}

		public static void SuspendWorkshopDownloads()
		{
			WorkshopService.SuspendWorkshopDownloads();
		}

		public static void ResumeWorkshopDownloads()
		{
			WorkshopService.ResumeWorkshopDownloads();
		}

		public static void AddUnownedItems()
		{
			if (EnsureInventoryService())
			{
				m_inventoryCache.AddUnownedItems();
			}
		}

		public static void OpenInShop(string shopUrl)
		{
			string[] array = shopUrl.Split(new char[1] { ':' });
			if (array.Length == 2 && uint.TryParse(array[1], out var result))
			{
				if (array[0] == "app")
				{
					OpenDlcInShop(result);
				}
				else
				{
					OpenInventoryItemInShop((int)result);
				}
			}
		}

		public static void Trace(bool enable)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.Trace(enable);
			}
		}

		public static void AddDlcPackages(List<MyDlcPackage> packages)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddDlcPackages(packages);
			}
		}

		public static void ConnectToServer(MyGameServerItem server, Action<JoinResult> onDone)
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.Connect(server, onDone);
			}
		}

		public static void DisconnectFromServer()
		{
			if (EnsureServerDiscovery())
			{
				m_serverDiscovery.Disconnect();
			}
		}

		public static void UpdateNetworkThread()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdateNetworkThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void OnInvite(string dataProtocol)
		{
			bool flag = false;
			if (EnsureServerDiscovery())
			{
				flag = m_serverDiscovery.OnInvite(dataProtocol);
			}
			if (!flag && EnsureLobbyDiscovery())
			{
				m_lobbyDiscovery.OnInvite(dataProtocol);
			}
		}

		public static void RequestPermissions(Permissions permission, bool attemptResolution, Action<PermissionResult> onDone)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestPermissions(permission, attemptResolution, onDone);
			}
			else
			{
				onDone.InvokeIfNotNull(PermissionResult.Error);
			}
		}

		public static void RequestPermissionsWithTargetUser(Permissions permission, ulong userId, Action<PermissionResult> onDone)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestPermissionsWithTargetUser(permission, userId, onDone);
			}
			else
			{
				onDone.InvokeIfNotNull(PermissionResult.Error);
			}
		}

		public static IMyUGCService GetDefaultUGC()
		{
			return WorkshopService.GetAggregates()[0];
		}

		public static string[] GetUGCNamesList()
		{
			return WorkshopService.GetAggregates().ConvertAll((IMyUGCService x) => x.ServiceName).ToArray();
		}

		public static int GetUGCIndex(string serviceName)
		{
			return WorkshopService.GetAggregates().FindIndex((IMyUGCService x) => x.ServiceName == serviceName);
		}
	}
}
