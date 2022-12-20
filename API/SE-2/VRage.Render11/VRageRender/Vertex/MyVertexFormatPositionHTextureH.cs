using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionHTextureH
	{
		internal HalfVector4 Position;

		internal HalfVector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionHTextureH);

		internal MyVertexFormatPositionHTextureH(HalfVector4 position, HalfVector2 texcoord)
		{
			Position = position;
			Texcoord = texcoord;
		}
	}
}
