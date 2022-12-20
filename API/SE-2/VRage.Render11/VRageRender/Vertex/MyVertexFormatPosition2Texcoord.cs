using VRageMath;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPosition2Texcoord
	{
		internal Vector2 Position;

		internal Vector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPosition2Texcoord);

		internal MyVertexFormatPosition2Texcoord(Vector2 position, Vector2 texcoord)
		{
			Position = position;
			Texcoord = texcoord;
		}
	}
}
