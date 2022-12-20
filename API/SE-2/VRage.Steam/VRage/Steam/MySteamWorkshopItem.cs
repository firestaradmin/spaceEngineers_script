using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Steam.Steamworks;
using VRage.Utils;

namespace VRage.Steam
{
	internal class MySteamWorkshopItem : MyWorkshopItem
	{
		private readonly TimeSpan m_previewImageCacheTimeout = new TimeSpan(0, 5, 0);

		private readonly string[] m_steamFormattingTags = new string[24]
		{
			"[h1]", "[/h1]", "[b]", "[/b]", "[u]", "[/u]", "[i]", "[/i]", "[strike]", "[/strike]",
			"[spoiler]", "[/spoiler]", "[noparse]", "[/noparse]", "[/url]", "[img]", "[/img]", "[code]", "[/code]", "[list]",
			"[/list]", "[*]", "[olist]", "[/olist]"
		};

		private ulong m_bytesDownloaded;

		private ulong m_bytesTotal;

		private readonly MySteamUGCService m_steamService;

		private Callback<DownloadItemResult_t> m_downloadItemResult;

		private Callback<DownloadItemResult_t> m_downloadItemResultServer;

		private CallResult<RemoteStorageDownloadUGCResult_t> m_downloadPreviewImageResult;

		private CallResult<GetAppDependenciesResult_t> m_getAppDependenciesResult;

		private CallResult<RemoteStorageSubscribePublishedFileResult_t> m_subscribeResult;

		private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> m_unsubscribeResult;

		private UGCHandle_t m_previewFileHandle;

		private Action<MyWorkshopItem, bool> m_downloadPreviewImageCompleteCallback;

		private string m_previewImageFileTarget;

		private CallResult<GetUserItemVoteResult_t> m_getUserItemVoteResult;

		public override string ServiceName => m_steamService.ServiceName;

		private IMySteamUgc SteamUGC => MySteamUgc.Instance;

		public override ulong BytesDownloaded
		{
			get
			{
				UpdateDownloadProgress();
				return m_bytesDownloaded;
			}
		}

		public override ulong BytesTotal
		{
			get
			{
				UpdateDownloadProgress();
				return m_bytesTotal;
			}
		}

		public override float DownloadProgress
		{
			get
			{
				UpdateDownloadProgress();
				if (m_bytesTotal == 0L)
				{
					return 0f;
				}
				return (float)m_bytesDownloaded / (float)m_bytesTotal;
			}
		}

		public override string PreviewImageHandle => m_previewFileHandle.ToString();

		internal MySteamWorkshopItem(MySteamUGCService service)
		{
			m_steamService = service;
			m_downloadItemResult = Callback<DownloadItemResult_t>.Create(DownloadWorkshopItemResult);
			m_downloadItemResultServer = Callback<DownloadItemResult_t>.CreateGameServer(DownloadWorkshopItemResultServer);
		}

		internal MySteamWorkshopItem(MySteamUGCService service, SteamUGCDetails_t details)
			: this(service)
		{
			base.Id = (ulong)details.m_nPublishedFileId;
			base.OwnerId = details.m_ulSteamIDOwner;
			base.Title = details.m_rgchTitle;
			base.Description = CleanSteamTextFormatting(details.m_rgchDescription);
			base.Size = (uint)details.m_nFileSize;
			base.ItemType = MyWorkshopItemType.Item;
			if (details.m_eFileType == EWorkshopFileType.k_EWorkshopFileTypeCollection)
			{
				base.ItemType = MyWorkshopItemType.Collection;
			}
			base.Visibility = (MyPublishedFileVisibility)details.m_eVisibility;
			string[] collection = details.m_rgchTags.ToLowerInvariant().Split(new char[1] { ',' });
			m_tags = new List<string>(collection);
			base.TimeUpdated = details.m_rtimeUpdated.ToDateTimeFromUnixTimestamp();
			base.TimeCreated = details.m_rtimeCreated.ToDateTimeFromUnixTimestamp();
			base.Score = details.m_flScore;
			UpdateRating();
			SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies((PublishedFileId_t)base.Id);
			m_getAppDependenciesResult = CallResult<GetAppDependenciesResult_t>.Create();
			m_getAppDependenciesResult.Set(appDependencies, GetAppDependenciesResult);
			m_previewFileHandle = details.m_hPreviewFile;
			base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
			if (base.State.HasFlag(MyWorkshopItemState.Installed))
			{
				UpdateInstalledItem();
			}
		}

		private void GetUserItemVoteResult(GetUserItemVoteResult_t param, bool failure)
		{
			if (!failure)
			{
				base.MyRating = (param.m_bVotedUp ? 1 : (param.m_bVotedDown ? (-1) : 0));
			}
		}

		public override void Rate(bool positive)
		{
			SteamUGC.SetUserItemVote((PublishedFileId_t)base.Id, positive);
		}

		public override void UpdateRating()
		{
			m_getUserItemVoteResult = m_getUserItemVoteResult ?? CallResult<GetUserItemVoteResult_t>.Create();
			m_getUserItemVoteResult.Set(SteamUGC.GetUserItemVote((PublishedFileId_t)base.Id), GetUserItemVoteResult);
		}

		private string CleanSteamTextFormatting(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			for (int i = 0; i < m_steamFormattingTags.Length; i++)
			{
				text = text.Replace(m_steamFormattingTags[i], string.Empty);
			}
			int num = 0;
			while (num != -1)
			{
				num = text.IndexOf("[url=");
				if (num == -1)
				{
					break;
				}
				int num2 = text.IndexOf("]", num);
				if (num2 <= num)
				{
					break;
				}
				text = text.Remove(num, num2 - num + 1);
			}
			return text;
		}

		~MySteamWorkshopItem()
		{
			m_downloadItemResult.Dispose();
			m_downloadItemResultServer.Dispose();
			m_getAppDependenciesResult?.Dispose();
			m_getUserItemVoteResult?.Dispose();
			if (m_downloadPreviewImageResult != null)
			{
				m_downloadPreviewImageResult.Dispose();
			}
		}

		internal void SetMetadata(string meta)
		{
			SetMetadata(MyModMetadataLoader.Parse(meta));
		}

		internal void SetMetadata(MyModMetadata meta)
		{
			base.Metadata = meta;
		}

		internal void AddDependency(ulong dependency)
		{
			if (m_dependencies == null)
			{
				m_dependencies = new List<ulong>();
			}
			m_dependencies.Add(dependency);
		}

		internal void AddDependency(IEnumerable<ulong> dependencies)
		{
			if (m_dependencies == null)
			{
				m_dependencies = new List<ulong>(dependencies);
			}
			else
			{
				m_dependencies.AddRange(dependencies);
			}
		}

		internal void RemoveDependency(ulong dependency)
		{
			if (m_dependencies != null)
			{
				m_dependencies.Remove(dependency);
			}
		}

		internal void ClearDependency()
		{
			if (m_dependencies != null)
			{
				m_dependencies.Clear();
			}
		}

		public override MyWorkshopItemPublisher GetPublisher()
		{
			return new MySteamWorkshopItemPublisher(m_steamService, this);
		}

		public override void Download()
		{
			base.Download();
			if (base.Id == 0L)
			{
				OnItemDownloaded(MyGameServiceCallResult.Fail, base.Id);
				return;
			}
			base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
			if (!SteamUGC.DownloadItem((PublishedFileId_t)base.Id, bHighPriority: true))
			{
				UpdateState();
				if (IsUpToDate())
				{
					OnItemDownloaded(MyGameServiceCallResult.OK, base.Id);
				}
				else
				{
					OnItemDownloaded(MyGameServiceCallResult.Fail, base.Id);
				}
			}
		}

		public override void DownloadPreviewImage(string directory, Action<MyWorkshopItem, bool> completeCallback)
		{
			if (m_previewFileHandle == UGCHandle_t.Invalid)
			{
				completeCallback.InvokeIfNotNull(this, arg2: false);
				return;
			}
			MyFileSystem.EnsureDirectoryExists(directory);
			m_previewImageFileTarget = Path.Combine(directory, m_previewFileHandle.m_UGCHandle + ".png");
			if (MyFileSystem.FileExists(m_previewImageFileTarget))
			{
				if (File.GetLastWriteTimeUtc(m_previewImageFileTarget) + m_previewImageCacheTimeout > DateTime.UtcNow)
				{
					base.PreviewImageFile = m_previewImageFileTarget;
					completeCallback.InvokeIfNotNull(this, arg2: true);
					return;
				}
				File.Delete(m_previewImageFileTarget);
			}
			m_downloadPreviewImageCompleteCallback = completeCallback;
			SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownloadToLocation(m_previewFileHandle, m_previewImageFileTarget, 0u);
			m_downloadPreviewImageResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create();
			m_downloadPreviewImageResult.Set(hAPICall, DownloadPreviewImageResult);
		}

		private void DownloadPreviewImageResult(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
		{
			if (param.m_eResult != EResult.k_EResultOK || param.m_hFile != m_previewFileHandle)
			{
				m_downloadPreviewImageCompleteCallback.InvokeIfNotNull(this, arg2: false);
				return;
			}
			base.PreviewImageFile = m_previewImageFileTarget;
			if (MyFileSystem.FileExists(m_previewImageFileTarget))
			{
				File.SetLastWriteTimeUtc(m_previewImageFileTarget, DateTime.UtcNow);
			}
			m_downloadPreviewImageCompleteCallback.InvokeIfNotNull(this, arg2: true);
			m_downloadPreviewImageResult.Dispose();
			m_downloadPreviewImageResult = null;
		}

		public override void UpdateState()
		{
			base.UpdateState();
			if (base.Id != 0L)
			{
				base.LocalTimeUpdated = DateTimeExtensions.Epoch;
				base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
				if (base.State.HasFlag(MyWorkshopItemState.Installed))
				{
					UpdateInstalledItem();
				}
			}
		}

		public override void Subscribe()
		{
			SteamAPICall_t hAPICall = SteamUGC.SubscribeItem((PublishedFileId_t)base.Id);
			m_subscribeResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create();
			m_subscribeResult.Set(hAPICall, OnSubscribeResult);
		}

		private void OnSubscribeResult(RemoteStorageSubscribePublishedFileResult_t param, bool bIOFailure)
		{
			base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
			m_subscribeResult.Dispose();
			m_subscribeResult = null;
		}

		public override void Unsubscribe()
		{
			SteamAPICall_t hAPICall = SteamUGC.UnsubscribeItem((PublishedFileId_t)base.Id);
			m_unsubscribeResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create();
			m_unsubscribeResult.Set(hAPICall, OnUnsubscribeResult);
		}

		private void OnUnsubscribeResult(RemoteStorageUnsubscribePublishedFileResult_t param, bool bIOFailure)
		{
			base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
			m_unsubscribeResult.Dispose();
			m_unsubscribeResult = null;
		}

		public override bool IsUpToDate()
		{
			if (!base.State.HasFlag(MyWorkshopItemState.LegacyItem))
			{
				return base.IsUpToDate();
			}
			return base.LocalTimeUpdated >= base.TimeUpdated;
		}

		public override string GetItemUrl()
		{
			return $"http://steamcommunity.com/sharedfiles/filedetails/?id={base.Id}";
		}

		private void UpdateDownloadProgress()
		{
			SteamUGC.GetItemDownloadInfo((PublishedFileId_t)base.Id, out m_bytesDownloaded, out m_bytesTotal);
		}

		private void UpdateInstalledItem()
		{
			SteamUGC.GetItemInstallInfo((PublishedFileId_t)base.Id, out var punSizeOnDisk, out var pchFolder, 260u, out var punTimeStamp);
			if (!base.State.HasFlag(MyWorkshopItemState.LegacyItem))
			{
				base.Size = punSizeOnDisk;
			}
			base.Folder = pchFolder;
			base.LocalTimeUpdated = punTimeStamp.ToDateTimeFromUnixTimestamp();
			if (!Directory.Exists(base.Folder) || base.TimeUpdated > base.LocalTimeUpdated)
			{
				base.State |= MyWorkshopItemState.NeedsUpdate;
			}
		}

		private void DownloadWorkshopItemResultServer(DownloadItemResult_t result)
		{
			if (!(result.m_unAppID != m_steamService.SteamAppId) && !(result.m_nPublishedFileId != (PublishedFileId_t)base.Id))
			{
				MyLog.Default.WriteLineAndConsole($"Workshop item with id {base.Id} download finished. Result: {result.m_eResult}");
				base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
				if (result.m_eResult == EResult.k_EResultOK)
				{
					UpdateInstalledItem();
				}
				OnItemDownloaded((MyGameServiceCallResult)result.m_eResult, base.Id);
			}
		}

		private void DownloadWorkshopItemResult(DownloadItemResult_t result)
		{
			if (!(result.m_unAppID != m_steamService.SteamAppId) && !(result.m_nPublishedFileId != (PublishedFileId_t)base.Id))
			{
				MyLog.Default.WriteLineAndConsole($"Workshop item with id {base.Id} download finished. Result: {result.m_eResult}");
				base.State = (MyWorkshopItemState)SteamUGC.GetItemState((PublishedFileId_t)base.Id);
				if (result.m_eResult == EResult.k_EResultOK)
				{
					UpdateInstalledItem();
				}
				OnItemDownloaded((MyGameServiceCallResult)result.m_eResult, base.Id);
			}
		}

		private void GetAppDependenciesResult(GetAppDependenciesResult_t result, bool ioFailure)
		{
			if (!ioFailure && result.m_eResult == EResult.k_EResultOK && !(result.m_nPublishedFileId != (PublishedFileId_t)base.Id) && result.m_nTotalNumAppDependencies != 0)
			{
				if (m_DLCs != null)
				{
					m_DLCs.Clear();
				}
				else
				{
					m_DLCs = new List<uint>((int)result.m_nNumAppDependencies);
				}
				for (int i = 0; i < result.m_nNumAppDependencies; i++)
				{
					m_DLCs.Add((uint)result.m_rgAppIDs[i]);
				}
			}
		}
	}
}
