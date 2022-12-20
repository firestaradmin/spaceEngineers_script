using System.Collections.Generic;
using Steamworks;

namespace VRage.Steam.Steamworks
{
	internal interface IMySteamUgc
	{
		UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle);

		bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

		bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize);

		bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize);

		bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue);

		uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

		bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType);

		uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

		bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize);

		bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

		bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName);

		bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName);

		bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs);

		bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags);

		bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription);

		bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata);

		bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren);

		bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews);

		bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly);

		bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays);

		bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage);

		bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName);

		bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag);

		bool SetSearchText(UGCQueryHandle_t handle, string pSearchText);

		bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

		bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue);

		SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

		UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

		bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle);

		bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription);

		bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage);

		bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData);

		bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

		bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags);

		bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder);

		bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile);

		bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey);

		bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue);

		bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type);

		bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID);

		bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile);

		bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID);

		bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index);

		SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote);

		EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp);

		SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID);

		SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID);

		SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID);

		uint GetNumSubscribedItems();

		uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		uint GetItemState(PublishedFileId_t nPublishedFileID);

		bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp);

		bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority);

		bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder);

		void SuspendDownloads(bool bSuspend);

		SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		SteamAPICall_t StopPlaytimeTrackingForAllItems();

		SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID);

		SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID);

		SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID);

		SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID);

		SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID);
	}
}
