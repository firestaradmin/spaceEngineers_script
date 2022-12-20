using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Collections;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
	public class MyGridClusterAnalyzeHelper
	{
		private class OctreeHeatmap
		{
			public class OctreeHeatmapNode
			{
				public enum MyQueryResult
				{
					Contain,
					Disjoint,
					Intersect
				}

				public static readonly double LEAF_SIZE;

				public static readonly List<double> PRECOUNTED_HALF_EXTENTS;

				public static readonly int PRECOUNT_AMOUNT;

				public int Level;

				public Vector3D Midpoint;

				public float Value;

				public List<OctreeHeatmapNode> Children;

				public bool IsLeaf => Children == null;

				static OctreeHeatmapNode()
				{
					LEAF_SIZE = 500.0;
					PRECOUNTED_HALF_EXTENTS = new List<double>();
					PRECOUNT_AMOUNT = 25;
					PRECOUNTED_HALF_EXTENTS.Add(0.5 * LEAF_SIZE);
					for (int i = 1; i < PRECOUNT_AMOUNT; i++)
					{
						PRECOUNTED_HALF_EXTENTS.Add(2.0 * PRECOUNTED_HALF_EXTENTS[PRECOUNTED_HALF_EXTENTS.Count - 1]);
					}
				}

				public OctreeHeatmapNode(int level, Vector3D midpoint)
				{
					Level = level;
					Midpoint = midpoint;
				}

				public bool IsPointInside(Vector3D point)
				{
					double num = PRECOUNTED_HALF_EXTENTS[Level];
					Vector3D vector3D = Midpoint - num;
					Vector3D vector3D2 = Midpoint + num;
					if (point.X < vector3D.X || point.Y < vector3D.Y || point.Z < vector3D.Z)
					{
						return false;
					}
					if (point.X >= vector3D2.X || point.Y >= vector3D2.Y || point.Z >= vector3D2.Z)
					{
						return false;
					}
					return true;
				}

				public Vector3D GetNewMidpointOfParent(int index)
				{
					double num = PRECOUNTED_HALF_EXTENTS[Level];
					Vector3D result = default(Vector3D);
					if (index % 2 == 1)
					{
						result.Z = Midpoint.Z + num;
					}
					else
					{
						result.Z = Midpoint.Z - num;
					}
					int num2 = index / 2;
					if (num2 % 2 == 1)
					{
						result.Y = Midpoint.Y + num;
					}
					else
					{
						result.Y = Midpoint.Y - num;
					}
					if (num2 / 2 % 2 == 1)
					{
						result.X = Midpoint.X + num;
					}
					else
					{
						result.X = Midpoint.X - num;
					}
					return result;
				}

				public Vector3D GetNewMidpointOfChild(int index)
				{
					if (Level == 0)
					{
						return Vector3D.Zero;
					}
					double num = PRECOUNTED_HALF_EXTENTS[Level - 1];
					Vector3D result = default(Vector3D);
					if (index % 2 == 1)
					{
						result.Z = Midpoint.Z + num;
					}
					else
					{
						result.Z = Midpoint.Z - num;
					}
					int num2 = index / 2;
					if (num2 % 2 == 1)
					{
						result.Y = Midpoint.Y + num;
					}
					else
					{
						result.Y = Midpoint.Y - num;
					}
					if (num2 / 2 % 2 == 1)
					{
						result.X = Midpoint.X + num;
					}
					else
					{
						result.X = Midpoint.X - num;
					}
					return result;
				}

				internal int GetChildIndex(Vector3D position)
				{
					int num = 0;
					if (position.X >= Midpoint.X)
					{
						num += 4;
					}
					if (position.Y >= Midpoint.Y)
					{
						num += 2;
					}
					if (position.Z >= Midpoint.Z)
					{
						num++;
					}
					return num;
				}

				public bool BuildChildren()
				{
					if (Level == 0)
					{
						return false;
					}
					Children = new List<OctreeHeatmapNode>();
					for (int i = 0; i < 8; i++)
					{
						int level = Level - 1;
						Vector3D newMidpointOfChild = GetNewMidpointOfChild(i);
						OctreeHeatmapNode octreeHeatmapNode = new OctreeHeatmapNode(level, newMidpointOfChild);
						octreeHeatmapNode.Value = Value;
						Children.Add(octreeHeatmapNode);
					}
					Value = 0f;
					return true;
				}

				internal bool IsAABBQueryIInside(Vector3D queryMin, Vector3D queryMax)
				{
					Vector3D vector3D = Midpoint - PRECOUNTED_HALF_EXTENTS[Level];
					Vector3D vector3D2 = Midpoint + PRECOUNTED_HALF_EXTENTS[Level];
					if (queryMin.X < vector3D.X || queryMin.Y < vector3D.Y || queryMin.Z < vector3D.Z)
					{
						return false;
					}
					if (queryMax.X >= vector3D2.X || queryMax.Y >= vector3D2.Y || queryMax.Z >= vector3D2.Z)
					{
						return false;
					}
					return true;
				}

				internal OctreeHeatmapNode BuildParentByQuery(Vector3D queryMin, Vector3D queryMax)
				{
					Vector3D vector3D = Midpoint - PRECOUNTED_HALF_EXTENTS[Level];
					_ = Midpoint + PRECOUNTED_HALF_EXTENTS[Level];
					int num = 0;
					if (queryMin.X < vector3D.X)
					{
						num += 4;
					}
					if (queryMin.Y < vector3D.Y)
					{
						num += 2;
					}
					if (queryMin.Z < vector3D.Z)
					{
						num++;
					}
					Vector3D newMidpointOfParent = GetNewMidpointOfParent(num);
					OctreeHeatmapNode octreeHeatmapNode = new OctreeHeatmapNode(Level + 1, newMidpointOfParent);
					octreeHeatmapNode.BuildChildren();
					octreeHeatmapNode.Children[num] = this;
					return octreeHeatmapNode;
				}

				public void WriteQueryRecursive(float delta, ref Vector3D position, double radius)
				{
					switch (TestQuery(position, radius))
					{
					case MyQueryResult.Contain:
						AddValueRecursive(delta);
						break;
					case MyQueryResult.Intersect:
						if (Level <= 0)
						{
							break;
						}
						if (Children == null)
						{
							BuildChildren();
						}
						foreach (OctreeHeatmapNode child in Children)
						{
							child.WriteQueryRecursive(delta, ref position, radius);
						}
						break;
					}
				}

				private void AddValueRecursive(float delta)
				{
					if (Level == 0 || Children == null)
					{
						Value += delta;
						return;
					}
					foreach (OctreeHeatmapNode child in Children)
					{
						child.AddValueRecursive(delta);
					}
				}

				private List<Vector3D> GetCorners()
				{
					double num = PRECOUNTED_HALF_EXTENTS[Level];
					List<Vector3D> list = new List<Vector3D>();
					for (int i = -1; i < 2; i += 2)
					{
						for (int j = -1; j < 2; j += 2)
						{
							for (int k = -1; k < 2; k += 2)
							{
								list.Add(Midpoint + new Vector3D((double)i * num, (double)j * num, (double)k * num));
							}
						}
					}
					return list;
				}

				public bool SphereIntersection(Vector3D position, double radius)
				{
					double num = PRECOUNTED_HALF_EXTENTS[Level];
					Vector3D vector3D = Midpoint - num;
					Vector3D vector3D2 = Midpoint + num;
					double num2 = 0.0;
					if (position.X < vector3D.X)
					{
						num2 += (vector3D.X - position.X) * (vector3D.X - position.X);
					}
					else if (position.X > vector3D2.X)
					{
						num2 += (vector3D2.X - position.X) * (vector3D2.X - position.X);
					}
					if (position.Y < vector3D.Y)
					{
						num2 += (vector3D.Y - position.Y) * (vector3D.Y - position.Y);
					}
					else if (position.Y > vector3D2.Y)
					{
						num2 += (vector3D2.Y - position.Y) * (vector3D2.Y - position.Y);
					}
					if (position.Z < vector3D.Z)
					{
						num2 += (vector3D.Z - position.Z) * (vector3D.Z - position.Z);
					}
					else if (position.Z > vector3D2.Z)
					{
						num2 += (vector3D2.Z - position.Z) * (vector3D2.Z - position.Z);
					}
					return num2 < radius * radius;
				}

				private MyQueryResult TestQuery(Vector3D position, double radius)
				{
					if (!SphereIntersection(position, radius))
					{
						return MyQueryResult.Disjoint;
					}
					List<Vector3D> corners = GetCorners();
					int num = 0;
					double num2 = radius * radius;
					foreach (Vector3D item in corners)
					{
						if ((item - position).LengthSquared() <= num2)
						{
							num++;
						}
					}
					if (num == corners.Count)
					{
						return MyQueryResult.Contain;
					}
					return MyQueryResult.Intersect;
				}

				public void DebugPrintChildrenRecursion(string key)
				{
					if (!IsLeaf)
					{
						DebugPrintChildrenDraw();
						for (int i = 0; i < 8; i++)
						{
							Children[i].DebugPrintChildrenRecursion(key + i);
						}
					}
				}

				public void DebugPrintChildrenDraw()
				{
					_ = IsLeaf;
				}

				internal float Sample(Vector3D point)
				{
					Vector3D vector3D = Midpoint - PRECOUNTED_HALF_EXTENTS[Level];
					Vector3D vector3D2 = Midpoint + PRECOUNTED_HALF_EXTENTS[Level];
					if (point.X < vector3D.X || point.Y < vector3D.Y || point.Z < vector3D.Z || point.X >= vector3D2.X || point.Y >= vector3D2.Y || point.Z >= vector3D2.Z)
					{
						return -1f;
					}
					if (Children == null)
					{
						return Value;
					}
					int num = 0;
					if (point.X >= Midpoint.X)
					{
						num += 4;
					}
					if (point.Y >= Midpoint.Y)
					{
						num += 2;
					}
					if (point.Z >= Midpoint.Z)
					{
						num++;
					}
					return Children[num].Sample(point);
				}

				internal float GetHighestHeatPoint(out Vector3D heatPoint)
				{
					if (Level == 0 || Children == null)
					{
						heatPoint = Midpoint;
						return Value;
					}
					heatPoint = Vector3D.Zero;
					float num = 0f;
					foreach (OctreeHeatmapNode child in Children)
					{
						Vector3D heatPoint2;
						float highestHeatPoint = child.GetHighestHeatPoint(out heatPoint2);
						if (highestHeatPoint > num)
						{
							heatPoint = heatPoint2;
							num = highestHeatPoint;
						}
					}
					return num;
				}
			}

			public OctreeHeatmapNode Root;

			public void BuildRootFromBounds(Vector3D min, Vector3D max)
			{
				Vector3D vector3D = max - min;
				Vector3D midpoint = 0.5 * (max + min);
				double num = double.MinValue;
				if (vector3D.X > num)
				{
					num = vector3D.X;
				}
				if (vector3D.Y > num)
				{
					num = vector3D.Y;
				}
				if (vector3D.Z > num)
				{
					num = vector3D.Z;
				}
				int num2 = -1;
				for (int i = 0; i < OctreeHeatmapNode.PRECOUNT_AMOUNT; i++)
				{
					if (num < 2.0 * OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[i])
					{
						num2 = i;
						_ = OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[i];
						break;
					}
				}
				if (num2 == -1)
				{
					Root = null;
				}
				else
				{
					OctreeHeatmapNode octreeHeatmapNode = (Root = new OctreeHeatmapNode(num2, midpoint));
				}
			}

			public void AddValueInSphere(float delta, Vector3D position, double radius)
			{
				Vector3D queryMin = position - radius;
				Vector3D queryMax = position + radius;
				while (!Root.IsAABBQueryIInside(queryMin, queryMax))
				{
					Root = Root.BuildParentByQuery(queryMin, queryMax);
				}
				Root.WriteQueryRecursive(delta, ref position, radius);
			}

			public void DebugPrint()
			{
			}

			public void DebugDrawTree()
			{
				if (Root == null)
				{
					return;
				}
				Vector3D vector3D = Root.Midpoint - OctreeHeatmapNode.PRECOUNTED_HALF_EXTENTS[Root.Level];
				double lEAF_SIZE = OctreeHeatmapNode.LEAF_SIZE;
				Vector3D vector3D2 = vector3D + 0.5 * OctreeHeatmapNode.LEAF_SIZE;
				double num = 0.5;
				int num2 = 2 << Root.Level;
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						for (int k = 0; k < num2; k++)
						{
							float num3 = Root.Sample(vector3D2 + new Vector3D((double)i * lEAF_SIZE, (double)j * lEAF_SIZE, (double)k * lEAF_SIZE));
							if (num3 > 0f)
							{
								MyRenderProxy.DebugDrawPoint(new Vector3D((double)i * num, (double)j * num, (double)k * num), ColorDecoder(num3), depthRead: false, persistent: true);
							}
							_ = 0f;
						}
					}
				}
				double num4 = (double)num2 * num;
				MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(num4, 0.0, 0.0), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(0.0, num4, 0.0), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(Vector3D.Zero, new Vector3D(0.0, 0.0, num4), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(0.0, num4, num4), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(num4, 0.0, num4), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(new Vector3D(num4), new Vector3D(num4, num4, 0.0), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
			}

			public Color ColorDecoder(float sample)
			{
				if (sample < 1f)
				{
					return Color.Black;
				}
				if (sample < 2f)
				{
					return Color.PaleGoldenrod;
				}
				if (sample < 3f)
				{
					return Color.Yellow;
				}
				if (sample < 4f)
				{
					return Color.Orange;
				}
				if (sample < 5f)
				{
					return Color.OrangeRed;
				}
				return Color.White;
			}

			internal float GetHighestHeat(out Vector3D heatPoint)
			{
				if (Root == null)
				{
					heatPoint = Vector3D.Zero;
					return -1f;
				}
				return Root.GetHighestHeatPoint(out heatPoint);
			}
		}

		public float GetHighestHeatPoint(out Vector3D heatPoint, double heatRange = 3000.0)
		{
			MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
			Vector3D min = new Vector3D(double.MaxValue);
			Vector3D max = new Vector3D(double.MinValue);
			List<Vector3D> list = new List<Vector3D>();
			foreach (MyEntity item in entities)
			{
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = item as MyCubeGrid) != null && !myCubeGrid.IsStatic)
				{
					Vector3D position = item.PositionComp.GetPosition();
					ExpandMinMax(ref min, ref max, position);
					list.Add(position);
				}
			}
			min -= heatRange;
			max += heatRange;
			OctreeHeatmap octreeHeatmap = new OctreeHeatmap();
			octreeHeatmap.BuildRootFromBounds(min, max);
			foreach (Vector3D item2 in list)
			{
				Vector3D position2 = item2;
				octreeHeatmap.Root.WriteQueryRecursive(1f, ref position2, heatRange);
			}
			return octreeHeatmap.GetHighestHeat(out heatPoint);
		}

		public static void TEST_01()
		{
			OctreeHeatmap octreeHeatmap = new OctreeHeatmap();
			Vector3D min = new Vector3D(0.0, 0.0, 0.0);
			Vector3D max = new Vector3D(300.0, 300.0, 300.0);
			octreeHeatmap.BuildRootFromBounds(min, max);
			min = new Vector3D(0.0, 0.0, 0.0);
			max = new Vector3D(600.0, 600.0, 600.0);
			octreeHeatmap.BuildRootFromBounds(min, max);
			min = new Vector3D(-1000.0, -1000.0, -1000.0);
			max = new Vector3D(999.0, 999.0, 999.0);
			octreeHeatmap.BuildRootFromBounds(min, max);
			Vector3D position = new Vector3D(250.0, 250.0, 250.0);
			octreeHeatmap.Root.WriteQueryRecursive(1f, ref position, 501.0);
			octreeHeatmap.Root.DebugPrintChildrenDraw();
			octreeHeatmap.Root.DebugPrintChildrenRecursion("");
		}

		public static void TEST_02()
		{
			OctreeHeatmap octreeHeatmap = new OctreeHeatmap();
			Vector3D min = new Vector3D(0.0, 0.0, 0.0);
			Vector3D max = new Vector3D(10000.0, 10000.0, 10000.0);
			octreeHeatmap.BuildRootFromBounds(min, max);
			Vector3D position = new Vector3D(750.0, 750.0, 750.0);
			octreeHeatmap.Root.WriteQueryRecursive(1f, ref position, 3000.0);
			position = new Vector3D(4750.0, 2750.0, 1750.0);
			octreeHeatmap.Root.WriteQueryRecursive(1.5f, ref position, 3000.0);
			position = new Vector3D(2500.0, 1500.0, 3000.0);
			octreeHeatmap.Root.WriteQueryRecursive(2f, ref position, 2000.0);
			octreeHeatmap.DebugDrawTree();
		}

		private void ExpandMinMax(ref Vector3D min, ref Vector3D max, Vector3D position)
		{
			if (position.X < min.X)
			{
				min.X = position.X;
			}
			if (position.Y < min.Y)
			{
				min.Y = position.Y;
			}
			if (position.Z < min.Z)
			{
				min.Z = position.Z;
			}
			if (position.X > max.X)
			{
				max.X = position.X;
			}
			if (position.Y > max.Y)
			{
				max.Y = position.Y;
			}
			if (position.Z > max.Z)
			{
				max.Z = position.Z;
			}
		}
	}
}
