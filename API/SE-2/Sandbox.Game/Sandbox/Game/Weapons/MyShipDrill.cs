using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Drill))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipDrill),
		typeof(Sandbox.ModAPI.Ingame.IMyShipDrill)
	})]
	public class MyShipDrill : MyFunctionalBlock, IMyGunObject<MyToolBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyShipToolBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		public class MyDrillHead : MyEntitySubpart
		{
			private class Sandbox_Game_Weapons_MyShipDrill_003C_003EMyDrillHead_003C_003EActor
			{
			}

			public MyShipDrill DrillParent;

			public new MyShipDrillRenderComponent.MyDrillHeadRenderComponent Render => (MyShipDrillRenderComponent.MyDrillHeadRenderComponent)base.Render;

			public MyDrillHead(MyShipDrill parent)
			{
				DrillParent = parent;
				base.Render = new MyShipDrillRenderComponent.MyDrillHeadRenderComponent();
				base.InvalidateOnMove = false;
				base.NeedsWorldMatrix = false;
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipDrill)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_MyShipDrill_003C_003EActor : IActivator, IActivator<MyShipDrill>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipDrill();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipDrill CreateInstance()
			{
				return new MyShipDrill();
			}

			MyShipDrill IActivator<MyShipDrill>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const float HEAD_MAX_ROTATION_SPEED = (float)Math.PI * 4f;

		public const float HEAD_SLOWDOWN_TIME_IN_SECONDS = 0.5f;

		public const float DRILL_RANGE_SQ = 0.960400045f;

		private static int m_countdownDistributor;

		private int m_blockLength;

		private float m_cubeSideLength;

		private MyDefinitionId m_defId;

		private int m_headLastUpdateTime;

		private bool m_isControlled;

		private MyDrillBase m_drillBase;

		private int m_drillFrameCountdown = 90;

		private bool m_wantsToDrill;

		private bool m_isManuallyActivated;

		private bool m_wantsToCollect;

		private MyDrillHead m_drillHeadEntity;

		private MyCharacter m_owner;

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private IMyConveyorEndpoint m_multilineConveyorEndpoint;

<<<<<<< HEAD
		/// <summary>
		/// Id of this drill in the parent ship.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal int ShipDrillId = -1;

		private float m_drillMultiplier = 1f;

		private float m_powerConsumptionMultiplier = 1f;

		public override bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (!m_isManuallyActivated)
				{
					m_enabled.Value = value;
				}
			}
		}

		private bool WantsToDrill
		{
			get
			{
				return m_wantsToDrill;
			}
			set
			{
				m_wantsToDrill = value;
				WantstoDrillChanged();
			}
		}

		public MyDrillHead DrillHeadEntity => m_drillHeadEntity;

		public bool IsDeconstructor => false;

		public MyCharacter Owner => m_owner;

		public float BackkickForcePerSecond => 0f;

		public float ShakeAmount { get; protected set; }

		public bool EnabledInWorldRules => true;

		public new MyDefinitionId DefinitionId => m_defId;

		public bool IsSkinnable => false;

		public bool IsTargetLockingCapable => false;

		public bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_multilineConveyorEndpoint;

		public bool IsShooting => m_drillBase.IsDrilling;

		int IMyGunObject<MyToolBase>.ShootDirectionUpdateTime => 0;

<<<<<<< HEAD
		/// <inheritdoc />
		bool IMyGunObject<MyToolBase>.NeedsShootDirectionWhileAiming => false;

		/// <inheritdoc />
=======
		bool IMyGunObject<MyToolBase>.NeedsShootDirectionWhileAiming => false;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float IMyGunObject<MyToolBase>.MaximumShotLength => 0f;

		public MyToolBase GunBase => null;

		bool Sandbox.ModAPI.Ingame.IMyShipToolBase.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipToolBase.IsActivated => WantsToDrill;

		float Sandbox.ModAPI.IMyShipDrill.DrillHarvestMultiplier
		{
			get
			{
				return m_drillMultiplier;
			}
			set
			{
				m_drillMultiplier = value;
				if (m_drillBase != null)
				{
					m_drillBase.VoxelHarvestRatio = 0.009f * m_drillMultiplier * MySession.Static.Settings.HarvestRatioMultiplier;
					m_drillBase.VoxelHarvestRatio = MathHelper.Clamp(m_drillBase.VoxelHarvestRatio, 0f, 1f);
				}
			}
		}

		float Sandbox.ModAPI.IMyShipDrill.PowerConsumptionMultiplier
		{
			get
			{
				return m_powerConsumptionMultiplier;
			}
			set
			{
				m_powerConsumptionMultiplier = value;
				if (m_powerConsumptionMultiplier < 0.01f)
				{
					m_powerConsumptionMultiplier = 0.01f;
				}
				if (base.ResourceSink != null)
				{
					base.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, ComputeMaxRequiredPower() * m_powerConsumptionMultiplier);
					base.ResourceSink.Update();
					SetDetailedInfoDirty();
					RaisePropertiesChanged();
				}
			}
		}

		public MyShipDrill()
		{
			base.Render = new MyShipDrillRenderComponent();
			CreateTerminalControls();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			SetupDrillFrameCountdown();
			base.NeedsWorldMatrix = true;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyShipDrill>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyShipDrill> obj = new MyTerminalControlOnOffSwitch<MyShipDrill>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = (MyShipDrill x) => x.UseConveyorSystem,
					Setter = delegate(MyShipDrill x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			m_defId = builder.GetId();
			MyShipDrillDefinition myShipDrillDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(m_defId) as MyShipDrillDefinition;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(myShipDrillDefinition.ResourceSinkGroup, ComputeMaxRequiredPower(), ComputeRequiredPower, this);
			base.ResourceSink = myResourceSinkComponent;
			m_drillBase = new MyDrillBase(this, "Smoke_DrillDust", "Smoke_DrillDust", "Smoke_DrillDust_Metal", new MyDrillSensorSphere(myShipDrillDefinition.SensorRadius, myShipDrillDefinition.SensorOffset, base.BlockDefinition), new MyDrillCutOut(myShipDrillDefinition.CutOutOffset, myShipDrillDefinition.CutOutRadius), 0.5f, -0.4f, 0.4f, 1f, delegate(float amount, string typeId, string subtypeId)
			{
				if (MyVisualScriptLogicProvider.ShipDrillCollected != null)
				{
					MyVisualScriptLogicProvider.ShipDrillCollected(base.Name, base.EntityId, base.CubeGrid.Name, base.CubeGrid.EntityId, typeId, subtypeId, amount);
				}
			});
			base.Init(builder, cubeGrid);
			m_blockLength = myShipDrillDefinition.Size.Z;
			m_cubeSideLength = MyDefinitionManager.Static.GetCubeSize(myShipDrillDefinition.CubeSize);
			float maxVolume = (float)(myShipDrillDefinition.Size.X * myShipDrillDefinition.Size.Y * myShipDrillDefinition.Size.Z) * m_cubeSideLength * m_cubeSideLength * m_cubeSideLength * 0.5f;
			Vector3 size = new Vector3(myShipDrillDefinition.Size.X, myShipDrillDefinition.Size.Y, (float)myShipDrillDefinition.Size.Z * 0.5f);
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = new MyInventory(maxVolume, size, MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
			}
			this.GetInventory().Constraint = new MyInventoryConstraint(MySpaceTexts.ToolTipItemFilter_AnyOre).AddObjectBuilderType(typeof(MyObjectBuilder_Ore));
			m_drillBase.OutputInventory = this.GetInventory();
			m_drillBase.IgnoredEntities.Add((MyEntity)this);
			m_drillBase.IgnoredEntities.Add((MyEntity)cubeGrid);
			m_drillBase.UpdatePosition(base.WorldMatrix);
			m_wantsToCollect = false;
			AddDebugRenderComponent(new MyDebugRenderCompomentDrawDrillBase(m_drillBase));
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(base.ResourceSink, this));
			MyObjectBuilder_Drill myObjectBuilder_Drill = (MyObjectBuilder_Drill)builder;
			this.GetInventory().Init(myObjectBuilder_Drill.Inventory);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_Drill.UseConveyorSystem);
			SetDetailedInfoDirty();
			m_wantsToDrill = myObjectBuilder_Drill.Enabled;
			base.IsWorkingChanged += OnIsWorkingChanged;
			m_drillBase.m_drillMaterial = MyStringHash.GetOrCompute("ShipDrill");
			m_baseIdleSound = myShipDrillDefinition.PrimarySound;
			m_drillBase.m_idleSoundLoop = m_baseIdleSound;
			m_drillBase.ParticleOffset = myShipDrillDefinition.ParticleOffset;
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void OnEnabledChanged()
		{
			WantsToDrill = Enabled;
			base.OnEnabledChanged();
		}

		private void OnIsWorkingChanged(MyCubeBlock obj)
		{
			WantstoDrillChanged();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			WantstoDrillChanged();
		}

		private void SetupDrillFrameCountdown()
		{
			m_countdownDistributor += 10;
			if (m_countdownDistributor > 10)
			{
				m_countdownDistributor = -10;
			}
			int num = (MyShipMiningSystem.DebugDisable ? m_countdownDistributor : 0);
			m_drillFrameCountdown = 90 + num;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void WantstoDrillChanged()
		{
			base.ResourceSink.Update();
			if ((Enabled || WantsToDrill) && base.IsFunctional && base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				if (CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _))
				{
					if (!m_drillBase.IsDrilling)
					{
						m_drillBase.StartDrillingAnimation(startSound: true);
					}
				}
				else
				{
					m_drillBase.StopDrill();
				}
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				base.CubeGrid.GridSystems.MiningSystem?.AddDrillUpdate(this);
			}
			else
			{
				base.CubeGrid.GridSystems.MiningSystem?.RemoveDrillUpdate(this);
				SetupDrillFrameCountdown();
				m_drillBase.StopDrill();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Drill obj = (MyObjectBuilder_Drill)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		protected override void Closing()
		{
			base.Closing();
			m_drillBase.Close();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			if (m_drillBase != null)
			{
				m_drillBase.UpdatePosition(base.WorldMatrix);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.ResourceSink.Update();
			base.UpdateAfterSimulation100();
			m_drillBase.UpdateSoundEmitter(Vector3.Zero);
			if (Sync.IsServer && base.IsFunctional && (bool)m_useConveyorSystem)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.GetItemsCount() > 0)
				{
					MyGridConveyorSystem.PushAnyRequest(this, inventory);
				}
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			Receiver_IsPoweredChanged();
			base.UpdateBeforeSimulation10();
			if (base.Parent == null || base.Parent.Physics == null)
			{
				return;
			}
			m_drillFrameCountdown -= 10;
			if (m_drillFrameCountdown > 0)
			{
				return;
			}
			m_drillFrameCountdown += 90;
			if (CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _))
			{
				if (m_drillBase.Drill(Enabled || m_wantsToCollect, performCutout: true, assignDamagedMaterial: false, 0.1f))
				{
					ShakeAmount = 1f;
				}
				else
				{
					ShakeAmount = 0.5f;
				}
			}
			else
			{
				ShakeAmount = 0f;
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.IsPreview)
			{
				return;
			}
			m_drillBase.UpdateAfterSimulation();
<<<<<<< HEAD
			if ((WantsToDrill || Enabled) && CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _) && m_drillBase.AnimationMaxSpeedRatio > 0f)
=======
			if ((WantsToDrill || base.Enabled) && CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _) && m_drillBase.AnimationMaxSpeedRatio > 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (CheckPlayerControl() && MySession.Static.CameraController != null && (MySession.Static.CameraController.IsInFirstPersonView || MySession.Static.CameraController.ForceFirstPersonCamera))
				{
					m_drillBase.PerformCameraShake(ShakeAmount);
				}
				if (MySession.Static.EnableToolShake && MyFakes.ENABLE_TOOL_SHAKE && !base.CubeGrid.Physics.IsStatic)
				{
					ApplyShakeForce();
				}
				if (WantsToDrill || Enabled)
				{
					CheckDustEffect();
				}
				if (m_drillHeadEntity != null)
				{
					m_drillHeadEntity.Render.UpdateSpeed((float)Math.PI * 4f);
				}
			}
			else
			{
				if (m_drillHeadEntity != null)
				{
					m_drillHeadEntity.Render.UpdateSpeed(0f);
				}
				if (!base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		/// <summary>
		/// Returns true if the current local player is controlling this drill
		/// </summary>
		/// <returns></returns>
		private bool CheckPlayerControl()
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer == null || localHumanPlayer.Controller == null)
			{
				return false;
			}
			MyCubeBlock myCubeBlock = localHumanPlayer.Controller.ControlledEntity as MyCubeBlock;
			if (myCubeBlock == null || myCubeBlock is MyRemoteControl)
			{
				return false;
			}
			if (myCubeBlock.CubeGrid != base.CubeGrid)
			{
				return false;
			}
			return true;
		}

		private void CheckDustEffect()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || m_drillBase.DustParticles == null || m_drillBase.DustParticles.IsEmittingStopped)
<<<<<<< HEAD
			{
				return;
			}
			float num = float.MaxValue;
			Vector3D vector3D = Vector3D.Zero;
			Vector3D center = m_drillBase.Sensor.Center;
			foreach (KeyValuePair<long, MyDrillSensorBase.DetectionInfo> item in m_drillBase.Sensor.CachedEntitiesInRange)
			{
=======
			{
				return;
			}
			float num = float.MaxValue;
			Vector3D vector3D = Vector3D.Zero;
			Vector3D center = m_drillBase.Sensor.Center;
			foreach (KeyValuePair<long, MyDrillSensorBase.DetectionInfo> item in m_drillBase.Sensor.CachedEntitiesInRange)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyDrillSensorBase.DetectionInfo value = item.Value;
				if (value.Entity is MyVoxelBase)
				{
					float num2 = Vector3.DistanceSquared(value.DetectionPoint, center);
					if (num2 < num)
					{
						num = num2;
						vector3D = value.DetectionPoint;
					}
				}
			}
			if (num != float.MaxValue)
			{
				_ = ref base.PositionComp.WorldMatrixRef;
				_ = (vector3D + center) / 2.0;
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.AppendFormat("\n");
		}

		public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (action != 0 && action != MyShootActionEnum.SecondaryAction)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!base.IsFunctional)
			{
				status = MyGunStatusEnum.NotFunctional;
				return false;
			}
			if (!HasPlayerAccess(shooter))
			{
				status = MyGunStatusEnum.AccessDenied;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Drilling, shooter, 0uL))
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				status = MyGunStatusEnum.OutOfPower;
				return false;
			}
			return true;
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (action == MyShootActionEnum.PrimaryAction || action == MyShootActionEnum.SecondaryAction)
			{
				ShakeAmount = 0.5f;
				m_wantsToCollect = action == MyShootActionEnum.PrimaryAction;
				WantsToDrill = true;
				m_isManuallyActivated = true;
			}
		}

		public Vector3 GetShootDirection()
		{
			return base.WorldMatrix.Forward;
		}

		public void BeginShoot(MyShootActionEnum action)
		{
		}

		public void EndShoot(MyShootActionEnum action)
		{
			WantsToDrill = false;
			m_isManuallyActivated = false;
			base.ResourceSink.Update();
		}

		public void OnControlAcquired(IMyCharacter owner)
		{
			m_owner = (MyCharacter)owner;
			m_isControlled = true;
			if (Owner == MySession.Static.LocalCharacter)
			{
				MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
		}

		public void OnControlReleased()
		{
			m_owner = null;
			m_isControlled = false;
			if (!Enabled)
			{
				m_drillBase.StopDrill();
			}
			if (MySession.Static.TopMostControlledEntity == base.CubeGrid)
			{
				MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			MyHud.BlockInfo.MissingComponentIndex = -1;
			MyHud.BlockInfo.DefinitionId = base.BlockDefinition.Id;
			MyHud.BlockInfo.BlockName = base.BlockDefinition.DisplayNameText;
			MyHud.BlockInfo.SetContextHelp(base.BlockDefinition);
			MyHud.BlockInfo.PCUCost = 0;
			MyHud.BlockInfo.BlockIcons = base.BlockDefinition.Icons;
			MyHud.BlockInfo.BlockIntegrity = 1f;
			MyHud.BlockInfo.CriticalIntegrity = 0f;
			MyHud.BlockInfo.CriticalComponentIndex = 0;
			MyHud.BlockInfo.OwnershipIntegrity = 0f;
			MyHud.BlockInfo.BlockBuiltBy = 0L;
			MyHud.BlockInfo.GridSize = MyCubeSize.Small;
			MyHud.BlockInfo.Components.Clear();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory());
			base.OnDestroy();
		}

		private void ApplyShakeForce(float standbyRotationRatio = 1f)
		{
			MyGridPhysics physics = base.CubeGrid.Physics;
			MyPositionComponentBase positionComp = base.PositionComp;
			if (physics != null && positionComp != null)
			{
				int hashCode = GetHashCode();
				float num = ((base.CubeGrid.GridSizeEnum == MyCubeSize.Small) ? 1f : 5f);
				MatrixD worldMatrix = base.WorldMatrix;
				Vector3D up = worldMatrix.Up;
				Vector3D right = worldMatrix.Right;
				float num2 = (float)MyPerformanceCounter.TicksToMs(MyPerformanceCounter.ElapsedTicks);
				Vector3 zero = Vector3.Zero;
				float num3 = (float)hashCode + num2;
				zero += (Vector3)(up * Math.Sin(num3 * 13.35f / 5f));
				zero += (Vector3)(right * Math.Sin(num3 * 18.154f / 5f));
				zero *= standbyRotationRatio * num * 240f * m_drillBase.AnimationMaxSpeedRatio * m_drillBase.AnimationMaxSpeedRatio;
				physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, zero, positionComp.GetPosition(), null);
			}
		}

		private Vector3D ComputeDrillSensorCenter()
		{
			return base.WorldMatrix.Forward * (m_blockLength - 2) * m_cubeSideLength + base.WorldMatrix.Translation;
		}

		private float ComputeMaxRequiredPower()
		{
			return 0.002f * m_powerConsumptionMultiplier;
		}

		private float ComputeRequiredPower()
		{
<<<<<<< HEAD
			if (!base.IsFunctional || !CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _) || (!Enabled && !WantsToDrill))
=======
			if (!base.IsFunctional || !CanShoot(MyShootActionEnum.PrimaryAction, base.OwnerId, out var _) || (!base.Enabled && !WantsToDrill))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return 1E-06f;
			}
			return base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public int GetTotalAmmunitionAmount()
		{
			return 0;
		}

		public int GetAmmunitionAmount()
		{
			return 0;
		}

		public int GetMagazineAmount()
		{
			return 0;
		}

		public void InitializeConveyorEndpoint()
		{
			m_multilineConveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_multilineConveyorEndpoint));
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			throw new NotImplementedException();
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			m_drillHeadEntity = new MyDrillHead(this);
			m_drillHeadEntity.OnClosing += delegate(MyEntity x)
			{
				MyDrillHead myDrillHead = x as MyDrillHead;
				if (myDrillHead != null && myDrillHead.DrillParent != null)
				{
					myDrillHead.DrillParent.UnregisterHead(myDrillHead);
				}
			};
			return m_drillHeadEntity;
		}

		public void UnregisterHead(MyDrillHead head)
		{
			if (m_drillHeadEntity == head)
			{
				m_drillHeadEntity = null;
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			m_drillBase.IgnoredEntities.Remove((MyEntity)oldGrid);
			m_drillBase.IgnoredEntities.Add((MyEntity)base.CubeGrid);
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_drillHeadEntity != null)
			{
				m_drillHeadEntity.Render.UpdateSpeed(0f);
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.CubeGrid.GridSystems.GetOrCreateMiningSystem().RegisterDrill(this);
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			base.CubeGrid.GridSystems.MiningSystem?.UnRegisterDrill(this);
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public BoundingSphere GetDrillingSphere()
		{
			ref readonly Matrix localMatrixRef = ref base.PositionComp.LocalMatrixRef;
			Vector3 center = localMatrixRef.Translation + localMatrixRef.Forward * m_drillBase.CutOut.CenterOffset;
			float radius = m_drillBase.CutOut.Radius;
			return new BoundingSphere(center, radius);
		}

		public void TryDrillVoxel(MyVoxelBase voxel, Vector3D hitPosition, bool collectOre, bool applyDamagedMaterial)
		{
			base.CubeGrid.GridSystems.MiningSystem.RequestCutOut(this, !collectOre, applyDamagedMaterial, hitPosition, voxel);
		}

		public void OnDrillResults(Dictionary<MyVoxelMaterialDefinition, int> materials, Vector3D hitPosition, bool collectOre)
		{
			m_drillBase.OnDrillResults(materials, hitPosition, collectOre);
		}

		public void SynchronizeWith(MyShipDrill other)
		{
			m_drillFrameCountdown = other.m_drillFrameCountdown;
		}

		public Vector3D GetMuzzlePosition()
		{
			return base.PositionComp.GetPosition();
		}

<<<<<<< HEAD
		public bool IsToolbarUsable()
		{
			return true;
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[SpecialName]
		int IMyInventoryOwner.get_InventoryCount()
		{
			return base.InventoryCount;
		}

		[SpecialName]
		bool IMyInventoryOwner.get_HasInventory()
		{
			return base.HasInventory;
		}
	}
}
