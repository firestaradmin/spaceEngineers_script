<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using VRage.Network;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Render11.GeometryStage.Voxel
{
	/// <summary>
	/// Actor used to store display voxel mesh data.
	/// </summary>
	internal class MyRenderVoxelActor : MyActorComponent, IMyVoxelActor
	{
		private class VRage_Render11_GeometryStage_Voxel_MyRenderVoxelActor_003C_003EActor : IActivator, IActivator<MyRenderVoxelActor>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderVoxelActor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderVoxelActor CreateInstance()
			{
				return new MyRenderVoxelActor();
			}

			MyRenderVoxelActor IActivator<MyRenderVoxelActor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private IMyLodController m_clipmap;

		private Vector3D m_spherizeCenter;

		private float? m_spherizeRadius;

		private MyRenderableProxyFlags m_additionalFlags;

		private readonly HashSet<MyVoxelCellComponent> m_parts = new HashSet<MyVoxelCellComponent>();

		private readonly HashSet<MyVoxelCellComponent> m_partsForExplicitUpdate = new HashSet<MyVoxelCellComponent>();

		private MyVoxelActorTransitionMode m_mode;

		private bool m_forceImmediate;

		private float m_dithering;

		private readonly Dictionary<MyVoxelCellComponent, bool> m_batchedCellTransitions = new Dictionary<MyVoxelCellComponent, bool>();

		private readonly HashSet<MyVoxelCellComponent> m_bacthedCellUpdates = new HashSet<MyVoxelCellComponent>();

		private static bool m_firstFrame = true;

		private bool m_fadeOut;

		public IMyLodController Clipmap => m_clipmap;

		public bool Spherize => m_spherizeRadius.HasValue;

		public float? SpherizeRadius
		{
			get
			{
				return m_spherizeRadius;
			}
			set
			{
				m_spherizeRadius = value;
				UpdateActors();
			}
		}

		public Vector3D SpherizeCenter
		{
			get
			{
				return m_spherizeCenter;
			}
			set
			{
				m_spherizeCenter = value;
				UpdateActors();
			}
		}

		public Vector3D SpherizeCenterWorld => Vector3D.Transform(m_spherizeCenter, base.Owner.WorldMatrix);

		public MyRenderableProxyFlags RenderFlags
		{
			get
			{
				return m_additionalFlags;
			}
			set
			{
				m_additionalFlags = value;
				UpdateActors();
			}
		}

		public uint Id => base.Owner.ID;

		public Vector3I Size { get; set; }

		public MyVoxelActorTransitionMode TransitionMode
		{
			get
			{
				if (!m_forceImmediate)
				{
					return m_mode;
				}
				return MyVoxelActorTransitionMode.Immediate;
			}
			set
			{
				if (IsBatching)
				{
					MyLog.Default.Error("Changing batch mode while batching is not allowed. Current batch mode: {0}; to set: {1}", m_mode, value);
				}
				else
				{
					m_mode = value;
				}
			}
		}

		public bool IsBatching { get; private set; }

		public event VisibilityChange CellChange;

		public event ActionRef<MatrixD> Move;

		public void Init(Vector3I size, IMyLodController clipmap)
		{
			Size = size;
			m_clipmap = clipmap;
			m_clipmap.BindToActor(this);
		}

		public override void Assign(MyActor owner)
		{
			base.Assign(owner);
			owner.OnMove += OnMatrixChange;
		}

		public override void OnRemove(MyActor owner)
		{
			base.Owner.OnMove -= OnMatrixChange;
			base.OnRemove(owner);
		}

		public override void Destruct()
		{
			base.Destruct();
<<<<<<< HEAD
			while (m_partsForExplicitUpdate.Count > 0)
			{
				m_partsForExplicitUpdate.FirstElement().Owner.Destroy();
			}
			while (m_parts.Count > 0)
			{
				m_parts.FirstElement().Owner.Destroy();
=======
			while (m_partsForExplicitUpdate.get_Count() > 0)
			{
				m_partsForExplicitUpdate.FirstElement<MyVoxelCellComponent>().Owner.Destroy();
			}
			while (m_parts.get_Count() > 0)
			{
				m_parts.FirstElement<MyVoxelCellComponent>().Owner.Destroy();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_clipmap.Unload();
			m_clipmap = null;
			m_spherizeCenter = Vector3D.Zero;
			m_spherizeRadius = null;
			this.Move = null;
			this.CellChange = null;
			IsBatching = false;
			m_bacthedCellUpdates.Clear();
			m_batchedCellTransitions.Clear();
			m_forceImmediate = false;
			m_fadeOut = false;
		}

		private void OnMatrixChange()
		{
			UpdateActors();
			if (this.Move != null)
			{
				MatrixD item = base.Owner.WorldMatrix;
				this.Move(ref item);
			}
		}

		public IMyVoxelActorCell CreateCell(Vector3D offset, int lod, bool notify)
		{
			MyVoxelCellComponent myVoxelCellComponent = MyComponentFactory<MyVoxelCellComponent>.Create();
			MyActor myActor = MyActorFactory.Create($"Clipmap Cell {offset}:{lod}");
			myActor.AddComponent<MyRenderableComponent>(myVoxelCellComponent);
			myVoxelCellComponent.SetAlpha(m_dithering);
			myVoxelCellComponent.Prepare(this, offset, lod);
			myActor.SetVisibility(visibility: false);
			myVoxelCellComponent.SetNotify(notify);
			m_parts.Add(myVoxelCellComponent);
			return myVoxelCellComponent;
		}

		public void DeleteCell(IMyVoxelActorCell cell, bool notify = false)
		{
			MyVoxelCellComponent myVoxelCellComponent = (MyVoxelCellComponent)cell;
			m_parts.Remove(myVoxelCellComponent);
			myVoxelCellComponent.SetVisible(visible: false);
		}

		public void BeginBatch(MyVoxelActorTransitionMode? switchMode)
		{
			if (switchMode.HasValue && m_dithering == 0f)
			{
				TransitionMode = switchMode.Value;
			}
			IsBatching = true;
		}

		public void EndBatch(bool justLoaded)
		{
<<<<<<< HEAD
=======
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			IsBatching = false;
			MyVoxelActorTransitionMode transitionMode = TransitionMode;
			TransitionMode = ((!justLoaded) ? transitionMode : MyVoxelActorTransitionMode.Immediate);
			foreach (KeyValuePair<MyVoxelCellComponent, bool> batchedCellTransition in m_batchedCellTransitions)
			{
				batchedCellTransition.Key.UpdateVisibilityInternal(batchedCellTransition.Value);
			}
			TransitionMode = transitionMode;
<<<<<<< HEAD
			foreach (MyVoxelCellComponent bacthedCellUpdate in m_bacthedCellUpdates)
			{
				bacthedCellUpdate.MarkDirty();
=======
			Enumerator<MyVoxelCellComponent> enumerator2 = m_bacthedCellUpdates.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().MarkDirty();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_bacthedCellUpdates.Clear();
			m_batchedCellTransitions.Clear();
			m_forceImmediate = false;
		}

		public override bool StartFadeOut()
		{
<<<<<<< HEAD
			m_fadeOut = true;
			foreach (MyVoxelCellComponent part in m_parts)
			{
				part.StartFadeOut();
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			m_fadeOut = true;
			Enumerator<MyVoxelCellComponent> enumerator = m_parts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().StartFadeOut();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

		private void UpdateActors()
		{
<<<<<<< HEAD
			foreach (MyVoxelCellComponent part in m_parts)
			{
				part.UpdateParametersFromEntity();
			}
			foreach (MyVoxelCellComponent item in m_partsForExplicitUpdate)
			{
				item.UpdateParametersFromEntity();
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyVoxelCellComponent> enumerator = m_parts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateParametersFromEntity();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			enumerator = m_partsForExplicitUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateParametersFromEntity();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal void NotifyChildClose(MyVoxelCellComponent cell)
		{
			m_parts.Remove(cell);
			m_partsForExplicitUpdate.Remove(cell);
			if (IsBatching)
			{
				m_bacthedCellUpdates.Remove(cell);
				m_batchedCellTransitions.Remove(cell);
			}
		}

		internal void NotifyTransition(int lod, Vector3D offset, bool visible)
		{
			if (this.CellChange != null)
			{
				this.CellChange(offset, lod, visible);
			}
		}

		internal void QueueCellVisibilityChange(MyVoxelCellComponent cell, bool visible)
		{
			m_batchedCellTransitions[cell] = visible;
			RegisterForExplicitUpdate(cell);
		}

		internal void QueueCellDataUpdate(MyVoxelCellComponent cell)
		{
			m_bacthedCellUpdates.Add(cell);
		}

		/// <summary>
		/// Whether a cell is contained in this actor.
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public bool Contains(MyVoxelCellComponent cell)
		{
			return m_parts.Contains(cell);
		}

		public void RegisterForExplicitUpdate(MyVoxelCellComponent cell)
		{
			if (!m_parts.Contains(cell))
			{
				m_partsForExplicitUpdate.Add(cell);
			}
		}

		public void RemoveFromExplicitUpdate(MyVoxelCellComponent cell)
		{
			m_partsForExplicitUpdate.Remove(cell);
		}

		internal static void UpdateQueued()
		{
			MyEnvironmentMatrices matrices = MyRender11.Environment.Matrices;
			if (matrices.ViewAt0.Scale == Vector3.Zero)
			{
				m_firstFrame = true;
				return;
			}
			foreach (MyRenderVoxelActor item in MyComponentFactory<MyRenderVoxelActor>.GetAll())
			{
				if (m_firstFrame)
				{
					m_firstFrame = false;
					item.m_forceImmediate = true;
				}
				MatrixD view = matrices.InvViewD;
				if (MyClipmap.EnableUpdate && !item.m_fadeOut)
				{
					item.Clipmap.Update(ref view, matrices.ViewFrustumClippedD, matrices.FarClipping, matrices.LastUpdateWasSmooth);
				}
				if (MyClipmap.EnableDebugDraw)
				{
					item.Clipmap.DebugDraw(ref view);
				}
			}
		}

		public void SetDithering(float dithering)
		{
<<<<<<< HEAD
			m_dithering = dithering;
			TransitionMode = ((dithering == 0f) ? MyVoxelActorTransitionMode.Fade : MyVoxelActorTransitionMode.Immediate);
			foreach (MyVoxelCellComponent part in m_parts)
			{
				part.SetAlpha(m_dithering);
=======
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			m_dithering = dithering;
			TransitionMode = ((dithering == 0f) ? MyVoxelActorTransitionMode.Fade : MyVoxelActorTransitionMode.Immediate);
			Enumerator<MyVoxelCellComponent> enumerator = m_parts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().SetAlpha(m_dithering);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
