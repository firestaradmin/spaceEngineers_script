using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	/// <summary>
	/// Abstract engine physics body object.
	/// </summary>
	/// <summary>
	/// Abstract engine physics body object.
	/// </summary>
	/// <summary>
	/// Abstract engine physics body object.
	/// </summary>
	/// <summary>
	/// Abstract engine physics body object.
	/// </summary>
	[MyComponentBuilder(typeof(MyObjectBuilder_PhysicsBodyComponent), true)]
	public class MyPhysicsBody : MyPhysicsComponentBase, MyClusterTree.IMyActivationHandler
	{
		public delegate void PhysicsContactHandler(ref MyPhysics.MyContactPointEvent e);

		public class MyWeldInfo
		{
			public MyPhysicsBody Parent;

			public Matrix Transform = Matrix.Identity;

			/// <summary>
			/// This does NOT contain all welded bodies, see @MyWeldGroupData
			/// </summary>
			public readonly HashSet<MyPhysicsBody> Children = new HashSet<MyPhysicsBody>();

			public HkMassElement MassElement;

			internal void UpdateMassProps(HkRigidBody rb)
			{
				HkMassProperties properties = default(HkMassProperties);
				properties.InertiaTensor = rb.InertiaTensor;
				properties.Mass = rb.Mass;
				properties.CenterOfMass = rb.CenterOfMassLocal;
				MassElement = default(HkMassElement);
				MassElement.Properties = properties;
				MassElement.Tranform = Transform;
			}

			internal void SetMassProps(HkMassProperties mp)
			{
				MassElement = default(HkMassElement);
				MassElement.Properties = mp;
				MassElement.Tranform = Transform;
			}
		}

		private class Sandbox_Engine_Physics_MyPhysicsBody_003C_003EActor : IActivator, IActivator<MyPhysicsBody>
		{
			private sealed override object CreateInstance()
			{
				return new MyPhysicsBody();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPhysicsBody CreateInstance()
			{
				return new MyPhysicsBody();
			}

			MyPhysicsBody IActivator<MyPhysicsBody>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private PhysicsContactHandler m_contactPointCallbackHandler;

		private Action<MyPhysicsComponentBase, bool> m_onBodyActiveStateChangedHandler;

		private bool m_activationCallbackRegistered;

		private bool m_contactPointCallbackRegistered;

		private static MyStringHash m_character = MyStringHash.GetOrCompute("Character");

		private int m_motionCounter;

		protected float m_angularDamping;

		protected float m_linearDamping;

		private ulong m_clusterObjectID = ulong.MaxValue;

		private Vector3D m_offset = Vector3D.Zero;

		protected Matrix m_bodyMatrix;

		protected HkWorld m_world;

		private HkWorld m_lastWorld;

		private HkRigidBody m_rigidBody;

		private HkRigidBody m_rigidBody2;

		private float m_animatedClientMass;

		private readonly HashSet<HkConstraint> m_constraints = new HashSet<HkConstraint>();

		private readonly List<HkConstraint> m_constraintsAddBatch = new List<HkConstraint>();

		private readonly List<HkConstraint> m_constraintsRemoveBatch = new List<HkConstraint>();

		/// <summary>
		/// Must be set before creating rigid body
		/// </summary>
		public HkSolverDeactivation InitialSolverDeactivation = HkSolverDeactivation.Low;

		private bool m_isInWorld;

		private bool m_shapeChangeInProgress;

		private HashSet<IMyEntity> m_batchedChildren = new HashSet<IMyEntity>();

		private List<MyPhysicsBody> m_batchedBodies = new List<MyPhysicsBody>();

		private Vector3D? m_lastComPosition;

		private Vector3 m_lastComLocal;

		private bool m_isStaticForCluster;

		private bool m_batchRequest;

		private static List<HkConstraint> m_notifyConstraints = new List<HkConstraint>();

		private HkdBreakableBody m_breakableBody;

		private List<HkdBreakableBodyInfo> m_tmpLst = new List<HkdBreakableBodyInfo>();

		protected HkRagdoll m_ragdoll;

		private bool m_ragdollDeadMode;

		private readonly MyWeldInfo m_weldInfo = new MyWeldInfo();

		private List<HkShape> m_tmpShapeList = new List<HkShape>();

		private bool NeedsActivationCallback
		{
			get
			{
				if (!(m_rigidBody2 != null))
				{
					return m_onBodyActiveStateChangedHandler != null;
				}
				return true;
			}
		}

		private bool NeedsContactPointCallback => m_contactPointCallbackHandler != null;

		protected ulong ClusterObjectID
		{
			get
			{
				return m_clusterObjectID;
			}
			set
			{
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				m_clusterObjectID = value;
				if (value != ulong.MaxValue)
				{
					Offset = MyPhysics.GetObjectOffset(value);
				}
				else
				{
					Offset = Vector3D.Zero;
				}
				Enumerator<MyPhysicsBody> enumerator = WeldInfo.Children.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().Offset = Offset;
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		protected Vector3D Offset
		{
			get
			{
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					return ((MyPhysicsBody)topMostParent.Physics).Offset;
				}
				return m_offset;
			}
			set
			{
				m_offset = value;
			}
		}

		public new MyPhysicsBodyComponentDefinition Definition { get; private set; }

		public HkWorld HavokWorld
		{
			get
			{
				if (IsWelded)
				{
					return WeldInfo.Parent.m_world;
				}
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					return ((MyPhysicsBody)topMostParent.Physics).HavokWorld;
				}
				return m_world;
			}
		}

		public virtual int HavokCollisionSystemID
		{
			get
			{
				if (!(RigidBody != null))
				{
					return 0;
				}
				return HkGroupFilter.GetSystemGroupFromFilterInfo(RigidBody.GetCollisionFilterInfo());
			}
			protected set
			{
				if (RigidBody != null)
				{
					RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(RigidBody.Layer, value, 1, 1));
				}
				if (RigidBody2 != null)
				{
					RigidBody2.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(RigidBody2.Layer, value, 1, 1));
				}
			}
		}

		public override HkRigidBody RigidBody
		{
			get
			{
				if (WeldInfo.Parent == null)
				{
					return m_rigidBody;
				}
				return WeldInfo.Parent.RigidBody;
			}
			protected set
			{
				if (m_rigidBody != value)
				{
					if (m_rigidBody != null && !m_rigidBody.IsDisposed)
					{
						m_rigidBody.ContactSoundCallback -= OnContactSoundCallback;
						UnregisterContactPointCallback();
						UnregisterActivationCallbacks();
					}
					m_rigidBody = value;
					m_activationCallbackRegistered = false;
					m_contactPointCallbackRegistered = false;
					if (m_rigidBody != null)
					{
						RegisterActivationCallbacksIfNeeded();
						RegisterContactPointCallbackIfNeeded();
						m_rigidBody.ContactSoundCallback += OnContactSoundCallback;
					}
				}
			}
		}

		public override HkRigidBody RigidBody2
		{
			get
			{
				if (WeldInfo.Parent == null)
				{
					return m_rigidBody2;
				}
				return WeldInfo.Parent.RigidBody2;
			}
			protected set
			{
				if (m_rigidBody2 != value)
				{
					m_rigidBody2 = value;
					if (NeedsActivationCallback)
					{
						RegisterActivationCallbacksIfNeeded();
					}
					else
					{
						UnregisterActivationCallbacks();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the mass.
		/// </summary>
		/// <value>
		/// The mass.
		/// </value>
		public override float Mass
		{
			get
			{
				if (CharacterProxy != null)
				{
					return CharacterProxy.Mass;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer)
					{
						return m_animatedClientMass;
					}
					return RigidBody.Mass;
				}
				if (Ragdoll != null)
				{
					return Ragdoll.Mass;
				}
				return 0f;
			}
		}

		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		public override float Speed => LinearVelocity.Length();

		public override float Friction
		{
			get
			{
				return RigidBody.Friction;
			}
			set
			{
				RigidBody.Friction = value;
			}
		}

		public override bool IsStatic
		{
			get
			{
				if (RigidBody != null)
				{
					return RigidBody.IsFixed;
				}
				return false;
			}
		}

		public override bool IsKinematic
		{
			get
			{
				if (RigidBody != null)
				{
					if (!RigidBody.IsFixed)
					{
						return RigidBody.IsFixedOrKeyframed;
					}
					return false;
				}
				return false;
			}
		}

		public bool IsSubpart { get; set; }

		public override bool IsActive
		{
			get
			{
				if (RigidBody != null)
				{
					return RigidBody.IsActive;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.GetHitRigidBody().IsActive;
				}
				if (Ragdoll != null)
				{
					return Ragdoll.IsActive;
				}
				return false;
			}
		}

		public MyCharacterProxy CharacterProxy { get; set; }

<<<<<<< HEAD
		/// This system group id will is used to set's this character rigid bodies collision filters        
		public int CharacterSystemGroupCollisionFilterID { get; private set; }

		/// This is character collision filter, use this to avoid collisions with character and character holding bodies
=======
		public int CharacterSystemGroupCollisionFilterID { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public uint CharacterCollisionFilter { get; private set; }

		public override bool IsInWorld
		{
			get
			{
				return m_isInWorld;
			}
			protected set
			{
				m_isInWorld = value;
			}
		}

		public override bool ShapeChangeInProgress
		{
			get
			{
				return m_shapeChangeInProgress;
			}
			set
			{
				m_shapeChangeInProgress = value;
			}
		}

		public override Vector3 AngularVelocityLocal
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.AngularVelocity;
					}
					return RigidBody.AngularVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.AngularVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().AngularVelocity;
				}
				return base.AngularVelocity;
			}
		}

		public override Vector3 LinearVelocityLocal
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.LinearVelocity;
					}
					return RigidBody.LinearVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.LinearVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().LinearVelocity;
				}
				return base.LinearVelocity;
			}
		}

		/// <summary>
		/// Gets or sets the linear velocity.
		/// </summary>
		/// <value>
		/// The linear velocity.
		/// </value>
		public override Vector3 LinearVelocity
		{
			get
			{
				return GetLinearVelocity(safe: true);
			}
			set
			{
				SetLinearVelocity(value, safe: true);
			}
		}

<<<<<<< HEAD
		/// <summary>
		///
		///  </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override Vector3 LinearVelocityUnsafe
		{
			get
			{
				return GetLinearVelocity(safe: false);
			}
			set
			{
				SetLinearVelocity(value, safe: true);
			}
		}

		/// <summary>
		/// Gets or sets the linear damping.
		/// </summary>
		/// <value>
		/// The linear damping.
		/// </value>
		public override float LinearDamping
		{
			get
			{
				return RigidBody.LinearDamping;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.LinearDamping = value;
				}
				m_linearDamping = value;
			}
		}

		/// <summary>
		/// Gets or sets the angular damping.
		/// </summary>
		/// <value>
		/// The angular damping.
		/// </value>
		public override float AngularDamping
		{
			get
			{
				return RigidBody.AngularDamping;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.AngularDamping = value;
				}
				m_angularDamping = value;
			}
		}

		/// <summary>
		/// Gets or sets the angular velocity.
		/// </summary>
		/// <value>
		/// The angular velocity.
		/// </value>
		public override Vector3 AngularVelocity
		{
			get
			{
				if (RigidBody != null)
				{
					if (MyMultiplayer.Static != null && !Sync.IsServer && IsStatic)
					{
						return base.AngularVelocity;
					}
					return RigidBody.AngularVelocity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.AngularVelocity;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					return Ragdoll.GetRootRigidBody().AngularVelocity;
				}
				return base.AngularVelocity;
			}
			set
			{
				if (RigidBody != null)
				{
					RigidBody.AngularVelocity = value;
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.AngularVelocity = value;
				}
				if (Ragdoll != null && Ragdoll.IsActive)
				{
					foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
					{
						rigidBody.AngularVelocity = value;
					}
				}
				base.AngularVelocity = value;
			}
		}

		public override Vector3 SupportNormal
		{
			get
			{
				if (CharacterProxy != null)
				{
					return CharacterProxy.SupportNormal;
				}
				return base.SupportNormal;
			}
			set
			{
				base.SupportNormal = value;
			}
		}

		/// <summary>
		/// Returns true when linear velocity or angular velocity is non-zero.
		/// </summary>
		public override bool IsMoving
		{
			get
			{
				if (Vector3.IsZero(LinearVelocity))
				{
					return !Vector3.IsZero(AngularVelocity);
				}
				return true;
			}
		}

		public override Vector3 Gravity
		{
			get
			{
				if (!Enabled)
				{
					return Vector3.Zero;
				}
				if (RigidBody != null)
				{
					return RigidBody.Gravity;
				}
				if (CharacterProxy != null)
				{
					return CharacterProxy.Gravity;
				}
				return Vector3.Zero;
			}
			set
			{
				HkRigidBody rigidBody = RigidBody;
				if (rigidBody != null)
				{
					rigidBody.Gravity = value;
				}
				if (CharacterProxy != null)
				{
					CharacterProxy.Gravity = value;
				}
			}
		}

		public override bool HasRigidBody => RigidBody != null;

		public override Vector3 CenterOfMassLocal => RigidBody.CenterOfMassLocal;

		public override Vector3D CenterOfMassWorld => RigidBody.CenterOfMassWorld + Offset;

		public HashSetReader<HkConstraint> Constraints => m_constraints;

		public virtual bool IsStaticForCluster
		{
			get
			{
				return m_isStaticForCluster;
			}
			set
			{
				m_isStaticForCluster = value;
			}
		}

		bool MyClusterTree.IMyActivationHandler.IsStaticForCluster => IsStaticForCluster;

		public Vector3 LastLinearVelocity => m_lastLinearVelocity;

		public Vector3 LastAngularVelocity => m_lastAngularVelocity;

		public override HkdBreakableBody BreakableBody
		{
			get
			{
				return m_breakableBody;
			}
			set
			{
				m_breakableBody = value;
				RigidBody = value;
			}
		}

<<<<<<< HEAD
		/// This System Collision ID is used for ragdoll in non-dead mode to avoid collision with character's rigid body
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int RagdollSystemGroupCollisionFilterID { get; private set; }

		/// <summary>
		/// Returns true when ragdoll is in world
		/// </summary>
		public bool IsRagdollModeActive
		{
			get
			{
				if (Ragdoll == null)
				{
					return false;
				}
				return Ragdoll.InWorld;
			}
		}

		public HkRagdoll Ragdoll
		{
			get
			{
				return m_ragdoll;
			}
			set
			{
				m_ragdoll = value;
				if (m_ragdoll != null)
				{
					HkRagdoll ragdoll = m_ragdoll;
					ragdoll.AddedToWorld = (Action<HkRagdoll>)Delegate.Combine(ragdoll.AddedToWorld, new Action<HkRagdoll>(OnRagdollAddedToWorld));
				}
			}
		}

		public bool ReactivateRagdoll { get; set; }

		public bool SwitchToRagdollModeOnActivate { get; set; }

		public bool IsWelded => WeldInfo.Parent != null;

		[Obsolete]
		public MyWeldInfo WeldInfo => m_weldInfo;

		public HkRigidBody WeldedRigidBody { get; protected set; }

		/// <summary>
		/// Not thread safe event. Needs to be called from update thread!
		/// </summary>
		public override event Action<MyPhysicsComponentBase, bool> OnBodyActiveStateChanged
		{
			add
			{
				_ = IsInWorld;
				m_onBodyActiveStateChangedHandler = (Action<MyPhysicsComponentBase, bool>)Delegate.Combine(m_onBodyActiveStateChangedHandler, value);
				RegisterActivationCallbacksIfNeeded();
			}
			remove
			{
				_ = IsInWorld;
				m_onBodyActiveStateChangedHandler = (Action<MyPhysicsComponentBase, bool>)Delegate.Remove(m_onBodyActiveStateChangedHandler, value);
				if (!NeedsActivationCallback)
				{
					UnregisterActivationCallbacks();
				}
			}
		}

		public event PhysicsContactHandler ContactPointCallback
		{
			add
			{
				_ = IsInWorld;
				m_contactPointCallbackHandler = (PhysicsContactHandler)Delegate.Combine(m_contactPointCallbackHandler, value);
				RegisterContactPointCallbackIfNeeded();
			}
			remove
			{
				_ = IsInWorld;
				m_contactPointCallbackHandler = (PhysicsContactHandler)Delegate.Remove(m_contactPointCallbackHandler, value);
				if (!NeedsContactPointCallback)
				{
					UnregisterContactPointCallback();
				}
			}
		}

		private void OnContactPointCallback(ref HkContactPointEvent e)
		{
			if (m_contactPointCallbackHandler != null)
			{
				MyPhysics.MyContactPointEvent myContactPointEvent = default(MyPhysics.MyContactPointEvent);
				myContactPointEvent.ContactPointEvent = e;
				myContactPointEvent.Position = e.ContactPoint.Position + Offset;
				MyPhysics.MyContactPointEvent e2 = myContactPointEvent;
				m_contactPointCallbackHandler(ref e2);
			}
		}

		private void OnDynamicRigidBodyActivated(HkEntity entity)
		{
			SynchronizeKeyframedRigidBody();
			InvokeOnBodyActiveStateChanged(active: true);
		}

		private void OnDynamicRigidBodyDeactivated(HkEntity entity)
		{
			SynchronizeKeyframedRigidBody();
			InvokeOnBodyActiveStateChanged(active: false);
		}

		protected void InvokeOnBodyActiveStateChanged(bool active)
		{
			m_onBodyActiveStateChangedHandler.InvokeIfNotNull(this, active);
		}

		private void RegisterActivationCallbacksIfNeeded()
		{
			if (!m_activationCallbackRegistered && NeedsActivationCallback && !(m_rigidBody == null))
			{
				m_activationCallbackRegistered = true;
				m_rigidBody.Activated += OnDynamicRigidBodyActivated;
				m_rigidBody.Deactivated += OnDynamicRigidBodyDeactivated;
			}
		}

		private void RegisterContactPointCallbackIfNeeded()
		{
			if (!m_contactPointCallbackRegistered && NeedsContactPointCallback && !(m_rigidBody == null))
			{
				m_contactPointCallbackRegistered = true;
				m_rigidBody.ContactPointCallback += OnContactPointCallback;
			}
		}

		private void UnregisterActivationCallbacks()
		{
			if (m_activationCallbackRegistered)
			{
				m_activationCallbackRegistered = false;
				if (!(m_rigidBody == null))
				{
					m_rigidBody.Activated -= OnDynamicRigidBodyActivated;
					m_rigidBody.Deactivated -= OnDynamicRigidBodyDeactivated;
				}
			}
		}

		private void UnregisterContactPointCallback()
		{
			if (m_contactPointCallbackRegistered)
			{
				m_contactPointCallbackRegistered = false;
				if (!(m_rigidBody == null))
				{
					m_rigidBody.ContactPointCallback -= OnContactPointCallback;
				}
			}
		}

		protected override void CloseRigidBody()
		{
			if (IsWelded)
			{
				WeldInfo.Parent.Unweld(this, insertToWorld: false);
			}
			if (WeldInfo.Children.get_Count() != 0)
			{
				MyWeldingGroups.ReplaceParent(MyWeldingGroups.Static.GetGroup((MyEntity)base.Entity), (MyEntity)base.Entity, null);
			}
			CheckRBNotInWorld();
			if (RigidBody != null)
			{
				if (!RigidBody.IsDisposed)
				{
					RigidBody.Dispose();
				}
				RigidBody = null;
			}
			if (RigidBody2 != null)
			{
				RigidBody2.Dispose();
				RigidBody2 = null;
			}
			if (BreakableBody != null)
			{
				BreakableBody.Dispose();
				BreakableBody = null;
			}
			if (WeldedRigidBody != null)
			{
				WeldedRigidBody.Dispose();
				WeldedRigidBody = null;
			}
		}

		private Vector3 GetLinearVelocity(bool safe)
		{
			if (!Enabled)
			{
				return Vector3.Zero;
			}
			if (RigidBody != null)
			{
				if (MyMultiplayer.Static != null && !Sync.IsServer)
				{
					MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MyEntity entityById = MyEntities.GetEntityById(myCubeGrid.ClosestParentId);
						if (entityById != null && entityById.Physics != null)
						{
							if (safe)
							{
								return entityById.Physics.LinearVelocity + RigidBody.LinearVelocity;
							}
							return entityById.Physics.LinearVelocityUnsafe + RigidBody.LinearVelocityUnsafe;
						}
						if (IsStatic)
						{
							if (safe)
							{
								return base.LinearVelocity;
							}
							return base.LinearVelocityUnsafe;
						}
					}
					else if (IsStatic)
					{
						MyCharacter myCharacter = base.Entity as MyCharacter;
						if (myCharacter != null)
						{
							MyEntity entityById2 = MyEntities.GetEntityById(myCharacter.ClosestParentId);
							if (entityById2 != null && entityById2.Physics != null)
							{
								if (myCharacter.InheritRotation)
								{
									Vector3D worldPos = base.Entity.PositionComp.GetPosition();
									entityById2.Physics.GetVelocityAtPointLocal(ref worldPos, out var linearVelocity);
									if (safe)
									{
										return linearVelocity + base.LinearVelocity;
									}
									return linearVelocity + base.LinearVelocityUnsafe;
								}
								if (safe)
								{
									return entityById2.Physics.LinearVelocity + base.LinearVelocity;
								}
								return entityById2.Physics.LinearVelocityUnsafe + base.LinearVelocityUnsafe;
							}
						}
						if (safe)
						{
							return base.LinearVelocity;
						}
						return base.LinearVelocityUnsafe;
					}
				}
				if (safe)
				{
					return RigidBody.LinearVelocity;
				}
				return RigidBody.LinearVelocityUnsafe;
			}
			if (CharacterProxy != null)
			{
				if (MyMultiplayer.Static != null && !Sync.IsServer)
				{
					MyCharacter myCharacter2 = (MyCharacter)base.Entity;
					MyEntity entityById3 = MyEntities.GetEntityById(myCharacter2.ClosestParentId);
					if (entityById3 != null && entityById3.Physics != null)
					{
						if (myCharacter2.InheritRotation)
						{
							Vector3D worldPos2 = base.Entity.PositionComp.GetPosition();
							entityById3.Physics.GetVelocityAtPointLocal(ref worldPos2, out var linearVelocity2);
							return linearVelocity2 + CharacterProxy.LinearVelocity;
						}
						if (safe)
						{
							return entityById3.Physics.LinearVelocity + CharacterProxy.LinearVelocity;
						}
						return entityById3.Physics.LinearVelocityUnsafe + CharacterProxy.LinearVelocity;
					}
				}
				return CharacterProxy.LinearVelocity;
			}
			if (Ragdoll != null && Ragdoll.IsActive)
			{
				if (safe)
				{
					return Ragdoll.GetRootRigidBody().LinearVelocity;
				}
				return Ragdoll.GetRootRigidBody().LinearVelocityUnsafe;
			}
			if (safe)
			{
				return base.LinearVelocity;
			}
			return base.LinearVelocityUnsafe;
		}

		private void SetLinearVelocity(Vector3 value, bool safe)
		{
			if (RigidBody != null)
			{
				if (safe)
				{
					RigidBody.LinearVelocity = value;
				}
				else
				{
					RigidBody.LinearVelocityUnsafe = value;
				}
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.LinearVelocity = value;
			}
			if (Ragdoll != null && Ragdoll.IsActive)
			{
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					if (safe)
					{
						rigidBody.LinearVelocity = value;
					}
					else
					{
						rigidBody.LinearVelocityUnsafe = value;
					}
				}
			}
			if (safe)
			{
				base.LinearVelocity = value;
			}
			else
			{
				base.LinearVelocityUnsafe = value;
			}
		}

		public MyPhysicsBody()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sandbox.Engine.Physics.MyPhysicsBody" /> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="flags"></param>
		public MyPhysicsBody(IMyEntity entity, RigidBodyFlag flags)
		{
			base.Entity = entity;
			m_enabled = false;
			Flags = flags;
			IsSubpart = false;
		}

		private void OnContactSoundCallback(ref HkContactPointEvent e)
		{
			if (Sync.IsServer && MyAudioComponent.ShouldPlayContactSound(base.Entity.EntityId, e.EventType))
			{
				ContactPointWrapper wrap = new ContactPointWrapper(ref e);
				wrap.WorldPosition = ClusterToWorld(wrap.position);
				MySandboxGame.Static.Invoke(delegate
				{
					MyAudioComponent.PlayContactSound(wrap, base.Entity);
				}, "MyAudioComponent::PlayContactSound");
			}
		}

		public override void Close()
		{
			CloseRagdoll();
			base.Close();
			if (CharacterProxy != null)
			{
				CharacterProxy.Dispose();
				CharacterProxy = null;
			}
		}

		/// <summary>
		/// Applies external force to the physics object.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="force">The force.</param>
		/// <param name="position">The position.</param>
		/// <param name="torque">The torque.</param>
		/// <param name="maxSpeed">Clamp max speed</param>
		/// <param name="applyImmediately">Apply immediately or enqueue to apply right before simulation</param>
		/// <param name="activeOnly">Only apply if the object is already active</param>
		public override void AddForce(MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, float? maxSpeed = null, bool applyImmediately = true, bool activeOnly = false)
		{
			if (applyImmediately)
			{
				AddForceInternal(type, force, position, torque, maxSpeed, activeOnly);
				return;
			}
			if (!activeOnly && !IsActive)
			{
				if (RigidBody != null)
				{
					RigidBody.Activate();
				}
				else if (CharacterProxy != null)
				{
					CharacterProxy.GetHitRigidBody().Activate();
				}
				else if (Ragdoll != null)
				{
					Ragdoll.Activate();
				}
			}
			lock (MyPhysics.QueuedForces)
			{
				MyPhysics.QueuedForces.Enqueue(new MyPhysics.ForceInfo(this, activeOnly, maxSpeed, force, torque, position, type));
			}
		}

		private void AddForceInternal(MyPhysicsForceType type, Vector3? force, Vector3D? position, Vector3? torque, float? maxSpeed, bool activeOnly)
		{
			if (IsStatic || (activeOnly && !IsActive))
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_FORCES)
			{
				MyPhysicsDebugDraw.DebugDrawAddForce(this, type, force, position, torque);
			}
			switch (type)
			{
			case MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE:
				if (RigidBody != null)
				{
					RigidBody.GetRigidBodyMatrix(out Matrix matrix);
					AddForceTorqueBody(force, torque, position, RigidBody, ref matrix);
				}
				if (CharacterProxy != null && CharacterProxy.GetHitRigidBody() != null)
				{
					MatrixD m = base.Entity.WorldMatrix;
					Matrix matrix = m;
					AddForceTorqueBody(force, torque, position, CharacterProxy.GetHitRigidBody(), ref matrix);
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					MatrixD m = base.Entity.WorldMatrix;
					Matrix matrix = m;
					ApplyForceTorqueOnRagdoll(force, torque, Ragdoll, ref matrix);
				}
				break;
			case MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE:
				ApplyImplusesWorld(force, position, torque, RigidBody);
				if (CharacterProxy != null && force.HasValue)
				{
					CharacterProxy.ApplyLinearImpulse(force.Value);
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					ApplyImpuseOnRagdoll(force, position, torque, Ragdoll);
				}
				break;
			case MyPhysicsForceType.APPLY_WORLD_FORCE:
				ApplyForceWorld(force, position, RigidBody);
				if (CharacterProxy != null)
				{
					if (CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
					{
						CharacterProxy.ApplyLinearImpulse(force.Value / Mass * 10f);
					}
					else
					{
						CharacterProxy.ApplyLinearImpulse(force.Value / Mass);
					}
				}
				if (Ragdoll != null && Ragdoll.InWorld && !Ragdoll.IsKeyframed)
				{
					ApplyForceOnRagdoll(force, position, Ragdoll);
				}
				break;
			}
			if (LinearVelocity.LengthSquared() > maxSpeed * maxSpeed)
			{
				Vector3 linearVelocity = LinearVelocity;
				linearVelocity.Normalize();
				linearVelocity *= maxSpeed.Value;
				if (RigidBody != null && MyMultiplayer.Static != null && !Sync.IsServer && base.Entity is MyCubeGrid && MyEntities.TryGetEntityById(((MyCubeGrid)base.Entity).ClosestParentId, out var entity))
				{
					linearVelocity -= entity.Physics.LinearVelocity;
				}
				LinearVelocity = linearVelocity;
			}
		}

		private void ApplyForceWorld(Vector3? force, Vector3D? position, HkRigidBody rigidBody)
		{
			if (!(rigidBody == null) && force.HasValue && !MyUtils.IsZero(force.Value))
			{
				if (position.HasValue)
				{
					Vector3 point = position.Value - Offset;
					rigidBody.ApplyForce(0.0166666675f, force.Value, point);
				}
				else
				{
					rigidBody.ApplyForce(0.0166666675f, force.Value);
				}
			}
		}

		private void ApplyImplusesWorld(Vector3? force, Vector3D? position, Vector3? torque, HkRigidBody rigidBody)
		{
			if (!(rigidBody == null))
			{
				if (force.HasValue && position.HasValue)
				{
					rigidBody.ApplyPointImpulse(force.Value, position.Value - Offset);
				}
				if (torque.HasValue)
				{
					rigidBody.ApplyAngularImpulse(torque.Value * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				}
			}
		}

		private void AddForceTorqueBody(Vector3? force, Vector3? torque, Vector3? position, HkRigidBody rigidBody, ref Matrix transform)
		{
			if (force.HasValue && !MyUtils.IsZero(force.Value))
			{
				Vector3 normal = force.Value;
				Vector3.TransformNormal(ref normal, ref transform, out normal);
				if (position.HasValue)
				{
					Vector3 position2 = position.Value;
					Vector3.Transform(ref position2, ref transform, out position2);
					ApplyForceWorld(normal, position2 + Offset, rigidBody);
				}
				else
				{
					rigidBody.ApplyLinearImpulse(normal * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				}
			}
			if (torque.HasValue && !MyUtils.IsZero(torque.Value))
			{
				Vector3 vector = Vector3.TransformNormal(torque.Value, transform);
				rigidBody.ApplyAngularImpulse(vector * 0.0166666675f * MyFakes.SIMULATION_SPEED);
				Vector3 angularVelocity = rigidBody.AngularVelocity;
				float maxAngularVelocity = rigidBody.MaxAngularVelocity;
				if (angularVelocity.LengthSquared() > maxAngularVelocity * maxAngularVelocity)
				{
					angularVelocity.Normalize();
					angularVelocity *= maxAngularVelocity;
					rigidBody.AngularVelocity = angularVelocity;
				}
			}
		}

		/// <summary>
		/// Applies the impulse.
		/// </summary>
		/// <param name="impulse">The dir.</param>
		/// <param name="pos">The pos.</param>
		public override void ApplyImpulse(Vector3 impulse, Vector3D pos)
		{
			AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, impulse, pos, null);
		}

		/// <summary>
		/// Clears the speeds.
		/// </summary>
		public override void ClearSpeed()
		{
			if (RigidBody != null)
			{
				RigidBody.LinearVelocity = Vector3.Zero;
				RigidBody.AngularVelocity = Vector3.Zero;
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.LinearVelocity = Vector3.Zero;
				CharacterProxy.AngularVelocity = Vector3.Zero;
				CharacterProxy.PosX = 0f;
				CharacterProxy.PosY = 0f;
				CharacterProxy.Elevate = 0f;
			}
		}

		/// <summary>
		/// Clear all dynamic values of physics object.
		/// </summary>
		public override void Clear()
		{
			ClearSpeed();
		}

		public override void DebugDraw()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS)
			{
				int num = 0;
				Enumerator<HkConstraint> enumerator = Constraints.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						HkConstraint current = enumerator.get_Current();
						if (!current.IsDisposed)
						{
							Color color = Color.Green;
							if (!IsConstraintValid(current))
							{
								color = Color.Red;
							}
							else if (!current.Enabled)
							{
								color = Color.Yellow;
							}
							current.GetPivotsInWorld(out var pivotA, out var pivotB);
							Vector3D vector3D = ClusterToWorld(pivotA);
							Vector3D vector3D2 = ClusterToWorld(pivotB);
							MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, color, color, depthRead: false);
							MyRenderProxy.DebugDrawSphere(vector3D, 0.2f, color, 1f, depthRead: false);
							MyRenderProxy.DebugDrawText3D(vector3D, num + " A", Color.White, 0.7f, depthRead: true);
							MyRenderProxy.DebugDrawSphere(vector3D2, 0.2f, color, 1f, depthRead: false);
							MyRenderProxy.DebugDrawText3D(vector3D2, num + " B", Color.White, 0.7f, depthRead: true);
							num++;
						}
<<<<<<< HEAD
						constraint.GetPivotsInWorld(out var pivotA, out var pivotB);
						Vector3D vector3D = ClusterToWorld(pivotA);
						Vector3D vector3D2 = ClusterToWorld(pivotB);
						MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, color, color, depthRead: false);
						MyRenderProxy.DebugDrawSphere(vector3D, 0.2f, color, 1f, depthRead: false);
						MyRenderProxy.DebugDrawText3D(vector3D, num + " A", Color.White, 0.7f, depthRead: true);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.2f, color, 1f, depthRead: false);
						MyRenderProxy.DebugDrawText3D(vector3D2, num + " B", Color.White, 0.7f, depthRead: true);
						num++;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_INERTIA_TENSORS && RigidBody != null)
			{
				Vector3D vector3D3 = ClusterToWorld(RigidBody.CenterOfMassWorld);
				MyRenderProxy.DebugDrawLine3D(vector3D3, vector3D3 + RigidBody.AngularVelocity, Color.Blue, Color.Red, depthRead: false);
				float num2 = 1f / RigidBody.Mass;
				Vector3 scale = RigidBody.InertiaTensor.Scale;
				float num3 = (scale.X - scale.Y + scale.Z) * num2 * 6f;
				float num4 = scale.X * num2 * 12f - num3;
				float num5 = scale.Z * num2 * 12f - num3;
				float num6 = 0.505f;
				Vector3 vector = new Vector3(Math.Sqrt(num5), Math.Sqrt(num3), Math.Sqrt(num4)) * num6;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(new BoundingBoxD(-vector, vector), MatrixD.Identity);
				Matrix m = RigidBody.GetRigidBodyMatrix();
				obb.Transform(m);
				obb.Center = CenterOfMassWorld;
				MyRenderProxy.DebugDrawOBB(obb, Color.Purple, 0.05f, depthRead: false, smooth: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_MOTION_TYPES && RigidBody != null)
			{
				MyRenderProxy.DebugDrawText3D(CenterOfMassWorld, RigidBody.GetMotionType().ToString(), Color.Purple, 0.5f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES && !IsWelded)
			{
				if (RigidBody != null && BreakableBody != null)
				{
					_ = Vector3D.Transform((Vector3D)BreakableBody.BreakableShape.CoM, RigidBody.GetRigidBodyMatrix()) + Offset;
					Color color2 = ((RigidBody.GetMotionType() != HkMotionType.Box_Inertia) ? Color.Gray : (RigidBody.IsActive ? Color.Red : Color.Blue));
					MyRenderProxy.DebugDrawSphere(RigidBody.CenterOfMassWorld + Offset, 0.2f, color2, 1f, depthRead: false);
					MyRenderProxy.DebugDrawAxis(base.Entity.PositionComp.WorldMatrixRef, 0.2f, depthRead: false);
				}
				if (RigidBody != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyMatrix = RigidBody.GetRigidBodyMatrix();
					MatrixD worldMatrix = MatrixD.CreateWorld(rigidBodyMatrix.Translation + Offset, rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(RigidBody.GetShape(), worldMatrix, 0.3f, ref shapeIndex);
				}
				if (RigidBody2 != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyMatrix2 = RigidBody2.GetRigidBodyMatrix();
					MatrixD worldMatrix2 = MatrixD.CreateWorld(rigidBodyMatrix2.Translation + Offset, rigidBodyMatrix2.Forward, rigidBodyMatrix2.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(RigidBody2.GetShape(), worldMatrix2, 0.3f, ref shapeIndex);
				}
				if (CharacterProxy != null)
				{
					int shapeIndex = 0;
					Matrix rigidBodyTransform = CharacterProxy.GetRigidBodyTransform();
					MatrixD worldMatrix3 = MatrixD.CreateWorld(rigidBodyTransform.Translation + Offset, rigidBodyTransform.Forward, rigidBodyTransform.Up);
					MyPhysicsDebugDraw.DrawCollisionShape(CharacterProxy.GetShape(), worldMatrix3, 0.3f, ref shapeIndex);
				}
			}
		}

		public virtual void CreateFromCollisionObject(HkShape shape, Vector3 center, MatrixD worldTransform, HkMassProperties? massProperties = null, int collisionFilter = 15)
		{
			CloseRigidBody();
			base.Center = center;
			base.CanUpdateAccelerations = true;
			CreateBody(ref shape, massProperties);
			RigidBody.UserObject = this;
			RigidBody.SetWorldMatrix(worldTransform);
			RigidBody.Layer = collisionFilter;
			if ((Flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) > RigidBodyFlag.RBF_DEFAULT)
			{
				RigidBody.Layer = 19;
			}
		}

		protected virtual void CreateBody(ref HkShape shape, HkMassProperties? massProperties)
		{
			HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
			hkRigidBodyCinfo.AngularDamping = m_angularDamping;
			hkRigidBodyCinfo.LinearDamping = m_linearDamping;
			hkRigidBodyCinfo.Shape = shape;
			hkRigidBodyCinfo.SolverDeactivation = InitialSolverDeactivation;
			hkRigidBodyCinfo.ContactPointCallbackDelay = ContactPointDelay;
			if (massProperties.HasValue)
			{
				m_animatedClientMass = massProperties.Value.Mass;
				hkRigidBodyCinfo.SetMassProperties(massProperties.Value);
			}
			GetInfoFromFlags(hkRigidBodyCinfo, Flags);
			RigidBody = new HkRigidBody(hkRigidBodyCinfo);
		}

		protected static void GetInfoFromFlags(HkRigidBodyCinfo rbInfo, RigidBodyFlag flags)
		{
			if ((flags & RigidBodyFlag.RBF_STATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Fixed;
				rbInfo.QualityType = HkCollidableQualityType.Fixed;
			}
			else if ((flags & RigidBodyFlag.RBF_BULLET) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Bullet;
			}
			else if ((flags & RigidBodyFlag.RBF_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Keyframed;
				rbInfo.QualityType = HkCollidableQualityType.Keyframed;
			}
			else if ((flags & RigidBodyFlag.RBF_DOUBLED_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Moving;
			}
			else if ((flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Fixed;
				rbInfo.QualityType = HkCollidableQualityType.Fixed;
			}
			else if ((flags & RigidBodyFlag.RBF_DEBRIS) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Debris;
				rbInfo.SolverDeactivation = HkSolverDeactivation.Max;
			}
			else if ((flags & RigidBodyFlag.RBF_KEYFRAMED_REPORTING) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MotionType = HkMotionType.Keyframed;
				rbInfo.QualityType = HkCollidableQualityType.KeyframedReporting;
			}
			else
			{
				rbInfo.MotionType = HkMotionType.Dynamic;
				rbInfo.QualityType = HkCollidableQualityType.Moving;
			}
			if ((flags & RigidBodyFlag.RBF_UNLOCKED_SPEEDS) > RigidBodyFlag.RBF_DEFAULT)
			{
				rbInfo.MaxLinearVelocity = MyGridPhysics.LargeShipMaxLinearVelocity() * 10f;
				rbInfo.MaxAngularVelocity = MyGridPhysics.GetLargeShipMaxAngularVelocity() * 10f;
			}
		}

		public override void CreateCharacterCollision(Vector3 center, float characterWidth, float characterHeight, float crouchHeight, float ladderHeight, float headSize, float headHeight, MatrixD worldTransform, float mass, ushort collisionLayer, bool isOnlyVertical, float maxSlope, float maxImpulse, float maxSpeedRelativeToShip, bool networkProxy, float? maxForce = null)
		{
			base.Center = center;
			base.CanUpdateAccelerations = false;
			if (networkProxy)
			{
				float downOffset = (((MyCharacter)base.Entity).IsCrouching ? 1f : 0f);
				HkShape shape = MyCharacterProxy.CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0f, downOffset);
				HkMassProperties value = default(HkMassProperties);
				value.Mass = mass;
				value.InertiaTensor = Matrix.Identity;
				value.Volume = characterWidth * characterWidth * (characterHeight + 2f * characterWidth);
				CreateFromCollisionObject(shape, center, worldTransform, value, collisionLayer);
				base.CanUpdateAccelerations = false;
			}
			else
			{
				CharacterProxy = new MyCharacterProxy(isDynamic: true, isCapsule: true, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, Matrix.CreateWorld(Vector3.TransformNormal(base.Center, worldTransform) + worldTransform.Translation, worldTransform.Forward, worldTransform.Up).Translation, worldTransform.Up, worldTransform.Forward, mass, this, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, maxForce);
				CharacterProxy.GetRigidBody().ContactPointCallbackDelay = 0;
			}
		}

		protected virtual void ActivateCollision()
		{
			((MyEntity)base.Entity).RaisePhysicsChanged();
		}

		/// <summary>
		/// Deactivates this rigid body in physics.
		/// </summary>
		public override void Deactivate()
		{
			if (ClusterObjectID != ulong.MaxValue)
			{
				if (IsWelded)
				{
					Unweld(insertInWorld: false);
					return;
				}
				MyPhysics.RemoveObject(ClusterObjectID);
				ClusterObjectID = ulong.MaxValue;
				CheckRBNotInWorld();
			}
			else
			{
				if (base.Entity == null)
				{
					return;
				}
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent.Physics != null && IsInWorld)
				{
					if (((MyPhysicsBody)topMostParent.Physics).HavokWorld != null)
					{
						Deactivate(m_world);
						return;
					}
					RigidBody = null;
					RigidBody2 = null;
					CharacterProxy = null;
				}
			}
		}

		private void CheckRBNotInWorld()
		{
			if (RigidBody != null && RigidBody.InWorld)
			{
				RigidBody.RemoveFromWorld();
			}
			if (RigidBody2 != null && RigidBody2.InWorld)
			{
				RigidBody2.RemoveFromWorld();
			}
		}

		public virtual void Deactivate(object world)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			if (RigidBody != null && !RigidBody.InWorld)
			{
				return;
			}
			if (IsRagdollModeActive)
			{
				ReactivateRagdoll = true;
				CloseRagdollMode(world as HkWorld);
			}
			if (IsInWorld && RigidBody != null && !RigidBody.IsActive)
			{
				if (!RigidBody.IsFixed)
				{
					RigidBody.Activate();
				}
				else
				{
					BoundingBoxD box = base.Entity.PositionComp.WorldAABB;
					box.Inflate(0.5);
					MyPhysics.ActivateInBox(ref box);
				}
			}
			if (m_constraints.get_Count() > 0)
			{
				m_world.LockCriticalOperations();
				Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						HkConstraint current = enumerator.get_Current();
						if (!current.IsDisposed)
						{
							m_world.RemoveConstraint(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_world.UnlockCriticalOperations();
			}
			if (BreakableBody != null && m_world.DestructionWorld != null)
			{
				m_world.DestructionWorld.RemoveBreakableBody(BreakableBody);
			}
			else if (RigidBody != null && !RigidBody.IsDisposed)
			{
				m_world.RemoveRigidBody(RigidBody);
			}
			if (RigidBody2 != null && !RigidBody2.IsDisposed)
			{
				m_world.RemoveRigidBody(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.Deactivate(m_world);
			}
			CheckRBNotInWorld();
			m_world = null;
			IsInWorld = false;
		}

		private void DeactivateBatchInternal(object world)
		{
<<<<<<< HEAD
=======
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_world == null)
			{
				return;
			}
			if (IsRagdollModeActive)
			{
				ReactivateRagdoll = true;
				CloseRagdollMode(world as HkWorld);
			}
			if (BreakableBody != null && m_world.DestructionWorld != null)
			{
				m_world.DestructionWorld.RemoveBreakableBody(BreakableBody);
			}
			else if (RigidBody != null)
			{
				m_world.RemoveRigidBodyBatch(RigidBody);
			}
			if (RigidBody2 != null)
			{
				m_world.RemoveRigidBodyBatch(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.Deactivate(m_world);
			}
<<<<<<< HEAD
			foreach (HkConstraint constraint in m_constraints)
			{
				if (IsConstraintValid(constraint, checkBodiesInWorld: false))
				{
					m_constraintsRemoveBatch.Add(constraint);
				}
			}
=======
			Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkConstraint current = enumerator.get_Current();
					if (IsConstraintValid(current, checkBodiesInWorld: false))
					{
						m_constraintsRemoveBatch.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_enabled = false;
			if (EnabledChanged != null)
			{
				EnabledChanged();
			}
			m_world = null;
			IsInWorld = false;
		}

		public virtual void DeactivateBatch(object world)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			MyHierarchyComponentBase hierarchy = base.Entity.Hierarchy;
			if (hierarchy != null)
			{
				hierarchy.GetChildrenRecursive(m_batchedChildren);
				Enumerator<IMyEntity> enumerator = m_batchedChildren.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyEntity current = enumerator.get_Current();
						if (current.Physics != null && current.Physics.Enabled)
						{
							m_batchedBodies.Add((MyPhysicsBody)current.Physics);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_batchedChildren.Clear();
			}
			foreach (MyPhysicsBody batchedBody in m_batchedBodies)
			{
				batchedBody.DeactivateBatchInternal(world);
			}
			DeactivateBatchInternal(world);
		}

		public void FinishAddBatch()
		{
			ActivateCollision();
			if (EnabledChanged != null)
			{
				EnabledChanged();
			}
			foreach (HkConstraint item in m_constraintsAddBatch)
			{
				if (IsConstraintValid(item))
				{
					m_world.AddConstraint(item);
				}
			}
			m_constraintsAddBatch.Clear();
			if (CharacterProxy != null && CharacterProxy.GetRigidBody() != null)
			{
				MyPhysics.RefreshCollisionFilter(this);
			}
			if (ReactivateRagdoll)
			{
				GetRigidBodyMatrix(out m_bodyMatrix, useCenterOffset: false);
				ActivateRagdoll(m_bodyMatrix);
				ReactivateRagdoll = false;
			}
		}

		public void FinishRemoveBatch(object userData)
		{
			HkWorld hkWorld = (HkWorld)userData;
			foreach (HkConstraint item in m_constraintsRemoveBatch)
			{
				if (IsConstraintValid(item, checkBodiesInWorld: false))
				{
					hkWorld.RemoveConstraint(item);
				}
			}
			if (IsRagdollModeActive)
			{
				ReactivateRagdoll = true;
				CloseRagdollMode(hkWorld);
			}
			m_constraintsRemoveBatch.Clear();
		}

		/// <summary>
		///
		/// </summary>
		public override void ForceActivate()
		{
			if (IsInWorld && RigidBody != null)
			{
				RigidBody.ForceActivate();
				m_world.RigidBodyActivated(RigidBody);
			}
		}

		public void OnMotionKinematic()
		{
			HkRigidBody rigidBody = RigidBody;
			if (rigidBody.MarkedForVelocityRecompute)
			{
				rigidBody.SetCustomVelocity(rigidBody.LinearVelocity, valid: true);
			}
		}

		public void OnMotionDynamic()
		{
			IMyEntity entity = base.Entity;
			if (entity == null || IsPhantom || (Flags & (RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE | RigidBodyFlag.RBF_NO_POSITION_UPDATES)) != 0 || (!IsSubpart && entity.Parent != null))
			{
				return;
			}
			if (base.CanUpdateAccelerations)
			{
				UpdateAccelerations();
			}
			HkRigidBody rigidBody = RigidBody;
			Vector3 translation = m_bodyMatrix.Translation;
			rigidBody.GetRigidBodyMatrix(out m_bodyMatrix);
<<<<<<< HEAD
			bool flag = (m_bodyMatrix.Translation - translation).LengthSquared() > 1E-06f || entity is MyCubeGrid;
=======
			bool flag = (m_bodyMatrix.Translation - translation).LengthSquared() > 1E-06f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!flag)
			{
				flag = rigidBody.AngularVelocity.LengthSquared() > 0.0001f;
				if (!flag)
				{
					flag = rigidBody.LinearVelocity.LengthSquared() > 0.0001f;
					if (!flag)
					{
						int num = ((entity is MyFloatingObject) ? 600 : 60);
						flag = m_motionCounter++ > num;
					}
				}
			}
			if (flag)
			{
				m_motionCounter = 0;
				GetWorldMatrix(out var matrix);
				entity.PositionComp.SetWorldMatrix(ref matrix, this);
				UpdateCluster();
				UpdateInterpolatedVelocities(rigidBody, moved: true);
			}
			else
			{
				UpdateInterpolatedVelocities(rigidBody, moved: false);
			}
		}

		private void UpdateInterpolatedVelocities(HkRigidBody rb, bool moved)
		{
			if (rb.MarkedForVelocityRecompute)
			{
				Vector3D centerOfMassWorld = CenterOfMassWorld;
				if (m_lastComPosition.HasValue && m_lastComLocal == rb.CenterOfMassLocal)
				{
					Vector3 velocity = Vector3.Zero;
					if (moved)
					{
						velocity = (Vector3)(centerOfMassWorld - m_lastComPosition.Value) / 0.0166666675f;
					}
					rb.SetCustomVelocity(velocity, valid: true);
				}
				rb.MarkedForVelocityRecompute = false;
				m_lastComPosition = centerOfMassWorld;
				m_lastComLocal = rb.CenterOfMassLocal;
			}
			else if (m_lastComPosition.HasValue)
			{
				m_lastComPosition = null;
				rb.SetCustomVelocity(Vector3.Zero, valid: false);
			}
		}

		public void SynchronizeKeyframedRigidBody()
		{
			if (RigidBody != null && RigidBody2 != null && RigidBody.IsActive != RigidBody2.IsActive)
			{
				if (RigidBody.IsActive)
				{
					RigidBody2.IsActive = true;
					return;
				}
				RigidBody2.LinearVelocity = Vector3.Zero;
				RigidBody2.AngularVelocity = Vector3.Zero;
				RigidBody2.IsActive = false;
			}
		}

		public sealed override void GetWorldMatrix(out MatrixD entityMatrix)
		{
			if (WeldInfo.Parent != null)
			{
				WeldInfo.Parent.GetWorldMatrix(out var matrix);
				MatrixD.Multiply(ref WeldInfo.Transform, ref matrix, out entityMatrix);
			}
			if (RigidBody != null)
			{
				RigidBody.GetRigidBodyMatrix(out entityMatrix);
				entityMatrix.Translation += Offset;
			}
			else if (RigidBody2 != null)
			{
				RigidBody2.GetRigidBodyMatrix(out entityMatrix);
				entityMatrix.Translation += Offset;
			}
			else if (CharacterProxy != null)
			{
				Matrix m = CharacterProxy.GetRigidBodyTransform();
				MatrixD matrixD = m;
				matrixD.Translation = CharacterProxy.Position + Offset;
				entityMatrix = matrixD;
			}
			else
			{
				if ((Ragdoll != null) & IsRagdollModeActive)
				{
					Matrix m = Ragdoll.WorldMatrix;
					entityMatrix = m;
					entityMatrix.Translation += Offset;
					return;
				}
				entityMatrix = MatrixD.Identity;
			}
			if (base.Center != Vector3.Zero)
			{
				entityMatrix.Translation -= Vector3D.TransformNormal(base.Center, ref entityMatrix);
			}
		}

		public override Vector3 GetVelocityAtPoint(Vector3D worldPos)
		{
			Vector3 worldPos2 = WorldToCluster(worldPos);
			if (RigidBody != null)
			{
				return RigidBody.GetVelocityAtPoint(worldPos2);
			}
			return Vector3.Zero;
		}

		public override void GetVelocityAtPointLocal(ref Vector3D worldPos, out Vector3 linearVelocity)
		{
			Vector3 vector = worldPos - CenterOfMassWorld;
			linearVelocity = Vector3.Cross(AngularVelocityLocal, vector);
			linearVelocity.Add(LinearVelocity);
		}

		/// <summary>
		/// Called when [world position changed].
		/// </summary>
		/// <param name="source">The source object that caused this event.</param>
		public override void OnWorldPositionChanged(object source)
		{
			if (!IsInWorld)
			{
				return;
			}
			Vector3 velocity = Vector3.Zero;
			IMyEntity topMostParent = base.Entity.GetTopMostParent();
			if (topMostParent.Physics != null)
			{
				velocity = topMostParent.Physics.LinearVelocity;
			}
			if (!IsWelded && ClusterObjectID != ulong.MaxValue)
			{
				MyPhysics.MoveObject(ClusterObjectID, topMostParent.WorldAABB, velocity);
			}
			GetRigidBodyMatrix(out var m);
			if (m.EqualsFast(ref m_bodyMatrix) && CharacterProxy == null)
			{
				return;
			}
			m_bodyMatrix = m;
			if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
			}
			if (CharacterProxy != null)
			{
				CharacterProxy.Speed = 0f;
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
			}
			if (Ragdoll == null || !IsRagdollModeActive)
			{
				return;
			}
			bool flag = source is MyCockpit;
			bool flag2 = source == MyGridPhysicalHierarchy.Static;
			if (flag || flag2)
			{
				if (flag)
				{
					Ragdoll.ResetToRigPose();
				}
				GetRigidBodyMatrix(out var m2, useCenterOffset: false);
				MyCharacter myCharacter = (MyCharacter)base.Entity;
				bool flag3 = flag2 && !myCharacter.IsClientPredicted;
				bool flag4 = myCharacter.m_positionResetFromServer || Vector3D.DistanceSquared(Ragdoll.WorldMatrix.Translation, m2.Translation) > 0.5;
				Ragdoll.SetWorldMatrix(m2, !flag4 && flag3, updateVelocities: true);
				if (flag)
				{
					SetRagdollVelocities();
				}
			}
		}

		protected void GetRigidBodyMatrix(out Matrix m, bool useCenterOffset = true)
		{
			MatrixD m2 = base.Entity.WorldMatrix;
			if (base.Center != Vector3.Zero && useCenterOffset)
			{
				m2.Translation += Vector3.TransformNormal(base.Center, m2);
			}
			m2.Translation -= Offset;
			m = m2;
		}

		protected Matrix GetRigidBodyMatrix()
		{
			Vector3 vector = Vector3.TransformNormal(base.Center, base.Entity.WorldMatrix);
			Vector3D objectOffset = MyPhysics.GetObjectOffset(ClusterObjectID);
			return Matrix.CreateWorld((Vector3D)vector + base.Entity.GetPosition() - objectOffset, base.Entity.WorldMatrix.Forward, base.Entity.WorldMatrix.Up);
		}

		protected Matrix GetRigidBodyMatrix(MatrixD worldMatrix)
		{
			if (base.Center != Vector3.Zero)
			{
				worldMatrix.Translation += Vector3D.TransformNormal(base.Center, ref worldMatrix);
			}
			worldMatrix.Translation -= Offset;
			return worldMatrix;
		}

		public virtual void ChangeQualityType(HkCollidableQualityType quality)
		{
			RigidBody.Quality = quality;
		}

		private static bool IsConstraintValid(HkConstraint constraint, bool checkBodiesInWorld)
		{
			if (constraint == null)
			{
				return false;
			}
			if (constraint.IsDisposed)
			{
				return false;
			}
			HkRigidBody rigidBodyA = constraint.RigidBodyA;
			HkRigidBody rigidBodyB = constraint.RigidBodyB;
			if (rigidBodyA == null || rigidBodyB == null)
			{
				return false;
			}
			if (checkBodiesInWorld)
			{
				if (!rigidBodyA.InWorld || !rigidBodyB.InWorld)
				{
					return false;
				}
				if (((MyPhysicsBody)rigidBodyA.UserObject).HavokWorld != ((MyPhysicsBody)rigidBodyB.UserObject).HavokWorld)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsConstraintValid(HkConstraint constraint)
		{
			return IsConstraintValid(constraint, checkBodiesInWorld: true);
		}

		public void AddConstraint(HkConstraint constraint)
		{
			if (IsWelded)
			{
				WeldInfo.Parent.AddConstraint(constraint);
			}
			else if (HavokWorld != null && !(RigidBody == null) && IsConstraintValid(constraint))
			{
				m_constraints.Add(constraint);
				HavokWorld.AddConstraint(constraint);
				if (!MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS && !Sync.IsServer)
				{
					UpdateConstraintForceDisable(constraint);
				}
			}
		}

		public void UpdateConstraintsForceDisable()
		{
<<<<<<< HEAD
			if (MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS || Sync.IsServer)
			{
				return;
			}
			foreach (HkConstraint constraint in m_constraints)
			{
				UpdateConstraintForceDisable(constraint);
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS || Sync.IsServer)
			{
				return;
			}
			Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkConstraint current = enumerator.get_Current();
					UpdateConstraintForceDisable(current);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UpdateConstraintForceDisable(HkConstraint constraint)
		{
			if (!(constraint.RigidBodyA.GetEntity(0u) is MyCharacter) && !(constraint.RigidBodyB.GetEntity(0u) is MyCharacter) && constraint.InWorld)
			{
				MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
				if ((myCubeGrid == null || !myCubeGrid.IsClientPredicted) && !IsPhantomOrSubPart(constraint.RigidBodyA) && !IsPhantomOrSubPart(constraint.RigidBodyB))
				{
					constraint.ForceDisabled = true;
				}
				else
				{
					constraint.ForceDisabled = false;
				}
			}
		}

		private static bool IsPhantomOrSubPart(HkRigidBody rigidBody)
		{
			MyPhysicsBody myPhysicsBody = (MyPhysicsBody)rigidBody.UserObject;
			if (!myPhysicsBody.IsPhantom)
			{
				return myPhysicsBody.IsSubpart;
			}
			return true;
		}

		public bool RemoveConstraint(HkConstraint constraint)
		{
			if (IsWelded)
			{
				m_constraints.Remove(constraint);
				WeldInfo.Parent.RemoveConstraint(constraint);
				return true;
			}
			m_constraints.Remove(constraint);
			if (HavokWorld != null)
			{
				HavokWorld.RemoveConstraint(constraint);
				return true;
			}
			return false;
		}

		public override Vector3D WorldToCluster(Vector3D worldPos)
		{
			return worldPos - Offset;
		}

		public override Vector3D ClusterToWorld(Vector3 clusterPos)
		{
			return (Vector3D)clusterPos + Offset;
		}

		public void EnableBatched()
		{
			if (!m_enabled)
			{
				m_enabled = true;
				if (base.Entity.InScene)
				{
					m_batchRequest = true;
					Activate();
					m_batchRequest = false;
				}
				if (EnabledChanged != null)
				{
					EnabledChanged();
				}
			}
		}

		/// <summary>
		/// Activates this rigid body in physics.
		/// </summary>
		public override void Activate()
		{
			if (Enabled)
			{
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				if (topMostParent != base.Entity && topMostParent.Physics != null)
				{
					Activate(((MyPhysicsBody)topMostParent.Physics).HavokWorld, ulong.MaxValue);
				}
				else if (ClusterObjectID == ulong.MaxValue)
				{
					ClusterObjectID = MyPhysics.TryAddEntity(base.Entity, this, m_batchRequest);
				}
			}
		}

		public virtual void Activate(object world, ulong clusterObjectID)
		{
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			m_world = (HkWorld)world;
			if (m_world == null)
			{
				return;
			}
			ClusterObjectID = clusterObjectID;
			ActivateCollision();
			IsInWorld = true;
			GetRigidBodyMatrix(out m_bodyMatrix);
			if (BreakableBody != null)
			{
				if (RigidBody != null)
				{
					RigidBody.SetWorldMatrix(m_bodyMatrix);
				}
				if (Sync.IsServer)
				{
					m_world.DestructionWorld.AddBreakableBody(BreakableBody);
				}
				else if (RigidBody != null)
				{
					m_world.AddRigidBody(RigidBody);
				}
			}
			else if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBody(RigidBody);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBody(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				RagdollSystemGroupCollisionFilterID = 0;
				CharacterSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, CharacterSystemGroupCollisionFilterID, 0, 0);
				CharacterProxy.SetCollisionFilterInfo(CharacterCollisionFilter);
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
				CharacterProxy.Activate(m_world);
			}
			if (ReactivateRagdoll)
			{
				GetRigidBodyMatrix(out m_bodyMatrix, useCenterOffset: false);
				ActivateRagdoll(m_bodyMatrix);
				ReactivateRagdoll = false;
			}
			if (SwitchToRagdollModeOnActivate)
			{
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				SwitchToRagdollModeOnActivate = false;
				SwitchToRagdollMode(m_ragdollDeadMode);
			}
			m_world.LockCriticalOperations();
			Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkConstraint current = enumerator.get_Current();
					if (IsConstraintValid(current))
					{
						m_world.AddConstraint(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_world.UnlockCriticalOperations();
			m_enabled = true;
		}

		private void ActivateBatchInternal(object world)
		{
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			m_world = (HkWorld)world;
			IsInWorld = true;
			GetRigidBodyMatrix(out m_bodyMatrix);
			if (RigidBody != null)
			{
				RigidBody.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBodyBatch(RigidBody);
			}
			if (RigidBody2 != null)
			{
				RigidBody2.SetWorldMatrix(m_bodyMatrix);
				m_world.AddRigidBodyBatch(RigidBody2);
			}
			if (CharacterProxy != null)
			{
				RagdollSystemGroupCollisionFilterID = 0;
				CharacterSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, CharacterSystemGroupCollisionFilterID, 1, 1);
				CharacterProxy.SetCollisionFilterInfo(CharacterCollisionFilter);
				CharacterProxy.SetRigidBodyTransform(ref m_bodyMatrix);
				CharacterProxy.Activate(m_world);
			}
			if (SwitchToRagdollModeOnActivate)
			{
				_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
				SwitchToRagdollModeOnActivate = false;
				SwitchToRagdollMode(m_ragdollDeadMode);
			}
			Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkConstraint current = enumerator.get_Current();
					m_constraintsAddBatch.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_enabled = true;
		}

		public virtual void ActivateBatch(object world, ulong clusterObjectID)
		{
			IMyEntity topMostParent = base.Entity.GetTopMostParent();
			if (topMostParent != base.Entity && topMostParent.Physics != null)
			{
				return;
<<<<<<< HEAD
			}
			ClusterObjectID = clusterObjectID;
			foreach (MyPhysicsBody batchedBody in m_batchedBodies)
			{
				batchedBody.ActivateBatchInternal(world);
			}
=======
			}
			ClusterObjectID = clusterObjectID;
			foreach (MyPhysicsBody batchedBody in m_batchedBodies)
			{
				batchedBody.ActivateBatchInternal(world);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_batchedBodies.Clear();
			ActivateBatchInternal(world);
		}

		public void UpdateCluster()
		{
			if (!MyPerGameSettings.LimitedWorld && base.Entity != null && !base.Entity.Closed)
			{
				MyPhysics.MoveObject(ClusterObjectID, base.Entity.WorldAABB, LinearVelocity);
			}
		}

		[Conditional("DEBUG")]
		private void CheckUnlockedSpeeds()
		{
			if (IsPhantom || IsSubpart || base.Entity.Parent != null)
			{
				_ = RigidBody == null;
			}
		}

		void MyClusterTree.IMyActivationHandler.Activate(object userData, ulong clusterObjectID)
		{
			Activate(userData, clusterObjectID);
		}

		void MyClusterTree.IMyActivationHandler.Deactivate(object userData)
		{
			Deactivate(userData);
		}

		void MyClusterTree.IMyActivationHandler.ActivateBatch(object userData, ulong clusterObjectID)
		{
			ActivateBatch(userData, clusterObjectID);
		}

		void MyClusterTree.IMyActivationHandler.DeactivateBatch(object userData)
		{
			DeactivateBatch(userData);
		}

		void MyClusterTree.IMyActivationHandler.FinishAddBatch()
		{
			FinishAddBatch();
		}

		void MyClusterTree.IMyActivationHandler.FinishRemoveBatch(object userData)
		{
			FinishRemoveBatch(userData);
		}

		/// <summary>
		/// Gets shape of this physics body even if its welded with other
		/// </summary>
		/// <returns></returns>
		public virtual HkShape GetShape()
		{
			if (WeldedRigidBody != null)
			{
				return WeldedRigidBody.GetShape();
			}
			HkShape shape = RigidBody.GetShape();
			if (shape.ShapeType == HkShapeType.List)
			{
				HkShapeContainerIterator container = RigidBody.GetShape().GetContainer();
				while (container.IsValid)
				{
					HkShape shape2 = container.GetShape(container.CurrentShapeKey);
					if (RigidBody.GetGcRoot() == shape2.UserData)
					{
						return shape2;
					}
					container.Next();
				}
			}
			return shape;
		}

		private static HkMassProperties? GetMassPropertiesFromDefinition(MyPhysicsBodyComponentDefinition physicsBodyComponentDefinition, MyModelComponentDefinition modelComponentDefinition)
		{
			HkMassProperties? result = null;
			MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType massPropertiesComputation = physicsBodyComponentDefinition.MassPropertiesComputation;
			if (massPropertiesComputation != 0 && massPropertiesComputation == MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType.Box)
			{
				result = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(modelComponentDefinition.Size / 2f, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(modelComponentDefinition.Mass) : modelComponentDefinition.Mass);
			}
			return result;
		}

		private void OnModelChanged(MyEntityContainerEventExtensions.EntityEventParams eventParams)
		{
			Close();
			InitializeRigidBodyFromModel();
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			Definition = definition as MyPhysicsBodyComponentDefinition;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (Definition != null)
			{
				InitializeRigidBodyFromModel();
				this.RegisterForEntityEvent(MyModelComponent.ModelChanged, OnModelChanged);
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (Definition != null)
			{
				Enabled = true;
				if (Definition.ForceActivate)
				{
					ForceActivate();
				}
			}
		}

		private void InitializeRigidBodyFromModel()
		{
			if (Definition != null && RigidBody == null && Definition.CreateFromCollisionObject && base.Container.Has<MyModelComponent>())
			{
				MyModelComponent myModelComponent = base.Container.Get<MyModelComponent>();
				if (myModelComponent.Definition != null && myModelComponent.ModelCollision != null && myModelComponent.ModelCollision.HavokCollisionShapes.Length >= 1)
				{
					HkMassProperties? massPropertiesFromDefinition = GetMassPropertiesFromDefinition(Definition, myModelComponent.Definition);
					int collisionFilter = ((Definition.CollisionLayer != null) ? MyPhysics.GetCollisionLayer(Definition.CollisionLayer) : 15);
					CreateFromCollisionObject(myModelComponent.ModelCollision.HavokCollisionShapes[0], Vector3.Zero, base.Entity.WorldMatrix, massPropertiesFromDefinition, collisionFilter);
				}
			}
		}

		public override void UpdateFromSystem()
		{
			if (Definition != null && (Definition.UpdateFlags & MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags.Gravity) != 0 && MyFakes.ENABLE_PLANETS && base.Entity != null && base.Entity.PositionComp != null && Enabled && RigidBody != null)
			{
				RigidBody.Gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.Entity.PositionComp.GetPosition());
			}
		}

		protected void NotifyConstraintsAddedToWorld()
		{
			foreach (HkConstraint notifyConstraint in m_notifyConstraints)
			{
				notifyConstraint.NotifyAddedToWorld();
			}
			m_notifyConstraints.Clear();
		}

		protected void NotifyConstraintsRemovedFromWorld()
		{
			m_notifyConstraints.AssertEmpty();
			HkRigidBody rigidBody = RigidBody;
			if (rigidBody != null)
			{
				HkConstraint.GetAttachedConstraints(rigidBody, m_notifyConstraints);
			}
			foreach (HkConstraint notifyConstraint in m_notifyConstraints)
			{
				notifyConstraint.NotifyRemovedFromWorld();
			}
		}

		public virtual void FracturedBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			e.GetNewBodies(m_tmpLst);
			if (m_tmpLst.Count == 0)
			{
				return;
			}
			MyPhysics.RemoveDestructions(RigidBody);
			foreach (HkdBreakableBodyInfo item in m_tmpLst)
			{
				HkdBreakableBody breakableBody = MyFracturedPiecesManager.Static.GetBreakableBody(item);
				Matrix m = breakableBody.GetRigidBody().GetRigidBodyMatrix();
				MatrixD worldMatrix = m;
				worldMatrix.Translation = ClusterToWorld(worldMatrix.Translation);
				if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, (base.Entity as MyFracturedPiece).OriginalBlocks) == null)
				{
					MyFracturedPiecesManager.Static.ReturnToPool(breakableBody);
				}
			}
			m_tmpLst.Clear();
			BreakableBody.AfterReplaceBody -= FracturedBody_AfterReplaceBody;
			MyFracturedPiecesManager.Static.RemoveFracturePiece(base.Entity as MyFracturedPiece, 0f);
		}

		public void CloseRagdoll()
		{
			if (Ragdoll != null)
			{
				if (IsRagdollModeActive)
				{
					CloseRagdollMode(HavokWorld);
				}
				if (Ragdoll.InWorld)
				{
					HavokWorld.RemoveRagdoll(Ragdoll);
				}
				Ragdoll.Dispose();
				Ragdoll = null;
			}
		}

		public void SwitchToRagdollMode(bool deadMode = true, int firstRagdollSubID = 1)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode");
			}
			if (HavokWorld == null || !Enabled)
			{
				SwitchToRagdollModeOnActivate = true;
				m_ragdollDeadMode = deadMode;
			}
			else
			{
				if (IsRagdollModeActive)
				{
					return;
				}
				MatrixD m = base.Entity.WorldMatrix;
				m.Translation = WorldToCluster(m.Translation);
				if (RagdollSystemGroupCollisionFilterID == 0)
				{
					RagdollSystemGroupCollisionFilterID = m_world.GetCollisionFilter().GetNewSystemGroup();
				}
				Ragdoll.SetToKeyframed();
				Ragdoll.GenerateRigidBodiesCollisionFilters(deadMode ? 18 : 31, RagdollSystemGroupCollisionFilterID, firstRagdollSubID);
				Ragdoll.ResetToRigPose();
				Ragdoll.SetWorldMatrix(m, updateOnlyKeyframed: false, updateVelocities: false);
				if (deadMode)
				{
					Ragdoll.SetToDynamic();
					SetRagdollVelocities(null, RigidBody);
				}
				else
				{
					SetRagdollVelocities();
				}
				if (CharacterProxy != null && deadMode)
				{
					CharacterProxy.Deactivate(HavokWorld);
					CharacterProxy.Dispose();
					CharacterProxy = null;
				}
				if (RigidBody != null && deadMode)
				{
					RigidBody.Deactivate();
					HavokWorld.RemoveRigidBody(RigidBody);
					RigidBody.Dispose();
					RigidBody = null;
				}
				if (RigidBody2 != null && deadMode)
				{
					RigidBody2.Deactivate();
					HavokWorld.RemoveRigidBody(RigidBody2);
					RigidBody2.Dispose();
					RigidBody2 = null;
				}
				foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
				{
					rigidBody.UserObject = this;
					rigidBody.Motion.SetDeactivationClass(deadMode ? HkSolverDeactivation.High : HkSolverDeactivation.Medium);
				}
				Ragdoll.OptimizeInertiasOfConstraintTree();
				if (!Ragdoll.InWorld)
				{
					HavokWorld.AddRagdoll(Ragdoll);
				}
				Ragdoll.EnableConstraints();
				Ragdoll.Activate();
				m_ragdollDeadMode = deadMode;
				if (MyFakes.ENABLE_RAGDOLL_DEBUG)
				{
					MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode - FINISHED");
				}
			}
		}

		private void ActivateRagdoll(Matrix worldMatrix)
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll");
			}
			if (Ragdoll == null || HavokWorld == null || IsRagdollModeActive)
			{
				return;
			}
			foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
			{
				rigidBody.UserObject = this;
			}
			Ragdoll.SetWorldMatrix(worldMatrix, updateOnlyKeyframed: false, updateVelocities: false);
			HavokWorld.AddRagdoll(Ragdoll);
			if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
			{
				DisableRagdollBodiesCollisions();
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll - FINISHED");
			}
		}

		private void OnRagdollAddedToWorld(HkRagdoll ragdoll)
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			Ragdoll.Activate();
			Ragdoll.EnableConstraints();
			HkConstraintStabilizationUtil.StabilizeRagdollInertias(ragdoll, 1f, 0f);
		}

		public void CloseRagdollMode()
		{
			CloseRagdollMode(HavokWorld);
		}

		public void CloseRagdollMode(HkWorld world)
		{
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
			if (!IsRagdollModeActive || world == null)
			{
				return;
			}
			foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
			{
				rigidBody.UserObject = null;
			}
			Ragdoll.Deactivate();
			world.RemoveRagdoll(Ragdoll);
			_ = MyFakes.ENABLE_RAGDOLL_DEBUG;
		}

		/// <summary>
		///  Sets default values for ragdoll bodies and constraints - useful if ragdoll model is not correct
		/// </summary>
		public void SetRagdollDefaults()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults");
			}
			bool isKeyframed = Ragdoll.IsKeyframed;
			Ragdoll.SetToDynamic();
			float num = (base.Entity as MyCharacter).Definition.Mass;
			if (num <= 1f)
			{
				num = 80f;
			}
			float num2 = 0f;
			foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
			{
				float num3 = 0f;
				rigidBody.GetShape().GetLocalAABB(0.01f, out var min, out var max);
				num3 = (max - min).Length();
				num2 += num3;
			}
			if (num2 <= 0f)
			{
				num2 = 1f;
			}
			foreach (HkRigidBody rigidBody2 in Ragdoll.RigidBodies)
			{
				rigidBody2.MaxLinearVelocity = 1000f;
				rigidBody2.MaxAngularVelocity = 1000f;
				HkShape shape = rigidBody2.GetShape();
				shape.GetLocalAABB(0.01f, out var min2, out var max2);
				float num4 = (max2 - min2).Length();
				float num5 = num / num2 * num4;
				rigidBody2.Mass = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(num5) : num5);
				float convexRadius = shape.ConvexRadius;
				if (shape.ShapeType == HkShapeType.Capsule)
				{
					HkCapsuleShape hkCapsuleShape = (HkCapsuleShape)shape;
					rigidBody2.InertiaTensor = HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(hkCapsuleShape.VertexA, hkCapsuleShape.VertexB, convexRadius, rigidBody2.Mass).InertiaTensor;
				}
				else
				{
					rigidBody2.InertiaTensor = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(Vector3.One * num4 * 0.5f, rigidBody2.Mass).InertiaTensor;
				}
				rigidBody2.AngularDamping = 0.005f;
				rigidBody2.LinearDamping = 0f;
				rigidBody2.Friction = 6f;
				rigidBody2.AllowedPenetrationDepth = 0.1f;
				rigidBody2.Restitution = 0.05f;
			}
			Ragdoll.OptimizeInertiasOfConstraintTree();
			if (isKeyframed)
			{
				Ragdoll.SetToKeyframed();
			}
			foreach (HkConstraint constraint in Ragdoll.Constraints)
			{
				if (constraint.ConstraintData is HkRagdollConstraintData)
				{
					(constraint.ConstraintData as HkRagdollConstraintData).MaxFrictionTorque = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f);
				}
				else if (constraint.ConstraintData is HkFixedConstraintData)
				{
					HkFixedConstraintData obj = constraint.ConstraintData as HkFixedConstraintData;
					obj.MaximumLinearImpulse = 3.40282E+28f;
					obj.MaximumAngularImpulse = 3.40282E+28f;
				}
				else if (constraint.ConstraintData is HkHingeConstraintData)
				{
					HkHingeConstraintData obj2 = constraint.ConstraintData as HkHingeConstraintData;
					obj2.MaximumAngularImpulse = 3.40282E+28f;
					obj2.MaximumLinearImpulse = 3.40282E+28f;
				}
				else if (constraint.ConstraintData is HkLimitedHingeConstraintData)
				{
					(constraint.ConstraintData as HkLimitedHingeConstraintData).MaxFrictionTorque = (MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f);
				}
			}
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults FINISHED");
			}
		}

		internal void DisableRagdollBodiesCollisions()
		{
			if (MyFakes.ENABLE_RAGDOLL_DEBUG)
			{
				_ = HavokWorld;
			}
			if (Ragdoll == null)
			{
				return;
			}
			foreach (HkRigidBody rigidBody in Ragdoll.RigidBodies)
			{
				uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(31, 0, 0, 0);
				rigidBody.SetCollisionFilterInfo(collisionFilterInfo);
				rigidBody.LinearVelocity = Vector3.Zero;
				rigidBody.AngularVelocity = Vector3.Zero;
				MyPhysics.RefreshCollisionFilter(this);
			}
		}

		private void ApplyForceTorqueOnRagdoll(Vector3? force, Vector3? torque, HkRagdoll ragdoll, ref Matrix transform)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				if (rigidBody != null)
				{
					Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
					transform = rigidBody.GetRigidBodyMatrix();
					AddForceTorqueBody(value, torque, null, rigidBody, ref transform);
				}
			}
		}

		private void ApplyImpuseOnRagdoll(Vector3? force, Vector3D? position, Vector3? torque, HkRagdoll ragdoll)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
				ApplyImplusesWorld(value, position, torque, rigidBody);
			}
		}

		private void ApplyForceOnRagdoll(Vector3? force, Vector3D? position, HkRagdoll ragdoll)
		{
			foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
			{
				Vector3 value = force.Value * rigidBody.Mass / ragdoll.Mass;
				ApplyForceWorld(value, position, rigidBody);
			}
		}

		public void SetRagdollVelocities(List<int> bodiesToUpdate = null, HkRigidBody leadingBody = null)
		{
			List<HkRigidBody> rigidBodies = Ragdoll.RigidBodies;
			if (leadingBody == null && CharacterProxy != null)
			{
				HkRigidBody hitRigidBody = CharacterProxy.GetHitRigidBody();
				if (hitRigidBody != null)
				{
					leadingBody = hitRigidBody;
				}
			}
			if (leadingBody == null)
			{
				leadingBody = rigidBodies[0];
			}
			Vector3 angularVelocity = leadingBody.AngularVelocity;
			if (bodiesToUpdate != null)
			{
				foreach (int item in bodiesToUpdate)
				{
					HkRigidBody hkRigidBody = rigidBodies[item];
					hkRigidBody.AngularVelocity = angularVelocity;
					hkRigidBody.LinearVelocity = leadingBody.GetVelocityAtPoint(hkRigidBody.Position);
				}
				return;
			}
			foreach (HkRigidBody item2 in rigidBodies)
			{
				item2.AngularVelocity = angularVelocity;
				item2.LinearVelocity = leadingBody.GetVelocityAtPoint(item2.Position);
			}
		}

		public void Weld(MyPhysicsComponentBase other, bool recreateShape = true)
		{
			Weld(other as MyPhysicsBody, recreateShape);
		}

		public void Weld(MyPhysicsBody other, bool recreateShape = true)
		{
			if (other.WeldInfo.Parent == this)
			{
				return;
			}
			if (other.IsWelded && !IsWelded)
			{
				other.Weld(this);
				return;
			}
			if (IsWelded)
			{
				WeldInfo.Parent.Weld(other);
				return;
			}
			if (other.WeldInfo.Children.get_Count() > 0)
			{
				other.UnweldAll(insertInWorld: false);
			}
			if (WeldInfo.Children.get_Count() == 0)
			{
				WeldedRigidBody = RigidBody;
				HkShape shape = RigidBody.GetShape();
				if (HavokWorld != null)
				{
					HavokWorld.RemoveRigidBody(WeldedRigidBody);
				}
				RigidBody = HkRigidBody.Clone(WeldedRigidBody);
				if (HavokWorld != null)
				{
					HavokWorld.AddRigidBody(RigidBody);
				}
				HkShape.SetUserData(shape, RigidBody);
				base.Entity.OnPhysicsChanged += WeldedEntity_OnPhysicsChanged;
				WeldInfo.UpdateMassProps(RigidBody);
			}
			else
			{
				GetShape();
			}
			other.Deactivate();
			MatrixD m = other.Entity.WorldMatrix * base.Entity.WorldMatrixInvScaled;
			other.WeldInfo.Transform = m;
			other.WeldInfo.UpdateMassProps(other.RigidBody);
			other.WeldedRigidBody = other.RigidBody;
			other.RigidBody = RigidBody;
			other.WeldInfo.Parent = this;
			other.ClusterObjectID = ClusterObjectID;
			WeldInfo.Children.Add(other);
			OnWelded(other);
			other.OnWelded(this);
		}

		private void Entity_OnClose(IMyEntity obj)
		{
			UnweldAll(insertInWorld: true);
		}

		private void WeldedEntity_OnPhysicsChanged(IMyEntity obj)
		{
<<<<<<< HEAD
=======
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (base.Entity == null || base.Entity.Physics == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyPhysicsBody child in WeldInfo.Children)
			{
				if (child.Entity == null)
				{
					child.WeldInfo.Parent = null;
					WeldInfo.Children.Remove(child);
					if (obj.Physics != null)
					{
						Weld(obj.Physics as MyPhysicsBody);
=======
			Enumerator<MyPhysicsBody> enumerator = WeldInfo.Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyPhysicsBody current = enumerator.get_Current();
					if (current.Entity == null)
					{
						current.WeldInfo.Parent = null;
						WeldInfo.Children.Remove(current);
						if (obj.Physics != null)
						{
							Weld(obj.Physics as MyPhysicsBody);
						}
						break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
			}
<<<<<<< HEAD
=======
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RecreateWeldedShape(GetShape());
		}

		public void RecreateWeldedShape()
		{
			if (WeldInfo.Children.get_Count() != 0)
			{
				RecreateWeldedShape(GetShape());
			}
		}

		public void UpdateMassProps()
		{
<<<<<<< HEAD
=======
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (RigidBody.IsFixedOrKeyframed)
			{
				return;
			}
			if (WeldInfo.Parent != null)
			{
				WeldInfo.Parent.UpdateMassProps();
				return;
			}
<<<<<<< HEAD
			int num = 1 + WeldInfo.Children.Count;
			HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num);
			array[0] = WeldInfo.MassElement;
			int num2 = 1;
			foreach (MyPhysicsBody child in WeldInfo.Children)
			{
				array[num2++] = child.WeldInfo.MassElement;
			}
=======
			int num = 1 + WeldInfo.Children.get_Count();
			HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num);
			array[0] = WeldInfo.MassElement;
			int num2 = 1;
			Enumerator<MyPhysicsBody> enumerator = WeldInfo.Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyPhysicsBody current = enumerator.get_Current();
					array[num2++] = current.WeldInfo.MassElement;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			HkInertiaTensorComputer.CombineMassProperties(new Span<HkMassElement>(array, 0, num), out var massProperties);
			RigidBody.SetMassProperties(ref massProperties);
			ArrayPool<HkMassElement>.Shared.Return(array);
		}

		private void RecreateWeldedShape(HkShape thisShape)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (RigidBody == null || RigidBody.IsDisposed)
			{
				return;
			}
			if (WeldInfo.Children.get_Count() == 0)
			{
				RigidBody.SetShape(thisShape);
				if (RigidBody2 != null)
				{
					RigidBody2.SetShape(thisShape);
				}
				return;
			}
			m_tmpShapeList.Add(thisShape);
			Enumerator<MyPhysicsBody> enumerator = WeldInfo.Children.GetEnumerator();
			try
			{
<<<<<<< HEAD
				HkTransformShape hkTransformShape = new HkTransformShape(child.WeldedRigidBody.GetShape(), ref child.WeldInfo.Transform);
				HkShape.SetUserData(hkTransformShape, child.WeldedRigidBody);
				m_tmpShapeList.Add(hkTransformShape);
				if (m_tmpShapeList.Count == 128)
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyPhysicsBody current = enumerator.get_Current();
					HkTransformShape hkTransformShape = new HkTransformShape(current.WeldedRigidBody.GetShape(), ref current.WeldInfo.Transform);
					HkShape.SetUserData(hkTransformShape, current.WeldedRigidBody);
					m_tmpShapeList.Add(hkTransformShape);
					if (m_tmpShapeList.Count == 128)
					{
						break;
					}
				}
			}
<<<<<<< HEAD
=======
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			HkSmartListShape hkSmartListShape = new HkSmartListShape(0);
			foreach (HkShape tmpShape in m_tmpShapeList)
			{
				hkSmartListShape.AddShape(tmpShape);
			}
			RigidBody.SetShape(hkSmartListShape);
			if (RigidBody2 != null)
			{
				RigidBody2.SetShape(hkSmartListShape);
			}
			hkSmartListShape.Base.RemoveReference();
			WeldedMarkBreakable();
			for (int i = 1; i < m_tmpShapeList.Count; i++)
			{
				m_tmpShapeList[i].RemoveReference();
			}
			m_tmpShapeList.Clear();
			UpdateMassProps();
		}

		private void WeldedMarkBreakable()
		{
<<<<<<< HEAD
			if (HavokWorld == null)
			{
				return;
			}
			MyGridPhysics myGridPhysics = this as MyGridPhysics;
			if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
			{
				HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, 0u, myGridPhysics.Shape.BreakImpulse);
			}
			uint num = 1u;
			foreach (MyPhysicsBody child in WeldInfo.Children)
			{
				myGridPhysics = child as MyGridPhysics;
				if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
				{
					HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, num, myGridPhysics.Shape.BreakImpulse);
=======
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			if (HavokWorld == null)
			{
				return;
			}
			MyGridPhysics myGridPhysics = this as MyGridPhysics;
			if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
			{
				HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, 0u, myGridPhysics.Shape.BreakImpulse);
			}
			uint num = 1u;
			Enumerator<MyPhysicsBody> enumerator = WeldInfo.Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					myGridPhysics = enumerator.get_Current() as MyGridPhysics;
					if (myGridPhysics != null && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
					{
						HavokWorld.BreakOffPartsUtil.MarkPieceBreakable(RigidBody, num, myGridPhysics.Shape.BreakImpulse);
					}
					num++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				num++;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void UnweldAll(bool insertInWorld)
		{
			while (WeldInfo.Children.get_Count() > 1)
			{
				Unweld(Enumerable.First<MyPhysicsBody>((IEnumerable<MyPhysicsBody>)WeldInfo.Children), insertInWorld, recreateShape: false);
			}
			if (WeldInfo.Children.get_Count() > 0)
			{
				Unweld(Enumerable.First<MyPhysicsBody>((IEnumerable<MyPhysicsBody>)WeldInfo.Children), insertInWorld);
			}
		}

		public void Unweld(MyPhysicsBody other, bool insertToWorld = true, bool recreateShape = true)
		{
			if (IsWelded)
			{
				WeldInfo.Parent.Unweld(other, insertToWorld, recreateShape);
				return;
			}
			if (other.IsInWorld || RigidBody == null || other.WeldedRigidBody == null)
			{
				WeldInfo.Children.Remove(other);
				return;
			}
			Matrix rigidBodyMatrix = RigidBody.GetRigidBodyMatrix();
			other.WeldInfo.Parent = null;
			WeldInfo.Children.Remove(other);
			HkRigidBody rigidBody = other.RigidBody;
			other.RigidBody = other.WeldedRigidBody;
			other.WeldedRigidBody = null;
			if (!other.RigidBody.IsDisposed)
			{
				other.RigidBody.SetWorldMatrix(other.WeldInfo.Transform * rigidBodyMatrix);
				other.RigidBody.LinearVelocity = rigidBody.LinearVelocity;
				other.WeldInfo.MassElement.Tranform = Matrix.Identity;
				other.WeldInfo.Transform = Matrix.Identity;
				_ = other.RigidBody2 != null;
			}
			other.ClusterObjectID = ulong.MaxValue;
			if (insertToWorld)
			{
				other.Activate();
			}
			if (WeldInfo.Children.get_Count() == 0)
			{
				recreateShape = false;
				base.Entity.OnPhysicsChanged -= WeldedEntity_OnPhysicsChanged;
				base.Entity.OnClose -= Entity_OnClose;
				WeldedRigidBody.LinearVelocity = RigidBody.LinearVelocity;
				WeldedRigidBody.AngularVelocity = RigidBody.AngularVelocity;
				if (HavokWorld != null)
				{
					HavokWorld.RemoveRigidBody(RigidBody);
				}
				RigidBody.Dispose();
				RigidBody = WeldedRigidBody;
				WeldedRigidBody = null;
				RigidBody.SetWorldMatrix(rigidBodyMatrix);
				WeldInfo.Transform = Matrix.Identity;
				if (HavokWorld != null)
				{
					HavokWorld.AddRigidBody(RigidBody);
					ActivateCollision();
				}
				else if (!base.Entity.MarkedForClose)
				{
					Activate();
				}
				if (RigidBody2 != null)
				{
					RigidBody2.SetShape(RigidBody.GetShape());
				}
			}
			if (RigidBody != null && recreateShape)
			{
				RecreateWeldedShape(GetShape());
			}
			OnUnwelded(other);
			other.OnUnwelded(this);
		}

		public void Unweld(bool insertInWorld = true)
		{
			WeldInfo.Parent.Unweld(this, insertInWorld);
		}

		protected virtual void OnWelded(MyPhysicsBody other)
		{
		}

		protected virtual void OnUnwelded(MyPhysicsBody other)
		{
		}

		private void RemoveConstraints(HkRigidBody hkRigidBody)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<HkConstraint> enumerator = m_constraints.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HkConstraint current = enumerator.get_Current();
					if (current.IsDisposed || current.RigidBodyA == hkRigidBody || current.RigidBodyB == hkRigidBody)
					{
						m_constraintsRemoveBatch.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (HkConstraint item in m_constraintsRemoveBatch)
			{
				m_constraints.Remove(item);
				if (!item.IsDisposed && item.InWorld)
				{
					HavokWorld.RemoveConstraint(item);
				}
			}
			m_constraintsRemoveBatch.Clear();
		}
	}
}
