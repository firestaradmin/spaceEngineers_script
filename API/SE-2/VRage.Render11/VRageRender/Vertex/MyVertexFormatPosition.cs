using VRageMath;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPosition
	{
		internal Vector3 Position;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPosition);

		internal MyVertexFormatPosition(Vector3 position)
		{
			Position = position;
		}
	}
}
