using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Network;
using VRage.Security;
using VRage.Utils;

namespace VRage.Render11.Resources
{
	[ProtoContract]
	public class MyTextureCache
	{
		private class VRage_Render11_Resources_MyTextureCache_003C_003EActor : IActivator, IActivator<MyTextureCache>
		{
			private sealed override object CreateInstance()
			{
				return new MyTextureCache();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTextureCache CreateInstance()
			{
				return new MyTextureCache();
			}

			MyTextureCache IActivator<MyTextureCache>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly string m_mipMapTextureCacheFile = "mipMapTextureCache2.B5";

		private static readonly string m_contentPath = "Textures";

		private MyConcurrentHashSet<uint> m_refreshedTextures = new MyConcurrentHashSet<uint>();

		private ConcurrentDictionary<string, uint> m_textureKeyCache = new ConcurrentDictionary<string, uint>();

		[ProtoMember(1)]
		public int Version { get; set; }

		[ProtoMember(2)]
		public ConcurrentDictionary<uint, MyTextureCacheItem> LowMipMapCache { get; set; }

		private static string UserDataPath => Path.Combine(MyFileSystem.UserDataPath, m_mipMapTextureCacheFile);

		private static string ContentPath => Path.Combine(MyFileSystem.ContentPath, m_contentPath, m_mipMapTextureCacheFile);

		public MyTextureCache()
		{
			Version = 2;
			LowMipMapCache = new ConcurrentDictionary<uint, MyTextureCacheItem>();
		}

		public void Update(string textureName, MyTextureCacheItem cacheItem)
		{
			uint textureKey = GetTextureKey(textureName);
			m_refreshedTextures.Add(textureKey);
<<<<<<< HEAD
			LowMipMapCache[textureKey] = cacheItem;
=======
			LowMipMapCache.set_Item(textureKey, cacheItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool TryGet(string textureName, out MyTextureCacheItem cacheItem)
		{
			uint textureKey = GetTextureKey(textureName);
<<<<<<< HEAD
			return LowMipMapCache.TryGetValue(textureKey, out cacheItem);
=======
			return LowMipMapCache.TryGetValue(textureKey, ref cacheItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool NeedsCacheUpdate(string textureName)
		{
			uint textureKey = GetTextureKey(textureName);
			return !m_refreshedTextures.Contains(textureKey);
		}

		private uint GetTextureKey(string textureName)
		{
<<<<<<< HEAD
			if (m_textureKeyCache.TryGetValue(textureName, out var value))
			{
				return value;
			}
			value = FnvHash.Compute(textureName.ToLower());
			m_textureKeyCache.TryAdd(textureName, value);
			return value;
=======
			uint result = default(uint);
			if (m_textureKeyCache.TryGetValue(textureName, ref result))
			{
				return result;
			}
			result = FnvHash.Compute(textureName.ToLower());
			m_textureKeyCache.TryAdd(textureName, result);
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void Save(bool toContent = false)
		{
<<<<<<< HEAD
			using (Stream destination = MyFileSystem.OpenWrite(toContent ? ContentPath : UserDataPath))
			{
				Serializer.Serialize(destination, this);
			}
=======
			using Stream destination = MyFileSystem.OpenWrite(toContent ? ContentPath : UserDataPath);
			Serializer.Serialize(destination, this);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static MyTextureCache Load()
		{
			MyTextureCache myTextureCache = null;
			try
			{
				myTextureCache = Load(UserDataPath);
			}
			catch (Exception)
			{
				MyLog.Default.Warning("Low MipMap texture cache load failed.");
			}
			try
			{
				MyTextureCache myTextureCache2 = Load(ContentPath);
				if (myTextureCache == null)
				{
					myTextureCache = myTextureCache2;
				}
				else if (myTextureCache2 != null)
				{
					foreach (KeyValuePair<uint, MyTextureCacheItem> item in myTextureCache2.LowMipMapCache)
					{
<<<<<<< HEAD
						myTextureCache.LowMipMapCache[item.Key] = item.Value;
=======
						myTextureCache.LowMipMapCache.set_Item(item.Key, item.Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			catch (Exception)
			{
				MyLog.Default.Warning("Default Low MipMap texture cache load failed.");
			}
			if (myTextureCache == null)
			{
				myTextureCache = new MyTextureCache();
			}
			return myTextureCache;
		}

		private static MyTextureCache Load(string mipMapTextureCacheFile)
		{
			MyTextureCache result = null;
			if (MyFileSystem.FileExists(mipMapTextureCacheFile))
			{
				using (Stream source = MyFileSystem.OpenRead(mipMapTextureCacheFile))
				{
					return Serializer.Deserialize<MyTextureCache>(source);
				}
			}
			return result;
		}
	}
}
