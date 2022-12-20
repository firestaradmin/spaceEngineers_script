using System.Collections.Generic;
using System.Linq;

namespace VRage.Render11.Common
{
	internal class MyGeneralManager
	{
		private readonly List<IManager> m_allManagers = new List<IManager>();

		private MyGeneralManagerState m_deviceState;

		public void RegisterManager(IManager manager)
		{
			m_allManagers.Add(manager);
			if (manager is IManagerDevice)
			{
				IManagerDevice managerDevice = (IManagerDevice)manager;
				if (m_deviceState == MyGeneralManagerState.INIT)
				{
					managerDevice.OnDeviceInit();
				}
			}
		}

		public void UnregisterManager(IManager manager)
		{
			if (manager is IManagerDevice)
			{
				IManagerDevice managerDevice = (IManagerDevice)manager;
				if (m_deviceState == MyGeneralManagerState.INIT)
				{
					managerDevice.OnDeviceEnd();
				}
			}
			m_allManagers.Remove(manager);
		}

		public void OnDeviceInit()
		{
			m_deviceState = MyGeneralManagerState.INIT;
			foreach (IManager allManager in m_allManagers)
			{
				if (allManager is IManagerDevice)
				{
					((IManagerDevice)allManager).OnDeviceInit();
				}
			}
		}

		public void OnDeviceReset()
		{
			if (m_deviceState == MyGeneralManagerState.NOT_INIT)
			{
				return;
			}
			foreach (IManager allManager in m_allManagers)
			{
				if (allManager is IManagerDevice)
				{
					((IManagerDevice)allManager).OnDeviceReset();
				}
			}
		}

		public void OnDeviceEnd()
		{
			if (m_deviceState == MyGeneralManagerState.NOT_INIT)
			{
				return;
			}
<<<<<<< HEAD
			foreach (IManager item in Enumerable.Reverse(m_allManagers))
=======
			foreach (IManager item in Enumerable.Reverse<IManager>((IEnumerable<IManager>)m_allManagers))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (item is IManagerDevice)
				{
					((IManagerDevice)item).OnDeviceEnd();
				}
			}
			m_deviceState = MyGeneralManagerState.NOT_INIT;
		}

		public void OnUnloadData()
		{
<<<<<<< HEAD
			foreach (IManager item in Enumerable.Reverse(m_allManagers))
=======
			foreach (IManager item in Enumerable.Reverse<IManager>((IEnumerable<IManager>)m_allManagers))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (item is IManagerUnloadData)
				{
					((IManagerUnloadData)item).OnUnloadData();
				}
			}
		}

		public void OnFrameEnd()
		{
			foreach (IManager allManager in m_allManagers)
			{
				if (allManager is IManagerFrameEnd)
				{
					((IManagerFrameEnd)allManager).OnFrameEnd();
				}
			}
		}

		public void OnUpdate()
		{
			foreach (IManager allManager in m_allManagers)
			{
				if (allManager is IManagerUpdate)
				{
					((IManagerUpdate)allManager).OnUpdate();
				}
			}
		}
	}
}
