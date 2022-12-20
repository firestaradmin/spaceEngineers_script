<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Concurrent;
using SharpDX.Toolkit.Graphics;
using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Resources
{
	internal static class MyFileTextureParamsManager
	{
		private static ConcurrentDictionary<string, MyFileTextureParams> m_dictCached = new ConcurrentDictionary<string, MyFileTextureParams>();

		public static bool LoadFromFile(string name, out MyFileTextureParams outParams, bool onlyIfCheap = false)
		{
			outParams = default(MyFileTextureParams);
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			string textureFullPath = MyResourceUtils.GetTextureFullPath(name);
<<<<<<< HEAD
			if (m_dictCached.TryGetValue(textureFullPath, out outParams))
=======
			if (m_dictCached.TryGetValue(textureFullPath, ref outParams))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return true;
			}
			if (MyManagers.FileTextures.TryGetTextureParams(name, out outParams))
			{
				m_dictCached.TryAdd(textureFullPath, outParams);
				return true;
			}
			if (onlyIfCheap)
			{
				return false;
			}
<<<<<<< HEAD
			outParams = m_dictCached.GetOrAdd(textureFullPath, delegate(string imagePath)
			{
				using (MyFileTextureImageCache.CacheToken cacheToken = MyManagers.FileTextures.LoadImage(imagePath, headerOnly: true))
				{
					if (cacheToken == null)
					{
						return default(MyFileTextureParams);
					}
					ImageDescription description = cacheToken.Image.Description;
					MyFileTextureParams result = default(MyFileTextureParams);
					result.Format = description.Format;
					result.MipLevels = description.MipLevels;
					result.ArraySize = description.ArraySize;
					result.Resolution = new Vector2I(description.Width, description.Height);
					return result;
				}
=======
			outParams = m_dictCached.GetOrAdd(textureFullPath, (Func<string, MyFileTextureParams>)delegate(string imagePath)
			{
				using MyFileTextureImageCache.CacheToken cacheToken = MyManagers.FileTextures.LoadImage(imagePath, headerOnly: true);
				if (cacheToken == null)
				{
					return default(MyFileTextureParams);
				}
				ImageDescription description = cacheToken.Image.Description;
				MyFileTextureParams result = default(MyFileTextureParams);
				result.Format = description.Format;
				result.MipLevels = description.MipLevels;
				result.ArraySize = description.ArraySize;
				result.Resolution = new Vector2I(description.Width, description.Height);
				return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			});
			return !outParams.IsEmpty;
		}

		public static Vector2I GetResolutionFromFile(string filepath)
		{
			if (!LoadFromFile(filepath, out var outParams))
			{
				return Vector2I.Zero;
			}
			return outParams.Resolution;
		}

		public static bool IsArrayTextureInFile(string filepath)
		{
			if (!LoadFromFile(filepath, out var outParams))
			{
				return false;
			}
			return outParams.ArraySize > 1;
		}

		public static MyFileTextureParams LoadFromSrv(ISrvBindable srv)
		{
			MyFileTextureParams result = default(MyFileTextureParams);
			result.Resolution.X = srv.Size.X;
			result.Resolution.Y = srv.Size.Y;
			result.Format = srv.Srv.Description.Format;
			result.MipLevels = srv.Srv.Description.Texture2D.MipLevels;
			result.ArraySize = 1;
			return result;
		}
	}
}
