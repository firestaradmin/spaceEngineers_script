using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionTexcoordNormalTangent
	{
		internal HalfVector4 Position;

		internal Byte4 Normal;

		internal Byte4 Tangent;

		internal HalfVector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionTexcoordNormalTangent);
	}
}
