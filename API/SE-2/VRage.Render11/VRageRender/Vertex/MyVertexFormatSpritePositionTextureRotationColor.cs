using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatSpritePositionTextureRotationColor
	{
		internal HalfVector4 ClipspaceOffsetScale;

		internal HalfVector4 TexcoordOffsetScale;

		internal HalfVector4 OriginTangent;

		internal Byte4 Color;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatSpritePositionTextureRotationColor);

		internal MyVertexFormatSpritePositionTextureRotationColor(HalfVector4 position, HalfVector4 texcoord, HalfVector4 originTangent, Byte4 color)
		{
			ClipspaceOffsetScale = position;
			TexcoordOffsetScale = texcoord;
			OriginTangent = originTangent;
			Color = color;
		}
	}
}
