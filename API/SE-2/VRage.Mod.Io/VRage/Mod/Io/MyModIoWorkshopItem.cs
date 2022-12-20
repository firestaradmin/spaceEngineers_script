using System;
using System.Collections.Generic;
using VRage.GameServices;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopItem : MyWorkshopItem
	{
		private ModProfile m_profile;

		private ulong m_bytesDownloaded;

		private ulong m_bytesTotal;

		private MyWorkshopItemState m_subscribedState;

		internal string NameId;

		private readonly MyModIoServiceInternal m_service;

		public override string ServiceName => m_service.ServiceName;

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

		public MyModIoWorkshopItem(MyModIoServiceInternal service)
		{
			m_service = service;
		}

		public MyModIoWorkshopItem(MyModIoServiceInternal service, ModProfile profile, bool subscribed)
		{
			m_service = service;
			m_profile = profile;
			NameId = profile.name_id;
			base.Id = (ulong)profile.id;
			if (MyModIo.IsAuthenticated)
			{
				base.OwnerId = ((profile.submitted_by.id == MyModIo.UserProfile.id) ? MyModIo.ServiceUserId : 0);
			}
			base.Title = profile.name;
			base.Description = profile.description_plaintext ?? profile.summary;
			base.Size = (ulong)profile.modfile.filesize;
			base.ItemType = MyWorkshopItemType.Item;
			base.Visibility = ((profile.visible != 1) ? MyPublishedFileVisibility.Private : MyPublishedFileVisibility.Public);
			m_tags = new List<string>();
			ModTag[] tags = profile.tags;
			foreach (ModTag modTag in tags)
			{
				m_tags.Add(modTag.name);
			}
			base.TimeUpdated = ((uint)profile.date_updated).ToDateTimeFromUnixTimestamp();
			base.TimeCreated = ((uint)profile.date_added).ToDateTimeFromUnixTimestamp();
			base.Score = (float)profile.stats.ratings_weighted_aggregate;
			base.NumSubscriptions = (ulong)profile.stats.subscribers_total;
			UpdateRating();
			if (subscribed)
			{
				base.State = (m_subscribedState = MyWorkshopItemState.Subscribed);
			}
		}

		public override void UpdateDependencyBlocking()
		{
			MyModIo.GetModDependenciesBlocking(base.Id, DependenciesResponse);
		}

		private void DependenciesResponse(RequestPage<ModDependency> dependency, MyGameServiceCallResult result)
		{
			if (m_dependencies != null)
			{
				m_dependencies.Clear();
			}
			else
			{
				m_dependencies = new List<ulong>(dependency.result_total);
			}
			if (result != MyGameServiceCallResult.OK || dependency == null)
			{
				return;
			}
			for (int i = 0; i < dependency.data.Length; i++)
			{
				if (dependency.data[i].mod_id != 0)
				{
					m_dependencies.Add((ulong)dependency.data[i].mod_id);
				}
			}
		}

		private void UpdateDownloadProgress()
		{
			MyModIoCache.GetItemDownloadInfo(m_profile.modfile.download.binary_url, out m_bytesDownloaded, out m_bytesTotal);
		}

		public override MyWorkshopItemPublisher GetPublisher()
		{
			return new MyModIoWorkshopItemPublisher(m_service, this);
		}

		public override void Download()
		{
			base.Download();
			if (base.Id == 0L)
			{
				OnItemDownloaded(MyGameServiceCallResult.Fail, base.Id);
				return;
			}
			base.State = MyModIoCache.GetItemState(m_profile.modfile) | m_subscribedState;
			if (base.State.HasFlag(MyWorkshopItemState.Downloading))
			{
				OnItemDownloaded(MyGameServiceCallResult.Pending, base.Id);
			}
			else if (base.State != MyWorkshopItemState.Installed)
			{
				MyModIoCache.DownloadItem(m_profile.modfile, DownloadItemResponse);
			}
			else
			{
				DownloadItemResponse(MyGameServiceCallResult.OK);
			}
		}

		private void DownloadItemResponse(MyGameServiceCallResult result)
		{
			MyLog.Default.WriteLineAndConsole($"Workshop item with id {base.Id} download finished. Result: {result}");
			base.State = MyModIoCache.GetItemState(m_profile.modfile) | m_subscribedState;
			if (result == MyGameServiceCallResult.OK)
			{
				UpdateInstalledItem();
			}
			OnItemDownloaded(result, base.Id);
		}

		private void UpdateInstalledItem()
		{
			if (MyModIoCache.GetItemInstallInfo(m_profile.modfile, out var size, out var folder, out var timeStamp))
			{
				if (!base.State.HasFlag(MyWorkshopItemState.LegacyItem))
				{
					base.Size = size;
				}
				base.Folder = folder;
				base.LocalTimeUpdated = timeStamp.ToDateTimeFromUnixTimestamp();
			}
			else
			{
				base.State |= MyWorkshopItemState.NeedsUpdate;
			}
		}

		public override void UpdateState()
		{
			base.UpdateState();
			if (base.Id != 0L)
			{
				base.LocalTimeUpdated = DateTimeExtensions.Epoch;
				base.State = MyModIoCache.GetItemState(m_profile.modfile) | m_subscribedState;
				if (base.State.HasFlag(MyWorkshopItemState.Installed))
				{
					UpdateInstalledItem();
				}
			}
		}

		public override void DownloadPreviewImage(string directory, Action<MyWorkshopItem, bool> completeCallback)
		{
			string url = m_profile.logo.thumb_320x180;
			if (string.IsNullOrEmpty(url))
			{
				completeCallback.InvokeIfNotNull(this, arg2: false);
				return;
			}
			ulong modId = (ulong)m_profile.id;
			MyWorkshopItemState itemState = MyModIoCache.GetItemState(modId, url, (uint)m_profile.date_updated);
			if (itemState != MyWorkshopItemState.Installed && (itemState & MyWorkshopItemState.DownloadPending) == 0)
			{
				MyModIoCache.DownloadItem(modId, url, (uint)m_profile.date_updated, delegate(MyGameServiceCallResult x)
				{
					if (x == MyGameServiceCallResult.OK)
					{
						base.PreviewImageFile = MyModIoCache.GetItemFilePath(modId, url);
					}
					completeCallback.InvokeIfNotNull(this, x == MyGameServiceCallResult.OK);
				});
			}
			else
			{
				base.PreviewImageFile = MyModIoCache.GetItemFilePath(modId, url);
				completeCallback.InvokeIfNotNull(this, arg2: true);
			}
		}

		public override void Subscribe()
		{
			MyModIo.Subscribe(base.Id, state: true);
			m_subscribedState = MyWorkshopItemState.Subscribed;
			base.State |= m_subscribedState;
		}

		public override void Unsubscribe()
		{
			MyModIo.Subscribe(base.Id, state: false);
			m_subscribedState = MyWorkshopItemState.None;
			base.State &= ~m_subscribedState;
		}

		public override bool IsUpToDate()
		{
			return (MyModIoCache.GetItemState(m_profile.modfile) & (MyWorkshopItemState.Installed | MyWorkshopItemState.NeedsUpdate)) == MyWorkshopItemState.Installed;
		}

		public override void Report(string reason)
		{
			MyModIo.ReportMod(m_profile.modfile, MyModIo.ReportType.Not_Working, reason);
		}

		public override string GetItemUrl()
		{
			return string.Format(MyModIo.GetWebUrl(), NameId);
		}

		public override void Rate(bool positive)
		{
			MyModIo.Rate(base.Id, positive);
		}

		public override void UpdateRating()
		{
			base.MyRating = MyModIo.GetMyRating(base.Id);
		}
	}
}
