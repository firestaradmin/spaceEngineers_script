using System;
using VRage.GameServices;

namespace VRage.Mod.Io
{
	internal class MyModIoServiceInternal : IDisposable, IMyUGCService
	{
		private readonly IMyGameService m_service;

		private bool m_isConsentGiven;

		private bool m_forceConsent;

		public string ServiceName => "mod.io";

		public string LegalUrl => "https://mod.io/terms";

		public ulong UserId => m_service.UserId;

		public bool IsConsoleCompatible => true;

		public bool IsConsentGiven
		{
			get
			{
				if (m_forceConsent)
				{
					return true;
				}
				return m_isConsentGiven;
			}
			set
			{
				m_isConsentGiven = value;
			}
		}

		public bool HasFriend(ulong userId)
		{
			return m_service.HasFriend(userId);
		}

		public MyModIoServiceInternal(IMyGameService service, string gameName, string liveGameId, string liveApiKey, string testGameId, string testApiKey, bool testEnabled, bool forceConsent)
		{
			m_forceConsent = forceConsent;
			m_service = service;
			MyModIo.Init(m_service, this, gameName, liveGameId, liveApiKey, testGameId, testApiKey, testEnabled);
		}

		public MyWorkshopItem CreateWorkshopItem()
		{
			return new MyModIoWorkshopItem(this);
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher()
		{
			return new MyModIoWorkshopItemPublisher(this);
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return new MyModIoWorkshopItemPublisher(this, item);
		}

		public MyWorkshopQuery CreateWorkshopQuery()
		{
			return new MyModIoWorkshopQuery(this);
		}

		public void SuspendWorkshopDownloads()
		{
			MyModIo.SuspendDownloads(state: true);
		}

		public void ResumeWorkshopDownloads()
		{
			MyModIo.SuspendDownloads(state: false);
		}

		public string GetItemListUrl(string requiredTag)
		{
			return string.Format(MyModIo.GetWebUrl(), "") + "&filter=t&tag[]=" + requiredTag;
		}

		public void SetTestEnvironment(bool testEnabled)
		{
			MyModIo.SetTestEnvironment(testEnabled);
		}

		public void Dispose()
		{
		}

		public void Update()
		{
			MyModIo.Update();
		}
	}
}
