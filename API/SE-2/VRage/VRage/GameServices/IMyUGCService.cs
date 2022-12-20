namespace VRage.GameServices
{
	public interface IMyUGCService
	{
		string ServiceName { get; }

		string LegalUrl { get; }

		bool IsConsoleCompatible { get; }

		bool IsConsentGiven { get; set; }

		MyWorkshopItem CreateWorkshopItem();

		MyWorkshopItemPublisher CreateWorkshopPublisher();

		MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item);

		MyWorkshopQuery CreateWorkshopQuery();

		void SuspendWorkshopDownloads();

		void ResumeWorkshopDownloads();

		string GetItemListUrl(string requiredTag);

		void SetTestEnvironment(bool testEnabled);

		void Update();
	}
}
