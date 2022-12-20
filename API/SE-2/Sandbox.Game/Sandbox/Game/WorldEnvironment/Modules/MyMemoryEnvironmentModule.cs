<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyMemoryEnvironmentModule : IMyEnvironmentModule
	{
		private MyLogicalEnvironmentSectorBase m_sector;

		private readonly HashSet<int> m_disabledItems = new HashSet<int>();

		public bool NeedToSave => m_disabledItems.get_Count() > 0;

		public void ProcessItems(Dictionary<short, MyLodEnvironmentItemSet> items, int changedLodMin, int changedLodMax)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<int> enumerator = m_disabledItems.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					m_sector.InvalidateItem(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
		{
			if (ob != null)
			{
				m_disabledItems.UnionWith((IEnumerable<int>)((MyObjectBuilder_DummyEnvironmentModule)ob).DisabledItems);
			}
			m_sector = sector;
		}

		public void Close()
		{
		}

		public MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder()
		{
			if (m_disabledItems.get_Count() > 0)
			{
				return new MyObjectBuilder_DummyEnvironmentModule
				{
					DisabledItems = m_disabledItems
				};
			}
			return null;
		}

		public void OnItemEnable(int itemId, bool enabled)
		{
			if (enabled)
			{
				m_disabledItems.Remove(itemId);
			}
			else
			{
				m_disabledItems.Add(itemId);
			}
			m_sector.InvalidateItem(itemId);
		}

		public void HandleSyncEvent(int logicalItem, object data, bool fromClient)
		{
		}

		public void DebugDraw()
		{
		}
	}
}
