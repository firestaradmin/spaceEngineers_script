using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using VRage.GameServices;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamService : IDisposable, IMyGameService
	{
		public static ulong OFFLINE_STEAM_ID = 1234567891011uL;

		private readonly ConcurrentDictionary<string, Action<bool, MyRemoteStorageFileShareResult>> m_fileShareCompleteActions = new ConcurrentDictionary<string, Action<bool, MyRemoteStorageFileShareResult>>();

		private readonly ConcurrentDictionary<ulong, Action<bool, MyRemoteStorageSubscribePublishedFileResult>> m_subscribePublishedFileActions = new ConcurrentDictionary<ulong, Action<bool, MyRemoteStorageSubscribePublishedFileResult>>();

		private readonly MySteamRemoteStorage m_remoteStorage;

		private readonly ConcurrentQueue<Action> m_updateThreadInvocationQueue = new ConcurrentQueue<Action>();

		internal AppId_t SteamAppId;

		internal CSteamID SteamUserId;

		private Callback<GameOverlayActivated_t> m_overlayActivated;

		private CallResult<RemoteStorageUpdatePublishedFileResult_t> m_commitPublishedFileCallResult;

		private Action<bool, MyRemoteStorageUpdatePublishedFileResult> m_commitPublishedFileAction;

		private CallResult<RemoteStoragePublishFileResult_t> m_publishFileCallResult;

		private Action<bool, MyRemoteStoragePublishFileResult> m_publishedFileAction;

		private Callback<DlcInstalled_t> m_dlcInstalled;

		private CallResult<EncryptedAppTicketResponse_t> m_encryptedAppTicketCallback;

		private readonly HashSet<string> m_knownAchievements = new HashSet<string>();

		private readonly ConcurrentDictionary<ulong, CallResult<RemoteStorageSubscribePublishedFileResult_t>> m_subscribePublishedFileResults = new ConcurrentDictionary<ulong, CallResult<RemoteStorageSubscribePublishedFileResult_t>>();

		private readonly ConcurrentDictionary<string, CallResult<RemoteStorageFileShareResult_t>> m_fileShareResults = new ConcurrentDictionary<string, CallResult<RemoteStorageFileShareResult_t>>();

		public static MySteamService Static { get; private set; }

		public bool IsActive { get; private set; }

		public bool IsOnline
		{
			get
			{
				if (UserId != OFFLINE_STEAM_ID && IsActive)
				{
					return SteamUser.BLoggedOn();
				}
				return false;
			}
		}

		public bool IsOverlayEnabled
		{
			get
			{
				if (IsActive)
				{
					return SteamUtils.IsOverlayEnabled();
				}
				return false;
			}
		}

		public bool IsOverlayBrowserAvailable => IsOverlayEnabled;

		public uint AppId { get; private set; }

		public ulong UserId { get; set; }

		public string UserName { get; private set; }

		public string SerialKey { get; private set; }

		public MyGameServiceUniverse UserUniverse { get; private set; }

		public string BranchName { get; private set; }

		public bool OwnsGame { get; private set; }

		public string BranchNameFriendly
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

		public string ServiceName => "Steam";

		public event Action<bool> OnOverlayActivated;

		public event Action<uint> OnDLCInstalled;

		public event Action<bool> OnUserChanged;

		public event Action OnUpdate;

		public event Action<bool> OnUpdateNetworkThread;

		private event Action<bool, string> m_onEncryptedAppTicketResponse;

		public MySteamService(bool isDedicated, uint appId)
		{
			AppId = appId;
			SteamAppId = (AppId_t)AppId;
			Static = this;
			if (!isDedicated)
			{
				IsActive = SteamAPI.Init();
				if (SteamAPI.RestartAppIfNecessary(SteamAppId))
				{
					Environment.Exit(0);
				}
				IMyInventoryService serviceInstance;
				if (IsActive)
				{
					SteamUserId = SteamUser.GetSteamID();
					UserId = (ulong)SteamUserId;
					UserName = SteamFriends.GetPersonaName();
					EUserHasLicenseForAppResult eUserHasLicenseForAppResult = SteamUser.UserHasLicenseForApp(SteamUserId, SteamAppId);
					OwnsGame = eUserHasLicenseForAppResult == EUserHasLicenseForAppResult.k_EUserHasLicenseResultHasLicense;
					UserUniverse = (MyGameServiceUniverse)SteamUtils.GetConnectedUniverse();
					string pchName;
					bool currentBetaName = SteamApps.GetCurrentBetaName(out pchName, 512);
					BranchName = (currentBetaName ? pchName : "default");
					SteamUserStats.RequestCurrentStats();
					RegisterCallbacks();
					m_remoteStorage = new MySteamRemoteStorage();
					serviceInstance = new MySteamInventory(this);
				}
				else
				{
					serviceInstance = new MyNullInventoryService();
				}
				MyServiceManager.Instance.AddService(serviceInstance);
				if (!IsOnline)
				{
					UserId = OFFLINE_STEAM_ID;
				}
			}
		}

		private void RegisterCallbacks()
		{
			m_overlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			m_dlcInstalled = Callback<DlcInstalled_t>.Create(DLCInstalled);
			m_commitPublishedFileCallResult = CallResult<RemoteStorageUpdatePublishedFileResult_t>.Create(OnCommitPublishedFileComplete);
			m_publishFileCallResult = CallResult<RemoteStoragePublishFileResult_t>.Create(OnPublishFileComplete);
			m_encryptedAppTicketCallback = CallResult<EncryptedAppTicketResponse_t>.Create(OnEncryptedAppTicketResponse);
		}

		private void UnregisterCallbacks()
		{
			if (m_commitPublishedFileCallResult != null)
			{
				m_commitPublishedFileCallResult.Dispose();
			}
			if (m_publishFileCallResult != null)
			{
				m_publishFileCallResult.Dispose();
			}
			if (m_overlayActivated != null)
			{
				m_overlayActivated.Unregister();
			}
			if (m_dlcInstalled != null)
			{
				m_dlcInstalled.Unregister();
			}
			if (m_encryptedAppTicketCallback != null)
			{
				m_encryptedAppTicketCallback.Dispose();
			}
			if (m_remoteStorage != null)
			{
				m_remoteStorage.UnregisterCallbacks();
			}
		}

		public void Update()
		{
			if (IsActive)
			{
				SteamAPI.RunCallbacks();
			}
			Action result;
			while (m_updateThreadInvocationQueue.TryDequeue(out result))
			{
				result();
			}
			this.OnUpdate?.Invoke();
		}

		public void UpdateNetworkThread(bool sessionEnabled)
		{
			this.OnUpdateNetworkThread?.Invoke(sessionEnabled);
		}

		public void InvokeOnMainThread(Action action)
		{
			m_updateThreadInvocationQueue.Enqueue(action);
		}

		public void SetNotificationPosition(NotificationPosition position)
		{
			if (IsActive)
			{
				SteamUtils.SetOverlayNotificationPosition((ENotificationPosition)position);
			}
		}

		public void Dispose()
		{
			if (MyLog.Default != null)
			{
				MyLog.Default.WriteLine("Steam closed");
			}
			if (IsActive)
			{
				SteamAPI.Shutdown();
			}
			SteamAppId = AppId_t.Invalid;
			SteamUserId = CSteamID.Nil;
			IsActive = false;
			UserId = 0uL;
			UserName = string.Empty;
			OwnsGame = false;
			UserUniverse = MyGameServiceUniverse.Invalid;
			BranchName = null;
			UnregisterCallbacks();
		}

		public void OpenOverlayUrl(string url)
		{
			if (IsActive)
			{
				SteamFriends.ActivateGameOverlayToWebPage(url);
			}
		}

		public void ShutDown()
		{
			Dispose();
		}

		public bool IsAppInstalled(uint appId)
		{
			return SteamApps.BIsAppInstalled((AppId_t)appId);
		}

		public void OpenDlcInShop(uint dlcId)
		{
			OpenOverlayUrl($"https://store.steampowered.com/app/{dlcId}");
		}

		public void OpenInventoryItemInShop(int itemId)
		{
			OpenOverlayUrl($"https://store.steampowered.com/itemstore/{SteamAppId}/detail/{itemId}");
		}

		public void AddDlcPackages(List<MyDlcPackage> packages)
		{
		}

		public bool IsDlcSupported(uint dlcId)
		{
			return SteamApps.BIsDlcInstalled((AppId_t)dlcId);
		}

		public bool IsDlcInstalled(uint dlcId)
		{
			return SteamApps.BIsDlcInstalled((AppId_t)dlcId);
		}

		public int GetDLCCount()
		{
			return SteamApps.GetDLCCount();
		}

		public bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize)
		{
			AppId_t pAppID;
			bool result = SteamApps.BGetDLCDataByIndex(index, out pAppID, out available, out name, nameBufferSize);
			dlcId = (uint)pAppID;
			return result;
		}

		private void DLCInstalled(DlcInstalled_t param)
		{
			this.OnDLCInstalled.InvokeIfNotNull((uint)param.m_nAppID);
		}

		public void OpenOverlayUser(ulong id)
		{
			if (IsActive)
			{
				SteamFriends.ActivateGameOverlayToUser(null, (CSteamID)id);
			}
		}

		public bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
		{
			HAuthTicket authSessionTicket = SteamUser.GetAuthSessionTicket(buffer, buffer.Length, out length);
			ticketHandle = (uint)authSessionTicket;
			return authSessionTicket != HAuthTicket.Invalid;
		}

		public void LoadStats()
		{
			SteamUserStats.RequestCurrentStats();
		}

		private void CheckKnownAchievement(string achievement)
		{
			if (!m_knownAchievements.Contains(achievement))
			{
				throw new Exception("Achievement " + achievement + " is not registered. Call RegisterAchievement before querying it");
			}
		}

		public IMyAchievement GetAchievement(string achievementName, string statName, float maxValue)
		{
			CheckKnownAchievement(achievementName);
			return new MySteamAchievement(achievementName, statName, maxValue);
		}

		public IMyAchievement GetAchievement(string achievementName)
		{
			CheckKnownAchievement(achievementName);
			return new MySteamAchievement(achievementName, "", 0f);
		}

		public void RegisterAchievement(string achievementName, string XBLId)
		{
			m_knownAchievements.Add(achievementName);
		}

		public void ResetAllStats(bool achievementsToo)
		{
			SteamUserStats.ResetAllStats(achievementsToo);
		}

		public void StoreStats()
		{
			SteamUserStats.StoreStats();
		}

		public string GetPersonaName(ulong userId)
		{
			return SteamFriends.GetFriendPersonaName((CSteamID)userId);
		}

		public bool HasFriend(ulong userId)
		{
			return SteamFriends.HasFriend((CSteamID)userId, EFriendFlags.k_EFriendFlagImmediate);
		}

		public string GetClanName(ulong groupId)
		{
			return SteamFriends.GetClanName((CSteamID)groupId);
		}

		public bool IsUserInGroup(ulong groupId)
		{
			if (groupId == 0L)
			{
				return true;
			}
			return SteamFriends.IsUserInSource(SteamUserId, (CSteamID)groupId);
		}

		public bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
		{
			return SteamRemoteStorage.GetQuota(out totalBytes, out availableBytes);
		}

		public int GetRemoteStorageFileCount()
		{
			return SteamRemoteStorage.GetFileCount();
		}

		public string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
		{
			return SteamRemoteStorage.GetFileNameAndSize(fileIndex, out fileSizeInBytes);
		}

		public bool IsRemoteStorageFilePersisted(string file)
		{
			return SteamRemoteStorage.FilePersisted(file);
		}

		public bool RemoteStorageFileForget(string file)
		{
			return SteamRemoteStorage.FileForget(file);
		}

		public ulong CreatePublishedFileUpdateRequest(ulong publishedFileId)
		{
			return SteamRemoteStorage.CreatePublishedFileUpdateRequest((PublishedFileId_t)publishedFileId).m_PublishedFileUpdateHandle;
		}

		public void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
		{
			SteamRemoteStorage.UpdatePublishedFileTags((PublishedFileUpdateHandle_t)updateHandle, tags.ToList());
		}

		public void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
		{
			SteamRemoteStorage.UpdatePublishedFileFile((PublishedFileUpdateHandle_t)updateHandle, steamItemFileName);
		}

		public void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName)
		{
			SteamRemoteStorage.UpdatePublishedFilePreviewFile((PublishedFileUpdateHandle_t)updateHandle, steamPreviewFileName);
		}

		public void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
		{
			m_commitPublishedFileAction = onCallResult;
			SteamAPICall_t hAPICall = SteamRemoteStorage.CommitPublishedFileUpdate((PublishedFileUpdateHandle_t)updateHandle);
			m_commitPublishedFileCallResult.Set(hAPICall);
		}

		private void OnCommitPublishedFileComplete(RemoteStorageUpdatePublishedFileResult_t param, bool bIOFailure)
		{
			if (m_commitPublishedFileAction != null)
			{
				MyRemoteStorageUpdatePublishedFileResult arg = default(MyRemoteStorageUpdatePublishedFileResult);
				arg.PublishedFileId = param.m_nPublishedFileId.m_PublishedFileId;
				arg.Result = (MyGameServiceCallResult)param.m_eResult;
				m_commitPublishedFileAction(bIOFailure, arg);
				m_commitPublishedFileAction = null;
			}
		}

		public void FileDelete(string steamItemFileName)
		{
			SteamRemoteStorage.FileDelete(steamItemFileName);
		}

		public void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
		{
			m_publishedFileAction = onCallResult;
			SteamAPICall_t hAPICall = SteamRemoteStorage.PublishWorkshopFile(file, previewFile, SteamAppId, title, description, (ERemoteStoragePublishedFileVisibility)visibility, tags, EWorkshopFileType.k_EWorkshopFileTypeFirst);
			m_publishFileCallResult.Set(hAPICall);
		}

		private void OnPublishFileComplete(RemoteStoragePublishFileResult_t param, bool bIOFailure)
		{
			if (m_publishedFileAction != null)
			{
				MyRemoteStoragePublishFileResult arg = default(MyRemoteStoragePublishFileResult);
				arg.PublishedFileId = param.m_nPublishedFileId.m_PublishedFileId;
				arg.Result = (MyGameServiceCallResult)param.m_eResult;
				m_publishedFileAction(bIOFailure, arg);
				m_publishedFileAction = null;
			}
		}

		public void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
		{
			m_subscribePublishedFileActions.TryAdd(publishedFileId, onCallResult);
			SteamAPICall_t hAPICall = SteamRemoteStorage.SubscribePublishedFile((PublishedFileId_t)publishedFileId);
			CallResult<RemoteStorageSubscribePublishedFileResult_t> callResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnSubscribePublishedFile);
			callResult.Set(hAPICall);
			m_subscribePublishedFileResults.TryAdd(publishedFileId, callResult);
		}

		private void OnSubscribePublishedFile(RemoteStorageSubscribePublishedFileResult_t param, bool bIOFailure)
		{
			Action<bool, MyRemoteStorageSubscribePublishedFileResult> value = null;
			if (m_subscribePublishedFileActions.TryGetValue(param.m_nPublishedFileId.m_PublishedFileId, out value))
			{
				MyRemoteStorageSubscribePublishedFileResult arg = default(MyRemoteStorageSubscribePublishedFileResult);
				arg.PublishedFileId = param.m_nPublishedFileId.m_PublishedFileId;
				arg.Result = (MyGameServiceCallResult)param.m_eResult;
				value(bIOFailure, arg);
				m_subscribePublishedFileActions.Remove(param.m_nPublishedFileId.m_PublishedFileId);
				CallResult<RemoteStorageSubscribePublishedFileResult_t> value2 = null;
				if (m_subscribePublishedFileResults.TryGetValue(param.m_nPublishedFileId.m_PublishedFileId, out value2))
				{
					value2.Dispose();
					m_subscribePublishedFileResults.Remove(param.m_nPublishedFileId.m_PublishedFileId);
				}
			}
		}

		public bool FileExists(string fileName)
		{
			return SteamRemoteStorage.FileExists(fileName);
		}

		public int GetFileSize(string fileName)
		{
			return SteamRemoteStorage.GetFileSize(fileName);
		}

		public ulong FileWriteStreamOpen(string fileName)
		{
			return SteamRemoteStorage.FileWriteStreamOpen(fileName).m_UGCFileWriteStreamHandle;
		}

		public void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
		{
			SteamRemoteStorage.FileWriteStreamWriteChunk((UGCFileWriteStreamHandle_t)handle, buffer, size);
		}

		public void FileWriteStreamClose(ulong handle)
		{
			SteamRemoteStorage.FileWriteStreamClose((UGCFileWriteStreamHandle_t)handle);
		}

		public void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult)
		{
			m_fileShareCompleteActions.TryAdd(file, onCallResult);
			SteamAPICall_t hAPICall = SteamRemoteStorage.FileShare(file);
			CallResult<RemoteStorageFileShareResult_t> callResult = CallResult<RemoteStorageFileShareResult_t>.Create(OnFileShareResultComplete);
			callResult.Set(hAPICall);
			m_fileShareResults.TryAdd(file, callResult);
		}

		private void OnFileShareResultComplete(RemoteStorageFileShareResult_t param, bool bIOFailure)
		{
			Action<bool, MyRemoteStorageFileShareResult> value = null;
			if (m_fileShareCompleteActions.TryGetValue(param.m_rgchFilename, out value))
			{
				MyRemoteStorageFileShareResult arg = default(MyRemoteStorageFileShareResult);
				arg.FileHandle = param.m_hFile.m_UGCHandle;
				arg.Result = (MyGameServiceCallResult)param.m_eResult;
				value(bIOFailure, arg);
				m_fileShareCompleteActions.Remove(param.m_rgchFilename);
				CallResult<RemoteStorageFileShareResult_t> value2 = null;
				if (m_fileShareResults.TryGetValue(param.m_rgchFilename, out value2))
				{
					value2.Dispose();
					m_fileShareResults.Remove(param.m_rgchFilename);
				}
			}
		}

		public void OnGameOverlayActivated(GameOverlayActivated_t callback)
		{
			this.OnOverlayActivated.InvokeIfNotNull(callback.m_bActive != 0);
		}

		public int GetFriendsCount()
		{
			return SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
		}

		public ulong GetFriendIdByIndex(int index)
		{
			return SteamFriends.GetFriendByIndex(index, EFriendFlags.k_EFriendFlagImmediate).m_SteamID;
		}

		public string GetFriendNameByIndex(int index)
		{
			return SteamFriends.GetFriendPersonaName(SteamFriends.GetFriendByIndex(index, EFriendFlags.k_EFriendFlagImmediate));
		}

		public void SaveToCloudAsync(string storageName, byte[] buffer, Action<CloudResult> completedAction)
		{
			if (m_remoteStorage == null)
			{
				completedAction?.Invoke(CloudResult.Failed);
			}
			else
			{
				m_remoteStorage.SaveBufferAsync(storageName, buffer, completedAction);
			}
		}

		public CloudResult SaveToCloud(string fileName, byte[] buffer)
		{
			return m_remoteStorage?.SaveBuffer(fileName, buffer) ?? CloudResult.Failed;
		}

		public CloudResult SaveToCloud(string containerName, List<MyCloudFile> files)
		{
			if (m_remoteStorage == null || files == null)
			{
				return CloudResult.Failed;
			}
			ulong num = 0uL;
			foreach (MyCloudFile file in files)
			{
				FileInfo fileInfo = new FileInfo(file.FileName);
				num += (ulong)fileInfo.Length;
			}
			CloudResult cloudResult = MySteamRemoteStorage.IsCorrectSize(num);
			if (cloudResult != 0)
			{
				return cloudResult;
			}
			foreach (MyCloudFile file2 in files)
			{
				string fileName = containerName + "/" + Path.GetFileName(file2.FileName);
				CloudResult cloudResult2 = m_remoteStorage.SaveBuffer(fileName, File.ReadAllBytes(file2.FileName));
				if (cloudResult2 != 0)
				{
					return cloudResult2;
				}
			}
			return CloudResult.Ok;
		}

		public bool LoadFromCloudAsync(string fileName, Action<byte[]> completedAction)
		{
			if (m_remoteStorage == null)
			{
				return false;
			}
			return m_remoteStorage.LoadBufferAsync(fileName, delegate(bool x)
			{
				completedAction(x ? m_remoteStorage.GetFileBuffer(fileName) : null);
			});
		}

		public List<MyCloudFileInfo> GetCloudFiles(string directoryFilter)
		{
			if (m_remoteStorage == null)
			{
				return null;
			}
			return m_remoteStorage.GetCloudFiles(directoryFilter);
		}

		public byte[] LoadFromCloud(string fileName)
		{
			if (m_remoteStorage == null)
			{
				return null;
			}
			return m_remoteStorage.LoadBuffer(fileName);
		}

		public bool DeleteFromCloud(string fileName)
		{
			if (m_remoteStorage == null)
			{
				return false;
			}
			List<MyCloudFileInfo> cloudFiles = GetCloudFiles(fileName);
			if (cloudFiles != null)
			{
				bool flag = true;
				{
					foreach (MyCloudFileInfo item in cloudFiles)
					{
						flag &= m_remoteStorage.DeleteFile(item.Name);
					}
					return flag;
				}
			}
			return false;
		}

		public bool IsProductOwned(uint productId, out DateTime? purchaseTime)
		{
			purchaseTime = null;
			uint earliestPurchaseUnixTime = SteamApps.GetEarliestPurchaseUnixTime((AppId_t)productId);
			purchaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(earliestPurchaseUnixTime).ToLocalTime();
			if (!IsAppInstalled(productId))
			{
				return IsDlcInstalled(productId);
			}
			return true;
		}

		public static string EncodeEncryptedAppTicket(byte[] ticketData, uint ticketSize)
		{
			byte[] array = new byte[ticketSize];
			Array.Copy(ticketData, array, ticketSize);
			string result = null;
			try
			{
				result = Convert.ToBase64String(array);
				return result;
			}
			catch
			{
				return result;
			}
		}

		public void RequestEncryptedAppTicket(string url, Action<bool, string> onDone)
		{
			if (!IsActive)
			{
				onDone.InvokeIfNotNull(arg1: false, string.Empty);
				return;
			}
			m_onEncryptedAppTicketResponse += onDone;
			SteamAPICall_t hAPICall = SteamUser.RequestEncryptedAppTicket(null, 0);
			m_encryptedAppTicketCallback.Set(hAPICall);
		}

		public void RequestPermissions(Permissions permission, bool attemptResolution, Action<PermissionResult> onDone)
		{
			InvokeOnMainThread(delegate
			{
				onDone.InvokeIfNotNull(PermissionResult.Granted);
			});
		}

		public void RequestPermissionsWithTargetUser(Permissions permission, ulong userId, Action<PermissionResult> onDone)
		{
			InvokeOnMainThread(delegate
			{
				onDone.InvokeIfNotNull(PermissionResult.Granted);
			});
		}

		private void OnEncryptedAppTicketResponse(EncryptedAppTicketResponse_t param, bool failure)
		{
			bool arg = false;
			string arg2 = null;
			if (!failure && param.m_eResult == EResult.k_EResultOK)
			{
				byte[] array = new byte[1024];
				if (SteamUser.GetEncryptedAppTicket(array, 1024, out var pcbTicket))
				{
					arg2 = EncodeEncryptedAppTicket(array, pcbTicket);
					arg = true;
				}
			}
			this.m_onEncryptedAppTicketResponse.InvokeIfNotNull(arg, arg2);
			this.m_onEncryptedAppTicketResponse = null;
		}

		protected void OnOnUserChanged()
		{
			this.OnUserChanged?.Invoke(obj: true);
		}

		public void OnThreadpoolInitialized()
		{
		}

		public bool GetInstallStatus(out int percentage)
		{
			percentage = 100;
			return true;
		}

		public void Trace(bool enable)
		{
			throw new NotImplementedException();
		}

		public void SetPlayerMuted(ulong playerId, bool muted)
		{
		}

		public bool IsPlayerMuted(ulong playerId)
		{
			return false;
		}

		public void UpdateMutedPlayers(Action onDone)
		{
			onDone.InvokeIfNotNull();
		}

		public MyGameServiceAccountType GetServerAccountType(ulong steamId)
		{
			return (MyGameServiceAccountType)((CSteamID)steamId).GetEAccountType();
		}
	}
}
