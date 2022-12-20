using System;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyPathfinding : MyPathFindingSystem<MyNavigationPrimitive>, IMyPathfinding
	{
		public readonly Func<long> NextTimestampFunction;

		private MyNavigationPrimitive m_reachEndPrimitive;

		private float m_reachPredicateDistance;

		public MyGridPathfinding GridPathfinding { get; private set; }

		public MyVoxelPathfinding VoxelPathfinding { get; private set; }

		public MyNavmeshCoordinator Coordinator { get; private set; }

		public MyDynamicObstacles Obstacles { get; private set; }

		public long LastHighLevelTimestamp { get; set; }

		private long GenerateNextTimestamp()
		{
			CalculateNextTimestamp();
			return GetCurrentTimestamp();
		}

		public MyPathfinding()
			: base(128, (Func<long>)null)
		{
			NextTimestampFunction = GenerateNextTimestamp;
			Obstacles = new MyDynamicObstacles();
			Coordinator = new MyNavmeshCoordinator(Obstacles);
			GridPathfinding = new MyGridPathfinding(Coordinator);
			VoxelPathfinding = new MyVoxelPathfinding(Coordinator);
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
		}

		public void Update()
		{
			if (MyPerGameSettings.EnablePathfinding)
			{
				Obstacles.Update();
				GridPathfinding.Update();
				VoxelPathfinding.Update();
			}
		}

		public IMyPathfindingLog GetPathfindingLog()
		{
			return VoxelPathfinding.DebugLog;
		}

		public void UnloadData()
		{
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			VoxelPathfinding.UnloadData();
			GridPathfinding = null;
			VoxelPathfinding = null;
			Coordinator = null;
			Obstacles.Clear();
			Obstacles = null;
		}

		private void MyEntities_OnEntityAdd(MyEntity newEntity)
		{
			Obstacles.TryCreateObstacle(newEntity);
			MyCubeGrid grid;
			if ((grid = newEntity as MyCubeGrid) != null)
			{
				GridPathfinding.GridAdded(grid);
			}
		}

		public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity entity = null)
		{
			if (!MyPerGameSettings.EnablePathfinding)
			{
				return null;
			}
			MySmartPath mySmartPath = new MySmartPath(this);
			MySmartGoal goal = new MySmartGoal(end, entity);
			mySmartPath.Init(begin, goal);
			return mySmartPath;
		}

		private bool ReachablePredicate(MyNavigationPrimitive primitive)
		{
			return (m_reachEndPrimitive.WorldPosition - primitive.WorldPosition).LengthSquared() <= (double)(m_reachPredicateDistance * m_reachPredicateDistance);
		}

		public bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance)
		{
			m_reachPredicateDistance = thresholdDistance;
			MyNavigationPrimitive myNavigationPrimitive = FindClosestPrimitive(begin, highLevel: false);
			MyNavigationPrimitive myNavigationPrimitive2 = FindClosestPrimitive(end.GetDestination(), highLevel: false);
			if (myNavigationPrimitive == null || myNavigationPrimitive2 == null)
			{
				return false;
			}
			MyHighLevelPrimitive highLevelPrimitive = myNavigationPrimitive.GetHighLevelPrimitive();
			myNavigationPrimitive2.GetHighLevelPrimitive();
			if (new MySmartGoal(end).FindHighLevelPath(this, highLevelPrimitive) == null)
			{
				return false;
			}
			m_reachEndPrimitive = myNavigationPrimitive2;
			PrepareTraversal(myNavigationPrimitive, null, ReachablePredicate);
			try
			{
<<<<<<< HEAD
				using (Enumerator enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Equals(m_reachEndPrimitive))
						{
							return true;
						}
=======
				using Enumerator enumerator = GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(m_reachEndPrimitive))
					{
						return true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			finally
			{
			}
			return false;
		}

		public MyPath<MyNavigationPrimitive> FindPathLowlevel(Vector3D begin, Vector3D end)
		{
			MyPath<MyNavigationPrimitive> result = null;
			if (!MyPerGameSettings.EnablePathfinding)
			{
				return result;
			}
			MyNavigationPrimitive myNavigationPrimitive = FindClosestPrimitive(begin, highLevel: false);
			MyNavigationPrimitive myNavigationPrimitive2 = FindClosestPrimitive(end, highLevel: false);
			if (myNavigationPrimitive != null && myNavigationPrimitive2 != null)
			{
				result = FindPath(myNavigationPrimitive, myNavigationPrimitive2);
			}
			return result;
		}

		public MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, MyEntity entity = null)
		{
			double closestDistanceSq = double.PositiveInfinity;
			MyNavigationPrimitive result = null;
			MyNavigationPrimitive myNavigationPrimitive = null;
			MyVoxelMap myVoxelMap = entity as MyVoxelMap;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myVoxelMap != null)
			{
				result = VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq, myVoxelMap);
			}
			else if (myCubeGrid != null)
			{
				result = GridPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq, myCubeGrid);
			}
			else
			{
				myNavigationPrimitive = VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
				myNavigationPrimitive = GridPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
			}
			return result;
		}

		public void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != 0)
				{
					Coordinator.Links.DebugDraw(Color.Khaki);
				}
				if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
				{
					Coordinator.HighLevelLinks.DebugDraw(Color.LightGreen);
				}
				Coordinator.DebugDraw();
				Obstacles.DebugDraw();
			}
		}
	}
}
