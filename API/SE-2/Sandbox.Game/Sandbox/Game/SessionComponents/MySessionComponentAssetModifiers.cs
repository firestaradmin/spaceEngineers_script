using System.Collections.Generic;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 999, typeof(MyObjectBuilder_SessionComponentAssetModifiers), null, false)]
	public class MySessionComponentAssetModifiers : MySessionComponentBase
	{
		public static readonly byte[] INVALID_CHECK_DATA = new byte[1] { 255 };

		private List<MyAssetModifierComponent> m_componentListForLazyUpdates = new List<MyAssetModifierComponent>();

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			int num = 0;
			while (num < m_componentListForLazyUpdates.Count)
			{
				bool flag = true;
				MyAssetModifierComponent myAssetModifierComponent = m_componentListForLazyUpdates[num];
				if (myAssetModifierComponent?.Entity != null && !myAssetModifierComponent.Entity.Closed && !myAssetModifierComponent.Entity.MarkedForClose)
				{
					flag = myAssetModifierComponent.LazyUpdate();
				}
				if (flag)
				{
					m_componentListForLazyUpdates.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}

		public void RegisterComponentForLazyUpdate(MyAssetModifierComponent comp)
		{
			lock (m_componentListForLazyUpdates)
			{
				m_componentListForLazyUpdates.Add(comp);
			}
		}
	}
}
