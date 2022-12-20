using System;
using System.Collections.Generic;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public sealed class MyTargetingHelper
	{
		public static MyTargetingHelper m_instance = new MyTargetingHelper();

		private List<MyCubeGrid> m_connectedMy = new List<MyCubeGrid>();

		private List<MyCubeGrid> m_connectedTarget = new List<MyCubeGrid>();

		private List<MyLineSegmentOverlapResult<MyVoxelBase>> m_entityRaycastResult;

		public static MyTargetingHelper Instance => m_instance;

		private MyTargetingHelper()
		{
		}

		public Vector3D GetLockingPosition(MyCubeGrid grid)
		{
			if (!grid.IsStatic)
			{
				return MyGridPhysicalGroupData.GetGroupSharedProperties(grid).CoMWorld;
			}
			return grid.GetPhysicalGroupAABB().Center;
		}

		public void RaycastCheck(MyCubeBlock myCockpit, MyCubeGrid grid, Action<MyCubeGrid, List<MyPhysics.HitInfo>> callback)
		{
			Vector3D from = GetLockingPosition(grid);
			Vector3D to = myCockpit.WorldMatrix.Translation;
			if (MyFakes.ENABLE_TARGET_LOCKING_RAYCAST_VOXEL_PHYSICS_PREFETCH && PrefetchVoxels(to, from))
			{
				return;
			}
			MyCubeGrid target = grid;
			MyPhysics.CastRayParallel(ref from, ref to, new List<MyPhysics.HitInfo>(), 15, delegate(List<MyPhysics.HitInfo> list)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					callback(target, list);
				}, "Raycast locking result");
			});
		}

		public bool IsVisible(MyCubeGrid myGrid, MyCubeGrid target, List<MyPhysics.HitInfo> hits)
		{
			if (hits.Count == 0)
			{
				return true;
			}
			using (MyUtils.ReuseCollection(ref m_connectedMy))
			{
				using (MyUtils.ReuseCollection(ref m_connectedTarget))
				{
					myGrid.GetConnectedGrids(GridLinkTypeEnum.Physical, m_connectedMy);
					target.GetConnectedGrids(GridLinkTypeEnum.Physical, m_connectedTarget);
					foreach (MyPhysics.HitInfo hit in hits)
					{
						IMyEntity myEntity = hit.HkHitInfo.GetHitEntity();
						if (myEntity.Physics?.IsPhantom ?? false)
						{
							continue;
						}
						if (myEntity is MyEntitySubpart)
						{
							myEntity = myEntity.Parent;
							MyEntitySubpart myEntitySubpart;
							while ((myEntitySubpart = myEntity as MyEntitySubpart) != null)
							{
								myEntity = myEntitySubpart.Parent;
							}
							MyCubeBlock myCubeBlock;
							if ((myCubeBlock = myEntity as MyCubeBlock) != null)
							{
								myEntity = myCubeBlock.CubeGrid;
							}
						}
						MyCubeGrid item;
						if ((item = myEntity as MyCubeGrid) != null)
						{
							if (!m_connectedTarget.Contains(item))
							{
								return m_connectedMy.Contains(item);
							}
							continue;
						}
						return false;
					}
				}
			}
			return true;
		}

		public bool PrefetchVoxels(Vector3D from, Vector3D to)
		{
			using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
			{
				LineD ray = new LineD(from, to);
				MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, m_entityRaycastResult);
				bool flag = false;
				foreach (MyLineSegmentOverlapResult<MyVoxelBase> item in m_entityRaycastResult)
				{
					MyVoxelPhysics myVoxelPhysics;
					if ((myVoxelPhysics = item.Element as MyVoxelPhysics) != null)
					{
						if (myVoxelPhysics.ParentPlanet != null)
						{
							continue;
						}
						flag |= myVoxelPhysics.PrefetchShapeOnRay(ref ray);
					}
					MyPlanet myPlanet;
					if ((myPlanet = item.Element as MyPlanet) != null)
					{
						flag |= myPlanet.PrefetchShapeOnRay(ref ray);
					}
				}
				return flag;
			}
		}
	}
}
