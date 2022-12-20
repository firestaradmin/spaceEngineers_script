using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Collections;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRageMath;

namespace VRage.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_RespawnComponent), true)]
	public class MyRespawnComponent : MyEntityRespawnComponentBase
	{
		private static HashSet<MyRespawnComponent> m_respawns = new HashSet<MyRespawnComponent>();

		public new MyTerminalBlock Entity => (MyTerminalBlock)base.Entity;

		public bool SpawnWithoutOxygen => (Entity as MyMedicalRoom)?.SpawnWithoutOxygenEnabled ?? true;

		public override string ComponentTypeDebugString => "Respawn";

		public static HashSetReader<MyRespawnComponent> GetAllRespawns()
		{
			return m_respawns;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (base.Container.Entity.InScene)
			{
				MyTerminalBlock entity = Entity;
				if (entity != null && entity.CubeGrid?.IsPreview == false)
				{
					m_respawns.Add(this);
				}
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			m_respawns.Remove(this);
			base.OnBeforeRemovedFromContainer();
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			MyTerminalBlock entity = Entity;
			if (entity == null || entity.CubeGrid?.IsPreview != true)
			{
				m_respawns.Add(this);
			}
		}

		public override void OnRemovedFromScene()
		{
			m_respawns.Remove(this);
			base.OnRemovedFromScene();
		}

		public MatrixD GetSpawnPosition()
		{
			MatrixD matrixD = MatrixD.Identity;
			if (Entity.Model.Dummies.TryGetValue("dummy detector_respawn", out var value))
			{
				matrixD = value.Matrix;
			}
			return MatrixD.Multiply(MatrixD.CreateTranslation(matrixD.Translation), Entity.WorldMatrix);
		}

		public float GetOxygenLevel()
		{
			if (!MySession.Static.Settings.EnableOxygen)
			{
				return 0f;
			}
			MyTerminalBlock entity = Entity;
			MyCubeGrid cubeGrid = entity.CubeGrid;
			MyGridGasSystem gasSystem = cubeGrid.GridSystems.GasSystem;
			if (gasSystem == null)
			{
				return 0f;
			}
			MyOxygenBlock oxygenBlock = gasSystem.GetOxygenBlock(entity.WorldMatrix.Translation);
			if (oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight)
			{
				return 0f;
			}
			return oxygenBlock.OxygenLevel(cubeGrid.GridSize);
		}

		public bool CanPlayerSpawn(long playerId, bool acceptPublicRespawn)
		{
			MyTerminalBlock entity = Entity;
			if (entity.HasPlayerAccess(playerId))
			{
				if (acceptPublicRespawn)
				{
					return true;
				}
				MyIDModule iDModule = entity.IDModule;
<<<<<<< HEAD
				MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(iDModule.Owner, playerId, iDModule.ShareMode, MyRelationsBetweenPlayerAndBlock.Enemies, MyRelationsBetweenFactions.Enemies, MyRelationsBetweenPlayerAndBlock.Enemies);
=======
				MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(iDModule.Owner, playerId, iDModule.ShareMode, MyRelationsBetweenPlayerAndBlock.Enemies, MyRelationsBetweenFactions.Enemies, acceptPublicRespawn ? MyRelationsBetweenPlayerAndBlock.FactionShare : MyRelationsBetweenPlayerAndBlock.Enemies);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.Owner || relationPlayerBlock == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					return true;
				}
			}
			if (acceptPublicRespawn)
			{
				MyMedicalRoom myMedicalRoom = entity as MyMedicalRoom;
				if (myMedicalRoom != null && myMedicalRoom.SetFactionToSpawnee)
				{
					return true;
				}
			}
			return false;
		}
	}
}
