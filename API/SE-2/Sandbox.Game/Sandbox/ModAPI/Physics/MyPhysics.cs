using System;
using System.Collections.Generic;
using System.Threading;
<<<<<<< HEAD
using Havok;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Engine.Physics;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
<<<<<<< HEAD
using VRage.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI.Physics
{
	internal class MyPhysics : IMyPhysics
	{
		public static readonly MyPhysics Static;

		private MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>> m_collectorsPool = new MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>>(10, delegate(List<Sandbox.Engine.Physics.MyPhysics.HitInfo> x)
		{
			x.Clear();
		});

		int IMyPhysics.StepsLastSecond => Sandbox.Engine.Physics.MyPhysics.StepsLastSecond;

		float IMyPhysics.SimulationRatio => Sandbox.Engine.Physics.MyPhysics.SimulationRatio;

		float IMyPhysics.ServerSimulationRatio => Sync.ServerSimulationRatio;

		static MyPhysics()
		{
			Static = new MyPhysics();
		}

		bool IMyPhysics.CastLongRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, bool any)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo? hitInfo2 = Sandbox.Engine.Physics.MyPhysics.CastLongRay(from, to, any);
			if (hitInfo2.HasValue)
			{
				hitInfo = hitInfo2;
				return true;
			}
			hitInfo = null;
			return false;
		}

		bool IMyPhysics.CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, int raycastFilterLayer)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo? hitInfo2 = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, raycastFilterLayer);
			if (hitInfo2.HasValue)
			{
				hitInfo = hitInfo2;
				return true;
			}
			hitInfo = null;
			return false;
		}

		void IMyPhysics.CastRay(Vector3D from, Vector3D to, List<IHitInfo> toList, int raycastFilterLayer)
		{
			AssertMainThread();
			List<Sandbox.Engine.Physics.MyPhysics.HitInfo> list = m_collectorsPool.Get();
			toList.Clear();
			Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, list, raycastFilterLayer);
			foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo item in list)
			{
				toList.Add(item);
			}
			m_collectorsPool.Return(list);
		}

		bool IMyPhysics.CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, uint raycastCollisionFilter, bool ignoreConvexShape)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.HitInfo hitInfo2;
			bool result = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, out hitInfo2, raycastCollisionFilter, ignoreConvexShape);
			hitInfo = hitInfo2;
			return result;
		}

		public void CastRayParallel(ref Vector3D from, ref Vector3D to, int raycastCollisionFilter, Action<IHitInfo> callback)
		{
			Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, raycastCollisionFilter, delegate(Sandbox.Engine.Physics.MyPhysics.HitInfo? x)
			{
				callback(x);
			});
		}

		public void CastRayParallel(ref Vector3D from, ref Vector3D to, List<IHitInfo> toList, int raycastCollisionFilter, Action<List<IHitInfo>> callback)
		{
			Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, m_collectorsPool.Get(), raycastCollisionFilter, delegate(List<Sandbox.Engine.Physics.MyPhysics.HitInfo> hits)
			{
				foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo hit in hits)
				{
					toList.Add(hit);
				}
				m_collectorsPool.Return(hits);
				callback(toList);
			});
		}

		void IMyPhysics.EnsurePhysicsSpace(BoundingBoxD aabb)
		{
			AssertMainThread();
			Sandbox.Engine.Physics.MyPhysics.EnsurePhysicsSpace(aabb);
		}

		int IMyPhysics.GetCollisionLayer(string strLayer)
		{
			return Sandbox.Engine.Physics.MyPhysics.GetCollisionLayer(strLayer);
		}

		public Vector3 CalculateNaturalGravityAt(Vector3D worldPosition, out float naturalGravityInterference)
		{
			return MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPosition, out naturalGravityInterference);
		}

		public Vector3 CalculateArtificialGravityAt(Vector3D worldPosition, float naturalGravityInterference = 1f)
		{
			float gravityMultiplier = MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(naturalGravityInterference);
			return MyGravityProviderSystem.CalculateArtificialGravityInPoint(worldPosition, gravityMultiplier);
		}

		private void AssertMainThread()
		{
			if (MyUtils.MainThread != Thread.get_CurrentThread())
			{
				MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_PhysicsParallelAccess);
<<<<<<< HEAD
			}
		}

		public ModAPIMass CreateMassCombined(ICollection<ModAPIMassElement> massElements)
		{
			Span<HkMassElement> elements = default(Span<HkMassElement>);
			foreach (ModAPIMassElement massElement in massElements)
			{
				HkMassProperties properties = new HkMassProperties(massElement.Properties.Volume, massElement.Properties.Mass, massElement.Properties.CenterOfMass, massElement.Properties.InertiaTensor);
				Matrix transform = massElement.Tranform;
				elements.Fill(new HkMassElement(ref properties, ref transform));
			}
			HkInertiaTensorComputer.CombineMassProperties(elements, out var massProperties);
			return ModAPIMass.FromHkMass(massProperties);
		}

		public ModAPIMass CreateMassForBox(Vector3 halfExtents, float mass)
		{
			return ModAPIMass.FromHkMass(HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(halfExtents, mass));
		}

		public ModAPIMass CreateMassForCapsule(Vector3 startAxis, Vector3 endAxis, float radius, float mass)
		{
			return ModAPIMass.FromHkMass(HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(startAxis, endAxis, radius, mass));
		}

		public ModAPIMass CreateMassForCylinder(Vector3 startAxis, Vector3 endAxis, float radius, float mass)
		{
			return ModAPIMass.FromHkMass(HkInertiaTensorComputer.ComputeCylinderVolumeMassProperties(startAxis, endAxis, radius, mass));
		}

		public ModAPIMass CreateMassForSphere(float radius, float mass)
		{
			return ModAPIMass.FromHkMass(HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(radius, mass));
		}

		public PhysicsSettings CreateSettingsForDetector(IMyEntity entity, Action<IMyEntity, bool> detectorColliderCallback, MatrixD worldMatrix, Vector3 localCenter, RigidBodyFlag rigidBodyFlags = RigidBodyFlag.RBF_DEFAULT, ushort collisionLayer = 15, bool isPhantom = true)
		{
			if (entity == null || entity.Closed)
			{
				throw new ArgumentException("Entity can't be null, or closed");
			}
			PhysicsSettings result = default(PhysicsSettings);
			result.Entity = entity;
			result.WorldMatrix = worldMatrix;
			result.LocalCenter = localCenter;
			result.CollisionLayer = collisionLayer;
			result.RigidBodyFlags = rigidBodyFlags;
			result.DetectorColliderCallback = detectorColliderCallback;
			result.IsPhantom = isPhantom;
			return result;
		}

		public PhysicsSettings CreateSettingsForPhysics(IMyEntity entity, MatrixD worldMatrix, Vector3 localCenter, float linearDamping = 0f, float angularDamping = 0f, ushort collisionLayer = 15, RigidBodyFlag rigidBodyFlags = RigidBodyFlag.RBF_DEFAULT, bool isPhantom = false, ModAPIMass? mass = null)
		{
			if (entity == null || entity.Closed)
			{
				throw new ArgumentException("Entity can't be null, or closed");
			}
			PhysicsSettings result = default(PhysicsSettings);
			result.Entity = entity;
			result.WorldMatrix = worldMatrix;
			result.LocalCenter = localCenter;
			result.LinearDamping = linearDamping;
			result.AngularDamping = angularDamping;
			result.CollisionLayer = collisionLayer;
			result.RigidBodyFlags = rigidBodyFlags;
			result.IsPhantom = isPhantom;
			result.Mass = mass;
			return result;
		}

		public void CreateBoxPhysics(PhysicsSettings settings, Vector3 halfExtends, float convexRadius = 0f)
		{
			CreatePhysics(new HkBoxShape(halfExtends, convexRadius), settings);
		}

		public void CreateSpherePhysics(PhysicsSettings settings, float radius)
		{
			CreatePhysics(new HkSphereShape(radius), settings);
		}

		public void CreateCapsulePhysics(PhysicsSettings settings, Vector3 vertexA, Vector3 vertexB, float radius)
		{
			CreatePhysics(new HkCapsuleShape(vertexA, vertexB, radius), settings);
		}

		public void CreateModelPhysics(PhysicsSettings settings)
		{
			MyEntity myEntity = settings.Entity as MyEntity;
			if (myEntity.ModelCollision.HavokCollisionShapes != null && myEntity.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				HkShape[] havokCollisionShapes = myEntity.ModelCollision.HavokCollisionShapes;
				CreatePhysics(new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None), settings);
			}
		}

		public void CreatePhysics(HkShape shape, PhysicsSettings settings)
		{
			HkMassProperties? massProperties = null;
			if (settings.Mass.HasValue)
			{
				ModAPIMass value = settings.Mass.Value;
				massProperties = new HkMassProperties(value.Volume, value.Mass, value.CenterOfMass, value.InertiaTensor);
			}
			MyPhysicsBody myPhysicsBody = new MyPhysicsBody(settings.Entity, settings.RigidBodyFlags)
			{
				MaterialType = settings.MaterialType,
				AngularDamping = settings.AngularDamping,
				LinearDamping = settings.LinearDamping
			};
			if (settings.DetectorColliderCallback != null)
			{
				HkBvShape hkBvShape = new HkBvShape(childShape: new HkPhantomCallbackShape(delegate(HkPhantomCallbackShape sender, HkRigidBody body)
				{
					settings.DetectorColliderCallback(body.GetEntity(0u), arg2: true);
				}, delegate(HkPhantomCallbackShape sender, HkRigidBody body)
				{
					settings.DetectorColliderCallback(body.GetEntity(0u), arg2: false);
				}), boundingVolumeShape: shape, policy: HkReferencePolicy.TakeOwnership);
				myPhysicsBody.CreateFromCollisionObject(hkBvShape, settings.LocalCenter, settings.WorldMatrix, massProperties, settings.CollisionLayer);
				hkBvShape.Base.RemoveReference();
			}
			else
			{
				myPhysicsBody.CreateFromCollisionObject(shape, settings.LocalCenter, settings.WorldMatrix, massProperties, settings.CollisionLayer);
				shape.Base.RemoveReference();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			settings.Entity.Physics = myPhysicsBody;
			settings.Entity.Physics.Enabled = true;
			settings.Entity.Physics.IsPhantom = settings.IsPhantom;
		}
	}
}
