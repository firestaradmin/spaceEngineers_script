using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyNavmeshCoordinator
	{
		private static readonly List<MyEntity> m_tmpEntityList = new List<MyEntity>();

		private static readonly List<MyGridPathfinding.CubeId> m_tmpLinkCandidates = new List<MyGridPathfinding.CubeId>();

		private static readonly List<MyNavigationTriangle> m_tmpNavTris = new List<MyNavigationTriangle>();

		private static readonly List<MyNavigationPrimitive> m_tmpNavPrims = new List<MyNavigationPrimitive>(4);

		private MyGridPathfinding m_gridPathfinding;

		private MyVoxelPathfinding m_voxelPathfinding;

		private readonly MyDynamicObstacles m_obstacles;

		private readonly Dictionary<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>> m_voxelLinkDictionary = new Dictionary<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>>();

		private readonly Dictionary<MyGridPathfinding.CubeId, int> m_gridLinkCounter = new Dictionary<MyGridPathfinding.CubeId, int>();

		public MyNavgroupLinks Links { get; }

		public MyNavgroupLinks HighLevelLinks { get; }

		public MyNavmeshCoordinator(MyDynamicObstacles obstacles)
		{
			Links = new MyNavgroupLinks();
			HighLevelLinks = new MyNavgroupLinks();
			m_obstacles = obstacles;
		}

		public void SetGridPathfinding(MyGridPathfinding gridPathfinding)
		{
			m_gridPathfinding = gridPathfinding;
		}

		public void SetVoxelPathfinding(MyVoxelPathfinding myVoxelPathfinding)
		{
			m_voxelPathfinding = myVoxelPathfinding;
		}

		public void PrepareVoxelTriangleTests(BoundingBoxD cellBoundingBox, List<MyCubeGrid> gridsToTestOutput)
		{
			m_tmpEntityList.Clear();
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
			cellBoundingBox.Inflate(cubeSize);
			if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
			{
				Vector3D min = cellBoundingBox.Min;
				min.Y -= cubeSize;
				cellBoundingBox.Min = min;
			}
			MyGamePruningStructure.GetAllEntitiesInBox(ref cellBoundingBox, m_tmpEntityList);
			foreach (MyEntity tmpEntity in m_tmpEntityList)
			{
				MyCubeGrid myCubeGrid = tmpEntity as MyCubeGrid;
				if (myCubeGrid != null && MyGridPathfinding.GridCanHaveNavmesh(myCubeGrid))
				{
					gridsToTestOutput.Add(myCubeGrid);
				}
			}
			m_tmpEntityList.Clear();
		}

		public void TestVoxelNavmeshTriangle(ref Vector3D a, ref Vector3D b, ref Vector3D c, List<MyCubeGrid> gridsToTest, List<MyGridPathfinding.CubeId> linkCandidatesOutput, out bool intersecting)
		{
			Vector3D point = (a + b + c) / 3.0;
			if (m_obstacles.IsInObstacle(point))
			{
				intersecting = true;
				return;
			}
			Vector3D normal = Vector3D.Zero;
			if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
			{
				normal = Vector3.Down * 2f;
			}
			m_tmpLinkCandidates.Clear();
			intersecting = false;
			foreach (MyCubeGrid item in gridsToTest)
			{
				MatrixD matrix = item.PositionComp.WorldMatrixNormalizedInv;
				Vector3D.Transform(ref a, ref matrix, out var result);
				Vector3D.Transform(ref b, ref matrix, out var result2);
				Vector3D.Transform(ref c, ref matrix, out var result3);
				Vector3D.TransformNormal(ref normal, ref matrix, out var result4);
				BoundingBoxD boundingBoxD = new BoundingBoxD(Vector3D.MaxValue, Vector3D.MinValue);
				boundingBoxD.Include(ref result, ref result2, ref result3);
				Vector3I vector3I = item.LocalToGridInteger(boundingBoxD.Min);
				Vector3I vector3I2 = item.LocalToGridInteger(boundingBoxD.Max);
				Vector3I start = vector3I - Vector3I.One;
				Vector3I end = vector3I2 + Vector3I.One;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					if (item.GetCubeBlock(start) != null)
					{
						Vector3 vector = (start - Vector3.One) * item.GridSize;
						Vector3 vector2 = (start + Vector3.One) * item.GridSize;
						Vector3 vector3 = (start - Vector3.Half) * item.GridSize;
						Vector3 vector4 = (start + Vector3.Half) * item.GridSize;
						BoundingBoxD boundingBoxD2 = new BoundingBoxD(vector, vector2);
						BoundingBoxD boundingBoxD3 = new BoundingBoxD(vector3, vector4);
						boundingBoxD2.Include(vector + result4);
						boundingBoxD2.Include(vector2 + result4);
						boundingBoxD3.Include(vector3 + result4);
						boundingBoxD3.Include(vector4 + result4);
						if (boundingBoxD2.IntersectsTriangle(ref result, ref result2, ref result3))
						{
							if (boundingBoxD3.IntersectsTriangle(ref result, ref result2, ref result3))
							{
								intersecting = true;
								break;
							}
							int num = Math.Min(Math.Abs(vector3I.X - start.X), Math.Abs(vector3I2.X - start.X));
							int num2 = Math.Min(Math.Abs(vector3I.Y - start.Y), Math.Abs(vector3I2.Y - start.Y));
							int num3 = Math.Min(Math.Abs(vector3I.Z - start.Z), Math.Abs(vector3I2.Z - start.Z));
							if (num + num2 + num3 < 3)
							{
								m_tmpLinkCandidates.Add(new MyGridPathfinding.CubeId
								{
									Grid = item,
									Coords = start
								});
							}
						}
					}
					vector3I_RangeIterator.GetNext(out start);
				}
				if (intersecting)
				{
					break;
				}
			}
			if (!intersecting)
			{
				for (int i = 0; i < m_tmpLinkCandidates.Count; i++)
				{
					linkCandidatesOutput.Add(m_tmpLinkCandidates[i]);
				}
			}
			m_tmpLinkCandidates.Clear();
		}

		public void TryAddVoxelNavmeshLinks(MyNavigationTriangle addedPrimitive, MyVoxelPathfinding.CellId cellId, List<MyGridPathfinding.CubeId> linkCandidates)
		{
			m_tmpNavTris.Clear();
			foreach (MyGridPathfinding.CubeId linkCandidate in linkCandidates)
			{
				m_gridPathfinding.GetCubeTriangles(linkCandidate, m_tmpNavTris);
				double num = double.MaxValue;
				MyNavigationTriangle myNavigationTriangle = null;
				foreach (MyNavigationTriangle tmpNavTri in m_tmpNavTris)
				{
					Vector3D vector3D = addedPrimitive.WorldPosition - tmpNavTri.WorldPosition;
					if (MyPerGameSettings.NavmeshPresumesDownwardGravity && Math.Abs(vector3D.Y) < 0.3 && vector3D.LengthSquared() < num)
					{
						num = vector3D.LengthSquared();
						myNavigationTriangle = tmpNavTri;
					}
				}
				if (myNavigationTriangle != null)
				{
					bool flag = true;
					List<MyNavigationPrimitive> links = Links.GetLinks(myNavigationTriangle);
					List<MyNavigationPrimitive> value = null;
					m_voxelLinkDictionary.TryGetValue(cellId, out value);
					if (links != null)
					{
						m_tmpNavPrims.Clear();
						CollectClosePrimitives(addedPrimitive, m_tmpNavPrims, 2);
						for (int i = 0; i < m_tmpNavPrims.Count; i++)
						{
							if (links.Contains(m_tmpNavPrims[i]) && value != null && value.Contains(m_tmpNavPrims[i]))
							{
								if ((m_tmpNavPrims[i].WorldPosition - myNavigationTriangle.WorldPosition).LengthSquared() < num)
								{
									flag = false;
									break;
								}
								Links.RemoveLink(myNavigationTriangle, m_tmpNavPrims[i]);
								if (Links.GetLinkCount(m_tmpNavPrims[i]) == 0)
								{
									RemoveVoxelLinkFromDictionary(cellId, m_tmpNavPrims[i]);
								}
								DecreaseGridLinkCounter(linkCandidate);
							}
						}
						m_tmpNavPrims.Clear();
					}
					if (flag)
					{
						Links.AddLink(addedPrimitive, myNavigationTriangle);
						SaveVoxelLinkToDictionary(cellId, addedPrimitive);
						IncreaseGridLinkCounter(linkCandidate);
					}
				}
				m_tmpNavTris.Clear();
			}
		}

		public void TryAddVoxelNavmeshLinks2(MyVoxelPathfinding.CellId cellId, Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> linkCandidates)
		{
			foreach (KeyValuePair<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> linkCandidate in linkCandidates)
			{
				double num = double.MaxValue;
				MyNavigationTriangle myNavigationTriangle = null;
				MyNavigationPrimitive myNavigationPrimitive = null;
				m_tmpNavTris.Clear();
				m_gridPathfinding.GetCubeTriangles(linkCandidate.Key, m_tmpNavTris);
				foreach (MyNavigationTriangle tmpNavTri in m_tmpNavTris)
				{
					tmpNavTri.GetVertices(out var a, out var b, out var c);
					a = tmpNavTri.Parent.LocalToGlobal(a);
					b = tmpNavTri.Parent.LocalToGlobal(b);
					c = tmpNavTri.Parent.LocalToGlobal(c);
					Vector3D vector = (c - a).Cross(b - a);
					Vector3D vector3D = (a + b + c) / 3f;
					double num2 = Math.Min(a.Y, Math.Min(b.Y, c.Y));
					double num3 = Math.Max(a.Y, Math.Max(b.Y, c.Y));
					num2 -= 0.25;
					num3 += 0.25;
					foreach (MyNavigationPrimitive item in linkCandidate.Value)
					{
						Vector3D worldPosition = item.WorldPosition;
						Vector3D vector2 = worldPosition - vector3D;
						double num4 = vector2.Length();
						vector2 /= num4;
						Vector3D.Dot(ref vector2, ref vector, out var result);
						if (result > -0.20000000298023224 && worldPosition.Y < num3 && worldPosition.Y > num2)
						{
							double num5 = num4 / (result + 0.30000001192092896);
							if (num5 < num)
							{
								num = num5;
								myNavigationTriangle = tmpNavTri;
								myNavigationPrimitive = item;
							}
						}
					}
				}
				m_tmpNavTris.Clear();
				if (myNavigationTriangle != null)
				{
					Links.AddLink(myNavigationPrimitive, myNavigationTriangle);
					SaveVoxelLinkToDictionary(cellId, myNavigationPrimitive);
					IncreaseGridLinkCounter(linkCandidate.Key);
				}
			}
		}

		public void RemoveVoxelNavmeshLinks(MyVoxelPathfinding.CellId cellId)
		{
			List<MyNavigationPrimitive> value = null;
			if (!m_voxelLinkDictionary.TryGetValue(cellId, out value))
			{
				return;
			}
			foreach (MyNavigationPrimitive item in value)
			{
				Links.RemoveAllLinks(item);
			}
			m_voxelLinkDictionary.Remove(cellId);
		}

		public void RemoveGridNavmeshLinks(MyCubeGrid grid)
		{
			MyGridNavigationMesh navmesh = m_gridPathfinding.GetNavmesh(grid);
			if (navmesh == null)
			{
				return;
			}
			m_tmpNavPrims.Clear();
			MyVector3ISet.Enumerator cubes = navmesh.GetCubes();
			while (cubes.MoveNext())
			{
				MyGridPathfinding.CubeId cubeId = default(MyGridPathfinding.CubeId);
				cubeId.Grid = grid;
				cubeId.Coords = cubes.Current;
				MyGridPathfinding.CubeId key = cubeId;
				if (!m_gridLinkCounter.TryGetValue(key, out var _))
				{
					continue;
				}
				m_tmpNavTris.Clear();
				navmesh.GetCubeTriangles(cubes.Current, m_tmpNavTris);
				foreach (MyNavigationTriangle tmpNavTri in m_tmpNavTris)
				{
					Links.RemoveAllLinks(tmpNavTri);
					MyHighLevelPrimitive highLevelPrimitive = tmpNavTri.GetHighLevelPrimitive();
					if (!m_tmpNavPrims.Contains(highLevelPrimitive))
					{
						m_tmpNavPrims.Add(highLevelPrimitive);
					}
				}
				m_tmpNavTris.Clear();
				m_gridLinkCounter.Remove(key);
			}
			cubes.Dispose();
			foreach (MyNavigationPrimitive tmpNavPrim in m_tmpNavPrims)
			{
				HighLevelLinks.RemoveAllLinks(tmpNavPrim);
			}
			m_tmpNavPrims.Clear();
		}

		private void SaveVoxelLinkToDictionary(MyVoxelPathfinding.CellId cellId, MyNavigationPrimitive linkedPrimitive)
		{
			List<MyNavigationPrimitive> value = null;
			if (!m_voxelLinkDictionary.TryGetValue(cellId, out value))
			{
				value = new List<MyNavigationPrimitive>();
			}
			else if (value.Contains(linkedPrimitive))
			{
				return;
			}
			value.Add(linkedPrimitive);
			m_voxelLinkDictionary[cellId] = value;
		}

		private void RemoveVoxelLinkFromDictionary(MyVoxelPathfinding.CellId cellId, MyNavigationPrimitive linkedPrimitive)
		{
			List<MyNavigationPrimitive> value = null;
			if (m_voxelLinkDictionary.TryGetValue(cellId, out value))
			{
				value.Remove(linkedPrimitive);
				if (value.Count == 0)
				{
					m_voxelLinkDictionary.Remove(cellId);
				}
			}
		}

		private void IncreaseGridLinkCounter(MyGridPathfinding.CubeId candidate)
		{
			int value = 0;
			value = ((!m_gridLinkCounter.TryGetValue(candidate, out value)) ? 1 : (value + 1));
			m_gridLinkCounter[candidate] = value;
		}

		private void DecreaseGridLinkCounter(MyGridPathfinding.CubeId candidate)
		{
			int value = 0;
			if (m_gridLinkCounter.TryGetValue(candidate, out value))
			{
				value--;
				if (value == 0)
				{
					m_gridLinkCounter.Remove(candidate);
				}
				else
				{
					m_gridLinkCounter[candidate] = value;
				}
			}
		}

		private void CollectClosePrimitives(MyNavigationPrimitive addedPrimitive, List<MyNavigationPrimitive> output, int depth)
		{
			if (depth < 0)
			{
				return;
			}
			int num = output.Count;
			output.Add(addedPrimitive);
			int num2 = output.Count;
			for (int i = 0; i < addedPrimitive.GetOwnNeighborCount(); i++)
			{
				MyNavigationPrimitive item;
				if ((item = addedPrimitive.GetOwnNeighbor(i) as MyNavigationPrimitive) != null)
				{
					output.Add(item);
				}
			}
			int count = output.Count;
			for (depth--; depth > 0; depth--)
			{
				for (int j = num2; j < count; j++)
				{
					MyNavigationPrimitive myNavigationPrimitive = output[j];
					for (int k = 0; k < myNavigationPrimitive.GetOwnNeighborCount(); k++)
					{
						MyNavigationPrimitive myNavigationPrimitive2 = myNavigationPrimitive.GetOwnNeighbor(k) as MyNavigationPrimitive;
						bool flag = false;
						for (int l = num; l < count; l++)
						{
							if (output[l] == myNavigationPrimitive2)
							{
								flag = true;
								break;
							}
						}
						if (!flag && myNavigationPrimitive2 != null)
						{
							output.Add(myNavigationPrimitive2);
						}
					}
				}
				num = num2;
				num2 = count;
				count = output.Count;
			}
		}

		public void UpdateVoxelNavmeshCellHighLevelLinks(MyVoxelPathfinding.CellId cellId)
		{
			List<MyNavigationPrimitive> value = null;
			if (!m_voxelLinkDictionary.TryGetValue(cellId, out value))
			{
				return;
			}
			MyNavigationPrimitive myNavigationPrimitive = null;
			MyNavigationPrimitive myNavigationPrimitive2 = null;
			foreach (MyNavigationPrimitive item in value)
			{
				myNavigationPrimitive = item.GetHighLevelPrimitive();
				List<MyNavigationPrimitive> list = null;
				list = Links.GetLinks(item);
				if (list == null)
				{
					continue;
				}
				foreach (MyNavigationPrimitive item2 in list)
				{
					myNavigationPrimitive2 = item2.GetHighLevelPrimitive();
					HighLevelLinks.AddLink(myNavigationPrimitive, myNavigationPrimitive2, onlyIfNotPresent: true);
				}
			}
		}

		public void InvalidateVoxelsBBox(ref BoundingBoxD bbox)
		{
			m_voxelPathfinding.InvalidateBox(ref bbox);
		}

		public void DebugDraw()
		{
			if (!MyFakes.DEBUG_DRAW_NAVMESH_LINKS || !MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
			{
				return;
			}
			foreach (KeyValuePair<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>> item in m_voxelLinkDictionary)
			{
				MyVoxelBase voxelMap = item.Key.VoxelMap;
				Vector3I geometryCellCoord = item.Key.Pos;
				BoundingBoxD worldAABB = default(BoundingBoxD);
				MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(voxelMap.PositionLeftBottomCorner, ref geometryCellCoord, out worldAABB);
				MyRenderProxy.DebugDrawText3D(worldAABB.Center, "LinkNum: " + item.Value.Count, Color.Red, 1f, depthRead: false);
			}
		}
	}
}
