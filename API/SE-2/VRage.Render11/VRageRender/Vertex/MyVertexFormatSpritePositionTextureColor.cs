using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatSpritePositionTextureColor
	{
		internal HalfVector4 ClipspaceOffsetScale;

		internal HalfVector4 TexcoordOffsetScale;

		internal Byte4 Color;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatSpritePositionTextureColor);

		internal MyVertexFormatSpritePositionTextureColor(HalfVector4 position, HalfVector4 texcoord, Byte4 color)
		{
			ClipspaceOffsetScale = position;
			TexcoordOffsetScale = texcoord;
			Color = color;
		}
	}
}
