using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public interface IMyGameService
	{
		uint AppId { get; }

		bool IsActive { get; }

		bool IsOnline { get; }

		bool IsOverlayEnabled { get; }

		bool IsOverlayBrowserAvailable { get; }

		bool OwnsGame { get; }

		ulong UserId { get; set; }

		string UserName { get; }

		MyGameServiceUniverse UserUniverse { get; }

		string BranchName { get; }

		string BranchNameFriendly { get; }

		string ServiceName { get; }

		event Action<bool> OnOverlayActivated;

		event Action<uint> OnDLCInstalled;

		event Action<bool> OnUserChanged;

		event Action OnUpdate;

<<<<<<< HEAD
		event Action<bool> OnUpdateNetworkThread;
=======
		event Action OnUpdateNetworkThread;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		void OpenOverlayUrl(string url);

		void SetNotificationPosition(NotificationPosition notificationPosition);

		void ShutDown();

		bool IsAppInstalled(uint appId);

		void OpenDlcInShop(uint dlcId);

		void OpenInventoryItemInShop(int itemId);

		void AddDlcPackages(List<MyDlcPackage> packages);

		bool IsDlcSupported(uint dlcId);

		bool IsDlcInstalled(uint dlcId);

		int GetDLCCount();

		bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize);

		void OpenOverlayUser(ulong id);

		bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length);

		void LoadStats();

		IMyAchievement GetAchievement(string achievementName, string statName, float maxValue);

		IMyAchievement GetAchievement(string achievementName);

		void RegisterAchievement(string achievementName, string XBLId);

		void ResetAllStats(bool achievementsToo);

		void StoreStats();

		string GetPersonaName(ulong userId);

		bool HasFriend(ulong userId);

		string GetClanName(ulong groupId);

		void Update();

<<<<<<< HEAD
		void UpdateNetworkThread(bool sessionEnabled);
=======
		void UpdateNetworkThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		bool IsUserInGroup(ulong groupId);

		bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes);

		int GetRemoteStorageFileCount();

		string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes);

		bool IsRemoteStorageFilePersisted(string file);

		bool RemoteStorageFileForget(string file);

		ulong CreatePublishedFileUpdateRequest(ulong publishedFileId);

		void UpdatePublishedFileTags(ulong updateHandle, string[] tags);

		void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName);

		void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName);

		void FileDelete(string steamItemFileName);

		bool FileExists(string fileName);

		int GetFileSize(string fileName);

		ulong FileWriteStreamOpen(string fileName);

		void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size);

		void FileWriteStreamClose(ulong handle);

		void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult);

		void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult);

		void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult);

		void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult);

		int GetFriendsCount();

		ulong GetFriendIdByIndex(int index);

		string GetFriendNameByIndex(int index);

		void SaveToCloudAsync(string storageName, byte[] buffer, Action<CloudResult> completedAction);

		CloudResult SaveToCloud(string fileName, byte[] buffer);

		CloudResult SaveToCloud(string containerName, List<MyCloudFile> fileNames);

		bool LoadFromCloudAsync(string fileName, Action<byte[]> completedAction);

		List<MyCloudFileInfo> GetCloudFiles(string directoryFilter);

		byte[] LoadFromCloud(string fileName);

		bool DeleteFromCloud(string fileName);

		bool IsProductOwned(uint productId, out DateTime? purchaseTime);

		void RequestEncryptedAppTicket(string url, Action<bool, string> onDone);

		void RequestPermissions(Permissions permission, bool attemptResolution, Action<PermissionResult> onDone);

		void RequestPermissionsWithTargetUser(Permissions permission, ulong userId, Action<PermissionResult> onDone);

		void OnThreadpoolInitialized();

		bool GetInstallStatus(out int percentage);

		void Trace(bool enable);

		bool IsPlayerMuted(ulong playerId);

		void UpdateMutedPlayers(Action onDone);

		void SetPlayerMuted(ulong playerId, bool muted);

		MyGameServiceAccountType GetServerAccountType(ulong steamId);
	}
}
