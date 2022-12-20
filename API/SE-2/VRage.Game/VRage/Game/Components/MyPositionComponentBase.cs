using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;

namespace VRage.Game.Components
{
	public abstract class MyPositionComponentBase : MyEntityComponentBase
	{
		public static Action<IMyEntity> OnReportInvalidMatrix;

		protected MatrixD m_worldMatrix = MatrixD.Identity;

		public uint m_worldMatrixCounter;

		public uint m_lastParentWorldMatrixCounter;

		public bool m_worldMatrixDirty;

		/// Internal local matrix relative to parent of entity.
		protected Matrix m_localMatrix = Matrix.Identity;

		protected BoundingBox m_localAABB;

		protected BoundingSphere m_localVolume;

		protected Vector3 m_localVolumeOffset;

		protected BoundingBoxD m_worldAABB;

		protected BoundingSphereD m_worldVolume;

		protected bool m_worldVolumeDirty;

		protected bool m_worldAABBDirty;

		private float? m_scale;

		private MyPositionComponentBase m_parent;

		protected bool m_normalizedInvMatrixDirty = true;

		private MatrixD m_normalizedWorldMatrixInv;

		protected bool m_invScaledMatrixDirty = true;

		private MatrixD m_worldMatrixInvScaled;

		/// <summary>
		/// World matrix of this physic object. Use it whenever you want to do world-matrix transformations with this physic objects.
		/// </summary>
		public ref readonly MatrixD WorldMatrixRef
		{
			get
			{
				RecalculateWorldMatrixHRIfNeeded();
				return ref m_worldMatrix;
			}
		}

		[Obsolete("Deprecated, use WorldMatrixRef instead.")]
		public MatrixD WorldMatrix
		{
			get
			{
				RecalculateWorldMatrixHRIfNeeded();
				return m_worldMatrix;
			}
			set
			{
				SetWorldMatrix(ref value);
			}
		}

		/// <summary>
		/// Gets or sets the local matrix.
		/// </summary>
		/// <value>
		/// The local matrix.
		/// </value>
		public ref readonly Matrix LocalMatrixRef => ref m_localMatrix;

<<<<<<< HEAD
		[Obsolete("Deprecated, use LocalMatrixRef instead.")]
=======
		[Obsolete("Deprecated, use WorldMatrixRef instead.")]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Matrix LocalMatrix
		{
			get
			{
				return m_localMatrix;
			}
			set
			{
				SetLocalMatrix(ref value);
			}
		}

		/// <summary>
		/// Gets the world aabb.
		/// </summary>
		public BoundingBoxD WorldAABB
		{
			get
			{
				if (m_worldAABBDirty || RecalculateWorldMatrixHRIfNeeded())
				{
					m_localAABB.Transform(ref m_worldMatrix, ref m_worldAABB);
					m_worldAABBDirty = false;
				}
				return m_worldAABB;
			}
			set
			{
				m_worldAABB = value;
				Vector3 normal = value.Center - WorldMatrixRef.Translation;
				MatrixD matrix = WorldMatrixInvScaled;
				Vector3.TransformNormal(ref normal, ref matrix, out normal);
				LocalAABB = new BoundingBox(normal - (Vector3)value.HalfExtents, normal + (Vector3)value.HalfExtents);
				m_worldAABBDirty = false;
			}
		}

		/// <summary>
		/// Gets the world volume.
		/// </summary>
		public BoundingSphereD WorldVolume
		{
			get
			{
				if (m_worldVolumeDirty || RecalculateWorldMatrixHRIfNeeded())
				{
					m_worldVolume.Center = Vector3D.Transform(m_localVolume.Center, ref m_worldMatrix);
					m_worldVolume.Radius = m_localVolume.Radius;
					m_worldVolumeDirty = false;
				}
				return m_worldVolume;
			}
			set
			{
				m_worldVolume = value;
				Vector3 normal = value.Center - WorldMatrixRef.Translation;
				MatrixD matrix = WorldMatrixInvScaled;
				Vector3.TransformNormal(ref normal, ref matrix, out normal);
				LocalVolume = new BoundingSphere(normal, (float)value.Radius);
				m_worldVolumeDirty = false;
			}
		}

		/// <summary>
		/// Sets the local aabb.
		/// </summary>
		/// <value>
		/// The local aabb.
		/// </value>
		public virtual BoundingBox LocalAABB
		{
			get
			{
				return m_localAABB;
			}
			set
			{
				if (m_localAABB != value)
				{
					m_localAABB = value;
					m_localVolume = BoundingSphere.CreateFromBoundingBox(m_localAABB);
					m_worldVolumeDirty = true;
					m_worldAABBDirty = true;
					if (this.OnLocalAABBChanged != null)
					{
						this.OnLocalAABBChanged(this);
					}
				}
			}
		}

		/// <summary>
		/// Sets the local volume.
		/// </summary>
		/// <value>
		/// The local volume.
		/// </value>
		public BoundingSphere LocalVolume
		{
			get
			{
				return m_localVolume;
			}
			set
			{
				m_localVolume = value;
				m_localAABB = MyMath.CreateFromInsideRadius(value.Radius);
				m_localAABB = m_localAABB.Translate(value.Center);
				m_worldVolumeDirty = true;
				m_worldAABBDirty = true;
			}
		}

		/// <summary>
		/// Gets or sets the local volume offset.
		/// </summary>
		/// <value>
		/// The local volume offset.
		/// </value>
		public Vector3 LocalVolumeOffset
		{
			get
			{
				return m_localVolumeOffset;
			}
			set
			{
				m_localVolumeOffset = value;
				m_worldVolumeDirty = true;
			}
		}

		protected virtual bool ShouldSync => base.Container.Get<MySyncComponentBase>() != null;

		public float? Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				if (m_scale == value)
				{
					return;
				}
				m_scale = value;
				Matrix m = LocalMatrixRef;
				if (m_scale.HasValue)
				{
					MatrixD m2 = WorldMatrixRef;
					if (m_parent == null)
					{
						MyUtils.Normalize(ref m2, out m2);
						MatrixD worldMatrix = Matrix.CreateScale(m_scale.Value) * m2;
						SetWorldMatrix(ref worldMatrix);
					}
					else
					{
						MyUtils.Normalize(ref m, out m);
						Matrix localMatrix = Matrix.CreateScale(m_scale.Value) * m;
						SetLocalMatrix(ref localMatrix);
					}
				}
				else
				{
					MyUtils.Normalize(ref m, out m);
					SetLocalMatrix(ref m);
				}
				UpdateWorldMatrix();
			}
		}

		public bool NeedsRecalculateWorldMatrix
		{
			get
			{
				if (m_worldMatrixDirty)
				{
					return true;
				}
				if (base.Entity == null)
				{
					return false;
				}
				MyPositionComponentBase parent = m_parent;
				uint lastParentWorldMatrixCounter = m_lastParentWorldMatrixCounter;
				while (parent != null)
				{
					if (lastParentWorldMatrixCounter < parent.m_worldMatrixCounter)
					{
						return true;
					}
					lastParentWorldMatrixCounter = parent.m_lastParentWorldMatrixCounter;
					parent = parent.m_parent;
				}
				return false;
			}
		}

		public ref readonly MatrixD WorldMatrixNormalizedInv
		{
			get
			{
				if (m_normalizedInvMatrixDirty || RecalculateWorldMatrixHRIfNeeded())
				{
					MatrixD matrix = m_worldMatrix;
					if (!MyUtils.IsZero(matrix.Left.LengthSquared() - 1.0))
					{
						MatrixD matrix2 = MatrixD.Normalize(matrix);
						MatrixD.Invert(ref matrix2, out m_normalizedWorldMatrixInv);
					}
					else
					{
						MatrixD.Invert(ref matrix, out m_normalizedWorldMatrixInv);
					}
					m_normalizedInvMatrixDirty = false;
					if (!Scale.HasValue)
					{
						m_worldMatrixInvScaled = m_normalizedWorldMatrixInv;
						m_invScaledMatrixDirty = false;
					}
				}
				return ref m_normalizedWorldMatrixInv;
			}
		}

		public ref readonly MatrixD WorldMatrixInvScaled
		{
			get
			{
				if (m_invScaledMatrixDirty || RecalculateWorldMatrixHRIfNeeded())
				{
					MatrixD matrix = m_worldMatrix;
					if (!MyUtils.IsZero(matrix.Left.LengthSquared() - 1.0))
					{
						matrix = MatrixD.Normalize(matrix);
					}
					if (Scale.HasValue)
					{
						matrix *= Matrix.CreateScale(Scale.Value);
					}
					MatrixD.Invert(ref matrix, out m_worldMatrixInvScaled);
					m_invScaledMatrixDirty = false;
					if (!Scale.HasValue)
					{
						m_normalizedWorldMatrixInv = m_worldMatrixInvScaled;
						m_normalizedInvMatrixDirty = false;
					}
				}
				return ref m_worldMatrixInvScaled;
			}
		}

		public override string ComponentTypeDebugString => "Position";

		public event Action<MyPositionComponentBase> OnPositionChanged;

		public event Action<MyPositionComponentBase> OnLocalAABBChanged;

		protected void RaiseOnPositionChanged(MyPositionComponentBase component)
		{
			this.OnPositionChanged.InvokeIfNotNull(component);
		}

		[Obsolete("SetWorldMatrix(MatrixD,...) is deprecated, please use SetWorldMatrix(ref MatrixD,...) instead.")]
		public void SetWorldMatrix(MatrixD worldMatrix, object source = null, bool forceUpdate = false, bool updateChildren = true, bool updateLocal = true, bool skipTeleportCheck = false, bool forceUpdateAllChildren = false, bool ignoreAssert = false)
		{
			SetWorldMatrix(ref worldMatrix, source, forceUpdate, updateChildren, updateLocal, skipTeleportCheck, forceUpdateAllChildren, ignoreAssert);
		}

		/// <summary>
		/// Sets the world matrix.
		/// </summary>
		/// <param name="worldMatrix">The world matrix.</param>
		/// <param name="source">The source object that caused this change or null when not important.</param>
		/// <param name="forceUpdate"></param>
		/// <param name="updateChildren"></param>
		/// <param name="updateLocal"></param>
		/// <param name="skipTeleportCheck"></param>
		/// <param name="forceUpdateAllChildren"></param>
		/// <param name="ignoreAssert"></param>
		public void SetWorldMatrix(ref MatrixD worldMatrix, object source = null, bool forceUpdate = false, bool updateChildren = true, bool updateLocal = true, bool skipTeleportCheck = false, bool forceUpdateAllChildren = false, bool ignoreAssert = false)
		{
			if (OnReportInvalidMatrix != null && !worldMatrix.IsValid())
			{
				OnReportInvalidMatrix(base.Entity);
			}
			else if (!skipTeleportCheck && base.Entity.InScene && Vector3D.DistanceSquared(worldMatrix.Translation, WorldMatrixRef.Translation) > (double)MyClusterTree.IdealClusterSizeHalfSqr.X)
			{
				base.Entity.Teleport(worldMatrix, source, ignoreAssert);
			}
			else
			{
				if (base.Entity.Parent != null && source != base.Entity.Parent)
				{
					return;
				}
				if (Scale.HasValue)
				{
					MyUtils.Normalize(ref worldMatrix, out worldMatrix);
					worldMatrix = MatrixD.CreateScale(Scale.Value) * worldMatrix;
				}
				if (forceUpdate || !m_worldMatrix.EqualsFast(ref worldMatrix, 1E-06))
				{
					if (m_parent == null)
					{
						m_worldMatrix = worldMatrix;
						m_localMatrix.SetFrom(in worldMatrix);
					}
					else if (updateLocal)
					{
						MatrixD worldMatrixInvScaled = m_parent.WorldMatrixInvScaled;
						ref Matrix localMatrix = ref m_localMatrix;
						MatrixD m = worldMatrix * worldMatrixInvScaled;
						localMatrix.SetFrom(in m);
					}
					m_worldMatrixCounter++;
					m_worldMatrixDirty = false;
					UpdateWorldMatrix(source, updateChildren, forceUpdateAllChildren);
				}
			}
		}

		/// <summary>
		/// Recursively recalculate world matrices for this entity and it's parents.
		/// </summary>
		/// <param name="updateChildren"></param>
		/// <returns>True if the world matrix has changed.</returns>
		protected bool RecalculateWorldMatrixHRIfNeeded(bool updateChildren = false)
		{
			if (m_parent == null)
			{
				return false;
<<<<<<< HEAD
			}
			m_parent.RecalculateWorldMatrixHRIfNeeded();
			if (m_lastParentWorldMatrixCounter == m_parent.m_worldMatrixCounter && !m_worldMatrixDirty)
			{
				return false;
			}
=======
			}
			m_parent.RecalculateWorldMatrixHRIfNeeded();
			if (m_lastParentWorldMatrixCounter == m_parent.m_worldMatrixCounter && !m_worldMatrixDirty)
			{
				return false;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_lastParentWorldMatrixCounter = m_parent.m_worldMatrixCounter;
			MatrixD other = m_worldMatrix;
			MatrixD.Multiply(ref m_localMatrix, ref m_parent.m_worldMatrix, out m_worldMatrix);
			m_worldMatrixDirty = false;
			if (m_worldMatrix.EqualsFast(ref other))
			{
				return false;
			}
			m_worldMatrixCounter++;
			m_worldVolumeDirty = true;
			m_worldAABBDirty = true;
			m_normalizedInvMatrixDirty = true;
			m_invScaledMatrixDirty = true;
			return true;
		}

		public void SetLocalMatrix(ref Matrix localMatrix, object source, bool updateWorld, ref Matrix renderLocal, bool forceUpdateRender = false)
		{
			if (SetLocalMatrix(ref localMatrix, source, updateWorld) || forceUpdateRender)
			{
				base.Entity.Render?.UpdateRenderObjectLocal(renderLocal);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="localMatrix"></param>
		/// <param name="source"></param>
		/// <param name="updateWorld"></param>
<<<<<<< HEAD
		/// <returns> true when World matrix needed recalculations as it got changed in here</returns>
=======
		/// <returns> true when World matrix needed recalcualtions as it got changed in here</returns>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool SetLocalMatrix(ref Matrix localMatrix, object source = null, bool updateWorld = true)
		{
			bool num = !m_localMatrix.EqualsFast(ref localMatrix);
			if (num)
			{
				m_localMatrix = localMatrix;
				m_worldMatrixCounter++;
				m_worldMatrixDirty = true;
			}
			if (NeedsRecalculateWorldMatrix && updateWorld)
			{
				UpdateWorldMatrix(source);
			}
			return num;
		}

		/// <summary>
		/// Gets the entity position.
		/// </summary>
		/// <returns></returns>
		public Vector3D GetPosition()
		{
			RecalculateWorldMatrixHRIfNeeded();
			return m_worldMatrix.Translation;
		}

		/// <summary>
		/// Sets the position.
		/// </summary>
		/// <param name="pos">The position</param>
		/// <param name="source"></param>
		/// <param name="forceUpdate"></param>
		/// <param name="updateChildren"></param>
		public void SetPosition(Vector3D pos, object source = null, bool forceUpdate = false, bool updateChildren = true)
		{
			if (!MyUtils.IsZero(m_worldMatrix.Translation - pos))
			{
				MatrixD worldMatrix = m_worldMatrix;
				worldMatrix.Translation = pos;
				SetWorldMatrix(ref worldMatrix, source, forceUpdate, updateChildren);
			}
		}

		/// <summary>
		/// Gets the entity orientation.
		/// </summary>
		/// <returns></returns>
		public MatrixD GetOrientation()
		{
			RecalculateWorldMatrixHRIfNeeded();
			return m_worldMatrix.GetOrientation();
		}

		public virtual MatrixD GetViewMatrix()
		{
			return WorldMatrixNormalizedInv;
		}

		/// <summary>
		/// Updates the world matrix (change caused by this entity)
		/// </summary>
		protected virtual void UpdateWorldMatrix(object source = null, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			if (m_parent != null)
			{
				MatrixD parentWorldMatrix = m_parent.WorldMatrixRef;
				UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren, forceUpdateAllChildren);
			}
			else
			{
				OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}
		}

		/// <summary>
		/// Updates the world matrix (change caused by parent)
		/// </summary>
		public virtual void UpdateWorldMatrix(ref MatrixD parentWorldMatrix, object source = null, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			if (m_parent != null)
			{
				MatrixD.Multiply(ref m_localMatrix, ref parentWorldMatrix, out m_worldMatrix);
				m_lastParentWorldMatrixCounter = m_parent.m_worldMatrixCounter;
				m_worldMatrixCounter++;
				m_worldMatrixDirty = false;
				OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}
		}

		/// <summary>
		/// Called when [world position changed].
		/// </summary>
		/// <param name="source">The source object that caused this event.</param>
		/// <param name="updateChildren"></param>
		/// <param name="forceUpdateAllChildren"></param>
		protected virtual void OnWorldPositionChanged(object source, bool updateChildren = true, bool forceUpdateAllChildren = false)
		{
			m_worldVolumeDirty = true;
			m_worldAABBDirty = true;
			m_normalizedInvMatrixDirty = true;
			m_invScaledMatrixDirty = true;
			RaiseOnPositionChanged(this);
		}

		/// <inheritdoc />
		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			base.Container.ComponentAdded += ContainerOnComponentAdded;
			base.Container.ComponentRemoved += ContainerOnComponentRemoved;
			HookHierarchy(base.Entity.Hierarchy);
		}

		/// <inheritdoc />
		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			base.Container.ComponentAdded -= ContainerOnComponentAdded;
			base.Container.ComponentRemoved -= ContainerOnComponentRemoved;
			UnHookHierarchy(base.Entity.Hierarchy);
		}

		private void ContainerOnComponentAdded(Type arg1, MyEntityComponentBase arg2)
		{
			MyHierarchyComponentBase hierarchy;
			if ((hierarchy = arg2 as MyHierarchyComponentBase) != null)
			{
				HookHierarchy(hierarchy);
			}
		}

		private void ContainerOnComponentRemoved(Type arg1, MyEntityComponentBase arg2)
		{
			MyHierarchyComponentBase hierarchy;
			if ((hierarchy = arg2 as MyHierarchyComponentBase) != null)
			{
				UnHookHierarchy(hierarchy);
			}
		}

		private void HookHierarchy(MyHierarchyComponentBase hierarchy)
		{
			if (hierarchy != null)
			{
				hierarchy.OnParentChanged += Hierarchy_OnParentChanged;
				m_parent = base.Entity.Parent?.PositionComp;
			}
		}

		private void UnHookHierarchy(MyHierarchyComponentBase hierarchy)
		{
			if (hierarchy != null)
			{
				hierarchy.OnParentChanged -= Hierarchy_OnParentChanged;
				m_parent = null;
			}
		}

		private void Hierarchy_OnParentChanged(MyHierarchyComponentBase arg1, MyHierarchyComponentBase arg2)
		{
			m_parent = arg2?.Entity?.PositionComp;
			m_lastParentWorldMatrixCounter = 0u;
		}

		public override string ToString()
		{
			return string.Concat("worldpos=", GetPosition(), ", worldmat=", WorldMatrixRef);
		}
	}
}
