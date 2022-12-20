using System;
using System.Collections.Generic;
using VRage.GameServices;

namespace VRage.EOS
{
	internal class MyEOSGameService : IMyGameService
	{
		public uint AppId { get; }

		public bool IsActive { get; }

		public bool IsOnline { get; }

		public bool IsOverlayEnabled { get; }

		public bool IsOverlayBrowserAvailable { get; }

		public bool OwnsGame { get; }

		public ulong UserId { get; set; }

		public string UserName { get; }

		public MyGameServiceUniverse UserUniverse { get; }

		public string BranchName { get; }

		public string BranchNameFriendly { get; }

		public string ServiceName => "EOS";

		public event Action<bool> OnOverlayActivated;

		public event Action<uint> OnDLCInstalled;

		public event Action<bool> OnUserChanged;

		public event Action OnUpdate;

		public event Action<bool> OnUpdateNetworkThread;

		public void OpenOverlayUrl(string url)
		{
		}

		public void SetNotificationPosition(NotificationPosition notificationPosition)
		{
		}

		public void ShutDown()
		{
		}

		public bool IsAppInstalled(uint appId)
		{
			return false;
		}

		public void OpenDlcInShop(uint dlcId)
		{
		}

		public void OpenInventoryItemInShop(int itemId)
		{
		}

		public void AddDlcPackages(List<MyDlcPackage> packages)
		{
		}

		public bool IsDlcSupported(uint dlcId)
		{
			return false;
		}

		public bool IsDlcInstalled(uint dlcId)
		{
			return false;
		}

		public int GetDLCCount()
		{
			return 0;
		}

		public bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize)
		{
			dlcId = 0u;
			available = false;
			name = null;
			return false;
		}

		public void OpenOverlayUser(ulong id)
		{
		}

		public bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
		{
			ticketHandle = 0u;
			length = 0u;
			return false;
		}

		public void LoadStats()
		{
		}

		public IMyAchievement GetAchievement(string achievementName, string statName, float maxValue)
		{
			return null;
		}

		public IMyAchievement GetAchievement(string achievementName)
		{
			return null;
		}

		public void RegisterAchievement(string achievementName, string XBLId)
		{
		}

		public void ResetAllStats(bool achievementsToo)
		{
		}

		public void StoreStats()
		{
		}

		public string GetPersonaName(ulong userId)
		{
			return null;
		}

		public bool HasFriend(ulong userId)
		{
			return false;
		}

		public string GetClanName(ulong groupId)
		{
			return null;
		}

		public void Update()
		{
			this.OnUpdate?.Invoke();
		}

		public void UpdateNetworkThread(bool sessionEnabled)
		{
			this.OnUpdateNetworkThread?.Invoke(sessionEnabled);
		}

		public bool IsUserInGroup(ulong groupId)
		{
			return false;
		}

		public bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
		{
			totalBytes = 0uL;
			availableBytes = 0uL;
			return false;
		}

		public int GetRemoteStorageFileCount()
		{
			return 0;
		}

		public string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
		{
			fileSizeInBytes = 0;
			return null;
		}

		public bool IsRemoteStorageFilePersisted(string file)
		{
			return false;
		}

		public bool RemoteStorageFileForget(string file)
		{
			return false;
		}

		public ulong CreatePublishedFileUpdateRequest(ulong publishedFileId)
		{
			return 0uL;
		}

		public void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
		{
		}

		public void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
		{
		}

		public void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName)
		{
		}

		public void FileDelete(string steamItemFileName)
		{
		}

		public bool FileExists(string fileName)
		{
			return false;
		}

		public int GetFileSize(string fileName)
		{
			return 0;
		}

		public ulong FileWriteStreamOpen(string fileName)
		{
			return 0uL;
		}

		public void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
		{
		}

		public void FileWriteStreamClose(ulong handle)
		{
		}

		public void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
		{
		}

		public void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
		{
		}

		public void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
		{
		}

		public void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult)
		{
		}

		public int GetFriendsCount()
		{
			return 0;
		}

		public ulong GetFriendIdByIndex(int index)
		{
			return 0uL;
		}

		public string GetFriendNameByIndex(int index)
		{
			return null;
		}

		public void SaveToCloudAsync(string storageName, byte[] buffer, Action<CloudResult> completedAction)
		{
		}

		public CloudResult SaveToCloud(string fileName, byte[] buffer)
		{
			return CloudResult.Ok;
		}

		public CloudResult SaveToCloud(string containerName, List<MyCloudFile> fileNames)
		{
			return CloudResult.Ok;
		}

		public bool LoadFromCloudAsync(string fileName, Action<byte[]> completedAction)
		{
			return false;
		}

		public List<MyCloudFileInfo> GetCloudFiles(string directoryFilter)
		{
			return null;
		}

		public byte[] LoadFromCloud(string fileName)
		{
			return new byte[0];
		}

		public bool DeleteFromCloud(string fileName)
		{
			return false;
		}

		public bool IsProductOwned(uint productId, out DateTime? purchaseTime)
		{
			purchaseTime = null;
			return false;
		}

		public void RequestEncryptedAppTicket(string url, Action<bool, string> onDone)
		{
		}

		public void RequestPermissions(Permissions permission, bool attemptResolution, Action<PermissionResult> onDone)
		{
			onDone.InvokeIfNotNull(PermissionResult.Granted);
		}

		public void RequestPermissionsWithTargetUser(Permissions permission, ulong userId, Action<PermissionResult> onDone)
		{
			onDone.InvokeIfNotNull(PermissionResult.Granted);
		}

		public void OnThreadpoolInitialized()
		{
		}

		public bool GetInstallStatus(out int percentage)
		{
			percentage = 0;
			return false;
		}

		public void Trace(bool enable)
		{
		}

		public void SetPlayerMuted(ulong playerId, bool muted)
		{
		}

		public MyGameServiceAccountType GetServerAccountType(ulong steamId)
		{
			return MyGameServiceAccountType.Individual;
		}

		public bool IsPlayerMuted(ulong playerId)
		{
			return false;
		}

		public void UpdateMutedPlayers(Action onDone)
		{
			onDone.InvokeIfNotNull();
		}
	}
}
