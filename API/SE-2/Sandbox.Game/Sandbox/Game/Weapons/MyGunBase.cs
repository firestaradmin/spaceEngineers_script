using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	public class MyGunBase : MyDeviceBase
	{
		public class DummyContainer
		{
			public List<KeyValuePair<string, MatrixD>> Dummies = new List<KeyValuePair<string, MatrixD>>();

			public int DummyIndex;

			public MatrixD DummyInWorld = MatrixD.Identity;

			public bool Dirty = true;

			public MatrixD DummyToUse => Dummies[DummyIndex].Value;

			public string DummyToUseName => Dummies[DummyIndex].Key;
		}

		public class WeaponEffect
		{
			public Matrix LocalMatrix;

			public MyParticleEffect Effect;

			public MyWeaponDefinition.MyWeaponEffect WeaponEffectDefinition;

			public bool UseMuzzlePosition;

			public WeaponEffect(Matrix localMatrix, MyParticleEffect effect, MyWeaponDefinition.MyWeaponEffect weaponEffect)
			{
				Effect = effect;
				LocalMatrix = localMatrix * Matrix.CreateTranslation(weaponEffect.Offset);
				WeaponEffectDefinition = weaponEffect;
				Effect.UserBirthMultiplier = weaponEffect.ParticleBirthStart;
			}

<<<<<<< HEAD
			public void Update()
			{
				Effect.UserBirthMultiplier = MathHelper.Clamp(Effect.UserBirthMultiplier - WeaponEffectDefinition.ParticleBirthDecrease, WeaponEffectDefinition.ParticleBirthMin, WeaponEffectDefinition.ParticleBirthMax);
			}

			public void RefreshTimer()
			{
				Effect.UserBirthMultiplier = MathHelper.Clamp(Effect.UserBirthMultiplier + WeaponEffectDefinition.ParticleBirthIncrease, WeaponEffectDefinition.ParticleBirthMin, WeaponEffectDefinition.ParticleBirthMax);
			}
		}
=======
		private static readonly bool DEBUG_PROJECTILE_VIEW_TRAJECTORIES;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public static int DUMMY_KEY_PROJECTILE = 0;

		public static int DUMMY_KEY_MISSILE = 1;

		public static int DUMMY_KEY_HOLDING = 2;

		private static readonly bool DEBUG_PROJECTILE_VIEW_TRAJECTORIES = false;

		public const int AMMO_PER_SHOOT = 1;

		private const string PARENT = "None";

		protected MyWeaponPropertiesWrapper m_weaponProperties;

		protected Dictionary<MyDefinitionId, int> m_remainingAmmos;

		protected Dictionary<int, DummyContainer> m_dummiesByAmmoType;

		protected MatrixD m_worldMatrix;

		protected IMyGunBaseUser m_user;

		public List<WeaponEffect> ActiveLoopEffects = new List<WeaponEffect>();

		public Matrix m_holdingDummyMatrix;

		private int m_shotProjectiles;

		private Dictionary<string, MyModelDummy> m_dummies;

		private int m_currentAmmo;

<<<<<<< HEAD
		private bool? m_isUserControllableGunBlock;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int CurrentAmmo
		{
			get
			{
				return m_currentAmmo;
			}
			set
			{
				if (m_currentAmmo != value)
				{
					m_currentAmmo = value;
					this.OnAmmoAmountChanged?.Invoke();
				}
			}
		}

		public int CurrentMagazines { get; set; }

		public MyWeaponPropertiesWrapper WeaponProperties => m_weaponProperties;

		public MyAmmoMagazineDefinition CurrentAmmoMagazineDefinition => WeaponProperties.AmmoMagazineDefinition;

		public MyDefinitionId CurrentAmmoMagazineId => WeaponProperties.AmmoMagazineId;

		public MyAmmoDefinition CurrentAmmoDefinition => WeaponProperties.AmmoDefinition;

		public MyWeaponDefinition WeaponDefinition => WeaponProperties.WeaponDefinition;

		public float BackkickForcePerSecond
		{
			get
			{
				if (WeaponProperties != null && WeaponProperties.AmmoDefinition != null)
				{
					return WeaponProperties.AmmoDefinition.BackkickForce;
				}
				return 0f;
			}
		}

		public bool HasMissileAmmoDefined => m_weaponProperties.WeaponDefinition.HasMissileAmmoDefined;

		public bool HasProjectileAmmoDefined => m_weaponProperties.WeaponDefinition.HasProjectileAmmoDefined;

		public int MuzzleFlashLifeSpan => m_weaponProperties.WeaponDefinition.MuzzleFlashLifeSpan;

		public int ShootIntervalInMiliseconds
		{
			get
			{
				if (ShootIntervalModifier != 1f)
				{
					return (int)(ShootIntervalModifier * (float)m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds);
				}
				return m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds;
			}
		}

		public float ShootIntervalModifier { get; set; }

		public float ReleaseTimeAfterFire => m_weaponProperties.WeaponDefinition.ReleaseTimeAfterFire;

		public MySoundPair ShootSound => m_weaponProperties.CurrentWeaponShootSound;

		public MySoundPair PreShotSound => m_weaponProperties.CurrentWeaponPreShotSound;

		public MySoundPair NoAmmoSound => m_weaponProperties.WeaponDefinition.NoAmmoSound;

		public MySoundPair ReloadSound => m_weaponProperties.WeaponDefinition.ReloadSound;

		public MySoundPair SecondarySound => m_weaponProperties.WeaponDefinition.SecondarySound;

		public bool UseDefaultMuzzleFlash => m_weaponProperties.WeaponDefinition.UseDefaultMuzzleFlash;

		public float MechanicalDamage
		{
			get
			{
				if (WeaponProperties.AmmoDefinition != null)
				{
					return m_weaponProperties.AmmoDefinition.GetDamageForMechanicalObjects();
				}
				return 0f;
			}
		}

		public float DeviateAngle => m_weaponProperties.WeaponDefinition.DeviateShotAngle;

		public float DeviateAngleWhileAiming => m_weaponProperties.WeaponDefinition.DeviateShotAngleAiming;

		public bool HasAmmoMagazines => m_weaponProperties.WeaponDefinition.HasAmmoMagazines();

		public bool IsAmmoProjectile => m_weaponProperties.IsAmmoProjectile;

		public bool IsAmmoMissile => m_weaponProperties.IsAmmoMissile;

		public int ShotsInBurst => WeaponProperties.ShotsInBurst;

		public int ReloadTime => WeaponProperties.ReloadTime;

		public bool HasDummies => m_dummiesByAmmoType.Count > 0;

		public bool IsUserControllableGunBlock
		{
			get
			{
				if (!m_isUserControllableGunBlock.HasValue)
				{
					MyUserControllableGun myUserControllableGun;
					m_isUserControllableGunBlock = m_user?.Weapon?.Render != null && (myUserControllableGun = m_user as MyUserControllableGun) != null;
				}
				return m_isUserControllableGunBlock.Value;
			}
		}

		public MatrixD WorldMatrix
		{
			get
			{
				return m_worldMatrix;
			}
			set
			{
				m_worldMatrix = value;
				RecalculateMuzzles();
<<<<<<< HEAD
				if (!IsUserControllableGunBlock)
				{
					UpdateEffectPositions();
				}
=======
				UpdateEffectPositions();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public DateTime LastShootTime { get; private set; }

		public bool HasIronSightsActive { get; set; }

		public event Action OnAmmoAmountChanged;

		public int DummiesPerType(MyAmmoType ammoType)
		{
			if (m_dummiesByAmmoType.ContainsKey((int)ammoType))
			{
				return m_dummiesByAmmoType[(int)ammoType].Dummies.Count;
			}
			return 0;
		}

		public MyGunBase()
		{
			m_dummiesByAmmoType = new Dictionary<int, DummyContainer>();
			m_remainingAmmos = new Dictionary<MyDefinitionId, int>();
			ShootIntervalModifier = 1f;
		}

		public MyObjectBuilder_GunBase GetObjectBuilder()
		{
			MyObjectBuilder_GunBase myObjectBuilder_GunBase = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GunBase>();
			myObjectBuilder_GunBase.CurrentAmmoMagazineName = CurrentAmmoMagazineId.SubtypeName;
			myObjectBuilder_GunBase.RemainingAmmo = CurrentAmmo;
			myObjectBuilder_GunBase.RemainingMagazines = CurrentMagazines;
			myObjectBuilder_GunBase.LastShootTime = LastShootTime.Ticks;
			myObjectBuilder_GunBase.RemainingAmmosList = new List<MyObjectBuilder_GunBase.RemainingAmmoIns>();
			foreach (KeyValuePair<MyDefinitionId, int> remainingAmmo in m_remainingAmmos)
			{
				MyObjectBuilder_GunBase.RemainingAmmoIns remainingAmmoIns = new MyObjectBuilder_GunBase.RemainingAmmoIns();
				remainingAmmoIns.SubtypeName = remainingAmmo.Key.SubtypeName;
				remainingAmmoIns.Amount = remainingAmmo.Value;
				myObjectBuilder_GunBase.RemainingAmmosList.Add(remainingAmmoIns);
			}
			myObjectBuilder_GunBase.InventoryItemId = base.InventoryItemId;
			return myObjectBuilder_GunBase;
		}

		public void Init(MyObjectBuilder_GunBase objectBuilder, MyCubeBlockDefinition cubeBlockDefinition, IMyGunBaseUser gunBaseUser)
		{
			if (cubeBlockDefinition is MyWeaponBlockDefinition)
			{
				MyWeaponBlockDefinition myWeaponBlockDefinition = cubeBlockDefinition as MyWeaponBlockDefinition;
				Init(objectBuilder, myWeaponBlockDefinition.WeaponDefinitionId, gunBaseUser);
			}
			else
			{
				MyDefinitionId backwardCompatibleDefinitionId = GetBackwardCompatibleDefinitionId(cubeBlockDefinition.Id.TypeId);
				Init(objectBuilder, backwardCompatibleDefinitionId, gunBaseUser);
			}
		}

		public void Init(MyObjectBuilder_GunBase objectBuilder, MyDefinitionId weaponDefinitionId, IMyGunBaseUser gunBaseUser)
		{
			if (objectBuilder != null)
			{
				Init(objectBuilder);
			}
			m_user = gunBaseUser;
			m_weaponProperties = new MyWeaponPropertiesWrapper(weaponDefinitionId);
			PerformCheck(weaponDefinitionId, m_weaponProperties);
			m_remainingAmmos = new Dictionary<MyDefinitionId, int>(WeaponProperties.AmmoMagazinesCount);
			if (objectBuilder != null)
			{
				MyDefinitionId newAmmoMagazineId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), objectBuilder.CurrentAmmoMagazineName);
				if (m_weaponProperties.CanChangeAmmoMagazine(newAmmoMagazineId))
				{
					CurrentAmmo = objectBuilder.RemainingAmmo;
					CurrentMagazines = objectBuilder.RemainingMagazines;
					m_weaponProperties.ChangeAmmoMagazine(newAmmoMagazineId);
				}
				else if (WeaponProperties.WeaponDefinition.HasAmmoMagazines())
				{
					m_weaponProperties.ChangeAmmoMagazine(m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
				}
				foreach (MyObjectBuilder_GunBase.RemainingAmmoIns remainingAmmos in objectBuilder.RemainingAmmosList)
				{
					m_remainingAmmos.Add(new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), remainingAmmos.SubtypeName), remainingAmmos.Amount);
				}
				LastShootTime = new DateTime(objectBuilder.LastShootTime);
			}
			else
			{
				if (WeaponProperties.WeaponDefinition.HasAmmoMagazines())
				{
					m_weaponProperties.ChangeAmmoMagazine(m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
				}
				if (MySession.Static.CreativeMode)
				{
					CurrentAmmo = WeaponProperties.AmmoMagazineDefinition.Capacity;
				}
				LastShootTime = new DateTime(0L);
			}
			if (m_user.AmmoInventory != null)
			{
				if (m_user.PutConstraint())
				{
					m_user.AmmoInventory.Constraint = CreateAmmoInventoryConstraints(m_user.ConstraintDisplayName);
				}
				RefreshAmmunitionAmount();
			}
			if (m_user.Weapon != null)
			{
				m_user.Weapon.OnClosing += Weapon_OnClosing;
			}
		}

		private void PerformCheck(MyDefinitionId blockId, MyWeaponPropertiesWrapper weaponProperties)
		{
			if (weaponProperties != null)
			{
				_ = weaponProperties.WeaponDefinitionId;
				if (weaponProperties.WeaponDefinition != null && (!weaponProperties.WeaponDefinition.HasAmmoMagazines() || (weaponProperties.WeaponDefinition.AmmoMagazinesId != null && weaponProperties.WeaponDefinition.AmmoMagazinesId.Length != 0)))
				{
					return;
				}
			}
			string text = string.Empty;
			if (weaponProperties == null)
			{
				text += "\nWeaponProperties is missing";
			}
			else
			{
				_ = weaponProperties.WeaponDefinitionId;
				if (weaponProperties.WeaponDefinition == null)
				{
					text += "\nWeapon definition is missing";
				}
				else if (weaponProperties.WeaponDefinition.HasAmmoMagazines() && (weaponProperties.WeaponDefinition.AmmoMagazinesId == null || weaponProperties.WeaponDefinition.AmmoMagazinesId.Length == 0))
				{
					text += "\nWeapon definition say it has ammo magazines defined, but no Ids are assigned.";
				}
			}
			text = $"Weapon definition '{blockId}' is missing important data: {text}";
			MyLog.Default.Error(text);
		}

		private void Weapon_OnClosing(MyEntity obj)
		{
			if (m_user.Weapon != null)
			{
				m_user.Weapon.OnClosing -= Weapon_OnClosing;
			}
		}

		public Vector3 GetDeviatedVector(float deviateAngle, Vector3 direction)
		{
			return MyUtilRandomVector3ByDeviatingVector.GetRandom(direction, deviateAngle);
		}

		private void AddProjectile(MyWeaponPropertiesWrapper weaponProperties, Vector3D initialPosition, Vector3 initialVelocity, Vector3 direction, MyEntity owner)
		{
			Vector3 directionNormalized = direction;
			if (weaponProperties.IsDeviated)
			{
				float deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngle;
				if (owner is MyCharacter && (((MyCharacter)owner).IsCrouching || ((MyCharacter)owner).ZoomMode == MyZoomModeEnum.IronSight))
				{
					deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngleAiming;
				}
				directionNormalized = GetDeviatedVector(deviateAngle, direction);
				directionNormalized.Normalize();
			}
			if (!directionNormalized.IsValid())
			{
				return;
			}
			m_shotProjectiles++;
			if (DEBUG_PROJECTILE_VIEW_TRAJECTORIES)
			{
<<<<<<< HEAD
				MyAdvancedDebugDraw.DebugDrawLine3DSync(initialPosition, initialPosition + 10f * direction, Color.Blue, Color.CornflowerBlue);
=======
				MyAdvancedDebugDraw.DebugDrawLine3DSync(initialPosition, initialPosition + 10.0 * direction, Color.Blue, Color.CornflowerBlue);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!Sync.IsServer)
				{
					MyRenderProxy.DebugDrawLine3D(MySector.MainCamera.Position, MySector.MainCamera.Position + 10f * MySector.MainCamera.ForwardVector, Color.Red, Color.Red, depthRead: true, persistent: true);
				}
			}
			MyProjectiles.Static.Add(weaponProperties, initialPosition, initialVelocity, directionNormalized, m_user, owner);
		}

		private void AddMissile(MyWeaponPropertiesWrapper weaponProperties, Vector3D initialPosition, Vector3 initialVelocity, Vector3 direction, MyEntity controller)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MyMissileAmmoDefinition currentAmmoDefinitionAs = weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
			Vector3 vector = direction;
			if (weaponProperties.IsDeviated)
			{
				float deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngle;
				if (controller is MyCharacter && (((MyCharacter)controller).IsCrouching || ((MyCharacter)controller).ZoomMode == MyZoomModeEnum.IronSight))
				{
					deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngleAiming;
				}
				vector = GetDeviatedVector(deviateAngle, direction);
				vector.Normalize();
			}
			if (vector.IsValid())
			{
				initialVelocity = GetMissileInitialSpeed(initialVelocity, vector, currentAmmoDefinitionAs);
				long owner = 0L;
				IMyControllableEntity myControllableEntity;
				if ((myControllableEntity = controller as IMyControllableEntity) != null && myControllableEntity.ControllerInfo != null)
				{
					owner = myControllableEntity.ControllerInfo.ControllingIdentityId;
				}
				else if (m_user.OwnerId != 0L)
				{
					owner = m_user.OwnerId;
				}
				MyObjectBuilder_Missile builder = MyMissile.PrepareBuilder(weaponProperties, initialPosition, initialVelocity, vector, owner, m_user.Owner.EntityId, (m_user.Launcher as MyEntity).EntityId);
				m_user.Launcher.ShootMissile(builder);
			}
		}

		public static Vector3 GetMissileInitialSpeed(Vector3 initialVelocity, Vector3 vectorToTarget, MyMissileAmmoDefinition missileAmmoDefinition)
		{
			initialVelocity += Vector3.ClampToSphere(vectorToTarget, 1f) * missileAmmoDefinition.MissileInitialSpeed;
			if (initialVelocity.Length() > missileAmmoDefinition.DesiredSpeed)
			{
				Vector3.ClampToSphere(ref initialVelocity, missileAmmoDefinition.DesiredSpeed);
			}
			return initialVelocity;
		}

		public void Shoot(Vector3 initialVelocity, MyEntity owner = null, MyLargeBarrelBase barrel = null)
		{
			MatrixD muzzleWorldMatrix = GetMuzzleWorldMatrix();
			Shoot(muzzleWorldMatrix.Translation, initialVelocity, muzzleWorldMatrix.Forward, owner, barrel);
		}

		public void Shoot(Vector3 initialVelocity, Vector3D Target, MyEntity owner = null, MyLargeBarrelBase barrel = null)
		{
			MatrixD muzzleWorldMatrix = GetMuzzleWorldMatrix();
			Shoot(muzzleWorldMatrix.Translation, initialVelocity, Target - muzzleWorldMatrix.Translation, owner, barrel);
		}

		/// <summary>
		/// This function changes default shooting position (muzzle flash) with an offset.
		/// (allow us to shoot at close objects)
		/// </summary>
		public void ShootWithOffset(Vector3 initialVelocity, Vector3 direction, float offset, MyEntity owner = null, MyLargeBarrelBase barrel = null, bool userNormalizedPositionForEffects = true)
		{
			Shoot(GetMuzzleWorldMatrix().Translation + direction * offset, initialVelocity, direction, owner, barrel, userNormalizedPositionForEffects);
		}

		public void Shoot(Vector3D initialPosition, Vector3 initialVelocity, Vector3 direction, MyEntity owner = null, MyLargeBarrelBase barrel = null, bool userNormalizedPositionForEffects = true)
		{
			MyAmmoDefinition ammoDefinition = m_weaponProperties.AmmoDefinition;
			switch (ammoDefinition.AmmoType)
			{
			case MyAmmoType.HighSpeed:
			{
				int projectileCount = (ammoDefinition as MyProjectileAmmoDefinition).ProjectileCount;
				for (int i = 0; i < projectileCount; i++)
				{
					AddProjectile(m_weaponProperties, initialPosition, initialVelocity, direction, owner);
				}
				CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot, barrel, userNormalizedPositionForEffects);
				break;
			}
			case MyAmmoType.Missile:
				if (Sync.IsServer)
				{
					AddMissile(m_weaponProperties, initialPosition, initialVelocity, direction, owner);
				}
				break;
			}
			LastShootTime = DateTime.UtcNow;
		}

		public void CreateEffects(MyWeaponDefinition.WeaponEffectAction action, MyLargeBarrelBase barrelOfOrigin = null, bool userNormalizedPositionForEffects = true)
		{
			if (WeaponProperties.WeaponDefinition.WeaponEffects.Length == 0)
			{
				return;
			}
			for (int i = 0; i < WeaponProperties.WeaponDefinition.WeaponEffects.Length; i++)
			{
<<<<<<< HEAD
				MyWeaponDefinition.MyWeaponEffect myWeaponEffect = WeaponProperties.WeaponDefinition.WeaponEffects[i];
				if (myWeaponEffect.Action != action || (!m_dummies.TryGetValue(myWeaponEffect.Dummy, out var value) && !(myWeaponEffect.Dummy == "None")) || (myWeaponEffect.DisplayOnlyOnDummyFiring && value.Name != GetCurrentDummyName()))
=======
				if (WeaponProperties.WeaponDefinition.WeaponEffects[i].Action != action || !m_dummies.TryGetValue(WeaponProperties.WeaponDefinition.WeaponEffects[i].Dummy, out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					continue;
				}
				bool flag = true;
				if (myWeaponEffect.Loop)
				{
					for (int j = 0; j < ActiveLoopEffects.Count; j++)
					{
						if (ActiveLoopEffects[j].WeaponEffectDefinition == myWeaponEffect)
						{
							flag = false;
							ActiveLoopEffects[j].RefreshTimer();
							break;
						}
					}
				}
				if (flag)
				{
<<<<<<< HEAD
					uint parentID = uint.MaxValue;
					MyUserControllableGun myUserControllableGun;
					if (barrelOfOrigin != null)
					{
						parentID = barrelOfOrigin.Entity.Render.GetRenderObjectID();
=======
					MatrixD m = MatrixD.Normalize(value.Matrix);
					if (MyParticlesManager.TryCreateParticleEffect(empty, MatrixD.Multiply(m, WorldMatrix), out var effect) && WeaponProperties.WeaponDefinition.WeaponEffects[i].Loop)
					{
						m_activeEffects.Add(new WeaponEffect(empty, value.Name, m, action, effect, WeaponProperties.WeaponDefinition.WeaponEffects[i].InstantStop));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else if (m_user?.Weapon?.Render != null && (myUserControllableGun = m_user as MyUserControllableGun) != null)
					{
						parentID = myUserControllableGun.Render.GetRenderObjectID();
					}
					myWeaponEffect.UseNormalizedPosition = userNormalizedPositionForEffects;
					myWeaponEffect.BarrelOfOrigin = barrelOfOrigin;
					GetEffectPosition(myWeaponEffect, out var matrix, out var position, barrelOfOrigin);
					if (MyParticlesManager.TryCreateParticleEffect(myWeaponEffect.Particle, ref matrix, ref position, parentID, out var effect) && myWeaponEffect.Loop)
					{
						ActiveLoopEffects.Add(new WeaponEffect(matrix, effect, myWeaponEffect));
					}
				}
			}
		}

		private void GetEffectPosition(MyWeaponDefinition.MyWeaponEffect myWeaponEffect, out MatrixD matrix, out Vector3D position, MyLargeBarrelBase barrelOfOrigin = null)
		{
			matrix = default(MatrixD);
			position = default(Vector3D);
			if (myWeaponEffect != null && (m_dummies.TryGetValue(myWeaponEffect.Dummy, out var value) || myWeaponEffect.Dummy == "None"))
			{
				MatrixD matrixD;
				if (barrelOfOrigin != null && !myWeaponEffect.DisplayOnlyOnDummyFiring)
				{
					matrix = MatrixD.Identity;
					position = GetMuzzleWorldPosition();
					matrixD = MatrixD.Identity;
				}
				else if (value != null)
				{
					matrixD = MatrixD.Normalize(value.Matrix);
					matrix = MatrixD.Multiply(matrixD, WorldMatrix);
					position = matrix.Translation;
				}
				else
				{
					matrix = (m_user as MyEntity).WorldMatrix;
					position = (m_user as MyEntity).WorldMatrix.Translation;
					matrixD = MatrixD.Identity;
				}
				matrixD.Translation += myWeaponEffect.Offset;
				position += myWeaponEffect.Offset;
				matrix = (myWeaponEffect.UseNormalizedPosition ? matrixD : matrix);
			}
		}

		public bool HasActiveEffects()
		{
			return ActiveLoopEffects.Count > 0;
		}

		public void UpdateEffects()
		{
			for (int i = 0; i < ActiveLoopEffects.Count; i++)
			{
<<<<<<< HEAD
				if (ActiveLoopEffects[i].Effect.IsStopped)
				{
					ActiveLoopEffects.RemoveAt(i);
					i--;
				}
				else
				{
					ActiveLoopEffects[i].Update();
				}
			}
		}

		public void UpdateEffectPositions()
		{
			for (int i = 0; i < ActiveLoopEffects.Count; i++)
			{
				if (!ActiveLoopEffects[i].Effect.IsStopped)
				{
					GetEffectPosition(ActiveLoopEffects[i].WeaponEffectDefinition, out var matrix, out var position, ActiveLoopEffects[i].WeaponEffectDefinition.BarrelOfOrigin);
					matrix.Translation = position;
					ActiveLoopEffects[i].Effect.WorldMatrix = matrix;
=======
				if (m_activeEffects[i].Effect.IsStopped)
				{
					m_activeEffects.RemoveAt(i);
					i--;
				}
			}
		}

		public void UpdateEffectPositions()
		{
			for (int i = 0; i < m_activeEffects.Count; i++)
			{
				if (!m_activeEffects[i].Effect.IsStopped)
				{
					m_activeEffects[i].Effect.WorldMatrix = MatrixD.Multiply(m_activeEffects[i].LocalMatrix, WorldMatrix);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public void RemoveOldEffects(MyWeaponDefinition.WeaponEffectAction action = MyWeaponDefinition.WeaponEffectAction.Shoot)
		{
			for (int i = 0; i < ActiveLoopEffects.Count; i++)
			{
				if (ActiveLoopEffects[i].WeaponEffectDefinition.Action == action)
				{
<<<<<<< HEAD
					ActiveLoopEffects[i].Effect.Stop(ActiveLoopEffects[i].WeaponEffectDefinition.InstantStop);
					ActiveLoopEffects.RemoveAt(i);
=======
					m_activeEffects[i].Effect.Stop(m_activeEffects[i].InstantStop);
					m_activeEffects.RemoveAt(i);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					i--;
				}
			}
		}

		public void RemoveAllEffects()
		{
			while (ActiveLoopEffects.Count > 0)
			{
				ActiveLoopEffects[0].Effect.Stop(ActiveLoopEffects[0].WeaponEffectDefinition.InstantStop);
				ActiveLoopEffects.RemoveAt(0);
			}
			ActiveLoopEffects.Clear();
		}

		public MyInventoryConstraint CreateAmmoInventoryConstraints(string displayName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(MyTexts.GetString(MySpaceTexts.ToolTipItemFilter_AmmoMagazineInput), displayName);
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(stringBuilder.ToString());
			MyDefinitionId[] ammoMagazinesId = m_weaponProperties.WeaponDefinition.AmmoMagazinesId;
			foreach (MyDefinitionId id in ammoMagazinesId)
			{
				myInventoryConstraint.Add(id);
			}
			return myInventoryConstraint;
		}

		public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineId)
		{
			return WeaponProperties.CanChangeAmmoMagazine(ammoMagazineId);
		}

		public override bool CanSwitchAmmoMagazine()
		{
			if (m_weaponProperties != null)
			{
				return m_weaponProperties.WeaponDefinition.HasAmmoMagazines();
			}
			return false;
		}

		public bool SwitchAmmoMagazine(MyDefinitionId ammoMagazineId)
		{
			m_remainingAmmos[CurrentAmmoMagazineId] = CurrentAmmo;
			WeaponProperties.ChangeAmmoMagazine(ammoMagazineId);
			int value = 0;
			m_remainingAmmos.TryGetValue(ammoMagazineId, out value);
			CurrentAmmo = value;
			RefreshAmmunitionAmount();
			return ammoMagazineId == WeaponProperties.AmmoMagazineId;
		}

		public override bool SwitchAmmoMagazineToNextAvailable()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			if (!weaponDefinition.HasAmmoMagazines())
			{
				return false;
			}
			int ammoMagazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(CurrentAmmoMagazineId);
			int num = weaponDefinition.AmmoMagazinesId.Length;
			int num2 = ammoMagazineIdArrayIndex + 1;
			for (int i = 0; i != num; i++)
			{
				if (num2 == num)
				{
					num2 = 0;
				}
				if (weaponDefinition.AmmoMagazinesId[num2].SubtypeId != CurrentAmmoMagazineId.SubtypeId)
				{
					if (MySession.Static.InfiniteAmmo)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
					int value = 0;
					if (m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[num2], out value) && value > 0)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
					if (m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[num2]) > 0)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
				}
				num2++;
			}
			return false;
		}

		public override bool SwitchToNextAmmoMagazine()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			int ammoMagazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(CurrentAmmoMagazineId);
			int num = weaponDefinition.AmmoMagazinesId.Length;
			ammoMagazineIdArrayIndex++;
			if (ammoMagazineIdArrayIndex == num)
			{
				ammoMagazineIdArrayIndex = 0;
			}
			return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[ammoMagazineIdArrayIndex]);
		}

		public bool SwitchAmmoMagazineToFirstAvailable()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			for (int i = 0; i < WeaponProperties.AmmoMagazinesCount; i++)
			{
				int value = 0;
				if (m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[i], out value) && value > 0)
				{
					return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[i]);
				}
				if (m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[i]) > 0)
				{
					return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[i]);
				}
			}
			return false;
		}

		public bool HasEnoughMagazines()
		{
			if (MySession.Static.InfiniteAmmo)
			{
				return true;
			}
			if (!Sync.IsServer)
			{
				return CurrentMagazines > 0;
			}
			if (m_user == null || m_user.AmmoInventory == null)
			{
				return false;
			}
			return m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId) > 0;
		}

		public bool HasEnoughAmmunition()
		{
			if (MySession.Static.InfiniteAmmo)
			{
				return true;
			}
			if (!Sync.IsServer)
			{
				return CurrentAmmo > 0;
			}
			if (CurrentAmmo < 1)
			{
				if (m_user == null || m_user.AmmoInventory == null)
				{
					string msg = string.Format("Error: {0} should not be null!", (m_user == null) ? "User" : "AmmoInventory");
					MyLog.Default.WriteLine(msg);
					return false;
				}
				return m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId) > 0;
			}
			return true;
		}

		public bool IsAmmoFull()
		{
			return CurrentAmmo == WeaponProperties.AmmoMagazineDefinition.Capacity;
		}

		public void EmptyMagazine()
		{
			CurrentAmmo = 0;
			SaveAmmoCount();
		}

		public void ConsumeMagazine()
		{
			if (MySession.Static.SimplifiedSimulation)
			{
				if (Sync.IsServer && HasEnoughMagazines())
				{
					CurrentAmmo = WeaponProperties.AmmoMagazineDefinition.Capacity;
				}
				return;
			}
			if (Sync.IsServer && HasEnoughMagazines())
			{
				CurrentAmmo = WeaponProperties.AmmoMagazineDefinition.Capacity;
				if (!MySession.Static.InfiniteAmmo)
				{
					m_user.AmmoInventory.RemoveItemsOfType(1, CurrentAmmoMagazineId);
				}
			}
			SaveAmmoCount();
		}

		public void ConsumeAmmo()
		{
			if (MySession.Static.SimplifiedSimulation)
			{
				return;
			}
			if (Sync.IsServer)
			{
				CurrentAmmo--;
				if (CurrentAmmo < 0 && HasEnoughAmmunition())
				{
					CurrentAmmo = WeaponProperties.AmmoMagazineDefinition.Capacity - 1;
					if (!MySession.Static.InfiniteAmmo)
					{
						m_user.AmmoInventory.RemoveItemsOfType(1, CurrentAmmoMagazineId);
					}
				}
				RefreshAmmunitionAmount();
			}
			SaveAmmoCount();
		}

		private void SaveAmmoCount()
		{
			MyInventory ammoInventory = m_user.AmmoInventory;
			if (ammoInventory == null)
			{
				return;
			}
			MyPhysicalInventoryItem? myPhysicalInventoryItem = null;
			if (base.InventoryItemId.HasValue)
			{
				myPhysicalInventoryItem = ammoInventory.GetItemByID(base.InventoryItemId.Value);
			}
			else
			{
				myPhysicalInventoryItem = ammoInventory.FindUsableItem(m_user.PhysicalItemId);
				if (myPhysicalInventoryItem.HasValue)
				{
					base.InventoryItemId = myPhysicalInventoryItem.Value.ItemId;
				}
			}
			if (!myPhysicalInventoryItem.HasValue)
			{
				return;
			}
			MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject = myPhysicalInventoryItem.Value.Content as MyObjectBuilder_PhysicalGunObject;
			if (myObjectBuilder_PhysicalGunObject == null)
			{
				return;
			}
			IMyObjectBuilder_GunObject<MyObjectBuilder_GunBase> myObjectBuilder_GunObject = myObjectBuilder_PhysicalGunObject.GunEntity as IMyObjectBuilder_GunObject<MyObjectBuilder_GunBase>;
			if (myObjectBuilder_GunObject != null)
			{
				if (myObjectBuilder_GunObject.DeviceBase == null)
				{
					myObjectBuilder_GunObject.InitializeDeviceBase(GetObjectBuilder());
				}
				else
				{
					myObjectBuilder_GunObject.GetDevice().RemainingAmmo = CurrentAmmo;
				}
			}
		}

		public void StopShoot()
		{
			m_shotProjectiles = 0;
		}

		public int GetTotalAmmunitionAmount()
		{
			return CurrentAmmo + CurrentMagazines * CurrentAmmoMagazineDefinition.Capacity;
		}

		public int GetAmmunitionAmount()
		{
			return CurrentAmmo;
		}

		public int GetMagazineAmount()
		{
			return CurrentMagazines;
		}

		public int GetInventoryAmmoMagazinesCount()
		{
			return (int)m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId);
		}

		public void RefreshAmmunitionAmount(bool forceUpdate = false)
		{
			if (!Sync.IsServer && !forceUpdate)
			{
				return;
			}
			if (MySession.Static.InfiniteAmmo)
			{
				CurrentMagazines = 0;
			}
			else if (m_user != null && m_user.AmmoInventory != null && m_weaponProperties.WeaponDefinition.HasAmmoMagazines())
			{
				if (!HasEnoughAmmunition())
				{
					SwitchAmmoMagazineToFirstAvailable();
				}
				CurrentMagazines = (int)m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId);
			}
			else
			{
				CurrentAmmo = 0;
				CurrentMagazines = 0;
			}
		}

		private MyDefinitionId GetBackwardCompatibleDefinitionId(MyObjectBuilderType typeId)
		{
			if (typeId == typeof(MyObjectBuilder_LargeGatlingTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeGatlingTurret");
			}
			if (typeId == typeof(MyObjectBuilder_LargeMissileTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeMissileTurret");
			}
			if (typeId == typeof(MyObjectBuilder_InteriorTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeInteriorTurret");
			}
			if (typeId == typeof(MyObjectBuilder_SmallMissileLauncher) || typeId == typeof(MyObjectBuilder_SmallMissileLauncherReload))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "SmallMissileLauncher");
			}
			if (typeId == typeof(MyObjectBuilder_SmallGatlingGun))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "GatlingGun");
			}
			return default(MyDefinitionId);
		}

		public void AddMuzzleMatrix(MyAmmoType ammoType, Matrix localMatrix, string dummyName = "")
		{
			if (!m_dummiesByAmmoType.ContainsKey((int)ammoType))
			{
				m_dummiesByAmmoType[(int)ammoType] = new DummyContainer();
			}
			m_dummiesByAmmoType[(int)ammoType].Dummies.Add(new KeyValuePair<string, MatrixD>(dummyName, MatrixD.Normalize(localMatrix)));
		}

		public void LoadDummies(Dictionary<string, MyModelDummy> dummies, Dictionary<int, string> dummyKeys)
		{
			m_dummies = dummies;
			m_dummiesByAmmoType.Clear();
			foreach (KeyValuePair<string, MyModelDummy> dummy in dummies)
			{
				if (DummyNameCheck(dummyKeys, DUMMY_KEY_PROJECTILE, dummy.Key, "muzzle_projectile"))
				{
					AddMuzzleMatrix(MyAmmoType.HighSpeed, dummy.Value.Matrix, dummy.Key);
					m_holdingDummyMatrix = Matrix.Invert(Matrix.CreateScale(1f / dummy.Value.Matrix.Scale) * dummy.Value.Matrix);
				}
				if (DummyNameCheck(dummyKeys, DUMMY_KEY_MISSILE, dummy.Key, "muzzle_missile"))
				{
					AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix, dummy.Key);
				}
				if (DummyNameCheck(dummyKeys, DUMMY_KEY_HOLDING, dummy.Key, "holding_dummy") || dummy.Key.ToLower().Contains("holdingdummy"))
				{
					m_holdingDummyMatrix = Matrix.Normalize(dummy.Value.Matrix);
				}
			}
		}

		private bool DummyNameCheck(Dictionary<int, string> dummyNames, int key, string dummyName, string defaultValue)
		{
			if (dummyNames != null && dummyNames.TryGetValue(DUMMY_KEY_MISSILE, out var value) && dummyName.ToLower().Contains(value))
			{
				return true;
			}
			return dummyName.ToLower().Contains(defaultValue);
		}

		public override Vector3D GetMuzzleLocalPosition()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return Vector3D.Zero;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out var value))
			{
				return value.DummyToUse.Translation;
			}
			return Vector3D.Zero;
		}

		public string GetCurrentDummyName()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return string.Empty;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out var value))
			{
				return value.DummyToUseName;
			}
			return string.Empty;
		}

		public MatrixD GetMuzzleLocalMatrix()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return MatrixD.Identity;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out var value))
			{
				return value.DummyToUse;
			}
			return MatrixD.Identity;
		}

		public override Vector3D GetMuzzleWorldPosition()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return m_worldMatrix.Translation;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out var value))
			{
				if (value.Dirty)
				{
					value.DummyInWorld = value.DummyToUse * m_worldMatrix;
					value.Dirty = false;
				}
				return value.DummyInWorld.Translation;
			}
			return m_worldMatrix.Translation;
		}

		public MatrixD GetMuzzleWorldMatrix()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return m_worldMatrix;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out var value))
			{
				if (value.Dirty)
				{
					value.DummyInWorld = value.DummyToUse * m_worldMatrix;
					value.Dirty = false;
				}
				return value.DummyInWorld;
			}
			return m_worldMatrix;
		}

		public void MoveToNextMuzzle(MyAmmoType ammoType)
		{
			if (m_dummiesByAmmoType.TryGetValue((int)ammoType, out var value) && value.Dummies.Count > 1)
			{
				value.DummyIndex++;
				if (value.DummyIndex == value.Dummies.Count)
				{
					value.DummyIndex = 0;
				}
				value.Dirty = true;
			}
		}

		private void RecalculateMuzzles()
		{
			foreach (DummyContainer value in m_dummiesByAmmoType.Values)
			{
				value.Dirty = true;
			}
		}

		public void StartShootSound(MyEntity3DSoundEmitter soundEmitter, bool force2D = false)
		{
			if (ShootSound == null || soundEmitter == null)
			{
				return;
			}
			if (soundEmitter.IsPlaying)
			{
				if (!soundEmitter.Loop)
				{
					soundEmitter.PlaySound(ShootSound, stopPrevious: false, skipIntro: false, force2D);
				}
			}
			else
			{
				soundEmitter.PlaySound(ShootSound, stopPrevious: true, skipIntro: false, force2D);
			}
		}

		public void StartPreShotSound(MyEntity3DSoundEmitter soundEmitter, bool force2D = false)
		{
			if (PreShotSound == null || soundEmitter == null)
			{
				return;
			}
			if (soundEmitter.IsPlaying)
			{
				if (!soundEmitter.Loop)
				{
					soundEmitter.PlaySound(PreShotSound, stopPrevious: false, skipIntro: false, force2D);
				}
			}
			else
			{
				soundEmitter.PlaySound(PreShotSound, stopPrevious: true, skipIntro: false, force2D);
			}
		}

		internal void StartNoAmmoSound(MyEntity3DSoundEmitter soundEmitter)
		{
			if (NoAmmoSound != null && soundEmitter != null)
			{
				soundEmitter.StopSound(forced: true);
				soundEmitter.PlaySingleSound(NoAmmoSound, stopPrevious: true);
			}
		}

		internal void StartReloadSound(MyEntity3DSoundEmitter soundEmitter)
		{
			if (ReloadSound != null && soundEmitter != null)
			{
				soundEmitter.StopSound(forced: true);
				soundEmitter.PlaySingleSound(ReloadSound, stopPrevious: true);
			}
		}
	}
}
