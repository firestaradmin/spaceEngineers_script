using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
=======
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using VRage;
using VRage.Game;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

namespace Sandbox.Game.GameSystems
{
	public class MyGridWeaponSystem : IMyGridWeaponSystem
	{
		public struct EventArgs
		{
			public IMyGunObject<MyDeviceBase> Weapon;
		}

		private Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> m_gunsByDefId;

		private Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> m_gunsToolbarUsableByDefId;

		public event Action<MyGridWeaponSystem, EventArgs> WeaponRegistered;

		public event Action<MyGridWeaponSystem, EventArgs> WeaponUnregistered;

		public MyGridWeaponSystem()
		{
			m_gunsByDefId = new Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>>(5, MyDefinitionId.Comparer);
			m_gunsToolbarUsableByDefId = new Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>>(5, MyDefinitionId.Comparer);
		}

		public IMyGunObject<MyDeviceBase> GetGun(MyDefinitionId defId)
		{
			if (m_gunsByDefId.ContainsKey(defId))
			{
				return Enumerable.FirstOrDefault<IMyGunObject<MyDeviceBase>>((IEnumerable<IMyGunObject<MyDeviceBase>>)m_gunsByDefId[defId]);
			}
			return null;
		}

		public Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> GetGunSets()
		{
			return m_gunsByDefId;
		}

		public Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> GetGunToolbarUsableSets()
		{
			return m_gunsToolbarUsableByDefId;
		}

		public bool HasGunsOfId(MyDefinitionId defId)
		{
			if (m_gunsByDefId.ContainsKey(defId))
			{
				return m_gunsByDefId[defId].get_Count() > 0;
			}
			return false;
		}

		public void Register(IMyGunObject<MyDeviceBase> gun)
		{
			if (!m_gunsByDefId.ContainsKey(gun.DefinitionId))
			{
				m_gunsByDefId.Add(gun.DefinitionId, new HashSet<IMyGunObject<MyDeviceBase>>());
			}
			m_gunsByDefId[gun.DefinitionId].Add(gun);
			if (IsToolbarUsable(gun))
			{
				if (!m_gunsToolbarUsableByDefId.ContainsKey(gun.DefinitionId))
				{
					m_gunsToolbarUsableByDefId.Add(gun.DefinitionId, new HashSet<IMyGunObject<MyDeviceBase>>());
				}
				m_gunsToolbarUsableByDefId[gun.DefinitionId].Add(gun);
			}
			if (this.WeaponRegistered != null)
			{
				this.WeaponRegistered(this, new EventArgs
				{
					Weapon = gun
				});
			}
		}

		public void Unregister(IMyGunObject<MyDeviceBase> gun)
		{
			if (!m_gunsByDefId.ContainsKey(gun.DefinitionId))
			{
<<<<<<< HEAD
				MyDebug.FailRelease(string.Concat("definition ID ", gun.DefinitionId, " not in m_gunsByDefId"), "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\GameSystems\\MyGridWeaponSystem.cs", 96);
=======
				MyDebug.FailRelease(string.Concat("definition ID ", gun.DefinitionId, " not in m_gunsByDefId"), "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\GameSystems\\MyGridWeaponSystem.cs", 81);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			m_gunsByDefId[gun.DefinitionId].Remove(gun);
			if (IsToolbarUsable(gun))
			{
				m_gunsToolbarUsableByDefId[gun.DefinitionId].Remove(gun);
			}
			if (this.WeaponUnregistered != null)
			{
				this.WeaponUnregistered(this, new EventArgs
				{
					Weapon = gun
				});
			}
		}

		public HashSet<IMyGunObject<MyDeviceBase>> GetGunsById(MyDefinitionId gunId)
		{
			if (m_gunsByDefId.ContainsKey(gunId))
			{
				return m_gunsByDefId[gunId];
			}
			return null;
		}

		public HashSet<IMyGunObject<MyDeviceBase>> GetGunsToolbarUsableById(MyDefinitionId gunId)
		{
			if (m_gunsToolbarUsableByDefId.ContainsKey(gunId))
			{
				return m_gunsToolbarUsableByDefId[gunId];
			}
			return null;
		}

		internal IMyGunObject<MyDeviceBase> GetGunWithAmmo(MyDefinitionId gunId, long shooter)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (!m_gunsByDefId.ContainsKey(gunId))
			{
				return null;
			}
			IMyGunObject<MyDeviceBase> result = Enumerable.FirstOrDefault<IMyGunObject<MyDeviceBase>>((IEnumerable<IMyGunObject<MyDeviceBase>>)m_gunsByDefId[gunId]);
			Enumerator<IMyGunObject<MyDeviceBase>> enumerator = m_gunsByDefId[gunId].GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (item.CanShoot(MyShootActionEnum.PrimaryAction, shooter, out var _))
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					IMyGunObject<MyDeviceBase> current = enumerator.get_Current();
					if (current.CanShoot(MyShootActionEnum.PrimaryAction, shooter, out var _))
					{
						return current;
					}
				}
				return result;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private bool IsToolbarUsable(IMyGunObject<MyDeviceBase> gun)
		{
			return gun.IsToolbarUsable();
		}
	}
}
