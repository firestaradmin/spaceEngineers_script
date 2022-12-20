using System;
using Havok;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	[Obsolete("Use IMyPhysics instead", false)]
	public static class MyPhysicsHelper
	{
		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitSpherePhysics(this IMyEntity entity, MyStringHash materialType, Vector3 sphereCenter, float sphereRadius, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(sphereRadius, mass);
			HkSphereShape hkSphereShape = new HkSphereShape(sphereRadius);
			myPhysicsBody.CreateFromCollisionObject(hkSphereShape, sphereCenter, entity.PositionComp.WorldMatrixRef, value);
			hkSphereShape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitSpherePhysics(this IMyEntity entity, MyStringHash materialType, MyModel model, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			entity.InitSpherePhysics(materialType, model.BoundingSphere.Center, model.BoundingSphere.Radius, mass, linearDamping, angularDamping, collisionLayer, rbFlag);
		}

		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitBoxPhysics(this IMyEntity entity, MyStringHash materialType, Vector3 center, Vector3 size, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkBoxShape hkBoxShape = new HkBoxShape(size * 0.5f);
			myPhysicsBody.CreateFromCollisionObject(hkBoxShape, center, entity.PositionComp.WorldMatrixRef, value);
			hkBoxShape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		[Obsolete("Use IMyPhysics instead", false)]
		internal static void InitBoxPhysics(this IMyEntity entity, Matrix worldMatrix, MyStringHash materialType, Vector3 center, Vector3 size, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(null, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkBoxShape hkBoxShape = new HkBoxShape(size * 0.5f);
			myPhysicsBody.CreateFromCollisionObject(hkBoxShape, center, worldMatrix, value);
			hkBoxShape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitBoxPhysics(this IMyEntity entity, MyStringHash materialType, MyModel model, float mass, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			Vector3 center = model.BoundingBox.Center;
			Vector3 boundingBoxSize = model.BoundingBoxSize;
			entity.InitBoxPhysics(materialType, center, boundingBoxSize, mass, 0f, angularDamping, collisionLayer, rbFlag);
		}

		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitCharacterPhysics(this IMyEntity entity, MyStringHash materialType, Vector3 center, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag, float mass, bool isOnlyVertical, float maxSlope, float maxImpulse, float maxSpeedRelativeToShip, bool networkProxy, float? maxForce)
		{
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			myPhysicsBody.CreateCharacterCollision(center, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, entity.PositionComp.WorldMatrixRef, mass, collisionLayer, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, networkProxy, maxForce);
			entity.Physics = myPhysicsBody;
		}

		[Obsolete("Use IMyPhysics instead", false)]
		public static void InitCapsulePhysics(this IMyEntity entity, MyStringHash materialType, Vector3 vertexA, Vector3 vertexB, float radius, float mass, float linearDamping, float angularDamping, ushort collisionLayer, RigidBodyFlag rbFlag)
		{
			mass = (((rbFlag & RigidBodyFlag.RBF_STATIC) != 0) ? 0f : mass);
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(entity, rbFlag)
			{
				MaterialType = materialType,
				AngularDamping = angularDamping,
				LinearDamping = linearDamping
			};
			HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(radius, mass);
			myPhysicsBody.ReportAllContacts = true;
			HkCapsuleShape hkCapsuleShape = new HkCapsuleShape(vertexA, vertexB, radius);
			myPhysicsBody.CreateFromCollisionObject(hkCapsuleShape, (vertexA + vertexB) / 2f, entity.PositionComp.WorldMatrixRef, value);
			hkCapsuleShape.Base.RemoveReference();
			entity.Physics = myPhysicsBody;
		}

		/// <summary>
		/// Initialize entity physics using the model's collision shapes.
		/// </summary>
		/// <param name="entity">Entity to create physics for</param>
		/// <param name="rbFlags">One or more of <see cref="T:VRage.Game.Components.RigidBodyFlag" /> flags</param>
		/// <param name="collisionLayers">Collision layer physics operates on, see <see cref="T:Sandbox.Engine.Physics.MyPhysics.CollisionLayers" /></param>
		/// <returns><b>true</b> if new physics was initialized. <b>false</b> otherwise.</returns>
		[Obsolete("Use IMyPhysics instead", false)]
		public static bool InitModelPhysics(this IMyEntity entity, RigidBodyFlag rbFlags = RigidBodyFlag.RBF_KINEMATIC, int collisionLayers = 17)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity.Closed)
			{
				return false;
			}
			if (myEntity.ModelCollision.HavokCollisionShapes != null && myEntity.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				HkShape[] havokCollisionShapes = myEntity.ModelCollision.HavokCollisionShapes;
				HkListShape hkListShape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
				myEntity.Physics = new MyPhysicsBody(myEntity, rbFlags);
				myEntity.Physics.IsPhantom = false;
<<<<<<< HEAD
				(myEntity.Physics as MyPhysicsBody).CreateFromCollisionObject(hkListShape, Vector3.Zero, myEntity.WorldMatrix, null, collisionLayers);
=======
				(myEntity.Physics as MyPhysicsBody).CreateFromCollisionObject(hkListShape, Vector3D.Zero, myEntity.WorldMatrix, null, collisionLayers);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myEntity.Physics.Enabled = true;
				hkListShape.Base.RemoveReference();
				return true;
			}
			return false;
		}
	}
}
