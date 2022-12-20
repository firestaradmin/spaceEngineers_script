using System;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.Renders;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Serialization;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LargeGatlingTurret))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret)
	})]
	public class MyLargeGatlingTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, IMyTargetingCapableBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret
	{
<<<<<<< HEAD
		public static string DUMMY_NAME_BASE1 = "Base1";

		public static string DUMMY_NAME_BASE2 = "Base2";
=======
		public int Burst { get; private set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public static string DUMMY_NAME_BARREL = "Barrel";

		public int Burst { get; private set; }

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			if (m_gunBase.HasAmmoMagazines)
			{
				m_shootingCueEnum = m_gunBase.ShootSound;
			}
			m_rotatingCueEnum.Init("WepTurretGatlingRotate");
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction)
			{
				m_gunBase.Shoot(base.Parent.Physics.LinearVelocity);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				if (base.BlockDefinition.SubpartPairing != null)
				{
					m_base1 = GetTurretSubpart(base.BlockDefinition.SubpartPairing, DUMMY_NAME_BASE1);
					m_base2 = GetTurretSubpart(base.BlockDefinition.SubpartPairing, DUMMY_NAME_BASE2);
					m_barrel = new MyLargeGatlingBarrel();
					((MyLargeGatlingBarrel)m_barrel).Init(GetTurretSubpart(base.BlockDefinition.SubpartPairing, DUMMY_NAME_BARREL), this);
					GetCameraDummy();
				}
				else
				{
					m_base1 = base.Subparts["GatlingTurretBase1"];
					m_base2 = m_base1.Subparts["GatlingTurretBase2"];
					m_barrel = new MyLargeGatlingBarrel();
					MyEntity entity = null;
					if (m_base2.Subparts.TryGetValue("GatlingBarrel", out var value))
					{
						entity = value;
					}
					else if (m_base2 != null)
					{
						entity = m_base2;
					}
					else if (m_base1 != null)
					{
						entity = m_base1;
					}
					((MyLargeGatlingBarrel)m_barrel).Init(entity, this);
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

<<<<<<< HEAD
		public MyEntitySubpart GetTurretSubpart(SerializableDictionary<string, string> pairing, string key)
		{
			if (!pairing.Dictionary.TryGetValue(key, out var value))
			{
				return null;
			}
			string[] array = value.Split(new char[1] { '/' });
			MyEntity myEntity = this;
			string[] array2 = array;
			foreach (string key2 in array2)
			{
				if (!myEntity.Subparts.TryGetValue(key2, out var value2))
				{
					return null;
				}
				myEntity = value2;
			}
			return myEntity as MyEntitySubpart;
		}

=======
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

		public MyLargeGatlingTurret()
		{
			base.Render = new MyRenderComponentLargeTurret();
		}
	}
}
