using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;

namespace VRageMath.Spatial
{
	public class MyClusterTree
	{
		public interface IMyActivationHandler
		{
			/// <summary>
			/// If true, than this object is not calculated into cluster division algorithm. It is just added or removed if dynamic object is in range.
			/// </summary>
			bool IsStaticForCluster { get; }

			/// <summary>
			/// Called when standalone object is added to cluster
			/// </summary>
			/// <param name="userData"></param>
			/// <param name="clusterObjectID"></param>
			void Activate(object userData, ulong clusterObjectID);

			/// <summary>
			/// Called when standalone object is removed from cluster
			/// </summary>
			/// <param name="userData"></param>
			void Deactivate(object userData);

			/// <summary>
			/// Called when multiple objects are added to cluster.
			/// </summary>
			/// <param name="userData"></param>
			/// <param name="clusterObjectID"></param>
			void ActivateBatch(object userData, ulong clusterObjectID);

			/// <summary>
			/// Called when multiple objects are removed from cluster.
			/// </summary>
			/// <param name="userData"></param>
			void DeactivateBatch(object userData);

			/// <summary>
			/// Called when adding multiple objects is finished.
			/// </summary>
			void FinishAddBatch();

			/// <summary>
			/// Called when removing multiple objects is finished.
			/// </summary>
			void FinishRemoveBatch(object userData);
		}

		public class MyCluster
		{
			public int ClusterId;

			public BoundingBoxD AABB;

			public HashSet<ulong> Objects;

			public object UserData;

			public override string ToString()
			{
				return "MyCluster" + ClusterId + ": " + AABB.Center;
			}
		}

		private class MyObjectData
		{
			public ulong Id;

			public MyCluster Cluster;

			public IMyActivationHandler ActivationHandler;

			public BoundingBoxD AABB;

			public int StaticId;

			public string Tag;

			public long EntityId;
		}

		public struct MyClusterQueryResult
		{
			public BoundingBoxD AABB;

			public object UserData;
		}

		private class AABBComparerX : IComparer<MyObjectData>
		{
			public static AABBComparerX Static = new AABBComparerX();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.X.CompareTo(y.AABB.Min.X);
			}
		}

		private class AABBComparerY : IComparer<MyObjectData>
		{
			public static AABBComparerY Static = new AABBComparerY();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.Y.CompareTo(y.AABB.Min.Y);
			}
		}

		private class AABBComparerZ : IComparer<MyObjectData>
		{
			public static AABBComparerZ Static = new AABBComparerZ();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.Z.CompareTo(y.AABB.Min.Z);
			}
		}

		private struct MyClusterDescription
		{
			public BoundingBoxD AABB;

			public List<MyObjectData> DynamicObjects;

			public List<MyObjectData> StaticObjects;
		}

		public Func<int, BoundingBoxD, object> OnClusterCreated;

<<<<<<< HEAD
		public Action<int, BoundingBoxD> OnAfterClusterCreated;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Action<object, int> OnClusterRemoved;

		public Action<object> OnFinishBatch;

		public Action OnClustersReordered;

		public Func<long, bool> GetEntityReplicableExistsById;

		/// <summary>
		/// On Entity Added, entity ID, cluster ID
		/// </summary>
		public Action<long, int> EntityAdded;

		/// <summary>
		/// On Entity Removed, entity ID, cluster ID
		/// </summary>
		public Action<long, int> EntityRemoved;

		public const ulong CLUSTERED_OBJECT_ID_UNITIALIZED = ulong.MaxValue;

		public static Vector3 IdealClusterSize = new Vector3(20000f);

		public static Vector3 IdealClusterSizeHalfSqr = IdealClusterSize * IdealClusterSize / 4f;

		public static Vector3 MinimumDistanceFromBorder = IdealClusterSize / 100f;

		public static Vector3 MaximumForSplit = IdealClusterSize * 2f;

		public static float MaximumClusterSize = 100000f;

		public readonly BoundingBoxD? SingleCluster;

		public readonly bool ForcedClusters;

		private bool m_suppressClusterReorder;

		private FastResourceLock m_clustersLock = new FastResourceLock();

		private FastResourceLock m_clustersReorderLock = new FastResourceLock();

		private MyDynamicAABBTreeD m_clusterTree = new MyDynamicAABBTreeD(Vector3D.Zero);

		private MyDynamicAABBTreeD m_staticTree = new MyDynamicAABBTreeD(Vector3D.Zero);

		private Dictionary<ulong, MyObjectData> m_objectsData = new Dictionary<ulong, MyObjectData>();

		private List<MyCluster> m_clusters = new List<MyCluster>();

		private ulong m_clusterObjectCounter;

		private List<MyCluster> m_returnedClusters = new List<MyCluster>(1);

		private List<object> m_userObjects = new List<object>();

		[ThreadStatic]
		private static List<MyLineSegmentOverlapResult<MyCluster>> m_lineResultListPerThread;

		[ThreadStatic]
		private static List<MyCluster> m_resultListPerThread;

		[ThreadStatic]
		private static List<ulong> m_objectDataResultListPerThread;

		public bool SuppressClusterReorder
		{
			get
			{
				return m_suppressClusterReorder;
			}
			set
			{
				m_suppressClusterReorder = value;
			}
		}

		private static List<MyLineSegmentOverlapResult<MyCluster>> m_lineResultList
		{
			get
			{
				if (m_lineResultListPerThread == null)
				{
					m_lineResultListPerThread = new List<MyLineSegmentOverlapResult<MyCluster>>();
				}
				return m_lineResultListPerThread;
			}
		}

		private static List<MyCluster> m_resultList
		{
			get
			{
				if (m_resultListPerThread == null)
				{
					m_resultListPerThread = new List<MyCluster>();
				}
				return m_resultListPerThread;
			}
		}

		private static List<ulong> m_objectDataResultList
		{
			get
			{
				if (m_objectDataResultListPerThread == null)
				{
					m_objectDataResultListPerThread = new List<ulong>();
				}
				return m_objectDataResultListPerThread;
			}
		}

		public MyClusterTree(BoundingBoxD? singleCluster, bool forcedClusters)
		{
			SingleCluster = singleCluster;
			ForcedClusters = forcedClusters;
		}

		public ulong AddObject(BoundingBoxD bbox, IMyActivationHandler activationHandler, ulong? customId, string tag, long entityId, bool batch)
		{
			using (m_clustersLock.AcquireExclusiveUsing())
			{
				if (SingleCluster.HasValue && m_clusters.Count == 0)
				{
					BoundingBoxD clusterBB = SingleCluster.Value;
					clusterBB.Inflate(200.0);
					CreateCluster(ref clusterBB);
				}
				BoundingBoxD bbox2 = ((!SingleCluster.HasValue && !ForcedClusters) ? bbox.GetInflated(IdealClusterSize / 100f) : bbox);
				m_clusterTree.OverlapAllBoundingBox(ref bbox2, m_returnedClusters);
				MyCluster myCluster = null;
				bool flag = false;
				if (m_returnedClusters.Count == 1)
				{
					if (m_returnedClusters[0].AABB.Contains(bbox2) == ContainmentType.Contains)
					{
						myCluster = m_returnedClusters[0];
					}
					else if (m_returnedClusters[0].AABB.Contains(bbox2) == ContainmentType.Intersects && activationHandler.IsStaticForCluster)
					{
						if (m_returnedClusters[0].AABB.Contains(bbox) != 0)
						{
							myCluster = m_returnedClusters[0];
						}
					}
					else
					{
						flag = true;
					}
				}
				else if (m_returnedClusters.Count > 1)
				{
					if (!activationHandler.IsStaticForCluster)
					{
						flag = true;
					}
				}
				else if (m_returnedClusters.Count == 0)
				{
					if (SingleCluster.HasValue)
					{
						return ulong.MaxValue;
					}
					if (!activationHandler.IsStaticForCluster)
					{
						BoundingBoxD bbox3 = new BoundingBoxD(bbox.Center - IdealClusterSize / 2f, bbox.Center + IdealClusterSize / 2f);
						m_clusterTree.OverlapAllBoundingBox(ref bbox3, m_returnedClusters);
						if (m_returnedClusters.Count == 0)
						{
							m_staticTree.OverlapAllBoundingBox(ref bbox3, m_objectDataResultList);
							myCluster = CreateCluster(ref bbox3);
							foreach (ulong objectDataResult in m_objectDataResultList)
							{
								if (m_objectsData[objectDataResult].Cluster == null)
								{
									AddObjectToCluster(myCluster, objectDataResult, entityId, batch: false);
								}
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				ulong num = (customId.HasValue ? customId.Value : m_clusterObjectCounter++);
				int staticId = -1;
				m_objectsData[num] = new MyObjectData
				{
					Id = num,
					ActivationHandler = activationHandler,
					AABB = bbox,
					StaticId = staticId,
					Tag = tag,
					EntityId = entityId
				};
				if (flag && !SingleCluster.HasValue && !ForcedClusters)
				{
					ReorderClusters(bbox, num);
					_ = m_objectsData[num].ActivationHandler.IsStaticForCluster;
				}
				if (activationHandler.IsStaticForCluster)
				{
					staticId = m_staticTree.AddProxy(ref bbox, num, 0u);
					m_objectsData[num].StaticId = staticId;
				}
				if (myCluster != null)
				{
					return AddObjectToCluster(myCluster, num, entityId, batch);
				}
				return num;
			}
		}

		private ulong AddObjectToCluster(MyCluster cluster, ulong objectId, long entityId, bool batch, bool fireEvent = true)
		{
			cluster.Objects.Add(objectId);
			MyObjectData myObjectData = m_objectsData[objectId];
			m_objectsData[objectId].Id = objectId;
			m_objectsData[objectId].Cluster = cluster;
			if (batch)
			{
				if (myObjectData.ActivationHandler != null)
				{
					myObjectData.ActivationHandler.ActivateBatch(cluster.UserData, objectId);
				}
			}
			else if (myObjectData.ActivationHandler != null)
			{
				myObjectData.ActivationHandler.Activate(cluster.UserData, objectId);
			}
			if (fireEvent)
			{
				EntityAdded?.Invoke(entityId, cluster.ClusterId);
			}
			return objectId;
		}

		private MyCluster CreateCluster(ref BoundingBoxD clusterBB)
		{
			MyCluster myCluster = new MyCluster
			{
				AABB = clusterBB,
				Objects = new HashSet<ulong>()
			};
			myCluster.ClusterId = m_clusterTree.AddProxy(ref myCluster.AABB, myCluster, 0u);
			if (OnClusterCreated != null)
			{
				myCluster.UserData = OnClusterCreated(myCluster.ClusterId, myCluster.AABB);
			}
			OnAfterClusterCreated?.Invoke(myCluster.ClusterId, myCluster.AABB);
			m_clusters.Add(myCluster);
			m_userObjects.Add(myCluster.UserData);
			return myCluster;
		}

		public static BoundingBoxD AdjustAABBByVelocity(BoundingBoxD aabb, Vector3 velocity, float inflate = 1.1f)
		{
			if (velocity.LengthSquared() > 0.001f)
			{
				velocity.Normalize();
			}
			aabb.Inflate(inflate);
			BoundingBoxD box = aabb;
			box += (Vector3D)(velocity * 10f * inflate);
			aabb.Include(box);
			return aabb;
		}

		public void MoveObject(ulong id, BoundingBoxD aabb, Vector3 velocity)
		{
			using (m_clustersLock.AcquireExclusiveUsing())
			{
				if (!m_objectsData.TryGetValue(id, out var value))
				{
					return;
				}
				BoundingBoxD aABB = value.AABB;
				value.AABB = aabb;
				if (m_suppressClusterReorder)
				{
					return;
				}
				aabb = AdjustAABBByVelocity(aabb, velocity, 0f);
				ContainmentType containmentType = value.Cluster.AABB.Contains(aabb);
				if (containmentType == ContainmentType.Contains || SingleCluster.HasValue || ForcedClusters)
				{
					return;
				}
				if (containmentType == ContainmentType.Disjoint)
				{
					m_clusterTree.OverlapAllBoundingBox(ref aabb, m_returnedClusters);
					if (m_returnedClusters.Count == 1 && m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
					{
						MyCluster cluster = value.Cluster;
						RemoveObjectFromCluster(value, batch: false);
<<<<<<< HEAD
						if (cluster.Objects.Count == 0)
=======
						if (cluster.Objects.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							RemoveCluster(cluster);
						}
						AddObjectToCluster(m_returnedClusters[0], value.Id, value.EntityId, batch: false);
					}
					else
					{
						aabb.InflateToMinimum(IdealClusterSize);
						ReorderClusters(aabb.Include(aABB), id);
					}
				}
				else
				{
					aabb.InflateToMinimum(IdealClusterSize);
					ReorderClusters(aabb.Include(aABB), id);
				}
			}
		}

		public void EnsureClusterSpace(BoundingBoxD aabb)
		{
			if (SingleCluster.HasValue || ForcedClusters)
<<<<<<< HEAD
			{
				return;
			}
			using (m_clustersLock.AcquireExclusiveUsing())
			{
=======
			{
				return;
			}
			using (m_clustersLock.AcquireExclusiveUsing())
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_clusterTree.OverlapAllBoundingBox(ref aabb, m_returnedClusters);
				bool flag = true;
				if (m_returnedClusters.Count == 1 && m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
				{
					flag = false;
				}
				if (flag)
				{
					ulong num = m_clusterObjectCounter++;
					int staticId = -1;
					m_objectsData[num] = new MyObjectData
					{
						Id = num,
						Cluster = null,
						ActivationHandler = null,
						AABB = aabb,
						StaticId = staticId
					};
					ReorderClusters(aabb, num);
					RemoveObjectFromCluster(m_objectsData[num], batch: false);
					m_objectsData.Remove(num);
				}
			}
		}

		public void RemoveObject(ulong id)
		{
			if (!m_objectsData.TryGetValue(id, out var value))
			{
				return;
			}
			MyCluster cluster = value.Cluster;
			if (cluster != null)
			{
				RemoveObjectFromCluster(value, batch: false);
				if (cluster.Objects.get_Count() == 0)
				{
					RemoveCluster(cluster);
				}
			}
			if (value.StaticId != -1)
			{
				m_staticTree.RemoveProxy(value.StaticId);
				value.StaticId = -1;
			}
			m_objectsData.Remove(id);
		}

		private void RemoveObjectFromCluster(MyObjectData objectData, bool batch, bool fireEvent = true)
		{
			objectData.Cluster.Objects.Remove(objectData.Id);
			int clusterId = objectData.Cluster.ClusterId;
			if (batch)
			{
				if (objectData.ActivationHandler != null)
				{
					objectData.ActivationHandler.DeactivateBatch(objectData.Cluster.UserData);
				}
			}
			else
<<<<<<< HEAD
			{
				if (objectData.ActivationHandler != null)
				{
					objectData.ActivationHandler.Deactivate(objectData.Cluster.UserData);
				}
				m_objectsData[objectData.Id].Cluster = null;
			}
			if (fireEvent)
			{
=======
			{
				if (objectData.ActivationHandler != null)
				{
					objectData.ActivationHandler.Deactivate(objectData.Cluster.UserData);
				}
				m_objectsData[objectData.Id].Cluster = null;
			}
			if (fireEvent)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				EntityRemoved?.Invoke(objectData.EntityId, clusterId);
			}
		}

		private void RemoveCluster(MyCluster cluster)
		{
			m_clusterTree.RemoveProxy(cluster.ClusterId);
			m_clusters.Remove(cluster);
			m_userObjects.Remove(cluster.UserData);
			if (OnClusterRemoved != null)
			{
				OnClusterRemoved(cluster.UserData, cluster.ClusterId);
			}
		}

		public Vector3D GetObjectOffset(ulong id)
		{
			if (m_objectsData.TryGetValue(id, out var value))
			{
				if (value.Cluster == null)
				{
					return Vector3D.Zero;
				}
				return value.Cluster.AABB.Center;
			}
			return Vector3D.Zero;
		}

		public MyCluster GetClusterForPosition(Vector3D pos)
		{
			BoundingSphereD sphere = new BoundingSphereD(pos, 1.0);
			m_clusterTree.OverlapAllBoundingSphere(ref sphere, m_returnedClusters);
			if (m_returnedClusters.Count <= 0)
			{
				return null;
			}
			return Enumerable.Single<MyCluster>((IEnumerable<MyCluster>)m_returnedClusters);
		}

		public void Dispose()
		{
			foreach (MyCluster cluster in m_clusters)
			{
				if (OnClusterRemoved != null)
				{
					OnClusterRemoved(cluster.UserData, cluster.ClusterId);
				}
			}
			m_clusters.Clear();
			m_userObjects.Clear();
			m_clusterTree.Clear();
			m_objectsData.Clear();
			m_staticTree.Clear();
			m_clusterObjectCounter = 0uL;
		}

		public ListReader<object> GetList()
		{
			return new ListReader<object>(m_userObjects);
		}

		public ListReader<object> GetListCopy()
		{
			return new ListReader<object>(new List<object>(m_userObjects));
		}

		public ListReader<MyCluster> GetClusters()
		{
			return m_clusters;
		}

		public void CastRay(Vector3D from, Vector3D to, List<MyClusterQueryResult> results)
		{
			if (m_clusterTree == null || results == null)
<<<<<<< HEAD
			{
				return;
			}
			LineD line = new LineD(from, to);
			m_clusterTree.OverlapAllLineSegment(ref line, m_lineResultList);
			foreach (MyLineSegmentOverlapResult<MyCluster> lineResult in m_lineResultList)
			{
=======
			{
				return;
			}
			LineD line = new LineD(from, to);
			m_clusterTree.OverlapAllLineSegment(ref line, m_lineResultList);
			foreach (MyLineSegmentOverlapResult<MyCluster> lineResult in m_lineResultList)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (lineResult.Element != null)
				{
					results.Add(new MyClusterQueryResult
					{
						AABB = lineResult.Element.AABB,
						UserData = lineResult.Element.UserData
					});
				}
			}
			m_lineResultList.Clear();
		}

		public void Intersects(Vector3D translation, List<MyClusterQueryResult> results)
		{
			BoundingBoxD bbox = new BoundingBoxD(translation - new Vector3D(1.0), translation + new Vector3D(1.0));
			m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
			foreach (MyCluster result in m_resultList)
			{
				results.Add(new MyClusterQueryResult
				{
					AABB = result.AABB,
					UserData = result.UserData
				});
			}
			m_resultList.Clear();
		}

		public void GetAll(List<MyClusterQueryResult> results)
		{
			m_clusterTree.GetAll(m_resultList, clear: true);
			foreach (MyCluster result in m_resultList)
			{
				results.Add(new MyClusterQueryResult
				{
					AABB = result.AABB,
					UserData = result.UserData
				});
			}
			m_resultList.Clear();
		}

		public void ReorderClusters(BoundingBoxD aabb, ulong objectId = ulong.MaxValue)
		{
			//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
			using (m_clustersReorderLock.AcquireExclusiveUsing())
			{
				BoundingBoxD bbox = aabb.GetInflated(IdealClusterSize / 2f);
				bbox.InflateToMinimum(IdealClusterSize);
				m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
				HashSet<MyObjectData> val = new HashSet<MyObjectData>();
				bool flag = false;
				while (!flag)
				{
					val.Clear();
					if (objectId != ulong.MaxValue)
					{
						val.Add(m_objectsData[objectId]);
					}
					foreach (MyCluster collidedCluster in m_resultList)
					{
						foreach (MyObjectData item in Enumerable.Select<KeyValuePair<ulong, MyObjectData>, MyObjectData>(Enumerable.Where<KeyValuePair<ulong, MyObjectData>>((IEnumerable<KeyValuePair<ulong, MyObjectData>>)m_objectsData, (Func<KeyValuePair<ulong, MyObjectData>, bool>)((KeyValuePair<ulong, MyObjectData> x) => collidedCluster.Objects.Contains(x.Key))), (Func<KeyValuePair<ulong, MyObjectData>, MyObjectData>)((KeyValuePair<ulong, MyObjectData> x) => x.Value)))
						{
							val.Add(item);
							bbox.Include(item.AABB.GetInflated(IdealClusterSize / 2f));
						}
					}
					int count = m_resultList.Count;
					m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
					flag = count == m_resultList.Count;
					m_staticTree.OverlapAllBoundingBox(ref bbox, m_objectDataResultList);
					foreach (ulong objectDataResult in m_objectDataResultList)
					{
						if (m_objectsData[objectDataResult].Cluster != null && !m_resultList.Contains(m_objectsData[objectDataResult].Cluster))
						{
							bbox.Include(m_objectsData[objectDataResult].AABB.GetInflated(IdealClusterSize / 2f));
							flag = false;
						}
					}
				}
				m_staticTree.OverlapAllBoundingBox(ref bbox, m_objectDataResultList);
				foreach (ulong objectDataResult2 in m_objectDataResultList)
				{
					val.Add(m_objectsData[objectDataResult2]);
				}
				int num = 8;
				bool flag2 = true;
				Stack<MyClusterDescription> val2 = new Stack<MyClusterDescription>();
				List<MyClusterDescription> list = new List<MyClusterDescription>();
				List<MyObjectData> list2 = null;
				Vector3 idealClusterSize = IdealClusterSize;
				while (num > 0 && flag2)
				{
					val2.Clear();
					list.Clear();
					MyClusterDescription myClusterDescription = default(MyClusterDescription);
					myClusterDescription.AABB = bbox;
					myClusterDescription.DynamicObjects = Enumerable.ToList<MyObjectData>(Enumerable.Where<MyObjectData>((IEnumerable<MyObjectData>)val, (Func<MyObjectData, bool>)((MyObjectData x) => x.ActivationHandler == null || !x.ActivationHandler.IsStaticForCluster)));
					myClusterDescription.StaticObjects = Enumerable.ToList<MyObjectData>(Enumerable.Where<MyObjectData>((IEnumerable<MyObjectData>)val, (Func<MyObjectData, bool>)((MyObjectData x) => x.ActivationHandler != null && x.ActivationHandler.IsStaticForCluster)));
					MyClusterDescription myClusterDescription2 = myClusterDescription;
					val2.Push(myClusterDescription2);
					list2 = Enumerable.ToList<MyObjectData>(Enumerable.Where<MyObjectData>((IEnumerable<MyObjectData>)myClusterDescription2.StaticObjects, (Func<MyObjectData, bool>)((MyObjectData x) => x.Cluster != null)));
					_ = myClusterDescription2.StaticObjects.Count;
					while (val2.get_Count() > 0)
					{
<<<<<<< HEAD
						MyClusterDescription myClusterDescription2 = stack.Pop();
						if (myClusterDescription2.DynamicObjects.Count == 0)
=======
						MyClusterDescription myClusterDescription3 = val2.Pop();
						if (myClusterDescription3.DynamicObjects.Count == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							continue;
						}
						BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
<<<<<<< HEAD
						for (int i = 0; i < myClusterDescription2.DynamicObjects.Count; i++)
						{
							BoundingBoxD inflated = myClusterDescription2.DynamicObjects[i].AABB.GetInflated(idealClusterSize / 2f);
=======
						for (int i = 0; i < myClusterDescription3.DynamicObjects.Count; i++)
						{
							BoundingBoxD inflated = myClusterDescription3.DynamicObjects[i].AABB.GetInflated(idealClusterSize / 2f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							boundingBoxD.Include(inflated);
						}
						BoundingBoxD aABB = boundingBoxD;
						int num2 = boundingBoxD.Size.AbsMaxComponent();
						switch (num2)
						{
						case 0:
<<<<<<< HEAD
							myClusterDescription2.DynamicObjects.Sort(AABBComparerX.Static);
							break;
						case 1:
							myClusterDescription2.DynamicObjects.Sort(AABBComparerY.Static);
							break;
						case 2:
							myClusterDescription2.DynamicObjects.Sort(AABBComparerZ.Static);
=======
							myClusterDescription3.DynamicObjects.Sort(AABBComparerX.Static);
							break;
						case 1:
							myClusterDescription3.DynamicObjects.Sort(AABBComparerY.Static);
							break;
						case 2:
							myClusterDescription3.DynamicObjects.Sort(AABBComparerZ.Static);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							break;
						}
						bool flag3 = false;
						if (boundingBoxD.Size.AbsMax() >= (double)MaximumForSplit.AbsMax())
						{
							BoundingBoxD boundingBoxD2 = BoundingBoxD.CreateInvalid();
							double num3 = double.MinValue;
<<<<<<< HEAD
							for (int j = 1; j < myClusterDescription2.DynamicObjects.Count; j++)
							{
								MyObjectData myObjectData = myClusterDescription2.DynamicObjects[j - 1];
								MyObjectData myObjectData2 = myClusterDescription2.DynamicObjects[j];
=======
							for (int j = 1; j < myClusterDescription3.DynamicObjects.Count; j++)
							{
								MyObjectData myObjectData = myClusterDescription3.DynamicObjects[j - 1];
								MyObjectData myObjectData2 = myClusterDescription3.DynamicObjects[j];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								BoundingBoxD inflated2 = myObjectData.AABB.GetInflated(idealClusterSize / 2f);
								BoundingBoxD inflated3 = myObjectData2.AABB.GetInflated(idealClusterSize / 2f);
								double dim = inflated2.Max.GetDim(num2);
								if (dim > num3)
								{
									num3 = dim;
								}
								boundingBoxD2.Include(inflated2);
								double dim2 = inflated3.Min.GetDim(num2);
								double dim3 = inflated2.Max.GetDim(num2);
								if (dim2 - dim3 > 0.0 && num3 <= dim2)
								{
									flag3 = true;
									aABB = boundingBoxD2;
									break;
								}
							}
						}
						aABB.InflateToMinimum(idealClusterSize);
						myClusterDescription = default(MyClusterDescription);
						myClusterDescription.AABB = aABB;
						myClusterDescription.DynamicObjects = new List<MyObjectData>();
						myClusterDescription.StaticObjects = new List<MyObjectData>();
<<<<<<< HEAD
						MyClusterDescription item2 = myClusterDescription;
						foreach (MyObjectData item5 in myClusterDescription2.DynamicObjects.ToList())
						{
							if (aABB.Contains(item5.AABB) == ContainmentType.Contains)
							{
								item2.DynamicObjects.Add(item5);
								myClusterDescription2.DynamicObjects.Remove(item5);
							}
						}
						foreach (MyObjectData item6 in myClusterDescription2.StaticObjects.ToList())
						{
							ContainmentType containmentType = aABB.Contains(item6.AABB);
							if (containmentType == ContainmentType.Contains || containmentType == ContainmentType.Intersects)
							{
								item2.StaticObjects.Add(item6);
								myClusterDescription2.StaticObjects.Remove(item6);
							}
						}
						if (myClusterDescription2.DynamicObjects.Count > 0)
						{
							BoundingBoxD aABB2 = BoundingBoxD.CreateInvalid();
							foreach (MyObjectData dynamicObject in myClusterDescription2.DynamicObjects)
=======
						MyClusterDescription myClusterDescription4 = myClusterDescription;
						foreach (MyObjectData item2 in Enumerable.ToList<MyObjectData>((IEnumerable<MyObjectData>)myClusterDescription3.DynamicObjects))
						{
							if (aABB.Contains(item2.AABB) == ContainmentType.Contains)
							{
								myClusterDescription4.DynamicObjects.Add(item2);
								myClusterDescription3.DynamicObjects.Remove(item2);
							}
						}
						foreach (MyObjectData item3 in Enumerable.ToList<MyObjectData>((IEnumerable<MyObjectData>)myClusterDescription3.StaticObjects))
						{
							ContainmentType containmentType = aABB.Contains(item3.AABB);
							if (containmentType == ContainmentType.Contains || containmentType == ContainmentType.Intersects)
							{
								myClusterDescription4.StaticObjects.Add(item3);
								myClusterDescription3.StaticObjects.Remove(item3);
							}
						}
						if (myClusterDescription3.DynamicObjects.Count > 0)
						{
							BoundingBoxD aABB2 = BoundingBoxD.CreateInvalid();
							foreach (MyObjectData dynamicObject in myClusterDescription3.DynamicObjects)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								aABB2.Include(dynamicObject.AABB.GetInflated(idealClusterSize / 2f));
							}
							aABB2.InflateToMinimum(idealClusterSize);
							myClusterDescription = default(MyClusterDescription);
							myClusterDescription.AABB = aABB2;
<<<<<<< HEAD
							myClusterDescription.DynamicObjects = myClusterDescription2.DynamicObjects.ToList();
							myClusterDescription.StaticObjects = myClusterDescription2.StaticObjects.ToList();
							MyClusterDescription item3 = myClusterDescription;
							if (item3.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()))
							{
								stack.Push(item3);
							}
							else
							{
								list.Add(item3);
							}
						}
						if (item2.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()) && flag3)
						{
							stack.Push(item2);
						}
						else
						{
							list.Add(item2);
=======
							myClusterDescription.DynamicObjects = Enumerable.ToList<MyObjectData>((IEnumerable<MyObjectData>)myClusterDescription3.DynamicObjects);
							myClusterDescription.StaticObjects = Enumerable.ToList<MyObjectData>((IEnumerable<MyObjectData>)myClusterDescription3.StaticObjects);
							MyClusterDescription myClusterDescription5 = myClusterDescription;
							if (myClusterDescription5.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()))
							{
								val2.Push(myClusterDescription5);
							}
							else
							{
								list.Add(myClusterDescription5);
							}
						}
						if (myClusterDescription4.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()) && flag3)
						{
							val2.Push(myClusterDescription4);
						}
						else
						{
							list.Add(myClusterDescription4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					flag2 = false;
					foreach (MyClusterDescription item4 in list)
					{
<<<<<<< HEAD
						if (item7.AABB.Size.AbsMax() > (double)MaximumClusterSize)
=======
						if (item4.AABB.Size.AbsMax() > (double)MaximumClusterSize)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							flag2 = true;
							idealClusterSize /= 2f;
							break;
						}
					}
					if (flag2)
					{
						num--;
					}
				}
				HashSet<MyCluster> val3 = new HashSet<MyCluster>();
				HashSet<MyCluster> val4 = new HashSet<MyCluster>();
				foreach (MyObjectData item5 in list2)
				{
					if (item5.Cluster != null)
					{
<<<<<<< HEAD
						hashSet2.Add(item8.Cluster);
						RemoveObjectFromCluster(item8, batch: true, fireEvent: false);
=======
						val3.Add(item5.Cluster);
						RemoveObjectFromCluster(item5, batch: true, fireEvent: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				foreach (MyObjectData item6 in list2)
				{
					if (item6.Cluster != null)
					{
						item6.ActivationHandler.FinishRemoveBatch(item6.Cluster.UserData);
						item6.Cluster = null;
					}
				}
				int num4 = 0;
				Enumerator<MyCluster> enumerator6;
				foreach (MyClusterDescription item7 in list)
				{
					BoundingBoxD clusterBB = item7.AABB;
					MyCluster myCluster = CreateCluster(ref clusterBB);
					foreach (MyObjectData dynamicObject2 in item7.DynamicObjects)
					{
						if (dynamicObject2.Cluster != null)
						{
<<<<<<< HEAD
							hashSet2.Add(dynamicObject2.Cluster);
=======
							val3.Add(dynamicObject2.Cluster);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							RemoveObjectFromCluster(dynamicObject2, batch: true, fireEvent: false);
						}
					}
					foreach (MyObjectData dynamicObject3 in item7.DynamicObjects)
					{
						if (dynamicObject3.Cluster != null)
						{
							dynamicObject3.ActivationHandler.FinishRemoveBatch(dynamicObject3.Cluster.UserData);
							dynamicObject3.Cluster = null;
						}
					}
					enumerator6 = val3.GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							MyCluster current12 = enumerator6.get_Current();
							if (OnFinishBatch != null)
							{
								OnFinishBatch(current12.UserData);
							}
						}
					}
					finally
					{
						((IDisposable)enumerator6).Dispose();
					}
					foreach (MyObjectData dynamicObject4 in item7.DynamicObjects)
					{
						AddObjectToCluster(myCluster, dynamicObject4.Id, dynamicObject4.EntityId, batch: true, fireEvent: false);
						EntityAdded?.Invoke(dynamicObject4.EntityId, myCluster.ClusterId);
					}
					foreach (MyObjectData staticObject in item7.StaticObjects)
					{
						if (myCluster.AABB.Contains(staticObject.AABB) != 0)
						{
							AddObjectToCluster(myCluster, staticObject.Id, staticObject.EntityId, batch: true, fireEvent: false);
							EntityAdded?.Invoke(staticObject.EntityId, myCluster.ClusterId);
							num4++;
						}
					}
					val4.Add(myCluster);
				}
				enumerator6 = val3.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						MyCluster current15 = enumerator6.get_Current();
						RemoveCluster(current15);
					}
				}
				finally
				{
					((IDisposable)enumerator6).Dispose();
				}
				enumerator6 = val4.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						MyCluster current16 = enumerator6.get_Current();
						if (OnFinishBatch != null)
						{
							OnFinishBatch(current16.UserData);
						}
						Enumerator<ulong> enumerator7 = current16.Objects.GetEnumerator();
						try
						{
							while (enumerator7.MoveNext())
							{
								ulong current17 = enumerator7.get_Current();
								if (m_objectsData[current17].ActivationHandler != null)
								{
									m_objectsData[current17].ActivationHandler.FinishAddBatch();
								}
							}
						}
						finally
						{
							((IDisposable)enumerator7).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator6).Dispose();
				}
				if (OnClustersReordered != null)
				{
					OnClustersReordered();
				}
			}
			m_resultList.Clear();
		}

		public void GetAllStaticObjects(List<BoundingBoxD> staticObjects)
		{
			m_staticTree.GetAll(m_objectDataResultList, clear: true);
			staticObjects.Clear();
			foreach (ulong objectDataResult in m_objectDataResultList)
			{
				staticObjects.Add(m_objectsData[objectDataResult].AABB);
			}
		}

		public void Serialize(List<BoundingBoxD> list)
		{
			foreach (MyCluster cluster in m_clusters)
			{
				list.Add(cluster.AABB);
			}
		}

		public void Deserialize(List<BoundingBoxD> list)
		{
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			foreach (MyObjectData value in m_objectsData.Values)
			{
				if (value.Cluster != null)
				{
					RemoveObjectFromCluster(value, batch: true);
				}
			}
			foreach (MyObjectData value2 in m_objectsData.Values)
			{
				if (value2.Cluster != null)
				{
					value2.ActivationHandler.FinishRemoveBatch(value2.Cluster.UserData);
					value2.Cluster = null;
				}
			}
			foreach (MyCluster cluster in m_clusters)
			{
				if (OnFinishBatch != null)
				{
					OnFinishBatch(cluster.UserData);
				}
			}
			while (m_clusters.Count > 0)
			{
				RemoveCluster(m_clusters[0]);
			}
			for (int i = 0; i < list.Count; i++)
			{
				BoundingBoxD clusterBB = list[i];
				CreateCluster(ref clusterBB);
			}
			foreach (KeyValuePair<ulong, MyObjectData> objectsDatum in m_objectsData)
			{
				m_clusterTree.OverlapAllBoundingBox(ref objectsDatum.Value.AABB, m_returnedClusters);
				if (m_returnedClusters.Count != 1 && !objectsDatum.Value.ActivationHandler.IsStaticForCluster)
				{
					throw new Exception($"Inconsistent objects and deserialized clusters. Entity name: {objectsDatum.Value.Tag}, Found clusters: {m_returnedClusters.Count}, Replicable exists: {GetEntityReplicableExistsById(objectsDatum.Value.EntityId)}");
				}
				if (m_returnedClusters.Count == 1)
				{
					AddObjectToCluster(m_returnedClusters[0], objectsDatum.Key, objectsDatum.Value.EntityId, batch: true);
				}
			}
			foreach (MyCluster cluster2 in m_clusters)
			{
				if (OnFinishBatch != null)
				{
					OnFinishBatch(cluster2.UserData);
				}
				Enumerator<ulong> enumerator4 = cluster2.Objects.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						ulong current6 = enumerator4.get_Current();
						if (m_objectsData[current6].ActivationHandler != null)
						{
							m_objectsData[current6].ActivationHandler.FinishAddBatch();
						}
					}
				}
				finally
				{
					((IDisposable)enumerator4).Dispose();
				}
			}
		}
	}
}
