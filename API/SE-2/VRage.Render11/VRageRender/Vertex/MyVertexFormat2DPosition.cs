using VRageMath;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormat2DPosition
	{
		internal Vector2 Position;

		internal unsafe static int STRIDE = sizeof(MyVertexFormat2DPosition);

		internal MyVertexFormat2DPosition(Vector2 position)
		{
			Position = position;
		}
	}
}
