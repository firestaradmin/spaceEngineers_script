using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyNavigationTriangle : MyNavigationPrimitive
	{
		public bool Registered;

		public MyNavigationMesh Parent { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Face index of this triangle in the winged-edge mesh
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int Index { get; private set; }

		public int ComponentIndex { get; set; }

		public Vector3 Center
		{
			get
			{
				int num = 0;
				Vector3 zero = Vector3.Zero;
				MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
				while (vertexEnumerator.MoveNext())
				{
					zero += vertexEnumerator.Current;
					num++;
				}
				return zero / num;
			}
		}

		public Vector3 Normal
		{
			get
			{
				MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
				vertexEnumerator.MoveNext();
				Vector3 current = vertexEnumerator.Current;
				vertexEnumerator.MoveNext();
				Vector3 current2 = vertexEnumerator.Current;
				vertexEnumerator.MoveNext();
				Vector3 result = (vertexEnumerator.Current - current).Cross(current2 - current);
				result.Normalize();
				return result;
			}
		}

		public override Vector3 Position => Center;

		public override Vector3D WorldPosition
		{
			get
			{
				MatrixD matrix = Parent.GetWorldMatrix();
				Vector3D position = Center;
				Vector3D.Transform(ref position, ref matrix, out var result);
				return result;
			}
		}

		public override IMyNavigationGroup Group => Parent;

		public void Init(MyNavigationMesh mesh, int triangleIndex)
		{
			Parent = mesh;
			Index = triangleIndex;
			ComponentIndex = -1;
			Registered = false;
			base.HasExternalNeighbors = false;
		}

		public override string ToString()
		{
			return string.Concat(Parent, "; Tri: ", Index);
		}

		public void GetVertices(out Vector3 a, out Vector3 b, out Vector3 c)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			a = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			b = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			c = vertexEnumerator.Current;
		}

		public void GetVertices(out int indA, out int indB, out int indC, out Vector3 a, out Vector3 b, out Vector3 c)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			indA = vertexEnumerator.CurrentIndex;
			a = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			indB = vertexEnumerator.CurrentIndex;
			b = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			indC = vertexEnumerator.CurrentIndex;
			c = vertexEnumerator.Current;
		}

		public void GetTransformed(ref MatrixI tform, out Vector3 newA, out Vector3 newB, out Vector3 newC)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			newA = vertexEnumerator.Current;
			Vector3.Transform(ref newA, ref tform, out newA);
			vertexEnumerator.MoveNext();
			newB = vertexEnumerator.Current;
			Vector3.Transform(ref newB, ref tform, out newB);
			vertexEnumerator.MoveNext();
			newC = vertexEnumerator.Current;
			Vector3.Transform(ref newC, ref tform, out newC);
		}

		public MyWingedEdgeMesh.FaceVertexEnumerator GetVertexEnumerator()
		{
			return Parent.Mesh.GetFace(Index).GetVertexEnumerator();
		}

		public MyNavigationEdge GetNavigationEdge(int index)
		{
			MyWingedEdgeMesh mesh = Parent.Mesh;
			int edgeIndex = GetEdgeIndex(index);
			MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex);
			MyNavigationTriangle triA = null;
			MyNavigationTriangle triB = null;
			if (edge.LeftFace != -1)
			{
				triA = mesh.GetFace(edge.LeftFace).GetUserData<MyNavigationTriangle>();
			}
			if (edge.RightFace != -1)
			{
				triB = mesh.GetFace(edge.RightFace).GetUserData<MyNavigationTriangle>();
			}
			MyNavigationEdge.Static.Init(triA, triB, edgeIndex);
			return MyNavigationEdge.Static;
		}

		public void GetEdgeVertices(int index, out Vector3 pred, out Vector3 succ)
		{
			MyWingedEdgeMesh mesh = Parent.Mesh;
			int edgeIndex = GetEdgeIndex(index);
			MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex);
			pred = mesh.GetVertexPosition(edge.GetFacePredVertex(Index));
			succ = mesh.GetVertexPosition(edge.GetFaceSuccVertex(Index));
		}

<<<<<<< HEAD
		/// <summary>
		/// Whether it's dangerous for the bot to navigate close to this edge
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsEdgeVertexDangerous(int index, bool predVertex)
		{
			MyWingedEdgeMesh mesh = Parent.Mesh;
			int edgeIndex = GetEdgeIndex(index);
			int edgeIndex2 = edgeIndex;
			MyWingedEdgeMesh.Edge edge = mesh.GetEdge(edgeIndex2);
			int vertexIndex = (predVertex ? edge.GetFacePredVertex(Index) : edge.GetFaceSuccVertex(Index));
			do
			{
				if (IsTriangleDangerous(edge.VertexLeftFace(vertexIndex)))
				{
					return true;
				}
				edgeIndex2 = edge.GetNextVertexEdge(vertexIndex);
				edge = mesh.GetEdge(edgeIndex2);
			}
			while (edgeIndex2 != edgeIndex);
			return false;
		}

		public void FindDangerousVertices(List<int> output)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = Parent.Mesh.GetFace(Index).GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			_ = vertexEnumerator.CurrentIndex;
			vertexEnumerator.MoveNext();
			_ = vertexEnumerator.CurrentIndex;
			vertexEnumerator.MoveNext();
			_ = vertexEnumerator.CurrentIndex;
		}

		public int GetEdgeIndex(int index)
		{
			MyWingedEdgeMesh.FaceEdgeEnumerator faceEdgeEnumerator = new MyWingedEdgeMesh.FaceEdgeEnumerator(Parent.Mesh, Index);
			faceEdgeEnumerator.MoveNext();
			while (index != 0)
			{
				faceEdgeEnumerator.MoveNext();
				index--;
			}
			return faceEdgeEnumerator.Current;
		}

		private static bool IsTriangleDangerous(int triIndex)
		{
			return triIndex == -1;
		}

		public override Vector3 ProjectLocalPoint(Vector3 point)
		{
			GetVertices(out var a, out var b, out var c);
			Vector3.Subtract(ref b, ref a, out var result);
			Vector3.Subtract(ref c, ref a, out var result2);
			Vector3.Subtract(ref point, ref a, out var result3);
			Vector3.Cross(ref result, ref result2, out var result4);
			Vector3.Cross(ref result, ref result3, out var result5);
			Vector3.Cross(ref result3, ref result2, out var result6);
			float num = 1f / result4.LengthSquared();
			float num2 = Vector3.Dot(result5, result4);
			float num3 = Vector3.Dot(result6, result4) * num;
			float num4 = num2 * num;
			float num5 = 1f - num3 - num4;
			if (num5 < 0f)
			{
				if (num3 < 0f)
				{
					return c;
				}
				if (num4 < 0f)
				{
					return b;
				}
				float num6 = 1f / (1f - num5);
				num5 = 0f;
				num3 *= num6;
				num4 *= num6;
			}
			else if (num3 < 0f)
			{
				if (num4 < 0f)
				{
					return a;
				}
				float num7 = 1f / (1f - num3);
				num3 = 0f;
				num5 *= num7;
				num4 *= num7;
			}
			else if (num4 < 0f)
			{
				float num8 = 1f / (1f - num4);
				num4 = 0f;
				num5 *= num8;
				num3 *= num8;
			}
			return a * num5 + b * num3 + c * num4;
		}

		public override int GetOwnNeighborCount()
		{
			return 3;
		}

		public override IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(int index)
		{
			int edgeIndex = GetEdgeIndex(index);
			int num = Parent.Mesh.GetEdge(edgeIndex).OtherFace(Index);
			if (num != -1)
			{
				return Parent.Mesh.GetFace(num).GetUserData<MyNavigationPrimitive>();
			}
			return null;
		}

		public override IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(int index)
		{
			return GetNavigationEdge(index);
		}

		public override MyHighLevelPrimitive GetHighLevelPrimitive()
		{
			return Parent.GetHighLevelPrimitive(this);
		}
	}
}
