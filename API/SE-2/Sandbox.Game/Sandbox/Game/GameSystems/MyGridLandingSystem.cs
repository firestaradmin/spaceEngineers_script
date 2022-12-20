<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.Entities.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyGridLandingSystem
	{
		private static readonly int GEAR_MODE_COUNT = MyUtils.GetMaxValueFromEnum<LandingGearMode>() + 1;

		private readonly List<IMyLandingGear> m_gearTmpList = new List<IMyLandingGear>();

		private HashSet<IMyLandingGear>[] m_gearStates;

		private LockModeChangedHandler m_onStateChanged;

		private bool m_isParked;

		public MyMultipleEnabledEnum Locked
		{
			get
			{
				int totalGearCount = TotalGearCount;
				if (totalGearCount == 0)
				{
					return MyMultipleEnabledEnum.NoObjects;
				}
				if (totalGearCount == this[LandingGearMode.Locked])
				{
					return MyMultipleEnabledEnum.AllEnabled;
				}
				if (totalGearCount == this[LandingGearMode.ReadyToLock] + this[LandingGearMode.Unlocked])
				{
					return MyMultipleEnabledEnum.AllDisabled;
				}
				return MyMultipleEnabledEnum.Mixed;
			}
		}

		public int TotalGearCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < GEAR_MODE_COUNT; i++)
				{
					num += m_gearStates[i].get_Count();
				}
				return num;
			}
		}

		public int this[LandingGearMode mode] => m_gearStates[(int)mode].get_Count();

		public bool IsParked => m_isParked;

		public MyGridLandingSystem()
		{
			m_gearStates = new HashSet<IMyLandingGear>[GEAR_MODE_COUNT];
			for (int i = 0; i < GEAR_MODE_COUNT; i++)
			{
				m_gearStates[i] = new HashSet<IMyLandingGear>();
			}
			m_onStateChanged = StateChanged;
		}

		private void StateChanged(IMyLandingGear gear, LandingGearMode oldMode)
		{
			m_gearStates[(int)oldMode].Remove(gear);
			m_gearStates[(int)gear.LockMode].Add(gear);
			UpdateParkedStatus();
		}

		public void Switch()
		{
			if (Locked == MyMultipleEnabledEnum.AllEnabled || Locked == MyMultipleEnabledEnum.Mixed)
			{
				Switch(enabled: false, forceSwitch: false);
			}
			else if (Locked == MyMultipleEnabledEnum.AllDisabled)
			{
				Switch(enabled: true, forceSwitch: false);
			}
		}

		public List<IMyEntity> GetAttachedEntities()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			List<IMyEntity> list = new List<IMyEntity>();
			Enumerator<IMyLandingGear> enumerator = m_gearStates[2].GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyEntity attachedEntity = enumerator.get_Current().GetAttachedEntity();
					if (attachedEntity != null)
					{
						list.Add(attachedEntity);
					}
				}
				return list;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void Switch(bool enabled, bool forceSwitch = true)
		{
<<<<<<< HEAD
			int num = (enabled ? 1 : 2);
			bool flag = !enabled && m_gearStates[2].Count > 0;
			m_gearTmpList.Clear();
			foreach (IMyLandingGear item in m_gearStates[num])
=======
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			int num = (enabled ? 1 : 2);
			bool flag = !enabled && m_gearStates[2].get_Count() > 0;
			Enumerator<IMyLandingGear> enumerator = m_gearStates[num].GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyLandingGear current = enumerator.get_Current();
					m_gearTmpList.Add(current);
				}
			}
			finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				((IDisposable)enumerator).Dispose();
			}
			if (enabled)
			{
				enumerator = m_gearStates[0].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyLandingGear current2 = enumerator.get_Current();
						m_gearTmpList.Add(current2);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			foreach (IMyLandingGear gearTmp in m_gearTmpList)
			{
				if (forceSwitch || gearTmp.IsParkingEnabled)
				{
					gearTmp.RequestLock(enabled);
				}
			}
<<<<<<< HEAD
			if (!flag)
			{
				return;
			}
			HashSet<IMyLandingGear>[] gearStates = m_gearStates;
			for (int i = 0; i < gearStates.Length; i++)
			{
				foreach (IMyLandingGear item3 in gearStates[i])
				{
					if (item3.AutoLock)
					{
						item3.ResetAutolock();
=======
			m_gearTmpList.Clear();
			if (!flag)
			{
				return;
			}
			HashSet<IMyLandingGear>[] gearStates = m_gearStates;
			for (int i = 0; i < gearStates.Length; i++)
			{
				enumerator = gearStates[i].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyLandingGear current3 = enumerator.get_Current();
						if (current3.AutoLock)
						{
							current3.ResetAutolock();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public void Register(IMyLandingGear gear)
		{
			gear.LockModeChanged += m_onStateChanged;
			m_gearStates[(int)gear.LockMode].Add(gear);
			UpdateParkedStatus();
		}

		public void Unregister(IMyLandingGear gear)
		{
			m_gearStates[(int)gear.LockMode].Remove(gear);
			gear.LockModeChanged -= m_onStateChanged;
			UpdateParkedStatus();
		}

		public void UpdateParkedStatus()
		{
			bool isParked = false;
			foreach (IMyLandingGear item in m_gearStates[2])
			{
				if (item.IsParkingEnabled)
				{
					isParked = true;
				}
			}
			m_isParked = isParked;
		}
	}
}
