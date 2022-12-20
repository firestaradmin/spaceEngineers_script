using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Library.Memory;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyDepthStencilManager : IManager, IManagerDevice, IManagerUnloadData
	{
		public readonly MyMemorySystem MemoryTracker = MyManagers.TexturesMemoryTracker.RegisterSubsystem("DepthStencil");

		private readonly MyObjectsPool<MyDepthStencil> m_objectsPool = new MyObjectsPool<MyDepthStencil>(16);

		private bool m_isDeviceInit;

<<<<<<< HEAD
=======
		private bool HQ_DEPTH;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public IDepthStencil CreateDepthStencil(string debugName, int width, int height, bool hqDepth, int samplesCount = 1, int samplesQuality = 0)
		{
			Format resourceFormat;
			Format dsvFormat;
			Format srvDepthFormat;
			Format srvStencilFormat;
			if (hqDepth)
			{
				resourceFormat = Format.R32G8X24_Typeless;
				dsvFormat = Format.D32_Float_S8X24_UInt;
				srvDepthFormat = Format.R32_Float_X8X24_Typeless;
				srvStencilFormat = Format.X32_Typeless_G8X24_UInt;
			}
			else
			{
				resourceFormat = Format.R24G8_Typeless;
				dsvFormat = Format.D24_UNorm_S8_UInt;
				srvDepthFormat = Format.R24_UNorm_X8_Typeless;
				srvStencilFormat = Format.X24_Typeless_G8_UInt;
			}
			MyDepthStencil myDepthStencil = m_objectsPool.Allocate();
			myDepthStencil.Init(debugName, width, height, resourceFormat, dsvFormat, srvDepthFormat, srvStencilFormat, samplesCount, samplesQuality);
			if (m_isDeviceInit)
			{
				try
				{
					myDepthStencil.OnDeviceInit();
					return myDepthStencil;
				}
				catch (Exception)
				{
					IDepthStencil tex = myDepthStencil;
					DisposeTex(ref tex);
					throw;
				}
			}
			return myDepthStencil;
		}

		public int GetDepthStencilsCount()
		{
			return m_objectsPool.ActiveCount;
		}

		internal void DisposeTex(ref IDepthStencil tex)
		{
			if (tex != null)
			{
				MyDepthStencil myDepthStencil = (MyDepthStencil)tex;
				if (m_isDeviceInit)
				{
					myDepthStencil.OnDeviceEnd();
				}
				m_objectsPool.Deallocate(myDepthStencil);
				tex = null;
			}
		}

		public void OnDeviceInit()
		{
<<<<<<< HEAD
			m_isDeviceInit = true;
			foreach (MyDepthStencil item in m_objectsPool.Active)
			{
				item.OnDeviceInit();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = true;
			Enumerator<MyDepthStencil> enumerator = m_objectsPool.Active.GetEnumerator();
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
			foreach (MyDepthStencil item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyDepthStencil> enumerator = m_objectsPool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDepthStencil current = enumerator.get_Current();
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
			foreach (MyDepthStencil item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = false;
			Enumerator<MyDepthStencil> enumerator = m_objectsPool.Active.GetEnumerator();
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
	}
}
