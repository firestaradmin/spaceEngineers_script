using System;
using System.Collections.Generic;
using System.Threading;
using Havok;
using ParallelTasks;
using RecastDetour;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Groups;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	public class MyExternalPathfinding : IMyPathfinding
	{
		private class GeometryCenterPair
		{
			public HkGeometry Geometry { get; set; }

			public Vector3D Center { get; set; }
		}

		public enum OBBCorner
		{
			UpperFrontLeft,
			UpperBackLeft,
			LowerBackLeft,
			LowerFrontLeft,
			UpperFrontRight,
			UpperBackRight,
			LowerBackRight,
			LowerFrontRight
		}

		private struct Vertex
		{
			public Vector3D pos;

			public Color color;
		}

		private MyRecastOptions m_recastOptions;

		private readonly List<MyRecastDetourPolygon> m_polygons = new List<MyRecastDetourPolygon>();

		private Vector3D m_meshCenter;

<<<<<<< HEAD
=======
		private Vector3D m_currentCenter;

		private int m_meshMaxSize;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_singleTileSize;

		private int m_singleTileHeight;

		private int m_tileLineCount;

		private bool m_isNavmeshInitialized;

		private MyNavmeshOBBs m_navmeshOBBs;

		private MyRDWrapper m_rdWrapper;

		private readonly List<MyRDPath> m_debugDrawPaths = new List<MyRDPath>();

		private readonly List<BoundingBoxD> m_lastGroundMeshQuery = new List<BoundingBoxD>();

		private readonly Dictionary<string, GeometryCenterPair> m_cachedGeometry = new Dictionary<string, GeometryCenterPair>();

		private bool m_drawMesh;

		private bool m_isNavmeshCreationRunning;

		private Vector3D? m_pathfindingDebugTarget;

		private List<MyNavmeshOBBs.OBBCoords> m_debugDrawIntersectedOBBs = new List<MyNavmeshOBBs.OBBCoords>();

		private List<MyFormatPositionColor> m_visualNavmesh = new List<MyFormatPositionColor>();

		private List<MyFormatPositionColor> m_newVisualNavmesh;

		private uint m_drawNavmeshId = uint.MaxValue;

		public bool DrawDebug { get; set; }

		public bool DrawPhysicalMesh { get; set; }

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
					DrawPersistentDebugNavmesh(force: true);
				}
				else
				{
					HidePersistentDebugNavmesh();
				}
			}
		}

		public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity relativeEntity)
		{
			return null;
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
		}

		public void UnloadData()
		{
			HidePersistentDebugNavmesh();
			m_visualNavmesh.Clear();
			m_newVisualNavmesh?.Clear();
			m_newVisualNavmesh = null;
		}

		public void DebugDraw()
		{
			DebugDrawInternal();
			int count = m_debugDrawPaths.Count;
			int num = 0;
			while (num < count)
			{
				MyRDPath myRDPath = m_debugDrawPaths[num];
				if (!myRDPath.IsValid || myRDPath.PathCompleted)
				{
					m_debugDrawPaths.RemoveAt(num);
					count = m_debugDrawPaths.Count;
				}
				else
				{
					myRDPath.DebugDraw();
					num++;
				}
			}
		}

		public static Vector3D GetOBBCorner(MyOrientedBoundingBoxD obb, OBBCorner corner)
		{
			Vector3D[] array = new Vector3D[8];
			obb.GetCorners(array, 0);
			return array[(int)corner];
		}

		public static List<Vector3D> GetOBBCorners(MyOrientedBoundingBoxD obb, List<OBBCorner> corners)
		{
			Vector3D[] array = new Vector3D[8];
			obb.GetCorners(array, 0);
			List<Vector3D> list = new List<Vector3D>();
			foreach (OBBCorner corner in corners)
			{
				list.Add(array[(int)corner]);
			}
			return list;
		}

		public void InitializeNavmesh(Vector3D center)
		{
			m_isNavmeshInitialized = true;
			float cellSize = 0.2f;
			m_singleTileSize = 20;
			m_tileLineCount = 50;
			m_singleTileHeight = 70;
			m_recastOptions = new MyRecastOptions
			{
				cellHeight = 0.2f,
				agentHeight = 1.5f,
				agentRadius = 0.5f,
				agentMaxClimb = 0.5f,
				agentMaxSlope = 50f,
				regionMinSize = 1f,
				regionMergeSize = 10f,
				edgeMaxLen = 50f,
				edgeMaxError = 3f,
				vertsPerPoly = 6f,
				detailSampleDist = 6f,
				detailSampleMaxError = 1f,
				partitionType = 1
			};
			float num = (float)m_singleTileSize * 0.5f + (float)m_singleTileSize * (float)Math.Floor((float)m_tileLineCount * 0.5f);
			float num2 = (float)m_singleTileHeight * 0.5f;
			float[] bMin = new float[3]
			{
				0f - num,
				0f - num2,
				0f - num
			};
			float[] bMax = new float[3] { num, num2, num };
			m_rdWrapper = new MyRDWrapper();
			m_rdWrapper.Init(cellSize, m_singleTileSize, bMin, bMax);
			Vector3D forwardDirection = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center)));
			UnloadData();
			m_navmeshOBBs = new MyNavmeshOBBs(GetPlanet(center), center, forwardDirection, m_tileLineCount, m_singleTileSize, m_singleTileHeight);
			m_meshCenter = center;
			m_visualNavmesh.Clear();
		}

		public void StartNavmeshTileCreation(List<MyNavmeshOBBs.OBBCoords> obbList)
		{
			if (!m_isNavmeshCreationRunning)
			{
				m_isNavmeshCreationRunning = true;
				Parallel.Start(delegate
				{
					GenerateTiles(obbList);
				});
			}
		}

		private MyPlanet GetPlanet(Vector3D position)
		{
			int num = 100;
			BoundingBoxD box = new BoundingBoxD(position - (float)num * 0.5f, position + (float)num * 0.5f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		private void GenerateTiles(List<MyNavmeshOBBs.OBBCoords> obbList)
		{
			MyPlanet planet = GetPlanet(m_meshCenter);
			foreach (MyNavmeshOBBs.OBBCoords obb in obbList)
			{
				MyOrientedBoundingBoxD oBB = obb.OBB;
				Vector3D pos = WorldPositionToLocalNavmeshPosition(oBB.Center, 0f);
				if (!m_rdWrapper.TileAlreadyGenerated(pos))
				{
					List<Vector3D> list = new List<Vector3D>();
					int num = list.Count / 3;
					float[] array = new float[list.Count * 3];
					int i = 0;
					int num2 = 0;
					for (; i < list.Count; i++)
					{
						array[num2++] = (float)list[i].X;
						array[num2++] = (float)list[i].Y;
						array[num2++] = (float)list[i].Z;
					}
					int[] array2 = new int[num * 3];
					for (int j = 0; j < num * 3; j++)
					{
						array2[j] = j;
					}
					m_polygons.Clear();
					if (num > 0)
					{
						List<MyFormatPositionColor> list2 = new List<MyFormatPositionColor>();
						GenerateDebugDrawPolygonNavmesh(planet, oBB, list2, obb.Coords.X, obb.Coords.Y);
						m_newVisualNavmesh = list2;
						Thread.Sleep(10);
					}
				}
			}
			m_isNavmeshCreationRunning = false;
		}

		private void GenerateDebugDrawPolygonNavmesh(MyPlanet planet, MyOrientedBoundingBoxD obb, List<MyFormatPositionColor> navmesh, int xCoord, int yCoord)
		{
			int num = 10;
			int num2 = 0;
			int num3 = 95;
			int num4 = 10;
			foreach (MyRecastDetourPolygon polygon in m_polygons)
			{
				Vector3[] vertices = polygon.Vertices;
				foreach (Vector3 vector in vertices)
				{
					Vector3D vector3D = LocalNavmeshPositionToWorldPosition(obb, vector, m_meshCenter, Vector3D.Zero);
					MyFormatPositionColor myFormatPositionColor = default(MyFormatPositionColor);
					myFormatPositionColor.Position = vector3D;
					myFormatPositionColor.Color = new Color(0, num + num2, 0);
					MyFormatPositionColor item = myFormatPositionColor;
					navmesh.Add(item);
				}
				num2 += num4;
				num2 %= num3;
			}
		}

		private static MatrixD LocalNavmeshPositionToWorldPositionTransform(MyOrientedBoundingBoxD obb, Vector3D center)
		{
			Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
			return MatrixD.CreateFromQuaternion(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D));
		}

		private Vector3D LocalNavmeshPositionToWorldPosition(MyOrientedBoundingBoxD obb, Vector3D position, Vector3D center, Vector3D heightIncrease)
		{
			MatrixD matrix = LocalNavmeshPositionToWorldPositionTransform(obb, center);
			return Vector3D.Transform(position, matrix) + m_meshCenter;
		}

		public void SetTarget(Vector3D? target)
		{
			m_pathfindingDebugTarget = target;
		}

		private Vector3D WorldPositionToLocalNavmeshPosition(Vector3D position, float heightIncrease)
		{
			MyOrientedBoundingBoxD? oBB = m_navmeshOBBs.GetOBB(position);
			if (oBB.HasValue)
			{
				_ = oBB.Value;
			}
			else
			{
				_ = m_meshCenter;
			}
			Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(m_meshCenter));
			MatrixD matrix = MatrixD.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D)));
			return Vector3D.Transform(position - m_meshCenter + heightIncrease * vector3D, matrix);
		}

		private Vector3D LocalPositionToWorldPosition(Vector3D position)
		{
			Vector3D vector3D = position;
			if (m_navmeshOBBs != null)
			{
				vector3D = m_meshCenter;
			}
			Vector3D vector3D2 = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D));
			return LocalNavmeshPositionToWorldPosition(m_navmeshOBBs.CenterOBB, position, vector3D, 0.5 * vector3D2);
		}

		public List<Vector3D> GetPathPoints(Vector3D initialPosition, Vector3D targetPosition)
		{
			List<Vector3D> list = new List<Vector3D>();
			if (m_isNavmeshCreationRunning)
			{
				return list;
			}
			if (!m_isNavmeshInitialized)
			{
				InitializeNavmesh(initialPosition);
			}
			Vector3D vector3D = WorldPositionToLocalNavmeshPosition(initialPosition, 0.5f);
			Vector3D vector3D2 = WorldPositionToLocalNavmeshPosition(targetPosition, 0.5f);
			List<Vector3> path = m_rdWrapper.GetPath(vector3D, vector3D2);
			if (path.Count == 0)
			{
				List<MyNavmeshOBBs.OBBCoords> intersectedOBB = m_navmeshOBBs.GetIntersectedOBB(new LineD(initialPosition, targetPosition));
				StartNavmeshTileCreation(intersectedOBB);
				m_debugDrawIntersectedOBBs = intersectedOBB;
				return list;
			}
			foreach (Vector3 item in path)
			{
				list.Add(LocalPositionToWorldPosition(item));
			}
			return list;
		}

		public void DrawGeometry(HkGeometry geometry, MatrixD worldMatrix, Color color, bool depthRead = false, bool shaded = false)
		{
			MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
			for (int i = 0; i < geometry.TriangleCount; i++)
			{
				geometry.GetTriangle(i, out var i2, out var i3, out var i4, out var _);
				myRenderMessageDebugDrawTriangles.AddIndex(i2);
				myRenderMessageDebugDrawTriangles.AddIndex(i3);
				myRenderMessageDebugDrawTriangles.AddIndex(i4);
			}
			for (int j = 0; j < geometry.VertexCount; j++)
			{
				myRenderMessageDebugDrawTriangles.AddVertex(geometry.GetVertex(j));
			}
		}

		private void DebugDrawShape(string blockName, HkShape shape, MatrixD worldMatrix)
		{
			float num = 1.05f;
			float value = 0.02f;
			if (MyPerGameSettings.Game == GameEnum.SE_GAME)
			{
				value = 0.1f;
			}
			switch (shape.ShapeType)
			{
			case HkShapeType.Box:
				MyRenderProxy.DebugDrawOBB(MatrixD.CreateScale(((HkBoxShape)shape).HalfExtents * 2f + new Vector3(value)) * worldMatrix, Color.Red, 0f, depthRead: true, smooth: false);
				break;
			case HkShapeType.List:
			{
				HkShapeContainerIterator iterator = ((HkListShape)shape).GetIterator();
				int num2 = 0;
				while (iterator.IsValid)
				{
					DebugDrawShape(blockName + num2++, iterator.CurrentValue, worldMatrix);
					iterator.Next();
				}
				break;
			}
			case HkShapeType.Mopp:
				DebugDrawShape(blockName, ((HkMoppBvTreeShape)shape).ShapeCollection, worldMatrix);
				break;
			case HkShapeType.ConvexTransform:
			{
				HkConvexTransformShape hkConvexTransformShape = (HkConvexTransformShape)shape;
				DebugDrawShape(blockName, hkConvexTransformShape.ChildShape, hkConvexTransformShape.Transform * worldMatrix);
				break;
			}
			case HkShapeType.ConvexTranslate:
			{
				HkConvexTranslateShape hkConvexTranslateShape = (HkConvexTranslateShape)shape;
				DebugDrawShape(blockName, hkConvexTranslateShape.ChildShape, Matrix.CreateTranslation(hkConvexTranslateShape.Translation) * worldMatrix);
				break;
			}
			case HkShapeType.ConvexVertices:
			{
				HkConvexVerticesShape hkConvexVerticesShape = (HkConvexVerticesShape)shape;
				if (!m_cachedGeometry.TryGetValue(blockName, out var value2))
				{
					HkGeometry geometry = new HkGeometry();
					hkConvexVerticesShape.GetGeometry(geometry, out var center);
					value2 = new GeometryCenterPair
					{
						Geometry = geometry,
						Center = center
					};
					if (!string.IsNullOrEmpty(blockName))
					{
						m_cachedGeometry.Add(blockName, value2);
					}
				}
				Vector3D vector3D = Vector3D.Transform(value2.Center, worldMatrix.GetOrientation());
				MatrixD matrixD = worldMatrix;
				matrixD = MatrixD.CreateScale(num) * matrixD;
				matrixD.Translation -= vector3D * (num - 1f);
				DrawGeometry(value2.Geometry, matrixD, Color.Olive);
				break;
			}
			case HkShapeType.Capsule:
			case HkShapeType.TriSampledHeightFieldCollection:
			case HkShapeType.TriSampledHeightFieldBvTree:
				break;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Draws physical mesh
		/// </summary>
		public void DebugDrawPhysicalShapes()
		{
=======
		public void DebugDrawPhysicalShapes()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
			if (targetGrid == null)
			{
				return;
			}
			List<MyCubeGrid> list = new List<MyCubeGrid>();
<<<<<<< HEAD
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(targetGrid).Nodes)
			{
				list.Add(node.NodeData);
=======
			Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Logical.GetGroup(targetGrid).Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
					list.Add(current.NodeData);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MatrixD.Invert(list[0].WorldMatrix);
			foreach (MyCubeGrid item in list)
			{
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					new HkGridShape(item.GridSize, HkReferencePolicy.None);
					MyCubeBlockCollector myCubeBlockCollector = new MyCubeBlockCollector();
					MyVoxelSegmentation segmenter = new MyVoxelSegmentation();
					Dictionary<Vector3I, HkMassElement> massResults = new Dictionary<Vector3I, HkMassElement>();
					myCubeBlockCollector.Collect(item, segmenter, MyVoxelSegmentationType.Simple, massResults);
					foreach (HkShape shape3 in myCubeBlockCollector.Shapes)
					{
						DebugDrawShape("", shape3, item.WorldMatrix);
					}
					continue;
				}
<<<<<<< HEAD
				foreach (MySlimBlock block in item.GetBlocks())
				{
					if (block.FatBlock == null)
					{
						continue;
					}
					if (block.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							HkShape shape = block2.FatBlock.ModelCollision.HavokCollisionShapes[0];
							DebugDrawShape(block2.BlockDefinition.Id.SubtypeName, shape, block2.FatBlock.PositionComp.WorldMatrixRef);
						}
					}
					else if (block.FatBlock.ModelCollision.HavokCollisionShapes != null)
					{
						HkShape[] havokCollisionShapes = block.FatBlock.ModelCollision.HavokCollisionShapes;
						foreach (HkShape shape2 in havokCollisionShapes)
						{
							DebugDrawShape(block.BlockDefinition.Id.SubtypeName, shape2, block.FatBlock.PositionComp.WorldMatrixRef);
						}
					}
				}
=======
				Enumerator<MySlimBlock> enumerator4 = item.GetBlocks().GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						MySlimBlock current4 = enumerator4.get_Current();
						if (current4.FatBlock == null)
						{
							continue;
						}
						if (current4.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block in (current4.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								HkShape shape = block.FatBlock.ModelCollision.HavokCollisionShapes[0];
								DebugDrawShape(block.BlockDefinition.Id.SubtypeName, shape, block.FatBlock.PositionComp.WorldMatrixRef);
							}
						}
						else if (current4.FatBlock.ModelCollision.HavokCollisionShapes != null)
						{
							HkShape[] havokCollisionShapes = current4.FatBlock.ModelCollision.HavokCollisionShapes;
							foreach (HkShape shape2 in havokCollisionShapes)
							{
								DebugDrawShape(current4.BlockDefinition.Id.SubtypeName, shape2, current4.FatBlock.PositionComp.WorldMatrixRef);
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator4).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void DrawPersistentDebugNavmesh(bool force)
		{
			if (m_newVisualNavmesh != null)
			{
				m_visualNavmesh = m_newVisualNavmesh;
				m_newVisualNavmesh = null;
				force = true;
			}
			if (!force)
			{
				return;
			}
			if (m_visualNavmesh.Count > 0)
			{
				if (m_drawNavmeshId == uint.MaxValue)
				{
					m_drawNavmeshId = MyRenderProxy.DebugDrawMesh(m_visualNavmesh, MatrixD.Identity, depthRead: true, shaded: true);
				}
				else
				{
					MyRenderProxy.DebugDrawUpdateMesh(m_drawNavmeshId, m_visualNavmesh, MatrixD.Identity, depthRead: true, shaded: true);
				}
			}
			else
			{
				HidePersistentDebugNavmesh();
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

<<<<<<< HEAD
		/// <summary>
		/// Just for debug
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private unsafe static Vector3* GetMiddleOBBPoints(MyOrientedBoundingBoxD obb, ref Vector3* points)
		{
			Vector3 vector = obb.Orientation.Right * (float)obb.HalfExtent.X;
			Vector3 vector2 = obb.Orientation.Forward * (float)obb.HalfExtent.Z;
			*points = obb.Center - vector - vector2;
			points[1] = obb.Center + vector - vector2;
			points[2] = obb.Center + vector + vector2;
			points[3] = obb.Center - vector + vector2;
			return points;
		}

<<<<<<< HEAD
		/// <summary>
		/// Just for debug
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private unsafe static bool DrawTerrainLimits(MyPlanet planet, MyOrientedBoundingBoxD obb)
		{
			int pointCount = 4;
			Vector3* points = stackalloc Vector3[4];
			GetMiddleOBBPoints(obb, ref points);
			planet.Provider.Shape.GetBounds(points, pointCount, out var minHeight, out var maxHeight);
			if (minHeight.IsValid() && maxHeight.IsValid())
			{
				Vector3D vector3D = obb.Orientation.Up * minHeight;
				Vector3D vector3D2 = obb.Orientation.Up * maxHeight;
				obb.Center = vector3D + (vector3D2 - vector3D) * 0.5;
				obb.HalfExtent.Y = (maxHeight - minHeight) * 0.5f;
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(obb.Center, obb.HalfExtent, obb.Orientation), Color.Blue, 0f, depthRead: true, smooth: false);
				return true;
			}
			return false;
		}

		private unsafe void DebugDrawInternal()
		{
			m_navmeshOBBs?.DebugDraw();
			if (DrawNavmesh)
			{
				DrawPersistentDebugNavmesh(force: false);
			}
			if (DrawPhysicalMesh)
			{
				DebugDrawPhysicalShapes();
			}
			Vector3D position = MySession.Static.ControlledEntity.ControllerInfo.Controller.Player.GetPosition();
			position.Y += 2.4000000953674316;
			MyRenderProxy.DebugDrawText3D(position, $"X: {Math.Round(position.X, 2)}\nY: {Math.Round(position.Y, 2)}\nZ: {Math.Round(position.Z, 2)}", Color.Red, 1f, depthRead: true);
			if (m_lastGroundMeshQuery.Count > 0)
			{
				MyRenderProxy.DebugDrawSphere(m_lastGroundMeshQuery[0].Center, 1f, Color.Yellow);
				foreach (BoundingBoxD item in m_lastGroundMeshQuery)
				{
					MyRenderProxy.DebugDrawOBB(item.Matrix, Color.Yellow, 0f, depthRead: true, smooth: false);
				}
				if (m_navmeshOBBs != null)
				{
					foreach (MyNavmeshOBBs.OBBCoords debugDrawIntersectedOBB in m_debugDrawIntersectedOBBs)
					{
						MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(debugDrawIntersectedOBB.OBB.Center, new Vector3(debugDrawIntersectedOBB.OBB.HalfExtent.X, debugDrawIntersectedOBB.OBB.HalfExtent.Y / 2.0, debugDrawIntersectedOBB.OBB.HalfExtent.Z), debugDrawIntersectedOBB.OBB.Orientation), Color.White, 0f, depthRead: true, smooth: false);
					}
					MyOrientedBoundingBoxD value = m_navmeshOBBs.GetOBB(0, 0).Value;
					MyPlanet planet = GetPlanet(value.Center);
					Vector3* points = stackalloc Vector3[4];
					GetMiddleOBBPoints(value, ref points);
					planet.Provider.Shape.GetBounds(points, 4, out var minHeight, out var maxHeight);
					if (minHeight.IsValid() && maxHeight.IsValid())
					{
						Vector3D position2 = value.Orientation.Up * minHeight;
						Vector3D position3 = value.Orientation.Up * maxHeight;
						MyRenderProxy.DebugDrawSphere(position2, 1f, Color.Blue, 0f);
						MyRenderProxy.DebugDrawSphere(position3, 1f, Color.Blue, 0f);
					}
					DrawTerrainLimits(planet, value);
				}
				MyRenderProxy.DebugDrawSphere(m_meshCenter, 2f, Color.Red, 0f);
			}
			if (m_polygons != null && m_pathfindingDebugTarget.HasValue)
			{
				Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(m_pathfindingDebugTarget.Value));
				MyRenderProxy.DebugDrawSphere(m_pathfindingDebugTarget.Value + 1.5 * vector3D, 0.2f, Color.Red, 0f);
			}
		}
	}
}
