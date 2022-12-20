using System;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;
using VRage.Steam.Steamworks;

namespace VRage.Steam
{
	internal class MySteamWorkshopQuery : MyWorkshopQuery
	{
		private MySteamUGCService m_steamService;

		private UGCQueryHandle_t m_queryHandle;

		private uint m_resultsReturned;

		private uint m_pageStart = 1u;

		private uint m_pageCurrent;

		private uint m_pageSteam = 1u;

		private uint m_steamPageItemSkip;

		private uint m_skippedItems;

		private bool m_cancelQuery;

		private CallResult<SteamUGCQueryCompleted_t> m_queryCompletedResult;

		private static readonly uint PAGE_SIZE = 9u;

		private IMySteamUgc SteamUGC => MySteamUgc.Instance;

		public override uint ItemsPerPage
		{
			get
			{
				return PAGE_SIZE;
			}
			set
			{
			}
		}

		private uint ItemsPerPageSteam
		{
			get
			{
				return m_steamService.UGCItemsPerPage;
			}
			set
			{
			}
		}

		private List<MyWorkshopItem> ItemsInternal { get; set; }

		public override bool IsRunning
		{
			get
			{
				if (m_queryCompletedResult != null)
				{
					return m_queryCompletedResult.IsActive();
				}
				return false;
			}
		}

		internal MySteamWorkshopQuery(MySteamUGCService service)
		{
			m_steamService = service;
		}

		public override void Run()
		{
			Run(1u);
		}

		public override void Run(uint startingPage)
		{
			if (m_queryCompletedResult == null || !m_queryCompletedResult.IsActive())
			{
				base.TotalResults = 0u;
				m_resultsReturned = 0u;
				m_pageStart = startingPage;
				m_pageCurrent = m_pageStart - 1;
				uint num = m_pageCurrent * ItemsPerPage;
				m_pageSteam = num / ItemsPerPageSteam + 1;
				m_steamPageItemSkip = num % ItemsPerPageSteam;
				m_skippedItems = num - m_steamPageItemSkip;
				if (base.Items == null)
				{
					base.Items = new List<MyWorkshopItem>();
				}
				if (ItemsInternal == null)
				{
					ItemsInternal = new List<MyWorkshopItem>();
				}
				base.Items.Clear();
				ItemsInternal.Clear();
				RunQuery();
			}
		}

		public override void Stop()
		{
			base.Stop();
			m_cancelQuery = true;
		}

		public override void Dispose()
		{
			base.Dispose();
			m_steamService = null;
			if (base.ItemIds != null)
			{
				base.ItemIds.Clear();
			}
			if (base.RequiredTags != null)
			{
				base.RequiredTags.Clear();
			}
			if (base.ExcludedTags != null)
			{
				base.ExcludedTags.Clear();
			}
			base.Items = null;
			ItemsInternal = null;
			if (m_queryCompletedResult != null)
			{
				m_queryCompletedResult.Dispose();
				m_queryCompletedResult = null;
			}
		}

		private void RunQuery()
		{
			QueryCleanup();
			if (base.ItemIds != null && base.ItemIds.Count > 0)
			{
				PublishedFileId_t[] array = new PublishedFileId_t[base.ItemIds.Count];
				for (int i = 0; i < base.ItemIds.Count; i++)
				{
					array[i] = (PublishedFileId_t)base.ItemIds[i];
				}
				m_queryHandle = SteamUGC.CreateQueryUGCDetailsRequest(array, (uint)array.Length);
			}
			else if (base.UserId != 0L)
			{
				m_queryHandle = SteamUGC.CreateQueryUserUGCRequest((AccountID_t)(uint)base.UserId, ConvertFromListType(base.ListType), ConvertFromItemType(base.ItemType), EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, AppId_t.Invalid, m_steamService.SteamAppId, m_pageSteam);
			}
			else
			{
				m_queryHandle = SteamUGC.CreateQueryAllUGCRequest(ConvertFromQueryType(base.QueryType), ConvertFromItemType(base.ItemType), AppId_t.Invalid, m_steamService.SteamAppId, m_pageSteam);
			}
			if (!string.IsNullOrWhiteSpace(base.SearchString))
			{
				SteamUGC.SetSearchText(m_queryHandle, base.SearchString);
			}
			if (base.RequiredTags != null && base.RequiredTags.Count > 0)
			{
				foreach (string requiredTag in base.RequiredTags)
				{
					SteamUGC.AddRequiredTag(m_queryHandle, requiredTag);
				}
				SteamUGC.SetMatchAnyTag(m_queryHandle, !base.RequireAllTags);
			}
			if (base.ExcludedTags != null && base.ExcludedTags.Count > 0)
			{
				foreach (string excludedTag in base.ExcludedTags)
				{
					SteamUGC.AddExcludedTag(m_queryHandle, excludedTag);
				}
			}
			SteamUGC.SetReturnMetadata(m_queryHandle, bReturnMetadata: true);
			SteamUGC.SetReturnChildren(m_queryHandle, bReturnChildren: true);
			SteamUGC.SetAllowCachedResponse(m_queryHandle, 0u);
			SteamUGC.SetReturnLongDescription(m_queryHandle, bReturnLongDescription: true);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(m_queryHandle);
			m_queryCompletedResult = CallResult<SteamUGCQueryCompleted_t>.Create();
			m_queryCompletedResult.Set(hAPICall, QueryCompletedResult);
		}

		private EUserUGCList ConvertFromListType(WorkshopListType listType)
		{
			switch (listType)
			{
			case WorkshopListType.Subscribed:
				return EUserUGCList.k_EUserUGCList_Subscribed;
			case WorkshopListType.Favourited:
				return EUserUGCList.k_EUserUGCList_Favorited;
			case WorkshopListType.None:
				return EUserUGCList.k_EUserUGCList_Published;
			default:
				throw new ArgumentOutOfRangeException("listType", listType, null);
			}
		}

		private EUGCMatchingUGCType ConvertFromItemType(WorkshopItemType itemType)
		{
			switch (itemType)
			{
			case WorkshopItemType.All:
				return EUGCMatchingUGCType.k_EUGCMatchingUGCType_All;
			case WorkshopItemType.Mod:
				return EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items;
			case WorkshopItemType.Collection:
				return EUGCMatchingUGCType.k_EUGCMatchingUGCType_Collections;
			case WorkshopItemType.Guide:
				return EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides;
			default:
				throw new ArgumentOutOfRangeException("itemType", itemType, null);
			}
		}

		private EUGCQuery ConvertFromQueryType(MyWorkshopQueryType queryType)
		{
			switch (queryType)
			{
			case MyWorkshopQueryType.SortByVotes:
				return EUGCQuery.k_EUGCQuery_RankedByVote;
			case MyWorkshopQueryType.SortByPublicationDate:
				return EUGCQuery.k_EUGCQuery_RankedByPublicationDate;
			case MyWorkshopQueryType.SortByTotalUniqueSubscriptions:
				return EUGCQuery.k_EUGCQuery_RankedByTotalUniqueSubscriptions;
			default:
				throw new ArgumentOutOfRangeException("queryType", queryType, null);
			}
		}

		private void QueryCleanup()
		{
			m_cancelQuery = false;
			SteamUGC.ReleaseQueryUGCRequest(m_queryHandle);
			m_queryHandle = UGCQueryHandle_t.Invalid;
			if (m_queryCompletedResult != null)
			{
				m_queryCompletedResult.Dispose();
			}
		}

		private void QueryCompletedResult(SteamUGCQueryCompleted_t result, bool ioFailure)
		{
			if (ioFailure || result.m_eResult != EResult.k_EResultOK)
			{
				QueryCleanup();
				OnQueryCompleted((MyGameServiceCallResult)(ioFailure ? EResult.k_EResultIOFailure : result.m_eResult));
				return;
			}
			base.TotalResults = result.m_unTotalMatchingResults;
			uint unNumResultsReturned = result.m_unNumResultsReturned;
			m_resultsReturned += unNumResultsReturned;
			for (uint num = 0u; num < unNumResultsReturned; num++)
			{
				if (!SteamUGC.GetQueryUGCResult(m_queryHandle, num, out var pDetails) || pDetails.m_eResult != EResult.k_EResultOK)
				{
					continue;
				}
				MySteamWorkshopItem mySteamWorkshopItem = new MySteamWorkshopItem(m_steamService, pDetails);
				SteamUGC.GetQueryUGCMetadata(m_queryHandle, num, out var pchMetadata, m_steamService.UGCMaxMetadataLength);
				mySteamWorkshopItem.SetMetadata(pchMetadata);
				if (pDetails.m_unNumChildren != 0)
				{
					PublishedFileId_t[] array = new PublishedFileId_t[pDetails.m_unNumChildren];
					SteamUGC.GetQueryUGCChildren(m_queryHandle, num, array, pDetails.m_unNumChildren);
					PublishedFileId_t[] array2 = array;
					foreach (PublishedFileId_t publishedFileId_t in array2)
					{
						mySteamWorkshopItem.AddDependency((ulong)publishedFileId_t);
					}
				}
				SteamUGC.GetQueryUGCStatistic(m_queryHandle, num, EItemStatistic.k_EItemStatistic_NumSubscriptions, out var pStatValue);
				mySteamWorkshopItem.NumSubscriptions = pStatValue;
				if ((mySteamWorkshopItem.Visibility != MyPublishedFileVisibility.Private || mySteamWorkshopItem.OwnerId == m_steamService.UserId) && (mySteamWorkshopItem.Visibility != MyPublishedFileVisibility.FriendsOnly || m_steamService.HasFriend(mySteamWorkshopItem.OwnerId) || mySteamWorkshopItem.OwnerId == m_steamService.UserId))
				{
					ItemsInternal.Add(mySteamWorkshopItem);
				}
			}
			bool flag = m_skippedItems + m_resultsReturned == base.TotalResults;
			MyGameServiceCallResult eResult = (MyGameServiceCallResult)result.m_eResult;
			while (!m_cancelQuery && CommitItemPages(eResult))
			{
			}
			if (m_cancelQuery)
			{
				QueryCleanup();
				OnQueryCompleted(MyGameServiceCallResult.OK);
			}
			else if (flag)
			{
				CommitRemaining(eResult);
				OnQueryCompleted(MyGameServiceCallResult.OK);
			}
			else
			{
				m_pageSteam++;
				RunQuery();
			}
		}

		private bool CommitItemPages(MyGameServiceCallResult result)
		{
			int num = ItemsInternal.Count - (int)m_steamPageItemSkip;
			if (base.Items.Count + ItemsPerPage > num)
			{
				return false;
			}
			for (int i = 0; i < ItemsPerPage; i++)
			{
				base.Items.Add(ItemsInternal[base.Items.Count + (int)m_steamPageItemSkip]);
			}
			m_pageCurrent++;
			OnPageQueryCompleted(result, m_pageCurrent);
			return true;
		}

		private void CommitRemaining(MyGameServiceCallResult result)
		{
			int num = ItemsInternal.Count - (int)m_steamPageItemSkip;
			if (base.Items.Count != num)
			{
				int num2 = num - base.Items.Count;
				for (int i = 0; i < num2; i++)
				{
					base.Items.Add(ItemsInternal[base.Items.Count + (int)m_steamPageItemSkip]);
				}
				m_pageCurrent++;
				OnPageQueryCompleted(result, m_pageCurrent);
			}
		}
	}
}
