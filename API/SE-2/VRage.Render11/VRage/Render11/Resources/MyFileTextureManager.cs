using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ParallelTasks;
using VRage.Collections;
using VRage.Generics;
using VRage.Library.Memory;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Render11.Profiler;
using VRage.Render11.Render;
using VRage.Render11.Resources.Internal;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Resources
{
	internal class MyFileTextureManager : IManager, IManagerDevice, IManagerFrameEnd, IManagerUnloadData
	{
		internal static class MyFileTextureHelper
		{
			internal static bool IsAssetTextureFilter(IFileTexture texture)
			{
				return ((MyFileTexture)texture).TextureType != MyFileTextureEnum.SYSTEM;
			}

			internal static bool UsePrioritizedLoad(MyFileTextureEnum textureType)
			{
				if (textureType == MyFileTextureEnum.GUI || textureType == MyFileTextureEnum.CUSTOM)
				{
					return true;
				}
				return false;
			}

			internal static bool ShouldUseMipCache(MyFileTextureEnum textureType)
			{
				return IsQualityDependentFilter(textureType);
			}

			internal static bool IsQualityDependentFilter(IFileTexture texture)
			{
				return IsQualityDependentFilter(((MyFileTexture)texture).TextureType);
			}

			internal static bool IsQualityDependentFilter(MyFileTextureEnum type)
			{
<<<<<<< HEAD
				switch (type)
				{
				case MyFileTextureEnum.SYSTEM:
					return false;
				case MyFileTextureEnum.GUI:
					return false;
				default:
					return true;
				}
=======
				return type switch
				{
					MyFileTextureEnum.SYSTEM => false, 
					MyFileTextureEnum.GUI => false, 
					_ => true, 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static readonly bool DEBUG_TEXTURE_STREAMING = false;

		public static readonly bool ENABLE_LOW_MIPMAP_CACHE = true;

		public static readonly bool ENABLE_TEXTURE_ASYNC_LOADING = true;

		public static readonly bool ENABLE_MIPMAP_GENERATING = false;

		private const string FILE_SCHEME = "file";

		private static MyFileTextureUsageReport m_report;

		private Dictionary<string, IGeneratedTexture> m_generatedTextures = new Dictionary<string, IGeneratedTexture>();

		private int m_userGeneratedTextureCount;

		private MyTextureCache m_lowMipMapTextureCache;

		private readonly ConcurrentDictionary<string, MyFileTexture> m_textures = new ConcurrentDictionary<string, MyFileTexture>();

		private readonly MyObjectsPool<MyFileTexture> m_texturesPool = new MyObjectsPool<MyFileTexture>(1024);

		private readonly MyFileTextureImageCache m_imageCache = new MyFileTextureImageCache();

		private readonly MyConcurrentQueue<Action> m_endOfFrameUpdates = new MyConcurrentQueue<Action>();

		public MyMemorySystem MemoryTracker { get; } = MyManagers.TexturesMemoryTracker.RegisterSubsystem("FileTextures");


		public int GeneratedTexturesCount => m_generatedTextures.Count;

		private void RegisterDefaultTextures()
		{
			m_generatedTextures = new Dictionary<string, IGeneratedTexture>();
			m_generatedTextures[MyGeneratedTextureManager.ZeroTex.Name] = MyGeneratedTextureManager.ZeroTex;
			m_generatedTextures[MyGeneratedTextureManager.MissingNormalGlossTex.Name] = MyGeneratedTextureManager.MissingNormalGlossTex;
			m_generatedTextures[MyGeneratedTextureManager.MissingExtensionTex.Name] = MyGeneratedTextureManager.MissingExtensionTex;
			m_generatedTextures[MyGeneratedTextureManager.PinkTex.Name] = MyGeneratedTextureManager.PinkTex;
			m_generatedTextures[MyGeneratedTextureManager.MissingCubeTex.Name] = MyGeneratedTextureManager.MissingCubeTex;
			m_generatedTextures[MyGeneratedTextureManager.IntelFallbackCubeTex.Name] = MyGeneratedTextureManager.IntelFallbackCubeTex;
			m_generatedTextures[MyGeneratedTextureManager.Dithering8x8Tex.Name] = MyGeneratedTextureManager.Dithering8x8Tex;
			m_generatedTextures[MyGeneratedTextureManager.MissingAlphamaskTex.Name] = MyGeneratedTextureManager.MissingAlphamaskTex;
			m_generatedTextures[MyGeneratedTextureManager.RandomTex.Name] = MyGeneratedTextureManager.RandomTex;
		}

		/// <remarks>On big loops, or whenever recommendable, cache the returned reference</remarks>
		public ITexture GetTexture(string name, MyFileTextureEnum type, bool isVoxel, bool waitTillLoaded = false, bool skipQualityReduction = false, bool temporary = false)
		{
			try
			{
				if (string.IsNullOrEmpty(name))
				{
					return ReturnDefaultTexture(type, errorTexture: false);
				}
				if (!MyResourceUtils.NormalizeFileTextureName(ref name, out var uri))
				{
					if (m_generatedTextures.TryGetValue(name, out var value))
					{
						return value;
					}
					return ReturnDefaultTexture(type, errorTexture: true);
				}
<<<<<<< HEAD
				if (!m_textures.TryGetValue(name, out var value2))
=======
				MyFileTexture item = default(MyFileTexture);
				if (!m_textures.TryGetValue(name, ref item))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (uri.Scheme != "file")
					{
						return ReturnDefaultTexture(type, errorTexture: true);
					}
<<<<<<< HEAD
					m_texturesPool.AllocateOrCreate(out value2);
					value2.Init(name, uri.LocalPath, type, isVoxel, waitTillLoaded, skipQualityReduction);
					SetUnloadedContent(value2, firstLoad: true);
					if (!m_textures.TryAdd(name, value2))
					{
						m_texturesPool.Deallocate(value2);
						value2 = m_textures[name];
					}
				}
				if (waitTillLoaded && value2.CurrentTextureState != 3)
				{
					ChangeTextureState(value2, FileTextureState.Loaded, forceSyncLoad: true);
					while (value2.CurrentTextureState != 3)
=======
					m_texturesPool.AllocateOrCreate(out item);
					item.Init(name, uri.LocalPath, type, isVoxel, waitTillLoaded, skipQualityReduction);
					SetUnloadedContent(item, firstLoad: true);
					if (!m_textures.TryAdd(name, item))
					{
						m_texturesPool.Deallocate(item);
						item = m_textures.get_Item(name);
					}
				}
				if (waitTillLoaded && item.CurrentTextureState != 3)
				{
					ChangeTextureState(item, FileTextureState.Loaded, forceSyncLoad: true);
					while (item.CurrentTextureState != 3)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						Thread.Yield();
					}
				}
<<<<<<< HEAD
				return value2;
=======
				return item;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyRender11.Log.WriteLine("Error loading texture " + name);
				MyRender11.Log.WriteLine(ex);
				return ReturnDefaultTexture(type, errorTexture: true);
			}
		}

		public void ChangeTextureState(MyFileTexture texture, FileTextureState targetState, bool forceSyncLoad = false, WorkPriority? priority = null)
		{
			if (Interlocked.Exchange(ref texture.TargetTextureState, (int)targetState) != (int)targetState || (forceSyncLoad && texture.CurrentTextureState != (int)targetState))
			{
				ScheduleUpdate();
			}
			void PerformLoad()
			{
				if (!texture.LoadFailed)
				{
					if (DEBUG_TEXTURE_STREAMING && ReturnDefaultTexture(texture.TextureType, errorTexture: true) == MyGeneratedTextureManager.PinkTex)
					{
						texture.ShareResourceFrom(MyGeneratedTextureManager.GreenTex);
					}
					texture.Load(m_imageCache, m_lowMipMapTextureCache);
					LogTextureLoad(texture);
				}
			}
			void PerformUpdate()
			{
				if (Interlocked.CompareExchange(ref texture.CurrentTextureState, 2, 1) == 1)
				{
					FileTextureState targetTextureState = (FileTextureState)texture.TargetTextureState;
					if (texture.CurrentTextureState != (int)targetTextureState)
					{
						switch (targetTextureState)
						{
						case FileTextureState.Loaded:
							PerformLoad();
							break;
						case FileTextureState.Unloaded:
							SetUnloadedContent(texture);
							break;
						}
					}
					Interlocked.Exchange(ref texture.CurrentTextureState, (int)targetTextureState);
					if (Volatile.Read(ref texture.TargetTextureState) != (int)targetTextureState)
					{
						forceSyncLoad = false;
						ScheduleUpdate();
					}
				}
			}
			void ScheduleUpdate()
			{
				FileTextureState fileTextureState = (FileTextureState)Volatile.Read(ref texture.CurrentTextureState);
				FileTextureState fileTextureState2;
				do
				{
					switch (fileTextureState)
					{
					case FileTextureState.TransitionScheduled:
						if (!forceSyncLoad)
						{
							return;
						}
						break;
					case FileTextureState.InTransition:
						return;
					}
					fileTextureState2 = fileTextureState;
					fileTextureState = (FileTextureState)Interlocked.CompareExchange(ref texture.CurrentTextureState, 1, (int)fileTextureState2);
				}
				while (fileTextureState != fileTextureState2);
				if (DEBUG_TEXTURE_STREAMING && !forceSyncLoad && ReturnDefaultTexture(texture.TextureType, errorTexture: true) == MyGeneratedTextureManager.PinkTex)
				{
					texture.ShareResourceFrom(MyGeneratedTextureManager.RedTex);
				}
				if (texture.TargetTextureState == 3 && ENABLE_TEXTURE_ASYNC_LOADING && !forceSyncLoad)
				{
					if (!priority.HasValue)
					{
						priority = (MyFileTextureHelper.UsePrioritizedLoad(texture.TextureType) ? WorkPriority.Normal : WorkPriority.VeryLow);
					}
					Parallel.Start(priority.Value, PerformUpdate, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.AssetLoad, "FileTextureManager"));
				}
				else
				{
					PerformUpdate();
				}
			}
		}

		public void CreateLowMipMapCache(string texturePath, MyFileTextureEnum type)
		{
			string name = texturePath;
			if (!MyResourceUtils.NormalizeFileTextureName(ref name, out var _))
			{
				return;
			}
<<<<<<< HEAD
			using (MyFileTextureImageCache.CacheToken cacheToken = LoadImage(texturePath))
			{
				if (cacheToken?.Image != null)
				{
					MyTextureCacheItem cacheItem = MyFileTexture.CreateTextureCacheItem(type, cacheToken.Image);
					m_lowMipMapTextureCache.Update(name, cacheItem);
				}
=======
			using MyFileTextureImageCache.CacheToken cacheToken = LoadImage(texturePath);
			if (cacheToken?.Image != null)
			{
				MyTextureCacheItem cacheItem = MyFileTexture.CreateTextureCacheItem(type, cacheToken.Image);
				m_lowMipMapTextureCache.Update(name, cacheItem);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool TryGetTexture(string name, out ITexture texture)
		{
			if (!MyResourceUtils.NormalizeFileTextureName(ref name, out var _))
			{
				IGeneratedTexture value;
				bool result = m_generatedTextures.TryGetValue(name, out value);
				texture = value;
				return result;
			}
<<<<<<< HEAD
			MyFileTexture value2;
			bool result2 = m_textures.TryGetValue(name, out value2);
			texture = value2;
=======
			MyFileTexture myFileTexture = default(MyFileTexture);
			bool result2 = m_textures.TryGetValue(name, ref myFileTexture);
			texture = myFileTexture;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return result2;
		}

		public bool TryGetTexture(string name, out IUserGeneratedTexture texture)
		{
			if (MyResourceUtils.NormalizeFileTextureName(ref name, out var _))
			{
				texture = null;
				return false;
			}
			m_generatedTextures.TryGetValue(name, out var value);
			texture = value as IUserGeneratedTexture;
			return texture != null;
		}

		public bool TryGetTextureParams(string name, out MyFileTextureParams textureParams)
		{
			if (m_lowMipMapTextureCache.TryGet(name, out var cacheItem) && !cacheItem.FullTextureParams.IsEmpty)
			{
				textureParams = cacheItem.FullTextureParams;
				return true;
			}
<<<<<<< HEAD
			if (MyResourceUtils.NormalizeFileTextureName(ref name, out var _) && m_textures.TryGetValue(name, out var value))
			{
				MyFileTextureParams? textureParams2 = value.TextureParams;
=======
			MyFileTexture myFileTexture = default(MyFileTexture);
			if (MyResourceUtils.NormalizeFileTextureName(ref name, out var _) && m_textures.TryGetValue(name, ref myFileTexture))
			{
				MyFileTextureParams? textureParams2 = myFileTexture.TextureParams;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (textureParams2.HasValue)
				{
					textureParams = textureParams2.Value;
					return true;
				}
			}
			textureParams = default(MyFileTextureParams);
			return false;
		}

		public IUserGeneratedTexture CreateGeneratedTexture(string name, int width, int height, MyGeneratedTextureType type, bool generateMipmaps, byte[] data = null, bool immediatelyReady = false)
		{
			if (m_generatedTextures.TryGetValue(name, out var value))
			{
				IUserGeneratedTexture userGeneratedTexture = value as IUserGeneratedTexture;
				if (userGeneratedTexture == null)
				{
					return null;
				}
				if (userGeneratedTexture.Size.X == width && userGeneratedTexture.Size.Y == height)
				{
					_ = userGeneratedTexture.Type;
				}
				return userGeneratedTexture;
			}
			IUserGeneratedTexture userGeneratedTexture2 = MyManagers.GeneratedTextures.NewUserTexture(name, width, height, type, generateMipmaps, immediatelyReady, data);
			m_generatedTextures[name] = userGeneratedTexture2;
			m_userGeneratedTextureCount++;
			MyManagers.Textures.InvalidateTextureHandle(name, removed: false);
			MyMeshMaterials1.InvalidateMaterials(name);
			return userGeneratedTexture2;
		}

		public void DestroyGeneratedTexture(string name)
		{
			IUserGeneratedTexture userGeneratedTexture;
			if (m_generatedTextures.TryGetValue(name, out var value) && (userGeneratedTexture = value as IUserGeneratedTexture) != null)
			{
				if (MyOffscreenRenderer.OnTextureDisposed(userGeneratedTexture))
				{
					MyManagers.GeneratedTextures.DisposeTex(userGeneratedTexture);
				}
				m_generatedTextures.Remove(name);
				m_userGeneratedTextureCount--;
				MyManagers.Textures.InvalidateTextureHandle(name, removed: true);
			}
		}

		internal void DestroyGeneratedTexture(IUserGeneratedTexture userTexture)
		{
			ScheduleTextureUpdate(delegate
			{
				MyManagers.GeneratedTextures.DisposeTex(userTexture);
			});
		}

		public void ResetGeneratedTexture(string name, byte[] data)
		{
			IUserGeneratedTexture userGeneratedTexture;
			if (m_generatedTextures.TryGetValue(name, out var value) && (userGeneratedTexture = value as IUserGeneratedTexture) != null)
			{
				userGeneratedTexture.Reset(data);
			}
		}

		public bool IsTextureReadyForMaterialSwap(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return true;
			}
			string name = filePath;
			MyResourceUtils.NormalizeFileTextureName(ref name);
			if (m_generatedTextures.TryGetValue(name, out var value))
			{
				return value.IsTextureLoaded();
			}
<<<<<<< HEAD
			if (!m_textures.TryGetValue(name, out var value2))
			{
				return true;
			}
			if (value2.CurrentTextureState == 3)
			{
				return true;
			}
			FileTextureState targetTextureState = (FileTextureState)value2.TargetTextureState;
=======
			MyFileTexture myFileTexture = default(MyFileTexture);
			if (!m_textures.TryGetValue(name, ref myFileTexture))
			{
				return true;
			}
			if (myFileTexture.CurrentTextureState == 3)
			{
				return true;
			}
			FileTextureState targetTextureState = (FileTextureState)myFileTexture.TargetTextureState;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (targetTextureState == FileTextureState.Loaded)
			{
				return false;
			}
			return true;
		}

		private static ITexture ReturnDefaultTexture(MyFileTextureEnum type, bool errorTexture)
		{
			switch (type)
			{
			case MyFileTextureEnum.NORMALMAP_GLOSS:
				return MyGeneratedTextureManager.MissingNormalGlossTex;
			case MyFileTextureEnum.EXTENSIONS:
				return MyGeneratedTextureManager.MissingExtensionTex;
			case MyFileTextureEnum.ALPHAMASK:
				return MyGeneratedTextureManager.MissingAlphamaskTex;
			default:
				if (errorTexture)
				{
					if (type == MyFileTextureEnum.CUBEMAP)
					{
						return MyGeneratedTextureManager.MissingCubeTex;
					}
					if (!DEBUG_TEXTURE_STREAMING)
					{
						return MyGeneratedTextureManager.MissingColorMetal;
					}
					return MyGeneratedTextureManager.PinkTex;
				}
				return MyGeneratedTextureManager.ZeroTex;
			}
		}

		private void SetUnloadedContent(MyFileTexture tex, bool firstLoad = false)
		{
			if (!firstLoad)
			{
				LogTextureUnload(tex);
			}
			bool flag = false;
			if (ENABLE_LOW_MIPMAP_CACHE && !DEBUG_TEXTURE_STREAMING && MyFileTextureHelper.ShouldUseMipCache(tex.TextureType) && m_lowMipMapTextureCache.TryGet(tex.Name, out var cacheItem))
			{
				flag = tex.Load(cacheItem);
			}
			if (!flag)
			{
				ITexture texture = ReturnDefaultTexture(tex.TextureType, DEBUG_TEXTURE_STREAMING);
				tex.ShareResourceFrom(texture);
			}
		}

		public MyGeneratedTexture CreateCacheTexture(string textureName)
		{
			int totalSize;
			if (m_lowMipMapTextureCache.TryGet(textureName, out var cacheItem) && !cacheItem.TextureParams.IsEmpty)
			{
				return MyFileTexture.CreateCacheTexture("low://" + textureName, cacheItem, out totalSize);
			}
			return null;
		}

		public MyTextureCacheItem GetLowMipMapCacheItem(string textureName)
		{
			if (MyResourceUtils.NormalizeFileTextureName(ref textureName) && m_lowMipMapTextureCache.TryGet(textureName, out var cacheItem) && !cacheItem.TextureParams.IsEmpty)
			{
				return cacheItem;
			}
			return null;
		}

		internal MyFileTextureImageCache.CacheToken LoadImage(string filepath, bool headerOnly = false)
		{
			return m_imageCache.GetImage(filepath, headerOnly);
		}

		public void DisposeAll()
		{
			foreach (KeyValuePair<string, MyFileTexture> texture in m_textures)
			{
				LinqExtensions.Deconstruct(texture, out var _, out var v);
				v.Destroy();
			}
			if (ENABLE_LOW_MIPMAP_CACHE)
			{
				m_lowMipMapTextureCache.Save();
			}
		}

		public void ReloadTextures(Func<IFileTexture, bool> filter)
		{
			foreach (var (_, myFileTexture2) in m_textures)
			{
				if ((filter?.Invoke(myFileTexture2) ?? true) && myFileTexture2.TargetTextureState == 3)
				{
					ChangeTextureState(myFileTexture2, FileTextureState.Unloaded);
					ChangeTextureState(myFileTexture2, FileTextureState.Loaded);
				}
			}
		}

		private void LogTextureUnload(MyFileTexture texture)
		{
			Interlocked.Decrement(ref m_report.TexturesLoaded);
			for (int num = 1; num < 512; num <<= 1)
			{
				if (((uint)texture.TextureType & (uint)num) != 0)
				{
					MyPerTextureTypeUsageReport myPerTextureTypeUsageReport = m_report.TexturesLoadedByTypeData[texture.TextureType];
					if (MyResourceUtils.IsCompressed(texture.Format))
					{
						Interlocked.Decrement(ref myPerTextureTypeUsageReport.CompressedCount);
						Interlocked.Add(ref myPerTextureTypeUsageReport.CompressedMemory, -texture.ByteSize);
					}
					else
					{
						Interlocked.Decrement(ref myPerTextureTypeUsageReport.NoncompressedCount);
						Interlocked.Add(ref myPerTextureTypeUsageReport.NoncompressedMemory, -texture.ByteSize);
					}
				}
			}
		}

		private void LogTextureLoad(MyFileTexture texture)
		{
			Interlocked.Increment(ref m_report.TexturesLoaded);
			for (int num = 1; num < 512; num <<= 1)
			{
				if (((uint)texture.TextureType & (uint)num) != 0)
				{
					MyPerTextureTypeUsageReport myPerTextureTypeUsageReport = m_report.TexturesLoadedByTypeData[texture.TextureType];
					if (MyResourceUtils.IsCompressed(texture.Format))
					{
						Interlocked.Increment(ref myPerTextureTypeUsageReport.CompressedCount);
						Interlocked.Add(ref myPerTextureTypeUsageReport.CompressedMemory, texture.ByteSize);
					}
					else
					{
						Interlocked.Increment(ref myPerTextureTypeUsageReport.NoncompressedCount);
						Interlocked.Add(ref myPerTextureTypeUsageReport.NoncompressedMemory, texture.ByteSize);
					}
				}
			}
		}

		public MyFileTextureUsageReport GetReport()
		{
<<<<<<< HEAD
			m_report.TexturesTotal = m_textures.Count;
=======
			m_report.TexturesTotal = m_textures.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_report.TotalTextureMemory = GetTotalByteSizeOfResources();
			m_report.TexturesTotalPeak = Math.Max(m_report.TexturesTotalPeak, m_report.TexturesTotal);
			m_report.TexturesLoadedPeak = Math.Max(m_report.TexturesLoadedPeak, m_report.TexturesLoaded);
			m_report.TotalTextureMemoryPeak = Math.Max(m_report.TotalTextureMemoryPeak, m_report.TotalTextureMemory);
			return m_report;
			long GetTotalByteSizeOfResources()
			{
				long num = 0L;
				foreach (KeyValuePair<string, MyFileTexture> texture in m_textures)
				{
					num += texture.Value.ByteSize;
				}
				return num;
			}
		}

		public StringBuilder GetFileTexturesDesc()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Loaded file textures:");
			stringBuilder.AppendLine("[TextureType;ByteSize;Width;Height;Format;Filename]");
			foreach (KeyValuePair<string, MyFileTexture> texture in m_textures)
			{
				if (texture.Value.IsLoaded)
				{
					stringBuilder.AppendFormat("{0};{1};{2};{3};{4};{5}", texture.Value.TextureType, texture.Value.ByteSize, texture.Value.Size.X, texture.Value.Size.Y, texture.Value.Format, texture.Key);
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder;
		}

		public void GenerateMipCacheOffline(IEnumerable<(string Path, MyFileTextureEnum Type)> textures)
		{
			if (ENABLE_LOW_MIPMAP_CACHE)
			{
				MyTextureCache lowMipMapTextureCache = m_lowMipMapTextureCache;
				m_lowMipMapTextureCache = new MyTextureCache();
				Parallel.ForEach(textures, delegate((string Path, MyFileTextureEnum Type) x)
				{
					CreateLowMipMapCache(x.Path, x.Type);
				});
				m_lowMipMapTextureCache.Save(toContent: true);
				m_lowMipMapTextureCache = lowMipMapTextureCache;
			}
		}

		public void OnDeviceInit()
		{
			m_report.TexturesLoadedByTypeData = new Dictionary<MyFileTextureEnum, MyPerTextureTypeUsageReport>();
			for (int num = 1; num < 512; num <<= 1)
			{
				m_report.TexturesLoadedByTypeData[(MyFileTextureEnum)num] = new MyPerTextureTypeUsageReport();
			}
			RegisterDefaultTextures();
			if (ENABLE_LOW_MIPMAP_CACHE)
			{
				m_lowMipMapTextureCache = MyTextureCache.Load();
			}
			else
			{
				m_lowMipMapTextureCache = new MyTextureCache();
			}
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
			DisposeAll();
			RemoveUserGeneratedTextures();
		}

		private void RemoveUserGeneratedTextures()
		{
			List<IUserGeneratedTexture> list = new List<IUserGeneratedTexture>();
			foreach (IGeneratedTexture value in m_generatedTextures.Values)
			{
				IUserGeneratedTexture item;
				if ((item = value as IUserGeneratedTexture) != null)
				{
					list.Add(item);
				}
			}
			foreach (IUserGeneratedTexture item2 in list)
			{
				MyManagers.GeneratedTextures.DisposeTex(item2);
				m_generatedTextures.Remove(item2.Name);
				m_userGeneratedTextureCount--;
			}
			m_generatedTextures.Clear();
		}

		internal void ScheduleTextureUpdate(Action update)
		{
			m_endOfFrameUpdates.Enqueue(update);
		}

		void IManagerFrameEnd.OnFrameEnd()
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("Generate MipMaps", "OnFrameEnd", "E:\\Repo1\\Sources\\VRage.Render11\\Resources\\MyFileTextureManager.cs");
=======
			MyGpuProfiler.IC_BeginBlock("Generate MipMaps", "OnFrameEnd", "E:\\Repo3\\Sources\\VRage.Render11\\Resources\\MyFileTextureManager.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Action instance;
			while (m_endOfFrameUpdates.TryDequeue(out instance))
			{
				instance.InvokeIfNotNull();
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "OnFrameEnd", "E:\\Repo1\\Sources\\VRage.Render11\\Resources\\MyFileTextureManager.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "OnFrameEnd", "E:\\Repo3\\Sources\\VRage.Render11\\Resources\\MyFileTextureManager.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void OnUnloadData()
		{
		}

		public void SetCheckerTexture(bool checkerTextureEnabled)
		{
			foreach (KeyValuePair<string, MyFileTexture> texture in m_textures)
			{
				if (texture.Value.TextureType == MyFileTextureEnum.COLOR_METAL)
				{
					texture.Value.SetCheckerTexture(checkerTextureEnabled);
				}
			}
		}
	}
}
