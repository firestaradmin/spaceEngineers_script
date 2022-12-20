using System;
using System.Collections.Generic;
using System.Linq;
using Havok;
using Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.WorldEnvironment;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Groups;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	internal class MyNavigationInputMesh
	{
		public class CubeInfo
		{
			public int Id { get; set; }

			public BoundingBoxD BoundingBox { get; set; }

			public List<Vector3D> TriangleVertices { get; set; }
		}

		private struct GridInfo
		{
			public long Id { get; set; }

			public List<CubeInfo> Cubes { get; set; }
		}

		private struct CacheInterval
		{
			public Vector3I Min;

			public Vector3I Max;
		}

		public class IcoSphereMesh
		{
			private struct TriangleIndices
			{
				public readonly int v1;

				public readonly int v2;

				public readonly int v3;

				public TriangleIndices(int v1, int v2, int v3)
				{
					this.v1 = v1;
					this.v2 = v2;
					this.v3 = v3;
				}
			}

			private const int RECURSION_LEVEL = 1;

			private int m_index;

			private Dictionary<long, int> m_middlePointIndexCache;

			private List<int> m_triangleIndices;

			private List<Vector3> m_positions;

			public IcoSphereMesh()
			{
				Create();
			}

			private int AddVertex(Vector3 p)
			{
				double num = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
				m_positions.Add(new Vector3((double)p.X / num, (double)p.Y / num, (double)p.Z / num));
				return m_index++;
			}

			private int GetMiddlePoint(int p1, int p2)
			{
				bool num = p1 < p2;
				long num2 = (num ? p1 : p2);
				long num3 = (num ? p2 : p1);
				long key = (num2 << 32) + num3;
				if (m_middlePointIndexCache.TryGetValue(key, out var value))
				{
					return value;
				}
				Vector3 vector = m_positions[p1];
				Vector3 vector2 = m_positions[p2];
				Vector3 p3 = new Vector3((double)(vector.X + vector2.X) / 2.0, (double)(vector.Y + vector2.Y) / 2.0, (double)(vector.Z + vector2.Z) / 2.0);
				int num4 = AddVertex(p3);
				m_middlePointIndexCache.Add(key, num4);
				return num4;
			}

			private void Create()
			{
				m_middlePointIndexCache = new Dictionary<long, int>();
				m_triangleIndices = new List<int>();
				m_positions = new List<Vector3>();
				m_index = 0;
				double num = (1.0 + Math.Sqrt(5.0)) / 2.0;
				AddVertex(new Vector3(-1.0, num, 0.0));
				AddVertex(new Vector3(1.0, num, 0.0));
				AddVertex(new Vector3(-1.0, 0.0 - num, 0.0));
				AddVertex(new Vector3(1.0, 0.0 - num, 0.0));
				AddVertex(new Vector3(0.0, -1.0, num));
				AddVertex(new Vector3(0.0, 1.0, num));
				AddVertex(new Vector3(0.0, -1.0, 0.0 - num));
				AddVertex(new Vector3(0.0, 1.0, 0.0 - num));
				AddVertex(new Vector3(num, 0.0, -1.0));
				AddVertex(new Vector3(num, 0.0, 1.0));
				AddVertex(new Vector3(0.0 - num, 0.0, -1.0));
				AddVertex(new Vector3(0.0 - num, 0.0, 1.0));
				List<TriangleIndices> list = new List<TriangleIndices>();
				list.Add(new TriangleIndices(0, 11, 5));
				list.Add(new TriangleIndices(0, 5, 1));
				list.Add(new TriangleIndices(0, 1, 7));
				list.Add(new TriangleIndices(0, 7, 10));
				list.Add(new TriangleIndices(0, 10, 11));
				list.Add(new TriangleIndices(1, 5, 9));
				list.Add(new TriangleIndices(5, 11, 4));
				list.Add(new TriangleIndices(11, 10, 2));
				list.Add(new TriangleIndices(10, 7, 6));
				list.Add(new TriangleIndices(7, 1, 8));
				list.Add(new TriangleIndices(3, 9, 4));
				list.Add(new TriangleIndices(3, 4, 2));
				list.Add(new TriangleIndices(3, 2, 6));
				list.Add(new TriangleIndices(3, 6, 8));
				list.Add(new TriangleIndices(3, 8, 9));
				list.Add(new TriangleIndices(4, 9, 5));
				list.Add(new TriangleIndices(2, 4, 11));
				list.Add(new TriangleIndices(6, 2, 10));
				list.Add(new TriangleIndices(8, 6, 7));
				list.Add(new TriangleIndices(9, 8, 1));
				for (int i = 0; i < 1; i++)
				{
					List<TriangleIndices> list2 = new List<TriangleIndices>();
					foreach (TriangleIndices item in list)
					{
						int middlePoint = GetMiddlePoint(item.v1, item.v2);
						int middlePoint2 = GetMiddlePoint(item.v2, item.v3);
						int middlePoint3 = GetMiddlePoint(item.v3, item.v1);
						list2.Add(new TriangleIndices(item.v1, middlePoint, middlePoint3));
						list2.Add(new TriangleIndices(item.v2, middlePoint2, middlePoint));
						list2.Add(new TriangleIndices(item.v3, middlePoint3, middlePoint2));
						list2.Add(new TriangleIndices(middlePoint, middlePoint2, middlePoint3));
					}
					list = list2;
				}
				foreach (TriangleIndices item2 in list)
				{
					m_triangleIndices.Add(item2.v1);
					m_triangleIndices.Add(item2.v2);
					m_triangleIndices.Add(item2.v3);
				}
			}

			public void AddTrianglesToWorldVertices(Vector3 center, float radius, WorldVerticesInfo worldVertices)
			{
				foreach (int triangleIndex in m_triangleIndices)
				{
					worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + triangleIndex);
				}
				foreach (Vector3 position in m_positions)
				{
					worldVertices.Vertices.Add(center + position * radius);
				}
				worldVertices.VerticesMaxValue += m_positions.Count;
			}
<<<<<<< HEAD

			public int GetTrianglesCount()
			{
				return m_triangleIndices.Count;
			}

			public int GetVerticesCount()
			{
				return m_positions.Count;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public class CapsuleMesh
		{
			private const double P_ID2 = Math.PI / 2.0;

			private const double P_IM2 = Math.PI * 2.0;

			private readonly List<Vector3> m_verticeList = new List<Vector3>();

			private readonly List<int> m_triangleList = new List<int>();

			private readonly int N = 8;

			private readonly float m_radius = 1f;

			private readonly float m_height;

			public CapsuleMesh()
			{
				Create();
			}

			private void Create()
			{
				for (int i = 0; i <= N / 4; i++)
				{
					for (int j = 0; j <= N; j++)
					{
						Vector3 item = default(Vector3);
						double num = (double)j * (Math.PI * 2.0) / (double)N;
						double num2 = -Math.PI / 2.0 + Math.PI * (double)i / (double)(N / 2);
						item.X = m_radius * (float)(Math.Cos(num2) * Math.Cos(num));
						item.Y = m_radius * (float)(Math.Cos(num2) * Math.Sin(num));
						item.Z = m_radius * (float)Math.Sin(num2) - m_height / 2f;
						m_verticeList.Add(item);
					}
				}
				for (int i = N / 4; i <= N / 2; i++)
				{
					for (int j = 0; j <= N; j++)
					{
						Vector3 item2 = default(Vector3);
						double num = (double)j * (Math.PI * 2.0) / (double)N;
						double num2 = -Math.PI / 2.0 + Math.PI * (double)i / (double)(N / 2);
						item2.X = m_radius * (float)(Math.Cos(num2) * Math.Cos(num));
						item2.Y = m_radius * (float)(Math.Cos(num2) * Math.Sin(num));
						item2.Z = m_radius * (float)Math.Sin(num2) + m_height / 2f;
						m_verticeList.Add(item2);
					}
				}
				for (int i = 0; i <= N / 2; i++)
				{
					for (int j = 0; j < N; j++)
					{
						int item3 = i * (N + 1) + j;
						int item4 = i * (N + 1) + (j + 1);
						int item5 = (i + 1) * (N + 1) + (j + 1);
						int item6 = (i + 1) * (N + 1) + j;
						m_triangleList.Add(item3);
						m_triangleList.Add(item4);
						m_triangleList.Add(item5);
						m_triangleList.Add(item3);
						m_triangleList.Add(item5);
						m_triangleList.Add(item6);
					}
				}
			}

			public void AddTrianglesToWorldVertices(Matrix transformMatrix, float radius, Line axisLine, WorldVerticesInfo worldVertices)
			{
				Matrix matrix = Matrix.CreateFromDir(axisLine.Direction);
				Vector3 translation = transformMatrix.Translation;
				transformMatrix.Translation = Vector3.Zero;
				int num = m_verticeList.Count / 2;
				Vector3 vector = new Vector3(0f, 0f, axisLine.Length * 0.5f);
				for (int i = 0; i < num; i++)
				{
					worldVertices.Vertices.Add(Vector3.Transform(translation + m_verticeList[i] * radius - vector, matrix));
				}
				for (int j = num; j < m_verticeList.Count; j++)
				{
					worldVertices.Vertices.Add(Vector3.Transform(translation + m_verticeList[j] * radius + vector, matrix));
				}
				foreach (int triangle in m_triangleList)
				{
					worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + triangle);
				}
				worldVertices.VerticesMaxValue += m_verticeList.Count;
			}
		}

		private readonly bool ENABLE_GRID_PATHFINDING = true;

		private static readonly IcoSphereMesh m_icosphereMesh = new IcoSphereMesh();

		private static readonly CapsuleMesh m_capsuleMesh = new CapsuleMesh();

		private static List<HkShape> m_tmpShapes;

		private const int NAVMESH_LOD = 0;

		private readonly Dictionary<Vector3I, MyIsoMesh> m_meshCache = new Dictionary<Vector3I, MyIsoMesh>(1024, new Vector3I.EqualityComparer());

		private readonly List<CacheInterval> m_invalidateMeshCacheCoord = new List<CacheInterval>();

		private readonly List<CacheInterval> m_tmpInvalidCache = new List<CacheInterval>();

		private readonly MyPlanet m_planet;

		private readonly Vector3D m_center;

		private readonly Quaternion m_rdWorldQuaternion;

		private readonly MyRDPathfinding m_rdPathfinding;

		private readonly List<GridInfo> m_lastGridsInfo = new List<GridInfo>();

		private readonly List<CubeInfo> m_lastIntersectedGridsInfoCubes = new List<CubeInfo>();

		public MyNavigationInputMesh(MyRDPathfinding rdPathfinding, MyPlanet planet, Vector3D center)
		{
			m_rdPathfinding = rdPathfinding;
			m_planet = planet;
			m_center = center;
			Vector3 vector = -(Vector3)Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(m_center));
			Vector3 forward = Vector3.CalculatePerpendicularVector(vector);
			m_rdWorldQuaternion = Quaternion.Inverse(Quaternion.CreateFromForwardUp(forward, vector));
		}

<<<<<<< HEAD
		/// <summary>
		/// Creates new WorldVerticesInfo with VoxelMap entities and shapes.
		/// This method needs to be called on the main thread so that it can access HkShape.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void CreateWorldVerticesAndShapes(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<MyVoxelMap> trackedEntities, out List<BoundingBoxD> boundingBoxes, out WorldVerticesInfo worldVertices, out MyShapesInfo myShapesInfo)
		{
			worldVertices = new WorldVerticesInfo();
			myShapesInfo = new MyShapesInfo();
			boundingBoxes = new List<BoundingBoxD>();
			AddEntities(border, originPosition, obb, boundingBoxes, trackedEntities, worldVertices, myShapesInfo);
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds ground vertices to world vertices and returns world vertices.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public WorldVerticesInfo AddGroundToWorldVertices(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> boundingBoxes, WorldVerticesInfo worldVertices)
		{
			AddGround(border, originPosition, obb, boundingBoxes, worldVertices);
			return worldVertices;
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds physical shapes to world vertices and returns world vertices.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public WorldVerticesInfo AddPhysicalShapeVertices(MyShapesInfo shapesInfo, WorldVerticesInfo worldVertices)
		{
			AddPhysicalShapes(worldVertices, shapesInfo);
			return worldVertices;
		}

		public void AddTrees(MyOrientedBoundingBoxD obb, List<MyBoxShapeInfo> boxShapes)
		{
			MyEnvironmentSector myEnvironmentSector = m_planet.Components.Get<MyPlanetEnvironmentComponent>()?.GetSectorForPosition(obb.Center);
			if (myEnvironmentSector?.DataView == null)
			{
				return;
			}
			foreach (ItemInfo item in myEnvironmentSector.DataView.Items)
			{
				if (myEnvironmentSector.EnvironmentDefinition.Items.TryGetValue(item.DefinitionIndex, out var value) && !(value.Type.Name != "Tree"))
				{
					Vector3D vector3D = item.Position + myEnvironmentSector.SectorCenter;
					if (obb.GetAABB().Contains(vector3D) == ContainmentType.Contains)
					{
<<<<<<< HEAD
						MatrixD value2 = MatrixD.CreateTranslation(vector3D);
						value2.Translation -= m_center;
						MatrixD m = MatrixD.Transform(value2, m_rdWorldQuaternion);
						boxShapes.Add(new MyBoxShapeInfo(m, 2f, 2f, 15f));
=======
						Matrix m = Matrix.CreateTranslation(vector3D);
						m.Translation -= m_center;
						MatrixD m2 = MatrixD.Transform(m, m_rdWorldQuaternion);
						boxShapes.Add(new MyBoxShapeInfo(m2, 2f, 2f, 15f));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public void DebugDraw()
		{
			foreach (GridInfo item in m_lastGridsInfo)
			{
				foreach (CubeInfo cube in item.Cubes)
				{
					if (m_lastIntersectedGridsInfoCubes.Contains(cube))
					{
						MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.White);
					}
					else
					{
						MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.Yellow);
					}
				}
			}
		}

		public void InvalidateCache(BoundingBoxD box)
		{
			Vector3D xyz = Vector3D.Transform(box.Min, m_planet.PositionComp.WorldMatrixInvScaled);
			Vector3D xyz2 = Vector3D.Transform(box.Max, m_planet.PositionComp.WorldMatrixInvScaled);
			xyz += m_planet.SizeInMetresHalf;
			xyz2 += m_planet.SizeInMetresHalf;
			Vector3I voxelCoord = new Vector3I(xyz);
			Vector3I voxelCoord2 = new Vector3I(xyz2);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out var geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out var geometryCellCoord2);
			m_invalidateMeshCacheCoord.Add(new CacheInterval
			{
				Min = geometryCellCoord,
				Max = geometryCellCoord2
			});
		}

		public void RefreshCache()
		{
			m_meshCache.Clear();
		}

		public void Clear()
		{
			m_meshCache.Clear();
		}

		private void AddEntities(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> boundingBoxes, List<MyVoxelMap> trackedEntities, WorldVerticesInfo worldVertices, MyShapesInfo myShapesInfo)
		{
			obb.HalfExtent += new Vector3D(border, 0.0, border);
			BoundingBoxD box = obb.GetAABB();
			AddTrees(obb, myShapesInfo.Boxes);
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, list, MyEntityQueryType.Static);
			foreach (MyEntity item in list)
			{
				using (item.Pin())
				{
					if (!item.MarkedForClose)
					{
						MyCubeGrid grid;
						MyVoxelMap myVoxelMap;
						if (ENABLE_GRID_PATHFINDING && (grid = item as MyCubeGrid) != null)
						{
							AddGridVerticesInsideOBB(grid, obb, myShapesInfo);
						}
						else if ((myVoxelMap = item as MyVoxelMap) != null)
						{
							trackedEntities.Add(myVoxelMap);
							AddVoxelVertices(myVoxelMap, border, originPosition, obb, boundingBoxes, worldVertices);
						}
					}
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Adds the vertices from the grid blocks that are inside the given OBB
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="obb"></param>
		/// <param name="myShapesInfo"></param>
		private void AddGridVerticesInsideOBB(MyCubeGrid grid, MyOrientedBoundingBoxD obb, MyShapesInfo myShapesInfo)
		{
			BoundingBoxD aABB = obb.GetAABB();
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(grid).Nodes)
			{
				MyCubeGrid nodeData = node.NodeData;
				m_rdPathfinding.AddToTrackedGrids(nodeData);
				MatrixD worldMatrix = nodeData.WorldMatrix;
				worldMatrix.Translation -= m_center;
				MatrixD m = MatrixD.Transform(worldMatrix, m_rdWorldQuaternion);
				if (MyPerGameSettings.Game != GameEnum.SE_GAME)
				{
					continue;
				}
				BoundingBoxD boundingBoxD = aABB.TransformFast(nodeData.PositionComp.WorldMatrixNormalizedInv);
				Vector3I value = new Vector3I((int)Math.Round(boundingBoxD.Min.X), (int)Math.Round(boundingBoxD.Min.Y), (int)Math.Round(boundingBoxD.Min.Z));
				Vector3I value2 = new Vector3I((int)Math.Round(boundingBoxD.Max.X), (int)Math.Round(boundingBoxD.Max.Y), (int)Math.Round(boundingBoxD.Max.Z));
				value = Vector3I.Min(value, value2);
				value2 = Vector3I.Max(value, value2);
				if (nodeData.Physics == null)
				{
					continue;
				}
				using (MyUtils.ReuseCollection(ref m_tmpShapes))
				{
					nodeData.Physics.Shape.GetShapesInInterval(value, value2, m_tmpShapes);
					foreach (HkShape tmpShape in m_tmpShapes)
					{
						ParsePhysicalShape(tmpShape, m, myShapesInfo.Boxes, myShapesInfo.Spheres, myShapesInfo.ConvexVertices);
					}
				}
			}
=======
		private void AddGridVerticesInsideOBB(MyCubeGrid grid, MyOrientedBoundingBoxD obb, MyShapesInfo myShapesInfo)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			BoundingBoxD aABB = obb.GetAABB();
			Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Logical.GetGroup(grid).Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCubeGrid nodeData = enumerator.get_Current().NodeData;
					m_rdPathfinding.AddToTrackedGrids(nodeData);
					MatrixD worldMatrix = nodeData.WorldMatrix;
					worldMatrix.Translation -= m_center;
					MatrixD m = MatrixD.Transform(worldMatrix, m_rdWorldQuaternion);
					if (MyPerGameSettings.Game != GameEnum.SE_GAME)
					{
						continue;
					}
					BoundingBoxD boundingBoxD = aABB.TransformFast(nodeData.PositionComp.WorldMatrixNormalizedInv);
					Vector3I value = new Vector3I((int)Math.Round(boundingBoxD.Min.X), (int)Math.Round(boundingBoxD.Min.Y), (int)Math.Round(boundingBoxD.Min.Z));
					Vector3I value2 = new Vector3I((int)Math.Round(boundingBoxD.Max.X), (int)Math.Round(boundingBoxD.Max.Y), (int)Math.Round(boundingBoxD.Max.Z));
					value = Vector3I.Min(value, value2);
					value2 = Vector3I.Max(value, value2);
					if (nodeData.Physics == null)
					{
						continue;
					}
					using (MyUtils.ReuseCollection(ref m_tmpShapes))
					{
						nodeData.Physics.Shape.GetShapesInInterval(value, value2, m_tmpShapes);
						foreach (HkShape tmpShape in m_tmpShapes)
						{
							ParsePhysicalShape(tmpShape, m, myShapesInfo.Boxes, myShapesInfo.Spheres, myShapesInfo.ConvexVertices);
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void ParsePhysicalShape(HkShape shape, Matrix rdWorldMatrix, List<MyBoxShapeInfo> boxes, List<MySphereShapeInfo> spheres, List<MyConvexVerticesInfo> convexVertices)
		{
			while (true)
			{
				switch (shape.ShapeType)
				{
				default:
					return;
				case HkShapeType.Box:
				{
					HkBoxShape hkBoxShape = (HkBoxShape)shape;
					boxes.Add(new MyBoxShapeInfo(rdWorldMatrix, hkBoxShape.HalfExtents.X, hkBoxShape.HalfExtents.Y, hkBoxShape.HalfExtents.Z));
					return;
				}
				case HkShapeType.List:
				{
					HkShapeContainerIterator iterator = ((HkListShape)shape).GetIterator();
					while (iterator.IsValid)
					{
						ParsePhysicalShape(iterator.CurrentValue, rdWorldMatrix, boxes, spheres, convexVertices);
						iterator.Next();
					}
					return;
				}
				case HkShapeType.Mopp:
					shape = ((HkMoppBvTreeShape)shape).ShapeCollection;
					break;
				case HkShapeType.ConvexTransform:
				{
					HkConvexTransformShape hkConvexTransformShape = (HkConvexTransformShape)shape;
					shape = hkConvexTransformShape.ChildShape;
					rdWorldMatrix = hkConvexTransformShape.Transform * rdWorldMatrix;
					break;
				}
				case HkShapeType.ConvexTranslate:
				{
					HkConvexTranslateShape hkConvexTranslateShape = (HkConvexTranslateShape)shape;
					Matrix matrix = Matrix.CreateTranslation(hkConvexTranslateShape.Translation);
					shape = hkConvexTranslateShape.ChildShape;
					rdWorldMatrix = matrix * rdWorldMatrix;
					break;
				}
				case HkShapeType.Sphere:
					spheres.Add(new MySphereShapeInfo(rdWorldMatrix.Translation, ((HkSphereShape)shape).Radius));
					return;
				case HkShapeType.ConvexVertices:
				{
					((HkConvexVerticesShape)shape).GetVertices(out var vertices);
					convexVertices.Add(new MyConvexVerticesInfo(rdWorldMatrix, vertices));
					return;
				}
				case HkShapeType.Cylinder:
				case HkShapeType.Triangle:
				case HkShapeType.Capsule:
				case HkShapeType.TriSampledHeightFieldCollection:
				case HkShapeType.TriSampledHeightFieldBvTree:
					return;
				}
			}
		}

		private static void AddPhysicalShapes(WorldVerticesInfo worldVertices, MyShapesInfo shapesInfo)
		{
<<<<<<< HEAD
			int num = 0;
			int num2 = 0;
			foreach (MyConvexVerticesInfo convexVertex in shapesInfo.ConvexVertices)
			{
				num += convexVertex.Vertices.Length;
				num2 += convexVertex.Vertices.Length;
			}
			num += 8 * shapesInfo.Boxes.Count;
			num2 += 36 * shapesInfo.Boxes.Count;
			foreach (MySphereShapeInfo sphere in shapesInfo.Spheres)
			{
				_ = sphere;
				num += m_icosphereMesh.GetVerticesCount();
				num2 += m_icosphereMesh.GetTrianglesCount();
			}
			worldVertices.Vertices.EnsureCapacity(num);
			worldVertices.Triangles.EnsureCapacity(num2);
			foreach (MyConvexVerticesInfo convexVertex2 in shapesInfo.ConvexVertices)
			{
				HkConvexVerticesShape hkConvexVerticesShape = new HkConvexVerticesShape(convexVertex2.Vertices, convexVertex2.Vertices.Length);
=======
			foreach (MyConvexVerticesInfo convexVertex in shapesInfo.ConvexVertices)
			{
				HkConvexVerticesShape hkConvexVerticesShape = new HkConvexVerticesShape(convexVertex.Vertices, convexVertex.Vertices.Length);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				using (HkGeometry hkGeometry = new HkGeometry())
				{
					hkConvexVerticesShape.GetGeometry(hkGeometry, out var _);
					for (int i = 0; i < hkGeometry.TriangleCount; i++)
					{
						hkGeometry.GetTriangle(i, out var i2, out var i3, out var i4, out var _);
						worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i2);
						worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i3);
						worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i4);
					}
					for (int j = 0; j < hkGeometry.VertexCount; j++)
					{
						Vector3 position = hkGeometry.GetVertex(j);
<<<<<<< HEAD
						Vector3.Transform(ref position, ref convexVertex2.m_rdWorldMatrix, out position);
=======
						Vector3.Transform(ref position, ref convexVertex.m_rdWorldMatrix, out position);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						worldVertices.Vertices.Add(position);
					}
					worldVertices.VerticesMaxValue += hkGeometry.VertexCount;
				}
				hkConvexVerticesShape.Base.RemoveReference();
			}
			foreach (MyBoxShapeInfo box in shapesInfo.Boxes)
			{
				Vector3D min = new Vector3D(0f - box.HalfExtentsX, 0f - box.HalfExtentsY, 0f - box.HalfExtentsZ);
				Vector3D max = new Vector3D(box.HalfExtentsX, box.HalfExtentsY, box.HalfExtentsZ);
				BoundingBoxToTranslatedTriangles(new BoundingBoxD(min, max), box.RdWorldMatrix, worldVertices);
			}
<<<<<<< HEAD
			foreach (MySphereShapeInfo sphere2 in shapesInfo.Spheres)
			{
				m_icosphereMesh.AddTrianglesToWorldVertices(sphere2.RdWorldTranslation, sphere2.Radius, worldVertices);
			}
		}

		/// <summary>
		/// Adds the vertices from the physical body (rock) that is inside the given OBB
		/// </summary>
		/// <param name="voxelMap"></param>
		/// <param name="border"></param>
		/// <param name="originPosition"></param>
		/// <param name="obb"></param>
		/// <param name="bbList"></param>
		/// <param name="worldVertices"></param>
=======
			foreach (MySphereShapeInfo sphere in shapesInfo.Spheres)
			{
				m_icosphereMesh.AddTrianglesToWorldVertices(sphere.RdWorldTranslation, sphere.Radius, worldVertices);
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void AddVoxelVertices(MyVoxelMap voxelMap, float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList, WorldVerticesInfo worldVertices)
		{
			AddVoxelMesh(voxelMap, voxelMap.Storage, null, border, originPosition, obb, bbList, worldVertices);
		}

		private static void BoundingBoxToTranslatedTriangles(BoundingBoxD bbox, Matrix worldMatrix, WorldVerticesInfo worldVertices)
		{
			Vector3 position = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Max.Z);
			Vector3 position2 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Max.Z);
			Vector3 position3 = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);
			Vector3 position4 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Min.Z);
			Vector3 position5 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Max.Z);
			Vector3 position6 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Max.Z);
			Vector3 position7 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Min.Z);
			Vector3 position8 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Min.Z);
			Vector3.Transform(ref position, ref worldMatrix, out position);
			Vector3.Transform(ref position2, ref worldMatrix, out position2);
			Vector3.Transform(ref position3, ref worldMatrix, out position3);
			Vector3.Transform(ref position4, ref worldMatrix, out position4);
			Vector3.Transform(ref position5, ref worldMatrix, out position5);
			Vector3.Transform(ref position6, ref worldMatrix, out position6);
			Vector3.Transform(ref position7, ref worldMatrix, out position7);
			Vector3.Transform(ref position8, ref worldMatrix, out position8);
			worldVertices.Vertices.Add(position);
			worldVertices.Vertices.Add(position2);
			worldVertices.Vertices.Add(position3);
			worldVertices.Vertices.Add(position4);
			worldVertices.Vertices.Add(position5);
			worldVertices.Vertices.Add(position6);
			worldVertices.Vertices.Add(position7);
			worldVertices.Vertices.Add(position8);
			int verticesMaxValue = worldVertices.VerticesMaxValue;
			int item = worldVertices.VerticesMaxValue + 1;
			int item2 = worldVertices.VerticesMaxValue + 2;
			int item3 = worldVertices.VerticesMaxValue + 3;
			int item4 = worldVertices.VerticesMaxValue + 4;
			int item5 = worldVertices.VerticesMaxValue + 5;
			int item6 = worldVertices.VerticesMaxValue + 6;
			int item7 = worldVertices.VerticesMaxValue + 7;
			worldVertices.Triangles.Add(item3);
			worldVertices.Triangles.Add(item2);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(item);
			worldVertices.Triangles.Add(item3);
			worldVertices.Triangles.Add(item4);
			worldVertices.Triangles.Add(item6);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(item5);
			worldVertices.Triangles.Add(item4);
			worldVertices.Triangles.Add(item2);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(item6);
			worldVertices.Triangles.Add(item2);
			worldVertices.Triangles.Add(item3);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(item4);
			worldVertices.Triangles.Add(item5);
			worldVertices.Triangles.Add(item5);
			worldVertices.Triangles.Add(item);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(item6);
			worldVertices.Triangles.Add(item4);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(verticesMaxValue);
			worldVertices.Triangles.Add(item2);
			worldVertices.Triangles.Add(item6);
			worldVertices.Triangles.Add(item);
			worldVertices.Triangles.Add(item5);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(item7);
			worldVertices.Triangles.Add(item3);
			worldVertices.Triangles.Add(item);
			worldVertices.VerticesMaxValue += 8;
		}

		private static void AddMeshTriangles(MyIsoMesh mesh, Vector3 offset, Matrix rotation, Matrix ownRotation, WorldVerticesInfo worldVertices)
		{
<<<<<<< HEAD
			worldVertices.Vertices.EnsureCapacity(mesh.VerticesCount);
			worldVertices.Triangles.EnsureCapacity(mesh.TrianglesCount);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < mesh.TrianglesCount; i++)
			{
				ushort v = mesh.Triangles[i].V0;
				ushort v2 = mesh.Triangles[i].V1;
				ushort v3 = mesh.Triangles[i].V2;
				worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + v3);
				worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + v2);
				worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + v);
			}
			for (int j = 0; j < mesh.VerticesCount; j++)
			{
				mesh.GetUnpackedPosition(j, out var position);
				Vector3.Transform(ref position, ref ownRotation, out position);
				position -= offset;
				Vector3.Transform(ref position, ref rotation, out position);
				worldVertices.Vertices.Add(position);
			}
			worldVertices.VerticesMaxValue += mesh.VerticesCount;
		}

		private unsafe void GetMiddleOBBLocalPoints(MyOrientedBoundingBoxD obb, ref Vector3* points)
		{
			Vector3 vector = obb.Orientation.Right * (float)obb.HalfExtent.X;
			Vector3 vector2 = obb.Orientation.Forward * (float)obb.HalfExtent.Z;
			Vector3 vector3 = obb.Center - m_planet.PositionComp.GetPosition();
			*points = vector3 - vector - vector2;
			points[1] = vector3 + vector - vector2;
			points[2] = vector3 + vector + vector2;
			points[3] = vector3 - vector + vector2;
		}

<<<<<<< HEAD
		/// <summary>
		/// Changes the given OBB so it is Y bounded by the range where surface may exist
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private unsafe bool SetTerrainLimits(ref MyOrientedBoundingBoxD obb)
		{
			Vector3* points = stackalloc Vector3[4];
			GetMiddleOBBLocalPoints(obb, ref points);
			m_planet.Provider.Shape.GetBounds(points, 4, out var minHeight, out var maxHeight);
			if (minHeight.IsValid() && maxHeight.IsValid())
			{
				Vector3D vector3D = obb.Orientation.Up * minHeight + m_planet.PositionComp.GetPosition();
				Vector3D vector3D2 = obb.Orientation.Up * maxHeight + m_planet.PositionComp.GetPosition();
				obb.Center = (vector3D + vector3D2) * 0.5;
				float num = Math.Max(maxHeight - minHeight, 1f);
				obb.HalfExtent.Y = num * 0.5f;
				return true;
			}
			return false;
		}

		private void AddGround(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList, WorldVerticesInfo worldVertices)
		{
			if (SetTerrainLimits(ref obb))
			{
				AddVoxelMesh(m_planet, m_planet.Storage, m_meshCache, border, originPosition, obb, bbList, worldVertices);
			}
		}

		private void CheckCacheValidity()
		{
			if (m_invalidateMeshCacheCoord.Count <= 0)
			{
				return;
			}
			m_tmpInvalidCache.AddRange(m_invalidateMeshCacheCoord);
			m_invalidateMeshCacheCoord.Clear();
			foreach (CacheInterval item in m_tmpInvalidCache)
			{
				for (int i = 0; i < m_meshCache.Count; i++)
				{
<<<<<<< HEAD
					Vector3I key = m_meshCache.ElementAt(i).Key;
=======
					Vector3I key = Enumerable.ElementAt<KeyValuePair<Vector3I, MyIsoMesh>>((IEnumerable<KeyValuePair<Vector3I, MyIsoMesh>>)m_meshCache, i).Key;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (key.X >= item.Min.X && key.Y >= item.Min.Y && key.Z >= item.Min.Z && key.X <= item.Max.X && key.Y <= item.Max.Y && key.Z <= item.Max.Z)
					{
						m_meshCache.Remove(key);
						break;
					}
				}
			}
			m_tmpInvalidCache.Clear();
		}

		private void AddVoxelMesh(MyVoxelBase voxelBase, IMyStorage storage, Dictionary<Vector3I, MyIsoMesh> cache, float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList, WorldVerticesInfo worldVertices)
		{
			bool flag = cache != null;
			if (flag)
			{
				CheckCacheValidity();
			}
			obb.HalfExtent += new Vector3D(border, 0.0, border);
			BoundingBoxD aABB = obb.GetAABB();
			int num = (int)Math.Round(aABB.HalfExtents.Max() * 2.0);
			aABB = new BoundingBoxD(aABB.Min, aABB.Min + num);
			aABB.Translate(obb.Center - aABB.Center);
			bbList.Add(new BoundingBoxD(aABB.Min, aABB.Max));
			aABB = aABB.TransformFast(voxelBase.PositionComp.WorldMatrixInvScaled);
			aABB.Translate(voxelBase.SizeInMetresHalf);
			Vector3I voxelCoord = Vector3I.Round(aABB.Min);
			Vector3I voxelCoord2 = voxelCoord + num;
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out var geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out var geometryCellCoord2);
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = obb;
			myOrientedBoundingBoxD.Transform(voxelBase.PositionComp.WorldMatrixInvScaled);
			myOrientedBoundingBoxD.Center += voxelBase.SizeInMetresHalf;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref geometryCellCoord, ref geometryCellCoord2);
			MyCellCoord myCellCoord = default(MyCellCoord);
			myCellCoord.Lod = 0;
			int num2 = 0;
			Vector3 offset = originPosition - voxelBase.PositionLeftBottomCorner;
			Vector3 vector = -Vector3.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(originPosition));
			Matrix rotation = Matrix.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3.CalculatePerpendicularVector(vector), vector)));
			MatrixD m = voxelBase.PositionComp.WorldMatrixRef.GetOrientation();
			Matrix ownRotation = m;
			while (vector3I_RangeIterator.IsValid())
			{
				if (flag && cache.TryGetValue(vector3I_RangeIterator.Current, out var value))
				{
					if (value != null)
					{
						AddMeshTriangles(value, offset, rotation, ownRotation, worldVertices);
					}
					vector3I_RangeIterator.MoveNext();
					continue;
				}
				myCellCoord.CoordInLod = vector3I_RangeIterator.Current;
				MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref myCellCoord.CoordInLod, out var localAABB);
				if (!myOrientedBoundingBoxD.Intersects(ref localAABB))
				{
					num2++;
					vector3I_RangeIterator.MoveNext();
					continue;
				}
				BoundingBoxD item = new BoundingBoxD(localAABB.Min, localAABB.Max).Translate(-voxelBase.SizeInMetresHalf);
				bbList.Add(item);
				Vector3I vector3I = myCellCoord.CoordInLod * 8 - 1;
				Vector3I lodVoxelMax = vector3I + 8 + 1 + 1;
				MyIsoMesh myIsoMesh = MyPrecalcComponent.IsoMesher.Precalc(storage, 0, vector3I, lodVoxelMax, MyStorageDataTypeFlags.Content);
				if (flag)
				{
					cache[vector3I_RangeIterator.Current] = myIsoMesh;
				}
				if (myIsoMesh != null)
				{
					AddMeshTriangles(myIsoMesh, offset, rotation, ownRotation, worldVertices);
				}
				vector3I_RangeIterator.MoveNext();
			}
		}
	}
}
