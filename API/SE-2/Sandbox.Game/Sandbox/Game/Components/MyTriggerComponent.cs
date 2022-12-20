using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_TriggerBase), true)]
	public class MyTriggerComponent : MyEntityComponentBase
	{
		public enum TriggerType
		{
			AABB,
			Sphere,
			OBB
		}

		private class Sandbox_Game_Components_MyTriggerComponent_003C_003EActor : IActivator, IActivator<MyTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTriggerComponent CreateInstance()
			{
				return new MyTriggerComponent();
			}

			MyTriggerComponent IActivator<MyTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static uint m_triggerCounter;

		private const uint PRIME = 31u;

		private readonly uint m_updateOffset;

		protected readonly List<MyEntity> m_queryResult = new List<MyEntity>();

		protected TriggerType m_triggerType;

		protected BoundingBoxD m_AABB;

		protected BoundingSphereD m_boundingSphere;

		protected MyOrientedBoundingBoxD m_orientedBoundingBox;

		protected Vector3 m_relativeOffset;

		private bool m_updatedOnce;

		private bool m_registered;

		protected bool DoQuery { get; set; }

		protected List<MyEntity> QueryResult => m_queryResult;

		public uint UpdateFrequency { get; set; }

		public virtual bool Enabled { get; protected set; }

		public override string ComponentTypeDebugString => "Trigger";

		public Color? CustomDebugColor { get; set; }

		/// <summary>
		/// Trigger BB center position.
		/// </summary>
		public Vector3D Center
		{
			get
			{
				return m_triggerType switch
				{
<<<<<<< HEAD
				case TriggerType.AABB:
					return m_AABB.Center;
				case TriggerType.Sphere:
					return m_boundingSphere.Center;
				case TriggerType.OBB:
					return m_orientedBoundingBox.Center;
				default:
					return Vector3D.Zero;
				}
=======
					TriggerType.AABB => m_AABB.Center, 
					TriggerType.Sphere => m_boundingSphere.Center, 
					TriggerType.OBB => m_orientedBoundingBox.Center, 
					_ => Vector3D.Zero, 
				};
			}
			set
			{
				m_AABB.Centerize(value);
				m_boundingSphere.Center = value;
				m_orientedBoundingBox.Center = value;
				CalculateRelativeOffset();
			}
		}

		public MyOrientedBoundingBoxD OBB
		{
			get
			{
				return m_orientedBoundingBox;
			}
			set
			{
				m_orientedBoundingBox = value;
			}
		}

		public TriggerType TriggerAreaType
		{
			get
			{
				return m_triggerType;
			}
			set
			{
				m_triggerType = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				m_AABB.Centerize(value);
				m_boundingSphere.Center = value;
				m_orientedBoundingBox.Center = value;
				CalculateRelativeOffset();
			}
		}

		public MyOrientedBoundingBoxD OBB
		{
			get
			{
				return m_orientedBoundingBox;
			}
			set
			{
				m_orientedBoundingBox = value;
			}
		}

		public TriggerType TriggerAreaType
		{
			get
			{
				return m_triggerType;
			}
			set
			{
				m_triggerType = value;
			}
		}

		public MyTriggerComponent(TriggerType type, uint updateFrequency = 300u)
		{
			m_triggerType = type;
			UpdateFrequency = updateFrequency;
			m_updateOffset = m_triggerCounter++ * 31 % UpdateFrequency;
			DoQuery = true;
		}

		public MyTriggerComponent()
		{
			m_triggerType = TriggerType.AABB;
			UpdateFrequency = 300u;
			m_updateOffset = m_triggerCounter++ * 31 % UpdateFrequency;
			DoQuery = true;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_TriggerBase myObjectBuilder_TriggerBase = base.Serialize(copy) as MyObjectBuilder_TriggerBase;
			if (myObjectBuilder_TriggerBase != null)
			{
				myObjectBuilder_TriggerBase.AABB = m_AABB;
				myObjectBuilder_TriggerBase.BoundingSphere = m_boundingSphere;
				myObjectBuilder_TriggerBase.Type = (int)m_triggerType;
				myObjectBuilder_TriggerBase.OrientedBoundingBox = m_orientedBoundingBox;
			}
			return myObjectBuilder_TriggerBase;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_TriggerBase myObjectBuilder_TriggerBase = builder as MyObjectBuilder_TriggerBase;
			if (myObjectBuilder_TriggerBase != null)
			{
				m_AABB = myObjectBuilder_TriggerBase.AABB;
				m_boundingSphere = myObjectBuilder_TriggerBase.BoundingSphere;
				if (!myObjectBuilder_TriggerBase.OrientedBoundingBox.HalfExtent.IsZero)
				{
					m_orientedBoundingBox = myObjectBuilder_TriggerBase.OrientedBoundingBox;
				}
				else if (m_boundingSphere.Radius > 0.0)
				{
					m_orientedBoundingBox = new MyOrientedBoundingBoxD(m_boundingSphere.Center, new Vector3D(m_boundingSphere.Radius), Quaternion.Identity);
				}
				else
				{
					m_orientedBoundingBox = new MyOrientedBoundingBoxD(m_AABB.Center, m_AABB.Size / 2.0, Quaternion.Identity);
				}
				m_triggerType = ((myObjectBuilder_TriggerBase.Type != -1) ? ((TriggerType)myObjectBuilder_TriggerBase.Type) : TriggerType.AABB);
			}
		}

		private void CalculateRelativeOffset()
		{
			Vector3D vector3D = Vector3D.Zero;
			switch (m_triggerType)
			{
			case TriggerType.AABB:
				vector3D = m_AABB.Center;
				break;
			case TriggerType.Sphere:
				vector3D = m_boundingSphere.Center;
				break;
			case TriggerType.OBB:
				vector3D = m_orientedBoundingBox.Center;
				break;
			}
<<<<<<< HEAD
			Vector3 normal = vector3D - base.Entity.PositionComp.WorldMatrixRef.Translation;
			normal = (m_relativeOffset = Vector3D.TransformNormal(normal, base.Entity.PositionComp.WorldMatrixNormalizedInv));
=======
			Vector3D normal = vector3D - base.Entity.PositionComp.WorldMatrixRef.Translation;
			normal = Vector3D.TransformNormal(normal, base.Entity.PositionComp.WorldMatrixNormalizedInv);
			m_relativeOffset = normal;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void RegisterComponent()
		{
			if (!m_registered)
			{
				m_registered = true;
				IMyEntity topMostParent = base.Entity.GetTopMostParent();
				topMostParent.PositionComp.OnPositionChanged += OnEntityPositionCompPositionChanged;
				topMostParent.NeedsWorldMatrix = true;
				MySessionComponentTriggerSystem.Static.AddTrigger(this);
				CalculateRelativeOffset();
			}
		}

		private void UnRegisterComponent()
		{
			if (m_registered)
			{
				m_registered = false;
				MySessionComponentTriggerSystem.RemoveTrigger((MyEntity)base.Entity, this);
				base.Entity.GetTopMostParent().PositionComp.OnPositionChanged -= OnEntityPositionCompPositionChanged;
				Dispose();
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			RegisterComponent();
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			if (base.Entity.InScene)
			{
				RegisterComponent();
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			UnRegisterComponent();
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			UnRegisterComponent();
		}

		private void OnEntityPositionCompPositionChanged(MyPositionComponentBase myPositionComponentBase)
		{
			if (base.Entity != null && base.Entity.PositionComp != null)
			{
				Vector3 vector = Vector3.TransformNormal(m_relativeOffset, base.Entity.PositionComp.WorldMatrixRef);
				switch (m_triggerType)
				{
				case TriggerType.AABB:
				{
					Vector3D center = base.Entity.PositionComp.GetPosition() + vector;
					m_AABB.Centerize(center);
					break;
				}
				case TriggerType.Sphere:
					m_boundingSphere.Center = base.Entity.PositionComp.GetPosition() + vector;
					break;
				case TriggerType.OBB:
				{
					MatrixD matrixD = MatrixD.CreateTranslation(m_relativeOffset);
					matrixD.Right *= m_orientedBoundingBox.HalfExtent.X * 2.0;
					matrixD.Up *= m_orientedBoundingBox.HalfExtent.Y * 2.0;
					matrixD.Forward *= m_orientedBoundingBox.HalfExtent.Z * 2.0;
					m_orientedBoundingBox = new MyOrientedBoundingBoxD(matrixD * base.Entity.PositionComp.WorldMatrixRef);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void Update()
		{
			if ((long)MySession.Static.GameplayFrameCounter % (long)UpdateFrequency == m_updateOffset || !m_updatedOnce)
			{
				m_updatedOnce = true;
				UpdateInternal();
			}
		}

		/// <summary>
		/// Override this function to set custom update behaviour.
		/// Call base at first because it queries objects if DoQuery is set.
		/// </summary>
		protected virtual void UpdateInternal()
		{
			if (!DoQuery)
			{
				return;
			}
			m_queryResult.Clear();
			switch (m_triggerType)
			{
			case TriggerType.AABB:
				MyGamePruningStructure.GetTopMostEntitiesInBox(ref m_AABB, m_queryResult);
				break;
			case TriggerType.Sphere:
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref m_boundingSphere, m_queryResult);
				break;
			case TriggerType.OBB:
				MyGamePruningStructure.GetAllEntitiesInOBB(ref m_orientedBoundingBox, m_queryResult);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			int num = 0;
			while (num < m_queryResult.Count)
			{
				MyEntity entity = m_queryResult[num];
				if (!QueryEvaluator(entity))
				{
					m_queryResult.RemoveAtFast(num);
					continue;
				}
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = MyOrientedBoundingBoxD.CreateFromBoundingBox(m_queryResult[num].PositionComp.LocalAABB);
				myOrientedBoundingBoxD.Transform(m_queryResult[num].PositionComp.WorldMatrixRef);
				switch (m_triggerType)
				{
				case TriggerType.AABB:
					if (!myOrientedBoundingBoxD.Intersects(ref m_AABB))
					{
						m_queryResult.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
					break;
				case TriggerType.Sphere:
					if (!myOrientedBoundingBoxD.Intersects(ref m_boundingSphere))
					{
						m_queryResult.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
					break;
				case TriggerType.OBB:
					if (!myOrientedBoundingBoxD.Intersects(ref m_orientedBoundingBox))
					{
						m_queryResult.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
					break;
				default:
					num++;
					break;
				}
			}
		}

		/// <summary>
		/// Override for custom trigger disposal before removing.
		/// </summary>
		public virtual void Dispose()
		{
			m_queryResult.Clear();
		}

		public virtual void DebugDraw()
		{
			Color color = Color.Red;
			if (CustomDebugColor.HasValue)
			{
				color = CustomDebugColor.Value;
			}
			switch (m_triggerType)
			{
			case TriggerType.AABB:
				MyRenderProxy.DebugDrawAABB(m_AABB, (m_queryResult.Count == 0) ? color : Color.Green, 1f, 1f, depthRead: false);
				break;
			case TriggerType.Sphere:
				MyRenderProxy.DebugDrawSphere(m_boundingSphere.Center, (float)m_boundingSphere.Radius, (m_queryResult.Count == 0) ? color : Color.Green, 1f, depthRead: false);
				break;
			case TriggerType.OBB:
				MyRenderProxy.DebugDrawOBB(m_orientedBoundingBox, (m_queryResult.Count == 0) ? color : Color.Green, 1f, depthRead: false, smooth: false);
				break;
			}
			if (base.Entity != null)
			{
				MyRenderProxy.DebugDrawLine3D(Center, base.Entity.PositionComp.GetPosition(), Color.Yellow, Color.Green, depthRead: false);
			}
			foreach (MyEntity item in m_queryResult)
			{
				MyOrientedBoundingBoxD obb = MyOrientedBoundingBoxD.CreateFromBoundingBox(item.PositionComp.LocalAABB);
				obb.Transform(item.PositionComp.WorldMatrixRef);
				MyRenderProxy.DebugDrawOBB(obb, Color.Yellow, 1f, depthRead: false, smooth: false);
				MyRenderProxy.DebugDrawLine3D(item.WorldMatrix.Translation, base.Entity.WorldMatrix.Translation, Color.Yellow, Color.Green, depthRead: false);
			}
		}

		/// <summary>
		/// Override to discard query results of your choice.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>True for valid entities.</returns>
		protected virtual bool QueryEvaluator(MyEntity entity)
		{
			return true;
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public bool Contains(Vector3D point)
		{
			return m_triggerType switch
			{
<<<<<<< HEAD
			case TriggerType.AABB:
				return m_AABB.Contains(point) == ContainmentType.Contains;
			case TriggerType.Sphere:
				return m_boundingSphere.Contains(point) == ContainmentType.Contains;
			case TriggerType.OBB:
				return m_orientedBoundingBox.Contains(ref point);
			default:
				return false;
			}
=======
				TriggerType.AABB => m_AABB.Contains(point) == ContainmentType.Contains, 
				TriggerType.Sphere => m_boundingSphere.Contains(point) == ContainmentType.Contains, 
				TriggerType.OBB => m_orientedBoundingBox.Contains(ref point), 
				_ => false, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
