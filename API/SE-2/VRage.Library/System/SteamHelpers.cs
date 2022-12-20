using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System
{
	public static class SteamHelpers
	{
		public static bool IsSteamPath(string path)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			try
			{
				DirectoryInfo val = new DirectoryInfo(path);
				return val.get_Parent() != null && val.get_Parent().get_Parent() != null && ((FileSystemInfo)val.get_Parent()).get_Name().Equals("Common", StringComparison.InvariantCultureIgnoreCase) && ((FileSystemInfo)val.get_Parent().get_Parent()).get_Name().Equals("SteamApps", StringComparison.InvariantCultureIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		public static bool IsAppManifestPresent(string path, uint appId)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			try
			{
				DirectoryInfo val = new DirectoryInfo(path);
				return IsSteamPath(path) && Enumerable.Contains<string>((IEnumerable<string>)Directory.GetFiles(((FileSystemInfo)val.get_Parent().get_Parent()).get_FullName()), "AppManifest_" + appId + ".acf", (IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
			}
			catch
			{
				return false;
			}
		}
	}
}
