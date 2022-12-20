using System;
<<<<<<< HEAD
using Sandbox;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.Renders;
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
	[MyCubeBlockType(typeof(MyObjectBuilder_InteriorTurret))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyLargeInteriorTurret),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyLargeInteriorTurret)
	})]
	public class MyLargeInteriorTurret : MyLargeTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeInteriorTurret, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, IMyTargetingCapableBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeInteriorTurret
	{
<<<<<<< HEAD
		private long m_nextShotAt;
=======
		public int Burst { get; private set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private bool m_shouldBeFlashVisible;

		public int Burst { get; private set; }

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			if (m_gunBase.HasAmmoMagazines)
			{
				m_shootingCueEnum = m_gunBase.ShootSound;
			}
			m_rotatingCueEnum.Init("WepTurretInteriorRotate");
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction)
			{
				m_gunBase.Shoot(Vector3.Zero);
			}
		}

		public override void ShootFromTerminal(Vector3 direction)
		{
			base.ShootFromTerminal(direction);
			m_shouldBeFlashVisible = true;
			m_nextShotAt = MySandboxGame.TotalGamePlayTimeInMilliseconds + (int)m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				if (!base.Subparts.TryGetValue("InteriorTurretBase1", out var value))
				{
					return;
				}
				m_base1 = value;
				if (!m_base1.Subparts.TryGetValue("InteriorTurretBase2", out var value2))
				{
					return;
				}
				m_base2 = value2;
				m_barrel = new MyLargeInteriorBarrel();
				((MyLargeInteriorBarrel)m_barrel).Init(m_base2, this);
				GetCameraDummy();
			}
			else
			{
				m_base1 = null;
				m_base2 = null;
				m_barrel = null;
			}
			ResetRotation();
		}

		public override void UpdateAfterSimulationParallel()
		{
			if (!MySession.Static.WeaponsEnabled)
			{
				RotateModels();
				return;
			}
<<<<<<< HEAD
			if (m_shouldBeFlashVisible && m_nextShotAt < MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				m_shouldBeFlashVisible = false;
				m_barrel.StopShooting();
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.UpdateAfterSimulationParallel();
			DrawLasers();
		}

		public MyLargeInteriorTurret()
		{
			base.Render = new MyRenderComponentLargeTurret();
		}
	}
}
