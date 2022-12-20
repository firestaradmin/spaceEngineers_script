<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using VRage;

namespace Sandbox.Game.GameSystems
{
	public class MyGridReflectorLightSystem
	{
		private HashSet<MyReflectorLight> m_reflectors;

		public bool IsClosing;

		private MyMultipleEnabledEnum m_reflectorsEnabled;

		private bool m_reflectorsEnabledNeedsRefresh;

		private MyCubeGrid m_grid;

		public int ReflectorCount => m_reflectors.get_Count();

		public MyMultipleEnabledEnum ReflectorsEnabled
		{
			get
			{
				if (m_reflectorsEnabledNeedsRefresh)
				{
					RefreshReflectorsEnabled();
				}
				return m_reflectorsEnabled;
			}
			set
			{
				if (m_reflectorsEnabled != value && m_reflectorsEnabled != 0 && !IsClosing)
				{
					m_grid.SendReflectorState(value);
				}
			}
		}

		public MyGridReflectorLightSystem(MyCubeGrid grid)
		{
			m_reflectors = new HashSet<MyReflectorLight>();
			m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
			m_grid = grid;
		}

		public void ReflectorStateChanged(MyMultipleEnabledEnum enabledState)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			m_reflectorsEnabled = enabledState;
			if (!Sync.IsServer)
			{
				return;
<<<<<<< HEAD
			}
			bool enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
			foreach (MyReflectorLight reflector in m_reflectors)
			{
				reflector.EnabledChanged -= reflector_EnabledChanged;
				reflector.Enabled = enabled;
				reflector.EnabledChanged += reflector_EnabledChanged;
=======
			}
			bool enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
			Enumerator<MyReflectorLight> enumerator = m_reflectors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyReflectorLight current = enumerator.get_Current();
					current.EnabledChanged -= reflector_EnabledChanged;
					current.Enabled = enabled;
					current.EnabledChanged += reflector_EnabledChanged;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_reflectorsEnabledNeedsRefresh = false;
		}

		public void Register(MyReflectorLight reflector)
		{
			m_reflectors.Add(reflector);
			reflector.EnabledChanged += reflector_EnabledChanged;
			if (m_reflectors.get_Count() == 1)
			{
				m_reflectorsEnabled = ((!reflector.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			}
			else if ((ReflectorsEnabled == MyMultipleEnabledEnum.AllEnabled && !reflector.Enabled) || (ReflectorsEnabled == MyMultipleEnabledEnum.AllDisabled && reflector.Enabled))
			{
				m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
			}
		}

		public void Unregister(MyReflectorLight reflector)
		{
			m_reflectors.Remove(reflector);
			reflector.EnabledChanged -= reflector_EnabledChanged;
			if (m_reflectors.get_Count() == 0)
			{
				m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
			}
			else if (m_reflectors.get_Count() == 1)
			{
				m_reflectorsEnabled = ((!Enumerable.First<MyReflectorLight>((IEnumerable<MyReflectorLight>)m_reflectors).Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
			}
			else if (ReflectorsEnabled == MyMultipleEnabledEnum.Mixed)
			{
				m_reflectorsEnabledNeedsRefresh = true;
			}
		}

		private void RefreshReflectorsEnabled()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			m_reflectorsEnabledNeedsRefresh = false;
			if (!Sync.IsServer)
			{
				return;
			}
			bool flag = true;
			bool flag2 = true;
<<<<<<< HEAD
			foreach (MyReflectorLight reflector in m_reflectors)
			{
				flag = flag && reflector.Enabled;
				flag2 = flag2 && !reflector.Enabled;
				if (!flag && !flag2)
				{
					m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
					return;
				}
			}
=======
			Enumerator<MyReflectorLight> enumerator = m_reflectors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyReflectorLight current = enumerator.get_Current();
					flag = flag && current.Enabled;
					flag2 = flag2 && !current.Enabled;
					if (!flag && !flag2)
					{
						m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
						return;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ReflectorsEnabled = ((!flag) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
		}

		private void reflector_EnabledChanged(MyTerminalBlock obj)
		{
			m_reflectorsEnabledNeedsRefresh = true;
		}
	}
}
