using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sandbox.Engine.Utils;
using VRage.Algorithms;
using VRage.Generics;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public abstract class MyNavigationMesh : MyPathFindingSystem<MyNavigationPrimitive>, IMyNavigationGroup
	{
		private class Funnel
		{
			private enum PointTestResult
			{
				LEFT,
				INSIDE,
				RIGHT
			}

			private Vector3 m_end;

			private int m_endIndex;

			private MyPath<MyNavigationPrimitive> m_input;

			private List<Vector4D> m_output;

			private Vector3 m_apex;

			private Vector3 m_apexNormal;

			private Vector3 m_leftPoint;

			private Vector3 m_rightPoint;

			private int m_leftIndex;

			private int m_rightIndex;

			private Vector3 m_leftPlaneNormal;

			private Vector3 m_rightPlaneNormal;

			private float m_leftD;

			private float m_rightD;

			private bool m_funnelConstructed;

			private bool m_segmentDangerous;

			private static readonly float SAFE_DISTANCE = 0.7f;

			private static readonly float SAFE_DISTANCE_SQ = SAFE_DISTANCE * SAFE_DISTANCE;

			private static readonly float SAFE_DISTANCE2_SQ = (SAFE_DISTANCE + SAFE_DISTANCE) * (SAFE_DISTANCE + SAFE_DISTANCE);

			public void Calculate(MyPath<MyNavigationPrimitive> inputPath, List<Vector4D> refinedPath, ref Vector3 start, ref Vector3 end, int startIndex, int endIndex)
			{
				m_debugFunnel.Clear();
				m_debugPointsLeft.Clear();
				m_debugPointsRight.Clear();
				m_end = end;
				m_endIndex = endIndex;
				m_input = inputPath;
				m_output = refinedPath;
				m_apex = start;
				m_funnelConstructed = false;
				m_segmentDangerous = false;
				int num = startIndex;
				while (num < endIndex)
				{
					num = AddTriangle(num);
					if (num == endIndex)
					{
						PointTestResult pointTestResult = TestPoint(end);
						switch (pointTestResult)
						{
						case PointTestResult.LEFT:
							m_apex = m_leftPoint;
							m_funnelConstructed = false;
							ConstructFunnel(m_leftIndex);
							num = m_leftIndex + 1;
							break;
						case PointTestResult.RIGHT:
							m_apex = m_rightPoint;
							m_funnelConstructed = false;
							ConstructFunnel(m_rightIndex);
							num = m_rightIndex + 1;
							break;
						}
						if (pointTestResult == PointTestResult.INSIDE || num == endIndex)
						{
							AddPoint(ProjectEndOnTriangle(num));
						}
					}
				}
				if (startIndex == endIndex)
				{
					AddPoint(ProjectEndOnTriangle(num));
				}
				m_input = null;
				m_output = null;
			}

			private void AddPoint(Vector3D point)
			{
				float num = (m_segmentDangerous ? 0.5f : 2f);
				m_output.Add(new Vector4D(point, num));
				int num2 = m_output.Count - 1;
				if (num2 >= 0)
				{
					Vector4D value = m_output[num2];
					if (value.W > (double)num)
					{
						value.W = num;
						m_output[num2] = value;
					}
				}
				m_segmentDangerous = false;
			}

			private Vector3 ProjectEndOnTriangle(int i)
			{
				return (m_input[i].Vertex as MyNavigationTriangle).ProjectLocalPoint(m_end);
			}

			private int AddTriangle(int index)
			{
				if (!m_funnelConstructed)
				{
					ConstructFunnel(index);
				}
				else
				{
					MyPath<MyNavigationPrimitive>.PathNode pathNode = m_input[index];
					MyNavigationTriangle myNavigationTriangle = pathNode.Vertex as MyNavigationTriangle;
					myNavigationTriangle.GetNavigationEdge(pathNode.nextVertex);
					GetEdgeVerticesSafe(myNavigationTriangle, pathNode.nextVertex, out var left, out var right);
					PointTestResult pointTestResult = TestPoint(left);
					PointTestResult pointTestResult2 = TestPoint(right);
					if (pointTestResult == PointTestResult.INSIDE)
					{
						NarrowFunnel(left, index, left: true);
					}
					if (pointTestResult2 == PointTestResult.INSIDE)
					{
						NarrowFunnel(right, index, left: false);
					}
					if (pointTestResult == PointTestResult.RIGHT)
					{
						m_apex = m_rightPoint;
						m_funnelConstructed = false;
						ConstructFunnel(m_rightIndex + 1);
						return m_rightIndex + 1;
					}
					if (pointTestResult2 == PointTestResult.LEFT)
					{
						m_apex = m_leftPoint;
						m_funnelConstructed = false;
						ConstructFunnel(m_leftIndex + 1);
						return m_leftIndex + 1;
					}
					if (pointTestResult == PointTestResult.INSIDE || pointTestResult2 == PointTestResult.INSIDE)
					{
						m_debugFunnel.Add(new FunnelState
						{
							Apex = m_apex,
							Left = m_leftPoint,
							Right = m_rightPoint
						});
					}
				}
				return index + 1;
			}

			private void GetEdgeVerticesSafe(MyNavigationTriangle triangle, int edgeIndex, out Vector3 left, out Vector3 right)
			{
				triangle.GetEdgeVertices(edgeIndex, out left, out right);
				float num = (left - right).LengthSquared();
				bool flag = triangle.IsEdgeVertexDangerous(edgeIndex, predVertex: true);
				bool flag2 = triangle.IsEdgeVertexDangerous(edgeIndex, predVertex: false);
				m_segmentDangerous |= flag || flag2;
				if (flag)
				{
					if (flag2)
					{
						if (SAFE_DISTANCE2_SQ > num)
						{
							left = (left + right) * 0.5f;
							right = left;
						}
						else
						{
							float num2 = SAFE_DISTANCE / (float)Math.Sqrt(num);
							Vector3 vector = right * num2 + left * (1f - num2);
							right = left * num2 + right * (1f - num2);
							left = vector;
						}
					}
					else if (SAFE_DISTANCE_SQ > num)
					{
						left = right;
					}
					else
					{
						float num3 = SAFE_DISTANCE / (float)Math.Sqrt(num);
						left = right * num3 + left * (1f - num3);
					}
				}
				else if (flag2)
				{
					if (SAFE_DISTANCE_SQ > num)
					{
						right = left;
					}
					else
					{
						float num4 = SAFE_DISTANCE / (float)Math.Sqrt(num);
						right = left * num4 + right * (1f - num4);
					}
				}
				m_debugPointsLeft.Add(left);
				m_debugPointsRight.Add(right);
			}

			private void NarrowFunnel(Vector3 point, int index, bool left)
			{
				if (left)
				{
					m_leftPoint = point;
					m_leftIndex = index;
					RecalculateLeftPlane();
				}
				else
				{
					m_rightPoint = point;
					m_rightIndex = index;
					RecalculateRightPlane();
				}
			}

			private void ConstructFunnel(int index)
			{
				if (index >= m_endIndex)
				{
					AddPoint(m_apex);
					return;
				}
				MyPath<MyNavigationPrimitive>.PathNode pathNode = m_input[index];
				MyNavigationTriangle myNavigationTriangle = pathNode.Vertex as MyNavigationTriangle;
				myNavigationTriangle.GetNavigationEdge(pathNode.nextVertex);
				GetEdgeVerticesSafe(myNavigationTriangle, pathNode.nextVertex, out m_leftPoint, out m_rightPoint);
				if (Vector3.IsZero(m_leftPoint - m_apex))
				{
					m_apex = myNavigationTriangle.Center;
					return;
				}
				if (Vector3.IsZero(m_rightPoint - m_apex))
				{
					m_apex = myNavigationTriangle.Center;
					return;
				}
				m_apexNormal = myNavigationTriangle.Normal;
				float num = m_leftPoint.Dot(m_apexNormal);
				m_apex -= m_apexNormal * (m_apex.Dot(m_apexNormal) - num);
				m_leftIndex = (m_rightIndex = index);
				RecalculateLeftPlane();
				RecalculateRightPlane();
				m_funnelConstructed = true;
				AddPoint(m_apex);
				m_debugFunnel.Add(new FunnelState
				{
					Apex = m_apex,
					Left = m_leftPoint,
					Right = m_rightPoint
				});
			}

			private PointTestResult TestPoint(Vector3 point)
			{
				if (point.Dot(m_leftPlaneNormal) < 0f - m_leftD)
				{
					return PointTestResult.LEFT;
				}
				if (point.Dot(m_rightPlaneNormal) < 0f - m_rightD)
				{
					return PointTestResult.RIGHT;
				}
				return PointTestResult.INSIDE;
			}

			private void RecalculateLeftPlane()
			{
				Vector3 vector = m_leftPoint - m_apex;
				vector.Normalize();
				m_leftPlaneNormal = Vector3.Cross(vector, m_apexNormal);
				m_leftPlaneNormal.Normalize();
				m_leftD = 0f - m_leftPoint.Dot(m_leftPlaneNormal);
			}

			private void RecalculateRightPlane()
			{
				Vector3 vector = m_rightPoint - m_apex;
				vector.Normalize();
				m_rightPlaneNormal = Vector3.Cross(m_apexNormal, vector);
				m_rightPlaneNormal.Normalize();
				m_rightD = 0f - m_rightPoint.Dot(m_rightPlaneNormal);
			}
		}

		public struct FunnelState
		{
			public Vector3 Apex;

			public Vector3 Left;

			public Vector3 Right;
		}

		private MyDynamicObjectPool<MyNavigationTriangle> m_triPool;

		private readonly MyNavgroupLinks m_externalLinks;

		private Vector3 m_vertex;

		private Vector3 m_left;

		private Vector3 m_right;

		private Vector3 m_normal;

		private static readonly List<Vector3> m_debugPointsLeft = new List<Vector3>();

		private static readonly List<Vector3> m_debugPointsRight = new List<Vector3>();

		private static readonly List<Vector3> m_path = new List<Vector3>();

		private static List<Vector3> m_path2;

		private static readonly List<FunnelState> m_debugFunnel = new List<FunnelState>();

		public static int m_debugFunnelIdx = 0;

		public MyWingedEdgeMesh Mesh { get; }

		public abstract MyHighLevelGroup HighLevelGroup { get; }

		protected MyNavigationMesh(MyNavgroupLinks externalLinks, int trianglePrealloc = 16, Func<long> timestampFunction = null)
			: base(128, timestampFunction)
		{
			m_triPool = new MyDynamicObjectPool<MyNavigationTriangle>(trianglePrealloc);
			Mesh = new MyWingedEdgeMesh();
			m_externalLinks = externalLinks;
		}

		protected MyNavigationTriangle AddTriangle(ref Vector3 A, ref Vector3 B, ref Vector3 C, ref int edgeAB, ref int edgeBC, ref int edgeCA)
		{
			MyNavigationTriangle myNavigationTriangle = m_triPool.Allocate();
			int num = 0;
			num += ((edgeAB == -1) ? 1 : 0);
			num += ((edgeBC == -1) ? 1 : 0);
			num += ((edgeCA == -1) ? 1 : 0);
			int num2 = -1;
			switch (num)
			{
			case 3:
				num2 = Mesh.MakeNewTriangle(myNavigationTriangle, ref A, ref B, ref C, out edgeAB, out edgeBC, out edgeCA);
				break;
			case 2:
				num2 = ((edgeAB == -1) ? ((edgeBC == -1) ? Mesh.ExtrudeTriangleFromEdge(ref B, edgeCA, myNavigationTriangle, out edgeAB, out edgeBC) : Mesh.ExtrudeTriangleFromEdge(ref A, edgeBC, myNavigationTriangle, out edgeCA, out edgeAB)) : Mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, myNavigationTriangle, out edgeBC, out edgeCA));
				break;
			case 1:
				num2 = ((edgeAB != -1) ? ((edgeBC != -1) ? GetTriangleOneNewEdge(ref edgeCA, ref edgeAB, ref edgeBC, myNavigationTriangle) : GetTriangleOneNewEdge(ref edgeBC, ref edgeCA, ref edgeAB, myNavigationTriangle)) : GetTriangleOneNewEdge(ref edgeAB, ref edgeBC, ref edgeCA, myNavigationTriangle));
				break;
			default:
			{
				MyWingedEdgeMesh.Edge other = Mesh.GetEdge(edgeAB);
				MyWingedEdgeMesh.Edge other2 = Mesh.GetEdge(edgeBC);
				MyWingedEdgeMesh.Edge other3 = Mesh.GetEdge(edgeCA);
				int num3 = other3.TryGetSharedVertex(ref other);
				int num4 = other.TryGetSharedVertex(ref other2);
				int num5 = other2.TryGetSharedVertex(ref other3);
				int num6 = 0;
				num6 += ((num3 != -1) ? 1 : 0);
				num6 += ((num4 != -1) ? 1 : 0);
				switch (num6 + ((num5 != -1) ? 1 : 0))
				{
				case 3:
					num2 = Mesh.MakeFace(myNavigationTriangle, edgeAB);
					break;
				case 2:
					num2 = ((num3 != -1) ? ((num4 != -1) ? GetTriangleTwoSharedVertices(edgeCA, edgeAB, ref edgeBC, num3, num4, myNavigationTriangle) : GetTriangleTwoSharedVertices(edgeBC, edgeCA, ref edgeAB, num5, num3, myNavigationTriangle)) : GetTriangleTwoSharedVertices(edgeAB, edgeBC, ref edgeCA, num4, num5, myNavigationTriangle));
					break;
				case 1:
					num2 = ((num3 == -1) ? ((num4 == -1) ? GetTriangleOneSharedVertex(edgeBC, edgeCA, ref edgeAB, num5, myNavigationTriangle) : GetTriangleOneSharedVertex(edgeAB, edgeBC, ref edgeCA, num4, myNavigationTriangle)) : GetTriangleOneSharedVertex(edgeCA, edgeAB, ref edgeBC, num3, myNavigationTriangle));
					break;
				default:
				{
					num2 = Mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, myNavigationTriangle, out var newEdgeS, out var newEdgeP);
					Mesh.MergeEdges(newEdgeP, edgeCA);
					Mesh.MergeEdges(newEdgeS, edgeBC);
					break;
				}
				}
				break;
			}
			}
			myNavigationTriangle.Init(this, num2);
			return myNavigationTriangle;
		}

		protected void RemoveTriangle(MyNavigationTriangle tri)
		{
			Mesh.RemoveFace(tri.Index);
			m_triPool.Deallocate(tri);
		}

		private int GetTriangleOneNewEdge(ref int newEdge, ref int succ, ref int pred, MyNavigationTriangle newTri)
		{
			MyWingedEdgeMesh.Edge edge = Mesh.GetEdge(pred);
			MyWingedEdgeMesh.Edge other = Mesh.GetEdge(succ);
			int num = edge.TryGetSharedVertex(ref other);
			if (num == -1)
			{
				int edge2 = succ;
				Vector3 newVertex = Mesh.GetVertexPosition(other.GetFacePredVertex(-1));
				int result = Mesh.ExtrudeTriangleFromEdge(ref newVertex, pred, newTri, out newEdge, out succ);
				Mesh.MergeEdges(edge2, succ);
				return result;
			}
			int vert = edge.OtherVertex(num);
			int vert2 = other.OtherVertex(num);
			return Mesh.MakeEdgeFace(vert, vert2, pred, succ, newTri, out newEdge);
		}

		private int GetTriangleOneSharedVertex(int edgeCA, int edgeAB, ref int edgeBC, int sharedA, MyNavigationTriangle newTri)
		{
			int vert = Mesh.GetEdge(edgeAB).OtherVertex(sharedA);
			int vert2 = Mesh.GetEdge(edgeCA).OtherVertex(sharedA);
			int edge = edgeBC;
			int result = Mesh.MakeEdgeFace(vert, vert2, edgeAB, edgeCA, newTri, out edgeBC);
			Mesh.MergeEdges(edge, edgeBC);
			return result;
		}

		private int GetTriangleTwoSharedVertices(int edgeAB, int edgeBC, ref int edgeCA, int sharedB, int sharedC, MyNavigationTriangle newTri)
		{
			int vert = Mesh.GetEdge(edgeAB).OtherVertex(sharedB);
			int leftEdge = edgeCA;
			int result = Mesh.MakeEdgeFace(sharedC, vert, edgeBC, edgeAB, newTri, out edgeCA);
			Mesh.MergeAngle(leftEdge, edgeCA, sharedC);
			return result;
		}

		public MyNavigationTriangle GetTriangle(int index)
		{
			return Mesh.GetFace(index).GetUserData<MyNavigationTriangle>();
		}

		protected MyNavigationTriangle GetEdgeTriangle(int edgeIndex)
		{
			MyWingedEdgeMesh.Edge edge = Mesh.GetEdge(edgeIndex);
			if (edge.LeftFace == -1)
			{
				return GetTriangle(edge.RightFace);
			}
			return GetTriangle(edge.LeftFace);
		}

		protected List<Vector4D> FindRefinedPath(MyNavigationTriangle start, MyNavigationTriangle end, ref Vector3 startPoint, ref Vector3 endPoint)
		{
			MyPath<MyNavigationPrimitive> myPath = FindPath(start, end);
			if (myPath == null)
			{
				return null;
			}
			List<Vector4D> list = new List<Vector4D>();
			list.Add(new Vector4D(startPoint, 1.0));
			new Funnel().Calculate(myPath, list, ref startPoint, ref endPoint, 0, myPath.Count - 1);
			m_path.Clear();
			foreach (Vector4D item in list)
			{
<<<<<<< HEAD
				m_path.Add(new Vector3(item));
=======
				m_path.Add(new Vector3D(item));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return list;
		}

		public void RefinePath(MyPath<MyNavigationPrimitive> path, List<Vector4D> output, ref Vector3 startPoint, ref Vector3 endPoint, int begin, int end)
		{
			new Funnel().Calculate(path, output, ref startPoint, ref endPoint, begin, end);
		}

		public abstract Vector3 GlobalToLocal(Vector3D globalPos);

		public abstract Vector3D LocalToGlobal(Vector3 localPos);

		public abstract MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle);

		public abstract IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive);

		public abstract MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq);

<<<<<<< HEAD
		/// <summary>
		/// Gets rid of the vertex and edge preallocation pools. You MUST NOT add any more triangles or edges after calling this method.
		/// It is here only to save memory when the mesh won't be modified any more.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void ErasePools()
		{
			m_triPool = null;
		}

		[Conditional("DEBUG")]
		public virtual void DebugDraw(ref Matrix drawMatrix)
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != 0)
			{
				Mesh.DebugDraw(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
				Mesh.CustomDebugDrawFaces(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES, (object obj) => (obj as MyNavigationTriangle).Index.ToString());
			}
			if (!MyFakes.DEBUG_DRAW_FUNNEL)
			{
				return;
			}
			MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_vertex, drawMatrix), 0.05f, Color.Yellow.ToVector3(), 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_vertex + m_normal, drawMatrix), 0.05f, Color.Orange.ToVector3(), 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_left, drawMatrix), 0.05f, Color.Red.ToVector3(), 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_right, drawMatrix), 0.05f, Color.Green.ToVector3(), 1f, depthRead: false);
			foreach (Vector3 item in m_debugPointsLeft)
			{
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(item, drawMatrix), 0.03f, Color.Red.ToVector3(), 1f, depthRead: false);
			}
			foreach (Vector3 item2 in m_debugPointsRight)
			{
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(item2, drawMatrix), 0.04f, Color.Green.ToVector3(), 1f, depthRead: false);
			}
			Vector3? vector = null;
			if (m_path != null)
			{
				foreach (Vector3 item3 in m_path)
				{
					Vector3 vector2 = Vector3.Transform(item3, drawMatrix);
					MyRenderProxy.DebugDrawSphere(vector2 + Vector3.Up * 0.2f, 0.02f, Color.Orange.ToVector3(), 1f, depthRead: false);
					if (vector.HasValue)
					{
						MyRenderProxy.DebugDrawLine3D(vector.Value + Vector3.Up * 0.2f, vector2 + Vector3.Up * 0.2f, Color.Orange, Color.Orange, depthRead: true);
					}
					vector = vector2;
				}
			}
			vector = null;
			if (m_path2 != null)
			{
				foreach (Vector3 item4 in m_path2)
				{
					Vector3 vector3 = Vector3.Transform(item4, drawMatrix);
					if (vector.HasValue)
					{
						MyRenderProxy.DebugDrawLine3D(vector.Value + Vector3.Up * 0.1f, vector3 + Vector3.Up * 0.1f, Color.Violet, Color.Violet, depthRead: true);
					}
					vector = vector3;
				}
			}
			if (m_debugFunnel.Count > 0)
			{
				FunnelState funnelState = m_debugFunnel[m_debugFunnelIdx % m_debugFunnel.Count];
				Vector3 vector4 = Vector3.Transform(funnelState.Apex, drawMatrix);
				Vector3 vector5 = Vector3.Transform(funnelState.Left, drawMatrix);
				Vector3 vector6 = Vector3.Transform(funnelState.Right, drawMatrix);
				vector5 = vector4 + (vector5 - vector4) * 10f;
				vector6 = vector4 + (vector6 - vector4) * 10f;
				Color cyan = Color.Cyan;
				MyRenderProxy.DebugDrawLine3D(vector4 + Vector3.Up * 0.1f, vector5 + Vector3.Up * 0.1f, cyan, cyan, depthRead: true);
				MyRenderProxy.DebugDrawLine3D(vector4 + Vector3.Up * 0.1f, vector6 + Vector3.Up * 0.1f, cyan, cyan, depthRead: true);
			}
		}

		public void RemoveFace(int index)
		{
			Mesh.RemoveFace(index);
		}

		public virtual MatrixD GetWorldMatrix()
		{
			return MatrixD.Identity;
		}

		[Conditional("DEBUG")]
		public void CheckMeshConsistency()
		{
		}

		public int ApproximateMemoryFootprint()
		{
<<<<<<< HEAD
			return Mesh.ApproximateMemoryFootprint() + m_triPool.Count * (Environment.Is64BitProcess ? 88 : 56);
=======
			return Mesh.ApproximateMemoryFootprint() + m_triPool.Count * (Environment.get_Is64BitProcess() ? 88 : 56);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public int GetExternalNeighborCount(MyNavigationPrimitive primitive)
		{
			return m_externalLinks?.GetLinkCount(primitive) ?? 0;
		}

		public MyNavigationPrimitive GetExternalNeighbor(MyNavigationPrimitive primitive, int index)
		{
			return m_externalLinks?.GetLinkedNeighbor(primitive, index);
		}

		public IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(MyNavigationPrimitive primitive, int index)
		{
			return m_externalLinks?.GetEdge(primitive, index);
		}
	}
}
