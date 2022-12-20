<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;

namespace VRage.Render11.Resources
{
	internal class MyDynamicFileArrayTextureManager : IManager, IManagerUpdate, IManagerUnloadData, IManagerDevice
	{
		public static readonly bool ENABLE_TEXTURE_ASYNC_LOADING = true;

		private readonly MyObjectsPool<MyDynamicFileArrayTexture> m_objectsPool = new MyObjectsPool<MyDynamicFileArrayTexture>(1);

		public void ReloadAll()
		{
<<<<<<< HEAD
			foreach (MyDynamicFileArrayTexture item in m_objectsPool.Active)
			{
				item.Reload();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyDynamicFileArrayTexture> enumerator = m_objectsPool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Reload();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public IDynamicFileArrayTexture CreateTexture(string name, MyFileTextureEnum type, byte[] bytePattern, Format bytePatternFormat, int minSlices, bool keepAsLowMipMap = false)
		{
			m_objectsPool.AllocateOrCreate(out var item);
			item.Init(name, type, bytePattern, bytePatternFormat, minSlices, keepAsLowMipMap);
			return item;
		}

		public void DisposeTex(ref IDynamicFileArrayTexture inTex)
		{
			if (inTex != null)
			{
				MyDynamicFileArrayTexture myDynamicFileArrayTexture = (MyDynamicFileArrayTexture)inTex;
				myDynamicFileArrayTexture.Release();
				m_objectsPool.Deallocate(myDynamicFileArrayTexture);
				inTex = null;
			}
		}

		void IManagerUpdate.OnUpdate()
		{
<<<<<<< HEAD
			foreach (MyDynamicFileArrayTexture item in m_objectsPool.Active)
			{
				item.Update(isDeviceInit: false);
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyDynamicFileArrayTexture> enumerator = m_objectsPool.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Update(isDeviceInit: false);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		void IManagerUnloadData.OnUnloadData()
		{
		}

		public void OnDeviceInit()
		{
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
<<<<<<< HEAD
			foreach (MyDynamicFileArrayTexture item in m_objectsPool.Active)
			{
				item.OnDeviceEnd();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyDynamicFileArrayTexture> enumerator = m_objectsPool.Active.GetEnumerator();
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
