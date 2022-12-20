using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
using Havok;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SmallGatlingGun))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySmallGatlingGun),
		typeof(Sandbox.ModAPI.Ingame.IMySmallGatlingGun)
	})]
	public class MySmallGatlingGun : MyUserControllableGun, IMyGunObject<MyGunBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallGatlingGun, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallGatlingGun, IMyMissileGunObject, IMyShootOrigin
	{
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallGatlingGun, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallGatlingGun @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MySmallGatlingGun, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallGatlingGun @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		protected class m_lateStartRandom_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lateStartRandom;
				ISyncType result = (lateStartRandom = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MySmallGatlingGun)P_0).m_lateStartRandom = (Sync<int, SyncDirection.FromServer>)lateStartRandom;
				return result;
			}
		}

		protected class m_cachedAmmunitionAmount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cachedAmmunitionAmount;
				ISyncType result = (cachedAmmunitionAmount = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MySmallGatlingGun)P_0).m_cachedAmmunitionAmount = (Sync<int, SyncDirection.FromServer>)cachedAmmunitionAmount;
				return result;
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MySmallGatlingGun)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_MySmallGatlingGun_003C_003EActor : IActivator, IActivator<MySmallGatlingGun>
		{
			private sealed override object CreateInstance()
			{
				return new MySmallGatlingGun();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySmallGatlingGun CreateInstance()
			{
				return new MySmallGatlingGun();
			}

			MySmallGatlingGun IActivator<MySmallGatlingGun>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string BAREL_SUBPART_NAME = "Barrel";

		private const string BAREL_SUBPART_NAME = "Barrel";

		private int m_lastTimeShoot;

		private float m_rotationTimeout;

		private ShootStateEnum currentState;

		private readonly Sync<int, SyncDirection.FromServer> m_lateStartRandom;

		private Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;
<<<<<<< HEAD
=======

		private int m_currentLateStart;

		private float m_muzzleFlashLength;

		private float m_muzzleFlashRadius;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private int m_currentLateStart;

		private MyEntity3DSoundEmitter m_soundEmitterRotor;

		private MyEntity m_barrel;

		private MyGunBase m_gunBase;

		private Vector3D m_targetLocal = Vector3.Zero;

		private MyHudNotification m_safezoneNotification;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private MyEntity[] m_shootIgnoreEntities;

		private float m_inventoryFillFactor = 0.5f;

		public int LastTimeShoot => m_lastTimeShoot;

		public int LateStartRandom => m_lateStartRandom.Value;

		public Vector3D ShootOrigin => GetWeaponMuzzleWorldPosition();

		public new MyWeaponBlockDefinition BlockDefinition => base.BlockDefinition as MyWeaponBlockDefinition;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool IsSkinnable => false;

		public bool IsTargetLockingCapable => true;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount { get; protected set; }

		public float ProjectileCountMultiplier => 0f;

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => BlockDefinition.Id;

		public bool IsShooting => MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds * 2;

		public int ShootDirectionUpdateTime => 0;

<<<<<<< HEAD
		/// <inheritdoc />
		public bool NeedsShootDirectionWhileAiming => false;

		/// <inheritdoc />
=======
		public bool NeedsShootDirectionWhileAiming => false;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float MaximumShotLength => 0f;

		private bool UseConveyorSystem
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

		public MyGunBase GunBase => m_gunBase;

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon => base.Parent;

		MyEntity IMyGunBaseUser.Owner => base.Parent;

		IMyMissileGunObject IMyGunBaseUser.Launcher => this;

		MyInventory IMyGunBaseUser.AmmoInventory => this.GetInventory();

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => default(MyDefinitionId);

		MyInventory IMyGunBaseUser.WeaponInventory => null;

		long IMyGunBaseUser.OwnerId => base.OwnerId;

		string IMyGunBaseUser.ConstraintDisplayName => BlockDefinition.DisplayNameText;

		bool Sandbox.ModAPI.Ingame.IMySmallGatlingGun.UseConveyorSystem => m_useConveyorSystem;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
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

		public MyDefinitionBase GetAmmoDefinition => m_gunBase.CurrentAmmoDefinition;

		public float MaxShootRange => m_gunBase.CurrentAmmoDefinition.MaxTrajectory;

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		public override bool IsStationary()
		{
			return true;
		}

		public override Vector3D GetWeaponMuzzleWorldPosition()
		{
			if (m_gunBase != null)
			{
				UpdateMuzzlePosition();
				return m_gunBase.GetMuzzleWorldPosition();
			}
			return base.GetWeaponMuzzleWorldPosition();
		}

		public MySmallGatlingGun()
		{
			m_shootIgnoreEntities = new MyEntity[1] { this };
			CreateTerminalControls();
			m_lastTimeShoot = -60000;
			m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_gunBase = new MyGunBase();
			if (Sync.IsServer)
			{
				m_gunBase.OnAmmoAmountChanged += OnAmmoAmountChangedOnServer;
			}
			else
			{
				m_cachedAmmunitionAmount.ValueChanged += OnAmmoAmountChangedFromServer;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			AddDebugRenderComponent(new MyDebugRenderComponentSmallGatlingGun(this));
			base.SyncType.Append(m_gunBase);
		}

		private void OnAmmoAmountChangedFromServer(SyncBase obj)
		{
			GunBase.CurrentAmmo = m_cachedAmmunitionAmount.Value;
		}

		private void OnAmmoAmountChangedOnServer()
		{
			if (Sync.IsServer)
			{
				m_cachedAmmunitionAmount.Value = GunBase.CurrentAmmo;
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySmallGatlingGun>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MySmallGatlingGun> obj = new MyTerminalControlOnOffSwitch<MySmallGatlingGun>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = (MySmallGatlingGun x) => x.UseConveyorSystem,
					Setter = delegate(MySmallGatlingGun x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SmallGatlingGun obj = (MyObjectBuilder_SmallGatlingGun)base.GetObjectBuilderCubeBlock(copy);
			obj.GunBase = m_gunBase.GetObjectBuilder();
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_SmallGatlingGun myObjectBuilder_SmallGatlingGun = objectBuilder as MyObjectBuilder_SmallGatlingGun;
			MyWeaponBlockDefinition blockDefinition = BlockDefinition;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			m_soundEmitterRotor = new MyEntity3DSoundEmitter(this);
<<<<<<< HEAD
			if (blockDefinition != null)
			{
				m_inventoryFillFactor = blockDefinition.InventoryFillFactorMin;
=======
			if (myWeaponBlockDefinition != null)
			{
				m_inventoryFillFactor = myWeaponBlockDefinition.InventoryFillFactorMin;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = ((blockDefinition == null) ? new MyInventory(0.064f, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive) : new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive));
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_SmallGatlingGun.Inventory);
			}
			if (blockDefinition != null)
			{
				MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
				myResourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 0.0002f, UpdatePowerInput, this);
				myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
				base.ResourceSink = myResourceSinkComponent;
			}
			m_gunBase.Init(myObjectBuilder_SmallGatlingGun.GunBase, BlockDefinition, this);
			base.Init(objectBuilder, cubeGrid);
			if (Sync.IsServer)
			{
				m_lateStartRandom.Value = MyUtils.GetRandomInt(0, 30);
			}
			base.ResourceSink.Update();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(base.ResourceSink, this));
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_SmallGatlingGun.UseConveyorSystem);
			base.IsWorkingChanged += MySmallGatlingGun_IsWorkingChanged;
			base.NeedsWorldMatrix = false;
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += AmmoInventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= AmmoInventory_ContentsChanged;
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private float UpdatePowerInput()
		{
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
		}

		private void AmmoInventory_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount(forceUpdate: true);
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (m_soundEmitterRotor != null)
			{
				m_soundEmitterRotor.StopSound(forced: true);
			}
			m_gunBase.RemoveAllEffects();
			base.Closing();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		public override void InitComponents()
		{
			base.Render = new MyRenderComponentCubeBlockWithParentedSubpart();
			base.InitComponents();
		}

		protected override MyEntitySubpart InstantiateSubpart(MyModelDummy subpartDummy, ref MyEntitySubpart.Data data)
		{
			MyEntitySubpart myEntitySubpart = base.InstantiateSubpart(subpartDummy, ref data);
			myEntitySubpart.Render = new MyParentedSubpartRenderComponent();
			return myEntitySubpart;
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			if (base.Subparts.TryGetValue("Barrel", out var value))
			{
				m_barrel = value;
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
		}

		private void MySmallGatlingGun_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.IsWorking)
			{
				if (currentState == ShootStateEnum.Continuous)
				{
					StartLoopSound();
				}
			}
			else
			{
				StopLoopSound();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.PositionComp == null)
			{
				return;
			}
<<<<<<< HEAD
			if (!Sync.IsDedicated)
=======
			bool flag = m_flashEffect == null;
			bool flag2 = currentState != ShootStateEnum.Off;
			if (!Sync.IsDedicated && flag == flag2)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
				amount = MathHelper.SmoothStep(0f, 1f, amount);
				float num = amount * ((float)Math.PI * 4f) * 0.0166666675f;
				if (num != 0f && m_barrel != null && m_barrel.PositionComp != null)
				{
<<<<<<< HEAD
					Matrix localMatrix = Matrix.CreateRotationZ(num) * m_barrel.PositionComp.LocalMatrixRef;
					Matrix renderLocal = localMatrix * base.PositionComp.LocalMatrixRef;
					m_barrel.PositionComp.SetLocalMatrix(ref localMatrix, null, updateWorld: true, ref renderLocal);
=======
					MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", base.PositionComp.WorldMatrixRef, out m_flashEffect);
					if (currentState == ShootStateEnum.Once)
					{
						m_smokesToGenerate = 10;
						m_shootOvertime = 5;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) > m_gunBase.ReleaseTimeAfterFire)
				{
					m_gunBase.RemoveOldEffects();
				}
				else
				{
<<<<<<< HEAD
					UpdateMuzzlePosition();
					m_gunBase.UpdateEffects();
=======
					m_shootOvertime--;
				}
			}
			if (currentState == ShootStateEnum.Once)
			{
				currentState = ShootStateEnum.Off;
			}
			if (!Sync.IsDedicated)
			{
				float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
				amount = MathHelper.SmoothStep(0f, 1f, amount);
				float num = amount * ((float)Math.PI * 4f) * 0.0166666675f;
				if (num != 0f && m_barrel != null && m_barrel.PositionComp != null)
				{
					Matrix localMatrix = Matrix.CreateRotationZ(num) * m_barrel.PositionComp.LocalMatrixRef;
					Matrix renderLocal = localMatrix * base.PositionComp.LocalMatrixRef;
					m_barrel.PositionComp.SetLocalMatrix(ref localMatrix, null, updateWorld: true, ref renderLocal);
				}
				if (num == 0f && !base.HasDamageEffect && m_smokeOvertime <= 0 && currentState == ShootStateEnum.Off)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
				else
				{
					UpdateMuzzlePosition();
				}
				SmokesToGenerateDecrease();
				if (m_smokesToGenerate > 0)
				{
					m_smokeOvertime = 120;
					if (MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) < 150.0)
					{
						if (m_smokeEffect == null)
						{
							MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", base.PositionComp.WorldMatrixRef, out m_smokeEffect);
						}
						else if (m_smokeEffect.IsEmittingStopped)
						{
							m_smokeEffect.Play();
							m_smokeEffect.WorldMatrix = base.PositionComp.WorldMatrixRef;
						}
					}
				}
				else
				{
					m_smokeOvertime--;
					if (m_smokeEffect != null && !m_smokeEffect.IsEmittingStopped)
					{
						m_smokeEffect.StopEmitting();
					}
					if (m_flashEffect != null)
					{
						m_flashEffect.Stop();
						m_flashEffect = null;
					}
				}
				if (m_smokeEffect != null)
				{
					m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(GetWeaponMuzzleWorldPosition());
					m_smokeEffect.UserBirthMultiplier = m_smokesToGenerate / 10 * 10;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (num == 0f && !base.HasDamageEffect && currentState == ShootStateEnum.Off && !m_gunBase.HasActiveEffects())
				{
<<<<<<< HEAD
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
=======
					MatrixD worldMatrixRef = base.PositionComp.WorldMatrixRef;
					worldMatrixRef.Translation = GetWeaponMuzzleWorldPosition();
					m_flashEffect.WorldMatrix = worldMatrixRef;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			else if (currentState == ShootStateEnum.Off && !m_isShooting && !m_forceShoot)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (MySession.Static.SurvivalMode && Sync.IsServer && base.IsWorking && (bool)m_useConveyorSystem)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.VolumeFillFactor < m_inventoryFillFactor)
				{
					MyAmmoMagazineDefinition currentAmmoMagazineDefinition = m_gunBase.CurrentAmmoMagazineDefinition;
					if (currentAmmoMagazineDefinition != null)
					{
						MyFixedPoint myFixedPoint = MyFixedPoint.Floor((inventory.MaxVolume - inventory.CurrentVolume) * (1f / currentAmmoMagazineDefinition.Volume));
						if (myFixedPoint == 0)
						{
							return;
						}
						base.CubeGrid.GridSystems.ConveyorSystem.PullItem(m_gunBase.CurrentAmmoMagazineId, myFixedPoint, this, inventory, remove: false, calcImmediately: false);
					}
				}
			}
			if (Sync.IsServer && m_gunBase.CurrentAmmo == 0)
			{
				m_gunBase.ConsumeMagazine();
			}
<<<<<<< HEAD
=======
		}

		private void ClampSmokesToGenerate()
		{
			m_smokesToGenerate = MyUtils.GetClampInt(m_smokesToGenerate, 0, 50);
		}

		private void SmokesToGenerateIncrease()
		{
			m_smokesToGenerate += 19;
			ClampSmokesToGenerate();
		}

		private void SmokesToGenerateDecrease()
		{
			m_smokesToGenerate--;
			ClampSmokesToGenerate();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return base.PositionComp.WorldMatrixRef.Forward;
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			if (action != 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (base.Parent == null || base.Parent.Physics == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!m_gunBase.HasAmmoMagazines)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
			{
				status = MyGunStatusEnum.Cooldown;
				return false;
			}
			if (!HasPlayerAccess(shooter))
			{
				status = MyGunStatusEnum.AccessDenied;
				return false;
			}
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				status = MyGunStatusEnum.OutOfPower;
				return false;
			}
			if (!base.IsFunctional)
			{
				status = MyGunStatusEnum.NotFunctional;
				return false;
			}
			if (!Enabled)
			{
				status = MyGunStatusEnum.Disabled;
				return false;
			}
			if (!MySession.Static.CreativeMode && !m_gunBase.HasEnoughAmmunition())
			{
				status = MyGunStatusEnum.OutOfAmmo;
				return false;
			}
			return true;
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (base.Parent.Physics == null)
			{
				return;
			}
			if (m_shootingBegun && (int)m_lateStartRandom > m_currentLateStart && currentState == ShootStateEnum.Continuous)
			{
				m_currentLateStart++;
				return;
			}
			if (currentState == ShootStateEnum.Off)
			{
				currentState = ShootStateEnum.Once;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			base.Render.NeedsDrawFromParent = true;
			PlayShotSound();
			UpdateMuzzlePosition();
			m_gunBase.Shoot(base.Parent.Physics.LinearVelocity);
			m_gunBase.ConsumeAmmo();
			if (BackkickForcePerSecond > 0f && !base.CubeGrid.Physics.IsStatic)
			{
				base.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, -direction * BackkickForcePerSecond, base.PositionComp.GetPosition(), null);
			}
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public Vector3 GetShootDirection()
		{
			return base.WorldMatrix.Forward;
		}

		public override void BeginShoot(MyShootActionEnum action)
		{
			currentState = ShootStateEnum.Continuous;
			base.BeginShoot(action);
			StartLoopSound();
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			currentState = ShootStateEnum.Off;
			base.EndShoot(action);
			m_currentLateStart = 0;
			StopLoopSound();
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode && this.GetInventory().GetItemAmount(m_gunBase.CurrentAmmoMagazineId) < 1)
			{
				StartNoAmmoSound();
			}
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				if (m_safezoneNotification == null)
				{
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_ShootingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
		}

		public void OnControlAcquired(IMyCharacter owner)
		{
			if (owner != null)
			{
				MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
		}

		public void OnControlReleased()
		{
			if (MySession.Static.TopMostControlledEntity == base.CubeGrid)
			{
				MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out var status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				if (fullUpdate)
				{
					Vector3D from = base.PositionComp.GetPosition() + base.PositionComp.WorldMatrixRef.Forward;
					Vector3D to = base.PositionComp.GetPosition() + 200.0 * base.PositionComp.WorldMatrixRef.Forward;
					Vector3D target = Vector3D.Zero;
					if (MyHudCrosshair.GetTarget(from, to, ref target))
					{
						MatrixD matrix = base.WorldMatrix;
						MatrixD.Invert(ref matrix, out var result);
						Vector3D.Transform(ref target, ref result, out m_targetLocal);
					}
					else
					{
						m_targetLocal = Vector3.Zero;
					}
				}
				if (!Vector3D.IsZero(m_targetLocal))
				{
					Vector3D result2 = Vector3.Zero;
					MatrixD matrix2 = base.WorldMatrix;
					Vector3D.Transform(ref m_targetLocal, ref matrix2, out result2);
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, result2);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, fullUpdate ? Vector4.One : new Vector4(0.6f, 0.6f, 0.6f, 0.6f), result2, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
			MyHud.BlockInfo.MissingComponentIndex = -1;
<<<<<<< HEAD
			MyHud.BlockInfo.DefinitionId = BlockDefinition.Id;
			MyHud.BlockInfo.BlockName = BlockDefinition.DisplayNameText;
			MyHud.BlockInfo.SetContextHelp(BlockDefinition);
=======
			MyHud.BlockInfo.DefinitionId = base.BlockDefinition.Id;
			MyHud.BlockInfo.BlockName = base.BlockDefinition.DisplayNameText;
			MyHud.BlockInfo.SetContextHelp(base.BlockDefinition);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyHud.BlockInfo.PCUCost = 0;
			MyHud.BlockInfo.BlockIcons = BlockDefinition.Icons;
			MyHud.BlockInfo.BlockIntegrity = 1f;
			MyHud.BlockInfo.CriticalIntegrity = 0f;
			MyHud.BlockInfo.CriticalComponentIndex = 0;
			MyHud.BlockInfo.OwnershipIntegrity = 0f;
			MyHud.BlockInfo.BlockBuiltBy = 0L;
			MyHud.BlockInfo.GridSize = MyCubeSize.Small;
			MyHud.BlockInfo.Components.Clear();
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out var status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				Vector3D from = base.PositionComp.GetPosition() + base.PositionComp.WorldMatrixRef.Forward;
				Vector3D to = base.PositionComp.GetPosition() + 200.0 * base.PositionComp.WorldMatrixRef.Forward;
				Vector3D target = Vector3D.Zero;
				if (MyHudCrosshair.GetTarget(from, to, ref target))
				{
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, target);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, new Vector4(1f, 1f, 1f, 1f), target, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
		}

		private void UpdatePower()
		{
			base.ResourceSink.Update();
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
		}

		public void StartNoAmmoSound()
		{
			m_gunBase.StartNoAmmoSound(m_soundEmitter);
		}

		private void StopLoopSound()
		{
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying && m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (m_soundEmitterRotor != null && m_soundEmitterRotor.IsPlaying && m_soundEmitterRotor.Loop)
			{
				m_soundEmitterRotor.StopSound(forced: true);
				m_soundEmitterRotor.PlaySound(m_gunBase.SecondarySound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: true);
			}
		}

		private void PlayShotSound()
		{
			m_gunBase.StartShootSound(m_soundEmitter);
		}

		private void StartLoopSound()
		{
			if (m_soundEmitterRotor != null && m_gunBase.SecondarySound != MySoundPair.Empty && (!m_soundEmitterRotor.IsPlaying || !m_soundEmitterRotor.Loop) && base.IsWorking)
			{
				if (m_soundEmitterRotor.IsPlaying)
				{
					m_soundEmitterRotor.StopSound(forced: true);
				}
				m_soundEmitterRotor.PlaySound(m_gunBase.SecondarySound, stopPrevious: true);
			}
		}

		public int GetTotalAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetAmmunitionAmount();
		}

		public int GetMagazineAmount()
		{
			return m_gunBase.GetMagazineAmount();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				GetBarrelAndMuzzle();
			}
			else
			{
				m_barrel = null;
			}
		}

		private void GetBarrelAndMuzzle()
		{
<<<<<<< HEAD
			if (base.Subparts.TryGetValue("Barrel", out var value))
=======
			if (!base.Subparts.TryGetValue("Barrel", out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_barrel = value;
			}
			MyModel myModel = ((m_barrel == null) ? base.Model : m_barrel.Model);
			m_gunBase.LoadDummies(myModel.Dummies, BlockDefinition.DummyNames);
			if (!m_gunBase.HasDummies)
			{
				string text = "Muzzle";
				if (myModel.Dummies.ContainsKey(text))
				{
					m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, myModel.Dummies[text].Matrix, text);
					return;
				}
				Matrix localMatrix = Matrix.CreateTranslation(new Vector3(0f, 0f, -1f));
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, localMatrix, text);
			}
		}

		private void UpdateMuzzlePosition()
		{
			if (m_gunBase != null)
			{
				if (m_barrel != null)
				{
					m_gunBase.WorldMatrix = m_barrel.PositionComp.WorldMatrixRef;
				}
				else
				{
					m_gunBase.WorldMatrix = base.PositionComp.WorldMatrixRef;
				}
			}
		}

		private void UpdateMuzzlePosition()
		{
			if (m_gunBase != null && m_barrel != null)
			{
				m_gunBase.WorldMatrix = m_barrel.PositionComp.WorldMatrixRef;
			}
		}

		public override bool CanOperate()
		{
			return CheckIsWorking();
		}

		public override void ShootFromTerminal(Vector3 direction)
		{
			base.ShootFromTerminal(direction);
			Shoot(MyShootActionEnum.PrimaryAction, direction, null, null);
		}

		public override void StopShootFromTerminal()
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

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public PullInformation GetPullInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallGatlingGun x) => x.OnShootMissile, builder);
		}

<<<<<<< HEAD
		[Event(null, 1059)]
=======
		[Event(null, 1144)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Static.Add(builder);
			if (GunBase != null)
			{
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.BeforeShoot);
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
			}
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallGatlingGun x) => x.OnRemoveMissile, entityId);
		}

<<<<<<< HEAD
		[Event(null, 1077)]
=======
		[Event(null, 1157)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Static.Remove(entityId);
		}

		public Vector3D GetMuzzlePosition()
		{
			if (m_barrel != null && m_barrel.PositionComp.WorldMatrixRef.Translation != Vector3D.Zero)
			{
				return m_barrel.PositionComp.WorldMatrixRef.Translation;
			}
			if (m_gunBase.GetMuzzleWorldPosition() != Vector3D.Zero)
			{
				return m_gunBase.GetMuzzleWorldPosition();
			}
			return SlimBlock.WorldPosition;
		}

		public bool IsToolbarUsable()
		{
			return true;
		}

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}
	}
}
