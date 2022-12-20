using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.ModAPI.Weapons;
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
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[StaticEventOwner]
	[MyEntityType(typeof(MyObjectBuilder_AutomaticRifle), true)]
	public class MyAutomaticRifleGun : MyEntity, IMyHandheldGunObject<MyGunBase>, IMyGunObject<MyGunBase>, IMyGunBaseUser, IMyEventProxy, IMyEventOwner, IMyAutomaticRifleGun, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyMissileGunObject, IMyShootOrigin, IMySyncedEntity
	{
<<<<<<< HEAD
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile_0023System_Int64 : ICallSite<IMyEventOwner, MyObjectBuilder_Missile, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_Missile builder, in long entityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnShootMissile(builder, entityId);
=======
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<IMyEventOwner, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnShootMissile(builder);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRemoveMissile(entityId);
			}
		}

		private class Sandbox_Game_Weapons_MyAutomaticRifleGun_003C_003EActor : IActivator, IActivator<MyAutomaticRifleGun>
		{
			private sealed override object CreateInstance()
			{
				return new MyAutomaticRifleGun();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAutomaticRifleGun CreateInstance()
			{
				return new MyAutomaticRifleGun();
			}

			MyAutomaticRifleGun IActivator<MyAutomaticRifleGun>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static string NAME_SUBPART_MAGAZINE = "magazine";

		public static string NAME_DUMMY_MAGAZINE = "subpart_magazine";

		private static readonly string STATE_KEY_STANDING = "Standing";

		private static readonly string STATE_KEY_WALKING = "Walking";

		private static readonly string STATE_KEY_RUNNING = "Running";

		private static readonly string STATE_KEY_CROUCHING = "Crouching";

		private static readonly string STATE_KEY_AIMING = "Aiming";

		public static readonly int BASE_SHOOT_DIRECTION_UPDATE_TIME = 200;

		public static float RIFLE_MAX_SHAKE = 0.5f;

		public static float RIFLE_FOV_SHAKE = 0.0065f;

		public static float MAX_HORIZONTAL_RECOIL_DEVIATION = 10f;

		public static readonly float HORIZONTAL_RECOIL_OFFSET = 0.5f;
<<<<<<< HEAD
=======

		public static readonly float RECOIL_RETURN_SPEED = 0.106f;

		public static readonly float RECOIL_SPEED = 1f;

		private readonly float RECOIL_COMPENSATION_MULTIPLIER = 5f;

		private static float TO_RAD = 0.0174444448f;

		private int m_lastRecoil;

		private int m_lastTimeShoot;

		private float m_startingVertAngle;

		private float m_nextVertAngle;

		private float m_currentVertAngle;

		private float m_recoilTimer;

		private float m_backRecoilTimer;

		private float m_lastVertRecoilDiff;

		private float m_horizontalAngle;

		private float m_horizontalAngleOriginal;

		private bool m_isRecoiling;

		private MyRandom random = new MyRandom();

		private int m_lastDirectionChangeAnnounce;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public static readonly float RECOIL_RETURN_SPEED = 0.106f;

		public static readonly float RECOIL_SPEED = 1f;

		private static float TO_RAD = 0.0174444448f;

		private int m_lastRecoil;

		private int m_lastTimeShoot;

		private float m_startingVertAngle;

		private float m_nextVertAngle;

		private float m_currentVertAngle;

		private float m_recoilTimer;

		private float m_backRecoilTimer;

		private float m_lastVertRecoilDiff;

		private float m_horizontalAngle;

		private float m_horizontalAngleOriginal;

		private bool m_isRecoiling;

		private MyRandom random = new MyRandom();

		private bool m_firstDraw;

		private MyGunBase m_gunBase;

		private MyDefinitionId m_handItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "AutomaticRifleGun");

		private MyPhysicalItemDefinition m_physicalItemDef;

		private MyCharacter m_owner;

		protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();

		private bool m_canZoom = true;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private MyEntity3DSoundEmitter m_reloadSoundEmitter;

		private int m_shotsFiredInBurst;

		private MyHudNotification m_outOfAmmoNotification;

		private MyHudNotification m_safezoneNotification;

		private bool m_isAfterReleaseFire;

		private MyWeaponDefinition m_definition;

		private int m_weaponEquipDelay;

		private MyEntity[] m_shootIgnoreEntities;

		private int m_shootDirectionUpdateTime = BASE_SHOOT_DIRECTION_UPDATE_TIME;

		private MyTimeSpan m_reloadEndTime;

		private bool m_isReloading;

		public int LastTimeShoot => m_lastTimeShoot;

		public MyCharacter Owner => m_owner;

		public long OwnerId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.EntityId;
				}
				return 0L;
			}
		}

		public long OwnerIdentityId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetPlayerIdentityId();
				}
				return 0L;
			}
		}

		public bool IsShooting { get; private set; }

		public int ShootDirectionUpdateTime
		{
			get
			{
				return m_shootDirectionUpdateTime;
			}
			private set
			{
				if (m_shootDirectionUpdateTime != value)
				{
					if (value >= 0)
					{
						m_shootDirectionUpdateTime = value;
					}
					else
					{
						m_shootDirectionUpdateTime = BASE_SHOOT_DIRECTION_UPDATE_TIME;
					}
				}
			}
		}

		public bool NeedsShootDirectionWhileAiming => false;

		public float MaximumShotLength => m_gunBase.CurrentAmmoDefinition.MaxTrajectory;

		public bool ForceAnimationInsteadOfIK => false;

		public bool IsBlocking => false;

		public MyObjectBuilder_PhysicalGunObject PhysicalObject { get; set; }

		public SyncType SyncType { get; set; }

		public bool IsSkinnable => true;

		public bool IsTargetLockingCapable => true;

		public Vector3D ShootOrigin => base.PositionComp.GetPosition();

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount { get; protected set; }

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => m_handItemDefId;

		public MyGunBase GunBase => m_gunBase;

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon => this;

		MyEntity IMyGunBaseUser.Owner => m_owner;

		IMyMissileGunObject IMyGunBaseUser.Launcher => this;

		MyInventory IMyGunBaseUser.AmmoInventory
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetInventory();
				}
				return null;
			}
		}

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => m_physicalItemDef.Id;

		MyInventory IMyGunBaseUser.WeaponInventory
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetInventory();
				}
				return null;
			}
		}

		long IMyGunBaseUser.OwnerId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.ControllerInfo.ControllingIdentityId;
				}
				return 0L;
			}
		}

		string IMyGunBaseUser.ConstraintDisplayName => null;

		public MyPhysicalItemDefinition PhysicalItemDefinition => m_physicalItemDef;

		public int CurrentAmmunition
		{
			get
			{
				return m_gunBase.GetTotalAmmunitionAmount();
			}
			set
			{
				m_gunBase.CurrentAmmo = value;
			}
		}

		public int CurrentMagazineAmmunition
		{
			get
			{
				return m_gunBase.CurrentAmmo;
			}
			set
			{
				m_gunBase.CurrentAmmo = value;
			}
		}

		public int CurrentMagazineAmount
		{
			get
			{
				return m_gunBase.CurrentMagazines;
			}
			set
			{
				m_gunBase.CurrentMagazines = value;
			}
		}

		public bool HasIronSightsActive
		{
			get
			{
				return m_gunBase.HasIronSightsActive;
			}
			set
			{
				m_gunBase.HasIronSightsActive = value;
			}
		}

		public bool Reloadable => true;

		public bool IsRecoiling => m_isRecoiling;

		public bool IsReloading
		{
			get
			{
				return m_isReloading;
			}
			set
			{
				if (m_isReloading != value)
				{
					m_isReloading = value;
					if (Sync.IsServer)
					{
						Owner.IsReloading.Value = value;
					}
				}
			}
		}

		public bool NeedsReload
		{
			get
			{
				if (!IsReloading)
				{
					return CurrentMagazineAmmunition <= 0;
				}
				return false;
			}
		}

<<<<<<< HEAD
		public MyDefinitionBase GetAmmoDefinition => m_gunBase.CurrentAmmoDefinition;

		public float MaxShootRange => m_gunBase.CurrentAmmoDefinition.MaxTrajectory;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyAutomaticRifleGun()
		{
			m_shootIgnoreEntities = new MyEntity[1] { this };
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;
			base.Render.NeedsDraw = true;
			m_gunBase = new MyGunBase();
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
			m_reloadSoundEmitter = new MyEntity3DSoundEmitter(this);
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = WorldPositionChanged;
			base.Render = new MyRenderComponentAutomaticRifle();
			SyncType = SyncHelpers.Compose(this);
			SyncType.Append(m_gunBase);
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
			{
				m_handItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_AutomaticRifle), objectBuilder.SubtypeName);
			}
			MyObjectBuilder_AutomaticRifle myObjectBuilder_AutomaticRifle = (MyObjectBuilder_AutomaticRifle)objectBuilder;
			MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref m_handItemDefId);
			m_physicalItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(m_handItemDefId);
			MyDefinitionId myDefinitionId = ((!(m_physicalItemDef is MyWeaponItemDefinition)) ? new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "AutomaticRifleGun") : (m_physicalItemDef as MyWeaponItemDefinition).WeaponDefinitionId);
			m_gunBase.Init(myObjectBuilder_AutomaticRifle.GunBase, myDefinitionId, this);
			base.Init(objectBuilder);
			Init(MyTexts.Get(MySpaceTexts.DisplayName_Rifle), m_physicalItemDef.Model, null, null);
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(m_physicalItemDef.Model);
			MyWeaponItemDefinition myWeaponItemDefinition;
			if ((myWeaponItemDefinition = m_physicalItemDef as MyWeaponItemDefinition) != null)
			{
				m_gunBase.LoadDummies(modelOnlyDummies.Dummies, myWeaponItemDefinition.DummyNames);
			}
			else
			{
				m_gunBase.LoadDummies(modelOnlyDummies.Dummies, null);
			}
			if (!m_gunBase.HasDummies)
			{
				Matrix localMatrix = Matrix.CreateTranslation(myHandItemDefinition.MuzzlePosition);
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, localMatrix);
			}
			PhysicalObject = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(m_physicalItemDef.Id.TypeId, m_physicalItemDef.Id.SubtypeName);
			PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)myObjectBuilder_AutomaticRifle.Clone();
			PhysicalObject.GunEntity.EntityId = base.EntityId;
			MyDefinitionManager.Static.TryGetWeaponDefinition(myDefinitionId, out var definition);
			m_definition = definition;
			if (m_definition != null)
			{
				ShootDirectionUpdateTime = m_definition.ShootDirectionUpdateTime;
				m_weaponEquipDelay = MySandboxGame.TotalGamePlayTimeInMilliseconds + (int)(m_definition.EquipDuration * 1000f);
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_AutomaticRifle obj = (MyObjectBuilder_AutomaticRifle)base.GetObjectBuilder(copy);
			obj.SubtypeName = DefinitionId.SubtypeName;
			obj.GunBase = m_gunBase.GetObjectBuilder();
			obj.CurrentAmmo = CurrentMagazineAmmunition;
			return obj;
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			MyCharacterWeaponPositionComponent myCharacterWeaponPositionComponent = Owner.Components.Get<MyCharacterWeaponPositionComponent>();
			Vector3D vector3D = ((myCharacterWeaponPositionComponent == null || !Sandbox.Engine.Platform.Game.IsDedicated) ? Vector3D.Normalize(target - base.PositionComp.WorldMatrixRef.Translation) : Vector3D.Normalize(target - myCharacterWeaponPositionComponent.LogicalPositionWorld));
			return vector3D;
		}

		public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (m_owner == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(m_owner, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				if (!m_gunBase.HasAmmoMagazines)
				{
					status = MyGunStatusEnum.Failed;
					return false;
				}
				if (m_gunBase.ShotsInBurst > 0 && m_shotsFiredInBurst >= m_gunBase.ShotsInBurst)
				{
					status = MyGunStatusEnum.BurstLimit;
					return false;
				}
				if (IsReloading)
				{
					status = MyGunStatusEnum.Reloading;
					return false;
				}
				if (NeedsReload)
				{
					if (m_gunBase.HasEnoughMagazines())
					{
						status = MyGunStatusEnum.Reloading;
					}
					else
					{
						status = MyGunStatusEnum.OutOfAmmo;
					}
					return false;
				}
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
				{
					status = MyGunStatusEnum.Cooldown;
					return false;
				}
				if (!MySession.Static.CreativeMode && (!(m_owner.CurrentWeapon is MyAutomaticRifleGun) || !m_gunBase.HasEnoughAmmunition()))
				{
					status = MyGunStatusEnum.OutOfAmmo;
					return false;
				}
				status = MyGunStatusEnum.OK;
				return true;
			case MyShootActionEnum.SecondaryAction:
				if (!m_canZoom)
				{
					status = MyGunStatusEnum.Cooldown;
					return false;
				}
				return true;
			default:
				status = MyGunStatusEnum.Failed;
				return false;
			}
		}

		private void DebugDrawShots(Vector3 direction, int order)
		{
			Vector3D position = base.PositionComp.GetPosition();
			Vector3D vector3D = position + direction * (10 + order);
			MyRenderProxy.DebugDrawLine3D(position, vector3D, Color.Green, Color.Green, depthRead: false, persistent: true);
			MyRenderProxy.DebugDrawText3D(vector3D, order.ToString(), Color.Red, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				Shoot(direction, overrideWeaponPos);
				m_shotsFiredInBurst++;
				IsShooting = true;
				if (m_owner.ControllerInfo.IsLocallyControlled() && !MySession.Static.IsCameraUserAnySpectator())
				{
					MySector.MainCamera.CameraShake.AddShake(RIFLE_MAX_SHAKE);
					MySector.MainCamera.AddFovSpring(RIFLE_FOV_SHAKE);
				}
				break;
			case MyShootActionEnum.SecondaryAction:
				if (MySession.Static.ControlledEntity == m_owner && m_canZoom)
				{
					m_owner.Zoom(newKeyPress: true);
					m_canZoom = false;
					HasIronSightsActive = Owner.ZoomMode == MyZoomModeEnum.IronSight;
				}
				break;
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
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				IsShooting = false;
				m_shotsFiredInBurst = 0;
				m_gunBase.StopShoot();
				break;
			case MyShootActionEnum.SecondaryAction:
				m_canZoom = true;
				break;
			}
			m_isActionDoubleClicked[action] = false;
		}

		private void Shoot(Vector3 direction, Vector3D? overrideWeaponPos)
		{
			if (!overrideWeaponPos.HasValue || m_gunBase.DummiesPerType(MyAmmoType.HighSpeed) > 1)
			{
				if (m_owner != null)
				{
					m_gunBase.ShootWithOffset(m_owner.Physics.LinearVelocity, direction, -0.25f, m_owner, null, userNormalizedPositionForEffects: false);
				}
				else
				{
					m_gunBase.ShootWithOffset(Vector3.Zero, direction, -0.25f, null, null, userNormalizedPositionForEffects: false);
				}
			}
			else
			{
				m_gunBase.Shoot(overrideWeaponPos.Value + direction * -0.25f, m_owner.Physics.LinearVelocity, direction, m_owner, null, userNormalizedPositionForEffects: false);
			}
			if (Sync.IsServer && MySession.Static.Settings.EnableRecoil)
			{
				ApplyRecoilServer();
				m_lastRecoil = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_isAfterReleaseFire = false;
			if (m_gunBase.ShootSound != null)
			{
				StartLoopSound(m_gunBase.ShootSound);
			}
			m_gunBase.ConsumeAmmo();
		}

		public void SetMagazinePosition(MatrixD mat)
		{
<<<<<<< HEAD
			if (base.Subparts.ContainsKey(NAME_SUBPART_MAGAZINE))
=======
			if (m_smokeEffect == null && MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) < 150.0 && MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", base.PositionComp.WorldMatrixRef, out m_smokeEffect))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyEntitySubpart myEntitySubpart = base.Subparts[NAME_SUBPART_MAGAZINE];
				myEntitySubpart.PositionComp.SetWorldMatrix(ref mat, myEntitySubpart.Parent);
			}
		}

<<<<<<< HEAD
		public void ResetMagazinePosition()
=======
		private void OnSmokeEffectDelete(MyParticleEffect _)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			Dictionary<string, MyModelDummy> dummies = base.Render.GetModel().Dummies;
			if (dummies.ContainsKey(NAME_DUMMY_MAGAZINE) && base.Subparts.ContainsKey(NAME_SUBPART_MAGAZINE))
			{
				MyEntitySubpart myEntitySubpart = base.Subparts[NAME_SUBPART_MAGAZINE];
				Matrix localMatrix = Matrix.Normalize(dummies[NAME_DUMMY_MAGAZINE].Matrix);
				myEntitySubpart.PositionComp.SetLocalMatrix(ref localMatrix, myEntitySubpart.Parent);
			}
		}

		public void SetMagazinePosition(MatrixD mat)
		{
			if (base.Subparts.ContainsKey(NAME_SUBPART_MAGAZINE))
			{
				MyEntitySubpart myEntitySubpart = base.Subparts[NAME_SUBPART_MAGAZINE];
				myEntitySubpart.PositionComp.SetWorldMatrix(mat, myEntitySubpart.Parent);
			}
		}

		public void ResetMagazinePosition()
		{
			Dictionary<string, MyModelDummy> dummies = base.Render.GetModel().Dummies;
			if (dummies.ContainsKey(NAME_DUMMY_MAGAZINE) && base.Subparts.ContainsKey(NAME_SUBPART_MAGAZINE))
			{
				MyEntitySubpart myEntitySubpart = base.Subparts[NAME_SUBPART_MAGAZINE];
				Matrix localMatrix = Matrix.Normalize(dummies[NAME_DUMMY_MAGAZINE].Matrix);
				myEntitySubpart.PositionComp.SetLocalMatrix(ref localMatrix, myEntitySubpart.Parent);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
<<<<<<< HEAD
=======
			if (m_smokeEffect != null)
			{
				float num = 0.2f;
				m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(m_gunBase.GetMuzzleWorldPosition() + base.PositionComp.WorldMatrixRef.Forward * num);
				m_smokeEffect.UserBirthMultiplier = 50f;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_gunBase.UpdateEffects();
			if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) > m_gunBase.ReleaseTimeAfterFire && !m_isAfterReleaseFire)
			{
				StopLoopSound();
				m_isAfterReleaseFire = true;
				m_gunBase.RemoveOldEffects();
			}
			if (!Sync.IsDedicated)
			{
				AnimateRecoil();
			}
			AnimateRecoilHorizontal();
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo)
			{
				m_gunBase.StartNoAmmoSound(m_soundEmitter);
			}
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.Failed || status == MyGunStatusEnum.SafeZoneDenied)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
			if (status == MyGunStatusEnum.OutOfAmmo)
			{
				if (m_outOfAmmoNotification == null)
				{
					m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
				}
				m_outOfAmmoNotification.SetTextFormatArguments(base.DisplayName);
				MyHud.Notifications.Add(m_outOfAmmoNotification);
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

		public void StartLoopSound(MySoundPair cueEnum)
		{
			bool force2D = m_owner != null && m_owner.IsInFirstPersonView && m_owner == MySession.Static.LocalCharacter;
			m_gunBase.StartShootSound(m_soundEmitter, force2D);
			UpdateSoundEmitter();
		}

		public void StopLoopSound()
		{
			if (m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		private void WorldPositionChanged(object source)
		{
			m_gunBase.WorldMatrix = base.WorldMatrix;
		}

		protected override void Closing()
		{
			IsShooting = false;
			m_gunBase.RemoveOldEffects();
			if (m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: false);
			}
			if (m_reloadSoundEmitter.Loop)
			{
				m_reloadSoundEmitter.StopSound(forced: false);
			}
			base.Closing();
		}

		public void OnControlAcquired(IMyCharacter owner)
		{
			m_owner = (MyCharacter)owner;
			if (m_owner != null)
			{
				m_shootIgnoreEntities = new MyEntity[2] { this, m_owner };
				MyInventory inventory = m_owner.GetInventory();
				if (inventory != null)
				{
					inventory.ContentsChanged += MyAutomaticRifleGun_ContentsChanged;
				}
				Owner.RegisterRecoilDataChange(RecoilValueChangedCallback);
				Owner.IsReloading.ValueChanged += IsReloadingSynced;
			}
			m_gunBase.RefreshAmmunitionAmount();
			m_firstDraw = true;
			if (Owner == MySession.Static.LocalCharacter)
			{
				MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
			m_startingVertAngle = Owner.HeadLocalXAngle;
			m_backRecoilTimer = 1f;
		}

		private void MyAutomaticRifleGun_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount();
		}

		public void OnControlReleased()
		{
			if (m_owner != null)
			{
				Owner.IsReloading.ValueChanged -= IsReloadingSynced;
				Owner.UnregisterRecoilDataChange(RecoilValueChangedCallback);
				MyInventory inventory = m_owner.GetInventory();
				if (inventory != null)
				{
					inventory.ContentsChanged -= MyAutomaticRifleGun_ContentsChanged;
				}
			}
			if (Owner == MySession.Static.LocalCharacter)
			{
				MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
			}
			m_owner = null;
		}

		private void IsReloadingSynced(SyncBase obj)
		{
			if (!Sync.IsServer)
			{
				IsReloading = ((Sync<bool, SyncDirection.FromServer>)obj).Value;
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			if (m_firstDraw)
			{
				MyHud.BlockInfo.MissingComponentIndex = -1;
				MyHud.BlockInfo.DefinitionId = PhysicalItemDefinition.Id;
				MyHud.BlockInfo.BlockName = PhysicalItemDefinition.DisplayNameText;
				MyHud.BlockInfo.PCUCost = 0;
				MyHud.BlockInfo.BlockIcons = PhysicalItemDefinition.Icons;
				MyHud.BlockInfo.BlockIntegrity = 1f;
				MyHud.BlockInfo.CriticalIntegrity = 0f;
				MyHud.BlockInfo.CriticalComponentIndex = 0;
				MyHud.BlockInfo.OwnershipIntegrity = 0f;
				MyHud.BlockInfo.BlockBuiltBy = 0L;
				MyHud.BlockInfo.GridSize = MyCubeSize.Small;
				MyHud.BlockInfo.Components.Clear();
				MyHud.BlockInfo.SetContextHelp(PhysicalItemDefinition);
				m_firstDraw = false;
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

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			UpdateSoundEmitter();
			if (!Sync.IsServer || !IsReloading)
			{
				return;
			}
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (m_reloadEndTime < myTimeSpan)
			{
				if (m_gunBase.HasAmmoMagazines)
				{
					m_gunBase.ConsumeMagazine();
				}
				m_gunBase.RefreshAmmunitionAmount();
				IsReloading = false;
			}
		}

		public void UpdateSoundEmitter()
		{
			UpdateSingleEmitter(m_soundEmitter);
			UpdateSingleEmitter(m_reloadSoundEmitter);
		}

		private void UpdateSingleEmitter(MyEntity3DSoundEmitter soundEmitter)
		{
			if (soundEmitter != null)
			{
				if (m_owner != null)
				{
					Vector3 velocityVector = Vector3.Zero;
					m_owner.GetLinearVelocity(ref velocityVector);
					soundEmitter.SetVelocity(velocityVector);
				}
				soundEmitter.Update();
			}
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		public bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return false;
		}

		public void DoubleClicked(MyShootActionEnum action)
		{
			m_isActionDoubleClicked[action] = true;
		}

		private void AnimateRecoilHorizontal()
		{
			if (m_horizontalAngle != 0f && !((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastRecoil) < m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds))
			{
				if (m_horizontalAngleOriginal == 0f)
				{
					m_horizontalAngleOriginal = m_horizontalAngle;
				}
				if (m_backRecoilTimer >= 1f)
				{
					Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle - m_horizontalAngle);
					m_horizontalAngle = 0f;
					m_horizontalAngleOriginal = 0f;
				}
				else
				{
					float num = m_horizontalAngle - MathHelper.Lerp(m_horizontalAngleOriginal, 0f, m_backRecoilTimer);
					Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle - num);
					m_horizontalAngle -= num;
				}
			}
		}

		private void HorizontalRotation(float angle)
		{
<<<<<<< HEAD
			MatrixD worldMatrixRef = Owner.PositionComp.WorldMatrixRef;
			worldMatrixRef = MatrixD.CreateRotationY(angle) * worldMatrixRef;
			Owner.PositionComp.SetWorldMatrix(ref worldMatrixRef);
=======
			MatrixD worldMatrix = Owner.PositionComp.WorldMatrix;
			worldMatrix = MatrixD.CreateRotationY(angle) * worldMatrix;
			Owner.PositionComp.WorldMatrix = worldMatrix;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void AnimateRecoil()
		{
<<<<<<< HEAD
			if (Owner == null)
			{
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastRecoil) > m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds)
			{
				float headLocalXAngle = Owner.HeadLocalXAngle;
				bool flag = Math.Abs(headLocalXAngle - m_startingVertAngle) < 0.001f;
				if (m_backRecoilTimer >= 1f || flag)
				{
					if (m_isRecoiling)
					{
						m_backRecoilTimer = 1f;
						m_currentVertAngle = m_startingVertAngle;
						Owner.SetHeadLocalXAngle(m_currentVertAngle);
						m_isRecoiling = false;
					}
					return;
				}
				float num = (m_currentVertAngle = MathHelper.Lerp(headLocalXAngle, m_startingVertAngle, m_backRecoilTimer));
				m_backRecoilTimer += RECOIL_RETURN_SPEED;
			}
			else
			{
				if (m_recoilTimer >= 1f)
				{
					m_lastVertRecoilDiff = float.MaxValue;
					return;
				}
				float headLocalXAngle2 = Owner.HeadLocalXAngle;
				m_recoilTimer += RECOIL_SPEED / ((float)m_gunBase.WeaponProperties.CurrentWeaponRateOfFire / 60f);
				float num2 = MathHelper.Lerp(headLocalXAngle2, m_nextVertAngle, m_recoilTimer);
				m_lastVertRecoilDiff = num2 - headLocalXAngle2;
				m_currentVertAngle = num2;
			}
			Owner.SetHeadLocalXAngle(m_currentVertAngle);
		}

		private void ApplyRecoilServer()
		{
			float verticalValue = 1f;
			float horizontalValue = random.NextFloat(-1f, 1f);
			Owner.SetRecoilData(verticalValue, horizontalValue);
		}

		private void RecoilValueChangedCallback(SyncBase obj)
		{
			if (Owner == null)
			{
				return;
			}
			Owner.GetRecoilData(out var verticalValue, out var horizontalValue);
			if (Owner.ShouldRecoilRotate && Owner.JetpackRunning)
			{
				m_startingVertAngle = (m_currentVertAngle = (m_nextVertAngle = Owner.HeadLocalXAngle));
<<<<<<< HEAD
				Vector3 vector = Owner.PositionComp.WorldMatrixRef.Right;
				Vector3 vector2 = Owner.PositionComp.WorldMatrixRef.Up;
=======
				Vector3 vector = Owner.PositionComp.WorldMatrix.Right;
				Vector3 vector2 = Owner.PositionComp.WorldMatrix.Up;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Vector3 angularVelocity = Owner.Physics.AngularVelocity;
				float num = Vector3.Dot(angularVelocity, vector);
				float num2 = Vector3.Dot(angularVelocity, vector2);
				float num3 = 0f;
				float num4 = 0f;
				num3 = ((!(num < verticalValue * m_definition.RecoilJetpackVertical * TO_RAD)) ? num : (verticalValue * m_definition.RecoilJetpackVertical * TO_RAD));
				num4 = num2;
				float num5 = horizontalValue * (1f - HORIZONTAL_RECOIL_OFFSET);
				num5 = ((!(num5 < 0f)) ? (num5 + HORIZONTAL_RECOIL_OFFSET) : (num5 - HORIZONTAL_RECOIL_OFFSET));
				num4 += m_definition.RecoilJetpackHorizontal * num5 * TO_RAD;
				Owner.Physics.AngularVelocity = vector * num3 + vector2 * num4;
				m_isRecoiling = true;
				m_backRecoilTimer = 0f;
				m_startingVertAngle = Owner.HeadLocalXAngle;
				m_nextVertAngle = m_startingVertAngle;
			}
			else
			{
				float recoilGroundVertical = m_definition.RecoilGroundVertical;
				float recoilGroundHorizontal = m_definition.RecoilGroundHorizontal;
				GetRecoilMultipliers(Owner, out var recoilVertical, out var recoilHorizontal);
				if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastRecoil) > m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds)
				{
					if (m_backRecoilTimer >= 1f)
					{
						m_startingVertAngle = Owner.HeadLocalXAngle;
						m_nextVertAngle = m_startingVertAngle;
					}
					else
					{
						m_nextVertAngle = Owner.HeadLocalXAngle;
					}
					m_backRecoilTimer = 0f;
				}
				m_recoilTimer = 0f;
				float num6 = recoilGroundVertical * verticalValue * recoilVertical;
				m_nextVertAngle += num6;
				m_isRecoiling = true;
				float angleAddition = recoilGroundHorizontal * horizontalValue * recoilHorizontal;
				AddHorizontalRecoil(angleAddition);
			}
			m_lastRecoil = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void GetRecoilMultipliers(MyCharacter owner, out float recoilVertical, out float recoilHorizontal)
<<<<<<< HEAD
=======
		{
			recoilVertical = 1f;
			recoilHorizontal = 1f;
			if (m_definition == null)
			{
				return;
			}
			string text = "";
			if (owner.IsIronSighted)
			{
				text = STATE_KEY_AIMING;
			}
			else
			{
				switch (owner.GetCurrentMovementState())
				{
				case MyCharacterMovementEnum.Standing:
					text = STATE_KEY_STANDING;
					break;
				case MyCharacterMovementEnum.Crouching:
					text = STATE_KEY_CROUCHING;
					break;
				case MyCharacterMovementEnum.Jump:
					text = STATE_KEY_WALKING;
					break;
				case MyCharacterMovementEnum.Walking:
				case MyCharacterMovementEnum.BackWalking:
				case MyCharacterMovementEnum.WalkStrafingLeft:
				case MyCharacterMovementEnum.WalkingLeftFront:
				case MyCharacterMovementEnum.WalkingLeftBack:
				case MyCharacterMovementEnum.WalkStrafingRight:
				case MyCharacterMovementEnum.WalkingRightFront:
				case MyCharacterMovementEnum.WalkingRightBack:
					text = STATE_KEY_WALKING;
					break;
				case MyCharacterMovementEnum.Running:
				case MyCharacterMovementEnum.Backrunning:
				case MyCharacterMovementEnum.RunStrafingLeft:
				case MyCharacterMovementEnum.RunningLeftFront:
				case MyCharacterMovementEnum.RunningLeftBack:
				case MyCharacterMovementEnum.RunStrafingRight:
				case MyCharacterMovementEnum.RunningRightFront:
				case MyCharacterMovementEnum.RunningRightBack:
					text = STATE_KEY_RUNNING;
					break;
				case MyCharacterMovementEnum.CrouchWalking:
				case MyCharacterMovementEnum.CrouchBackWalking:
				case MyCharacterMovementEnum.CrouchStrafingLeft:
				case MyCharacterMovementEnum.CrouchWalkingLeftFront:
				case MyCharacterMovementEnum.CrouchWalkingLeftBack:
				case MyCharacterMovementEnum.CrouchStrafingRight:
				case MyCharacterMovementEnum.CrouchWalkingRightFront:
				case MyCharacterMovementEnum.CrouchWalkingRightBack:
				case MyCharacterMovementEnum.CrouchRotatingLeft:
				case MyCharacterMovementEnum.CrouchRotatingRight:
					text = STATE_KEY_CROUCHING;
					break;
				}
			}
			if (!string.IsNullOrEmpty(text) && m_definition.RecoilMultiplierData.ContainsKey(text))
			{
				Tuple<float, float> tuple = m_definition.RecoilMultiplierData[text];
				recoilVertical = tuple.Item1;
				recoilHorizontal = tuple.Item2;
			}
		}

		private void AddHorizontalRecoil(float angleAddition)
		{
			m_horizontalAngleOriginal = 0f;
			float num = m_horizontalAngle + angleAddition;
			if (Math.Abs(num) < MAX_HORIZONTAL_RECOIL_DEVIATION)
			{
				Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle + angleAddition);
				m_horizontalAngle += angleAddition;
				return;
			}
			float num2 = MAX_HORIZONTAL_RECOIL_DEVIATION - Math.Abs(m_horizontalAngle);
			if (num < 0f)
			{
				num2 *= -1f;
			}
			Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle + num2);
			m_horizontalAngle += num2;
		}

		public void ChangeRecoilVertAngles(float diffAngle)
		{
			_ = m_definition.RecoilGroundVertical;
			m_nextVertAngle += diffAngle;
			m_startingVertAngle = m_nextVertAngle;
		}

		public void MissileShootEffect()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			recoilVertical = 1f;
			recoilHorizontal = 1f;
			if (m_definition == null)
			{
				return;
			}
			string text = "";
			if (owner.IsIronSighted)
			{
				text = STATE_KEY_AIMING;
			}
			else
			{
				switch (owner.GetCurrentMovementState())
				{
				case MyCharacterMovementEnum.Standing:
					text = STATE_KEY_STANDING;
					break;
				case MyCharacterMovementEnum.Crouching:
					text = STATE_KEY_CROUCHING;
					break;
				case MyCharacterMovementEnum.Jump:
					text = STATE_KEY_WALKING;
					break;
				case MyCharacterMovementEnum.Walking:
				case MyCharacterMovementEnum.BackWalking:
				case MyCharacterMovementEnum.WalkStrafingLeft:
				case MyCharacterMovementEnum.WalkingLeftFront:
				case MyCharacterMovementEnum.WalkingLeftBack:
				case MyCharacterMovementEnum.WalkStrafingRight:
				case MyCharacterMovementEnum.WalkingRightFront:
				case MyCharacterMovementEnum.WalkingRightBack:
					text = STATE_KEY_WALKING;
					break;
				case MyCharacterMovementEnum.Running:
				case MyCharacterMovementEnum.Backrunning:
				case MyCharacterMovementEnum.RunStrafingLeft:
				case MyCharacterMovementEnum.RunningLeftFront:
				case MyCharacterMovementEnum.RunningLeftBack:
				case MyCharacterMovementEnum.RunStrafingRight:
				case MyCharacterMovementEnum.RunningRightFront:
				case MyCharacterMovementEnum.RunningRightBack:
					text = STATE_KEY_RUNNING;
					break;
				case MyCharacterMovementEnum.CrouchWalking:
				case MyCharacterMovementEnum.CrouchBackWalking:
				case MyCharacterMovementEnum.CrouchStrafingLeft:
				case MyCharacterMovementEnum.CrouchWalkingLeftFront:
				case MyCharacterMovementEnum.CrouchWalkingLeftBack:
				case MyCharacterMovementEnum.CrouchStrafingRight:
				case MyCharacterMovementEnum.CrouchWalkingRightFront:
				case MyCharacterMovementEnum.CrouchWalkingRightBack:
				case MyCharacterMovementEnum.CrouchRotatingLeft:
				case MyCharacterMovementEnum.CrouchRotatingRight:
					text = STATE_KEY_CROUCHING;
					break;
				}
			}
			if (!string.IsNullOrEmpty(text) && m_definition.RecoilMultiplierData.ContainsKey(text))
			{
				Tuple<float, float> tuple = m_definition.RecoilMultiplierData[text];
				recoilVertical = tuple.Item1;
				recoilHorizontal = tuple.Item2;
			}
		}

		private void AddHorizontalRecoil(float angleAddition)
		{
			m_horizontalAngleOriginal = 0f;
			float num = m_horizontalAngle + angleAddition;
			if (Math.Abs(num) < MAX_HORIZONTAL_RECOIL_DEVIATION)
			{
				Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle + angleAddition);
				m_horizontalAngle += angleAddition;
				return;
			}
			float num2 = MAX_HORIZONTAL_RECOIL_DEVIATION - Math.Abs(m_horizontalAngle);
			if (num < 0f)
			{
				num2 *= -1f;
			}
			Owner.SetHeadLocalYAngle(Owner.HeadLocalYAngle + num2);
			m_horizontalAngle += num2;
		}

		public void ChangeRecoilVertAngles(float diffAngle)
		{
			_ = m_definition.RecoilGroundVertical;
			m_nextVertAngle += diffAngle;
			m_startingVertAngle = m_nextVertAngle;
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnShootMissile, builder, base.EntityId);
		}

		[Event(null, 1185)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnShootMissile(MyObjectBuilder_Missile builder, long entityId)
=======
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnShootMissile, builder);
		}

		[Event(null, 1197)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnShootMissile(MyObjectBuilder_Missile builder)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			MyMissiles.Static.Add(builder);
			MyAutomaticRifleGun myAutomaticRifleGun = MyEntities.GetEntityById(entityId) as MyAutomaticRifleGun;
			if (myAutomaticRifleGun?.GunBase != null)
			{
				myAutomaticRifleGun.GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.BeforeShoot);
				myAutomaticRifleGun.GunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
			}
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnRemoveMissile, entityId);
		}

<<<<<<< HEAD
		[Event(null, 1206)]
=======
		[Event(null, 1210)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnRemoveMissile(long entityId)
		{
			MyMissiles.Static.Remove(entityId);
		}

		public bool CanReload()
		{
			bool flag = !IsReloading && !m_gunBase.IsAmmoFull() && m_gunBase.HasAmmoMagazines && m_gunBase.HasEnoughMagazines() && MySandboxGame.TotalGamePlayTimeInMilliseconds > m_weaponEquipDelay;
			if (Owner?.EntityId == MySession.Static?.LocalCharacter?.EntityId)
			{
				flag = flag && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && !MyScreenManager.IsAnyScreenOpening();
			}
			return flag;
		}

		public bool Reload()
		{
			if (!Sync.IsServer || IsReloading || m_gunBase.IsAmmoFull() || !m_gunBase.HasAmmoMagazines)
			{
				return false;
			}
			if (Owner.IsIronSighted)
			{
				Owner.EnableIronsight(enable: false, newKeyPress: true, changeCamera: true, hideCrosshairWhenAiming: true, forceChange: true);
			}
			float num = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_reloadEndTime = MyTimeSpan.FromMilliseconds(num + (float)m_definition.ReloadTime);
			CurrentAmmunition = 0;
			m_gunBase.EmptyMagazine();
			IsReloading = true;
			return true;
		}

		public float GetReloadDuration()
		{
			return (float)m_definition.ReloadTime * 0.001f;
		}

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}

		void IMyHandheldGunObject<MyGunBase>.PlayReloadSound()
		{
			m_gunBase.StartReloadSound(m_reloadSoundEmitter);
		}

		public bool GetShakeOnAction(MyShootActionEnum action)
		{
			if (m_definition.ShakeOnAction.TryGetValue(action, out var value))
			{
				return value;
			}
			return true;
		}

		public bool IsToolbarUsable()
		{
			return true;
		}

		public bool CanReload()
		{
			bool flag = !IsReloading && !m_gunBase.IsAmmoFull() && m_gunBase.HasAmmoMagazines && m_gunBase.HasEnoughMagazines() && MySandboxGame.TotalGamePlayTimeInMilliseconds > m_weaponEquipDelay;
			if (Owner?.EntityId == MySession.Static?.LocalCharacter?.EntityId)
			{
				flag = flag && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && !MyScreenManager.IsAnyScreenOpening();
			}
			return flag;
		}

		public bool Reload()
		{
			if (!Sync.IsServer || IsReloading || m_gunBase.IsAmmoFull() || !m_gunBase.HasAmmoMagazines)
			{
				return false;
			}
			if (Owner.IsIronSighted)
			{
				Owner.EnableIronsight(enable: false, newKeyPress: true, changeCamera: true, hideCrosshairWhenAiming: true, forceChange: true);
			}
			float num = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_reloadEndTime = MyTimeSpan.FromMilliseconds(num + (float)m_definition.ReloadTime);
			CurrentAmmunition = 0;
			m_gunBase.EmptyMagazine();
			IsReloading = true;
			return true;
		}

		public float GetReloadDuration()
		{
			return (float)m_definition.ReloadTime * 0.001f;
		}

		public Vector3D GetMuzzlePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition();
		}

		void IMyHandheldGunObject<MyGunBase>.PlayReloadSound()
		{
			m_gunBase.StartReloadSound(m_reloadSoundEmitter);
		}

		public bool GetShakeOnAction(MyShootActionEnum action)
		{
			if (m_definition.ShakeOnAction.TryGetValue(action, out var value))
			{
				return value;
			}
			return true;
		}
	}
}
