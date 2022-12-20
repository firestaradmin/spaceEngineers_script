using System;
using System.Collections.Generic;
using System.Linq;
using RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using VRage.Game.Entity;
<<<<<<< HEAD
using VRage.Utils;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	public class MyRDPathfinding : IMyPathfinding
	{
<<<<<<< HEAD
		/// <summary>
		/// Class for the debug drawing of the path
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private class RequestedPath
		{
			public List<Vector3D> Path;

			public int LocalTicks;
		}

		private const int DEBUG_PATH_MAX_TICKS = 150;

		private const int TILE_SIZE = 16;

		private const int TILE_HEIGHT = 70;

		private const int TILE_LINE_COUNT = 25;

		private readonly double MIN_NAVMESH_MANAGER_SQUARED_DISTANCE = Math.Pow(160.0, 2.0);

		private readonly Dictionary<MyPlanet, List<MyNavmeshManager>> m_planetManagers = new Dictionary<MyPlanet, List<MyNavmeshManager>>();

		private readonly HashSet<MyCubeGrid> m_grids = new HashSet<MyCubeGrid>();

		private bool m_drawNavmesh;

		private BoundingBoxD? m_debugInvalidateTileAABB;

		private readonly List<RequestedPath> m_debugDrawPaths = new List<RequestedPath>();

		private int m_frameCounter;

		public MyRDPathfinding()
		{
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
			MyEntities.OnEntityRemove += MyEntities_OnEntityRemove;
		}

		public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity relativeEntity)
		{
			MyRDPath myRDPath = new MyRDPath(this, begin, end);
			if (!myRDPath.GetNextTarget(begin, out var _, out var _, out var _) && !myRDPath.IsWaitingForTileGeneration)
			{
				myRDPath = null;
			}
			return myRDPath;
		}

		public bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance)
		{
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Backwards compatibility
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public IMyPathfindingLog GetPathfindingLog()
		{
			return null;
		}

		public void Update()
		{
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				for (int i = 0; i < planetManager.Value.Count; i++)
				{
					if (!planetManager.Value[i].Update())
					{
						planetManager.Value.RemoveAt(i);
						i--;
					}
				}
			}
			if (m_frameCounter++ % 100 == 0)
			{
				MyRDWrapper.UpdateMemory();
			}
		}

		public void UnloadData()
		{
<<<<<<< HEAD
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			foreach (MyCubeGrid grid in m_grids)
			{
				grid.OnBlockAdded -= Grid_OnBlockAdded;
				grid.OnBlockRemoved -= Grid_OnBlockRemoved;
=======
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			Enumerator<MyCubeGrid> enumerator = m_grids.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCubeGrid current = enumerator.get_Current();
					current.OnBlockAdded -= Grid_OnBlockAdded;
					current.OnBlockRemoved -= Grid_OnBlockRemoved;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_grids.Clear();
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.UnloadData();
				}
			}
			MyRDWrapper.UpdateMemory();
		}

		public void DebugDraw()
		{
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.DebugDraw();
				}
			}
			if (m_debugInvalidateTileAABB.HasValue)
			{
				MyRenderProxy.DebugDrawAABB(m_debugInvalidateTileAABB.Value, Color.Yellow, 0f);
			}
			DebugDrawPaths();
		}

		public static BoundingBoxD GetVoxelAreaAABB(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
		{
			MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref minVoxelChanged, out var worldPosition);
			MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref maxVoxelChanged, out var worldPosition2);
			return new BoundingBoxD(worldPosition, worldPosition2);
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the path between given positions.
		/// </summary>
		/// <param name="planet"></param>
		/// <param name="initialPosition"></param>
		/// <param name="targetPosition"></param>
		/// <param name="allTilesGenerated"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public List<Vector3D> GetPath(MyPlanet planet, Vector3D initialPosition, Vector3D targetPosition, out bool allTilesGenerated)
		{
			if (!m_planetManagers.ContainsKey(planet))
			{
				m_planetManagers[planet] = new List<MyNavmeshManager>();
				planet.RangeChanged += VoxelChanged;
			}
			List<Vector3D> bestPathFromManagers = GetBestPathFromManagers(planet, initialPosition, targetPosition, out allTilesGenerated);
<<<<<<< HEAD
			if (bestPathFromManagers == null)
			{
				return null;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (bestPathFromManagers.Count > 0)
			{
				m_debugDrawPaths.Add(new RequestedPath
				{
					Path = bestPathFromManagers,
					LocalTicks = 0
				});
			}
			return bestPathFromManagers;
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds to the tracked grids so the changes to it are followed
		/// </summary>
		/// <param name="cubeGrid"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool AddToTrackedGrids(MyCubeGrid cubeGrid)
		{
			if (m_grids.Add(cubeGrid))
			{
				cubeGrid.OnBlockAdded += Grid_OnBlockAdded;
				cubeGrid.OnBlockRemoved += Grid_OnBlockRemoved;
				return true;
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Invalidates intersected tiles of navmesh managers, if they were generated
		/// </summary>
		/// <param name="areaBox"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void InvalidateArea(BoundingBoxD areaBox)
		{
			MyPlanet planet = GetPlanet(areaBox.Center);
			AreaChanged(planet, areaBox);
		}

<<<<<<< HEAD
		/// <summary>
		/// Enables or disables the drawing of the navmesh.
		/// </summary>
		/// <param name="drawNavmesh"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void SetDrawNavmesh(bool drawNavmesh)
		{
			m_drawNavmesh = drawNavmesh;
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.DrawNavmesh = m_drawNavmesh;
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the planet closest to the given position
		/// </summary>
		/// <param name="position">3D Point from where the search is centered</param>
		/// <returns>The closest planet</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static MyPlanet GetPlanet(Vector3D position)
		{
			BoundingBoxD box = new BoundingBoxD(position - 250.0, position + 250f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		private void MyEntities_OnEntityAdd(MyEntity obj)
		{
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = obj as MyCubeGrid) == null)
			{
				return;
			}
			MyPlanet planet = GetPlanet(myCubeGrid.PositionComp.WorldAABB.Center);
			if (planet == null || !m_planetManagers.TryGetValue(planet, out var value))
			{
				return;
			}
			bool flag = false;
			foreach (MyNavmeshManager item in value)
			{
				flag |= item.InvalidateArea(myCubeGrid.PositionComp.WorldAABB);
			}
			if (flag)
			{
				AddToTrackedGrids(myCubeGrid);
			}
		}

		private void MyEntities_OnEntityRemove(MyEntity obj)
		{
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = obj as MyCubeGrid) == null || !m_grids.Remove(myCubeGrid))
			{
				return;
			}
			myCubeGrid.OnBlockAdded -= Grid_OnBlockAdded;
			myCubeGrid.OnBlockRemoved -= Grid_OnBlockRemoved;
			MyPlanet planet = GetPlanet(myCubeGrid.PositionComp.WorldAABB.Center);
			if (planet == null || !m_planetManagers.TryGetValue(planet, out var value))
			{
				return;
			}
			foreach (MyNavmeshManager item in value)
			{
				item.InvalidateArea(myCubeGrid.PositionComp.WorldAABB);
			}
		}

		private void Grid_OnBlockAdded(MySlimBlock slimBlock)
		{
			MyPlanet planet = GetPlanet(slimBlock.WorldPosition);
			if (planet == null || !m_planetManagers.TryGetValue(planet, out var value))
			{
				return;
			}
			BoundingBoxD worldAABB = slimBlock.WorldAABB;
			foreach (MyNavmeshManager item in value)
			{
				item.InvalidateArea(worldAABB);
			}
		}

		private void Grid_OnBlockRemoved(MySlimBlock slimBlock)
		{
			MyPlanet planet = GetPlanet(slimBlock.WorldPosition);
			if (planet == null || !m_planetManagers.TryGetValue(planet, out var value))
			{
				return;
			}
			BoundingBoxD worldAABB = slimBlock.WorldAABB;
			foreach (MyNavmeshManager item in value)
			{
				item.InvalidateArea(worldAABB);
			}
		}

		private void VoxelChanged(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			MyPlanet myPlanet;
			if ((myPlanet = storage as MyPlanet) != null)
			{
				BoundingBoxD voxelAreaAABB = GetVoxelAreaAABB(myPlanet, minVoxelChanged, maxVoxelChanged);
				AreaChanged(myPlanet, voxelAreaAABB);
				m_debugInvalidateTileAABB = voxelAreaAABB;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Signals the navigation that the area changed and needs update.
		/// </summary>
		/// <param name="planet"></param>
		/// <param name="areaBox"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void AreaChanged(MyPlanet planet, BoundingBoxD areaBox)
		{
			if (!m_planetManagers.TryGetValue(planet, out var value))
			{
				return;
			}
			foreach (MyNavmeshManager item in value)
			{
				item.InvalidateArea(areaBox);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the best path from managers according to both initial and target positions
		/// </summary>
		/// <param name="planet"></param>
		/// <param name="initialPosition"></param>
		/// <param name="targetPosition"></param>
		/// <param name="allTilesGenerated"></param>
		/// <returns></returns>
		private List<Vector3D> GetBestPathFromManagers(MyPlanet planet, Vector3D initialPosition, Vector3D targetPosition, out bool allTilesGenerated)
		{
			allTilesGenerated = true;
			List<MyNavmeshManager> list = m_planetManagers[planet].Where((MyNavmeshManager m) => m.ContainsPosition(initialPosition)).ToList();
=======
		private List<Vector3D> GetBestPathFromManagers(MyPlanet planet, Vector3D initialPosition, Vector3D targetPosition, out bool allTilesGenerated)
		{
			allTilesGenerated = true;
			List<MyNavmeshManager> list = Enumerable.ToList<MyNavmeshManager>(Enumerable.Where<MyNavmeshManager>((IEnumerable<MyNavmeshManager>)m_planetManagers[planet], (Func<MyNavmeshManager, bool>)((MyNavmeshManager m) => m.ContainsPosition(initialPosition))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (list.Count > 0)
			{
				List<Vector3D> path;
				bool noTilesToGenerate;
				foreach (MyNavmeshManager item in list)
				{
					if (item.ContainsPosition(targetPosition) && item.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate))
					{
						return path;
					}
				}
				MyNavmeshManager myNavmeshManager = null;
				double num = double.MaxValue;
				foreach (MyNavmeshManager item2 in list)
				{
					double num2 = (item2.Center - initialPosition).LengthSquared();
					if (num > num2)
					{
						num = num2;
						myNavmeshManager = item2;
					}
				}
<<<<<<< HEAD
				if (myNavmeshManager == null)
				{
					MyLog.Default.Warning("Planet manager not found");
					return null;
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				bool pathPoints = myNavmeshManager.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate);
				allTilesGenerated = noTilesToGenerate;
				if (!pathPoints && noTilesToGenerate && path.Count <= 2 && num > MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
				{
					double num3 = (initialPosition - targetPosition).LengthSquared();
					if ((myNavmeshManager.Center - targetPosition).LengthSquared() - num3 > MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
					{
						CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
						allTilesGenerated = false;
					}
				}
				return path;
			}
			CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
			allTilesGenerated = false;
			return new List<Vector3D>();
		}

<<<<<<< HEAD
		/// <summary>
		/// Creates a new manager centered in targetPosition and adds it to the list of managers
		/// </summary>
		/// <param name="center"></param>
		/// <param name="forwardDirection"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyNavmeshManager CreateManager(Vector3D center, Vector3D? forwardDirection = null)
		{
			if (!forwardDirection.HasValue)
			{
				Vector3D v = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
				forwardDirection = Vector3D.CalculatePerpendicularVector(v);
			}
			MyRecastOptions recastOptions = GetRecastOptions(null);
			MyNavmeshManager myNavmeshManager = new MyNavmeshManager(this, center, forwardDirection.Value, 16, 70, 25, recastOptions);
			myNavmeshManager.DrawNavmesh = m_drawNavmesh;
			m_planetManagers[myNavmeshManager.Planet].Add(myNavmeshManager);
			return myNavmeshManager;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the Recast options to the navmesh is generating appropriately to this character
		/// </summary>
		/// <param name="character">The character </param>
		/// <returns>The options for navmesh creation</returns>
		private static MyRecastOptions GetRecastOptions(MyCharacter character)
		{
			if (MyAIComponent.Static.Bots.BotsDictionary.First().Value.BotDefinition.BehaviorSubtype == "Wolf")
=======
		private static MyRecastOptions GetRecastOptions(MyCharacter character)
		{
			if (Enumerable.First<KeyValuePair<int, IMyBot>>((IEnumerable<KeyValuePair<int, IMyBot>>)MyAIComponent.Static.Bots.BotsDictionary).Value.BotDefinition.BehaviorSubtype == "Wolf")
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return new MyRecastOptions
				{
					cellHeight = 0.2f,
					agentHeight = 0.75f,
					agentRadius = 0.25f,
					agentMaxClimb = 0.7f,
					agentMaxSlope = 55f,
					regionMinSize = 1f,
					regionMergeSize = 10f,
					edgeMaxLen = 50f,
					edgeMaxError = 3f,
					vertsPerPoly = 6f,
					detailSampleDist = 6f,
					detailSampleMaxError = 1f,
					partitionType = 1
				};
			}
			return new MyRecastOptions
			{
				cellHeight = 0.2f,
				agentHeight = 1.5f,
				agentRadius = 0.8f,
				agentMaxClimb = 1.3f,
				agentMaxSlope = 90f,
				regionMinSize = 1f,
				regionMergeSize = 10f,
				edgeMaxLen = 50f,
				edgeMaxError = 3f,
				vertsPerPoly = 6f,
				detailSampleDist = 6f,
				detailSampleMaxError = 1f,
				partitionType = 1
			};
		}

		private static void DebugDrawSinglePath(List<Vector3D> path)
		{
			for (int i = 1; i < path.Count; i++)
			{
				MyRenderProxy.DebugDrawSphere(path[i], 0.5f, Color.Yellow, 0f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(path[i - 1], path[i], Color.Yellow, Color.Yellow, depthRead: false);
			}
		}

		private void DebugDrawPaths()
		{
			for (int i = 0; i < m_debugDrawPaths.Count; i++)
			{
				RequestedPath requestedPath = m_debugDrawPaths[i];
				requestedPath.LocalTicks++;
				if (requestedPath.LocalTicks > 150)
				{
					m_debugDrawPaths.RemoveAt(i);
					i--;
				}
				else
				{
					DebugDrawSinglePath(requestedPath.Path);
				}
			}
		}
	}
}
