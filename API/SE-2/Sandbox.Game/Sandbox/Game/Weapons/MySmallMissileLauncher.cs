using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Text;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
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
	/// <summary>
	///             Note: "Small"MissileLauncher doesn't mean "small grid" only! This type is used for both, small and large grid blocks.
	///      Large is reloadable from conveyor system, small is not. (This behavior is explicitly hardcoded inside <see cref="M:Sandbox.Game.Weapons.MySmallMissileLauncher.Init(VRage.Game.MyObjectBuilder_CubeBlock,Sandbox.Game.Entities.MyCubeGrid)" />.
	///      For "small grid reloadable" missile launcher see<see cref="T:Sandbox.Game.Weapons.MySmallMissileLauncherReload" />.
	/// </summary>
	[MyCubeBlockType(typeof(MyObjectBuilder_SmallMissileLauncher))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySmallMissileLauncher),
		typeof(Sandbox.ModAPI.Ingame.IMySmallMissileLauncher)
	})]
	public class MySmallMissileLauncher : MyUserControllableGun, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyShootOrigin, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallMissileLauncher, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallMissileLauncher
	{
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallMissileLauncher, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallMissileLauncher @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnPreShootMissile_003C_003E : ICallSite<MySmallMissileLauncher, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallMissileLauncher @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnPreShootMissile();
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MySmallMissileLauncher, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallMissileLauncher @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		protected class m_cachedAmmunitionAmount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cachedAmmunitionAmount;
				ISyncType result = (cachedAmmunitionAmount = new Sync<int, SyncDirection.FromServer>(P_1, P_2));
				((MySmallMissileLauncher)P_0).m_cachedAmmunitionAmount = (Sync<int, SyncDirection.FromServer>)cachedAmmunitionAmount;
				return result;
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MySmallMissileLauncher)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_MySmallMissileLauncher_003C_003EActor : IActivator, IActivator<MySmallMissileLauncher>
		{
			private sealed override object CreateInstance()
			{
				return new MySmallMissileLauncher();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySmallMissileLauncher CreateInstance()
			{
				return new MySmallMissileLauncher();
			}

			MySmallMissileLauncher IActivator<MySmallMissileLauncher>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected int m_shotsLeftInBurst;

		protected int m_nextShootTime;

		private int m_nextNotificationTime;

		private MyHudNotification m_reloadNotification;

		private MyGunBase m_gunBase;

		private int m_shoot;

		private Vector3 m_shootDirection;

		private int m_lateStartRandom = MyUtils.GetRandomInt(0, 3);

		private int m_currentLateStart;

		private Vector3D m_targetLocal = Vector3.Zero;

		private MyEntity[] m_shootIgnoreEntities;

		private MyHudNotification m_safezoneNotification;

		private Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;

		private MyMultilineConveyorEndpoint m_endpoint;

		protected Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		protected MyHudNotification ReloadNotification
		{
			get
			{
				if (m_reloadNotification == null)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.MissileLauncherReloadingNotification, m_gunBase.ReloadTime - 250, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
				}
				return m_reloadNotification;
			}
		}

		public MyEntityCapacitorComponent CapacitorComponent => base.Components.Get<MyEntityCapacitorComponent>();

		public IMyConveyorEndpoint ConveyorEndpoint => m_endpoint;

		public bool IsSkinnable => false;

		public bool IsTargetLockingCapable => true;

		public Vector3D ShootOrigin => GetWeaponMuzzleWorldPosition();

		public new MyWeaponBlockDefinition BlockDefinition => base.BlockDefinition as MyWeaponBlockDefinition;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount { get; protected set; }

		public bool IsControlled => Controller != null;

		public MyCharacter Controller { get; protected set; }

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => BlockDefinition.Id;

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

		public bool IsShooting => m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds;

		public int ShootDirectionUpdateTime => 0;

<<<<<<< HEAD
		/// <inheritdoc />
		public bool NeedsShootDirectionWhileAiming => false;

		/// <inheritdoc />
=======
		public bool NeedsShootDirectionWhileAiming => false;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float MaximumShotLength => 0f;

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

		bool Sandbox.ModAPI.Ingame.IMySmallMissileLauncher.UseConveyorSystem => m_useConveyorSystem;

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
			m_endpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_endpoint));
		}

		public override bool IsStationary()
		{
			return true;
		}

		public override Vector3D GetWeaponMuzzleWorldPosition()
		{
			if (m_gunBase != null)
			{
				return m_gunBase.GetMuzzleWorldPosition();
			}
			return base.GetWeaponMuzzleWorldPosition();
		}

		public MySmallMissileLauncher()
		{
			m_shootIgnoreEntities = new MyEntity[1] { this };
			CreateTerminalControls();
			m_gunBase = new MyGunBase();
			if (Sync.IsServer)
			{
				m_gunBase.OnAmmoAmountChanged += OnAmmoAmountChangedOnServer;
			}
			else
			{
				m_cachedAmmunitionAmount.ValueChanged += OnAmmoAmountChangedFromServer;
			}
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			base.SyncType.Append(m_gunBase);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySmallMissileLauncher>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MySmallMissileLauncher> obj = new MyTerminalControlOnOffSwitch<MySmallMissileLauncher>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = (MySmallMissileLauncher x) => x.UseConveyorSystem,
					Setter = delegate(MySmallMissileLauncher x, bool v)
					{
						x.UseConveyorSystem = v;
					},
					Visible = (MySmallMissileLauncher x) => x.CubeGrid.GridSizeEnum == MyCubeSize.Large
				};
				obj.EnableToggleAction((MySmallMissileLauncher x) => x.CubeGrid.GridSizeEnum == MyCubeSize.Large);
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_SmallMissileLauncher myObjectBuilder_SmallMissileLauncher = builder as MyObjectBuilder_SmallMissileLauncher;
			MyWeaponBlockDefinition blockDefinition = BlockDefinition;
			MyStringHash group;
			if (blockDefinition != null && this.GetInventory() == null)
			{
				MyInventory component = new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive);
				base.Components.Add((MyInventoryBase)component);
				group = blockDefinition.ResourceSinkGroup;
			}
			else
			{
				if (this.GetInventory() == null)
				{
					MyInventory myInventory = null;
					myInventory = ((cubeGrid.GridSizeEnum != MyCubeSize.Small) ? new MyInventory(1.14f, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive) : new MyInventory(0.24f, new Vector3(1.2f, 0.45f, 0.45f), MyInventoryFlags.CanReceive));
					base.Components.Add(myInventory);
				}
				group = MyStringHash.GetOrCompute("Defense");
			}
			this.GetInventory();
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(group, 0.0002f, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			base.ResourceSink = myResourceSinkComponent;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			m_gunBase.Init(myObjectBuilder_SmallMissileLauncher.GunBase, BlockDefinition, this);
			base.Init(builder, cubeGrid);
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			base.ResourceSink.Update();
			this.GetInventory().Init(myObjectBuilder_SmallMissileLauncher.Inventory);
			m_shotsLeftInBurst = m_gunBase.ShotsInBurst;
			AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(base.ResourceSink, this));
			if (base.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				m_useConveyorSystem.SetLocalValue(myObjectBuilder_SmallMissileLauncher.UseConveyorSystem);
			}
			else
			{
				m_useConveyorSystem.SetLocalValue(newValue: false);
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			LoadDummies();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
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

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += m_ammoInventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= m_ammoInventory_ContentsChanged;
			}
		}

		private void LoadDummies()
		{
<<<<<<< HEAD
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(BlockDefinition.Model);
			m_gunBase.LoadDummies(modelOnlyDummies.Dummies, BlockDefinition.DummyNames);
=======
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(base.BlockDefinition.Model);
			m_gunBase.LoadDummies(modelOnlyDummies.Dummies);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_gunBase.HasDummies)
			{
				return;
			}
			foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
			{
				if (dummy.Key.ToLower().Contains("barrel"))
				{
					m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix);
				}
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void m_ammoInventory_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount(forceUpdate: true);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SmallMissileLauncher obj = (MyObjectBuilder_SmallMissileLauncher)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			obj.GunBase = m_gunBase.GetObjectBuilder();
			return obj;
		}

		protected void InterruptShot()
		{
			m_shoot = 0;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
			if (!Enabled)
			{
				InterruptShot();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			if (!base.IsFunctional)
			{
				InterruptShot();
			}
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

		private Vector3D GetSmokePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition() - base.WorldMatrix.Forward * 0.5;
		}

		public void OnControlAcquired(IMyCharacter owner)
		{
<<<<<<< HEAD
			Controller = (MyCharacter)owner;
=======
			Controller = owner;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (owner == MySession.Static.LocalCharacter)
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
			Controller = null;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && base.IsFunctional && UseConveyorSystem && MySession.Static.SurvivalMode)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.VolumeFillFactor * MySession.Static.BlocksInventorySizeMultiplier < 1f)
				{
					int num = m_gunBase.WeaponProperties.CurrentWeaponRateOfFire / 36 + 1;
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(m_gunBase.CurrentAmmoMagazineId, num, this, inventory, remove: false, calcImmediately: false);
				}
			}
			if (Sync.IsServer && m_gunBase.CurrentAmmo == 0)
			{
				m_gunBase.ConsumeMagazine();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_shoot != 0 && MySandboxGame.TotalGamePlayTimeInMilliseconds >= m_shoot)
			{
				ShootMissile();
				m_shoot = 0;
			}
			UpdateReloadNotification();
			base.NeedsUpdate &= (MyEntityUpdateEnum)(-1);
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out var status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				if (fullUpdate)
				{
					MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
					Vector3D translation = muzzleWorldMatrix.Translation;
					Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
					Vector3D target = Vector3D.Zero;
					if (MyHudCrosshair.GetTarget(translation, to, ref target))
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
				MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				Vector3D translation = muzzleWorldMatrix.Translation;
				Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
				Vector3D target = Vector3D.Zero;
				if (MyHudCrosshair.GetTarget(translation, to, ref target))
				{
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, target);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, Vector4.One, target, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
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

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return base.WorldMatrix.Forward;
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (action != 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			if (!m_gunBase.HasAmmoMagazines)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				status = MyGunStatusEnum.Cooldown;
				return false;
			}
			if (m_shotsLeftInBurst == 0 && m_gunBase.ShotsInBurst > 0)
			{
				status = MyGunStatusEnum.Failed;
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
			if (!(CapacitorComponent?.IsCharged ?? true))
			{
				status = MyGunStatusEnum.NotCharged;
				return false;
			}
			if (!MySession.Static.CreativeMode && !m_gunBase.HasEnoughAmmunition())
			{
				status = MyGunStatusEnum.OutOfAmmo;
				return false;
			}
			return true;
		}

		public virtual void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (m_shootingBegun && m_lateStartRandom > m_currentLateStart)
			{
				m_currentLateStart++;
				return;
			}
			m_shoot = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.WeaponDefinition.ShotDelay;
			if (m_gunBase.WeaponDefinition.ShotDelay > 0)
			{
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseEvent(this, (MySmallMissileLauncher x) => x.OnPreShootMissile);
				}
			}
			else
			{
				OnPreShootMissile();
			}
			m_shootDirection = direction;
			m_gunBase.ConsumeAmmo();
			m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ShootIntervalInMiliseconds;
			if (m_gunBase.ShotsInBurst > 0)
			{
				m_shotsLeftInBurst--;
				if (m_shotsLeftInBurst <= 0)
				{
					m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
					m_shotsLeftInBurst = m_gunBase.ShotsInBurst;
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public Vector3 GetShootDirection()
		{
			return base.WorldMatrix.Forward;
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			base.EndShoot(action);
			m_currentLateStart = 0;
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode)
			{
				m_gunBase.StartNoAmmoSound(m_soundEmitter);
			}
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.NotCharged)
			{
				MyEntityCapacitorComponent capacitorComponent = CapacitorComponent;
				if (capacitorComponent != null)
				{
					MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationWeaponNotFullyCharged, 1500);
					myHudNotification.SetTextFormatArguments((capacitorComponent.StoredPower / capacitorComponent.Capacity).ToString("P"));
					MyHud.Notifications.Add(myHudNotification);
				}
			}
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

		private void ShootMissile()
		{
			if (m_gunBase == null)
			{
				MySandboxGame.Log.WriteLine("Missile launcher barrel null");
				return;
			}
			if (base.Parent.Physics == null || base.Parent.Physics.RigidBody == null)
			{
				MySandboxGame.Log.WriteLine("Missile launcher parent physics null");
				return;
			}
			Vector3 linearVelocity = base.Parent.Physics.LinearVelocity;
			ShootMissile(linearVelocity);
			if (BackkickForcePerSecond > 0f && !base.CubeGrid.Physics.IsStatic)
			{
				Vector3D backward = m_gunBase.GetMuzzleWorldMatrix().Backward;
				base.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, (Vector3)backward * BackkickForcePerSecond, base.PositionComp.GetPosition(), null);
			}
		}

		private void ShootMissile(Vector3 velocity)
		{
			if (Sync.IsServer)
			{
				m_gunBase.Shoot(velocity, (Controller != null) ? Controller.IsUsing : null);
				CapacitorComponent?.SetStoredPower(0f);
			}
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			m_gunBase.WorldMatrix = base.WorldMatrix;
		}

		private void UpdateReloadNotification()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds > m_nextNotificationTime)
			{
				m_reloadNotification = null;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			if (Controller != MySession.Static.LocalCharacter)
			{
				if (m_reloadNotification != null)
				{
					MyHud.Notifications.Remove(m_reloadNotification);
					m_reloadNotification = null;
				}
			}
			else if (CapacitorComponent != null)
			{
				if (!CapacitorComponent.IsCharged && MySandboxGame.TotalGamePlayTimeInMilliseconds >= m_shoot)
				{
					ShowReloadNotification((int)Math.Min(CapacitorComponent.TimeRemaining * 1000f, 5000f));
				}
			}
			else if (m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds && m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds > m_gunBase.ShootIntervalInMiliseconds)
			{
				ShowReloadNotification(m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

		/// <summary>
		/// Will show the reload notification for the specified duration.
		/// </summary>
		/// <param name="duration">The time in MS it should show reloading.</param>
		private void ShowReloadNotification(int duration)
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
			if (m_reloadNotification == null)
			{
				duration = Math.Max(0, duration - 250);
				if (duration != 0)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					MyHud.Notifications.Add(m_reloadNotification);
					m_nextNotificationTime = num;
				}
			}
			else
			{
				int timeStep = num - m_nextNotificationTime;
				m_reloadNotification.AddAliveTime(timeStep);
				m_nextNotificationTime = num;
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			CapacitorComponent?.UpdateDetailedInfo(detailedInfo);
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

		public bool SupressShootAnimation()
		{
			return false;
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallMissileLauncher x) => x.OnShootMissile, builder);
		}

<<<<<<< HEAD
		[Event(null, 990)]
=======
		[Event(null, 906)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Static.Add(builder);
			if (GunBase != null)
			{
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
				m_gunBase.StartShootSound(m_soundEmitter);
			}
		}

		/// <summary>
		/// Called only if gun has continuous shoot. Like railgun
		/// </summary>
		[Event(null, 1004)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnPreShootMissile()
		{
			if (GunBase != null)
			{
				GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.BeforeShoot);
				m_gunBase.StartPreShotSound(m_soundEmitter);
			}
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallMissileLauncher x) => x.OnRemoveMissile, entityId);
		}

<<<<<<< HEAD
		[Event(null, 1019)]
=======
		[Event(null, 917)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Static.Remove(entityId);
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

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}
<<<<<<< HEAD

		public bool IsToolbarUsable()
		{
			return true;
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
