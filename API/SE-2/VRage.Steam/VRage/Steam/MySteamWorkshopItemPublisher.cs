using Steamworks;
using VRage.GameServices;
using VRage.Steam.Steamworks;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamWorkshopItemPublisher : MyWorkshopItemPublisher
	{
		private PublishedFileId_t m_itemId;

		private MySteamUGCService m_steamService;

		private CallResult<CreateItemResult_t> m_createItemResult;

		private CallResult<SubmitItemUpdateResult_t> m_submitItemUpdateResult;

		private CallResult<AddAppDependencyResult_t> m_addItemDependencyResult;

		private IMySteamUgc SteamUGC => MySteamUgc.Instance;

		internal MySteamWorkshopItemPublisher(MySteamUGCService service)
		{
			m_steamService = service;
			m_createItemResult = CallResult<CreateItemResult_t>.Create();
			m_submitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create();
			m_addItemDependencyResult = CallResult<AddAppDependencyResult_t>.Create();
		}

		internal MySteamWorkshopItemPublisher(MySteamUGCService service, MyWorkshopItem item)
			: this(service)
		{
			if (item != null)
			{
				Init(item);
			}
		}

		~MySteamWorkshopItemPublisher()
		{
			m_createItemResult.Dispose();
			m_submitItemUpdateResult.Dispose();
			m_addItemDependencyResult.Dispose();
		}

		public override void Publish()
		{
			base.Publish();
			if (base.Id == 0L)
			{
				SteamAPICall_t hAPICall = SteamUGC.CreateItem(m_steamService.SteamAppId, EWorkshopFileType.k_EWorkshopFileTypeFirst);
				m_createItemResult.Set(hAPICall, CreateItemResult);
			}
			else
			{
				m_itemId = (PublishedFileId_t)base.Id;
				UpdatePublishedItem();
			}
		}

		private void UpdatePublishedItem()
		{
			UGCUpdateHandle_t uGCUpdateHandle_t = SteamUGC.StartItemUpdate(m_steamService.SteamAppId, m_itemId);
			string pchTitle = (string.IsNullOrWhiteSpace(base.Folder) ? $"Item {base.Id}" : base.Title);
			SteamUGC.SetItemTitle(uGCUpdateHandle_t, pchTitle);
			SteamUGC.SetItemTags(uGCUpdateHandle_t, Tags);
			SteamUGC.SetItemVisibility(uGCUpdateHandle_t, base.Visibility.ToSteam());
			if (!string.IsNullOrWhiteSpace(base.Description))
			{
				SteamUGC.SetItemDescription(uGCUpdateHandle_t, base.Description);
			}
			if (!string.IsNullOrWhiteSpace(base.Folder))
			{
				SteamUGC.SetItemContent(uGCUpdateHandle_t, base.Folder);
			}
			if (!string.IsNullOrWhiteSpace(base.Thumbnail))
			{
				SteamUGC.SetItemPreview(uGCUpdateHandle_t, base.Thumbnail);
			}
			string empty = string.Empty;
			if (base.Metadata != null)
			{
				empty = MyModMetadataLoader.Serialize(base.Metadata);
				SteamUGC.SetItemMetadata(uGCUpdateHandle_t, empty);
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(uGCUpdateHandle_t, string.Empty);
			m_submitItemUpdateResult.Set(hAPICall, SubmitItemUpdateResult);
		}

		private void CreateItemResult(CreateItemResult_t result, bool ioFailure)
		{
			if (ioFailure || result.m_eResult != EResult.k_EResultOK)
			{
				MyWorkshopItem myWorkshopItem = m_steamService.CreateWorkshopItem();
				myWorkshopItem.Init(this);
				MyLog.Default.Info("Failed to create new workshop item. Result: {0} / {1}", result.m_eResult, ioFailure);
				OnItemPublished((MyGameServiceCallResult)(ioFailure ? EResult.k_EResultIOFailure : result.m_eResult), myWorkshopItem);
			}
			else
			{
				m_itemId = result.m_nPublishedFileId;
				base.Id = (ulong)m_itemId;
				MyLog.Default.Info($"New workshop item with id {base.Id} created successfully.");
				UpdatePublishedItem();
			}
		}

		private void SubmitItemUpdateResult(SubmitItemUpdateResult_t result, bool ioFailure)
		{
			MyWorkshopItem myWorkshopItem = m_steamService.CreateWorkshopItem();
			myWorkshopItem.Init(this);
			if (ioFailure)
			{
				OnItemPublished(MyGameServiceCallResult.IOFailure, myWorkshopItem);
				return;
			}
			OnItemPublished((MyGameServiceCallResult)result.m_eResult, myWorkshopItem);
			MyLog.Default.Info("Workshop item with id {0} update finished. Result: {1}", base.Id, result.m_eResult);
			foreach (uint dLC in DLCs)
			{
				SteamAPICall_t hAPICall = SteamUGC.AddAppDependency((PublishedFileId_t)base.Id, (AppId_t)dLC);
				m_addItemDependencyResult.Set(hAPICall, AddAppDependencyResult);
			}
			SteamUGC.SubscribeItem(m_itemId);
		}

		private void AddAppDependencyResult(AddAppDependencyResult_t result, bool ioFailure)
		{
			if (!ioFailure)
			{
				MyLog.Default.Info("Workshop item with id {0} app dependency update finished. Result: {1}", base.Id, result.m_eResult);
			}
		}
	}
}
