using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
	public class MyConvexVerticesInfo
	{
		public Matrix m_rdWorldMatrix;

		public Vector3[] Vertices { get; set; }

		public MyConvexVerticesInfo(Matrix rdWorldMatrix, Vector3[] vertices)
		{
			m_rdWorldMatrix = rdWorldMatrix;
			Vertices = vertices;
		}
	}
}
