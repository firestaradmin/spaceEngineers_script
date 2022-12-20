<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyFileArrayTextureManager : IManager, IManagerUpdate, IManagerDevice, IManagerUnloadData
	{
		public MyTextureStatistics Statistics = new MyTextureStatistics(MyManagers.TexturesMemoryTracker.RegisterSubsystem("FileArrayTextures"));

		private readonly MyObjectsPool<MyFileArrayTexture> m_fileTextureArrays = new MyObjectsPool<MyFileArrayTexture>(16);

		private bool m_isDeviceInit;

		public IFileArrayTexture CreateFromFiles(string resourceName, string[] inputFiles, MyFileTextureEnum type, byte[] bytePatternFor4x4, Format formatBytePattern, int minSlices, IFileArrayTexture source = null, bool forceSyncLoad = false)
		{
			m_fileTextureArrays.AllocateOrCreate(out var item);
			IFileArrayTexture tex = item;
			InitFromFiles(ref tex, resourceName, inputFiles, type, bytePatternFor4x4, formatBytePattern, minSlices, source, forceSyncLoad);
			return tex;
		}

		public IFileArrayTexture CreateFromFilesLowMipMap(string resourceName, string[] inputFiles, MyFileTextureEnum type, byte[] bytePatternFor4x4, Format formatBytePattern, int minSlices, IFileArrayTexture source = null)
		{
			m_fileTextureArrays.AllocateOrCreate(out var item);
			item.IsLowMipMapVersion = true;
			IFileArrayTexture tex = item;
			InitFromFiles(ref tex, resourceName, inputFiles, type, bytePatternFor4x4, formatBytePattern, minSlices, source);
			return tex;
		}

		private void InitFromFiles(ref IFileArrayTexture tex, string resourceName, string[] inputFiles, MyFileTextureEnum type, byte[] bytePatternFor4x4, Format formatBytePattern, int minSlices, IFileArrayTexture source = null, bool forceSyncLoad = false)
		{
			MyFileArrayTexture myFileArrayTexture = (MyFileArrayTexture)tex;
			if (!myFileArrayTexture.Load(resourceName, inputFiles, type, bytePatternFor4x4, formatBytePattern, minSlices, forceSyncLoad))
			{
				DisposeTex(ref tex);
			}
			else if (m_isDeviceInit)
			{
				MyFileArrayTexture source2 = (MyFileArrayTexture)source;
				if (myFileArrayTexture.OnDeviceInit(source2, forceSyncLoad))
				{
					Statistics.Add(myFileArrayTexture);
				}
			}
		}

		public IFileArrayTexture InitFromFilesAsync(string resourceName, string[] inputFiles, MyFileTextureEnum type, byte[] bytePatternFor4x4, Format formatBytePattern, int minSlices)
		{
			m_fileTextureArrays.AllocateOrCreate(out var item);
			if (!item.Load(resourceName, inputFiles, type, bytePatternFor4x4, formatBytePattern, minSlices))
			{
				IFileArrayTexture texture = item;
				DisposeTex(ref texture);
				return null;
			}
			return item;
		}

		public int GetTexturesCount()
		{
			return m_fileTextureArrays.ActiveCount;
		}

		public long GetTotalByteSizeOfResources()
		{
<<<<<<< HEAD
			long num = 0L;
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				num += item.ByteSize;
			}
			return num;
=======
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			long num = 0L;
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyFileArrayTexture current = enumerator.get_Current();
					num += current.ByteSize;
				}
				return num;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public StringBuilder GetFileTexturesDesc()
		{
<<<<<<< HEAD
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Loaded file array textures:");
			stringBuilder.AppendLine("[DebugName;ByteSize;Width;Height;Slices;Format;Filenames]");
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				stringBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};", item.Name, item.ByteSize, item.Size.X, item.Size.Y, item.Size3.Z, item.Format);
				foreach (var subResourceFileName in item.SubResourceFileNames)
				{
					stringBuilder.AppendFormat("{0}, ", subResourceFileName);
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder;
=======
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Loaded file array textures:");
			stringBuilder.AppendLine("[DebugName;ByteSize;Width;Height;Slices;Format;Filenames]");
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyFileArrayTexture current = enumerator.get_Current();
					stringBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};", current.Name, current.ByteSize, current.Size.X, current.Size.Y, current.Size3.Z, current.Format);
					foreach (var subResourceFileName in current.SubResourceFileNames)
					{
						stringBuilder.AppendFormat("{0}, ", subResourceFileName);
					}
					stringBuilder.AppendLine();
				}
				return stringBuilder;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void DisposeTex(ref IFileArrayTexture texture, bool deallocate = true)
		{
			if (texture != null)
			{
				MyFileArrayTexture myFileArrayTexture = (MyFileArrayTexture)texture;
				if (myFileArrayTexture.Format != 0)
				{
					Statistics.Remove(myFileArrayTexture);
				}
				if (m_isDeviceInit)
				{
					myFileArrayTexture.OnDeviceEnd();
				}
				if (deallocate)
				{
					m_fileTextureArrays.Deallocate(myFileArrayTexture);
				}
				texture = null;
			}
		}

		public void OnDeviceInit()
		{
<<<<<<< HEAD
=======
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_isDeviceInit)
			{
				return;
			}
			m_isDeviceInit = true;
<<<<<<< HEAD
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				item.OnDeviceInit();
=======
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceReset()
		{
<<<<<<< HEAD
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyFileArrayTexture current = enumerator.get_Current();
					current.OnDeviceEnd();
					current.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceEnd()
		{
<<<<<<< HEAD
			m_isDeviceInit = false;
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				item.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = false;
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().OnDeviceEnd();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnUnloadData()
		{
		}

		void IManagerUpdate.OnUpdate()
		{
<<<<<<< HEAD
=======
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_fileTextureArrays.Active.Count == 0)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyFileArrayTexture item in m_fileTextureArrays.Active)
			{
				item.Update(m_isDeviceInit);
=======
			Enumerator<MyFileArrayTexture> enumerator = m_fileTextureArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Update(m_isDeviceInit);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
