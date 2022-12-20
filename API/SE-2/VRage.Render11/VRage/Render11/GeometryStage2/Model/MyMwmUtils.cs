using System.IO;
using VRage.FileSystem;
using VRage.Render11.Resources;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Model
{
	internal static class MyMwmUtils
	{
		public static string GetColorMetalTexture(MyMeshPartInfo mwmPart, string contentPath)
		{
			if (mwmPart.m_MaterialDesc == null)
			{
				return "";
			}
			return MyResourceUtils.GetTextureFullPath(mwmPart.m_MaterialDesc.Textures.Get("ColorMetalTexture", ""), contentPath);
		}

		public static string GetNormalGlossTexture(MyMeshPartInfo mwmPart, string contentPath)
		{
			if (mwmPart.m_MaterialDesc == null)
			{
				return "";
			}
			return MyResourceUtils.GetTextureFullPath(mwmPart.m_MaterialDesc.Textures.Get("NormalGlossTexture", ""), contentPath);
		}

		public static string GetExtensionTexture(MyMeshPartInfo mwmPart, string contentPath)
		{
			if (mwmPart.m_MaterialDesc == null)
			{
				return "";
			}
			return MyResourceUtils.GetTextureFullPath(mwmPart.m_MaterialDesc.Textures.Get("AddMapsTexture", ""), contentPath);
		}

		public static string GetAlphamaskTexture(MyMeshPartInfo mwmPart, string contentPath)
		{
			if (mwmPart.m_MaterialDesc == null)
			{
				return "";
			}
			return MyResourceUtils.GetTextureFullPath(mwmPart.m_MaterialDesc.Textures.Get("AlphamaskTexture", ""), contentPath);
		}

		public static string GetFullMwmFilepath(string mwmFilepath)
		{
			mwmFilepath = (Path.IsPathRooted(mwmFilepath) ? mwmFilepath : Path.Combine(MyFileSystem.ContentPath, mwmFilepath));
			mwmFilepath = mwmFilepath.ToLower();
			if (!mwmFilepath.EndsWith(".mwm"))
			{
				mwmFilepath += ".mwm";
			}
			return mwmFilepath;
		}

		public static string GetFullMwmContentPath(string mwmFilePath)
		{
			string result = null;
			if (Path.IsPathRooted(mwmFilePath) && mwmFilePath.ToLower().Contains("models"))
			{
				result = mwmFilePath.Substring(0, mwmFilePath.ToLower().LastIndexOf("models"));
			}
			return result;
		}

		public static bool IsInContentPath(string mwmFilePath)
		{
			if (Path.IsPathRooted(mwmFilePath) && !mwmFilePath.ToLower().StartsWith(MyFileSystem.ContentPath.ToLower()))
			{
				return false;
			}
			return true;
		}
	}
}
