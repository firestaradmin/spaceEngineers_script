using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProjectileAmmoDefinition), null)]
	public class MyProjectileAmmoDefinition : MyAmmoDefinition
	{
		private class Sandbox_Definitions_MyProjectileAmmoDefinition_003C_003EActor : IActivator, IActivator<MyProjectileAmmoDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProjectileAmmoDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProjectileAmmoDefinition CreateInstance()
			{
				return new MyProjectileAmmoDefinition();
			}

			MyProjectileAmmoDefinition IActivator<MyProjectileAmmoDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float ProjectileHitImpulse;

		public float ProjectileTrailScale;

		public Vector3 ProjectileTrailColor;

		public string ProjectileTrailMaterial;

		public float ProjectileTrailProbability;

		public string ProjectileOnHitEffectName;

		public float ProjectileMassDamage;

		public float ProjectileHealthDamage;

		public bool HeadShot;

		public float ProjectileHeadShotDamage;

		/// <summary>
		/// Number of pellets (shotgun)
		/// </summary>
		public int ProjectileCount;

		public float ProjectileExplosionRadius;

		public float ProjectileExplosionDamage;

		public MyStringId ProjectileTrailMaterialId { get; set; }

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProjectileAmmoDefinition obj = builder as MyObjectBuilder_ProjectileAmmoDefinition;
			AmmoType = MyAmmoType.HighSpeed;
			MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties projectileProperties = obj.ProjectileProperties;
			ProjectileHealthDamage = projectileProperties.ProjectileHealthDamage;
			ProjectileHitImpulse = projectileProperties.ProjectileHitImpulse;
			ProjectileMassDamage = projectileProperties.ProjectileMassDamage;
			ProjectileOnHitEffectName = projectileProperties.ProjectileOnHitEffectName;
			ProjectileTrailColor = projectileProperties.ProjectileTrailColor;
			ProjectileTrailMaterial = projectileProperties.ProjectileTrailMaterial;
			ProjectileTrailMaterialId = MyStringId.GetOrCompute(ProjectileTrailMaterial);
			ProjectileTrailProbability = projectileProperties.ProjectileTrailProbability;
			ProjectileTrailScale = projectileProperties.ProjectileTrailScale;
			HeadShot = projectileProperties.HeadShot;
			ProjectileHeadShotDamage = projectileProperties.ProjectileHeadShotDamage;
			ProjectileCount = projectileProperties.ProjectileCount;
			ProjectileExplosionRadius = projectileProperties.ProjectileExplosionRadius;
			ProjectileExplosionDamage = projectileProperties.ProjectileExplosionDamage;
		}

		public override float GetDamageForMechanicalObjects()
		{
			return ProjectileMassDamage;
		}
	}
}
