<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Generics;
using VRage.Render11.Common;

namespace VRage.Render11.Resources
{
	internal abstract class MyPersistentResourceManager<TResource, TDescription> : IManager, IManagerDevice where TResource : MyPersistentResourceBase<TDescription>, new()
	{
		private bool m_isInit;

		private MyObjectsPool<TResource> m_objectsPool;

		protected TResource CreateResource(string name, ref TDescription desc)
		{
			m_objectsPool.AllocateOrCreate(out var item);
			item.Init(name, ref desc);
			if (m_isInit)
			{
				item.OnDeviceInit();
			}
			return item;
		}

		protected abstract int GetAllocResourceCount();

		public MyPersistentResourceManager()
		{
			m_objectsPool = new MyObjectsPool<TResource>(GetAllocResourceCount());
		}

		public int GetResourcesCount()
		{
			return m_objectsPool.ActiveCount;
		}

		public virtual void OnDeviceInit()
		{
<<<<<<< HEAD
			m_isInit = true;
			foreach (TResource item in m_objectsPool.Active)
			{
				item.OnDeviceInit();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isInit = true;
			Enumerator<TResource> enumerator = m_objectsPool.Active.GetEnumerator();
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

		public virtual void OnDeviceReset()
		{
<<<<<<< HEAD
			foreach (TResource item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
				item.OnDeviceInit();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<TResource> enumerator = m_objectsPool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TResource current = enumerator.get_Current();
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

		public virtual void OnDeviceEnd()
		{
<<<<<<< HEAD
			m_isInit = false;
			foreach (TResource item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_isInit = false;
			Enumerator<TResource> enumerator = m_objectsPool.Active.GetEnumerator();
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
	}
}
