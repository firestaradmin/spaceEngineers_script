<<<<<<< HEAD
using Sandbox.Game.EntityComponents;
using VRage;
=======
using System;
using System.Collections.Generic;
using Sandbox.Game.EntityComponents;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridResourceDistributorSystem : MyResourceDistributorComponent
	{
		private class Sandbox_Game_Entities_Cube_MyGridResourceDistributorSystem_003C_003EActor
		{
		}

		private readonly MyGridLogicalGroupData m_gridLogicalGroupData;

		private bool m_scheduled;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGridResourceDistributorSystem(string debugName, MyGridLogicalGroupData gridLogicalGroupData)
			: base(debugName)
		{
			m_gridLogicalGroupData = gridLogicalGroupData;
		}

		public override void UpdateBeforeSimulation100()
		{
<<<<<<< HEAD
=======
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.UpdateBeforeSimulation100();
			if (!m_scheduled)
			{
				return;
			}
			bool flag = false;
<<<<<<< HEAD
			foreach (MyDefinitionId initializedType in m_initializedTypes)
			{
				MyResourceStateEnum state = ResourceStateByType(initializedType);
				flag |= PowerStateWorks(state);
=======
			Enumerator<MyDefinitionId> enumerator = m_initializedTypes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDefinitionId current = enumerator.get_Current();
					MyResourceStateEnum state = ResourceStateByType(current);
					flag |= PowerStateWorks(state);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (!flag)
			{
				DeSchedule();
			}
		}

		public void Schedule(MyCubeGrid root = null)
		{
			if (!m_scheduled)
			{
				root = root ?? m_gridLogicalGroupData.Root;
				if (root != null)
				{
					root.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, base.UpdateBeforeSimulation, 14, parallel: true);
					m_scheduled = true;
				}
			}
		}

		protected void DeSchedule(MyCubeGrid root = null)
		{
			root = root ?? m_gridLogicalGroupData.Root;
			root.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, base.UpdateBeforeSimulation);
			m_scheduled = false;
		}

		public void OnRootChanged(MyCubeGrid oldRoot, MyCubeGrid newRoot)
		{
			if (m_scheduled)
			{
				DeSchedule(oldRoot);
				if (newRoot != null)
				{
					Schedule(newRoot);
				}
			}
		}
	}
}
