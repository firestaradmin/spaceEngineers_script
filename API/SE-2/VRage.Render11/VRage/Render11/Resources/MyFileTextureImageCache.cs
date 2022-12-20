using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.Toolkit.Graphics;
using VRage.FileSystem;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyFileTextureImageCache
	{
		public abstract class CacheToken : IDisposable
		{
			protected string m_key;

			protected int m_pinCount;

			protected MyFileTextureImageCache m_owner;

			public Image Image { get; private set; }

			protected CacheToken(MyFileTextureImageCache owner, Image image, string key)
			{
				m_key = key;
				m_pinCount = 0;
				m_owner = owner;
				Image = image;
			}

			public void Dispose()
			{
				if (m_owner == null)
				{
					return;
				}
				lock (m_owner.m_imageCache)
				{
					m_pinCount--;
					if (m_pinCount == 0)
					{
						Image.Dispose();
						m_owner.m_imageCache.Remove(m_key);
					}
				}
			}
		}

		private class CacheTokenImpl : CacheToken
		{
			public CacheTokenImpl(MyFileTextureImageCache owner, Image image, string key)
				: base(owner, image, key)
			{
			}

			public void AddReference()
			{
				m_pinCount++;
			}
		}

		private readonly Dictionary<string, CacheTokenImpl> m_imageCache = new Dictionary<string, CacheTokenImpl>();

		public CacheToken TryGetImage(string path)
		{
			lock (m_imageCache)
			{
				if (m_imageCache.TryGetValue(path, out var value))
				{
					value.AddReference();
					return value;
				}
			}
			return null;
		}

		public CacheToken GetImage(string path, bool headerOnly = false)
		{
			lock (m_imageCache)
			{
				if (m_imageCache.TryGetValue(path, out var value))
				{
					value.AddReference();
					return value;
				}
			}
			Image image = LoadImage(path, headerOnly);
			if (image == null)
			{
				return null;
			}
			if (headerOnly)
			{
				return new CacheTokenImpl(null, image, null);
			}
			lock (m_imageCache)
			{
				if (m_imageCache.TryGetValue(path, out var value2))
				{
					image.Dispose();
				}
				else
				{
					value2 = new CacheTokenImpl(this, image, path);
					m_imageCache.Add(path, value2);
				}
				value2.AddReference();
				return value2;
			}
		}

		private Image LoadImage(string filepath, bool headerOnly)
		{
			if (!MyFileSystem.FileExists(filepath))
			{
				MyRender11.Log.WriteLine("Texture does not exist: " + filepath);
				return null;
			}
			try
			{
<<<<<<< HEAD
				using (Stream imageStream = MyFileSystem.OpenRead(filepath))
				{
					if (headerOnly)
					{
						ImageDescription? imageDescription = Image.LoadImageDescription(imageStream, filepath);
						if (imageDescription.HasValue)
						{
							return new Image(imageDescription.Value);
						}
						return null;
					}
					return Image.Load(imageStream, filepath);
				}
=======
				using Stream imageStream = MyFileSystem.OpenRead(filepath);
				if (headerOnly)
				{
					ImageDescription? imageDescription = Image.LoadImageDescription(imageStream, filepath);
					if (imageDescription.HasValue)
					{
						return new Image(imageDescription.Value);
					}
					return null;
				}
				return Image.Load(imageStream, filepath);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyRender11.Log.WriteLine("Error while loading texture: " + filepath + ", exception: " + ex);
			}
			return null;
		}
	}
}
