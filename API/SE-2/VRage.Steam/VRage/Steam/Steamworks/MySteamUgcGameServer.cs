using System.Collections.Generic;
using Steamworks;

namespace VRage.Steam.Steamworks
{
	internal class MySteamUgcGameServer : IMySteamUgc
	{
		public static readonly IMySteamUgc Instance = new MySteamUgcGameServer();

		private MySteamUgcGameServer()
		{
		}

		public UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			return SteamGameServerUGC.CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		public UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			return SteamGameServerUGC.CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		public UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamGameServerUGC.CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			return SteamGameServerUGC.SendQueryUGCRequest(handle);
		}

		public bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			return SteamGameServerUGC.GetQueryUGCResult(handle, index, out pDetails);
		}

		public bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			return SteamGameServerUGC.GetQueryUGCPreviewURL(handle, index, out pchURL, cchURLSize);
		}

		public bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			return SteamGameServerUGC.GetQueryUGCMetadata(handle, index, out pchMetadata, cchMetadatasize);
		}

		public bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			return SteamGameServerUGC.GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
		}

		public bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			return SteamGameServerUGC.GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
		}

		public uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			return SteamGameServerUGC.GetQueryUGCNumAdditionalPreviews(handle, index);
		}

		public bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			return SteamGameServerUGC.GetQueryUGCAdditionalPreview(handle, index, previewIndex, out pchURLOrVideoID, cchURLSize, out pchOriginalFileName, cchOriginalFileNameSize, out pPreviewType);
		}

		public uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			return SteamGameServerUGC.GetQueryUGCNumKeyValueTags(handle, index);
		}

		public bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			return SteamGameServerUGC.GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, out pchKey, cchKeySize, out pchValue, cchValueSize);
		}

		public bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			return SteamGameServerUGC.ReleaseQueryUGCRequest(handle);
		}

		public bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			return SteamGameServerUGC.AddRequiredTag(handle, pTagName);
		}

		public bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			return SteamGameServerUGC.AddExcludedTag(handle, pTagName);
		}

		public bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			return SteamGameServerUGC.SetReturnOnlyIDs(handle, bReturnOnlyIDs);
		}

		public bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			return SteamGameServerUGC.SetReturnKeyValueTags(handle, bReturnKeyValueTags);
		}

		public bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			return SteamGameServerUGC.SetReturnLongDescription(handle, bReturnLongDescription);
		}

		public bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			return SteamGameServerUGC.SetReturnMetadata(handle, bReturnMetadata);
		}

		public bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			return SteamGameServerUGC.SetReturnChildren(handle, bReturnChildren);
		}

		public bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			return SteamGameServerUGC.SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
		}

		public bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			return SteamGameServerUGC.SetReturnTotalOnly(handle, bReturnTotalOnly);
		}

		public bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			return SteamGameServerUGC.SetReturnPlaytimeStats(handle, unDays);
		}

		public bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			return SteamGameServerUGC.SetLanguage(handle, pchLanguage);
		}

		public bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			return SteamGameServerUGC.SetAllowCachedResponse(handle, unMaxAgeSeconds);
		}

		public bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			return SteamGameServerUGC.SetCloudFileNameFilter(handle, pMatchCloudFileName);
		}

		public bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			return SteamGameServerUGC.SetMatchAnyTag(handle, bMatchAnyTag);
		}

		public bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			return SteamGameServerUGC.SetSearchText(handle, pSearchText);
		}

		public bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			return SteamGameServerUGC.SetRankedByTrendDays(handle, unDays);
		}

		public bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			return SteamGameServerUGC.AddRequiredKeyValueTag(handle, pKey, pValue);
		}

		public SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			return SteamGameServerUGC.RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
		}

		public SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			return SteamGameServerUGC.CreateItem(nConsumerAppId, eFileType);
		}

		public UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.StartItemUpdate(nConsumerAppId, nPublishedFileID);
		}

		public bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			return SteamGameServerUGC.SetItemTitle(handle, pchTitle);
		}

		public bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			return SteamGameServerUGC.SetItemDescription(handle, pchDescription);
		}

		public bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			return SteamGameServerUGC.SetItemUpdateLanguage(handle, pchLanguage);
		}

		public bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			return SteamGameServerUGC.SetItemMetadata(handle, pchMetaData);
		}

		public bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			return SteamGameServerUGC.SetItemVisibility(handle, eVisibility);
		}

		public bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
		{
			return SteamGameServerUGC.SetItemTags(updateHandle, pTags);
		}

		public bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			return SteamGameServerUGC.SetItemContent(handle, pszContentFolder);
		}

		public bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			return SteamGameServerUGC.SetItemPreview(handle, pszPreviewFile);
		}

		public bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			return SteamGameServerUGC.RemoveItemKeyValueTags(handle, pchKey);
		}

		public bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			return SteamGameServerUGC.AddItemKeyValueTag(handle, pchKey, pchValue);
		}

		public bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			return SteamGameServerUGC.AddItemPreviewFile(handle, pszPreviewFile, type);
		}

		public bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			return SteamGameServerUGC.AddItemPreviewVideo(handle, pszVideoID);
		}

		public bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			return SteamGameServerUGC.UpdateItemPreviewFile(handle, index, pszPreviewFile);
		}

		public bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			return SteamGameServerUGC.UpdateItemPreviewVideo(handle, index, pszVideoID);
		}

		public bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			return SteamGameServerUGC.RemoveItemPreview(handle, index);
		}

		public SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			return SteamGameServerUGC.SubmitItemUpdate(handle, pchChangeNote);
		}

		public EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			return SteamGameServerUGC.GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		}

		public SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			return SteamGameServerUGC.SetUserItemVote(nPublishedFileID, bVoteUp);
		}

		public SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.GetUserItemVote(nPublishedFileID);
		}

		public SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.AddItemToFavorites(nAppId, nPublishedFileID);
		}

		public SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.RemoveItemFromFavorites(nAppId, nPublishedFileID);
		}

		public SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.SubscribeItem(nPublishedFileID);
		}

		public SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.UnsubscribeItem(nPublishedFileID);
		}

		public uint GetNumSubscribedItems()
		{
			return SteamGameServerUGC.GetNumSubscribedItems();
		}

		public uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			return SteamGameServerUGC.GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
		}

		public uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.GetItemState(nPublishedFileID);
		}

		public bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			return SteamGameServerUGC.GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, out pchFolder, cchFolderSize, out punTimeStamp);
		}

		public bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			return SteamGameServerUGC.GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		public bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			return SteamGameServerUGC.DownloadItem(nPublishedFileID, bHighPriority);
		}

		public bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			return SteamGameServerUGC.BInitWorkshopForGameServer(unWorkshopDepotID, pszFolder);
		}

		public void SuspendDownloads(bool bSuspend)
		{
			SteamGameServerUGC.SuspendDownloads(bSuspend);
		}

		public SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamGameServerUGC.StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamGameServerUGC.StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			return SteamGameServerUGC.StopPlaytimeTrackingForAllItems();
		}

		public SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			return SteamGameServerUGC.AddDependency(nParentPublishedFileID, nChildPublishedFileID);
		}

		public SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			return SteamGameServerUGC.RemoveDependency(nParentPublishedFileID, nChildPublishedFileID);
		}

		public SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			return SteamGameServerUGC.AddAppDependency(nPublishedFileID, nAppID);
		}

		public SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			return SteamGameServerUGC.RemoveAppDependency(nPublishedFileID, nAppID);
		}

		public SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.GetAppDependencies(nPublishedFileID);
		}

		public SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamGameServerUGC.DeleteItem(nPublishedFileID);
		}
	}
}
