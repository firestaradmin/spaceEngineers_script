using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.GameServices
{
	public class MyWorkshopItem
	{
		public delegate void DownloadItemResult(MyGameServiceCallResult result, ulong publishedId);

		private class MyWorkshopItemComparer : IComparer<MyWorkshopItem>
		{
			public int Compare(MyWorkshopItem x, MyWorkshopItem y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x != null && y == null)
				{
					return 1;
				}
				if (x == null && y != null)
				{
					return -1;
				}
				return string.CompareOrdinal(x.Title, y.Title);
			}
		}

		protected List<string> m_tags = new List<string>();

		protected List<ulong> m_dependencies = new List<ulong>();

		protected List<uint> m_DLCs = new List<uint>();

		public static IComparer<MyWorkshopItem> NameComparer = new MyWorkshopItemComparer();

		public string Title { get; set; }

		public string Description { get; protected set; }

		public string Thumbnail { get; protected set; }

		public string Folder { get; protected set; }

		public MyWorkshopItemType ItemType { get; protected set; }

		public ulong Id { get; set; }

		public ulong OwnerId { get; protected set; }

		public DateTime TimeUpdated { get; protected set; }

		public DateTime LocalTimeUpdated { get; protected set; }

		public DateTime TimeCreated { get; protected set; }

		public virtual ulong BytesDownloaded { get; protected set; }

		public virtual ulong BytesTotal { get; protected set; }

		public virtual float DownloadProgress { get; protected set; }

<<<<<<< HEAD
		public virtual string PreviewImageHandle { get; protected set; }

		public ulong Size { get; protected set; }

		public MyModMetadata Metadata { get; protected set; }

		public MyPublishedFileVisibility Visibility { get; protected set; }

=======
		public ulong Size { get; protected set; }

		public MyModMetadata Metadata { get; protected set; }

		public MyPublishedFileVisibility Visibility { get; protected set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyWorkshopItemState State { get; protected set; }

		/// <summary>
		/// Whether this mod is compatible with the current game version.
		/// </summary>
		public MyModCompatibility Compatibility { get; set; }

		public ListReader<string> Tags => m_tags;

		public ListReader<ulong> Dependencies => m_dependencies;

		public ListReader<uint> DLCs => m_DLCs;

		public float Score { get; protected set; }

		public int MyRating { get; protected set; }

		public ulong NumSubscriptions { get; set; }

		public string PreviewImageFile { get; protected set; }

		public virtual string ServiceName { get; }

		public event DownloadItemResult ItemDownloaded;

		/// <summary>
		/// Returns a publisher instance that is initialized with properties of this workshop item.
		/// </summary>
		/// <returns></returns>
		public virtual MyWorkshopItemPublisher GetPublisher()
		{
			return null;
		}

		/// <summary>
		/// Download the newest version of this workshop item.
		/// </summary>
		public virtual void Download()
		{
		}

		/// <summary>
		/// Rate this workshop item by giving thumbs up or thumbs down.
		/// </summary>
		public virtual void Rate(bool positive)
		{
		}

		/// <summary>
		/// Updates MyRating property of this workshop item.
		/// </summary>
		public virtual void UpdateRating()
		{
		}

		/// <summary>
		/// Download preview image
		/// </summary>
		public virtual void DownloadPreviewImage(string directory, Action<MyWorkshopItem, bool> completeCallback)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates the workshop item with available information.
		/// First tries local storage.
		/// Then tries remote.
		/// </summary>
		public virtual void UpdateState()
		{
		}

		/// <summary>
		/// Subscribes to the workshop item.
		/// </summary>
		public virtual void Subscribe()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Unsubscribe item on the workshop
		/// </summary>
		public virtual void Unsubscribe()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks whether the mod is up-to-date.
		/// </summary>
		/// <returns></returns>
		public virtual bool IsUpToDate()
		{
			if (State.HasFlag(MyWorkshopItemState.Installed))
			{
				return !State.HasFlag(MyWorkshopItemState.NeedsUpdate);
			}
			return false;
		}

		protected virtual void OnItemDownloaded(MyGameServiceCallResult result, ulong publishedId)
		{
			this.ItemDownloaded?.Invoke(result, publishedId);
		}

		protected bool Equals(MyWorkshopItem other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MyWorkshopItem)obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[{0}] {1}", Id, Title ?? "N/A");
		}

		public virtual void UpdateDependencyBlocking()
		{
		}

		public virtual void Report(string reason)
		{
		}

		public void ChangeRatingValue(bool positive)
		{
			MyRating = (positive ? 1 : (-1));
		}

		public virtual string GetItemUrl()
		{
			return string.Empty;
		}

		public void Init(MyWorkshopItemPublisher publisher)
		{
			Id = publisher.Id;
			Title = publisher.Title;
			Description = publisher.Description;
			Metadata = publisher.Metadata;
			Visibility = publisher.Visibility;
			m_tags = new List<string>(publisher.Tags);
<<<<<<< HEAD
			m_DLCs = new List<uint>(publisher.DLCs);
=======
			m_DLCs = new List<uint>((IEnumerable<uint>)publisher.DLCs);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_dependencies = new List<ulong>(publisher.Dependencies);
		}
	}
}
