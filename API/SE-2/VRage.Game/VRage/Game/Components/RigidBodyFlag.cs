using System;

namespace VRage.Game.Components
{
	[Flags]
	public enum RigidBodyFlag
	{
		/// <summary>
		/// Default flag
		/// </summary>
		RBF_DEFAULT = 0x0,
		/// <summary>
		/// Rigid body is kinematic (has to be updated (matrix) per frame, velocity etc is then computed..)
		/// Changing:
		/// MotionType = HkMotionType.Keyframed;
		/// QualityType = HkCollidableQualityType.Keyframed;
		/// </summary>
		RBF_KINEMATIC = 0x2,
		/// <summary>
		/// Rigid body is static, and colliding with it, won't move it (in Havok and SE worlds)
		/// Changing:
		/// MotionType = HkMotionType.Fixed;
		/// QualityType = HkCollidableQualityType.Fixed;
		/// </summary>
		RBF_STATIC = 0x4,
		/// <summary>
		/// Rigid body has no collision response. Entities marked with this flag would not update position after Havok physics update.
		/// However you still can get position from Havok with code `Physics.GetWorldMatrix`
		/// Changing:
		/// MotionType = HkMotionType.Fixed;
		/// QualityType = HkCollidableQualityType.Fixed;
		/// </summary>
		RBF_DISABLE_COLLISION_RESPONSE = 0x40,
		/// <summary>
		/// Used for moving objects with high quality of simulation
		/// Changing:
		/// MotionType = HkMotionType.Dynamic;
		/// QualityType = HkCollidableQualityType.Moving;
		/// </summary>
		RBF_DOUBLED_KINEMATIC = 0x80,
		/// <summary>
		/// Used for fast moving objects
		/// Changing:
		/// MotionType = HkMotionType.Dynamic;
		/// QualityType = HkCollidableQualityType.Bullet;
		/// </summary>
		RBF_BULLET = 0x100,
		/// <summary>
		/// Used for low quality physics
		/// Changing:
		/// MotionType = HkMotionType.Dynamic;
		/// QualityType = HkCollidableQualityType.Debris;
		/// SolverDeactivation = HkSolverDeactivation.Max;
		/// </summary>
		RBF_DEBRIS = 0x200,
		/// <summary>
		/// Changing:
		/// MotionType = HkMotionType.Keyframed;
		/// QualityType = HkCollidableQualityType.KeyframedReporting;
		/// </summary>
		RBF_KEYFRAMED_REPORTING = 0x400,
<<<<<<< HEAD
		/// <summary>
		/// Making maximum Velocity of entity to x10 of Large/Small ShipMaxLinearVelocity. Used for simulation of very fast moving objects
		/// </summary>
		RBF_UNLOCKED_SPEEDS = 0x800,
		/// <summary>
		/// Don't update entity position from havok after simulation
		/// </summary>
=======
		RBF_UNLOCKED_SPEEDS = 0x800,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		RBF_NO_POSITION_UPDATES = 0x1000
	}
}
