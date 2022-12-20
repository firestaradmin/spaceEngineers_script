<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Generics;
using VRage.Library.Memory;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyCustomTextureManager : IManager, IManagerDevice, IManagerUnloadData
	{
		public readonly MyMemorySystem MemoryTracker = MyManagers.TexturesMemoryTracker.RegisterSubsystem("CustomTextures");

		private MyObjectsPool<MyCustomTexture> m_objectsPool = new MyObjectsPool<MyCustomTexture>(16);

		private bool m_isDeviceInit;

		public ICustomTexture CreateTexture(string debugName, int width, int height, int samplesCount = 1, int samplesQuality = 0)
		{
			m_objectsPool.AllocateOrCreate(out var item);
			item.Init(debugName, width, height, samplesCount, samplesQuality);
			if (m_isDeviceInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		public int GetTexturesCount()
		{
			return m_objectsPool.ActiveCount;
		}

		public void DisposeTex(ref ICustomTexture texture)
		{
			if (texture != null)
			{
				MyCustomTexture myCustomTexture = (MyCustomTexture)texture;
				if (m_isDeviceInit)
				{
					myCustomTexture.OnDeviceEnd();
				}
				m_objectsPool.Deallocate(myCustomTexture);
			}
		}

		public void OnDeviceInit()
		{
<<<<<<< HEAD
			m_isDeviceInit = true;
			foreach (MyCustomTexture item in m_objectsPool.Active)
			{
				item.OnDeviceInit();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = true;
			Enumerator<MyCustomTexture> enumerator = m_objectsPool.Active.GetEnumerator();
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
			foreach (MyCustomTexture item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyCustomTexture> enumerator = m_objectsPool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCustomTexture current = enumerator.get_Current();
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
			foreach (MyCustomTexture item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isDeviceInit = false;
			Enumerator<MyCustomTexture> enumerator = m_objectsPool.Active.GetEnumerator();
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
