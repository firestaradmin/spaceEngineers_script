using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MyShipMiningSystem : MyUpdateableGridSystem
	{
		private class DrillCluster
		{
<<<<<<< HEAD
			/// <summary>
			/// Drills in this cluster.
			/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			private readonly HashSet<int> m_drills = new HashSet<int>();

			private BoundingBox m_bounds = BoundingBox.CreateInvalid();

			private BoundingBox m_expandedBounds = BoundingBox.CreateInvalid();

			private bool m_dirtyBounds;

			private int m_removalsSinceLastRebuild;

<<<<<<< HEAD
			/// <summary>
			/// Whether this cluster was explicitly marked for reconstruction.
			/// </summary>
			public bool NeedsReconstruction;

			/// <summary>
			/// Bounds of this cluster.
			/// </summary>
			public BoundingBox Bounds => m_bounds;

			/// <summary>
			/// Bounds of this cluster for discarding cut-outs.
			/// </summary>
			public BoundingBox ExpandedBounds => m_expandedBounds;

			/// <summary>
			/// Whether this cluster has been updated too many times and should be reconstructed.
			/// </summary>
=======
			public bool NeedsReconstruction;

			public BoundingBox Bounds => m_bounds;

			public BoundingBox ExpandedBounds => m_expandedBounds;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public bool RemoveThresholdReached
			{
				get
				{
					if (m_removalsSinceLastRebuild < 10)
					{
<<<<<<< HEAD
						return m_removalsSinceLastRebuild >= m_drills.Count;
=======
						return m_removalsSinceLastRebuild >= m_drills.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					return true;
				}
			}

<<<<<<< HEAD
			/// <summary>
			/// Whether this cluster is empty.
			/// </summary>
			public bool IsEmpty => m_drills.Count == 0;

			/// <summary>
			/// Drills in this cluster.
			/// </summary>
=======
			public bool IsEmpty => m_drills.get_Count() == 0;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public HashSetReader<int> Drills => m_drills;

			public void AddDrill(int drillIndex, in DrillData data)
			{
				m_drills.Add(drillIndex);
				BoundingSphere bondingSphere = data.BondingSphere;
				m_bounds.Include(bondingSphere);
				bondingSphere.Radius *= 3f;
				m_expandedBounds.Include(bondingSphere);
			}

			public void RemoveDrill(int drillIndex)
			{
				m_drills.Remove(drillIndex);
				m_dirtyBounds = true;
				m_removalsSinceLastRebuild++;
			}

			public void UpdateBounds(MyShipMiningSystem system)
			{
<<<<<<< HEAD
=======
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_dirtyBounds)
				{
					return;
				}
				m_dirtyBounds = false;
				m_bounds = BoundingBox.CreateInvalid();
				m_expandedBounds = BoundingBox.CreateInvalid();
<<<<<<< HEAD
				foreach (int drill in m_drills)
				{
					BoundingSphere bondingSphere = system.GetDrillData(drill).BondingSphere;
					m_bounds.Include(bondingSphere);
					bondingSphere.Radius *= 3f;
					m_expandedBounds.Include(bondingSphere);
=======
				Enumerator<int> enumerator = m_drills.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						BoundingSphere bondingSphere = system.GetDrillData(current).BondingSphere;
						m_bounds.Include(bondingSphere);
						bondingSphere.Radius *= 3f;
						m_expandedBounds.Include(bondingSphere);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}

			public void DebugDraw(MyShipMiningSystem system, MatrixD gridMatrix)
			{
<<<<<<< HEAD
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(Bounds, gridMatrix), Color.Orange, 0.7f, depthRead: true, smooth: false);
				foreach (int drill in m_drills)
				{
					ref readonly DrillData drillData = ref system.GetDrillData(drill);
					Vector3 position = drillData.BondingSphere.Center;
					Vector3D.Transform(ref position, ref gridMatrix, out var result);
					MyRenderProxy.DebugDrawArrow3D(drillData.Drill.PositionComp.GetPosition(), result, Color.Green);
					MyRenderProxy.DebugDrawSphere(result, drillData.BondingSphere.Radius, Color.Green);
=======
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(Bounds, gridMatrix), Color.Orange, 0.7f, depthRead: true, smooth: false);
				Enumerator<int> enumerator = m_drills.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						ref readonly DrillData drillData = ref system.GetDrillData(current);
						Vector3 position = drillData.BondingSphere.Center;
						Vector3D.Transform(ref position, ref gridMatrix, out var result);
						MyRenderProxy.DebugDrawArrow3D(drillData.Drill.PositionComp.GetPosition(), result, Color.Green);
						MyRenderProxy.DebugDrawSphere(result, drillData.BondingSphere.Radius, Color.Green);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Information about a cut-out request.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[DebuggerDisplay("{DebugText}")]
		private readonly struct CutOutRequest
		{
			private sealed class CutOutDataClusteringComparer : IComparer<CutOutRequest>
			{
				public int Compare(CutOutRequest x, CutOutRequest y)
				{
					int num = x.Cluster.GetHashCode().CompareTo(y.Cluster.GetHashCode());
					if (num == 0)
					{
						return x.Voxel.GetHashCode().CompareTo(y.Voxel.GetHashCode());
					}
					return num;
				}
			}

			public readonly MyShipDrill Drill;

			public readonly bool ApplyDiscardingMultiplier;

			public readonly bool ApplyDamagedMaterial;

			public readonly Vector3D HitPosition;

			public readonly MyVoxelBase Voxel;

			internal readonly DrillCluster Cluster;

			public static IComparer<CutOutRequest> CutOutDataComparer { get; } = new CutOutDataClusteringComparer();


			private string DebugText => string.Format("{0} vs {1}{2}{3}", Drill, Voxel.DebugName, ApplyDiscardingMultiplier ? " Discarding" : "", ApplyDamagedMaterial ? " ApplyDamage" : "");

			public CutOutRequest(MyShipDrill drill, DrillCluster cluster, bool applyDiscardingMultiplier, bool applyDamagedMaterial, Vector3D hitPosition, MyVoxelBase voxel)
			{
				Drill = drill;
				ApplyDiscardingMultiplier = applyDiscardingMultiplier;
				ApplyDamagedMaterial = applyDamagedMaterial;
				HitPosition = hitPosition;
				Voxel = voxel;
				Cluster = cluster;
			}
		}

		private class ClusterCutOut : IWork
		{
<<<<<<< HEAD
			/// <summary>
			/// Data about each drill's cutout.
			/// </summary>
			[DebuggerDisplay("{DebugText}")]
			private struct CutOutData
			{
				/// <summary>
				/// Bounding sphere of the drill already in voxel space.
				/// </summary>
				/// <remarks>When initially allocated this sphere is still in ship space, it is transformed in the first pass of the async cutout task.</remarks>
				public BoundingSphere SphereInVoxelSpace;

				/// <summary>
				/// Bounds of the cutout in voxel space.
				/// </summary>
				public BoundingBoxI StorageBounds;

				/// <summary>
				/// Whether this cutout should apply damage materials to the remaining voxel.
				/// </summary>
				public readonly bool ApplyDamagedMaterial;

				/// <summary>
				/// Whether the extracted results should be discarded.
				/// </summary>
				public readonly bool DiscardResults;

				/// <summary>
				/// The hit position;
				/// </summary>
				public Vector3D HitPosition;

				/// <summary>
				/// Whether this cutout caused any change on the target.
				/// </summary>
				public bool CausedChange;

				/// <summary>
				/// the drill requesting this cutout.
				/// </summary>
				public readonly MyShipDrill Drill;

				/// <summary>
				/// Per material counts of the cut-out amounts.
				/// </summary>
=======
			[DebuggerDisplay("{DebugText}")]
			private struct CutOutData
			{
				public BoundingSphere SphereInVoxelSpace;

				public BoundingBoxI StorageBounds;

				public readonly bool ApplyDamagedMaterial;

				public readonly bool DiscardResults;

				public Vector3D HitPosition;

				public bool CausedChange;

				public readonly MyShipDrill Drill;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				public readonly int[] CutOutMaterials;

				private string DebugText => string.Format("{0}{1}", SphereInVoxelSpace, ApplyDamagedMaterial ? " ApplyDamage" : "");

				public CutOutData(in CutOutRequest request, in DrillData data)
				{
					SphereInVoxelSpace = data.BondingSphere;
					if (request.ApplyDiscardingMultiplier)
					{
						SphereInVoxelSpace.Radius *= 3f;
					}
					StorageBounds = default(BoundingBoxI);
					ApplyDamagedMaterial = request.ApplyDamagedMaterial;
					DiscardResults = request.ApplyDiscardingMultiplier;
					Drill = request.Drill;
					CutOutMaterials = new int[MyDefinitionManager.Static.VoxelMaterialCount];
					CausedChange = false;
					HitPosition = request.HitPosition;
				}

				public CutOutData(in NetworkCutoutData.CutOut cutout, Vector3I storageOffset)
				{
					SphereInVoxelSpace = cutout.Sphere;
					StorageBounds = new BoundingBoxI(Vector3I.Floor(SphereInVoxelSpace.Center - SphereInVoxelSpace.Radius) + storageOffset - 1, Vector3I.Ceiling(SphereInVoxelSpace.Center + SphereInVoxelSpace.Radius) + storageOffset + 1);
					Drill = null;
					CutOutMaterials = null;
					CausedChange = false;
					ApplyDamagedMaterial = cutout.ApplyDamagedMaterial;
					DiscardResults = true;
					HitPosition = default(Vector3D);
				}
			}

			private class DrillChunkComparer : IComparer<(Vector3I Chunk, int Drill)>
			{
				public int Compare((Vector3I Chunk, int Drill) x, (Vector3I Chunk, int Drill) y)
				{
					return x.Chunk.CompareTo(y.Chunk);
				}
			}

			private enum State : byte
			{
				Pooled,
				Idle,
				Queued,
				Preparing,
				Executing,
				Finished
			}

<<<<<<< HEAD
			/// <summary>
			/// Log_2 of the voxel chunk stride.
			/// </summary>
			private const int ChunkBits = 4;

			/// <summary>
			/// Voxel Chunk side length.
			/// </summary>
			private const int ChunkSize = 16;

			/// <summary>
			/// Target Storage
			/// </summary>
			private VRage.Game.Voxels.IMyStorage m_storage;

			/// <summary>
			/// VoxelMap affected by the cutouts.
			/// </summary>
			private MyVoxelBase m_targetVoxel;

			/// <summary>
			/// Matrix that transforms the drill cutouts from their local space to storage space.
			/// </summary>
			private Matrix m_drillToStorageMatrix;

			/// <summary>
			/// Position of the center of the cluster bounding box in the voxel storage.
			/// </summary>
			/// <remarks>This is used later to retain precision in the cut-out while not requiring the use of a double precision transformation matrix.</remarks>
			private Vector3I m_storageOffset;

			/// <summary>
			/// Bounding box of the cluster in ship space.
			/// </summary>
			private BoundingBox m_clusterBounds;

			/// <summary>
			/// Bounding box of the cluster in ship space.
			/// </summary>
			private BoundingBoxI m_totalAffectedRange;

			/// <summary>
			/// Whether any materials were affected.
			/// </summary>
			private bool m_materialsChanged;

			/// <summary>
			/// Cutouts for each drill
			/// </summary>
			private PooledMemory<CutOutData> m_cutOuts;

			/// <summary>
			/// List of (chunk, drill) pairs used to prepare the operation.
			/// </summary>
			private readonly List<(Vector3I Chunk, int CutOut)> m_chunkCutOutPairs = new List<(Vector3I, int)>();

			/// <summary>
			/// Execution state.
			/// </summary>
			private State m_state;

			/// <summary>
			/// Whether this is a cutout performed by a client.
			/// </summary>
			private bool IsClient;

			/// <summary>
			/// Comparer used to sort the <see cref="F:Sandbox.Game.GameSystems.MyShipMiningSystem.ClusterCutOut.m_chunkCutOutPairs" /> array.
			/// </summary>
=======
			private const int ChunkBits = 4;

			private const int ChunkSize = 16;

			private VRage.Game.Voxels.IMyStorage m_storage;

			private MyVoxelBase m_targetVoxel;

			private Matrix m_drillToStorageMatrix;

			private Vector3I m_storageOffset;

			private BoundingBox m_clusterBounds;

			private BoundingBoxI m_totalAffectedRange;

			private bool m_materialsChanged;

			private PooledMemory<CutOutData> m_cutOuts;

			private readonly List<(Vector3I Chunk, int CutOut)> m_chunkCutOutPairs = new List<(Vector3I, int)>();

			private State m_state;

			private bool IsClient;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			private static readonly IComparer<(Vector3I Chunk, int Drill)> m_sortPerChunk = new DrillChunkComparer();

			[ThreadStatic]
			private static MyStorageData m_storageData;

<<<<<<< HEAD
			/// <summary>
			/// Matrix that transforms the drill cutouts from their local space to storage space.
			/// </summary>
			private MatrixD m_debugDrawMatrix;

			/// <summary>
			/// Chunk progress in the cutout.
			/// </summary>
			private int m_debugChunkProgress;

			/// <summary>
			/// Time this operation switched to the finished state.
			/// </summary>
			private DateTime m_debugFinishedTime;

			/// <inheritdoc />
=======
			private MatrixD m_debugDrawMatrix;

			private int m_debugChunkProgress;

			private DateTime m_debugFinishedTime;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public WorkOptions Options
			{
				get
				{
					WorkOptions result = default(WorkOptions);
					result.DebugName = "Drill Cluster Cutout";
					result.MaximumThreads = 1;
					result.TaskType = MyProfiler.TaskType.Voxels;
					return result;
				}
			}

<<<<<<< HEAD
			/// <summary>
			/// Get a new pooled cutout and prepare it for execution.
			/// </summary>
			/// <param name="system"></param>
			/// <param name="cluster"></param>
			/// <param name="target"></param>
			/// <param name="slice"></param>
			/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public static ClusterCutOut GetAndPrepare(MyShipMiningSystem system, DrillCluster cluster, MyVoxelBase target, Span<CutOutRequest> slice)
			{
				ClusterCutOut clusterCutOut = PoolManager.Get<ClusterCutOut>();
				clusterCutOut.Prepare(system, cluster, target, slice);
				return clusterCutOut;
			}

			private void Prepare(MyShipMiningSystem system, DrillCluster cluster, MyVoxelBase target, Span<CutOutRequest> slice)
			{
				m_state = State.Idle;
				IsClient = false;
				m_storage = target.Storage;
				m_targetVoxel = target;
				MatrixD.Multiply(ref Unsafe.AsRef(in system.Grid.PositionComp.WorldMatrixRef), ref Unsafe.AsRef(in target.PositionComp.WorldMatrixInvScaled), out var result);
				result.Translation += target.StorageMin + target.SizeInMetresHalf;
				Vector3 center = cluster.Bounds.Center;
				m_storageOffset = Vector3I.Floor(Vector3D.Transform(center, ref result));
				result.Translation -= (Vector3D)m_storageOffset;
				m_drillToStorageMatrix = result;
				m_debugDrawMatrix = target.PositionComp.WorldMatrixRef;
				m_debugDrawMatrix = MatrixD.CreateTranslation(m_storageOffset - target.SizeInMetresHalf - target.StorageMin) * m_debugDrawMatrix;
				PoolManager.BorrowMemory(slice.Length, out m_cutOuts);
				bool flag = false;
				for (int i = 0; i < slice.Length; i++)
				{
					ref CutOutRequest reference = ref slice[i];
					m_cutOuts[i] = new CutOutData(in reference, in system.GetDrillData(reference.Drill));
					flag |= reference.ApplyDiscardingMultiplier;
				}
				if (flag)
				{
					m_clusterBounds = cluster.ExpandedBounds;
				}
				else
				{
					m_clusterBounds = cluster.Bounds;
				}
				m_clusterBounds = m_clusterBounds.Transform(m_drillToStorageMatrix);
			}

<<<<<<< HEAD
			/// <summary>
			/// Prepare from a networked cutout data set.
			/// </summary>
			/// <param name="data"></param>
			/// <param name="target"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void Prepare(in NetworkCutoutData data, MyVoxelBase target)
			{
				m_state = State.Idle;
				IsClient = true;
				m_targetVoxel = target;
				m_storage = target.Storage;
				m_storageOffset = data.StorageOffset;
				m_totalAffectedRange = data.AffectedRange;
				NetworkCutoutData.CutOut[] cutOuts = data.CutOuts;
				PoolManager.BorrowMemory(cutOuts.Length, out m_cutOuts);
				for (int i = 0; i < cutOuts.Length; i++)
				{
					m_cutOuts[i] = new CutOutData(in cutOuts[i], m_storageOffset);
				}
			}

			public void Dispatch(HashSet<ClusterCutOut> operationQueue)
			{
				m_state = State.Queued;
				operationQueue?.Add(this);
				Parallel.Start(this, delegate
				{
					Finish(operationQueue);
				});
			}

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void DoWork(WorkData workData = null)
			{
				m_storageData = m_storageData ?? new MyStorageData(MyStorageDataTypeFlags.ContentAndMaterial);
				m_state = State.Preparing;
				BoundingBoxI bounds;
				if (!IsClient)
				{
					for (int i = 0; i < m_cutOuts.Length; i++)
					{
						ref CutOutData reference = ref m_cutOuts[i];
						ref BoundingSphere sphereInVoxelSpace = ref reference.SphereInVoxelSpace;
						sphereInVoxelSpace = sphereInVoxelSpace.Transform(m_drillToStorageMatrix);
						reference.StorageBounds = new BoundingBoxI(Vector3I.Floor(sphereInVoxelSpace.Center - sphereInVoxelSpace.Radius) + m_storageOffset - 1, Vector3I.Ceiling(sphereInVoxelSpace.Center + sphereInVoxelSpace.Radius) + m_storageOffset + 1);
					}
					BoundingBox clusterBounds = m_clusterBounds;
					bounds = new BoundingBoxI(Vector3I.Floor(clusterBounds.Min), Vector3I.Ceiling(clusterBounds.Max));
					bounds.Min += m_storageOffset - 1;
					bounds.Max += m_storageOffset + 1;
					m_totalAffectedRange = BoundingBoxI.CreateInvalid();
				}
				else
				{
					bounds = m_totalAffectedRange;
				}
				if (bounds.Size.AbsMax() <= 16)
				{
					CutSmallArea(in bounds);
				}
				else
				{
					CutLargeArea();
				}
				m_state = State.Finished;
				m_debugFinishedTime = DateTime.Now;
			}

			private void CutSmallArea(in BoundingBoxI bounds)
			{
				m_storageData.Resize(bounds.Size);
				m_storage.ReadRange(m_storageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, bounds.Min, bounds.Max - 1);
				m_state = State.Executing;
				m_debugChunkProgress = 0;
				(bool, bool) changes = (false, false);
				for (int i = 0; i < m_cutOuts.Length; i++)
				{
					ref CutOutData reference = ref m_cutOuts[i];
					BoundingSphere sphereInVoxelSpace = reference.SphereInVoxelSpace;
					Vector3I vector3I = m_storageOffset - bounds.Min;
					sphereInVoxelSpace.Center += (Vector3)vector3I;
					CutOutChunk(sphereInVoxelSpace, m_storageData, ref reference, ref changes);
					if (DebugOperationDelay > 0f)
					{
						Thread.Sleep((int)(DebugOperationDelay * 1000f));
					}
				}
				CommitDataToStorage(in bounds, in changes);
				m_materialsChanged = changes.Item2;
			}

			private void CutLargeArea()
			{
				for (int i = 0; i < m_cutOuts.Length; i++)
				{
					BoundingBoxI storageBounds = m_cutOuts[i].StorageBounds;
					storageBounds.Min >>= 4;
					storageBounds.Max = storageBounds.Max + 15 >> 4;
					foreach (Vector3I item in BoundingBoxI.EnumeratePoints(storageBounds))
					{
						m_chunkCutOutPairs.Add((item, i));
					}
				}
				m_chunkCutOutPairs.Sort(m_sortPerChunk);
				m_state = State.Executing;
				m_debugChunkProgress = 0;
				m_storageData.Resize(new Vector3I(16));
				BoundingBoxI range = default(BoundingBoxI);
				Vector3I vector3I = -Vector3I.One;
				Vector3I vector3I2 = default(Vector3I);
				(bool, bool) changes = (false, false);
				foreach (var (vector3I3, index) in m_chunkCutOutPairs)
				{
					if (vector3I3 != vector3I)
					{
						if (vector3I != -Vector3I.One)
						{
							CommitDataToStorage(in range, in changes);
						}
						range = new BoundingBoxI(vector3I3 << 4, vector3I3 + 1 << 4);
						m_storage.ReadRange(m_storageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, range.Min, range.Max - 1);
						vector3I2 = m_storageOffset - range.Min;
						vector3I = vector3I3;
					}
					ref CutOutData reference = ref m_cutOuts[index];
					BoundingSphere sphereInVoxelSpace = reference.SphereInVoxelSpace;
					sphereInVoxelSpace.Center += (Vector3)vector3I2;
					CutOutChunk(sphereInVoxelSpace, m_storageData, ref reference, ref changes);
					if (DebugOperationDelay > 0f)
					{
						Thread.Sleep((int)(DebugOperationDelay * 1000f));
					}
					m_debugChunkProgress++;
				}
				CommitDataToStorage(in range, in changes);
				m_materialsChanged = changes.Item2;
			}

			private void CommitDataToStorage(in BoundingBoxI range, in (bool Content, bool Material) changes)
			{
				if (changes.Content)
				{
					m_totalAffectedRange.Include(range);
					m_storage.WriteRange(m_storageData, (!changes.Material) ? MyStorageDataTypeFlags.Content : MyStorageDataTypeFlags.ContentAndMaterial, range.Min, range.Max - 1, notify: false);
				}
			}

			private void CutOutChunk(BoundingSphere sphere, MyStorageData data, ref CutOutData cutOutData, ref (bool Content, bool Material) changes)
			{
				changes.Material |= cutOutData.ApplyDamagedMaterial;
				Vector3 vector = -sphere.Center;
				Vector3 vector2 = vector;
				float num = (sphere.Radius + 1f) * (sphere.Radius + 1f);
				float num2 = (sphere.Radius - 1f) * (sphere.Radius - 1f);
				if ((double)sphere.Radius < 0.5)
				{
					num2 = 0f;
				}
				int num3 = 0;
				bool flag = IsClient;
				Vector3I size3D = data.Size3D;
				Vector3I vector3I = default(Vector3I);
				vector3I.Z = 0;
				while (vector3I.Z < size3D.Z)
				{
					vector3I.Y = 0;
					while (vector3I.Y < size3D.Y)
					{
						vector3I.X = 0;
						while (vector3I.X < size3D.X)
						{
							float num4 = vector.LengthSquared();
							if (num4 < num)
							{
								byte b = data.Content(num3);
								byte b2;
								if (num4 < num2)
								{
									b2 = 0;
								}
								else
								{
									float num5 = (float)Math.Sqrt(num4) - sphere.Radius;
									b2 = (byte)((num5 + 1f) * 127.5f);
									if (b2 >= b)
									{
										b2 = (byte)((float)(int)b * num5);
									}
								}
								if (b2 < b)
								{
									if (!IsClient)
									{
										flag = true;
										int num6 = b - b2;
										if (MyFakes.ENABLE_REMOVED_VOXEL_CONTENT_HACK)
										{
											num6 = (int)((float)num6 * 4.62f);
										}
										byte b3 = data.Material(num3);
										if (b3 >= 0 && b3 < cutOutData.CutOutMaterials.Length)
										{
											cutOutData.CutOutMaterials[b3] += num6;
										}
									}
									data.Content(num3, b2);
								}
							}
							num3++;
							vector.X += 1f;
							vector3I.X++;
						}
						vector.X = vector2.X;
						vector.Y += 1f;
						vector3I.Y++;
					}
					vector.Y = vector2.Y;
					vector.Z += 1f;
					vector3I.Z++;
				}
				cutOutData.CausedChange |= flag;
				changes.Content |= flag;
			}

			private void Finish(HashSet<ClusterCutOut> operationQueue)
			{
				if (m_totalAffectedRange.IsValid)
				{
					if (!IsClient)
					{
						for (int i = 0; i < m_cutOuts.Length; i++)
						{
							ref CutOutData reference = ref m_cutOuts[i];
							Dictionary<MyVoxelMaterialDefinition, int> dictionary = new Dictionary<MyVoxelMaterialDefinition, int>();
							for (int j = 0; j < reference.CutOutMaterials.Length; j++)
							{
								dictionary[MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)j)] = reference.CutOutMaterials[j];
							}
							reference.Drill.OnDrillResults(dictionary, reference.HitPosition, !reference.DiscardResults);
						}
					}
					Vector3I voxelRangeMin = m_totalAffectedRange.Min;
					Vector3I voxelRangeMax = m_totalAffectedRange.Max - 1;
					m_storage.NotifyRangeChanged(ref voxelRangeMin, ref voxelRangeMax, (!m_materialsChanged) ? MyStorageDataTypeFlags.Content : MyStorageDataTypeFlags.ContentAndMaterial);
					BoundingBoxD cutOutBox = m_totalAffectedRange;
					cutOutBox.Translate(m_targetVoxel.SizeInMetresHalf + m_targetVoxel.StorageMin);
					cutOutBox.TransformFast(m_targetVoxel.PositionComp.WorldMatrixRef);
					MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, m_targetVoxel, ref cutOutBox);
					if (Sync.IsServer && Sync.Clients.Count > 1)
					{
						int num = 0;
						for (int k = 0; k < m_cutOuts.Length; k++)
						{
							if (m_cutOuts[k].CausedChange)
							{
								num++;
							}
						}
						NetworkCutoutData.CutOut[] array = new NetworkCutoutData.CutOut[num];
						int num2 = 0;
						for (int l = 0; l < m_cutOuts.Length; l++)
						{
							ref CutOutData reference2 = ref m_cutOuts[l];
							if (reference2.CausedChange)
							{
								array[num2++] = new NetworkCutoutData.CutOut(reference2.SphereInVoxelSpace, reference2.ApplyDamagedMaterial);
							}
						}
						MyVoxelBase rootVoxel = m_targetVoxel.RootVoxel;
						NetworkCutoutData data = new NetworkCutoutData(m_storageOffset, m_totalAffectedRange, array);
						rootVoxel.BroadcastShipCutout(in data);
					}
				}
				if (IsClient || !DebugDrawCutOuts || DebugDrawCutOutPermanence <= 0f)
				{
					Return(operationQueue);
				}
			}

			public void Return(HashSet<ClusterCutOut> operationQueue)
			{
				m_state = State.Pooled;
				operationQueue?.Remove(this);
				m_chunkCutOutPairs.Clear();
				PoolManager.ReturnBorrowedMemory(ref m_cutOuts);
				ClusterCutOut obj = this;
				PoolManager.Return(ref obj);
			}

<<<<<<< HEAD
			/// <summary>
			/// Draws debug information for this cutout and returns a boolean indicating whether this cutout should be removed from the execution queue.
			/// </summary>
			/// <returns></returns>
			public bool DebugDraw()
			{
				Color color;
				switch (m_state)
				{
				case State.Pooled:
					color = Color.Red;
					break;
				case State.Queued:
					color = Color.Blue;
					break;
				case State.Preparing:
					color = Color.Orange;
					break;
				case State.Executing:
					color = Color.Yellow;
					break;
				case State.Finished:
					color = Color.Green;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
=======
			public bool DebugDraw()
			{
				Color color = m_state switch
				{
					State.Pooled => Color.Red, 
					State.Queued => Color.Blue, 
					State.Preparing => Color.Orange, 
					State.Executing => Color.Yellow, 
					State.Finished => Color.Green, 
					_ => throw new ArgumentOutOfRangeException(), 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(m_clusterBounds, m_debugDrawMatrix), color, 0.1f, depthRead: true, smooth: true);
				if ((int)m_state > 3)
				{
					for (int i = 0; i < m_cutOuts.Length; i++)
					{
						ref BoundingSphere sphereInVoxelSpace = ref m_cutOuts[i].SphereInVoxelSpace;
						Vector3D.Transform(ref sphereInVoxelSpace.Center, ref m_debugDrawMatrix, out var result);
						MyRenderProxy.DebugDrawSphere(result, sphereInVoxelSpace.Radius, color.Alpha(0.2f), 1f, depthRead: true, smooth: true);
						MyRenderProxy.DebugDrawSphere(result, sphereInVoxelSpace.Radius, color);
					}
					Vector3D pointFrom = Vector3D.Zero;
					Vector3I vector3I = new Vector3I(int.MinValue);
					for (int j = 0; j < m_chunkCutOutPairs.Count; j++)
					{
						var (vector3I2, index) = m_chunkCutOutPairs[j];
						if (vector3I != vector3I2)
						{
							Color color2 = Color.Blue;
							if (m_debugChunkProgress >= j)
							{
								color2 = ((m_debugChunkProgress != m_chunkCutOutPairs.Count && !(m_chunkCutOutPairs[m_debugChunkProgress].Chunk != vector3I2)) ? Color.Yellow : Color.Green);
							}
							BoundingBox boundingBox = new BoundingBox(vector3I2 << 4, vector3I2 + 1 << 4);
							boundingBox.Translate(-m_storageOffset);
							pointFrom = Vector3D.Transform(boundingBox.Center, m_debugDrawMatrix);
							MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(boundingBox, m_debugDrawMatrix), color2, 0.1f, depthRead: true, smooth: true);
						}
						Vector3D.Transform(ref m_cutOuts[index].SphereInVoxelSpace.Center, ref m_debugDrawMatrix, out var result2);
						Color colorFrom = ((m_debugChunkProgress > j) ? Color.Green : ((m_debugChunkProgress == j) ? Color.Yellow : Color.Blue));
						MyRenderProxy.DebugDrawArrow3D(pointFrom, result2, colorFrom);
						vector3I = vector3I2;
					}
				}
				if (m_state == State.Finished)
				{
					return (DateTime.Now - m_debugFinishedTime).TotalSeconds > (double)DebugDrawCutOutPermanence;
				}
				return false;
			}
		}

		[Serializable]
		public struct NetworkCutoutData
		{
			[Serializable]
			[DebuggerDisplay("{DebugText}")]
			public struct CutOut
			{
				protected class Sandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData_003C_003ECutOut_003C_003ESphere_003C_003EAccessor : IMemberAccessor<CutOut, BoundingSphere>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref CutOut owner, in BoundingSphere value)
					{
						owner.Sphere = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref CutOut owner, out BoundingSphere value)
					{
						value = owner.Sphere;
					}
				}

				protected class Sandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData_003C_003ECutOut_003C_003EApplyDamagedMaterial_003C_003EAccessor : IMemberAccessor<CutOut, bool>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref CutOut owner, in bool value)
					{
						owner.ApplyDamagedMaterial = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref CutOut owner, out bool value)
					{
						value = owner.ApplyDamagedMaterial;
					}
				}

				public BoundingSphere Sphere;

				public bool ApplyDamagedMaterial;

				private string DebugText => string.Format("{0}{1}", Sphere, ApplyDamagedMaterial ? " ApplyDamage" : "");

				public CutOut(BoundingSphere sphere, bool applyDamagedMaterial)
				{
					Sphere = sphere;
					ApplyDamagedMaterial = applyDamagedMaterial;
				}
			}

			protected class Sandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData_003C_003EStorageOffset_003C_003EAccessor : IMemberAccessor<NetworkCutoutData, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref NetworkCutoutData owner, in Vector3I value)
				{
					owner.StorageOffset = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref NetworkCutoutData owner, out Vector3I value)
				{
					value = owner.StorageOffset;
				}
			}

			protected class Sandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData_003C_003EAffectedRange_003C_003EAccessor : IMemberAccessor<NetworkCutoutData, BoundingBoxI>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref NetworkCutoutData owner, in BoundingBoxI value)
				{
					owner.AffectedRange = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref NetworkCutoutData owner, out BoundingBoxI value)
				{
					value = owner.AffectedRange;
				}
			}

			protected class Sandbox_Game_GameSystems_MyShipMiningSystem_003C_003ENetworkCutoutData_003C_003ECutOuts_003C_003EAccessor : IMemberAccessor<NetworkCutoutData, CutOut[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref NetworkCutoutData owner, in CutOut[] value)
				{
					owner.CutOuts = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref NetworkCutoutData owner, out CutOut[] value)
				{
					value = owner.CutOuts;
				}
			}

			public Vector3I StorageOffset;

			public BoundingBoxI AffectedRange;

			public CutOut[] CutOuts;

			public NetworkCutoutData(Vector3I storageOffset, BoundingBoxI affectedRange, CutOut[] cutOuts)
			{
				StorageOffset = storageOffset;
				CutOuts = cutOuts;
				AffectedRange = affectedRange;
			}
		}

		private struct DrillData
		{
<<<<<<< HEAD
			/// <summary>
			/// Drill for this data.
			/// </summary>
			public readonly MyShipDrill Drill;

			/// <summary>
			/// Cluster that contains this mining drill.
			/// </summary>
			public DrillCluster Cluster;

			/// <summary>
			/// Id of the tree node for this drill.
			/// </summary>
			public int TreeProxyId;

			/// <summary>
			/// Cut out sphere for this drill in grid space.
			/// </summary>
=======
			public readonly MyShipDrill Drill;

			public DrillCluster Cluster;

			public int TreeProxyId;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public readonly BoundingSphere BondingSphere;

			public DrillData(MyShipDrill drill)
			{
				Drill = drill;
				BondingSphere = drill.GetDrillingSphere();
				Cluster = null;
				TreeProxyId = -1;
			}
		}

		private readonly MyDynamicAABBTree m_drillTree = new MyDynamicAABBTree(Vector3.Zero);

		private readonly HashSet<DrillCluster> m_dirtyClusters = new HashSet<DrillCluster>();

<<<<<<< HEAD
		/// <summary>
		/// Whether to draw mining drill clusters and their contents.
		/// </summary>
		public static bool DebugDisable = false;

		/// <summary>
		/// Whether to draw mining drill clusters and their contents.
		/// </summary>
		public static bool DebugDrawClusters = false;

		/// <summary>
		/// Debug draw in-flight cut-outs.
		/// </summary>
		public static bool DebugDrawCutOuts = false;

		/// <summary>
		/// When debug drawing cutouts this is the number of seconds they remain visible after having finished.
		/// </summary>
		public static float DebugDrawCutOutPermanence = 15f;

		/// <summary>
		/// Cause each individual cut-out operation to be delayed by the provided number of seconds.
		/// </summary>
		public static float DebugOperationDelay = 0f;

		/// <summary>
		/// List of cutout requests collected this frame and still pending.
		/// </summary>
		private readonly MyList<CutOutRequest> m_pendingCutOuts = new MyList<CutOutRequest>();

		/// <summary>
		/// Debug hashset used to verify that out cutout classification works.
		/// </summary>
		private readonly HashSet<(DrillCluster, MyVoxelBase)> m_cutOutsPerClusterVoxel = new HashSet<(DrillCluster, MyVoxelBase)>();

		/// <summary>
		/// Cutouts that are execution (or that have already finished if <see cref="F:Sandbox.Game.GameSystems.MyShipMiningSystem.DebugDrawCutOuts" /> is true and <see cref="F:Sandbox.Game.GameSystems.MyShipMiningSystem.DebugDrawCutOutPermanence" /> is greater than zero).
		/// </summary>
=======
		public static bool DebugDisable = false;

		public static bool DebugDrawClusters = false;

		public static bool DebugDrawCutOuts = false;

		public static float DebugDrawCutOutPermanence = 15f;

		public static float DebugOperationDelay = 0f;

		private readonly MyList<CutOutRequest> m_pendingCutOuts = new MyList<CutOutRequest>();

		private readonly HashSet<(DrillCluster, MyVoxelBase)> m_cutOutsPerClusterVoxel = new HashSet<(DrillCluster, MyVoxelBase)>();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly HashSet<ClusterCutOut> m_executingCutOuts = new HashSet<ClusterCutOut>();

		private const float VOXEL_CONTENT_HACK_MULTIPLIER = 4.62f;

		private readonly MyFreeList<DrillData> m_drills = new MyFreeList<DrillData>();

		private readonly CachingHashSet<MyShipDrill> m_drillsForUpdate10 = new CachingHashSet<MyShipDrill>();

<<<<<<< HEAD
		/// <inheritdoc />
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceAfterSimulation;

		/// <inheritdoc />
=======
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceAfterSimulation;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override bool UpdateInParallel => true;

		private void AddDrillToCluster(MyShipDrill drill)
		{
			ref DrillData reference = ref m_drills[drill.ShipDrillId];
			BoundingSphere bondingSphere = reference.BondingSphere;
			bondingSphere.Radius *= 3f;
			BoundingBox aabb = bondingSphere.GetBoundingBox();
			ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode> nodes = m_drillTree.Nodes;
			int root = m_drillTree.GetRoot();
			bool flag = false;
			DrillCluster drillCluster = null;
			if (root != -1)
			{
				Stack<int> poolObject;
				using (PoolManager.Get<Stack<int>>(out poolObject))
				{
					poolObject.Push(root);
<<<<<<< HEAD
					while (poolObject.Count > 0)
=======
					while (poolObject.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyDynamicAABBTree.DynamicTreeNode dynamicTreeNode = nodes[poolObject.Pop()];
						if (!dynamicTreeNode.IsLeaf())
						{
							if (nodes[dynamicTreeNode.Child1].Aabb.Intersects(aabb))
							{
								poolObject.Push(dynamicTreeNode.Child1);
							}
							if (dynamicTreeNode.Child1 != -1 && nodes[dynamicTreeNode.Child2].Aabb.Intersects(aabb))
							{
								poolObject.Push(dynamicTreeNode.Child2);
							}
							continue;
						}
						MyShipDrill myShipDrill = (MyShipDrill)dynamicTreeNode.UserData;
						DrillCluster cluster = m_drills[myShipDrill.ShipDrillId].Cluster;
						if (drillCluster == null)
						{
							drillCluster = cluster;
						}
						else if (cluster != drillCluster)
						{
							flag = true;
							Mark(drillCluster);
							Mark(cluster);
						}
					}
				}
			}
			if (drillCluster == null)
			{
				drillCluster = new DrillCluster();
			}
			drillCluster.AddDrill(drill.ShipDrillId, in reference);
			reference.TreeProxyId = m_drillTree.AddProxy(ref aabb, drill, 0u);
			reference.Cluster = drillCluster;
			if (flag)
			{
				Schedule();
			}
			void Mark(DrillCluster cc)
			{
				if (!cc.NeedsReconstruction)
				{
					m_dirtyClusters.Add(cc);
					cc.NeedsReconstruction = true;
				}
			}
		}

		private void RemoveDrillFromCluster(MyShipDrill drill)
		{
			ref DrillData reference = ref m_drills[drill.ShipDrillId];
			DrillCluster cluster = reference.Cluster;
			cluster.RemoveDrill(drill.ShipDrillId);
			m_drillTree.RemoveProxy(reference.TreeProxyId);
			if (cluster.IsEmpty)
			{
				m_dirtyClusters.Remove(cluster);
<<<<<<< HEAD
				if (m_dirtyClusters.Count == 0)
=======
				if (m_dirtyClusters.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					DeSchedule();
				}
			}
			else
			{
				Schedule();
				m_dirtyClusters.Add(cluster);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Re-calculate all drill clusters.
		/// </summary>
		private void UpdateClusters()
		{
			List<int> obj = null;
			foreach (DrillCluster dirtyCluster in m_dirtyClusters)
			{
				if (!dirtyCluster.NeedsReconstruction && !dirtyCluster.RemoveThresholdReached)
				{
					dirtyCluster.UpdateBounds(this);
					continue;
				}
				obj = obj ?? PoolManager.Get<List<int>>();
				obj.AddRange(dirtyCluster.Drills);
				foreach (int drill in dirtyCluster.Drills)
				{
					m_drills[drill].Cluster = null;
				}
			}
=======
		private void UpdateClusters()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			List<int> obj = null;
			Enumerator<DrillCluster> enumerator = m_dirtyClusters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DrillCluster current = enumerator.get_Current();
					if (!current.NeedsReconstruction && !current.RemoveThresholdReached)
					{
						current.UpdateBounds(this);
						continue;
					}
					obj = obj ?? PoolManager.Get<List<int>>();
					obj.AddRange(current.Drills);
					Enumerator<int> enumerator2 = current.Drills.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							m_drills[current2].Cluster = null;
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_dirtyClusters.Clear();
			if (obj == null)
			{
				return;
			}
			MyUnionFind myUnionFind = new MyUnionFind(m_drills.UsedLength);
			for (int i = 0; i < obj.Count; i++)
			{
				CollectSiblings(obj[i], myUnionFind);
			}
			for (int j = 0; j < obj.Count; j++)
			{
				int num = obj[j];
				ref DrillData reference = ref m_drills[num];
				int index = myUnionFind.Find(num);
				ref DrillData reference2 = ref m_drills[index];
				if (reference2.Cluster == null)
				{
					reference2.Cluster = new DrillCluster();
				}
				reference2.Cluster.AddDrill(num, in reference);
				reference.Cluster = reference2.Cluster;
			}
			PoolManager.Return(ref obj);
		}

<<<<<<< HEAD
		/// <summary>
		/// Collect all drills that overlap the current one.
		/// </summary>
		/// <param name="drillIndex">The current drill.</param>
		/// <param name="sets">The set of all drill clusters.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void CollectSiblings(int drillIndex, MyUnionFind sets)
		{
			BoundingSphere bondingSphere = m_drills[drillIndex].BondingSphere;
			bondingSphere.Radius *= 3f;
			BoundingBox boundingBox = bondingSphere.GetBoundingBox();
			ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode> nodes = m_drillTree.Nodes;
			int root = m_drillTree.GetRoot();
			Stack<int> poolObject;
			using (PoolManager.Get<Stack<int>>(out poolObject))
			{
				poolObject.Push(root);
<<<<<<< HEAD
				while (poolObject.Count > 0)
=======
				while (poolObject.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyDynamicAABBTree.DynamicTreeNode dynamicTreeNode = nodes[poolObject.Pop()];
					if (!dynamicTreeNode.IsLeaf())
					{
						if (nodes[dynamicTreeNode.Child1].Aabb.Intersects(boundingBox))
						{
							poolObject.Push(dynamicTreeNode.Child1);
						}
						if (dynamicTreeNode.Child1 != -1 && nodes[dynamicTreeNode.Child2].Aabb.Intersects(boundingBox))
						{
							poolObject.Push(dynamicTreeNode.Child2);
						}
					}
					else
					{
						MyShipDrill myShipDrill = (MyShipDrill)dynamicTreeNode.UserData;
						sets.Union(drillIndex, myShipDrill.ShipDrillId);
					}
				}
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyShipMiningSystem(MyCubeGrid grid)
			: base(grid)
		{
		}

<<<<<<< HEAD
		/// <inheritdoc />
		protected override void Update()
		{
			if (m_dirtyClusters.Count > 0)
=======
		protected override void Update()
		{
			if (m_dirtyClusters.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				UpdateClusters();
			}
			if (m_pendingCutOuts.Count > 0)
			{
				ScheduleCutouts();
			}
		}

		public void DebugDraw()
		{
<<<<<<< HEAD
			if (DebugDrawClusters)
			{
				foreach (DrillCluster item in m_drillTree.Leaves.Select((MyDynamicAABBTree.DynamicTreeNode x) => GetDrillData((MyShipDrill)x.UserData).Cluster).Distinct())
=======
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (DebugDrawClusters)
			{
				foreach (DrillCluster item in Enumerable.Distinct<DrillCluster>(Enumerable.Select<MyDynamicAABBTree.DynamicTreeNode, DrillCluster>((IEnumerable<MyDynamicAABBTree.DynamicTreeNode>)m_drillTree.Leaves, (Func<MyDynamicAABBTree.DynamicTreeNode, DrillCluster>)((MyDynamicAABBTree.DynamicTreeNode x) => GetDrillData((MyShipDrill)x.UserData).Cluster))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					item.DebugDraw(this, base.Grid.PositionComp.WorldMatrixRef);
				}
			}
			if (!DebugDrawCutOuts)
			{
				return;
			}
			List<ClusterCutOut> list = null;
<<<<<<< HEAD
			foreach (ClusterCutOut executingCutOut in m_executingCutOuts)
			{
				if (executingCutOut.DebugDraw())
				{
					list = list ?? new List<ClusterCutOut>();
					list.Add(executingCutOut);
				}
			}
=======
			Enumerator<ClusterCutOut> enumerator2 = m_executingCutOuts.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					ClusterCutOut current = enumerator2.get_Current();
					if (current.DebugDraw())
					{
						list = list ?? new List<ClusterCutOut>();
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (list == null)
			{
				return;
			}
			foreach (ClusterCutOut item2 in list)
			{
				item2.Return(m_executingCutOuts);
			}
		}

		private void ScheduleCutouts()
		{
			m_pendingCutOuts.Sort(CutOutRequest.CutOutDataComparer);
			int i = 0;
			while (i < m_pendingCutOuts.Count)
			{
				CutOutRequest cutOutRequest = m_pendingCutOuts[i];
				(DrillCluster, MyVoxelBase) tuple = (cutOutRequest.Cluster, cutOutRequest.Voxel);
				int num = i;
				for (; i < m_pendingCutOuts.Count; i++)
				{
					CutOutRequest cutOutRequest2 = m_pendingCutOuts[i];
					if (cutOutRequest2.Cluster != tuple.Item1 || cutOutRequest2.Voxel != tuple.Item2)
					{
						break;
					}
				}
				int length = i - num;
<<<<<<< HEAD
				cutOutRequest.Voxel.BeforeContentChanged = true;
				ClusterCutOut.GetAndPrepare(this, cutOutRequest.Cluster, cutOutRequest.Voxel, m_pendingCutOuts.Slice(num, length)).Dispatch(m_executingCutOuts);
				cutOutRequest.Voxel.ContentChanged = true;
=======
				ClusterCutOut.GetAndPrepare(this, cutOutRequest.Cluster, cutOutRequest.Voxel, m_pendingCutOuts.Slice(num, length)).Dispatch(m_executingCutOuts);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_cutOutsPerClusterVoxel.Clear();
			m_pendingCutOuts.Clear();
		}

		public static void PerformCutoutClient(MyVoxelBase target, in NetworkCutoutData data)
		{
			ClusterCutOut clusterCutOut = PoolManager.Get<ClusterCutOut>();
			clusterCutOut.Prepare(in data, target);
			clusterCutOut.Dispatch(null);
		}

		public void RegisterDrill(MyShipDrill drill)
		{
			drill.ShipDrillId = m_drills.Allocate(new DrillData(drill));
			AddDrillToCluster(drill);
		}

		public void UnRegisterDrill(MyShipDrill drill)
		{
			RemoveDrillFromCluster(drill);
			m_drills.Free(drill.ShipDrillId);
			drill.ShipDrillId = -1;
		}

<<<<<<< HEAD
		/// <summary>
		/// Request a cutout for <paramref name="drill" />.
		/// </summary>
		/// <param name="drill">The drill requesting the cutout.</param>
		/// <param name="applyDiscardingModifier">Whether to multiply the cutout radius by the discarding multiplier (<see cref="F:Sandbox.Game.Weapons.MyDrillBase.DISCARDING_RADIUS_MULTIPLIER" />).</param>
		/// <param name="applyDamagedMaterial">Whether to apply the respective damaged material for the remaining affected volume</param>
		/// <param name="hitPosition">The position in space for the hit that triggered this cutout.</param>
		/// <param name="voxel">The target voxel volume.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RequestCutOut(MyShipDrill drill, bool applyDiscardingModifier, bool applyDamagedMaterial, Vector3D hitPosition, MyVoxelBase voxel)
		{
			Schedule();
			ref DrillData reference = ref m_drills[drill.ShipDrillId];
			m_pendingCutOuts.Add(new CutOutRequest(drill, reference.Cluster, applyDiscardingModifier, applyDamagedMaterial, hitPosition, voxel));
		}

<<<<<<< HEAD
		/// <summary>
		/// Get the data stored about a given drill.
		/// </summary>
		/// <param name="drill"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private ref readonly DrillData GetDrillData(MyShipDrill drill)
		{
			if (drill.ShipDrillId != -1)
			{
				return ref m_drills[drill.ShipDrillId];
			}
			throw new KeyNotFoundException();
		}

<<<<<<< HEAD
		/// <summary>
		/// Get the data stored about a given drill.
		/// </summary>
		/// <param name="drillId"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private ref readonly DrillData GetDrillData(int drillId)
		{
			return ref m_drills[drillId];
		}

		public void AddDrillUpdate(MyShipDrill drill)
		{
<<<<<<< HEAD
=======
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (DebugDisable)
			{
				drill.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
			else
			{
				if (!m_drillsForUpdate10.Add(drill))
				{
					return;
				}
				MyShipDrill myShipDrill = null;
<<<<<<< HEAD
				foreach (MyShipDrill item in m_drillsForUpdate10)
				{
					if (item != drill)
					{
						myShipDrill = item;
						break;
					}
				}
=======
				Enumerator<MyShipDrill> enumerator = m_drillsForUpdate10.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyShipDrill current = enumerator.get_Current();
						if (current != drill)
						{
							myShipDrill = current;
							break;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myShipDrill != null)
				{
					drill.SynchronizeWith(myShipDrill);
				}
			}
		}

		public void RemoveDrillUpdate(MyShipDrill drill)
		{
			if (DebugDisable)
			{
				drill.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
			else
			{
				m_drillsForUpdate10.Remove(drill);
			}
		}

		internal void UpdateBeforeSimulation10()
		{
<<<<<<< HEAD
=======
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (DebugDisable)
			{
				return;
			}
			m_drillsForUpdate10.ApplyChanges();
<<<<<<< HEAD
			foreach (MyShipDrill item in m_drillsForUpdate10)
			{
				if (item.Closed)
				{
					m_drillsForUpdate10.Remove(item);
				}
				else
				{
					item.UpdateBeforeSimulation10();
				}
			}
=======
			Enumerator<MyShipDrill> enumerator = m_drillsForUpdate10.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyShipDrill current = enumerator.get_Current();
					if (current.Closed)
					{
						m_drillsForUpdate10.Remove(current);
					}
					else
					{
						current.UpdateBeforeSimulation10();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
