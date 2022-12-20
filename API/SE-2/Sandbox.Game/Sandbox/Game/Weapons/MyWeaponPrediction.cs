using System;
using Sandbox.Definitions;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public static class MyWeaponPrediction
	{
		/// <summary>
		/// Algorithm to predict the position of the target
		/// </summary>
		public static bool GetPredictedTargetPosition(MyGunBase gun, MyEntity shooter, MyEntity target, out Vector3D predictedPosition, out float timeToHit, float shootDelay = 0f)
		{
			if (target == null || target.PositionComp == null || shooter == null || shooter.PositionComp == null)
			{
				predictedPosition = Vector3.Zero;
				timeToHit = 0f;
				return false;
			}
<<<<<<< HEAD
			Vector3D center = target.PositionComp.WorldAABB.Center;
			Vector3D muzzleWorldPosition = gun.GetMuzzleWorldPosition();
			Vector3 vector = center - muzzleWorldPosition;
			Vector3 vector2 = Vector3.Zero;
			if (target.Physics != null)
			{
				vector2 = target.Physics.LinearVelocity;
			}
			Vector3 vector3 = Vector3.Zero;
			if (shooter.Physics != null)
			{
				vector3 = shooter.Physics.LinearVelocity;
			}
			Vector3 vector4 = vector2 - vector3;
			float projectileSpeed = GetProjectileSpeed(gun);
			float num = vector4.LengthSquared() - projectileSpeed * projectileSpeed;
			float num2 = 2f * Vector3.Dot(vector4, vector);
			float num3 = vector.LengthSquared();
=======
			Vector3 vector = target.PositionComp.WorldAABB.Center;
			Vector3 vector2 = gun.GetMuzzleWorldPosition();
			Vector3 vector3 = vector - vector2;
			Vector3 vector4 = Vector3.Zero;
			if (target.Physics != null)
			{
				vector4 = target.Physics.LinearVelocity;
			}
			Vector3 vector5 = Vector3.Zero;
			if (shooter.Physics != null)
			{
				vector5 = shooter.Physics.LinearVelocity;
			}
			Vector3 vector6 = vector4 - vector5;
			float projectileSpeed = GetProjectileSpeed(gun);
			float num = vector6.LengthSquared() - projectileSpeed * projectileSpeed;
			float num2 = 2f * Vector3.Dot(vector6, vector3);
			float num3 = vector3.LengthSquared();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			float num4 = (0f - num2) / (2f * num);
			float num5 = (float)Math.Sqrt(num2 * num2 - 4f * num * num3) / (2f * num);
			float num6 = num4 - num5;
			float num7 = num4 + num5;
			float num8 = ((!(num6 > num7) || !(num7 > 0f)) ? num6 : num7);
			num8 += shootDelay;
<<<<<<< HEAD
			predictedPosition = center + vector4 * num8;
			timeToHit = (float)(predictedPosition - muzzleWorldPosition).Length() / projectileSpeed;
=======
			predictedPosition = vector + vector6 * num8;
			timeToHit = (predictedPosition - vector2).Length() / projectileSpeed;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return true;
		}

		public static float GetProjectileSpeed(MyGunBase gun)
		{
			if (gun == null)
			{
				return 0f;
			}
			float result = 0f;
			if (gun.CurrentAmmoMagazineDefinition != null)
			{
				result = MyDefinitionManager.Static.GetAmmoDefinition(gun.CurrentAmmoMagazineDefinition.AmmoDefinitionId).DesiredSpeed;
			}
			return result;
		}
	}
}
