using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
<<<<<<< HEAD
using Sandbox.Game.Weapons.Guns.Barrels;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeaponDefinition), null)]
	public class MyWeaponDefinition : MyDefinitionBase
	{
		public class MyWeaponAmmoData
		{
			public int RateOfFire;

			public int ShotsInBurst;

			public MySoundPair ShootSound;

			public MySoundPair PreShotSound;

			public MySoundPair FlightSound;

			public int ShootIntervalInMiliseconds;

			public MyWeaponAmmoData(MyObjectBuilder_WeaponDefinition.WeaponAmmoData data)
				: this(data.RateOfFire, data.ShootSoundName, data.PreShotSoundName, data.ShotsInBurst, data.FlightSoundName)
			{
			}

			public MyWeaponAmmoData(int rateOfFire, string soundName, string preShotSoundName, int shotsInBurst)
				: this(rateOfFire, soundName, preShotSoundName, shotsInBurst, null)
			{
			}

			public MyWeaponAmmoData(int rateOfFire, string soundName, string preShotSoundName, int shotsInBurst, string flightSoundName)
			{
				RateOfFire = rateOfFire;
				ShotsInBurst = shotsInBurst;
				ShootSound = new MySoundPair(soundName);
				PreShotSound = new MySoundPair(preShotSoundName);
				FlightSound = new MySoundPair(flightSoundName);
				ShootIntervalInMiliseconds = (int)(1000f / ((float)RateOfFire / 60f));
			}
		}

		public enum WeaponEffectAction
		{
			Unknown,
			Shoot,
			BeforeShoot
		}

		public class MyWeaponEffect
		{
			public WeaponEffectAction Action;

			public string Dummy = "";

			public string Particle = "";

			public bool Loop;

			public bool InstantStop;

			public bool DisplayOnlyOnDummyFiring;

			public bool UseNormalizedPosition = true;

			public float ParticleBirthStart = 1f;

			public float ParticleBirthMax = 1f;

			public float ParticleBirthMin = 1f;

			public float ParticleBirthIncrease = 1f;

			public float ParticleBirthDecrease = 1f;

			public MyLargeBarrelBase BarrelOfOrigin;

			public Vector3 Offset;

			public MyWeaponEffect(string action, string dummy, string particle, bool loop, bool instantStop, Vector3 offset, float particleBirthStart, float particleBirthMin, float particleBirthMax, float particleBirthIncrease, float particleBirthDecrease, bool displayOnlyOnDummyFiring)
			{
				Dummy = dummy;
				Particle = particle;
				Loop = loop;
				InstantStop = instantStop;
				Offset = offset;
				ParticleBirthStart = particleBirthStart;
				ParticleBirthMin = particleBirthMin;
				ParticleBirthMax = particleBirthMax;
				ParticleBirthIncrease = particleBirthIncrease;
				ParticleBirthDecrease = particleBirthDecrease;
				DisplayOnlyOnDummyFiring = displayOnlyOnDummyFiring;
				foreach (WeaponEffectAction value in Enum.GetValues(typeof(WeaponEffectAction)))
				{
					if (value.ToString().Equals(action))
					{
						Action = value;
						break;
					}
				}
			}
		}

		private class Sandbox_Definitions_MyWeaponDefinition_003C_003EActor : IActivator, IActivator<MyWeaponDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeaponDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeaponDefinition CreateInstance()
			{
				return new MyWeaponDefinition();
			}

			MyWeaponDefinition IActivator<MyWeaponDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly string ErrorMessageTemplate = "No weapon ammo data specified for {0} ammo (<{1}AmmoData> tag is missing in weapon definition)";

		public MySoundPair NoAmmoSound;

		public MySoundPair ReloadSound;

		public MySoundPair SecondarySound;

		public float DeviateShotAngle;

		public float DeviateShotAngleAiming;

		public float ReleaseTimeAfterFire;

		public int MuzzleFlashLifeSpan;

		public MyDefinitionId[] AmmoMagazinesId;

		public MyWeaponAmmoData[] WeaponAmmoDatas;

		public MyWeaponEffect[] WeaponEffects;

		public MyStringHash PhysicalMaterial;

		public bool UseDefaultMuzzleFlash;

		public int ReloadTime = 2000;

		public float DamageMultiplier = 1f;

		public float RangeMultiplier = 1f;

		public bool UseRandomizedRange = true;

<<<<<<< HEAD
		public int MinimumTimeBetweenIdleRotationsMs = 3500;

		public int MaximumTimeBetweenIdleRotationsMs = 4500;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float RecoilJetpackVertical;

		public float RecoilJetpackHorizontal;

		public float RecoilGroundVertical;

		public float RecoilGroundHorizontal;

		public float RecoilResetTimeMilliseconds;

		public int ShootDirectionUpdateTime = 200;

		public float EquipDuration = 0.5f;

<<<<<<< HEAD
		/// <summary>
		/// Working only for MissileLauncher
		/// </summary>
		public int ShotDelay;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Dictionary<MyShootActionEnum, bool> ShakeOnAction = new Dictionary<MyShootActionEnum, bool>();

		public Dictionary<string, Tuple<float, float>> RecoilMultiplierData = new Dictionary<string, Tuple<float, float>>();

		public bool HasProjectileAmmoDefined => WeaponAmmoDatas[0] != null;

		public bool HasMissileAmmoDefined => WeaponAmmoDatas[1] != null;

		public bool HasSpecificAmmoData(MyAmmoDefinition ammoDefinition)
		{
			return WeaponAmmoDatas[(int)ammoDefinition.AmmoType] != null;
		}

		public bool HasAmmoMagazines()
		{
			if (AmmoMagazinesId != null)
			{
				return AmmoMagazinesId.Length != 0;
			}
			return false;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeaponDefinition myObjectBuilder_WeaponDefinition = builder as MyObjectBuilder_WeaponDefinition;
			WeaponAmmoDatas = new MyWeaponAmmoData[Enum.GetValues(typeof(MyAmmoType)).Length];
			WeaponEffects = new MyWeaponEffect[(myObjectBuilder_WeaponDefinition.Effects != null) ? myObjectBuilder_WeaponDefinition.Effects.Length : 0];
			if (myObjectBuilder_WeaponDefinition.Effects != null)
			{
				for (int i = 0; i < myObjectBuilder_WeaponDefinition.Effects.Length; i++)
				{
					MyObjectBuilder_WeaponDefinition.WeaponEffect weaponEffect = myObjectBuilder_WeaponDefinition.Effects[i];
					WeaponEffects[i] = new MyWeaponEffect(weaponEffect.Action, weaponEffect.Dummy, weaponEffect.Particle, weaponEffect.Loop, weaponEffect.InstantStop, new Vector3(weaponEffect.OffsetX, weaponEffect.OffsetY, weaponEffect.OffsetZ), weaponEffect.ParticleBirthStart, weaponEffect.ParticleBirthMin, weaponEffect.ParticleBirthMax, weaponEffect.ParticleBirthIncrease, weaponEffect.ParticleBirthDecrease, weaponEffect.DisplayOnlyOnDummyFiring);
				}
			}
			PhysicalMaterial = MyStringHash.GetOrCompute(myObjectBuilder_WeaponDefinition.PhysicalMaterial);
			UseDefaultMuzzleFlash = myObjectBuilder_WeaponDefinition.UseDefaultMuzzleFlash;
			NoAmmoSound = new MySoundPair(myObjectBuilder_WeaponDefinition.NoAmmoSoundName);
			ReloadSound = new MySoundPair(myObjectBuilder_WeaponDefinition.ReloadSoundName);
			SecondarySound = new MySoundPair(myObjectBuilder_WeaponDefinition.SecondarySoundName);
			DeviateShotAngle = MathHelper.ToRadians(myObjectBuilder_WeaponDefinition.DeviateShotAngle);
			DeviateShotAngleAiming = MathHelper.ToRadians(myObjectBuilder_WeaponDefinition.DeviateShotAngleAiming);
<<<<<<< HEAD
			ShotDelay = myObjectBuilder_WeaponDefinition.ShotDelay;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ReleaseTimeAfterFire = myObjectBuilder_WeaponDefinition.ReleaseTimeAfterFire;
			MuzzleFlashLifeSpan = myObjectBuilder_WeaponDefinition.MuzzleFlashLifeSpan;
			ReloadTime = myObjectBuilder_WeaponDefinition.ReloadTime;
			DamageMultiplier = myObjectBuilder_WeaponDefinition.DamageMultiplier;
			RangeMultiplier = myObjectBuilder_WeaponDefinition.RangeMultiplier;
			UseRandomizedRange = myObjectBuilder_WeaponDefinition.UseRandomizedRange;
<<<<<<< HEAD
			MinimumTimeBetweenIdleRotationsMs = myObjectBuilder_WeaponDefinition.MinimumTimeBetweenIdleRotationsMs;
			MaximumTimeBetweenIdleRotationsMs = myObjectBuilder_WeaponDefinition.MaximumTimeBetweenIdleRotationsMs;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RecoilJetpackVertical = myObjectBuilder_WeaponDefinition.RecoilJetpackVertical;
			RecoilJetpackHorizontal = myObjectBuilder_WeaponDefinition.RecoilJetpackHorizontal;
			RecoilGroundVertical = myObjectBuilder_WeaponDefinition.RecoilGroundVertical;
			RecoilGroundHorizontal = myObjectBuilder_WeaponDefinition.RecoilGroundHorizontal;
			RecoilResetTimeMilliseconds = myObjectBuilder_WeaponDefinition.RecoilResetTimeMilliseconds;
			ShootDirectionUpdateTime = myObjectBuilder_WeaponDefinition.ShootDirectionUpdateTime;
			EquipDuration = myObjectBuilder_WeaponDefinition.EquipDuration;
			ShakeOnAction.Add(MyShootActionEnum.PrimaryAction, myObjectBuilder_WeaponDefinition.ShakeOnActionPrimary);
			ShakeOnAction.Add(MyShootActionEnum.SecondaryAction, myObjectBuilder_WeaponDefinition.ShakeOnActionSecondary);
			ShakeOnAction.Add(MyShootActionEnum.TertiaryAction, myObjectBuilder_WeaponDefinition.ShakeOnActionTertiary);
			AmmoMagazinesId = new MyDefinitionId[myObjectBuilder_WeaponDefinition.AmmoMagazines.Length];
			for (int j = 0; j < AmmoMagazinesId.Length; j++)
			{
				MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine weaponAmmoMagazine = myObjectBuilder_WeaponDefinition.AmmoMagazines[j];
				AmmoMagazinesId[j] = new MyDefinitionId(weaponAmmoMagazine.Type, weaponAmmoMagazine.Subtype);
				MyAmmoMagazineDefinition ammoMagazineDefinition = MyDefinitionManager.Static.GetAmmoMagazineDefinition(AmmoMagazinesId[j]);
				MyAmmoType ammoType = MyDefinitionManager.Static.GetAmmoDefinition(ammoMagazineDefinition.AmmoDefinitionId).AmmoType;
				string text = null;
				switch (ammoType)
				{
				case MyAmmoType.HighSpeed:
					if (myObjectBuilder_WeaponDefinition.ProjectileAmmoData != null)
					{
						WeaponAmmoDatas[0] = new MyWeaponAmmoData(myObjectBuilder_WeaponDefinition.ProjectileAmmoData);
					}
					else
					{
						text = string.Format(ErrorMessageTemplate, "projectile", "Projectile");
					}
					break;
				case MyAmmoType.Missile:
					if (myObjectBuilder_WeaponDefinition.MissileAmmoData != null)
					{
						WeaponAmmoDatas[1] = new MyWeaponAmmoData(myObjectBuilder_WeaponDefinition.MissileAmmoData);
					}
					else
					{
						text = string.Format(ErrorMessageTemplate, "missile", "Missile");
					}
					break;
				default:
					throw new NotImplementedException();
				}
				if (!string.IsNullOrEmpty(text))
				{
					MyDefinitionErrors.Add(Context, text, TErrorSeverity.Critical);
				}
			}
			if (myObjectBuilder_WeaponDefinition.RecoilMultiplierDataNames.Count == 0 || myObjectBuilder_WeaponDefinition.RecoilMultiplierDataNames.Count != myObjectBuilder_WeaponDefinition.RecoilMultiplierDataVerticals.Count || myObjectBuilder_WeaponDefinition.RecoilMultiplierDataVerticals.Count != myObjectBuilder_WeaponDefinition.RecoilMultiplierDataHorizontals.Count)
			{
				return;
			}
			RecoilMultiplierData.Clear();
			for (int k = 0; k < myObjectBuilder_WeaponDefinition.RecoilMultiplierDataNames.Count; k++)
			{
				if (!RecoilMultiplierData.ContainsKey(myObjectBuilder_WeaponDefinition.RecoilMultiplierDataNames[k]))
				{
					RecoilMultiplierData.Add(myObjectBuilder_WeaponDefinition.RecoilMultiplierDataNames[k], new Tuple<float, float>(myObjectBuilder_WeaponDefinition.RecoilMultiplierDataVerticals[k], myObjectBuilder_WeaponDefinition.RecoilMultiplierDataHorizontals[k]));
				}
			}
		}

		public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineDefinitionId)
		{
			for (int i = 0; i < AmmoMagazinesId.Length; i++)
			{
				if (ammoMagazineDefinitionId.SubtypeId == AmmoMagazinesId[i].SubtypeId)
				{
					return true;
				}
			}
			return false;
		}

		public int GetAmmoMagazineIdArrayIndex(MyDefinitionId ammoMagazineId)
		{
			for (int i = 0; i < AmmoMagazinesId.Length; i++)
			{
				if (ammoMagazineId.SubtypeId == AmmoMagazinesId[i].SubtypeId)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
