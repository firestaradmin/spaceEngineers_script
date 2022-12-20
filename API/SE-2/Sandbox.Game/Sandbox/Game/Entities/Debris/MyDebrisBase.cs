using System;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.GameSystems;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Debris
{
	/// <summary>
	/// Single physical debris object. These are stored in a pool and are not meant to be created
	/// all the time, but only pulled out and returned back to the pool. Therefore, we do not use constructor
	/// to initialize an instance.
	/// </summary>
	public class MyDebrisBase : MyEntity
	{
		public class MyDebrisPhysics : MyPhysicsBody
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisBase_003C_003EMyDebrisPhysics_003C_003EActor
			{
			}

			public MyDebrisPhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlag)
				: base(entity, rigidBodyFlag)
			{
			}

			public virtual void CreatePhysicsShape(out HkShape shape, out HkMassProperties massProperties, float mass)
			{
				HkBoxShape hkBoxShape = new HkBoxShape((((MyEntity)base.Entity).Render.GetModel().BoundingBox.Max - ((MyEntity)base.Entity).Render.GetModel().BoundingBox.Min) / 2f * base.Entity.PositionComp.Scale.Value);
				Vector3 translation = (((MyEntity)base.Entity).Render.GetModel().BoundingBox.Max + ((MyEntity)base.Entity).Render.GetModel().BoundingBox.Min) / 2f;
				shape = new HkTransformShape(hkBoxShape, ref translation, ref Quaternion.Identity);
				massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, mass);
				massProperties.CenterOfMass = translation;
			}

			public virtual void ScalePhysicsShape(ref HkMassProperties massProperties)
			{
				MyModel model = base.Entity.Render.GetModel();
				HkShape shape;
				if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
				{
					shape = model.HavokCollisionShapes[0];
					shape.GetLocalAABB(0.1f, out var min, out var max);
					Vector3 halfExtents = new Vector3((max - min) * 0.5f);
					massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(halfExtents, halfExtents.Volume * 50f);
					massProperties.CenterOfMass = new Vector3((min + max) * 0.5f);
				}
				else
				{
					HkTransformShape hkTransformShape = (HkTransformShape)RigidBody.GetShape();
					HkBoxShape hkBoxShape = (HkBoxShape)hkTransformShape.ChildShape;
					hkBoxShape.HalfExtents = (model.BoundingBox.Max - model.BoundingBox.Min) / 2f * base.Entity.PositionComp.Scale.Value;
					massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, hkBoxShape.HalfExtents.Volume * 0.5f);
					massProperties.CenterOfMass = hkTransformShape.Transform.Translation;
					shape = hkTransformShape;
				}
				RigidBody.SetShape(shape);
				RigidBody.SetMassProperties(ref massProperties);
				RigidBody.UpdateShape();
			}
		}

		public class MyDebrisBaseLogic : MyEntityGameLogic
		{
			private class Sandbox_Game_Entities_Debris_MyDebrisBase_003C_003EMyDebrisBaseLogic_003C_003EActor : IActivator, IActivator<MyDebrisBaseLogic>
			{
				private sealed override object CreateInstance()
				{
					return new MyDebrisBaseLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDebrisBaseLogic CreateInstance()
				{
					return new MyDebrisBaseLogic();
				}

				MyDebrisBaseLogic IActivator<MyDebrisBaseLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private MyDebrisBase m_debris;

			/// <summary>
			///  We cannot use OnClose event from entity to deallocate pool objects,
			///  so this callback solves that (although it isn't quite as pretty). 
			/// </summary>
			private Action<MyDebrisBase> m_onCloseCallback;

			private bool m_isStarted;

			private int m_createdTime;

			public int LifespanInMiliseconds;

			protected HkMassProperties m_massProperties;

			/// <summary>
			/// One time initialization for debris entity. These are settings that do not change 
			/// when this debris entity is pulled from the pool.
			/// Also note that this is the only way Debris should be initialized. It calls other Init methods with 
			/// correct arguments.
			/// </summary>
			public virtual void Init(MyDebrisBaseDescription desc)
			{
				Init(null, desc.Model, null, 1f);
				LifespanInMiliseconds = MyUtils.GetRandomInt(desc.LifespanMinInMiliseconds, desc.LifespanMaxInMiliseconds);
				MyDebrisPhysics myDebrisPhysics = (MyDebrisPhysics)GetPhysics(RigidBodyFlag.RBF_DEBRIS);
				base.Container.Entity.Physics = myDebrisPhysics;
				float mass = CalculateMass(((MyEntity)base.Entity).Render.GetModel().BoundingSphere.Radius);
				myDebrisPhysics.CreatePhysicsShape(out var shape, out m_massProperties, mass);
				myDebrisPhysics.CreateFromCollisionObject(shape, Vector3.Zero, MatrixD.Identity, m_massProperties, 20);
				HkMassChangerUtil.Create(myDebrisPhysics.RigidBody, -21, 1f, 0f);
				HkEasePenetrationAction hkEasePenetrationAction = new HkEasePenetrationAction(myDebrisPhysics.RigidBody, 3f);
				hkEasePenetrationAction.InitialAllowedPenetrationDepthMultiplier = 10f;
				hkEasePenetrationAction.Dispose();
				base.Container.Entity.Physics.Enabled = false;
				shape.RemoveReference();
				m_entity.Save = false;
				base.Container.Entity.Physics.PlayCollisionCueEnabled = true;
				base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
				base.Container.Entity.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
				m_onCloseCallback = desc.OnCloseAction;
			}

			protected virtual float CalculateMass(float r)
			{
				return 3.141593f * (r * r) * (1.33333337f * r) * 2600f;
			}

			protected virtual MyPhysicsComponentBase GetPhysics(RigidBodyFlag rigidBodyFlag)
			{
				return new MyDebrisPhysics(base.Container.Entity, rigidBodyFlag);
			}

			/// <summary>
			/// Called to clean up resources before pool is destroyed. Think of this as finalization.
			/// </summary>
			public virtual void Free()
			{
				if (base.Container.Entity.Physics != null)
				{
					base.Container.Entity.Physics.Close();
					base.Container.Entity.Physics = null;
				}
			}

			/// <summary>
			/// Initialization of each debris instance when it is taken out of the pool.
			/// </summary>
			public virtual void Start(Vector3D position, Vector3D initialVelocity)
			{
				Start(MatrixD.CreateTranslation(position), initialVelocity);
			}

			public virtual void Start(MatrixD position, Vector3D initialVelocity, bool randomRotation = true)
			{
				m_createdTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				base.Container.Entity.WorldMatrix = position;
				base.Container.Entity.Physics.Clear();
				base.Container.Entity.Physics.LinearVelocity = initialVelocity;
				if (randomRotation)
				{
					base.Container.Entity.Physics.AngularVelocity = new Vector3(MyUtils.GetRandomRadian(), MyUtils.GetRandomRadian(), MyUtils.GetRandomRadian());
				}
				MyEntities.Add(m_entity);
				base.Container.Entity.Physics.Enabled = true;
				Vector3 gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position.Translation);
				((MyPhysicsBody)base.Container.Entity.Physics).RigidBody.Gravity = gravity;
				m_isStarted = true;
			}

			public override void OnAddedToContainer()
			{
				base.OnAddedToContainer();
				m_debris = base.Container.Entity as MyDebrisBase;
			}

			public override void UpdateAfterSimulation()
			{
				base.UpdateAfterSimulation();
				if (!m_isStarted)
				{
					return;
				}
				int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_createdTime;
				if (num > LifespanInMiliseconds || MyDebris.Static.TooManyDebris)
				{
					MarkForClose();
					return;
				}
				float num2 = (float)num / (float)LifespanInMiliseconds;
				float num3 = 0.75f;
				if (num2 > num3)
				{
					uint renderObjectID = base.Container.Entity.Render.GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						MyRenderProxy.UpdateRenderEntity(renderObjectID, null, null, (num2 - num3) / (1f - num3));
					}
				}
			}

			public override void MarkForClose()
			{
				if (m_onCloseCallback != null)
				{
					m_onCloseCallback(m_debris);
					m_onCloseCallback = null;
				}
				base.MarkForClose();
			}

			public override void Close()
			{
				base.Close();
			}
		}

		private class Sandbox_Game_Entities_Debris_MyDebrisBase_003C_003EActor : IActivator, IActivator<MyDebrisBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyDebrisBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebrisBase CreateInstance()
			{
				return new MyDebrisBase();
			}

			MyDebrisBase IActivator<MyDebrisBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float STONE_DENSITY = 2600f;

		public MyDebrisBaseLogic Debris => (MyDebrisBaseLogic)GameLogic;

		public override void InitComponents()
		{
			if (GameLogic == null)
			{
				GameLogic = new MyDebrisBaseLogic();
			}
			base.InitComponents();
		}
	}
}
