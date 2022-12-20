<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Havok;
using Sandbox.Game.World;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.Mesh;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	internal sealed class MyPrecalcJobPhysicsBatch : MyPrecalcJob
	{
		private class Sandbox_Engine_Voxels_MyPrecalcJobPhysicsBatch_003C_003EActor : IActivator, IActivator<MyPrecalcJobPhysicsBatch>
		{
			private sealed override object CreateInstance()
			{
				return new MyPrecalcJobPhysicsBatch();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPrecalcJobPhysicsBatch CreateInstance()
			{
				return new MyPrecalcJobPhysicsBatch();
			}

			MyPrecalcJobPhysicsBatch IActivator<MyPrecalcJobPhysicsBatch>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyDynamicObjectPool<MyPrecalcJobPhysicsBatch> m_instancePool = new MyDynamicObjectPool<MyPrecalcJobPhysicsBatch>(8);

		private MyVoxelPhysicsBody m_targetPhysics;

		internal HashSet<Vector3I> CellBatch = new HashSet<Vector3I>((IEqualityComparer<Vector3I>)Vector3I.Comparer);

		private Dictionary<Vector3I, HkBvCompressedMeshShape> m_newShapes = new Dictionary<Vector3I, HkBvCompressedMeshShape>(Vector3I.Comparer);

		private volatile bool m_isCancelled;

		public int Lod;

		public MyPrecalcJobPhysicsBatch()
			: base(enableCompletionCallback: true)
		{
		}

		public static void Start(MyVoxelPhysicsBody targetPhysics, ref HashSet<Vector3I> cellBatchForSwap, int lod)
		{
			MyPrecalcJobPhysicsBatch myPrecalcJobPhysicsBatch = m_instancePool.Allocate();
			myPrecalcJobPhysicsBatch.Lod = lod;
			myPrecalcJobPhysicsBatch.m_targetPhysics = targetPhysics;
			MyUtils.Swap(ref myPrecalcJobPhysicsBatch.CellBatch, ref cellBatchForSwap);
			targetPhysics.RunningBatchTask[lod] = myPrecalcJobPhysicsBatch;
			MyPrecalcComponent.EnqueueBack(myPrecalcJobPhysicsBatch);
		}

		public override void DoWork()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Enumerator<Vector3I> enumerator = CellBatch.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Vector3I current = enumerator.get_Current();
						if (m_isCancelled)
						{
							break;
						}
						VrVoxelMesh vrVoxelMesh = m_targetPhysics.CreateMesh(new MyCellCoord(Lod, current));
						try
						{
<<<<<<< HEAD
							m_newShapes.Add(item, (HkBvCompressedMeshShape)HkShape.Empty);
							continue;
						}
						HkBvCompressedMeshShape value = m_targetPhysics.CreateShape(vrVoxelMesh, Lod);
						m_newShapes.Add(item, value);
=======
							if (m_isCancelled)
							{
								return;
							}
							if (vrVoxelMesh.IsEmpty())
							{
								m_newShapes.Add(current, (HkBvCompressedMeshShape)HkShape.Empty);
								continue;
							}
							HkBvCompressedMeshShape value = m_targetPhysics.CreateShape(vrVoxelMesh, Lod);
							m_newShapes.Add(current, value);
						}
						finally
						{
							vrVoxelMesh?.Dispose();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			finally
			{
			}
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			if (MySession.Static != null && MySession.Static.GetComponent<MyPrecalcComponent>().Loaded && !m_isCancelled)
			{
				m_targetPhysics.OnBatchTaskComplete(m_newShapes, Lod);
			}
			foreach (HkBvCompressedMeshShape value in m_newShapes.Values)
			{
				HkShape @base = value.Base;
				if (!@base.IsZero)
				{
					@base = value.Base;
					@base.RemoveReference();
				}
			}
			if (m_targetPhysics.RunningBatchTask[Lod] == this)
			{
				m_targetPhysics.RunningBatchTask[Lod] = null;
			}
			m_targetPhysics = null;
			CellBatch.Clear();
			m_newShapes.Clear();
			m_isCancelled = false;
			m_instancePool.Deallocate(this);
		}

		public override void Cancel()
		{
			m_isCancelled = true;
		}
	}
}
