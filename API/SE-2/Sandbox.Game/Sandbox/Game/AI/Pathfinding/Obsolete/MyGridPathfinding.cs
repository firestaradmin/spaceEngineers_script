using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyGridPathfinding
	{
		public struct CubeId
		{
			public MyCubeGrid Grid;

			public Vector3I Coords;

			public override bool Equals(object obj)
			{
				if (obj is CubeId)
				{
					CubeId cubeId = (CubeId)obj;
					if (cubeId.Grid == Grid)
					{
						return cubeId.Coords == Coords;
					}
					return false;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Grid.GetHashCode() * 1610612741 + Coords.GetHashCode();
			}
		}

		private readonly Dictionary<MyCubeGrid, MyGridNavigationMesh> m_navigationMeshes;

		private readonly MyNavmeshCoordinator m_coordinator;

		private bool m_highLevelNavigationDirty;

		public MyGridPathfinding(MyNavmeshCoordinator coordinator)
		{
			m_navigationMeshes = new Dictionary<MyCubeGrid, MyGridNavigationMesh>();
			m_coordinator = coordinator;
			m_coordinator.SetGridPathfinding(this);
			m_highLevelNavigationDirty = false;
		}

		public void GridAdded(MyCubeGrid grid)
		{
			if (GridCanHaveNavmesh(grid))
			{
				m_navigationMeshes.Add(grid, new MyGridNavigationMesh(grid, m_coordinator, 32, MyCestmirPathfindingShorts.Pathfinding.NextTimestampFunction));
				RegisterGridEvents(grid);
			}
		}

		private void RegisterGridEvents(MyCubeGrid grid)
		{
			grid.OnClose += grid_OnClose;
		}

		public static bool GridCanHaveNavmesh(MyCubeGrid grid)
		{
			return false;
		}

		private void grid_OnClose(MyEntity entity)
		{
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = entity as MyCubeGrid) != null && GridCanHaveNavmesh(myCubeGrid))
			{
				m_coordinator.RemoveGridNavmeshLinks(myCubeGrid);
				m_navigationMeshes.Remove(myCubeGrid);
			}
		}

		public void Update()
		{
			if (!m_highLevelNavigationDirty)
			{
				return;
			}
			foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				MyGridNavigationMesh value = navigationMesh.Value;
				if (value.HighLevelDirty)
				{
					value.UpdateHighLevel();
				}
			}
			m_highLevelNavigationDirty = false;
		}

		public List<Vector4D> FindPathGlobal(MyCubeGrid startGrid, MyCubeGrid endGrid, ref Vector3D start, ref Vector3D end)
		{
			if (startGrid != endGrid)
			{
				return null;
			}
			Vector3D vector3D = Vector3D.Transform(start, startGrid.PositionComp.WorldMatrixInvScaled);
			Vector3D vector3D2 = Vector3D.Transform(end, endGrid.PositionComp.WorldMatrixInvScaled);
			MyGridNavigationMesh value = null;
			if (m_navigationMeshes.TryGetValue(startGrid, out value))
			{
				return value.FindPath(vector3D, vector3D2);
			}
			return null;
		}

		public MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistSq, MyCubeGrid grid = null)
		{
			if (highLevel)
			{
				return null;
			}
			MyNavigationPrimitive result = null;
			if (grid != null)
			{
				if (m_navigationMeshes.TryGetValue(grid, out var value))
				{
					result = value.FindClosestPrimitive(point, highLevel, ref closestDistSq);
				}
				return result;
			}
			foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				MyNavigationPrimitive myNavigationPrimitive = navigationMesh.Value.FindClosestPrimitive(point, highLevel, ref closestDistSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
			}
			return result;
		}

		public void GetCubeTriangles(CubeId cubeId, List<MyNavigationTriangle> trianglesOut)
		{
			((MyGridNavigationMesh)null)?.GetCubeTriangles(cubeId.Coords, trianglesOut);
		}

		public MyGridNavigationMesh GetNavmesh(MyCubeGrid grid)
		{
			MyGridNavigationMesh value = null;
			m_navigationMeshes.TryGetValue(grid, out value);
			return value;
		}

		public void MarkHighLevelDirty()
		{
			m_highLevelNavigationDirty = true;
		}

		[Conditional("DEBUG")]
		public void DebugDraw()
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES == MyWEMDebugDrawMode.NONE)
			{
				return;
			}
			foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				MatrixD m = navigationMesh.Key.WorldMatrix;
				Matrix matrix = m;
				Matrix.Rescale(ref matrix, 2.5f);
			}
		}

		[Conditional("DEBUG")]
		public void RemoveTriangle(int index)
		{
			if (m_navigationMeshes.Count == 0)
			{
				return;
			}
			foreach (MyGridNavigationMesh value in m_navigationMeshes.Values)
			{
				value.RemoveFace(index);
			}
		}
	}
}
