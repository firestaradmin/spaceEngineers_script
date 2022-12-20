<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.Resources
{
	internal class MyRwTextureManager : IManager, IManagerDevice, IManagerUnloadData
	{
		private MyTextureStatistics m_statistics = new MyTextureStatistics(MyManagers.TexturesMemoryTracker.RegisterSubsystem("RwTextures"));

		private MyObjectsPool<MySrvTexture> m_srvTextures = new MyObjectsPool<MySrvTexture>(16);

		private MyObjectsPool<MyRtvTexture> m_rtvTextures = new MyObjectsPool<MyRtvTexture>(64);

		private MyObjectsPool<MyUavTexture> m_uavTextures = new MyObjectsPool<MyUavTexture>(64);

		private MyObjectsPool<MyDepthTexture> m_depthTextures = new MyObjectsPool<MyDepthTexture>(16);

		private bool m_isDeviceInit;

		private int m_srvTexturesActiveStart;

		private int m_rtvTexturesActiveStart;

		private int m_uavTexturesActiveStart;

		private int m_depthTexturesActiveStart;

		public MyTextureStatistics Statistics => m_statistics;

		public ISrvTexture CreateSrv(string debugName, int width, int height, Format format, int samplesCount = 1, int samplesQuality = 0, ResourceOptionFlags optionFlags = ResourceOptionFlags.None, ResourceUsage resourceUsage = ResourceUsage.Default, int mipLevels = 1, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None)
		{
			m_srvTextures.AllocateOrCreate(out var item);
			item.Init(debugName, width, height, format, format, BindFlags.ShaderResource, samplesCount, samplesQuality, optionFlags, resourceUsage, mipLevels, cpuAccessFlags);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			m_statistics.Add(item);
			return item;
		}

		public IRtvTexture CreateRtv(string debugName, int width, int height, Format format, int samplesCount = 1, int samplesQuality = 0, ResourceOptionFlags optionFlags = ResourceOptionFlags.None, ResourceUsage resourceUsage = ResourceUsage.Default, int mipLevels = 1, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None)
		{
			m_rtvTextures.AllocateOrCreate(out var item);
			item.Init(debugName, width, height, format, format, BindFlags.ShaderResource | BindFlags.RenderTarget, samplesCount, samplesQuality, optionFlags, resourceUsage, mipLevels, cpuAccessFlags);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			m_statistics.Add(item);
			return item;
		}

		public IUavTexture CreateUav(string debugName, int width, int height, Format format, int samplesCount = 1, int samplesQuality = 0, ResourceOptionFlags roFlags = ResourceOptionFlags.None, int mipLevels = 1, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None)
		{
			m_uavTextures.AllocateOrCreate(out var item);
			item.Init(debugName, width, height, format, format, BindFlags.ShaderResource | BindFlags.RenderTarget | BindFlags.UnorderedAccess, samplesCount, samplesQuality, roFlags, ResourceUsage.Default, mipLevels, cpuAccessFlags);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			m_statistics.Add(item);
			return item;
		}

		public IDepthTexture CreateDepth(string debugName, int width, int height, Format resourceFormat, Format srvFormat, Format dsvFormat, int samplesCount = 1, int samplesQuality = 0, ResourceOptionFlags roFlags = ResourceOptionFlags.None, int mipLevels = 1, CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None)
		{
			m_depthTextures.AllocateOrCreate(out var item);
			item.Init(debugName, width, height, resourceFormat, srvFormat, dsvFormat, BindFlags.ShaderResource | BindFlags.DepthStencil, samplesCount, samplesQuality, roFlags, ResourceUsage.Default, mipLevels, cpuAccessFlags);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			m_statistics.Add(item);
			return item;
		}

		public int GetTexturesCount()
		{
			return 0 + m_srvTextures.ActiveCount + m_rtvTextures.ActiveCount + m_uavTextures.ActiveCount + m_depthTextures.ActiveCount;
		}

		public void DisposeTex(ref ISrvTexture texture)
		{
			if (texture != null)
			{
				MySrvTexture mySrvTexture = (MySrvTexture)texture;
				if (m_isDeviceInit)
				{
					mySrvTexture.OnDeviceEnd();
				}
				m_srvTextures.Deallocate(mySrvTexture);
				m_statistics.Remove(mySrvTexture);
				texture = null;
			}
		}

		public void DisposeTex(ref IRtvTexture texture)
		{
			if (texture != null)
			{
				MyRtvTexture myRtvTexture = (MyRtvTexture)texture;
				if (m_isDeviceInit)
				{
					myRtvTexture.OnDeviceEnd();
				}
				m_rtvTextures.Deallocate(myRtvTexture);
				m_statistics.Remove(myRtvTexture);
				texture = null;
			}
		}

		public void DisposeTex(ref IUavTexture texture)
		{
			if (texture != null)
			{
				MyUavTexture myUavTexture = (MyUavTexture)texture;
				if (m_isDeviceInit)
				{
					myUavTexture.OnDeviceEnd();
				}
				m_uavTextures.Deallocate(myUavTexture);
				m_statistics.Remove(myUavTexture);
				texture = null;
			}
		}

		public void DisposeTex(ref IDepthTexture texture)
		{
			if (texture != null)
			{
				MyDepthTexture myDepthTexture = (MyDepthTexture)texture;
				if (m_isDeviceInit)
				{
					myDepthTexture.OnDeviceEnd();
				}
				m_depthTextures.Deallocate(myDepthTexture);
				m_statistics.Remove(myDepthTexture);
				texture = null;
			}
		}

		public void OnDeviceInit()
		{
<<<<<<< HEAD
			m_isDeviceInit = true;
			foreach (MySrvTexture item in m_srvTextures.Active)
			{
				item.OnDeviceInit();
			}
			foreach (MyRtvTexture item2 in m_rtvTextures.Active)
			{
				item2.OnDeviceInit();
			}
			foreach (MyUavTexture item3 in m_uavTextures.Active)
			{
				item3.OnDeviceInit();
			}
			foreach (MyDepthTexture item4 in m_depthTextures.Active)
			{
				item4.OnDeviceInit();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = true;
			Enumerator<MySrvTexture> enumerator = m_srvTextures.Active.GetEnumerator();
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
			Enumerator<MyRtvTexture> enumerator2 = m_rtvTextures.Active.GetEnumerator();
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
			Enumerator<MyUavTexture> enumerator3 = m_uavTextures.Active.GetEnumerator();
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
			}
			Enumerator<MyDepthTexture> enumerator4 = m_depthTextures.Active.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					enumerator4.get_Current().OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceReset()
		{
<<<<<<< HEAD
			foreach (MySrvTexture item in m_srvTextures.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
			}
			foreach (MyRtvTexture item2 in m_rtvTextures.Active)
			{
				item2.OnDeviceEnd();
				item2.OnDeviceInit();
			}
			foreach (MyUavTexture item3 in m_uavTextures.Active)
			{
				item3.OnDeviceEnd();
				item3.OnDeviceInit();
			}
			foreach (MyDepthTexture item4 in m_depthTextures.Active)
			{
				item4.OnDeviceEnd();
				item4.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MySrvTexture> enumerator = m_srvTextures.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySrvTexture current = enumerator.get_Current();
					current.OnDeviceEnd();
					current.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MyRtvTexture> enumerator2 = m_rtvTextures.Active.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyRtvTexture current2 = enumerator2.get_Current();
					current2.OnDeviceEnd();
					current2.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			Enumerator<MyUavTexture> enumerator3 = m_uavTextures.Active.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MyUavTexture current3 = enumerator3.get_Current();
					current3.OnDeviceEnd();
					current3.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
			Enumerator<MyDepthTexture> enumerator4 = m_depthTextures.Active.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					MyDepthTexture current4 = enumerator4.get_Current();
					current4.OnDeviceEnd();
					current4.OnDeviceInit();
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnDeviceEnd()
		{
<<<<<<< HEAD
			m_isDeviceInit = false;
			foreach (MySrvTexture item in m_srvTextures.Active)
			{
				item.OnDeviceEnd();
			}
			foreach (MyRtvTexture item2 in m_rtvTextures.Active)
			{
				item2.OnDeviceEnd();
			}
			foreach (MyUavTexture item3 in m_uavTextures.Active)
			{
				item3.OnDeviceEnd();
			}
			foreach (MyDepthTexture item4 in m_depthTextures.Active)
			{
				item4.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = false;
			Enumerator<MySrvTexture> enumerator = m_srvTextures.Active.GetEnumerator();
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
			Enumerator<MyRtvTexture> enumerator2 = m_rtvTextures.Active.GetEnumerator();
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
			Enumerator<MyUavTexture> enumerator3 = m_uavTextures.Active.GetEnumerator();
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
			}
			Enumerator<MyDepthTexture> enumerator4 = m_depthTextures.Active.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					enumerator4.get_Current().OnDeviceEnd();
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnSessionStart()
		{
			m_srvTexturesActiveStart = m_srvTextures.ActiveCount;
			m_rtvTexturesActiveStart = m_rtvTextures.ActiveCount;
			m_uavTexturesActiveStart = m_uavTextures.ActiveCount;
			m_depthTexturesActiveStart = m_depthTextures.ActiveCount;
		}

		public void OnUnloadData()
		{
		}
	}
}
