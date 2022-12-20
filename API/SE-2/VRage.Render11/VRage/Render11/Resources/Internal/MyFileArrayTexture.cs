using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.FileSystem;
using VRage.Network;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyFileArrayTexture : IFileArrayTexture, ITexture, ISrvBindable, IResource
	{
		private struct MyErrorRecoverySystem
		{
			public bool UseBytePattern;

			public Format FormatBytePattern;

			public byte[] BytePattern;
		}

		private class VRage_Render11_Resources_Internal_MyFileArrayTexture_003C_003EActor : IActivator, IActivator<MyFileArrayTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyFileArrayTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFileArrayTexture CreateInstance()
			{
				return new MyFileArrayTexture();
			}

			MyFileArrayTexture IActivator<MyFileArrayTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_resourceName;

		private Vector2I m_size;

		private int m_slices;

		private MyFileTextureEnum m_type;

		private readonly List<(string Path, MyStreamedTexturePin TextureHandle)> m_subresourceTextures = new List<(string, MyStreamedTexturePin)>();

		private MyErrorRecoverySystem m_recoverySystem;

		private ShaderResourceView m_srv;

		private SharpDX.Direct3D11.Resource m_resource;

		private bool m_dirtyFlag;

		private int m_minSlices;

		private Texture2DDescription m_description;

		private Format m_format;

		public bool IsLowMipMapVersion { get; set; }

		public ShaderResourceView Srv => m_srv;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public string Name => m_resourceName;

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, m_slices);

		public Vector2I Size => m_size;

		public int MipLevels { get; private set; }

		public long ByteSize { get; private set; }

		public MyFileTextureEnum Type => m_type;

		public Format Format
		{
			get
			{
				return m_format;
			}
			private set
			{
				m_format = value;
				this.OnFormatChanged.InvokeIfNotNull(this);
			}
		}

		internal List<(string Path, MyStreamedTexturePin TextureHandle)> SubResourceFileNames => m_subresourceTextures;

		public int SubTexturesCount => m_subresourceTextures.Count;

<<<<<<< HEAD
		IEnumerable<string> IFileArrayTexture.SubTextures => m_subresourceTextures.Select(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path);
=======
		IEnumerable<string> IFileArrayTexture.SubTextures => Enumerable.Select<(string, MyStreamedTexturePin), string>((IEnumerable<(string, MyStreamedTexturePin)>)m_subresourceTextures, (Func<(string, MyStreamedTexturePin), string>)(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public event Action<ITexture> OnFormatChanged;

		public bool Load(string resourceName, string[] filePaths, MyFileTextureEnum type, byte[] bytePattern, Format formatBytePattern, int minSlices, bool forceSyncLoad = false)
		{
			m_resourceName = resourceName;
			m_minSlices = minSlices;
			Format = Format.Unknown;
			m_subresourceTextures.Clear();
			int num = -1;
			for (int i = 0; i < filePaths.Length; i++)
			{
				string text = filePaths[i];
				bool flag = !string.IsNullOrEmpty(text);
				if (num == -1 && flag)
				{
					num = i;
				}
				if (MyFileTextureManager.ENABLE_LOW_MIPMAP_CACHE && type != MyFileTextureEnum.GPUPARTICLES && flag && MyManagers.FileTextures.GetLowMipMapCacheItem(text) == null)
				{
					MyManagers.FileTextures.CreateLowMipMapCache(text, type);
				}
				MyStreamedTexturePin item = default(MyStreamedTexturePin);
				if (!IsLowMipMapVersion)
				{
					item = MyManagers.Textures.GetPermanentTexture(text, new MyTextureStreamingManager.QueryArgs
					{
						TextureType = type,
						IsVoxel = true,
						WaitUntilLoaded = forceSyncLoad
					});
				}
				m_subresourceTextures.Add((text, item));
			}
			m_type = type;
			if (num == -1)
			{
				return false;
			}
			string text2 = filePaths[num];
			if (IsLowMipMapVersion)
			{
				MyTextureCacheItem lowMipMapCacheItem = MyManagers.FileTextures.GetLowMipMapCacheItem(text2);
				if (lowMipMapCacheItem == null)
				{
					throw new ArgumentNullException("Missing low mip map cache item for texture " + text2);
				}
				m_size = lowMipMapCacheItem.TextureParams.Resolution;
			}
			else
			{
				if (!MyFileTextureParamsManager.LoadFromFile(text2, out var outParams))
				{
					throw new Exception("Could not load: " + text2);
				}
				m_size = MyResourceUtils.GetTextureSizeAfterMipmapSkip(type, isVoxel: true, outParams.Resolution, outParams.MipLevels).Resolution;
			}
			m_recoverySystem.UseBytePattern = true;
			m_recoverySystem.FormatBytePattern = formatBytePattern;
			m_recoverySystem.BytePattern = bytePattern;
			m_dirtyFlag = true;
			return true;
		}

		private bool GetCorrectedFileTextureParams(out MyFileTextureParams parameters)
		{
			parameters = default(MyFileTextureParams);
			foreach (var subresourceTexture in m_subresourceTextures)
			{
				string item = subresourceTexture.Path;
				bool flag = false;
				if (IsLowMipMapVersion)
				{
					MyTextureCacheItem lowMipMapCacheItem = MyManagers.FileTextures.GetLowMipMapCacheItem(item);
					flag = lowMipMapCacheItem != null;
					if (flag)
					{
						parameters = lowMipMapCacheItem.TextureParams;
					}
				}
				else
				{
					flag = MyFileTextureParamsManager.LoadFromFile(item, out parameters);
				}
				if (flag)
				{
					if (m_type != MyFileTextureEnum.NORMALMAP_GLOSS)
					{
						parameters.Format = MyResourceUtils.MakeSrgb(parameters.Format);
					}
					ref Vector2I resolution = ref parameters.Resolution;
					ref int mipLevels = ref parameters.MipLevels;
					(resolution, mipLevels) = MyResourceUtils.GetTextureSizeAfterMipmapSkip(m_type, isVoxel: true, parameters.Resolution, parameters.MipLevels);
					return true;
				}
			}
			parameters.Format = m_recoverySystem.FormatBytePattern;
			parameters.MipLevels = 3;
			parameters.Resolution = new Vector2I(4, 4);
			parameters.ArraySize = 1;
			return false;
		}

		public void AddSlices(IEnumerable<(string Path, ITexture texture)> textures)
		{
			_ = IsLowMipMapVersion;
			foreach (var texture in textures)
			{
				string item = texture.Path;
				ITexture item2 = texture.texture;
				int count = m_subresourceTextures.Count;
				if (count == m_minSlices)
				{
					break;
				}
				AddSlice(count, item2, 0);
				m_subresourceTextures.Add((item, default(MyStreamedTexturePin)));
			}
		}

		public void SwapSlice(int sliceIndex, string path, ITexture texture)
		{
			_ = IsLowMipMapVersion;
			if (sliceIndex < m_subresourceTextures.Count)
			{
				AddSlice(sliceIndex, texture, 0, recordByteSize: false);
				m_subresourceTextures[sliceIndex] = (path, default(MyStreamedTexturePin));
			}
		}

		private void AddSlice(int i, IResource sourceResource, int sourceSlice, bool recordByteSize = true)
		{
			string text = string.Empty;
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			Texture2D texture2D = sourceResource.Resource as Texture2D;
			if (texture2D != null)
			{
				text = texture2D.DebugName;
				texture2DDescription = texture2D.Description;
			}
			bool flag = MyResourceUtils.CheckTexturesConsistency(m_description, texture2DDescription);
			if (!flag && !IsLowMipMapVersion && !string.IsNullOrEmpty(text) && MyFileSystem.FileExists(text))
			{
				_ = $"Texture {text} cannot be loaded. If this message is displayed on reloading textures, please restart the game. If it is not, please notify developers.";
			}
			IGeneratedTexture generatedTexture = null;
			if (!flag && m_recoverySystem.UseBytePattern)
			{
				MyRender11.Log.WriteLine($"Texture {text} with size {texture2DDescription.Width}x{texture2DDescription.Height} can't fit to file array with size {m_description.Width}x{m_description.Height}");
				generatedTexture = MyManagers.GeneratedTextures.CreateFromBytePattern("MyFileArrayTexture.Tmp", Size.X, Size.Y, m_recoverySystem.FormatBytePattern, m_recoverySystem.BytePattern);
				sourceResource = generatedTexture;
				texture2DDescription = (generatedTexture.Resource as Texture2D).Description;
				sourceSlice = 0;
				flag = MyResourceUtils.CheckTexturesConsistency(m_description, texture2DDescription);
			}
			if (!flag)
			{
				Texture2DDescription description = m_description;
				Texture2DDescription texture2DDescription2 = texture2DDescription;
				string text2 = $"Textures ({text}) is not compatible within array texture! Width: ({description.Width},{texture2DDescription2.Width}) Height: ({description.Height},{texture2DDescription2.Height}) Mipmaps: ({description.MipLevels},{texture2DDescription2.MipLevels}) Format: ({description.Format},{texture2DDescription2.Format})";
				MyRenderProxy.Error(text2);
				MyRender11.Log.WriteLine(text2);
			}
			else
			{
				for (int j = 0; j < MipLevels; j++)
				{
					MyRender11.RC.CopySubresourceRegion(sourceResource, SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(j, sourceSlice, MipLevels), null, Resource, SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(j, i, MipLevels));
					if (recordByteSize)
					{
						int height = SharpDX.Direct3D11.Resource.CalculateMipSize(j, Size.X);
						int height2 = SharpDX.Direct3D11.Resource.CalculateMipSize(j, Size.Y);
						ByteSize += Format.ComputeScanlineCount(height) * 4 * Format.ComputeScanlineCount(height2) * 4 * Format.SizeOfInBytes();
					}
				}
			}
			MyGeneratedTexture myGeneratedTexture;
			if (generatedTexture != null)
			{
				MyManagers.GeneratedTextures.DisposeTex(generatedTexture);
			}
			else if ((myGeneratedTexture = sourceResource as MyGeneratedTexture) != null)
			{
				myGeneratedTexture.Dispose();
			}
		}

		public bool OnDeviceInit(MyFileArrayTexture source = null, bool forceSyncLoad = false)
		{
			if (MyFileTextureManager.ENABLE_TEXTURE_ASYNC_LOADING && !IsLowMipMapVersion)
			{
				foreach (var subresourceTexture in m_subresourceTextures)
				{
					MyStreamedTexturePin item = subresourceTexture.TextureHandle;
					if (!item.Texture.IsTextureLoaded())
					{
						if (!forceSyncLoad)
						{
							return false;
						}
						while (!item.Texture.IsTextureLoaded())
						{
							Thread.Yield();
						}
					}
				}
			}
			m_dirtyFlag = false;
			GetCorrectedFileTextureParams(out var parameters);
			m_size = parameters.Resolution;
			m_slices = Math.Max(m_subresourceTextures.Count, m_minSlices);
			Format = ((parameters.Format == Format.Unknown) ? Format.BC1_UNorm_SRgb : parameters.Format);
			MipLevels = parameters.MipLevels;
			m_description = new Texture2DDescription
			{
				ArraySize = m_slices,
				BindFlags = BindFlags.ShaderResource,
				CpuAccessFlags = CpuAccessFlags.None,
				Format = Format,
				Height = Size.Y,
				Width = Size.X,
				MipLevels = MipLevels,
				SampleDescription = 
				{
					Count = 1,
					Quality = 0
				},
				Usage = ResourceUsage.Default
			};
			m_resource = new Texture2D(MyRender11.DeviceInstance, m_description)
			{
				DebugName = m_resourceName
			};
			for (int i = 0; i < m_subresourceTextures.Count; i++)
			{
				(string Path, MyStreamedTexturePin TextureHandle) tuple = m_subresourceTextures[i];
				string item2 = tuple.Path;
				MyStreamedTexturePin item3 = tuple.TextureHandle;
				int sourceSlice;
				IResource resource = GetResource(item2, item3, source, i, Size, out sourceSlice);
				AddSlice(i, resource, sourceSlice);
				if (!item3.IsEmpty)
				{
					item3.Dispose();
					m_subresourceTextures[i] = (item2, default(MyStreamedTexturePin));
				}
			}
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, Resource);
			return true;
		}

		public void OnDeviceEnd()
		{
			ByteSize = 0L;
			m_dirtyFlag = true;
			IsLowMipMapVersion = false;
			Format = Format.Unknown;
			foreach (var subresourceTexture in m_subresourceTextures)
			{
				subresourceTexture.TextureHandle.Dispose();
			}
			m_subresourceTextures.Clear();
			DisposeTextures();
		}

		private void DisposeTextures()
		{
			if (m_srv != null)
			{
				m_srv.Dispose();
				m_srv = null;
			}
			if (m_resource != null)
			{
				m_resource.Dispose();
				m_resource = null;
			}
		}

		private IResource GetResource(string filepath, MyStreamedTexturePin textureHandle, MyFileArrayTexture referenceArray, int referenceSlice, Vector2I textureSize, out int sourceSlice)
		{
			if (referenceArray != null && referenceSlice < referenceArray.SubTexturesCount && referenceArray.Size == textureSize && filepath == referenceArray.SubResourceFileNames[referenceSlice].Path)
			{
				sourceSlice = referenceSlice;
				return referenceArray;
			}
			ITexture texture = null;
			if (IsLowMipMapVersion)
			{
				string name = filepath;
				if (!string.IsNullOrEmpty(name) && MyResourceUtils.NormalizeFileTextureName(ref name))
				{
					texture = MyManagers.FileTextures.CreateCacheTexture(name);
				}
				if (texture == null)
				{
					MyManagers.FileTextures.TryGetTexture("EMPTY", out texture);
				}
			}
			else
			{
				texture = textureHandle.Texture;
			}
			if (!(texture.Resource is Texture2D))
			{
				sourceSlice = -1;
				return null;
			}
			sourceSlice = 0;
			return texture;
		}

		public void Update(bool isDeviceInit)
		{
			if (m_dirtyFlag && isDeviceInit && OnDeviceInit())
			{
				MyManagers.FileArrayTextures.Statistics.Add(this);
			}
		}
	}
}
