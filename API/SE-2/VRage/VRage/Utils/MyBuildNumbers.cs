using System.IO;

namespace VRage.Utils
{
	public static class MyBuildNumbers
	{
		private const int LENGTH_MAJOR = 2;

		private const int LENGTH_MINOR1 = 3;

		private const int LENGTH_MINOR2 = 3;

		public const string SEPARATOR = "_";

		public static int GetBuildNumberWithoutMajor(int buildNumberInt)
		{
			int num = 1;
			for (int i = 0; i < 6; i++)
			{
				num *= 10;
			}
			return buildNumberInt - buildNumberInt / num * num;
		}

		public static string ConvertBuildNumberFromIntToString(int buildNumberInt)
		{
			return ConvertBuildNumberFromIntToString(buildNumberInt, "_");
		}

		public static string ConvertBuildNumberFromIntToString(int buildNumberInt, string separator)
		{
			string text = MyUtils.AlignIntToRight(buildNumberInt, 8, '0');
			return text.Substring(0, 2) + separator + text.Substring(2, 3) + separator + text.Substring(5, 3);
		}

		public static string ConvertBuildNumberFromIntToStringFriendly(int buildNumberInt, string separator)
		{
			int num = 1;
			string text = MyUtils.AlignIntToRight(buildNumberInt, num + 3 + 3, '0');
			return text.Substring(0, num) + separator + text.Substring(num, 3) + separator + text.Substring(num + 3, 3);
		}

		public static bool IsValidBuildNumber(string buildNumberString)
		{
			return ConvertBuildNumberFromStringToInt(buildNumberString).HasValue;
		}

		public static int? ConvertBuildNumberFromStringToInt(string buildNumberString)
		{
			if (buildNumberString.Length < 2 * "_".Length + 2 + 3 + 3)
			{
				return null;
			}
			if (buildNumberString.Substring(2, "_".Length) != "_" || buildNumberString.Substring(2 + "_".Length + 3, "_".Length) != "_")
			{
				return null;
			}
			string text = buildNumberString.Substring(0, 2);
			string text2 = buildNumberString.Substring(2 + "_".Length, 3);
			string text3 = buildNumberString.Substring(2 + "_".Length + 3 + "_".Length, 3);
			if (!int.TryParse(text, out var _))
			{
				return null;
			}
			if (!int.TryParse(text2, out var _))
			{
				return null;
			}
			if (!int.TryParse(text3, out var _))
			{
				return null;
			}
			return int.Parse(text + text2 + text3);
		}

		public static int? GetBuildNumberFromFileName(string filename, string executableFileName, string extensionName)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (filename.Length < executableFileName.Length + 3 * "_".Length + 2 + 3 + 3)
			{
				return null;
			}
			if (filename.Substring(executableFileName.Length, "_".Length) != "_")
			{
				return null;
			}
			if (((FileSystemInfo)new FileInfo(filename)).get_Extension() != extensionName)
			{
				return null;
			}
			return ConvertBuildNumberFromStringToInt(filename.Substring(executableFileName.Length + "_".Length, 2 + "_".Length + 3 + "_".Length + 3));
		}

		public static string GetFilenameFromBuildNumber(int buildNumber, string executableFileName)
		{
			return executableFileName + "_" + ConvertBuildNumberFromIntToString(buildNumber) + ".exe";
		}
	}
}
