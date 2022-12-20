namespace VRage.GameServices
{
	public class MyNullUGCService : IMyUGCService
	{
		public string ServiceName { get; }

		public string LegalUrl { get; }

		public bool IsConsoleCompatible { get; }

		public bool IsConsentGiven
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public MyWorkshopItem CreateWorkshopItem()
		{
			return new MyWorkshopItem();
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher()
		{
			return new MyNullWorkshopItemPublisher();
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return new MyNullWorkshopItemPublisher();
		}

		public MyWorkshopQuery CreateWorkshopQuery()
		{
			return new MyNullWorkshopQuery();
		}

		public void SuspendWorkshopDownloads()
		{
		}

		public void ResumeWorkshopDownloads()
		{
		}

		public string GetItemListUrl(string requiredTag)
		{
			return string.Empty;
		}

		public void SetTestEnvironment(bool testEnabled)
		{
		}

		public void Update()
		{
		}

		public bool IsConsentRequired()
		{
			return false;
		}
	}
}
