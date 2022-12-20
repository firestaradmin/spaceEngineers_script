using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionTextureH
	{
		internal Vector3 Position;

		internal HalfVector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionTextureH);

		internal MyVertexFormatPositionTextureH(Vector3 position, HalfVector2 texcoord)
		{
			Position = position;
			Texcoord = texcoord;
		}
	}
}
