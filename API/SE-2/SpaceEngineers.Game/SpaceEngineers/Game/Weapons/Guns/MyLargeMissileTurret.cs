using System;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LargeMissileTurret))]
	public class MyLargeMissileTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeMissileTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, IMyTargetingCapableBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeMissileTurret, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyShootOrigin
	{
		private static readonly string DUMMY_NAME_BASE1 = "MissileTurretBase1";

		private static readonly string DUMMY_NAME_BARRELS = "MissileTurretBarrels";

		private static readonly string DUMMY_NAME_BASE1 = "MissileTurretBase1";

		private static readonly string DUMMY_NAME_BARRELS = "MissileTurretBarrels";

		public override IMyMissileGunObject Launcher => this;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_rotationSpeed = 0.00157079648f;
			m_elevationSpeed = 0.00157079648f;
			if (base.BlockDefinition != null)
			{
				m_rotationSpeed = base.BlockDefinition.RotationSpeed;
				m_elevationSpeed = base.BlockDefinition.ElevationSpeed;
			}
			if (m_gunBase.HasAmmoMagazines)
			{
				m_shootingCueEnum = m_gunBase.ShootSound;
			}
			m_rotatingCueEnum.Init("WepTurretGatlingRotate");
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				if (base.Subparts.ContainsKey(DUMMY_NAME_BASE1))
				{
					m_base1 = base.Subparts[DUMMY_NAME_BASE1];
					m_base2 = m_base1.Subparts[DUMMY_NAME_BARRELS];
					m_barrel = new MyLargeMissileBarrel();
					((MyLargeMissileBarrel)m_barrel).Init(m_base2, this);
					GetCameraDummy();
				}
			}
			else
			{
				m_base1 = null;
				m_base2 = null;
				m_barrel = null;
			}
			ResetRotation();
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction && m_barrel != null)
			{
				m_barrel.StartShooting();
			}
		}

<<<<<<< HEAD
=======
		public new void MissileShootEffect()
		{
			if (m_barrel != null)
			{
				m_barrel.ShootEffect();
			}
		}

		public new void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeMissileTurret x) => x.OnShootMissile, builder);
		}

		[Event(null, 101)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Add(builder);
		}

		public new void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyLargeMissileTurret x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 114)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Remove(entityId);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void UpdateAfterSimulationParallel()
		{
			if (!MySession.Static.WeaponsEnabled)
			{
				RotateModels();
				return;
			}
			base.UpdateAfterSimulationParallel();
			DrawLasers();
		}

		public override void ShootFromTerminal(Vector3 direction)
		{
			if (m_barrel != null)
			{
				base.ShootFromTerminal(direction);
				base.IsControlled = true;
				m_barrel.StartShooting();
				base.IsControlled = false;
			}
		}
	}
}
