using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ShipWelder))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipWelder),
		typeof(Sandbox.ModAPI.Ingame.IMyShipWelder)
	})]
	public class MyShipWelder : MyShipToolBase, Sandbox.ModAPI.IMyShipWelder, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyShipWelder
	{
		protected class m_helpOthers_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType helpOthers;
				ISyncType result = (helpOthers = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipWelder)P_0).m_helpOthers = (Sync<bool, SyncDirection.BothWays>)helpOthers;
				return result;
			}
		}

		private static MySoundPair METAL_SOUND = new MySoundPair("ToolLrgWeldMetal");

		private static MySoundPair IDLE_SOUND = new MySoundPair("ToolLrgWeldIdle");

		private const string PARTICLE_EFFECT = "ShipWelderArc";

		private Sync<bool, SyncDirection.BothWays> m_helpOthers;

		public static readonly float WELDER_AMOUNT_PER_SECOND = 4f;

		public static readonly float WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED = 0.6f;

		private Dictionary<string, int> m_missingComponents;

		private List<MyWelder.ProjectionRaycastData> m_raycastData = new List<MyWelder.ProjectionRaycastData>();

		private HashSet<MySlimBlock> m_projectedBlock = new HashSet<MySlimBlock>();

		private MyParticleEffect m_particleEffect;

		private MyFlareDefinition m_flare;

		private MyShipWelderDefinition m_welderDef;

		private Matrix m_particleDummyMatrix1;

		public bool HelpOthers
		{
			get
			{
				return m_helpOthers;
			}
			set
			{
				m_helpOthers.Value = value;
			}
		}

		protected override bool CanInteractWithSelf => true;

		public MyShipWelder()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyShipWelder>())
			{
				return;
			}
			base.CreateTerminalControls();
			if (MyFakes.ENABLE_WELDER_HELP_OTHERS)
			{
				MyTerminalControlCheckbox<MyShipWelder> obj = new MyTerminalControlCheckbox<MyShipWelder>("helpOthers", MyCommonTexts.ShipWelder_HelpOthers, MyCommonTexts.ShipWelder_HelpOthers)
				{
					Getter = (MyShipWelder x) => x.HelpOthers,
					Setter = delegate(MyShipWelder x, bool v)
					{
						x.m_helpOthers.Value = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.Init(objectBuilder, cubeGrid);
			m_missingComponents = new Dictionary<string, int>();
			m_welderDef = base.BlockDefinition as MyShipWelderDefinition;
			if (m_welderDef != null)
			{
				if (m_welderDef.Flare != "")
				{
					MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), m_welderDef.Flare);
					m_flare = MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition;
				}
				if (m_welderDef.EmissiveColorPreset == MyStringHash.NullOrEmpty)
				{
					m_welderDef.EmissiveColorPreset = MyStringHash.GetOrCompute("Welder");
				}
			}
			MyObjectBuilder_ShipWelder myObjectBuilder_ShipWelder = (MyObjectBuilder_ShipWelder)objectBuilder;
			m_helpOthers.SetLocalValue(myObjectBuilder_ShipWelder.HelpOthers);
			LoadParticleDummyMatrices();
		}

		private void LoadParticleDummyMatrices()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.ToLower().Contains("particles1"))
				{
					m_particleDummyMatrix1 = dummy.Value.Matrix;
				}
			}
		}

		public override void OnControlAcquired(IMyCharacter owner)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.CubeGrid.Closed && ((base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.ControlSystem != null && base.CubeGrid.GridSystems.ControlSystem.IsLocallyControlled) || (owner != null && MySession.Static.LocalCharacter == owner)))
				{
<<<<<<< HEAD
					IMyCharacter myCharacter;
					if (owner == null)
					{
						IMyCharacter character = base.CubeGrid.GridSystems.ControlSystem.GetController().Player.Character;
						myCharacter = character;
					}
					else
					{
						myCharacter = owner;
					}
					IMyCharacter myCharacter2 = myCharacter;
					if (myCharacter2 != null && myCharacter2.Parent != null && !myCharacter2.Parent.Components.Contains(typeof(MyCasterComponent)))
=======
					MyCharacter myCharacter = ((owner != null) ? owner : base.CubeGrid.GridSystems.ControlSystem.GetController().Player.Character);
					if (myCharacter != null && myCharacter.Parent != null && !myCharacter.Parent.Components.Contains(typeof(MyCasterComponent)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyCasterComponent component = new MyCasterComponent(new MyDrillSensorRayCast(0f, DEFAULT_REACH_DISTANCE, base.BlockDefinition));
						myCharacter2.Parent.Components.Add(component);
						m_controller = (MyCharacter)myCharacter2;
					}
				}
			}, "MyShipWelder::OnControlAcquired");
		}

		public override void OnControlReleased()
		{
			base.OnControlReleased();
			if (m_controller != null && m_controller.Parent != null && m_controller == MySession.Static.LocalCharacter && m_controller.Parent.Components.Contains(typeof(MyCasterComponent)))
			{
				m_controller.Parent.Components.Remove(typeof(MyCasterComponent));
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ShipWelder obj = (MyObjectBuilder_ShipWelder)base.GetObjectBuilderCubeBlock(copy);
			obj.HelpOthers = m_helpOthers;
			return obj;
		}

		public bool IsWithinWorldLimits(Sandbox.ModAPI.IMyProjector projector, string name, int pcuToBuild)
		{
			return IsWithinWorldLimits(projector as MyProjectorBase, name, pcuToBuild);
		}

		/// <summary>
		/// Determines whether the projected grid still fits within block limits set by server after a new block is added
		/// </summary>
		private bool IsWithinWorldLimits(MyProjectorBase projector, string name, int pcuToBuild)
		{
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				return true;
			}
			bool flag = true;
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(base.BuiltBy);
			MyBlockLimits myBlockLimits = null;
			if (myIdentity != null)
			{
				myBlockLimits = myIdentity.BlockLimits;
			}
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && myIdentity != null && MySession.Static.Factions.GetPlayerFaction(myIdentity.IdentityId) == null)
			{
				return false;
			}
			flag &= base.BuiltBy == 0L || base.IDModule.GetUserRelationToOwner(base.BuiltBy) != MyRelationsBetweenPlayerAndBlock.Enemies;
			flag &= projector.BuiltBy == 0L || base.IDModule.GetUserRelationToOwner(projector.BuiltBy) != MyRelationsBetweenPlayerAndBlock.Enemies;
			if (myIdentity != null)
			{
				if (MySession.Static.MaxBlocksPerPlayer > 0)
				{
					flag &= myBlockLimits.BlocksBuilt < myBlockLimits.MaxBlocks;
				}
				if (MySession.Static.TotalPCU != 0)
				{
					flag &= myBlockLimits.PCU >= pcuToBuild;
				}
			}
			flag &= MySession.Static.MaxGridSize == 0 || projector.CubeGrid.BlocksCount < MySession.Static.MaxGridSize;
			short blockTypeLimit = MySession.Static.GetBlockTypeLimit(name);
			if (myIdentity != null && blockTypeLimit > 0)
			{
<<<<<<< HEAD
				flag &= (myBlockLimits.BlockTypeBuilt.TryGetValue(name, out var value) ? value.BlocksBuilt : 0) < blockTypeLimit;
=======
				MyBlockLimits.MyTypeLimitData myTypeLimitData = default(MyBlockLimits.MyTypeLimitData);
				flag &= (myBlockLimits.BlockTypeBuilt.TryGetValue(name, ref myTypeLimitData) ? myTypeLimitData.BlocksBuilt : 0) < blockTypeLimit;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return flag;
		}

		protected override bool Activate(HashSet<MySlimBlock> targets)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			int num = targets.get_Count();
			m_missingComponents.Clear();
			Enumerator<MySlimBlock> enumerator = targets.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					num--;
					continue;
				}
				MyCubeBlockDefinition.PreloadConstructionModels(target.BlockDefinition);
				target.GetMissingComponents(m_missingComponents);
=======
					MySlimBlock current = enumerator.get_Current();
					if (current.IsFullIntegrity || current == SlimBlock)
					{
						num--;
						continue;
					}
					MyCubeBlockDefinition.PreloadConstructionModels(current.BlockDefinition);
					current.GetMissingComponents(m_missingComponents);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			MyInventory inventory = this.GetInventory();
			foreach (KeyValuePair<string, int> missingComponent in m_missingComponents)
			{
				MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_Component), missingComponent.Key);
				if (Math.Max(missingComponent.Value - (int)inventory.GetItemAmount(myDefinitionId), 0) != 0 && Sync.IsServer && base.UseConveyorSystem)
				{
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(myDefinitionId, missingComponent.Value, this, this.GetInventory(), remove: false, calcImmediately: false);
				}
			}
			if (Sync.IsServer)
			{
				float num2 = 0.25f / (float)Math.Min(4, (num <= 0) ? 1 : num);
				enumerator = targets.GetEnumerator();
				try
				{
<<<<<<< HEAD
					if (target2.CubeGrid.Physics == null || !target2.CubeGrid.Physics.Enabled || target2 == SlimBlock)
					{
						continue;
					}
					float num3 = MySession.Static.WelderSpeedMultiplier * WELDER_AMOUNT_PER_SECOND * num2;
					bool? flag2 = target2.ComponentStack.WillFunctionalityRise(num3);
					if (!flag2.HasValue || !flag2.Value || MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, target2.BlockDefinition.BlockPairName, target2.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST))
					{
						if (target2.CanContinueBuild(inventory))
						{
							flag = true;
						}
						target2.MoveItemsToConstructionStockpile(inventory);
						target2.MoveUnneededItemsFromConstructionStockpile(inventory);
						if (target2.HasDeformation || target2.MaxDeformation > 0.0001f || !target2.IsFullIntegrity)
						{
							float maxAllowedBoneMovement = WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED * 250f * 0.001f;
							target2.IncreaseMountLevel(num3, base.OwnerId, inventory, maxAllowedBoneMovement, m_helpOthers, base.IDModule.ShareMode);
=======
					while (enumerator.MoveNext())
					{
						MySlimBlock current3 = enumerator.get_Current();
						if (current3.CubeGrid.Physics == null || !current3.CubeGrid.Physics.Enabled || current3 == SlimBlock)
						{
							continue;
						}
						float num3 = MySession.Static.WelderSpeedMultiplier * WELDER_AMOUNT_PER_SECOND * num2;
						bool? flag2 = current3.ComponentStack.WillFunctionalityRise(num3);
						if (!flag2.HasValue || !flag2.Value || MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, current3.BlockDefinition.BlockPairName, current3.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST))
						{
							if (current3.CanContinueBuild(inventory))
							{
								flag = true;
							}
							current3.MoveItemsToConstructionStockpile(inventory);
							current3.MoveUnneededItemsFromConstructionStockpile(inventory);
							if (current3.HasDeformation || current3.MaxDeformation > 0.0001f || !current3.IsFullIntegrity)
							{
								float maxAllowedBoneMovement = WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED * 250f * 0.001f;
								current3.IncreaseMountLevel(num3, base.OwnerId, inventory, maxAllowedBoneMovement, m_helpOthers, base.IDModule.ShareMode);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			else
			{
				enumerator = targets.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MySlimBlock current4 = enumerator.get_Current();
						if (current4 != SlimBlock && current4.CanContinueBuild(inventory))
						{
							flag = true;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			m_missingComponents.Clear();
			if (!flag && Sync.IsServer)
			{
				MyWelder.ProjectionRaycastData[] array = FindProjectedBlocks();
				MyWelder.ProjectionRaycastData[] array2;
				if (base.UseConveyorSystem)
				{
					array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						MyCubeBlockDefinition.Component[] components = array2[i].hitCube.BlockDefinition.Components;
						if (components != null && components.Length != 0)
						{
							MyDefinitionId id = components[0].Definition.Id;
							base.CubeGrid.GridSystems.ConveyorSystem.PullItem(id, 1, this, inventory, remove: false, calcImmediately: false);
						}
					}
				}
				new HashSet<MyCubeGrid.MyBlockLocation>();
				bool flag3 = MySession.Static.CreativeMode;
				if (MySession.Static.Players.TryGetPlayerId(base.BuiltBy, out var result) && MySession.Static.Players.TryGetPlayerById(result, out var _))
				{
					flag3 |= MySession.Static.CreativeToolsEnabled(Sync.MyId);
				}
				array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					MyWelder.ProjectionRaycastData projectionRaycastData = array2[i];
					if (!IsWithinWorldLimits(projectionRaycastData.cubeProjector, projectionRaycastData.hitCube.BlockDefinition.BlockPairName, flag3 ? projectionRaycastData.hitCube.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST) || (!MySession.Static.CreativeMode && !inventory.ContainItems(1, projectionRaycastData.hitCube.BlockDefinition.Components[0].Definition.Id)))
					{
						continue;
					}
					MyWelder.ProjectionRaycastData invokedBlock = projectionRaycastData;
					MySandboxGame.Static.Invoke(delegate
					{
						if (!invokedBlock.cubeProjector.Closed && !invokedBlock.cubeProjector.CubeGrid.Closed && (invokedBlock.hitCube.FatBlock == null || !invokedBlock.hitCube.FatBlock.Closed))
						{
							invokedBlock.cubeProjector.Build(invokedBlock.hitCube, base.OwnerId, base.EntityId, requestInstant: true, base.BuiltBy);
						}
					}, "ShipWelder BuildProjection");
					flag = true;
				}
			}
			if (flag)
			{
				SetBuildingMusic(150);
			}
			return flag;
		}

		private MyWelder.ProjectionRaycastData[] FindProjectedBlocks()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			BoundingSphereD boundingSphere = new BoundingSphereD(Vector3D.Transform(m_detectorSphere.Center, base.CubeGrid.WorldMatrix), m_detectorSphere.Radius);
			List<MyWelder.ProjectionRaycastData> list = new List<MyWelder.ProjectionRaycastData>();
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			foreach (MyEntity item in entitiesInSphere)
			{
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (myCubeGrid == null || myCubeGrid.Projector == null)
<<<<<<< HEAD
				{
					continue;
				}
				myCubeGrid.GetBlocksInsideSphere(ref boundingSphere, m_projectedBlock);
				foreach (MySlimBlock item2 in m_projectedBlock)
				{
					if (myCubeGrid.Projector.CanBuild(item2, checkHavokIntersections: true) == BuildCheckResult.OK)
					{
						MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(item2.Position);
						if (cubeBlock != null)
						{
							list.Add(new MyWelder.ProjectionRaycastData(BuildCheckResult.OK, cubeBlock, myCubeGrid.Projector));
						}
					}
				}
=======
				{
					continue;
				}
				myCubeGrid.GetBlocksInsideSphere(ref boundingSphere, m_projectedBlock);
				Enumerator<MySlimBlock> enumerator2 = m_projectedBlock.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MySlimBlock current = enumerator2.get_Current();
						if (myCubeGrid.Projector.CanBuild(current, checkHavokIntersections: true) == BuildCheckResult.OK)
						{
							MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(current.Position);
							if (cubeBlock != null)
							{
								list.Add(new MyWelder.ProjectionRaycastData(BuildCheckResult.OK, cubeBlock, myCubeGrid.Projector));
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_projectedBlock.Clear();
			}
			m_projectedBlock.Clear();
			entitiesInSphere.Clear();
			return list.ToArray();
		}

		protected override void StartShooting()
		{
			base.StartShooting();
			SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
		}

		protected override void StopShooting()
		{
			base.StopShooting();
			SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			if (action == MyShootActionEnum.SecondaryAction && MySession.Static.ControlledEntity != null && !Sync.IsDedicated && GetTopMostParent() == MySession.Static.ControlledEntity.Entity.GetTopMostParent() && base.CubeGrid.GridSystems.ControlSystem.GetShipController() != null)
			{
				MySlimBlock mySlimBlock = base.CubeGrid.GridSystems.ControlSystem.GetShipController().RaycasterHitBlock;
				if (mySlimBlock == null)
				{
					MyWelder.ProjectionRaycastData projectionRaycastData = base.CubeGrid.GridSystems.ControlSystem.GetShipController().FindProjectedBlock();
					if (projectionRaycastData.raycastResult == BuildCheckResult.OK)
					{
						mySlimBlock = projectionRaycastData.hitCube;
					}
				}
				MyWelder.AddMissingComponentsToBuildPlanner(mySlimBlock);
			}
			base.EndShoot(action);
		}

		public override bool SetEmissiveStateDamaged()
		{
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		public override bool SetEmissiveStateDisabled()
		{
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		public override bool SetEmissiveStateWorking()
		{
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		protected override void StartEffects()
		{
			Vector3D worldPosition = base.WorldMatrix.Translation;
			Matrix m = m_particleDummyMatrix1 * base.PositionComp.LocalMatrixRef;
			MatrixD effectMatrix = m;
			MyParticlesManager.TryCreateParticleEffect("ShipWelderArc", ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_particleEffect);
		}

		protected override void StopEffects()
		{
			if (m_particleEffect != null)
			{
				m_particleEffect.Stop();
				m_particleEffect = null;
			}
		}

		private Vector3D GetLightPosition()
		{
			return base.WorldMatrix.Translation + base.WorldMatrix.Forward * ((base.CubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2.7f : 1.5f);
		}

		protected override void StopLoopSound()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		protected override void PlayLoopSound(bool activated)
		{
			if (m_soundEmitter != null)
			{
				if (activated)
				{
					m_soundEmitter.PlaySingleSound(METAL_SOUND, stopPrevious: true);
				}
				else
				{
					m_soundEmitter.PlaySingleSound(IDLE_SOUND, stopPrevious: true);
				}
			}
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Welding, shooter, 0uL))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			return base.CanShoot(action, shooter, out status);
		}

		public override PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public override PullInformation GetPushInformation()
		{
			return null;
		}
	}
}
