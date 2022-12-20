using System;
using System.Collections.Generic;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Models
{
	public class MyWorkshopItemModel : BindableBase
	{
		private readonly int m_numberOfStars = 5;

		private readonly string m_fullStar = "Textures\\GUI\\Icons\\Rating\\FullStar.png";

		private readonly string m_halfStar = "Textures\\GUI\\Icons\\Rating\\HalfStar.png";

		private readonly string m_noStar = "Textures\\GUI\\Icons\\Rating\\NoStar.png";

		private bool m_isSubscribed;

		private BitmapImage m_previewImage;

		private MyWorkshopItem m_workshopItem;

		private List<BitmapImage> m_rating;

		public Action OnIsSubscribedChanged;

		public ulong Id
		{
			get
			{
				if (WorkshopItem == null)
				{
					return 0uL;
				}
				return WorkshopItem.Id;
			}
		}

		public string Title
		{
			get
			{
				if (WorkshopItem == null)
				{
					return string.Empty;
				}
				return WorkshopItem.Title;
			}
		}

		public string Description
		{
			get
			{
				if (WorkshopItem == null)
				{
					return string.Empty;
				}
				return WorkshopItem.Description;
			}
		}

		public DateTime TimeUpdated
		{
			get
			{
				if (WorkshopItem == null)
				{
					return DateTime.MinValue;
				}
				return WorkshopItem.TimeUpdated;
			}
		}

		public DateTime TimeCreated
		{
			get
			{
				if (WorkshopItem == null)
				{
					return DateTime.MinValue;
				}
				return WorkshopItem.TimeCreated;
			}
		}

		public ulong Size
		{
			get
			{
				if (WorkshopItem == null)
				{
					return 0uL;
				}
				return WorkshopItem.Size;
			}
		}

		public float Score
		{
			get
			{
				if (WorkshopItem == null)
				{
					return 0f;
				}
				return WorkshopItem.Score;
			}
		}

		public ulong NumSubscriptions
		{
			get
			{
				if (WorkshopItem == null)
				{
					return 0uL;
				}
				return WorkshopItem.NumSubscriptions;
			}
		}

		public bool IsSubscribed
		{
			get
			{
				return m_isSubscribed;
			}
			set
			{
				SetProperty(ref m_isSubscribed, value, "IsSubscribed");
				if (WorkshopItem != null)
				{
					bool flag = WorkshopItem.State.HasFlag(MyWorkshopItemState.Subscribed);
					if (m_isSubscribed && !flag)
					{
						WorkshopItem.Subscribe();
					}
					else if (!m_isSubscribed && flag)
					{
						WorkshopItem.Unsubscribe();
					}
					if (OnIsSubscribedChanged != null)
					{
						OnIsSubscribedChanged();
					}
				}
			}
		}

		public MyWorkshopItem WorkshopItem
		{
			get
			{
				return m_workshopItem;
			}
			set
			{
				SetProperty(ref m_workshopItem, value, "WorkshopItem");
				if (m_workshopItem != null)
				{
					CreateRating();
					IsSubscribed = WorkshopItem != null && WorkshopItem.State.HasFlag(MyWorkshopItemState.Subscribed);
				}
			}
		}

		public BitmapImage PreviewImage
		{
			get
			{
				return m_previewImage;
			}
			set
			{
				SetProperty(ref m_previewImage, value, "PreviewImage");
			}
		}

		public List<BitmapImage> Rating
		{
			get
			{
				return m_rating;
			}
			set
			{
				SetProperty(ref m_rating, value, "Rating");
			}
		}

		internal void OnDownloadPreviewImageCompleted(MyWorkshopItem item, bool success)
		{
			if (item == WorkshopItem && success)
			{
				ImageManager.Instance.AddImage(item.PreviewImageFile);
				ImageManager.Instance.LoadImages(null);
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.TextureAsset = item.PreviewImageFile;
				PreviewImage = bitmapImage;
			}
		}

		private void CreateRating()
		{
			List<BitmapImage> list = new List<BitmapImage>(m_numberOfStars);
			float num = m_workshopItem.Score * (float)m_numberOfStars;
			for (int i = 0; i < (int)num; i++)
			{
				BitmapImage item = new BitmapImage
				{
					TextureAsset = m_fullStar
				};
				list.Add(item);
			}
			if (Math.Round(num, 0, MidpointRounding.AwayFromZero) >= (double)num)
			{
				BitmapImage item2 = new BitmapImage
				{
					TextureAsset = m_fullStar
				};
				list.Add(item2);
			}
			else
			{
				BitmapImage item3 = new BitmapImage
				{
					TextureAsset = m_halfStar
				};
				list.Add(item3);
			}
			int num2 = m_numberOfStars - list.Count;
			for (int j = 0; j < num2; j++)
			{
				BitmapImage item4 = new BitmapImage
				{
					TextureAsset = m_noStar
				};
				list.Add(item4);
			}
			Rating = list;
		}
	}
}
