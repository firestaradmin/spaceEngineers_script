using System.Collections.Generic;
using Steamworks;

namespace VRage.Steam.Steamworks
{
	internal class MySteamUgcClient : IMySteamUgc
	{
		public static readonly IMySteamUgc Instance = new MySteamUgcClient();

		private MySteamUgcClient()
		{
		}

		public UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			return SteamUGC.CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		public UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			return SteamUGC.CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		public UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamUGC.CreateQueryUGCDetailsRequest(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			return SteamUGC.SendQueryUGCRequest(handle);
		}

		public bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			return SteamUGC.GetQueryUGCResult(handle, index, out pDetails);
		}

		public bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			return SteamUGC.GetQueryUGCPreviewURL(handle, index, out pchURL, cchURLSize);
		}

		public bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			return SteamUGC.GetQueryUGCMetadata(handle, index, out pchMetadata, cchMetadatasize);
		}

		public bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			return SteamUGC.GetQueryUGCChildren(handle, index, pvecPublishedFileID, cMaxEntries);
		}

		public bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			return SteamUGC.GetQueryUGCStatistic(handle, index, eStatType, out pStatValue);
		}

		public uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			return SteamUGC.GetQueryUGCNumAdditionalPreviews(handle, index);
		}

		public bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			return SteamUGC.GetQueryUGCAdditionalPreview(handle, index, previewIndex, out pchURLOrVideoID, cchURLSize, out pchOriginalFileName, cchOriginalFileNameSize, out pPreviewType);
		}

		public uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			return SteamUGC.GetQueryUGCNumKeyValueTags(handle, index);
		}

		public bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			return SteamUGC.GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, out pchKey, cchKeySize, out pchValue, cchValueSize);
		}

		public bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			return SteamUGC.ReleaseQueryUGCRequest(handle);
		}

		public bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			return SteamUGC.AddRequiredTag(handle, pTagName);
		}

		public bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			return SteamUGC.AddExcludedTag(handle, pTagName);
		}

		public bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			return SteamUGC.SetReturnOnlyIDs(handle, bReturnOnlyIDs);
		}

		public bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			return SteamUGC.SetReturnKeyValueTags(handle, bReturnKeyValueTags);
		}

		public bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			return SteamUGC.SetReturnLongDescription(handle, bReturnLongDescription);
		}

		public bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			return SteamUGC.SetReturnMetadata(handle, bReturnMetadata);
		}

		public bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			return SteamUGC.SetReturnChildren(handle, bReturnChildren);
		}

		public bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			return SteamUGC.SetReturnAdditionalPreviews(handle, bReturnAdditionalPreviews);
		}

		public bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			return SteamUGC.SetReturnTotalOnly(handle, bReturnTotalOnly);
		}

		public bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			return SteamUGC.SetReturnPlaytimeStats(handle, unDays);
		}

		public bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			return SteamUGC.SetLanguage(handle, pchLanguage);
		}

		public bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			return SteamUGC.SetAllowCachedResponse(handle, unMaxAgeSeconds);
		}

		public bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			return SteamUGC.SetCloudFileNameFilter(handle, pMatchCloudFileName);
		}

		public bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			return SteamUGC.SetMatchAnyTag(handle, bMatchAnyTag);
		}

		public bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			return SteamUGC.SetSearchText(handle, pSearchText);
		}

		public bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			return SteamUGC.SetRankedByTrendDays(handle, unDays);
		}

		public bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			return SteamUGC.AddRequiredKeyValueTag(handle, pKey, pValue);
		}

		public SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			return SteamUGC.RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
		}

		public SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			return SteamUGC.CreateItem(nConsumerAppId, eFileType);
		}

		public UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.StartItemUpdate(nConsumerAppId, nPublishedFileID);
		}

		public bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			return SteamUGC.SetItemTitle(handle, pchTitle);
		}

		public bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			return SteamUGC.SetItemDescription(handle, pchDescription);
		}

		public bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			return SteamUGC.SetItemUpdateLanguage(handle, pchLanguage);
		}

		public bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			return SteamUGC.SetItemMetadata(handle, pchMetaData);
		}

		public bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			return SteamUGC.SetItemVisibility(handle, eVisibility);
		}

		public bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
		{
			return SteamUGC.SetItemTags(updateHandle, pTags);
		}

		public bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			return SteamUGC.SetItemContent(handle, pszContentFolder);
		}

		public bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			return SteamUGC.SetItemPreview(handle, pszPreviewFile);
		}

		public bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			return SteamUGC.RemoveItemKeyValueTags(handle, pchKey);
		}

		public bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			return SteamUGC.AddItemKeyValueTag(handle, pchKey, pchValue);
		}

		public bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			return SteamUGC.AddItemPreviewFile(handle, pszPreviewFile, type);
		}

		public bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			return SteamUGC.AddItemPreviewVideo(handle, pszVideoID);
		}

		public bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			return SteamUGC.UpdateItemPreviewFile(handle, index, pszPreviewFile);
		}

		public bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			return SteamUGC.UpdateItemPreviewVideo(handle, index, pszVideoID);
		}

		public bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			return SteamUGC.RemoveItemPreview(handle, index);
		}

		public SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			return SteamUGC.SubmitItemUpdate(handle, pchChangeNote);
		}

		public EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			return SteamUGC.GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		}

		public SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			return SteamUGC.SetUserItemVote(nPublishedFileID, bVoteUp);
		}

		public SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.GetUserItemVote(nPublishedFileID);
		}

		public SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.AddItemToFavorites(nAppId, nPublishedFileID);
		}

		public SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.RemoveItemFromFavorites(nAppId, nPublishedFileID);
		}

		public SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.SubscribeItem(nPublishedFileID);
		}

		public SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.UnsubscribeItem(nPublishedFileID);
		}

		public uint GetNumSubscribedItems()
		{
			return SteamUGC.GetNumSubscribedItems();
		}

		public uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			return SteamUGC.GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
		}

		public uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.GetItemState(nPublishedFileID);
		}

		public bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			return SteamUGC.GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, out pchFolder, cchFolderSize, out punTimeStamp);
		}

		public bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			return SteamUGC.GetItemDownloadInfo(nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		public bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			return SteamUGC.DownloadItem(nPublishedFileID, bHighPriority);
		}

		public bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			return SteamUGC.BInitWorkshopForGameServer(unWorkshopDepotID, pszFolder);
		}

		public void SuspendDownloads(bool bSuspend)
		{
			SteamUGC.SuspendDownloads(bSuspend);
		}

		public SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamUGC.StartPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			return SteamUGC.StopPlaytimeTracking(pvecPublishedFileID, unNumPublishedFileIDs);
		}

		public SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			return SteamUGC.StopPlaytimeTrackingForAllItems();
		}

		public SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			return SteamUGC.AddDependency(nParentPublishedFileID, nChildPublishedFileID);
		}

		public SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			return SteamUGC.RemoveDependency(nParentPublishedFileID, nChildPublishedFileID);
		}

		public SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			return SteamUGC.AddAppDependency(nPublishedFileID, nAppID);
		}

		public SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			return SteamUGC.RemoveAppDependency(nPublishedFileID, nAppID);
		}

		public SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.GetAppDependencies(nPublishedFileID);
		}

		public SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			return SteamUGC.DeleteItem(nPublishedFileID);
		}
	}
}
