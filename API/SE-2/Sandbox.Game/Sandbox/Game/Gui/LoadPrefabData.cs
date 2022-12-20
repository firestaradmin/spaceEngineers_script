using System;
using ParallelTasks;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public class LoadPrefabData : WorkData
	{
		private MyObjectBuilder_Definitions m_prefab;

		private string m_path;

		private MyGuiBlueprintScreen_Reworked m_blueprintScreen;

		private ulong? m_workshopId;

		private string m_workshopServiceName;

		private MyBlueprintItemInfo m_info;

		public static Action<WorkData> CallLoadPrefab = delegate(WorkData lpd)
		{
			((LoadPrefabData)lpd).CallLoadPrefabInternal();
		};

		public static Action<WorkData> CallLoadWorkshopPrefab = delegate(WorkData lpd)
		{
			((LoadPrefabData)lpd).CallLoadWorkshopPrefabInternal();
		};

		public static Action<WorkData> CallLoadPrefabFromCloud = delegate(WorkData lpd)
		{
			((LoadPrefabData)lpd).CallLoadPrefabFromCloudInternal();
		};

		public static Action<WorkData> CallOnPrefabLoaded = delegate(WorkData lpd)
		{
			((LoadPrefabData)lpd).CallOnPrefabLoadedInternal();
		};

		public MyObjectBuilder_Definitions Prefab => m_prefab;

		public LoadPrefabData(MyObjectBuilder_Definitions prefab, string path, MyGuiBlueprintScreen_Reworked blueprintScreen, ulong? workshopId = null, string workshopServiceName = null)
		{
			m_prefab = prefab;
			m_path = path;
			m_blueprintScreen = blueprintScreen;
			m_workshopId = workshopId;
			m_workshopServiceName = workshopServiceName;
		}

		public LoadPrefabData(MyObjectBuilder_Definitions prefab, MyBlueprintItemInfo info, MyGuiBlueprintScreen_Reworked blueprintScreen)
		{
			m_prefab = prefab;
			m_blueprintScreen = blueprintScreen;
			m_info = info;
		}

		private void CallLoadPrefabInternal()
		{
			m_prefab = MyBlueprintUtils.LoadPrefab(m_path);
		}

		private void CallLoadWorkshopPrefabInternal()
		{
			m_prefab = MyBlueprintUtils.LoadWorkshopPrefab(m_path, m_workshopId, m_workshopServiceName, isOldBlueprintScreen: false);
		}

		private void CallLoadPrefabFromCloudInternal()
		{
			m_prefab = MyBlueprintUtils.LoadPrefabFromCloud(m_info);
		}

		private void CallOnPrefabLoadedInternal()
		{
			if (m_blueprintScreen != null && m_blueprintScreen.State == MyGuiScreenState.OPENED)
			{
				m_blueprintScreen.OnPrefabLoaded(m_prefab);
			}
		}
	}
}
