using System;
using System.Collections.Generic;
using System.Linq;
using ParallelTasks;
using RecastDetour;
using Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using VRage.Library.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	public class MyNavmeshManager
	{
<<<<<<< HEAD
=======
		public class CoordComparer : IEqualityComparer<Vector2I>
		{
			public bool Equals(Vector2I a, Vector2I b)
			{
				if (a.X == b.X)
				{
					return a.Y == b.Y;
				}
				return false;
			}

			public int GetHashCode(Vector2I point)
			{
				return (point.X + point.Y.ToString()).GetHashCode();
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public class OBBCoordComparer : IEqualityComparer<MyNavmeshOBBs.OBBCoords>
		{
			public bool Equals(MyNavmeshOBBs.OBBCoords a, MyNavmeshOBBs.OBBCoords b)
			{
<<<<<<< HEAD
				return a.Coords == b.Coords;
=======
				if (a.Coords.X == b.Coords.X)
				{
					return a.Coords.Y == b.Coords.Y;
				}
				return false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			public int GetHashCode(MyNavmeshOBBs.OBBCoords point)
			{
<<<<<<< HEAD
				return point.Coords.GetHashCode();
=======
				return (point.Coords.X + point.Coords.Y.ToString()).GetHashCode();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private struct Vertex
		{
			public Vector3D pos;

			public Color color;
		}

		private static readonly MyRandom m_myRandom = new MyRandom(0);

		public Color m_debugColor;

		private const float RECAST_CELL_SIZE = 0.2f;

<<<<<<< HEAD
		/// <summary>
		/// The maximum number of tiles that, each time, can be added to navmesh generation
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private const int MAX_TILES_TO_GENERATE = 7;

		private const int MAX_TICKS_WITHOUT_HEARTBEAT = 5000;

		private int m_ticksAfterLastPathRequest;

		private readonly int m_tileSize;

		private readonly int m_tileHeight;

		private readonly int m_tileLineCount;

		private readonly float m_border;

		private readonly float m_heightCoordTransformationIncrease;

		private bool m_allTilesGenerated;

		private bool m_isManagerAlive = true;

		private bool m_isTileBeingGenerated;

		private MyNavmeshOBBs m_navmeshOBBs;

		private MyRecastOptions m_recastOptions;

		private MyNavigationInputMesh m_navInputMesh;

<<<<<<< HEAD
		private HashSet<MyNavmeshOBBs.OBBCoords> m_obbCoordsToUpdate = new HashSet<MyNavmeshOBBs.OBBCoords>(new OBBCoordComparer());

		private HashSet<Vector2I> m_coordsAlreadyGenerated = new HashSet<Vector2I>(new Vector2I.ComparerClass());
=======
		private HashSet<MyNavmeshOBBs.OBBCoords> m_obbCoordsToUpdate = new HashSet<MyNavmeshOBBs.OBBCoords>((IEqualityComparer<MyNavmeshOBBs.OBBCoords>)new OBBCoordComparer());

		private HashSet<Vector2I> m_coordsAlreadyGenerated = new HashSet<Vector2I>((IEqualityComparer<Vector2I>)new CoordComparer());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private Dictionary<Vector2I, List<MyFormatPositionColor>> m_obbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();

		private Dictionary<Vector2I, List<MyFormatPositionColor>> m_newObbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();

		private MyRDWrapper m_rdWrapper;

		private MyOrientedBoundingBoxD m_extendedBaseOBB;

		private readonly List<MyVoxelMap> m_tmpTrackedVoxelMaps = new List<MyVoxelMap>();

		private readonly Dictionary<long, MyVoxelMap> m_trackedVoxelMaps = new Dictionary<long, MyVoxelMap>();

		private readonly int?[][] m_debugTileSize;

		private bool m_drawMesh;

		private bool m_updateDrawMesh;

		private List<BoundingBoxD> m_groundCaptureAABBs = new List<BoundingBoxD>();

		private int m_remainingTilesToGenerate;

<<<<<<< HEAD
=======
		private readonly object m_tileGenerationLock = new object();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private uint m_drawNavmeshId = uint.MaxValue;

		public Vector3D Center => m_navmeshOBBs.CenterOBB.Center;

		public MyOrientedBoundingBoxD CenterOBB => m_navmeshOBBs.CenterOBB;

		public MyPlanet Planet { get; }

<<<<<<< HEAD
		private bool TilesAreAwaitingGeneration => m_obbCoordsToUpdate.Count > 0;
=======
		private bool TilesAreAwaitingGeneration => m_obbCoordsToUpdate.get_Count() > 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public bool DrawNavmesh
		{
			get
			{
				return m_drawMesh;
			}
			set
			{
				m_drawMesh = value;
				if (m_drawMesh)
				{
					DrawPersistentDebugNavmesh();
				}
				else
				{
					HidePersistentDebugNavmesh();
				}
			}
		}

		public MyNavmeshManager(MyRDPathfinding rdPathfinding, Vector3D center, Vector3D forwardDirection, int tileSize, int tileHeight, int tileLineCount, MyRecastOptions recastOptions)
		{
			Vector3 vector = new Vector3(m_myRandom.NextFloat(), m_myRandom.NextFloat(), m_myRandom.NextFloat());
			vector -= Math.Min(vector.X, Math.Min(vector.Y, vector.Z));
			vector /= Math.Max(vector.X, Math.Max(vector.Y, vector.Z));
			m_debugColor = new Color(vector);
			m_tileSize = tileSize;
			m_tileHeight = tileHeight;
			m_tileLineCount = tileLineCount;
			Planet = GetPlanet(center);
			m_heightCoordTransformationIncrease = 0.5f;
			m_recastOptions = recastOptions;
			float num = (float)m_tileSize * 0.5f + (float)m_tileSize * (float)Math.Floor((float)m_tileLineCount * 0.5f);
			float num2 = (float)m_tileHeight * 0.5f;
			m_border = m_recastOptions.agentRadius + 0.6f;
			float[] bMin = new float[3]
			{
				0f - num,
				0f - num2,
				0f - num
			};
			float[] bMax = new float[3] { num, num2, num };
			m_rdWrapper = new MyRDWrapper();
			m_rdWrapper.Init(0.2f, m_tileSize, bMin, bMax);
			Vector3D forwardDirection2 = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center)));
			m_navmeshOBBs = new MyNavmeshOBBs(Planet, center, forwardDirection2, m_tileLineCount, m_tileSize, m_tileHeight);
			m_debugTileSize = new int?[m_tileLineCount][];
			for (int i = 0; i < m_tileLineCount; i++)
			{
				m_debugTileSize[i] = new int?[m_tileLineCount];
			}
			m_extendedBaseOBB = new MyOrientedBoundingBoxD(m_navmeshOBBs.BaseOBB.Center, new Vector3D(m_navmeshOBBs.BaseOBB.HalfExtent.X, m_tileHeight, m_navmeshOBBs.BaseOBB.HalfExtent.Z), m_navmeshOBBs.BaseOBB.Orientation);
			m_navInputMesh = new MyNavigationInputMesh(rdPathfinding, Planet, center);
		}

<<<<<<< HEAD
		/// <summary>
		/// Checks if the manager intersects the given OBB.
		/// </summary>
		/// <param name="areaAABB"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool InvalidateArea(BoundingBoxD areaAABB)
		{
			bool flag = false;
			if (!Intersects(areaAABB))
			{
				return false;
			}
			bool flag2 = false;
			for (int i = 0; i < m_tileLineCount; i++)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = 0; j < m_tileLineCount; j++)
				{
					if (m_navmeshOBBs.GetOBB(i, j).Value.Intersects(ref areaAABB))
					{
						Vector2I vector2I = new Vector2I(i, j);
						flag3 = (flag4 = true);
						if (m_coordsAlreadyGenerated.Remove(vector2I))
						{
							flag = true;
							m_allTilesGenerated = false;
							m_newObbCoordsPolygons[vector2I] = null;
							m_navInputMesh.InvalidateCache(areaAABB);
						}
					}
					else if (flag4)
					{
						break;
					}
				}
				if (flag3)
				{
					flag2 = true;
				}
				else if (flag2)
				{
					break;
				}
			}
			if (flag)
			{
				m_updateDrawMesh = true;
			}
			return flag;
		}

<<<<<<< HEAD
		/// <summary>
		/// Checks if the given point is within the bounds of the navmesh
		/// </summary>
		/// <param name="position"></param>
		/// <returns>Returns true if the point is within bounds</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool ContainsPosition(Vector3D position)
		{
			LineD line = new LineD(Planet.PositionComp.WorldAABB.Center, position);
			return m_navmeshOBBs.BaseOBB.Intersects(ref line).HasValue;
		}

<<<<<<< HEAD
		/// <summary>
		/// Saves the tiles that need to be generated
		/// </summary>
		/// <param name="initialPosition"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void TilesToGenerate(Vector3D initialPosition, Vector3D targetPosition)
		{
			TilesToGenerateInternal(initialPosition, targetPosition, out var _);
		}

<<<<<<< HEAD
		/// <summary>
		/// Delivers the path and returns true if the path contains the target position
		/// </summary>
		/// <param name="initialPosition"></param>
		/// <param name="targetPosition"></param>
		/// <param name="path"></param>
		/// <param name="noTilesToGenerate"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool GetPathPoints(Vector3D initialPosition, Vector3D targetPosition, out List<Vector3D> path, out bool noTilesToGenerate)
		{
			Heartbeat();
			bool result = false;
			noTilesToGenerate = true;
			path = new List<Vector3D>();
			if (!m_allTilesGenerated)
			{
				TilesToGenerateInternal(initialPosition, targetPosition, out var tilesAddedToGeneration);
				noTilesToGenerate = tilesAddedToGeneration == 0;
			}
			Vector3D vector3D = WorldPositionToLocalNavmeshPosition(initialPosition, m_heightCoordTransformationIncrease);
			Vector3D vector3D2 = targetPosition;
			bool flag = !ContainsPosition(targetPosition);
			if (flag)
			{
				vector3D2 = GetBorderPoint(initialPosition, targetPosition);
				vector3D2 = GetPositionAtDistanceFromPlanetCenter(vector3D2, (initialPosition - Planet.PositionComp.WorldAABB.Center).Length());
			}
			Vector3D vector3D3 = WorldPositionToLocalNavmeshPosition(vector3D2, m_heightCoordTransformationIncrease);
			List<Vector3> path2 = m_rdWrapper.GetPath(vector3D, vector3D3);
			if (path2.Count > 0)
			{
				foreach (Vector3 item in path2)
				{
					path.Add(LocalPositionToWorldPosition(item));
				}
<<<<<<< HEAD
				Vector3D vector3D4 = path.Last();
=======
				Vector3D vector3D4 = Enumerable.Last<Vector3D>((IEnumerable<Vector3D>)path);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				double num = (vector3D2 - vector3D4).Length();
				double num2 = 0.25;
				bool num3 = num <= num2;
				result = num3 && !flag;
				if (num3)
				{
					if (flag)
					{
						path.RemoveAt(path.Count - 1);
						path.Add(targetPosition);
					}
					else if (noTilesToGenerate)
					{
						double pathDistance = GetPathDistance(path);
						double num4 = Vector3D.Distance(initialPosition, targetPosition);
						if (pathDistance > 3.0 * num4)
						{
							noTilesToGenerate = !TryGenerateTilesAroundPosition(initialPosition);
						}
					}
				}
				if ((!num3 && !m_allTilesGenerated) & noTilesToGenerate)
				{
					noTilesToGenerate = !TryGenerateTilesAroundPosition(vector3D4);
				}
			}
			noTilesToGenerate = noTilesToGenerate && m_remainingTilesToGenerate == 0;
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Updates the navmesh manager by generating the next necessary tile and updates the debug mesh.
		/// Returns false if the manager is no longer valid - it was unloaded.
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Update()
		{
			if (!CheckManagerHeartbeat())
			{
				return false;
			}
			GenerateNextQueuedTile();
			if (m_updateDrawMesh)
			{
				m_updateDrawMesh = false;
				UpdatePersistentDebugNavmesh();
			}
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Clears the data
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void UnloadData()
		{
			m_isManagerAlive = false;
			foreach (KeyValuePair<long, MyVoxelMap> trackedVoxelMap in m_trackedVoxelMaps)
			{
				trackedVoxelMap.Value.RangeChanged -= VoxelMapRangeChanged;
			}
			m_trackedVoxelMaps.Clear();
			m_rdWrapper.Clear();
			m_rdWrapper = null;
			m_navInputMesh.Clear();
			m_navInputMesh = null;
			m_navmeshOBBs.Clear();
			m_navmeshOBBs = null;
			m_obbCoordsToUpdate.Clear();
			m_obbCoordsToUpdate = null;
			m_coordsAlreadyGenerated.Clear();
			m_coordsAlreadyGenerated = null;
			m_obbCoordsPolygons.Clear();
			m_obbCoordsPolygons = null;
			m_newObbCoordsPolygons.Clear();
			m_newObbCoordsPolygons = null;
		}

		public void DebugDraw()
		{
			m_navmeshOBBs.DebugDraw();
			m_navInputMesh.DebugDraw();
			MyRenderProxy.DebugDrawOBB(m_extendedBaseOBB, Color.White, 0f, depthRead: true, smooth: false);
			for (int i = 0; i < m_groundCaptureAABBs.Count; i++)
			{
				MyRenderProxy.DebugDrawAABB(m_groundCaptureAABBs[i], Color.Yellow);
			}
		}

		private Vector3D LocalPositionToWorldPosition(Vector3D position)
		{
			Vector3D vector3D = position;
			if (m_navmeshOBBs != null)
			{
				vector3D = Center;
			}
			Vector3D vector3D2 = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D));
			return LocalNavmeshPositionToWorldPosition(m_navmeshOBBs.CenterOBB, position, vector3D, (0f - m_heightCoordTransformationIncrease) * vector3D2);
		}

		private static MatrixD LocalNavmeshPositionToWorldPositionTransform(MyOrientedBoundingBoxD obb, Vector3D center)
		{
			Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
			return MatrixD.CreateFromQuaternion(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D));
		}

		private Vector3D LocalNavmeshPositionToWorldPosition(MyOrientedBoundingBoxD obb, Vector3D position, Vector3D center, Vector3D heightIncrease)
		{
			MatrixD matrix = LocalNavmeshPositionToWorldPositionTransform(obb, center);
			return Vector3D.Transform(position, matrix) + Center + heightIncrease;
		}

		private Vector3D WorldPositionToLocalNavmeshPosition(Vector3D position, float heightIncrease)
		{
			Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(Center));
			MatrixD matrix = MatrixD.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D)));
			return Vector3D.Transform(position - Center + heightIncrease * vector3D, matrix);
		}

		private Vector3D GetBorderPoint(Vector3D startingPoint, Vector3D outsidePoint)
		{
			LineD line = new LineD(startingPoint, outsidePoint);
			double? num = m_extendedBaseOBB.Intersects(ref line);
			if (!num.HasValue)
			{
				return outsidePoint;
			}
			line.Length = num.Value - 1.0;
			line.To = startingPoint + line.Direction * num.Value;
			return line.To;
		}

<<<<<<< HEAD
		/// <summary>
		/// Updates the heartbeat in order to keep the manager alive
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void Heartbeat()
		{
			m_ticksAfterLastPathRequest = 0;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns true if the manager is still alive 
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool CheckManagerHeartbeat()
		{
			if (!m_isManagerAlive)
			{
				return false;
			}
			m_ticksAfterLastPathRequest++;
			m_isManagerAlive = m_ticksAfterLastPathRequest < 5000;
			if (!m_isManagerAlive)
			{
				UnloadData();
			}
			return m_isManagerAlive;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the distance of the given path
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static double GetPathDistance(List<Vector3D> path)
		{
			double num = 0.0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				num += Vector3D.Distance(path[i], path[i + 1]);
			}
			return num;
		}

<<<<<<< HEAD
		/// <summary>
		/// Checks if the manager intersects the given OBB.
		/// </summary>
		/// <param name="obb"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool Intersects(BoundingBoxD obb)
		{
			return m_extendedBaseOBB.Intersects(ref obb);
		}

<<<<<<< HEAD
		/// <summary>
		/// Generate tiles around position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool TryGenerateTilesAroundPosition(Vector3D position)
		{
			MyNavmeshOBBs.OBBCoords? oBBCoord = m_navmeshOBBs.GetOBBCoord(position);
			if (oBBCoord.HasValue)
			{
				return TryGenerateNeighbourTiles(oBBCoord.Value);
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Generate tiles around the obbCoord. Each time, a bigger "circle" around it.
		/// </summary>
		/// <param name="obbCoord"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool TryGenerateNeighbourTiles(MyNavmeshOBBs.OBBCoords obbCoord, int radius = 1)
		{
			int num = 0;
			bool flag = false;
			Vector2I vector2I = default(Vector2I);
			for (int i = -radius; i <= radius; i++)
			{
				int num2 = ((i == -radius || i == radius) ? 1 : (2 * radius));
				for (int j = -radius; j <= radius; j += num2)
				{
					vector2I.X = obbCoord.Coords.X + j;
					vector2I.Y = obbCoord.Coords.Y + i;
					MyNavmeshOBBs.OBBCoords? oBBCoord = m_navmeshOBBs.GetOBBCoord(vector2I.X, vector2I.Y);
					if (!oBBCoord.HasValue)
					{
						continue;
					}
					flag = true;
					if (AddTileToGeneration(oBBCoord.Value))
					{
						num++;
						if (num >= 7)
						{
							return true;
						}
					}
				}
			}
			if (num > 0)
			{
				return true;
			}
			m_allTilesGenerated = !flag;
			if (m_allTilesGenerated)
			{
				return false;
			}
			return TryGenerateNeighbourTiles(obbCoord, radius + 1);
		}

<<<<<<< HEAD
		/// <summary>
		/// Saves the tiles that need to be generated and returns the full intersected OBB list
		/// </summary>
		/// <param name="initialPosition"></param>
		/// <param name="targetPosition"></param>
		/// /// <param name="tilesAddedToGeneration"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private List<MyNavmeshOBBs.OBBCoords> TilesToGenerateInternal(Vector3D initialPosition, Vector3D targetPosition, out int tilesAddedToGeneration)
		{
			tilesAddedToGeneration = 0;
			List<MyNavmeshOBBs.OBBCoords> intersectedOBB = m_navmeshOBBs.GetIntersectedOBB(new LineD(initialPosition, targetPosition));
			foreach (MyNavmeshOBBs.OBBCoords item in intersectedOBB)
			{
				if (AddTileToGeneration(item))
				{
					tilesAddedToGeneration++;
					if (tilesAddedToGeneration == 7)
					{
						return intersectedOBB;
					}
				}
			}
			return intersectedOBB;
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds the file the be generation, if it wasn't generated before.
		/// </summary>
		/// <param name="obbCoord"></param>
		/// <returns>True if it was added to the list of the tiles to be generated</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private bool AddTileToGeneration(MyNavmeshOBBs.OBBCoords obbCoord)
		{
			if (!m_coordsAlreadyGenerated.Contains(obbCoord.Coords))
			{
				if (!m_obbCoordsToUpdate.Add(obbCoord))
				{
					return false;
				}
<<<<<<< HEAD
				m_remainingTilesToGenerate++;
=======
				lock (m_tileGenerationLock)
				{
					m_remainingTilesToGenerate++;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return true;
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns a new position moved along the gravity vector by distance amount
		/// </summary>
		/// <param name="position"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private Vector3D GetPositionAtDistanceFromPlanetCenter(Vector3D position, double distance)
		{
			(position - Planet.PositionComp.WorldAABB.Center).Length();
			return -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(position)) * distance + Planet.PositionComp.WorldAABB.Center;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the planet closest to the given position
		/// </summary>
		/// <param name="position">3D Point from where the search is started</param>
		/// <returns>The closest planet</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyPlanet GetPlanet(Vector3D position)
		{
			int num = 200;
			BoundingBoxD box = new BoundingBoxD(position - (float)num * 0.5f, position + (float)num * 0.5f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}

<<<<<<< HEAD
		/// <summary>
		/// Generates the next tile in the queue
		/// </summary>
		private void GenerateNextQueuedTile()
		{
			if (TilesAreAwaitingGeneration && !m_isTileBeingGenerated)
			{
				m_isTileBeingGenerated = true;
				MyNavmeshOBBs.OBBCoords obb = m_obbCoordsToUpdate.First();
				m_obbCoordsToUpdate.Remove(obb);
				m_coordsAlreadyGenerated.Add(obb.Coords);
				m_navInputMesh.CreateWorldVerticesAndShapes(m_border, Center, obb.OBB, m_tmpTrackedVoxelMaps, out var bbs, out var worldVertices, out var shapesInfo);
				Parallel.Start(delegate
				{
					GenerateTile(obb, bbs, worldVertices, shapesInfo);
				}, delegate
				{
					m_remainingTilesToGenerate--;
					m_isTileBeingGenerated = false;
				});
			}
		}

		/// <summary>
		/// Generates a navmesh tile
		/// </summary>
		/// <param name="obbCoord"></param>
		/// <param name="bbs"></param>
		/// <param name="worldVertices"></param>
		/// <param name="shapesInfo"></param>
		private unsafe void GenerateTile(MyNavmeshOBBs.OBBCoords obbCoord, List<BoundingBoxD> bbs, WorldVerticesInfo worldVertices, MyShapesInfo shapesInfo)
		{
			Vector3D vector3D = WorldPositionToLocalNavmeshPosition(obbCoord.OBB.Center, 0f);
=======
		private void GenerateNextQueuedTile()
		{
			if (!TilesAreAwaitingGeneration || m_isTileBeingGenerated)
			{
				return;
			}
			m_isTileBeingGenerated = true;
			MyNavmeshOBBs.OBBCoords obb = Enumerable.First<MyNavmeshOBBs.OBBCoords>((IEnumerable<MyNavmeshOBBs.OBBCoords>)m_obbCoordsToUpdate);
			m_obbCoordsToUpdate.Remove(obb);
			m_coordsAlreadyGenerated.Add(obb.Coords);
			m_navInputMesh.CreateWorldVerticesAndShapes(m_border, Center, obb.OBB, m_tmpTrackedVoxelMaps, out var bbs, out var worldVertices, out var shapesInfo);
			Parallel.Start(delegate
			{
				GenerateTile(obb, bbs, worldVertices, shapesInfo);
			}, delegate
			{
				lock (m_tileGenerationLock)
				{
					m_remainingTilesToGenerate--;
					m_isTileBeingGenerated = false;
				}
			});
		}

		private unsafe void GenerateTile(MyNavmeshOBBs.OBBCoords obbCoord, List<BoundingBoxD> bbs, WorldVerticesInfo worldVertices, MyShapesInfo shapesInfo)
		{
			Vector3 vector = WorldPositionToLocalNavmeshPosition(obbCoord.OBB.Center, 0f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_groundCaptureAABBs = bbs;
			foreach (MyVoxelMap tmpTrackedVoxelMap in m_tmpTrackedVoxelMaps)
			{
				if (!m_trackedVoxelMaps.ContainsKey(tmpTrackedVoxelMap.EntityId))
				{
					tmpTrackedVoxelMap.RangeChanged += VoxelMapRangeChanged;
					m_trackedVoxelMaps.Add(tmpTrackedVoxelMap.EntityId, tmpTrackedVoxelMap);
				}
			}
			m_tmpTrackedVoxelMaps.Clear();
			worldVertices = m_navInputMesh.AddGroundToWorldVertices(m_border, Center, obbCoord.OBB, bbs, worldVertices);
			worldVertices = m_navInputMesh.AddPhysicalShapeVertices(shapesInfo, worldVertices);
			if (worldVertices.Triangles.Count > 0)
			{
<<<<<<< HEAD
				List<MyRecastDetourPolygon> list = null;
				fixed (Vector3* ptr = worldVertices.Vertices.GetInternalArray())
				{
					float* vertices = (float*)ptr;
					fixed (int* triangles = worldVertices.Triangles.GetInternalArray())
					{
						m_rdWrapper.CreateNavmeshTile(vector3D, ref m_recastOptions, list, obbCoord.Coords.X, obbCoord.Coords.Y, 0, vertices, worldVertices.Vertices.Count, triangles, worldVertices.Triangles.Count / 3);
					}
				}
				if (list != null)
				{
					GenerateDebugDrawPolygonNavmesh(Planet, list, m_navmeshOBBs.CenterOBB, obbCoord.Coords);
				}
				GenerateDebugTileDataSize(vector3D, obbCoord.Coords.X, obbCoord.Coords.Y);
				if (list != null)
				{
					list.Clear();
=======
				List<MyRecastDetourPolygon> polygons = null;
				fixed (Vector3* ptr = worldVertices.Vertices.ToArray())
				{
					float* vertices = (float*)ptr;
					fixed (int* triangles = worldVertices.Triangles.ToArray())
					{
						m_rdWrapper.CreateNavmeshTile(vector, ref m_recastOptions, ref polygons, obbCoord.Coords.X, obbCoord.Coords.Y, 0, vertices, worldVertices.Vertices.Count, triangles, worldVertices.Triangles.Count / 3);
					}
				}
				GenerateDebugDrawPolygonNavmesh(Planet, polygons, m_navmeshOBBs.CenterOBB, obbCoord.Coords);
				GenerateDebugTileDataSize(vector, obbCoord.Coords.X, obbCoord.Coords.Y);
				if (polygons != null)
				{
					polygons.Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_updateDrawMesh = true;
				}
			}
			else
			{
				m_newObbCoordsPolygons[obbCoord.Coords] = null;
			}
		}

		private void VoxelMapRangeChanged(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			BoundingBoxD voxelAreaAABB = MyRDPathfinding.GetVoxelAreaAABB(storage, minVoxelChanged, maxVoxelChanged);
			InvalidateArea(voxelAreaAABB);
		}

<<<<<<< HEAD
		private void GenerateDebugTileDataSize(Vector3D center, int xCoord, int yCoord)
=======
		private void GenerateDebugTileDataSize(Vector3 center, int xCoord, int yCoord)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			int tileDataSize = m_rdWrapper.GetTileDataSize(center, 0);
			m_debugTileSize[xCoord][yCoord] = tileDataSize;
		}

		private void GenerateDebugDrawPolygonNavmesh(MyPlanet planet, IReadOnlyCollection<MyRecastDetourPolygon> polygons, MyOrientedBoundingBoxD centerOBB, Vector2I coords)
		{
			if (polygons == null)
			{
				return;
			}
			List<MyFormatPositionColor> list = new List<MyFormatPositionColor>();
			int num = 0;
			foreach (MyRecastDetourPolygon polygon in polygons)
			{
				Vector3[] vertices = polygon.Vertices;
				foreach (Vector3 vector in vertices)
				{
					MyFormatPositionColor myFormatPositionColor = default(MyFormatPositionColor);
					myFormatPositionColor.Position = LocalNavmeshPositionToWorldPosition(centerOBB, vector, Center, Vector3D.Zero);
					myFormatPositionColor.Color = new Color(0, 10 + num, 0);
					MyFormatPositionColor item = myFormatPositionColor;
					list.Add(item);
				}
				num += 10;
				num %= 95;
			}
			if (list.Count > 0)
			{
				m_newObbCoordsPolygons[coords] = list;
			}
		}

		private void DrawPersistentDebugNavmesh()
		{
			foreach (KeyValuePair<Vector2I, List<MyFormatPositionColor>> newObbCoordsPolygon in m_newObbCoordsPolygons)
			{
				if (m_newObbCoordsPolygons[newObbCoordsPolygon.Key] == null)
				{
					m_obbCoordsPolygons.Remove(newObbCoordsPolygon.Key);
				}
				else
				{
					m_obbCoordsPolygons[newObbCoordsPolygon.Key] = newObbCoordsPolygon.Value;
				}
			}
			m_newObbCoordsPolygons.Clear();
			if (m_obbCoordsPolygons.Count <= 0)
			{
				return;
			}
			List<MyFormatPositionColor> list = new List<MyFormatPositionColor>();
			foreach (List<MyFormatPositionColor> value in m_obbCoordsPolygons.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					list.Add(value[i]);
				}
			}
			if (m_drawNavmeshId == uint.MaxValue)
			{
				m_drawNavmeshId = MyRenderProxy.DebugDrawMesh(list, MatrixD.Identity, depthRead: true, shaded: true);
			}
			else
			{
				MyRenderProxy.DebugDrawUpdateMesh(m_drawNavmeshId, list, MatrixD.Identity, depthRead: true, shaded: true);
			}
		}

		private void HidePersistentDebugNavmesh()
		{
			if (m_drawNavmeshId != uint.MaxValue)
			{
				MyRenderProxy.RemoveRenderObject(m_drawNavmeshId, MyRenderProxy.ObjectType.DebugDrawMesh);
				m_drawNavmeshId = uint.MaxValue;
			}
		}

		private void UpdatePersistentDebugNavmesh()
		{
			DrawNavmesh = DrawNavmesh;
		}
	}
}
