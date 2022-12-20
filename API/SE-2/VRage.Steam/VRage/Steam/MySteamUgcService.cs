using System;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	public static class MySteamUgcService
	{
		public static IMyUGCService Create(uint appId, IMyGameService gameService)
		{
			return new MySteamUGCService(appId, gameService);
		}
	}
	internal class MySteamUGCService : IDisposable, IMyUGCService
	{
		internal uint UGCItemsPerPage = 50u;

		internal uint UGCMaxMetadataLength = 5000u;

		internal readonly AppId_t SteamAppId;

		private readonly IMyGameService m_gameService;

		private uint m_webAppId;

		internal ulong UserId => m_gameService.UserId;

		public string LegalUrl => "http://steamcommunity.com/sharedfiles/workshoplegalagreement";

		public string ServiceName => "Steam";

		public bool IsConsoleCompatible => false;

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

		public MySteamUGCService(uint appId, IMyGameService gameService)
		{
			m_webAppId = appId;
			SteamAppId = (AppId_t)appId;
			m_gameService = gameService;
		}

		public MyWorkshopItem CreateWorkshopItem()
		{
			return new MySteamWorkshopItem(this);
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher()
		{
			return new MySteamWorkshopItemPublisher(this);
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return new MySteamWorkshopItemPublisher(this, item);
		}

		public MyWorkshopQuery CreateWorkshopQuery()
		{
			return new MySteamWorkshopQuery(this);
		}

		public void SuspendWorkshopDownloads()
		{
			SteamUGC.SuspendDownloads(bSuspend: true);
		}

		public void ResumeWorkshopDownloads()
		{
			SteamUGC.SuspendDownloads(bSuspend: false);
		}

		public string GetItemListUrl(string requiredTag)
		{
			return $"http://steamcommunity.com/workshop/browse/?appid={m_webAppId}&requiredtags%5B%5D={requiredTag}";
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

		public void Dispose()
		{
		}

		public bool HasFriend(ulong itemOwnerId)
		{
			return m_gameService.HasFriend(itemOwnerId);
		}
	}
}
