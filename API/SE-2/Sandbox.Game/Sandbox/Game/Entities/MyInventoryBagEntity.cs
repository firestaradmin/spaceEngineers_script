using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
	/// <summary>
	/// Inventory bag spawned when character died, container breaks, or when entity from other inventory cannot be spawned then bag spawned with the item in its inventory.
	/// </summary>
	[MyEntityType(typeof(MyObjectBuilder_ReplicableEntity), false)]
	[MyEntityType(typeof(MyObjectBuilder_InventoryBagEntity), true)]
	public class MyInventoryBagEntity : MyEntity, IMyInventoryBag, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		private class Sandbox_Game_Entities_MyInventoryBagEntity_003C_003EActor : IActivator, IActivator<MyInventoryBagEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyInventoryBagEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInventoryBagEntity CreateInstance()
			{
				return new MyInventoryBagEntity();
			}

			MyInventoryBagEntity IActivator<MyInventoryBagEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Vector3 m_gravity = Vector3.Zero;

		public long OwnerIdentityId;

		public Vector3 GeneratedGravity { get; set; }

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (objectBuilder.EntityDefinitionId.HasValue && objectBuilder.EntityDefinitionId.Value.TypeId != typeof(MyObjectBuilder_InventoryBagEntity))
			{
				objectBuilder.EntityDefinitionId = new SerializableDefinitionId(typeof(MyObjectBuilder_InventoryBagEntity), objectBuilder.EntityDefinitionId.Value.SubtypeName);
			}
			base.Init(objectBuilder);
			if (!Sync.IsServer)
			{
				HkShape shape = base.Physics.RigidBody.GetShape();
				HkMassProperties value = default(HkMassProperties);
				value.Mass = base.Physics.RigidBody.Mass;
				MyPhysicsBody obj = base.Physics as MyPhysicsBody;
				obj.Close();
				obj.ReportAllContacts = true;
				obj.Flags = RigidBodyFlag.RBF_STATIC;
				obj.CreateFromCollisionObject(shape, Vector3.Zero, base.WorldMatrix, value, 10);
				obj.RigidBody.ContactPointCallbackEnabled = true;
				obj.ContactPointCallback += OnPhysicsContactPointCallback;
			}
			base.Physics.Friction = 2f;
			if (objectBuilder is MyObjectBuilder_InventoryBagEntity)
			{
				MyObjectBuilder_InventoryBagEntity myObjectBuilder_InventoryBagEntity = (MyObjectBuilder_InventoryBagEntity)objectBuilder;
				if (GetPhysicsComponentBuilder(myObjectBuilder_InventoryBagEntity) == null)
				{
					base.Physics.LinearVelocity = myObjectBuilder_InventoryBagEntity.LinearVelocity;
					base.Physics.AngularVelocity = myObjectBuilder_InventoryBagEntity.AngularVelocity;
				}
				if (myObjectBuilder_InventoryBagEntity != null)
				{
					OwnerIdentityId = myObjectBuilder_InventoryBagEntity.OwnerIdentityId;
				}
			}
			else if (objectBuilder is MyObjectBuilder_ReplicableEntity)
			{
				MyObjectBuilder_ReplicableEntity myObjectBuilder_ReplicableEntity = (MyObjectBuilder_ReplicableEntity)objectBuilder;
				base.Physics.LinearVelocity = myObjectBuilder_ReplicableEntity.LinearVelocity;
				base.Physics.AngularVelocity = myObjectBuilder_ReplicableEntity.AngularVelocity;
			}
			base.OnClosing += MyInventoryBagEntity_OnClosing;
		}

		internal static MyObjectBuilder_PhysicsComponentBase GetPhysicsComponentBuilder(MyObjectBuilder_InventoryBagEntity builder)
		{
			if (builder.ComponentContainer != null && builder.ComponentContainer.Components.Count > 0)
			{
				foreach (MyObjectBuilder_ComponentContainer.ComponentData component in builder.ComponentContainer.Components)
				{
					if (component.Component is MyObjectBuilder_PhysicsComponentBase)
					{
						return component.Component as MyObjectBuilder_PhysicsComponentBase;
					}
				}
			}
			return null;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
			MyObjectBuilder_InventoryBagEntity myObjectBuilder_InventoryBagEntity = objectBuilder as MyObjectBuilder_InventoryBagEntity;
			if (myObjectBuilder_InventoryBagEntity != null)
			{
				myObjectBuilder_InventoryBagEntity.OwnerIdentityId = OwnerIdentityId;
			}
			return objectBuilder;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateGravity();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.Physics != null && !base.Physics.IsStatic)
			{
				base.Physics.RigidBody.Gravity = m_gravity + GeneratedGravity;
				GeneratedGravity = Vector3.Zero;
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			UpdateGravity();
		}

		private void UpdateGravity()
		{
			if (base.Physics != null && !base.Physics.IsStatic)
			{
				m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.PositionComp.GetPosition());
			}
		}

		private void MyInventoryBagEntity_OnClosing(MyEntity obj)
		{
			if (Sync.IsServer)
			{
				MyGps gpsByEntityId = MySession.Static.Gpss.GetGpsByEntityId(OwnerIdentityId, base.EntityId);
				if (gpsByEntityId != null)
				{
					MySession.Static.Gpss.SendDelete(OwnerIdentityId, gpsByEntityId.Hash);
				}
			}
		}

		private void OnPhysicsContactPointCallback(ref MyPhysics.MyContactPointEvent e)
		{
			if (base.Physics.LinearVelocity.LengthSquared() > 225f && e.ContactPointEvent.GetOtherEntity(this) is MyCharacter)
			{
				e.ContactPointEvent.Base.Disable();
			}
		}

		public override void OnReplicationStarted()
		{
			base.OnReplicationStarted();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted(this);
		}

		public override void OnReplicationEnded()
		{
			base.OnReplicationEnded();
			MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded(this);
		}
	}
}
