using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Toolkit.Graphics;
using VRage.FileSystem;
using VRage.Library.Memory;
using VRage.Library.Parallelization;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyFileTexture : IFileTexture, IAsyncTexture, ITexture, ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyFileTexture_003C_003EActor : IActivator, IActivator<MyFileTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyFileTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFileTexture CreateInstance()
			{
				return new MyFileTexture();
			}

			MyFileTexture IActivator<MyFileTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_name;

		private string m_path;

		private Vector2I m_size;

		private MyFileTextureEnum m_type;

		private bool m_ownsData;

		private bool m_skipQualityReduction;

		private Format m_format;

		private int m_byteSize;

		private MyMemorySystem.AllocationRecord? m_allocationRecord;

		private ShaderResourceView m_srv;

		public ShaderResourceView m_currentSrv;

		private SharpDX.Direct3D11.Resource m_resource;

		private Ref<MyFileTextureParams> m_textureParams;

		private AtomicFlag m_shouldGenerateMipmaps;

		internal int TargetTextureState;

		internal int CurrentTextureState;

		private bool m_isVoxel;

		public MyFileTextureParams? TextureParams => m_textureParams?.Value;

		internal Format ImageFormatInFile { get; private set; }

		public int MipLevels { get; private set; }

		public ShaderResourceView Srv => m_currentSrv;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public string Name => m_name;

		public string Path => m_path;

		public bool LoadFailed { get; private set; }

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, 1);

		public Vector2I Size => m_size;

		public MyFileTextureEnum TextureType => m_type;

		public long ByteSize => m_byteSize;

		public Format Format => m_format;

		public bool IsLoaded => CurrentTextureState == 3;

		public bool SkipQualityReduction => m_skipQualityReduction;

		public event Action<ITexture> OnFormatChanged;

		public void Init(string name, string localPath, MyFileTextureEnum type, bool isVoxel, bool waitTillLoaded, bool skipQualityReduction)
		{
			m_name = name;
			m_path = System.IO.Path.GetFullPath(localPath);
			m_type = type;
			m_isVoxel = isVoxel;
			m_skipQualityReduction = skipQualityReduction;
			TargetTextureState = 0;
			CurrentTextureState = 0;
		}

		public unsafe static MyGeneratedTexture CreateCacheTexture(string name, MyTextureCacheItem cacheItem, out int totalSize)
		{
			totalSize = 0;
			if (cacheItem.TextureParams.IsEmpty)
			{
				return null;
			}
			DataBox[] array = new DataBox[cacheItem.TextureParams.MipLevels * cacheItem.TextureParams.ArraySize];
			for (int i = 0; i < cacheItem.TextureParams.ArraySize; i++)
			{
				for (int j = 0; j < cacheItem.TextureParams.MipLevels; j++)
				{
					int index = i * cacheItem.TextureParams.MipLevels + j;
					int num = cacheItem.Data[index].Length;
					fixed (byte* ptr = cacheItem.Data[index])
					{
						array[SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(j, i, cacheItem.TextureParams.MipLevels)] = new DataBox
						{
							DataPointer = (IntPtr)ptr,
							RowPitch = cacheItem.RowStrides[index]
						};
					}
					totalSize += num;
				}
			}
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.MipLevels = cacheItem.TextureParams.MipLevels;
			texture2DDescription.Format = cacheItem.TextureParams.Format;
			texture2DDescription.Height = MathHelper.GetNearestBiggerPowerOfTwo(cacheItem.TextureParams.Resolution.Y);
			texture2DDescription.Width = MathHelper.GetNearestBiggerPowerOfTwo(cacheItem.TextureParams.Resolution.X);
			texture2DDescription.ArraySize = cacheItem.TextureParams.ArraySize;
			texture2DDescription.BindFlags = BindFlags.ShaderResource;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Usage = ResourceUsage.Immutable;
			texture2DDescription.SampleDescription = new SampleDescription
			{
				Count = 1,
				Quality = 0
			};
			texture2DDescription.OptionFlags = ((cacheItem.TextureParams.Dimension == TextureDimension.TextureCube) ? ResourceOptionFlags.TextureCube : ResourceOptionFlags.None);
			Texture2DDescription desc = texture2DDescription;
			try
			{
				MyGeneratedTexture myGeneratedTexture = new MyGeneratedTexture();
				myGeneratedTexture.Init(name, desc, cacheItem.TextureParams.Resolution, isGeneratingMipmaps: false);
				myGeneratedTexture.Reset(array);
				return myGeneratedTexture;
			}
			catch (SharpDXException)
			{
				MyRender11.Log.WriteLine($"Low MipMap cache texture failed to load: {name}");
<<<<<<< HEAD
				return null;
			}
=======
			}
			return null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public bool Load(MyTextureCacheItem cacheItem)
		{
			if (!cacheItem.FullTextureParams.IsEmpty)
			{
				m_textureParams = Ref.Create(cacheItem.FullTextureParams);
			}
			int totalSize;
			MyGeneratedTexture myGeneratedTexture = CreateCacheTexture("low://" + m_name, cacheItem, out totalSize);
			if (myGeneratedTexture == null)
			{
				return false;
			}
			SwapResources(myGeneratedTexture.Srv, myGeneratedTexture.Resource, ownsData: true, myGeneratedTexture.GetTextureByteSize());
			m_byteSize = totalSize;
			m_size = new Vector2I(cacheItem.TextureParams.Resolution.X, cacheItem.TextureParams.Resolution.Y);
			ImageFormatInFile = cacheItem.TextureParams.Format;
			MipLevels = cacheItem.TextureParams.MipLevels;
			return true;
		}

		public void ShareResourceFrom(ITexture texture)
		{
			SwapResources(texture.Srv, texture.Resource, ownsData: false, 0L);
			m_byteSize = 0;
			m_size = texture.Size;
		}

		public void Load(MyFileTextureImageCache imageCache, MyTextureCache lowMipMapTextureCache)
		{
			string name = Name;
			string path = Path;
			MyFileTextureImageCache.CacheToken cacheToken = imageCache.TryGetImage(path);
			try
			{
				bool flag = false;
				MyTextureCacheItem cacheItem = null;
				bool generateMipCache = MyFileTextureManager.ENABLE_LOW_MIPMAP_CACHE && lowMipMapTextureCache.NeedsCacheUpdate(name);
				if (cacheToken == null)
				{
					flag = TryLoadDDS(path, out cacheItem, generateMipCache, out var loadFailed);
					if (loadFailed)
					{
						throw new FileLoadException("Load failed", path);
					}
				}
				if (!flag)
				{
					if (cacheToken == null)
					{
						cacheToken = imageCache.GetImage(path);
					}
					Load(cacheToken?.Image, out cacheItem, generateMipCache, skipQualityReduction: false, 0);
				}
				if (cacheItem != null)
				{
					lowMipMapTextureCache.Update(name, cacheItem);
				}
			}
			catch (Exception)
			{
				SetLoadFailed();
				throw;
			}
			finally
			{
				cacheToken?.Dispose();
			}
		}

		private bool TryLoadDDS(string texPath, out MyTextureCacheItem cacheItem, bool generateMipCache, out bool loadFailed)
		{
			loadFailed = false;
			if (m_skipQualityReduction || !texPath.EndsWith(".dds", StringComparison.InvariantCultureIgnoreCase))
			{
				cacheItem = null;
				return false;
			}
			using (Stream stream = MyFileSystem.OpenRead(texPath))
			{
				if (stream != null)
				{
					if (DDSHelper.TryLoadDDSStreamWithMipSelection(stream, GetMipsToSkip, out var image, out var mipsSkipped, out var loadFailed2))
					{
						try
						{
							return Load(image, out cacheItem, generateMipCache, skipQualityReduction: true, mipsSkipped);
						}
						finally
						{
							image.Dispose();
						}
					}
					if (loadFailed2)
					{
						MyRender11.Log.WriteLine("Error while loading texture. Texture is most likely corrupted. - " + texPath);
						loadFailed = true;
					}
				}
			}
			cacheItem = null;
			return false;
			int GetMipsToSkip(in ImageDescription description)
			{
				return MyResourceUtils.GetMipsToSkip(m_type, m_isVoxel, new Vector2I(description.Width, description.Height), description.MipLevels);
			}
		}

		private bool Load(Image img, out MyTextureCacheItem cacheItem, bool generateMipCache, bool skipQualityReduction, int mipsSkipped)
		{
			cacheItem = null;
			bool flag = false;
			skipQualityReduction |= m_skipQualityReduction;
			if (img != null)
			{
				ImageFormatInFile = img.Description.Format;
				MyFileTextureParams correctedParamsFromImage = GetCorrectedParamsFromImage(img, mipsSkipped);
				_ = m_textureParams;
				m_textureParams = Ref.Create(correctedParamsFromImage);
				bool generateFullMipCache = generateMipCache;
				int num = 0;
				if (!skipQualityReduction)
				{
					num = MyResourceUtils.GetMipsToSkip(m_type, m_isVoxel, new Vector2I(img.Description.Width, img.Description.Height), img.Description.MipLevels);
				}
				if (!ShouldGenerateContentMips(m_type, img.Description))
				{
					generateFullMipCache = false;
				}
				bool flag2 = MyFileTextureManager.ENABLE_MIPMAP_GENERATING && img.Description.MipLevels == 1 && !MyResourceUtils.IsCompressed(ImageFormatInFile);
				int num2 = img.Description.MipLevels - num;
				if (flag2)
				{
					num2 = 0;
					int num3 = img.Description.Width;
					int num4 = img.Description.Height;
					while (num3 != 1 || num4 != 1)
					{
						num2++;
						num3 = (num3 >> 1) + num3 % 2;
						num4 = (num4 >> 1) + num4 % 2;
					}
				}
				DataBox[] array = new DataBox[num2 * img.Description.ArraySize];
				if (MyResourceUtils.IsCompressed(ImageFormatInFile) && img.Description.MipLevels <= 1)
				{
					MyRender11.Log.WriteLine("Compressed texture with MipLevels <= 1: " + Name);
				}
				if (generateMipCache)
				{
					cacheItem = CreateTextureCacheItem(img, num2, ref generateFullMipCache, mipsSkipped);
				}
				int num5 = ExtractImageData(img, cacheItem, num2, flag2, array, num, generateFullMipCache);
				int num6 = img.Description.Width >> num;
				int num7 = img.Description.Height >> num;
				bool flag3 = false;
				flag3 = m_type != MyFileTextureEnum.NORMALMAP_GLOSS && !img.Description.Format.IsSRgb();
				m_format = (flag3 ? MyResourceUtils.MakeSrgb(img.Description.Format) : img.Description.Format);
				if (m_type == MyFileTextureEnum.GUI && MyResourceUtils.IsRawRGBA(ImageFormatInFile))
				{
					PremultiplyAlpha(img);
				}
				Texture2DDescription texture2DDescription = default(Texture2DDescription);
				texture2DDescription.MipLevels = num2;
				texture2DDescription.Format = m_format;
				texture2DDescription.Height = num7;
				texture2DDescription.Width = num6;
				texture2DDescription.ArraySize = img.Description.ArraySize;
				texture2DDescription.BindFlags = BindFlags.ShaderResource | (flag2 ? BindFlags.RenderTarget : BindFlags.None);
				texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
				texture2DDescription.Usage = ((!flag2) ? ResourceUsage.Immutable : ResourceUsage.Default);
				texture2DDescription.SampleDescription = new SampleDescription
				{
					Count = 1,
					Quality = 0
				};
				texture2DDescription.OptionFlags = ((img.Description.Dimension == TextureDimension.TextureCube) ? ResourceOptionFlags.TextureCube : ResourceOptionFlags.None) | (flag2 ? ResourceOptionFlags.GenerateMipMaps : ResourceOptionFlags.None);
				Texture2DDescription description = texture2DDescription;
				try
				{
					m_size = new Vector2I(num6, num7);
					m_byteSize = num5;
					Texture2D texture2D = new Texture2D(MyRender11.DeviceInstance, description, array);
					ShaderResourceView shaderResourceView = new ShaderResourceView(MyRender11.DeviceInstance, texture2D);
					texture2D.DebugName = m_name;
					shaderResourceView.DebugName = m_name;
					SwapResources(shaderResourceView, texture2D, ownsData: true, num5);
					MipLevels = num2;
					if (flag2 && m_shouldGenerateMipmaps.Set())
					{
						MyManagers.FileTextures.ScheduleTextureUpdate(GenerateMipMaps);
					}
					flag = true;
				}
				catch (SharpDXException)
				{
				}
				this.OnFormatChanged.InvokeIfNotNull(this);
			}
			if (!flag)
			{
				MyRender11.Log.WriteLine("Could not load texture: " + m_path);
				MyRender11.Log.WriteLine("Missing or invalid texture: " + Name);
				SetLoadFailed();
			}
			return flag;
		}

		private static int ExtractImageData(Image image, MyTextureCacheItem cacheItem, int mipLevels, bool shouldGenerateMipmaps, DataBox[] mipData, int skipMipLevels, bool generateFullMipCache)
		{
			int num = 0;
			for (int i = 0; i < image.Description.ArraySize; i++)
			{
				PixelBuffer pixelBuffer = image.GetPixelBuffer(i, 0);
				for (int j = 0; j < mipLevels; j++)
				{
					DataBox dataBox;
					if (shouldGenerateMipmaps && j > 0)
					{
						int num2 = SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(j, i, mipLevels);
						dataBox = new DataBox
						{
							DataPointer = pixelBuffer.DataPointer,
							RowPitch = pixelBuffer.RowStride
						};
						mipData[num2] = dataBox;
						continue;
					}
					int num3 = j + skipMipLevels;
					pixelBuffer = image.GetPixelBuffer(i, num3);
					if (generateFullMipCache && num3 >= image.Description.MipLevels - cacheItem.TextureParams.MipLevels)
					{
						byte[] array = new byte[pixelBuffer.BufferStride];
						Marshal.Copy(pixelBuffer.DataPointer, array, 0, pixelBuffer.BufferStride);
						cacheItem.Data.Add(array);
						cacheItem.RowStrides.Add(pixelBuffer.RowStride);
					}
					if (mipData != null)
					{
						int num4 = SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(j, i, mipLevels);
						dataBox = new DataBox
						{
							DataPointer = pixelBuffer.DataPointer,
							RowPitch = pixelBuffer.RowStride
						};
						mipData[num4] = dataBox;
					}
					num += pixelBuffer.BufferStride;
				}
			}
			return num;
		}

		public static MyTextureCacheItem CreateTextureCacheItem(MyFileTextureEnum type, Image image)
		{
			bool generateFullMipCache = ShouldGenerateContentMips(type, image.Description);
			MyTextureCacheItem myTextureCacheItem = CreateTextureCacheItem(image, image.Description.MipLevels, ref generateFullMipCache, 0);
			ExtractImageData(image, myTextureCacheItem, image.Description.MipLevels, shouldGenerateMipmaps: false, null, 0, generateFullMipCache);
			return myTextureCacheItem;
		}

		private static bool ShouldGenerateContentMips(MyFileTextureEnum type, ImageDescription imageInfo)
		{
			if (MyFileTextureManager.MyFileTextureHelper.IsQualityDependentFilter(type))
			{
				return imageInfo.MipLevels > 1;
			}
			return false;
		}

		private static MyTextureCacheItem CreateTextureCacheItem(Image img, int mipLevels, ref bool generateFullMipCache, int preSkippedMipLevels)
		{
			MyTextureCacheItem myTextureCacheItem = new MyTextureCacheItem();
			generateFullMipCache &= mipLevels > 2;
			if (generateFullMipCache)
			{
				ImageDescription description = img.Description;
				int num = Math.Max((description.MipLevels + preSkippedMipLevels) / 2, 3);
				int num2 = description.Width >> description.MipLevels - num;
				int num3 = description.Height >> description.MipLevels - num;
				if (num2 < 4 || num3 < 4)
				{
					num++;
					num2 *= 2;
					num3 *= 2;
					generateFullMipCache &= num2 >= 4 && num3 >= 4;
				}
				if (generateFullMipCache)
				{
					myTextureCacheItem.TextureParams.MipLevels = num;
					myTextureCacheItem.TextureParams.Resolution.X = num2;
					myTextureCacheItem.TextureParams.Resolution.Y = num3;
					myTextureCacheItem.TextureParams.ArraySize = description.ArraySize;
					myTextureCacheItem.TextureParams.Dimension = description.Dimension;
					myTextureCacheItem.TextureParams.Format = description.Format;
					myTextureCacheItem.Data = new List<byte[]>();
					myTextureCacheItem.RowStrides = new List<int>();
				}
			}
			myTextureCacheItem.Timestamp = DateTime.Now.Ticks;
			myTextureCacheItem.FullTextureParams = GetCorrectedParamsFromImage(img, preSkippedMipLevels);
			return myTextureCacheItem;
		}

		private static void PremultiplyAlpha(Image img)
		{
			for (int i = 0; i < img.PixelBuffer.Count; i++)
			{
				PixelBuffer pixelBuffer = img.PixelBuffer[i];
				byte[] pixels = pixelBuffer.GetPixels<byte>();
				for (int j = 0; j < pixels.Length; j += 4)
				{
					double num = (double)(int)pixels[j + 3] / 255.0;
					Vector3D vector3D = new Vector3D((double)(int)pixels[j] / 255.0, (double)(int)pixels[j + 1] / 255.0, (double)(int)pixels[j + 2] / 255.0);
					vector3D.X = Math.Pow(vector3D.X, 2.2);
					vector3D.Y = Math.Pow(vector3D.Y, 2.2);
					vector3D.Z = Math.Pow(vector3D.Z, 2.2);
					vector3D *= num;
					vector3D.X = Math.Pow(vector3D.X, 0.45454545454545453);
					vector3D.Y = Math.Pow(vector3D.Y, 0.45454545454545453);
					vector3D.Z = Math.Pow(vector3D.Z, 0.45454545454545453);
					pixels[j] = (byte)(vector3D.X * 255.0);
					pixels[j + 1] = (byte)(vector3D.Y * 255.0);
					pixels[j + 2] = (byte)(vector3D.Z * 255.0);
				}
				pixelBuffer.SetPixels(pixels);
			}
		}

		private void SwapResources(ShaderResourceView srv, SharpDX.Direct3D11.Resource resource, bool ownsData, long size, bool disposeImmediately = false)
		{
			ShaderResourceView oldSrv;
			SharpDX.Direct3D11.Resource oldResource;
			MyMemorySystem.AllocationRecord allocationRecord;
			if (m_ownsData)
			{
				oldSrv = m_srv;
				oldResource = m_resource;
				allocationRecord = m_allocationRecord.Value;
				m_allocationRecord = null;
				if (disposeImmediately)
				{
					DisposeResources();
				}
				else
				{
					MyManagers.FileTextures.ScheduleTextureUpdate(DisposeResources);
				}
			}
			m_srv = srv;
			m_currentSrv = ((MyRender11.Settings.DrawCheckerTexture && TextureType == MyFileTextureEnum.COLOR_METAL) ? MyGeneratedTextureManager.DebugCheckerCMTex.Srv : srv);
			m_resource = resource;
			m_ownsData = ownsData;
			if (ownsData)
			{
				m_allocationRecord = MyManagers.FileTextures.MemoryTracker.RegisterAllocation(Name, size);
			}
			void DisposeResources()
			{
				oldSrv.Dispose();
				oldResource.Dispose();
				allocationRecord.Dispose();
			}
		}

		public void Destroy()
		{
			SwapResources(null, null, ownsData: false, 0L, disposeImmediately: true);
			m_format = Format.Unknown;
			m_size = Vector2I.Zero;
			m_byteSize = 0;
			m_textureParams = null;
		}

		private void GenerateMipMaps()
		{
			m_shouldGenerateMipmaps.Clear();
			MyRender11.RC.GenerateMips(this);
		}

		private static MyFileTextureParams GetCorrectedParamsFromImage(Image image, int mipsSkipped)
		{
			ImageDescription description = image.Description;
			int num = (int)Math.Pow(2.0, mipsSkipped);
			description.Width *= num;
			description.Height *= num;
			description.MipLevels += mipsSkipped;
			MyFileTextureParams result = default(MyFileTextureParams);
			result.Format = description.Format;
			result.MipLevels = description.MipLevels;
			result.ArraySize = description.ArraySize;
			result.Dimension = description.Dimension;
			result.Resolution = new Vector2I(description.Width, description.Height);
			return result;
		}

		private void SetLoadFailed()
		{
			LoadFailed = true;
		}

		public void SetCheckerTexture(bool checkerTextureEnabled)
		{
			m_currentSrv = (checkerTextureEnabled ? MyGeneratedTextureManager.DebugCheckerCMTex.Srv : m_srv);
		}
	}
}
