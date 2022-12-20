using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;
using VRageMath;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyDynamicFileArrayTexture : IDynamicFileArrayTexture, IFileArrayTexture, ITexture, ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyDynamicFileArrayTexture_003C_003EActor : IActivator, IActivator<MyDynamicFileArrayTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyDynamicFileArrayTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDynamicFileArrayTexture CreateInstance()
			{
				return new MyDynamicFileArrayTexture();
			}

			MyDynamicFileArrayTexture IActivator<MyDynamicFileArrayTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private IFileArrayTexture m_arrayTexture;

		private string m_name;

		private MyFileTextureEnum m_type;

		private bool m_dirtyFlag;

		private bool m_swappedFlag;

		private Format m_formatBytePattern;

		private byte[] m_errorBytePattern;

		private readonly List<(string Path, MyStreamedTexturePin TextureHandle)> m_filePaths = new List<(string, MyStreamedTexturePin)>();

		private readonly Dictionary<string, int> m_lookupTable = new Dictionary<string, int>();

		private int m_minSlices;

		private bool m_keepAsLowMipMap;

		public Format Format
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.Format;
				}
				return Format.Unknown;
			}
		}

		public int MipLevels
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.MipLevels;
				}
				return 0;
			}
		}

		public int SubTexturesCount
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.SubTexturesCount;
				}
				return 0;
			}
		}

		IEnumerable<string> IFileArrayTexture.SubTextures => m_arrayTexture.SubTextures;

		public int SlicesCount
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.SubTexturesCount;
				}
				return 0;
			}
		}

		public string Name => m_name;

		public SharpDX.Direct3D11.Resource Resource => m_arrayTexture?.Resource;

		public MyFileTextureEnum Type => m_type;

		public Vector3I Size3 => new Vector3I(Size.X, Size.Y, SlicesCount);

		public Vector2I Size
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.Size;
				}
				return Vector2I.Zero;
			}
		}

		public int MinSlices
		{
			get
			{
				return m_minSlices;
			}
			set
			{
				m_minSlices = value;
				m_dirtyFlag = true;
			}
		}

		public ShaderResourceView Srv => m_arrayTexture?.Srv;

		public bool IsLowMipMapVersion
		{
			get
			{
				if (m_arrayTexture != null)
				{
					return m_arrayTexture.IsLowMipMapVersion;
				}
				return false;
			}
		}

		public event Action<ITexture> OnFormatChanged;

		public void AddSlices(IEnumerable<(string Path, ITexture texture)> textures)
		{
		}

		public void SwapSlice(int sliceIndex, string path, ITexture texture)
		{
		}

		public int GetOrAddSlice(string filepath)
		{
			filepath = MyResourceUtils.GetTextureFullPath(filepath);
			if (m_lookupTable.ContainsKey(filepath))
			{
				return m_lookupTable[filepath];
			}
			int count = m_filePaths.Count;
			m_lookupTable.Add(filepath, count);
			m_filePaths.Add((filepath, PrepareTexture(filepath)));
			m_dirtyFlag = true;
			return count;
		}

		public void SwapSlice(int sliceNum, string filepath)
		{
			filepath = MyResourceUtils.GetTextureFullPath(filepath);
			_ = m_filePaths.Count;
			(string, MyStreamedTexturePin) tuple = m_filePaths[sliceNum];
			m_lookupTable.Remove(tuple.Item1);
			DisposePrefetchedTexture(sliceNum);
			m_lookupTable[filepath] = sliceNum;
			m_filePaths[sliceNum] = (filepath, PrepareTexture(filepath));
			m_dirtyFlag = true;
			m_swappedFlag = true;
		}

		private MyStreamedTexturePin PrepareTexture(string filepath)
		{
			MyStreamedTexturePin result = default(MyStreamedTexturePin);
			if (!m_keepAsLowMipMap)
			{
				return MyManagers.Textures.GetPermanentTexture(filepath, new MyTextureStreamingManager.QueryArgs
				{
					TextureType = m_type,
					IsVoxel = true
				});
			}
			return result;
		}

		public void Init(string name, MyFileTextureEnum type, byte[] errorBytePattern, Format formatBytePattern, int minSlices, bool keepAsLow = false)
		{
			m_name = name;
			m_type = type;
			m_arrayTexture = null;
			m_dirtyFlag = false;
			m_swappedFlag = false;
			m_errorBytePattern = errorBytePattern;
			m_formatBytePattern = formatBytePattern;
			m_minSlices = minSlices;
			m_keepAsLowMipMap = keepAsLow;
		}

		public void Update(bool isDeviceInit)
		{
			if (m_dirtyFlag)
			{
				Load();
			}
		}

		private void Load(bool forceSync = false)
		{
			IFileArrayTexture previousArrayTexture = m_arrayTexture;
			if ((MyDynamicFileArrayTextureManager.ENABLE_TEXTURE_ASYNC_LOADING && !forceSync) || m_keepAsLowMipMap)
			{
				bool flag = HasAllTexturesPreLoaded();
				if (m_arrayTexture == null || (IsLowMipMapVersion && !flag))
				{
					if (m_arrayTexture == null || m_swappedFlag || m_filePaths.Count != m_arrayTexture.SubTexturesCount)
					{
						LoadLowMipMap();
						m_swappedFlag = false;
						m_dirtyFlag = !m_keepAsLowMipMap;
					}
					return;
				}
				if (m_arrayTexture != null && !IsLowMipMapVersion && m_filePaths.Count <= Math.Max(m_minSlices, m_arrayTexture.Size3.Z))
				{
					int subTexturesCount = m_arrayTexture.SubTexturesCount;
					bool flag2 = false;
					for (int i = 0; i < subTexturesCount; i++)
					{
						(string, MyStreamedTexturePin) tuple = m_filePaths[i];
						if (!tuple.Item2.IsEmpty)
						{
							if (tuple.Item2.Texture.IsTextureLoaded())
							{
								m_arrayTexture.SwapSlice(i, tuple.Item1, tuple.Item2.Texture);
								DisposePrefetchedTexture(i);
							}
							else
							{
								flag2 = true;
							}
						}
					}
					int num = 0;
					for (int j = subTexturesCount; j < m_filePaths.Count && m_filePaths[j].TextureHandle.Texture.IsTextureLoaded(); j++)
					{
						num++;
					}
<<<<<<< HEAD
					IEnumerable<(string, MyStreamedTexturePin)> source = m_filePaths.Skip(subTexturesCount).Take(num);
					m_arrayTexture.AddSlices(source.Select<(string, MyStreamedTexturePin), (string, ITexture)>(((string Path, MyStreamedTexturePin TextureHandle) x) => (x.Path, x.TextureHandle.Texture)));
=======
					IEnumerable<(string, MyStreamedTexturePin)> enumerable = Enumerable.Take<(string, MyStreamedTexturePin)>(Enumerable.Skip<(string, MyStreamedTexturePin)>((IEnumerable<(string, MyStreamedTexturePin)>)m_filePaths, subTexturesCount), num);
					m_arrayTexture.AddSlices(Enumerable.Select<(string, MyStreamedTexturePin), (string, ITexture)>(enumerable, (Func<(string, MyStreamedTexturePin), (string, ITexture)>)(((string Path, MyStreamedTexturePin TextureHandle) x) => (x.Path, x.TextureHandle.Texture))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					DisposePrefetchedTextures(subTexturesCount, num);
					bool flag3 = m_filePaths.Count != m_arrayTexture.SubTexturesCount;
					m_dirtyFlag = flag2 || flag3;
					m_swappedFlag = flag2;
					return;
				}
				if (m_swappedFlag || IsLowMipMapVersion)
				{
					MyManagers.FileArrayTextures.DisposeTex(ref previousArrayTexture);
					previousArrayTexture = null;
				}
			}
<<<<<<< HEAD
			m_arrayTexture = MyManagers.FileArrayTextures.CreateFromFiles(m_name, m_filePaths.Select(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path).ToArray(), m_type, m_errorBytePattern, m_formatBytePattern, m_minSlices, previousArrayTexture as MyFileArrayTexture, forceSync);
=======
			m_arrayTexture = MyManagers.FileArrayTextures.CreateFromFiles(m_name, Enumerable.ToArray<string>(Enumerable.Select<(string, MyStreamedTexturePin), string>((IEnumerable<(string, MyStreamedTexturePin)>)m_filePaths, (Func<(string, MyStreamedTexturePin), string>)(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path))), m_type, m_errorBytePattern, m_formatBytePattern, m_minSlices, previousArrayTexture as MyFileArrayTexture, forceSync);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			this.OnFormatChanged.InvokeIfNotNull(this);
			this.OnFormatChanged = null;
			if (m_arrayTexture != null)
			{
				DisposePrefetchedTextures(0, m_filePaths.Count);
			}
			MyManagers.FileArrayTextures.DisposeTex(ref previousArrayTexture);
			m_dirtyFlag = false;
			m_swappedFlag = false;
			void DisposePrefetchedTextures(int begin, int count)
			{
				for (int k = begin; k < begin + count; k++)
				{
					DisposePrefetchedTexture(k);
				}
			}
			bool HasAllTexturesPreLoaded()
			{
				foreach (var filePath in m_filePaths)
				{
					MyStreamedTexturePin item = filePath.TextureHandle;
					if (item.IsEmpty || !item.Texture.IsTextureLoaded())
					{
						return false;
					}
				}
				return true;
			}
			void LoadLowMipMap()
			{
				if (HasAnyTexture(m_filePaths))
				{
<<<<<<< HEAD
					IFileArrayTexture fileArrayTexture = MyManagers.FileArrayTextures.CreateFromFilesLowMipMap(m_name, m_filePaths.Select(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path).ToArray(), m_type, m_errorBytePattern, m_formatBytePattern, m_minSlices);
=======
					IFileArrayTexture fileArrayTexture = MyManagers.FileArrayTextures.CreateFromFilesLowMipMap(m_name, Enumerable.ToArray<string>(Enumerable.Select<(string, MyStreamedTexturePin), string>((IEnumerable<(string, MyStreamedTexturePin)>)m_filePaths, (Func<(string, MyStreamedTexturePin), string>)(((string Path, MyStreamedTexturePin TextureHandle) x) => x.Path))), m_type, m_errorBytePattern, m_formatBytePattern, m_minSlices);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (fileArrayTexture != null)
					{
						MyManagers.FileArrayTextures.DisposeTex(ref previousArrayTexture);
						m_arrayTexture = fileArrayTexture;
					}
				}
			}
		}

		private void DisposePrefetchedTexture(int id)
		{
			(string, MyStreamedTexturePin) tuple = m_filePaths[id];
			tuple.Item2.Dispose();
			m_filePaths[id] = (tuple.Item1, default(MyStreamedTexturePin));
		}

		private bool HasAnyTexture(List<(string, MyStreamedTexturePin)> files)
		{
			foreach (var file in files)
			{
				if (!string.IsNullOrEmpty(file.Item1))
				{
					return true;
				}
			}
			return false;
		}

		public void OnDeviceEnd()
		{
			MyManagers.FileArrayTextures.DisposeTex(ref m_arrayTexture);
			for (int i = 0; i < m_filePaths.Count; i++)
			{
				DisposePrefetchedTexture(i);
			}
			m_dirtyFlag = true;
		}

		public void Release()
		{
			OnDeviceEnd();
			foreach (var filePath in m_filePaths)
			{
				_ = filePath;
			}
			m_filePaths.Clear();
			m_lookupTable.Clear();
		}

		public void Reload()
		{
			OnDeviceEnd();
			Load(forceSync: true);
		}
	}
}
