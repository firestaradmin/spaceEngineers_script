using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Scene;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Culling.Frustum
{
	[PooledObject(2)]
	internal sealed class MyFrustumCullingWork : IPooledObject
	{
		private interface CullGroupOp
		{
			ContainmentType Intersect(ref MyCullFrustum cullFrustum, ref MyCullAABB aabb);

			void CullTree(MyDynamicAABBTree tree, BoundingFrustum frustum, ref CullTreeOp cullOp);
		}

		private struct CullShadowTree : CullGroupOp
		{
			public readonly float m_tSqr;

			public CullShadowTree(float sqr)
			{
				m_tSqr = sqr;
			}

			public ContainmentType Intersect(ref MyCullFrustum cullFrustum, ref MyCullAABB aabb)
			{
				if (aabb.LengthSquared > m_tSqr)
				{
					return cullFrustum.Intersects(aabb);
				}
				return ContainmentType.Disjoint;
			}

			public void CullTree(MyDynamicAABBTree tree, BoundingFrustum frustum, ref CullTreeOp cullOp)
			{
				tree.OverlapAllFrustum<Action<MyCullResults, bool>, CullTreeOp>(ref frustum, m_tSqr, ref cullOp);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CullDeepTree : CullGroupOp
		{
			public ContainmentType Intersect(ref MyCullFrustum cullFrustum, ref MyCullAABB aabb)
			{
				if (cullFrustum.Contains(aabb))
				{
					return ContainmentType.Contains;
				}
				return ContainmentType.Disjoint;
			}

			public void CullTree(MyDynamicAABBTree tree, BoundingFrustum frustum, ref CullTreeOp cullOp)
			{
				tree.OverlapAllFrustum<Action<MyCullResults, bool>, CullTreeOp>(ref frustum, ref cullOp);
			}
		}

<<<<<<< HEAD
		private struct CullTreeOp : IAddOp<Action<MyCullResults, bool>>
=======
		private struct CullTreeOp : AddOp<Action<MyCullResults, bool>>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			public int Count;

			public MyCullResults Results;

			public void Add(Action<MyCullResults, bool> userData, bool contained)
			{
				Count++;
				userData(Results, contained);
			}
		}

		private MyDynamicAABBTreeD m_renderables;

		private MyDynamicAABBTreeD m_renderablesFar;

		private MyDynamicAABBTreeD m_groups;

		private MyCullQuery m_query;

		private readonly List<bool> m_groupContainedList = new List<bool>();

		private readonly BoundingFrustum m_dummyFrustum = new BoundingFrustum();

		internal long Elapsed { get; private set; }

		internal void Init(MyCullQuery query, MyDynamicAABBTreeD renderables, MyDynamicAABBTreeD renderableFar, MyDynamicAABBTreeD groups)
		{
			m_query = query;
			m_renderables = renderables;
			m_renderablesFar = renderableFar;
			m_groups = groups;
		}

		void IPooledObject.Cleanup()
		{
			m_query = null;
			m_renderables = null;
		}

		public static int ExecuteCulling(MyCullQuery cullQuery)
		{
			cullQuery.CullWork.DoWork();
			return cullQuery.Results.GetCounts();
		}

		public void DoWork()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (m_query.SmallObjects.HasValue)
			{
				if (MyRender11.Settings.DrawNonMergeInstanced)
				{
					Cull(m_renderables, m_renderablesFar, m_groups, delegate(BoundingFrustumD frustum, MyDynamicAABBTreeD tree)
					{
						tree.OverlapAllFrustum(ref frustum, (MyCullResultsBase)m_query.Results, m_query.SmallObjects.Value.TSqr);
					}, new CullShadowTree(m_query.SmallObjects.Value.TSqr));
				}
				if (MyRender11.Settings.DrawMergeInstanced)
				{
					BoundingFrustumD frustum2 = m_query.Frustum;
					MyScene11.MergeGroupsDBVH.OverlapAllFrustum(ref frustum2, (MyCullResultsBase)m_query.Results, m_query.SmallObjects.Value.TSqr);
				}
			}
			else
			{
				if (MyRender11.Settings.DrawNonMergeInstanced)
				{
					Cull(m_renderables, m_renderablesFar, m_groups, delegate(BoundingFrustumD frustum, MyDynamicAABBTreeD tree)
					{
						tree.OverlapAllFrustum(ref frustum, (MyCullResultsBase)m_query.Results);
					}, default(CullDeepTree));
				}
				if (MyRender11.Settings.DrawMergeInstanced)
				{
					BoundingFrustumD frustum3 = m_query.Frustum;
					MyScene11.MergeGroupsDBVH.OverlapAllFrustum(ref frustum3, (MyCullResultsBase)m_query.Results);
				}
			}
<<<<<<< HEAD
			if (MyRender11.DebugOverrides.DisableEnvironmentLight && m_query.ViewType == MyViewType.EnvironmentProbe)
			{
				m_query.Results.Clear();
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Elapsed = Stopwatch.GetTimestamp() - timestamp;
		}

		private static void CullGroup<T>(T cullOp, BoundingFrustum frustum, MyManualCullTreeData data, MyCullQuery query) where T : CullGroupOp
		{
			int count = data.BruteCull.Count;
			bool flag = count <= MyRender11.Settings.CullGroupsThreshold;
			CullTreeOp cullTreeOp;
			if (MyRender11.Settings.UseIncrementalCulling)
			{
				MyCullFrustum cullFrustum = new MyCullFrustum(frustum);
				CullData cullData = data.RenderCullData[query.ViewId];
				if (flag || (float)cullData.ActiveActorsLastFrame >= (float)count * MyRender11.Settings.IncrementalCullingTreeFallbackThreshold)
				{
					List<MyBruteCullData> activeActors = cullData.ActiveActors;
					List<MyBruteCullData> culledActors = cullData.CulledActors;
					MyCullResults myCullResults = (MyCullResults)cullData.ActiveResults;
					int count2 = activeActors.Count;
					for (int i = 0; i < culledActors.Count; i++)
					{
						MyBruteCullData item = culledActors[i];
						ContainmentType containmentType = cullOp.Intersect(ref cullFrustum, ref item.Aabb);
						if (containmentType != 0)
						{
							item.UserData.Add(myCullResults, containmentType == ContainmentType.Contains);
							culledActors.RemoveAtFast(i);
							activeActors.Add(item);
							i--;
						}
					}
					int num = ((count2 >= MyRender11.Settings.IncrementalCullFrames) ? (count2 / MyRender11.Settings.IncrementalCullFrames) : count2);
					int num2 = cullData.IterationOffset;
					if (num2 >= count2)
					{
						num2 = 0;
					}
					int num3 = Math.Min(count2, num2 + num);
					for (int j = num2; j < num3; j++)
					{
						MyBruteCullData item2 = activeActors[j];
						if (cullOp.Intersect(ref cullFrustum, ref item2.Aabb) == ContainmentType.Disjoint)
						{
							item2.UserData.Remove(myCullResults);
							cullData.CulledActors.Add(item2);
							activeActors.RemoveAtFast(j);
							num3--;
							j--;
						}
					}
					data.RenderCullData[query.ViewId].IterationOffset = num3;
					data.RenderCullData[query.ViewId].ActiveActorsLastFrame = activeActors.Count;
					query.Results.Append(myCullResults);
				}
				else
				{
					cullTreeOp = default(CullTreeOp);
					cullTreeOp.Count = 0;
					cullTreeOp.Results = query.Results;
					CullTreeOp cullOp2 = cullTreeOp;
					cullOp.CullTree(data.Children, frustum, ref cullOp2);
					data.RenderCullData[query.ViewId].ActiveActorsLastFrame = cullOp2.Count;
				}
			}
			else if (flag)
			{
				MyCullFrustum cullFrustum2 = new MyCullFrustum(frustum);
				foreach (MyBruteCullData value in data.BruteCull.Values)
				{
					MyCullAABB aabb = value.Aabb;
					ContainmentType containmentType2 = cullOp.Intersect(ref cullFrustum2, ref aabb);
					if (containmentType2 != 0)
					{
						value.UserData.Add(query.Results, containmentType2 == ContainmentType.Contains);
					}
				}
			}
			else
			{
				cullTreeOp = default(CullTreeOp);
				cullTreeOp.Count = 0;
				cullTreeOp.Results = query.Results;
				CullTreeOp cullOp3 = cullTreeOp;
				cullOp.CullTree(data.Children, frustum, ref cullOp3);
			}
		}

		private void Cull<T>(MyDynamicAABBTreeD renderables, MyDynamicAABBTreeD renderablesFar, MyDynamicAABBTreeD groups, Action<BoundingFrustumD, MyDynamicAABBTreeD> cullAction, T cullGroupOp) where T : struct, CullGroupOp
		{
			MyCullQuery query = m_query;
			BoundingFrustumD frustum = query.Frustum;
			int count = query.Results.CullProxies.Count;
			int count2 = query.Results.Instances.Count;
			cullAction(frustum, renderables);
			cullAction(query.FrustumFar, renderablesFar);
			query.RootObjectsCount += query.Results.CullProxies.Count - count;
			query.RootInstancesCount += query.Results.Instances.Count - count2;
			if (!MyRender11.Settings.DrawGroups || groups == null)
			{
				return;
			}
			_ = m_query.ViewId;
			List<MyManualCullTreeData> groups2 = query.Groups;
			groups.OverlapAllFrustum(ref frustum, groups2, m_groupContainedList);
			MyCullResults results = query.Results;
			for (int i = 0; i < groups2.Count; i++)
			{
				int count3 = results.PointLights.Count;
				MyManualCullTreeData myManualCullTreeData = groups2[i];
				MyActor actor = myManualCullTreeData.Actor;
				if (!actor.IsOccluded(query.ViewId))
				{
					query.GroupsCount++;
					if (m_groupContainedList[i])
					{
						results.Append((MyCullResults)myManualCullTreeData.All);
					}
					else
					{
						MatrixD m = actor.WorldMatrix * frustum.Matrix;
						m_dummyFrustum.Matrix = m;
						CullGroup(cullGroupOp, m_dummyFrustum, myManualCullTreeData, query);
					}
				}
				int num = results.PointLights.Count - count3;
				if (num > 0)
				{
					query.PointLightsInfo.Add(new MyCullQuery.MyPointLightsInfo
					{
						GroupsIndex = i,
						Start = count3,
						Count = num
					});
				}
			}
		}
	}
}
