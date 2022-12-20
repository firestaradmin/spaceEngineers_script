using VRage.Utils;

namespace VRage.Platform.Windows
{
	public static class MyVRageWindows
	{
		public static void Init(string applicationName, MyLog log, string appDataPath, bool detectLeaks, bool performInit = true)
		{
			MyVRage.Init(new MyVRagePlatform(applicationName, log, appDataPath, detectLeaks));
			if (performInit)
			{
				MyVRage.Platform.Init();
			}
		}
	}
}
