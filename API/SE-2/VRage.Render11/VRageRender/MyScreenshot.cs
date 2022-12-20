using System;
using System.IO;
using VRage.FileSystem;
using VRage.Render.Image;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	internal struct MyScreenshot
	{
		internal readonly MyImage.FileFormat Format;

		internal readonly string SavePath;

		internal readonly Vector2 SizeMult;

		internal readonly bool IgnoreSprites;

		internal readonly bool ShowNotification;

		private static readonly string screenshotsFolder;

		static MyScreenshot()
		{
			screenshotsFolder = "Screenshots";
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, screenshotsFolder));
		}

		internal MyScreenshot(string path, Vector2 sizeMult, bool ignoreSprites, bool showNotification)
		{
			SavePath = path ?? GetDefaultScreenshotFilenameWithExtension();
			Format = MyTextureData.GetFormat(Path.GetExtension(SavePath).ToLower());
			SizeMult = sizeMult;
			IgnoreSprites = ignoreSprites;
			ShowNotification = showNotification;
		}

		private static string GetDefaultScreenshotFilenameWithExtension()
		{
			return Path.Combine(MyFileSystem.UserDataPath, screenshotsFolder, MyValueFormatter.GetFormatedDateTimeForFilename(DateTime.Now) + ".png");
		}
	}
}
