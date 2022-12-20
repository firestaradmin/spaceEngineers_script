<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyArrayTextureManager : IManager, IManagerDevice
	{
		private MyObjectsPool<MyRtvArrayTexture> m_rtvArrays = new MyObjectsPool<MyRtvArrayTexture>(16);

		private MyObjectsPool<MyUavArrayTexture> m_uavArrays = new MyObjectsPool<MyUavArrayTexture>(16);

		private MyObjectsPool<MyDepthArrayTexture> m_depthArrays = new MyObjectsPool<MyDepthArrayTexture>(16);

		private bool m_isDeviceInit;

		internal static bool CheckArrayCompatible(Texture2DDescription desc1, Texture2DDescription desc2)
		{
			if (desc1.Format == desc2.Format && desc1.Width == desc2.Width && desc1.Height == desc2.Height && desc1.Format == desc2.Format && desc1.MipLevels == desc2.MipLevels && desc1.SampleDescription.Count == desc2.SampleDescription.Count)
			{
				return desc1.SampleDescription.Quality == desc2.SampleDescription.Quality;
			}
			return false;
		}

		internal IUavArrayTexture CreateUavCube(string debugName, int size, Format format, int mipLevels = 1, bool generateMipMaps = false)
		{
			m_uavArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = 6;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget | BindFlags.UnorderedAccess;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = format;
			texture2DDescription.Height = size;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.TextureCube | (generateMipMaps ? ResourceOptionFlags.GenerateMipMaps : ResourceOptionFlags.None);
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = size;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = format;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.TextureCube;
			shaderResourceViewDescription.TextureCube.MipLevels = mipLevels;
			shaderResourceViewDescription.TextureCube.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitUav(debugName, resourceDesc, srvDesc, format, format);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal IDepthArrayTexture CreateDepthCube(string debugName, int size, Format resourceFormat, Format srvFormat, Format dsvFormat, int mipLevels = 1)
		{
			m_depthArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = 6;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.DepthStencil;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = resourceFormat;
			texture2DDescription.Height = size;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.TextureCube;
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = size;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = srvFormat;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.TextureCube;
			shaderResourceViewDescription.TextureCube.MipLevels = mipLevels;
			shaderResourceViewDescription.TextureCube.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitDepth(debugName, resourceDesc, srvDesc, dsvFormat);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal IRtvArrayTexture CreateRtvArray(string debugName, int width, int height, int arraySize, Format format, int mipLevels = 1)
		{
			m_rtvArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = arraySize;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = format;
			texture2DDescription.Height = height;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = width;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = format;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDescription.Texture2DArray.ArraySize = arraySize;
			shaderResourceViewDescription.Texture2DArray.FirstArraySlice = 0;
			shaderResourceViewDescription.Texture2DArray.MipLevels = mipLevels;
			shaderResourceViewDescription.Texture2DArray.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitRtv(debugName, resourceDesc, srvDesc, format);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal IRtvArrayTexture CreateRtvCube(string debugName, int size, Format format, int mipLevels = 1, bool generateMipMaps = false)
		{
			m_rtvArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = 6;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = format;
			texture2DDescription.Height = size;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.TextureCube | (generateMipMaps ? ResourceOptionFlags.GenerateMipMaps : ResourceOptionFlags.None);
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = size;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = format;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.TextureCube;
			shaderResourceViewDescription.TextureCube.MipLevels = mipLevels;
			shaderResourceViewDescription.TextureCube.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitRtv(debugName, resourceDesc, srvDesc, format);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal IUavArrayTexture CreateUavArray(string debugName, int width, int height, int arraySize, Format format, int mipLevels = 1)
		{
			m_uavArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = arraySize;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget | BindFlags.UnorderedAccess;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = format;
			texture2DDescription.Height = height;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = width;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = format;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDescription.Texture2DArray.ArraySize = arraySize;
			shaderResourceViewDescription.Texture2DArray.FirstArraySlice = 0;
			shaderResourceViewDescription.Texture2DArray.MipLevels = mipLevels;
			shaderResourceViewDescription.Texture2DArray.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitUav(debugName, resourceDesc, srvDesc, format, format);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal IDepthArrayTexture CreateDepthArray(string debugName, int width, int height, int arraySize, Format resourceFormat, Format srvFormat, Format dsvFormat, int mipLevels = 1)
		{
			m_depthArrays.AllocateOrCreate(out var item);
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.ArraySize = arraySize;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.DepthStencil;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.Format = resourceFormat;
			texture2DDescription.Height = height;
			texture2DDescription.MipLevels = mipLevels;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			texture2DDescription.SampleDescription.Count = 1;
			texture2DDescription.SampleDescription.Quality = 0;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.Width = width;
			Texture2DDescription resourceDesc = texture2DDescription;
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = srvFormat;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDescription.Texture2DArray.ArraySize = arraySize;
			shaderResourceViewDescription.Texture2DArray.FirstArraySlice = 0;
			shaderResourceViewDescription.Texture2DArray.MipLevels = mipLevels;
			shaderResourceViewDescription.Texture2DArray.MostDetailedMip = 0;
			ShaderResourceViewDescription srvDesc = shaderResourceViewDescription;
			item.InitDepth(debugName, resourceDesc, srvDesc, dsvFormat);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		internal void DisposeTex(ref IRtvArrayTexture texture)
		{
			if (texture != null)
			{
				MyRtvArrayTexture myRtvArrayTexture = (MyRtvArrayTexture)texture;
				if (m_isDeviceInit)
				{
					myRtvArrayTexture.OnDeviceEnd();
				}
				m_rtvArrays.Deallocate(myRtvArrayTexture);
				texture = null;
			}
		}

		internal void DisposeTex(ref IUavArrayTexture texture)
		{
			if (texture != null)
			{
				MyUavArrayTexture myUavArrayTexture = (MyUavArrayTexture)texture;
				if (m_isDeviceInit)
				{
					myUavArrayTexture.OnDeviceEnd();
				}
				m_uavArrays.Deallocate(myUavArrayTexture);
				texture = null;
			}
		}

		internal void DisposeTex(ref IDepthArrayTexture texture)
		{
			if (texture != null)
			{
				MyDepthArrayTexture myDepthArrayTexture = (MyDepthArrayTexture)texture;
				if (m_isDeviceInit)
				{
					myDepthArrayTexture.OnDeviceEnd();
				}
				m_depthArrays.Deallocate(myDepthArrayTexture);
				texture = null;
			}
		}

		public void OnDeviceInit()
		{
<<<<<<< HEAD
			m_isDeviceInit = true;
			foreach (MyRtvArrayTexture item in m_rtvArrays.Active)
			{
				item.OnDeviceInit();
			}
			foreach (MyUavArrayTexture item2 in m_uavArrays.Active)
			{
				item2.OnDeviceInit();
			}
			foreach (MyDepthArrayTexture item3 in m_depthArrays.Active)
			{
				item3.OnDeviceInit();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = true;
			Enumerator<MyRtvArrayTexture> enumerator = m_rtvArrays.Active.GetEnumerator();
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
			}
			Enumerator<MyUavArrayTexture> enumerator2 = m_uavArrays.Active.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			Enumerator<MyDepthArrayTexture> enumerator3 = m_depthArrays.Active.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.get_Current().OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceReset()
		{
<<<<<<< HEAD
			foreach (MyRtvArrayTexture item in m_rtvArrays.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
			}
			foreach (MyUavArrayTexture item2 in m_uavArrays.Active)
			{
				item2.OnDeviceEnd();
				item2.OnDeviceInit();
			}
			foreach (MyDepthArrayTexture item3 in m_depthArrays.Active)
			{
				item3.OnDeviceEnd();
				item3.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyRtvArrayTexture> enumerator = m_rtvArrays.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyRtvArrayTexture current = enumerator.get_Current();
					current.OnDeviceEnd();
					current.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MyUavArrayTexture> enumerator2 = m_uavArrays.Active.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyUavArrayTexture current2 = enumerator2.get_Current();
					current2.OnDeviceEnd();
					current2.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			Enumerator<MyDepthArrayTexture> enumerator3 = m_depthArrays.Active.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MyDepthArrayTexture current3 = enumerator3.get_Current();
					current3.OnDeviceEnd();
					current3.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceEnd()
		{
<<<<<<< HEAD
			m_isDeviceInit = false;
			foreach (MyRtvArrayTexture item in m_rtvArrays.Active)
			{
				item.OnDeviceEnd();
			}
			foreach (MyUavArrayTexture item2 in m_uavArrays.Active)
			{
				item2.OnDeviceEnd();
			}
			foreach (MyDepthArrayTexture item3 in m_depthArrays.Active)
			{
				item3.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = false;
			Enumerator<MyRtvArrayTexture> enumerator = m_rtvArrays.Active.GetEnumerator();
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
			}
			Enumerator<MyUavArrayTexture> enumerator2 = m_uavArrays.Active.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnDeviceEnd();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			Enumerator<MyDepthArrayTexture> enumerator3 = m_depthArrays.Active.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.get_Current().OnDeviceEnd();
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public int GetArrayTexturesCount()
		{
			return 0 + m_rtvArrays.ActiveCount + m_uavArrays.ActiveCount + m_depthArrays.ActiveCount;
		}
	}
}
