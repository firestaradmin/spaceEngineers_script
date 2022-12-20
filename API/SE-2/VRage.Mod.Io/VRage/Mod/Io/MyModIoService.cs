using VRage.GameServices;

namespace VRage.Mod.Io
{
	public static class MyModIoService
	{
		public static IMyUGCService Create(IMyGameService service, string gameName, string liveGameId, string liveApiKey, string testGameId, string testApiKey, bool testEnabled, bool forceConsent)
		{
			return new MyModIoServiceInternal(service, gameName, liveGameId, liveApiKey, testGameId, testApiKey, testEnabled, forceConsent);
		}
	}
}
